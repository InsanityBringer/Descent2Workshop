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
using PiggyDump.Editor.Render;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace PiggyDump.Editor
{
    public partial class EditorUI : Form
    {
        private bool contextCreated = false;
        private Level level;
        private HAMFile datafile;
        private SharedRendererState sharedState;
        private bool testDown = false;
        private int testX = 0, testY = 0;
        private GLControl lastFocus;
        private int focusMouseX, focusMouseY;
        private LevelTransform transform;
        private OpenTK.GLControl gl3DView;
        private OpenTK.GLControl glTopView;
        private OpenTK.GLControl glFrontView;
        private OpenTK.GLControl glSideView;
        public EditorUI(Level level, HAMFile datafile)
        {
            InitializeComponent();
            this.gl3DView = new OpenTK.GLControl();
            this.glTopView = new OpenTK.GLControl();
            this.glFrontView = new OpenTK.GLControl();
            this.glSideView = new OpenTK.GLControl();
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
            this.glTopView.BackColor = System.Drawing.Color.Black;
            this.glTopView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glTopView.Location = new System.Drawing.Point(3, 3);
            this.glTopView.Name = "glTopView";
            this.glTopView.Size = new System.Drawing.Size(626, 334);
            this.glTopView.TabIndex = 1;
            this.glTopView.VSync = false;
            this.glTopView.Load += new System.EventHandler(this.GLControlOrtho_Load);
            this.glTopView.Paint += new System.Windows.Forms.PaintEventHandler(this.GLControlOrtho_Paint);
            this.glTopView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gl3DView_KeyDown);
            this.glTopView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gl3DView_MouseDown);
            this.glTopView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gl3DView_MouseMove);
            this.glTopView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gl3DView_MouseUp);
            this.glFrontView.BackColor = System.Drawing.Color.Black;
            this.glFrontView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glFrontView.Location = new System.Drawing.Point(3, 343);
            this.glFrontView.Name = "glFrontView";
            this.glFrontView.Size = new System.Drawing.Size(626, 335);
            this.glFrontView.TabIndex = 2;
            this.glFrontView.VSync = false;
            this.glFrontView.Load += new System.EventHandler(this.GLControlOrtho_Load);
            this.glFrontView.Paint += new System.Windows.Forms.PaintEventHandler(this.GLControlOrtho_Paint);
            this.glFrontView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gl3DView_KeyDown);
            this.glFrontView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gl3DView_MouseDown);
            this.glFrontView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gl3DView_MouseMove);
            this.glFrontView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gl3DView_MouseUp);
            this.glSideView.BackColor = System.Drawing.Color.Black;
            this.glSideView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glSideView.Location = new System.Drawing.Point(635, 343);
            this.glSideView.Name = "glSideView";
            this.glSideView.Size = new System.Drawing.Size(626, 335);
            this.glSideView.TabIndex = 3;
            this.glSideView.VSync = false;
            this.glSideView.Load += new System.EventHandler(this.GLControlOrtho_Load);
            this.glSideView.Paint += new System.Windows.Forms.PaintEventHandler(this.GLControlOrtho_Paint);
            this.glSideView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gl3DView_KeyDown);
            this.glSideView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gl3DView_MouseDown);
            this.glSideView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gl3DView_MouseMove);
            this.glSideView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gl3DView_MouseUp);

            this.tableLayoutPanel1.Controls.Add(this.glTopView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gl3DView, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.glFrontView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.glSideView, 1, 1);
            //what a pile of shit
            this.level = level;
            this.datafile = datafile;
            sharedState = new SharedRendererState(level);
            transform = new LevelTransform(level, sharedState);
            sharedState.BuildWorld();
            gl3DView.Tag = new MineRender(level, datafile, sharedState, gl3DView);
            glTopView.Tag = new MineRender(level, datafile, sharedState, glTopView);
            glSideView.Tag = new MineRender(level, datafile, sharedState, glSideView);
            glFrontView.Tag = new MineRender(level, datafile, sharedState, glFrontView);
            sharedState.AddRenderer((MineRender)gl3DView.Tag);
            sharedState.AddRenderer((MineRender)glTopView.Tag);
            sharedState.AddRenderer((MineRender)glSideView.Tag);
            sharedState.AddRenderer((MineRender)glFrontView.Tag);
        }

        private void GLControlPerspective_Load(object sender, EventArgs e)
        {
            GLControl control = (GLControl)sender;
            contextCreated = true;
            control.MakeCurrent();
            MineRender controlRenderer = (MineRender)control.Tag;
            controlRenderer.Init();
            controlRenderer.MakePerpectiveCamera((float)(Math.PI / 2), (float)control.Width / control.Height);
            controlRenderer.BuildWorld();
            controlRenderer.BuildWorldOutline();
            GL.Viewport(0, 0, control.Width, control.Height);
            GL.ClearColor(0.5f, 0.0f, 0.0f, 1.0f);
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            GLControl control = (GLControl)sender;
            control.MakeCurrent();
            MineRender controlRenderer = (MineRender)control.Tag;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            controlRenderer.DrawWorld();
            controlRenderer.DrawWorldOutline();
            controlRenderer.DrawWorldPoints();
            controlRenderer.DrawSelectedPoints();
            controlRenderer.DrawShadow();
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
            if (sender == glFrontView)
                controlRenderer.MakeOrthoCamera(100.0f, (float)control.Width / control.Height, 0);
            else if (sender == glTopView)
                controlRenderer.MakeOrthoCamera(100.0f, (float)control.Width / control.Height, 1);
            else if (sender == glSideView)
                controlRenderer.MakeOrthoCamera(100.0f, (float)control.Width / control.Height, 2);
            controlRenderer.BuildWorld();
            controlRenderer.BuildWorldOutline();
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
            controlRenderer.DrawWorldOutline();
            controlRenderer.DrawWorldPoints();
            controlRenderer.DrawSelectedPoints();
            controlRenderer.DrawShadow();
            control.SwapBuffers();
        }

        private void gl3DView_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("mousedown");
            testX = e.X; testY = e.Y;
            testDown = true;
        }

        private void gl3DView_MouseUp(object sender, MouseEventArgs e)
        {
            GLControl control = (GLControl)sender;
            MineRender controlRenderer = (MineRender)control.Tag;
            Console.WriteLine("mouseup {0} {1}", e.X, e.Y);
            testDown = false;
            gl3DView.Invalidate();
            glTopView.Invalidate();
            glSideView.Invalidate();
            glFrontView.Invalidate();
            if (e.Button == MouseButtons.Left)
            {
                if (sender == lastFocus)
                {
                    if (transform.Type != TransformType.None)
                    {
                        transform.FinalizeTransform();
                        return;
                    }
                }
                controlRenderer.testPick(((float)e.X / control.Size.Width) * 2f - 1f, ((float)e.Y / control.Size.Height) * 2f - 1f);
            }
        }

        private void gl3DView_MouseEnter(object sender, EventArgs e)
        {
            if (transform.Type == TransformType.None)
            {
                lastFocus = (GLControl)sender;
            }
        }

        private void gl3DView_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("{0} got a KeyDown", sender);
            /*if (sender is GLControl)
                e.Handled = false;*/
            if (e.KeyCode == Keys.G)
            {
                if (lastFocus != null)
                {
                    transform.InitTransform(sharedState.SelectedVertices);
                    Vector3 side, up;
                    MineRender renderer = (MineRender)lastFocus.Tag;
                    renderer.GetCameraUpSide(out up, out side);
                    transform.InitTranslation(side, up, focusMouseX, focusMouseY);
                }
            }
        }

        private void gl3DView_MouseMove(object sender, MouseEventArgs e)
        {
            GLControl control = (GLControl)sender;
            control.MakeCurrent();
            if (testDown)
            {
                int deltaX = e.X - testX;
                int deltaY = e.Y - testY;
                //Console.WriteLine("{0} {1}", deltaX, deltaY);
                int moveType = 0;
                if (e.Button == MouseButtons.Middle)
                    moveType = 1;
                ((MineRender)control.Tag).testMouseMove(-deltaX, -deltaY, moveType);

                testX = e.X;
                testY = e.Y;

                control.Invalidate();
            }
            if (sender == lastFocus)
            {
                focusMouseX = e.X; focusMouseY = e.Y;
                if (transform.Type != TransformType.None)
                {
                    transform.MouseMove(e.X, e.Y);
                    control.Invalidate();
                }
            }
        }
    }
}
