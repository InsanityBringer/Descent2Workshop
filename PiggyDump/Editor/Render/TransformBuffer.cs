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
using OpenTK.Graphics.OpenGL;

namespace Descent2Workshop.Editor.Render
{
    public class TransformBuffer
    {
        public const int MaxTransformedVerts = 1024 * 4;
        public const int MaxTransformedVertSize = 16 * MaxTransformedVerts; //64 KB of verts
        private int numVerts = 0;
        private int numLines = 0;
        private int transformBufferName, transformElemBufferName, transformVAOName;

        public void Init()
        {
            transformVAOName = GL.GenVertexArray();
            GL.BindVertexArray(transformVAOName);
            transformBufferName = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, transformBufferName);
            transformElemBufferName = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, transformElemBufferName);
            GL.BufferData(BufferTarget.ArrayBuffer, MaxTransformedVertSize, (IntPtr)0, BufferUsageHint.DynamicDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, MaxTransformedVertSize, (IntPtr)0, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(0); //position
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
            GL.EnableVertexAttribArray(1); //weight
            GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, sizeof(float) * 4, sizeof(float) * 3);
        }

        public void Generate(List<LevelVertex> selectedVerts)
        {
            Clear();
            HashSet<Segment> segments = new HashSet<Segment>();
            foreach (LevelVertex vert in selectedVerts)
            {
                foreach (Segment seg in vert.connectedSegs)
                {
                    segments.Add(seg);
                }
            }
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
            Fill(vertBuffer, indexBuffer);
        }

        public void Fill(float[] vertbuffer, int[] indexbuffer)
        {
            GL.BindVertexArray(transformVAOName);
            GL.BindBuffer(BufferTarget.ArrayBuffer, transformBufferName);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, transformElemBufferName);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, vertbuffer.Length * sizeof(float), vertbuffer);
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)0, indexbuffer.Length * sizeof(int), indexbuffer);
            numVerts = vertbuffer.Length / 4;
            numLines = indexbuffer.Length;
        }

        public void Clear()
        {
            numVerts = numLines = 0;
        }

        public void DrawTransform()
        {
            if (numVerts == 0) return;
            GL.BindVertexArray(transformVAOName);
            GL.BindBuffer(BufferTarget.ArrayBuffer, transformBufferName);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, transformElemBufferName);
            GL.DrawArrays(PrimitiveType.Points, 0, numVerts);
            GLUtilities.ErrorCheck("Drawing transformed vertexes");
            GL.DrawElements(PrimitiveType.LineLoop, numLines, DrawElementsType.UnsignedInt, 0);
            GLUtilities.ErrorCheck("Drawing transformed faces");
        }
    }
}
