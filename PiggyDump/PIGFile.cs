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
using System.Drawing;

namespace PiggyDump
{
    public class PIGFile
    {
        public List<ImageData> images = new List<ImageData>();
        public long startptr = 0L;
        private int header, version;
        private Palette palette;
        public Palette PiggyPalette { get { return palette; } }
        public PIGFile(Palette palette)
        {
            this.palette = palette;
            //Init a bogus texture for all piggyfiles
            ImageData bogusTexture = new ImageData(64, 64, 0, 0, 0, 0, "bogus", 0);
            bogusTexture.data = new byte[64 * 64];
            //Create an X using descent 1 palette indicies. For accuracy. Heh
            for (int i = 0; i < 4096; i++)
            {
                bogusTexture.data[i] = 85;
            }
            for (int i = 0; i < 64; i++)
            {
                bogusTexture.data[i * 64 + i] = 193;
                bogusTexture.data[i * 64 + (63 - i)] = 193;
            }
            images.Add(bogusTexture);
        }

        public void LoadDataFile(string name)
        {
            BinaryReader br = new BinaryReader(File.Open(name, FileMode.Open));

            header = br.ReadInt32();
            version = br.ReadInt32();
            int textureCount = br.ReadInt32();

            for (int x = 0; x < textureCount; x++)
            {
                bool hashitnull = false;
                char[] localname = new char[8];
                for (int i = 0; i < 8; i++)
                {
                    char c = (char)br.ReadByte();
                    if (c == 0)
                        hashitnull = true;
                    if (!hashitnull)
                        localname[i] = c;
                }
                string imagename = new String(localname);
                imagename = imagename.Trim(' ', '\0');
                byte framedata = br.ReadByte();
                byte lx = br.ReadByte();
                byte ly = br.ReadByte();
                byte extension = br.ReadByte();
                byte flag = br.ReadByte();
                byte average = br.ReadByte();
                int offset = br.ReadInt32();

                ImageData image = new ImageData(lx, ly, framedata, flag, average, offset, imagename, extension);
                images.Add(image);
            }
            startptr = br.BaseStream.Position;

            for (int i = 1; i < images.Count; i++)
            {
                br.BaseStream.Seek(startptr, SeekOrigin.Begin);
                images[i].LoadData(br);
            }
            
            br.Close();
        }

        public void SaveDataFile(string name)
        {
            BinaryWriter bw = new BinaryWriter(File.Open(name, FileMode.Create));
            int offset = 0;
            bw.Write(header);
            bw.Write(version);
            bw.Write(images.Count-1);
            for (int i = 1; i < images.Count; i++)
            {
                images[i].writeImageHeader(ref offset, bw);
            }
            for (int i = 1; i < images.Count; i++)
            {
                images[i].WriteImage(bw);
            }
            bw.Flush();
            bw.Close();
        }

        public Bitmap GetBitmap(int id)
        {
            ImageData image = (ImageData)images[id];
            return image.GetPicture(palette);
        }

        public Bitmap GetBitmap(string name)
        {
            for (int x = 0; x < images.Count; x++)
            {
                //todo: Dictionary
                if (images[x].name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return GetBitmap(x);
                }
            }
            return GetBitmap(0);
        }

        public int GetBitmapIDFromName(string name)
        {
            for (int x = 0; x < images.Count; x++)
            {
                if (images[x].name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return x;
                }
            }
            return 0;
        }
    }
}
