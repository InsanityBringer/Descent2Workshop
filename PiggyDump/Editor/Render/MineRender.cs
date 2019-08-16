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
    public class MineRender
    {
        class InternalChain
        {
            public short texture1;
            public short texture2;
            public int numVerts;
            private int bufferNum;
            private int arrayNum;

            public void InitBuffer(TextureChain source)
            {
                texture1 = source.texture1; texture2 = source.texture2;
                numVerts = source.numVerts;
                bufferNum = GL.GenBuffer();
                arrayNum = GL.GenVertexArray();
                GL.BindVertexArray(arrayNum);
                GL.BindBuffer(BufferTarget.ArrayBuffer, bufferNum);
                GL.BufferData(BufferTarget.ArrayBuffer, source.pointer * sizeof(float), source.vertBuffer, BufferUsageHint.StaticDraw);
                GLUtilities.ErrorCheck(string.Format("Uploading chain {0} + {1}", texture1, texture2));
                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 6, 0);
                GLUtilities.ErrorCheck(string.Format("Chain {0} + {1} Attrib 0", texture1, texture2));
                GL.EnableVertexAttribArray(1);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 6, sizeof(float) * 3);
                GLUtilities.ErrorCheck(string.Format("Chain {0} + {1} Attrib 1", texture1, texture2));
            }

            public void UpdateChain(TextureChain source)
            {
                GL.BindVertexArray(arrayNum);
                GL.BindBuffer(BufferTarget.ArrayBuffer, bufferNum);
                GL.BufferData(BufferTarget.ArrayBuffer, source.pointer * sizeof(float), source.vertBuffer, BufferUsageHint.StaticDraw);
            }

            public void DrawChain()
            {
                GL.BindVertexArray(arrayNum);
                GL.BindBuffer(BufferTarget.ArrayBuffer, bufferNum);
                GL.DrawArrays(PrimitiveType.Triangles, 0, numVerts);
                GLUtilities.ErrorCheck(string.Format("Drawing chain {0} + {1}", texture1, texture2));
            }

            public void FreeChain()
            {
            }
        }
        private SharedRendererState sharedState;
        private Shader mineShader, outlineShader, shadowShader;
        private List<InternalChain> currentChains = new List<InternalChain>();
        private Level level;
        private HAMFile datafile;
        private int mineVAO;
        private int paletteTexture;
        private Dictionary<int, int> textureChainMapping = new Dictionary<int, int>();
        private Dictionary<int, int> textureMapping = new Dictionary<int, int>();
        private int outlineBuffer, outlineIndexBuffer, outlineCount, outlinePoints;
        private PickBuffer pickBuffer = new PickBuffer();
        private TransformBuffer transformBuffer = new TransformBuffer();
        private Camera camera;
        private GLControl host;

        public Camera ViewCamera { get { return camera; } }

        public MineRender(Level level, HAMFile datafile, SharedRendererState sharedState, GLControl host)
        {
            this.level = level;
            this.datafile = datafile;
            this.sharedState = sharedState;
            camera = new Camera();
            this.host = host;
        }

        public void Init()
        {
            mineVAO = GL.GenVertexArray();
            mineShader = new Shader("MineRenderer");
            mineShader.Init();
            mineShader.AddShader("./Editor/VertexShaderMine.txt", ShaderType.VertexShader);
            mineShader.AddShader("./Editor/FragmentShaderMine.txt", ShaderType.FragmentShader);
            mineShader.LinkShader();
            GLUtilities.ErrorCheck("Mine shader init");
            outlineShader = new Shader("PointRenderer");
            outlineShader.Init();
            outlineShader.AddShader("./Editor/VertexShaderOutline.txt", ShaderType.VertexShader);
            outlineShader.AddShader("./Editor/FragmentShaderOutline.txt", ShaderType.FragmentShader);
            outlineShader.LinkShader();
            GLUtilities.ErrorCheck("Outline shader init");
            shadowShader = new Shader("ShadowRenderer");
            shadowShader.Init();
            shadowShader.AddShader("./Editor/VertexShaderShadow.txt", ShaderType.VertexShader);
            shadowShader.AddShader("./Editor/FragmentShaderOutline.txt", ShaderType.FragmentShader);
            shadowShader.LinkShader();
            GLUtilities.ErrorCheck("Shadow render init");
            GL.Enable(EnableCap.DepthTest);
            InitPalette(datafile.piggyFile.PiggyPalette.GetLinear());
            Console.WriteLine("Renderer init");
            GL.Enable(EnableCap.PrimitiveRestart);
            GL.PrimitiveRestartIndex(32767);
            pickBuffer.Init();
            transformBuffer.Init();
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
            camera.SetShaderProjection(mineShader);
            camera.SetShaderProjection(outlineShader);
            camera.SetShaderProjection(shadowShader);
            camera.UpdateShader(mineShader);
            camera.UpdateShader(outlineShader);
            camera.UpdateShader(shadowShader);
        }

        public void MakePerpectiveCamera(float fov, float aspect)
        {
            camera.MakePerpectiveCamera(fov, aspect);
            camera.SetShaderProjection(mineShader);
            camera.SetShaderProjection(outlineShader);
            camera.SetShaderProjection(shadowShader);
            camera.UpdateShader(mineShader);
            camera.UpdateShader(outlineShader);
            camera.UpdateShader(shadowShader);
        }

        public void InitPalette(byte[] palette)
        {
            GL.ActiveTexture(TextureUnit.Texture2);
            paletteTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture1D, paletteTexture);
            GL.TexImage1D(TextureTarget.Texture1D, 0, PixelInternalFormat.Rgba, 256, 0, PixelFormat.Rgb, PixelType.UnsignedByte, palette);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture1D, TextureParameterName.TextureMaxLevel, 0);
            GLUtilities.ErrorCheck("Palette init");
            mineShader.UseShader();
            int palettePos = GL.GetUniformLocation(mineShader.shaderID, "palette");
            GL.Uniform1(palettePos, 2);
            int texture1 = GL.GetUniformLocation(mineShader.shaderID, "texture1");
            GL.Uniform1(texture1, 0);
            int texture2 = GL.GetUniformLocation(mineShader.shaderID, "texture2");
            GL.Uniform1(texture2, 1);
            GLUtilities.ErrorCheck("Palette uniform set");
        }

        private void BindChainTextures(InternalChain chain)
        {
            int tex1 = chain.texture1;
            if (!textureMapping.ContainsKey(tex1))
            {
                ushort texture = datafile.Textures[tex1];
                ImageData img = datafile.piggyFile.images[texture];
                GL.ActiveTexture(TextureUnit.Texture0);
                int textureID = LoadTexture(img.data, img.width, img.height);
                textureMapping.Add(tex1, textureID);
            }
            GL.ActiveTexture(TextureUnit.Texture0);
            int textureName = textureMapping[tex1];
            GL.BindTexture(TextureTarget.Texture2D, textureName);
            int tex2 = chain.texture2 & 0x3FFF;
            int hasTexture2 = GL.GetUniformLocation(mineShader.shaderID, "hasTexture2");
            GL.Uniform1(hasTexture2, chain.texture2);
            if (tex2 != 0)
            {
                if (!textureMapping.ContainsKey(tex2))
                {
                    ushort texture = datafile.Textures[tex2];
                    ImageData img = datafile.piggyFile.images[texture];
                    GL.ActiveTexture(TextureUnit.Texture1);
                    int textureID = LoadTexture(img.data, img.width, img.height);
                    textureMapping.Add(tex2, textureID);
                }
                GL.ActiveTexture(TextureUnit.Texture1);
                textureName = textureMapping[tex2];
                GL.BindTexture(TextureTarget.Texture2D, textureName);
            }
            GLUtilities.ErrorCheck("Chain texture binding");
        }

        public int LoadTexture(byte[] texture, int w, int h)
        {
            int textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8ui, w, h, 0, PixelFormat.RedInteger, PixelType.UnsignedByte, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);

            return textureID;
        }

        public void testMouseMove(int x, int y, int move)
        {
            float ang = (float)(x * .25 * Math.PI / 180.0);
            float pitch = (float)(y * .25 * Math.PI / 180.0);
            //camera.Rotate(new Vector3(1.0f, 0.0f, 0.0f), pitch);
            //camera.Rotate(new Vector3(0.0f, 1.0f, 0.0f), ang);
            if (move == 0)
                camera.Translate(new Vector3(-x / 5.0f, y / 5.0f, 0.0f));
            else if (move == 1)
                camera.Rotate(-ang, -pitch);
            //camera.Pitch(pitch);
            camera.UpdateShader(mineShader);
            camera.UpdateShader(outlineShader);
            camera.UpdateShader(shadowShader);
        }

        public void DrawWorld()
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            mineShader.UseShader();
            //Console.WriteLine("Redrawing all chains");
            foreach (InternalChain chain in currentChains)
            {
                BindChainTextures(chain);
                chain.DrawChain();
            }
            //DrawWorldOutline();
        }

        public void DrawWorldOutline()
        {
            //GL.LineWidth(2.0f);
            GL.PointSize(3.0f);
            outlineShader.UseShader();
            int lineColor = GL.GetUniformLocation(outlineShader.shaderID, "lineColor");
            GL.Uniform4(lineColor, 0.75f, 0.75f, 0.75f, 1.0f);
            GLUtilities.ErrorCheck("Setting line color");
            GL.BindVertexArray(mineVAO);
            GL.DrawElements(PrimitiveType.LineLoop, outlineCount, DrawElementsType.UnsignedInt, 0);
            //GL.DrawArrays(PrimitiveType.Points, 0, outlinePoints);
            GLUtilities.ErrorCheck("Drawing outline");
        }

        public void DrawWorldPoints()
        {
            //GL.LineWidth(2.0f);
            GL.PointSize(3.0f);
            outlineShader.UseShader();
            int lineColor = GL.GetUniformLocation(outlineShader.shaderID, "lineColor");
            GL.Uniform4(lineColor, 0.75f, 0.75f, 0.75f, 1.0f);
            GLUtilities.ErrorCheck("Setting vertex color");
            GL.BindVertexArray(mineVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, outlineBuffer);
            GL.DrawArrays(PrimitiveType.Points, 0, outlinePoints);
            GLUtilities.ErrorCheck("Drawing vertexes");
        }

        public void DrawSelectedPoints()
        {
            GL.PointSize(10.0f);
            outlineShader.UseShader();
            int lineColor = GL.GetUniformLocation(outlineShader.shaderID, "lineColor");
            GL.Uniform4(lineColor, 0.5f, 0.5f, 1.0f, 1.0f);
            GL.Disable(EnableCap.DepthTest);
            GLUtilities.ErrorCheck("Setting vertex color");
            pickBuffer.DrawSelectedPoints();
            GL.Enable(EnableCap.DepthTest);
        }

        public void DrawShadow()
        {
            shadowShader.UseShader();
            int lineColor = GL.GetUniformLocation(shadowShader.shaderID, "lineColor");
            GL.Uniform4(lineColor, 0.25f, 0.25f, 1.0f, 1.0f);
            GLUtilities.ErrorCheck("Setting vertex color");
            GL.Disable(EnableCap.DepthTest);
            transformBuffer.DrawTransform();
            GL.Enable(EnableCap.DepthTest);
        }

        public void UpdateWorldOutline()
        {
            GL.BindVertexArray(mineVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, outlineBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, outlineIndexBuffer);

            int numVerts = level.Verts.Count;
            int numIndicies = level.Segments.Count * Segment.MaxSegmentSides * 5; //Line loops with injected reset 
            outlineCount = numIndicies;
            outlinePoints = numVerts;

            float[] vertBuffer = new float[numVerts * 4];
            FixVector vec;
            //Build the vertex buffer
            for (int i = 0; i < numVerts; i++)
            {
                vec = level.Verts[i].location;
                vertBuffer[i * 4 + 0] = -vec.x / 65536.0f;
                vertBuffer[i * 4 + 1] = vec.y / 65536.0f;
                vertBuffer[i * 4 + 2] = vec.z / 65536.0f;
                //TODO: make this integer. 
                vertBuffer[i * 4 + 3] = i;
            }
            uint[] indexBuffer = new uint[numIndicies];
            Segment seg;
            Side side;
            int pointer = 0;
            for (int segnum = 0; segnum < level.Segments.Count; segnum++)
            {
                seg = level.Segments[segnum];
                for (int sidenum = 0; sidenum < Segment.MaxSegmentSides; sidenum++)
                {
                    side = seg.sides[sidenum];
                    for (int i = 0; i < 4; i++)
                    {
                        indexBuffer[pointer++] = (uint)seg.verts[Segment.SideVerts[sidenum, i]];
                    }
                    indexBuffer[pointer++] = 32767; //Add reset code. Resets every four verts is questionable, but...
                }
            }
            GL.BufferData(BufferTarget.ArrayBuffer, numVerts * 4 * sizeof(float), vertBuffer, BufferUsageHint.StaticDraw);
            GL.BufferData(BufferTarget.ElementArrayBuffer, numIndicies * sizeof(uint), indexBuffer, BufferUsageHint.StaticDraw);
            GLUtilities.ErrorCheck("Uploading outline buffer");
        }

        public void BuildWorldOutline()
        {
            GL.BindVertexArray(mineVAO);
            outlineBuffer = GL.GenBuffer();
            outlineIndexBuffer = GL.GenBuffer();
            GLUtilities.ErrorCheck("Generating outline buffer");
            UpdateWorldOutline();

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, sizeof(float) * 4, sizeof(float) * 3);
            GLUtilities.ErrorCheck("Outline vertex attribs");
        }

        public void GetCameraUpSide(out Vector3 up, out Vector3 side)
        {
            camera.GetUpSide(out up, out side);
        }

        public int testPick(float testx, float testy)
        {
            float[] verts = sharedState.VertBuffer;
            int numVerts = verts.Length / 4;
            Vector3 vec;
            List<Vector3> pts = new List<Vector3>();
            float bestZ = 10000.0f;
            int bestID = 0;
            Vector3 bestVec = new Vector3(0, 0, 0);
            for (int i = 0; i < numVerts; i++)
            {
                vec.X = verts[i * 4 + 0];
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
                sharedState.ToggleSelectedVert(level.Verts[bestID]);
            }
            /*testSelectBuffer = new float[pts.Count * 3];
            for (int i = 0; i < pts.Count; i++)
            {
                testSelectBuffer[i * 3 + 0] = pts[i].X;
                testSelectBuffer[i * 3 + 1] = pts[i].Y;
                testSelectBuffer[i * 3 + 2] = pts[i].Z;
            }
            testSelectCount = pts.Count;
            testSelectVAO = GL.GenVertexArray();
            GL.BindVertexArray(testSelectVAO);
            testSelectBufferName = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, testSelectBufferName);
            GL.BufferData(BufferTarget.ArrayBuffer, testSelectBuffer.Length * sizeof(float), testSelectBuffer, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);*/
            return -1;
        }

        public void AddSelectedVert(LevelVertex vert)
        {
            host.MakeCurrent();
            pickBuffer.AddVertex(vert);
        }

        public void RemoveSelectedVertAt(int index)
        {
            host.MakeCurrent();
            pickBuffer.RemoveVertAt(index);
        }

        public void InitTransformBuffer(float[] vertbuffer, int[] indexbuffer)
        {
            host.MakeCurrent();
            transformBuffer.Fill(vertbuffer, indexbuffer);
        }

        public void SetShadowTranslation(Vector3 xAxis, Vector3 yAxis)
        {
            host.MakeCurrent();
            shadowShader.UseShader();
            int translateX = GL.GetUniformLocation(shadowShader.shaderID, "translateX");
            int translateY = GL.GetUniformLocation(shadowShader.shaderID, "translateY");
            GL.Uniform3(translateX, xAxis);
            GL.Uniform3(translateY, yAxis);
            GLUtilities.ErrorCheck("Updating translation");
        }

        public void BuildWorld()
        {
            currentChains.Clear();
            //four loops uuuuuugh
            int identifier;
            int count = 0;
            foreach (TextureChain chain in sharedState.TextureChains)
            {
                identifier = chain.texture1;
                if (chain.texture2 != 0)
                    identifier += (chain.texture2 + 0x8000);
                textureChainMapping.Add(identifier, count++);
                InternalChain internalChain = new InternalChain();
                internalChain.InitBuffer(chain);
                currentChains.Add(internalChain);
            }
        }

        public void MakeHostCurrent()
        {
            host.MakeCurrent();
        }

        //TODO: make this considerably less ass
        public void RegenerateSelectedPoints(List<LevelVertex> verts)
        {
            pickBuffer.ClearVerts();
            foreach (LevelVertex vert in verts)
            {
                pickBuffer.AddVertex(vert);
            }
        }

        public void ClearShadow()
        {
            transformBuffer.Clear();
        }

        public void UpdateChains(List<TextureChain> chains)
        {
            int identifier;
            foreach (TextureChain chain in chains)
            {
                identifier = chain.texture1;
                if (chain.texture2 != 0)
                    identifier += (chain.texture2 + 0x8000);
                InternalChain internalChain = currentChains[textureChainMapping[identifier]];
                internalChain.UpdateChain(chain);
            }
        }
    }
}
