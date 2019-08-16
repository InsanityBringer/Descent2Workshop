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
using System.IO;

namespace PiggyDump
{
    public class HOGFile
    {
        public List<HOGLump> lumpList = new List<HOGLump>();
        
        public void LoadDataFile(string name)
        {
            BinaryReader br = new BinaryReader(File.Open(name, FileMode.Open));

            char[] header = new char[3];
            header[0] = (char)br.ReadByte();
            header[1] = (char)br.ReadByte();
            header[2] = (char)br.ReadByte();

            try
            {
                while (true)
                {
                    char[] filenamedata = new char[13];
                    bool hashitnull = false;
                    for (int x = 0; x < 13; x++)
                    {
                        char c = (char)br.ReadByte();
                        if (c == 0)
                        {
                            hashitnull = true;
                        }
                        if (!hashitnull)
                        {
                            filenamedata[x] = c;
                        }
                    }
                    string filename = new string(filenamedata);
                    filename = filename.Trim(' ', '\0');
                    int filesize = br.ReadInt32();
                    byte[] data = br.ReadBytes(filesize);

                    HOGLump lump = new HOGLump(filename, filesize, data);
                    lumpList.Add(lump);
                }
            }
            catch (EndOfStreamException)
            {
                //we got all the files
                //heh
            }

            br.Close();
        }

        public void SaveDataFile(string name)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(name, FileMode.Create));
            bw.Write((byte)'D');
            bw.Write((byte)'H');
            bw.Write((byte)'F');
            HOGLump lump;
            for (int i = 0; i < lumpList.Count; i++)
            {
                lump = lumpList[i];
                for (int c = 0; c < 13; c++)
                {
                    if (c < lump.name.Length)
                        bw.Write((byte)lump.name[c]);
                    else
                        bw.Write((byte)0);
                }
                bw.Write(lump.size);
                bw.Write(lump.data);
            }
            bw.Flush();
            bw.Close();
            bw.Dispose();
            
        }

        public Palette LookUpPalette(string name)
        {
            for (int x = 0; x < lumpList.Count; x++)
            {
                if (name.Equals(((HOGLump)lumpList[x]).name, StringComparison.OrdinalIgnoreCase))
                {
                    HOGLump lump = (HOGLump)lumpList[x];
                    Palette pal = new Palette();
                    int index = 0;
                    for (int i = 0; i < 256; i++)
                    {
                        for (int c = 0; c < 3; c++)
                        {
                            pal.palette[i, c] = (byte)(lump.data[index] * 4);
                            index++;
                        }
                    }
                    return pal;
                }
            }
            return Palette.defaultPalette;
        }
    }
}
