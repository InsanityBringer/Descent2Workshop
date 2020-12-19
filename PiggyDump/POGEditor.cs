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
            pictureBox1.Refresh();
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
                ImageSelector imageSelector = new ImageSelector(host.DefaultPigFile, currentPalette, false);

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
    }
}
