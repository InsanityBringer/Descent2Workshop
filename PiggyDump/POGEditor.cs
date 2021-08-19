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
        ImageEditorPanel panel;

        public POGEditor(POGFile data, EditorHOGFile hogFile, StandardUI host, string filename)
        {
            datafile = data;
            this.filename = filename;
            this.hogFile = hogFile;
            this.host = host;
            InitializeComponent();
            this.Text = string.Format("{0} - POG Editor", filename);
            panel = new ImageEditorPanel(data, true, host.DefaultPigFile, host.DefaultPalette);
            panel.PaletteChanged += PaletteComboBox_SelectedIndexChanged;
            panel.SetPalette(host.DefaultPalette);
            components.Add(panel);
            Controls.Add(panel);
            panel.Dock = DockStyle.Fill;
        }

        private void POGEditor_Load(object sender, EventArgs e)
        {
        }

        private void PaletteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox paletteComboBox = (ComboBox)sender;
            byte[] data = hogFile.GetLumpData(hogFile.GetLumpNum(paletteComboBox.Text));
            if (data != null)
            {
                currentPalette = new Palette(data);
                panel.SetPalette(currentPalette);
                panel.ChangeImageToSelected();
            }
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //ImageSelector imageSelector = new ImageSelector(host.DefaultPigFile, host.DefaultPalette, false);

                //if (imageSelector.ShowDialog() == DialogResult.OK)
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

        private void MoveUpMenuItem_Click(object sender, EventArgs e)
        {
            panel.MoveSelectionUp();
        }

        private void MoveDownMenuItem_Click(object sender, EventArgs e)
        {
            panel.MoveSelectionDown();
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

        private void ExportMenuItem_Click(object sender, EventArgs e)
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
                        Bitmap img = PiggyBitmapUtilities.GetBitmap(datafile, currentPalette, index);
                        string newpath = directory + Path.DirectorySeparatorChar + ImageFilename(index) + ".png";
                        img.Save(newpath);
                        img.Dispose();
                    }
                }
                else
                {
                    if (saveFileDialog1.FileName != "")
                    {
                        Bitmap img = PiggyBitmapUtilities.GetBitmap(datafile, currentPalette, panel.SelectedIndices[0]);
                        img.Save(saveFileDialog1.FileName);
                        img.Dispose();
                    }
                }
            }
        }
    }
}
