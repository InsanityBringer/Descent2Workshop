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
using Descent2Workshop.Transactions;
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
        private string currentFilename;

        private ModelTextureManager texMan = new ModelTextureManager();

        private List<ushort> ObjBitmaps = new List<ushort>();
        private List<ushort> ObjBitmapPtrs = new List<ushort>();
        private ModelRenderer modelRenderer;

        private Palette palette;

        private int ElementNumber { get { return (int)nudElementNum.Value; } }

        private EditorPanels.RobotPanel robotPanel;
        private EditorPanels.PolymodelPanel polymodelPanel;
        private TransactionManager transactionManager = new TransactionManager();

        public HXMEditor(EditorHXMFile datafile, StandardUI host, string filename)
        {
            InitializeComponent();

            robotPanel = new EditorPanels.RobotPanel(transactionManager, 0);
            robotPanel.Dock = DockStyle.Fill;
            RobotTabPage.Controls.Add(robotPanel);
            components.Add(robotPanel);
            polymodelPanel = new EditorPanels.PolymodelPanel(transactionManager, 1, host.DefaultPigFile, host.DefaultPalette, datafile);
            polymodelPanel.Dock = DockStyle.Fill;
            ModelTabPage.Controls.Add(polymodelPanel);
            components.Add(polymodelPanel);

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
                        robotPanel.Visible = true;
                        UpdateRobotPanel(val);
                    }
                    else
                    {
                        robotPanel.Visible = false;
                        statusBar1.Text = "No robot elements in HXM";
                    }
                    break;
                case 1:
                    if (datafile.replacedModels.Count != 0)
                    {
                        polymodelPanel.Visible = true;
                        UpdateModelPanel(val);
                        ReplacedElementComboBox.SelectedIndex = datafile.replacedModels[val].replacementID;
                    }
                    else
                    {
                        polymodelPanel.Visible = false;
                        statusBar1.Text = "no model elements in HXM";
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
            robotPanel.Update(robot, num);

            ReplacedElementComboBox.SelectedIndex = robot.replacementID;
        }
        
        private void InitModelPanel()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < datafile.GetNumModels(); i++)
            {
                names.Add(datafile.GetModelName(i));
            }

            polymodelPanel.Init(names);

            names.Clear();
            //annoying hack, need a different list for the replacement box
            for (int i = 0; i < datafile.GetNumModels(); i++)
            {
                names.Add(datafile.GetModelName(i, true));
            }
            string[] nameArray = names.ToArray();

            ReplacedElementComboBox.Items.Clear();
            ReplacedElementComboBox.Items.AddRange(nameArray);
        }

        private void UpdateModelPanel(int num)
        {
            Polymodel model = datafile.replacedModels[(int)nudElementNum.Value];
            polymodelPanel.Update(model, num);
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

        //---------------------------------------------------------------------
        // MODEL UPDATORS
        //---------------------------------------------------------------------

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
