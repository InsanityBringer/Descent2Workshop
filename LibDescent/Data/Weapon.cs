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

namespace LibDescent.Data
{
    public class Weapon
    {
        public byte render_type;				// How to draw 0=crash, 1=blob, 2=object, 3=vclip, 255=invis
        public byte persistent;					//	0 = dies when it hits something, 1 = continues (eg, fusion cannon)
        public short model_num;					// Model num if rendertype==2.
        public short model_num_inner;			// Model num of inner part if rendertype==2.

        public sbyte flash_vclip;				// What vclip to use for muzzle flash
        public sbyte robot_hit_vclip;			// What vclip for impact with robot
        public short flash_sound;				// What sound to play when fired

        public sbyte wall_hit_vclip;			// What vclip for impact with wall
        public byte fire_count;					//	Number of bursts fired from EACH GUN per firing.  For weapons which fire from both sides, 3*fire_count shots will be fired.
        public short robot_hit_sound;			// What sound for impact with robot
       
        public byte ammo_usage;					//	How many units of ammunition it uses.
        public sbyte weapon_vclip;				//	Vclip to render for the weapon, itself.
        public short wall_hit_sound;			// What sound for impact with wall
        
        public byte destroyable;				//	If !0, this weapon can be destroyed by another weapon.
        public byte matter;						//	Flag: set if this object is matter (as opposed to energy)
        public byte bounce;						//	Flag: set if this object bounces off walls
        public byte homing_flag;				//	Set if this weapon can home in on a target.

        public byte speedvar; //	allowed variance in speed below average, /128: 64 = 50% meaning if speed = 100, can be 50..100

        public byte flags;

        public sbyte flash;
        public sbyte afterburner_size;

        public sbyte children;

        public Fix energy_usage;				//	How much fuel is consumed to fire this weapon.
        public Fix fire_wait;					//	Time until this weapon can be fired again.

        public Fix multi_damage_scale;

        public ushort bitmap;				// Pointer to bitmap if rendertype==0 or 1.

        public Fix blob_size;					// Size of blob if blob type
        public Fix flash_size;					// How big to draw the flash
        public Fix impact_size;				// How big of an impact
        public Fix[] strength = new Fix[5];				// How much damage it can inflict
        public Fix[] speed = new Fix[5];					// How fast it can move, difficulty level based.
        public Fix mass;							// How much mass it has
        public Fix drag;							// How much drag it has
        public Fix thrust;						//	How much thrust it has
        public Fix po_len_to_width_ratio;	// For polyobjects, the ratio of len/width. (10 maybe?)
        public Fix light;						//	Amount of light this weapon casts.
        public Fix lifetime;					//	Lifetime in seconds of this weapon.
        public Fix damage_radius;				//	Radius of damage caused by weapon, used for missiles (not lasers) to apply to damage to things it did not hit
        
        public ushort picture;				// a picture of the weapon for the cockpit
        public ushort hires_picture;                // a picture of the weapon for the cockpit

        public int ID;

        public Weapon()
        {
            model_num = -1;
            model_num_inner = -1;
            render_type = 3;
            po_len_to_width_ratio = 10;
            speedvar = 128;
            fire_count = 1;
            children = -1;
        }

        public void CopyDataFrom(Weapon other, IElementManager manager)
        {
            render_type = other.render_type;
            model_num = other.model_num;
            model_num_inner = other.model_num_inner;
            flash_vclip = other.flash_vclip;
            robot_hit_vclip = other.robot_hit_vclip;
            flash_sound = other.flash_sound;
            wall_hit_vclip = other.wall_hit_vclip;
            fire_count = other.fire_count;
            robot_hit_sound = other.robot_hit_sound;
            ammo_usage = other.ammo_usage;
            weapon_vclip = other.weapon_vclip;
            wall_hit_sound = other.wall_hit_sound;
            bounce = other.bounce;
            speedvar = other.speedvar;
            flash = other.flash;
            afterburner_size = other.afterburner_size;
            children = other.children;
            energy_usage = other.energy_usage;
            fire_wait = other.fire_wait;
            multi_damage_scale = other.multi_damage_scale;
            destroyable = other.destroyable;
            matter = other.matter;
            homing_flag = other.homing_flag;
            flags = other.flags;
            bitmap = other.bitmap;
            blob_size = other.blob_size;
            flash_size = other.flash_size;
            impact_size = other.impact_size;
            mass = other.mass;
            drag = other.drag;
            thrust = other.thrust;
            po_len_to_width_ratio = other.po_len_to_width_ratio;
            light = other.light;
            lifetime = other.lifetime;
            damage_radius = other.damage_radius;
            picture = other.picture;
            hires_picture = other.hires_picture;
            for (int i = 0; i < 5; i++)
            {
                strength[i] = other.strength[i];
                speed[i] = other.speed[i];
            }
        }

        public void UpdateWeapon(int tag, int value, int index, IElementManager manager)
        {
            switch (tag)
            {
                case 2:
                    model_num = (sbyte)(value-1);
                    break;
                case 3:
                    model_num_inner = (sbyte)(value-1);
                    break;
                case 4:
                    flash_vclip = (sbyte)(value - 1);
                    break;
                case 5:
                    robot_hit_vclip = (sbyte)(value - 1);
                    break;
                case 6:
                    if (value == 0) flash_sound = -1;
                    else flash_sound = (short)(value - 1);
                    break;
                case 7:
                    wall_hit_vclip = (sbyte)(value - 1);
                    break;
                case 8:
                    fire_count = (byte)value;
                    break;
                case 9:
                    if (value == 0) robot_hit_sound = -1;
                    else robot_hit_sound = (short)(value - 1);
                    break;
                case 10:
                    ammo_usage = (byte)value;
                    break;
                case 11:
                    weapon_vclip = (sbyte)(value - 1);
                    break;
                case 12:
                    if (value == 0) wall_hit_sound = -1;
                    else wall_hit_sound = (short)(value - 1);
                    break;
                case 15:
                    bounce = (byte)value;
                    break;
                case 17:
                    speedvar = (byte)value;
                    break;
                case 19:
                    flash = (sbyte)value;
                    break;
                case 20:
                    afterburner_size = (sbyte)value;
                    break;
                case 21:
                    children = (sbyte)(value - 1);
                    break;
                case 22:
                    energy_usage = new Fix(value);
                    break;
                case 23:
                    fire_wait = new Fix(value);
                    break;
                case 24:
                    multi_damage_scale = value;
                    break;
                case 25:
                    bitmap = (ushort)value;
                    break;
                case 26:
                    blob_size = new Fix(value);
                    break;
                case 27:
                    flash_size = new Fix(value);
                    break;
                case 28:
                    impact_size = new Fix(value);
                    break;
                case 29:
                    strength[index] = new Fix(value);
                    break;
                case 30:
                    speed[index] = new Fix(value);
                    break;
                case 31:
                    mass = new Fix(value);
                    break;
                case 32:
                    drag = new Fix(value);
                    break;
                case 33:
                    thrust = new Fix(value);
                    break;
                case 34:
                    po_len_to_width_ratio = new Fix(value);
                    break;
                case 35:
                    light = new Fix(value);
                    break;
                case 36:
                    lifetime = new Fix(value);
                    break;
                case 37:
                    picture = (ushort)value;
                    break;
                case 38:
                    hires_picture = (ushort)value;
                    break;
                case 39:
                    damage_radius = new Fix(value);
                    break;
            }
        }
    }
}
