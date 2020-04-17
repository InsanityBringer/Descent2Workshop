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
using LibDescent.Data;

namespace Descent2Workshop
{
    public partial class POGEditor : Form
    {
        public POGFile datafile;
        private HOGFile hogFile; //used as a source for palettes
        private string filename;
        public StandardUI host;
        private Palette currentPalette = new Palette();
        public POGEditor(POGFile data, HOGFile hogFile, string filename)
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
                ListViewItem lvi = new ListViewItem(image.name);
                lvi.SubItems.Add(image.GetSize().ToString());
                if (image.isAnimated)
                {
                    lvi.SubItems.Add(image.frame.ToString());
                }
                else
                {
                    lvi.SubItems.Add("-1");
                }
                lvi.SubItems.Add(image.ReplacementNum.ToString());
                listView1.Items.Add(lvi);
            }
        }

        private void PaletteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] data = hogFile.GetLumpData(hogFile.GetLumpNum(PaletteComboBox.Text));
            if (data != null)
            {
                currentPalette = new Palette(data);
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
            pictureBox1.Image = PiggyBitmapConverter.GetBitmap(datafile.Bitmaps[id], currentPalette);
            TransparentCheck.Checked = image.Transparent;
            SupertransparentCheck.Checked = image.SuperTransparent;
            NoLightingCheck.Checked = image.NoLighting;
            CompressCheckBox.Checked = image.RLECompressed;
            Color color = Color.FromArgb(currentPalette.GetDrawingColorH(image.averageIndex));
            ColorPreview.BackColor = color;
            pictureBox1.Refresh();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0) return;
            if (listView1.SelectedIndices.Count == 0) return;
            UpdateImage(listView1.SelectedIndices[0]);
        }
    }
}
