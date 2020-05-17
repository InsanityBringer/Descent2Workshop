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

using LibDescent.Data;
using LibDescent.Edit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Descent2Workshop.EditorPanels;
using Descent2Workshop.Transactions;

namespace Descent2Workshop
{
    public partial class VHAMEditor : Form
    {
        private OpenTK.GLControl glControl1;
        private static HAMType[] typeTable = { HAMType.TMAPInfo, HAMType.VClip, HAMType.EClip, HAMType.WClip, HAMType.Robot, HAMType.Weapon,
            HAMType.Model, HAMType.Sound, HAMType.Reactor, HAMType.Powerup, HAMType.Ship, HAMType.Gauge, HAMType.Cockpit, HAMType.XLAT };
        public int[] texturelist;
        public EditorVHAMFile datafile;
        public StandardUI host;
        public bool isLocked = false;
        public bool glContextCreated = false;
        private ModelRenderer modelRenderer;
        private bool noPMView = false;
        private Palette palette;

        private RobotPanel robotPanel;
        private WeaponPanel weaponPanel;

        private TransactionManager transactionManager = new TransactionManager();

        public VHAMEditor(EditorVHAMFile data, StandardUI host)
        {
            InitializeComponent();
            this.glControl1 = new OpenTK.GLControl();
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.glControl1.Location = new System.Drawing.Point(452, 61);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(256, 256);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            robotPanel = new RobotPanel(transactionManager, 0);
            robotPanel.Dock = DockStyle.Fill;
            weaponPanel = new WeaponPanel();
            weaponPanel.Dock = DockStyle.Fill;
            components.Add(robotPanel); components.Add(weaponPanel); components.Add(glControl1);

            RobotTabPage.Controls.Add(robotPanel);
            WeaponTabPage.Controls.Add(weaponPanel);
            if (!noPMView)
                this.tabPage3.Controls.Add(this.glControl1);
            datafile = data;
            this.host = host;
            palette = host.DefaultPalette;
            modelRenderer = new ModelRenderer(datafile.BaseHAM, datafile.BaseHAM.piggyFile, palette);
        }

        private void VHAMEditor_Load(object sender, EventArgs e)
        {
            isLocked = true;
            ElementListInit();
            FillOutCurrentPanel(0, 0);
            isLocked = false;
        }

        private void ElementListInit()
        {
            switch (TabPages.SelectedIndex)
            {
                case 0:
                    nudElementNum.Maximum = datafile.Robots.Count - 1;
                    InitRobotPanel();
                    break;
                case 1:
                    nudElementNum.Maximum = datafile.Weapons.Count - 1;
                    InitWeaponPanel();
                    break;
                case 2:
                    nudElementNum.Maximum = datafile.Models.Count - 1;
                    InitModelPanel();
                    break;
            }
        }

        private void FillOutCurrentPanel(int id, int val)
        {
            switch (id)
            {
                case 0:
                    if (datafile.Robots.Count > 0)
                        UpdateRobotPanel(val);
                    else
                        statusBar1.Text = "No robots in VHAM.";
                    break;
                case 1:
                    if (datafile.Weapons.Count > 0)
                        UpdateWeaponPanel(val);
                    else
                        statusBar1.Text = "No weapons in VHAM.";
                    break;
                case 2:
                    if (datafile.Models.Count > 0)
                        UpdateModelPanel(val);
                    else
                        statusBar1.Text = "No models in VHAM.";
                    break;
            }
        }

        private void nudElementNum_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                FillOutCurrentPanel(TabPages.SelectedIndex, (int)nudElementNum.Value);
                isLocked = false;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isLocked = true;
            ElementListInit();
            nudElementNum.Value = 0;
            FillOutCurrentPanel(TabPages.SelectedIndex, 0);
            isLocked = false;
        }

        private void InitWeaponPanel()
        {
            List<string> weaponList = new List<string>();
            for (int i = 0; i < datafile.GetNumWeapons(); i++)
                weaponList.Add(datafile.GetWeaponName(i));
            List<string> modelList = new List<string>();
            for (int i = 0; i < datafile.GetNumModels(); i++)
                modelList.Add(datafile.GetModelName(i));
            weaponPanel.Init(datafile.BaseHAM.SoundNames, datafile.BaseHAM.VClipNames, weaponList, modelList, datafile.BaseHAM.piggyFile, palette);
        }

        private void InitRobotPanel()
        {
            List<string> robotList = new List<string>();
            for (int i = 0; i < datafile.GetNumRobots(); i++)
                robotList.Add(datafile.GetRobotName(i));
            List<string> weaponList = new List<string>();
            for (int i = 0; i < datafile.GetNumWeapons(); i++)
                weaponList.Add(datafile.GetWeaponName(i));
            List<string> modelList = new List<string>();
            for (int i = 0; i < datafile.GetNumModels(); i++)
                modelList.Add(datafile.GetModelName(i));

            robotPanel.Init(datafile.BaseHAM.VClipNames, datafile.BaseHAM.SoundNames, robotList, weaponList, datafile.BaseHAM.PowerupNames, modelList);
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
        }

        private void UpdateRobotDropTypes(int dropType, Robot robot)
        {
        }

        public void UpdateRobotPanel(int num)
        {
            Robot robot = datafile.Robots[num];
            robotPanel.Update(robot, num);

            txtElemName.Text = datafile.RobotNames[num];
        }

        public void UpdateWeaponPanel(int num)
        {
            Weapon weapon = datafile.Weapons[num];
            weaponPanel.Update(weapon);

            txtElemName.Text = datafile.WeaponNames[num];
        }

        private void UpdateModelPanel(int num)
        {
            Polymodel model = datafile.Models[num];
            txtModelNumModels.Text = model.NumSubmodels.ToString();
            txtModelDataSize.Text = model.ModelIDTASize.ToString();
            txtModelRadius.Text = ((float)(model.Radius)).ToString();
            txtModelTextureCount.Text = model.NumTextures.ToString();
            cbModelLowDetail.SelectedIndex = model.SimplerModels + 1;
            cbModelDyingModel.SelectedIndex = model.DyingModelnum + 1;
            cbModelDeadModel.SelectedIndex = model.DeadModelnum + 1;

            txtModelMinX.Text = ((float)(model.Mins.x)).ToString();
            txtModelMinY.Text = ((float)(model.Mins.y)).ToString();
            txtModelMinZ.Text = ((float)(model.Mins.z)).ToString();
            txtModelMaxX.Text = ((float)(model.Maxs.x)).ToString();
            txtModelMaxY.Text = ((float)(model.Maxs.y)).ToString();
            txtModelMaxZ.Text = ((float)(model.Maxs.z)).ToString();

            txtElemName.Text = datafile.ModelNames[num];
            if (!noPMView)
            {
                Polymodel mainmodel = datafile.Models[(int)nudElementNum.Value];
                modelRenderer.SetModel(mainmodel);
                glControl1.Invalidate();
            }
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            if (noPMView) return;
            if (!glContextCreated)
            {
                glControl1.Location = glControlStandin.Location;
                glControl1.Size = glControlStandin.Size;
                glControlStandin.Visible = false;
            }
            glContextCreated = true;
            modelRenderer.Init();
            SetupViewport();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (noPMView) return;
            if (!glContextCreated)
                return; //can't do anything with this, heh
            glControl1.MakeCurrent();

            modelRenderer.Pitch = (trackBar3.Value - 8) * -22.5d;
            modelRenderer.Angle = (trackBar1.Value - 8) * -22.5d;
            modelRenderer.ShowBBs = chkShowBBs.Checked;
            modelRenderer.ShowNormals = chkNorm.Checked;
            modelRenderer.Wireframe = chkWireframe.Checked;
            modelRenderer.ShowRadius = chkRadius.Checked;

            modelRenderer.Draw();
            glControl1.SwapBuffers();
        }

        private void SetupViewport()
        {
            if (noPMView) return;
            modelRenderer.SetupViewport(glControl1.Width, glControl1.Height, trackBar2.Value * 0.5d + 4.0d);
        }

        public double GetFloatFromFixed88(short fixedvalue)
        {
            return (double)fixedvalue / 256D;
        }

        private void ModelOrientationControl_Scroll(object sender, EventArgs e)
        {
            if (noPMView) return;
            SetupViewport();
            glControl1.Invalidate();
        }
    }
}
