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

namespace Descent2Workshop
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

        private int ElementNumber { get { return (int)nudElementNum.Value; } }

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
                    break;
                case 1:
                    InitModelPanel();
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
                        UpdateModelPanel(val);
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

        private void InitRobotPanel()
        {
            cbRobotAttackSound.Items.Clear();
            cbRobotClawSound.Items.Clear();
            cbRobotDyingSound.Items.Clear();
            cbRobotSeeSound.Items.Clear();
            cbRobotTauntSound.Items.Clear();
            cbRobotHitSound.Items.Clear();
            cbRobotDeathSound.Items.Clear();
            for (int i = 0; i < datafile.baseFile.Sounds.Count; i++)
            {
                cbRobotAttackSound.Items.Add(datafile.baseFile.SoundNames[i]);
                cbRobotClawSound.Items.Add(datafile.baseFile.SoundNames[i]);
                cbRobotDyingSound.Items.Add(datafile.baseFile.SoundNames[i]);
                cbRobotSeeSound.Items.Add(datafile.baseFile.SoundNames[i]);
                cbRobotTauntSound.Items.Add(datafile.baseFile.SoundNames[i]);
                cbRobotHitSound.Items.Add(datafile.baseFile.SoundNames[i]);
                cbRobotDeathSound.Items.Add(datafile.baseFile.SoundNames[i]);
            }
            cbRobotWeapon1.Items.Clear();
            cbRobotWeapon2.Items.Clear(); cbRobotWeapon2.Items.Add("None");
            for (int i = 0; i < datafile.GetNumWeapons(); i++)
            {
                cbRobotWeapon1.Items.Add(datafile.GetWeaponName(i));
                cbRobotWeapon2.Items.Add(datafile.GetWeaponName(i));
            }
            cbRobotHitVClip.Items.Clear(); cbRobotHitVClip.Items.Add("None");
            cbRobotDeathVClip.Items.Clear(); cbRobotDeathVClip.Items.Add("None");
            for (int i = 0; i < datafile.baseFile.VClips.Count; i++)
            {
                cbRobotHitVClip.Items.Add(datafile.baseFile.VClipNames[i]);
                cbRobotDeathVClip.Items.Add(datafile.baseFile.VClipNames[i]);
            }
            cbRobotModel.Items.Clear();

            for (int i = 0; i < datafile.GetNumModels(); i++)
                cbRobotModel.Items.Add(datafile.GetModelName(i));
        }

        private void UpdateRobotDropTypes(int dropType, Robot robot)
        {
            cbRobotDropItem.Items.Clear();
            if (dropType != 1)
            {
                for (int i = 0; i < datafile.baseFile.Powerups.Count; i++)
                    cbRobotDropItem.Items.Add(datafile.baseFile.PowerupNames[i]);
                cbRobotDropItem.SelectedIndex = robot.contains_id;
            }
            else
            {
                for (int i = 0; i < datafile.GetNumRobots(); i++)
                    cbRobotDropItem.Items.Add(datafile.GetRobotName(i));
                cbRobotDropItem.SelectedIndex = robot.contains_id;
            }
            //cbRobotDropItem.SelectedIndex = 0;
        }

        public void UpdateRobotPanel(int num)
        {
            Robot robot = datafile.replacedRobots[num];
            cbRobotAttackSound.SelectedIndex = robot.attack_sound;
            cbRobotClawSound.SelectedIndex = robot.claw_sound;
            txtRobotDrag.Text = robot.drag.ToString();
            txtRobotDropProb.Text = robot.contains_prob.ToString();
            txtRobotDrops.Text = robot.contains_count.ToString();
            txtRobotLight.Text = robot.lighting.ToString();
            txtRobotMass.Text = robot.mass.ToString();
            txtRobotScore.Text = robot.score_value.ToString();
            cbRobotHitSound.SelectedIndex = robot.exp1_sound_num;
            cbRobotDeathSound.SelectedIndex = robot.exp2_sound_num;
            cbRobotSeeSound.SelectedIndex = robot.see_sound;
            txtRobotShield.Text = robot.strength.ToString();
            cbRobotTauntSound.SelectedIndex = robot.taunt_sound;
            txtRobotAim.Text = robot.aim.ToString();
            txtRobotBadass.Text = robot.badass.ToString();
            txtRobotDeathBlobs.Text = robot.smart_blobs.ToString();
            txtRobotDeathRolls.Text = robot.death_roll.ToString();
            txtRobotEnergyDrain.Text = robot.energy_drain.ToString();
            txtRobotHitBlobs.Text = robot.energy_blobs.ToString();
            txtRobotGlow.Text = robot.glow.ToString();
            txtRobotPursuit.Text = robot.pursuit.ToString();
            cbRobotDyingSound.SelectedIndex = robot.deathroll_sound;
            txtRobotLightcast.Text = robot.lightcast.ToString();

            cbRobotCompanion.Checked = robot.companion != 0;
            cbRobotClaw.Checked = robot.attack_type != 0;
            cbRobotThief.Checked = robot.thief != 0;
            cbKamikaze.Checked = robot.kamikaze != 0;

            int dropType = robot.contains_type;
            if (dropType == 2)
                dropType = 1;
            else
                dropType = 0;

            UpdateRobotDropTypes(dropType, robot);
            cbRobotDropType.SelectedIndex = dropType;

            cbRobotHitVClip.SelectedIndex = robot.exp1_vclip_num + 1;
            cbRobotDeathVClip.SelectedIndex = robot.exp2_vclip_num + 1;
            cbRobotModel.SelectedIndex = robot.model_num;
            cbRobotWeapon1.SelectedIndex = robot.weapon_type;
            cbRobotWeapon2.SelectedIndex = robot.weapon_type2 + 1;
            if (robot.behavior >= 128)
            {
                cbRobotAI.SelectedIndex = robot.behavior - 128;
            }
            else cbRobotAI.SelectedIndex = 0;

            int bossMode = robot.boss_flag;
            if (bossMode > 20)
                bossMode -= 18;
            cmRobotBoss.SelectedIndex = bossMode;
            cmRobotCloak.SelectedIndex = robot.cloak_type;

            if (robot.behavior != 0)
                cbRobotAI.SelectedIndex = robot.behavior - 0x80;
            else
                cbRobotAI.SelectedIndex = 0;

            nudRobotAI.Value = 0;
            UpdateRobotAI(0);

            tbReplacementElement.Text = robot.replacementID.ToString();

            UpdateRobotAnimation(robot);
        }

        private void UpdateRobotAnimation(Robot robot)
        {
            if (datafile.GetModel(robot.model_num).isAnimated)
            {
                RobotAnimationCheckbox.Checked = true;
                BaseJointSpinner.Value = (Decimal)robot.baseJoint;
            }
            else
                RobotAnimationCheckbox.Checked = false;

            NumJointsTextBox.Text = (Robot.NUM_ANIMATION_STATES * (datafile.GetModel(robot.model_num).n_models - 1)).ToString();
        }

        private void UpdateRobotAI(int num)
        {
            Robot robot = datafile.replacedRobots[(int)nudElementNum.Value];

            txtRobotCircleDist.Text = robot.circle_distance[num].ToString();
            txtRobotEvadeSpeed.Text = robot.evade_speed[num].ToString();
            txtRobotFireDelay.Text = robot.firing_wait[num].ToString();
            txtRobotFireDelay2.Text = robot.firing_wait2[num].ToString();
            txtRobotFOV.Text = ((int)(Math.Round(Math.Acos(robot.field_of_view[num]) * 180 / Math.PI, MidpointRounding.AwayFromZero))).ToString();
            txtRobotMaxSpeed.Text = robot.max_speed[num].ToString();
            txtRobotTurnSpeed.Text = robot.turn_time[num].ToString();
            txtRobotShotCount.Text = robot.rapidfire_count[num].ToString();
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

        private void UpdateModelPanel(int num)
        {
            Polymodel model = datafile.replacedModels[(int)nudElementNum.Value];
            //Polymodel model = datafile.PolygonModels[num];
            txtModelNumModels.Text = model.n_models.ToString();
            txtModelDataSize.Text = model.model_data_size.ToString();
            txtModelRadius.Text = model.rad.ToString();
            txtModelTextureCount.Text = model.n_textures.ToString();
            cbModelLowDetail.SelectedIndex = model.simpler_model;
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
            ModelNumPointers.Text = model.n_textures.ToString();
            ModelBasePointerSpinner.Value = model.first_texture;
            ModelBaseTextureSpinner.Value = model.BaseTexture;

            for (int i = 0; i < numNewTextures; i++)
            {
                ushort index = datafile.GetObjBitmap(i + model.BaseTexture);
                if (datafile.baseFile.piggyFile.images[index].isAnimated)
                {
                    AnimatedWarningLabel.Visible = true;
                }
                else
                    AnimatedWarningLabel.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    datafile.replacedRobots.Add(new Robot());
                    ResetMaxes();
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
            ResetMaxes();
            FillOutCurrentPanel(0, 0);
            isLocked = false;
        }

        private void nudRobotAI_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                UpdateRobotAI((int)nudRobotAI.Value);
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

        //---------------------------------------------------------------------
        // ROBOT UPDATORS
        //---------------------------------------------------------------------

        private void RobotComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
            {
                return;
            }
            ComboBox sendingControl = (ComboBox)sender;
            string tagstr = (string)sendingControl.Tag;
            int tagvalue = Int32.Parse(tagstr);
            int value = sendingControl.SelectedIndex;
            Robot robot = datafile.replacedRobots[ElementNumber];
            robot.UpdateRobot(tagvalue, ref value, (int)nudRobotAI.Value, 0, datafile);
            //[ISB] ugly hack, show new value of joints and animation checkbox
            isLocked = true;
            UpdateRobotAnimation(robot);
            isLocked = false;
        }

        private void RobotProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            TextBox textBox = (TextBox)sender;
            int tagvalue = int.Parse((string)textBox.Tag);
            int value;
            if (int.TryParse(textBox.Text, out value))
            {
                Robot robot = datafile.replacedRobots[ElementNumber];
                bool clamped = robot.UpdateRobot(tagvalue, ref value, (int)nudRobotAI.Value, 0, datafile);
                if (clamped) //parrot back the value if it clamped
                {
                    isLocked = true;
                    textBox.Text = value.ToString();
                    isLocked = false;
                }
            }
        }

        private void RobotPropertyFixed_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
            {
                return;
            }
            TextBox textBox = (TextBox)sender;
            int tagvalue = int.Parse((string)textBox.Tag);

            float fvalue;
            if (float.TryParse(textBox.Text, out fvalue))
            {
                int value = (int)(fvalue * 65536f);
                Robot robot = datafile.replacedRobots[ElementNumber];
                bool clamped = robot.UpdateRobot(tagvalue, ref value, (int)nudRobotAI.Value, 0, datafile);
                if (clamped) //parrot back the value if it clamped
                {
                    isLocked = true;
                    textBox.Text = (value / 65536d).ToString();
                    isLocked = false;
                }
            }
        }

        private void cmRobotCloak_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            Robot robot = datafile.replacedRobots[ElementNumber];
            robot.cloak_type = (sbyte)cmRobotCloak.SelectedIndex;
        }

        private void cmRobotBoss_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            Robot robot = datafile.replacedRobots[ElementNumber];
            int bosstype = cmRobotBoss.SelectedIndex;
            if (bosstype >= 3)
            {
                bosstype += 18;
            }
            robot.boss_flag = (sbyte)bosstype;
        }

        private void cbRobotAI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            Robot robot = datafile.replacedRobots[ElementNumber];
            int bosstype = cbRobotAI.SelectedIndex;
            robot.behavior = (byte)(bosstype + 0x80);
        }

        private void cbRobotDropType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            Robot robot = datafile.replacedRobots[ElementNumber];
            robot.ClearAndUpdateDropReference(null, cbRobotDropType.SelectedIndex == 1 ? 2 : 7);
            UpdateRobotDropTypes(cbRobotDropType.SelectedIndex, robot);
        }

        private void RobotCheckBox_CheckedChange(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            CheckBox input = (CheckBox)sender;
            Robot robot = datafile.replacedRobots[ElementNumber];
            switch (input.Tag)
            {
                case "0":
                    robot.thief = (sbyte)(input.Checked ? 1 : 0);
                    break;
                case "1":
                    robot.kamikaze = (sbyte)(input.Checked ? 1 : 0);
                    break;
                case "2":
                    robot.companion = (sbyte)(input.Checked ? 1 : 0);
                    break;
                case "3":
                    robot.attack_type = (sbyte)(input.Checked ? 1 : 0);
                    break;
            }
        }

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

        private void BaseJointSpinner_ValueChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            Robot robot = datafile.replacedRobots[ElementNumber];
            robot.baseJoint = (int)BaseJointSpinner.Value;
        }

        private void RobotAnimationCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            Polymodel model = datafile.GetModel(datafile.replacedRobots[ElementNumber].model_num);
            model.isAnimated = RobotAnimationCheckbox.Checked;
        }
    }
}
