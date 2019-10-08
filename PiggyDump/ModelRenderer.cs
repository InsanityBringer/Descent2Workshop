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
using OpenTK.Graphics.OpenGL;

namespace PiggyDump
{
    public class ModelRenderer
    {
        private int frame = -1;
        private Polymodel model;
        private PIGFile piggyFile;
        private HAMFile dataFile;
        private List<int> textureList;
        private ModelTextureManager texMan = new ModelTextureManager();
        private bool wireframe = false;
        private bool showRadius = false;
        private bool showBBs = false;
        private bool showNormals = false;
        private double angle = 0;
        private double pitch = 0;

        public bool Wireframe { get => wireframe; set => wireframe = value; }
        public bool ShowRadius { get => showRadius; set => showRadius = value; }
        public bool ShowBBs { get => showBBs; set => showBBs = value; }
        public bool ShowNormals { get => showNormals; set => showNormals = value; }
        public double Angle { get => angle; set => angle = value; }
        public double Pitch { get => pitch; set => pitch = value; }
        public int Frame { get => frame; set => frame = value; }

        public ModelRenderer(PIGFile piggyFile)
        {
            this.piggyFile = piggyFile;
        }

        public ModelRenderer(HAMFile dataFile, PIGFile piggyFile)
        {
            this.piggyFile = piggyFile;
            this.dataFile = dataFile;
        }

        public void SetModel(Polymodel model)
        {
            if (textureList != null)
                texMan.FreeTextureList(textureList);
            this.model = model;
            frame = -1;
            if (dataFile == null)
                textureList = texMan.LoadPolymodelTextures(model, piggyFile);
            else
                textureList = texMan.LoadPolymodelTextures(model, piggyFile, dataFile);
        }

        public void Init()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }

        public void SetupViewport(int width, int height, double scale)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Viewport(0, 0, width, height);
            GL.Ortho(-scale, scale, -scale, scale, -32, 32);
        }

        public void Draw()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.CullFace(CullFaceMode.Front);
            if (wireframe)
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            else
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            GL.Scale(-1.0, 1.0, 1.0);
            GL.Rotate(angle, 1.0, 0.0, 0.0);
            GL.Rotate(pitch, 0.0, 1.0, 0.0);

            DrawSubobject(model, model.submodels[0]);

            if (ShowRadius)
            {
                GL.LoadIdentity();
                GL.Scale(-1.0, 1.0, 1.0);
                GL.Disable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Texture2D);

                //Draw the radius
                double radius = (double)model.rad / 65536.0;
                double minx = (double)model.mins.x;
                double miny = (double)model.mins.y;
                double minz = (double)model.mins.z;
                double maxx = (double)model.maxs.x;
                double maxy = (double)model.maxs.y;
                double maxz = (double)model.maxs.z;
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
                GL.Rotate(angle, 1.0, 0.0, 0.0);
                GL.Rotate(pitch, 0.0, 1.0, 0.0);
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
            }
        }

        private void DrawSubobject(Polymodel mainModel, Submodel model)
        {
            GL.PushMatrix();
            GL.Translate(model.Offset.x, model.Offset.y, model.Offset.z);
            if (frame >= 0)
            {
                GL.Rotate((mainModel.animationMatrix[model.ID, frame].b / 16384.0f) * 90.0, 0.0, 0.0, 1.0);
                GL.Rotate((mainModel.animationMatrix[model.ID, frame].h / 16384.0f) * 90.0, 0.0, 1.0, 0.0);
                GL.Rotate((mainModel.animationMatrix[model.ID, frame].p / 16384.0f) * 90.0, 1.0, 0.0, 0.0);
            }
            double shade = 1.0;
            Vector3 light = new Vector3(0.5f, 0.5f, 0.0f);
            light.Normalize();
            Vector3 norm;
            for (int x = 0; x < model.faces.Count; x++)
            {
                PolymodelFace face = model.faces[x];
                norm = new Vector3(face.Normal.x, face.Normal.y, face.Normal.z);
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
                        double vx = face.points[i].x;
                        double vy = face.points[i].y;
                        double vz = face.points[i].z;

                        GL.Vertex3(vx, vy, vz);
                    }
                    GL.End();
                    if (showNormals)
                    {
                        GL.Color3(1.0f, 1.0f, 1.0f);
                        GL.Begin(PrimitiveType.Lines);
                        {
                            double vx = face.points[0].x;
                            double vy = face.points[0].y;
                            double vz = face.points[0].z;

                            double dx = face.points[0].x + face.Normal.x;
                            double dy = face.points[0].y + face.Normal.y;
                            double dz = face.points[0].z + face.Normal.z;

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
                    GL.BindTexture(TextureTarget.Texture2D, textureList[textureID]);

                    GL.Begin(PrimitiveType.TriangleFan);
                    for (int i = 0; i < face.points.Length; i++)
                    {
                        double u = face.UVLCoords[i].x;
                        double v = face.UVLCoords[i].y;

                        double vx = face.points[i].x;
                        double vy = face.points[i].y;
                        double vz = face.points[i].z;

                        GL.TexCoord2(u, v); GL.Vertex3(vx, vy, vz);
                    }
                    GL.End();
                    if (showNormals)
                    {
                        GL.Disable(EnableCap.Texture2D);
                        GL.Begin(PrimitiveType.Lines);
                        {
                            double vx = face.points[0].x;
                            double vy = face.points[0].y;
                            double vz = face.points[0].z;

                            double dx = face.points[0].x + face.Normal.x;
                            double dy = face.points[0].y + face.Normal.y;
                            double dz = face.points[0].z + face.Normal.z;

                            GL.Vertex3(vx, vy, vz);
                            GL.Vertex3(dx, dy, dz);
                        }
                        GL.End();
                        GL.Begin(PrimitiveType.Lines);
                        {
                            double vx = face.points[2].x;
                            double vy = face.points[2].y;
                            double vz = face.points[2].z;

                            double dx = (face.FaceVector.x);
                            double dy = (face.FaceVector.y);
                            double dz = (face.FaceVector.z);

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
            foreach (Submodel child in model.Children)
            {
                DrawSubobject(mainModel, child);
            }
            GL.LineWidth(2);
            GL.PointSize(3);
            //GL.Disable(EnableCap.Texture2D);
            if (showNormals)
            {
                double nx = model.Normal.x;
                double ny = model.Normal.y;
                double nz = model.Normal.z;

                double px = model.Point.x;
                double py = model.Point.y;
                double pz = model.Point.z;

                //Points are globally valid, so undo the translation
                GL.PushMatrix();
                GL.LoadIdentity();
                GL.Rotate(angle, 1.0, 0.0, 0.0);
                GL.Rotate(pitch, 0.0, 1.0, 0.0);
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
            if (showBBs)
            {
                GL.Disable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Texture2D);

                //Draw the radius
                double radius = model.Radius / 65536.0;
                double minx = model.Mins.x;
                double miny = model.Mins.y;
                double minz = model.Mins.z;
                double maxx = model.Maxs.x;
                double maxy = model.Maxs.y;
                double maxz = model.Maxs.z;
                //Follow the chain
                GL.PushMatrix();
                GL.Rotate(-pitch, 0.0, 1.0, 0.0);
                GL.Rotate(-angle, 1.0, 0.0, 0.0);
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
        }
    }
}
