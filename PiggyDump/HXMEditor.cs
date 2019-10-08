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
//using OpenTK.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace PiggyDump
{
    public partial class HXMEditor : Form
    {
        public HXMFile datafile;
        public StandardUI host;
        private bool isLocked = false;
        private bool glContextCreated = false;

        private ModelTextureManager texMan = new ModelTextureManager();

        private List<ushort> ObjBitmaps = new List<ushort>();
        private List<ushort> ObjBitmapPtrs = new List<ushort>();

        private OpenTK.GLControl glControl1;
        private ModelRenderer modelRenderer;

        public HXMEditor(HXMFile datafile, StandardUI host)
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

            this.datafile = datafile;
            this.host = host;
            modelRenderer = new ModelRenderer(datafile.baseFile, host.DefaultPigFile);

            BuildObjBitmapsTable();
        }

        public void BuildObjBitmapsTable()
        {
        }

        public double GetFloatFromFixed(int fixedvalue)
        {
            return (double)fixedvalue / 65536D;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isLocked = true;
            resetMaxes();
            if (nudElementNum.Maximum != -1)
            {
                nudElementNum.Value = 0;
            }

            FillOutCurrentPanel(tabControl1.SelectedIndex, 0);

            isLocked = false;
        }

        private void resetMaxes()
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    nudElementNum.Maximum = (decimal)datafile.replacedRobots.Count - 1;
                    break;
                case 1:
                    nudElementNum.Maximum = (decimal)datafile.replacedModels.Count - 1;
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
                        UpdateModel(val);
                        glControl1.Invalidate();
                        tbReplacementElement.Text = datafile.replacedModels[val].replacementID.ToString();
                    }
                    else
                    {
                        statusBar1.Text = "no model elements in HXM!";
                    }
                    break;
            }
        }

        public void UpdateRobotPanel(int num)
        {
            Robot robot = datafile.replacedRobots[num];
            txtRobotDrag.Text = GetFloatFromFixed(robot.drag).ToString();
            txtRobotDropProb.Text = robot.contains_prob.ToString();
            txtRobotDrops.Text = robot.contains_count.ToString();
            txtRobotLight.Text = GetFloatFromFixed(robot.lighting).ToString();
            txtRobotMass.Text = GetFloatFromFixed(robot.mass).ToString();
            txtRobotScore.Text = robot.score_value.ToString();
            txtRobotShield.Text = GetFloatFromFixed(robot.strength).ToString();
            txtRobotAim.Text = robot.aim.ToString();
            txtRobotBadass.Text = robot.badass.ToString();
            txtRobotDeathBlobs.Text = robot.smart_blobs.ToString();
            txtRobotDeathRolls.Text = robot.death_roll.ToString();
            txtRobotEnergyDrain.Text = robot.energy_drain.ToString();
            txtRobotHitBlobs.Text = robot.energy_blobs.ToString();
            txtRobotGlow.Text = robot.glow.ToString();
            txtRobotPursuit.Text = robot.pursuit.ToString();
            txtRobotLightcast.Text = robot.lightcast.ToString();
            if (robot.behavior >= 128)
            {
                cbRobotAI.SelectedIndex = robot.behavior - 128;
            }
            else cbRobotAI.SelectedIndex = 0;
            cbRobotCompanion.Checked = robot.companion != 0;
            cbRobotClaw.Checked = robot.attack_type != 0;
            cbRobotThief.Checked = robot.thief != 0;
            cbKamikaze.Checked = robot.kamikaze != 0;

            int dropType = robot.contains_type;
            if (dropType == 2)
            {
                dropType = 1;
            }
            else
            {
                dropType = 0;
            }

            int bossMode = robot.boss_flag;
            if (bossMode > 20)
            {
                bossMode -= 18;
            }
            cmRobotBoss.SelectedIndex = bossMode;
            cmRobotCloak.SelectedIndex = robot.cloak_type;

            cbRobotDropType.SelectedIndex = dropType;

            nudRobotAI.Value = 0;
            updateRobotAI(0);

            tbReplacementElement.Text = robot.replacementID.ToString();
        }

        private void updateRobotAI(int num)
        {
            Robot robot = datafile.replacedRobots[(int)nudElementNum.Value];

            txtRobotCircleDist.Text = GetFloatFromFixed(robot.circle_distance[num]).ToString();
            txtRobotEvadeSpeed.Text = robot.evade_speed[num].ToString();
            txtRobotFireDelay.Text = GetFloatFromFixed(robot.firing_wait[num]).ToString();
            txtRobotFireDelay2.Text = GetFloatFromFixed(robot.firing_wait2[num]).ToString();
            txtRobotFOV.Text = GetFloatFromFixed(robot.field_of_view[num]).ToString();
            txtRobotMaxSpeed.Text = GetFloatFromFixed(robot.max_speed[num]).ToString();
            txtRobotTurnSpeed.Text = GetFloatFromFixed(robot.turn_time[num]).ToString();
            txtRobotShotCount.Text = robot.rapidfire_count[num].ToString();
        }

        private void UpdateModel(int num)
        {
            Polymodel mainmodel = datafile.replacedModels[(int)nudElementNum.Value];
            modelRenderer.SetModel(mainmodel);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    datafile.replacedRobots.Add(new Robot());
                    resetMaxes();
                    if (nudElementNum.Value == -1)
                    {
                        nudElementNum.Value = 0;
                    }
                    break;
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
            resetMaxes();
            FillOutCurrentPanel(0, 0);
            isLocked = false;
        }

        private void nudRobotAI_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                updateRobotAI((int)nudRobotAI.Value);
                isLocked = false;
            }
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

            modelRenderer.Angle = (trackBar3.Value - 8) * -22.5d;
            modelRenderer.Pitch = (trackBar1.Value - 8) * -22.5d;
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

        #region robot updators
        private void txtRobotModel_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
            {
                return;
            }
            try
            {
                TextBox sendingControl = (TextBox)sender;
                string tagstr = (string)sendingControl.Tag;
                int tagvalue = Int32.Parse(tagstr);

                int value = Int32.Parse(sendingControl.Text);

                Robot robot = datafile.GetRobotAt((int)nudElementNum.Value);

                robot.UpdateRobot(tagvalue, ref value, (int)nudRobotAI.Value, 0, null);

                datafile.replacedRobots[(int)nudElementNum.Value] = robot;
                statusBar1.Text = "data update successful!";
            }
            catch (Exception)
            {
                //silently fail, don't update data
                statusBar1.Text = "failed to update data!";
            }
        }

        private void fixedRobotElemChange_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
            {
                return;
            }
            try
            {
                TextBox sendingControl = (TextBox)sender;
                string tagstr = (string)sendingControl.Tag;
                int tagvalue = Int32.Parse(tagstr);

                float fvalue = Single.Parse(sendingControl.Text);
                int value = (int)(fvalue * 65536f);

                Robot robot = datafile.GetRobotAt((int)nudElementNum.Value);

                robot.UpdateRobot(tagvalue, ref value, (int)nudRobotAI.Value, 0, null);

                datafile.replacedRobots[(int)nudElementNum.Value] = robot;
                statusBar1.Text = "data update successful!";
            }
            catch (Exception)
            {
                //silently fail, don't update data
                statusBar1.Text = "failed to update data!";
            }
        }

        private void cmRobotCloak_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            Robot robot = datafile.GetRobotAt((int)nudElementNum.Value);
            robot.cloak_type = (sbyte)cmRobotCloak.SelectedIndex;
            datafile.replacedRobots[(int)nudElementNum.Value] = robot;
            statusBar1.Text = "data update successful!";
        }

        private void cmRobotBoss_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            Robot robot = datafile.GetRobotAt((int)nudElementNum.Value);
            int bosstype = cmRobotBoss.SelectedIndex;
            if (bosstype >= 3)
            {
                bosstype += 16;
            }
            robot.boss_flag = (sbyte)bosstype;
            datafile.replacedRobots[(int)nudElementNum.Value] = robot;
            statusBar1.Text = "data update successful!";
        }

        private void cbRobotAI_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbRobotThief_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            Robot robot = datafile.GetRobotAt((int)nudElementNum.Value);
            if (cbRobotThief.Checked)
                robot.thief = 1;
            else robot.thief = 0;
            datafile.replacedRobots[(int)nudElementNum.Value] = robot;
            statusBar1.Text = "data update successful!";
        }


        private void cbKamikaze_CheckedChanged(object sender, EventArgs e)
        {

        }

        #endregion

        private void menuItem3_Click_1(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != "")
                {
                    datafile.SaveDataFile(saveFileDialog1.FileName);
                }
            }
        }
    }
}
