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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LibDescent.Data;

namespace Descent2Workshop.Editor
{
    public class LevelVertex
    {
        public FixVector location;
        public List<Side> connectedSides = new List<Side>();
        public List<Segment> connectedSegs = new List<Segment>();
        public int exportID = 0;
        public bool selected = false;

        public long GetCode()
        {
            // one fix 16.16 takes 32b, so for 64b we can fit 3*21b (16.5)
            // therefore we shift 11 to get a 16.5

            long xi = location.x.GetRawValue() >> 11;
            long yi = location.y.GetRawValue() >> 11;
            long zi = location.z.GetRawValue() >> 11;

            long code = xi + (yi << 21) + (zi << 42);

            return code;
        }

        public string GetMatchCode()
        {
            return string.Format("{0:N3}-{1:N3}-{2:N3}", location.x / 65536.0d, location.y / 65536.0d, location.z / 65536.0d);
        }
    }
    public class Level
    {
        public const int DefaultGameDataVersion = 32;
        public const int GameDataSize = 143;
        public const int PlayerSize = 142;
        public const int ObjectSize = 264;
        public const int WallSize = 24;
        public const int DoorSize = 16;
        public const int TriggerSize = 52;
        public const int ControlSize = 42;
        public const int MatcenSize = 20;
        public const int DLIndexSize = 6;
        public const int DLSize = 8;

        //Overall level data
        private int version;
        private int mineOffset;
        private int gameDataOffset;
        private string paletteName;
        private int countdownTime = 30;
        private int reactorStrength = -1;
        private List<FlickeringLight> flickeringLights = new List<FlickeringLight>();
        private int secretReturnSeg = 0;
        private FixMatrix secretReturnFacing;
        //Mine data
        private bool sharewareMine = false;
        private byte mineVersion;
        private List<Segment> segments;// = new List<Segment>(); //undone: optimized, kinda
        private List<LevelVertex> verts;// = new List<LevelVertex>(); //undone: optimized, kinda
        //Gameinfo data
        private short gameDataVersion;
        private string gameMineFilename = "";
        private string levelName = "";
        private int levelNumber; 
        private List<string> pofFiles = new List<string>();
        private List<EditorObject> objects = new List<EditorObject>();
        private List<Wall> walls = new List<Wall>();
        private List<Trigger> triggers = new List<Trigger>();
        private List<DynamicLightIndex> dlIndexes = new List<DynamicLightIndex>();
        private List<DynamicLight> deltaLights = new List<DynamicLight>();
        private ControlCenterTrigger controlCenterTrigger = new ControlCenterTrigger();
        private List<MatCenter> matcens = new List<MatCenter>();

        public List<Segment> Segments { get => segments; }
        public List<LevelVertex> Verts { get => verts; }
        public List<EditorObject> Objects { get => objects; }

        public int LoadMine(string filename)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".sl2", StringComparison.OrdinalIgnoreCase))
            {
                sharewareMine = true;
            }
            BinaryReader br = null;
            try
            {
                br = new BinaryReader(File.Open(filename, FileMode.Open));
            }
            catch (FileNotFoundException)
            {
                return -3;
            }
            catch (UnauthorizedAccessException)
            {
                return -4;
            }
            if (br == null)
                return -1;

            int sig = br.ReadInt32();
            version = br.ReadInt32();
            mineOffset = br.ReadInt32();
            gameDataOffset = br.ReadInt32();
            if (version >= 8) //useless hoard data
            {
                br.ReadInt32();
                br.ReadInt16();
                br.ReadByte();
            }
            if (version < 5)
                br.ReadInt32();

            if (version > 1)
            {
                char[] palNameChars = new char[13];
                for (int i = 0; i < 13; i++)
                {
                    char c = (char)br.ReadByte();
                    if (c == '\n')
                    {
                        palNameChars[i] = '\0';
                        break;
                    }
                    else palNameChars[i] = c;
                }
                paletteName = new string(palNameChars).Trim('\0');
            }
            else
                paletteName = "groupa.256";
            if (version >= 3)
                countdownTime = br.ReadInt32();
            if (version >= 4)
                reactorStrength = br.ReadInt32();
            if (version >= 7)
            {
                int numFlickeringLights = br.ReadInt32();
                for (int i = 0; i < numFlickeringLights; i++)
                {
                    FlickeringLight light;
                    light.segnum = br.ReadInt16();
                    light.sidenum = br.ReadInt16();
                    light.mask = br.ReadInt32();
                    light.timer = br.ReadInt32();
                    light.delay = br.ReadInt32();
                    flickeringLights.Add(light);
                }
            }
            if (version < 6)
            {
                secretReturnSeg = 0;
            }
            else
            {
                secretReturnSeg = br.ReadInt32();
                secretReturnFacing = ReadMatrix(br);
            }
            br.BaseStream.Seek(mineOffset, SeekOrigin.Begin);
            LoadMineData(br);
            br.BaseStream.Seek(gameDataOffset, SeekOrigin.Begin);
            LoadGameInfo(br);
            br.Close();
            br.Dispose();
            //Instead of using IDs to manage things like segment children and walls,
            //set references instead. IDs are computed on export time.
            SetMineReferences();

            return 0;
        }

        public int SaveMine(string filename)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create));
            bw.Write(0x504c564C); //sig
            bw.Write(7); //version, always 7 atm
            long pointerTable = bw.BaseStream.Position;
            bw.Write(0); bw.Write(0); //Pointers, will fill out later. 

            for (int i = 0; i < paletteName.Length; i++)
            {
                bw.Write((byte)paletteName[i]);
            }
            bw.Write((byte)'\n');
            bw.Write(countdownTime);
            bw.Write(reactorStrength);
            bw.Write(flickeringLights.Count);
            //for (int i = 0; i < numFlickeringLights; i++)
            foreach (FlickeringLight light in flickeringLights)
            {
                /*FlickeringLight light;
                light.segnum = br.ReadInt16();
                light.sidenum = br.ReadInt16();
                light.mask = br.ReadInt32();
                light.timer = br.ReadInt32();
                light.delay = br.ReadInt32();
                flickeringLights.Add(light);*/
                bw.Write(light.segnum);
                bw.Write(light.sidenum);
                bw.Write(light.mask);
                bw.Write(light.timer);
                bw.Write(light.delay);
            }
            bw.Write(secretReturnSeg);
            WriteMatrix(bw, secretReturnFacing);
            int minePointer = (int)bw.BaseStream.Position;
            SaveMineData(bw);
            int gameDataPointer = (int)bw.BaseStream.Position;
            SaveGameData(bw);
            bw.BaseStream.Seek(pointerTable, SeekOrigin.Begin);
            bw.Write(minePointer);
            bw.Write(gameDataPointer);

            bw.Flush();
            bw.Close();
            bw.Dispose();

            return 0;
        }

        private void SetMineReferences()
        {
            Side side;
            foreach (Segment seg in segments)
            {
                for (int i = 0; i < Segment.MaxSegmentSides; i++)
                {
                    side = seg.sides[i];
                    if (seg.childrenIDs[i] == -2)
                        side.exit = true;
                    else if (seg.childrenIDs[i] >= 0)
                        seg.children[i] = segments[seg.childrenIDs[i]];
                    if (side.wallNum != -1)
                        side.wall = walls[side.wallNum];
                    side.SetType();
                }
            }
        }

        public int LoadMineData(BinaryReader br)
        {
            mineVersion = br.ReadByte();
            short numVerts = br.ReadInt16();
            short numSegs = br.ReadInt16();

            //Since I read how many verts there are, why not actually allow enough space to hold them off the bat. This might make the loader work 1ms faster overall. 
            //Include a bit of extra space for new elements. 
            verts = new List<LevelVertex>(numVerts * 2);
            segments = new List<Segment>(numSegs * 2);
            for (int i = 0; i < numVerts; i++)
            {
                LevelVertex vert = new LevelVertex();
                vert.location = ReadFixVec(br);
                verts.Add(vert);
            }
            for (int i = 0; i < numSegs; i++)
            {
                byte bitMask = br.ReadByte();
                Segment seg = new Segment();
                if (sharewareMine)
                {
                    if ((bitMask & (1 << Segment.MaxSegmentSides)) != 0)
                    {
                        br.ReadByte(); br.ReadByte(); br.ReadInt16();
                    }
                    ReadSegmentVerts(br, seg);
                    ReadSegmentConnections(br, seg, bitMask);
                }
                else
                {
                    ReadSegmentConnections(br, seg, bitMask);
                    ReadSegmentVerts(br, seg);
                }
                bitMask = br.ReadByte();
                int flag = 1;
                byte wallNum;
                for (int bit = 0; bit < Segment.MaxSegmentSides; bit++)
                {
                    seg.sides[bit] = new Side(seg, (SegSide)bit);
                    if ((bitMask & flag) != 0)
                    {
                        wallNum = br.ReadByte();
                        if (wallNum == 255)
                            seg.sides[bit].wallNum = -1;
                        else
                            seg.sides[bit].wallNum = wallNum;
                    }
                    else
                        seg.sides[bit].wallNum = -1;
                    flag <<= 1;
                    LevelVertex sideVert;
                    for (int sv = 0; sv < 4; sv++)
                    {
                        sideVert = seg.vertices[Segment.SideVerts[bit, sv]];
                        sideVert.connectedSides.Add(seg.sides[bit]);
                    }
                }
                ushort temp;
                for (int side = 0; side < Segment.MaxSegmentSides; side++)
                {
                    if (seg.childrenIDs[side] == -1 || seg.sides[side].wallNum != -1)
                    {
                        temp = br.ReadUInt16();
                        seg.sides[side].tmapNum = (short)(temp & 0x7fff);
                        if ((temp & 0x8000) == 0)
                            seg.sides[side].tmapNum2 = 0;
                        else
                            seg.sides[side].tmapNum2 = br.ReadInt16();

                        for (int uv = 0; uv < 4; uv++)
                        {
                            seg.sides[side].uvls[uv].x = Fix.FromRawValue(br.ReadInt16() << 5);
                            seg.sides[side].uvls[uv].y = Fix.FromRawValue(br.ReadInt16() << 5);
                            seg.sides[side].uvls[uv].z = Fix.FromRawValue(br.ReadUInt16() << 1);
                        }
                    }
                    else
                    {
                        seg.sides[side].tmapNum = seg.sides[side].tmapNum2 = 0;
                        for (int uv = 0; uv < 4; uv++)
                        {
                            seg.sides[side].uvls[uv].x = 0;
                            seg.sides[side].uvls[uv].y = 0;
                            seg.sides[side].uvls[uv].z = 0;
                        }
                    }
                }
                segments.Add(seg);
            }
            //SEG2 information, on the main seg structure since I don't need to worry about that mutating...
            for (int i = 0; i < numSegs; i++)
            {
                segments[i].special = br.ReadByte();
                segments[i].matcenNum = br.ReadByte();
                segments[i].value = br.ReadByte();
                segments[i].flags = br.ReadByte();
                segments[i].staticLight = br.ReadInt32();
            }

            return 0;
        }

        private void ReadSegmentConnections(BinaryReader br, Segment seg, byte bitMask)
        {
            int flag = 1;
            for (int bit = 0; bit < Segment.MaxSegmentSides; bit++)
            {
                if ((bitMask & flag) != 0)
                    seg.childrenIDs[bit] = br.ReadInt16();
                else
                    seg.childrenIDs[bit] = -1;
                flag <<= 1;
            }
        }

        private void ReadSegmentVerts(BinaryReader br, Segment seg)
        {
            for (int i = 0; i < Segment.MaxSegmentVerts; i++)
            {
                seg.verts[i] = br.ReadInt16();
                seg.vertices[i] = verts[seg.verts[i]];
                LevelVertex vert = verts[seg.verts[i]];
                vert.connectedSegs.Add(seg);
            }
        }

        public int SaveMineData(BinaryWriter bw)
        {
            bw.Write((byte)0);
            bw.Write((short)verts.Count);
            bw.Write((short)segments.Count);

            for (int i = 0; i < verts.Count; i++)
            {
                WriteFixVec(bw, verts[i].location);
            }
            for (int i = 0; i < segments.Count; i++)
            {
                //byte bitMask;
                Segment seg = segments[i];
                //am I enough of a bad EULA-breaking boy to enable writing shareware levels? stay tuned...
                /*if (sharewareMine)
                {
                    if ((bitMask & (1 << Segment.MaxSegmentSides)) != 0)
                    {
                        br.ReadByte(); br.ReadByte(); br.ReadInt16();
                    }
                    ReadSegmentVerts(br, seg);
                    ReadSegmentConnections(br, seg, bitMask);
                }
                else*/
                {
                    WriteSegmentConnections(bw, seg);
                    WriteSegmentVerts(bw, seg);
                }
                WriteSegmentWalls(bw, seg);
                ushort temp;
                for (int side = 0; side < Segment.MaxSegmentSides; side++)
                {
                    if (seg.childrenIDs[side] == -1 || seg.sides[side].wallNum != -1)
                    {
                        //temp = br.ReadUInt16();
                        temp = (ushort)seg.sides[side].tmapNum;
                        if (seg.sides[side].tmapNum2 != 0)
                            temp |= 0x8000;
                        bw.Write(temp);
                        if (seg.sides[side].tmapNum2 != 0)
                            bw.Write(seg.sides[side].tmapNum2);
                        for (int uv = 0; uv < 4; uv++)
                        {
                            bw.Write((short)(seg.sides[side].uvls[uv].x >> 5));
                            bw.Write((short)(seg.sides[side].uvls[uv].y >> 5));
                            bw.Write((ushort)(seg.sides[side].uvls[uv].z >> 1));
                        }
                    }
                }
            }
            //SEG2 information, on the main seg structure since I don't need to worry about that mutating...
            for (int i = 0; i < segments.Count; i++)
            {
                bw.Write(segments[i].special);
                bw.Write(segments[i].matcenNum);
                bw.Write(segments[i].value);
                bw.Write(segments[i].flags);
                bw.Write(segments[i].staticLight);
            }

            return 0;
        }

        private void WriteSegmentWalls(BinaryWriter bw, Segment seg)
        {
            int flag = 1;
            byte bitMask = 0;
            for (int side = 0; side < Segment.MaxSegmentSides; side++)
            {
                if (seg.sides[side].wallNum != -1) bitMask |= (byte)flag;
                flag <<= 1;
            }
            bw.Write(bitMask);
            for (int bit = 0; bit < Segment.MaxSegmentSides; bit++)
            {
                if (seg.sides[bit].wallNum != -1)
                {
                    bw.Write((byte)seg.sides[bit].wallNum);
                }
            }
        }

        private void WriteSegmentConnections(BinaryWriter bw, Segment seg)
        {
            int flag = 1;
            byte bitMask = 0;
            for (int side = 0; side < Segment.MaxSegmentSides; side++)
            {
                if (seg.childrenIDs[side] != -1) bitMask |= (byte)flag;
                flag <<= 1;
            }
            bw.Write(bitMask);
            for (int bit = 0; bit < Segment.MaxSegmentSides; bit++)
            {
                if (seg.childrenIDs[bit] != -1)
                    bw.Write(seg.childrenIDs[bit]);
            }
        }

        private void WriteSegmentVerts(BinaryWriter bw, Segment seg)
        {
            for (int i = 0; i < Segment.MaxSegmentVerts; i++)
            {
                bw.Write(seg.verts[i]);
            }
        }

        private int LoadGameInfo(BinaryReader br)
        {
            short sig = br.ReadInt16();
            if (sig != 0x6705)
                throw new Exception("it broke, yo");
            gameDataVersion = br.ReadInt16();
            int gameDataSize = br.ReadInt32();
            char[] mapNameChars = new char[15];
            for (int i = 0; i < 15; i++)
            {
                mapNameChars[i] = (char)br.ReadByte();
            }
            gameMineFilename = new string(mapNameChars).Trim('\0');
            levelNumber = br.ReadInt32();
            int playerOffset = br.ReadInt32();
            int playerSize = br.ReadInt32();
            int objectsOffset = br.ReadInt32();
            int objectsCount = br.ReadInt32();
            int objectsSize = br.ReadInt32();
            int wallsOffset = br.ReadInt32();
            int wallsCount = br.ReadInt32();
            int wallsSize = br.ReadInt32();
            int doorsOffset = br.ReadInt32();
            int doorsCount = br.ReadInt32();
            int doorsSize = br.ReadInt32();
            int triggersOffset = br.ReadInt32();
            int triggersCount = br.ReadInt32();
            int triggersSize = br.ReadInt32();
            int linksOffset = br.ReadInt32();
            int linksCount = br.ReadInt32();
            int linksSize = br.ReadInt32();
            int controlOffset = br.ReadInt32();
            int controlCount = br.ReadInt32();
            int controlSize = br.ReadInt32();
            int matcenOffset = br.ReadInt32();
            int matcenCount = br.ReadInt32();
            int matcenSize = br.ReadInt32();
            int dlIndexOffset = -1;
            int dlIndexCount = 0;
            int dlIndexSize = 0;
            int dlOffset = -1;
            int dlCount = 0;
            int dlSize = 0;

            if (gameDataVersion >= 29)
            {
                dlIndexOffset = br.ReadInt32();
                dlIndexCount = br.ReadInt32();
                dlIndexSize = br.ReadInt32();
                dlOffset = br.ReadInt32();
                dlCount = br.ReadInt32();
                dlSize = br.ReadInt32();
            }
            if (gameDataVersion >= 14)
            {
                char[] levelNameChars = new char[36];
                for (int i = 0; i < 36; i++)
                {
                    char c = (char)br.ReadByte();
                    if (c == '\n')
                    {
                        levelNameChars[i] = '\0';
                        break;
                    }
                    else levelNameChars[i] = c;
                }
                levelName = new string(levelNameChars).Trim('\0');
            }
            if (gameDataVersion >= 19)
            {
                char[] pofName = new char[13];
                int numPofNames = br.ReadInt16();
                for (int i = 0; i < numPofNames; i++)
                {
                    for (int ch = 0; ch < 13; ch++)
                    {
                        pofName[ch] = (char)br.ReadByte();
                    }
                    pofFiles.Add(new string(pofName).Trim('\0'));
                }
            }
            br.BaseStream.Seek(objectsOffset, SeekOrigin.Begin);
            int objectSig = 0;
            for (int i = 0; i < objectsCount; i++)
            {
                EditorObject obj = ReadObject(br);
                obj.sig = objectSig;
                objectSig++;
                objects.Add(obj);
            }
            if (wallsOffset != -1)
            {
                br.BaseStream.Seek(wallsOffset, SeekOrigin.Begin);
                for (int i = 0; i < wallsCount; i++)
                {
                    if (gameDataVersion >= 20)
                    {
                        Wall wall = new Wall();
                        wall.segnum = br.ReadInt32();
                        wall.sidenum = br.ReadInt32();
                        wall.hp = br.ReadInt32();
                        wall.linkedWall = br.ReadInt32();
                        wall.type = (WallType)br.ReadByte();
                        wall.flags = br.ReadByte();
                        wall.state = br.ReadByte();
                        wall.trigger = br.ReadByte();
                        wall.clipNum = br.ReadByte();
                        wall.keys = br.ReadByte();
                        wall.controllingTrigger = br.ReadByte();
                        wall.cloakValue = br.ReadByte();
                        walls.Add(wall);
                    }
                }
            }
            if (triggersOffset != -1)
            {
                br.BaseStream.Seek(triggersOffset, SeekOrigin.Begin);
                for (int i = 0; i < triggersCount; i++)
                {
                    Trigger trigger = new Trigger();
                    trigger.type = (TriggerType)br.ReadByte();
                    trigger.flags = br.ReadByte();
                    trigger.numLinks = br.ReadSByte();
                    br.ReadByte(); //padding byte
                    trigger.value = br.ReadInt32();
                    trigger.time = br.ReadInt32();
                    for (int j = 0; j < Trigger.MaxWallsPerLink; j++)
                        trigger.seg[j] = br.ReadInt16();
                    for (int j = 0; j < Trigger.MaxWallsPerLink; j++)
                        trigger.side[j] = br.ReadInt16();
                    triggers.Add(trigger);
                }
            }
            if (controlOffset != -1)
            {
                br.BaseStream.Seek(controlOffset, SeekOrigin.Begin);
                for (int i = 0; i < controlCount; i++)
                {
                    /*				ControlCenterTriggers.num_links = read_short(LoadFile);
				for (j=0; j<MAX_CONTROLCEN_LINKS; j++ )
					ControlCenterTriggers.seg[j] = read_short( LoadFile );
				for (j=0; j<MAX_CONTROLCEN_LINKS; j++ )
					ControlCenterTriggers.side[j] = read_short( LoadFile );*/
                    controlCenterTrigger.numLinks = br.ReadInt16();
                    for (int j = 0; j < ControlCenter.MaxControlCenterLinks; j++)
                        controlCenterTrigger.seg[j] = br.ReadInt16();
                    for (int j = 0; j < ControlCenter.MaxControlCenterLinks; j++)
                        controlCenterTrigger.side[j] = br.ReadInt16();
                }
            }
            if (matcenOffset != -1)
            {
                br.BaseStream.Seek(matcenOffset, SeekOrigin.Begin);
                for (int i = 0; i < matcenCount; i++)
                {
                    /*					RobotCenters[i].robot_flags[0] = read_int(LoadFile);
					RobotCenters[i].robot_flags[1] = read_int(LoadFile);
					RobotCenters[i].hit_points = read_fix(LoadFile);
					RobotCenters[i].interval = read_fix(LoadFile);
					RobotCenters[i].segnum = read_short(LoadFile);
					RobotCenters[i].fuelcen_num = read_short(LoadFile);*/
                    MatCenter matcen = new MatCenter();
                    matcen.robotFlags[0] = br.ReadInt32();
                    matcen.robotFlags[1] = br.ReadInt32();
                    matcen.hitPoints = br.ReadInt32();
                    matcen.interval = br.ReadInt32();
                    matcen.segnum = br.ReadInt16();
                    matcen.fuelcenNum = br.ReadInt16();
                }
            }
            if (dlIndexOffset != -1)
            {
                br.BaseStream.Seek(dlIndexOffset, SeekOrigin.Begin);
                for (int i = 0; i < dlIndexCount; i++)
                {
                    DynamicLightIndex index = new DynamicLightIndex();
                    index.segnum = br.ReadInt16();
                    index.sidenum = br.ReadByte();
                    index.count = br.ReadByte();
                    index.index = br.ReadInt16();
                    dlIndexes.Add(index);
                }
            }
            if (dlOffset != 0)
            {
                br.BaseStream.Seek(dlOffset, SeekOrigin.Begin);
                for (int i = 0; i < dlCount; i++)
                {
                    DynamicLight light = new DynamicLight();
                    light.segnum = br.ReadInt16();
                    light.sidenum = br.ReadByte();
                    br.ReadByte();
                    light.vertLight[0] = br.ReadByte();
                    light.vertLight[1] = br.ReadByte();
                    light.vertLight[2] = br.ReadByte();
                    light.vertLight[3] = br.ReadByte();
                    deltaLights.Add(light);
                }
            }
            return 0;
        }

        private EditorObject ReadObject(BinaryReader br)
        {
            EditorObject obj = new EditorObject();
            obj.type = (ObjectType)br.ReadSByte();
            obj.id = br.ReadByte();
            obj.controlType = (ControlType)br.ReadByte();
            obj.moveType = (MovementType)br.ReadByte();
            obj.renderType = (RenderType)br.ReadByte();
            obj.flags = br.ReadByte();
            obj.segnum = br.ReadInt16();
            obj.attachedObject = -1;
            obj.position = ReadFixVec(br);
            obj.orientation = ReadMatrix(br);
            obj.size = br.ReadInt32();
            obj.shields = br.ReadInt32();
            obj.lastPos = ReadFixVec(br);
            obj.containsType = br.ReadByte();
            obj.containsId = br.ReadByte();
            obj.containsCount = br.ReadByte();

            switch (obj.moveType)
            {
                case MovementType.Physics:
                    obj.physicsInfo.velocity = ReadFixVec(br);
                    obj.physicsInfo.thrust = ReadFixVec(br);
                    obj.physicsInfo.mass = br.ReadInt32();
                    obj.physicsInfo.drag = br.ReadInt32();
                    obj.physicsInfo.brakes = br.ReadInt32();
                    obj.physicsInfo.angVel = ReadFixVec(br);
                    obj.physicsInfo.rotThrust = ReadFixVec(br);
                    obj.physicsInfo.turnroll = br.ReadInt16();
                    obj.physicsInfo.flags = br.ReadInt16();
                    break;
                case MovementType.Spinning:
                    obj.spinRate = ReadFixVec(br);
                    break;
            }
            switch (obj.controlType)
            {
                case ControlType.AI:
                    obj.aiInfo.behavior = br.ReadByte();
                    for (int i = 0; i < AIInfo.NumAIFlags; i++)
                        obj.aiInfo.aiFlags[i] = br.ReadByte();

                    obj.aiInfo.hideSegment = br.ReadInt16();
                    obj.aiInfo.hideIndex = br.ReadInt16();
                    obj.aiInfo.pathLength = br.ReadInt16();
                    obj.aiInfo.curPathIndex = br.ReadInt16();

                    if (gameDataVersion <= 25)
                    {
                        br.ReadInt32();
                    }
                    break;
                case ControlType.Explosion:
                    obj.explosionInfo.SpawnTime = br.ReadInt32();
                    obj.explosionInfo.DeleteTime = br.ReadInt32();
                    obj.explosionInfo.DeleteObject = br.ReadInt16();
                    break;
                case ControlType.Powerup:
                    if (gameDataVersion >= 25)
                    {
                        obj.powerupCount = br.ReadInt32();
                    }
                    break;
            }
            switch (obj.renderType)
            {
                case RenderType.Polyobj:
                    {
                        obj.modelInfo.modelNum = br.ReadInt32();
                        for (int i = 0; i < Polymodel.MAX_SUBMODELS; i++)
                        {
                            obj.modelInfo.animAngles[i] = ReadAngles(br);
                        }
                        obj.modelInfo.flags = br.ReadInt32();
                        obj.modelInfo.textureOverride = br.ReadInt32();
                    }
                    break;
                case RenderType.WeaponVClip:
                case RenderType.Hostage:
                case RenderType.Powerup:
                case RenderType.Fireball:
                    obj.spriteInfo.vclipNum = br.ReadInt32();
                    obj.spriteInfo.frameTime = br.ReadInt32();
                    obj.spriteInfo.frameNumber = br.ReadByte();
                    break;
            }

            if (obj.type == ObjectType.Robot && obj.id == 33)
            {
                //obj.id = 35; //no buddies for you
                obj.id = 92;
                obj.type = ObjectType.Clutter;
                obj.controlType = ControlType.None;
                obj.moveType = MovementType.None;
                obj.modelInfo.modelNum = 92;
                obj.containsType = (byte)ObjectType.Powerup;
                obj.containsId = 18;
                obj.containsCount = 3;
            }

            //molest all homing missiles
            if (obj.type == ObjectType.Powerup && obj.id == 18)
            {
                obj.renderType = RenderType.Polyobj;
                obj.modelInfo.modelNum = 1;
                obj.modelInfo.textureOverride = -1;
                obj.moveType = MovementType.Spinning;
                obj.spinRate = FixVector.FromRawValues(0, (int)(65536 / 1.5), 0);
            }

            if (obj.type == ObjectType.Powerup && obj.id == 19)
            {
                obj.renderType = RenderType.Polyobj;
                obj.modelInfo.modelNum = 3;
                obj.modelInfo.textureOverride = -1;
                obj.moveType = MovementType.Spinning;
                obj.spinRate = FixVector.FromRawValues(0, (int)(65536 / 1.5), 0);
            }

            return obj;
        }

        private int SaveGameData(BinaryWriter bw)
        {
            bw.Write((short)0x6705);
            //gameDataVersion = br.ReadInt16();
            bw.Write((short)32);
            //int gameDataSize = br.ReadInt32();
            bw.Write(GameDataSize); //updated later
            for (int i = 0; i < 15; i++)
            {
                bw.Write((byte)0); //map name is always uninitalized I guess
            }
            bw.Write(levelNumber);
            long tableOffset = bw.BaseStream.Position;
            bw.Write(0); //Player offset 0
            bw.Write(PlayerSize); //updated later 4
            bw.Write(0); //Obj offset 8
            bw.Write(objects.Count); //12
            bw.Write(ObjectSize); //16
            bw.Write(0); //Walls offset 20
            bw.Write(walls.Count); //24
            bw.Write(WallSize); //28
            bw.Write(0); //Doors offset //32
            bw.Write(0); //36
            bw.Write(DoorSize); //40
            bw.Write(0); //Walls offset //44
            bw.Write(triggers.Count); //48
            bw.Write(TriggerSize); //52
            bw.Write(0); //Links offset //56
            bw.Write(0); //60
            bw.Write(0); //64
            bw.Write(0); //Control offset 68
            bw.Write(1); //Control count, only 1 72
            bw.Write(ControlSize); //76
            bw.Write(0); //Matcen offset //80
            bw.Write(matcens.Count); //84
            bw.Write(MatcenSize); //88
            bw.Write(0); //DLIndex offset // 92
            bw.Write(dlIndexes.Count); //96
            bw.Write(DLIndexSize); //100
            bw.Write(0); //DL offset 104
            bw.Write(deltaLights.Count); //108
            bw.Write(DLSize); //112

            for (int i = 0; i < levelName.Length; i++)
            {
                bw.Write((byte)levelName[i]);
            }
            bw.Write((byte)'\n');
            bw.Write((short)pofFiles.Count);
            string pofName;
            for (int i = 0; i < pofFiles.Count; i++)
            {
                pofName = pofFiles[i];
                for (int ch = 0; ch < 13; ch++)
                {
                    if (ch < pofName.Length)
                        bw.Write((byte)pofName[ch]);
                    else
                        bw.Write((byte)0);
                }
            }
            int objOffset = (int)bw.BaseStream.Position;
            foreach (EditorObject obj in objects)
            {
                WriteObject(bw, obj);
            }
            int wallsOffset = (int)bw.BaseStream.Position;
            if (walls.Count == 0)
                wallsOffset = -1;

            foreach (Wall wall in walls)
            {
                bw.Write(wall.segnum);
                bw.Write(wall.sidenum);
                bw.Write(wall.hp);
                bw.Write(wall.linkedWall);
                bw.Write((byte)wall.type);
                bw.Write(wall.flags);
                bw.Write(wall.state);
                bw.Write(wall.trigger);
                bw.Write(wall.clipNum);
                bw.Write(wall.keys);
                bw.Write(wall.controllingTrigger);
                bw.Write(wall.cloakValue);
            }

            int triggersOffset = (int)bw.BaseStream.Position;
            if (triggers.Count == 0)
                triggersOffset = -1;
            foreach (Trigger trigger in triggers)
            {
                bw.Write((byte)trigger.type);
                bw.Write(trigger.flags);
                bw.Write(trigger.numLinks);
                bw.Write((byte)0); //padding byte
                bw.Write(trigger.value);
                bw.Write(trigger.time);
                for (int j = 0; j < Trigger.MaxWallsPerLink; j++)
                    bw.Write(trigger.seg[j]);
                for (int j = 0; j < Trigger.MaxWallsPerLink; j++)
                    bw.Write(trigger.side[j]);
            }
            int controlOffset = (int)bw.BaseStream.Position;

            bw.Write(controlCenterTrigger.numLinks);
            for (int j = 0; j < ControlCenter.MaxControlCenterLinks; j++)
                bw.Write(controlCenterTrigger.seg[j]);
            for (int j = 0; j < ControlCenter.MaxControlCenterLinks; j++)
                bw.Write(controlCenterTrigger.side[j]);

            int matcenOffset = (int)bw.BaseStream.Position;
            if (matcens.Count == 0)
                matcenOffset = -1;

            foreach (MatCenter matcen in matcens)
            {
                bw.Write(matcen.robotFlags[0]);
                bw.Write(matcen.robotFlags[1]);
                bw.Write(matcen.hitPoints);
                bw.Write(matcen.interval);
                bw.Write(matcen.segnum);
                bw.Write(matcen.fuelcenNum);
            }
            int dlIndexOffset = (int)bw.BaseStream.Position;
            if (dlIndexes.Count == 0)
                dlIndexOffset = -1;

            foreach (DynamicLightIndex index in dlIndexes)
            {
                bw.Write(index.segnum);
                bw.Write(index.sidenum);
                bw.Write(index.count);
                bw.Write(index.index);
            }

            int dlOffset = (int)bw.BaseStream.Position;
            if (deltaLights.Count == 0)
                dlOffset = -1;

            foreach (DynamicLight light in deltaLights)
            {
                bw.Write(light.segnum);
                bw.Write(light.sidenum);
                bw.Write((byte)0);
                bw.Write(light.vertLight[0]);
                bw.Write(light.vertLight[1]);
                bw.Write(light.vertLight[2]);
                bw.Write(light.vertLight[3]);
            }

            bw.BaseStream.Seek(tableOffset, SeekOrigin.Begin); bw.Write(objOffset);
            bw.BaseStream.Seek(tableOffset+8, SeekOrigin.Begin); bw.Write(objOffset);
            bw.BaseStream.Seek(tableOffset+20, SeekOrigin.Begin); bw.Write(wallsOffset);
            bw.BaseStream.Seek(tableOffset+44, SeekOrigin.Begin); bw.Write(triggersOffset);
            bw.BaseStream.Seek(tableOffset+68, SeekOrigin.Begin); bw.Write(controlOffset);
            bw.BaseStream.Seek(tableOffset+80, SeekOrigin.Begin); bw.Write(matcenOffset);
            bw.BaseStream.Seek(tableOffset+92, SeekOrigin.Begin); bw.Write(dlIndexOffset);
            bw.BaseStream.Seek(tableOffset+104, SeekOrigin.Begin); bw.Write(dlOffset);

            return 0;
        }

        private void WriteObject(BinaryWriter bw, EditorObject obj)
        {
            bw.Write((byte)obj.type);
            bw.Write(obj.id);
            bw.Write((byte)obj.controlType);
            bw.Write((byte)obj.moveType);
            bw.Write((byte)obj.renderType);
            bw.Write(obj.flags);
            bw.Write(obj.segnum);
            WriteFixVec(bw, obj.position);
            WriteMatrix(bw, obj.orientation);
            bw.Write(obj.size);
            bw.Write(obj.shields);
            WriteFixVec(bw, obj.lastPos);
            bw.Write(obj.containsType);
            bw.Write(obj.containsId);
            bw.Write(obj.containsCount);

            switch (obj.moveType)
            {
                case MovementType.Physics:
                    WriteFixVec(bw, obj.physicsInfo.velocity);
                    WriteFixVec(bw, obj.physicsInfo.thrust);
                    bw.Write(obj.physicsInfo.mass);
                    bw.Write(obj.physicsInfo.drag);
                    bw.Write(obj.physicsInfo.brakes);
                    WriteFixVec(bw, obj.physicsInfo.angVel);
                    WriteFixVec(bw, obj.physicsInfo.rotThrust);
                    bw.Write(obj.physicsInfo.turnroll);
                    bw.Write(obj.physicsInfo.flags);
                    break;
                case MovementType.Spinning:
                    WriteFixVec(bw, obj.spinRate);
                    break;
            }
            switch (obj.controlType)
            {
                case ControlType.AI:
                    bw.Write(obj.aiInfo.behavior);
                    for (int i = 0; i < AIInfo.NumAIFlags; i++)
                        bw.Write(obj.aiInfo.aiFlags[i]);

                    bw.Write(obj.aiInfo.hideSegment);
                    bw.Write(obj.aiInfo.hideIndex);
                    bw.Write(obj.aiInfo.pathLength);
                    bw.Write(obj.aiInfo.curPathIndex);

                    break;
                case ControlType.Explosion:
                    bw.Write(obj.explosionInfo.SpawnTime);
                    bw.Write(obj.explosionInfo.DeleteTime);
                    bw.Write(obj.explosionInfo.DeleteObject);
                    break;
                case ControlType.Powerup:
                    if (gameDataVersion >= 25)
                    {
                        bw.Write(obj.powerupCount);
                    }
                    break;
            }
            switch (obj.renderType)
            {
                case RenderType.Polyobj:
                    {
                        bw.Write(obj.modelInfo.modelNum);
                        for (int i = 0; i < Polymodel.MAX_SUBMODELS; i++)
                        {
                            WriteAngles(bw, obj.modelInfo.animAngles[i]);
                        }
                        bw.Write(obj.modelInfo.flags);
                        bw.Write(obj.modelInfo.textureOverride);
                    }
                    break;
                case RenderType.WeaponVClip:
                case RenderType.Hostage:
                case RenderType.Powerup:
                case RenderType.Fireball:
                    bw.Write(obj.spriteInfo.vclipNum);
                    bw.Write(obj.spriteInfo.frameTime);
                    bw.Write(obj.spriteInfo.frameNumber);
                    break;
            }
        }

        private FixVector ReadFixVec(BinaryReader br)
        {
            FixVector vec;
            vec.x = Fix.FromRawValue(br.ReadInt32());
            vec.y = Fix.FromRawValue(br.ReadInt32());
            vec.z = Fix.FromRawValue(br.ReadInt32());
            return vec;
        }

        private FixAngles ReadAngles(BinaryReader br)
        {
            FixAngles vec;
            vec.p = br.ReadInt16();
            vec.b = br.ReadInt16();
            vec.h = br.ReadInt16();
            return vec;
        }

        private FixMatrix ReadMatrix(BinaryReader br)
        {
            FixMatrix mat;
            mat.right = ReadFixVec(br);
            mat.forward = ReadFixVec(br);
            mat.up = ReadFixVec(br);
            return mat;
        }

        private void WriteFixVec(BinaryWriter bw, FixVector vec)
        {
            bw.Write(vec.x);
            bw.Write(vec.y);
            bw.Write(vec.z);
        }

        private void WriteAngles(BinaryWriter bw, FixAngles vec)
        {
            bw.Write(vec.p);
            bw.Write(vec.b);
            bw.Write(vec.h);
        }

        private void WriteMatrix(BinaryWriter bw, FixMatrix mat)
        {
            WriteFixVec(bw, mat.right);
            WriteFixVec(bw, mat.forward);
            WriteFixVec(bw, mat.up);
        }
    }
}
