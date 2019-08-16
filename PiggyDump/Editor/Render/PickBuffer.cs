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
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace PiggyDump.Editor.Render
{
    public class PickBuffer
    {
        public const int MaxSelectedVerts = 1024 * 4;
        public const int MaxSelectedVertSize = 12 * MaxSelectedVerts; //48 KB of verts
        private int lastVertex = 0;
        private int selectBufferName, selectFaceBufferName, selectVAOName, selectFaceVAOName;

        public void Init()
        {
            selectVAOName = GL.GenVertexArray();
            GL.BindVertexArray(selectVAOName);
            selectBufferName = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, selectBufferName);
            GL.BufferData(BufferTarget.ArrayBuffer, MaxSelectedVertSize, (IntPtr)0, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            selectFaceVAOName = GL.GenVertexArray();
            GL.BindVertexArray(selectFaceVAOName); 
            selectFaceBufferName = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, selectFaceBufferName);
            GL.BufferData(BufferTarget.ArrayBuffer, MaxSelectedVertSize, (IntPtr)0, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        public void AddVertex(LevelVertex vert)
        {
            float[] data = { -vert.location.x / 65536.0f, vert.location.y / 65536.0f, vert.location.z / 65536.0f };
            GL.BindVertexArray(selectVAOName);
            GL.BindBuffer(BufferTarget.ArrayBuffer, selectBufferName);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(lastVertex * 3 * sizeof(float)), 3 * sizeof(float), data);
            lastVertex++;
        }

        public void RemoveVertAt(int id)
        {
            lastVertex--;
            if (lastVertex != 0)
            {
                GL.BindVertexArray(selectVAOName);
                GL.BindBuffer(BufferTarget.ArrayBuffer, selectBufferName);
                GL.CopyBufferSubData(BufferTarget.ArrayBuffer, BufferTarget.ArrayBuffer, (IntPtr)(lastVertex * 3 * sizeof(float)), (IntPtr)(id * 3 * sizeof(float)), 3 * sizeof(float));
                GLUtilities.ErrorCheck("Removing selected vertex");
            }
        }

        public void ClearVerts()
        {
            lastVertex = 0;
        }

        public void DrawSelectedPoints()
        {
            GL.BindVertexArray(selectVAOName);
            GL.BindBuffer(BufferTarget.ArrayBuffer, selectBufferName);
            GL.DrawArrays(PrimitiveType.Points, 0, lastVertex);
            GLUtilities.ErrorCheck("Drawing selected vertexes");
        }
    }
}
