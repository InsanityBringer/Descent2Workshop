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

namespace Descent2Workshop
{
    public partial class ImageEditorPanel : UserControl
    {
        private bool isLocked = false;
        private IImageProvider imageProvider;
        private bool isReplacingArchive;
        private int zoom = 1;
        private IImageProvider baseProvider;
        private Palette basePalette;

        private Palette palette;
        //Hold a linear basic palette. Want simplest possible representation for perf reasons
        private byte[] localPalette;
        private byte[] inverseColormap;
        //Task to regenerate inverse colormap
        Task paletteTask;

        //Hack to signify the parent that a new palette needs to be set. 
        public EventHandler PaletteChanged;

        public ListView.SelectedIndexCollection SelectedIndices { get => ImageListView.SelectedIndices; }

        public ImageEditorPanel(IImageProvider imageProvider, bool isPOG, IImageProvider baseProvider = null, Palette basePalette = null)
        {
            InitializeComponent();
            this.imageProvider = imageProvider;
            isReplacingArchive = isPOG;

            ReplacamentPanel.Visible = isReplacingArchive;

            if (isPOG && (baseProvider == null || basePalette == null))
                throw new ArgumentException("baseProvider and basePalette must not be null if isPOG is set");

            this.baseProvider = baseProvider;
            this.basePalette = basePalette;

            PaletteComboBox.SelectedIndex = 0;

            GenerateList();
        }

        private ListViewItem GeneratePiggyEntry(int i)
        {
            PIGImage image = imageProvider.Bitmaps[i];
            ListViewItem lvi = new ListViewItem(image.Name);
            if (isReplacingArchive)
                lvi.SubItems.Add(image.ReplacementNum.ToString());
            else
                lvi.SubItems.Add(i.ToString());
            lvi.SubItems.Add(image.GetSize().ToString());
            lvi.SubItems.Add(string.Format("{0}x{1}", image.Width, image.Height));
            if (image.IsAnimated)
            {
                lvi.SubItems.Add(image.Frame.ToString());
            }
            else
            {
                lvi.SubItems.Add("-1");
            }

            return lvi;
        }

        private void RebuildItem(ListViewItem item)
        {
            PIGImage image;
            int i = item.Index;

            image = imageProvider.Bitmaps[i];
            if (isReplacingArchive)
                item.SubItems[1].Text = image.ReplacementNum.ToString();
            else
                item.SubItems[1].Text = i.ToString();
            item.SubItems[2].Text = image.GetSize().ToString();
            item.SubItems[3].Text = string.Format("{0}x{1}", image.Width, image.Height);
            item.Text = image.Name; //I hope there wasn't a reason why I didn't have this. 
            if (image.IsAnimated)
            {
                item.SubItems[4].Text = image.Frame.ToString();
            }
            else
            {
                item.SubItems[4].Text = "-1";
            }
        }

        private void GenerateList()
        {
            for (int x = 0; x < imageProvider.Bitmaps.Count; x++)
            {
                ListViewItem lvi = GeneratePiggyEntry(x);
                ImageListView.Items.Add(lvi);
            }
        }

        private void ChangeImage(int id)
        {
            isLocked = true;
            if (pictureBox1.Image != null)
            {
                Bitmap temp = (Bitmap)pictureBox1.Image;
                pictureBox1.Image = null;
                temp.Dispose();
            }
            PIGImage image = imageProvider.Bitmaps[id];
            pictureBox1.Image = PiggyBitmapUtilities.GetBitmap(imageProvider, palette, id, zoom);
            TransparentCheck.Checked = image.Transparent;
            SupertransparentCheck.Checked = image.SuperTransparent;
            NoLightingCheck.Checked = image.NoLighting;
            CompressCheckBox.Checked = image.RLECompressed;
            System.Drawing.Color color = System.Drawing.Color.FromArgb(palette.GetRGBAValue(image.AverageIndex));
            ColorPreview.BackColor = color;
            ReplacementSpinner.Value = image.ReplacementNum;
            pictureBox1.Refresh();
            isLocked = false;
        }

        private void CompressCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ImageListView.SelectedItems.Count == 0) return;
            if (isLocked) return; //will call ourselves in case of an error
            isLocked = true;
            bool currentState = !CompressCheckBox.Checked;
            try
            {
                PIGImage img = imageProvider.Bitmaps[ImageListView.SelectedIndices[0]];
                img.RLECompressed = CompressCheckBox.Checked;
                ImageListView.Items[ImageListView.SelectedIndices[0]].SubItems[2].Text = img.GetSize().ToString();
            }
            catch (Exception exc)
            {
                MessageBox.Show(string.Format("Error compressing image:\r\n{0}", exc.Message));
                CompressCheckBox.Checked = currentState;
            }
            isLocked = false;
        }

        private void SupertransparentCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            PIGImage img;
            for (int i = 0; i < ImageListView.SelectedIndices.Count; i++)
            {
                img = imageProvider.Bitmaps[ImageListView.SelectedIndices[i]];
                img.SuperTransparent = SupertransparentCheck.Checked;
            }
        }

        private void TransparentCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            PIGImage img;
            for (int i = 0; i < ImageListView.SelectedIndices.Count; i++)
            {
                img = imageProvider.Bitmaps[ImageListView.SelectedIndices[i]];
                img.Transparent = TransparentCheck.Checked;
            }
        }

        private void NoLightingCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            PIGImage img;
            for (int i = 0; i < ImageListView.SelectedIndices.Count; i++)
            {
                img = imageProvider.Bitmaps[ImageListView.SelectedIndices[i]];
                img.NoLighting = NoLightingCheck.Checked;
            }
        }

        private void CalculateAverageButton_Click(object sender, EventArgs e)
        {
            if (ImageListView.SelectedItems.Count == 0) return;
            PIGImage image;
            for (int i = 0; i < ImageListView.SelectedItems.Count; i++)
            {
                image = imageProvider.Bitmaps[ImageListView.SelectedIndices[i]];
                PiggyBitmapUtilities.SetAverageColor(image, localPalette);

            }
            image = imageProvider.Bitmaps[ImageListView.SelectedIndices[0]];
            System.Drawing.Color color = System.Drawing.Color.FromArgb(palette.GetRGBAValue(image.AverageIndex));
            ColorPreview.BackColor = color;
            pictureBox1.Refresh();
        }

        private void ZoomTrackBar_Scroll(object sender, EventArgs e)
        {
            zoom = ZoomTrackBar.Value + 1;
            ZoomLabel.Text = string.Format("Zoom: {0}%", (int)(zoom * 100));
            ChangeImageToSelected();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeImageToSelected();
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != null) //if you don't do this your program crashes at complete random btw
            {
                imageProvider.Bitmaps[e.Item].Name = e.Label;
                ImageListView.Items[e.Item].SubItems[0].Text = imageProvider.Bitmaps[e.Item].Name; //In case it got changed
            }
        }

        private void ReplacementSpinner_ValueChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            int baseOffset = 0;
            foreach (int num in ImageListView.SelectedIndices)
            {
                imageProvider.Bitmaps[num].ReplacementNum = (ushort)Util.Clamp((int)ReplacementSpinner.Value + baseOffset, 0, 2619);
                baseOffset++;
                RebuildItem(ImageListView.Items[num]);
            }
        }

        private void ChooseReplacementButton_Click(object sender, EventArgs e)
        {
            if (isLocked) return;
            int baseOffset = 0;
            ImageSelector imageSelector = new ImageSelector(baseProvider, basePalette, false);
            imageSelector.Selection = (int)ReplacementSpinner.Value;

            if (imageSelector.ShowDialog() == DialogResult.OK)
            {
                foreach (int num in ImageListView.SelectedIndices)
                {
                    imageProvider.Bitmaps[num].ReplacementNum = (ushort)Util.Clamp(imageSelector.Selection + baseOffset, 0, 2619);
                    baseOffset++;
                    RebuildItem(ImageListView.Items[num]);
                }
            }
        }

        public void ChangeImageToSelected()
        {
            if (ImageListView.SelectedIndices.Count <= 0)
            {
                return;
            }
            ChangeImage(ImageListView.SelectedIndices[0]);
        }

        public void SetPalette(Palette palette)
        {
            this.palette = palette;
            localPalette = palette.GetLinear();
            if (paletteTask != null && paletteTask.Status == TaskStatus.Running)
                paletteTask.Wait();

            paletteTask = Task.Run(() => { inverseColormap = PiggyBitmapUtilities.BuildInverseColormap(localPalette); });
        }

        public void WaitPaletteTask()
        {
            paletteTask.Wait();
        }

        public void AddImageFromBitmap(Bitmap img, string name)
        {
            PIGImage bitmap = PiggyBitmapUtilities.CreatePIGImage(img, localPalette, inverseColormap, name.Substring(0, Math.Min(name.Length, 8)));
            imageProvider.Bitmaps.Add(bitmap);
            ListViewItem lvi = GeneratePiggyEntry(imageProvider.Bitmaps.Count - 1);
            ImageListView.Items.Add(lvi);
        }

        public void ReplaceSelectedFromBitmap(Bitmap img, string name)
        {
            PIGImage oldbitmap = imageProvider.Bitmaps[ImageListView.SelectedIndices[0]];
            PIGImage bitmap = PiggyBitmapUtilities.CreatePIGImage(img, localPalette, inverseColormap, name.Substring(0, Math.Min(name.Length, 8)));
            bitmap.ReplacementNum = oldbitmap.ReplacementNum;
            imageProvider.Bitmaps[ImageListView.SelectedIndices[0]] = bitmap;
            RebuildItem(ImageListView.SelectedItems[0]);
            ChangeImage(ImageListView.SelectedIndices[0]);
        }

        public void DeleteSelected()
        {
            if (ImageListView.SelectedIndices.Count == 0) return;
            int index = -1;
            int count = ImageListView.SelectedIndices.Count;
            for (int i = count-1; i >= 0; i--)
            {
                index = ImageListView.SelectedIndices[i];
                if (index == 0)
                    MessageBox.Show("Cannot delete the bogus bitmap!");
                else
                    DeleteAt(index);
            }
            if (index != -1)
                RebuildListFrom(index);
        }

        public void DeleteAt(int index)
        {
            ImageListView.Items.RemoveAt(index);
            imageProvider.Bitmaps.RemoveAt(index);
        }

        public void RebuildListFrom(int index)
        {
            //PIGImage image;
            for (int i = index; i < ImageListView.Items.Count; i++)
            {
                ListViewItem item = ImageListView.Items[i];
                item.SubItems[1].Text = i.ToString();
            }
        }

        public void MoveSelectionUp()
        {
            if (ImageListView.SelectedIndices.Count == 0) return;
            int baseIndex = int.MaxValue;
            int index;
            PIGImage image;
            ListViewItem temp;

            int[] test = new int[ImageListView.SelectedIndices.Count];
            ImageListView.SelectedIndices.CopyTo(test, 0);
            Array.Sort(test);
            for (int i = 0; i < test.Length; i++)
            {
                //This isn't the most elegant approach, but it should preserve relative ordering
                index = test[i];
                if (index < baseIndex) baseIndex = index;
                image = imageProvider.Bitmaps[index];
                if (index <= 1) continue; //Don't allow moving the bogus image, or allow swapping the bogus image. 
                //Remove the old image at its position
                imageProvider.Bitmaps.RemoveAt(index);
                //Reinsert the image at its new position
                imageProvider.Bitmaps.Insert(index - 1, image);
                //Do the same for the list view item
                temp = ImageListView.Items[index];
                ImageListView.Items.RemoveAt(index);
                ImageListView.Items.Insert(index - 1, temp);
                RebuildItem(temp);
                RebuildItem(ImageListView.Items[index]);
            }
        }

        public void MoveSelectionDown()
        {
            if (ImageListView.SelectedIndices.Count == 0) return;
            int baseIndex = int.MaxValue;
            int index;
            PIGImage image;
            ListViewItem temp;

            int[] test = new int[ImageListView.SelectedIndices.Count];
            ImageListView.SelectedIndices.CopyTo(test, 0);
            Array.Sort(test);
            for (int i = test.Length - 1; i >= 0; i--)
            {
                //This isn't the most elegant approach, but it should preserve relative ordering
                index = test[i];
                if (index < baseIndex) baseIndex = index;
                image = imageProvider.Bitmaps[index];
                if (index == 0) continue; //Don't allow moving the bogus image
                if (index + 1 >= imageProvider.Bitmaps.Count) continue; //Don't allow moving past the end of the list
                //Remove the old image at its position
                imageProvider.Bitmaps.RemoveAt(index);
                //Reinsert the image at its new position
                imageProvider.Bitmaps.Insert(index + 1, image);
                //Do the same for the list view item
                temp = ImageListView.Items[index];
                ImageListView.Items.RemoveAt(index);
                ImageListView.Items.Insert(index + 1, temp);
                RebuildItem(temp);
                RebuildItem(ImageListView.Items[index]);
            }
        }

        public void AnimateSelectedRange()
        {
            if (ImageListView.SelectedIndices.Count == 0) return;

            //All frames need the same name, though in practice this is just for the bitmaps.tbl compiler.
            //The shared name is used to determine how many frames are in an animation
            string animName = imageProvider.Bitmaps[ImageListView.SelectedIndices[0]].Name;
            PIGImage img;
            for (int i = 0; i < ImageListView.SelectedIndices.Count; i++)
            {
                img = imageProvider.Bitmaps[ImageListView.SelectedIndices[i]];
                img.Name = animName;
                img.IsAnimated = true;
                img.Frame = i;
                RebuildItem(ImageListView.SelectedItems[i]);
            }
        }

        public void UnanimateSelectedRange()
        {
            if (ImageListView.SelectedIndices.Count == 0) return;

            PIGImage img;
            for (int i = 0; i < ImageListView.SelectedIndices.Count; i++)
            {
                img = imageProvider.Bitmaps[ImageListView.SelectedIndices[i]];
                //TODO: I should have the IsAnimated property automatically clear the frame. 
                img.IsAnimated = false;
                img.Frame = 0;
                RebuildItem(ImageListView.SelectedItems[i]);
            }
        }

        private void PaletteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaletteChanged?.Invoke(sender, e);
        }
    }
}
