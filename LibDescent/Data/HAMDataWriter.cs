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
using System.Text;
using System.IO;

namespace LibDescent.Data
{
    class HAMDataWriter
    {
        public void WriteTMAPInfo(TMAPInfo tmapinfo, BinaryWriter bw)
        {
            bw.Write(tmapinfo.flags);
            bw.Write(new byte[3]);
            bw.Write(tmapinfo.lighting);
            bw.Write(tmapinfo.damage);
            //bw.Write(tmapinfo.eclip_num);
            bw.Write((short)tmapinfo.EClipID);
            bw.Write(tmapinfo.destroyed);
            bw.Write(tmapinfo.slide_u);
            bw.Write(tmapinfo.slide_v);
        }

        public void WriteVClip(VClip clip, BinaryWriter bw)
        {
            bw.Write(clip.play_time);
            bw.Write(clip.num_frames);
            bw.Write(clip.frame_time);
            bw.Write(clip.flags);
            bw.Write(clip.sound_num);
            for (int x = 0; x < 30; x++)
            {
                bw.Write(clip.frames[x]);
            }
            bw.Write(clip.light_value);
        }

        public void WriteEClip(EClip clip, BinaryWriter bw)
        {
            WriteVClip(clip.vc, bw);
            bw.Write(clip.time_left);
            bw.Write(clip.frame_count);
            //bw.Write(clip.changing_wall_texture);
            bw.Write((short)clip.GetCurrentTMap());
            bw.Write(clip.changing_object_texture);
            bw.Write(clip.flags);
            bw.Write(clip.CritClipID);
            bw.Write(clip.dest_bm_num);
            bw.Write(clip.DestVClipID);
            bw.Write(clip.DestEClipID);
            bw.Write(clip.dest_size);
            bw.Write(clip.sound_num);
            bw.Write(clip.segnum);
            bw.Write(clip.sidenum);
        }

        public void WriteWClip(WClip clip, BinaryWriter bw)
        {
            bw.Write(clip.play_time);
            bw.Write(clip.num_frames);
            for (int x = 0; x < 50; x++)
            {
                bw.Write(clip.frames[x]);
            }
            bw.Write(clip.open_sound);
            bw.Write(clip.close_sound);
            bw.Write(clip.flags);
            for (int x = 0; x < 13; x++)
            {
                bw.Write((byte)clip.filename[x]);
            }
            bw.Write(clip.pad);
        }

        public void WriteRobot(Robot robot, BinaryWriter bw)
        {
            bw.Write(robot.ModelID);
            for (int x = 0; x < 8; x++)
            {
                bw.Write(robot.gun_points[x].x);
                bw.Write(robot.gun_points[x].y);
                bw.Write(robot.gun_points[x].z);
            }
            for (int x = 0; x < 8; x++)
            {
                bw.Write(robot.gun_submodels[x]);
            }
            bw.Write((short)robot.Exp1VClipID);
            bw.Write(robot.exp1_sound_num);
            
            bw.Write((short)robot.Exp2VClipID);
            bw.Write(robot.exp2_sound_num);
            
            bw.Write((sbyte)robot.Weapon1ID);
            bw.Write((sbyte)robot.Weapon2ID);
            bw.Write(robot.n_guns);
            //bw.Write(robot.contains_id);
            if (robot.contains_type == 2)
                bw.Write((sbyte)robot.DropRobotID);
            else
                bw.Write((sbyte)robot.DropPowerupID);

            bw.Write(robot.contains_count);
            bw.Write(robot.contains_prob);
            bw.Write(robot.contains_type);
            bw.Write(robot.kamikaze);
            
            bw.Write(robot.score_value);
            bw.Write(robot.badass);
            bw.Write(robot.energy_drain);
            
            bw.Write(robot.lighting);
            bw.Write(robot.strength);
            
            bw.Write(robot.mass);
            bw.Write(robot.drag);
            
            for (int x = 0; x < 5; x++)
            {
                bw.Write(robot.field_of_view[x]);
            }
            for (int x = 0; x < 5; x++)
            {
                bw.Write(robot.firing_wait[x]);
            }
            for (int x = 0; x < 5; x++)
            {
                bw.Write(robot.firing_wait2[x]);
            }
            for (int x = 0; x < 5; x++)
            {
                bw.Write(robot.turn_time[x]);
            }
            for (int x = 0; x < 5; x++)
            {
                bw.Write(robot.max_speed[x]);
            }
            for (int x = 0; x < 5; x++)
            {
                bw.Write(robot.circle_distance[x]);
            }
            for (int x = 0; x < 5; x++)
            {
                bw.Write(robot.rapidfire_count[x]);
            }
            for (int x = 0; x < 5; x++)
            {
                bw.Write(robot.evade_speed[x]);
            }
            bw.Write(robot.cloak_type);
            bw.Write(robot.attack_type);
           
            bw.Write(robot.see_sound);
            bw.Write(robot.attack_sound);
            bw.Write(robot.claw_sound);
            bw.Write(robot.taunt_sound);

            bw.Write(robot.boss_flag);
            bw.Write(robot.companion);
            bw.Write(robot.smart_blobs);
            bw.Write(robot.energy_blobs);

            bw.Write(robot.thief);
            bw.Write(robot.pursuit);
            bw.Write(robot.lightcast);
            bw.Write(robot.death_roll);

            bw.Write(robot.flags);
            bw.Write(new byte[3]);

            bw.Write(robot.deathroll_sound);
            bw.Write(robot.glow);
            bw.Write(robot.behavior);
            bw.Write(robot.aim);

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    bw.Write(robot.anim_states[y, x].n_joints);
                    bw.Write(robot.anim_states[y, x].offset);
                }
            }
            bw.Write(robot.always_0xabcd);
        }

        public void WriteWeapon(Weapon weapon, BinaryWriter bw)
        {
            bw.Write(weapon.render_type);
            bw.Write(weapon.persistent);
            bw.Write((short)weapon.ModelID);
            bw.Write((short)weapon.ModelInnerID);
            
            bw.Write((sbyte)weapon.FlashVClipID);
            bw.Write((sbyte)weapon.RobotHitVClipID);
            bw.Write(weapon.flash_sound);

            bw.Write((sbyte)weapon.WallHitVClipID);
            bw.Write(weapon.fire_count);
            bw.Write(weapon.robot_hit_sound);
            
            bw.Write(weapon.ammo_usage);
            bw.Write((sbyte)weapon.WeaponVClipID);
            bw.Write(weapon.wall_hit_sound);

            bw.Write(weapon.destroyable);
            bw.Write(weapon.matter);
            bw.Write(weapon.bounce);
            bw.Write(weapon.homing_flag);

            bw.Write(weapon.speedvar);

            bw.Write(weapon.flags);

            bw.Write(weapon.flash);
            bw.Write(weapon.afterburner_size);

            bw.Write((sbyte)weapon.ChildrenID);
            
            bw.Write(weapon.energy_usage);
            bw.Write(weapon.fire_wait);

            bw.Write(weapon.multi_damage_scale);
            
            bw.Write(weapon.bitmap);
            
            bw.Write(weapon.blob_size);
            bw.Write(weapon.flash_size);
            bw.Write(weapon.impact_size);
            for (int x = 0; x < 5; x++)
            {
                bw.Write(weapon.strength[x]);
            }
            for (int x = 0; x < 5; x++)
            {
                bw.Write(weapon.speed[x]);
            }
            bw.Write(weapon.mass);
            bw.Write(weapon.drag);
            bw.Write(weapon.thrust);
            bw.Write(weapon.po_len_to_width_ratio);
            bw.Write(weapon.light);
            bw.Write(weapon.lifetime);
            bw.Write(weapon.damage_radius);
            
            bw.Write(weapon.picture);
            bw.Write(weapon.hires_picture);
        }

        public void WriteWeaponV2(Weapon weapon, BinaryWriter bw)
        {
            bw.Write(weapon.render_type);
            bw.Write(weapon.persistent);
            bw.Write((short)weapon.ModelID);
            bw.Write((short)weapon.ModelInnerID);

            bw.Write((sbyte)weapon.FlashVClipID);
            bw.Write((sbyte)weapon.RobotHitVClipID);
            bw.Write(weapon.flash_sound);

            bw.Write((sbyte)weapon.WallHitVClipID);
            bw.Write(weapon.fire_count);
            bw.Write(weapon.robot_hit_sound);

            bw.Write(weapon.ammo_usage);
            bw.Write((sbyte)weapon.WeaponVClipID);
            bw.Write(weapon.wall_hit_sound);

            bw.Write(weapon.destroyable);
            bw.Write(weapon.matter);
            bw.Write(weapon.bounce);
            bw.Write(weapon.homing_flag);

            bw.Write(weapon.speedvar);

            bw.Write(weapon.flags);

            bw.Write(weapon.flash);
            bw.Write(weapon.afterburner_size);

            bw.Write(weapon.energy_usage);
            bw.Write(weapon.fire_wait);

            bw.Write(weapon.bitmap);

            bw.Write(weapon.blob_size);
            bw.Write(weapon.flash_size);
            bw.Write(weapon.impact_size);
            for (int x = 0; x < 5; x++)
            {
                bw.Write(weapon.strength[x]);
            }
            for (int x = 0; x < 5; x++)
            {
                bw.Write(weapon.speed[x]);
            }
            bw.Write(weapon.mass);
            bw.Write(weapon.drag);
            bw.Write(weapon.thrust);
            bw.Write(weapon.po_len_to_width_ratio);
            bw.Write(weapon.light);
            bw.Write(weapon.lifetime);
            bw.Write(weapon.damage_radius);

            bw.Write(weapon.picture);
        }

        public void WritePolymodel(Polymodel model, BinaryWriter bw)
        {
            bw.Write(model.n_models);
            bw.Write(model.model_data_size);
            bw.Write(model.model_data);
            for (int s = 0; s < 10; s++)
            {
                bw.Write(model.submodels[s].Pointer);
            }
            for (int s = 0; s < 10; s++)
            {
                bw.Write(model.submodels[s].Offset.x);
                bw.Write(model.submodels[s].Offset.y);
                bw.Write(model.submodels[s].Offset.z);
            }
            for (int s = 0; s < 10; s++)
            {
                bw.Write(model.submodels[s].Normal.x);
                bw.Write(model.submodels[s].Normal.y);
                bw.Write(model.submodels[s].Normal.z);
            }
            for (int s = 0; s < 10; s++)
            {
                bw.Write(model.submodels[s].Point.x);
                bw.Write(model.submodels[s].Point.y);
                bw.Write(model.submodels[s].Point.z);
            }
            for (int s = 0; s < 10; s++)
            {
                bw.Write(model.submodels[s].Radius);
            }
            for (int s = 0; s < 10; s++)
            {
                bw.Write(model.submodels[s].Parent);
            }
            for (int s = 0; s < 10; s++)
            {
                bw.Write(model.submodels[s].Mins.x);
                bw.Write(model.submodels[s].Mins.y);
                bw.Write(model.submodels[s].Mins.z);
            }
            for (int s = 0; s < 10; s++)
            {
                bw.Write(model.submodels[s].Maxs.x);
                bw.Write(model.submodels[s].Maxs.y);
                bw.Write(model.submodels[s].Maxs.z);
            }
            bw.Write(model.mins.x);
            bw.Write(model.mins.y);
            bw.Write(model.mins.z);
            bw.Write(model.maxs.x);
            bw.Write(model.maxs.y);
            bw.Write(model.maxs.z);
            bw.Write(model.rad);
            bw.Write(model.n_textures);
            bw.Write(model.first_texture);
            bw.Write((byte)(model.SimpleModelID+1));
        }

        public void WritePlayerShip(Ship ship, BinaryWriter bw)
        {
            bw.Write(ship.model.ID);
            bw.Write(ship.explosion.ID);
            bw.Write(ship.mass);
            bw.Write(ship.drag);
            bw.Write(ship.max_thrust);
            bw.Write(ship.reverse_thrust);
            bw.Write(ship.brakes);
            bw.Write(ship.wiggle);
            bw.Write(ship.max_rotthrust);
            for (int x = 0; x < 8; x++)
            {
                bw.Write(ship.gun_points[x].x);
                bw.Write(ship.gun_points[x].y);
                bw.Write(ship.gun_points[x].z);
            }
        }

        public static string[] robotsbm = {"mech", "green", "spider", "josh", "violet",
                    "clkvulc", "clkmech", "brain", "onearm", "plasguy",
                    "toaster", "bird", "mislbird", "splitpod", "smspider",
                    "miniboss", "suprmech", "boss1", "cloakgrn", "vulcnguy", "rifleman",
                    "fourclaw", "quadlasr", "boss2", "babyplas1", "sloth", "icespidr",
                    "gaussguy", "fire", "spread", "sidearm", "xboss1", "newboss1",
                    "escort", "guard", "eviltwin", "sniper", "snipe", "frog", "minotaur",
                    "fourclaw2", "hornet", "bandit", "arnold", "sucker", "xboss3", "xboss2",
                    "boarshed", "spiderg", "omega", "smside", "toady", "xboss5",
                    "popcorn", "clkclaw", "clksloth", "guppy", "sloth2",
                    "omega2", "babyplas2", "spiderg2", "spawn", "xboss4", "spawn2",
                    "xboss6", "minireac"};

        public static string[] powerupsbm = {"Life", "Energy", "Shield", "Laser", "BlueKey", "RedKey", "YelKey",
                      "R_Pill", "P_Pill", "M_Pill", "Cmiss_1", "Cmiss_4", "QudLas",
                      "Vulcan", "Sprdfr", "Plasma", "Fusion", "Proxim", "Hmiss_1", "Hmiss_4",
                      "Smiss", "Mmiss", "V_Ammo", "Cloak", "Turbo", "Invuln",
                      "Headli", "Megwow", "Gauss", "Helix", "Phoenix", "Omega",
                      "SLaser", "Allmap", "Conv", "Ammork", "Burner", "Hlight", "Scmiss1",
                      "Scmiss4", "Shmiss1", "Shmiss4", "Sproxi", "Merc1",
                      "Merc4", "Eshkr", "BlueFlg", "RedFlag"};

        private static string[] AIBehaviors = { "STILL", "NORMAL", "BEHIND", "RUN_FROM", "SNIPE", "STATION", "FOLLOW" };

        public static string[] pofNames = { "robot09.pof", "robot09s.pof", "robot17.pof", "robot17s.pof", "robot22.pof", "robot22s.pof",
            "robot01.pof", "robot01s.pof", "robot23.pof", "robot23s.pof", "robot37.pof", "robot37s.pof","robot09.pof", "robot09s.pof",
            "robot26.pof", "robot27.pof", "robot27s.pof", "robot42.pof", "robot42s.pof", "robot08.pof", "robot16.pof", "robot16.pof",
            "robot31.pof", "robot32.pof", "robot32s.pof", "robot43.pof", "robot09.pof", "robot09s.pof", "boss01.pof", "robot35.pof",
            "robot35s.pof", "robot37.pof", "robot37s.pof", "robot38.pof", "robot38s.pof", "robot39.pof", "robot39s.pof", "robot40.pof",
            "robot40s.pof", "boss02.pof", "robot36.pof", "robot41.pof", "robot41s.pof", "robot44.pof", "robot45.pof", "robot46.pof",
            "robot47.pof", "robot48.pof", "robot48s.pof", "robot49.pof", "boss01.pof", "robot50.pof", "robot42.pof", "robot42s.pof",
            "robot50.pof", "robot51.pof", "robot53.pof", "robot53s.pof", "robot54.pof", "robot54s.pof", "robot56.pof", "robot56s.pof",
            "robot58.pof", "robot58s.pof", "robot57a.pof", "robot55.pof", "robot55s.pof", "robot59.pof", "robot60.pof", "robot52.pof",
            "robot61.pof", "robot62.pof", "robot63.pof", "robot64.pof", "robot65.pof", "robot66.pof", "boss5.pof", "robot49a.pof",
            "robot58.pof", "robot58.pof", "robot41.pof", "robot41.pof", "robot64.pof", "robot41.pof", "robot41s.pof", "robot64.pof",
            "robot36.pof", "robot63.pof", "robot57.pof", "Boss4.pof", "robot57.pof", "Boss06.pof", "reacbot.pof", "reactor.pof",
            "reactor2.pof", "reactor8.pof", "reactor9.pof", "newreac1.pof", "newreac2.pof", "newreac5.pof", "newreac6.pof", "newreac7.pof",
            "newreac8.pof", "newreac3.pof", "newreac4.pof", "newreac9.pof", "newreac0.pof", "marker.pof", "pship1.pof", "pship1s.pof",
            "pship1b.pof", "laser1-1.pof", "laser11s.pof", "laser12s.pof", "laser1-2.pof", "laser2-1.pof", "laser21s.pof", "laser22s.pof",
            "laser2-2.pof", "laser3-1.pof", "laser31s.pof", "laser32s.pof", "laser3-2.pof", "laser4-1.pof", "laser41s.pof", "laser42s.pof",
            "laser4-2.pof", "cmissile.pof", "flare.pof", "laser3-1.pof", "laser3-2.pof", "fusion1.pof", "fusion2.pof", "cmissile.pof",
            "smissile.pof", "mmissile.pof", "cmissile.pof", "cmissile.pof", "laser1-1.pof", "laser1-2.pof", "laser4-1.pof", "laser4-2.pof",
            "mmissile.pof", "laser5-1.pof", "laser51s.pof", "laser52s.pof", "laser5-2.pof", "laser6-1.pof", "laser61s.pof", "laser62s.pof",
            "laser6-2.pof", "cmissile.pof", "cmissile.pof", "mercmiss.pof", "erthshkr.pof", "tracer.pof", "laser6-1.pof", "laser6-2.pof",
            "cmissile.pof", "newbomb.pof", "erthbaby.pof", "mercmiss.pof", "smissile.pof", "erthshkr.pof", "erthbaby.pof", "cmissile.pof" };

        public static int[] pofIndicies = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 14, 15, 16, 17, 18, 19, 20, 22, 23, 24, 25, 28, 29, 30, 31, 32, 33,
            34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 51, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70,
            71, 72, 73, 74, 75, 76, 77, 88, 89, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112,
            113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 134, 135, 143, 144, 145, 146, 147,
            148, 149, 150, 153, 154, 155, 159, 160 };

        //TODO: This isn't internationalization safe, because c# makes it more painful than it needs to be to format something specifically
        public static string GenerateBitmapsTable(HAMFile datafile, PIGFile piggyFile, SNDFile sndFile)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Robot robot; Weapon weapon; VClip vclip;
            for (int i = 0; i < datafile.Robots.Count; i++)
            {
                robot = datafile.Robots[i];
                TableWriteRobot(datafile, stringBuilder, robot, i);
            }
            for (int i = 0; i < datafile.Robots.Count; i++)
            {
                robot = datafile.Robots[i];
                TableWriteRobotAI(datafile, stringBuilder, robot, i);
            }
            foreach (Reactor reactor in datafile.Reactors)
            {
                TableWriteReactor(datafile, stringBuilder, reactor);
            }
            TableWritePlayerShip(datafile, stringBuilder, datafile.PlayerShip, piggyFile);
            for (int i = 0; i < datafile.Sounds.Count; i++)
                TableWriteSound(datafile, stringBuilder, i, sndFile);
            //stringBuilder.Append("\n");
            TableWriteCockpits(datafile, stringBuilder, piggyFile); //stringBuilder.Append("\n");
            TableWriteGauges(datafile, stringBuilder, piggyFile); //stringBuilder.Append("\n");
            TableWriteGaugesHires(datafile, stringBuilder, piggyFile); //stringBuilder.Append("\n");
            for (int i = 0; i < datafile.Weapons.Count; i++)
            {
                weapon = datafile.Weapons[i];
                TableWriteWeapon(datafile, stringBuilder, weapon, piggyFile, i);
            }
            TableWritePowerups(datafile, stringBuilder); //stringBuilder.Append("\n");
            for (int i = 0; i < datafile.VClips.Count; i++)
            {
                vclip = datafile.VClips[i];
                TableWriteVClip(datafile, stringBuilder, vclip, i, piggyFile);
            }
            //stringBuilder.Append("\n");
            TableWriteTextures(datafile, stringBuilder, piggyFile);
            //stringBuilder.Append("\n");
            TableWriteEClips(datafile, stringBuilder, piggyFile);
            //stringBuilder.Append("\n");
            TableWriteWalls(datafile, stringBuilder, piggyFile);

            return stringBuilder.ToString();
        }

        public static void TableWriteEClips(HAMFile datafile, StringBuilder stringBuilder, PIGFile piggyFile)
        {
            int eclipCount = CountValidEClips(datafile);
            bool extra;
            PIGImage img;
            TMAPInfo info = null;
            stringBuilder.Append("!EFFECTS_FLAG\n$EFFECTS\n");
            foreach (EClip clip in datafile.EClips)
            {
                extra = false;
                if (clip.vc.play_time > 0)
                {
                    stringBuilder.AppendFormat("$ECLIP clip_num={0} time={1:F2} abm_flag=1 ", clip.ID, clip.vc.play_time / 65536.0f);
                    img = piggyFile.images[clip.vc.frames[0]];
                    if (clip.changing_wall_texture != -1)
                    {
                        if (clip.crit_clip != -1)
                            stringBuilder.AppendFormat("crit_clip={0} ", clip.crit_clip);
                        if (clip.dest_bm_num != -1)
                            stringBuilder.AppendFormat("dest_bm={0}.bbm ", piggyFile.images[datafile.Textures[clip.dest_bm_num]].name);
                        if (clip.dest_vclip != 0)
                            stringBuilder.AppendFormat("dest_vclip={0} ", clip.dest_vclip);
                        if (clip.dest_eclip != -1)
                            stringBuilder.AppendFormat("dest_eclip={0} ", clip.dest_eclip);
                        if (clip.sound_num != -1)
                            stringBuilder.AppendFormat("sound_num={0} ", clip.sound_num);
                        if (clip.dest_size != 0)
                            stringBuilder.AppendFormat("dest_size={0:F1} ", clip.dest_size / 65536.0f);
                        info = datafile.TMapInfo[clip.changing_wall_texture];
                        extra = true;
                    }
                    else if (clip.changing_object_texture != -1)
                        stringBuilder.Append("obj_eclip=1 ");
                    if ((img.flags & PIGImage.BM_FLAG_NO_LIGHTING) != 0)
                        stringBuilder.Append("vlighting=-1 ");

                    stringBuilder.AppendFormat("\n{0}.abm ", img.name);

                    if (extra)
                    {
                        if (info.lighting > 0)
                            stringBuilder.AppendFormat("lighting={0:F2} ", info.lighting / 65536.0f);
                        if (info.damage > 0)
                            stringBuilder.AppendFormat("damage={0:F2} ", info.damage/65536.0f);
                        if ((info.flags & TMAPInfo.TMI_VOLATILE) != 0)
                            stringBuilder.Append("volatile ");
                        if ((info.flags & TMAPInfo.TMI_GOAL_RED) != 0)
                            stringBuilder.Append("goal_red ");
                        if ((info.flags & TMAPInfo.TMI_GOAL_BLUE) != 0)
                            stringBuilder.Append("goal_blue ");
                        if ((info.flags & TMAPInfo.TMI_WATER) != 0)
                            stringBuilder.Append("water ");
                        if ((info.flags & TMAPInfo.TMI_FORCE_FIELD) != 0)
                            stringBuilder.Append("force_field ");
                        if ((img.flags & PIGImage.BM_FLAG_SUPER_TRANSPARENT) != 0)
                            stringBuilder.Append("superx=254 ");
                        if (info.slide_u != 0 || info.slide_v != 0)
                            stringBuilder.AppendFormat("slide={0:F1} {1:F1} ", info.slide_u / 256.0f, info.slide_v / 256.0f);
                    }
                    stringBuilder.Append("\n");
                }
            }
        }

        public static void TableWriteWalls(HAMFile datafile, StringBuilder stringBuilder, PIGFile piggyFile)
        {
            PIGImage img;
            TMAPInfo info;
            WClip clip;
            stringBuilder.AppendFormat("$WALL_ANIMS Num_wall_anims={0}\n", datafile.WClips.Count);
            for (int i = 0; i < datafile.WClips.Count; i++)
            {
                clip = datafile.WClips[i];
                if (clip.play_time != 0)
                {
                    info = datafile.TMapInfo[clip.frames[0]];
                    img = piggyFile.images[datafile.Textures[clip.frames[0]]];
                    stringBuilder.AppendFormat("$WCLIP clip_num={0} time={1:F2} abm_flag=1 ", i, clip.play_time / 65536.0f);
                    if (clip.open_sound != -1)
                        stringBuilder.AppendFormat("open_sound={0} ", clip.open_sound);
                    if (clip.close_sound != -1)
                        stringBuilder.AppendFormat("close_sound={0} ", clip.close_sound);
                    if ((img.flags & PIGImage.BM_FLAG_NO_LIGHTING) != 0)
                        stringBuilder.Append("vlighting=-1 ");
                    else
                        stringBuilder.Append("vlighting=0 ");
                    if ((clip.flags & WClip.WCF_TMAP1) != 0)
                        stringBuilder.Append("tmap1_flag=1 ");
                    if ((clip.flags & WClip.WCF_BLASTABLE) != 0)
                        stringBuilder.Append("blastable=1 ");
                    if ((clip.flags & WClip.WCF_BLASTABLE) != 0)
                        stringBuilder.Append("explodes=1 ");
                    if ((clip.flags & WClip.WCF_HIDDEN) != 0)
                        stringBuilder.Append("hidden=1 ");
                    stringBuilder.AppendFormat("\n{0}.abm ", img.name);
                    if (info.lighting > 0)
                        stringBuilder.AppendFormat("lighting={0:F3} ", info.lighting / 65536.0f);
                    if (info.damage > 0)
                        stringBuilder.AppendFormat("damage={0} ", info.damage);
                    if ((info.flags & TMAPInfo.TMI_VOLATILE) != 0)
                        stringBuilder.Append("volatile ");
                    if ((info.flags & TMAPInfo.TMI_GOAL_RED) != 0)
                        stringBuilder.Append("goal_red ");
                    if ((info.flags & TMAPInfo.TMI_GOAL_BLUE) != 0)
                        stringBuilder.Append("goal_blue ");
                    if ((info.flags & TMAPInfo.TMI_WATER) != 0)
                        stringBuilder.Append("water ");
                    if ((info.flags & TMAPInfo.TMI_FORCE_FIELD) != 0)
                        stringBuilder.Append("force_field ");
                    if ((img.flags & PIGImage.BM_FLAG_SUPER_TRANSPARENT) != 0)
                        stringBuilder.Append("superx=254 ");
                    if (info.slide_u != 0 || info.slide_v != 0)
                        stringBuilder.AppendFormat("slide={0:F1} {1:F1} ", info.slide_u / 256.0f, info.slide_v / 256.0f);
                    stringBuilder.Append("\n");
                }
            }
        }

        public static void TableWriteTextures(HAMFile datafile, StringBuilder stringBuilder, PIGFile piggyFile)
        {
            PIGImage img;
            TMAPInfo info;
            EClip clip;
            int firstEClip = datafile.EClips[0].changing_wall_texture;
            bool extra;
            stringBuilder.Append("$TEXTURES\n");
            for (int i = 0; i < firstEClip; i++)
            {
                extra = false;
                img = piggyFile.images[datafile.Textures[i]];
                info = datafile.TMapInfo[i];
                if (img.isAnimated && info.eclip_num == -1) //Probably a WClip so don't write yet. 
                    continue;
                if (info.eclip_num == -1 && i < firstEClip)
                {
                    stringBuilder.AppendFormat("{0}.bbm ", img.name);
                    extra = true;
                }
                else if (info.eclip_num != -1)
                {
                    clip = datafile.EClips[info.eclip_num];
                }

                if (extra)
                {
                    if (info.lighting > 0)
                        stringBuilder.AppendFormat("lighting={0:F3} ", info.lighting / 65536.0f);
                    if (info.damage > 0)
                        stringBuilder.AppendFormat("damage={0} ", info.damage);
                    if ((info.flags & TMAPInfo.TMI_VOLATILE) != 0)
                        stringBuilder.Append("volatile ");
                    if ((info.flags & TMAPInfo.TMI_GOAL_RED) != 0)
                        stringBuilder.Append("goal_red ");
                    if ((info.flags & TMAPInfo.TMI_GOAL_BLUE) != 0)
                        stringBuilder.Append("goal_blue ");
                    if ((info.flags & TMAPInfo.TMI_WATER) != 0)
                        stringBuilder.Append("water ");
                    if ((info.flags & TMAPInfo.TMI_FORCE_FIELD) != 0)
                        stringBuilder.Append("force_field ");
                    if ((img.flags & PIGImage.BM_FLAG_SUPER_TRANSPARENT) != 0)
                        stringBuilder.Append("superx=254 ");
                    if (info.slide_u != 0 || info.slide_v != 0)
                        stringBuilder.AppendFormat("slide={0:F1} {1:F1} ", info.slide_u / 256.0f, info.slide_v / 256.0f);
                    if (info.destroyed != -1)
                    {
                        img = piggyFile.images[datafile.Textures[info.destroyed]];
                        stringBuilder.AppendFormat("destroyed={0}.bbm ", img.name);
                        i++;
                    }
                    stringBuilder.Append("\n");
                }
            }
        }


        public static int CountValidEClips(HAMFile datafile)
        {
            int count = 0;
            foreach (EClip clip in datafile.EClips)
            {
                if (clip.vc.play_time > 0)
                    count++;
            }
            return count;
        }

        private static void TableWriteVClip(HAMFile datafile, StringBuilder stringBuilder, VClip clip, int id, PIGFile piggyFile)
        {
            if (clip.play_time != 0)
            {
                stringBuilder.AppendFormat("$VCLIP clip_num={0} time={1:F2} abm_flag=1 vlighting={2:F2} sound_num={3} ", id, clip.play_time / 65536.0f, clip.light_value / 65536.0f, clip.sound_num);
                if ((clip.flags & 1) != 0)
                    stringBuilder.Append("rod_flag=1");
                stringBuilder.AppendFormat("\n{0}.abm\n", piggyFile.images[clip.frames[0]].name);
            }
        }

        private static void TableWriteWeapon(HAMFile datafile, StringBuilder stringBuilder, Weapon weapon, PIGFile piggyFile, int id)
        {
            if (weapon.render_type == 0)
                stringBuilder.Append("$WEAPON_UNUSED ");
            else
            {
                stringBuilder.Append("$WEAPON ");
                if (weapon.render_type == 1)
                {
                    stringBuilder.AppendFormat("blob_bmp={0}.bbm ", piggyFile.images[weapon.bitmap].name);
                }
                else if (weapon.render_type == 2)
                {
                    stringBuilder.Append("weapon_pof=");
                    WriteModel(datafile, stringBuilder, weapon.model_num);
                    if (weapon.model_num_inner != -1)
                    {
                        stringBuilder.Append("weapon_pof_inner=");
                        WriteModel(datafile, stringBuilder, weapon.model_num_inner);
                    }
                    stringBuilder.AppendFormat("lw_ratio={0:F1} ", weapon.po_len_to_width_ratio / 65536.0f);
                }
                else if (weapon.render_type == 3)
                {
                    stringBuilder.AppendFormat("weapon_vclip={0} ", weapon.weapon_vclip);
                }
                else if (weapon.render_type == 255)
                {
                    stringBuilder.AppendFormat("none_bmp={0}.bbm ", piggyFile.images[weapon.bitmap].name);
                }
                stringBuilder.AppendFormat("mass={0:F1} ", weapon.mass / 65536.0f);
                stringBuilder.AppendFormat("drag={0:F1} ", weapon.drag / 65536.0f);
                stringBuilder.AppendFormat("thrust={0:F1} ", weapon.thrust / 65536.0f);
                if (weapon.matter != 0)
                    stringBuilder.Append("matter=1 ");
                if (weapon.bounce != 0)
                    stringBuilder.AppendFormat("bounce={0} ", weapon.bounce);
                if (weapon.children != -1)
                    stringBuilder.AppendFormat("children={0} ", weapon.children);
                stringBuilder.Append("strength=");
                for (int i = 0; i < 5; i++)
                    stringBuilder.AppendFormat("{0:F1} ", weapon.strength[i] / 65536.0f);
                stringBuilder.Append("speed=");
                for (int i = 0; i < 5; i++)
                    stringBuilder.AppendFormat("{0:F1} ", weapon.speed[i] / 65536.0f);
                if (weapon.speedvar != 128)
                    stringBuilder.AppendFormat("speedvar={0} ", weapon.speedvar);
                stringBuilder.AppendFormat("blob_size={0:F1} ", weapon.blob_size / 65536.0f);
                stringBuilder.AppendFormat("flash_vclip={0} ", weapon.flash_vclip);
                stringBuilder.AppendFormat("flash_size={0:F2} ", weapon.flash_size / 65536.0f);
                if (weapon.flash_sound != 0)
                    stringBuilder.AppendFormat("flash_sound={0} ", weapon.flash_sound);
                stringBuilder.AppendFormat("robot_hit_vclip={0} ", weapon.robot_hit_vclip);
                stringBuilder.AppendFormat("wall_hit_vclip={0} ", weapon.wall_hit_vclip);
                stringBuilder.AppendFormat("robot_hit_sound={0} ", weapon.robot_hit_sound);
                stringBuilder.AppendFormat("wall_hit_sound={0} ", weapon.wall_hit_sound);
                stringBuilder.AppendFormat("impact_size={0:F2} ", weapon.impact_size / 65536.0f);
                if (weapon.afterburner_size != 0)
                    stringBuilder.AppendFormat("afterburner_size={0:F2} ", weapon.afterburner_size / 16.0f);
                stringBuilder.AppendFormat("energy_usage={0:F1} ", weapon.energy_usage / 65536.0f);
                stringBuilder.AppendFormat("ammo_usage={0:F1} ", weapon.ammo_usage / 65536.0f);
                stringBuilder.AppendFormat("fire_wait={0:F1} ", weapon.fire_wait / 65536.0f);
                stringBuilder.AppendFormat("lifetime={0:F1} ", weapon.lifetime / 65536.0f);
                stringBuilder.AppendFormat("lightcast={0:F1} ", weapon.light / 65536.0f);
                if (weapon.damage_radius != 0)
                    stringBuilder.AppendFormat("damage_radius={0:F1} ", weapon.damage_radius / 65536.0f);
                if (weapon.multi_damage_scale != 65536)
                    stringBuilder.AppendFormat("multi_damage_scale={0:F1} ", weapon.multi_damage_scale / 65536.0f);
                stringBuilder.AppendFormat("fire_count={0} ", weapon.fire_count);
                stringBuilder.AppendFormat("flash_vclip={0} ", weapon.flash_vclip);
                if (weapon.persistent != 0)
                    stringBuilder.Append("persistent=1" );
                if (weapon.homing_flag != 0)
                    stringBuilder.Append("homing=1 ");
                if (weapon.flags != 0)
                    stringBuilder.Append("placeable=1 ");
                if (weapon.flash != 0)
                    stringBuilder.AppendFormat("flash={0} ", weapon.flash);
                if (weapon.picture != 0)
                    stringBuilder.AppendFormat("picture={0}.bbm ", piggyFile.images[weapon.picture].name);
                if (weapon.hires_picture != 0)
                    stringBuilder.AppendFormat("hires_picture={0}.bbm ", piggyFile.images[weapon.hires_picture].name);

            }
            stringBuilder.Append("\n");
        }

        private static void TableWriteCockpits(HAMFile datafile, StringBuilder stringBuilder, PIGFile piggyFile)
        {
            stringBuilder.Append("$COCKPIT\n");
            foreach (ushort index in datafile.Cockpits)
            {
                stringBuilder.AppendFormat("{0}.bbm\n", piggyFile.images[index].name);
            }
        }

        private static void TableWritePowerups(HAMFile datafile, StringBuilder stringBuilder)
        {
            Powerup powerup;
            for (int i = 0; i < datafile.Powerups.Count; i++)
            {
                powerup = datafile.Powerups[i];
                if (powerup.vclip_num == 0)
                    stringBuilder.Append("$POWERUP_UNUSED\t");
                else
                    stringBuilder.Append("$POWERUP\t");
                stringBuilder.AppendFormat("name=\"{0}\"\t", powerupsbm[i]);
                stringBuilder.AppendFormat("vclip_num={0}\t", powerup.vclip_num);
                stringBuilder.AppendFormat("hit_sound={0}\t", powerup.hit_sound);
                if (powerup.size != (3 * 65536))
                    stringBuilder.AppendFormat("size={0:N1}\t", powerup.size / 65536.0f);
                if (powerup.size != 21845)
                    stringBuilder.AppendFormat("light={0:N2}", powerup.light / 65536.0f);
                stringBuilder.Append("\n");
            }
        }

        private static void TableWriteGauges(HAMFile datafile, StringBuilder stringBuilder, PIGFile piggyFile)
        {
            stringBuilder.Append("$GAUGES");
            string name = "", lastname;
            int id;
            for (int i = 0; i < datafile.Gauges.Count; i++)
            {
                id = datafile.Gauges[i];
                if (id != 0)
                {
                    PIGImage img = piggyFile.images[id];
                    lastname = name;
                    name = img.name;
                    if (lastname != name)
                    {
                        if (img.isAnimated)
                        {
                            stringBuilder.AppendFormat(" abm_flag=1\n{0}.abm", name);
                        }
                        else
                        {
                            stringBuilder.AppendFormat("\n{0}.bbm", name);
                        }
                    }
                }
            }
            stringBuilder.Append("\n");
        }

        private static void TableWriteGaugesHires(HAMFile datafile, StringBuilder stringBuilder, PIGFile piggyFile)
        {
            stringBuilder.Append("$GAUGES_HIRES");
            string name = "", lastname;
            int id;
            for (int i = 0; i < datafile.GaugesHires.Count; i++)
            {
                id = datafile.GaugesHires[i];
                if (id != 0)
                {
                    PIGImage img = piggyFile.images[id];
                    lastname = name;
                    name = img.name;
                    if (lastname != name)
                    {
                        if (img.isAnimated)
                        {
                            stringBuilder.AppendFormat(" abm_flag=1\n{0}.abm", name);
                        }
                        else
                        {
                            stringBuilder.AppendFormat("\n{0}.bbm", name);
                        }
                    }
                }
            }
            stringBuilder.Append("\n");
        }

        private static void TableWriteSound(HAMFile datafile, StringBuilder stringBuilder, int id, SNDFile sndFile)
        {
            int altID;
            if (datafile.Sounds[id] != 255)
            {
                if (datafile.AltSounds[id] == id)
                    altID = 0;
                else if (datafile.AltSounds[id] == 255)
                    altID = -1;
                else altID = datafile.AltSounds[id];
                stringBuilder.AppendFormat("$SOUND\t{0}\t{1}\t{2}.raw\t;{3}\n", id, altID, sndFile.sounds[datafile.Sounds[id]].name, ElementLists.GetSoundName(id));
            }
        }

        private static void TableWritePlayerShip(HAMFile datafile, StringBuilder stringBuilder, Ship ship, PIGFile pigFile)
        {
            stringBuilder.Append("$MARKER ");
            WriteModel(datafile, stringBuilder, datafile.PlayerShip.markerModel);
            stringBuilder.Append("\n");
            stringBuilder.Append("$PLAYER_SHIP ");
            stringBuilder.AppendFormat("mass={0:F2} ", ship.mass / 65536.0f);
            stringBuilder.AppendFormat("drag={0:F3} ", ship.drag / 65536.0f);
            stringBuilder.AppendFormat("max_thrust={0:F2} ", ship.max_thrust / 65536.0f);
            stringBuilder.AppendFormat("wiggle={0:F2} ", ship.wiggle / 65536.0f);
            stringBuilder.AppendFormat("max_rotthrust={0:F2} ", ship.max_rotthrust / 65536.0f);
            stringBuilder.AppendFormat("expl_vclip_num={0} ", ship.expl_vclip_num);
            stringBuilder.Append("model=");
            WriteModel(datafile, stringBuilder, ship.model_num);
            stringBuilder.Append("multi_textures ");
            for (int i = 0; i < 14; i++)
            {
                int bitmapID = datafile.ObjBitmaps[datafile.ObjBitmapPointers[datafile.FirstMultiBitmapNum + i]];
                string name = pigFile.images[bitmapID].name;
                stringBuilder.AppendFormat("{0}.bbm ", name);
            }
            stringBuilder.Append("\n");
        }

        private static void TableWriteReactor(HAMFile datafile, StringBuilder stringBuilder, Reactor reactor)
        {
            stringBuilder.Append("$REACTOR ");
            WriteModel(datafile, stringBuilder, reactor.model_id);
            stringBuilder.Append("\n");
        }

        private static void TableWriteRobotAI(HAMFile datafile, StringBuilder stringBuilder, Robot robot, int id)
        {
            stringBuilder.AppendFormat("$ROBOT_AI {0} ", id);
            for (int i = 0; i < 5; i++)
            {
                stringBuilder.AppendFormat("{0} ", (int)(Math.Acos(robot.field_of_view[i] / 65536.0d) * 180.0d / Math.PI));
            }
            for (int i = 0; i < 5; i++)
            {
                stringBuilder.AppendFormat("{0:F1} ", robot.firing_wait[i] / 65536.0d);
            }
            for (int i = 0; i < 5; i++)
            {
                stringBuilder.AppendFormat("{0:F1} ", robot.firing_wait2[i] / 65536.0d);
            }
            for (int i = 0; i < 5; i++)
            {
                stringBuilder.AppendFormat("{0} ", robot.rapidfire_count[i]);
            }
            for (int i = 0; i < 5; i++)
            {
                stringBuilder.AppendFormat("{0:F1} ", robot.turn_time[i] / 65536.0d);
            }
            for (int i = 0; i < 5; i++)
            {
                stringBuilder.AppendFormat("{0:F1} ", robot.max_speed[i] / 65536.0d);
            }
            for (int i = 0; i < 5; i++)
            {
                stringBuilder.AppendFormat("{0:F1} ", robot.circle_distance[i] / 65536.0d);
            }
            for (int i = 0; i < 5; i++)
            {
                stringBuilder.AppendFormat("{0} ", robot.evade_speed[i]);
            }
            stringBuilder.Append("\n");
        }

        private static void TableWriteRobot(HAMFile datafile, StringBuilder stringBuilder, Robot robot, int id)
        {
            stringBuilder.Append("$ROBOT ");
            WriteModel(datafile, stringBuilder, robot.model_num);
            stringBuilder.AppendFormat("name=\"{0}\" ", robotsbm[id]);
            stringBuilder.AppendFormat("score_value={0} ", robot.score_value);
            stringBuilder.AppendFormat("mass={0:F3} ", robot.mass / 65536.0f);
            stringBuilder.AppendFormat("drag={0:F3} ", robot.drag / 65536.0f);
            stringBuilder.AppendFormat("exp1_vclip={0} ", robot.exp1_vclip_num);
            stringBuilder.AppendFormat("exp1_sound={0} ", robot.exp1_sound_num);
            stringBuilder.AppendFormat("exp2_vclip={0} ", robot.exp2_vclip_num);
            stringBuilder.AppendFormat("exp2_sound={0} ", robot.exp2_sound_num);
            stringBuilder.AppendFormat("lighting={0:F3} ", robot.lighting / 65536.0f);
            if (robot.weapon_type != 0)
            stringBuilder.AppendFormat("weapon_type={0} ", robot.weapon_type);
            if (robot.weapon_type2 != -1)
                stringBuilder.AppendFormat("weapon_type2={0} ", robot.weapon_type2);
            stringBuilder.AppendFormat("strength={0} ", robot.strength/65536);
            stringBuilder.AppendFormat("weapon_type={0} ", robot.weapon_type);
            if (robot.contains_type == 2)
                stringBuilder.Append("contains_type=1 ");
            stringBuilder.AppendFormat("contains_id={0} ", robot.contains_id);
            stringBuilder.AppendFormat("contains_count={0} ", robot.contains_count);
            stringBuilder.AppendFormat("contains_prob={0} ", robot.contains_prob);
            if (robot.attack_type != 0)
                stringBuilder.Append("attack_type=1 ");
            stringBuilder.AppendFormat("see_sound={0} ", robot.see_sound);
            stringBuilder.AppendFormat("attack_sound={0} ", robot.attack_sound);
            if (robot.claw_sound != 190)
                stringBuilder.AppendFormat("claw_sound={0} ", robot.claw_sound);
            if (robot.cloak_type != 0)
                stringBuilder.AppendFormat("cloak_type={0} ", robot.cloak_type);
            if (robot.glow != 0)
                stringBuilder.AppendFormat("glow={0:F3} ", robot.glow / 65536.0f);
            if (robot.lightcast != 0)
                stringBuilder.AppendFormat("lightcast={0} ", robot.lightcast);
            if (robot.badass != 0)
                stringBuilder.AppendFormat("badass={0} ", robot.badass);
            if (robot.death_roll != 0)
                stringBuilder.AppendFormat("death_roll={0} ", robot.death_roll);
            if (robot.deathroll_sound != 185)
                stringBuilder.AppendFormat("deathroll_sound={0} ", robot.deathroll_sound);
            if (robot.thief != 0)
                stringBuilder.Append("thief=1 ");
            if (robot.kamikaze != 0)
                stringBuilder.Append("kamikaze=1 ");
            if (robot.companion != 0)
                stringBuilder.Append("companion=1 ");
            if (robot.pursuit != 0)
                stringBuilder.AppendFormat("pursuit={0} ", robot.pursuit);
            if (robot.smart_blobs != 0)
                stringBuilder.AppendFormat("smart_blobs={0} ", robot.smart_blobs);
            if (robot.energy_blobs != 0)
                stringBuilder.AppendFormat("energy_blobs={0} ", robot.energy_blobs);
            if (robot.energy_drain != 0)
                stringBuilder.AppendFormat("energy_drain={0} ", robot.energy_drain);
            if (robot.boss_flag != 0)
                stringBuilder.AppendFormat("boss={0} ", robot.boss_flag);
            if ((robot.flags & 1) != 0)
                stringBuilder.Append("big_radius=1 ");
            if (robot.aim != 255)
                stringBuilder.AppendFormat("aim={0:F2} ", robot.aim / 255.0f);
            if (robot.behavior >= 0x80 && robot.behavior != 0x81)
                stringBuilder.AppendFormat("behavior={0} ", AIBehaviors[robot.behavior - 0x80]);
            stringBuilder.Append("\n");
        }

        private static void WriteModel(HAMFile datafile, StringBuilder stringBuilder,  int id, bool hack = false)
        {
            Polymodel model = datafile.PolygonModels[id];
            //stringBuilder.AppendFormat("model{0}.pof ", id);
            stringBuilder.AppendFormat("{0} ", pofNames[id]);
            if (!hack)
            {
                foreach (string texture in model.textureList)
                {
                    if (datafile.EClipNameMapping.ContainsKey(texture.ToLower()))
                    {
                        stringBuilder.AppendFormat("%{0} ", datafile.EClipNameMapping[texture.ToLower()].ID);
                    }
                    else
                    {
                        stringBuilder.AppendFormat("{0}.bbm ", texture);
                    }
                }
            }
            if (model.DyingModelnum != -1)
            {
                stringBuilder.Append("dying_pof=");
                WriteModel(datafile, stringBuilder, model.DyingModelnum, true);
            }
            if (model.simpler_model != 0)
            {
                stringBuilder.Append("simple_model=");
                WriteModel(datafile, stringBuilder, model.simpler_model - 1);
            }
            if (model.DeadModelnum != -1)
            {
                stringBuilder.Append("dead_pof=");
                WriteModel(datafile, stringBuilder, model.DeadModelnum);
            }
        }
    }
}
