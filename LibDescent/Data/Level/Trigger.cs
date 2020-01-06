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

namespace LibDescent.Data
{
    public enum TriggerType
    {
        OpenDoor,
        CloseDoor,
        Matcen,
        Exit,
        SecretExit,
        IllusionOff,
        IllusionOn,
        UnlockDoor,
        LockDoor,
        OpenWall,
        CloseWall,
        IllusionWall,
        LightOff,
        LightOn,
    }

    [Flags]
    public enum D1TriggerFlags
    {
        ControlDoors = 0x1,
        ShieldDrain = 0x2,
        EnergyDrain = 0x4,
        Exit = 0x8,
        Enabled = 0x10,
        OneShot = 0x20,
        MatCenter = 0x40,
        IllusionOff = 0x80,
        SecretExit = 0x100,
        IllusionOn = 0x200,
#if false
        OpenWall = 0x400, // D2X-XL only
        CloseWall = 0x800, // D2X-XL only
        MakeIllusionary = 0x1000, // D2X-XL only
#endif
    }

    [Flags]
    public enum D2TriggerFlags
    {
        NoMessage = 0x1,
        OneShot = 0x2,
        Disabled = 0x4, // D2 sets this on a one-shot trigger when activated
#if false
        Permanent = 0x8, // D2X-XL only; control panel not destroyed when activated
        Alternate = 0x10, // D2X-XL only; trigger type inverts between activations
        SetOrient = 0x20, // D2X-XL only
        Silent = 0x40, // D2X-XL only
        AutoPlay = 0x80, // D2X-XL only
#endif
    }

    public interface ITrigger
    {
        TriggerType Type { get; set; }
        List<Side> Targets { get; }
        ValueType Value { get; set; }
    }

    /// <summary>
    /// A specialization of ITrigger that is agnostic regarding whether it is for D1 or D2
    /// (since the BLX format does not carry that information).
    /// </summary>
    public class BlockTrigger : ITrigger
    {
        private int value;

        public TriggerType Type { get; set; }

        public List<Side> Targets { get; } = new List<Side>();

        public ValueType Value { get => value; set => this.value = (int)value; }

        public ushort Flags { get; set; }

        public int Time { get; set; }
    }

    public class D1Trigger : ITrigger
    {
        public const int MaxWallsPerLink = 10;

        private Fix fixValue;

        public TriggerType Type { get; set; }

        public List<Side> Targets { get; } = new List<Side>();

        public ValueType Value { get => fixValue; set => fixValue = (Fix)value; }

        public D1TriggerFlags Flags { get; set; }

        public int Time { get; set; }
    }

    public class D2Trigger : ITrigger
    {
        public const int MaxWallsPerLink = 10;

        private Fix fixValue;
        private int intValue;

        public TriggerType Type { get; set; }

        public List<Side> Targets { get; } = new List<Side>();

        public ValueType Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public D2TriggerFlags Flags { get; set; }
    }
}