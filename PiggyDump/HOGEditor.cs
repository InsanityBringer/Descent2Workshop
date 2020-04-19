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
using LibDescent.Data;

namespace Descent2Workshop
{
    public partial class HOGEditor : Form
    {
        private HOGFile datafile;
        private StandardUI host;
        public HOGEditor(HOGFile data, StandardUI host)
        {
            InitializeComponent();
            datafile = data;
            this.host = host;
            this.Text = string.Format("{0} - Hog Editor", datafile.Filename);
        }

        private void HOGEditor_Load(object sender, EventArgs e)
        {
            byte[] data;
            for (int x = 0; x < datafile.NumLumps; x++)
            {
                ListViewItem lumpElement = new ListViewItem(datafile.GetLumpHeader(x).name);
                lumpElement.SubItems.Add(datafile.GetLumpHeader(x).size.ToString());
                lumpElement.SubItems.Add(datafile.GetLumpHeader(x).type.ToString());
                listView1.Items.Add(lumpElement);
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
            newLump.data = data;
            newLump.type = HOGLump.IdentifyLump(newLump.name, newLump.data);
            datafile.AddLump(newLump);

            ListViewItem lumpElement = new ListViewItem(newLump.name);
            lumpElement.SubItems.Add(newLump.size.ToString());
            lumpElement.SubItems.Add(newLump.type.ToString());
            if (index != -1)
            {
                listView1.Items.Insert(index, lumpElement);
                datafile.AddLumpAt(newLump, index);
            }
            else
            {
                listView1.Items.Add(lumpElement);
                datafile.AddLump(newLump);
            }
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
            Console.WriteLine("something entered the airlock");
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Console.WriteLine("okay, it's human");
                if ((e.AllowedEffect & DragDropEffects.Copy) != 0)
                {
                    Console.WriteLine("It can be copied");
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
                    if (targetPoint.X > itemBounds.Left + (itemBounds.Width / 2))
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
                if (targetIndex == -1)
                {
                    return;
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
    }
}
