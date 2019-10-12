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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibDescent.Data;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Descent2Workshop
{
    public partial class PolymodelPreviewer : Form
    {
        public StandardUI host;
        public Polymodel model;
        public bool glContextCreated = false;
        public ModelTextureManager texMan = new ModelTextureManager();

        private OpenTK.GLControl glControl1;
        private List<int> TextureList = new List<int>();
        private ModelRenderer renderer;

        public PolymodelPreviewer(Polymodel model, StandardUI host)
        {
            InitializeComponent();
            //moo
            this.glControl1 = new GLControl();
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(63, 12);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(384, 384);
            this.glControl1.TabIndex = 3;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);

            this.Controls.Add(glControl1);
            this.PerformLayout();

            this.model = model;
            if (this.model.isAnimated)
            {
                numericUpDown1.Minimum = 0;
                numericUpDown1.Maximum = Robot.NUM_ANIMATION_STATES - 1;
            }
            else
            {
                numericUpDown1.Enabled = false;
                chkAnimation.Enabled = false;
            }
            this.host = host;
            this.renderer = new ModelRenderer(host.DefaultPigFile);
        }

        public double GetFloatFromFixed(int fixedvalue)
        {
            return (double)fixedvalue / 65536D;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SetupViewport();
            glControl1.Invalidate();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            glContextCreated = true;
            renderer.Init();
            renderer.SetModel(model);
            SetupViewport();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!glContextCreated)
                return; //can't do anything with this, heh

            glControl1.MakeCurrent();
            renderer.Pitch = (trackBar2.Value - 8) * -22.5d;
            renderer.Angle = (trackBar1.Value - 8) * -22.5d;
            renderer.ShowBBs = chkShowBBs.Checked;
            renderer.ShowNormals = chkNorm.Checked;
            renderer.Wireframe = chkWireframe.Checked;
            renderer.ShowRadius = chkRadius.Checked;
            if (chkAnimation.Checked)
                renderer.Frame = (int)numericUpDown1.Value;
            else
                renderer.Frame = -1;

            renderer.Draw();

            /*GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.CullFace(CullFaceMode.Front);
            //glControl1.SwapBuffers();
            if (chkWireframe.Checked)
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            else
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }

            GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadMatrix(
            GL.LoadIdentity();
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            GL.Scale(-1.0, 1.0, 1.0);
            GL.Rotate((double)((16-trackBar2.Value) * 22.5d) - 180d, 1.0, 0.0, 0.0);
            GL.Rotate((double)(trackBar1.Value * 22.5d) - 180d, 0.0, 1.0, 0.0);

            Polymodel mainmodel = model;
            PolymodelData modeldata = mainmodel.data;
            DrawSubobject(mainmodel, model.submodels[0]);*/

            /*if (chkRadius.Checked)
            {
                GL.LoadIdentity();
                GL.Disable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Texture2D);

                //Draw the radius
                double radius = model.rad / 65536.0;
                double minx = model.mins.x / 65536.0;
                double miny = model.mins.y / 65536.0;
                double minz = model.mins.z / 65536.0;
                double maxx = model.maxs.x / 65536.0;
                double maxy = model.maxs.y / 65536.0;
                double maxz = model.maxs.z / 65536.0;
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color3(0.0, 0.0, 1.0);
                double radx, rady, radang;
                for (int i = 0; i < 32; i++)
                {
                    radang = (i / 16.0) * (Math.PI);
                    radx = Math.Cos(radang);
                    rady = Math.Sin(radang);
                    GL.Vertex3(radx * radius, rady * radius, 0);
                }
                GL.End();

                //Draw the bounding box
                GL.Rotate((double)((16 - trackBar2.Value) * 22.5d) - 180d, 1.0, 0.0, 0.0);
                GL.Rotate((double)(trackBar1.Value * 22.5d) - 180d, 0.0, 1.0, 0.0);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color3(1.0, 0.0, 0.0);

                GL.Vertex3(minx, miny, maxz);
                GL.Vertex3(minx, maxy, maxz);
                GL.Vertex3(maxx, maxy, maxz);
                GL.Vertex3(maxx, miny, maxz);

                GL.End();

                GL.Begin(PrimitiveType.LineLoop);
                GL.Color3(1.0, 0.0, 0.0);

                GL.Vertex3(minx, miny, minz);
                GL.Vertex3(minx, maxy, minz);
                GL.Vertex3(maxx, maxy, minz);
                GL.Vertex3(maxx, miny, minz);

                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Color3(1.0, 0.0, 0.0);

                GL.Vertex3(minx, miny, minz);
                GL.Vertex3(minx, miny, maxz);
                GL.Vertex3(minx, maxy, minz);
                GL.Vertex3(minx, maxy, maxz);
                GL.Vertex3(maxx, maxy, minz);
                GL.Vertex3(maxx, maxy, maxz);
                GL.Vertex3(maxx, miny, minz);
                GL.Vertex3(maxx, miny, maxz);

                GL.End();
            }*/
            glControl1.SwapBuffers();
        }

        /*private void DrawSubobject(Polymodel mainModel, Submodel model)
        {
            int lastBoundTexture = -1;

            GL.PushMatrix();
            GL.Translate(model.Offset.x / 65536.0f, model.Offset.y / 65536.0f, model.Offset.z / 65536.0f);
            if (chkAnimation.Checked && mainModel.isAnimated)
            {
                int frame = (int)numericUpDown1.Value;
                GL.Rotate((mainModel.animationMatrix[model.ID, frame].b / 16384.0f) * 90.0, 0.0, 0.0, 1.0);
                GL.Rotate((mainModel.animationMatrix[model.ID, frame].h / 16384.0f) * 90.0, 0.0, 1.0, 0.0);
                GL.Rotate((mainModel.animationMatrix[model.ID, frame].p / 16384.0f) * 90.0, 1.0, 0.0, 0.0);
            }
            foreach (Submodel child in model.Children)
            {
                DrawSubobject(mainModel, child);
            }
            double shade = 1.0;
            Vector3 light = new Vector3(0.5f, 0.5f, 0.0f);
            light.Normalize();
            Vector3 norm;
            for (int x = 0; x < model.faces.Count; x++)
            {
                PolymodelFace face = model.faces[x];
                norm = new Vector3(face.Normal.x / 65536.0f, face.Normal.y / 65536.0f, face.Normal.z / 65536.0f);
                shade = Vector3.Dot(light, norm) * .25f + .75f;
                if (!face.isTextured)
                {
                    GL.Disable(EnableCap.Texture2D);
                    float cr = (float)face.cr / 255f;
                    float cg = (float)face.cg / 255f;
                    float cb = (float)face.cb / 255f;
                    GL.Color3(cr * shade, cg * shade, cb * shade);

                    //GL.Begin(BeginMode.TriangleFan);
                    GL.Begin(PrimitiveType.TriangleFan);
                    for (int i = 0; i < face.points.Length; i++)
                    {
                        double vx = GetFloatFromFixed(face.points[i].x);
                        double vy = GetFloatFromFixed(face.points[i].y);
                        double vz = GetFloatFromFixed(face.points[i].z);

                        GL.Vertex3(vx, vy, vz);
                    }
                    GL.End();
                    if (chkNorm.Checked)
                    {
                        GL.Color3(1.0f, 1.0f, 1.0f);
                        GL.Begin(PrimitiveType.Lines);
                        {
                            double vx = GetFloatFromFixed(face.points[0].x);
                            double vy = GetFloatFromFixed(face.points[0].y);
                            double vz = GetFloatFromFixed(face.points[0].z);

                            double dx = GetFloatFromFixed(face.points[0].x + face.Normal.x);
                            double dy = GetFloatFromFixed(face.points[0].y + face.Normal.y);
                            double dz = GetFloatFromFixed(face.points[0].z + face.Normal.z);

                            GL.Vertex3(vx, vy, vz);
                            GL.Vertex3(dx, dy, dz);
                        }
                        GL.End();
                    }
                }
                else
                {
                    GL.Enable(EnableCap.Texture2D);
                    GL.Color3(shade, shade, shade);

                    int textureID = face.textureID;
                    //string texturename = mainmodel.textureList[textureID];
                    if (textureID != lastBoundTexture)
                    {
                        GL.BindTexture(TextureTarget.Texture2D, TextureList[textureID]);
                    }
                    lastBoundTexture = textureID;

                    GL.Begin(PrimitiveType.TriangleFan);
                    for (int i = 0; i < face.points.Length; i++)
                    {
                        double u = GetFloatFromFixed(face.UVLCoords[i].x);
                        double v = GetFloatFromFixed(face.UVLCoords[i].y);

                        double vx = GetFloatFromFixed(face.points[i].x);
                        double vy = GetFloatFromFixed(face.points[i].y);
                        double vz = GetFloatFromFixed(face.points[i].z);

                        GL.TexCoord2(u, v); GL.Vertex3(vx, vy, vz);
                    }
                    GL.End();
                    if (chkNorm.Checked)
                    {
                        GL.Disable(EnableCap.Texture2D);
                        GL.Begin(PrimitiveType.Lines);
                        {
                            double vx = GetFloatFromFixed(face.points[0].x);
                            double vy = GetFloatFromFixed(face.points[0].y);
                            double vz = GetFloatFromFixed(face.points[0].z);

                            double dx = GetFloatFromFixed(face.points[0].x + face.Normal.x);
                            double dy = GetFloatFromFixed(face.points[0].y + face.Normal.y);
                            double dz = GetFloatFromFixed(face.points[0].z + face.Normal.z);

                            GL.Vertex3(vx, vy, vz);
                            GL.Vertex3(dx, dy, dz);
                        }
                        GL.End();
                        GL.Begin(PrimitiveType.Lines);
                        {
                            double vx = GetFloatFromFixed(face.points[2].x);
                            double vy = GetFloatFromFixed(face.points[2].y);
                            double vz = GetFloatFromFixed(face.points[2].z);

                            double dx = GetFloatFromFixed((face.FaceVector.x));
                            double dy = GetFloatFromFixed((face.FaceVector.y));
                            double dz = GetFloatFromFixed((face.FaceVector.z));

                            GL.Color3(0.0f, 0.0f, 1.0f);
                            GL.Vertex3(vx, vy, vz);
                            GL.Color3(1.0f, 0.0f, 0.0f);
                            GL.Vertex3(dx, dy, dz);
                        }
                        GL.End();
                        GL.Enable(EnableCap.Texture2D);
                    }
                }
            }
            GL.LineWidth(2);
            GL.PointSize(3);
            //GL.Disable(EnableCap.Texture2D);
            if (chkNorm.Checked)
            {
                double nx = GetFloatFromFixed(model.Normal.x);
                double ny = GetFloatFromFixed(model.Normal.y);
                double nz = GetFloatFromFixed(model.Normal.z);

                double px = GetFloatFromFixed(model.Point.x);
                double py = GetFloatFromFixed(model.Point.y);
                double pz = GetFloatFromFixed(model.Point.z);

                //Points are globally valid, so undo the translation
                GL.PushMatrix();
                GL.LoadIdentity();
                GL.Rotate((double)((16 - trackBar2.Value) * 22.5d) - 180d, 1.0, 0.0, 0.0);
                GL.Rotate((double)(trackBar1.Value * 22.5d) - 180d, 0.0, 1.0, 0.0);
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(0.0f, 1.0f, 0.0f);
                GL.Vertex3(px, py, pz);
                GL.Vertex3(px + nx, py + ny, pz + nz);
                GL.End();
                GL.PopMatrix();

                GL.Begin(PrimitiveType.Points);
                GL.Color3(1.0f, 1.0f, 0.0f);
                GL.Vertex3(0, 0, 0);
                GL.End();
            }
            GL.LineWidth(1);
            if (chkShowBBs.Checked)
            {
                GL.Disable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Texture2D);

                //Draw the radius
                double radius = model.Radius / 65536.0;
                double minx = model.Mins.x / 65536.0;
                double miny = model.Mins.y / 65536.0;
                double minz = model.Mins.z / 65536.0;
                double maxx = model.Maxs.x / 65536.0;
                double maxy = model.Maxs.y / 65536.0;
                double maxz = model.Maxs.z / 65536.0;
                //Follow the chain
                GL.PushMatrix();
                GL.Rotate((double)(-((16 - trackBar2.Value) * 22.5d) - 180d), 1.0, 0.0, 0.0);
                GL.Rotate((double)(-(trackBar1.Value * 22.5d) - 180d), 0.0, 1.0, 0.0);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color3(0.0, 0.0, 1.0);
                double radx, rady, radang;
                for (int i = 0; i < 32; i++)
                {
                    radang = (i / 16.0) * (Math.PI);
                    radx = Math.Cos(radang);
                    rady = Math.Sin(radang);
                    GL.Vertex3(radx * radius, rady * radius, 0);
                }
                GL.End();
                GL.PopMatrix();

                //Draw the bounding box
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color3(1.0, 0.0, 0.0);

                GL.Vertex3(minx, miny, maxz);
                GL.Vertex3(minx, maxy, maxz);
                GL.Vertex3(maxx, maxy, maxz);
                GL.Vertex3(maxx, miny, maxz);

                GL.End();

                GL.Begin(PrimitiveType.LineLoop);
                GL.Color3(1.0, 0.0, 0.0);

                GL.Vertex3(minx, miny, minz);
                GL.Vertex3(minx, maxy, minz);
                GL.Vertex3(maxx, maxy, minz);
                GL.Vertex3(maxx, miny, minz);

                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Color3(1.0, 0.0, 0.0);

                GL.Vertex3(minx, miny, minz);
                GL.Vertex3(minx, miny, maxz);
                GL.Vertex3(minx, maxy, minz);
                GL.Vertex3(minx, maxy, maxz);
                GL.Vertex3(maxx, maxy, minz);
                GL.Vertex3(maxx, maxy, maxz);
                GL.Vertex3(maxx, miny, minz);
                GL.Vertex3(maxx, miny, maxz);

                GL.End();
                GL.Enable(EnableCap.DepthTest);
            }
            GL.PopMatrix();
        }*/

        private void SetupViewport()
        {
            double scale = 1 + ((double)trackBar3.Value * .5);
            renderer.SetupViewport(glControl1.Width, glControl1.Height, scale);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            glContextCreated = false;
            texMan.FreeTextureList(TextureList);
            Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            glControl1.Invalidate();
        }

        private void chkRadius_CheckedChanged(object sender, EventArgs e)
        {
            glControl1.Invalidate();
        }
    }
}
