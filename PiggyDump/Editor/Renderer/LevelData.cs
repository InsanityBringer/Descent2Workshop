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

namespace Descent2Workshop.Editor.Renderer
{
    public class TextureChain
    {
        public short texture1;
        public short texture2;
        public int numVerts;
        public float[] vertBuffer;
        public int pointer = 0;
        public bool chainDirty = false;

        public void InitChain()
        {
            vertBuffer = new float[numVerts * 6];
            pointer = 0;
        }

        public void AddVertex(FixVector point, Uvl uvl)
        {
            vertBuffer[pointer++] = -point.X;
            vertBuffer[pointer++] = point.Y;
            vertBuffer[pointer++] = point.Z;
            vertBuffer[pointer++] = uvl.U;
            vertBuffer[pointer++] = uvl.V;
            vertBuffer[pointer++] = uvl.L;
        }
    }

    public class InternalChain
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

    public class LevelData
    {
        private MineRenderer baseRenderer;

        private List<InternalChain> currentChains = new List<InternalChain>();
        private Dictionary<int, int> textureChainMapping = new Dictionary<int, int>();
        private Dictionary<int, int> textureMapping = new Dictionary<int, int>();
        private List<TextureChain> baseChains = new List<TextureChain>();
        private List<TextureChain> dirtyChains = new List<TextureChain>();

        private float[] vertBuffer;

        private int paletteTexture;

        private Shader mineShader, outlineShader, shadowShader;
        private int mineVAO;
        private int outlineBuffer, outlineIndexBuffer, outlineCount, outlinePoints;

        /*public TransformBuffer transformBuffer = new TransformBuffer();
        public PickBuffer pickBuffer = new PickBuffer();*/

        public float[] VertBuffer { get => vertBuffer; set => vertBuffer = value; }

        public LevelData(MineRenderer baseRenderer)
        {
            this.baseRenderer = baseRenderer;
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

            //InitPalette(baseRenderer.State.EditorData.piggyFile.PiggyPalette.GetLinear());
            InitPalette(new Palette().GetLinear());

            //transformBuffer.Init(shadowShader);
            //pickBuffer.Init();
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
                ushort texture = baseRenderer.State.EditorData.Textures[tex1];
                PIGImage img = baseRenderer.State.EditorData.piggyFile.Bitmaps[texture];
                GL.ActiveTexture(TextureUnit.Texture0);
                int textureID = LoadTexture(img.GetData(), img.Width, img.Height);
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
                    ushort texture = baseRenderer.State.EditorData.Textures[tex2];
                    PIGImage img = baseRenderer.State.EditorData.piggyFile.Bitmaps[texture];
                    GL.ActiveTexture(TextureUnit.Texture1);
                    int textureID = LoadTexture(img.GetData(), img.Width, img.Height);
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

        public void UpdateWorldOutline()
        {
            GL.BindVertexArray(mineVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, outlineBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, outlineIndexBuffer);

            int numVerts = baseRenderer.State.EditorLevel.Vertices.Count;
            int numIndicies = baseRenderer.State.EditorLevel.Segments.Count * Segment.MaxSides * 5; //Line loops with injected reset 
            outlineCount = numIndicies;
            outlinePoints = numVerts;

            float[] vertBuffer = new float[numVerts * 4];
            FixVector vec;
            //Build the vertex buffer
            for (int i = 0; i < numVerts; i++)
            {
                vec = baseRenderer.State.EditorLevel.Vertices[i].Location;
                vertBuffer[i * 4 + 0] = -vec.X;
                vertBuffer[i * 4 + 1] = vec.Y;
                vertBuffer[i * 4 + 2] = vec.Z;
                //TODO: make this integer. 
                vertBuffer[i * 4 + 3] = i;
            }
            uint[] indexBuffer = new uint[numIndicies];
            Segment seg;
            Side side;
            int pointer = 0;
            for (int segnum = 0; segnum < baseRenderer.State.EditorLevel.Segments.Count; segnum++)
            {
                seg = baseRenderer.State.EditorLevel.Segments[segnum];
                for (int sidenum = 0; sidenum < Segment.MaxSides; sidenum++)
                {
                    side = seg.Sides[sidenum];
                    for (int i = 0; i < 4; i++)
                    {
                        //[ISB] TODO: Optimize
                        indexBuffer[pointer++] = (uint)baseRenderer.State.EditorLevel.Vertices.IndexOf(seg.Sides[sidenum].GetVertex(i));
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

        public void BuildPointBuffer()
        {
            int numVerts = baseRenderer.State.EditorLevel.Vertices.Count;
            vertBuffer = new float[numVerts * 4];
            FixVector vec;
            //Build the vertex buffer
            for (int i = 0; i < numVerts; i++)
            {
                vec = baseRenderer.State.EditorLevel.Vertices[i].Location;
                vertBuffer[i * 4 + 0] = -vec.X;
                vertBuffer[i * 4 + 1] = vec.Y;
                vertBuffer[i * 4 + 2] = vec.Z;
                //TODO: make this integer. 
                vertBuffer[i * 4 + 3] = i;
            }
        }

        public void BuildWorld(bool rebuild = false)
        {
            BuildPointBuffer();
            int[] pointWinding02 = { 0, 2, 3, 0, 1, 2 };
            int[] pointWinding13 = { 0, 1, 3, 1, 2, 3 };
            int identifier;
            Side side;
            if (!rebuild)
            {
                currentChains.Clear();
                //Segment segment = level.Segments[4];
                //Count the amount of verts in each texture chain
                foreach (Segment segment in baseRenderer.State.EditorLevel.Segments)
                {
                    for (int i = 0; i < Segment.MaxSides; i++)
                    {
                        side = segment.Sides[i];
                        if (side.ConnectedSegment != null || side.Wall != null)
                        {
                            identifier = side.BaseTextureIndex;
                            if (side.OverlayTextureIndex != 0)
                                identifier += (side.OverlayTextureIndex + 0x8000);

                            if (!textureChainMapping.ContainsKey(identifier))
                            {
                                TextureChain chain = new TextureChain();
                                chain.texture1 = (short)side.BaseTextureIndex;
                                chain.texture2 = (short)side.OverlayTextureIndex;
                                baseChains.Add(chain);
                                textureChainMapping.Add(identifier, baseChains.Count - 1);
                            }

                            baseChains[textureChainMapping[identifier]].numVerts += 6;
                        }
                    }
                }
                //Allocate space for each texture chain.
                foreach (TextureChain chain in baseChains)
                {
                    chain.InitChain();
                }
                //Now actually generate the chain vertex buffer
                TextureChain currentChain;
                FixVector vert;
                Uvl uvl;
                foreach (Segment segment in baseRenderer.State.EditorLevel.Segments)
                {
                    for (int i = 0; i < Segment.MaxSides; i++)
                    {
                        side = segment.Sides[i];
                        if (side.ConnectedSegment != null || side.Wall != null)
                        {
                            identifier = side.BaseTextureIndex;
                            if (side.OverlayTextureIndex != 0)
                                identifier += (side.OverlayTextureIndex + 0x8000);

                            currentChain = baseChains[textureChainMapping[identifier]];
                            //TODO: Split triangle in a better manner based on the quad's four points. 
                            //0   1
                            //3   2 probably? I'm confused
                            if (side.Triangulation == TriangulationType.Tri012_230)
                            {
                                for (int p = 0; p < 6; p++)
                                {
                                    vert = baseRenderer.State.EditorLevel.Vertices[baseRenderer.State.EditorLevel.Vertices.IndexOf(side.GetVertex(pointWinding02[p]))].Location;
                                    uvl = side.Uvls[pointWinding02[p]];
                                    currentChain.AddVertex(vert, uvl);
                                }
                            }
                            else
                            {
                                for (int p = 0; p < 6; p++)
                                {
                                    vert = baseRenderer.State.EditorLevel.Vertices[baseRenderer.State.EditorLevel.Vertices.IndexOf(side.GetVertex(pointWinding13[p]))].Location;
                                    uvl = side.Uvls[pointWinding13[p]];
                                    currentChain.AddVertex(vert, uvl);
                                }
                            }
                        }
                    }
                }
                BuildWorldData();
            }
            else
            {
                //For each dirty chain, allocate space and ensure everything is ready for new data.
                //No counting, since chains are only dirty if nothing is added or rmemoved
                foreach (TextureChain chain in dirtyChains)
                {
                    chain.InitChain();
                }

                //Same deal for adding geometry
                TextureChain currentChain;
                FixVector vert;
                Uvl uvl;
                foreach (Segment segment in baseRenderer.State.EditorLevel.Segments)
                {
                    for (int i = 0; i < Segment.MaxSides; i++)
                    {
                        side = segment.Sides[i];
                        if (side.ConnectedSegment != null || side.Wall != null)
                        {
                            identifier = side.BaseTextureIndex;
                            if (side.OverlayTextureIndex != 0)
                                identifier += (side.OverlayTextureIndex + 0x8000);

                            currentChain = baseChains[textureChainMapping[identifier]];
                            if (currentChain.chainDirty)
                            {
                                //TODO: Split triangle in a better manner based on the quad's four points. 
                                //0   1
                                //2   3 probably
                                if (side.Triangulation == TriangulationType.Tri012_230)
                                {
                                    for (int p = 0; p < 6; p++)
                                    {
                                        vert = baseRenderer.State.EditorLevel.Vertices[baseRenderer.State.EditorLevel.Vertices.IndexOf(side.GetVertex(pointWinding02[p]))].Location;
                                        uvl = side.Uvls[pointWinding02[p]];
                                        currentChain.AddVertex(vert, uvl);
                                    }
                                }
                                else
                                {
                                    for (int p = 0; p < 6; p++)
                                    {
                                        vert = baseRenderer.State.EditorLevel.Vertices[baseRenderer.State.EditorLevel.Vertices.IndexOf(side.GetVertex(pointWinding13[p]))].Location;
                                        uvl = side.Uvls[pointWinding13[p]];
                                        currentChain.AddVertex(vert, uvl);
                                    }
                                }
                            }
                        }
                    }
                }

                //more shitty linear iterations
                foreach (TextureChain chain in dirtyChains)
                {
                    chain.chainDirty = false;
                }

                ClearShadow();
                UpdateWorldOutline();
                //TODO
                //baseRenderer.RegenerateSelectedPoints(baseRenderer.State.SelectedVertices);
                UpdateInternalChains(dirtyChains);
                dirtyChains.Clear();
            }
        }

        public void BuildWorldData()
        {
            currentChains.Clear();
            //four loops uuuuuugh
            int identifier;
            int count = 0;
            foreach (TextureChain chain in baseChains)
            {
                identifier = chain.texture1;
                if (chain.texture2 != 0)
                    identifier += (chain.texture2 + 0x8000);
                //textureChainMapping.Add(identifier, count++);
                InternalChain internalChain = new InternalChain();
                internalChain.InitBuffer(chain);
                currentChains.Add(internalChain);
            }
        }

        public void UpdateInternalChains(List<TextureChain> chains)
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

        public void MarkChainDirty(int tmap1, int tmap2)
        {
            int chainid = tmap1;
            if (tmap2 != 0)
                chainid += tmap2 + 0x8000;

            if (chainid == 0) return;

            baseChains[textureChainMapping[chainid]].chainDirty = true;
            dirtyChains.Add(baseChains[textureChainMapping[chainid]]);
        }

        public void MarkSegmentsDirty(ICollection<Segment> segments)
        {
            foreach (Segment seg in segments)
            {
                foreach (Side side in seg.Sides)
                {
                    MarkChainDirty(side.BaseTextureIndex, side.OverlayTextureIndex);
                }
            }
            BuildWorld(true);
        }

        public void SetShadowTranslation(Vector3 xAxis, Vector3 yAxis)
        {
            shadowShader.UseShader();
            int translateX = GL.GetUniformLocation(shadowShader.shaderID, "translateX");
            int translateY = GL.GetUniformLocation(shadowShader.shaderID, "translateY");
            GL.Uniform3(translateX, xAxis);
            GL.Uniform3(translateY, yAxis);
            GLUtilities.ErrorCheck("Updating translation");
        }

        public void ClearShadow()
        {
            //transformBuffer.Clear();
        }

        public void DrawLevel(Camera renderCamera, bool DrawMine, bool DrawOutline, bool DrawShadow)
        {
            //Ensure all the shader uniforms for projection and orientation are set right
            renderCamera.SetShaderProjection(mineShader);
            renderCamera.SetShaderProjection(outlineShader);
            renderCamera.SetShaderProjection(shadowShader);

            if (DrawMine)
                DrawWorld();
            if (DrawOutline)
            {
                DrawWorldOutline();
            }
            DrawSelectedPoints();
            if (DrawShadow)
            {
                this.DrawShadow();
            }
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
            GL.PointSize(5.0f);
            outlineShader.UseShader();
            int lineColor = GL.GetUniformLocation(outlineShader.shaderID, "lineColor");
            GL.Uniform4(lineColor, 0.75f, 0.75f, 0.75f, 1.0f);
            GLUtilities.ErrorCheck("Setting line color");
            GL.BindVertexArray(mineVAO);
            GL.DrawElements(PrimitiveType.LineLoop, outlineCount, DrawElementsType.UnsignedInt, 0);
            GL.DrawArrays(PrimitiveType.Points, 0, outlinePoints);
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
            //pickBuffer.DrawSelectedPoints();
            GL.Enable(EnableCap.DepthTest);
        }

        public void DrawShadow()
        {
            shadowShader.UseShader();
            int lineColor = GL.GetUniformLocation(shadowShader.shaderID, "lineColor");
            GL.Uniform4(lineColor, 0.25f, 0.25f, 1.0f, 1.0f);
            GLUtilities.ErrorCheck("Setting vertex color");
            GL.Disable(EnableCap.DepthTest);
            //transformBuffer.DrawTransform();
            GL.Enable(EnableCap.DepthTest);
        }
    }
}