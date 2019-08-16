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
using System.Text;
using System.IO;

namespace PiggyDump
{
    public class HAMFile : IElementManager
    {
        /// <summary>
        /// Version of the archive, needed for writing back. Version 2 has sound information, version 3 is latest supported, used by the release game.
        /// </summary>
        public int version = 0;

        //Hold a reference to a primary PIG file, as various HAM file operations need it on hand. 
        private int NumRobotJoints = 0; //needed to track the amount of robot joints constructed.

        /// <summary>
        /// Reference to a pig file, for looking up image names.
        /// </summary>
        public PIGFile piggyFile;
        /// <summary>
        /// Uh, i guess in case we want to verify that the ham file was actually loaded.
        /// </summary>
        public bool hasRead = false;
        /// <summary>
        /// Flag for the UI to check if changes have been saved. I think.
        /// </summary>
        public bool hasSaved = true;
        /// <summary>
        /// Original filename for the archive.
        /// </summary>
        public string Filename;
        /// <summary>
        /// Most recently written to filename. 
        /// </summary>
        public string lastFilename;

        //Data
        public List<ushort> Textures = new List<ushort>();
        public List<TMAPInfo> TMapInfo = new List<TMAPInfo>();
        public List<byte> Sounds = new List<byte>();
        public List<byte> AltSounds = new List<byte>();
        public List<VClip> VClips = new List<VClip>();
        public List<EClip> EClips = new List<EClip>();
        public List<WClip> WClips = new List<WClip>();
        public List<Robot> Robots = new List<Robot>();
        public List<JointPos> Joints = new List<JointPos>();
        public List<Weapon> Weapons = new List<Weapon>();
        public List<Polymodel> PolygonModels = new List<Polymodel>();
        public List<ushort> Gauges = new List<ushort>();
        public List<ushort> GaugesHires = new List<ushort>();
        public int NumObjBitmaps = 0; //This is important to track the unique number of obj bitmaps, to know where to inject new ones. 
        public int NumObjBitmapPointers = 0; //Also important to tell how many obj bitmap pointer slots the user have left. 
        public List<ushort> ObjBitmaps = new List<ushort>();
        public List<ushort> ObjBitmapPointers = new List<ushort>();
        public Ship PlayerShip;
        public List<ushort> Cockpits = new List<ushort>();
        public List<Reactor> Reactors = new List<Reactor>();
        public List<Powerup> Powerups = new List<Powerup>();
        public int FirstMultiBitmapNum;
        public ushort[] BitmapXLATData = new ushort[2620];
        //Demo specific data
        public int ExitModelnum, DestroyedExitModelnum;
        public byte[] sounddata;

        //Remapping information
        //Multiplayer color variants, injected into the object bitmap table
        public ushort[] multiplayerBitmaps = new ushort[14];
        //Map EClip names to their IDs, since this is important for model textures
        public Dictionary<string, EClip> EClipNameMapping = new Dictionary<string, EClip>();

        //Name information. Stored separately in a .names file
        public List<string> VClipNames = new List<string>();
        public List<string> EClipNames = new List<string>();
        public List<string> RobotNames = new List<string>();
        public List<string> WeaponNames = new List<string>();
        public List<string> ModelNames = new List<string>();
        public List<string> SoundNames = new List<string>();
        public List<string> ReactorNames = new List<string>();
        public List<string> PowerupNames = new List<string>();

        public HAMFile(PIGFile pigFile)
        {
            piggyFile = pigFile;
        }

        public int LoadDataFile(string filename)
        {
            lastFilename = filename;
            //If a namefile isn't present, automatically generate namelists for our convenience. 
            bool generateNameLists = true;
            Filename = filename;
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
            if (sig != 0x214D4148)
            {
                br.Close();
                br.Dispose();
                return -1;
            }
            version = br.ReadInt32();
            if (version < 2 || version > 3)
            {
                br.Close();
                br.Dispose();
                return -2;
            }
            int sndptr = 0;
            if (version == 2)
            {
                sndptr = br.ReadInt32();
            }

            int NumBitmaps = br.ReadInt32();
            for (int x = 0; x < NumBitmaps; x++)
            {
                Textures.Add(br.ReadUInt16());
            }
            for (int x = 0; x < NumBitmaps; x++)
            {
                TMapInfo.Add(bm.ReadTMAPInfo(br));
                TMapInfo[x].ID = x;
            }
            
            int NumSounds = br.ReadInt32();
            for (int x = 0; x < NumSounds; x++)
            {
                Sounds.Add(br.ReadByte());
            }
            for (int x = 0; x < NumSounds; x++)
            {
                AltSounds.Add(br.ReadByte());
            }
            
            int NumVClips = br.ReadInt32();
            for (int x = 0; x < NumVClips; x++)
            {
                VClips.Add(bm.ReadVClip(br));
                VClips[x].ID = x;
            }
            
            int NumEClips = br.ReadInt32();
            for (int x = 0; x < NumEClips; x++)
            {
                EClips.Add(bm.ReadEClip(br));
                EClips[x].ID = x;
            }
            
            int NumWallAnims = br.ReadInt32();
            for (int x = 0; x < NumWallAnims; x++)
            {
                WClips.Add(bm.ReadWClip(br));
            }
            
            int NumRobots = br.ReadInt32();
            for (int x = 0; x < NumRobots; x++)
            {
                Robots.Add(bm.ReadRobot(br));
                Robots[x].ID = x;
            }
            
            int NumLoadJoints = br.ReadInt32();
            for (int x = 0; x < NumLoadJoints; x++)
            {
                JointPos joint = new JointPos();
                joint.jointnum = br.ReadInt16();
                joint.angles.p = br.ReadInt16();
                joint.angles.b = br.ReadInt16();
                joint.angles.h = br.ReadInt16();
                Joints.Add(joint);
            }

            int NumWeaponTypes = br.ReadInt32();
            for (int x = 0; x < NumWeaponTypes; x++)
            {
                if (version >= 3)
                {
                    Weapons.Add(bm.ReadWeapon(br));
                }
                else
                {
                    Weapons.Add(bm.ReadWeaponInfoOld(br));
                }
                Weapons[x].ID = x;
            }

            int NumPowerups = br.ReadInt32();
            for (int x = 0; x < NumPowerups; x++)
            {
                Powerup powerup = new Powerup();
                powerup.vclip_num = br.ReadInt32();
                powerup.hit_sound = br.ReadInt32();
                powerup.size = br.ReadInt32();
                powerup.light = br.ReadInt32();
                powerup.ID = x;
                Powerups.Add(powerup);
            }
            
            int NumPolygonModels = br.ReadInt32();
            for (int x = 0; x < NumPolygonModels; x++)
            {
                PolygonModels.Add(bm.ReadPolymodelInfo(br));
                PolygonModels[x].ID = x;
            }

            for (int x = 0; x < NumPolygonModels; x++)
            {
                PolymodelData modeldata = new PolymodelData(PolygonModels[x].model_data_size);
                for (int y = 0; y < PolygonModels[x].model_data_size; y++)
                {
                    modeldata.InterpreterData[y] = br.ReadByte();
                }
                modeldata.BuildPolymodelData(x, PolygonModels[x], "");
                PolygonModels[x].data = modeldata;
                //PolymodelData.Add(modeldata);
            }
            for (int x = 0; x < NumPolygonModels; x++)
            {
                PolygonModels[x].DyingModelnum = br.ReadInt32();
            }
            for (int x = 0; x < NumPolygonModels; x++)
            {
                PolygonModels[x].DeadModelnum = br.ReadInt32();
            }
            int gagueCount = br.ReadInt32();
            for (int x = 0; x < gagueCount; x++)
            {
                Gauges.Add(br.ReadUInt16());
            }
            for (int x = 0; x < gagueCount; x++)
            {
                GaugesHires.Add(br.ReadUInt16());
            }
            
            int bitmapCount = br.ReadInt32();
            for (int x = 0; x < bitmapCount; x++)
            {
                ObjBitmaps.Add(br.ReadUInt16());
            }
            ushort value;
            for (int x = 0; x < bitmapCount; x++)
            {
                value = br.ReadUInt16();
                if ((value+1) > NumObjBitmaps)
                    NumObjBitmaps = (value+1);
                ObjBitmapPointers.Add(value);
            }
            
            PlayerShip = new Ship();
            PlayerShip.model_num = br.ReadInt32();
            PlayerShip.expl_vclip_num = br.ReadInt32();
            PlayerShip.mass = br.ReadInt32();
            PlayerShip.drag = br.ReadInt32();
            PlayerShip.max_thrust = br.ReadInt32();
            PlayerShip.reverse_thrust = br.ReadInt32();
            PlayerShip.brakes = br.ReadInt32();
            PlayerShip.wiggle = br.ReadInt32();
            PlayerShip.max_rotthrust = br.ReadInt32();
            for (int x = 0; x < 8; x++)
            {
                PlayerShip.gun_points[x] = new FixVector(br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
            }
            
            int NumCockpits = br.ReadInt32();
            for (int x = 0; x < NumCockpits; x++)
            {
                Cockpits.Add(br.ReadUInt16());
            }
            //Build a table of all multiplayer bitmaps, to inject into the object bitmap table
            FirstMultiBitmapNum = br.ReadInt32();
            for (int i = 0; i < 14; i++)
            {
                multiplayerBitmaps[i] = (ushort)(ObjBitmaps[ObjBitmapPointers[FirstMultiBitmapNum + i]]);
            }

            int NumReactors = br.ReadInt32();
            for (int x = 0; x < NumReactors; x++)
            {
                if (generateNameLists)
                    ReactorNames.Add(ElementLists.GetReactorName(x));
                Reactor reactor = new Reactor();
                reactor.model_id = br.ReadInt32();
                reactor.n_guns = br.ReadInt32();
                for (int y = 0; y < 8; y++)
                {
                    reactor.gun_points[y] = new FixVector(br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
                }
                for (int y = 0; y < 8; y++)
                {
                    reactor.gun_dirs[y] = new FixVector(br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
                }
                Reactors.Add(reactor);
            }
            PlayerShip.markerModel = br.ReadInt32();
            //2620
            if (version < 3)
            {
                ExitModelnum = br.ReadInt32();
                DestroyedExitModelnum = br.ReadInt32();
            }
            for (int x = 0; x < 2600; x++)
            {
                BitmapXLATData[x] = br.ReadUInt16();
            }

            if (version < 3)
            {
                br.BaseStream.Seek(sndptr, SeekOrigin.Begin);
                int dataToRead = (int)(br.BaseStream.Length - br.BaseStream.Position);
                sounddata = br.ReadBytes(dataToRead);
            }

            string nameFilename = Path.ChangeExtension(filename, ".names");
            if (ReadNamefile(nameFilename) != 0)
            {
                for (int i = 0; i < VClips.Count; i++)
                    VClipNames.Add(ElementLists.GetVClipName(i));
                for (int i = 0; i < EClips.Count; i++)
                    EClipNames.Add(ElementLists.GetEClipName(i));
                for (int i = 0; i < Robots.Count; i++)
                    RobotNames.Add(ElementLists.GetRobotName(i));
                for (int i = 0; i < Weapons.Count; i++)
                    WeaponNames.Add(ElementLists.GetWeaponName(i));
                for (int i = 0; i < Sounds.Count; i++)
                    SoundNames.Add(ElementLists.GetSoundName(i));
                if (version >= 3)
                {
                    for (int i = 0; i < PolygonModels.Count; i++)
                        ModelNames.Add(ElementLists.GetModelName(i));
                }
                else
                {
                    for (int i = 0; i < PolygonModels.Count; i++)
                        ModelNames.Add(ElementLists.GetDemoModelName(i));
                }
                for (int i = 0; i < Powerups.Count; i++)
                    PowerupNames.Add(ElementLists.GetPowerupName(i));

            }

            foreach (Robot robot in Robots)
            {
                BuildModelAnimation(robot);
            }
            foreach (Reactor reactor in Reactors)
            {
                BuildModelGunsFromReactor(reactor);
            }
            BuildModelGunsFromShip(PlayerShip);
            BuildModelTextureTables();
            UpdateEClipMapping();
            BuildReferenceLists();

            br.Close();
            hasRead = true;

            return 0;
        }

        private void BuildModelGunsFromShip(Ship ship)
        {
            Polymodel model = PolygonModels[ship.model_num];
            model.numGuns = 8;
            for (int i = 0; i < 8; i++)
            {
                model.gunPoints[i] = ship.gun_points[i];
                model.gunDirs[i] = new FixVector(65536, 0, 0);
                model.gunSubmodels[i] = 0;
            }
        }

        private void BuildModelGunsFromReactor(Reactor reactor)
        {
            if (reactor.model_id == -1) return;
            Polymodel model = PolygonModels[reactor.model_id];
            model.numGuns = reactor.n_guns;
            for (int i = 0; i < 8; i++)
            {
                model.gunPoints[i] = reactor.gun_points[i];
                model.gunDirs[i] = reactor.gun_dirs[i];
                model.gunSubmodels[i] = 0;
            }
        }

        private void BuildModelAnimation(Robot robot)
        {
            if (robot.model_num == -1) return;
            Polymodel model = PolygonModels[robot.model_num];
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
            for (int m = 0; m < Polymodel.MAX_GUNS+1; m++)
            {
                for (int f = 0; f < Robot.NUM_ANIMATION_STATES; f++)
                {
                    Robot.jointlist robotjointlist = robot.anim_states[m, f];
                    basejoint = robotjointlist.offset;
                    for (int j = 0; j < robotjointlist.n_joints; j++)
                    {
                        JointPos joint = Joints[basejoint];
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
                        JointPos joint = Joints[jointnum];
                        model.animationMatrix[m, f].p = joint.angles.p;
                        model.animationMatrix[m, f].b = joint.angles.b;
                        model.animationMatrix[m, f].h = joint.angles.h;
                    }
                }
            }
        }

        //Ultimately, it is too much of a pain to stick to the object bitmap and object bitmap pointer tables
        //Instead, don't track them at all in the first place. Build a texture list for each model, and only
        //reconstruct the tables at the time of export
        public void BuildModelTextureTables()
        {
            //Write down unanimated texture names
            Dictionary<int, string> TextureNames = new Dictionary<int, string>();
            //Write down EClip IDs for tracking animated texture names
            Dictionary<int, string> EClipNames = new Dictionary<int, string>();
            EClip clip;
            for (int i = 0; i < EClips.Count; i++)
            {
                clip = EClips[i];
                if (clip.changing_object_texture != -1)
                {
                    EClipNames.Add(clip.changing_object_texture, this.EClipNames[i]);
                }
            }
            ushort bitmap; string name;
            for (int i = 0; i < ObjBitmaps.Count; i++)
            {
                bitmap = ObjBitmaps[i];
                if (bitmap == 0) continue;
                ImageData image = piggyFile.images[bitmap];
                name = image.name.ToLower();
                if (!image.isAnimated)
                {
                    TextureNames.Add(i, name);
                }
            }
            foreach (Polymodel model in PolygonModels)
            {
                model.useTexList = true;
                int textureID, pointer;
                for (int i = model.first_texture; i < (model.first_texture + model.n_textures); i++)
                {
                    pointer = ObjBitmapPointers[i];
                    textureID = ObjBitmaps[pointer];
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
            EClip eclip;
            for (int i = 0; i < EClips.Count; i++)
            {
                eclip = EClips[i];
                eclip.InitReferences(this);
                eclip.AssignReferences(this);
            }
            Polymodel model;
            for (int i = 0; i < PolygonModels.Count; i++)
            {
                model = PolygonModels[i];
                model.InitReferences(this);
                model.AssignReferences(this);
            }
            PlayerShip.InitReferences(this);
            PlayerShip.AssignReferences(this);
            Reactor reactor;
            for (int i = 0; i < Reactors.Count; i++)
            {
                reactor = Reactors[i];
                reactor.InitReferences(this);
                reactor.AssignReferences(this);
            }
            Powerup powerup;
            for (int i = 0; i < Powerups.Count; i++)
            {
                powerup = Powerups[i];
                powerup.InitReferences(this);
                powerup.AssignReferences(this);
            }
            TMAPInfo info;
            for (int i = 0; i < TMapInfo.Count; i++)
            {
                info = TMapInfo[i];
                info.InitReferences(this);
                info.AssignReferences(this);
            }
        }

        public void UpdateEClipMapping()
        {
            EClipNameMapping.Clear();
            EClip clip;
            for (int i = 0; i < EClips.Count; i++)
            {
                clip = EClips[i];
                EClipNameMapping.Add(ElementLists.GetEClipName(i).ToLower(), clip);
            }
        }

        private void GenerateObjectBitmapTables()
        {
            ObjBitmaps.Clear();
            ObjBitmapPointers.Clear();
            int lastObjectBitmap = 0;
            int lastObjectBitmapPointer = 0;
            Dictionary<string, int> objectBitmapMapping = new Dictionary<string, int>();

            Polymodel model;
            if (StandardUI.options.GetOption("CompatObjBitmaps", bool.FalseString) == bool.TrueString)
            {
                int lastShipmodel = PlayerShip.model_num;
                if (PolygonModels[PlayerShip.model_num].DyingModelnum != -1)
                    lastShipmodel = PolygonModels[PlayerShip.model_num].DyingModelnum;
                for (int i = 0; i < PolygonModels.Count; i++)
                {
                    model = PolygonModels[i];
                    model.first_texture = (ushort)lastObjectBitmapPointer;
                    model.n_textures = (byte)model.textureList.Count;
                    if (i == lastShipmodel)
                    {
                        //Inject multiplayer bitmaps
                        FirstMultiBitmapNum = lastObjectBitmapPointer;
                        for (int j = 0; j < 14; j++)
                        {
                            ObjBitmaps.Add((ushort)(multiplayerBitmaps[j])); ObjBitmapPointers.Add((ushort)(ObjBitmaps.Count - 1));
                            lastObjectBitmap++; lastObjectBitmapPointer++;
                        }
                        //Don't load textures for the dying ship. Because reasons. 
                        model.first_texture = PolygonModels[PlayerShip.model_num].first_texture;
                        model.n_textures = PolygonModels[PlayerShip.model_num].n_textures;
                    }
                    else
                    {
                        foreach (string textureName in model.textureList)
                        {
                            if (EClipNameMapping.ContainsKey(textureName.ToLower()) && !objectBitmapMapping.ContainsKey(textureName.ToLower()))
                            {
                                objectBitmapMapping.Add(textureName.ToLower(), ObjBitmaps.Count);
                                ObjBitmaps.Add(0); //temp, will be remapped later
                                lastObjectBitmap++;
                            }
                            //In the compatible mode, all textures are redundant except vclips. 
                            if (objectBitmapMapping.ContainsKey(textureName.ToLower()))
                            {
                                ObjBitmapPointers.Add((ushort)objectBitmapMapping[textureName.ToLower()]);
                                lastObjectBitmapPointer++;
                            }
                            else
                            {
                                ObjBitmapPointers.Add((ushort)lastObjectBitmap);
                                lastObjectBitmapPointer++;
                                ObjBitmaps.Add((ushort)(piggyFile.GetBitmapIDFromName(textureName)));
                                lastObjectBitmap++;
                            }
                        }
                        //I hate hacks, but parallax couldn't keep tabs on their bitmaps.tbl file so...
                        //Descent's smart missile children are defined with model textures despite not being models so for compatibility add them in
                        if (i == Weapons[18].model_num || i == Weapons[28].model_num) //player and robot mega missiles
                        {
                            ObjBitmaps.Add((ushort)(piggyFile.GetBitmapIDFromName("glow04"))); ObjBitmapPointers.Add((ushort)(ObjBitmaps.Count - 1));
                            ObjBitmaps.Add((ushort)(piggyFile.GetBitmapIDFromName("rbot046"))); ObjBitmapPointers.Add((ushort)(ObjBitmaps.Count - 1));
                            lastObjectBitmapPointer += 2; lastObjectBitmap += 2;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < PolygonModels.Count; i++)
                {
                    model = PolygonModels[i];
                    model.first_texture = (ushort)lastObjectBitmapPointer;
                    model.n_textures = (byte)model.textureList.Count;
                    foreach (string textureName in model.textureList)
                    {
                        if (!objectBitmapMapping.ContainsKey(textureName.ToLower()))
                        {
                            objectBitmapMapping.Add(textureName.ToLower(), lastObjectBitmap);
                            ObjBitmaps.Add((ushort)(piggyFile.GetBitmapIDFromName(textureName)));
                            lastObjectBitmap++;
                        }
                        ObjBitmapPointers.Add((ushort)objectBitmapMapping[textureName.ToLower()]);
                        lastObjectBitmapPointer++;
                    }
                }
                //Inject multiplayer bitmaps
                FirstMultiBitmapNum = lastObjectBitmapPointer;
                for (int i = 0; i < 14; i++)
                {
                    ObjBitmaps.Add((ushort)(multiplayerBitmaps[i])); ObjBitmapPointers.Add((ushort)(ObjBitmaps.Count - 1));
                }
            }

            //Update EClips
            EClip clip;
            for (int i = 0; i < EClips.Count; i++)
            {
                clip = EClips[i];
                if (objectBitmapMapping.ContainsKey(ElementLists.GetEClipName(i).ToLower()))
                {
                    clip.changing_object_texture = (short)objectBitmapMapping[ElementLists.GetEClipName(i).ToLower()];
                    ObjBitmaps[clip.changing_object_texture] = (ushort)(clip.vc.frames[0]);
                }
            }
        }

        public void SaveDataFile(string filename)
        {
            //Brute force solution
            RenumberElements(HAMType.EClip);
            RenumberElements(HAMType.Weapon);
            RenumberElements(HAMType.Robot);
            RenumberElements(HAMType.Model);
            //Science experiment
            GenerateObjectBitmapTables();
            NumRobotJoints = 0;
            Console.WriteLine("Loaded {0} joints", Joints.Count);
            Joints.Clear();
            foreach (Robot robot in Robots)
            {
                //LoadAnimations(robot, PolygonModels[robot.model_num]);
                LoadAnimations(robot, robot.model);
            }
            Console.WriteLine("Constructed {0} joints", Joints.Count);
            foreach (Reactor reactor in Reactors)
            {
                LoadReactorGuns(reactor);
            }
            LoadShipGuns(PlayerShip);

            HAMDataWriter writer = new HAMDataWriter();
            BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create));
            bw.Write(558711112);
            bw.Write(version);
            int returnPoint = (int)bw.BaseStream.Position;
            if (version < 3)
            {
                bw.Write(0);
            }
            bw.Write(Textures.Count);
            for (int x = 0; x < Textures.Count; x++)
            {
                ushort texture = Textures[x];
                bw.Write(texture);
            }
            for (int x = 0; x < TMapInfo.Count; x++)
            {
                TMAPInfo texture = TMapInfo[x];
                writer.WriteTMAPInfo(texture, bw);
            }

            bw.Write(Sounds.Count);
            for (int x = 0; x < Sounds.Count; x++)
            {
                byte sound = Sounds[x];
                bw.Write(sound);
            }
            for (int x = 0; x < Sounds.Count; x++)
            {
                byte sound = AltSounds[x];
                bw.Write(sound);
            }
            bw.Write(VClips.Count);
            for (int x = 0; x < VClips.Count; x++)
            {
                writer.WriteVClip(VClips[x], bw);
            }
            bw.Write(EClips.Count);
            for (int x = 0; x < EClips.Count; x++)
            {
                writer.WriteEClip(EClips[x], bw);
            }
            bw.Write(WClips.Count);
            for (int x = 0; x < WClips.Count; x++)
            {
                writer.WriteWClip(WClips[x], bw);
            }
            bw.Write(Robots.Count);
            for (int x = 0; x < Robots.Count; x++)
            {
                writer.WriteRobot(Robots[x], bw);
            }
            bw.Write(Joints.Count);
            for (int x = 0; x < Joints.Count; x++)
            {
                JointPos joint = Joints[x];
                bw.Write(joint.jointnum);
                bw.Write(joint.angles.p);
                bw.Write(joint.angles.b);
                bw.Write(joint.angles.h);
            }
            bw.Write(Weapons.Count);
            if (version < 3)
            {
                for (int x = 0; x < Weapons.Count; x++)
                {
                    writer.WriteWeaponV2(Weapons[x], bw);
                }
            }
            else
            {
                for (int x = 0; x < Weapons.Count; x++)
                {
                    writer.WriteWeapon(Weapons[x], bw);
                }
            }
            bw.Write(Powerups.Count);
            for (int x = 0; x < Powerups.Count; x++)
            {
                Powerup powerup = Powerups[x];
                bw.Write(powerup.vclip_num);
                bw.Write(powerup.hit_sound);
                bw.Write(powerup.size);
                bw.Write(powerup.light);
            }
            bw.Write(PolygonModels.Count);
            for (int x = 0; x < PolygonModels.Count; x++)
            {
                writer.WritePolymodel(PolygonModels[x], bw);
            }
            for (int x = 0; x < PolygonModels.Count; x++)
            {
                PolymodelData pmd = PolygonModels[x].data;
                bw.Write(pmd.InterpreterData);
            }
            for (int x = 0; x < PolygonModels.Count; x++)
            {
                int modelnum = PolygonModels[x].DyingModelID;
                bw.Write(modelnum);
            }
            for (int x = 0; x < PolygonModels.Count; x++)
            {
                int modelnum = PolygonModels[x].DeadModelID;
                bw.Write(modelnum);
            }
            bw.Write(Gauges.Count);
            for (int x = 0; x < Gauges.Count; x++)
            {
                ushort gague = Gauges[x];
                bw.Write(gague);
            }
            for (int x = 0; x < Gauges.Count; x++)
            {
                ushort gague = GaugesHires[x];
                bw.Write(gague);
            }
            //Always write exactly 600 ObjBitmaps, the limit in Descent 2, to conform to the original data files.
            //Can be optimized if you need to save a kb of data I guess
            bw.Write(600);
            for (int x = 0; x < 600; x++)
            {
                if (x < ObjBitmaps.Count)
                {
                    bw.Write(ObjBitmaps[x]);
                }
                else
                {
                    bw.Write((ushort)0);
                }
            }
            for (int x = 0; x < 600; x++)
            {
                if (x < ObjBitmapPointers.Count)
                {
                    bw.Write(ObjBitmapPointers[x]);
                }
                else
                {
                    bw.Write((ushort)0);
                }
            }
            writer.WritePlayerShip(PlayerShip, bw);
            bw.Write(Cockpits.Count);
            for (int x = 0; x < Cockpits.Count; x++)
            {
                ushort cockpit = Cockpits[x];
                bw.Write(cockpit);
            }
            bw.Write(FirstMultiBitmapNum);
            bw.Write(Reactors.Count);
            for (int x = 0; x < Reactors.Count; x++)
            {
                Reactor reactor = Reactors[x];
                //bw.Write(reactor.model_id);
                bw.Write(reactor.ModelID);
                bw.Write(reactor.n_guns);
                for (int y = 0; y < 8; y++)
                {
                    bw.Write(reactor.gun_points[y].x);
                    bw.Write(reactor.gun_points[y].y);
                    bw.Write(reactor.gun_points[y].z);
                }
                for (int y = 0; y < 8; y++)
                {
                    bw.Write(reactor.gun_dirs[y].x);
                    bw.Write(reactor.gun_dirs[y].y);
                    bw.Write(reactor.gun_dirs[y].z);
                }
            }
            bw.Write(PlayerShip.markerModel);
            if (version < 3)
            {
                bw.Write(ExitModelnum);
                bw.Write(DestroyedExitModelnum);
            }
            for (int x = 0; x < 2600; x++)
            {
                bw.Write(BitmapXLATData[x]);
            }
            int ptr = (int)bw.BaseStream.Position;
            if (version < 3)
            {
                bw.BaseStream.Seek(returnPoint, SeekOrigin.Begin);
                bw.Write(ptr);
                bw.BaseStream.Seek(ptr, SeekOrigin.Begin);
                bw.Write(sounddata);
            }
            bw.Close();
            string nameFilename = Path.ChangeExtension(filename, ".names");
            SaveNamefile(nameFilename);
        }

        private void LoadReactorGuns(Reactor reactor)
        {
            Polymodel model = reactor.model;
            reactor.n_guns = (byte)model.numGuns;
            for (int i = 0; i < reactor.n_guns; i++)
            {
                reactor.gun_points[i] = model.gunPoints[i];
                reactor.gun_dirs[i] = model.gunDirs[i];
            }
        }

        private void LoadShipGuns(Ship ship)
        {
            Polymodel models = ship.model;
            for (int i = 0; i < 8; i++)
            {
                ship.gun_points[i] = models.gunPoints[i];
            }
        }

        //I actually hate this game's animation system sometimes
        private void LoadAnimations(Robot robot, Polymodel model)
        {
            robot.n_guns = (sbyte)model.numGuns;
            for (int i = 0; i < 8; i++)
            {
                robot.gun_points[i] = model.gunPoints[i];
                robot.gun_submodels[i] = (byte)model.gunSubmodels[i];
            }
            for (int m = 0; m < 9; m++)
            {
                for (int f = 0; f < 5; f++)
                {
                    robot.anim_states[m, f].n_joints = 0;
                    robot.anim_states[m, f].offset = 0;
                }
            }
            if (!model.isAnimated) return;
            int[] gunNums = new int[10];

            for (int i = 1; i < model.n_models; i++)
            {
                gunNums[i] = robot.n_guns;
            }
            gunNums[0] = -1;

            for (int g = 0; g < robot.n_guns; g++)
            {
                int m = robot.gun_submodels[g];

                while (m != 0)
                {
                    gunNums[m] = g;
                    m = model.submodels[m].Parent;
                }
            }

            for (int g = 0; g < robot.n_guns + 1; g++)
            {
                for (int state = 0; state < 5; state++)
                {
                    robot.anim_states[g, state].n_joints = 0;
                    robot.anim_states[g, state].offset = (short)NumRobotJoints;

                    for (int m = 0; m < model.n_models; m++)
                    {
                        if (gunNums[m] == g)
                        {
                            JointPos joint = new JointPos();
                            joint.jointnum = (short)m;
                            joint.angles = model.animationMatrix[m, state];
                            Joints.Add(joint);
                            robot.anim_states[g, state].n_joints++;
                            NumRobotJoints++;
                        }
                    }
                }
            }
        }

        public void ReplaceModel(int index, Polymodel newModel)
        {
            Polymodel model = PolygonModels[index];
            PolygonModels[index] = newModel;
            model.ClearReferences();
            newModel.ID = index;
            newModel.InitReferences(this);
            newModel.AssignReferences(this);
            RenumberElements(HAMType.Model);
            //PolymodelData[index] = newModel.data;
        }

        public int CopyElement(HAMType type, int source, int destination)
        {
            switch (type)
            {
                /*case HAMType.Robot:
                    if (source < Robots.Count && destination < Robots.Count)
                        Robots[destination].CopyDataFrom(Robots[source], this);
                    else
                        return -1;
                    break;
                case HAMType.Weapon:
                    if (source < Weapons.Count && destination < Weapons.Count)
                        Weapons[destination].CopyDataFrom(Weapons[source], this);
                    else
                        return -1;
                    break;
                default:
                    return 1;*/
            }
            return 0;
        }

        public int AddElement(HAMType type)
        {
            switch (type)
            {
                case HAMType.EClip:
                    EClip eclip = new EClip();
                    eclip.ID = EClips.Count;
                    eclip.AssignReferences(this);
                    EClips.Add(eclip);
                    EClipNames.Add(string.Format("NewEClip{0}", eclip.ID));
                    return eclip.ID;
                case HAMType.Robot:
                    Robot robot = new Robot();
                    robot.ID = Robots.Count;
                    robot.AssignReferences(this);
                    Robots.Add(robot);
                    RobotNames.Add(string.Format("New Robot #{0}", robot.ID));
                    return robot.ID;
                case HAMType.Weapon:
                    Weapon weapon = new Weapon();
                    weapon.ID = Weapons.Count;
                    weapon.AssignReferences(this);
                    Weapons.Add(weapon);
                    WeaponNames.Add(string.Format("New Weapon #{0}", weapon.ID));
                    return weapon.ID;
                case HAMType.Model:
                    Polymodel model = new Polymodel();
                    PolymodelData data = new PolymodelData(0);
                    model.data = data;
                    model.ExpandSubmodels();
                    model.ID = PolygonModels.Count;
                    PolygonModels.Add(model);
                    ModelNames.Add(string.Format("New Polymodel #{0}", model.ID));
                    return model.ID;
            }
            return -1;
        }

        //If you call this function, you're a masochist or don't know what you're doing tbh.
        //Can only delete from the top of the list, to avoid having to update all references of higher numbered elements. Ugh
        public int DeleteElement(HAMType type, int slot)
        {
            switch (type)
            {
                case HAMType.EClip:
                    {
                        EClip eclip = EClips[slot];
                        int refCount = eclip.References.Count;
                        if (refCount > 0)
                            return -1;
                        EClips.RemoveAt(slot);
                        EClipNameMapping.Remove(EClipNames[slot]);
                        EClipNames.RemoveAt(slot);
                        //eclip.ClearReferences(this);
                        RenumberElements(type);
                        return EClips.Count;
                    }
                case HAMType.Robot:
                    {
                        Robot robot = Robots[slot];
                        int refCount = robot.References.Count;
                        if (refCount > 0)
                            return -1;
                        Robots.RemoveAt(slot);
                        RobotNames.RemoveAt(slot);
                        //robot.ClearReferences(this);
                        RenumberElements(type);
                        return Robots.Count;
                    }
                case HAMType.Weapon:
                    {
                        Weapon weapon = Weapons[slot];
                        int refCount = weapon.References.Count;
                        if (refCount > 0)
                            return -1;
                        Weapons.RemoveAt(slot);
                        WeaponNames.RemoveAt(slot);
                        //weapon.ClearReferences(this);
                        RenumberElements(type);
                        return Weapons.Count;
                    }
                case HAMType.Model:
                    {
                        Polymodel model = PolygonModels[slot];
                        int refCount = model.References.Count;
                        if (refCount > 0)
                            return -1;
                        PolygonModels.RemoveAt(slot);
                        ModelNames.RemoveAt(slot);
                        //model.ClearReferences(this);
                        RenumberElements(type);
                        return PolygonModels.Count;
                    }
            }
            return -1;
        }

        public void RenumberElements(HAMType type)
        {
            switch (type)
            {
                case HAMType.EClip:
                    {
                        EClip eclip;

                        for (int i = 0; i < EClips.Count; i++)
                        {
                            eclip = EClips[i];
                            eclip.ID = i;
                        }
                        break;
                    }
                case HAMType.Robot:
                    {
                        Robot robot;

                        for (int i = 0; i < Robots.Count; i++)
                        {
                            robot = Robots[i];
                            robot.ID = i;
                        }
                        break;
                    }
                case HAMType.Weapon:
                    {
                        Weapon weapon;

                        for (int i = 0; i < Weapons.Count; i++)
                        {
                            weapon = Weapons[i];
                            weapon.ID = i;
                        }
                        break;
                    }
                case HAMType.Model:
                    {
                        Polymodel model;

                        for (int i = 0; i < PolygonModels.Count; i++)
                        {
                            model = PolygonModels[i];
                            model.ID = i;
                        }
                        break;
                    }
            }
        }

        public void UpdateName(HAMType type, int element, string newName)
        {
            switch (type)
            {
                case HAMType.VClip:
                    VClipNames[element] = newName;
                    return;
                case HAMType.EClip:
                    EClipNames[element] = newName;
                    return;
                case HAMType.Robot:
                    RobotNames[element] = newName;
                    return;
                case HAMType.Weapon:
                    WeaponNames[element] = newName;
                    return;
                case HAMType.Sound:
                    SoundNames[element] = newName;
                    return;
                case HAMType.Model:
                    ModelNames[element] = newName;
                    return;
                case HAMType.Powerup:
                    PowerupNames[element] = newName;
                    return;
            }
        }

        public int ReadNamefile(string filename)
        {
            BinaryReader br;
            try
            {
                br = new BinaryReader(File.Open(filename, FileMode.Open), Encoding.UTF8);
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
            catch (UnauthorizedAccessException)
            {
                return -2;
            }
            catch (Exception)
            {
                return -3;
            }
            int sig = br.ReadInt32();
            int ver = br.ReadInt32();
            if (sig != 0x4E4D4148 || ver != 1)
            {
                br.Close();
                br.Dispose();
                return -1;
            }
            int VClipsCount = br.ReadInt32();
            int EClipsCount = br.ReadInt32();
            int RobotsCount = br.ReadInt32();
            int WeaponsCount = br.ReadInt32();
            int SoundsCount = br.ReadInt32();
            int PolymodelCount = br.ReadInt32();
            int PowerupsCount = br.ReadInt32();
            if (VClipsCount != VClips.Count || EClipsCount != EClips.Count || RobotsCount != Robots.Count || WeaponsCount != Weapons.Count ||
                SoundsCount != Sounds.Count || PolymodelCount != PolygonModels.Count || PowerupsCount != Powerups.Count)
            {
                br.Close();
                br.Dispose();
                return -1;
            }
            for (int i = 0; i < VClips.Count; i++)
                VClipNames.Add(br.ReadString());
            for (int i = 0; i < EClips.Count; i++)
                EClipNames.Add(br.ReadString());
            for (int i = 0; i < Robots.Count; i++)
                RobotNames.Add(br.ReadString());
            for (int i = 0; i < Weapons.Count; i++)
                WeaponNames.Add(br.ReadString());
            for (int i = 0; i < Sounds.Count; i++)
                SoundNames.Add(br.ReadString());
            for (int i = 0; i < PolygonModels.Count; i++)
                ModelNames.Add(br.ReadString());
            for (int i = 0; i < Powerups.Count; i++)
                PowerupNames.Add(br.ReadString());

            br.Close();
            br.Dispose();
            return 0;
        }

        public int SaveNamefile(string filename)
        {
            BinaryWriter bw = null;
            try
            {
                bw = new BinaryWriter(File.Open(filename, FileMode.Create), Encoding.UTF8);
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
            catch (UnauthorizedAccessException)
            {
                return -2;
            }
            catch (Exception)
            {
                return -3;
            }
            //48 41 4D 4E
            bw.Write(0x4E4D4148); //HAMN
            bw.Write(1); //version. in case i fuck something up
            //Write out the counts for safety. 
            bw.Write(VClips.Count);
            bw.Write(EClips.Count);
            bw.Write(Robots.Count);
            bw.Write(Weapons.Count);
            bw.Write(Sounds.Count);
            bw.Write(PolygonModels.Count);
            bw.Write(Powerups.Count);
            foreach (string name in VClipNames)
                bw.Write(name);
            foreach (string name in EClipNames)
                bw.Write(name);
            foreach (string name in RobotNames)
                bw.Write(name);
            foreach (string name in WeaponNames)
                bw.Write(name);
            foreach (string name in SoundNames)
                bw.Write(name);
            foreach (string name in ModelNames)
                bw.Write(name);
            foreach (string name in PowerupNames)
                bw.Write(name);

            bw.Close();
            bw.Dispose();
            return 0;
        }

        public TMAPInfo GetTMAPInfo(int id)
        {
            if (id == -1) return null;
            return TMapInfo[id];
        }

        public VClip GetVClip(int id)
        {
            if (id == -1 || id == 255) return null;
            return VClips[id];
        }

        public EClip GetEClip(int id)
        {
            if (id == -1) return null;
            return EClips[id];
        }

        public WClip GetWClip(int id)
        {
            if (id == -1) return null;
            return WClips[id];
        }

        public Robot GetRobot(int id)
        {
            if (id == -1) return null;
            return Robots[id];
        }

        public Weapon GetWeapon(int id)
        {
            if (id == -1) return null;
            return Weapons[id];
        }

        public Polymodel GetModel(int id)
        {
            if (id == -1) return null;
            return PolygonModels[id];
        }

        public Powerup GetPowerup(int id)
        {
            if (id == -1) return null;
            return Powerups[id];
        }

        public Reactor GetReactor(int id)
        {
            if (id == -1) return null;
            return Reactors[id];
        }
    }
}
