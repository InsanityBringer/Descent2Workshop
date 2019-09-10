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
using System.Media;

namespace PiggyDump
{
    public partial class SXXEditor : Form
    {
        public SNDFile datafile;
        public StandardUI host;
        public bool isLowFi = false;
        public bool closeOnExit = true;
        public SXXEditor(StandardUI host, SNDFile datafile)
        {
            InitializeComponent();
            this.datafile = datafile;
            this.host = host;
            Text = string.Format("{0} - Sound Editor", datafile.filename);
        }

        private void SXXEditor_Load(object sender, EventArgs e)
        {
            SoundData sound;
            for (int i = 0; i < datafile.sounds.Count; i++)
            {
                sound = datafile.sounds[i];
                ListViewItem lvi = new ListViewItem(sound.name);
                lvi.SubItems.Add(sound.len.ToString());
                lvi.SubItems.Add(sound.offset.ToString());
                lvi.SubItems.Add(i.ToString());
                //int compressionPercentage = (int)(image.compressionratio * 100f);
                //lvi.SubItems.Add(compressionPercentage.ToString() + "%");
                listView1.Items.Add(lvi);
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            PlaySelected();
        }

        private void PlaySelected()
        {
            if (listView1.SelectedIndices.Count != 1) return;
            byte[] sndData = datafile.LoadSound(listView1.SelectedIndices[0]);
            byte[] buffer = new byte[sndData.Length * (isLowFi ? 8 : 4) + 44];

            WriteSound(new BinaryWriter(new MemoryStream(buffer, true)), sndData);

            using (MemoryStream memStream = new MemoryStream(buffer))
            using (SoundPlayer sp = new SoundPlayer(memStream))
            {
                sp.Load();
                sp.Play();
            };
        }

        private void WriteSoundToFile(string filename, byte[] data)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create));
            WriteSound(bw, data);
        }

        private void WriteSound(BinaryWriter bw, byte[] data)
        {
            bw.Write(0x46464952); //RIFF
            bw.Write(36 + data.Length);
            bw.Write(0x45564157);
            bw.Write(0x20746D66);
            bw.Write(16);
            bw.Write((short)1);
            bw.Write((short)1);
            if (isLowFi)
            {
                bw.Write(11025);
                bw.Write(11025);
            }
            else
            {
                bw.Write(22050);
                bw.Write(22050);
            }
            bw.Write((short)1);
            bw.Write((short)8);
            bw.Write(0x61746164);
            bw.Write(data.Length);
            bw.Write(data);
            bw.Flush();
            bw.Close();
        }

        private void SXXEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closeOnExit)
            {
                datafile.CloseDataFile();
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            PlaySelected();
        }

        private void ExtractMenu_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 0) return;
            saveFileDialog1.Filter = "WAV Files|*.wav";
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
                        string newpath = directory + Path.DirectorySeparatorChar + listView1.Items[index].Text + ".wav";
                        byte[] data = datafile.LoadSound(index);
                        WriteSoundToFile(newpath, data);
                    }
                }
                else
                {
                    if (saveFileDialog1.FileName != "")
                    {
                        byte[] data = datafile.LoadSound(listView1.SelectedIndices[0]);
                        WriteSoundToFile(saveFileDialog1.FileName, data);
                    }
                }
            }
        }
    }
}
