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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibDescent.Data;
using OpenTK;

namespace Descent2Workshop.Editor
{
    public enum SegSide
    {
        Left,
        Up,
        Right,
        Down,
        Back,
        Front,
    }

    public enum SideType
    {
        Unknown,
        Quad,
        Triangle02,
        Triangle13,
    }
    public class Side
    {
        //Quake-esque texture projection
        public static FixVector[] texturePlanes = {
            new FixVector(65536, 0, 0), new FixVector(0, 65536, 0), new FixVector(0, 0, -65536), //Left
            new FixVector(0, 0, -65536), new FixVector(65536, 0, 0), new FixVector(0, -65536, 0), //Ceiling
            new FixVector(-65536, 0, 0), new FixVector(0, 65536, 0), new FixVector(0, 0, -65536), //Right
            new FixVector(0, 0, 65536), new FixVector(65536, 0, 0), new FixVector(0, -65536, 0), //Floor
            new FixVector(0, -65536, 0), new FixVector(65536, 0, 0), new FixVector(0, 0, -65536), //Back
            new FixVector(0, 65536, 0), new FixVector(65536, 0, 0), new FixVector(0, 0, -65536), }; //Front

        public SideType type;
        public SegSide sideNum;
        public byte pad;
        public short wallNum;
        public Wall wall;
        public short tmapNum;
        public short tmapNum2;
        public FixVector[] uvls = new FixVector[4];
        public FixVector[] normals = new FixVector[2];
        public bool selected = false;
        public bool exit = false;
        private Segment parent;

        public Side(Segment parent, SegSide side)
        {
            this.parent = parent;
            sideNum = side;
        }

        //Decide how a side is supposed to be triangulated
        //Let's use the formula Matt came up with
        //Matt's formula: Na . AD > 0, where ABCD are vertices on side, a is face formed by A,B,C, Na is normal from face a.
        public void SetType()
        {
            FixVector a = parent.vertices[Segment.SideVerts[(int)sideNum, 0]].location;
            FixVector b = parent.vertices[Segment.SideVerts[(int)sideNum, 1]].location;
            FixVector c = parent.vertices[Segment.SideVerts[(int)sideNum, 2]].location;
            FixVector d = parent.vertices[Segment.SideVerts[(int)sideNum, 3]].location;
            FixVector normal = GetFaceNormal(a, b, c);
            //It's actually BD according to the source code? 
            //Please don't sue me parallax for referencing one line of your code
            FixVector AD = d - b;
            double dot = normal.Dot(AD);
            if (Math.Abs(dot) < 0.0001) //planar?
                type = SideType.Quad;
            else if (dot > 0)
                type = SideType.Triangle02;
            else
                type = SideType.Triangle13;
        }

        public FixVector GetFaceNormal(FixVector a, FixVector b, FixVector c)
        {
            FixVector v1 = b - a;
            FixVector v2 = c - a;
            return v1.Cross(v2);
        }

        public Vector2[] GetFlattenedFace()
        {
            Vector2[] points = new Vector2[4];

            return points;
        }

        public void ProjectTexture()
        {
        }
    }
    public class Segment
    {
        public const int MaxSegmentSides = 6;
        public const int MaxSegmentVerts = 8;
        public static int[,] SideVerts = { { 7, 6, 2, 3 }, { 0, 4, 7, 3 }, { 0, 1, 5, 4 }, { 2, 6, 5, 1 }, { 4, 5, 6, 7 }, { 3, 2, 1, 0 } };
            
        public Side[] sides = new Side[MaxSegmentSides];
        public short[] childrenIDs = new short[MaxSegmentSides];
        public Segment[] children = new Segment[MaxSegmentSides];
        public short[] verts = new short[MaxSegmentVerts];
        public LevelVertex[] vertices = new LevelVertex[MaxSegmentVerts];
        public int objPointer; //TODO: probably not needed here

        public byte special;
        public byte matcenNum;
        public byte value;
        public byte flags;
        public int staticLight;

        public Segment()
        {
        }

        public static Segment MakeDefaultSegment(LevelVertex[] vertices)
        {
            Segment seg = new Segment();
            Side side;
            for (int i = 0; i < MaxSegmentVerts; i++)
            {
                seg.vertices[i] = vertices[i];
            }
            for (int i = 0; i < MaxSegmentSides; i++)
            {
                seg.sides[i] = new Side(seg, (SegSide)i);
                side = seg.sides[i];
                if (i == (int)SegSide.Up)
                {
                    side.tmapNum = 3;
                }
                else if (i == (int)SegSide.Down)
                {
                    side.tmapNum = 2;
                }
                else
                {
                    side.tmapNum = 1;
                }

            }
            return seg;
        }
    }
}
