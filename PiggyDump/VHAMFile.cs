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

namespace PiggyDump
{
    public class VHAMFile : IElementManager
    {
        public HAMFile baseFile;
        public PIGFile piggyFile;

        public string lastFilename;
        public string datafile;

        public List<Robot> Robots = new List<Robot>();
        public List<Weapon> Weapons = new List<Weapon>();
        public List<Polymodel> Models = new List<Polymodel>();
        public List<JointPos> Joints = new List<JointPos>();
        public List<ushort> ObjBitmaps = new List<ushort>();
        public List<ushort> ObjBitmapPointers = new List<ushort>();

        //Namelists
        public List<string> RobotNames = new List<string>();
        public List<string> WeaponNames = new List<string>();
        public List<string> ModelNames = new List<string>();

        //ARGH
        //VHAM elements are loaded at fixed locations
        const int N_D2_ROBOT_TYPES = 66;
        const int N_D2_ROBOT_JOINTS = 1145;
        const int N_D2_POLYGON_MODELS = 166;
        const int N_D2_OBJBITMAPS = 422;
        const int N_D2_OBJBITMAPPTRS = 502;
        const int N_D2_WEAPON_TYPES = 62;

        public int NumRobots { get { return Robots.Count + N_D2_ROBOT_TYPES; } }
        public int NumWeapons { get { return Weapons.Count + N_D2_WEAPON_TYPES; } }
        public int NumModels { get { return Models.Count + N_D2_POLYGON_MODELS; } }

        public VHAMFile(HAMFile baseFile)
        {
            this.baseFile = baseFile;
            this.piggyFile = baseFile.piggyFile;
        }

        public int LoadDataFile(string filename)
        {
            lastFilename = filename;
            //If a namefile isn't present, automatically generate namelists for our convenience. 
            bool generateNameLists = true;
            datafile = filename;
            BinaryReader br;
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
            HAMDataReader bm = new HAMDataReader();
            int sig = br.ReadInt32();
            if (sig != 0x5848414D)
            {
                br.Close();
                br.Dispose();
                return -1;
            }
            int version = br.ReadInt32();
            if (version != 1)
            {
                br.Close();
                br.Dispose();
                return -2;
            }

            int numWeapons = br.ReadInt32();
            for (int i = 0; i < numWeapons; i++)
            {
                Weapons.Add(bm.ReadWeapon(br));
                Weapons[i].ID = i + N_D2_WEAPON_TYPES;
            }
            int numRobots = br.ReadInt32();
            for (int i = 0; i < numRobots; i++)
            {
                Robots.Add(bm.ReadRobot(br));
                Robots[i].ID = i + N_D2_ROBOT_TYPES;
            }
            int numJoints = br.ReadInt32();
            for (int i = 0; i < numJoints; i++)
            {
                JointPos joint = new JointPos();
                joint.jointnum = br.ReadInt16();
                joint.angles.p = br.ReadInt16();
                joint.angles.b = br.ReadInt16();
                joint.angles.h = br.ReadInt16();
                Joints.Add(joint);
            }
            int numModels = br.ReadInt32();
            for (int i = 0; i < numModels; i++)
            {
                Models.Add(bm.ReadPolymodelInfo(br));
                Models[i].ID = i + N_D2_POLYGON_MODELS;
            }
            for (int x = 0; x < numModels; x++)
            {
                PolymodelData modeldata = new PolymodelData(Models[x].model_data_size);
                for (int y = 0; y < Models[x].model_data_size; y++)
                {
                    modeldata.InterpreterData[y] = br.ReadByte();
                }
                modeldata.BuildPolymodelData(x, Models[x], "");
                Models[x].data = modeldata;
                //PolymodelData.Add(modeldata);
            }
            for (int i = 0; i < numModels; i++)
            {
                Models[i].DyingModelnum = br.ReadInt32();
            }
            for (int i = 0; i < numModels; i++)
            {
                Models[i].DeadModelnum = br.ReadInt32();
            }
            int numObjBitmaps = br.ReadInt32();
            for (int i = 0; i < numObjBitmaps; i++)
            {
                ObjBitmaps.Add(br.ReadUInt16());
            }
            int numObjBitmapPointers = br.ReadInt32();
            for (int i = 0; i < numObjBitmapPointers; i++)
            {
                ObjBitmapPointers.Add(br.ReadUInt16());
            }
            foreach (Robot robot in Robots)
            {
                BuildModelAnimation(robot);
            }
            BuildModelTextureTables();
            BuildReferenceLists();

            if (generateNameLists)
            {
                for (int i = 0; i < Weapons.Count; i++)
                {
                    WeaponNames.Add(String.Format("New Weapon {0}", i+1));
                }
                for (int i = 0; i < Robots.Count; i++)
                {
                    RobotNames.Add(String.Format("New Robot {0}", i+1));
                }
                for (int i = 0; i < Models.Count; i++)
                {
                    ModelNames.Add(String.Format("New Model {0}", i+1));
                }
            }

            br.Close();

            return 0;
        }

        private void BuildModelAnimation(Robot robot)
        {
            //this shouldn't happen?
            if (robot.model_num == -1) return;
            //If the robot is referring to a base HAM file model, reject it
            if (robot.model_num < N_D2_POLYGON_MODELS) return;
            Polymodel model = Models[robot.model_num - N_D2_POLYGON_MODELS];
            List<FixAngles> jointlist = new List<FixAngles>();
            model.numGuns = robot.n_guns;
            for (int i = 0; i < Polymodel.MAX_GUNS; i++)
            {
                model.gunPoints[i] = robot.gun_points[i];
                model.gunDirs[i] = new FixVector(65536, 0, 0);
                model.gunSubmodels[i] = robot.gun_submodels[i];
            }
            int[,] jointmapping = new int[10, 5];
            for (int m = 0; m < Polymodel.MAX_SUBMODELS; m++)
            {
                for (int f = 0; f < Robot.NUM_ANIMATION_STATES; f++)
                {
                    jointmapping[m, f] = -1;
                }
            }
            int basejoint = 0;
            for (int m = 0; m < Polymodel.MAX_GUNS + 1; m++)
            {
                for (int f = 0; f < Robot.NUM_ANIMATION_STATES; f++)
                {
                    Robot.jointlist robotjointlist = robot.anim_states[m, f];
                    basejoint = robotjointlist.offset;
                    for (int j = 0; j < robotjointlist.n_joints; j++)
                    {
                        JointPos joint = GetJoint(basejoint);
                        jointmapping[joint.jointnum, f] = basejoint;
                        model.isAnimated = true;
                        basejoint++;
                    }
                }
            }

            for (int m = 1; m < Polymodel.MAX_SUBMODELS; m++)
            {
                for (int f = 0; f < Robot.NUM_ANIMATION_STATES; f++)
                {
                    int jointnum = jointmapping[m, f];
                    if (jointnum != -1)
                    {
                        JointPos joint = GetJoint(jointnum);
                        model.animationMatrix[m, f].p = joint.angles.p;
                        model.animationMatrix[m, f].b = joint.angles.b;
                        model.animationMatrix[m, f].h = joint.angles.h;
                    }
                }
            }
        }

        //Variation of the HAM one, only applies to new models
        public void BuildModelTextureTables()
        {
            //Write down unanimated texture names
            Dictionary<int, string> TextureNames = new Dictionary<int, string>();
            //Write down EClip IDs for tracking animated texture names
            Dictionary<int, string> EClipNames = new Dictionary<int, string>();
            EClip clip;
            for (int i = 0; i < baseFile.EClips.Count; i++)
            {
                clip = baseFile.EClips[i];
                if (clip.changing_object_texture != -1)
                {
                    EClipNames.Add(clip.changing_object_texture, baseFile.EClipNames[i]);
                }
            }
            ushort bitmap; string name;
            for (int i = 0; i < N_D2_OBJBITMAPS + ObjBitmaps.Count; i++)
            {
                bitmap = GetObjBitmap(i);
                if (bitmap == 0) continue;
                PIGImage image = piggyFile.images[bitmap];
                name = image.name.ToLower();
                if (!image.isAnimated)
                {
                    TextureNames.Add(i, name);
                }
            }
            foreach (Polymodel model in Models)
            {
                model.useTexList = true;
                int textureID, pointer;
                for (int i = model.first_texture; i < (model.first_texture + model.n_textures); i++)
                {
                    pointer = GetObjBitmapPointer(i);
                    textureID = GetObjBitmap(pointer);
                    if (EClipNames.ContainsKey(pointer))
                    {
                        model.textureList.Add(EClipNames[pointer]);
                    }
                    else if (TextureNames.ContainsKey(pointer))
                    {
                        model.textureList.Add(TextureNames[pointer]);
                    }
                }
                Console.Write("Addon model texture list: [");
                foreach (string texture in model.textureList)
                {
                    Console.Write("{0} ", texture);
                }
                Console.WriteLine("]");
            }
        }

        //Reference counting. When adding and deleting elements, we must check that nothing is actually referencing
        //an element being deleted, to avoid baaad problem.
        //this is annoying and sucks aaa
        private void BuildReferenceLists()
        {
            Robot robot;
            for (int i = 0; i < Robots.Count; i++)
            {
                robot = Robots[i];
                robot.InitReferences(this);
                robot.AssignReferences(this);
            }
            Weapon weapon;
            for (int i = 0; i < Weapons.Count; i++)
            {
                weapon = Weapons[i];
                weapon.InitReferences(this);
                weapon.AssignReferences(this);
            }
            Polymodel model;
            for (int i = 0; i < Models.Count; i++)
            {
                model = Models[i];
                model.InitReferences(this);
                model.AssignReferences(this);
            }
        }

        //Convenience members to access elements by their absolute ID, when needed
        public Robot GetRobot(int id)
        {
            if (id >= 0 && id < baseFile.Robots.Count && id < N_D2_ROBOT_TYPES)
                return baseFile.Robots[id];
            else if (id >= N_D2_ROBOT_TYPES)
                return Robots[id - N_D2_ROBOT_TYPES];
            //sorry, you get null and you better like it
            return null;
        }

        public Weapon GetWeapon(int id)
        {
            if (id >= 0 && id < baseFile.Weapons.Count && id < N_D2_WEAPON_TYPES)
                return baseFile.Weapons[id];
            else if (id >= N_D2_WEAPON_TYPES)
                return Weapons[id - N_D2_WEAPON_TYPES];
            return null;
        }

        public Polymodel GetModel(int id)
        {
            if (id >= 0 && id < baseFile.PolygonModels.Count && id < N_D2_POLYGON_MODELS)
                return baseFile.PolygonModels[id];
            else if (id >= N_D2_POLYGON_MODELS)
                return Models[id - N_D2_POLYGON_MODELS];
            return null;
        }

        public JointPos GetJoint(int id)
        {
            if (id >= 0 && id < baseFile.Joints.Count && id < N_D2_ROBOT_JOINTS)
                return baseFile.Joints[id];
            else if (id >= N_D2_ROBOT_JOINTS)
                return Joints[id- N_D2_ROBOT_JOINTS];
            return new JointPos(); //shouldn't happen
        }

        public ushort GetObjBitmap(int id)
        {
            if (id >= 0 && id < baseFile.ObjBitmaps.Count && (id < N_D2_OBJBITMAPS || id >= ObjBitmaps.Count + N_D2_OBJBITMAPS))
                return baseFile.ObjBitmaps[id];
            else if (id >= N_D2_OBJBITMAPS)
                return ObjBitmaps[id - N_D2_OBJBITMAPS];
            return 0;
        }

        public ushort GetObjBitmapPointer(int id)
        {
            if (id >= 0 && id < baseFile.ObjBitmaps.Count && (id < N_D2_OBJBITMAPPTRS || id >= ObjBitmapPointers.Count + N_D2_OBJBITMAPPTRS))
                return baseFile.ObjBitmapPointers[id];
            else if (id >= N_D2_OBJBITMAPPTRS)
                return ObjBitmapPointers[id - N_D2_OBJBITMAPPTRS];
            return 0;
        }

        //same for strings
        public string GetRobotName(int id)
        {
            if (id >= 0 && id < baseFile.Robots.Count && id < N_D2_ROBOT_TYPES)
                return baseFile.RobotNames[id];
            else if (id >= N_D2_ROBOT_TYPES)
                return RobotNames[id - N_D2_ROBOT_TYPES];
            return "<undefined>";
        }

        public string GetWeaponName(int id)
        {
            if (id >= 0 && id < baseFile.Weapons.Count && id < N_D2_WEAPON_TYPES)
                return baseFile.WeaponNames[id];
            else if (id >= N_D2_WEAPON_TYPES)
                return WeaponNames[id - N_D2_WEAPON_TYPES];
            return "<undefined>";
        }

        public string GetModelName(int id)
        {
            if (id >= 0 && id < baseFile.PolygonModels.Count && id < N_D2_POLYGON_MODELS)
                return baseFile.ModelNames[id];
            else if (id >= N_D2_POLYGON_MODELS)
                return ModelNames[id - N_D2_POLYGON_MODELS];
            return "<undefined>";
        }

        //These shouldn't be called. Probably. They will inevitably be called
        public TMAPInfo GetTMAPInfo(int id)
        {
            throw new NotImplementedException();
        }

        //Passthrough nonedited elements to the base file. 
        public VClip GetVClip(int id)
        {
            return baseFile.GetVClip(id);
        }

        public EClip GetEClip(int id)
        {
            return baseFile.GetEClip(id);
        }

        public WClip GetWClip(int id)
        {
            return baseFile.GetWClip(id);
        }

        public Powerup GetPowerup(int id)
        {
            return baseFile.GetPowerup(id);
        }

        public Reactor GetReactor(int id)
        {
            return baseFile.GetReactor(id);
        }
    }
}
