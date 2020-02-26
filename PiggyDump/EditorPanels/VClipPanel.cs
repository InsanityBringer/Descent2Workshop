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
            txtAnimFrameNum.Text = clip.frames[frame].ToString();

            if (pbAnimFramePreview.Image != null)
            {
                Bitmap temp = (Bitmap)pbAnimFramePreview.Image;
                pbAnimFramePreview.Image = null;
                temp.Dispose();
            }
            pbAnimFramePreview.Image = PiggyBitmapConverter.GetBitmap(piggyFile, palette, clip.frames[frame]);
        }

        public void Update(VClip clip, PIGFile piggyFile, Palette palette)
        {
            isLocked = true;
            this.clip = clip;
            this.piggyFile = piggyFile;
            this.palette = palette;
            txtAnimFrameSpeed.Text = clip.frame_time.ToString();
            txtAnimTotalTime.Text = clip.play_time.ToString();
            txtAnimLight.Text = clip.light_value.ToString();
            cbVClipSound.SelectedIndex = clip.sound_num + 1;
            txtAnimFrameCount.Text = clip.num_frames.ToString();
            VClipRodFlag.Checked = ((clip.flags & 1) == 1);

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
                        int totalTimeFix = (int)(value * 65536);
                        clip.play_time = Fix.FromRawValue(totalTimeFix);
                        clip.frame_time = Fix.FromRawValue(totalTimeFix / clip.num_frames);
                        txtAnimFrameSpeed.Text = clip.frame_time.ToString();
                        break;
                    case "2":
                        clip.light_value = Fix.FromRawValue((int)(value * 65536));
                        break;
                }
            }
        }

        private void VClipSound_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            ComboBox comboBox = (ComboBox)sender;
            clip.sound_num = (short)(comboBox.SelectedIndex - 1);
        }

        private void VClipRodFlag_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            clip.flags = VClipRodFlag.Checked ? 1 : 0;
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
                txtAnimFrameCount.Text = clip.num_frames.ToString();
                txtAnimFrameSpeed.Text = clip.frame_time.ToString();
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
                        clip.num_frames = value;
                        break;
                    case "2":
                        clip.frames[(int)nudAnimFrame.Value] = (ushort)value;
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
                if (clip.num_frames < 0) return;
                //Ah, the horribly imprecise timer. Oh well
                AnimTimer.Interval = (int)(1000.0 * clip.frame_time);
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
            if (clip.num_frames < 0) return;
            int currentFrame = (int)nudAnimFrame.Value;
            isLocked = true;
            UpdateAnimationFrame(currentFrame);
            currentFrame++;
            if (currentFrame >= clip.num_frames)
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
                clip.frames[(int)nudAnimFrame.Value] = (ushort)value;
                UpdateAnimationFrame((int)nudAnimFrame.Value);
                txtAnimFrameNum.Text = value.ToString();
                isLocked = false;
            }
            selector.Dispose();
        }
    }
}
