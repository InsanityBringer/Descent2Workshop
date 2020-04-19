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

using System.IO;
using System.Windows.Forms;
using LibDescent.Data;
using LibDescent.Edit;
using OpenTK;

namespace Descent2Workshop
{
    public partial class HXMEditor : Form
    {
        public EditorHXMFile datafile;
        public StandardUI host;
        private bool isLocked = false;
        private bool glContextCreated = false;
        private string currentFilename;

        private ModelTextureManager texMan = new ModelTextureManager();

        private List<ushort> ObjBitmaps = new List<ushort>();
        private List<ushort> ObjBitmapPtrs = new List<ushort>();

        private OpenTK.GLControl glControl1;
        private ModelRenderer modelRenderer;

        private Palette palette;

        private int ElementNumber { get { return (int)nudElementNum.Value; } }

        private EditorPanels.RobotPanel robotPanel;

        public HXMEditor(EditorHXMFile datafile, StandardUI host, string filename)
        {
            InitializeComponent();
            //can't use GLControls with designer? eh?
            glControl1 = new GLControl();
            glControl1.Location = new System.Drawing.Point(pictureBox3.Location.X, pictureBox3.Location.Y);
            glControl1.Size = pictureBox3.Size;
            glControl1.Load += glControl1_Load;
            glControl1.Paint += glControl1_Paint;
            glControl1.Visible = true;
            glControl1.Enabled = true;
            tabPage7.Controls.Add(glControl1);
            tabPage7.PerformLayout();
            pictureBox3.Enabled = false;
            pictureBox3.Visible = false;

            robotPanel = new EditorPanels.RobotPanel();
            robotPanel.Dock = DockStyle.Fill;
            RobotTabPage.Controls.Add(robotPanel);
            components.Add(robotPanel);

            this.datafile = datafile;
            this.host = host;
            palette = host.DefaultPalette;
            modelRenderer = new ModelRenderer(datafile.BaseHAM, host.DefaultPigFile, palette);
            this.Text = string.Format("{0} - HXM Editor", currentFilename);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isLocked = true;
            ResetMaxes();
            if (nudElementNum.Maximum != -1)
            {
                nudElementNum.Value = 0;
            }

            FillOutCurrentPanel(tabControl1.SelectedIndex, 0);

            isLocked = false;
        }

        private void ResetMaxes()
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    InitRobotPanel();
                    nudElementNum.Maximum = (decimal)datafile.replacedRobots.Count - 1;
                    if (datafile.replacedRobots.Count == 0) nudElementNum.Minimum = -1;
                    else nudElementNum.Minimum = 0;
                    break;
                case 1:
                    InitModelPanel();
                    nudElementNum.Maximum = (decimal)datafile.replacedModels.Count - 1;
                    if (datafile.replacedModels.Count == 0) nudElementNum.Minimum = -1;
                    else nudElementNum.Minimum = 0;
                    break;
            }
        }

        private void FillOutCurrentPanel(int id, int val)
        {
            switch (id)
            {
                case 0:
                    if (datafile.replacedRobots.Count != 0)
                    {
                        UpdateRobotPanel(val);
                    }
                    else
                    {
                        statusBar1.Text = "No robot elements in HXM";
                    }
                    break;
                case 1:
                    if (datafile.replacedModels.Count != 0)
                    {
                        UpdateModelPanel(val);
                        glControl1.Invalidate();
                        ReplacedElementComboBox.SelectedIndex = datafile.replacedModels[val].replacementID;
                    }
                    else
                    {
                        statusBar1.Text = "no model elements in HXM!";
                    }
                    break;
            }
        }

        private void InitRobotPanel()
        {
            List<string> RobotNames = new List<string>();
            List<string> ModelNames = new List<string>();
            List<string> WeaponNames = new List<string>();

            ReplacedElementComboBox.Items.Clear();
            for (int i = 0; i < 85; i++)
            {
                RobotNames.Add(datafile.GetRobotName(i));
                ReplacedElementComboBox.Items.Add(datafile.GetRobotName(i, true));
            }

            for (int i = 0; i < 200; i++)
            {
                ModelNames.Add(datafile.GetModelName(i));
            }
            for (int i = 0; i < datafile.GetNumWeapons(); i++)
            {
                WeaponNames.Add(datafile.GetWeaponName(i));
            }

            robotPanel.Init(datafile.BaseHAM.VClipNames, datafile.BaseHAM.SoundNames, RobotNames, WeaponNames, datafile.BaseHAM.PowerupNames, ModelNames);
            robotPanel.InitHXM(datafile);
        }

        public void UpdateRobotPanel(int num)
        {
            Robot robot = datafile.replacedRobots[num];
            robotPanel.Update(robot);

            ReplacedElementComboBox.SelectedIndex = robot.replacementID;
        }
        
        private void InitModelPanel()
        {
            cbModelLowDetail.Items.Clear(); cbModelLowDetail.Items.Add("None");
            cbModelDyingModel.Items.Clear(); cbModelDyingModel.Items.Add("None");
            cbModelDeadModel.Items.Clear(); cbModelDeadModel.Items.Add("None");
            for (int i = 0; i < datafile.GetNumModels(); i++)
            {
                cbModelLowDetail.Items.Add(datafile.GetModelName(i));
                cbModelDyingModel.Items.Add(datafile.GetModelName(i));
                cbModelDeadModel.Items.Add(datafile.GetModelName(i));
            }

            ReplacedElementComboBox.Items.Clear();
            for (int i = 0; i < 200; i++)
                ReplacedElementComboBox.Items.Add(datafile.GetModelName(i, true));
        }

        private void UpdateModelPanel(int num)
        {
            Polymodel model = datafile.replacedModels[(int)nudElementNum.Value];
            //Polymodel model = datafile.PolygonModels[num];
            txtModelNumModels.Text = model.NumSubmodels.ToString();
            txtModelDataSize.Text = model.ModelIDTASize.ToString();
            txtModelRadius.Text = model.Radius.ToString();
            txtModelTextureCount.Text = model.NumTextures.ToString();
            cbModelLowDetail.SelectedIndex = model.SimplerModels;
            cbModelDyingModel.SelectedIndex = model.DyingModelnum + 1;
            cbModelDeadModel.SelectedIndex = model.DeadModelnum + 1;

            /*txtModelMinX.Text = model.mins.x.ToString();
            txtModelMinY.Text = model.mins.y.ToString();
            txtModelMinZ.Text = model.mins.z.ToString();
            txtModelMaxX.Text = model.maxs.x.ToString();
            txtModelMaxY.Text = model.maxs.y.ToString();
            txtModelMaxZ.Text = model.maxs.z.ToString();

            txtElemName.Text = datafile.ModelNames[num];*/
            //if (!noPMView)
            {
                modelRenderer.SetModel(model);
                glControl1.Invalidate();
            }

            UpdateModelTexturePanel(model);
        }

        private void UpdateModelTexturePanel(Polymodel model)
        {
            int numNewTextures = datafile.CountUniqueObjBitmaps(model);
            ModelNumTextures.Text = numNewTextures.ToString();
            ModelNumPointers.Text = model.NumTextures.ToString();
            ModelBasePointerSpinner.Value = model.FirstTexture;
            ModelBaseTextureSpinner.Value = model.BaseTexture;

            for (int i = 0; i < numNewTextures; i++)
            {
                ushort index = datafile.GetObjBitmap(i + model.BaseTexture);
                if (datafile.BaseHAM.piggyFile.Bitmaps[index].IsAnimated)
                {
                    AnimatedWarningLabel.Visible = true;
                }
                else
                    AnimatedWarningLabel.Visible = false;
            }
        }

        private void nudElementNum_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                FillOutCurrentPanel(tabControl1.SelectedIndex, (int)nudElementNum.Value);
                isLocked = false;
            }
        }

        private void HXMEditor_Load(object sender, EventArgs e)
        {
            isLocked = true;
            ResetMaxes();
            FillOutCurrentPanel(0, 0);
            isLocked = false;
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            glContextCreated = true;
            modelRenderer.Init();
            SetupViewport();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!glContextCreated)
                return; //can't do anything with this, heh

            if (nudElementNum.Value < 0)
            {
                return;
            }

            if (datafile.replacedModels.Count == 0)
            {
                return; //can't do anything with no models. 
            }
            glControl1.MakeCurrent();

            modelRenderer.Pitch = (trackBar3.Value - 8) * -22.5d;
            modelRenderer.Angle = (trackBar1.Value - 8) * -22.5d;
            //modelRenderer.ShowBBs = chkShowBBs.Checked;
            //modelRenderer.ShowNormals = chkNorm.Checked;
            //modelRenderer.Wireframe = chkWireframe.Checked;
            //modelRenderer.ShowRadius = chkRadius.Checked;

            modelRenderer.Draw();
            glControl1.SwapBuffers();
        }

        private void SetupViewport()
        {
            modelRenderer.SetupViewport(glControl1.Width, glControl1.Height, trackBar2.Value * 0.5d + 4.0d);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            SetupViewport();
            glControl1.Invalidate();
        }

        //---------------------------------------------------------------------
        // MODEL UPDATORS
        //---------------------------------------------------------------------

        private void btnImportModel_Click(object sender, EventArgs e)
        {
            if (datafile.replacedModels.Count == 0) return;
            ImportModel(datafile.replacedModels[ElementNumber]);
        }

        private void btnExportModel_Click(object sender, EventArgs e)
        {
            if (datafile.replacedModels.Count == 0) return;
            saveFileDialog1.Filter = "Parallax Object Files|*.pof";
            saveFileDialog1.FileName = string.Format("model_{0}.pof", ElementNumber);
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                BinaryWriter bw = new BinaryWriter(File.Open(saveFileDialog1.FileName, FileMode.Create));
                POFWriter.SerializePolymodel(bw, datafile.replacedModels[ElementNumber], short.Parse(StandardUI.options.GetOption("PMVersion", "8")));
                bw.Close();
                bw.Dispose();
            }
        }

        private void ImportModel(Polymodel original)
        {
            int oldNumTextures = original.NumTextures;

            List<string> newTextureNames = new List<string>();
            openFileDialog1.Filter = "Parallax Object Files|*.pof";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string traceto = "";
                if (bool.Parse(StandardUI.options.GetOption("TraceModels", bool.FalseString)))
                {
                    string bareFilename = Path.GetFileName(openFileDialog1.FileName);
                    traceto = StandardUI.options.GetOption("TraceDir", ".") + Path.DirectorySeparatorChar + Path.ChangeExtension(bareFilename, "txt");
                }

                Polymodel model = POFReader.ReadPOFFile(openFileDialog1.FileName, traceto);
                model.ExpandSubmodels();
                //int numTextures = model.n_textures;
                //datafile.ReplaceModel(ElementNumber, model);
                datafile.replacedModels[ElementNumber] = model;
                model.replacementID = ReplacedElementComboBox.SelectedIndex;
                UpdateModelPanel(ElementNumber);
            }
        }

        private void ModelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            if (datafile.replacedModels.Count == 0) return;
            Polymodel model = datafile.replacedModels[ElementNumber];
            ComboBox comboBox = (ComboBox)sender;
            switch (comboBox.Tag)
            {
                case "1":
                    model.SimplerModels = (byte)comboBox.SelectedIndex;
                    break;
                case "2":
                    model.DyingModelnum = comboBox.SelectedIndex - 1;
                    break;
                case "3":
                    model.DeadModelnum = comboBox.SelectedIndex - 1;
                    break;
            }
        }


        private void FindPackButton_Click(object sender, EventArgs e)
        {
            if (datafile.replacedModels.Count == 0) return;
            Polymodel model = datafile.replacedModels[ElementNumber];
            //Okay, the logic here is that we can pack new object bitmaps past 422.
            //So long as you aren't using more than 178 entirely new textures, this
            //should work fairly well.
            //TODO: Needs to consider additional ObjBitmaps introduced by a V-HAM, perhaps
            int bestFit = VHAMFile.N_D2_OBJBITMAPS;
            int testTextures;
            foreach (Polymodel testModel in datafile.replacedModels)
            {
                testTextures = testModel.BaseTexture + datafile.CountUniqueObjBitmaps(testModel);
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

        private void ModelBaseTextureSpinner_ValueChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            if (datafile.replacedModels.Count == 0) return;
            Polymodel model = datafile.replacedModels[ElementNumber];
            model.BaseTexture = (int)ModelBaseTextureSpinner.Value;
        }

        private void ModelBasePointerSpinner_ValueChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            if (datafile.replacedModels.Count == 0) return;
            Polymodel model = datafile.replacedModels[ElementNumber];
            model.FirstTexture = (ushort)ModelBasePointerSpinner.Value;
        }

        //---------------------------------------------------------------------
        // GENERIC FUNCTIONS
        //---------------------------------------------------------------------
        private void ReplacedElementComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    {
                        if (datafile.replacedRobots.Count == 0) return;
                        Robot robot = datafile.replacedRobots[ElementNumber];
                        robot.replacementID = ReplacedElementComboBox.SelectedIndex;
                    }
                    break;
                case 1:
                    {
                        if (datafile.replacedModels.Count == 0) return;
                        Polymodel model = datafile.replacedModels[ElementNumber];
                        model.replacementID = ReplacedElementComboBox.SelectedIndex;
                    }
                    break;
            }
        }

        private void InsertButton_Click(object sender, EventArgs e)
        {
            int newelem = 0;
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    newelem = datafile.AddRobot();
                    break;
                case 1:
                    newelem = datafile.AddModel();
                    break;
            }

            ResetMaxes();
            nudElementNum.Value = newelem;
        }

        //---------------------------------------------------------------------
        // MENU FUNCTIONS
        //---------------------------------------------------------------------

        /// <summary>
        /// Helper function to save a HAM file, with lots of dumb error handling that doesn't work probably.
        /// </summary>
        /// <param name="filename">The filename to save the file to.</param>
        private void SaveHXMFile(string filename)
        {
            //Get rid of any old backups
            try
            {
                File.Delete(Path.ChangeExtension(filename, "BAK"));
            }
            catch (FileNotFoundException) { }
            catch (DirectoryNotFoundException) { } //Discover this with our face to avoid a 1/1000000 race condition
            catch (UnauthorizedAccessException exc)
            {
                host.AppendConsole(String.Format("Cannot delete old backup file {0}: Permission denied.\r\nMsg: {1}\r\n", Path.ChangeExtension(filename, "BAK"), exc.Message));
            }
            catch (IOException exc)
            {
                host.AppendConsole(String.Format("Cannot delete old backup file {0}: IO error occurred.\r\nMsg: {1}\r\n", Path.ChangeExtension(filename, "BAK"), exc.Message));
            }
            //Move the current file into the backup slot
            try
            {
                File.Move(filename, Path.ChangeExtension(filename, "BAK"));
            }
            catch (FileNotFoundException) { }
            catch (DirectoryNotFoundException) { } //Discover this with our face to avoid a 1/1000000 race condition
            catch (UnauthorizedAccessException exc)
            {
                host.AppendConsole(String.Format("Cannot move old HXM file {0}: Permission denied.\r\nMsg: {1}\r\n", filename, exc.Message));
            }
            catch (IOException exc)
            {
                host.AppendConsole(String.Format("Cannot move old HXM file {0}: IO error occurred.\r\nMsg: {1}\r\n", filename, exc.Message));
            }
            //Finally write the new file
            FileStream stream;
            try
            {
                stream = File.Open(filename, FileMode.Create);
                datafile.Write(stream);
                stream.Close();
                stream.Dispose();
            }
            catch (Exception exc)
            {
                FileUtilities.FileExceptionHandler(exc, "HXM file");
            }
        }

        private void menuItem3_Click_1(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "HXM Files|*.hxm";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != "")
                {
                    currentFilename = saveFileDialog1.FileName;
                    SaveHXMFile(saveFileDialog1.FileName);
                    this.Text = string.Format("{0} - HXM Editor", currentFilename);
                }
            }
        }

        private void MenuItem2_Click(object sender, EventArgs e)
        {
            if (currentFilename == "")
            {
                menuItem3_Click_1(sender, e);
            }
            else
            {
                SaveHXMFile(currentFilename);
            }
        }
    }
}
