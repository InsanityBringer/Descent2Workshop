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
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using LibDescent.Data;

namespace Descent2Workshop
{
    public partial class PIGEditor : Form
    {
        public PIGFile datafile;
        private Palette palette;
        public StandardUI host;
        public PIGEditor(PIGFile data, Palette palette)
        {
            datafile = data;
            InitializeComponent();
            this.Text = string.Format("{0} - PIG Editor", datafile.filename);
            this.palette = palette;
        }

        private void PIGEditor_Load(object sender, EventArgs e)
        {
            for (int x = 0; x < datafile.Bitmaps.Count; x++)
            {
                PIGImage image = (PIGImage)datafile.Bitmaps[x];
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
                lvi.SubItems.Add(x.ToString());
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
            PIGImage image = datafile.Bitmaps[listView1.SelectedIndices[0]];
            pictureBox1.Image = PiggyBitmapConverter.GetBitmap(datafile, palette, listView1.SelectedIndices[0]);
            TransparentCheck.Checked = (image.flags & PIGImage.BM_FLAG_TRANSPARENT) != 0;
            SupertransparentCheck.Checked = (image.flags & PIGImage.BM_FLAG_SUPER_TRANSPARENT) != 0;
            NoLightingCheck.Checked = (image.flags & PIGImage.BM_FLAG_NO_LIGHTING) != 0;
            Color color = Color.FromArgb(palette.GetDrawingColorH(image.averageIndex));
            ColorPreview.BackColor = color;
            pictureBox1.Refresh();
        }

        private void PIGEditor_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void SaveAsMenu_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != "")
                {
                    datafile.Write(saveFileDialog1.FileName);
                    this.Text = string.Format("{0} - PIG Editor", datafile.filename);
                }
            }
        }

        private void DeleteMenu_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0) return;
            int index = listView1.SelectedIndices[0];
            int count = listView1.SelectedIndices.Count;
            for (int i = 0; i < count; i++)
            {
                if (index == 0)
                    MessageBox.Show("Cannot delete the bogus bitmap!");
                else
                    DeleteAt(index);
            }
            //Fix the indicies of all items
            for (int i = index-1; i < listView1.Items.Count; i++)
            {
                ListViewItem item = listView1.Items[i];
                item.SubItems[3].Text = i.ToString();
            }
        }

        private void DeleteAt(int index)
        {
            listView1.Items.RemoveAt(index);
            datafile.Bitmaps.RemoveAt(index);
        }

        private void listView1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private string ImageFilename(int index)
        {
            PIGImage image = datafile.Bitmaps[index];
            if (!image.isAnimated)
            {
                return image.name;
            }
            else
            {
                return String.Format("{0}+{1}", image.name, image.frame);
            }
        }

        private void ExportMenu_Click(object sender, EventArgs e)
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
                        Bitmap img = PiggyBitmapConverter.GetBitmap(datafile, palette, index);
                        string newpath = directory + Path.DirectorySeparatorChar + ImageFilename(index) + ".png";
                        img.Save(newpath);
                        img.Dispose();
                    }
                }
                else
                {
                    if (saveFileDialog1.FileName != "")
                    {
                        Bitmap img = PiggyBitmapConverter.GetBitmap(datafile, palette, listView1.SelectedIndices[0]);
                        img.Save(saveFileDialog1.FileName);
                        img.Dispose();
                    }
                }
            }
        }
    }
}
