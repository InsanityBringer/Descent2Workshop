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
using Descent2Workshop.Transactions;
using Descent2Workshop.SaveHandlers;

namespace Descent2Workshop
{
    public partial class HAMEditor : Form
    {
        private static HAMType[] typeTable = { HAMType.TMAPInfo, HAMType.VClip, HAMType.EClip, HAMType.WClip, HAMType.Robot, HAMType.Weapon,
            HAMType.Model, HAMType.Sound, HAMType.Reactor, HAMType.Powerup, HAMType.Ship, HAMType.Gauge, HAMType.Cockpit, HAMType.XLAT };
        public int[] texturelist;
        public EditorHAMFile datafile;
        public StandardUI host;
        public bool isLocked = false;
        public bool glContextCreated = false;
        private ModelRenderer modelRenderer;
        private bool noPMView = false;
        private Palette palette;
        private PIGFile piggyFile;
        private TransactionManager transactionManager = new TransactionManager();

        private int ElementNumber { get { return (int)ElementSpinner.Value; } }
        private int PageNumber { get { return EditorTabs.SelectedIndex; } }

        //I still don't get the VS toolbox. Ugh
        TMAPInfoPanel texturePanel;
        VClipPanel vclipPanel;
        EClipPanel eclipPanel;
        WClipPanel wclipPanel;
        RobotPanel robotPanel;
        WeaponPanel weaponPanel;
        PolymodelPanel polymodelPanel;

        //Save information
        private SaveHandler saveHandler;
        
        public HAMEditor(EditorHAMFile data, StandardUI host, PIGFile piggyFile, Palette palette, SaveHandler saveHandler)
        {
            InitializeComponent();

            texturePanel = new TMAPInfoPanel(transactionManager); components.Add(texturePanel);
            texturePanel.Dock = DockStyle.Fill;
            vclipPanel = new VClipPanel(transactionManager); components.Add(vclipPanel);
            vclipPanel.Dock = DockStyle.Fill;
            eclipPanel = new EClipPanel(transactionManager); components.Add(eclipPanel);
            eclipPanel.Dock = DockStyle.Fill;
            wclipPanel = new WClipPanel(transactionManager); components.Add(wclipPanel);
            wclipPanel.Dock = DockStyle.Fill;
            robotPanel = new RobotPanel(transactionManager, 4); components.Add(robotPanel);
            robotPanel.Dock = DockStyle.Fill;
            weaponPanel = new WeaponPanel(transactionManager, 5); components.Add(weaponPanel);
            weaponPanel.Dock = DockStyle.Fill;
            polymodelPanel = new PolymodelPanel(transactionManager, 6, piggyFile, palette, data); components.Add(polymodelPanel);
            polymodelPanel.Dock = DockStyle.Fill;
            TextureTabPage.Controls.Add(texturePanel);
            VClipTabPage.Controls.Add(vclipPanel);
            EffectsTabPage.Controls.Add(eclipPanel);
            DoorTabPage.Controls.Add(wclipPanel);
            RobotTabPage.Controls.Add(robotPanel);
            WeaponTabPage.Controls.Add(weaponPanel);
            ModelTabPage.Controls.Add(polymodelPanel);

            this.palette = palette;
            this.piggyFile = piggyFile;

            datafile = data;
            this.host = host;
            modelRenderer = new ModelRenderer(datafile, piggyFile, palette);
            this.saveHandler = saveHandler;

            string currentFilename = "Untitled";
            if (saveHandler != null)
                currentFilename = saveHandler.GetUIName();

            this.Text = string.Format("{0} - HAM Editor", currentFilename);

            transactionManager.undoEvent += DoUndoEvent;
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
            //Hacks aaaa
            vclipPanel.Stop();
            eclipPanel.Stop();
            switch (EditorTabs.SelectedIndex)
            {
                case 0:
                    ElementSpinner.Maximum = datafile.TMapInfo.Count - 1;
                    InitTexturePanel();
                    break;
                case 1:
                    ElementSpinner.Maximum = datafile.VClips.Count - 1;
                    InitVClipPanel();
                    break;
                case 2:
                    ElementSpinner.Maximum = datafile.EClips.Count - 1;
                    InitEClipPanel();
                    break;
                case 3:
                    ElementSpinner.Maximum = datafile.WClips.Count - 1;
                    InitWallPanel();
                    break;
                case 4:
                    ElementSpinner.Maximum = datafile.Robots.Count - 1;
                    InitRobotPanel();
                    break;
                case 5:
                    ElementSpinner.Maximum = datafile.Weapons.Count - 1;
                    InitWeaponPanel();
                    break;
                case 6:
                    ElementSpinner.Maximum = datafile.Models.Count - 1;
                    InitModelPanel();
                    break;
                case 7:
                    ElementSpinner.Maximum = datafile.Sounds.Count - 1;
                    InitSoundPanel();
                    break;
                case 8:
                    ElementSpinner.Maximum = datafile.Reactors.Count - 1;
                    InitReactorPanel();
                    break;
                case 9:
                    ElementSpinner.Maximum = datafile.Powerups.Count - 1;
                    InitPowerupPanel();
                    break;
                case 10:
                    ElementSpinner.Maximum = 0;
                    SetElementControl(false, false);
                    break;
                case 11:
                    ElementSpinner.Maximum = datafile.Gauges.Count - 1;
                    SetElementControl(false, false);
                    break;
                case 12:
                    ElementSpinner.Maximum = datafile.Cockpits.Count - 1;
                    SetElementControl(true, false);
                    break;
                case 13:
                    ElementSpinner.Maximum = 2619;
                    SetElementControl(false, false);
                    break;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isLocked = true;
            ElementListInit();
            ElementSpinner.Value = 0;
            FillOutCurrentPanel(EditorTabs.SelectedIndex, 0);
            isLocked = false;
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
                    cbReactorModel.SelectedIndex = reactor.ModelNum;
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
            HAMType type = typeTable[EditorTabs.SelectedIndex];
            int newID = datafile.AddElement(type);
            if (newID != -1)
            {
                //Update the maximum of the numeric up/down control and ensure that any comboboxes that need to be regenerated for the current element are
                ElementListInit();
                ElementSpinner.Value = newID;
            }
        }

        private void DeleteElem_Click(object sender, EventArgs e)
        {
            HAMType type = typeTable[EditorTabs.SelectedIndex];
            int returnv = datafile.DeleteElement(type, ElementNumber);
            if (returnv >= 0)
            {
                //Update the maximum of the numeric up/down control and ensure that any comboboxes that need to be regenerated for the current element are
                ElementListInit();
                isLocked = true;
                if (ElementSpinner.Value >= returnv)
                    ElementSpinner.Value = returnv - 1;
                FillOutCurrentPanel(EditorTabs.SelectedIndex, ElementNumber);
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
                    ElementSpinner.Value = elementList.ElementNumber;
                }
            }
            elementList.Dispose();
        }

        private void ElemName_TextChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            datafile.UpdateName(typeTable[EditorTabs.SelectedIndex], ElementNumber, txtElemName.Text);
        }

        //---------------------------------------------------------------------
        // PANEL INITALIZATION
        // The contents of these boxes can change if the amount of elements is changed,
        // so they must be updated each time before display. 
        //---------------------------------------------------------------------

        private void InitTexturePanel()
        {
            SetElementControl(true, false);
            texturePanel.Init(datafile.EClipNames, palette);
        }

        private void InitVClipPanel()
        {
            SetElementControl(true, true);
            vclipPanel.Init(datafile.SoundNames);
        }

        private void InitEClipPanel()
        {
            SetElementControl(true, true);
            eclipPanel.Init(datafile.VClipNames, datafile.EClipNames, datafile.SoundNames, palette);
        }

        private void InitWallPanel()
        {
            SetElementControl(true, false);
            wclipPanel.Init(datafile.SoundNames);
        }

        private void InitWeaponPanel()
        {
            SetElementControl(true, true);
            weaponPanel.Init(datafile.SoundNames, datafile.VClipNames, datafile.WeaponNames, datafile.ModelNames, piggyFile, palette);
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
            cbLowMemSound.Items.AddRange(datafile.SoundNames.ToArray());
            foreach (SoundData sound in host.DefaultSoundFile.sounds)
                cbSoundSNDid.Items.Add(sound.name);
        }

        private void InitPowerupPanel()
        {
            SetElementControl(true, true);
            cbPowerupPickupSound.Items.Clear();
            cbPowerupSprite.Items.Clear();
            cbPowerupPickupSound.Items.AddRange(datafile.SoundNames.ToArray());
            cbPowerupSprite.Items.AddRange(datafile.VClipNames.ToArray());
        }

        private void InitModelPanel()
        {
            SetElementControl(true, true);
            polymodelPanel.Init(datafile.ModelNames);
        }

        private void InitReactorPanel()
        {
            SetElementControl(true, true);
            cbReactorModel.Items.Clear();
            cbReactorModel.Items.AddRange(datafile.ModelNames.ToArray());
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
            texturePanel.Update(datafile, datafile.piggyFile, num, texture);
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
            pbGagueLores.Image = PiggyBitmapUtilities.GetBitmap(datafile.piggyFile, palette, gague);

            if (pbGagueHires.Image != null)
            {
                Bitmap temp = (Bitmap)pbGagueHires.Image;
                pbGagueHires.Image = null;
                temp.Dispose();
            }
            pbGagueHires.Image = PiggyBitmapUtilities.GetBitmap(datafile.piggyFile, palette, hiresgague);
        }

        public void UpdateVClipPanel(int num)
        {
            vclipPanel.Stop();
            VClip clip = datafile.VClips[num];
            vclipPanel.Update(num, clip, datafile.piggyFile, palette);
            txtElemName.Text = datafile.VClipNames[num];
        }

        public void UpdateEClipPanel(int num)
        {
            eclipPanel.Stop();
            EClip clip = datafile.EClips[num];
            eclipPanel.Update(num, clip, datafile.piggyFile);
            txtElemName.Text = datafile.EClipNames[num];
        }

        public void UpdateWClipPanel(int num)
        {
            WClip clip = datafile.WClips[num];
            wclipPanel.Update(num, clip, datafile, datafile.piggyFile, palette);

            txtElemName.Text = "<unnamed>";
        }

        public void UpdateRobotPanel(int num)
        {
            Robot robot = datafile.Robots[num];
            txtElemName.Text = datafile.RobotNames[num];

            robotPanel.Update(robot, num);
        }

        public void UpdateWeaponPanel(int num)
        {
            Weapon weapon = datafile.Weapons[num];
            txtElemName.Text = datafile.WeaponNames[num];

            weaponPanel.Update(weapon, num);
        }

        private void UpdateModelPanel(int num)
        {
            Polymodel model = datafile.Models[num];
            txtElemName.Text = datafile.ModelNames[num];

            polymodelPanel.Update(model, num);
        }

        private void UpdatePowerupPanel(int num)
        {
            Powerup powerup = datafile.Powerups[num];
            cbPowerupPickupSound.SelectedIndex = powerup.HitSound;
            cbPowerupSprite.SelectedIndex = powerup.VClipNum;
            txtPowerupSize.Text = powerup.Size.ToString();
            txtPowerupLight.Text = powerup.Light.ToString();
            txtElemName.Text = datafile.PowerupNames[num];
        }

        private void UpdateShipPanel()
        {
            Ship ship = datafile.PlayerShip;
            cbPlayerExplosion.Items.Clear();
            for (int i = 0; i < datafile.VClips.Count; i++)
                cbPlayerExplosion.Items.Add(datafile.VClipNames[i]);
            cbPlayerModel.Items.Clear(); cbMarkerModel.Items.Clear();
            for (int i = 0; i < datafile.Models.Count; i++)
            {
                cbPlayerModel.Items.Add(datafile.ModelNames[i]);
                cbMarkerModel.Items.Add(datafile.ModelNames[i]);
            }

            txtShipBrakes.Text = ship.Brakes.ToString();
            txtShipDrag.Text = ship.Drag.ToString();
            txtShipMass.Text = ship.Mass.ToString();
            txtShipMaxRotThrust.Text = ship.MaxRotationThrust.ToString();
            txtShipRevThrust.Text = ship.ReverseThrust.ToString();
            txtShipThrust.Text = ship.MaxThrust.ToString();
            txtShipWiggle.Text = ship.Wiggle.ToString();
            nudShipTextures.Value = nudShipTextures.Minimum;
            UpdateShipTextures(0);
            //This can thereoetically null, but it never will except on deformed data that descent itself probably wouldn't like
            cbPlayerExplosion.SelectedIndex = ship.DeathVClipNum;
            cbMarkerModel.SelectedIndex = ship.MarkerModel;
            cbPlayerModel.SelectedIndex = ship.ModelNum;

            txtElemName.Text = "Ship";
        }

        private void nudShipTextures_ValueChanged(object sender, EventArgs e)
        {
            UpdateShipTextures((int)nudShipTextures.Value - 2);
        }

        private void UpdateShipTextures(int id)
        {
            UpdatePictureBox(PiggyBitmapUtilities.GetBitmap(datafile.piggyFile, palette, datafile.multiplayerBitmaps[id * 2]), pbWeaponTexture);
            UpdatePictureBox(PiggyBitmapUtilities.GetBitmap(datafile.piggyFile, palette, datafile.multiplayerBitmaps[id * 2 + 1]), pbWingTex);
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
            pbBitmapSrc.Image = PiggyBitmapUtilities.GetBitmap(datafile.piggyFile, palette, num);

            if (pbBitmapDest.Image != null)
            {
                Bitmap temp = (Bitmap)pbBitmapDest.Image;
                pbBitmapDest.Image = null;
                temp.Dispose();
            }
            pbBitmapDest.Image = PiggyBitmapUtilities.GetBitmap(datafile.piggyFile, palette, dst);
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
            pbCockpit.Image = PiggyBitmapUtilities.GetBitmap(datafile.piggyFile, palette, cockpit);
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
        // SHARED UPDATORS
        //---------------------------------------------------------------------

        private void RemapSingleImage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ImageSelector selector = new ImageSelector(piggyFile, palette, false);
            if (selector.ShowDialog() == DialogResult.OK)
            {
                isLocked = true;
                int value = selector.Selection;
                int shipvalue = 0;
                switch (button.Tag)
                {
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
                        UpdatePictureBox(PiggyBitmapUtilities.GetBitmap(datafile.piggyFile, palette, value), pbGagueLores);
                        break;
                    case "8":
                        txtGagueHires.Text = (value).ToString();
                        datafile.GaugesHires[ElementNumber] = (ushort)(value);
                        UpdatePictureBox(PiggyBitmapUtilities.GetBitmap(datafile.piggyFile, palette, value), pbGagueHires);
                        break;
                    case "9":
                        txtCockpitID.Text = (value).ToString();
                        datafile.Cockpits[ElementNumber] = (ushort)(value);
                        UpdatePictureBox(PiggyBitmapUtilities.GetBitmap(datafile.piggyFile, palette, value), pbCockpit);
                        break;
                }
                isLocked = false;
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
            ImportModel(datafile.Models[ElementNumber]);
        }

        private void btnExportModel_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Parallax Object Files|*.pof";
            saveFileDialog1.FileName = string.Format("model_{0}.pof", ElementNumber);
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                BinaryWriter bw = new BinaryWriter(File.Open(saveFileDialog1.FileName, FileMode.Create));
                POFWriter.SerializePolymodel(bw, datafile.Models[ElementNumber], short.Parse(StandardUI.options.GetOption("PMVersion", "8")));
                bw.Close();
                bw.Dispose();
            }
        }

        private void ImportModel(Polymodel original)
        {
            int oldNumTextures = original.NumTextures;

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
                //TODO: does this work?
                //datafile.ReplaceModel(ElementNumber, model);
                datafile.Models[ElementNumber] = model;
                UpdateModelPanel(ElementNumber);
            }
        }

        private void ModelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            Polymodel model = datafile.Models[ElementNumber];
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
                    powerup.VClipNum = comboBox.SelectedIndex;
                    break;
                case "2":
                    powerup.HitSound = comboBox.SelectedIndex;
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
                        powerup.Size = value;
                        break;
                    case "4":
                        powerup.Light = value;
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
            reactor.ModelNum = cbReactorModel.SelectedIndex;
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
                    datafile.PlayerShip.ModelNum = (comboBox.SelectedIndex);
                    break;
                case "1":
                    datafile.PlayerShip.DeathVClipNum = (comboBox.SelectedIndex);
                    break;
                case "2":
                    datafile.PlayerShip.MarkerModel = (comboBox.SelectedIndex);
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
                        UpdatePictureBox(PiggyBitmapUtilities.GetBitmap(datafile.piggyFile, palette, value - 1), pbGagueLores);
                        break;
                    case "2":
                        datafile.GaugesHires[ElementNumber] = value;
                        UpdatePictureBox(PiggyBitmapUtilities.GetBitmap(datafile.piggyFile, palette, value - 1), pbGagueHires);
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
                result = datafile.CopyElement(typeTable[EditorTabs.SelectedIndex], (int)ElementSpinner.Value, elementNumber);
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
                sw.Write(BitmapTableFile.GenerateBitmapsTable(datafile, piggyFile, host.DefaultSoundFile));
                sw.Flush();
                sw.Close();
                sw.Dispose();
                string modelPath = Path.GetDirectoryName(saveFileDialog1.FileName);
                Polymodel model;
                if (datafile.Models.Count >= 160)
                {
                    //for (int i = 0; i < datafile.PolygonModels.Count; i++)
                    foreach (int i in BitmapTableFile.pofIndicies)
                    {
                        model = datafile.Models[i];
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
        // BASIC MENU FUNCTIONS
        //---------------------------------------------------------------------

        /// <summary>
        /// Helper function to save a HAM file, with lots of dumb error handling that doesn't work probably.
        /// </summary>
        private void SaveHAMFile()
        {
            if (saveHandler == null) //No save handler, so can't actually save right now
            {
                //HACK: Just call the save as handler for now... This needs restructuring
                mnuSaveAs_Click(this, new EventArgs());
                return; //and don't repeat the save code when we recall ourselves.
            }
            Stream stream = saveHandler.GetStream();
            if (stream == null)
            {
                MessageBox.Show(this, string.Format("Error opening save file {0}:\r\n{1}",saveHandler.GetUIName(), saveHandler.GetErrorMsg()));
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

        private void mnuSave_Click(object sender, EventArgs e)
        {
            bool compatObjBitmaps = (StandardUI.options.GetOption("CompatObjBitmaps", bool.FalseString) == bool.TrueString);
            SaveHAMFile();
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "HAM Files|*.HAM";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bool compatObjBitmaps = (StandardUI.options.GetOption("CompatObjBitmaps", bool.FalseString) == bool.TrueString);
                //Ensure the old save handler is detached from any UI properly.
                if (saveHandler != null) saveHandler.Destroy();
                saveHandler = new FileSaveHandler(saveFileDialog1.FileName);
                SaveHAMFile();
                this.Text = string.Format("{0} - HAM Editor", saveHandler.GetUIName());
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

            if (transaction.Page != ElementNumber)
                ElementSpinner.Value = transaction.Page;
            else
                FillOutCurrentPanel(transaction.Tab, transaction.Page); //force an update
        }

        private void HAMEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            string currentFilename = "Untitled";
            if (saveHandler != null)
                currentFilename = saveHandler.GetUIName();

            if (transactionManager.UnsavedFlag)
            {
                switch (MessageBox.Show(this, string.Format("Would you like to save changes to {0}?", currentFilename), "Unsaved changes", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        SaveHAMFile();
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }
    }
}
