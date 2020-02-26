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

namespace Descent2Workshop.EditorPanels
{
    public partial class WeaponPanel : UserControl
    {
        private Weapon weapon;
        //Needed to remap graphics
        private PIGFile piggyFile; //aaa
        private Palette palette; //aaaaaaaaaaaaaaaaaaaa
        private bool isLocked = false;

        private TextBox[] SpeedBoxes = new TextBox[5];
        private TextBox[] DmgBoxes = new TextBox[5];
        public WeaponPanel()
        {
            InitializeComponent();
            SpeedBoxes[0] = txtWeaponSpeed; SpeedBoxes[1] = Speed1; SpeedBoxes[2] = Speed2; SpeedBoxes[3] = Speed3; SpeedBoxes[4] = Speed4;
            DmgBoxes[0] = txtWeaponStr; DmgBoxes[1] = Damage1; DmgBoxes[2] = Damage2; DmgBoxes[3] = Damage3; DmgBoxes[4] = Damage4;
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

        public void Update(Weapon weapon)
        {
            isLocked = true;
            this.weapon = weapon;

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
                DmgBoxes[num].Text = weapon.strength[num].ToString();
                SpeedBoxes[num].Text = weapon.speed[num].ToString();
            }
        }

        //---------------------------------------------------------------------
        // WEAPON UPDATORS
        //---------------------------------------------------------------------

        private void WeaponProperty_TextChanged(object sender, EventArgs e)
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
                weapon.UpdateWeapon(tagvalue, value, 0);
            }
        }

        private void WeaponFixedProperty_TextChanged(object sender, EventArgs e)
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
                weapon.UpdateWeapon(tagvalue, value, 0);
            }
        }

        private void WeaponCheckBox_CheckChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
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

        private void WeaponRenderMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
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
            weapon.UpdateWeapon(int.Parse((string)comboBox.Tag), comboBox.SelectedIndex, 0);
        }

        private void WeaponCalculateLW_Click(object sender, EventArgs e)
        {
            if (weapon.render_type == 2)
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
            if (selector.ShowDialog() == DialogResult.OK)
            {
                isLocked = true;
                int value = selector.Selection;
                switch (button.Tag)
                {
                    case "1":
                        txtWeaponCockpitImage.Text = (value).ToString();
                        weapon.picture = (ushort)value;
                        break;
                    case "2":
                        txtWeaponCockpitImageh.Text = (value).ToString();
                        weapon.hires_picture = (ushort)value;
                        break;
                    case "3":
                        weapon.bitmap = (ushort)(value);
                        txtWeaponStaticSprite.Text = (value).ToString();
                        break;
                }
                isLocked = false;
            }
        }

        private void WeaponFixedProperty1_TextChanged(object sender, EventArgs e)
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
                weapon.UpdateWeapon(tagvalue, value, 1);
            }
        }

        private void WeaponFixedProperty2_TextChanged(object sender, EventArgs e)
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
                weapon.UpdateWeapon(tagvalue, value, 2);
            }
        }

        private void WeaponFixedProperty3_TextChanged(object sender, EventArgs e)
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
                weapon.UpdateWeapon(tagvalue, value, 3);
            }
        }

        private void WeaponFixedProperty4_TextChanged(object sender, EventArgs e)
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
                weapon.UpdateWeapon(tagvalue, value, 4);
            }
        }
    }
}
