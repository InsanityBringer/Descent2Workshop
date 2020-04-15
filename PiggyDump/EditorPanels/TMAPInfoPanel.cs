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
        private PIGFile piggyFile;
        private Palette palette;
        private int textureID; //Needed to look up and set
        private TMAPInfo info;
        private bool isLocked = false;
        public TMAPInfoPanel()
        {
            InitializeComponent();
        }

        public void Init(List<string> EClipNames, Palette palette)
        {
            cbTexEClip.Items.Clear(); cbTexEClip.Items.Add("None");
            cbTexEClip.Items.AddRange(EClipNames.ToArray());
            this.palette = palette;
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

        public void Update(HAMFile datafile, PIGFile piggyFile, int textureID, TMAPInfo info)
        {
            isLocked = true;
            this.textureID = textureID;
            this.datafile = datafile;
            this.piggyFile = piggyFile;
            this.info = info;

            txtTexID.Text = datafile.Textures[textureID].ToString();
            txtTexLight.Text = info.Lighting.ToString();
            txtTexDamage.Text = info.Damage.ToString();
            cbTexEClip.SelectedIndex = info.EClipNum + 1;
            txtTexSlideU.Text = GetFloatFromFixed88(info.SlideU).ToString();
            txtTexSlideV.Text = GetFloatFromFixed88(info.SlideV).ToString();
            txtTexDestroyed.Text = info.DestroyedID.ToString();
            cbTexLava.Checked = info.Volatile;
            cbTexWater.Checked = info.Water;
            cbTexForcefield.Checked = info.ForceField;
            cbTexRedGoal.Checked = info.RedGoal;
            cbTexBlueGoal.Checked = info.BlueGoal;
            cbTexHoardGoal.Checked = info.HoardGoal;

            UpdatePictureBox(PiggyBitmapConverter.GetBitmap(piggyFile, palette, datafile.Textures[textureID]), pbTexPrev);
            isLocked = false;
        }

        private void TextureFlagCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            TMAPInfo tmapinfo = datafile.TMapInfo[textureID];
            tmapinfo.Volatile = cbTexLava.Checked;
            tmapinfo.Water = cbTexWater.Checked;
            tmapinfo.ForceField = cbTexForcefield.Checked;
            tmapinfo.RedGoal = cbTexRedGoal.Checked;
            tmapinfo.BlueGoal = cbTexBlueGoal.Checked;
            tmapinfo.HoardGoal = cbTexHoardGoal.Checked;
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
                        UpdatePictureBox(PiggyBitmapConverter.GetBitmap(piggyFile, palette, datafile.Textures[textureID]), pbTexPrev);
                        break;
                    case "1":
                        info.Lighting = double.Parse(textBox.Text);
                        break;
                    case "2":
                        info.Damage = double.Parse(textBox.Text);
                        break;
                    case "4":
                        info.SlideU = (short)(double.Parse(textBox.Text) * 256.0d);
                        break;
                    case "5":
                        info.SlideV = (short)(double.Parse(textBox.Text) * 256.0d);
                        break;
                    case "6":
                        info.DestroyedID = short.Parse(textBox.Text);
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
                tmapInfo.EClipNum = -1;
            }
            else
            {
                int clipCurrentID = clip.ChangingWallTexture;
                if (clipCurrentID != -1 && clipCurrentID != textureID)
                {
                    if (MessageBox.Show("This EClip is already assigned to another wall texture, do you want to change it?", "EClip in use", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        TMAPInfo oldTMapInfo = datafile.TMapInfo[clipCurrentID];
                        oldTMapInfo.EClipNum = -1;
                        clip.ChangingWallTexture = (short)textureID;

                    }
                    else
                    {
                        cbTexEClip.SelectedIndex = tmapInfo.EClipNum + 1;
                        return;
                    }
                }
                //this was commented out and I have no idea if there was a good reason for it
                else
                {
                    clip.ChangingWallTexture = (short)textureID;
                }
            }
            tmapInfo.EClipNum = (short)eclipNum;
        }

        private void RemapSingleImage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ImageSelector selector = new ImageSelector(piggyFile, palette, false);
            if (selector.ShowDialog() == DialogResult.OK)
            {
                isLocked = true;
                int value = selector.Selection;
                datafile.Textures[textureID] = (ushort)value;
                UpdatePictureBox(PiggyBitmapConverter.GetBitmap(piggyFile, palette, value), pbTexPrev);
                txtTexID.Text = value.ToString();
                isLocked = false;
            }
        }
    }
}
