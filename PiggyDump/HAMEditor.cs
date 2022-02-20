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
        private EditorHAMFile datafile;
        private StandardUI host;
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
        SoundPanel soundPanel;
        ReactorPanel reactorPanel;

        //Save information
        private SaveHandler saveHandler;

        //Hacks
        private bool isLocked = false;
        private bool editedName = false;

        public HAMEditor(EditorHAMFile data, StandardUI host, PIGFile piggyFile, Palette palette, SaveHandler saveHandler)
        {
            InitializeComponent();

            this.palette = palette;
            this.piggyFile = piggyFile;

            datafile = data;
            this.host = host;
            this.saveHandler = saveHandler;

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
            soundPanel = new SoundPanel(transactionManager, 7, datafile, host.DefaultSoundFile); components.Add(soundPanel);
            soundPanel.Dock = DockStyle.Fill;
            reactorPanel = new ReactorPanel(transactionManager, 8); components.Add(reactorPanel);
            reactorPanel.Dock = DockStyle.Fill;
            TextureTabPage.Controls.Add(texturePanel);
            VClipTabPage.Controls.Add(vclipPanel);
            EffectsTabPage.Controls.Add(eclipPanel);
            DoorTabPage.Controls.Add(wclipPanel);
            RobotTabPage.Controls.Add(robotPanel);
            WeaponTabPage.Controls.Add(weaponPanel);
            ModelTabPage.Controls.Add(polymodelPanel);
            SoundTabPage.Controls.Add(soundPanel);
            ReactorTabPage.Controls.Add(reactorPanel);

            string currentFilename = "Untitled";
            if (saveHandler != null)
                currentFilename = saveHandler.GetUIName();

            this.Text = string.Format("{0} - HAM Editor", currentFilename);

            transactionManager.undoEvent += DoUndoEvent;

            datafile.CompatObjBitmaps = StandardUI.options.GetOption("CompatObjBitmaps", bool.FalseString) == bool.TrueString;
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
                    ElementSpinner.Maximum = Math.Max(0, datafile.TMapInfo.Count - 1);
                    InitTexturePanel();
                    break;
                case 1:
                    ElementSpinner.Maximum = Math.Max(0, datafile.VClips.Count - 1);
                    InitVClipPanel();
                    break;
                case 2:
                    ElementSpinner.Maximum = Math.Max(0, datafile.EClips.Count - 1);
                    InitEClipPanel();
                    break;
                case 3:
                    ElementSpinner.Maximum = Math.Max(0, datafile.WClips.Count - 1);
                    InitWallPanel();
                    break;
                case 4:
                    ElementSpinner.Maximum = Math.Max(0, datafile.Robots.Count - 1);
                    InitRobotPanel();
                    break;
                case 5:
                    ElementSpinner.Maximum = Math.Max(0, datafile.Weapons.Count - 1);
                    InitWeaponPanel();
                    break;
                case 6:
                    ElementSpinner.Maximum = Math.Max(0, datafile.Models.Count - 1);
                    InitModelPanel();
                    break;
                case 7:
                    ElementSpinner.Maximum = Math.Max(0, datafile.Sounds.Length - 1);
                    InitSoundPanel();
                    break;
                case 8:
                    ElementSpinner.Maximum = Math.Max(0, datafile.Reactors.Count - 1);
                    InitReactorPanel();
                    break;
                case 9:
                    ElementSpinner.Maximum = Math.Max(0, datafile.Powerups.Count - 1);
                    InitPowerupPanel();
                    break;
                case 10:
                    ElementSpinner.Maximum = 0;
                    SetElementControl(false, false);
                    break;
                case 11:
                    ElementSpinner.Maximum = Math.Max(0, datafile.Gauges.Count - 1);
                    SetElementControl(false, false);
                    break;
                case 12:
                    ElementSpinner.Maximum = Math.Max(0, datafile.Cockpits.Count - 1);
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

        private void SetElementNumber(int number)
        {
            if (!isLocked)
            {
                ElementSpinner.Value = number;
                isLocked = true;
            }

            FillOutCurrentPanel(EditorTabs.SelectedIndex, (int)ElementSpinner.Value);
            
            isLocked = false;
        }

        private void nudElementNum_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                SetElementNumber((int)ElementSpinner.Value);
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
                    UpdateReactorPanel(val);
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
            /*int newID = datafile.AddElement(type);
            if (newID != -1)
            {
                //Update the maximum of the numeric up/down control and ensure that any comboboxes that need to be regenerated for the current element are
                ElementListInit();
                ElementSpinner.Value = newID;
            }*/
            int newNum = 0;
            Transaction transaction = null;

            switch (type)
            {
                case HAMType.TMAPInfo:
                    newNum = datafile.Textures.Count;
                    transaction = new AddTextureTransaction("Add Texture", datafile, "Textures", "TMapInfo", datafile.Textures.Count, 0, new TMAPInfo(), ElementNumber, PageNumber);
                    break;
                case HAMType.VClip:
                    newNum = datafile.VClips.Count;
                    transaction = new ListAddTransaction("Add VClip", datafile, "VClips", datafile.VClips.Count, new VClip(), ElementNumber, PageNumber);
                    break;
                case HAMType.EClip:
                    newNum = datafile.EClips.Count;
                    transaction = new ListAddTransaction("Add EClip", datafile, "EClips", datafile.EClips.Count, new EClip(), ElementNumber, PageNumber);
                    break;
                case HAMType.WClip:
                    newNum = datafile.WClips.Count;
                    transaction = new ListAddTransaction("Add WClip", datafile, "WClips", datafile.WClips.Count, new WClip(), ElementNumber, PageNumber);
                    break;
                case HAMType.Robot:
                    newNum = datafile.Robots.Count;
                    transaction = new ListAddTransaction("Add Robot", datafile, "Robots", datafile.Robots.Count, new Robot(), ElementNumber, PageNumber);
                    break;
                case HAMType.Weapon:
                    newNum = datafile.Weapons.Count;
                    transaction = new ListAddTransaction("Add Weapon", datafile, "Weapons", datafile.Weapons.Count, new Weapon(), ElementNumber, PageNumber);
                    break;
                case HAMType.Model:
                    newNum = datafile.Models.Count;
                    transaction = new ListAddTransaction("Add Polymodel", datafile, "Models", datafile.Models.Count, new Polymodel(10), ElementNumber, PageNumber);
                    break;
                case HAMType.Powerup:
                    newNum = datafile.Powerups.Count;
                    transaction = new ListAddTransaction("Add Powerup", datafile, "Powerups", datafile.Powerups.Count, new Powerup(), ElementNumber, PageNumber);
                    break;
                case HAMType.Reactor:
                    newNum = datafile.Reactors.Count;
                    transaction = new ListAddTransaction("Add Reactor", datafile, "Reactors", datafile.Reactors.Count, new Reactor(), ElementNumber, PageNumber);
                    break;
                case HAMType.Cockpit:
                    newNum = datafile.Cockpits.Count;
                    transaction = new ListAddTransaction("Add Cockpit bitmap", datafile, "Cockpits", datafile.Cockpits.Count, (ushort)0, ElementNumber, PageNumber);
                    break;
            }

            if (transaction != null)
            {
                transactionManager.ApplyTransaction(transaction);
                ElementListInit(); //Ensure all local lists are updated, and update the spinner. 
                ElementSpinner.Value = newNum;
            }
        }

        private void DeleteElem_Click(object sender, EventArgs e)
        {
            HAMType type = typeTable[EditorTabs.SelectedIndex];
            Transaction transaction = null;
            int maxNum = 0;

            switch (type)
            {
                case HAMType.TMAPInfo:
                    transaction = new DeleteTMAPInfoTransaction(datafile, ElementNumber, PageNumber);
                    maxNum = Math.Max(0, datafile.TMapInfo.Count - 1);
                    break;
            }

            if (transaction != null)
            {
                transactionManager.ApplyTransaction(transaction);
                ElementListInit(); //Ensure all local lists are updated, and update the spinner. 
                SetElementNumber(Math.Min(ElementNumber, maxNum));
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
            editedName = true;
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
            texturePanel.Init(datafile.EClips, palette);
        }

        private void InitVClipPanel()
        {
            SetElementControl(true, true);
            vclipPanel.Init(datafile.SoundNames);
        }

        private void InitEClipPanel()
        {
            SetElementControl(true, true);
            eclipPanel.Init(datafile.VClips, datafile.EClips, datafile.SoundNames, palette);
        }

        private void InitWallPanel()
        {
            SetElementControl(true, false);
            wclipPanel.Init(datafile.SoundNames);
        }

        private void InitWeaponPanel()
        {
            SetElementControl(true, true);
            weaponPanel.Init(datafile.SoundNames, datafile.VClips, datafile.Weapons, datafile.Models, piggyFile, palette);
        }

        private void InitRobotPanel()
        {
            SetElementControl(true, true);
            robotPanel.Init(datafile.VClips, datafile.SoundNames, datafile.Robots, datafile.Weapons, datafile.Powerups, datafile.Models);
        }

        private void InitSoundPanel()
        {
            SetElementControl(false, true);
            soundPanel.Init(datafile.SoundNames);
        }

        private void InitPowerupPanel()
        {
            SetElementControl(true, true);
            cbPowerupPickupSound.Items.Clear();
            cbPowerupSprite.Items.Clear();
            cbPowerupPickupSound.Items.AddRange(datafile.SoundNames.ToArray());
            string[] stringarray = new string[datafile.VClips.Count];
            for (int i = 0; i < datafile.VClips.Count; i++)
                stringarray[i] = datafile.VClips[i].Name;
            cbPowerupSprite.Items.AddRange(stringarray);
        }

        private void InitModelPanel()
        {
            SetElementControl(true, true);
            polymodelPanel.Init(datafile.Models);
        }

        private void InitReactorPanel()
        {
            SetElementControl(true, true);
            reactorPanel.Init(datafile.Models);
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
            if (datafile.TMapInfo.Count == 0)
            {
                statusBar1.Text = "No TMAPInfo present";
                texturePanel.Enabled = false;
            }
            else
            {
                statusBar1.Text = "";
                texturePanel.Enabled = true;
                TMAPInfo texture = datafile.TMapInfo[num];
                texturePanel.Update(datafile, datafile.piggyFile, num, texture);
            }
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
            txtElemName.Text = clip.Name;
        }

        public void UpdateEClipPanel(int num)
        {
            eclipPanel.Stop();
            EClip clip = datafile.EClips[num];
            eclipPanel.Update(num, clip, datafile.piggyFile);
            txtElemName.Text = clip.Name;
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
            txtElemName.Text = robot.Name;

            robotPanel.Update(robot, num);
        }

        public void UpdateWeaponPanel(int num)
        {
            Weapon weapon = datafile.Weapons[num];
            txtElemName.Text = weapon.Name;

            weaponPanel.Update(weapon, num);
        }

        private void UpdateModelPanel(int num)
        {
            Polymodel model = datafile.Models[num];
            txtElemName.Text = model.Name;

            polymodelPanel.Update(model, num);
        }

        private void UpdateReactorPanel(int num)
        {
            Reactor reactor = datafile.Reactors[num];
            txtElemName.Text = reactor.Name;

            reactorPanel.Update(reactor, num);
        }

        private void UpdatePowerupPanel(int num)
        {
            Powerup powerup = datafile.Powerups[num];
            cbPowerupPickupSound.SelectedIndex = powerup.HitSound;
            cbPowerupSprite.SelectedIndex = powerup.VClipNum;
            txtPowerupSize.Text = powerup.Size.ToString();
            txtPowerupLight.Text = powerup.Light.ToString();
            txtElemName.Text = powerup.Name;
        }

        private void UpdateShipPanel()
        {
            Ship ship = datafile.PlayerShip;
            cbPlayerExplosion.Items.Clear();
            for (int i = 0; i < datafile.VClips.Count; i++)
                cbPlayerExplosion.Items.Add(datafile.VClips[i].Name);
            cbPlayerModel.Items.Clear(); cbMarkerModel.Items.Clear();
            for (int i = 0; i < datafile.Models.Count; i++)
            {
                cbPlayerModel.Items.Add(datafile.Models[i].Name);
                cbMarkerModel.Items.Add(datafile.Models[i].Name);
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
            soundPanel.Update(num);
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

            //Sigh. This code is awful and needs to be rewritten.
            //Hack to ensure the spinner and all combo boxes are set correctly.
            if (transaction.ChangesListSize())
            {
                isLocked = true;
                ElementSpinner.Value = 0;
                ElementListInit();
                isLocked = false;
                if (e.Redo)
                    ElementSpinner.Value = transaction.RedoPage;
                else
                    ElementSpinner.Value = transaction.Page;

                FillOutCurrentPanel(transaction.Tab, ElementNumber);
            }
            else
            {
                if (transaction.Page != ElementNumber)
                    ElementSpinner.Value = transaction.Page;
                else
                    FillOutCurrentPanel(transaction.Tab, transaction.Page); //force an update
            }

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

        private void txtElemName_Leave(object sender, EventArgs e)
        {
            //This is a big hack. This is needed to ensure local lists are updated when you change a name.
            //Names aren't automatically updated when changing which element is visible, so this makes sure
            //the name will be changed when it should be, or if you want to make von neumann constructs.
            if (editedName)
            {
                switch (PageNumber)
                {
                    case 2:
                        eclipPanel.ChangeOwnName(txtElemName.Text);
                        break;
                    case 4:
                        robotPanel.ChangeOwnName(txtElemName.Text);
                        break;
                    case 5:
                        weaponPanel.ChangeOwnName(txtElemName.Text);
                        break;
                    case 6:
                        polymodelPanel.ChangeOwnName(txtElemName.Text);
                        break;
                    case 7:
                        soundPanel.ChangeOwnName(txtElemName.Text);
                        break;
                }
                editedName = false;
            }
        }
    }
}
