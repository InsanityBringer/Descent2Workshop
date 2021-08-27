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
using Descent2Workshop.SaveHandlers;
using Descent2Workshop.Transactions;
using LibDescent.Data;
using LibDescent.Edit;

namespace Descent2Workshop
{
    public partial class HXMEditor : Form
    {
        public EditorHXMFile datafile;
        public StandardUI host;
        private bool isLocked = false;

        private ModelTextureManager texMan = new ModelTextureManager();

        private List<ushort> ObjBitmaps = new List<ushort>();
        private List<ushort> ObjBitmapPtrs = new List<ushort>();
        private ModelRenderer modelRenderer;

        private Palette palette;

        private int ElementNumber { get { return (int)ElementSpinner.Value; } }
        private int PageNumber { get { return EditorTabs.SelectedIndex; } }

        private EditorPanels.RobotPanel robotPanel;
        private EditorPanels.PolymodelPanel polymodelPanel;
        private TransactionManager transactionManager = new TransactionManager();

        private SaveHandler saveHandler;

        public HXMEditor(EditorHXMFile datafile, StandardUI host, SaveHandler saveHandler)
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

            string currentFilename = "Untitled";
            if (saveHandler != null)
                currentFilename = saveHandler.GetUIName();

            this.saveHandler = saveHandler;
            this.Text = string.Format("{0} - HXM Editor", currentFilename);

            transactionManager.undoEvent += DoUndoEvent;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isLocked = true;
            ResetMaxes();
            if (ElementSpinner.Maximum != -1)
            {
                ElementSpinner.Value = 0;
            }

            FillOutCurrentPanel(EditorTabs.SelectedIndex, 0);

            isLocked = false;
        }

        private void ResetMaxes()
        {
            switch (EditorTabs.SelectedIndex)
            {
                case 0:
                    InitRobotPanel();
                    ElementSpinner.Maximum = (decimal)datafile.ReplacedRobots.Count - 1;
                    if (datafile.ReplacedRobots.Count == 0) ElementSpinner.Minimum = -1;
                    else ElementSpinner.Minimum = 0;
                    break;
                case 1:
                    InitModelPanel();
                    ElementSpinner.Maximum = (decimal)datafile.ReplacedModels.Count - 1;
                    if (datafile.ReplacedModels.Count == 0) ElementSpinner.Minimum = -1;
                    else ElementSpinner.Minimum = 0;
                    break;
            }
        }

        private void FillOutCurrentPanel(int id, int val)
        {
            switch (id)
            {
                case 0:
                    if (datafile.ReplacedRobots.Count != 0)
                    {
                        robotPanel.Enabled = true;
                        UpdateRobotPanel(val);
                    }
                    else
                    {
                        robotPanel.Enabled = false;
                        statusBar1.Text = "No robot elements in HXM";
                    }
                    break;
                case 1:
                    if (datafile.ReplacedModels.Count != 0)
                    {
                        polymodelPanel.Enabled = true;
                        UpdateModelPanel(val);
                        ReplacedElementComboBox.SelectedIndex = datafile.ReplacedModels[val].ReplacementID;
                    }
                    else
                    {
                        polymodelPanel.Enabled = false;
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

            robotPanel.Init(datafile.BaseHAM.VClips, datafile.BaseHAM.SoundNames, RobotNames.ToArray(), WeaponNames.ToArray(), datafile.BaseHAM.Powerups, ModelNames.ToArray());
            robotPanel.InitHXM(datafile);
        }

        public void UpdateRobotPanel(int num)
        {
            Robot robot = datafile.ReplacedRobots[num];
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

            polymodelPanel.Init(names.ToArray());

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
            Polymodel model = datafile.ReplacedModels[(int)ElementSpinner.Value];
            polymodelPanel.Update(model, num);
        }

        private void nudElementNum_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                FillOutCurrentPanel(EditorTabs.SelectedIndex, (int)ElementSpinner.Value);
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
        // GENERIC FUNCTIONS
        //---------------------------------------------------------------------
        private void ReplacedElementComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            switch (EditorTabs.SelectedIndex)
            {
                case 0:
                    {
                        if (datafile.ReplacedRobots.Count == 0) return;
                        Robot robot = datafile.ReplacedRobots[ElementNumber];
                        robot.replacementID = ReplacedElementComboBox.SelectedIndex;
                    }
                    break;
                case 1:
                    {
                        if (datafile.ReplacedModels.Count == 0) return;
                        Polymodel model = datafile.ReplacedModels[ElementNumber];
                        model.ReplacementID = ReplacedElementComboBox.SelectedIndex;
                    }
                    break;
            }
        }

        private void InsertButton_Click(object sender, EventArgs e)
        {
            int newelem = ElementNumber + 1;
            switch (EditorTabs.SelectedIndex)
            {
                case 0:
                    {
                        ListAddTransaction transaction = new ListAddTransaction("New robot", datafile, "replacedRobots", newelem, new Robot(), ElementNumber, 0);
                        transactionManager.ApplyTransaction(transaction);
                    }
                    break;
                case 1:
                    {
                        ListAddTransaction transaction = new ListAddTransaction("New model", datafile, "replacedModels", newelem, new Polymodel(10), ElementNumber, 1);
                        transactionManager.ApplyTransaction(transaction);
                    }
                    break;
            }

            ResetMaxes();
            ElementSpinner.Value = newelem;
        }

        //---------------------------------------------------------------------
        // MENU FUNCTIONS
        //---------------------------------------------------------------------

        /// <summary>
        /// Helper function to save a HXM file, with lots of dumb error handling that doesn't work probably.
        /// </summary>
        private void SaveHXMFile()
        {
            if (saveHandler == null) //No save handler, so can't actually save right now
            {
                //HACK: Just call the save as handler for now... This needs restructuring
                SaveAsMenuItem_Click(this, new EventArgs());
                return; //and don't repeat the save code when we recall ourselves.
            }
            Stream stream = saveHandler.GetStream();
            if (stream == null)
            {
                MessageBox.Show(this, string.Format("Error opening save file {0}:\r\n{1}", saveHandler.GetUIName(), saveHandler.GetErrorMsg()));
            }
            else
            {
                datafile.Write(stream);
                if (saveHandler.FinalizeStream())
                {
                    MessageBox.Show(this, string.Format("Error writing save file {0}:\r\n{1}", saveHandler.GetUIName(), saveHandler.GetErrorMsg()));
                }
                else
                {
                    transactionManager.UnsavedFlag = false;
                }
            }
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "HXM Files|*.hxm";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != "")
                {
                    //Ensure the old save handler is detached from any UI properly.
                    if (saveHandler != null) saveHandler.Destroy();
                    saveHandler = new FileSaveHandler(saveFileDialog1.FileName);
                    SaveHXMFile();
                    this.Text = string.Format("{0} - HXM Editor", saveHandler.GetUIName());
                }
            }
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            if (saveHandler == null) //No save handler, so can't actually save right now
            {
                //HACK: Just call the save as handler for now... This needs restructuring
                SaveAsMenuItem_Click(sender, e);
            }
            SaveHXMFile();
        }

        private void HXMEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            string currentFilename = "Untitled";
            if (saveHandler != null)
                currentFilename = saveHandler.GetUIName();

            if (transactionManager.UnsavedFlag)
            {
                switch (MessageBox.Show(this, string.Format("Would you like to save changes to {0}?", currentFilename), "Unsaved changes", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        SaveHXMFile();
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void UndoMenuItem_Click(object sender, EventArgs e)
        {
            transactionManager.DoUndo();
        }

        private void RedoMenuItem_Click(object sender, EventArgs e)
        {
            transactionManager.DoRedo();
        }

        private void DoUndoEvent(object sender, UndoEventArgs e)
        {
            Transaction transaction = e.UndoneTransaction;
            if (transaction.Tab != PageNumber)
                EditorTabs.SelectedIndex = transaction.Tab;

            ResetMaxes();
            if (e.Redo)
            {
                if (transaction.Page != ElementNumber)
                    ElementSpinner.Value = transaction.RedoPage;
                else
                    FillOutCurrentPanel(transaction.Tab, transaction.RedoPage); //force an update
            }
            else
            {
                if (transaction.Page != ElementNumber)
                    ElementSpinner.Value = transaction.Page;
                else
                    FillOutCurrentPanel(transaction.Tab, transaction.Page); //force an update
            }
        }

    }
}
