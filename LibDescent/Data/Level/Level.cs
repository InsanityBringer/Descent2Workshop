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

using System.Collections.Generic;
using System.Linq;

namespace LibDescent.Data
{
    public interface ILevel
    {
        string LevelName { get; set; }
        List<LevelVertex> Vertices { get; }
        List<Segment> Segments { get; }
        List<LevelObject> Objects { get; }
        List<Wall> Walls { get; }
        List<ITrigger> Triggers { get; }
        List<Side> ReactorTriggerTargets { get; }
        List<MatCenter> MatCenters { get; }
    }

    public abstract partial class DescentLevelBase
    {
        public string LevelName { get; set; }

        public List<LevelVertex> Vertices { get; } = new List<LevelVertex>();

        public List<Segment> Segments { get; } = new List<Segment>();

        public List<LevelObject> Objects { get; } = new List<LevelObject>();

        public List<Wall> Walls { get; } = new List<Wall>();

        public const int MaxReactorTriggerTargets = 10;
        public List<Side> ReactorTriggerTargets { get; } = new List<Side>();

        public List<MatCenter> MatCenters { get; } = new List<MatCenter>();
    }

    public partial class D1Level : DescentLevelBase, ILevel
    {
        public List<D1Trigger> Triggers { get; } = new List<D1Trigger>();

        List<ITrigger> ILevel.Triggers => Triggers.ToList<ITrigger>();
    }

    public partial class D2Level : DescentLevelBase, ILevel
    {
        public Palette Palette { get; set; }

        /// <summary>
        /// The countdown time when the reactor is destroyed, on Insane difficulty.
        /// Lower difficulties use multiples of this value.
        /// </summary>
        public int BaseReactorCountdownTime { get; set; } = 30;

        /// <summary>
        /// How many "shields" the reactor has.
        /// Null causes Descent 2 to use the default strength, based on the level number.
        /// </summary>
        public int? ReactorStrength { get; set; } = null;

        public List<D2Trigger> Triggers { get; } = new List<D2Trigger>();

        List<ITrigger> ILevel.Triggers => Triggers.ToList<ITrigger>();

        public List<AnimatedLight> AnimatedLights { get; } = new List<AnimatedLight>();

        public Segment SecretReturnSegment { get; set; }

        public FixMatrix SecretReturnOrientation { get; set; }
    }
}
