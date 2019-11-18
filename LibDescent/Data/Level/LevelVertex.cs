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

using System;
using System.Collections.Generic;

namespace LibDescent.Data
{
    public class LevelVertex
    {
        private FixVector location;

        public LevelVertex(Fix x, Fix y, Fix z)
        {
            location = new FixVector(x, y, z);
        }

        public List<Tuple<Segment, uint>> ConnectedSegments { get; } = new List<Tuple<Segment, uint>>();
        public List<Tuple<Side, uint>> ConnectedSides { get; } = new List<Tuple<Side, uint>>();
        public FixVector Location { get => location; set => location = value; }
        public double X { get => location.x; set => location.x = value; }
        public double Y { get => location.y; set => location.y = value; }
        public double Z { get => location.z; set => location.z = value; }

        public static implicit operator FixVector(LevelVertex v) => v.Location;
    }
}