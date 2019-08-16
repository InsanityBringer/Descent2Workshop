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

namespace PiggyDump.Editor
{
    public class VertMatcher
    {
        private Dictionary<string, LevelVertex> nearVerts;
        private Level level;

        public VertMatcher(Level level)
        {
            this.level = level;
            //Hey, we actually know how many things need to exist in the dictionary
            //We can actually make use of this!
            nearVerts = new Dictionary<string, LevelVertex>(level.Verts.Count);
        }

        public void BuildUniqueVerts()
        {
            string matchCode;
            foreach (LevelVertex vertex in level.Verts)
            {
                matchCode = vertex.GetMatchCode();
                if (!nearVerts.ContainsKey(matchCode))
                {
                    nearVerts.Add(matchCode, vertex);
                }
            }
        }

        public void RemapSegmentVerts(Segment segment, bool joinSides = false)
        {
            //For each side, clear the references in each vertex to each side
            Side side;
            LevelVertex sideVert;
            for (int i = 0; i < Segment.MaxSegmentSides; i++)
            {
                side = segment.sides[i];
                for (int v = 0; v < 4; v++)
                {
                    sideVert = segment.vertices[Segment.SideVerts[i, v]];
                    sideVert.connectedSides.Remove(side);
                }
            }

            //Now, get the unique vertices by generating the match codes for each vertex currently present
            //While we're at it, lets purge the references to the segments.
            //Should this automatically connect sides?
            //I guess I can implement that as a parameter. 
            string matchCode;
            LevelVertex vert;
            //foreach (LevelVertex vert in segment.vertices)
            for (int i = 0; i < Segment.MaxSegmentVerts; i++)
            {
                vert = segment.vertices[i];
                vert.connectedSegs.Remove(segment);
                matchCode = vert.GetMatchCode();
                vert = nearVerts[matchCode];
                //Set references in the new vertex
                vert.connectedSegs.Add(segment);
            }

            //Fix up the side references, now
            for (int i = 0; i < Segment.MaxSegmentSides; i++)
            {
                side = segment.sides[i];
                for (int v = 0; v < 4; v++)
                {
                    sideVert = segment.vertices[Segment.SideVerts[i, v]];
                    sideVert.connectedSides.Add(side);
                }
            }
        }
    }
}
