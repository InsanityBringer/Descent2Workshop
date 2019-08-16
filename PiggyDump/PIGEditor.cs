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
using System.Windows.Forms;
using System.IO;

namespace PiggyDump
{
    public partial class PIGEditor : Form
    {
        public PIGFile datafile;
        public StandardUI host;
        public PIGEditor(PIGFile data)
        {
            datafile = data;
            InitializeComponent();
            lbCount.Text = String.Format("Count: {0}", datafile.images.Count);
        }

        private void PIGEditor_Load(object sender, EventArgs e)
        {
            for (int x = 0; x < datafile.images.Count; x++)
            {
                ImageData image = (ImageData)datafile.images[x];
                ListViewItem lvi = new ListViewItem(image.name);
                lvi.SubItems.Add(image.offset.ToString());
                //int compressionPercentage = (int)(image.compressionratio * 100f);
                //lvi.SubItems.Add(compressionPercentage.ToString() + "%");
                if (image.isAnimated)
                {
                    lvi.SubItems.Add(image.frame.ToString());
                }
                else
                {
                    lvi.SubItems.Add("-1");
                }
                listView1.Items.Add(lvi);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count <= 0)
            {
                return;
            }
            if (pictureBox1.Image != null)
            {
                Bitmap temp = (Bitmap)pictureBox1.Image;
                pictureBox1.Image = null;
                temp.Dispose();
            }
            ImageData image = datafile.images[listView1.SelectedIndices[0]];
            pictureBox1.Image = datafile.GetBitmap(listView1.SelectedIndices[0]);
            TransparentCheck.Checked = (image.flags & ImageData.BM_FLAG_TRANSPARENT) != 0;
            SupertransparentCheck.Checked = (image.flags & ImageData.BM_FLAG_SUPER_TRANSPARENT) != 0;
            NoLightingCheck.Checked = (image.flags & ImageData.BM_FLAG_NO_LIGHTING) != 0;
            Color color = datafile.PiggyPalette.GetDrawingColor(image.averageIndex);
            ColorPreview.BackColor = color;
            pictureBox1.Refresh();
        }

        private void PIGEditor_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != "")
                {
                    datafile.SaveDataFile(saveFileDialog1.FileName);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0) return;
            int index = listView1.SelectedIndices[0];
            int count = listView1.SelectedIndices.Count;
            for (int i = 0; i < count; i++)
            {
                DeleteAt(index);
            }
        }

        private void DeleteAt(int index)
        {
            listView1.Items.RemoveAt(index);
            datafile.images.RemoveAt(index);
            lbCount.Text = String.Format("Count: {0}", datafile.images.Count);
        }

        private void listView1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private string ImageFilename(int index)
        {
            ImageData image = datafile.images[index];
            if (!image.isAnimated)
            {
                return image.name;
            }
            else
            {
                return String.Format("{0}+{1}", image.name, image.frame);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "PNG Files|*.png";
            if (listView1.SelectedIndices.Count > 1)
            {
                saveFileDialog1.FileName = "ignored";
            }
            else
            {
                saveFileDialog1.FileName = ImageFilename(listView1.SelectedIndices[0]);//listView1.Items[listView1.SelectedIndices[0]].Text;
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (listView1.SelectedIndices.Count > 1)
                {
                    string directory = Path.GetDirectoryName(saveFileDialog1.FileName);
                    foreach (int index in listView1.SelectedIndices)
                    {
                        Bitmap img = datafile.GetBitmap(index);
                        string newpath = directory + Path.DirectorySeparatorChar + ImageFilename(index) + ".png";
                        lbCount.Text = newpath;
                        img.Save(newpath);
                        img.Dispose();
                    }
                }
                else
                {
                    if (saveFileDialog1.FileName != "")
                    {
                        Bitmap img = datafile.GetBitmap(listView1.SelectedIndices[0]);
                        img.Save(saveFileDialog1.FileName);
                        img.Dispose();
                    }
                }
            }
        }
    }
}
