/*
    Copyright (c) 2019 SaladBadger

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace LibDescent.Data
{
    public abstract partial class DescentLevelBase
    {
        protected virtual void LoadFromStream(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                int signature = reader.ReadInt32();
                const int expectedSignature = 'P' * 0x1000000 + 'L' * 0x10000 + 'V' * 0x100 + 'L';
                if (signature != expectedSignature)
                {
                    throw new InvalidDataException("Level signature is invalid.");
                }
                int version = reader.ReadInt32();
                CheckLevelVersion(version);

                var levelLoadData = new DeferredLevelLoadData(version);
                int mineDataOffset = reader.ReadInt32();
                int gameDataOffset = reader.ReadInt32();
                _ = reader.ReadInt32(); // hostageTextOffset - not used

                reader.BaseStream.Seek(mineDataOffset, SeekOrigin.Begin);
                LoadMineData(reader, ref levelLoadData);
                reader.BaseStream.Seek(gameDataOffset, SeekOrigin.Begin);
                LoadGameInfo(reader, ref levelLoadData);

                ApplyDeferredLoadData(ref levelLoadData);
            }
        }

        private protected struct DeferredLevelLoadData
        {
            public int version;
            public Dictionary<Side, uint> sideWallLinks;
            public Dictionary<Segment, uint> segmentMatcenLinks;
            public List<ITrigger> triggers;
            public Dictionary<Wall, byte> wallTriggerLinks;

            public DeferredLevelLoadData(int version)
            {
                this.version = version;
                sideWallLinks = new Dictionary<Side, uint>();
                segmentMatcenLinks = new Dictionary<Segment, uint>();
                triggers = new List<ITrigger>();
                wallTriggerLinks = new Dictionary<Wall, byte>();
            }
        }

        private protected virtual void LoadMineData(BinaryReader reader, ref DeferredLevelLoadData levelLoadData)
        {
            if (Vertices.Count > 0 || Segments.Count > 0)
            {
                throw new InvalidOperationException("Cannot load mine data when level is not empty.");
            }

            // Header
            _ = reader.ReadByte(); // compiled mine version, not used
            short numVertices = reader.ReadInt16();
            short numSegments = reader.ReadInt16();

            // Vertices
            for (int i = 0; i < numVertices; i++)
            {
                var vector = ReadFixVector(reader);
                var vertex = new LevelVertex(vector);
                Vertices.Add(vertex);
            }

            // Segments

            // Allocate segments/sides before reading data so we don't need a separate linking phase for them
            for (int i = 0; i < numSegments; i++)
            {
                var segment = new Segment();
                for (uint sideNum = 0; sideNum < Segment.MaxSegmentSides; sideNum++)
                {
                    segment.Sides[sideNum] = new Side(segment, sideNum);
                }
                Segments.Add(segment);
            }
            // Now read segment data
            foreach (var segment in Segments)
            {
                byte segmentBitMask = reader.ReadByte();
                if (levelLoadData.version == 5)
                {
                    if (SegmentHasSpecialData(segmentBitMask))
                    {
                        ReadSegmentSpecial(reader, segment, ref levelLoadData);
                    }
                    ReadSegmentVertices(reader, segment);
                    ReadSegmentConnections(reader, segment, segmentBitMask);
                }
                else
                {
                    ReadSegmentConnections(reader, segment, segmentBitMask);
                    ReadSegmentVertices(reader, segment);
                    if (levelLoadData.version <= 1 && SegmentHasSpecialData(segmentBitMask))
                    {
                        ReadSegmentSpecial(reader, segment, ref levelLoadData);
                    }
                }

                if (levelLoadData.version <= 5)
                {
                    segment.Light = Fix.FromRawValue(reader.ReadUInt16() << 4);
                }

                ReadSegmentWalls(reader, segment, ref levelLoadData);
                ReadSegmentTextures(reader, segment, ref levelLoadData);
            }

            // D2 retail location for segment special data
            if (levelLoadData.version > 5)
            {
                foreach (var segment in Segments)
                {
                    ReadSegmentSpecial(reader, segment, ref levelLoadData);
                }
            }
        }

        private void ReadSegmentConnections(BinaryReader reader, Segment segment, byte segmentBitMask)
        {
            for (int sideNum = 0; sideNum < Segment.MaxSegmentSides; sideNum++)
            {
                if ((segmentBitMask & (1 << sideNum)) != 0)
                {
                    var childSegmentId = reader.ReadInt16();
                    if (childSegmentId == -2)
                    {
                        segment.Sides[sideNum].Exit = true;
                    }
                    else
                    {
                        segment.Sides[sideNum].ConnectedSegment = Segments[childSegmentId];
                    }
                }
            }
        }

        private void ReadSegmentVertices(BinaryReader reader, Segment segment)
        {
            for (uint i = 0; i < Segment.MaxSegmentVerts; i++)
            {
                var vertexNum = reader.ReadInt16();
                segment.Vertices[i] = Vertices[vertexNum];
                segment.Vertices[i].ConnectedSegments.Add((segment, i));
            }

            // Connect vertices to sides
            foreach (var side in segment.Sides)
            {
                for (int vertexNum = 0; vertexNum < side.GetNumVertices(); vertexNum++)
                {
                    side.GetVertex(vertexNum).ConnectedSides.Add((side, (uint)vertexNum));
                }
            }
        }

        private void ReadSegmentWalls(BinaryReader reader, Segment segment, ref DeferredLevelLoadData levelLoadData)
        {
            byte wallsBitMask = reader.ReadByte();
            for (uint sideNum = 0; sideNum < Segment.MaxSegmentSides; sideNum++)
            {
                if ((wallsBitMask & (1 << (int)sideNum)) != 0)
                {
                    byte wallNum = reader.ReadByte();
                    if (wallNum != 255)
                    {
                        // Walls haven't been read yet so we need to record the numbers and link later
                        levelLoadData.sideWallLinks[segment.Sides[sideNum]] = wallNum;
                    }
                }
            }
        }

        private static bool SegmentHasSpecialData(byte segmentBitMask)
        {
            return (segmentBitMask & (1 << Segment.MaxSegmentSides)) != 0;
        }

        private void ReadSegmentSpecial(BinaryReader reader, Segment segment, ref DeferredLevelLoadData levelLoadData)
        {
            segment.Function = (SegFunction)reader.ReadByte();
            var matcenNum = reader.ReadByte();
            // fuelcen number
            _ = levelLoadData.version > 5 ? reader.ReadByte() : reader.ReadInt16();

            if (levelLoadData.version <= 1 && matcenNum != 0xFF)
            {
                levelLoadData.segmentMatcenLinks[segment] = matcenNum;
            }

            if (levelLoadData.version > 5)
            {
                segment.Flags = reader.ReadByte();
                segment.Light = Fix.FromRawValue(reader.ReadInt32());
            }
        }

        private void ReadSegmentTextures(BinaryReader reader, Segment segment, ref DeferredLevelLoadData levelLoadData)
        {
            for (int sideNum = 0; sideNum < Segment.MaxSegmentSides; sideNum++)
            {
                var side = segment.Sides[sideNum];
                if (side.ConnectedSegment == null || levelLoadData.sideWallLinks.ContainsKey(side))
                {
                    var rawTextureIndex = reader.ReadUInt16();
                    side.BaseTextureIndex = (ushort)(rawTextureIndex & 0x7fffu);
                    if ((rawTextureIndex & 0x8000) == 0)
                    {
                        side.OverlayTextureIndex = 0;
                    }
                    else
                    {
                        rawTextureIndex = reader.ReadUInt16();
                        side.OverlayTextureIndex = (ushort)(rawTextureIndex & 0x3fffu);
                        side.OverlayRotation = (OverlayRotation)((rawTextureIndex & 0xc000u) >> 14);
                    }

                    for (int uv = 0; uv < side.GetNumVertices(); uv++)
                    {
                        var uvl = Uvl.FromRawValues(reader.ReadInt16(), reader.ReadInt16(), reader.ReadUInt16());
                        side.Uvls[uv] = uvl;
                    }
                }
            }
        }

        private protected struct FileInfo
        {
            public ushort signature;
            public ushort version;
            public int size;
            public string mineFilename;
            public int levelNumber;
            public int playerOffset;
            public int playerSize;
            public int objectsOffset;
            public int objectsCount;
            public int objectsSize;
            public int wallsOffset;
            public int wallsCount;
            public int wallsSize;
            public int doorsOffset;
            public int doorsCount;
            public int doorsSize;
            public int triggersOffset;
            public int triggersCount;
            public int triggersSize;
            public int linksOffset;
            public int linksCount;
            public int linksSize;
            public int reactorTriggersOffset;
            public int reactorTriggersCount;
            public int reactorTriggersSize;
            public int matcenOffset;
            public int matcenCount;
            public int matcenSize;
            public int deltaLightIndicesOffset;
            public int deltaLightIndicesCount;
            public int deltaLightIndicesSize;
            public int deltaLightsOffset;
            public int deltaLightsCount;
            public int deltaLightsSize;
        }

        private protected virtual FileInfo LoadGameInfo(BinaryReader reader, ref DeferredLevelLoadData levelLoadData)
        {
            // "FileInfo" segment
            FileInfo fileInfo = new FileInfo();
            fileInfo.signature = reader.ReadUInt16();
            if (fileInfo.signature != 0x6705)
            {
                throw new InvalidDataException("Game info signature is invalid.");
            }

            const int MIN_GAMEINFO_VERSION = 22;
            fileInfo.version = reader.ReadUInt16();
            if (fileInfo.version < MIN_GAMEINFO_VERSION)
            {
                throw new InvalidDataException("Game info version is invalid.");
            }

            fileInfo.size = reader.ReadInt32();
            // This is not actually used by the game, it's from an older (probably obsolete) format
            fileInfo.mineFilename = ReadString(reader, 15, false);
            fileInfo.levelNumber = reader.ReadInt32();
            fileInfo.playerOffset = reader.ReadInt32();
            fileInfo.playerSize = reader.ReadInt32();
            fileInfo.objectsOffset = reader.ReadInt32();
            fileInfo.objectsCount = reader.ReadInt32();
            fileInfo.objectsSize = reader.ReadInt32();
            fileInfo.wallsOffset = reader.ReadInt32();
            fileInfo.wallsCount = reader.ReadInt32();
            fileInfo.wallsSize = reader.ReadInt32();
            fileInfo.doorsOffset = reader.ReadInt32();
            fileInfo.doorsCount = reader.ReadInt32();
            fileInfo.doorsSize = reader.ReadInt32();
            fileInfo.triggersOffset = reader.ReadInt32();
            fileInfo.triggersCount = reader.ReadInt32();
            fileInfo.triggersSize = reader.ReadInt32();
            fileInfo.linksOffset = reader.ReadInt32();
            fileInfo.linksCount = reader.ReadInt32();
            fileInfo.linksSize = reader.ReadInt32();
            fileInfo.reactorTriggersOffset = reader.ReadInt32();
            fileInfo.reactorTriggersCount = reader.ReadInt32();
            fileInfo.reactorTriggersSize = reader.ReadInt32();
            fileInfo.matcenOffset = reader.ReadInt32();
            fileInfo.matcenCount = reader.ReadInt32();
            fileInfo.matcenSize = reader.ReadInt32();

            if (fileInfo.version >= 29)
            {
                fileInfo.deltaLightIndicesOffset = reader.ReadInt32();
                fileInfo.deltaLightIndicesCount = reader.ReadInt32();
                fileInfo.deltaLightIndicesSize = reader.ReadInt32();
                fileInfo.deltaLightsOffset = reader.ReadInt32();
                fileInfo.deltaLightsCount = reader.ReadInt32();
                fileInfo.deltaLightsSize = reader.ReadInt32();
            }

            // Level name (as seen in automap)
            if (fileInfo.version >= 14)
            {
                LevelName = ReadString(reader, 36, true);
            }

            // POF file names (we currently don't use this)
            var pofFileNames = new List<string>();
            if (fileInfo.version >= 19)
            {
                int numPofNames = reader.ReadInt16();
                for (int i = 0; i < numPofNames; i++)
                {
                    pofFileNames.Add(ReadString(reader, 13, false));
                }
            }

            // Player info (empty)

            // Objects
            reader.BaseStream.Seek(fileInfo.objectsOffset, SeekOrigin.Begin);
            for (int i = 0; i < fileInfo.objectsCount; i++)
            {
                var levelObject = ReadObject(reader, ref fileInfo);
                Objects.Add(levelObject);
            }

            // Walls
            if (fileInfo.wallsOffset != -1)
            {
                reader.BaseStream.Seek(fileInfo.wallsOffset, SeekOrigin.Begin);
                for (int i = 0; i < fileInfo.wallsCount; i++)
                {
                    if (fileInfo.version >= 20)
                    {
                        var segmentNum = reader.ReadInt32();
                        var sideNum = reader.ReadInt32();
                        var side = Segments[segmentNum].Sides[sideNum];

                        Wall wall = new Wall(side);
                        wall.HitPoints = reader.ReadInt32();
                        _ = reader.ReadInt32(); // opposite wall - will recalculate
                        wall.Type = (WallType)reader.ReadByte();
                        wall.Flags = (WallFlags)reader.ReadByte();
                        wall.State = (WallState)reader.ReadByte();
                        var triggerNum = reader.ReadByte();
                        if (triggerNum != 0xFF)
                        {
                            levelLoadData.wallTriggerLinks[wall] = triggerNum;
                        }
                        wall.DoorClipNumber = reader.ReadByte();
                        wall.Keys = (WallKeyFlags)reader.ReadByte();
                        _ = reader.ReadByte(); // controlling trigger - will recalculate
                        wall.CloakOpacity = reader.ReadByte();
                        Walls.Add(wall);
                    }
                }
            }

            // Triggers
            if (fileInfo.triggersOffset != -1)
            {
                reader.BaseStream.Seek(fileInfo.triggersOffset, SeekOrigin.Begin);
                for (int i = 0; i < fileInfo.triggersCount; i++)
                {
                    ITrigger trigger = ReadTrigger(reader);
                    levelLoadData.triggers.Add(trigger);
                }
            }

            // Reactor triggers
            if (fileInfo.reactorTriggersOffset != -1)
            {
                reader.BaseStream.Seek(fileInfo.reactorTriggersOffset, SeekOrigin.Begin);
                for (int i = 0; i < fileInfo.reactorTriggersCount; i++)
                {
                    var numReactorTriggerTargets = reader.ReadInt16();

                    // Not actually counted by the number of targets, which is interesting
                    var targets = ReadFixedLengthTargetList(reader, MaxReactorTriggerTargets);

                    for (int targetNum = 0; targetNum < numReactorTriggerTargets; targetNum++)
                    {
                        var side = Segments[targets[targetNum].segmentNum].Sides[targets[targetNum].sideNum];
                        ReactorTriggerTargets.Add(side);
                    }
                }
            }

            // Matcens
            if (fileInfo.matcenOffset != -1)
            {
                reader.BaseStream.Seek(fileInfo.matcenOffset, SeekOrigin.Begin);
                for (int i = 0; i < fileInfo.matcenCount; i++)
                {
                    var robotFlags = new uint[2];
                    robotFlags[0] = reader.ReadUInt32();
                    if (fileInfo.version > 25)
                    {
                        robotFlags[1] = reader.ReadUInt32();
                    }
                    var hitPoints = reader.ReadInt32();
                    var interval = reader.ReadInt32();
                    var segmentNum = reader.ReadInt16();
                    _ = reader.ReadInt16(); // fuelcen number - not needed

                    MatCenter matcen = new MatCenter(Segments[segmentNum]);
                    matcen.InitializeSpawnedRobots(robotFlags);
                    matcen.HitPoints = hitPoints;
                    matcen.Interval = interval;
                    MatCenters.Add(matcen);
                }
            }

            return fileInfo;
        }

        private LevelObject ReadObject(BinaryReader reader, ref FileInfo fileInfo)
        {
            var levelObject = new LevelObject();
            levelObject.type = (ObjectType)reader.ReadSByte();
            levelObject.id = reader.ReadByte();
            levelObject.controlType = (ControlType)reader.ReadByte();
            levelObject.moveType = (MovementType)reader.ReadByte();
            levelObject.renderType = (RenderType)reader.ReadByte();
            levelObject.flags = reader.ReadByte();
            levelObject.segnum = reader.ReadInt16();
            levelObject.attachedObject = -1;
            levelObject.position = ReadFixVector(reader);
            levelObject.orientation = ReadFixMatrix(reader);
            levelObject.size = reader.ReadInt32();
            levelObject.shields = reader.ReadInt32();
            levelObject.lastPos = ReadFixVector(reader);
            levelObject.containsType = reader.ReadByte();
            levelObject.containsId = reader.ReadByte();
            levelObject.containsCount = reader.ReadByte();

            switch (levelObject.moveType)
            {
                case MovementType.Physics:
                    levelObject.physicsInfo.velocity = ReadFixVector(reader);
                    levelObject.physicsInfo.thrust = ReadFixVector(reader);
                    levelObject.physicsInfo.mass = reader.ReadInt32();
                    levelObject.physicsInfo.drag = reader.ReadInt32();
                    levelObject.physicsInfo.brakes = reader.ReadInt32();
                    levelObject.physicsInfo.angVel = ReadFixVector(reader);
                    levelObject.physicsInfo.rotThrust = ReadFixVector(reader);
                    levelObject.physicsInfo.turnroll = reader.ReadInt16();
                    levelObject.physicsInfo.flags = reader.ReadInt16();
                    break;
                case MovementType.Spinning:
                    levelObject.spinRate = ReadFixVector(reader);
                    break;
            }
            switch (levelObject.controlType)
            {
                case ControlType.AI:
                    levelObject.aiInfo.behavior = reader.ReadByte();
                    for (int i = 0; i < AIInfo.NumAIFlags; i++)
                        levelObject.aiInfo.aiFlags[i] = reader.ReadByte();

                    levelObject.aiInfo.hideSegment = reader.ReadInt16();
                    levelObject.aiInfo.hideIndex = reader.ReadInt16();
                    levelObject.aiInfo.pathLength = reader.ReadInt16();
                    levelObject.aiInfo.curPathIndex = reader.ReadInt16();

                    if (fileInfo.version <= 25)
                    {
                        reader.ReadInt32();
                    }
                    break;
                case ControlType.Explosion:
                    levelObject.explosionInfo.SpawnTime = reader.ReadInt32();
                    levelObject.explosionInfo.DeleteTime = reader.ReadInt32();
                    levelObject.explosionInfo.DeleteObject = reader.ReadInt16();
                    break;
                case ControlType.Powerup:
                    if (fileInfo.version >= 25)
                    {
                        levelObject.powerupCount = reader.ReadInt32();
                    }
                    break;
            }
            switch (levelObject.renderType)
            {
                case RenderType.Polyobj:
                    {
                        levelObject.modelInfo.modelNum = reader.ReadInt32();
                        for (int i = 0; i < Polymodel.MAX_SUBMODELS; i++)
                        {
                            levelObject.modelInfo.animAngles[i] = ReadFixAngles(reader);
                        }
                        levelObject.modelInfo.flags = reader.ReadInt32();
                        levelObject.modelInfo.textureOverride = reader.ReadInt32();
                    }
                    break;
                case RenderType.WeaponVClip:
                case RenderType.Hostage:
                case RenderType.Powerup:
                case RenderType.Fireball:
                    levelObject.spriteInfo.vclipNum = reader.ReadInt32();
                    levelObject.spriteInfo.frameTime = reader.ReadInt32();
                    levelObject.spriteInfo.frameNumber = reader.ReadByte();
                    break;
            }

            return levelObject;
        }

        private protected static string ReadString(BinaryReader reader, int maxStringLength, bool variableLength)
        {
            char[] stringBuffer = new char[maxStringLength];
            for (int i = 0; i < maxStringLength; i++)
            {
                stringBuffer[i] = (char)reader.ReadByte();
                if (stringBuffer[i] == '\n')
                {
                    stringBuffer[i] = '\0';
                }
                if (variableLength && stringBuffer[i] == '\0')
                {
                    break;
                }
            }
            return new string(stringBuffer).Trim('\0');
        }

        private protected static (short segmentNum, short sideNum)[] ReadFixedLengthTargetList(BinaryReader reader, int targetListLength)
        {
            var targetList = new (short segmentNum, short sideNum)[targetListLength];
            for (int i = 0; i < targetListLength; i++)
            {
                targetList[i].segmentNum = reader.ReadInt16();
            }
            for (int i = 0; i < targetListLength; i++)
            {
                targetList[i].sideNum = reader.ReadInt16();
            }
            return targetList;
        }

        private protected static FixVector ReadFixVector(BinaryReader reader)
        {
            return FixVector.FromRawValues(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
        }

        private protected static FixAngles ReadFixAngles(BinaryReader reader)
        {
            return FixAngles.FromRawValues(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
        }

        private protected FixMatrix ReadFixMatrix(BinaryReader reader)
        {
            return new FixMatrix(ReadFixVector(reader), ReadFixVector(reader), ReadFixVector(reader));
        }

        private protected abstract void CheckLevelVersion(int version);
        private protected abstract ITrigger ReadTrigger(BinaryReader reader);
        private protected abstract void ApplyDeferredLoadData(ref DeferredLevelLoadData levelLoadData);
    }

    public partial class D1Level
    {
        public static D1Level CreateFromStream(Stream stream)
        {
            var level = new D1Level();
            level.LoadFromStream(stream);
            return level;
        }

        private protected override void CheckLevelVersion(int version)
        {
            if (version != 1)
            {
                throw new InvalidDataException($"Level version should be 1 but was {version}.");
            }
        }

        private protected override ITrigger ReadTrigger(BinaryReader reader)
        {
            var trigger = new D1Trigger();
            trigger.Type = (TriggerType)reader.ReadByte();
            trigger.Flags = (D1TriggerFlags)reader.ReadUInt16();
            trigger.Value = Fix.FromRawValue(reader.ReadInt32());
            trigger.Time = reader.ReadInt32();
            _ = reader.ReadByte(); // link_num - does nothing
            var numLinks = reader.ReadInt16();

            var targets = ReadFixedLengthTargetList(reader, D1Trigger.MaxWallsPerLink);
            for (int i = 0; i < numLinks; i++)
            {
                var side = Segments[targets[i].segmentNum].Sides[targets[i].sideNum];
                trigger.Targets.Add(side);
            }

            return trigger;
        }

        private protected override void ApplyDeferredLoadData(ref DeferredLevelLoadData levelLoadData)
        {
            foreach (var sideWallLink in levelLoadData.sideWallLinks)
            {
                sideWallLink.Key.Wall = Walls[(int)sideWallLink.Value];
            }

            foreach (var segmentMatcenLink in levelLoadData.segmentMatcenLinks)
            {
                segmentMatcenLink.Key.MatCenter = MatCenters[(int)segmentMatcenLink.Value];
            }

            foreach (var trigger in levelLoadData.triggers)
            {
                Triggers.Add((D1Trigger)trigger);
                for (int targetNum = 0; targetNum < trigger.Targets.Count; targetNum++)
                {
                    trigger.Targets[targetNum].Wall?.ControllingTriggers.Add((trigger, (uint)targetNum));
                }
            }

            foreach (var wallTriggerLink in levelLoadData.wallTriggerLinks)
            {
                wallTriggerLink.Key.Trigger = Triggers[wallTriggerLink.Value];
            }
        }
    }

    public partial class D2Level
    {
        public static D2Level CreateFromStream(Stream stream)
        {
            var level = new D2Level();
            level.LoadFromStream(stream);
            return level;
        }

        private protected override FileInfo LoadGameInfo(BinaryReader reader, ref DeferredLevelLoadData levelLoadData)
        {
            var fileInfo = base.LoadGameInfo(reader, ref levelLoadData);

            // Delta light indices (D2)
            if (fileInfo.deltaLightIndicesOffset != -1)
            {
                reader.BaseStream.Seek(fileInfo.deltaLightIndicesOffset, SeekOrigin.Begin);
                for (int i = 0; i < fileInfo.deltaLightIndicesCount; i++)
                {
                    /*DynamicLightIndex index = new DynamicLightIndex();
                    index.segnum = reader.ReadInt16();
                    index.sidenum = reader.ReadByte();
                    index.count = reader.ReadByte();
                    index.index = reader.ReadInt16();
                    dlIndexes.Add(index);*/
                }
            }

            // Delta lights (D2)
            if (fileInfo.deltaLightsOffset != 0)
            {
                reader.BaseStream.Seek(fileInfo.deltaLightsOffset, SeekOrigin.Begin);
                for (int i = 0; i < fileInfo.deltaLightsCount; i++)
                {
                    /*DynamicLight light = new DynamicLight();
                    light.segnum = reader.ReadInt16();
                    light.sidenum = reader.ReadByte();
                    reader.ReadByte();
                    light.vertLight[0] = reader.ReadByte();
                    light.vertLight[1] = reader.ReadByte();
                    light.vertLight[2] = reader.ReadByte();
                    light.vertLight[3] = reader.ReadByte();
                    deltaLights.Add(light);*/
                }
            }

            return fileInfo;
        }

        private protected override void CheckLevelVersion(int version)
        {
            throw new NotImplementedException();
        }

        private protected override ITrigger ReadTrigger(BinaryReader reader)
        {
            var trigger = new D2Trigger();
            trigger.Type = (TriggerType)reader.ReadByte();
            trigger.Flags = (D2TriggerFlags)reader.ReadByte();
            var numLinks = reader.ReadSByte();
            reader.ReadByte(); //padding byte
            trigger.Value = reader.ReadInt32();
            trigger.Time = reader.ReadInt32();

            var targets = ReadFixedLengthTargetList(reader, D2Trigger.MaxWallsPerLink);
            for (int i = 0; i < numLinks; i++)
            {
                var side = Segments[targets[i].segmentNum].Sides[targets[i].sideNum];
                trigger.Targets.Add(side);
            }

            return trigger;
        }

        private protected override void ApplyDeferredLoadData(ref DeferredLevelLoadData levelLoadData)
        {
            foreach (var sideWallLink in levelLoadData.sideWallLinks)
            {
                sideWallLink.Key.Wall = Walls[(int)sideWallLink.Value];
            }

            foreach (var segmentMatcenLink in levelLoadData.segmentMatcenLinks)
            {
                segmentMatcenLink.Key.MatCenter = MatCenters[(int)segmentMatcenLink.Value];
            }

            foreach (var trigger in levelLoadData.triggers)
            {
                Triggers.Add((D2Trigger)trigger);
            }

            foreach (var wallTriggerLink in levelLoadData.wallTriggerLinks)
            {
                wallTriggerLink.Key.Trigger = Triggers[wallTriggerLink.Value];
            }
        }
    }
}