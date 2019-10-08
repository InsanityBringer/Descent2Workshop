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
            bw.Write(tmapinfo.eclip_num);
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
            bw.Write(clip.crit_clip);
            bw.Write(clip.dest_bm_num);
            bw.Write(clip.dest_vclip);
            bw.Write(clip.dest_eclip);
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
            bw.Write(robot.model_num);
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
            bw.Write(robot.exp1_vclip_num);
            bw.Write(robot.exp1_sound_num);
            
            bw.Write(robot.exp2_vclip_num);
            bw.Write(robot.exp2_sound_num);
            
            bw.Write(robot.weapon_type);
            bw.Write(robot.weapon_type2);
            bw.Write(robot.n_guns);
            bw.Write(robot.contains_id);

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
            bw.Write(weapon.model_num);
            bw.Write(weapon.model_num_inner);
            
            bw.Write(weapon.flash_vclip);
            bw.Write(weapon.robot_hit_vclip);
            bw.Write(weapon.flash_sound);

            bw.Write(weapon.wall_hit_vclip);
            bw.Write(weapon.fire_count);
            bw.Write(weapon.robot_hit_sound);
            
            bw.Write(weapon.ammo_usage);
            bw.Write(weapon.weapon_vclip);
            bw.Write(weapon.wall_hit_sound);

            bw.Write(weapon.destroyable);
            bw.Write(weapon.matter);
            bw.Write(weapon.bounce);
            bw.Write(weapon.homing_flag);

            bw.Write(weapon.speedvar);

            bw.Write(weapon.flags);

            bw.Write(weapon.flash);
            bw.Write(weapon.afterburner_size);

            bw.Write(weapon.children);
            
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
            bw.Write(weapon.model_num);
            bw.Write(weapon.model_num_inner);

            bw.Write(weapon.flash_vclip);
            bw.Write(weapon.robot_hit_vclip);
            bw.Write(weapon.flash_sound);

            bw.Write(weapon.wall_hit_vclip);
            bw.Write(weapon.fire_count);
            bw.Write(weapon.robot_hit_sound);

            bw.Write(weapon.ammo_usage);
            bw.Write(weapon.weapon_vclip);
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
            bw.Write(model.simpler_model);
        }

        public void WritePlayerShip(Ship ship, BinaryWriter bw)
        {
            bw.Write(ship.model_num);
            bw.Write(ship.expl_vclip_num);
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
    }
}
