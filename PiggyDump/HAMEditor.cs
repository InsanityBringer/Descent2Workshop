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
using Descent2Workshop.EditorPanels;

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

        //I still don't get the VS toolbox. Ugh
        RobotPanel robotPanel;
        WeaponPanel weaponPanel;
        
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

            robotPanel = new RobotPanel();
            robotPanel.Dock = DockStyle.Fill;
            weaponPanel = new WeaponPanel();
            weaponPanel.Dock = DockStyle.Fill;
            RobotTabPage.Controls.Add(robotPanel);
            WeaponTabPage.Controls.Add(weaponPanel);

            if (!noPMView)
                this.ModelTabPage.Controls.Add(this.glControl1);
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
                cbTexEClip.Items.Add(datafile.EClipNames[i] + " #" + i);
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
            weaponPanel.Init(datafile.SoundNames, datafile.VClipNames, datafile.WeaponNames, datafile.ModelNames, host.DefaultPigFile);
        }

        private void InitRobotPanel()
        {
            SetElementControl(true, true);
            robotPanel.Init(datafile.VClipNames, datafile.SoundNames, datafile.RobotNames, datafile.WeaponNames, datafile.PowerupNames, datafile.ModelNames);
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

            UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, datafile.Textures[num]), pbTexPrev);
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
            pbGagueLores.Image = PiggyBitmapConverter.GetBitmap(datafile.piggyFile, gague);

            if (pbGagueHires.Image != null)
            {
                Bitmap temp = (Bitmap)pbGagueHires.Image;
                pbGagueHires.Image = null;
                temp.Dispose();
            }
            pbGagueHires.Image = PiggyBitmapConverter.GetBitmap(datafile.piggyFile, hiresgague);
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

        public void UpdateRobotPanel(int num)
        {
            Robot robot = datafile.Robots[num];
            robotPanel.Update(robot);
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
            UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, datafile.multiplayerBitmaps[id * 2]), pbWeaponTexture);
            UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, datafile.multiplayerBitmaps[id * 2 + 1]), pbWingTex);
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
            pbAnimFramePreview.Image = PiggyBitmapConverter.GetBitmap(datafile.piggyFile, animation.frames[frame]);
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
            pbEffectFramePreview.Image = PiggyBitmapConverter.GetBitmap(datafile.piggyFile, animation.vc.frames[frame]);
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
            pbWallAnimPreview.Image = PiggyBitmapConverter.GetBitmap(datafile.piggyFile, datafile.Textures[animation.frames[frame]]);
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
            pbBitmapSrc.Image = PiggyBitmapConverter.GetBitmap(datafile.piggyFile, num);

            if (pbBitmapDest.Image != null)
            {
                Bitmap temp = (Bitmap)pbBitmapDest.Image;
                pbBitmapDest.Image = null;
                temp.Dispose();
            }
            pbBitmapDest.Image = PiggyBitmapConverter.GetBitmap(datafile.piggyFile, dst);
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
            pbCockpit.Image = PiggyBitmapConverter.GetBitmap(datafile.piggyFile, cockpit);
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
                        UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, value), pbTexPrev);
                        txtTexID.Text = (value).ToString();
                        break;
                    case "2":
                        datafile.VClips[ElementNumber].frames[(int)nudAnimFrame.Value] = (ushort)(value);
                        UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, value), pbAnimFramePreview);
                        txtAnimFrameNum.Text = (value).ToString();
                        break;
                    case "3":
                        datafile.EClips[ElementNumber].vc.frames[(int)nudAnimFrame.Value] = (ushort)(value);
                        UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, value), pbAnimFramePreview);
                        txtEffectFrameNum.Text = (value).ToString();
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
                        UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, value), pbGagueLores);
                        break;
                    case "8":
                        txtGagueHires.Text = (value).ToString();
                        datafile.GaugesHires[ElementNumber] = (ushort)(value);
                        UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, value), pbGagueHires);
                        break;
                    case "9":
                        txtCockpitID.Text = (value).ToString();
                        datafile.Cockpits[ElementNumber] = (ushort)(value);
                        UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, value), pbCockpit);
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
                        UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, datafile.VClips[ElementNumber].frames[0]), pbAnimFramePreview);
                        break;
                    case "2":
                        datafile.EClips[ElementNumber].vc.RemapVClip(value, host.DefaultPigFile);
                        txtEffectFrameCount.Text = datafile.EClips[ElementNumber].vc.num_frames.ToString();
                        txtEffectFrameSpeed.Text = datafile.EClips[ElementNumber].vc.frame_time.ToString();
                        nudEffectFrame.Value = 0;
                        UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, datafile.EClips[ElementNumber].vc.frames[0]), pbEffectFramePreview);
                        break;
                }
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
                        UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, value - 1), pbGagueLores);
                        break;
                    case "2":
                        datafile.GaugesHires[ElementNumber] = value;
                        UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, value - 1), pbGagueHires);
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
                if (datafile.PolygonModels.Count >= 160)
                {
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
