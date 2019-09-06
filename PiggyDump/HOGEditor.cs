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
    public partial class HOGEditor : Form
    {
        private HOGFile mainFile;
        private StandardUI host;
        public HOGEditor(HOGFile data, StandardUI host)
        {
            InitializeComponent();
            mainFile = data;
            this.host = host;
        }

        private void HOGEditor_Load(object sender, EventArgs e)
        {
            for (int x = 0; x < mainFile.NumLumps; x++)
            {
                ListViewItem lumpElement = new ListViewItem(mainFile.GetLumpHeader(x).name);
                lumpElement.SubItems.Add(mainFile.GetLumpHeader(x).size.ToString());
                listView1.Items.Add(lumpElement);
            }
            string count = string.Format("Total Elements: {0}", mainFile.NumLumps);
            label1.Text = count; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                BinaryReader br;
                try
                {
                    br = new BinaryReader(File.Open(openFileDialog1.FileName, FileMode.Open));
                    string filename = openFileDialog1.SafeFileName;
                    if (filename.Length > 12)
                    {
                        //trim the filename
                        filename = filename.Remove(12);
                    }
                    int size = (int)br.BaseStream.Length;
                    byte[] data = br.ReadBytes((int)br.BaseStream.Length);
                    HOGLump newLump = new HOGLump(filename, size, -1);
                    newLump.data = data;
                    ListViewItem lumpElement = new ListViewItem(newLump.name);
                    lumpElement.SubItems.Add(newLump.size.ToString());
                    listView1.Items.Add(lumpElement);
                    mainFile.AddLump(newLump);
                }
                catch (Exception)
                {
                    MessageBox.Show("There was an error loading the lump!");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0)
            {
                return;
            }
            mainFile.DeleteLump(listView1.SelectedIndices[0]);
            listView1.Items.RemoveAt(listView1.SelectedIndices[0]);
        }

        //TODO WHAT THE EVERLOVING CRAP IS THIS DOING HERE
        /*private void WriteHogLump(string filename, HOGLump lump)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create));
            bw.Write(lump.data);
            bw.Flush();
            bw.Close();
        }*/

        private void button3_Click(object sender, EventArgs e)
        {
            /*if (listView1.SelectedIndices.Count == 0) return;
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
                        WriteHogLump(newpath, mainFile.lumps[index]);
                    }
                }
                else
                {
                    if (saveFileDialog1.FileName != "")
                    {
                        WriteHogLump(saveFileDialog1.FileName, mainFile.lumps[listView1.SelectedIndices[0]]);
                    }
                }
            }*/
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "HOG Files|*.HOG";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int err = mainFile.SaveDataFile(saveFileDialog1.FileName);
                if (err != 0)
                {
                    host.AppendConsole(FileUtilities.FileErrorCodeHandler(err, "write", "HOG file"));
                }
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("I should probably fix this, perhaps. Use Save As for now...");
        }
    }
}
