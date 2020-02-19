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
    internal abstract class DescentLevelLoader
    {
        protected Stream _stream;
        /// <summary>
        /// Level version info copied from D2 gamesave.cpp:
        /// 1 -> 2  add palette name
        /// 2 -> 3  add control center explosion time
        /// 3 -> 4  add reactor strength
        /// 4 -> 5  killed hostage text stuff
        /// 5 -> 6  added Secret_return_segment and Secret_return_orient
        /// 6 -> 7  added flickering lights
        /// 7 -> 8  made version 8 to be not compatible with D2 1.0 & 1.1
        /// </summary>
        protected int _levelVersion;
        protected int _mineDataOffset;
        protected int _gameDataOffset;
        protected FileInfo _fileInfo;
        private Dictionary<Side, uint> _sideWallLinks = new Dictionary<Side, uint>();
        private Dictionary<Segment, uint> _segmentMatcenLinks = new Dictionary<Segment, uint>();
        private Dictionary<Wall, byte> _wallTriggerLinks = new Dictionary<Wall, byte>();

        protected abstract ILevel Level { get; }

        protected struct FileInfo
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

        protected void LoadLevel()
        {
            using (var reader = new BinaryReader(_stream))
            {
                int signature = reader.ReadInt32();
                const int expectedSignature = 'P' * 0x1000000 + 'L' * 0x10000 + 'V' * 0x100 + 'L';
                if (signature != expectedSignature)
                {
                    throw new InvalidDataException("Level signature is invalid.");
                }
                _levelVersion = reader.ReadInt32();
                CheckLevelVersion();

                _mineDataOffset = reader.ReadInt32();
                _gameDataOffset = reader.ReadInt32();

                if (_levelVersion >= 8)
                {
                    // Dummy Vertigo-related data
                    _ = reader.ReadInt32();
                    _ = reader.ReadInt16();
                    _ = reader.ReadByte();
                }

                if (_levelVersion < 5)
                {
                    // Hostage text offset - not used
                    _ = reader.ReadInt32();
                }

                LoadVersionSpecificLevelInfo(reader);
                LoadMineData(reader);
                LoadVersionSpecificMineData(reader);
                LoadGameInfo(reader);
                LoadVersionSpecificGameInfo(reader);
            }
        }

        private void LoadMineData(BinaryReader reader)
        {
            reader.BaseStream.Seek(_mineDataOffset, SeekOrigin.Begin);

            // Header
            _ = reader.ReadByte(); // compiled mine version, not used
            short numVertices = reader.ReadInt16();
            short numSegments = reader.ReadInt16();

            // Vertices
            for (int i = 0; i < numVertices; i++)
            {
                var vector = ReadFixVector(reader);
                var vertex = new LevelVertex(vector);
                Level.Vertices.Add(vertex);
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
                Level.Segments.Add(segment);
            }
            // Now read segment data
            foreach (var segment in Level.Segments)
            {
                byte segmentBitMask = reader.ReadByte();
                if (_levelVersion == 5)
                {
                    if (SegmentHasSpecialData(segmentBitMask))
                    {
                        ReadSegmentSpecial(reader, segment);
                    }
                    ReadSegmentVertices(reader, segment);
                    ReadSegmentConnections(reader, segment, segmentBitMask);
                }
                else
                {
                    ReadSegmentConnections(reader, segment, segmentBitMask);
                    ReadSegmentVertices(reader, segment);
                    if (_levelVersion <= 1 && SegmentHasSpecialData(segmentBitMask))
                    {
                        ReadSegmentSpecial(reader, segment);
                    }
                }

                if (_levelVersion <= 5)
                {
                    segment.Light = Fix.FromRawValue(reader.ReadUInt16() << 4);
                }

                ReadSegmentWalls(reader, segment);
                ReadSegmentTextures(reader, segment);
            }

            // D2 retail location for segment special data
            if (_levelVersion > 5)
            {
                foreach (var segment in Level.Segments)
                {
                    ReadSegmentSpecial(reader, segment);
                }
            }
        }

        internal void ReadSegmentConnections(BinaryReader reader, Segment segment, byte segmentBitMask)
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
                    else if (childSegmentId >= 0) // -1 = disconnected
                    {
                        segment.Sides[sideNum].ConnectedSegment = Level.Segments[childSegmentId];
                    }
                }
            }
        }

        private void ReadSegmentVertices(BinaryReader reader, Segment segment)
        {
            for (uint i = 0; i < Segment.MaxSegmentVerts; i++)
            {
                var vertexNum = reader.ReadInt16();
                segment.Vertices[i] = Level.Vertices[vertexNum];
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

        private void ReadSegmentWalls(BinaryReader reader, Segment segment)
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
                        _sideWallLinks[segment.Sides[sideNum]] = wallNum;
                    }
                }
            }
        }

        private static bool SegmentHasSpecialData(byte segmentBitMask)
        {
            return (segmentBitMask & (1 << Segment.MaxSegmentSides)) != 0;
        }

        private void ReadSegmentSpecial(BinaryReader reader, Segment segment)
        {
            segment.Function = (SegFunction)reader.ReadByte();
            var matcenNum = reader.ReadByte();
            // fuelcen number
            _ = _levelVersion > 5 ? reader.ReadByte() : reader.ReadInt16();

            if (_levelVersion <= 1 && matcenNum != 0xFF)
            {
                _segmentMatcenLinks[segment] = matcenNum;
            }

            if (_levelVersion > 5)
            {
                segment.Flags = reader.ReadByte();
                segment.Light = Fix.FromRawValue(reader.ReadInt32());
            }
        }

        private void ReadSegmentTextures(BinaryReader reader, Segment segment)
        {
            for (int sideNum = 0; sideNum < Segment.MaxSegmentSides; sideNum++)
            {
                var side = segment.Sides[sideNum];
                if (side.ConnectedSegment == null || _sideWallLinks.ContainsKey(side))
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

        private void LoadGameInfo(BinaryReader reader)
        {
            reader.BaseStream.Seek(_gameDataOffset, SeekOrigin.Begin);

            // "FileInfo" segment
            _fileInfo.signature = reader.ReadUInt16();
            if (_fileInfo.signature != 0x6705)
            {
                throw new InvalidDataException("Game info signature is invalid.");
            }

            const int MIN_GAMEINFO_VERSION = 22;
            _fileInfo.version = reader.ReadUInt16();
            if (_fileInfo.version < MIN_GAMEINFO_VERSION)
            {
                throw new InvalidDataException("Game info version is invalid.");
            }

            _fileInfo.size = reader.ReadInt32();
            // This is not actually used by the game, it's from an older (probably obsolete) format
            _fileInfo.mineFilename = ReadString(reader, 15, false);
            _fileInfo.levelNumber = reader.ReadInt32();
            _fileInfo.playerOffset = reader.ReadInt32();
            _fileInfo.playerSize = reader.ReadInt32();
            _fileInfo.objectsOffset = reader.ReadInt32();
            _fileInfo.objectsCount = reader.ReadInt32();
            _fileInfo.objectsSize = reader.ReadInt32();
            _fileInfo.wallsOffset = reader.ReadInt32();
            _fileInfo.wallsCount = reader.ReadInt32();
            _fileInfo.wallsSize = reader.ReadInt32();
            _fileInfo.doorsOffset = reader.ReadInt32();
            _fileInfo.doorsCount = reader.ReadInt32();
            _fileInfo.doorsSize = reader.ReadInt32();
            _fileInfo.triggersOffset = reader.ReadInt32();
            _fileInfo.triggersCount = reader.ReadInt32();
            _fileInfo.triggersSize = reader.ReadInt32();
            _fileInfo.linksOffset = reader.ReadInt32();
            _fileInfo.linksCount = reader.ReadInt32();
            _fileInfo.linksSize = reader.ReadInt32();
            _fileInfo.reactorTriggersOffset = reader.ReadInt32();
            _fileInfo.reactorTriggersCount = reader.ReadInt32();
            _fileInfo.reactorTriggersSize = reader.ReadInt32();
            _fileInfo.matcenOffset = reader.ReadInt32();
            _fileInfo.matcenCount = reader.ReadInt32();
            _fileInfo.matcenSize = reader.ReadInt32();

            if (_fileInfo.version >= 29)
            {
                _fileInfo.deltaLightIndicesOffset = reader.ReadInt32();
                _fileInfo.deltaLightIndicesCount = reader.ReadInt32();
                _fileInfo.deltaLightIndicesSize = reader.ReadInt32();
                _fileInfo.deltaLightsOffset = reader.ReadInt32();
                _fileInfo.deltaLightsCount = reader.ReadInt32();
                _fileInfo.deltaLightsSize = reader.ReadInt32();
            }

            // Level name (as seen in automap)
            if (_fileInfo.version >= 14)
            {
                Level.LevelName = ReadString(reader, 36, true);
            }

            // POF file names (we currently don't use this)
            var pofFileNames = new List<string>();
            if (_fileInfo.version >= 19)
            {
                int numPofNames = reader.ReadInt16();
                for (int i = 0; i < numPofNames; i++)
                {
                    pofFileNames.Add(ReadString(reader, 13, false));
                }
            }

            // Player info (empty)

            // Objects
            reader.BaseStream.Seek(_fileInfo.objectsOffset, SeekOrigin.Begin);
            for (int i = 0; i < _fileInfo.objectsCount; i++)
            {
                var levelObject = ReadObject(reader);
                Level.Objects.Add(levelObject);
            }

            // Walls
            if (_fileInfo.wallsOffset != -1)
            {
                reader.BaseStream.Seek(_fileInfo.wallsOffset, SeekOrigin.Begin);
                for (int i = 0; i < _fileInfo.wallsCount; i++)
                {
                    if (_fileInfo.version >= 20)
                    {
                        var segmentNum = reader.ReadInt32();
                        var sideNum = reader.ReadInt32();
                        var side = Level.Segments[segmentNum].Sides[sideNum];

                        Wall wall = new Wall(side);
                        wall.HitPoints = reader.ReadInt32();
                        _ = reader.ReadInt32(); // opposite wall - will recalculate
                        wall.Type = (WallType)reader.ReadByte();
                        wall.Flags = (WallFlags)reader.ReadByte();
                        wall.State = (WallState)reader.ReadByte();
                        var triggerNum = reader.ReadByte();
                        if (triggerNum != 0xFF)
                        {
                            _wallTriggerLinks[wall] = triggerNum;
                        }
                        wall.DoorClipNumber = reader.ReadByte();
                        wall.Keys = (WallKeyFlags)reader.ReadByte();
                        _ = reader.ReadByte(); // controlling trigger - will recalculate
                        wall.CloakOpacity = reader.ReadByte();
                        Level.Walls.Add(wall);
                    }
                }

                foreach (var sideWallLink in _sideWallLinks)
                {
                    sideWallLink.Key.Wall = Level.Walls[(int)sideWallLink.Value];
                }
            }

            // Triggers
            if (_fileInfo.triggersOffset != -1)
            {
                reader.BaseStream.Seek(_fileInfo.triggersOffset, SeekOrigin.Begin);
                for (int i = 0; i < _fileInfo.triggersCount; i++)
                {
                    ITrigger trigger = ReadTrigger(reader);
                    AddTrigger(trigger);
                    for (int targetNum = 0; targetNum < trigger.Targets.Count; targetNum++)
                    {
                        trigger.Targets[targetNum].Wall?.ControllingTriggers.Add((trigger, (uint)targetNum));
                    }
                }

                foreach (var wallTriggerLink in _wallTriggerLinks)
                {
                    wallTriggerLink.Key.Trigger = Level.Triggers[wallTriggerLink.Value];
                }
            }

            // Reactor triggers
            if (_fileInfo.reactorTriggersOffset != -1)
            {
                reader.BaseStream.Seek(_fileInfo.reactorTriggersOffset, SeekOrigin.Begin);
                for (int i = 0; i < _fileInfo.reactorTriggersCount; i++)
                {
                    var numReactorTriggerTargets = reader.ReadInt16();

                    // Not actually counted by the number of targets, which is interesting
                    var targets = ReadFixedLengthTargetList(reader, DescentLevelCommon.MaxReactorTriggerTargets);

                    for (int targetNum = 0; targetNum < numReactorTriggerTargets; targetNum++)
                    {
                        var side = Level.Segments[targets[targetNum].segmentNum].Sides[targets[targetNum].sideNum];
                        Level.ReactorTriggerTargets.Add(side);
                    }
                }
            }

            // Matcens
            if (_fileInfo.matcenOffset != -1)
            {
                reader.BaseStream.Seek(_fileInfo.matcenOffset, SeekOrigin.Begin);
                for (int i = 0; i < _fileInfo.matcenCount; i++)
                {
                    var robotFlags = new uint[2];
                    robotFlags[0] = reader.ReadUInt32();
                    if (_fileInfo.version > 25)
                    {
                        robotFlags[1] = reader.ReadUInt32();
                    }
                    var hitPoints = reader.ReadInt32();
                    var interval = reader.ReadInt32();
                    var segmentNum = reader.ReadInt16();
                    _ = reader.ReadInt16(); // fuelcen number - not needed

                    MatCenter matcen = new MatCenter(Level.Segments[segmentNum]);
                    matcen.InitializeSpawnedRobots(robotFlags);
                    matcen.HitPoints = hitPoints;
                    matcen.Interval = interval;
                    Level.MatCenters.Add(matcen);
                }

                foreach (var segmentMatcenLink in _segmentMatcenLinks)
                {
                    segmentMatcenLink.Key.MatCenter = Level.MatCenters[(int)segmentMatcenLink.Value];
                }
            }
        }

        private LevelObject ReadObject(BinaryReader reader)
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

                    if (_fileInfo.version <= 25)
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
                    if (_fileInfo.version >= 25)
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

        protected static string ReadString(BinaryReader reader, int maxStringLength, bool variableLength)
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

        protected static (short segmentNum, short sideNum)[] ReadFixedLengthTargetList(BinaryReader reader, int targetListLength)
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

        protected static FixVector ReadFixVector(BinaryReader reader)
        {
            return FixVector.FromRawValues(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
        }

        protected static FixAngles ReadFixAngles(BinaryReader reader)
        {
            return FixAngles.FromRawValues(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
        }

        protected FixMatrix ReadFixMatrix(BinaryReader reader)
        {
            return new FixMatrix(ReadFixVector(reader), ReadFixVector(reader), ReadFixVector(reader));
        }

        protected abstract void CheckLevelVersion();
        protected abstract void LoadVersionSpecificLevelInfo(BinaryReader reader);
        protected abstract void LoadVersionSpecificMineData(BinaryReader reader);
        protected abstract ITrigger ReadTrigger(BinaryReader reader);
        protected abstract void AddTrigger(ITrigger trigger);
        protected abstract void LoadVersionSpecificGameInfo(BinaryReader reader);
    }

    internal class D1LevelLoader : DescentLevelLoader
    {
        private readonly D1Level _level = new D1Level();

        protected override ILevel Level => _level;

        public D1LevelLoader(Stream stream)
        {
            _stream = stream;
        }

        public D1Level Load()
        {
            LoadLevel();
            return _level;
        }

        protected override void CheckLevelVersion()
        {
            if (_levelVersion != 1)
            {
                throw new InvalidDataException($"Level version should be 1 but was {_levelVersion}.");
            }
        }

        protected override void LoadVersionSpecificLevelInfo(BinaryReader reader)
        {
        }

        protected override void LoadVersionSpecificMineData(BinaryReader reader)
        {
        }

        protected override ITrigger ReadTrigger(BinaryReader reader)
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
                var side = Level.Segments[targets[i].segmentNum].Sides[targets[i].sideNum];
                trigger.Targets.Add(side);
            }

            return trigger;
        }

        protected override void AddTrigger(ITrigger trigger)
        {
            (Level as D1Level).Triggers.Add(trigger as D1Trigger);
        }

        protected override void LoadVersionSpecificGameInfo(BinaryReader reader)
        {
        }
    }

    internal class D2LevelLoader : DescentLevelLoader
    {
        private readonly D2Level _level = new D2Level();
        private List<(short segmentNum, short sideNum, uint mask, Fix timer, Fix delay)> _flickeringLights =
            new List<(short, short, uint, Fix, Fix)>();
        private int _secretReturnSegmentNum = 0;
        private List<LightDelta> _lightDeltas = new List<LightDelta>();

        protected override ILevel Level => _level;

        public D2LevelLoader(Stream stream)
        {
            _stream = stream;
        }

        public D2Level Load()
        {
            LoadLevel();
            return _level;
        }

        protected override void CheckLevelVersion()
        {
            if (_levelVersion < 2 || _levelVersion > 8)
            {
                throw new InvalidDataException($"Level version should be between 2 and 8 but was {_levelVersion}.");
            }
        }

        protected override void LoadVersionSpecificLevelInfo(BinaryReader reader)
        {
            if (_levelVersion >= 2)
            {
                var paletteName = ReadString(reader, 13, true);
                // now need to load it somehow?
            }

            if (_levelVersion >= 3)
            {
                _level.BaseReactorCountdownTime = reader.ReadInt32();
            }
            else
            {
                _level.BaseReactorCountdownTime = D2Level.DefaultBaseReactorCountdownTime;
            }

            if (_levelVersion >= 4)
            {
                _level.ReactorStrength = reader.ReadInt32();
            }
            else
            {
                _level.ReactorStrength = null;
            }

            if (_levelVersion >= 7)
            {
                var numFlickeringLights = reader.ReadInt32();
                for (int i = 0; i < numFlickeringLights; i++)
                {
                    // Probably should really be using a struct, but this is surprisingly readable...
                    _flickeringLights.Add((
                        segmentNum: reader.ReadInt16(),
                        sideNum: reader.ReadInt16(),
                        mask: reader.ReadUInt32(),
                        timer: Fix.FromRawValue(reader.ReadInt32()),
                        delay: Fix.FromRawValue(reader.ReadInt32())
                        ));
                }
            }

            if (_levelVersion >= 6)
            {
                _secretReturnSegmentNum = reader.ReadInt32();
                _level.SecretReturnOrientation = new FixMatrix(
                    FixVector.FromRawValues(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                    FixVector.FromRawValues(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32()),
                    FixVector.FromRawValues(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32())
                    );
            }
            else
            {
                _level.SecretReturnOrientation = new FixMatrix(
                    new FixVector(1, 0, 0), new FixVector(0, 1, 0), new FixVector(0, 0, 1)
                    );
            }
        }

        protected override void LoadVersionSpecificMineData(BinaryReader reader)
        {
            // Nothing to actually read, but we do need to set up some links

            foreach (var light in _flickeringLights)
            {
                var side = Level.Segments[light.segmentNum].Sides[light.sideNum];
                var animatedLight = new AnimatedLight(side);
                animatedLight.Mask = light.mask;
                animatedLight.TimeToNextTick = light.timer;
                animatedLight.TickLength = light.delay;
                _level.AnimatedLights.Add(animatedLight);
                side.AnimatedLight = animatedLight;
            }

            _level.SecretReturnSegment = _level.Segments[_secretReturnSegmentNum];
        }

        protected override ITrigger ReadTrigger(BinaryReader reader)
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
                var side = Level.Segments[targets[i].segmentNum].Sides[targets[i].sideNum];
                trigger.Targets.Add(side);
            }

            return trigger;
        }

        protected override void AddTrigger(ITrigger trigger)
        {
            (Level as D2Level).Triggers.Add(trigger as D2Trigger);
        }

        protected override void LoadVersionSpecificGameInfo(BinaryReader reader)
        {
            // Delta lights (D2)
            // Reading this first to make lights easier to link up
            if (_fileInfo.deltaLightsOffset != 0)
            {
                reader.BaseStream.Seek(_fileInfo.deltaLightsOffset, SeekOrigin.Begin);
                for (int i = 0; i < _fileInfo.deltaLightsCount; i++)
                {
                    var segmentNum = reader.ReadInt16();
                    var sideNum = reader.ReadByte();
                    var lightDelta = new LightDelta(Level.Segments[segmentNum].Sides[sideNum]);
                    _ = reader.ReadByte(); // dummy - probably used for dword alignment
                    // Vertex deltas scaled by 2048 - see DL_SCALE in segment.h
                    lightDelta.vertexDeltas[0] = Fix.FromRawValue(reader.ReadByte() * 2048);
                    lightDelta.vertexDeltas[1] = Fix.FromRawValue(reader.ReadByte() * 2048);
                    lightDelta.vertexDeltas[2] = Fix.FromRawValue(reader.ReadByte() * 2048);
                    lightDelta.vertexDeltas[3] = Fix.FromRawValue(reader.ReadByte() * 2048);
                    _lightDeltas.Add(lightDelta);
                }
            }

            // Delta light indices (D2)
            if (_fileInfo.deltaLightIndicesOffset != -1)
            {
                reader.BaseStream.Seek(_fileInfo.deltaLightIndicesOffset, SeekOrigin.Begin);
                for (int i = 0; i < _fileInfo.deltaLightIndicesCount; i++)
                {
                    var segmentNum = reader.ReadInt16();
                    var sideNum = reader.ReadByte();
                    var count = reader.ReadByte();
                    var index = reader.ReadInt16();

                    var side = Level.Segments[segmentNum].Sides[sideNum];
                    var dynamicLight = new DynamicLight(side);
                    dynamicLight.LightDeltas.AddRange(_lightDeltas.GetRange(index, count));
                    _level.DynamicLights.Add(dynamicLight);
                    side.DynamicLight = dynamicLight;
                }
            }
        }
    }
}