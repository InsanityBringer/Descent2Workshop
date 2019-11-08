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
    public partial class TMAPInfoPanel : UserControl
    {
        //TMAPInfos can only be in HAM files, so it's safe to have this
        private HAMFile datafile;
        private int textureID; //Needed to look up and set
        private TMAPInfo info;
        private bool isLocked = false;
        public TMAPInfoPanel()
        {
            InitializeComponent();
        }

        public void Init(List<string> EClipNames)
        {
            cbTexEClip.Items.Clear(); cbTexEClip.Items.Add("None");
            cbTexEClip.Items.AddRange(EClipNames.ToArray());
        }

        public double GetFloatFromFixed88(short fixedvalue)
        {
            return (double)fixedvalue / 256D;
        }

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

        public void Update(HAMFile datafile, int textureID, TMAPInfo info)
        {
            isLocked = true;
            this.textureID = textureID;
            this.datafile = datafile;
            this.info = info;

            txtTexID.Text = datafile.Textures[textureID].ToString();
            txtTexLight.Text = info.lighting.ToString();
            txtTexDamage.Text = info.damage.ToString();
            cbTexEClip.SelectedIndex = info.eclip_num + 1;
            txtTexSlideU.Text = GetFloatFromFixed88(info.slide_u).ToString();
            txtTexSlideV.Text = GetFloatFromFixed88(info.slide_v).ToString();
            txtTexDestroyed.Text = info.destroyed.ToString();
            cbTexLava.Checked = ((info.flags & TMAPInfo.TMI_VOLATILE) != 0);
            cbTexWater.Checked = ((info.flags & TMAPInfo.TMI_WATER) != 0);
            cbTexForcefield.Checked = ((info.flags & TMAPInfo.TMI_FORCE_FIELD) != 0);
            cbTexRedGoal.Checked = ((info.flags & TMAPInfo.TMI_GOAL_RED) != 0);
            cbTexBlueGoal.Checked = ((info.flags & TMAPInfo.TMI_GOAL_BLUE) != 0);
            cbTexHoardGoal.Checked = ((info.flags & TMAPInfo.TMI_GOAL_HOARD) != 0);

            UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, datafile.Textures[textureID]), pbTexPrev);
            isLocked = false;
        }

        private void TextureFlagCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            TMAPInfo tmapinfo = datafile.TMapInfo[textureID];

            CheckBox check = (CheckBox)sender;
            int flagvalue = int.Parse(check.Tag.ToString());
            tmapinfo.updateFlags(flagvalue, check.Checked);
        }

        private void TextureProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            TextBox textBox = (TextBox)sender;
            TMAPInfo info = datafile.TMapInfo[textureID];
            try
            {
                //double value = double.Parse(textBox.Text);
                switch (textBox.Tag)
                {
                    case "0":
                        datafile.Textures[textureID] = ushort.Parse(textBox.Text);
                        UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, datafile.Textures[textureID]), pbTexPrev);
                        break;
                    case "1":
                        info.lighting = double.Parse(textBox.Text);
                        break;
                    case "2":
                        info.damage = double.Parse(textBox.Text);
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
                //[ISB] heh
            }
        }

        private void TMAPInfoEClip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            int eclipNum = cbTexEClip.SelectedIndex - 1;
            EClip clip = datafile.GetEClip(eclipNum);
            TMAPInfo tmapInfo = datafile.TMapInfo[textureID];
            if (clip == null)
            {
                tmapInfo.eclip_num = -1;
            }
            else
            {
                int clipCurrentID = clip.GetCurrentTMap();
                if (clipCurrentID != -1 && clipCurrentID != textureID)
                {
                    if (MessageBox.Show("This EClip is already assigned to another wall texture, do you want to change it?", "EClip in use", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        TMAPInfo oldTMapInfo = datafile.TMapInfo[clipCurrentID];
                        oldTMapInfo.eclip_num = -1;
                        clip.changing_wall_texture = (short)textureID;

                    }
                    else
                    {
                        cbTexEClip.SelectedIndex = tmapInfo.eclip_num + 1;
                        return;
                    }
                }
                //this was commented out and I have no idea if there was a good reason for it
                else
                {
                    clip.changing_wall_texture = (short)textureID;
                }
            }
            tmapInfo.eclip_num = (short)eclipNum;
        }

        private void RemapSingleImage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ImageSelector selector = new ImageSelector(datafile.piggyFile, false);
            if (selector.ShowDialog() == DialogResult.OK)
            {
                isLocked = true;
                int value = selector.Selection;
                datafile.Textures[textureID] = (ushort)value;
                UpdatePictureBox(PiggyBitmapConverter.GetBitmap(datafile.piggyFile, value), pbTexPrev);
                txtTexID.Text = value.ToString();
                isLocked = false;
            }
        }
    }
}
