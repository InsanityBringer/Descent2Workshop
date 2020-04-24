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
        private string filename;
        public StandardUI host;
        private bool isLocked = false;
        public PIGEditor(PIGFile data, Palette palette, string filename)
        {
            datafile = data;
            this.filename = filename;
            InitializeComponent();
            this.Text = string.Format("{0} - PIG Editor", filename);
            this.palette = palette;
        }

        private ListViewItem GeneratePiggyEntry(int i)
        {
            PIGImage image = datafile.Bitmaps[i];
            ListViewItem lvi = new ListViewItem(image.Name);
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

        private void PIGEditor_Load(object sender, EventArgs e)
        {
            for (int x = 0; x < datafile.Bitmaps.Count; x++)
            {
                ListViewItem lvi = GeneratePiggyEntry(x);
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
            isLocked = true;
            PIGImage image = datafile.Bitmaps[listView1.SelectedIndices[0]];
            pictureBox1.Image = PiggyBitmapUtilities.GetBitmap(datafile, palette, listView1.SelectedIndices[0]);
            TransparentCheck.Checked = image.Transparent;
            SupertransparentCheck.Checked = image.SuperTransparent;
            NoLightingCheck.Checked = image.NoLighting;
            CompressCheckBox.Checked = image.RLECompressed;
            Color color = Color.FromArgb(palette.GetRGBAValue(image.AverageIndex));
            ColorPreview.BackColor = color;
            pictureBox1.Refresh();
            isLocked = false;
        }

        private void PIGEditor_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void DoSave(string filename)
        {
            string statusMsg;
            if (!FileUtilities.SaveDataFile(filename, datafile, out statusMsg))
            {
                MessageBox.Show(statusMsg, "Error saving PIG file.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                filename = saveFileDialog1.FileName;
                Text = string.Format("{0} - PIG Editor", filename);
            }
        }

        private void SaveAsMenu_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.FileName != "")
                {
                    DoSave(saveFileDialog1.FileName);
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
            RebuildListFrom(index);
        }

        private void DeleteAt(int index)
        {
            listView1.Items.RemoveAt(index);
            datafile.Bitmaps.RemoveAt(index);
        }

        private void RebuildItem(ListViewItem item)
        {
            PIGImage image;
            int i = item.Index;

            image = datafile.Bitmaps[i];
            item.SubItems[1].Text = i.ToString();
            /*item.SubItems[1].Text = image.Name;
            item.SubItems[2].Text = image.GetSize().ToString();
            item.SubItems[3].Text = string.Format("{0}x{1}", image.Width, image.Height);
            if (image.IsAnimated)
            {
                item.SubItems[4].Text = image.Frame.ToString();
            }
            else
            {
                item.SubItems[4].Text = "-1";
            }*/
        }

        private void RebuildListFrom(int index)
        {
            //PIGImage image;
            for (int i = index; i < listView1.Items.Count; i++)
            {
                //image = datafile.Bitmaps[i];
                ListViewItem item = listView1.Items[i];
                item.SubItems[1].Text = i.ToString();
                /*item.SubItems[0].Text = image.Name;
                item.SubItems[2].Text = image.GetSize().ToString();
                item.SubItems[3].Text = string.Format("{0}x{1}", image.Width, image.Height);
                if (image.IsAnimated)
                {
                    item.SubItems[4].Text = image.Frame.ToString();
                }
                else
                {
                    item.SubItems[4].Text = "-1";
                }*/
            }
        }

        private void listView1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private string ImageFilename(int index)
        {
            PIGImage image = datafile.Bitmaps[index];
            if (!image.IsAnimated)
            {
                return image.Name;
            }
            else
            {
                return String.Format("{0}+{1}", image.Name, image.DFlags);
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
                        Bitmap img = PiggyBitmapUtilities.GetBitmap(datafile, palette, index);
                        string newpath = directory + Path.DirectorySeparatorChar + ImageFilename(index) + ".png";
                        img.Save(newpath);
                        img.Dispose();
                    }
                }
                else
                {
                    if (saveFileDialog1.FileName != "")
                    {
                        Bitmap img = PiggyBitmapUtilities.GetBitmap(datafile, palette, listView1.SelectedIndices[0]);
                        img.Save(saveFileDialog1.FileName);
                        img.Dispose();
                    }
                }
            }
        }

        private void CompressCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            if (isLocked) return; //will call ourselves in case of an error
            isLocked = true;
            bool currentState = !CompressCheckBox.Checked;
            try
            {
                PIGImage img = datafile.Bitmaps[listView1.SelectedIndices[0]];
                img.RLECompressed = CompressCheckBox.Checked;
                listView1.Items[listView1.SelectedIndices[0]].SubItems[1].Text = img.GetSize().ToString();
            }
            catch (Exception exc)
            {
                MessageBox.Show(string.Format("Error compressing image:\r\n{0}", exc.Message));
                CompressCheckBox.Checked = currentState;
            }
            isLocked = false;
        }

        private void CalculateAverageButton_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            PIGImage image;
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                image = datafile.Bitmaps[listView1.SelectedIndices[i]];
                PiggyBitmapUtilities.SetAverageColor(image, palette);
                
            }
            image = datafile.Bitmaps[listView1.SelectedIndices[0]];
            Color color = Color.FromArgb(palette.GetRGBAValue(image.AverageIndex));
            ColorPreview.BackColor = color;
            pictureBox1.Refresh();
        }

        private void InsertMenu_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string name in openFileDialog1.FileNames)
                {
                    Bitmap img = new Bitmap(name);
                    PIGImage bitmap = PiggyBitmapUtilities.CreatePIGImage(img, palette, Path.GetFileName(name).Substring(0, Math.Min(Path.GetFileName(name).Length, 8)));
                    datafile.Bitmaps.Add(bitmap);
                    ListViewItem lvi = GeneratePiggyEntry(datafile.Bitmaps.Count - 1);
                    listView1.Items.Add(lvi);
                    img.Dispose();
                }
            }
        }

        private void MoveUpMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0) return;
            int baseIndex = int.MaxValue;
            int index;
            PIGImage image;
            ListViewItem temp;

            int[] test = new int[listView1.SelectedIndices.Count];
            listView1.SelectedIndices.CopyTo(test, 0);
            Array.Sort(test);
            for (int i = 0; i < test.Length; i++)
            {
                //This isn't the most elegant approach, but it should preserve relative ordering
                index = test[i];
                if (index < baseIndex) baseIndex = index;
                image = datafile.Bitmaps[index];
                if (index <= 1) continue; //Don't allow moving the bogus image, or allow swapping the bogus image. 
                //Remove the old image at its position
                datafile.Bitmaps.RemoveAt(index);
                //Reinsert the image at its new position
                datafile.Bitmaps.Insert(index - 1, image);
                //Do the same for the list view item
                temp = listView1.Items[index];
                listView1.Items.RemoveAt(index);
                listView1.Items.Insert(index - 1, temp);
                RebuildItem(temp);
                RebuildItem(listView1.Items[index]);
            }
        }

        private void MoveDownMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0) return;
            int baseIndex = int.MaxValue;
            int index;
            PIGImage image;
            ListViewItem temp;

            int[] test = new int[listView1.SelectedIndices.Count];
            listView1.SelectedIndices.CopyTo(test, 0);
            Array.Sort(test);
            for (int i = test.Length - 1; i >= 0; i--)
            {
                //This isn't the most elegant approach, but it should preserve relative ordering
                index = test[i];
                if (index < baseIndex) baseIndex = index;
                image = datafile.Bitmaps[index];
                if (index == 0) continue; //Don't allow moving the bogus image
                if (index + 1 >= datafile.Bitmaps.Count) continue; //Don't allow moving past the end of the list
                //Remove the old image at its position
                datafile.Bitmaps.RemoveAt(index);
                //Reinsert the image at its new position
                datafile.Bitmaps.Insert(index + 1, image);
                //Do the same for the list view item
                temp = listView1.Items[index];
                listView1.Items.RemoveAt(index);
                listView1.Items.Insert(index + 1, temp);
                RebuildItem(temp);
                RebuildItem(listView1.Items[index]);
            }
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != null) //if you don't do this your program crashes at complete random btw
            {
                datafile.Bitmaps[e.Item].Name = e.Label;
                listView1.Items[e.Item].SubItems[0].Text = datafile.Bitmaps[e.Item].Name; //In case it got changed
            }
        }
    }
}
