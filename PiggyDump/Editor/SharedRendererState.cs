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
    public class SharedRendererState
    {

        private Level level;
        private HAMFile datafile;
        public EditorState state;

        private List<Render.MineRender> renderers = new List<Render.MineRender>();

        public SharedRendererState(Level level)
        {
            this.level = level;
        }

        public void AddRenderer(Render.MineRender renderer)
        {
            renderers.Add(renderer);
        }

        public void SetSelectedVert(LevelVertex vert, int index)
        {
            if (vert.selected)
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
                    renderer.RemoveSelectedVertAt(index);
                }
            }
        }
    }
}
