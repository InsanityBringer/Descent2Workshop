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
    public partial class VClipPanel : UserControl
    {
        private VClip clip;
        private int vclipID;
        private PIGFile piggyFile;
        private Palette palette;
        private bool isLocked = false;
        private TransactionManager transactionManager;
        public VClipPanel(TransactionManager transactionManager)
        {
            InitializeComponent();
            this.transactionManager = transactionManager;
        }

        public void Init(List<string> SoundNames)
        {
            SoundComboBox.Items.Clear(); SoundComboBox.Items.Add("None");
            SoundComboBox.Items.AddRange(SoundNames.ToArray());
        }

        private void UpdateAnimationFrame(int frame)
        {
            FrameNumTextBox.Text = clip.Frames[frame].ToString();

            if (pbAnimFramePreview.Image != null)
            {
                Bitmap temp = (Bitmap)pbAnimFramePreview.Image;
                pbAnimFramePreview.Image = null;
                temp.Dispose();
            }
            pbAnimFramePreview.Image = PiggyBitmapUtilities.GetBitmap(piggyFile, palette, clip.Frames[frame]);
        }

        public void Update(int number, VClip clip, PIGFile piggyFile, Palette palette)
        {
            isLocked = true;
            vclipID = number;
            this.clip = clip;
            this.piggyFile = piggyFile;
            this.palette = palette;
            FrameTimeTextBox.Text = clip.FrameTime.ToString();
            TotalTimeTextBox.Text = clip.PlayTime.ToString();
            LightValueTextBox.Text = clip.LightValue.ToString();
            SoundComboBox.SelectedIndex = clip.SoundNum + 1;
            FrameCountTextBox.Text = clip.NumFrames.ToString();
            VClipRodFlag.Checked = clip.DrawAsRod;

            if (!transactionManager.TransactionInProgress)
            {
                FrameSpinner.Value = 0;
                UpdateAnimationFrame(0);
            }
            else
            {
                UpdateAnimationFrame((int)FrameSpinner.Value);
            }
            isLocked = false;
        }

        public void Stop()
        {
            if (AnimTimer.Enabled)
            {
                PlayCheckbox.Checked = false;
            }
        }

        private void nudAnimFrame_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                UpdateAnimationFrame((int)FrameSpinner.Value);
                isLocked = false;
            }
        }

        private void VClipFixedProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            TextBox textBox = (TextBox)sender;
            double value;
            if (double.TryParse(textBox.Text, out value))
            {
                /*switch (textBox.Tag)
                {
                    case "1":
                        clip.PlayTime = value;
                        clip.FrameTime = clip.PlayTime / clip.NumFrames;
                        FrameTimeTextBox.Text = clip.FrameTime.ToString();
                        break;
                    case "2":
                        clip.LightValue = value;
                        break;
                }*/
                FixTransaction transaction = new FixTransaction("VClip property", clip, (string)textBox.Tag, vclipID, 1, value);
                transactionManager.ApplyTransaction(transaction);
                //hack
                clip.FrameTime = clip.PlayTime / clip.NumFrames;
                FrameTimeTextBox.Text = clip.FrameTime.ToString();
            }
        }

        private void VClipSound_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            ComboBox comboBox = (ComboBox)sender;
            IntegerTransaction transaction = new IntegerTransaction("VClip sound", clip, (string)comboBox.Tag, vclipID, 1, comboBox.SelectedIndex - 1);
            transactionManager.ApplyTransaction(transaction);
        }

        private void VClipRodFlag_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;

            CheckBox checkBox = (CheckBox)sender;
            BoolTransaction transaction = new BoolTransaction("VClip flag", clip, (string)checkBox.Tag, vclipID, 1, checkBox.Checked);
            transactionManager.ApplyTransaction(transaction);
        }

        private void RemapMultiImage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ImageSelector selector = new ImageSelector(piggyFile, palette, true);
            if (selector.ShowDialog() == DialogResult.OK)
            {
                int value = selector.Selection;
                isLocked = true;
                clip.RemapVClip(value, piggyFile);
                FrameCountTextBox.Text = clip.NumFrames.ToString();
                FrameTimeTextBox.Text = clip.FrameTime.ToString();
                FrameSpinner.Value = 0;
                UpdateAnimationFrame(0);
                isLocked = false;
            }
        }

        private void VClipProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            TextBox textBox = (TextBox)sender;
            int value;
            if (int.TryParse(textBox.Text, out value))
            {
                IntegerTransaction transaction = new IntegerTransaction("VClip property", clip, (string)textBox.Tag, vclipID, 1, value);
                transactionManager.ApplyTransaction(transaction);
                /*switch (textBox.Tag)
                {
                    case "1":
                        clip.NumFrames = value;
                        break;
                    case "2":
                        clip.Frames[(int)FrameSpinner.Value] = (ushort)value;
                        UpdateAnimationFrame((int)FrameSpinner.Value);
                        break;
                }*/
            }
        }

        private void PlayCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (PlayCheckbox.Checked)
            {
                TotalTimeTextBox.Enabled = FrameCountTextBox.Enabled = FrameNumTextBox.Enabled = false;
                RemapAnimationButton.Enabled = FrameSpinner.Enabled = false;
                if (clip.NumFrames < 0) return;
                //Ah, the horribly imprecise timer. Oh well
                AnimTimer.Interval = (int)(1000.0 * clip.FrameTime);
                if (AnimTimer.Interval < 10) AnimTimer.Interval = 10;
                AnimTimer.Start();
            }
            else
            {
                TotalTimeTextBox.Enabled = FrameCountTextBox.Enabled = FrameNumTextBox.Enabled = true;
                RemapAnimationButton.Enabled = FrameSpinner.Enabled = true;
                AnimTimer.Stop();
            }
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            if (clip.NumFrames < 0) return;
            int currentFrame = (int)FrameSpinner.Value;
            isLocked = true;
            UpdateAnimationFrame(currentFrame);
            currentFrame++;
            if (currentFrame >= clip.NumFrames)
                currentFrame = 0;
            FrameSpinner.Value = currentFrame;
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
                clip.Frames[(int)FrameSpinner.Value] = (ushort)value;
                UpdateAnimationFrame((int)FrameSpinner.Value);
                FrameNumTextBox.Text = value.ToString();
                isLocked = false;
            }
            selector.Dispose();
        }

        private void FrameNumTextBox_TextChanged(object sender, EventArgs e)
        {
            if (isLocked || transactionManager.TransactionInProgress)
                return;
            TextBox textBox = (TextBox)sender;
            uint value;
            if (uint.TryParse(textBox.Text, out value))
            {
                IndexedUnsignedTransaction transaction = new IndexedUnsignedTransaction("VClip image", clip, "Frames", vclipID, 1, (uint)FrameSpinner.Value, value);
                transaction.undoEvent += IndexedUndoEvent;
                transactionManager.ApplyTransaction(transaction);
                UpdateAnimationFrame((int)FrameSpinner.Value);
            }
        }

        private void IndexedUndoEvent(object sender, UndoIndexedEventArgs e)
        {
            FrameSpinner.Value = e.Index;
        }
    }
}
