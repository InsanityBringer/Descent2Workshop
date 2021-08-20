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
using LibDescent.Edit;

namespace Descent2Workshop.EditorPanels
{
    public partial class TMAPInfoPanel : UserControl
    {
        //TMAPInfos can only be in HAM files, so it's safe to have this
        private EditorHAMFile datafile;
        private PIGFile piggyFile;
        private Palette palette;
        private int textureID; //Needed to look up and set
        private TMAPInfo info;
        private bool isLocked = false;

        private TransactionManager transactionManager;

        public TMAPInfoPanel(TransactionManager transactionManager)
        {
            InitializeComponent();
            this.transactionManager = transactionManager;
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

        public void Update(EditorHAMFile datafile, PIGFile piggyFile, int textureID, TMAPInfo info)
        {
            isLocked = true;
            this.textureID = textureID;
            this.datafile = datafile;
            this.piggyFile = piggyFile;
            this.info = info;

            TextureIDTextBox.Text = datafile.Textures[textureID].ToString();
            txtTexLight.Text = info.Lighting.ToString();
            txtTexDamage.Text = info.Damage.ToString();
            UIUtil.SafeFillComboBox(cbTexEClip, info.EClipNum + 1);
            txtTexSlideU.Text = GetFloatFromFixed88(info.SlideU).ToString();
            txtTexSlideV.Text = GetFloatFromFixed88(info.SlideV).ToString();
            TextureDestroyedTextBox.Text = info.DestroyedID.ToString();
            cbTexLava.Checked = info.Volatile;
            cbTexWater.Checked = info.Water;
            cbTexForcefield.Checked = info.ForceField;
            cbTexRedGoal.Checked = info.RedGoal;
            cbTexBlueGoal.Checked = info.BlueGoal;
            cbTexHoardGoal.Checked = info.HoardGoal;

            UpdatePictureBox(PiggyBitmapUtilities.GetBitmap(piggyFile, palette, datafile.Textures[textureID]), pbTexPrev);
            isLocked = false;
        }

        private void TextureFlagCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            TMAPInfo tmapinfo = datafile.TMapInfo[textureID];
            CheckBox control = (CheckBox)sender;
            BoolTransaction transaction = new BoolTransaction("TMapInfo property", tmapinfo, (string)control.Tag, textureID, 0, control.Checked);
            transactionManager.ApplyTransaction(transaction);
        }

        private void TextureProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            TextBox textBox = (TextBox)sender;
            TMAPInfo info = datafile.TMapInfo[textureID];
            double value;
            if (double.TryParse(textBox.Text, out value))
            {
                FixTransaction transaction = new FixTransaction("TMapInfo property", info, (string)textBox.Tag, textureID, 0, value);
                transactionManager.ApplyTransaction(transaction);
            }
        }

        private void TMAPInfoEClip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress) return;

            isLocked = true;
            int eclipNum = cbTexEClip.SelectedIndex - 1;
            EClip clip = datafile.GetEClip(eclipNum);
            TMAPInfo tmapInfo = datafile.TMapInfo[textureID];
            TMapInfoEClipTransaction transaction = null;
            if (clip == null)
            {
                transaction = new TMapInfoEClipTransaction("TMapInfo EClip change", textureID, 0, datafile, piggyFile, textureID, -1); 
            }
            else
            {
                int clipCurrentID = clip.ChangingWallTexture;
                if (clipCurrentID != -1 && clipCurrentID != textureID)
                {
                    if (MessageBox.Show("This EClip is already assigned to another wall texture, do you want to change it?", "EClip in use", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        transaction = new TMapInfoEClipTransaction("TMapInfo EClip change", textureID, 0, datafile, piggyFile, textureID, eclipNum);

                    }
                    else
                    {
                        cbTexEClip.SelectedIndex = tmapInfo.EClipNum + 1;
                        isLocked = false;
                        return;
                    }
                }
                //this was commented out and I have no idea if there was a good reason for it
                else
                {
                    transaction = new TMapInfoEClipTransaction("TMapInfo EClip change", textureID, 0, datafile, piggyFile, textureID, eclipNum);
                }
            }
            if (transaction != null)
                transactionManager.ApplyTransaction(transaction);

            isLocked = false;
        }

        private void RemapSingleImage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ImageSelector selector = new ImageSelector(piggyFile, palette, false);
            if (selector.ShowDialog() == DialogResult.OK)
            {
                isLocked = true;
                int value = selector.Selection;
                TMAPInfo tmapinfo = datafile.TMapInfo[textureID];
                TMapInfoTransaction transaction = new TMapInfoTransaction("TMapInfo texture change", textureID, 0, datafile, piggyFile, textureID, value);
                transaction.eventHandler += TextureEventHandler;
                transactionManager.ApplyTransaction(transaction);
                isLocked = false;
            }
        }

        private void TextureIntegerProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            int value;
            TextBox control = (TextBox)sender;
            if (int.TryParse(control.Text, out value))
            {
                TMAPInfo tmapinfo = datafile.TMapInfo[textureID];
                IntegerTransaction transaction = new IntegerTransaction("TMapInfo property", tmapinfo, (string)control.Tag, textureID, 0, value);
                transactionManager.ApplyTransaction(transaction);
            }
        }

        private void TextureIDTextBox_TextChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            int value;
            TextBox control = (TextBox)sender;
            if (int.TryParse(control.Text, out value))
            {
                TMAPInfo tmapinfo = datafile.TMapInfo[textureID];
                TMapInfoTransaction transaction = new TMapInfoTransaction("TMapInfo texture change", textureID, 0, datafile, piggyFile, textureID, value);
                transaction.eventHandler += TextureEventHandler;
                transactionManager.ApplyTransaction(transaction);
                
            }
        }

        private void TextureEventHandler(object sender, TmapInfoEventArgs e)
        {
            UpdatePictureBox(PiggyBitmapUtilities.GetBitmap(piggyFile, palette, e.Value), pbTexPrev);
            //mid transaction, so this is safe. Probably.
            //Make sure the value is clamped in the textbox
            TextureIDTextBox.Text = e.Value.ToString();
        }

        private void txtTexSlideU_TextChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            double value;
            TextBox control = (TextBox)sender;
            if (double.TryParse(control.Text, out value))
            {
                TMAPInfo tmapinfo = datafile.TMapInfo[textureID];
                IntegerTransaction transaction = new IntegerTransaction("TMapInfo property", tmapinfo, (string)control.Tag, textureID, 0, (int)(value * 256));
                transactionManager.ApplyTransaction(transaction);
            }
        }
    }
}
