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
    public partial class WClipPanel : UserControl
    {
        private WClip clip;
        private int wclipID;
        private EditorHAMFile hamFile;
        private PIGFile piggyFile;
        private Palette palette;
        private bool isLocked = false;
        private TransactionManager transactionManager;
        public WClipPanel(TransactionManager transactionManager)
        {
            InitializeComponent();
            this.transactionManager = transactionManager;
        }

        public void Init(List<string> names)
        {
            string[] namelist = names.ToArray();
            OpenSoundComboBox.Items.Clear();
            CloseSoundComboBox.Items.Clear();

            OpenSoundComboBox.Items.Add("None");
            CloseSoundComboBox.Items.Add("None");
            OpenSoundComboBox.Items.AddRange(namelist);
            CloseSoundComboBox.Items.AddRange(namelist);
        }

        public void Update(int number, WClip clip, EditorHAMFile hamFile, PIGFile piggyFile, Palette palette)
        {
            isLocked = true;
            wclipID = number;
            this.clip = clip;
            this.hamFile = hamFile;
            this.palette = palette;
            TotalTimeTextBox.Text = clip.PlayTime.ToString();
            OpenSoundComboBox.SelectedIndex = clip.OpenSound + 1;
            CloseSoundComboBox.SelectedIndex = clip.CloseSound + 1;
            FilenameTextBox.Text = new string(clip.Filename);
            NumFramesTextBox.Text = clip.NumFrames.ToString();

            ExplodesCheckBox.Checked = clip.Explodes;
            ShootableCheckBox.Checked = clip.Blastable;
            OnPrimaryTMapCheckBox.Checked = clip.PrimaryTMap;
            HiddenCheckBox.Checked = clip.SecretDoor;

            if (!transactionManager.TransactionInProgress)
            {
                FrameSpinner.Value = 0;
                UpdateWallFrame(0);
            }
            else
                UpdateWallFrame((int)FrameSpinner.Value);
            isLocked = false;
        }

        private void UpdateWallFrame(int frame)
        {
            FrameTextBox.Text = clip.Frames[frame].ToString();

            if (FramePictureBox.Image != null)
            {
                Bitmap temp = (Bitmap)FramePictureBox.Image;
                FramePictureBox.Image = null;
                temp.Dispose();
            }
            FramePictureBox.Image = PiggyBitmapUtilities.GetBitmap(hamFile.piggyFile, palette, hamFile.Textures[clip.Frames[frame]]);
        }

        private void FrameSpinner_ValueChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            UpdateWallFrame((int)FrameSpinner.Value);
        }

        private void FlagCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress) return;
            CheckBox control = (CheckBox)sender;
            BoolTransaction transaction = new BoolTransaction("Door flag", clip, (string)control.Tag, wclipID, 3, control.Checked);
            transactionManager.ApplyTransaction(transaction);
        }

        private void IndexedUndoEvent(object sender, UndoIndexedEventArgs e)
        {
            FrameSpinner.Value = e.Index;
        }

        private void FixPropertyTextBox_TextChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            TextBox textBox = (TextBox)sender;
            double value;
            if (double.TryParse(textBox.Text, out value))
            {
                FixTransaction transaction = new FixTransaction("WClip property", clip, (string)textBox.Tag, wclipID, 3, value);
                transactionManager.ApplyTransaction(transaction);
            }
        }

        private void SoundComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            ComboBox control = (ComboBox)sender;
            IntegerTransaction transaction = new IntegerTransaction("WClip property", clip, (string)control.Tag, wclipID, 3, control.SelectedIndex);
            transactionManager.ApplyTransaction(transaction);
        }

        private void NumFramesTextBox_TextChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            TextBox textBox = (TextBox)sender;
            int value;
            if (int.TryParse(textBox.Text, out value))
            {
                IntegerTransaction transaction = new IntegerTransaction("WClip property", clip, (string)textBox.Tag, wclipID, 3, value);
                transactionManager.ApplyTransaction(transaction);
            }
        }

        private void FrameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            TextBox textBox = (TextBox)sender;
            uint value;
            if (uint.TryParse(textBox.Text, out value))
            {
                if (value < 0) value = 0;
                if (value >= hamFile.Textures.Count) value = 0;
                IndexedUnsignedTransaction transaction = new IndexedUnsignedTransaction("WClip image", clip, "Frames", wclipID, 3, (uint)FrameSpinner.Value, value);
                transaction.undoEvent += IndexedUndoEvent;
                transactionManager.ApplyTransaction(transaction);
                UpdateWallFrame((int)FrameSpinner.Value);
            }
        }
    }
}
