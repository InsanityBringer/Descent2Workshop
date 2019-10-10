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
    public struct ReplacedBitmapElement
    {
        public int replacementID;
        public ushort data; 
    }
    public struct ReplacedModelID
    {
        public int replacementID;
        public int data;
    }
    public class HXMFile : IElementManager
    {
        public HAMFile baseFile;
        public VHAMFile augmentFile;
        public List<string> RobotNames = new List<string>();
        public List<string> ModelNames = new List<string>();
        public int sig, ver;
        public List<Robot> replacedRobots = new List<Robot>();
        public List<JointPos> replacedJoints = new List<JointPos>();
        public List<Polymodel> replacedModels = new List<Polymodel>();
        public List<PolymodelData> replacedModelData = new List<PolymodelData>();
        public List<ReplacedModelID> replacedDyingModelnums = new List<ReplacedModelID>();
        public List<ReplacedModelID> replacedDeadModelnums = new List<ReplacedModelID>();
        public List<ReplacedBitmapElement> replacedObjBitmaps = new List<ReplacedBitmapElement>();
        public List<ReplacedBitmapElement> replacedObjBitmapPtrs = new List<ReplacedBitmapElement>();

        //public List<JointPos> Joints = new List<JointPos>();
        //public List<Polymodel> PolygonModels = new List<Polymodel>();
        //public List<ushort> ObjBitmaps = new List<ushort>(); //thankfully only needed a few times. 
        //public List<ushort> ObjBitmapPointers = new List<ushort>(); //thankfully only needed a few times. 

        private Dictionary<int, Polymodel> remappedModels = new Dictionary<int, Polymodel>();

        public HXMFile(HAMFile baseFile)
        {
            this.baseFile = baseFile;
        }

        public int LoadDataFile(string filename)
        {
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

            HAMDataReader data = new HAMDataReader();

            sig = br.ReadInt32();
            ver = br.ReadInt32();

            int replacedRobotCount = br.ReadInt32();
            for (int x = 0; x < replacedRobotCount; x++)
            {
                int replacementID = br.ReadInt32();
                Robot robot = data.ReadRobot(br);
                robot.replacementID = replacementID;
                replacedRobots.Add(robot);
            }
            int replacedJointCount = br.ReadInt32();
            for (int x = 0; x < replacedJointCount; x++)
            {
                int replacementID = br.ReadInt32();
                JointPos joint = new JointPos();
                joint.jointnum = br.ReadInt16();
                joint.angles.p = br.ReadInt16();
                joint.angles.b = br.ReadInt16();
                joint.angles.h = br.ReadInt16();
                joint.replacementID = replacementID;
                replacedJoints.Add(joint);
            }
            int modelsToReplace = br.ReadInt32();
            for (int x = 0; x < modelsToReplace; x++)
            {
                int replacementID = br.ReadInt32();
                Polymodel model = data.ReadPolymodelInfo(br);
                model.replacementID = replacementID;
                PolymodelData modeldata = new PolymodelData(model.model_data_size);
                modeldata.InterpreterData = br.ReadBytes(model.model_data_size);
                modeldata.BuildPolymodelData(0, model, "");
                model.data = modeldata;
                replacedModels.Add(model);
                replacedModelData.Add(modeldata);
                model.DyingModelnum = br.ReadInt32();
                model.DeadModelnum = br.ReadInt32();
            }
            int objBitmapsToReplace = br.ReadInt32();
            for (int x = 0; x < objBitmapsToReplace; x++)
            {
                ReplacedBitmapElement objBitmap = new ReplacedBitmapElement();
                objBitmap.replacementID = br.ReadInt32();
                objBitmap.data = br.ReadUInt16();
                replacedObjBitmaps.Add(objBitmap);
                //Console.WriteLine("Loading replacement obj bitmap, replacing slot {0} with {1} ({2})", objBitmap.replacementID, objBitmap.data, baseFile.piggyFile.images[objBitmap.data].name);
            }
            int objBitmapPtrsToReplace = br.ReadInt32();
            for (int x = 0; x < objBitmapPtrsToReplace; x++)
            {
                ReplacedBitmapElement objBitmap = new ReplacedBitmapElement();
                objBitmap.replacementID = br.ReadInt32();
                objBitmap.data = br.ReadUInt16();
                replacedObjBitmapPtrs.Add(objBitmap);
            }

            GenerateNameTable();
            BuildModelTextureTables(); //fuck hxm files
            foreach (Robot robot in replacedRobots)
            {
                BuildModelAnimation(robot);
            }
            br.Close();
            return 0;
        }

        //Ultimately, it is too much of a pain to stick to the object bitmap and object bitmap pointer tables
        //Instead, don't track them at all in the first place. Build a texture list for each model, and only
        //reconstruct the tables at the time of export
        //I dunno why i'm being as masochistic as I am with this but okay. 
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
                    EClipNames.Add(clip.changing_object_texture, ElementLists.GetEClipName(i));
                }
            }
            ushort bitmap; string name;
            for (int i = 0; i < baseFile.ObjBitmaps.Count; i++)
            {
                bitmap = GetObjBitmap(i);
                if (bitmap == 0) continue;
                PIGImage image = baseFile.piggyFile.images[bitmap];
                name = image.name.ToLower();
                if (!image.isAnimated)
                {
                    TextureNames.Add(i, name);
                }
            }
            foreach (Polymodel model in replacedModels)
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
                Console.Write("Model texture list: [");
                foreach (string texture in model.textureList)
                {
                    Console.Write("{0} ", texture);
                }
                Console.WriteLine("]");
                model.BaseTexture = FindFirstObjBitmap(model);
            }
        }

        private void BuildModelAnimation(Robot robot)
        {
            int lowestJoint = int.MaxValue;
            if (robot.model_num == -1) return;
            Polymodel model = GetModel(robot.model_num);
            if (model.replacementID == -1) return;
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
            for (int m = 0; m < robot.n_guns + 1; m++)
            {
                for (int f = 0; f < Robot.NUM_ANIMATION_STATES; f++)
                {
                    Robot.jointlist robotjointlist = robot.anim_states[m, f];
                    basejoint = robotjointlist.offset;
                    if (basejoint < lowestJoint)
                        lowestJoint = basejoint;
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

            if (lowestJoint != int.MaxValue)
                robot.baseJoint = lowestJoint;
        }

        public void GenerateNameTable()
        {
            foreach (string name in baseFile.RobotNames)
            {
                RobotNames.Add(name);
            }
            for (int i = 0; i < replacedRobots.Count; i++)
            {
                int replacement = replacedRobots[i].replacementID;
                RobotNames[replacement] = string.Format("New Robot {0}", i);
            }
        }

        public void SaveDataFile(string name)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(name, FileMode.Create));
            HAMDataWriter datawriter = new HAMDataWriter();

            bw.Write(sig);
            bw.Write(ver);

            bw.Write(replacedRobots.Count);
            for (int x = 0; x < replacedRobots.Count; x++)
            {
                bw.Write(replacedRobots[x].replacementID);
                datawriter.WriteRobot(replacedRobots[x], bw);
            }
            bw.Write(replacedJoints.Count);
            for (int x = 0; x < replacedJoints.Count; x++)
            {
                bw.Write(replacedJoints[x].replacementID);
                bw.Write(replacedJoints[x].jointnum);
                bw.Write(replacedJoints[x].angles.p);
                bw.Write(replacedJoints[x].angles.b);
                bw.Write(replacedJoints[x].angles.h);
            }
            bw.Write(replacedModels.Count);
            for (int x = 0; x < replacedModels.Count; x++)
            {
                bw.Write(replacedModels[x].replacementID);
                datawriter.WritePolymodel(replacedModels[x], bw);
                bw.Write(replacedModels[x].data.InterpreterData);
                bw.Write(replacedDyingModelnums[x].data);
                bw.Write(replacedDeadModelnums[x].data);
            }
            bw.Write(replacedObjBitmaps.Count);
            for (int x = 0; x < replacedObjBitmaps.Count; x++)
            {
                bw.Write(replacedObjBitmaps[x].replacementID);
                bw.Write(replacedObjBitmaps[x].data);
            }
            bw.Write(replacedObjBitmapPtrs.Count);
            for (int x = 0; x < replacedObjBitmapPtrs.Count; x++)
            {
                bw.Write(replacedObjBitmapPtrs[x].replacementID);
                bw.Write(replacedObjBitmapPtrs[x].data);
            }

            bw.Close();
        }

        public Robot GetRobotAt(int p)
        {
            return replacedRobots[p];
        }

        public TMAPInfo GetTMAPInfo(int id)
        {
            return baseFile.GetTMAPInfo(id);
        }

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

        public int GetNumRobots()
        {
            int numRobots = baseFile.Robots.Count;
            if (augmentFile != null)
                numRobots += augmentFile.Robots.Count;
            return numRobots;
        }

        public string GetRobotName(int id)
        {
            if (augmentFile != null && id >= VHAMFile.N_D2_ROBOT_TYPES)
                return augmentFile.RobotNames[id - VHAMFile.N_D2_ROBOT_TYPES];
            return baseFile.RobotNames[id];
        }

        public Robot GetRobot(int id)
        {
            foreach (Robot robot in replacedRobots)
            {
                if (robot.ID == id) return robot;
            }
            if (augmentFile != null)
                return augmentFile.GetRobot(id); //passes through
            return baseFile.GetRobot(id);
        }

        public int GetNumWeapons()
        {
            int numWeapons = baseFile.Weapons.Count;
            if (augmentFile != null)
                numWeapons += augmentFile.Weapons.Count;
            return numWeapons;
        }

        public string GetWeaponName(int id)
        {
            if (augmentFile != null && id >= VHAMFile.N_D2_WEAPON_TYPES)
                return augmentFile.WeaponNames[id - VHAMFile.N_D2_WEAPON_TYPES];
            return baseFile.WeaponNames[id];
        }

        public Weapon GetWeapon(int id)
        {
            if (augmentFile != null)
                return augmentFile.GetWeapon(id); //passes through
            return baseFile.GetWeapon(id);
        }

        public int GetNumModels()
        {
            int numWeapons = baseFile.PolygonModels.Count;
            if (augmentFile != null)
                numWeapons += augmentFile.Models.Count;
            return numWeapons;
        }

        public string GetModelName(int id)
        {
            if (augmentFile != null && id >= VHAMFile.N_D2_POLYGON_MODELS)
                return augmentFile.ModelNames[id - VHAMFile.N_D2_POLYGON_MODELS];
            return baseFile.ModelNames[id];
        }

        public Polymodel GetModel(int id)
        {
            foreach (Polymodel model in replacedModels)
            {
                if (model.replacementID == id) return model;
            }
            if (augmentFile != null)
                return augmentFile.GetModel(id);
            return baseFile.GetModel(id);
        }

        public Powerup GetPowerup(int id)
        {
            return baseFile.GetPowerup(id);
        }

        public Reactor GetReactor(int id)
        {
            return baseFile.GetReactor(id);
        }

        public JointPos GetJoint(int id)
        {
            foreach (JointPos joint in replacedJoints)
            {
                if (joint.replacementID == id) return joint;
            }
            if (augmentFile != null)
                return augmentFile.GetJoint(id);
            return baseFile.Joints[id];
        }

        public ushort GetObjBitmap(int id)
        {
            foreach (ReplacedBitmapElement bitmap in replacedObjBitmaps)
                if (bitmap.replacementID == id) return bitmap.data;
            if (augmentFile != null)
                return augmentFile.GetObjBitmap(id);
            return baseFile.ObjBitmaps[id];
        }

        public ushort GetObjBitmapPointer(int id)
        {
            foreach (ReplacedBitmapElement bitmap in replacedObjBitmapPtrs)
                if (bitmap.replacementID == id) return bitmap.data;
            if (augmentFile != null)
                return augmentFile.GetObjBitmapPointer(id);
            return baseFile.ObjBitmapPointers[id];
        }

        public int CountUniqueObjBitmaps(Polymodel model)
        {
            int num = 0;
            foreach (string tex in model.textureList)
            {
                if (!baseFile.ObjBitmapMapping.ContainsKey(tex))
                {
                    num++;
                }
            }
            return num;
        }

        public int FindFirstObjBitmap(Polymodel model)
        {
            int num = int.MaxValue;
            string tex;
            for (int i = 0; i < model.n_textures; i++)
            {
                tex = model.textureList[i];
                if (!baseFile.ObjBitmapMapping.ContainsKey(tex))
                {
                    //This texture isn't present in the base file, so it's new. Figure out where it is
                    num = GetObjBitmapPointer(model.first_texture + i);
                }
            }
            if (num == int.MaxValue) return 0;
            return num;
        }
    }
}
