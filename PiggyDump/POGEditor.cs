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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LibDescent.Data;
using LibDescent.Edit;

namespace Descent2Workshop
{
    public partial class POGEditor : Form
    {
        public POGFile datafile;
        private EditorHOGFile hogFile; //used as a source for palettes
        private string filename;
        public StandardUI host;
        private Palette currentPalette = new Palette();

        //will I ever understand GUI programming? I wonder...
        private bool isLocked = false;

        //Hold a linear basic palette. Want simplest possible representation for perf reasons
        private byte[] localPalette;
        private byte[] inverseColormap;

        Task paletteTask = null;

        public POGEditor(POGFile data, EditorHOGFile hogFile, string filename)
        {
            datafile = data;
            this.filename = filename;
            this.hogFile = hogFile;
            InitializeComponent();
            this.Text = string.Format("{0} - POG Editor", filename);
            PaletteComboBox.SelectedIndex = 0; //default to groupa.256
        }

        private void POGEditor_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < datafile.Bitmaps.Count; i++)
            {
                PIGImage image = (PIGImage)datafile.Bitmaps[i];
                ListViewItem lvi = GeneratePiggyEntry(i);
                listView1.Items.Add(lvi);
            }
        }

        private ListViewItem GeneratePiggyEntry(int i)
        {
            PIGImage image = datafile.Bitmaps[i];
            ListViewItem lvi = new ListViewItem(image.Name);
            lvi.SubItems.Add(image.ReplacementNum.ToString());
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

            image = datafile.Bitmaps[i];
            item.SubItems[1].Text = image.ReplacementNum.ToString();
            item.SubItems[2].Text = image.GetSize().ToString();
            item.SubItems[3].Text = string.Format("{0}x{1}", image.Width, image.Height);
            item.Text = image.Name;
            if (image.IsAnimated)
            {
                item.SubItems[4].Text = image.Frame.ToString();
            }
            else
            {
                item.SubItems[4].Text = "-1";
            }
        }

        private void PaletteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] data = hogFile.GetLumpData(hogFile.GetLumpNum(PaletteComboBox.Text));
            if (data != null)
            {
                currentPalette = new Palette(data);
                localPalette = currentPalette.GetLinear();

                if (paletteTask != null) //I don't know if it's a good idea to dispose a running task, so just wait for now
                {
                    paletteTask.Wait();
                    paletteTask.Dispose();
                }
                paletteTask = Task.Run(() => { inverseColormap = PiggyBitmapUtilities.BuildInverseColormap(localPalette); });

                if (listView1.Items.Count == 0) return;
                if (listView1.SelectedIndices.Count == 0) return;

                UpdateImage(listView1.SelectedIndices[0]);
            }
        }

        private void UpdateImage(int id)
        {
            isLocked = true;
            if (pictureBox1.Image != null)
            {
                Bitmap temp = (Bitmap)pictureBox1.Image;
                pictureBox1.Image = null;
                temp.Dispose();
            }
            PIGImage image = datafile.Bitmaps[listView1.SelectedIndices[0]];
            pictureBox1.Image = PiggyBitmapUtilities.GetBitmap(datafile.Bitmaps[id], currentPalette);
            TransparentCheck.Checked = image.Transparent;
            SupertransparentCheck.Checked = image.SuperTransparent;
            NoLightingCheck.Checked = image.NoLighting;
            CompressCheckBox.Checked = image.RLECompressed;
            System.Drawing.Color color = System.Drawing.Color.FromArgb(currentPalette.GetRGBAValue(image.AverageIndex));
            ColorPreview.BackColor = color;
            ReplacementSpinner.Value = (decimal)image.ReplacementNum;
            pictureBox1.Refresh();
            isLocked = false;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0) return;
            if (listView1.SelectedIndices.Count == 0) return;
            UpdateImage(listView1.SelectedIndices[0]);
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {
            int baseImageID = 0;
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ImageSelector imageSelector = new ImageSelector(host.DefaultPigFile, host.DefaultPalette, false);

                if (imageSelector.ShowDialog() == DialogResult.OK)
                {
                    foreach (string name in openFileDialog1.FileNames)
                    {
                        //If the inverse colormap isn't done, wait for it.
                        paletteTask.Wait();

                        Bitmap img = new Bitmap(name);
                        PIGImage bitmap = PiggyBitmapUtilities.CreatePIGImage(img, localPalette, inverseColormap, Path.GetFileName(name).Substring(0, Math.Min(Path.GetFileName(name).Length, 8)));
                        bitmap.ReplacementNum = (ushort)(imageSelector.Selection + baseImageID);

                        if (openFileDialog1.FileNames.Length > 1) //Auto animation on multi import
                        {
                            bitmap.IsAnimated = true;
                            bitmap.Frame = baseImageID;
                        }
                        baseImageID++;

                        datafile.Bitmaps.Add(bitmap);
                        ListViewItem lvi = GeneratePiggyEntry(datafile.Bitmaps.Count - 1);
                        listView1.Items.Add(lvi);
                        img.Dispose();
                    }
                }
            }
        }

        private void DoSave(string filename)
        {
            string statusMsg;
            if (!FileUtilities.SaveDataFile(filename, datafile, out statusMsg))
            {
                MessageBox.Show(statusMsg, "Error saving POG file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                filename = saveFileDialog1.FileName;
                Text = string.Format("{0} - POG Editor", filename);
            }
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != "")
                {
                    DoSave(saveFileDialog1.FileName);
                }
            }
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            if (filename != "")
            {
                DoSave(filename);
            }
            else
            {
                SaveAsMenuItem_Click(sender, e);
            }
        }

        private void ReplacementSpinner_ValueChanged(object sender, EventArgs e)
        {
            if (isLocked) return;
            int baseOffset = 0;
            foreach (int num in listView1.SelectedIndices)
            {
                datafile.Bitmaps[num].ReplacementNum = (ushort)Util.Clamp((int)ReplacementSpinner.Value + baseOffset, 0, 2619);
                baseOffset++;
                RebuildItem(listView1.Items[num]);
            }
        }

        private void ChooseReplacementButton_Click(object sender, EventArgs e)
        {
            ImageSelector imageSelector = new ImageSelector(host.DefaultPigFile, host.DefaultPalette, false);
            imageSelector.Selection = (int)ReplacementSpinner.Value;

            if (imageSelector.ShowDialog() == DialogResult.OK)
            {
                //Lazy solution: propagate through the spinner
                ReplacementSpinner.Value = (decimal)imageSelector.Selection;
            }
        }
    }
}
