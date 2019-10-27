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
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Descent2Workshop.Editor.Render
{
    public class MineRender : IInputEventHandler
    {
        private EditorState state;
        private LevelData levelData;
        //private PickBuffer pickBuffer = new PickBuffer();
        private Camera camera;
        private GLControl host;
        private bool orbiting, translating;
        private int lastX, lastY;

        public Camera ViewCamera { get { return camera; } }

        public EditorState State { get => state; set => state = value; }
        public LevelData LevelData { get => levelData; set => levelData = value; }

        public MineRender(EditorState state, GLControl host)
        {
            this.state = state;
            camera = new Camera();
            this.host = host;
            levelData = new LevelData(this);
        }

        public void Init()
        {
            GL.Enable(EnableCap.DepthTest);
            Console.WriteLine("Renderer init");
            GL.Enable(EnableCap.PrimitiveRestart);
            GL.PrimitiveRestartIndex(32767);
            //pickBuffer.Init();
            levelData.Init();
            //transformBuffer.Init();
        }

        public void MakeOrthoCamera(float scale, float aspect, int mode)
        {
            camera.MakeOrthoCamera(scale, aspect);
            if (mode == 0)
                camera.Rotate((float)(Math.PI), 0);
            else if (mode == 1)
                camera.Rotate((float)Math.PI, (float)(Math.PI / 2));
            else if (mode == 2)
                camera.Rotate((float)(Math.PI / 2), 0);
        }

        public void MakePerpectiveCamera(float fov, float aspect)
        {
            camera.MakePerpectiveCamera(fov, aspect);
        }

        private void OrbitCamera(int dx, int dy)
        {
            float ang = (float)(dx * .25 * Math.PI / 180.0);
            float pitch = (float)(dy * .25 * Math.PI / 180.0);
            camera.Rotate(-ang, -pitch);
        }

        private void TranslateCamera(int dx, int dy)
        {
            camera.Translate(new Vector3(-dx / 5.0f, dy / 5.0f, 0.0f));
        }

        public void MakeHostCurrent()
        {
            host.MakeCurrent();
        }

        public void UpdateWorld()
        {
            UpdateFlags flags = state.updateFlags;
            if ((flags & UpdateFlags.World) != 0)
            {
                levelData.BuildWorld();
                levelData.BuildWorldOutline();

                //Selected verts needs to be rebuilt if the world changed
                levelData.pickBuffer.Generate(state.SelectedVertices);
            }
            else
            {
                if ((flags & UpdateFlags.Selected) != 0)
                    levelData.pickBuffer.Generate(state.SelectedVertices);
            }

            if ((flags & UpdateFlags.Shadow) != 0)
            {
                levelData.transformBuffer.Generate(state.shadow);
            }
        }

        public void DrawWorld()
        {
            levelData.DrawLevel(camera, true, true, true);
        }

        public bool HandleEvent(InputEvent ev)
        {
            int deltaX = 0; int deltaY = 0;
            if (ev.type == EventType.MouseButton || ev.type == EventType.MouseMove)
            {
                deltaX = lastX - ev.x; deltaY = lastY - ev.y;
                lastX = ev.x; lastY = ev.y;
            }

            if (ev.type == EventType.MouseButton)
            {
                if (ev.mouseButton == MouseButtons.Right)
                {
                    orbiting = ev.down;
                    return true;
                }
                else if (ev.mouseButton == MouseButtons.Middle)
                {
                    translating = ev.down;
                    return true;
                }
                else if (ev.mouseButton == MouseButtons.Left && ev.down)
                {
                    float xLocal = ((float)ev.x / ev.w) * 2 - 1f;
                    float yLocal = ((float)ev.y / ev.h) * 2 - 1f;
                    PickVertex(xLocal, yLocal);
                    return true;
                }
            }
            else if (ev.type == EventType.MouseWheel)
            {
                camera.Dolly(ev.wheelDelta * -4);
                host.Invalidate();
                return true;
            }
            else if (ev.type == EventType.MouseMove)
            {
                if (orbiting)
                {
                    OrbitCamera(deltaX, deltaY);
                    host.Invalidate();
                    return true;
                }
                else if (translating)
                {
                    TranslateCamera(deltaX, deltaY);
                    host.Invalidate();
                    return true;
                }
            }
            return false;
        }

        public int PickVertex(float testx, float testy)
        {
            float[] verts = levelData.VertBuffer;
            int numVerts = verts.Length / 4;
            Vector3 vec;
            List<Vector3> pts = new List<Vector3>();
            float bestZ = 10000.0f;
            int bestID = 0;
            Vector3 bestVec = new Vector3(0, 0, 0);
            for (int i = 0; i < numVerts; i++)
            {
                vec.X = -verts[i * 4 + 0];
                vec.Y = verts[i * 4 + 1];
                vec.Z = verts[i * 4 + 2];
                if (camera.CameraFacingPoint(vec))
                {
                    //Console.WriteLine("point in front of camera");
                    Vector4 projPoint = camera.TransformPoint(vec);
                    projPoint.Xyz /= projPoint.W;
                    if (Math.Abs(projPoint.X - testx) < 0.05f && Math.Abs(projPoint.Y - -testy) < 0.05f)
                    {
                        if (projPoint.Z < bestZ)
                        {
                            bestZ = projPoint.Z;
                            bestVec = vec;
                            bestID = i;
                        }
                    }
                }
            }
            if (bestZ < 9000.0f)
            {
                pts.Add(bestVec);
                state.ToggleSelectedVert(state.EditorLevel.Verts[bestID]);
            }
            return -1;
        }
    }
}
