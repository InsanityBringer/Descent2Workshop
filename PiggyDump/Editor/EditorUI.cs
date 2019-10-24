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
using System.Threading.Tasks;
using System.Windows.Forms;
using Descent2Workshop.Editor.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using LibDescent.Data;

namespace Descent2Workshop.Editor
{
    public partial class EditorUI : Form
    {
        private bool contextCreated = false;
        private EditorState state;
        private GLControl lastFocus;
        private OpenTK.GLControl gl3DView;
        public EditorUI(Level level, HAMFile datafile)
        {
            InitializeComponent();
            this.gl3DView = new OpenTK.GLControl();
            //I love this pile of shit system that continually forgets basic things like "the GLControl is a toolbox item that exists in this project!"
            //and then you can't even actually load it from the binary because ??? so where did it load it from initally then?
            //fuck this
            this.gl3DView.BackColor = System.Drawing.Color.Black;
            this.gl3DView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gl3DView.Location = new System.Drawing.Point(635, 3);
            this.gl3DView.Name = "gl3DView";
            this.gl3DView.Size = new System.Drawing.Size(626, 334);
            this.gl3DView.TabIndex = 0;
            this.gl3DView.VSync = false;
            this.gl3DView.Load += new System.EventHandler(this.GLControlPerspective_Load);
            this.gl3DView.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.gl3DView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gl3DView_KeyDown);
            this.gl3DView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gl3DView_MouseDown);
            this.gl3DView.MouseEnter += new System.EventHandler(this.gl3DView_MouseEnter);
            this.gl3DView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gl3DView_MouseMove);
            this.gl3DView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gl3DView_MouseUp);

            /*this.tableLayoutPanel1.Controls.Add(this.glTopView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gl3DView, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.glFrontView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.glSideView, 1, 1);*/

            this.Controls.Add(this.gl3DView);

            state = new EditorState(level, datafile, this);
            gl3DView.Tag = new MineRender(state, gl3DView);
            state.AttachRenderer((MineRender)gl3DView.Tag);
            //sharedState.LevelData.BuildWorld();

        }

        private void GLControlPerspective_Load(object sender, EventArgs e)
        {
            GLControl control = (GLControl)sender;
            contextCreated = true;
            control.MakeCurrent();
            MineRender controlRenderer = (MineRender)control.Tag;
            controlRenderer.Init();
            controlRenderer.MakePerpectiveCamera((float)(Math.PI / 2), (float)control.Width / control.Height);
            //controlRenderer.LevelData.BuildWorld();
            //controlRenderer.LevelData.BuildWorldOutline();
            GL.Viewport(0, 0, control.Width, control.Height);
            GL.ClearColor(0.5f, 0.0f, 0.0f, 1.0f);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            GLControl control = (GLControl)sender;
            control.MakeCurrent();
            MineRender controlRenderer = (MineRender)control.Tag;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            controlRenderer.UpdateWorld();
            controlRenderer.DrawWorld();
            state.updateFlags = UpdateFlags.None;
            control.SwapBuffers();
        }

        private void GLControlOrtho_Load(object sender, EventArgs e)
        {
            GLControl control = (GLControl)sender;
            contextCreated = true;
            control.MakeCurrent();
            MineRender controlRenderer = (MineRender)control.Tag;
            controlRenderer.Init();
            //TODO: Make this less shit
            /*if (sender == glFrontView)
                controlRenderer.MakeOrthoCamera(100.0f, (float)control.Width / control.Height, 0);
            else if (sender == glTopView)
                controlRenderer.MakeOrthoCamera(100.0f, (float)control.Width / control.Height, 1);
            else if (sender == glSideView)
                controlRenderer.MakeOrthoCamera(100.0f, (float)control.Width / control.Height, 2);*/
            //controlRenderer.BuildWorld();
            //controlRenderer.BuildWorldOutline();
            GL.Viewport(0, 0, control.Width, control.Height);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        }

        private void GLControlOrtho_Paint(object sender, PaintEventArgs e)
        {
            GLControl control = (GLControl)sender;
            control.MakeCurrent();
            MineRender controlRenderer = (MineRender)control.Tag;
            //renderer2.SetProjection(glControl2.Width, glControl2.Height, (float)(90 * Math.PI / 180.0));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            //renderer2.DrawWorld();
            /*controlRenderer.DrawWorldOutline();
            controlRenderer.DrawWorldPoints();
            controlRenderer.DrawSelectedPoints();
            controlRenderer.DrawShadow();*/
            controlRenderer.DrawWorld();
            control.SwapBuffers();
        }

        private void gl3DView_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("mousedown");
            Control sendingControl = (Control)sender;
            InputEvent ev = new InputEvent(e.Button, true);
            ev.x = e.X; ev.y = e.Y;
            ev.w = sendingControl.Width; ev.h = sendingControl.Height;

            if (state.HandleEvent(ev)) return;
            if (((IInputEventHandler)sendingControl.Tag).HandleEvent(ev)) return;
        }

        private void gl3DView_MouseUp(object sender, MouseEventArgs e)
        {
            GLControl control = (GLControl)sender;
            Control sendingControl = (Control)sender;
            InputEvent ev = new InputEvent(e.Button, false);
            ev.x = e.X; ev.y = e.Y;
            ev.w = sendingControl.Width; ev.h = sendingControl.Height;

            if (state.HandleEvent(ev)) return;
            if (((IInputEventHandler)sendingControl.Tag).HandleEvent(ev)) return;
        }

        private void gl3DView_MouseEnter(object sender, EventArgs e)
        {
        }

        private void gl3DView_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("{0} got a KeyDown", sender);
            //need to set up key up before doing event
        }

        private void gl3DView_MouseMove(object sender, MouseEventArgs e)
        {
            GLControl control = (GLControl)sender;
            control.MakeCurrent();
            InputEvent ev = new InputEvent(e.X, e.Y);
            ev.w = control.Width; ev.h = control.Height;

            if (state.HandleEvent(ev)) return;
            if (((IInputEventHandler)control.Tag).HandleEvent(ev)) return;
        }

        public void InvalidateAll()
        {
            gl3DView.Invalidate();
        }
    }
}
