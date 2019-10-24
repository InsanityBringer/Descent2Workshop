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
    public enum TransformType
    {
        None,
        Translate,
        Rotate,
        Scale,
    }
    //this class is bad and i should feel bad for writing it
    public class LevelTransform
    {
        private TransformType type = TransformType.None;
        private Vector3 xAxis, yAxis;
        private float xAmount, yAmount;
        //Mouse coordinates for implementing this transform. 
        private int startx, starty;
        private Level level;

        public TransformType Type { get { return type; } }

        //Information about the current transofmr
        HashSet<Segment> segments = new HashSet<Segment>();
        List<LevelVertex> operatedVerts;

        public LevelTransform(Level level)
        {
            this.level = level;
        }

        /// <summary>
        /// From the current set of selected vertices, generate the geometry 
        /// </summary>
        /// <param name="selectedVertices"></param>
        public void InitTransform(List<LevelVertex> selectedVertices)
        {
            foreach (LevelVertex vert in selectedVertices)
            {
                foreach (Segment seg in vert.connectedSegs)
                {
                    segments.Add(seg);
                }
            }
            operatedVerts = selectedVertices;
            //TODO: Each seg gets its own 8 verts for rendering the shadow. This can be optimized with more involved code. 
            int numVerts = segments.Count * 8;
            int lastVertex = 0;
            int lastIndex = 0;
            int lastSeg = 0;
            float[] vertBuffer = new float[numVerts * 4];
            int[] indexBuffer = new int[segments.Count * Segment.MaxSegmentSides * 5];
            foreach (Segment seg in segments)
            {
                foreach (LevelVertex vert in seg.vertices)
                {
                    vertBuffer[lastVertex * 4 + 0] = -vert.location.x / 65536.0f;
                    vertBuffer[lastVertex * 4 + 1] = vert.location.y / 65536.0f;
                    vertBuffer[lastVertex * 4 + 2] = vert.location.z / 65536.0f;
                    if (vert.selected)
                        vertBuffer[lastVertex * 4 + 3] = 1.0f;
                    else
                        vertBuffer[lastVertex * 4 + 3] = 0.0f;
                    lastVertex++;
                }
                for (int i = 0; i < Segment.MaxSegmentSides; i++)
                {
                    indexBuffer[lastIndex++] = Segment.SideVerts[i, 0] + (lastSeg * Segment.MaxSegmentVerts);
                    indexBuffer[lastIndex++] = Segment.SideVerts[i, 1] + (lastSeg * Segment.MaxSegmentVerts);
                    indexBuffer[lastIndex++] = Segment.SideVerts[i, 2] + (lastSeg * Segment.MaxSegmentVerts);
                    indexBuffer[lastIndex++] = Segment.SideVerts[i, 3] + (lastSeg * Segment.MaxSegmentVerts);
                    indexBuffer[lastIndex++] = 32767;
                }
                lastSeg++;
            }
            //sharedState.InitTransformBuffer(vertBuffer, indexBuffer);
        }

        public void InitTranslation(Vector3 xAxis, Vector3 yAxis, int mouseX, int mouseY)
        {
            type = TransformType.Translate;
            this.xAxis = xAxis; this.yAxis = yAxis;
            startx = mouseX; starty = mouseY;
        }

        public void MouseMove(int mouseX, int mouseY)
        {
            float diffx = mouseX - startx;
            float diffy = mouseY - starty;
            Console.WriteLine("{0} {1}", diffx, diffy);
            if (type == TransformType.Translate)
            {
                xAmount = diffx * .5f;
                yAmount = -diffy * .5f;
                Vector3 translateX = xAxis * xAmount;
                Vector3 translateY = yAxis * yAmount;
                //sharedState.SetShadowTranslation(translateX, translateY);
            }
        }

        public void FinalizeTransform()
        {
            if (type == TransformType.Translate)
            {
                Vector3 translateX = xAxis * xAmount;
                Vector3 translateY = yAxis * yAmount;
                FixVector xVecFix = new FixVector((int)(-translateX.X * 65536), (int)(translateX.Y * 65536), (int)(translateX.Z * 65536));
                FixVector yVecFix = new FixVector((int)(-translateY.X * 65536), (int)(translateY.Y * 65536), (int)(translateY.Z * 65536));
                foreach (LevelVertex vert in operatedVerts)
                {
                    vert.location.Add(xVecFix);
                    vert.location.Add(yVecFix);
                }
                //sharedState.MarkSegmentsDirty(segments);
                segments.Clear();
            }

            type = TransformType.None;
        }
    }
}
