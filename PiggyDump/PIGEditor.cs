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
using System.Threading.Tasks;
using LibDescent.Data;

namespace Descent2Workshop
{
    public partial class PIGEditor : Form
    {
        public PIGFile datafile;
        private Palette palette;
        private string filename;
        public StandardUI host;
        private LBMDecoder lbmDecoder = new LBMDecoder();

        private ImageEditorPanel panel;

        public PIGEditor(PIGFile data, Palette palette, string filename)
        {
            datafile = data;
            this.filename = filename;
            InitializeComponent();
            this.Text = string.Format("{0} - PIG Editor", filename);
            this.palette = palette;
            panel = new ImageEditorPanel(data, false);
            panel.SetPalette(palette);
            Controls.Add(panel);
            components.Add(panel);
            panel.Dock = DockStyle.Fill;

#if DEBUG==false
            mainMenu1.MenuItems.Remove(ExportILBMMenuItem);
            mainMenu1.MenuItems.Remove(DebugMenuSeparator);
#endif
        }

        private void PIGEditor_Load(object sender, EventArgs e)
        {
        }

        private void PIGEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            if (paletteTask != null)
            {
                paletteTask.Wait(); //Just be safe
                paletteTask.Dispose();
            }*/
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
            saveFileDialog1.Filter = "PIG Files|*.pig";
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
            panel.DeleteSelected();
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
                return String.Format("{0}+{1}", image.Name, image.Frame);
            }
        }

        private void ExportMenu_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "PNG Files|*.png";
            if (panel.SelectedIndices.Count > 1)
            {
                saveFileDialog1.FileName = "ignored";
            }
            else
            {
                saveFileDialog1.FileName = ImageFilename(panel.SelectedIndices[0]);//listView1.Items[listView1.SelectedIndices[0]].Text;
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (panel.SelectedIndices.Count > 1)
                {
                    string directory = Path.GetDirectoryName(saveFileDialog1.FileName);
                    foreach (int index in panel.SelectedIndices)
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
                        Bitmap img = PiggyBitmapUtilities.GetBitmap(datafile, palette, panel.SelectedIndices[0]);
                        img.Save(saveFileDialog1.FileName);
                        img.Dispose();
                    }
                }
            }
        }

        private void InsertMenu_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string name in openFileDialog1.FileNames)
                {
                    //If the inverse colormap isn't done, wait for it.
                    panel.WaitPaletteTask();

                    Bitmap img = new Bitmap(name);
                    panel.AddImageFromBitmap(img, Path.GetFileNameWithoutExtension(name));
                    img.Dispose();
                }
            }
        }

        private void MoveUpMenuItem_Click(object sender, EventArgs e)
        {
            panel.MoveSelectionUp();
        }

        private void MoveDownMenuItem_Click(object sender, EventArgs e)
        {
            panel.MoveSelectionDown();
        }

        private void ExportILBMMenu_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Deluxe Paint Brush|*.bbm";
            if (panel.SelectedIndices.Count > 1)
            {
                saveFileDialog1.FileName = "ignored";
            }
            else
            {
                saveFileDialog1.FileName = ImageFilename(panel.SelectedIndices[0]);//listView1.Items[listView1.SelectedIndices[0]].Text;
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (panel.SelectedIndices.Count > 1)
                {
                    string directory = Path.GetDirectoryName(saveFileDialog1.FileName);
                    foreach (int index in panel.SelectedIndices)
                    {
                        PIGImage image = datafile.Bitmaps[index];
                        string newpath = directory + Path.DirectorySeparatorChar + ImageFilename(index) + ".bbm";
                        BinaryWriter bw = new BinaryWriter(File.Open(newpath, FileMode.Create));
                        lbmDecoder.WriteBBM(image, palette, bw);
                        bw.Flush();
                        bw.Close();
                        bw.Dispose();
                    }
                }
                else
                {
                    if (saveFileDialog1.FileName != "")
                    {
                        PIGImage image = datafile.Bitmaps[panel.SelectedIndices[0]];
                        BinaryWriter bw = new BinaryWriter(File.Open(saveFileDialog1.FileName, FileMode.Create));
                        lbmDecoder.WriteBBM(image, palette, bw);
                        bw.Flush();
                        bw.Close();
                        bw.Dispose();
                    }
                }
            }
        }

        private void ImportMenuItem_Click(object sender, EventArgs e)
        {
            if (panel.SelectedIndices.Count == 0) return;
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string name in openFileDialog1.FileNames)
                {
                    //If the inverse colormap isn't done, wait for it.
                    panel.WaitPaletteTask();

                    Bitmap img = new Bitmap(name);
                    panel.ReplaceSelectedFromBitmap(img, Path.GetFileNameWithoutExtension(name));
                    img.Dispose();
                }
            }
        }

        private void MakeAnimatedMenuItem_Click(object sender, EventArgs e)
        {
            panel.AnimateSelectedRange();
        }

        private void ClearAnimationMenuItem_Click(object sender, EventArgs e)
        {
            panel.UnanimateSelectedRange();
        }

        private void SaveMenu_Click(object sender, EventArgs e)
        {
            if (File.Exists(filename))
            {
                DoSave(filename);
            }
            SaveAsMenu_Click(sender, e);
        }
    }
}
