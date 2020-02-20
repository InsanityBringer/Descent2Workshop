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

using System.Collections.Generic;
using System.IO;

namespace LibDescent.Data
{
    public interface ILevel
    {
        string LevelName { get; set; }
        List<LevelVertex> Vertices { get; }
        List<Segment> Segments { get; }
        List<LevelObject> Objects { get; }
        List<Wall> Walls { get; }
        IReadOnlyList<ITrigger> Triggers { get; }
        List<Side> ReactorTriggerTargets { get; }
        List<MatCenter> MatCenters { get; }
    }

    internal class DescentLevelCommon
    {
        public string LevelName { get; set; }

        public List<LevelVertex> Vertices { get; } = new List<LevelVertex>();

        public List<Segment> Segments { get; } = new List<Segment>();

        public List<LevelObject> Objects { get; } = new List<LevelObject>();

        public List<Wall> Walls { get; } = new List<Wall>();

        public const int MaxReactorTriggerTargets = 10;
        public List<Side> ReactorTriggerTargets { get; } = new List<Side>(MaxReactorTriggerTargets);

        public List<MatCenter> MatCenters { get; } = new List<MatCenter>();
    }

    public class D1Level : ILevel
    {
        private DescentLevelCommon _commonData = new DescentLevelCommon();

        public List<D1Trigger> Triggers { get; } = new List<D1Trigger>();

        #region ILevel implementation
        public string LevelName { get => _commonData.LevelName; set => _commonData.LevelName = value; }
        public List<LevelVertex> Vertices => _commonData.Vertices;
        public List<Segment> Segments => _commonData.Segments;
        public List<LevelObject> Objects => _commonData.Objects;
        public List<Wall> Walls => _commonData.Walls;
        IReadOnlyList<ITrigger> ILevel.Triggers => Triggers;
        public List<Side> ReactorTriggerTargets => _commonData.ReactorTriggerTargets;
        public List<MatCenter> MatCenters => _commonData.MatCenters;
        #endregion

        public static D1Level CreateFromStream(Stream stream)
        {
            return new D1LevelLoader(stream).Load();
        }

        public void WriteToStream(Stream stream)
        {
            new D1LevelWriter(this, stream).Write();
        }
    }

    public class D2Level : ILevel
    {
        private DescentLevelCommon _commonData = new DescentLevelCommon();

        public List<D2Trigger> Triggers { get; } = new List<D2Trigger>();

        public string PaletteName { get; set; }

        public const int DefaultBaseReactorCountdownTime = 30;

        /// <summary>
        /// The countdown time when the reactor is destroyed, on Insane difficulty.
        /// Lower difficulties use multiples of this value.
        /// </summary>
        public int BaseReactorCountdownTime { get; set; } = DefaultBaseReactorCountdownTime;

        /// <summary>
        /// How many "shields" the reactor has.
        /// Null causes Descent 2 to use the default strength, based on the level number.
        /// </summary>
        public int? ReactorStrength { get; set; } = null;

        public List<DynamicLight> DynamicLights { get; } = new List<DynamicLight>();

        public List<AnimatedLight> AnimatedLights { get; } = new List<AnimatedLight>();

        public Segment SecretReturnSegment { get; set; }

        public FixMatrix SecretReturnOrientation { get; set; }

        #region ILevel implementation
        public string LevelName { get => _commonData.LevelName; set => _commonData.LevelName = value; }
        public List<LevelVertex> Vertices => _commonData.Vertices;
        public List<Segment> Segments => _commonData.Segments;
        public List<LevelObject> Objects => _commonData.Objects;
        public List<Wall> Walls => _commonData.Walls;
        IReadOnlyList<ITrigger> ILevel.Triggers => Triggers;
        public List<Side> ReactorTriggerTargets => _commonData.ReactorTriggerTargets;
        public List<MatCenter> MatCenters => _commonData.MatCenters;
        #endregion

        public static D2Level CreateFromStream(Stream stream)
        {
            return new D2LevelLoader(stream).Load();
        }

        public void WriteToStream(Stream stream)
        {
            new D2LevelWriter(this, stream, false).Write();
        }
    }

    public static partial class Extensions
    {
        // It's not clear why .NET doesn't define this already, but it doesn't.
        // Remove if that changes.
        public static int IndexOf<T>(this IReadOnlyList<T> list, T obj)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(obj))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
