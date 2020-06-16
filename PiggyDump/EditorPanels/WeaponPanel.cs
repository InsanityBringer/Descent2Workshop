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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibDescent.Data;
using Descent2Workshop.Transactions;

namespace Descent2Workshop.EditorPanels
{
    public partial class WeaponPanel : UserControl
    {
        private Weapon weapon;
        //Needed to remap graphics
        private PIGFile piggyFile; //aaa
        private Palette palette; //aaaaaaaaaaaaaaaaaaaa
        private bool isLocked = false;

        private TransactionManager transactionManager;
        private int tabPage;
        private int weaponID;

        private TextBox[] SpeedBoxes = new TextBox[5];
        private TextBox[] DmgBoxes = new TextBox[5];
        public WeaponPanel(TransactionManager transactionManager, int tabPage)
        {
            InitializeComponent();
            SpeedBoxes[0] = txtWeaponSpeed; SpeedBoxes[1] = Speed1; SpeedBoxes[2] = Speed2; SpeedBoxes[3] = Speed3; SpeedBoxes[4] = Speed4;
            DmgBoxes[0] = txtWeaponStr; DmgBoxes[1] = Damage1; DmgBoxes[2] = Damage2; DmgBoxes[3] = Damage3; DmgBoxes[4] = Damage4;
            this.transactionManager = transactionManager;
            this.tabPage = tabPage;
        }

        public void Init(List<string> SoundNames, List<string> VClipNames, List<string> WeaponNames, List<string> ModelNames, PIGFile piggyFile, Palette palette)
        {
            this.piggyFile = piggyFile;
            this.palette = palette;
            isLocked = true;
            cbWeaponFireSound.Items.Clear(); cbWeaponFireSound.Items.Add("None");
            cbWeaponRobotHitSound.Items.Clear(); cbWeaponRobotHitSound.Items.Add("None");
            cbWeaponWallHitSound.Items.Clear(); cbWeaponWallHitSound.Items.Add("None");

            string[] stringarray = SoundNames.ToArray();
            cbWeaponFireSound.Items.AddRange(stringarray);
            cbWeaponRobotHitSound.Items.AddRange(stringarray);
            cbWeaponWallHitSound.Items.AddRange(stringarray);

            cbWeaponMuzzleFlash.Items.Clear(); cbWeaponMuzzleFlash.Items.Add("None");
            cbWeaponWallHit.Items.Clear(); cbWeaponWallHit.Items.Add("None");
            cbWeaponRobotHit.Items.Clear(); cbWeaponRobotHit.Items.Add("None");
            cbWeaponVClip.Items.Clear(); cbWeaponVClip.Items.Add("None");

            stringarray = VClipNames.ToArray();
            cbWeaponMuzzleFlash.Items.AddRange(stringarray);
            cbWeaponWallHit.Items.AddRange(stringarray);
            cbWeaponRobotHit.Items.AddRange(stringarray);
            cbWeaponVClip.Items.AddRange(stringarray);

            cbWeaponChildren.Items.Clear(); cbWeaponChildren.Items.Add("None"); //this will be fun since my own size can change. ugh
            cbWeaponChildren.Items.AddRange(WeaponNames.ToArray());
            cbWeaponModel1.Items.Clear(); cbWeaponModel1.Items.Add("None");
            cbWeaponModel2.Items.Clear(); cbWeaponModel2.Items.Add("None");

            stringarray = ModelNames.ToArray();
            cbWeaponModel1.Items.AddRange(stringarray);
            cbWeaponModel2.Items.AddRange(stringarray);
            isLocked = false;
        }

        public void ChangeOwnName(string newname)
        {
            cbWeaponChildren.Items[weaponID+1] = newname;
        }

        public void Update(Weapon weapon, int num)
        {
            isLocked = true;
            this.weapon = weapon;
            this.weaponID = num;

            int rendernum = (int)weapon.RenderType;

            if (rendernum == (int)WeaponRenderType.Invisible)
            {
                rendernum = 4;
            }

            cbWeaponRenderMode.SelectedIndex = rendernum;

            txtWeaponABSize.Text = weapon.AfterburnerSize.ToString();
            txtWeaponAmmoUse.Text = weapon.AmmoUsage.ToString();
            txtWeaponBlindSize.Text = weapon.Flash.ToString();
            cbWeaponBounce.SelectedIndex = (int)weapon.Bounce;
            txtWeaponCockpitImage.Text = weapon.CockpitPicture.ToString();
            txtWeaponCockpitImageh.Text = weapon.HiresCockpitPicture.ToString();
            txtWeaponDrag.Text = weapon.Drag.ToString();
            txtWeaponEnergyUsage.Text = weapon.EnergyUsage.ToString();
            txtWeaponExplosionSize.Text = weapon.DamageRadius.ToString();
            txtWeaponFireWait.Text = weapon.FireWait.ToString();
            txtWeaponFlashSize.Text = weapon.FlashSize.ToString();
            txtWeaponImpactSize.Text = weapon.ImpactSize.ToString();
            txtWeaponLifetime.Text = weapon.Lifetime.ToString();
            txtWeaponLight.Text = weapon.Light.ToString();
            txtWeaponMass.Text = weapon.Mass.ToString();
            txtWeaponMPScale.Text = weapon.MultiDamageScale.ToString();
            txtWeaponPolyLWRatio.Text = weapon.POLenToWidthRatio.ToString();
            txtWeaponProjectileCount.Text = weapon.FireCount.ToString();
            txtWeaponProjectileSize.Text = weapon.BlobSize.ToString();
            txtWeaponSpeedvar.Text = weapon.SpeedVariance.ToString();
            txtWeaponStaticSprite.Text = weapon.Bitmap.ToString();
            txtWeaponThrust.Text = weapon.Thrust.ToString();

            cbWeaponDestroyable.Checked = weapon.Destroyable;
            cbWeaponHoming.Checked = weapon.HomingFlag;
            cbWeaponIsMatter.Checked = weapon.Matter;
            cbWeaponPlacable.Checked = (weapon.Flags & 1) != 0;
            cbWeaponRipper.Checked = weapon.Persistent;

            cbWeaponChildren.SelectedIndex = weapon.Children + 1;
            cbWeaponFireSound.SelectedIndex = weapon.FiringSound + 1;
            cbWeaponRobotHitSound.SelectedIndex = weapon.RobotHitSound + 1;
            cbWeaponWallHitSound.SelectedIndex = weapon.WallHitSound + 1;
            cbWeaponModel1.SelectedIndex = weapon.ModelNum + 1;
            cbWeaponModel2.SelectedIndex = weapon.ModelNumInner + 1;
            cbWeaponWallHit.SelectedIndex = weapon.WallHitVClip + 1;
            cbWeaponRobotHit.SelectedIndex = weapon.RobotHitVClip + 1;
            cbWeaponMuzzleFlash.SelectedIndex = weapon.MuzzleFlashVClip + 1;
            cbWeaponVClip.SelectedIndex = weapon.WeaponVClip + 1;

            UpdateWeaponGraphicControls();
            UpdateWeaponPower();
            isLocked = false;
        }

        private void UpdateWeaponGraphicControls()
        {
            cbWeaponModel1.Visible = cbWeaponModel2.Visible = cbWeaponVClip.Visible = txtWeaponStaticSprite.Visible = txtWeaponProjectileSize.Visible = false;
            lbSprite.Visible = lbModelNum.Visible = lbModelNumInner.Visible = btnRemapWeaponSprite.Visible = SpriteSizeLabel.Visible = false;
            if (cbWeaponRenderMode.SelectedIndex == 0 || cbWeaponRenderMode.SelectedIndex == 1 || cbWeaponRenderMode.SelectedIndex == 4)
            {
                lbSprite.Visible = txtWeaponStaticSprite.Visible = btnRemapWeaponSprite.Visible = txtWeaponProjectileSize.Visible = SpriteSizeLabel.Visible = true;
            }
            else if (cbWeaponRenderMode.SelectedIndex == 3)
            {
                lbSprite.Visible = cbWeaponVClip.Visible = txtWeaponProjectileSize.Visible = SpriteSizeLabel.Visible = true;
            }
            else if (cbWeaponRenderMode.SelectedIndex == 2)
            {
                lbModelNum.Visible = lbModelNumInner.Visible = cbWeaponModel1.Visible = cbWeaponModel2.Visible = true;
            }
        }

        private void UpdateWeaponPower()
        {
            for (int num = 0; num < 5; num++)
            {
                DmgBoxes[num].Text = weapon.Strength[num].ToString();
                SpeedBoxes[num].Text = weapon.Speed[num].ToString();
            }
        }

        //---------------------------------------------------------------------
        // WEAPON UPDATORS
        //---------------------------------------------------------------------

        private void WeaponProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
            {
                return;
            }
            TextBox textBox = (TextBox)sender;

            int value = int.MinValue;
            if (int.TryParse(textBox.Text, out value))
            {
                IntegerTransaction transaction = new IntegerTransaction("Weapon property", weapon, (string)textBox.Tag, weaponID, tabPage, value);
                transactionManager.ApplyTransaction(transaction);
            }
        }

        private void WeaponFixedProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
            {
                return;
            }
            TextBox textBox = (TextBox)sender;

            float fvalue = 0.0f;
            if (float.TryParse(textBox.Text, out fvalue))
            {
                FixTransaction transaction = new FixTransaction("Weapon property", weapon, (string)textBox.Tag, weaponID, tabPage, fvalue);
                transactionManager.ApplyTransaction(transaction);
            }
        }

        private void WeaponCheckBox_CheckChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            CheckBox checkBox = (CheckBox)sender;

            BoolTransaction transaction = new BoolTransaction("Weapon property", weapon, (string)checkBox.Tag, weaponID, tabPage, checkBox.Checked);
            transactionManager.ApplyTransaction(transaction);
        }

        private void WeaponRenderMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            int weapontype = cbWeaponRenderMode.SelectedIndex;
            if (weapontype > 3)
            {
                weapontype = 255;
            }

            IntegerTransaction transaction = new IntegerTransaction("Weapon render type", weapon, (string)cbWeaponRenderMode.Tag, weaponID, tabPage, weapontype);
            transactionManager.ApplyTransaction(transaction);
            UpdateWeaponGraphicControls();
        }

        private void WeaponComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            ComboBox comboBox = (ComboBox)sender;

            IntegerTransaction transaction = new IntegerTransaction("Weapon property", weapon, (string)comboBox.Tag, weaponID, tabPage, comboBox.SelectedIndex-1);
            transactionManager.ApplyTransaction(transaction);
        }

        private void WeaponCalculateLW_Click(object sender, EventArgs e)
        {
            if (weapon.RenderType == WeaponRenderType.Object)
            {
                /*Polymodel model = datafile.PolygonModels[weapon.model_num];
                double width = Math.Abs(model.mins.x / 65536.0d) + Math.Abs(model.maxs.x / 65536.0d);
                double len = Math.Abs(model.mins.z / 65536.0d) + Math.Abs(model.maxs.z / 65536.0d);
                double ratio = len / width;
                weapon.po_len_to_width_ratio = (int)(ratio * 65536.0d);
                isLocked = true;
                txtWeaponPolyLWRatio.Text = ratio.ToString();
                isLocked = false;*/
                //We're gonna need to wait for the new Element Manager to fix this, I think...
                MessageBox.Show("STUB: uh... this is gonna need some rethinking. sorry guys");
            }
        }

        private void RemapSingleImage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ImageSelector selector = new ImageSelector(piggyFile, palette, false);
            IntegerTransaction transaction;
            if (selector.ShowDialog() == DialogResult.OK)
            {
                isLocked = true;
                int value = selector.Selection;
                switch (button.Tag)
                {
                    case "1":
                        txtWeaponCockpitImage.Text = (value).ToString();
                        transaction = new IntegerTransaction("Weapon property", weapon, "CockpitPicture", weaponID, tabPage, value);
                        transactionManager.ApplyTransaction(transaction);
                        break;
                    case "2":
                        txtWeaponCockpitImageh.Text = (value).ToString();
                        transaction = new IntegerTransaction("Weapon property", weapon, "HiresCockpitPicture", weaponID, tabPage, value);
                        transactionManager.ApplyTransaction(transaction);
                        break;
                    case "3":
                        txtWeaponStaticSprite.Text = (value).ToString();
                        transaction = new IntegerTransaction("Weapon property", weapon, "Bitmap", weaponID, tabPage, value);
                        transactionManager.ApplyTransaction(transaction);
                        break;
                }
                isLocked = false;
            }
        }

        private void WeaponComboBoxNormal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            ComboBox comboBox = (ComboBox)sender;

            IntegerTransaction transaction = new IntegerTransaction("Weapon property", weapon, (string)comboBox.Tag, weaponID, tabPage, comboBox.SelectedIndex);
            transactionManager.ApplyTransaction(transaction);
        }

        private void SkillProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;

            TextBox sendingControl = (TextBox)sender;
            string[] tagstrs = ((string)sendingControl.Tag).Split(',');
            string field = tagstrs[0];
            uint index = uint.Parse(tagstrs[1]);

            float value;
            if (float.TryParse(sendingControl.Text, out value))
            {
                IndexedFixTransaction transaction = new IndexedFixTransaction("Robot AI property", weapon, field, weaponID, tabPage, index, value);
                transactionManager.ApplyTransaction(transaction);
            }
        }
    }
}
