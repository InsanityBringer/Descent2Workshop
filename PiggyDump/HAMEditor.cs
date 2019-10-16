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
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using LibDescent.Data;
using LibDescent.Edit;

namespace Descent2Workshop
{
    public partial class HAMEditor : Form
    {
        private static HAMType[] typeTable = { HAMType.TMAPInfo, HAMType.VClip, HAMType.EClip, HAMType.WClip, HAMType.Robot, HAMType.Weapon,
            HAMType.Model, HAMType.Sound, HAMType.Reactor, HAMType.Powerup, HAMType.Ship, HAMType.Gauge, HAMType.Cockpit, HAMType.XLAT };
        public int[] texturelist;
        public HAMFile datafile;
        public StandardUI host;
        public bool isLocked = false;
        public bool glContextCreated = false;
        private ModelRenderer modelRenderer;
        private bool noPMView = false;

        private int ElementNumber { get { return (int)nudElementNum.Value; } }
        private int PageNumber { get { return tabControl1.SelectedIndex; } }
        
        public HAMEditor(HAMFile data, StandardUI host)
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
                this.tabPage7.Controls.Add(this.glControl1);
            datafile = data;
            this.host = host;
            modelRenderer = new ModelRenderer(datafile, host.DefaultPigFile);
            this.Text = string.Format("{0} - HAM Editor", datafile.Filename);
        }

        private void HAMEditor2_Load(object sender, EventArgs e)
        {
            isLocked = true;
            ElementListInit();
            FillOutCurrentPanel(0, 0);
            isLocked = false;
        }


        //---------------------------------------------------------------------
        // UI MANAGEMENT AND PANEL MANAGEMENT
        //---------------------------------------------------------------------

        private void SetElementControl(bool status, bool listable)
        {
            btnInsertElem.Enabled = btnDeleteElem.Enabled = status;
            btnList.Enabled = listable;
        }

        private void ElementListInit()
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    nudElementNum.Maximum = datafile.TMapInfo.Count - 1;
                    InitTexturePanel();
                    break;
                case 1:
                    nudElementNum.Maximum = datafile.VClips.Count - 1;
                    InitVClipPanel();
                    break;
                case 2:
                    nudElementNum.Maximum = datafile.EClips.Count - 1;
                    InitEClipPanel();
                    break;
                case 3:
                    nudElementNum.Maximum = datafile.WClips.Count - 1;
                    InitWallPanel();
                    break;
                case 4:
                    nudElementNum.Maximum = datafile.Robots.Count - 1;
                    InitRobotPanel();
                    break;
                case 5:
                    nudElementNum.Maximum = datafile.Weapons.Count - 1;
                    InitWeaponPanel();
                    break;
                case 6:
                    nudElementNum.Maximum = datafile.PolygonModels.Count - 1;
                    InitModelPanel();
                    break;
                case 7:
                    nudElementNum.Maximum = datafile.Sounds.Count - 1;
                    InitSoundPanel();
                    break;
                case 8:
                    nudElementNum.Maximum = datafile.Reactors.Count - 1;
                    InitReactorPanel();
                    break;
                case 9:
                    nudElementNum.Maximum = datafile.Powerups.Count - 1;
                    InitPowerupPanel();
                    break;
                case 10:
                    nudElementNum.Maximum = 0;
                    SetElementControl(false, false);
                    break;
                case 11:
                    nudElementNum.Maximum = datafile.Gauges.Count - 1;
                    SetElementControl(false, false);
                    break;
                case 12:
                    nudElementNum.Maximum = datafile.Cockpits.Count - 1;
                    SetElementControl(true, false);
                    break;
                case 13:
                    nudElementNum.Maximum = 2619;
                    SetElementControl(false, false);
                    break;
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

        private void nudElementNum_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                FillOutCurrentPanel(tabControl1.SelectedIndex, (int)nudElementNum.Value);
                isLocked = false;
            }
        }

        private void FillOutCurrentPanel(int id, int val)
        {
            switch (id)
            {
                case 0:
                    UpdateTexturePanel(val);
                    break;
                case 1:
                    UpdateVClipPanel(val);
                    break;
                case 2:
                    UpdateEClipPanel(val);
                    break;
                case 3:
                    UpdateWClipPanel(val);
                    break;
                case 4:
                    UpdateRobotPanel(val);
                    break;
                case 5:
                    UpdateWeaponPanel(val);
                    break;
                case 6:
                    UpdateModelPanel(val);
                    break;
                case 7:
                    UpdateSoundPanel(val);
                    break;
                case 8:
                    Reactor reactor = datafile.Reactors[val];
                    cbReactorModel.SelectedIndex = reactor.model_id;
                    break;
                case 9:
                    UpdatePowerupPanel(val);
                    break;
                case 10:
                    UpdateShipPanel();
                    break;
                case 11:
                    UpdateGaguePanel(val);
                    break;
                case 12:
                    UpdateCockpitPanel(val);
                    break;
                case 13:
                    UpdateXLATPanel(val);
                    break;
            }
        }

        private void InsertElem_Click(object sender, EventArgs e)
        {
            HAMType type = typeTable[tabControl1.SelectedIndex];
            int newID = datafile.AddElement(type);
            if (newID != -1)
            {
                //Update the maximum of the numeric up/down control and ensure that any comboboxes that need to be regenerated for the current element are
                ElementListInit();
                nudElementNum.Value = newID;
            }
        }

        private void DeleteElem_Click(object sender, EventArgs e)
        {
            HAMType type = typeTable[tabControl1.SelectedIndex];
            int returnv = datafile.DeleteElement(type, ElementNumber);
            if (returnv >= 0)
            {
                //Update the maximum of the numeric up/down control and ensure that any comboboxes that need to be regenerated for the current element are
                ElementListInit();
                isLocked = true;
                if (nudElementNum.Value >= returnv)
                    nudElementNum.Value = returnv - 1;
                FillOutCurrentPanel(tabControl1.SelectedIndex, ElementNumber);
                isLocked = false;
            }
            else
            {
                statusBar1.Text = "Can't delete last element: It is being referenced by other elements";
            }
        }

        private void ListElem_Click(object sender, EventArgs e)
        {
            HAMType type = typeTable[PageNumber];
            ElementList elementList = new ElementList(datafile, type);
            if (elementList.ShowDialog() == DialogResult.OK)
            {
                if (elementList.ElementNumber != -1)
                {
                    nudElementNum.Value = elementList.ElementNumber;
                }
            }
        }

        private void ElemName_TextChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            datafile.UpdateName(typeTable[tabControl1.SelectedIndex], ElementNumber, txtElemName.Text);
        }

        //---------------------------------------------------------------------
        // PANEL INITALIZATION
        // The contents of these boxes can change if the amount of elements is changed,
        // so they must be updated each time before display. 
        //---------------------------------------------------------------------

        private void InitTexturePanel()
        {
            SetElementControl(true, false);
            cbTexEClip.Items.Clear(); cbTexEClip.Items.Add("None");
            for (int i = 0; i < datafile.EClips.Count; i++)
                cbTexEClip.Items.Add(datafile.EClipNames[i]);
        }

        private void InitVClipPanel()
        {
            SetElementControl(true, true);
            cbVClipSound.Items.Clear(); cbVClipSound.Items.Add("None");
            for (int i = 0; i < datafile.Sounds.Count; i++)
                cbVClipSound.Items.Add(datafile.SoundNames[i]);
        }

        private void InitEClipPanel()
        {
            SetElementControl(true, true);
            cbEClipBreakEClip.Items.Clear(); cbEClipBreakEClip.Items.Add("None");
            cbEClipMineCritical.Items.Clear(); cbEClipMineCritical.Items.Add("None");
            cbEClipBreakVClip.Items.Clear(); cbEClipBreakVClip.Items.Add("None");
            cbEClipBreakSound.Items.Clear(); cbEClipBreakSound.Items.Add("None");
            for (int i = 0; i < datafile.EClips.Count; i++)
            {
                cbEClipBreakEClip.Items.Add(datafile.EClipNames[i]);
                cbEClipMineCritical.Items.Add(datafile.EClipNames[i]);
            }
            for (int i = 0; i < datafile.VClips.Count; i++)
                cbEClipBreakVClip.Items.Add(datafile.VClipNames[i]);
            for (int i = 0; i < datafile.Sounds.Count; i++)
                cbEClipBreakSound.Items.Add(datafile.SoundNames[i]);
        }

        private void InitWallPanel()
        {
            SetElementControl(true, false);
            cbWallCloseSound.Items.Clear(); cbWallCloseSound.Items.Add("None");
            cbWallOpenSound.Items.Clear(); cbWallOpenSound.Items.Add("None");
            for (int i = 0; i < datafile.Sounds.Count; i++)
            {
                cbWallOpenSound.Items.Add(datafile.SoundNames[i]);
                cbWallCloseSound.Items.Add(datafile.SoundNames[i]);
            }
        }

        private void InitWeaponPanel()
        {
            SetElementControl(true, true);
            cbWeaponFireSound.Items.Clear(); cbWeaponFireSound.Items.Add("None");
            cbWeaponRobotHitSound.Items.Clear(); cbWeaponRobotHitSound.Items.Add("None");
            cbWeaponWallHitSound.Items.Clear(); cbWeaponWallHitSound.Items.Add("None");
            for (int i = 0; i < datafile.Sounds.Count; i++)
            {
                cbWeaponFireSound.Items.Add(datafile.SoundNames[i]);
                cbWeaponRobotHitSound.Items.Add(datafile.SoundNames[i]);
                cbWeaponWallHitSound.Items.Add(datafile.SoundNames[i]);
            }
            cbWeaponMuzzleFlash.Items.Clear(); cbWeaponMuzzleFlash.Items.Add("None");
            cbWeaponWallHit.Items.Clear(); cbWeaponWallHit.Items.Add("None");
            cbWeaponRobotHit.Items.Clear(); cbWeaponRobotHit.Items.Add("None");
            cbWeaponVClip.Items.Clear(); cbWeaponVClip.Items.Add("None");
            for (int i = 0; i < datafile.VClips.Count; i++)
            {
                cbWeaponMuzzleFlash.Items.Add(datafile.VClipNames[i]);
                cbWeaponWallHit.Items.Add(datafile.VClipNames[i]);
                cbWeaponRobotHit.Items.Add(datafile.VClipNames[i]);
                cbWeaponVClip.Items.Add(datafile.VClipNames[i]);
            }
            cbWeaponChildren.Items.Clear(); cbWeaponChildren.Items.Add("None"); //this will be fun since my own size can change. ugh
            for (int i = 0; i < datafile.Weapons.Count; i++)
                cbWeaponChildren.Items.Add(datafile.WeaponNames[i]);
            cbWeaponModel1.Items.Clear(); cbWeaponModel1.Items.Add("None");
            cbWeaponModel2.Items.Clear(); cbWeaponModel2.Items.Add("None");
            for (int i = 0; i < datafile.PolygonModels.Count; i++)
            {
                cbWeaponModel1.Items.Add(datafile.ModelNames[i]);
                cbWeaponModel2.Items.Add(datafile.ModelNames[i]);
            }
        }

        private void InitRobotPanel()
        {
            SetElementControl(true, true);
            cbRobotAttackSound.Items.Clear();
            cbRobotClawSound.Items.Clear();
            cbRobotDyingSound.Items.Clear();
            cbRobotSeeSound.Items.Clear();
            cbRobotTauntSound.Items.Clear();
            cbRobotHitSound.Items.Clear();
            cbRobotDeathSound.Items.Clear();
            for (int i = 0; i < datafile.Sounds.Count; i++)
            {
                cbRobotAttackSound.Items.Add(datafile.SoundNames[i]);
                cbRobotClawSound.Items.Add(datafile.SoundNames[i]);
                cbRobotDyingSound.Items.Add(datafile.SoundNames[i]);
                cbRobotSeeSound.Items.Add(datafile.SoundNames[i]);
                cbRobotTauntSound.Items.Add(datafile.SoundNames[i]);
                cbRobotHitSound.Items.Add(datafile.SoundNames[i]);
                cbRobotDeathSound.Items.Add(datafile.SoundNames[i]);
            }
            cbRobotWeapon1.Items.Clear();
            cbRobotWeapon2.Items.Clear(); cbRobotWeapon2.Items.Add("None");
            for (int i = 0; i < datafile.Weapons.Count; i++)
            {
                cbRobotWeapon1.Items.Add(datafile.WeaponNames[i]);
                cbRobotWeapon2.Items.Add(datafile.WeaponNames[i]);
            }
            cbRobotHitVClip.Items.Clear(); cbRobotHitVClip.Items.Add("None");
            cbRobotDeathVClip.Items.Clear(); cbRobotDeathVClip.Items.Add("None");
            for (int i = 0; i < datafile.VClips.Count; i++)
            {
                cbRobotHitVClip.Items.Add(datafile.VClipNames[i]);
                cbRobotDeathVClip.Items.Add(datafile.VClipNames[i]);
            }
            cbRobotModel.Items.Clear();
            for (int i = 0; i < datafile.PolygonModels.Count; i++)
                cbRobotModel.Items.Add(datafile.ModelNames[i]);
        }

        private void InitSoundPanel()
        {
            SetElementControl(false, true);
            cbSoundSNDid.Items.Clear();
            cbLowMemSound.Items.Clear();
            cbSoundSNDid.Items.Add("None");
            cbLowMemSound.Items.Add("None");
            for (int i = 0; i < datafile.Sounds.Count; i++)
                cbLowMemSound.Items.Add(datafile.SoundNames[i]);
            foreach (SoundData sound in host.DefaultSoundFile.sounds)
                cbSoundSNDid.Items.Add(sound.name);
        }

        private void InitPowerupPanel()
        {
            SetElementControl(true, true);
            cbPowerupPickupSound.Items.Clear();
            cbPowerupSprite.Items.Clear();
            for (int i = 0; i < datafile.Sounds.Count; i++)
                cbPowerupPickupSound.Items.Add(datafile.SoundNames[i]);
            for (int i = 0; i < datafile.VClips.Count; i++)
                cbPowerupSprite.Items.Add(datafile.VClipNames[i]);
        }

        private void InitModelPanel()
        {
            SetElementControl(true, true);
            cbModelLowDetail.Items.Clear(); cbModelLowDetail.Items.Add("None");
            cbModelDyingModel.Items.Clear(); cbModelDyingModel.Items.Add("None");
            cbModelDeadModel.Items.Clear(); cbModelDeadModel.Items.Add("None");
            for (int i = 0; i < datafile.PolygonModels.Count; i++)
            {
                cbModelLowDetail.Items.Add(datafile.ModelNames[i]);
                cbModelDyingModel.Items.Add(datafile.ModelNames[i]);
                cbModelDeadModel.Items.Add(datafile.ModelNames[i]);
            }
        }

        private void InitReactorPanel()
        {
            SetElementControl(true, true);
            cbReactorModel.Items.Clear();
            for (int i = 0; i < datafile.PolygonModels.Count; i++)
                cbReactorModel.Items.Add(datafile.ModelNames[i]);
        }

        //---------------------------------------------------------------------
        // PANEL CREATORS
        //---------------------------------------------------------------------

        private void UpdatePictureBox(Image img, PictureBox pictureBox)
        {
            if (pictureBox.Image != null)
            {
                Image oldImg = pictureBox.Image;
                pictureBox.Image = null;
                oldImg.Dispose();
            }
            pictureBox.Image = img;
        }

        public void UpdateTexturePanel(int num)
        {
            TMAPInfo texture = datafile.TMapInfo[num];
            txtTexID.Text = datafile.Textures[num].ToString();
            txtTexLight.Text = texture.lighting.ToString();
            txtTexDamage.Text = texture.damage.ToString();
            cbTexEClip.SelectedIndex = texture.eclip_num + 1;
            txtTexSlideU.Text = GetFloatFromFixed88(texture.slide_u).ToString();
            txtTexSlideV.Text = GetFloatFromFixed88(texture.slide_v).ToString();
            txtTexDestroyed.Text = texture.destroyed.ToString();
            cbTexLava.Checked = ((texture.flags & TMAPInfo.TMI_VOLATILE) != 0);
            cbTexWater.Checked = ((texture.flags & TMAPInfo.TMI_WATER) != 0);
            cbTexForcefield.Checked = ((texture.flags & TMAPInfo.TMI_FORCE_FIELD) != 0);
            cbTexRedGoal.Checked = ((texture.flags & TMAPInfo.TMI_GOAL_RED) != 0);
            cbTexBlueGoal.Checked = ((texture.flags & TMAPInfo.TMI_GOAL_BLUE) != 0);
            cbTexHoardGoal.Checked = ((texture.flags & TMAPInfo.TMI_GOAL_HOARD) != 0);

            UpdatePictureBox(datafile.piggyFile.GetBitmap(datafile.Textures[num]), pbTexPrev);
        }

        public void UpdateGaguePanel(int num)
        {
            ushort gague = datafile.Gauges[num];
            ushort hiresgague = datafile.GaugesHires[num];

            txtGagueLores.Text = gague.ToString();
            txtGagueHires.Text = hiresgague.ToString();

            if (pbGagueLores.Image != null)
            {
                Bitmap temp = (Bitmap)pbGagueLores.Image;
                pbGagueLores.Image = null;
                temp.Dispose();
            }
            pbGagueLores.Image = datafile.piggyFile.GetBitmap(gague);

            if (pbGagueHires.Image != null)
            {
                Bitmap temp = (Bitmap)pbGagueHires.Image;
                pbGagueHires.Image = null;
                temp.Dispose();
            }
            pbGagueHires.Image = datafile.piggyFile.GetBitmap(hiresgague);
        }

        public void UpdateVClipPanel(int num)
        {
            VClip animation = datafile.VClips[num];
            txtAnimFrameSpeed.Text = animation.frame_time.ToString();
            txtAnimTotalTime.Text = animation.play_time.ToString();
            txtAnimLight.Text = animation.light_value.ToString();
            cbVClipSound.SelectedIndex = animation.sound_num + 1;
            txtAnimFrameCount.Text = animation.num_frames.ToString();
            cbAnimRod.Checked = ((animation.flags & 1) == 1);

            nudAnimFrame.Value = 0;
            UpdateAnimationFrame(0);

            txtElemName.Text = datafile.VClipNames[num];
        }

        private void nudAnimFrame_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                UpdateAnimationFrame((int)nudAnimFrame.Value);
                isLocked = false;
            }
        }

        public void UpdateEClipPanel(int num)
        {
            EClip animation = datafile.EClips[num];

            //vclip specific data
            txtEffectFrameSpeed.Text = animation.vc.frame_time.ToString();
            txtEffectTotalTime.Text = animation.vc.play_time.ToString();
            txtEffectLight.Text = animation.vc.light_value.ToString();
            txtEffectFrameCount.Text = animation.vc.num_frames.ToString();

            //eclip stuff
            cbEClipBreakEClip.SelectedIndex = animation.dest_eclip + 1;
            cbEClipBreakVClip.SelectedIndex = animation.dest_vclip + 1;
            txtEffectExplodeSize.Text = animation.dest_size.ToString();
            txtEffectBrokenID.Text = animation.dest_bm_num.ToString();
            cbEClipBreakSound.SelectedIndex = animation.sound_num + 1;
            cbEClipMineCritical.SelectedIndex = animation.crit_clip + 1;
            cbEffectCritical.Checked = (animation.flags & 1) != 0;
            cbEffectOneShot.Checked = (animation.flags & 2) != 0;

            nudEffectFrame.Value = 0;
            UpdateEffectFrame(0);

            txtElemName.Text = datafile.EClipNames[num];
        }

        private void nudEffectFrame_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                UpdateEffectFrame((int)nudEffectFrame.Value);
                isLocked = false;
            }
        }

        public void UpdateWClipPanel(int num)
        {
            WClip animation = datafile.WClips[num];
            txtWallTotalTime.Text = animation.play_time.ToString();
            cbWallOpenSound.SelectedIndex = animation.open_sound + 1;
            cbWallCloseSound.SelectedIndex = animation.close_sound + 1;
            txtWallFilename.Text = new string(animation.filename);
            txtWallFrames.Text = animation.num_frames.ToString();

            cbWallExplodeOpen.Checked = (animation.flags & 1) != 0;
            cbWallShootable.Checked = (animation.flags & 2) != 0;
            cbWallOnPrimaryTMAP.Checked = (animation.flags & 4) != 0;
            cbWallHidden.Checked = (animation.flags & 8) != 0;

            nudWFrame.Value = 0;
            UpdateWallFrame(0);

            txtElemName.Text = "<unnamed>";
        }

        private void nudWFrame_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                UpdateWallFrame((int)nudWFrame.Value);
                isLocked = false;
            }
        }

        private void UpdateRobotDropTypes(int dropType, Robot robot)
        {
            cbRobotDropItem.Items.Clear();
            if (dropType != 1)
            {
                for (int i = 0; i < datafile.Powerups.Count; i++)
                    cbRobotDropItem.Items.Add(datafile.PowerupNames[i]);
                cbRobotDropItem.SelectedIndex = robot.contains_id;
            }
            else
            {
                for (int i = 0; i < datafile.Robots.Count; i++)
                    cbRobotDropItem.Items.Add(datafile.RobotNames[i]);
                cbRobotDropItem.SelectedIndex = robot.contains_id;
            }
            //cbRobotDropItem.SelectedIndex = 0;
        }

        public void UpdateRobotPanel(int num)
        {
            Robot robot = datafile.Robots[num];
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

            txtElemName.Text = datafile.RobotNames[num];
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

        private void UpdateRobotAI(int num)
        {
            Robot robot = datafile.Robots[(int)nudElementNum.Value];

            txtRobotCircleDist.Text = robot.circle_distance[num].ToString();
            txtRobotEvadeSpeed.Text = robot.evade_speed[num].ToString();
            txtRobotFireDelay.Text = robot.firing_wait[num].ToString();
            txtRobotFireDelay2.Text = robot.firing_wait2[num].ToString();
            txtRobotFOV.Text = ((int)(Math.Round(Math.Acos(robot.field_of_view[num]) * 180 / Math.PI, MidpointRounding.AwayFromZero))).ToString();
            txtRobotMaxSpeed.Text = robot.max_speed[num].ToString();
            txtRobotTurnSpeed.Text = robot.turn_time[num].ToString();
            txtRobotShotCount.Text = robot.rapidfire_count[num].ToString();
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
            txtWeaponDrag.Text = weapon.drag.ToString();
            txtWeaponEnergyUsage.Text = weapon.energy_usage.ToString();
            txtWeaponExplosionSize.Text = weapon.damage_radius.ToString();
            txtWeaponFireWait.Text = weapon.fire_wait.ToString();
            txtWeaponFlashSize.Text = weapon.flash_size.ToString();
            txtWeaponImpactSize.Text = weapon.impact_size.ToString();
            txtWeaponLifetime.Text = weapon.lifetime.ToString();
            txtWeaponLight.Text = weapon.light.ToString();
            txtWeaponMass.Text = weapon.mass.ToString();
            txtWeaponMPScale.Text = weapon.multi_damage_scale.ToString();
            txtWeaponPolyLWRatio.Text = weapon.po_len_to_width_ratio.ToString();
            txtWeaponProjectileCount.Text = weapon.fire_count.ToString();
            txtWeaponProjectileSize.Text = weapon.blob_size.ToString();
            txtWeaponSpeedvar.Text = weapon.speedvar.ToString();
            txtWeaponStaticSprite.Text = weapon.bitmap.ToString();
            txtWeaponThrust.Text = weapon.thrust.ToString();

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

        private void nudWeaponStr_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                UpdateWeaponPower((int)nudWeaponStr.Value);
                isLocked = false;
            }
        }

        private void UpdateWeaponPower(int num)
        {
            Weapon weapon = datafile.Weapons[(int)nudElementNum.Value];

            txtWeaponStr.Text = weapon.strength[num].ToString();
            txtWeaponSpeed.Text = weapon.speed[num].ToString();
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

        private void UpdateModelPanel(int num)
        {
            Polymodel model = datafile.PolygonModels[num];
            txtModelNumModels.Text = model.n_models.ToString();
            txtModelDataSize.Text = model.model_data_size.ToString();
            txtModelRadius.Text = model.rad.ToString();
            txtModelTextureCount.Text = model.n_textures.ToString();
            cbModelLowDetail.SelectedIndex = model.simpler_model;
            cbModelDyingModel.SelectedIndex = model.DyingModelnum + 1;
            cbModelDeadModel.SelectedIndex = model.DeadModelnum + 1;

            txtModelMinX.Text = model.mins.x.ToString();
            txtModelMinY.Text = model.mins.y.ToString();
            txtModelMinZ.Text = model.mins.z.ToString();
            txtModelMaxX.Text = model.maxs.x.ToString();
            txtModelMaxY.Text = model.maxs.y.ToString();
            txtModelMaxZ.Text = model.maxs.z.ToString();

            txtElemName.Text = datafile.ModelNames[num];
            if (!noPMView)
            {
                Polymodel mainmodel = datafile.PolygonModels[(int)nudElementNum.Value];
                modelRenderer.SetModel(mainmodel);
                glControl1.Invalidate();
            }
        }

        private void UpdatePowerupPanel(int num)
        {
            Powerup powerup = datafile.Powerups[num];
            cbPowerupPickupSound.SelectedIndex = powerup.hit_sound;
            cbPowerupSprite.SelectedIndex = powerup.vclip_num;
            txtPowerupSize.Text = powerup.size.ToString();
            txtPowerupLight.Text = powerup.light.ToString();
            txtElemName.Text = datafile.PowerupNames[num];
        }

        private void UpdateShipPanel()
        {
            Ship ship = datafile.PlayerShip;
            cbPlayerExplosion.Items.Clear();
            for (int i = 0; i < datafile.VClips.Count; i++)
                cbPlayerExplosion.Items.Add(datafile.VClipNames[i]);
            cbPlayerModel.Items.Clear(); cbMarkerModel.Items.Clear();
            for (int i = 0; i < datafile.PolygonModels.Count; i++)
            {
                cbPlayerModel.Items.Add(datafile.ModelNames[i]);
                cbMarkerModel.Items.Add(datafile.ModelNames[i]);
            }

            txtShipBrakes.Text = ship.brakes.ToString();
            txtShipDrag.Text = ship.drag.ToString();
            txtShipMass.Text = ship.mass.ToString();
            txtShipMaxRotThrust.Text = ship.max_rotthrust.ToString();
            txtShipRevThrust.Text = ship.reverse_thrust.ToString();
            txtShipThrust.Text = ship.max_thrust.ToString();
            txtShipWiggle.Text = ship.wiggle.ToString();
            nudShipTextures.Value = nudShipTextures.Minimum;
            UpdateShipTextures(0);
            //This can thereoetically null, but it never will except on deformed data that descent itself probably wouldn't like
            cbPlayerExplosion.SelectedIndex = ship.expl_vclip_num;
            cbMarkerModel.SelectedIndex = ship.markerModel;
            cbPlayerModel.SelectedIndex = ship.model_num;

            txtElemName.Text = "Ship";
        }

        private void nudShipTextures_ValueChanged(object sender, EventArgs e)
        {
            UpdateShipTextures((int)nudShipTextures.Value - 2);
        }

        private void UpdateShipTextures(int id)
        {
            UpdatePictureBox(datafile.piggyFile.GetBitmap(datafile.multiplayerBitmaps[id * 2]), pbWeaponTexture);
            UpdatePictureBox(datafile.piggyFile.GetBitmap(datafile.multiplayerBitmaps[id * 2 + 1]), pbWingTex);
        }

        private void UpdateAnimationFrame(int frame)
        {
            VClip animation = datafile.VClips[(int)nudElementNum.Value];
            txtAnimFrameNum.Text = animation.frames[frame].ToString();

            if (pbAnimFramePreview.Image != null)
            {
                Bitmap temp = (Bitmap)pbAnimFramePreview.Image;
                pbAnimFramePreview.Image = null;
                temp.Dispose();
            }
            pbAnimFramePreview.Image = datafile.piggyFile.GetBitmap(animation.frames[frame]);
        }

        private void UpdateEffectFrame(int frame)
        {
            EClip animation = datafile.EClips[(int)nudElementNum.Value];
            txtEffectFrameNum.Text = animation.vc.frames[frame].ToString();

            if (pbEffectFramePreview.Image != null)
            {
                Bitmap temp = (Bitmap)pbEffectFramePreview.Image;
                pbEffectFramePreview.Image = null;
                temp.Dispose();
            }
            pbEffectFramePreview.Image = datafile.piggyFile.GetBitmap(animation.vc.frames[frame]);
        }

        private void UpdateWallFrame(int frame)
        {
            WClip animation = datafile.WClips[(int)nudElementNum.Value];
            txtWallCurrentFrame.Text = animation.frames[frame].ToString();

            if (pbWallAnimPreview.Image != null)
            {
                Bitmap temp = (Bitmap)pbWallAnimPreview.Image;
                pbWallAnimPreview.Image = null;
                temp.Dispose();
            }
            pbWallAnimPreview.Image = datafile.piggyFile.GetBitmap((int)((ushort)datafile.Textures[animation.frames[frame]]));
        }

        private void UpdateXLATPanel(int num)
        {
            ushort dst = datafile.BitmapXLATData[num];
            txtXLATDest.Text = dst.ToString();

            if (pbBitmapSrc.Image != null)
            {
                Bitmap temp = (Bitmap)pbBitmapSrc.Image;
                pbBitmapSrc.Image = null;
                temp.Dispose();
            }
            pbBitmapSrc.Image = datafile.piggyFile.GetBitmap(num);

            if (pbBitmapDest.Image != null)
            {
                Bitmap temp = (Bitmap)pbBitmapDest.Image;
                pbBitmapDest.Image = null;
                temp.Dispose();
            }
            pbBitmapDest.Image = datafile.piggyFile.GetBitmap(dst);
        }

        private void UpdateCockpitPanel(int num)
        {
            ushort cockpit = datafile.Cockpits[num];
            txtCockpitID.Text = cockpit.ToString();

            if (pbCockpit.Image != null)
            {
                Bitmap temp = (Bitmap)pbCockpit.Image;
                pbCockpit.Image = null;
                temp.Dispose();
            }
            pbCockpit.Image = datafile.piggyFile.GetBitmap(cockpit);
        }

        private void UpdateSoundPanel(int num)
        {
            //txtSoundID.Text = datafile.Sounds[num].ToString();
            if (datafile.Sounds[num] == 255)
                cbSoundSNDid.SelectedIndex = 0;
            else
                cbSoundSNDid.SelectedIndex = datafile.Sounds[num] + 1;
            if (datafile.AltSounds[num] == 255)
                cbLowMemSound.SelectedIndex = 0;
            else
                cbLowMemSound.SelectedIndex = datafile.AltSounds[num] + 1;
            txtElemName.Text = datafile.SoundNames[num];
        }

        //---------------------------------------------------------------------
        // UTILITIES
        //---------------------------------------------------------------------
        

        public double GetFloatFromFixed88(short fixedvalue)
        {
            return (double)fixedvalue / 256D;
        }

        //---------------------------------------------------------------------
        // SHARED UPDATORS
        //---------------------------------------------------------------------

        private void RemapSingleImage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ImageSelector selector = new ImageSelector(host.DefaultPigFile, false);
            if (selector.ShowDialog() == DialogResult.OK)
            {
                isLocked = true;
                int value = selector.Selection;
                int shipvalue = 0;
                switch (button.Tag)
                {
                    case "1":
                        datafile.Textures[ElementNumber] = (ushort)(value);
                        UpdatePictureBox(datafile.piggyFile.GetBitmap(value), pbTexPrev);
                        txtTexID.Text = (value).ToString();
                        break;
                    case "2":
                        datafile.VClips[ElementNumber].frames[(int)nudAnimFrame.Value] = (ushort)(value);
                        UpdatePictureBox(datafile.piggyFile.GetBitmap(value), pbAnimFramePreview);
                        txtAnimFrameNum.Text = (value).ToString();
                        break;
                    case "3":
                        datafile.EClips[ElementNumber].vc.frames[(int)nudAnimFrame.Value] = (ushort)(value);
                        UpdatePictureBox(datafile.piggyFile.GetBitmap(value), pbAnimFramePreview);
                        txtEffectFrameNum.Text = (value).ToString();
                        break;
                    case "4":
                        datafile.Weapons[ElementNumber].bitmap = (ushort)(value);
                        txtWeaponStaticSprite.Text = (value).ToString();
                        break;
                    case "5":
                        shipvalue = (int)nudShipTextures.Value - 2;
                        datafile.multiplayerBitmaps[shipvalue * 2 + 1] = (ushort)(value);
                        UpdateShipTextures(shipvalue);
                        break;
                    case "6":
                        shipvalue = (int)nudShipTextures.Value - 2;
                        datafile.multiplayerBitmaps[shipvalue * 2] = (ushort)(value);
                        UpdateShipTextures(shipvalue);
                        break;
                    case "7":
                        txtGagueLores.Text = (value).ToString();
                        datafile.Gauges[ElementNumber] = (ushort)(value);
                        UpdatePictureBox(datafile.piggyFile.GetBitmap(value), pbGagueLores);
                        break;
                    case "8":
                        txtGagueHires.Text = (value).ToString();
                        datafile.GaugesHires[ElementNumber] = (ushort)(value);
                        UpdatePictureBox(datafile.piggyFile.GetBitmap(value), pbGagueHires);
                        break;
                    case "9":
                        txtCockpitID.Text = (value).ToString();
                        datafile.Cockpits[ElementNumber] = (ushort)(value);
                        UpdatePictureBox(datafile.piggyFile.GetBitmap(value), pbCockpit);
                        break;
                    case "10":
                        txtWeaponCockpitImage.Text = (value).ToString();
                        datafile.Weapons[ElementNumber].picture = (ushort)value;
                        break;
                    case "11":
                        txtWeaponCockpitImageh.Text = (value).ToString();
                        datafile.Weapons[ElementNumber].hires_picture = (ushort)value;
                        break;
                }
                isLocked = false;
            }
        }

        private void RemapMultiImage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ImageSelector selector = new ImageSelector(host.DefaultPigFile, true);
            if (selector.ShowDialog() == DialogResult.OK)
            {
                int value = selector.Selection;
                isLocked = true;
                switch (button.Tag)
                {
                    case "1":
                        datafile.VClips[ElementNumber].RemapVClip(value, host.DefaultPigFile);
                        txtAnimFrameCount.Text = datafile.VClips[ElementNumber].num_frames.ToString();
                        txtAnimFrameSpeed.Text = datafile.VClips[ElementNumber].frame_time.ToString();
                        nudAnimFrame.Value = 0;
                        UpdatePictureBox(datafile.piggyFile.GetBitmap(datafile.VClips[ElementNumber].frames[0]), pbAnimFramePreview);
                        break;
                    case "2":
                        datafile.EClips[ElementNumber].vc.RemapVClip(value, host.DefaultPigFile);
                        txtEffectFrameCount.Text = datafile.EClips[ElementNumber].vc.num_frames.ToString();
                        txtEffectFrameSpeed.Text = datafile.EClips[ElementNumber].vc.frame_time.ToString();
                        nudEffectFrame.Value = 0;
                        UpdatePictureBox(datafile.piggyFile.GetBitmap(datafile.EClips[ElementNumber].vc.frames[0]), pbEffectFramePreview);
                        break;
                }
                isLocked = false;
            }
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
            Robot robot = datafile.Robots[ElementNumber];
            robot.UpdateRobot(tagvalue, ref value, (int)nudRobotAI.Value, 0, datafile);
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
                Robot robot = datafile.Robots[ElementNumber];
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
                Robot robot = datafile.Robots[ElementNumber];
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
            Robot robot = datafile.Robots[ElementNumber];
            robot.cloak_type = (sbyte)cmRobotCloak.SelectedIndex;
        }

        private void cmRobotBoss_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            Robot robot = datafile.Robots[ElementNumber];
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
            Robot robot = datafile.Robots[ElementNumber];
            int bosstype = cbRobotAI.SelectedIndex;
            robot.behavior = (byte)(bosstype + 0x80);
        }

        private void cbRobotDropType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            Robot robot = datafile.Robots[ElementNumber];
            robot.ClearAndUpdateDropReference(datafile, cbRobotDropType.SelectedIndex == 1 ? 2 : 7);
            UpdateRobotDropTypes(cbRobotDropType.SelectedIndex, robot);
        }

        private void RobotCheckBox_CheckedChange(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            CheckBox input = (CheckBox)sender;
            Robot robot = datafile.Robots[ElementNumber];
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

        //---------------------------------------------------------------------
        // WEAPON UPDATORS
        //---------------------------------------------------------------------

        private void txtWeaponBounce_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
            {
                return;
            }
            TextBox textBox = (TextBox)sender;
            int tagvalue = int.Parse((string)textBox.Tag);

            int value = int.MinValue;
            if (int.TryParse(textBox.Text, out value))
            {
                Weapon weapon = datafile.Weapons[ElementNumber];
                weapon.UpdateWeapon(tagvalue, value, (int)nudWeaponStr.Value, datafile);
            }
        }

        private void fixedWeaponElemChange_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
            {
                return;
            }
            TextBox textBox = (TextBox)sender;
            string tagstr = (string)textBox.Tag;
            int tagvalue = Int32.Parse(tagstr);

            float fvalue = 0.0f;
            if (float.TryParse(textBox.Text, out fvalue))
            {
                int value = (int)(fvalue * 65536f);
                Weapon weapon = datafile.Weapons[ElementNumber];
                weapon.UpdateWeapon(tagvalue, value, (int)nudWeaponStr.Value, datafile);
            }
        }

        private void WeaponCheckBox_CheckChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            Weapon weapon = datafile.Weapons[ElementNumber];
            CheckBox checkBox = (CheckBox)sender;

            switch (checkBox.Tag)
            {
                case "1":
                    weapon.destroyable = (byte)(checkBox.Checked ? 1 : 0);
                    break;
                case "2":
                    weapon.persistent = (byte)(checkBox.Checked ? 1 : 0);
                    break;
                case "3":
                    weapon.matter = (byte)(checkBox.Checked ? 1 : 0);
                    break;
                case "4":
                    weapon.homing_flag = (byte)(checkBox.Checked ? 1 : 0);
                    break;
                case "5":
                    weapon.flags = (byte)(checkBox.Checked ? 1 : 0);
                    break;
            }
        }

        private void cbWeaponRenderMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            Weapon weapon = datafile.Weapons[ElementNumber];
            int weapontype = cbWeaponRenderMode.SelectedIndex;
            if (weapontype > 3)
            {
                weapontype = 255;
            }
            weapon.render_type = (byte)weapontype;
            UpdateWeaponGraphicControls();
        }

        private void WeaponComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            ComboBox comboBox = (ComboBox)sender;
            Weapon weapon = datafile.Weapons[(int)nudElementNum.Value];
            weapon.UpdateWeapon(int.Parse((string)comboBox.Tag), comboBox.SelectedIndex, (int)nudWeaponStr.Value, datafile);
        }

        private void btnCalculateLW_Click(object sender, EventArgs e)
        {
            Weapon weapon = datafile.Weapons[ElementNumber];
            if (weapon.render_type == 2)
            {
                Polymodel model = datafile.PolygonModels[weapon.model_num];
                double width = Math.Abs(model.mins.x / 65536.0d) + Math.Abs(model.maxs.x / 65536.0d);
                double len = Math.Abs(model.mins.z / 65536.0d) + Math.Abs(model.maxs.z / 65536.0d);
                double ratio = len / width;
                weapon.po_len_to_width_ratio = (int)(ratio * 65536.0d);
                isLocked = true;
                txtWeaponPolyLWRatio.Text = ratio.ToString();
                isLocked = false;
            }
        }

        //---------------------------------------------------------------------
        // TMAPINFO UPDATORS
        //---------------------------------------------------------------------

        private void TextureFlagCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            TMAPInfo tmapinfo = datafile.TMapInfo[ElementNumber];

            CheckBox check = (CheckBox)sender;
            int flagvalue = int.Parse(check.Tag.ToString());
            tmapinfo.updateFlags(flagvalue, check.Checked);
        }

        private void TextureProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            TextBox textBox = (TextBox)sender;
            TMAPInfo info = datafile.TMapInfo[ElementNumber];
            try
            {
                //double value = double.Parse(textBox.Text);
                switch (textBox.Tag)
                {
                    case "0":
                        datafile.Textures[ElementNumber] = ushort.Parse(textBox.Text);
                        break;
                    case "1":
                        info.lighting = (int)(double.Parse(textBox.Text) * 65536.0d);
                        break;
                    case "2":
                        info.damage = (int)(double.Parse(textBox.Text) * 65536.0d);
                        break;
                    case "4":
                        info.slide_u = (short)(double.Parse(textBox.Text) * 256.0d);
                        break;
                    case "5":
                        info.slide_v = (short)(double.Parse(textBox.Text) * 256.0d);
                        break;
                    case "6":
                        info.destroyed = short.Parse(textBox.Text);
                        break;
                }
            }
            catch (Exception)
            {
                statusBar1.Text = "failed to update data!";
            }
        }

        private void cbTexEClip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            int eclipNum = cbTexEClip.SelectedIndex - 1;
            EClip clip = datafile.GetEClip(eclipNum);
            TMAPInfo tmapInfo = datafile.TMapInfo[ElementNumber];
            if (clip == null)
            {
                tmapInfo.eclip_num = -1;
            }
            else
            {
                int clipCurrentID = clip.GetCurrentTMap();
                if (clipCurrentID != -1 && clipCurrentID != ElementNumber)
                {
                    if (MessageBox.Show("This EClip is already assigned to another wall texture, do you want to change it?", "EClip in use", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        TMAPInfo oldTMapInfo = datafile.TMapInfo[clipCurrentID];
                        oldTMapInfo.eclip_num = -1;
                        clip.changing_wall_texture = (short)ElementNumber;

                    }
                    else
                    {
                        cbTexEClip.SelectedIndex = tmapInfo.eclip_num + 1;
                        return;
                    }
                }
                /*else
                {
                    clip.changing_wall_texture = (short)ElementNumber;
                }*/
            }
            tmapInfo.eclip_num = (short)eclipNum;
        }

        //---------------------------------------------------------------------
        // VCLIP UPDATORS
        //---------------------------------------------------------------------

        private void VClipProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            TextBox textBox = (TextBox)sender;
            VClip clip = datafile.VClips[ElementNumber];
            double value;
            if (double.TryParse(textBox.Text, out value))
            {
                switch (textBox.Tag)
                {
                    case "1":
                        int totalTimeFix = (int)(value * 65536);
                        clip.play_time = new Fix(totalTimeFix);
                        clip.frame_time = new Fix(totalTimeFix / clip.num_frames);
                        txtAnimFrameSpeed.Text = clip.frame_time.ToString();
                        break;
                    case "2":
                        clip.light_value = new Fix((int)(value * 65536));
                        break;
                }
            }
        }

        //---------------------------------------------------------------------
        // ECLIP UPDATORS
        //---------------------------------------------------------------------

        private void EClipProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            TextBox textBox = (TextBox)sender;
            EClip clip = datafile.EClips[ElementNumber];
            double value;
            if (double.TryParse(textBox.Text, out value))
            {
                switch (textBox.Tag)
                {
                    case "1":
                        int totalTimeFix = (int)(value * 65536);
                        clip.vc.play_time = new Fix(totalTimeFix);
                        clip.vc.frame_time = new Fix(totalTimeFix / clip.vc.num_frames);
                        txtEffectFrameSpeed.Text = clip.vc.frame_time.ToString();
                        break;
                    case "2":
                        clip.vc.light_value = new Fix((int)(value * 65536));
                        break;
                    case "3":
                        clip.dest_size = new Fix((int)(value * 65536));
                        break;
                    case "4":
                        clip.dest_bm_num = int.Parse(textBox.Text);
                        break;
                }
            }
        }

        private void EClipComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            ComboBox comboBox = (ComboBox)sender;
            EClip clip = datafile.EClips[ElementNumber];
            int value = comboBox.SelectedIndex;
            switch (comboBox.Tag)
            {
                case "1":
                    clip.dest_eclip = (value - 1);
                    break;
                case "2":
                    clip.dest_vclip = (value - 1);
                    break;
                case "3":
                    clip.sound_num = value - 1;
                    break;
                case "4":
                    clip.crit_clip = (value - 1);
                    break;
            }
        }

        //---------------------------------------------------------------------
        // WALL UPDATORS
        //---------------------------------------------------------------------

        private void WallComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            ComboBox comboBox = (ComboBox)sender;
            WClip clip = datafile.WClips[ElementNumber];
            switch (comboBox.Tag)
            {
                case "1":
                    clip.open_sound = (short)(comboBox.SelectedIndex - 1);
                    break;
                case "2":
                    clip.close_sound = (short)(comboBox.SelectedIndex - 1);
                    break;
            }
        }

        private void cbVClipSound_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            ComboBox comboBox = (ComboBox)sender;
            VClip clip = datafile.VClips[ElementNumber];
            clip.sound_num = (short)(comboBox.SelectedIndex - 1);
        }

        private void WallFlag_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            CheckBox checkBox = (CheckBox)sender;
            WClip clip = datafile.WClips[ElementNumber];
            int bit = int.Parse((string)checkBox.Tag);
            if ((clip.flags & bit) != 0)
                clip.flags -= (short)bit;
            else
                clip.flags |= (short)bit;
        }

        private void WallProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            TextBox textBox = (TextBox)sender;
            WClip clip = datafile.WClips[ElementNumber];
            double value;
            if (double.TryParse(textBox.Text, out value))
            {
                switch (textBox.Tag)
                {
                    case "1":
                        int totalTimeFix = (int)(value * 65536);
                        clip.play_time = new Fix(totalTimeFix);
                        break;
                    case "2":
                        clip.filename = textBox.Text.ToCharArray();
                        break;
                }
            }
        }

        //---------------------------------------------------------------------
        // SOUND UPDATORS
        //---------------------------------------------------------------------

        private void cbSoundSNDid_SelectedIndexChanged(object sender, EventArgs e)
        {
            int value = cbSoundSNDid.SelectedIndex;
            if (value == 0)
                datafile.Sounds[ElementNumber] = 255;
            else
                datafile.Sounds[ElementNumber] = (byte)(value - 1);
        }

        private void cbLowMemSound_SelectedIndexChanged(object sender, EventArgs e)
        {
            int value = cbLowMemSound.SelectedIndex;
            if (value == 0)
                datafile.AltSounds[ElementNumber] = 255;
            else
                datafile.AltSounds[ElementNumber] = (byte)(value - 1);
        }

        //---------------------------------------------------------------------
        // MODEL UPDATORS
        //---------------------------------------------------------------------

        private void btnImportModel_Click(object sender, EventArgs e)
        {
            ImportModel(datafile.PolygonModels[ElementNumber]);
        }

        private void btnExportModel_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Parallax Object Files|*.pof";
            saveFileDialog1.FileName = string.Format("model_{0}.pof", ElementNumber);
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                BinaryWriter bw = new BinaryWriter(File.Open(saveFileDialog1.FileName, FileMode.Create));
                POFWriter.SerializePolymodel(bw, datafile.PolygonModels[ElementNumber], short.Parse(StandardUI.options.GetOption("PMVersion", "8")));
                bw.Close();
                bw.Dispose();
            }
        }

        private void ImportModel(Polymodel original)
        {
            int oldNumTextures = original.n_textures;

            List<string> newTextureNames = new List<string>();
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
                datafile.ReplaceModel(ElementNumber, model);
                UpdateModelPanel(ElementNumber);
            }
        }

        private void ModelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            Polymodel model = datafile.PolygonModels[ElementNumber];
            ComboBox comboBox = (ComboBox)sender;
            switch (comboBox.Tag)
            {
                case "1":
                    model.simpler_model = (byte)comboBox.SelectedIndex;
                    break;
                case "2":
                    model.DyingModelnum = comboBox.SelectedIndex - 1;
                    break;
                case "3":
                    model.DeadModelnum = comboBox.SelectedIndex - 1;
                    break;
            }
        }

        //---------------------------------------------------------------------
        // POWERUP UPDATORS
        //---------------------------------------------------------------------

        private void PowerupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            Powerup powerup = datafile.Powerups[ElementNumber];
            ComboBox comboBox = (ComboBox)sender;
            switch (comboBox.Tag)
            {
                case "1":
                    powerup.vclip_num = comboBox.SelectedIndex;
                    break;
                case "2":
                    powerup.hit_sound = comboBox.SelectedIndex;
                    break;
            }
        }

        private void PowerupTextBox_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            Powerup powerup = datafile.Powerups[ElementNumber];
            TextBox textBox = (TextBox)sender;
            double value;
            if (double.TryParse(textBox.Text, out value))
            {
                switch (textBox.Tag)
                {
                    case "3":
                        powerup.size = new Fix((int)(value * 65536.0));
                        break;
                    case "4":
                        powerup.light = new Fix((int)(value * 65536.0));
                        break;
                }
            }
        }

        //---------------------------------------------------------------------
        // REACTOR UPDATOR
        //---------------------------------------------------------------------

        private void cbReactorModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            Reactor reactor = datafile.Reactors[ElementNumber];
            reactor.model_id = cbReactorModel.SelectedIndex;
        }

        //---------------------------------------------------------------------
        // SHIP UPDATORS
        //---------------------------------------------------------------------

        private void ShipProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
            {
                return;
            }
            TextBox textBox = (TextBox)sender;
            int input;
            int tagValue = int.Parse((String)textBox.Tag);
            Ship ship = datafile.PlayerShip;
            if (tagValue == 1 || tagValue == 2)
            {
                if (int.TryParse(textBox.Text, out input))
                {
                    ship.UpdateShip(tagValue, input);
                }
            }
            else
            {
                double temp;
                if (double.TryParse(textBox.Text, out temp))
                {
                    input = (int)(temp * 65536);
                    ship.UpdateShip(tagValue, input);
                }
            }
        }

        private void ShipComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            ComboBox comboBox = (ComboBox)sender;
            switch (comboBox.Tag)
            {
                case "0":
                    datafile.PlayerShip.model_num = (comboBox.SelectedIndex);
                    break;
                case "1":
                    datafile.PlayerShip.expl_vclip_num = (comboBox.SelectedIndex);
                    break;
                case "2":
                    datafile.PlayerShip.markerModel = (comboBox.SelectedIndex);
                    break;
            }
        }

        //---------------------------------------------------------------------
        // GAUGE UPDATORS
        //---------------------------------------------------------------------

        private void GaugeProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            ushort value;
            TextBox textBox = (TextBox)sender;
            if (ushort.TryParse(textBox.Text, out value))
            {
                switch (textBox.Tag)
                {
                    case "1":
                        datafile.Gauges[ElementNumber] = value;
                        UpdatePictureBox(datafile.piggyFile.GetBitmap(value - 1), pbGagueLores);
                        break;
                    case "2":
                        datafile.GaugesHires[ElementNumber] = value;
                        UpdatePictureBox(datafile.piggyFile.GetBitmap(value - 1), pbGagueHires);
                        break;
                }
            }
        }

        //---------------------------------------------------------------------
        // SPECIAL FUNCTIONS
        //---------------------------------------------------------------------

        private void menuItem5_Click(object sender, EventArgs e)
        {
            ElementCopy copyForm = new ElementCopy();

            if (copyForm.ShowDialog() == DialogResult.OK)
            {
                int result = 1;
                int elementNumber = copyForm.elementValue;
                result = datafile.CopyElement(typeTable[tabControl1.SelectedIndex], (int)nudElementNum.Value, elementNumber);
                if (result == -1)
                    MessageBox.Show("Cannot copy to an element that doesn't exist!");
                else if (result == 1)
                    MessageBox.Show("Element cannot be copied!");
            }
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Bitmap table files|*.TBL";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(File.Open(saveFileDialog1.FileName, FileMode.Create));
                sw.Write(BitmapTableFile.GenerateBitmapsTable(datafile, host.DefaultPigFile, host.DefaultSoundFile));
                sw.Flush();
                sw.Close();
                sw.Dispose();
                string modelPath = Path.GetDirectoryName(saveFileDialog1.FileName);
                Polymodel model;
                //for (int i = 0; i < datafile.PolygonModels.Count; i++)
                foreach (int i in BitmapTableFile.pofIndicies)
                {
                    model = datafile.PolygonModels[i];
                    BinaryWriter bw = new BinaryWriter(File.Open(String.Format("{0}{1}{2}", modelPath, Path.DirectorySeparatorChar, BitmapTableFile.pofNames[i]), FileMode.Create));
                    POFWriter.SerializePolymodel(bw, model, 8);
                    bw.Close();
                    bw.Dispose();
                }

            }
        }

        private void mnuFindRefs_Click(object sender, EventArgs e)
        {
            Console.WriteLine("mnuFindRefs_Click: STUB");
        }

        //---------------------------------------------------------------------
        // 3D VIEWER UTILITIES
        //---------------------------------------------------------------------

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (noPMView) return;
            SetupViewport();
            glControl1.Invalidate();
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
            modelRenderer.EmulateSoftware = chkSoftwareOverdraw.Checked;

            modelRenderer.Draw();
            glControl1.SwapBuffers();
        }

        private void SetupViewport()
        {
            if (noPMView) return;
            modelRenderer.SetupViewport(glControl1.Width, glControl1.Height, trackBar2.Value * 0.5d + 4.0d);
        }

        private void HAMEditor2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (glContextCreated)
            {
            }
        }

        private void PMCheckBox_CheckChanged(object sender, EventArgs e)
        {
            glControl1.Invalidate();
        }

        //---------------------------------------------------------------------
        // BASIC MENU FUNCTIONS
        //---------------------------------------------------------------------

        private void mnuSave_Click(object sender, EventArgs e)
        {
            bool compatObjBitmaps = (StandardUI.options.GetOption("CompatObjBitmaps", bool.FalseString) == bool.TrueString);
            datafile.SaveDataFile(datafile.lastFilename, compatObjBitmaps);
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "HAM Files|*.HAM";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                datafile.lastFilename = saveFileDialog1.FileName;
                bool compatObjBitmaps = (StandardUI.options.GetOption("CompatObjBitmaps", bool.FalseString) == bool.TrueString);
                datafile.SaveDataFile(saveFileDialog1.FileName, compatObjBitmaps);
            }
            this.Text = string.Format("{0} - HAM Editor", datafile.Filename);
        }
    }
}
