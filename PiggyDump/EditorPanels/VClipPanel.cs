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
    public partial class VClipPanel : UserControl
    {
        private VClip clip;
        private PIGFile piggyFile;
        private Palette palette;
        private bool isLocked = false;
        public VClipPanel()
        {
            InitializeComponent();
        }

        public void Init(List<string> SoundNames)
        {
            cbVClipSound.Items.Clear(); cbVClipSound.Items.Add("None");
            cbVClipSound.Items.AddRange(SoundNames.ToArray());
        }

        private void UpdateAnimationFrame(int frame)
        {
            txtAnimFrameNum.Text = clip.Frames[frame].ToString();

            if (pbAnimFramePreview.Image != null)
            {
                Bitmap temp = (Bitmap)pbAnimFramePreview.Image;
                pbAnimFramePreview.Image = null;
                temp.Dispose();
            }
            pbAnimFramePreview.Image = PiggyBitmapUtilities.GetBitmap(piggyFile, palette, clip.Frames[frame]);
        }

        public void Update(VClip clip, PIGFile piggyFile, Palette palette)
        {
            isLocked = true;
            this.clip = clip;
            this.piggyFile = piggyFile;
            this.palette = palette;
            txtAnimFrameSpeed.Text = clip.FrameTime.ToString();
            txtAnimTotalTime.Text = clip.PlayTime.ToString();
            txtAnimLight.Text = clip.LightValue.ToString();
            cbVClipSound.SelectedIndex = clip.SoundNum + 1;
            txtAnimFrameCount.Text = clip.NumFrames.ToString();
            VClipRodFlag.Checked = clip.DrawAsRod;

            nudAnimFrame.Value = 0;
            UpdateAnimationFrame(0);
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
                UpdateAnimationFrame((int)nudAnimFrame.Value);
                isLocked = false;
            }
        }

        private void VClipFixedProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            TextBox textBox = (TextBox)sender;
            double value;
            if (double.TryParse(textBox.Text, out value))
            {
                switch (textBox.Tag)
                {
                    case "1":
                        clip.PlayTime = value;
                        clip.FrameTime = clip.PlayTime / clip.NumFrames;
                        txtAnimFrameSpeed.Text = clip.FrameTime.ToString();
                        break;
                    case "2":
                        clip.LightValue = value;
                        break;
                }
            }
        }

        private void VClipSound_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            ComboBox comboBox = (ComboBox)sender;
            clip.SoundNum = (short)(comboBox.SelectedIndex - 1);
        }

        private void VClipRodFlag_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            clip.DrawAsRod = VClipRodFlag.Checked;
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
                txtAnimFrameCount.Text = clip.NumFrames.ToString();
                txtAnimFrameSpeed.Text = clip.FrameTime.ToString();
                nudAnimFrame.Value = 0;
                UpdateAnimationFrame(0);
                isLocked = false;
            }
        }

        private void VClipProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            TextBox textBox = (TextBox)sender;
            int value;
            if (int.TryParse(textBox.Text, out value))
            {
                switch (textBox.Tag)
                {
                    case "1":
                        clip.NumFrames = value;
                        break;
                    case "2":
                        clip.Frames[(int)nudAnimFrame.Value] = (ushort)value;
                        UpdateAnimationFrame((int)nudAnimFrame.Value);
                        break;
                }
            }
        }

        private void PlayCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (PlayCheckbox.Checked)
            {
                txtAnimTotalTime.Enabled = txtAnimFrameCount.Enabled = txtAnimFrameNum.Enabled = false;
                RemapAnimationButton.Enabled = nudAnimFrame.Enabled = false;
                if (clip.NumFrames < 0) return;
                //Ah, the horribly imprecise timer. Oh well
                AnimTimer.Interval = (int)(1000.0 * clip.FrameTime);
                if (AnimTimer.Interval < 10) AnimTimer.Interval = 10;
                AnimTimer.Start();
            }
            else
            {
                txtAnimTotalTime.Enabled = txtAnimFrameCount.Enabled = txtAnimFrameNum.Enabled = true;
                RemapAnimationButton.Enabled = nudAnimFrame.Enabled = true;
                AnimTimer.Stop();
            }
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            if (clip.NumFrames < 0) return;
            int currentFrame = (int)nudAnimFrame.Value;
            isLocked = true;
            UpdateAnimationFrame(currentFrame);
            currentFrame++;
            if (currentFrame >= clip.NumFrames)
                currentFrame = 0;
            nudAnimFrame.Value = currentFrame;
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
                clip.Frames[(int)nudAnimFrame.Value] = (ushort)value;
                UpdateAnimationFrame((int)nudAnimFrame.Value);
                txtAnimFrameNum.Text = value.ToString();
                isLocked = false;
            }
            selector.Dispose();
        }
    }
}
