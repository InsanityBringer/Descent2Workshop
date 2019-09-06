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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace PiggyDump
{
    public class PIGImage
    {
        public const int BM_FLAG_TRANSPARENT = 1;
        public const int BM_FLAG_SUPER_TRANSPARENT = 2;
        public const int BM_FLAG_NO_LIGHTING = 4;
        public const int BM_FLAG_RLE = 8;
        public const int BM_FLAG_PAGED_OUT = 16;
        public const int BM_FLAG_RLE_BIG = 32;
        /// <summary>
        /// Name of the image in the Piggy archive
        /// </summary>
        public string name;
        /// <summary>
        /// Animation flags for the image
        /// </summary>
        public byte frameData;
        /// <summary>
        /// Base width of the image, before the extra bits are added
        /// </summary>
        public int baseWidth;
        /// <summary>
        /// Base height of the image, before the extra bits are added
        /// </summary>
        public int baseHeight;
        /// <summary>
        /// Final width of the image.
        /// </summary>
        public int width;
        /// <summary>
        /// Final height of the image.
        /// </summary>
        public int height; 
        /// <summary>
        /// Uncompressed pixel data.
        /// </summary>
        public byte[] data;
        /// <summary>
        /// Compressed pixel data.
        /// </summary>
        public byte[] compressedData;
        /// <summary>
        /// Sizes of each scanline for compressed images.
        /// </summary>
        public byte[] linesizes;
        /// <summary>
        /// Sizes of each scanline for big compressed images.
        /// </summary>
        public ushort[] linesizesl;
        /// <summary>
        /// Size of the image when compressed
        /// </summary>
        public int compressedSize;
        public int bytes_read;
        public float compressionratio;
        public byte flags;
        public byte averageIndex;
        public int offset;
        public int frame;
        public Palette paletteData;
        public byte extension;
        public bool isAnimated;
        const byte animdata = 1 | 2 | 4 | 8 | 16;
        public PIGImage(int mx, int my, byte framed, byte flag, byte average, int dataOffset, string imagename, byte extension)
        {
            baseWidth = mx; baseHeight = my; flags = flag; averageIndex = average; frameData = framed; offset = dataOffset; this.extension = extension;
            width = baseWidth + (((int)extension & 0x0f) << 8); height = baseHeight + (((int)extension & 0xf0) << 4);
            name = imagename;
            frame = ((int)frameData & (int)animdata);
            isAnimated = ((frameData & 64) != 0);
        }

        public void LoadData(BinaryReader br)
        {
            long curpos = br.BaseStream.Position;
            bytes_read = 0;
            br.BaseStream.Seek(offset, SeekOrigin.Current);
            if ((flags & BM_FLAG_RLE_BIG) != 0)
            {
                compressedSize = br.ReadInt32();
                bytes_read += 4;
                compressionratio = (float)compressedSize / (float)(width * height);
                compressedData = new byte[compressedSize - (height * 2) - 4];
                ushort[] locallinesizes = new ushort[height];
                for (int row = 0; row < height; row++)
                {
                    locallinesizes[row] = br.ReadUInt16();
                    bytes_read += 2;
                }
                linesizesl = locallinesizes;
                for (int pos = 0; pos < (compressedSize - 4 - (height * 2)); pos++)
                {
                    compressedData[pos] = br.ReadByte();
                    bytes_read++;
                }
                RLEEncoder RLEDecode = new RLEEncoder();
                data = RLEDecode.DecodeImage_big(compressedData, locallinesizes, compressedSize - 4, width, height);
            }
            else if ((flags & BM_FLAG_RLE) != 0)
            {
                compressedSize = br.ReadInt32();
                //MessageBox.Show(offset.ToString());
                bytes_read += 4;
                compressionratio = (float)compressedSize / (float)(width * height);
                //compressed_data = new byte[compressed_size * sizeof(byte) - sizeof(Int32)];
                compressedData = new byte[compressedSize - height - 4];
                linesizes = new byte[height];
                for (int row = 0; row < height; row++)
                {
                    linesizes[row] = br.ReadByte();
                    bytes_read++;
                }
                for (int pos = 0; pos < (compressedSize - 4 - height); pos++)
                {
                    try
                    {
                        compressedData[pos] = br.ReadByte();
                        bytes_read++;
                    }
                    catch (EndOfStreamException)
                    {
                        data = new byte[width * height];
                        return;
                    }
                }
                RLEEncoder RLEDecode = new RLEEncoder();
                data = RLEDecode.DecodeImage(compressedData, linesizes, compressedSize - 5, width, height);
            }
            else
            {
                compressionratio = 1;
                data = new byte[width *height];
                for (int cur = 0; cur < width * height; cur++)
                {
                    data[cur] = br.ReadByte();
                    bytes_read++;
                }
            }
            br.BaseStream.Seek(curpos, SeekOrigin.Begin);
            //return localdata;
        }

        public Bitmap GetPicture(Palette palette)
        {
            Bitmap image = new Bitmap(width, height);
            int[] rgbData = new int[width * height];

            //if ((flags & 8) == 0)
            {
                for (int curx = 0; curx < width; curx++)
                {
                    for (int cury = 0; cury < height; cury++)
                    {
                        int colorIndex = data[curx + cury * width];
                        byte r = palette.palette[colorIndex,0];
                        byte g = palette.palette[colorIndex,1];
                        byte b = palette.palette[colorIndex,2];
                        byte a = 255;
                        if (colorIndex == 255)
                        {
                            a = 0;
                        }
                        rgbData[curx + cury * width] = b + (g << 8) + (r << 16) + (a << 24);
                    }
                }
            }
            
            BitmapData bits = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(rgbData, 0, bits.Scan0, width * height);
            image.UnlockBits(bits);

            return image;
        }

        public void WriteImage(BinaryWriter bw)
        {
            if ((flags & BM_FLAG_RLE_BIG) != 0)
            {
                bw.Write(compressedSize);
                for (int r = 0; r < height; r++)
                {
                    bw.Write(linesizesl[r]);
                }
                bw.Write(compressedData);
            }
            else if ((flags & BM_FLAG_RLE) != 0)
            {
                bw.Write(compressedSize);
                for (int r = 0; r < height; r++)
                {
                    bw.Write(linesizes[r]);
                }
                bw.Write(compressedData);
            }
            else
            {
                for (int cy = 0; cy < baseHeight; cy++)
                {
                    for (int cx = 0; cx < width; cx++)
                    {
                        bw.Write(data[cx + cy * width]);
                    }
                }
            }
        }

        public void writeImageHeader(ref int doffset, BinaryWriter bw)
        {
            for (int sx = 0; sx < 8; sx++)
            {
                if (sx < name.Length)
                {
                    bw.Write((byte)name[sx]);
                }
                else
                {
                    bw.Write((byte)0);
                }
            }
            bw.Write(frameData);
            bw.Write((byte)baseWidth);
            bw.Write((byte)baseHeight);
            bw.Write(extension);
            bw.Write(flags);
            bw.Write(averageIndex);
            bw.Write(doffset);
            doffset += bytes_read;
        }
    }
}
