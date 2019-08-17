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

namespace PiggyDump.Editor
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

        public void AddVertex(FixVector point, FixVector uvl)
        {
            vertBuffer[pointer++] = -point.x / 65536.0f;
            vertBuffer[pointer++] = point.y / 65536.0f;
            vertBuffer[pointer++] = point.z / 65536.0f;
            vertBuffer[pointer++] = uvl.x / 65536.0f;
            vertBuffer[pointer++] = uvl.y / 65536.0f;
            vertBuffer[pointer++] = uvl.z / 65536.0f;
        }
    }

    public class SharedRendererState
    {
        private List<TextureChain> currentChains = new List<TextureChain>();
        private Level level;
        private HAMFile datafile;
        private float[] vertBuffer;
        public EditorState state;

        private List<Render.MineRender> renderers = new List<Render.MineRender>();
        private Dictionary<int, int> textureChainMapping = new Dictionary<int, int>();
        private List<TextureChain> dirtyChains = new List<TextureChain>();

        public float[] VertBuffer
        {
            get { return vertBuffer; }
        }

        public List<TextureChain> TextureChains
        {
            get { return currentChains; }
        }

        public SharedRendererState(Level level)
        {
            this.level = level;
        }

        public void AddRenderer(Render.MineRender renderer)
        {
            renderers.Add(renderer);
        }

        public void ToggleSelectedVert(LevelVertex vert)
        {
            if (!vert.selected)
            {
                foreach (Render.MineRender renderer in renderers)
                {
                    renderer.AddSelectedVert(vert);
                }
            }
            else
            {
                foreach (Render.MineRender renderer in renderers)
                {
                    int deleteIndex = state.SelectedVertMapping[vert];
                    renderer.RemoveSelectedVertAt(deleteIndex);
                }
            }
        }

        public void InitTransformBuffer(float[] vertbuffer, int[] indexbuffer)
        {
            foreach (Render.MineRender renderer in renderers)
            {
                renderer.InitTransformBuffer(vertbuffer, indexbuffer);
            }
        }

        public void SetShadowTranslation(Vector3 xAxis, Vector3 yAxis)
        {
            foreach (Render.MineRender renderer in renderers)
            {
                renderer.SetShadowTranslation(xAxis, yAxis);
            }
        }

        public void BuildPointBuffer()
        {
            int numVerts = level.Verts.Count;
            vertBuffer = new float[numVerts * 4];
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
        }

        public void MarkChainDirty(int tmap1, int tmap2)
        {
            int chainid = tmap1;
            if (tmap2 != 0)
                chainid += tmap2 + 0x8000;

            if (chainid == 0) return;

            currentChains[textureChainMapping[chainid]].chainDirty = true;
            dirtyChains.Add(currentChains[textureChainMapping[chainid]]);
        }

        public void MarkSegmentsDirty(ICollection<Segment> segments)
        {
            foreach (Segment seg in segments)
            {
                foreach (Side side in seg.sides)
                {
                    MarkChainDirty(side.tmapNum, side.tmapNum2);
                    side.SetType();
                }
            }
            BuildWorld(true);
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
                foreach (Segment segment in level.Segments)
                {
                    for (int i = 0; i < Segment.MaxSegmentSides; i++)
                    {
                        side = segment.sides[i];
                        if (segment.childrenIDs[i] == -1 || side.wall != null)
                        {
                            identifier = side.tmapNum;
                            if (side.tmapNum2 != 0)
                                identifier += (side.tmapNum2 + 0x8000);

                            if (!textureChainMapping.ContainsKey(identifier))
                            {
                                TextureChain chain = new TextureChain();
                                chain.texture1 = side.tmapNum;
                                chain.texture2 = side.tmapNum2;
                                currentChains.Add(chain);
                                textureChainMapping.Add(identifier, currentChains.Count - 1);
                            }

                            currentChains[textureChainMapping[identifier]].numVerts += 6;
                        }
                    }
                }
                //Allocate space for each texture chain.
                foreach (TextureChain chain in currentChains)
                {
                    chain.InitChain();
                }
                //Now actually generate the chain vertex buffer
                TextureChain currentChain;
                FixVector vert, uvl;
                foreach (Segment segment in level.Segments)
                {
                    for (int i = 0; i < Segment.MaxSegmentSides; i++)
                    {
                        side = segment.sides[i];
                        if (segment.childrenIDs[i] == -1 || side.wall != null)
                        {
                            identifier = side.tmapNum;
                            if (side.tmapNum2 != 0)
                                identifier += (side.tmapNum2 + 0x8000);

                            currentChain = currentChains[textureChainMapping[identifier]];
                            //TODO: Split triangle in a better manner based on the quad's four points. 
                            //0   1
                            //3   2 probably? I'm confused
                            if (side.type == SideType.Triangle02)
                            {
                                for (int p = 0; p < 6; p++)
                                {
                                    vert = level.Verts[segment.verts[Segment.SideVerts[i, pointWinding02[p]]]].location;
                                    uvl = side.uvls[pointWinding02[p]];
                                    currentChain.AddVertex(vert, uvl);
                                }
                            }
                            else
                            {
                                for (int p = 0; p < 6; p++)
                                {
                                    vert = level.Verts[segment.verts[Segment.SideVerts[i, pointWinding13[p]]]].location;
                                    uvl = side.uvls[pointWinding13[p]];
                                    currentChain.AddVertex(vert, uvl);
                                }
                            }
                        }
                    }
                }
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
                FixVector vert, uvl;
                foreach (Segment segment in level.Segments)
                {
                    for (int i = 0; i < Segment.MaxSegmentSides; i++)
                    {
                        side = segment.sides[i];
                        if (segment.childrenIDs[i] == -1 || side.wall != null)
                        {
                            identifier = side.tmapNum;
                            if (side.tmapNum2 != 0)
                                identifier += (side.tmapNum2 + 0x8000);

                            currentChain = currentChains[textureChainMapping[identifier]];
                            if (currentChain.chainDirty)
                            {
                                //TODO: Split triangle in a better manner based on the quad's four points. 
                                //0   1
                                //2   3 probably
                                if (side.type == SideType.Triangle02)
                                {
                                    for (int p = 0; p < 6; p++)
                                    {
                                        vert = level.Verts[segment.verts[Segment.SideVerts[i, pointWinding02[p]]]].location;
                                        uvl = side.uvls[pointWinding02[p]];
                                        currentChain.AddVertex(vert, uvl);
                                    }
                                }
                                else
                                {
                                    for (int p = 0; p < 6; p++)
                                    {
                                        vert = level.Verts[segment.verts[Segment.SideVerts[i, pointWinding13[p]]]].location;
                                        uvl = side.uvls[pointWinding13[p]];
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

                foreach (Render.MineRender renderer in renderers)
                {
                    renderer.MakeHostCurrent();
                    renderer.ClearShadow();
                    renderer.UpdateWorldOutline();
                    renderer.RegenerateSelectedPoints(state.SelectedVertices);
                    renderer.UpdateChains(dirtyChains);
                }
                dirtyChains.Clear();
            }
        }
    }
}
