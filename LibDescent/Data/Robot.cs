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

namespace LibDescent.Data
{
    public class Robot
    {
        public const int NUM_ANIMATION_STATES = 5;
        public const int NUM_DIFFICULTY_LEVELS = 5;
        //important crap
        public struct jointlist
        {
            public short n_joints;
            public short offset;
        }

        public int model_num;							// which polygon model? //1
        
        public FixVector[] gun_points = new FixVector[Polymodel.MAX_GUNS];			// where each gun model is
        public byte[] gun_submodels = new byte[Polymodel.MAX_GUNS];		// which submodel is each gun in?
        
        public short exp1_vclip_num; //2
        public short exp1_sound_num; //3
        
        public short exp2_vclip_num; //4 
        public short exp2_sound_num; //5
        
        public sbyte weapon_type; //6
        public sbyte weapon_type2; //7
        public sbyte n_guns;								// how many different gun positions //8
        public sbyte contains_id;						//	ID of powerup this robot can contain.
        
        public sbyte contains_count;					//	Max number of things this instance can contain.
        public sbyte contains_prob;						//	Probability that this instance will contain something in N/16
        public sbyte contains_type;						//	Type of thing contained, robot or powerup, in bitmaps.tbl, !0=robot, 0=powerup
        public sbyte kamikaze; 
        
        public short score_value;						//	Score from this robot.
        public byte badass;
        public byte energy_drain;
        
        public Fix lighting;							// should this be here or with polygon model?
        public Fix strength;							// Initial shields of robot

        public Fix mass;										// how heavy is this thing?
        public Fix drag;										// how much drag does it have?

        public Fix[] field_of_view = new Fix[NUM_DIFFICULTY_LEVELS];					// compare this value with forward_vector.dot.vector_to_player, if field_of_view <, then robot can see player
        public Fix[] firing_wait = new Fix[NUM_DIFFICULTY_LEVELS];						//	time in seconds between shots
        public Fix[] firing_wait2 = new Fix[NUM_DIFFICULTY_LEVELS];						//	time in seconds between shots
        public Fix[] turn_time = new Fix[NUM_DIFFICULTY_LEVELS];						// time in seconds to rotate 360 degrees in a dimension
        public Fix[] max_speed = new Fix[NUM_DIFFICULTY_LEVELS];						//	maximum speed attainable by this robot
        public Fix[] circle_distance = new Fix[NUM_DIFFICULTY_LEVELS];				//	distance at which robot circles player

        public sbyte[] rapidfire_count = new sbyte[NUM_DIFFICULTY_LEVELS];				//	number of shots fired rapidly
        public sbyte[] evade_speed = new sbyte[NUM_DIFFICULTY_LEVELS];						//	rate at which robot can evade shots, 0=none, 4=very fast
        public sbyte cloak_type;								//	0=never, 1=always, 2=except-when-firing
        public sbyte attack_type;							//	0=firing, 1=charge (like green guy)

        public byte see_sound;								//	sound robot makes when it first sees the player
        public byte attack_sound;							//	sound robot makes when it attacks the player
        public byte claw_sound;								//	sound robot makes as it claws you (attack_type should be 1)
        public byte taunt_sound;
        
        public sbyte boss_flag;								//	0 = not boss, 1 = boss.  Is that surprising?
        public sbyte companion;								//	Companion robot, leads you to things.
        public sbyte smart_blobs;							//	how many smart blobs are emitted when this guy dies!
        public sbyte energy_blobs;							//	how many smart blobs are emitted when this guy gets hit by energy weapon!

        public sbyte thief;									//	!0 means this guy can steal when he collides with you!
        public sbyte pursuit;									//	!0 means pursues player after he goes around a corner.  4 = 4/2 pursue up to 4/2 seconds after becoming invisible if up to 4 segments away
        public sbyte lightcast;								//	Amount of light cast. 1 is default.  10 is very large.
        public sbyte death_roll;								//	0 = dies without death roll. !0 means does death roll, larger = faster and louder

        public byte	flags;									// misc properties, and by that a single flag that is literally not used in the final game. heeeh
        //three bytes pad

        public byte deathroll_sound;						// if has deathroll, what sound?
        public byte glow;										// apply this light to robot itself. stored as 4:4 fixed-point
        public byte behavior;								//	Default behavior.
        public byte aim;										//	255 = perfect, less = more likely to miss.  0 != random, would look stupid.  0=45 degree spread.  Specify in bitmaps.tbl in range 0.0..1.0

        //animation info
        public jointlist[,] anim_states = new jointlist[Polymodel.MAX_GUNS+1, NUM_ANIMATION_STATES];
        public int baseJoint = 0; //for HXM files aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa

        public int always_0xabcd;							// debugging

        public int replacementID;
        public int ID;

        public Robot()
        {
            always_0xabcd = 0xabcd;
            contains_type = 7;
            weapon_type2 = -1;
        }

        public bool UpdateRobot(int tag, ref int value, int curAI, int curGun)
        {
            bool clamped = false;
            switch (tag)
            {
                case 1:
                    value = Util.Clamp(value, int.MinValue, int.MaxValue, ref clamped);
                    model_num = value;
                    break;
                case 2:
                    value = Util.Clamp(value, short.MinValue, short.MaxValue, ref clamped);
                    exp1_vclip_num = (short)value;
                    break;
                case 3:
                    value = Util.Clamp(value, short.MinValue, short.MaxValue, ref clamped);
                    exp1_sound_num = (short)value;
                    break;
                case 4:
                    value = Util.Clamp(value, short.MinValue, short.MaxValue, ref clamped);
                    exp2_vclip_num = (short)value;
                    break;
                case 5:
                    value = Util.Clamp(value, sbyte.MinValue, sbyte.MaxValue, ref clamped);
                    weapon_type = (sbyte)value;
                    break;
                case 6:
                    weapon_type2 = (sbyte)(value - 1);
                    break;
                case 7:
                    value = Util.Clamp(value, sbyte.MinValue, sbyte.MaxValue, ref clamped);
                    n_guns = (sbyte)value;
                    break;
                case 8:
                    value = Util.Clamp(value, sbyte.MinValue, sbyte.MaxValue, ref clamped);
                    contains_id = (sbyte)value;
                    break;
                case 9:
                    value = Util.Clamp(value, sbyte.MinValue, sbyte.MaxValue, ref clamped);
                    contains_count = (sbyte)value;
                    break;
                case 10:
                    value = Util.Clamp(value, sbyte.MinValue, sbyte.MaxValue, ref clamped);
                    contains_prob = (sbyte)value;
                    break;
                /*case 11:
                    value = Utilites.Clamp(value, sbyte.MinValue, sbyte.MaxValue, ref clamped);
                    contains_type = (sbyte)value;
                    break;*/ //don't use doesn't update rferences
                case 12:
                    if (value == 0)
                        claw_sound = 255;
                    else
                        claw_sound = (byte)(value);
                    break;
                case 13:
                    value = Util.Clamp(value, short.MinValue, short.MaxValue, ref clamped);
                    score_value = (short)value;
                    break;
                case 14:
                    value = Util.Clamp(value, byte.MinValue, byte.MaxValue, ref clamped);
                    badass = (byte)value;
                    break;
                case 15:
                    value = Util.Clamp(value, byte.MinValue, byte.MaxValue, ref clamped);
                    energy_drain = (byte)value;
                    break;
                case 16:
                    value = Util.Clamp(value, int.MinValue, int.MaxValue, ref clamped);
                    lighting = Fix.FromRawValue(value);
                    break;
                case 17:
                    value = Util.Clamp(value, int.MinValue, int.MaxValue, ref clamped);
                    strength = Fix.FromRawValue(value);
                    break;
                case 18:
                    value = Util.Clamp(value, int.MinValue, int.MaxValue, ref clamped);
                    mass = Fix.FromRawValue(value);
                    break;
                case 19:
                    value = Util.Clamp(value, int.MinValue, int.MaxValue, ref clamped);
                    drag = Fix.FromRawValue(value);
                    break;
                case 20:
                    value = (int)(Math.Cos(value * Math.PI / 180.0D) * 65536.0);
                    value = Util.Clamp(value, int.MinValue, int.MaxValue, ref clamped);
                    field_of_view[curAI] = Fix.FromRawValue(value);
                    break;
                case 21:
                    value = Util.Clamp(value, int.MinValue, int.MaxValue, ref clamped);
                    firing_wait[curAI] = Fix.FromRawValue(value);
                    break;
                case 22:
                    value = Util.Clamp(value, int.MinValue, int.MaxValue, ref clamped);
                    firing_wait2[curAI] = Fix.FromRawValue(value);
                    break;
                case 23:
                    value = Util.Clamp(value, int.MinValue, int.MaxValue, ref clamped);
                    turn_time[curAI] = Fix.FromRawValue(value);
                    break;
                case 24:
                    value = Util.Clamp(value, int.MinValue, int.MaxValue, ref clamped);
                    max_speed[curAI] = Fix.FromRawValue(value);
                    break;
                case 25:
                    value = Util.Clamp(value, int.MinValue, int.MaxValue, ref clamped);
                    circle_distance[curAI] = Fix.FromRawValue(value);
                    break;
                case 26:
                    value = Util.Clamp(value, sbyte.MinValue, sbyte.MaxValue, ref clamped);
                    rapidfire_count[curAI] = (sbyte)value;
                    break;
                case 27:
                    value = Util.Clamp(value, sbyte.MinValue, sbyte.MaxValue, ref clamped);
                    evade_speed[curAI] = (sbyte)value;
                    break;
                case 30:
                    value = Util.Clamp(value, byte.MinValue, byte.MaxValue, ref clamped);
                    if (value == 0)
                        see_sound = 255;
                    else
                        see_sound = (byte)(value);
                    break;
                case 31:
                    value = Util.Clamp(value, byte.MinValue, byte.MaxValue, ref clamped);
                    if (value == 0)
                        attack_sound = 255;
                    else
                        attack_sound = (byte)(value);
                    break;
                case 33:
                    value = Util.Clamp(value, byte.MinValue, byte.MaxValue, ref clamped);
                    if (value == 0)
                        taunt_sound = 255;
                    else
                        taunt_sound = (byte)(value);
                    break;
                case 34:
                    value = Util.Clamp(value, byte.MinValue, byte.MaxValue, ref clamped);
                    if (value == 0)
                        deathroll_sound = 255;
                    else
                        deathroll_sound = (byte)(value);
                    break;
                case 36:
                    value = Util.Clamp(value, sbyte.MinValue, sbyte.MaxValue, ref clamped);
                    smart_blobs = (sbyte)value;
                    break;
                case 37:
                    value = Util.Clamp(value, sbyte.MinValue, sbyte.MaxValue, ref clamped);
                    energy_blobs = (sbyte)value;
                    break;
                case 38:
                    value = Util.Clamp(value, sbyte.MinValue, sbyte.MaxValue, ref clamped);
                    pursuit = (sbyte)value;
                    break;
                case 39:
                    value = Util.Clamp(value, sbyte.MinValue, sbyte.MaxValue, ref clamped);
                    lightcast = (sbyte)value;
                    break;
                case 40:
                    value = Util.Clamp(value, byte.MinValue, byte.MaxValue, ref clamped);
                    if (value == 0)
                        exp1_sound_num = 255;
                    else
                        exp1_sound_num = (byte)(value);
                    break;
                case 41:
                    value = Util.Clamp(value, byte.MinValue, byte.MaxValue, ref clamped);
                    if (value == 0)
                        exp2_sound_num = 255;
                    else
                        exp2_sound_num = (byte)(value);
                    break;
                case 43:
                    value = Util.Clamp(value, byte.MinValue, byte.MaxValue, ref clamped);
                    glow = (byte)value;
                    break;
                case 45:
                    value = Util.Clamp(value, byte.MinValue, byte.MaxValue, ref clamped);
                    aim = (byte)value;
                    break;
            }
            return clamped;
        }

        public void ClearAndUpdateDropReference(int v)
        {
            //[ISB] this doesn't really need to exist but may as well..
            contains_type = (sbyte)v;
            contains_id = 0;
        }
    }
}
