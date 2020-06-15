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
            glControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.Controls.Add(glControl1);
            this.PerformLayout();

            this.model = model;
            if (this.model.isAnimated)
            {
                numericUpDown1.Minimum = 0;
                numericUpDown1.Maximum = Robot.NumAnimationStates - 1;
            }
            else
            {
                numericUpDown1.Enabled = false;
                chkAnimation.Enabled = false;
            }
            this.host = host;
            this.renderer = new ModelRenderer(host.DefaultPigFile, host.DefaultPalette);
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
            renderer.EmulateSoftware = chkSoftwareOverdraw.Checked;
            if (chkAnimation.Checked)
                renderer.Frame = (int)numericUpDown1.Value;
            else
                renderer.Frame = -1;

            renderer.Draw();

            glControl1.SwapBuffers();
        }

        private void SetupViewport()
        {
            double scale = 1 + ((double)trackBar3.Value * .5);
            renderer.SetupViewport(glControl1.Width, glControl1.Height, scale);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            glControl1.Invalidate();
        }

        private void chkRadius_CheckedChanged(object sender, EventArgs e)
        {
            glControl1.Invalidate();
        }

        private void PolymodelPreviewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            renderer.Close();
        }
    }
}
