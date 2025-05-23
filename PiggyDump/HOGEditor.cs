﻿/*
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
using LibDescent.Data;
using LibDescent.Edit;
using System.Drawing.Imaging;

namespace Descent2Workshop
{
    public partial class HOGEditor : Form
    {
        private EditorHOGFile datafile;
        private StandardUI host;
        private bool CurrentTextLF;
        private string CurrentTextName;
        private bool CurrentTextEncoded;
        private bool _modified;

        private bool Modified {
            get => _modified;
            set
            {
                if (value == _modified) return;
                _modified = value;
                UpdateTitle();
            }
        }

        public HOGEditor(EditorHOGFile data, StandardUI host)
        {
            InitializeComponent();
            datafile = data;
            this.host = host;
            lvlPreview.Host = host;
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Text = string.Format("{0} - Hog Editor{1}", datafile.Filename,
                Modified ? " (modified)" : "");
        }

        private ListViewItem LumpToListItem(HOGLump lump)
        {
            ListViewItem listItem = new ListViewItem(lump.Name);
            listItem.SubItems.Add(lump.Size.ToString());
            listItem.SubItems.Add(lump.Type.ToString());
            return listItem;
        }

        private void HOGEditor_Load(object sender, EventArgs e)
        {
            for (int x = 0; x < datafile.NumLumps; x++)
            {
                listView1.Items.Add(LumpToListItem(datafile.GetLumpHeader(x)));
            }
            string count = string.Format("Total Elements: {0}", datafile.NumLumps);
        }

        private void LoadLumpAt(string path, int index)
        {
            string filename = Path.GetFileName(path);
            BinaryReader br;
            if (filename.Length > 12)
            {
                //trim the filename
                filename = filename.Remove(12);
            }
            br = new BinaryReader(File.Open(path, FileMode.Open));
            int size = (int)br.BaseStream.Length;
            byte[] data = br.ReadBytes((int)br.BaseStream.Length);
            HOGLump newLump = new HOGLump(filename, size, -1);
            newLump.Data = data;
            newLump.Type = HOGLump.IdentifyLump(newLump.Name, newLump.Data);
            var listItem = LumpToListItem(newLump);

            if (index != -1)
            {
                listView1.Items.Insert(index, listItem);
                datafile.AddLumpAt(newLump, index);
            }
            else
            {
                listView1.Items.Add(listItem);
                datafile.AddLump(newLump);
            }
            Modified = true;
        }

        private void InsertMenu_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                BinaryReader br;
                try
                {
                    LoadLumpAt(openFileDialog1.FileName, -1);
                }
                catch (Exception)
                {
                    MessageBox.Show("There was an error loading the lump!");
                }
            }
        }

        private void DeleteMenu_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                return;
            }
            datafile.DeleteLump(listView1.SelectedIndices[0]);
            listView1.Items.RemoveAt(listView1.SelectedIndices[0]);
        }

        //TODO WHAT THE EVERLOVING CRAP IS THIS DOING HERE
        private void WriteHogLump(string filename, int id)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create));
            bw.Write(datafile.GetLumpData(id));
            bw.Flush();
            bw.Close();
        }

        private void ExportMenu_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0) return;
            saveFileDialog1.Filter = "All Files|*.*";
            if (listView1.SelectedIndices.Count > 1)
            {
                saveFileDialog1.FileName = "ignored";
            }
            else
            {
                saveFileDialog1.FileName = listView1.Items[listView1.SelectedIndices[0]].Text;
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (listView1.SelectedIndices.Count > 1)
                {
                    string directory = Path.GetDirectoryName(saveFileDialog1.FileName);
                    foreach (int index in listView1.SelectedIndices)
                    {
                        string newpath = directory + Path.DirectorySeparatorChar + listView1.Items[index].Text;
                        WriteHogLump(newpath, index);
                    }
                }
                else
                {
                    if (saveFileDialog1.FileName != "")
                    {
                        WriteHogLump(saveFileDialog1.FileName, listView1.SelectedIndices[0]);
                    }
                }
            }
        }

        private void SaveAsMenu_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "HOG Files|*.HOG";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int err = 0;
                try
                {
                    datafile.Write(saveFileDialog1.FileName);
                    Modified = false;
                }
                catch (Exception exc)
                {
                    err = FileUtilities.GetErrorCode(exc);
                }

                if (err != 0)
                {
                    host.AppendConsole(FileUtilities.FileErrorCodeHandler(err, "write", "HOG file"));
                }
                else
                {
                    this.Text = string.Format("{0} - Hog Editor", datafile.Filename);
                }
            }
        }

        private void SaveMenu_Click(object sender, EventArgs e)
        {
            int err = 0;
            try
            {
                datafile.Write(datafile.Filename);
                Modified = false;
            }
            catch (Exception exc)
            {
                err = FileUtilities.GetErrorCode(exc);
            }

            if (err != 0)
            {
                host.AppendConsole(FileUtilities.FileErrorCodeHandler(err, "write", "HOG file"));
            }
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if ((e.AllowedEffect & DragDropEffects.Copy) != 0)
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listView1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //ugh, copy-paste from MSDN...
                // Retrieve the client coordinates of the mouse pointer.
                Point targetPoint =
                    listView1.PointToClient(new Point(e.X, e.Y));

                // Retrieve the index of the item closest to the mouse pointer.
                int targetIndex = listView1.InsertionMark.NearestIndex(targetPoint);

                // Confirm that the mouse pointer is not over the dragged item.
                if (targetIndex > -1)
                {
                    // Determine whether the mouse pointer is to the left or
                    // the right of the midpoint of the closest item and set
                    // the InsertionMark.AppearsAfterItem property accordingly.
                    Rectangle itemBounds = listView1.GetItemRect(targetIndex);
                    if (targetPoint.Y > itemBounds.Top + (itemBounds.Height / 2))
                    {
                        listView1.InsertionMark.AppearsAfterItem = true;
                    }
                    else
                    {
                        listView1.InsertionMark.AppearsAfterItem = false;
                    }
                }

                // Set the location of the insertion mark. If the mouse is
                // over the dragged item, the targetIndex value is -1 and
                // the insertion mark disappears.
                listView1.InsertionMark.Index = targetIndex;
            }
        }

        private void listView1_DragLeave(object sender, EventArgs e)
        {
            listView1.InsertionMark.Index = -1; //it went bye bye so
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Retrieve the index of the insertion mark;
                int targetIndex = listView1.InsertionMark.Index;

                // If the insertion mark is not visible, exit the method.
                //actually let's hock it at the end because otherwise you can't insert into an empty hog
                if (targetIndex == -1)
                {
                    targetIndex = datafile.NumLumps;
                }

                // If the insertion mark is to the right of the item with
                // the corresponding index, increment the target index.
                if (listView1.InsertionMark.AppearsAfterItem)
                {
                    targetIndex++;
                }

                string[] names = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string name in names)
                {
                    LoadLumpAt(name, targetIndex);
                    targetIndex++;
                }
            }
        }

        private void splitContainer1_Panel2_SizeChanged(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            txtPreview.Width = control.Width;
            txtPreview.Height = control.Height - txtPreview.Top;
            picPreview.Width = control.Width;
            picPreview.Height = control.Height;
            lvlPreview.Width = control.Width;
            lvlPreview.Height = control.Height;
        }

        private static Bitmap IndexedImageToBitmap(IIndexedImage img)
        {
            Bitmap bm;
            byte[] data = img.Data;
            int stride = img.Width;
            if ((stride & 3) != 0)
            {
                stride += 4 - (stride & 3);
                data = new byte[img.Height * stride];
                for (int y = 0; y < img.Height; y++)
                    Array.Copy(img.Data, y * img.Width, data, y * stride, img.Width);
            }
            unsafe
            {
                fixed (byte* ptr = data)
                    bm = new Bitmap(img.Width, img.Height, stride,
                            PixelFormat.Format8bppIndexed, new IntPtr(ptr));
            }
            var pal = bm.Palette;
            for (int i = 0; i < img.Palette.Length; i++)
            {
                var c = img.Palette[i];
                pal.Entries[i] = System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);
            }
            bm.Palette = pal;
            return bm;
        }

        private string LumpToText(int index)
        {
            switch (datafile.GetLumpHeader(index).Type)
            {
                case LumpType.EncodedText:
                    return TXBConverter.DecodeTXB(datafile.GetLumpData(index));

                case LumpType.Mission:
                case LumpType.Text:
                case LumpType.SongList:
                    var text = Encoding.GetEncoding("ISO-8859-1").GetString(datafile.GetLumpData(index));
                    if (text.EndsWith("\x1a"))
                        text = text.Substring(0, text.Length - 1);
                    return text;
            }
            return null;
        }

        private IIndexedImage LumpToIndexedImage(int index)
        {
            switch (datafile.GetLumpHeader(index).Type)
            {
                case LumpType.PCXImage:
                    var pcxImg = new PCXImage();
                    pcxImg.Read(datafile.GetLumpData(index));
                    return pcxImg;

                case LumpType.LBMImage:
                    var bbmImg = new BBMImage();
                    bbmImg.Read(datafile.GetLumpData(index));
                    return bbmImg;

                case LumpType.Palette:
                    var pal = new Palette(datafile.GetLumpData(index));
                    return PreviewGenerator.PaletteToImage(pal);

                case LumpType.Font:
                    var font = new LibDescent.Data.Font();
                    font.LoadFont(datafile.GetLumpAsStream(index));
                    return PreviewGenerator.FontToImage(font);
            }
            return null;
        }

        private ILevel LumpToLevel(int index)
        {
            var type = datafile.GetLumpHeader(index).Type;
            if (type != LumpType.Level) return null;
            string ext = Path.GetExtension(datafile.GetLumpHeader(index).Name);
            if (ext == ".rdl" || ext == ".sdl")
                return D1Level.CreateFromStream(datafile.GetLumpAsStream(index));
            else
                return D2Level.CreateFromStream(datafile.GetLumpAsStream(index));
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            int index = listView1.SelectedIndices.Count > 0 ? listView1.SelectedIndices[0] : -1;
            bool havePreview = false;

            string text;

            //Deal with bugs induced by listviews generating selected index change events in weird ways. 
            if (index >= datafile.Lumps.Count)
                return;

            if (index != -1 && (text = LumpToText(index)) != null)
            {
                CurrentTextName = datafile.GetLumpHeader(index).Name;
                CurrentTextEncoded = datafile.GetLumpHeader(index).Type == LumpType.EncodedText;
                CurrentTextLF = !text.Contains('\r');
                if (CurrentTextLF)
                    text = text.Replace("\n", "\r\n");
                txtPreview.Text = text;
                txtPreview.Visible = true;
                txtPreview.Modified = false;
                btnSave.Visible = true;
                btnSave.Enabled = false;
                havePreview = true;
            }
            else
            {
                txtPreview.Visible = false;
                btnSave.Visible = false;
            }

            IIndexedImage img;
            if (index != -1 && (img = LumpToIndexedImage(index)) != null)
            {
                picPreview.Image = IndexedImageToBitmap(img);
                picPreview.Visible = true;
                havePreview = true;
            }
            else
            {
                picPreview.Visible = false;
            }

            ILevel level;
            if (index != -1 && (level = LumpToLevel(index)) != null)
            {
                lvlPreview.Level = level;
                lvlPreview.Visible = true;
                havePreview = true;
            }
            else
            {
                lvlPreview.Visible = false;
            }

            lblPreviewPlaceholder.Visible = !havePreview;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string text = txtPreview.Text;
            if (CurrentTextLF)
                text = text.Replace("\r\n", "\n");
            byte[] data;
            if (CurrentTextEncoded)
                data = TXBConverter.EncodeTXB(text);
            else
                data = Encoding.GetEncoding("ISO-8859-1").GetBytes(text);

            var newLump = new HOGLump(CurrentTextName, data);

            var listItem = LumpToListItem(newLump);
            listItem.Selected = true;

            int index = datafile.GetLumpNum(newLump.Name);
            if (index == -1)
            {
                datafile.AddLump(newLump);
                listView1.Items.Add(listItem);
            }
            else
            {
                datafile.DeleteLump(index);
                datafile.AddLumpAt(newLump, index);
                listView1.Items[index] = listItem;
            }
            Modified = true;
        }

        private void txtPreview_ModifiedChanged(object sender, EventArgs e)
        {
            btnSave.Enabled = ((TextBox)sender).Modified;
        }

        private void HOGEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Modified)
                return;
            var res = MessageBox.Show("HOG file is modified, do you want to save the changes?",
                    "HOG Editor", MessageBoxButtons.YesNoCancel);
            if (res == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            if (res == DialogResult.Yes)
                SaveMenu_Click(null, null);
        }
    }
}
