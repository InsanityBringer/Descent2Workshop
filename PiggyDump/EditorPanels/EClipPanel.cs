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
    public partial class EClipPanel : UserControl
    {
        private EClip clip;
        private PIGFile piggyFile;
        private bool isLocked = false;
        public EClipPanel()
        {
            InitializeComponent();
        }

        public void Init(List<string> VClipNames, List<string> EClipNames, List<string> SoundNames)
        {
            cbEClipBreakEClip.Items.Clear(); cbEClipBreakEClip.Items.Add("None");
            cbEClipMineCritical.Items.Clear(); cbEClipMineCritical.Items.Add("None");
            cbEClipBreakVClip.Items.Clear(); cbEClipBreakVClip.Items.Add("None");
            cbEClipBreakSound.Items.Clear(); cbEClipBreakSound.Items.Add("None");
            cbEClipBreakEClip.Items.AddRange(EClipNames.ToArray());
            cbEClipMineCritical.Items.AddRange(EClipNames.ToArray());
            cbEClipBreakVClip.Items.AddRange(VClipNames.ToArray());
            cbEClipBreakSound.Items.AddRange(SoundNames.ToArray());
        }

        public void Stop()
        {
            if (AnimTimer.Enabled)
            {
                PlayCheckbox.Checked = false;
            }
        }

        public void Update(EClip clip, PIGFile piggyFile)
        {
            isLocked = true;
            this.clip = clip;
            this.piggyFile = piggyFile;
            //vclip specific data
            txtEffectFrameSpeed.Text = clip.vc.frame_time.ToString();
            txtEffectTotalTime.Text = clip.vc.play_time.ToString();
            txtEffectLight.Text = clip.vc.light_value.ToString();
            txtEffectFrameCount.Text = clip.vc.num_frames.ToString();

            //eclip stuff
            cbEClipBreakEClip.SelectedIndex = clip.dest_eclip + 1;
            cbEClipBreakVClip.SelectedIndex = clip.dest_vclip + 1;
            txtEffectExplodeSize.Text = clip.dest_size.ToString();
            txtEffectBrokenID.Text = clip.dest_bm_num.ToString();
            cbEClipBreakSound.SelectedIndex = clip.sound_num + 1;
            cbEClipMineCritical.SelectedIndex = clip.crit_clip + 1;
            cbEffectCritical.Checked = (clip.flags & 1) != 0;
            cbEffectOneShot.Checked = (clip.flags & 2) != 0;

            nudEffectFrame.Value = 0;
            UpdateEffectFrame(0);

            isLocked = false;
        }

        private void nudEffectFrame_ValueChanged(object sender, EventArgs e)
        {
            if (!isLocked)
            {
                isLocked = true;
                UpdateEffectFrame((int)nudEffectFrame.Value);
                isLocked = false;
            }
        }

        private void UpdateEffectFrame(int frame)
        {
            txtEffectFrameNum.Text = clip.vc.frames[frame].ToString();

            if (pbEffectFramePreview.Image != null)
            {
                Bitmap temp = (Bitmap)pbEffectFramePreview.Image;
                pbEffectFramePreview.Image = null;
                temp.Dispose();
            }
            pbEffectFramePreview.Image = PiggyBitmapConverter.GetBitmap(piggyFile, clip.vc.frames[frame]);
        }

        private void EClipFixedProperty_TextChanged(object sender, EventArgs e)
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
                        clip.vc.play_time = new Fix(totalTimeFix);
                        clip.vc.frame_time = new Fix(totalTimeFix / clip.vc.num_frames);
                        txtEffectFrameSpeed.Text = clip.vc.frame_time.ToString();
                        break;
                    case "2":
                        clip.vc.light_value = new Fix((int)(value * 65536));
                        break;
                    case "3":
                        clip.dest_size = new Fix((int)(value * 65536));
                        break;
                    case "4":
                        clip.dest_bm_num = int.Parse(textBox.Text);
                        break;
                    case "5":
                        clip.vc.num_frames = int.Parse(textBox.Text);
                        break;
                }
            }
        }

        private void EClipProperty_TextChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            TextBox textBox = (TextBox)sender;
            int value;
            if (int.TryParse(textBox.Text, out value))
            {
                switch (textBox.Tag)
                {
                    case "4":
                        clip.dest_bm_num = value;
                        break;
                    case "5":
                        clip.vc.num_frames = value;
                        break;
                    case "6":
                        clip.vc.frames[(int)nudEffectFrame.Value] = (ushort)value;
                        isLocked = true;
                        UpdateEffectFrame((int)nudEffectFrame.Value);
                        isLocked = false;
                        break;
                }
            }
        }

        private void EClipComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLocked)
                return;
            ComboBox comboBox = (ComboBox)sender;
            int value = comboBox.SelectedIndex;
            switch (comboBox.Tag)
            {
                case "1":
                    clip.dest_eclip = value - 1;
                    break;
                case "2":
                    clip.dest_vclip = value - 1;
                    break;
                case "3":
                    clip.sound_num = value - 1;
                    break;
                case "4":
                    clip.crit_clip = value - 1;
                    break;
            }
        }

        private void RemapMultiImage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ImageSelector selector = new ImageSelector(piggyFile, true);
            if (selector.ShowDialog() == DialogResult.OK)
            {
                int value = selector.Selection;
                isLocked = true;
                clip.vc.RemapVClip(value, piggyFile);
                txtEffectFrameCount.Text = clip.vc.num_frames.ToString();
                txtEffectFrameSpeed.Text = clip.vc.frame_time.ToString();
                nudEffectFrame.Value = 0;
                UpdateEffectFrame(0);
                isLocked = false;
            }
            selector.Dispose();
        }

        private void RemapSingleImage_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ImageSelector selector = new ImageSelector(piggyFile, false);
            if (selector.ShowDialog() == DialogResult.OK)
            {
                isLocked = true;
                int value = selector.Selection;
                clip.vc.frames[(int)nudEffectFrame.Value] = (ushort)value;
                UpdateEffectFrame((int)nudEffectFrame.Value);
                txtEffectFrameNum.Text = value.ToString();
                isLocked = false;
            }
            selector.Dispose();
        }

        private void PlayCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (PlayCheckbox.Checked)
            {
                txtEffectTotalTime.Enabled = txtEffectFrameCount.Enabled = txtEffectFrameNum.Enabled = false;
                RemapAnimationButton.Enabled = nudEffectFrame.Enabled = false;
                if (clip.vc.num_frames < 0) return;
                //Ah, the horribly imprecise timer. Oh well
                AnimTimer.Interval = (int)(1000.0 * clip.vc.frame_time);
                if (AnimTimer.Interval < 10) AnimTimer.Interval = 10;
                AnimTimer.Start();
            }
            else
            {
                txtEffectTotalTime.Enabled = txtEffectFrameCount.Enabled = txtEffectFrameNum.Enabled = true;
                RemapAnimationButton.Enabled = nudEffectFrame.Enabled = true;
                AnimTimer.Stop();
            }
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            if (clip.vc.num_frames < 0) return;
            int currentFrame = (int)nudEffectFrame.Value;
            isLocked = true;
            UpdateEffectFrame(currentFrame);
            currentFrame++;
            if (currentFrame >= clip.vc.num_frames)
                currentFrame = 0;
            nudEffectFrame.Value = currentFrame;
            isLocked = false;
        }

    }
}
