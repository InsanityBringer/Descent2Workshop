﻿/*
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
        private bool isLocked = false;
        private OpenTK.GLControl ModelViewControl;
        private bool glContextCreated = false;
        private ModelRenderer modelRenderer;

        private int modelID;
        private Polymodel model;
        private EditorHXMFile hxmFile;

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

        public PolymodelPanel(TransactionManager transactionManager, int tabPage, PIGFile piggyFile, Palette palette, EditorHXMFile datafile)
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

            modelRenderer = new ModelRenderer(datafile.BaseHAM, piggyFile, palette);
            TextureGroupBox.Visible = true;
            hxmFile = datafile;
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

            modelRenderer.Draw((int)ModelNumSpinner.Value);
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


        public void Init(List<Polymodel> models)
        {
            cbModelDeadModel.Items.Clear(); cbModelDeadModel.Items.Add("None");
            cbModelDyingModel.Items.Clear(); cbModelDyingModel.Items.Add("None");
            cbModelLowDetail.Items.Clear(); cbModelLowDetail.Items.Add("None");

            string[] names = new string[models.Count];
            for (int i = 0; i < models.Count; i++)
                names[i] = models[i].Name;

            cbModelDeadModel.Items.AddRange(names);
            cbModelDyingModel.Items.AddRange(names);
            cbModelLowDetail.Items.AddRange(names);
        }

        public void Init(string[] names)
        {
            cbModelDeadModel.Items.Clear(); cbModelDeadModel.Items.Add("None");
            cbModelDyingModel.Items.Clear(); cbModelDyingModel.Items.Add("None");
            cbModelLowDetail.Items.Clear(); cbModelLowDetail.Items.Add("None");

            cbModelDeadModel.Items.AddRange(names);
            cbModelDyingModel.Items.AddRange(names);
            cbModelLowDetail.Items.AddRange(names);
        }

        public void ChangeOwnName(string newname)
        {
            cbModelDeadModel.Items[modelID+1] = newname;
            cbModelDyingModel.Items[modelID + 1] = newname;
            cbModelLowDetail.Items[modelID + 1] = newname;
        }

        public void Update(Polymodel model, int id)
        {
            isLocked = true;
            txtModelNumModels.Text = model.NumSubmodels.ToString();
            txtModelDataSize.Text = model.ModelIDTASize.ToString();
            txtModelRadius.Text = model.Radius.ToString();
            txtModelTextureCount.Text = model.NumTextures.ToString();
            UIUtil.SafeFillComboBox(cbModelLowDetail, model.SimplerModels);
            UIUtil.SafeFillComboBox(cbModelDyingModel, model.DyingModelnum + 1);
            UIUtil.SafeFillComboBox(cbModelDeadModel, model.DeadModelnum + 1);
            ModelNumSpinner.Minimum = -1;
            ModelNumSpinner.Value = -1;
            ModelNumSpinner.Maximum = model.NumSubmodels - 1;

            txtModelMinX.Text = model.Mins.X.ToString();
            txtModelMinY.Text = model.Mins.Y.ToString();
            txtModelMinZ.Text = model.Mins.Z.ToString();
            txtModelMaxX.Text = model.Maxs.X.ToString();
            txtModelMaxY.Text = model.Maxs.Y.ToString();
            txtModelMaxZ.Text = model.Maxs.Z.ToString();

            if (hxmFile != null)
            {
                int numNewTextures = hxmFile.CountUniqueObjBitmaps(model);
                ModelNumTextures.Text = numNewTextures.ToString();
                ModelNumPointers.Text = model.NumTextures.ToString();
                ModelBasePointerSpinner.Value = model.FirstTexture;
                ModelBaseTextureSpinner.Value = model.BaseTexture;

                for (int i = 0; i < numNewTextures; i++)
                {
                    ushort index = hxmFile.GetObjBitmap(i + model.BaseTexture);
                    if (hxmFile.BaseHAM.piggyFile.Bitmaps[index].IsAnimated)
                    {
                        AnimatedWarningLabel.Visible = true;
                    }
                    else
                        AnimatedWarningLabel.Visible = false;
                }
            }

            modelRenderer.SetModel(model);
            ModelViewControl.Invalidate();

            modelID = id;
            this.model = model;
            isLocked = false;
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

        private void FindPackButton_Click(object sender, EventArgs e)
        {
            if (hxmFile == null) return;
            if (hxmFile.ReplacedModels.Count == 0) return;
            Polymodel model = hxmFile.ReplacedModels[modelID];
            //Okay, the logic here is that we can pack new object bitmaps past 422.
            //So long as you aren't using more than 178 entirely new textures, this
            //should work fairly well.
            //TODO: Needs to consider additional ObjBitmaps introduced by a V-HAM, perhaps
            int bestFit = VHAMFile.NumDescent2ObjBitmaps;
            int testTextures;
            foreach (Polymodel testModel in hxmFile.ReplacedModels)
            {
                if (testModel == model) continue;
                testTextures = testModel.BaseTexture + hxmFile.CountUniqueObjBitmaps(testModel);
                if (bestFit < testTextures)
                {
                    bestFit = testTextures;
                }
            }
            if (bestFit >= 600)
            {
                bestFit = 0;
                MessageBox.Show("Cannot find a open slot beyond 422.");
            }
            model.BaseTexture = bestFit;
            isLocked = true;
            ModelBaseTextureSpinner.Value = bestFit;
            isLocked = false;
        }

        private void btnImportModel_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string traceto = "";
                if (bool.Parse(StandardUI.options.GetOption("TraceModels", bool.FalseString)))
                {
                    string bareFilename = Path.GetFileName(openFileDialog1.FileName);
                    traceto = StandardUI.options.GetOption("TraceDir", ".") + Path.DirectorySeparatorChar + Path.ChangeExtension(bareFilename, "txt");
                }

                Stream stream = File.OpenRead(openFileDialog1.FileName);
                Polymodel newmodel = POFReader.ReadPOFFile(stream);
                stream.Close();
                stream.Dispose();
                newmodel.ExpandSubmodels();
                //datafile.ReplaceModel(ElementNumber, model);
                ModelReplaceTransaction transaction = new ModelReplaceTransaction("Load model", (object)model, newmodel, modelID, tabPage);
                transactionManager.ApplyTransaction(transaction);
                Update(model, modelID);
            }
        }

        private void ModelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress) return;

            ComboBox control = (ComboBox)sender;

            int value = control.SelectedIndex;
            //hack because low detail is 1 based but dying/dead model nums are 0 based
            if (control != cbModelLowDetail)
            {
                value--;
            }
            IntegerTransaction transaction = new IntegerTransaction("Model property", model, (string)control.Tag, modelID, tabPage, value);
            transactionManager.ApplyTransaction(transaction);
        }

        private void ModelSpinner_ValueChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress) return;

            NumericUpDown spinner = (NumericUpDown)sender;

            int value = (int)spinner.Value;

            IntegerTransaction transaction = new IntegerTransaction("Model property", model, (string)spinner.Tag, modelID, tabPage, value);
            transactionManager.ApplyTransaction(transaction);
        }

        private void PartitionButton_Click(object sender, EventArgs e)
        {
            //Do in a copy to allow undo
            Polymodel newModel = new Polymodel(model);
            PolymodelBuilder builder = new PolymodelBuilder();
            try
            {
                builder.RebuildModel(newModel);
            }
            catch (ArgumentException exc)
            {
                MessageBox.Show(exc.Message, "Error partitioning model");
                return;
            }

            ModelReplaceTransaction transaction = new ModelReplaceTransaction("Load model", (object)model, newModel, modelID, tabPage);
            transactionManager.ApplyTransaction(transaction);
            Update(newModel, modelID);
        }

        private void ModelNumSpinner_ValueChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            ModelViewControl.Invalidate();
        }
    }
}
