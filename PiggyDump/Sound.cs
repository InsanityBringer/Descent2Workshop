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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Descent2Workshop
{
    public class Sound
    {
        public char[] name = new char[8];
        public int length, datalength;
        public int offset;
        public byte[] sound_data;

        public ListViewItem getData()
        {
            ListViewItem lvi = new ListViewItem(new String(name));
            lvi.SubItems.Add(length.ToString());
            lvi.SubItems.Add(offset.ToString());

            return lvi;
        }

        public void LoadData(BinaryReader br)
        {
            long curpos = br.BaseStream.Position;
            br.BaseStream.Seek(offset, SeekOrigin.Current);
            //sound_data = new byte[datalength];
            sound_data = br.ReadBytes(datalength);
            br.BaseStream.Seek(curpos, SeekOrigin.Begin);
        }

        public void write_sound_header(int sndoffset, BinaryWriter bw)
        {
            for (int x = 0; x < 8; x++)
            {
                bw.Write((byte)name[x]);
            }
            bw.Write(length);
            bw.Write(datalength);
            bw.Write(sndoffset);
        }
    }
}
