﻿/*
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

using System.IO;

namespace LibDescent.Data
{
    class HAMDataReader
    {
        public TMAPInfo ReadTMAPInfo(BinaryReader br)
        {
            TMAPInfo mapinfo = new TMAPInfo();
            mapinfo.flags = br.ReadByte();
            br.ReadBytes(3);
            mapinfo.lighting = br.ReadInt32();
            mapinfo.damage = br.ReadInt32();
            mapinfo.eclip_num = br.ReadInt16();
            mapinfo.destroyed = br.ReadInt16();
            mapinfo.slide_u = br.ReadInt16();
            mapinfo.slide_v = br.ReadInt16();

            return mapinfo;
        }

        public VClip ReadVClip(BinaryReader br)
        {
            VClip clip = new VClip();
            clip.play_time = br.ReadInt32();
            clip.num_frames = br.ReadInt32();
            clip.frame_time = br.ReadInt32();
            clip.flags = br.ReadInt32();
            clip.sound_num = br.ReadInt16();
            for (int f = 0; f < 30; f++)
            {
                clip.frames[f] = br.ReadUInt16();
            }
            clip.light_value = br.ReadInt32();

            return clip;
        }

        public EClip ReadEClip(BinaryReader br)
        {
            EClip clip = new EClip();
            clip.vc.play_time = br.ReadInt32();
            clip.vc.num_frames = br.ReadInt32();
            clip.vc.frame_time = br.ReadInt32();
            clip.vc.flags = br.ReadInt32();
            clip.vc.sound_num = br.ReadInt16();
            for (int f = 0; f < 30; f++)
            {
                clip.vc.frames[f] = br.ReadUInt16();
            }
            clip.vc.light_value = br.ReadInt32();
            clip.time_left = br.ReadInt32();
            clip.frame_count = br.ReadInt32();
            clip.changing_wall_texture = br.ReadInt16();
            clip.changing_object_texture = br.ReadInt16();
            clip.flags = br.ReadInt32();
            clip.crit_clip = br.ReadInt32();
            clip.dest_bm_num = br.ReadInt32();
            clip.dest_vclip = br.ReadInt32();
            clip.dest_eclip = br.ReadInt32();
            clip.dest_size = br.ReadInt32();
            clip.sound_num = br.ReadInt32();
            clip.segnum = br.ReadInt32();
            clip.sidenum = br.ReadInt32();

            return clip;
        }

        public WClip ReadWClip(BinaryReader br)
        {
            WClip clip = new WClip();
            clip.play_time = br.ReadInt32();
            clip.num_frames = br.ReadInt16();
            for (int f = 0; f < 50; f++)
            {
                clip.frames[f] = br.ReadUInt16();
            }
            clip.open_sound = br.ReadInt16();
            clip.close_sound = br.ReadInt16();
            clip.flags = br.ReadInt16();
            for (int c = 0; c < 13; c++)
            {
                clip.filename[c] = br.ReadChar();
            }
            clip.pad = br.ReadByte();

            return clip;
        }

        public Robot ReadRobot(BinaryReader br)
        {
            Robot robot = new Robot();
            robot.model_num = br.ReadInt32();

            for (int s = 0; s < Polymodel.MAX_GUNS; s++)
            {
                robot.gun_points[s] = new FixVector(br.ReadInt32(), br.ReadInt32(), br.ReadInt32());
            }
            for (int s = 0; s < 8; s++)
            {
                robot.gun_submodels[s] = br.ReadByte();
            }
            robot.exp1_vclip_num = br.ReadInt16();
            robot.exp1_sound_num = br.ReadInt16();
            
            robot.exp2_vclip_num = br.ReadInt16();
            robot.exp2_sound_num = br.ReadInt16();
            
            robot.weapon_type = br.ReadSByte();
            robot.weapon_type2 = br.ReadSByte();
            robot.n_guns = br.ReadSByte();
            robot.contains_id = br.ReadSByte();
            
            robot.contains_count = br.ReadSByte();
            robot.contains_prob = br.ReadSByte();
            robot.contains_type = br.ReadSByte();
            robot.kamikaze = br.ReadSByte();
            
            robot.score_value = br.ReadInt16();
            robot.badass = br.ReadByte();
            robot.energy_drain = br.ReadByte();
            
            robot.lighting = br.ReadInt32();
            robot.strength = br.ReadInt32();
            
            robot.mass = br.ReadInt32();
            robot.drag = br.ReadInt32();
            for (int s = 0; s < Robot.NUM_DIFFICULTY_LEVELS; s++)
            {
                robot.field_of_view[s] = br.ReadInt32();
            }
            for (int s = 0; s < Robot.NUM_DIFFICULTY_LEVELS; s++)
            {
                robot.firing_wait[s] = br.ReadInt32();
            }
            for (int s = 0; s < Robot.NUM_DIFFICULTY_LEVELS; s++)
            {
                robot.firing_wait2[s] = br.ReadInt32();
            }
            for (int s = 0; s < Robot.NUM_DIFFICULTY_LEVELS; s++)
            {
                robot.turn_time[s] = br.ReadInt32();
            }
            for (int s = 0; s < Robot.NUM_DIFFICULTY_LEVELS; s++)
            {
                robot.max_speed[s] = br.ReadInt32();
            }
            for (int s = 0; s < Robot.NUM_DIFFICULTY_LEVELS; s++)
            {
                robot.circle_distance[s] = br.ReadInt32();
            }
            for (int s = 0; s < Robot.NUM_DIFFICULTY_LEVELS; s++)
            {
                robot.rapidfire_count[s] = br.ReadSByte();
            }
            for (int s = 0; s < Robot.NUM_DIFFICULTY_LEVELS; s++)
            {
                robot.evade_speed[s] = br.ReadSByte();
            }
            robot.cloak_type = br.ReadSByte();
            robot.attack_type = br.ReadSByte();
            
            robot.see_sound = br.ReadByte();
            robot.attack_sound = br.ReadByte();
            robot.claw_sound = br.ReadByte();
            robot.taunt_sound = br.ReadByte();

            robot.boss_flag = br.ReadSByte();
            robot.companion = br.ReadSByte();
            robot.smart_blobs = br.ReadSByte();
            robot.energy_blobs = br.ReadSByte();

            robot.thief = br.ReadSByte();
            robot.pursuit = br.ReadSByte();
            robot.lightcast = br.ReadSByte();
            robot.death_roll = br.ReadSByte();

            robot.flags = br.ReadByte();
            br.ReadBytes(3);

            robot.deathroll_sound = br.ReadByte();
            robot.glow = br.ReadByte();
            robot.behavior = br.ReadByte();
            robot.aim = br.ReadByte();

            for (int v = 0; v < 9; v++)
            {
                for (int u = 0; u < 5; u++)
                {
                    robot.anim_states[v, u].n_joints = br.ReadInt16();
                    robot.anim_states[v, u].offset = br.ReadInt16();
                }
            }
            robot.always_0xabcd = br.ReadInt32();

            return robot;
        }

        public Weapon ReadWeapon(BinaryReader br)
        {
            Weapon weapon = new Weapon();
            weapon.render_type = br.ReadByte();
            weapon.persistent = br.ReadByte();
            weapon.model_num = br.ReadInt16();
            weapon.model_num_inner = br.ReadInt16();
            
            weapon.flash_vclip = br.ReadSByte();
            weapon.robot_hit_vclip = br.ReadSByte();
            weapon.flash_sound = br.ReadInt16();

            weapon.wall_hit_vclip = br.ReadSByte();
            weapon.fire_count = br.ReadByte();
            weapon.robot_hit_sound = br.ReadInt16();
            
            weapon.ammo_usage = br.ReadByte();
            weapon.weapon_vclip = br.ReadByte();
            weapon.wall_hit_sound = br.ReadInt16();
            
            weapon.destroyable = br.ReadByte();
            weapon.matter = br.ReadByte();
            weapon.bounce = br.ReadByte();
            weapon.homing_flag = br.ReadByte();

            weapon.speedvar = br.ReadByte();

            weapon.flags = br.ReadByte();

            weapon.flash = br.ReadSByte();
            weapon.afterburner_size = br.ReadSByte();

            weapon.children = br.ReadSByte();
 
            weapon.energy_usage = br.ReadInt32();
            weapon.fire_wait = br.ReadInt32();

            weapon.multi_damage_scale = br.ReadInt32();
            
            weapon.bitmap = br.ReadUInt16();
            
            weapon.blob_size = br.ReadInt32();
            weapon.flash_size = br.ReadInt32();
            weapon.impact_size = br.ReadInt32();
            for (int s = 0; s < 5; s++)
            {
                weapon.strength[s] = br.ReadInt32();
            }
            for (int s = 0; s < 5; s++)
            {
                weapon.speed[s] = br.ReadInt32();
            }
            weapon.mass = br.ReadInt32();
            weapon.drag = br.ReadInt32();
            weapon.thrust = br.ReadInt32();
            weapon.po_len_to_width_ratio = br.ReadInt32();
            weapon.light = br.ReadInt32();
            weapon.lifetime = br.ReadInt32();
            weapon.damage_radius = br.ReadInt32();
            
            weapon.picture = br.ReadUInt16();
            weapon.hires_picture = br.ReadUInt16();

            return weapon;
        }

        public Weapon ReadWeaponInfoOld(BinaryReader br)
        {
            Weapon weapon = new Weapon();
            weapon.render_type = br.ReadByte();
            weapon.persistent = br.ReadByte();
            weapon.model_num = br.ReadInt16();
            weapon.model_num_inner = br.ReadInt16();

            weapon.flash_vclip = br.ReadSByte();
            weapon.robot_hit_vclip = br.ReadSByte();
            weapon.flash_sound = br.ReadInt16();

            weapon.wall_hit_vclip = br.ReadSByte();
            weapon.fire_count = br.ReadByte();
            weapon.robot_hit_sound = br.ReadInt16();

            weapon.ammo_usage = br.ReadByte();
            weapon.weapon_vclip = br.ReadByte();
            weapon.wall_hit_sound = br.ReadInt16();

            weapon.destroyable = br.ReadByte();
            weapon.matter = br.ReadByte();
            weapon.bounce = br.ReadByte();
            weapon.homing_flag = br.ReadByte();

            weapon.speedvar = br.ReadByte();

            weapon.flags = br.ReadByte();

            weapon.flash = br.ReadSByte();
            weapon.afterburner_size = br.ReadSByte();

            weapon.children = 0;

            weapon.energy_usage = br.ReadInt32();
            weapon.fire_wait = br.ReadInt32();

            weapon.multi_damage_scale = 65536;

            weapon.bitmap = br.ReadUInt16();

            weapon.blob_size = br.ReadInt32();
            weapon.flash_size = br.ReadInt32();
            weapon.impact_size = br.ReadInt32();
            for (int s = 0; s < 5; s++)
            {
                weapon.strength[s] = br.ReadInt32();
            }
            for (int s = 0; s < 5; s++)
            {
                weapon.speed[s] = br.ReadInt32();
            }
            weapon.mass = br.ReadInt32();
            weapon.drag = br.ReadInt32();
            weapon.thrust = br.ReadInt32();
            weapon.po_len_to_width_ratio = br.ReadInt32();
            weapon.light = br.ReadInt32();
            weapon.lifetime = br.ReadInt32();
            weapon.damage_radius = br.ReadInt32();

            weapon.picture = br.ReadUInt16();
            weapon.hires_picture = weapon.picture;

            return weapon;
        }
        
        public Polymodel ReadPolymodelInfo(BinaryReader br)
        {
            Polymodel model = new Polymodel(Polymodel.MAX_SUBMODELS);
            model.n_models = br.ReadInt32();
            model.model_data_size = br.ReadInt32();
            model.model_data = br.ReadInt32();
            for (int s = 0; s < Polymodel.MAX_SUBMODELS; s++)
            {
                model.submodels[s].Pointer = br.ReadInt32();
            }
            for (int s = 0; s < Polymodel.MAX_SUBMODELS; s++)
            {
                model.submodels[s].Offset.x = br.ReadInt32();
                model.submodels[s].Offset.y = br.ReadInt32();
                model.submodels[s].Offset.z = br.ReadInt32();
            }
            for (int s = 0; s < Polymodel.MAX_SUBMODELS; s++)
            {
                model.submodels[s].Normal.x = br.ReadInt32();
                model.submodels[s].Normal.y = br.ReadInt32();
                model.submodels[s].Normal.z = br.ReadInt32();
            }
            for (int s = 0; s < Polymodel.MAX_SUBMODELS; s++)
            {
                model.submodels[s].Point.x = br.ReadInt32();
                model.submodels[s].Point.y = br.ReadInt32();
                model.submodels[s].Point.z = br.ReadInt32();
            }
            for (int s = 0; s < Polymodel.MAX_SUBMODELS; s++)
            {
                model.submodels[s].Radius = br.ReadInt32();
            }
            for (int s = 0; s < Polymodel.MAX_SUBMODELS; s++)
            {
                byte parent = br.ReadByte();
                model.submodels[s].Parent = parent;
                if (parent != 255)
                    model.submodels[parent].Children.Add(model.submodels[s]);
            }
            for (int s = 0; s < Polymodel.MAX_SUBMODELS; s++)
            {
                model.submodels[s].Mins.x = br.ReadInt32();
                model.submodels[s].Mins.y = br.ReadInt32();
                model.submodels[s].Mins.z = br.ReadInt32();
            }
            for (int s = 0; s < Polymodel.MAX_SUBMODELS; s++)
            {
                model.submodels[s].Maxs.x = br.ReadInt32();
                model.submodels[s].Maxs.y = br.ReadInt32();
                model.submodels[s].Maxs.z = br.ReadInt32();
            }
            model.mins.x = br.ReadInt32();
            model.mins.y = br.ReadInt32();
            model.mins.z = br.ReadInt32();
            model.maxs.x = br.ReadInt32();
            model.maxs.y = br.ReadInt32();
            model.maxs.z = br.ReadInt32();
            model.rad = br.ReadInt32();
            model.n_textures = br.ReadByte();
            model.first_texture = br.ReadUInt16();
            model.simpler_model = br.ReadByte();

            return model;
        }

        public Ship ReadPlayerShip(BinaryReader br)
        {
            Ship PlayerShip = new Ship();
            PlayerShip.model_num = br.ReadInt32();
            PlayerShip.expl_vclip_num = br.ReadInt32();
            PlayerShip.mass = br.ReadInt32();
            PlayerShip.drag = br.ReadInt32();
            PlayerShip.max_thrust = br.ReadInt32();
            PlayerShip.reverse_thrust = br.ReadInt32();
            PlayerShip.brakes = br.ReadInt32();
            PlayerShip.wiggle = br.ReadInt32();
            PlayerShip.max_rotthrust = br.ReadInt32();

            return PlayerShip;
        }
    }
}
