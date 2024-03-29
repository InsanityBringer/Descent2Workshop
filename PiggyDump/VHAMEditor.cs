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
        private static HAMType[] typeTable = { HAMType.TMAPInfo, HAMType.VClip, HAMType.EClip, HAMType.WClip, HAMType.Robot, HAMType.Weapon,
            HAMType.Model, HAMType.Sound, HAMType.Reactor, HAMType.Powerup, HAMType.Ship, HAMType.Gauge, HAMType.Cockpit, HAMType.XLAT };
        public int[] texturelist;
        public EditorVHAMFile datafile;
        public StandardUI host;
        public bool isLocked = false;
        public bool glContextCreated = false;
        private Palette palette;

        private RobotPanel robotPanel;
        private WeaponPanel weaponPanel;
        private PolymodelPanel polymodelPanel;

        private TransactionManager transactionManager = new TransactionManager();

        public VHAMEditor(EditorVHAMFile data, StandardUI host)
        {
            InitializeComponent();
            robotPanel = new RobotPanel(transactionManager, 0);
            robotPanel.Dock = DockStyle.Fill;
            weaponPanel = new WeaponPanel(transactionManager, 1);
            weaponPanel.Dock = DockStyle.Fill;
            polymodelPanel = new PolymodelPanel(transactionManager, 2, host.DefaultPigFile, host.DefaultPalette, data.BaseHAM);
            polymodelPanel.Dock = DockStyle.Fill;
            components.Add(robotPanel); components.Add(weaponPanel); components.Add(polymodelPanel);

            RobotTabPage.Controls.Add(robotPanel);
            WeaponTabPage.Controls.Add(weaponPanel);
            ModelTabPage.Controls.Add(polymodelPanel);
            datafile = data;
            this.host = host;
            palette = host.DefaultPalette;
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
            weaponPanel.Init(datafile.BaseHAM.SoundNames, datafile.BaseHAM.VClips, weaponList.ToArray(), modelList.ToArray(), datafile.BaseHAM.piggyFile, palette);
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

            robotPanel.Init(datafile.BaseHAM.VClips, datafile.BaseHAM.SoundNames, robotList.ToArray(), weaponList.ToArray(), datafile.BaseHAM.Powerups, modelList.ToArray());
        }

        private void InitModelPanel()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < datafile.GetNumModels(); i++)
            {
                names.Add(datafile.GetModelName(i));
            }
            polymodelPanel.Init(names.ToArray());
        }

        public void UpdateRobotPanel(int num)
        {
            Robot robot = datafile.Robots[num];
            robotPanel.Update(robot, num);

            txtElemName.Text = datafile.Robots[num].Name;
        }

        public void UpdateWeaponPanel(int num)
        {
            Weapon weapon = datafile.Weapons[num];
            weaponPanel.Update(weapon, num);

            txtElemName.Text = datafile.Weapons[num].Name;
        }

        private void UpdateModelPanel(int num)
        {
            Polymodel model = datafile.Models[num];
            polymodelPanel.Update(model, num);

            txtElemName.Text = datafile.Models[num].Name;
        }
    }
}
