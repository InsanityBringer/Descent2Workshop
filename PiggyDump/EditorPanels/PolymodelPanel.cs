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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Descent2Workshop.Transactions;
using LibDescent.Data;
using LibDescent.Edit;

namespace Descent2Workshop.EditorPanels
{
    public partial class PolymodelPanel : UserControl
    {
        private OpenTK.GLControl ModelViewControl;
        private bool glContextCreated = false;
        private ModelRenderer modelRenderer;

        private int modelID;
        private Polymodel model;

        private TransactionManager transactionManager;
        private int tabPage;

        public PolymodelPanel(TransactionManager transactionManager, int tabPage, PIGFile piggyFile, Palette palette, EditorHAMFile datafile = null)
        {
            InitializeComponent();
            this.transactionManager = transactionManager;
            this.tabPage = tabPage;

            //Create the model viewer baround the template control
            ModelViewControl = new OpenTK.GLControl();
            ModelViewControl.BackColor = System.Drawing.Color.Black;
            ModelViewControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            ModelViewControl.Location = new System.Drawing.Point(452, 61);
            ModelViewControl.Anchor = GLControlStandin.Anchor;
            ModelViewControl.Name = "glControl1";
            ModelViewControl.Size = new System.Drawing.Size(256, 256);
            ModelViewControl.TabIndex = 0;
            ModelViewControl.VSync = false;
            ModelViewControl.Load += new System.EventHandler(this.ModelViewControl_Load);
            ModelViewControl.Paint += new System.Windows.Forms.PaintEventHandler(this.ModelViewControl_Paint);
            Controls.Add(ModelViewControl);

            if (datafile != null)
                modelRenderer = new ModelRenderer(datafile, piggyFile, palette);
            else
                modelRenderer = new ModelRenderer(piggyFile, palette);
        }

        private void ModelViewControl_Load(object sender, EventArgs e)
        {
            if (!glContextCreated)
            {
                ModelViewControl.Location = GLControlStandin.Location;
                ModelViewControl.Size = GLControlStandin.Size;
                GLControlStandin.Visible = false;
            }
            glContextCreated = true;
            modelRenderer.Init();
            SetupViewport();
        }

        private void ModelViewControl_Paint(object sender, PaintEventArgs e)
        {
            if (!glContextCreated)
                return; //can't do anything with this, heh
            ModelViewControl.MakeCurrent();

            SetupViewport();

            modelRenderer.Pitch = (PitchTrackBar.Value - 8) * -22.5d;
            modelRenderer.Angle = (AngleTrackBar.Value - 8) * -22.5d;
            modelRenderer.ShowBBs = ShowBBCheckBox.Checked;
            modelRenderer.ShowNormals = ShowNormCheckBox.Checked;
            modelRenderer.Wireframe = WireframeCheckBox.Checked;
            modelRenderer.ShowRadius = ShowRadiusCheckBox.Checked;
            modelRenderer.EmulateSoftware = NoDepthCheckBox.Checked;

            modelRenderer.Draw();
            ModelViewControl.SwapBuffers();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            SetupViewport();
            ModelViewControl.Invalidate();
        }

        private void SetupViewport()
        {
            modelRenderer.SetupViewport(ModelViewControl.Width, ModelViewControl.Height, ZoomTrackBar.Value * 0.5d + 4.0d);
        }


        public void Init(List<string> modelNames)
        {
            cbModelDeadModel.Items.Clear(); cbModelDeadModel.Items.Add("None");
            cbModelDyingModel.Items.Clear(); cbModelDyingModel.Items.Add("None");
            cbModelLowDetail.Items.Clear(); cbModelLowDetail.Items.Add("None");

            string[] names = modelNames.ToArray();

            cbModelDeadModel.Items.AddRange(names);
            cbModelDyingModel.Items.AddRange(names);
            cbModelLowDetail.Items.AddRange(names);
        }

        public void Update(Polymodel model, int id)
        {
            txtModelNumModels.Text = model.NumSubmodels.ToString();
            txtModelDataSize.Text = model.ModelIDTASize.ToString();
            txtModelRadius.Text = model.Radius.ToString();
            txtModelTextureCount.Text = model.NumTextures.ToString();
            cbModelLowDetail.SelectedIndex = model.SimplerModels;
            cbModelDyingModel.SelectedIndex = model.DyingModelnum + 1;
            cbModelDeadModel.SelectedIndex = model.DeadModelnum + 1;

            txtModelMinX.Text = model.Mins.x.ToString();
            txtModelMinY.Text = model.Mins.y.ToString();
            txtModelMinZ.Text = model.Mins.z.ToString();
            txtModelMaxX.Text = model.Maxs.x.ToString();
            txtModelMaxY.Text = model.Maxs.y.ToString();
            txtModelMaxZ.Text = model.Maxs.z.ToString();

            modelRenderer.SetModel(model);
            ModelViewControl.Invalidate();

            modelID = id;
            this.model = model;
        }

        private void ModelViewProperty_Changed(object sender, EventArgs e)
        {
            ModelViewControl.Invalidate();
        }

        private void btnExportModel_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Parallax Object Files|*.pof";
            saveFileDialog1.FileName = string.Format("model_{0}.pof", modelID);
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                BinaryWriter bw = new BinaryWriter(File.Open(saveFileDialog1.FileName, FileMode.Create));
                POFWriter.SerializePolymodel(bw, model, short.Parse(StandardUI.options.GetOption("PMVersion", "8")));
                bw.Close();
                bw.Dispose();
            }
        }
    }
}
