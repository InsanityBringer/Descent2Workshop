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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PiggyDump
{
    public partial class VHAMEditor : Form
    {
        private OpenTK.GLControl glControl1;
        private static HAMType[] typeTable = { HAMType.TMAPInfo, HAMType.VClip, HAMType.EClip, HAMType.WClip, HAMType.Robot, HAMType.Weapon,
            HAMType.Model, HAMType.Sound, HAMType.Reactor, HAMType.Powerup, HAMType.Ship, HAMType.Gauge, HAMType.Cockpit, HAMType.XLAT };
        public int[] texturelist;
        public VHAMFile datafile;
        public StandardUI host;
        public bool isLocked = false;
        public bool glContextCreated = false;
        private ModelRenderer modelRenderer;
        private bool noPMView = false;


        public VHAMEditor(VHAMFile data, StandardUI host)
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

            if (!noPMView)
                this.tabPage3.Controls.Add(this.glControl1);
            datafile = data;
            this.host = host;
            modelRenderer = new ModelRenderer(datafile.baseFile, host.DefaultPigFile);
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
            switch (tabControl1.SelectedIndex)
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
                FillOutCurrentPanel(tabControl1.SelectedIndex, (int)nudElementNum.Value);
                isLocked = false;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isLocked = true;
            ElementListInit();
            nudElementNum.Value = 0;
            FillOutCurrentPanel(tabControl1.SelectedIndex, 0);
            isLocked = false;
        }

        private void InitWeaponPanel()
        {
            cbWeaponFireSound.Items.Clear(); cbWeaponFireSound.Items.Add("None");
            cbWeaponRobotHitSound.Items.Clear(); cbWeaponRobotHitSound.Items.Add("None");
            cbWeaponWallHitSound.Items.Clear(); cbWeaponWallHitSound.Items.Add("None");
            for (int i = 0; i < datafile.baseFile.Sounds.Count; i++)
            {
                cbWeaponFireSound.Items.Add(datafile.baseFile.SoundNames[i]);
                cbWeaponRobotHitSound.Items.Add(datafile.baseFile.SoundNames[i]);
                cbWeaponWallHitSound.Items.Add(datafile.baseFile.SoundNames[i]);
            }
            cbWeaponMuzzleFlash.Items.Clear(); cbWeaponMuzzleFlash.Items.Add("None");
            cbWeaponWallHit.Items.Clear(); cbWeaponWallHit.Items.Add("None");
            cbWeaponRobotHit.Items.Clear(); cbWeaponRobotHit.Items.Add("None");
            cbWeaponVClip.Items.Clear(); cbWeaponVClip.Items.Add("None");
            for (int i = 0; i < datafile.baseFile.VClips.Count; i++)
            {
                cbWeaponMuzzleFlash.Items.Add(datafile.baseFile.VClipNames[i]);
                cbWeaponWallHit.Items.Add(datafile.baseFile.VClipNames[i]);
                cbWeaponRobotHit.Items.Add(datafile.baseFile.VClipNames[i]);
                cbWeaponVClip.Items.Add(datafile.baseFile.VClipNames[i]);
            }
            cbWeaponChildren.Items.Clear(); cbWeaponChildren.Items.Add("None"); //this will be fun since my own size can change. ugh
            for (int i = 0; i < datafile.NumWeapons; i++)
                cbWeaponChildren.Items.Add(datafile.GetWeaponName(i));
            cbWeaponModel1.Items.Clear(); cbWeaponModel1.Items.Add("None");
            cbWeaponModel2.Items.Clear(); cbWeaponModel2.Items.Add("None");
            for (int i = 0; i < datafile.NumModels; i++)
            {
                cbWeaponModel1.Items.Add(datafile.GetModelName(i));
                cbWeaponModel2.Items.Add(datafile.GetModelName(i));
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
            for (int i = 0; i < datafile.NumWeapons; i++)
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
            for (int i = 0; i < datafile.NumModels; i++)
                cbRobotModel.Items.Add(datafile.GetModelName(i));
        }

        private void InitModelPanel()
        {
            cbModelLowDetail.Items.Clear(); cbModelLowDetail.Items.Add("None");
            cbModelDyingModel.Items.Clear(); cbModelDyingModel.Items.Add("None");
            cbModelDeadModel.Items.Clear(); cbModelDeadModel.Items.Add("None");
            for (int i = 0; i < datafile.NumModels; i++)
            {
                cbModelLowDetail.Items.Add(datafile.GetModelName(i));
                cbModelDyingModel.Items.Add(datafile.GetModelName(i));
                cbModelDeadModel.Items.Add(datafile.GetModelName(i));
            }
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
                for (int i = 0; i < datafile.NumRobots; i++)
                    cbRobotDropItem.Items.Add(datafile.GetRobotName(i));
                cbRobotDropItem.SelectedIndex = robot.contains_id;
            }
            //cbRobotDropItem.SelectedIndex = 0;
        }

        public void UpdateRobotPanel(int num)
        {
            Robot robot = datafile.Robots[num];
            cbRobotAttackSound.SelectedIndex = robot.attack_sound;
            cbRobotClawSound.SelectedIndex = robot.claw_sound;
            txtRobotDrag.Text = ((float)(robot.drag)).ToString();
            txtRobotDropProb.Text = robot.contains_prob.ToString();
            txtRobotDrops.Text = robot.contains_count.ToString();
            txtRobotLight.Text = ((float)(robot.lighting)).ToString();
            txtRobotMass.Text = ((float)(robot.mass)).ToString();
            txtRobotScore.Text = robot.score_value.ToString();
            cbRobotHitSound.SelectedIndex = robot.exp1_sound_num;
            cbRobotDeathSound.SelectedIndex = robot.exp2_sound_num;
            cbRobotSeeSound.SelectedIndex = robot.see_sound;
            txtRobotShield.Text = ((float)(robot.strength)).ToString();
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

            txtElemName.Text = datafile.RobotNames[num];
        }

        private void UpdateRobotAI(int num)
        {
            Robot robot = datafile.Robots[(int)nudElementNum.Value];

            txtRobotCircleDist.Text = ((float)(robot.circle_distance[num])).ToString();
            txtRobotEvadeSpeed.Text = robot.evade_speed[num].ToString();
            txtRobotFireDelay.Text = ((float)(robot.firing_wait[num])).ToString();
            txtRobotFireDelay2.Text = ((float)(robot.firing_wait2[num])).ToString();
            txtRobotFOV.Text = ((int)(Math.Acos(((float)(robot.field_of_view[num]))) * 180 / Math.PI)).ToString();
            txtRobotMaxSpeed.Text = ((float)(robot.max_speed[num])).ToString();
            txtRobotTurnSpeed.Text = ((float)(robot.turn_time[num])).ToString();
            txtRobotShotCount.Text = robot.rapidfire_count[num].ToString();
        }

        private void nudRobotAI_ValueChanged_1(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                UpdateRobotAI((int)nudRobotAI.Value);
                isLocked = false;
            }
        }

        public void UpdateWeaponPanel(int num)
        {
            Weapon weapon = datafile.Weapons[num];

            int rendernum = weapon.render_type;

            if (rendernum == 255)
            {
                rendernum = 4;
            }

            cbWeaponRenderMode.SelectedIndex = rendernum;

            txtWeaponABSize.Text = weapon.afterburner_size.ToString();
            txtWeaponAmmoUse.Text = weapon.ammo_usage.ToString();
            txtWeaponBlindSize.Text = weapon.flash.ToString();
            cbWeaponBounce.SelectedIndex = weapon.bounce;
            txtWeaponCockpitImage.Text = weapon.picture.ToString();
            txtWeaponCockpitImageh.Text = weapon.hires_picture.ToString();
            txtWeaponDrag.Text = ((float)(weapon.drag)).ToString();
            txtWeaponEnergyUsage.Text = ((float)(weapon.energy_usage)).ToString();
            txtWeaponExplosionSize.Text = ((float)(weapon.damage_radius)).ToString();
            txtWeaponFireWait.Text = ((float)(weapon.fire_wait)).ToString();
            txtWeaponFlashSize.Text = ((float)(weapon.flash_size)).ToString();
            txtWeaponImpactSize.Text = ((float)(weapon.impact_size)).ToString();
            txtWeaponLifetime.Text = ((float)(weapon.lifetime)).ToString();
            txtWeaponLight.Text = ((float)(weapon.light)).ToString();
            txtWeaponMass.Text = ((float)(weapon.mass)).ToString();
            txtWeaponMPScale.Text = ((float)(weapon.multi_damage_scale)).ToString();
            txtWeaponPolyLWRatio.Text = ((float)(weapon.po_len_to_width_ratio)).ToString();
            txtWeaponProjectileCount.Text = weapon.fire_count.ToString();
            txtWeaponProjectileSize.Text = ((float)(weapon.blob_size)).ToString();
            txtWeaponSpeedvar.Text = weapon.speedvar.ToString();
            txtWeaponStaticSprite.Text = weapon.bitmap.ToString();
            txtWeaponThrust.Text = ((float)(weapon.thrust)).ToString();

            cbWeaponDestroyable.Checked = weapon.destroyable != 0;
            cbWeaponHoming.Checked = weapon.homing_flag != 0;
            cbWeaponIsMatter.Checked = weapon.matter != 0;
            cbWeaponPlacable.Checked = (weapon.flags & 1) != 0;
            cbWeaponRipper.Checked = weapon.persistent != 0;

            cbWeaponChildren.SelectedIndex = weapon.children + 1;
            cbWeaponFireSound.SelectedIndex = weapon.flash_sound + 1;
            cbWeaponRobotHitSound.SelectedIndex = weapon.robot_hit_sound + 1;
            cbWeaponWallHitSound.SelectedIndex = weapon.wall_hit_sound + 1;
            cbWeaponModel1.SelectedIndex = weapon.model_num + 1;
            cbWeaponModel2.SelectedIndex = weapon.model_num_inner + 1;
            cbWeaponWallHit.SelectedIndex = weapon.wall_hit_vclip + 1;
            cbWeaponRobotHit.SelectedIndex = weapon.robot_hit_vclip + 1;
            cbWeaponMuzzleFlash.SelectedIndex = weapon.flash_vclip + 1;
            cbWeaponVClip.SelectedIndex = weapon.weapon_vclip + 1;

            nudWeaponStr.Value = 0;
            UpdateWeaponPower(0);
            txtElemName.Text = datafile.WeaponNames[num];

            UpdateWeaponGraphicControls();
        }

        private void UpdateWeaponPower(int num)
        {
            Weapon weapon = datafile.Weapons[(int)nudElementNum.Value];

            txtWeaponStr.Text = ((float)(weapon.strength[num])).ToString();
            txtWeaponSpeed.Text = ((float)(weapon.speed[num])).ToString();
        }

        private void UpdateWeaponGraphicControls()
        {
            cbWeaponModel1.Visible = cbWeaponModel2.Visible = cbWeaponVClip.Visible = txtWeaponStaticSprite.Visible = false;
            lbSprite.Visible = lbModelNum.Visible = lbModelNumInner.Visible = btnRemapWeaponSprite.Visible = false;
            if (cbWeaponRenderMode.SelectedIndex == 0 || cbWeaponRenderMode.SelectedIndex == 1 || cbWeaponRenderMode.SelectedIndex == 4)
            {
                lbSprite.Visible = txtWeaponStaticSprite.Visible = btnRemapWeaponSprite.Visible = true;
            }
            else if (cbWeaponRenderMode.SelectedIndex == 3)
            {
                lbSprite.Visible = cbWeaponVClip.Visible = true;
            }
            else if (cbWeaponRenderMode.SelectedIndex == 2)
            {
                lbModelNum.Visible = lbModelNumInner.Visible = cbWeaponModel1.Visible = cbWeaponModel2.Visible = true;
            }
        }

        private void nudWeaponStr_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                UpdateWeaponPower((int)nudWeaponStr.Value);
                isLocked = false;
            }
        }

        private void UpdateModelPanel(int num)
        {
            Polymodel model = datafile.Models[num];
            txtModelNumModels.Text = model.n_models.ToString();
            txtModelDataSize.Text = model.model_data_size.ToString();
            txtModelRadius.Text = ((float)(model.rad)).ToString();
            txtModelTextureCount.Text = model.n_textures.ToString();
            cbModelLowDetail.SelectedIndex = model.simpler_model + 1;
            cbModelDyingModel.SelectedIndex = model.DyingModelnum + 1;
            cbModelDeadModel.SelectedIndex = model.DeadModelnum + 1;

            txtModelMinX.Text = ((float)(model.mins.x)).ToString();
            txtModelMinY.Text = ((float)(model.mins.y)).ToString();
            txtModelMinZ.Text = ((float)(model.mins.z)).ToString();
            txtModelMaxX.Text = ((float)(model.maxs.x)).ToString();
            txtModelMaxY.Text = ((float)(model.maxs.y)).ToString();
            txtModelMaxZ.Text = ((float)(model.maxs.z)).ToString();

            txtElemName.Text = datafile.ModelNames[num];
            if (!noPMView)
            {
                Polymodel mainmodel = datafile.Models[(int)nudElementNum.Value];
                modelRenderer.SetModel(mainmodel);
                glControl1.Invalidate();
            }
        }

        private void RobotComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbRobotAI_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RobotProperty_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbRobotDropType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RobotPropertyFixed_TextChanged(object sender, EventArgs e)
        {

        }

        private void RobotCheckBox_CheckedChange(object sender, EventArgs e)
        {

        }

        private void nudRobotAI_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cmRobotBoss_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmRobotCloak_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnImportModel_Click(object sender, EventArgs e)
        {

        }

        private void ModelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

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

            modelRenderer.Angle = (trackBar3.Value - 8) * -22.5d;
            modelRenderer.Pitch = (trackBar1.Value - 8) * -22.5d;
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
