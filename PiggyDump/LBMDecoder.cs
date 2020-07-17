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
using System.Threading.Tasks;
using System.IO;

using LibDescent.Data;

namespace Descent2Workshop
{
    public class LBMDecoder
    {
        private void WriteInt16BE(BinaryWriter bw, short value)
        {
            byte low = (byte)(value & 0xFF);
            byte high = (byte)((value >> 8) & 0xFF);

            bw.Write(high);
            bw.Write(low);
        }

        private void WriteUInt16BE(BinaryWriter bw, ushort value)
        {
            byte low = (byte)(value & 0xFF);
            byte high = (byte)((value >> 8) & 0xFF);

            bw.Write(high);
            bw.Write(low);
        }

        private void WriteInt32BE(BinaryWriter bw, int value)
        {
            byte l1 = (byte)(value & 0xFF);
            byte l2 = (byte)((value >> 8) & 0xFF);
            byte h1 = (byte)((value >> 16) & 0xFF);
            byte h2 = (byte)((value >> 24) & 0xFF);

            bw.Write(h2);
            bw.Write(h1);
            bw.Write(l2);
            bw.Write(l1);
        }

        /*public void writePiggyImages(string directory, PIGFile piggyfile)
        {
            int w, h;
            byte[] image;
            int bytesWritten;
            string name;
            for (int i = 0; i < piggyfile.images.Count; i++)
            {
                bytesWritten = 0;
                if (!piggyfile.isABM((ushort)i))
                {
                    image = piggyfile.Bitmaps[i];
                    BinaryWriter bw = new BinaryWriter(File.Open(String.Format("{0}{1}.bbm", directory, piggyfile.images[i].name), FileMode.Create));
                    //just write the image
                    WriteBBM(image, 
                    bw.Flush();
                    bw.Close();
                    bw.Dispose();
                }
                else
                {
                    BinaryWriter bw = new BinaryWriter(File.Open(String.Format("{0}{1}.abm", directory, piggyfile.Bitmaps[i].Name), FileMode.Create));
                    name = piggyfile.Bitmaps[i].Name;
                    //write FORM header
                    bw.Write(0x4D524F46);
                    //will update later on
                    long loc = bw.BaseStream.Position;
                    bw.Write(0);
                    //Note it as a ANIM
                    //41 4E 49 4D
                    bw.Write(0x4D494E41);
                    bytesWritten += 4;
                    //write the first frame
                    bytesWritten += WriteBBM(w, h, image, bw, (piggyfile.images[i].flags & 1) != 0);
                    while (i < piggyfile.images.Count - 1 && piggyfile.images[i + 1].name == name)
                    {
                        i++;
                        image = piggyfile.loadImage(i);
                        w = piggyfile.images[i].vx;
                        h = piggyfile.images[i].vy;
                        //write animation
                        bytesWritten += writeABMFrame(w, h, image, bw);
                        //increment frame
                    }
                    bw.BaseStream.Seek(loc, SeekOrigin.Begin);
                    writeInt32BE(bw, bytesWritten);
                    bw.Flush();
                    bw.Close();
                    bw.Dispose();
                }
            }
        }*/

        public int CompressData(int width, int height, byte[] input, out byte[] output)
        {
            output = new byte[input.Length];
            int bytesWritten = 0;

            int lastByte;
            int repeatCount = 0;
            int unrepeatCount = 0;
            int unrepeatStart = 0;

            int position = 0;

            byte b = input[0];
            lastByte = b;

            int offset;
            try
            {
                for (int y = 0; y < height; y++)
                {
                    offset = y * width;
                    for (int x = 0; x < width; x++)
                    {
                        b = input[offset + x];
                        if ((x + 1) < width && input[offset + x + 1] == b)
                        {
                            repeatCount = 0;
                            while ((x + 1) < width && input[offset + x + 1] == b)
                            {
                                x++;
                                repeatCount++;
                                if (repeatCount >= 127) break; //Too much data for a span
                                if (x >= width) break; //no more data
                            }
                            unchecked
                            {
                                //Write the header
                                output[position++] = (byte)-repeatCount;
                                output[position++] = b;
                                repeatCount = 0;
                                //i--;
                            }
                        }
                        else
                        {
                            unrepeatCount = 0;
                            unrepeatStart = x;
                            while ((x + 1) < width && input[offset + x] != input[offset + x + 1])
                            {
                                x++;
                                unrepeatCount++;
                                if (unrepeatCount >= 127) break; //Too much data for a span
                                if (x >= input.Length) break; //no more data
                            }

                            //Write the header
                            output[position++] = (byte)unrepeatCount;
                            for (int j = 0; j < unrepeatCount + 1; j++)
                            {
                                output[position++] = input[offset + unrepeatStart + j];
                            }
                            unrepeatCount = 0;
                            //i--;
                        }
                    }
                }
            }
            catch (Exception) //probably wrote too much data
            {
                //Console.Error.WriteLine("LBMDecoder::CompressData: too much compressed data");
                return 0;
            }

            return position;
        }

        public void WriteABM(PIGImage[] frames, int numFrames, Palette palette, BinaryWriter bw)
        {
            int bytesWritten = 0;
            PIGImage image = frames[0];
            bool transparent = false;

            for (int i = 0; i < numFrames; i++)
            {
                transparent |= frames[i].Transparent;
            }

            //write FORM header
            bw.Write(0x4D524F46);
            //will update later on
            long loc = bw.BaseStream.Position;
            bw.Write(0);
            //Note it as a ANIM
            //41 4E 49 4D
            bw.Write(0x4D494E41);
            bytesWritten += 4;
            //write the first frame
            bytesWritten += WriteBBM(image, palette, transparent, bw);
            for (int i = 1; i < numFrames; i++)
            {
                //write animation
                bytesWritten += WriteABMFrame(frames[i], palette, bw);
            }
            bw.BaseStream.Seek(loc, SeekOrigin.Begin);
            WriteInt32BE(bw, bytesWritten);
            bw.Flush();
        }

        public int WriteABMFrame(PIGImage image, Palette palette, BinaryWriter bw)
        {
            int bytesWritten = 0;
            byte[] data = image.GetData();

            //write FORM header
            bw.Write(0x4D524F46);
            //will update later on
            long loc = bw.BaseStream.Position;
            bw.Write(0);
            //Note it as a PBM
            bw.Write(0x204D4250);
            bytesWritten += 4;

            //Write the ANHD header
            //41 4E 48 44
            bw.Write(0x44484E41);
            WriteInt32BE(bw, 40);
            bytesWritten += 8;
            //delta mode
            bw.Write((byte)0);
            //mask
            bw.Write((byte)0);
            //wh for xor mode
            WriteInt16BE(bw, (short)image.Width);
            WriteInt16BE(bw, (short)image.Height);
            //xy for xor mode
            WriteInt16BE(bw, 0);
            WriteInt16BE(bw, 0);
            //abstime
            WriteInt32BE(bw, 1);
            //reltime
            WriteInt32BE(bw, 1);
            //interleave
            bw.Write((byte)0);
            //pad
            bw.Write((byte)0);
            //flags
            WriteInt32BE(bw, 0);
            //pad
            bw.Write(new byte[16]);
            bytesWritten += 40;

            //Write the body
            //42 4F 44 59
            int pixelcount = image.Width * image.Height;
            bool alignmentHack = false;
            //LBMs require scanlines to be padded to even byte counts. 
            if (image.Width % 2 != 0)
            {
                pixelcount = (image.Width + 1) * image.Height;
                alignmentHack = true;
            }
            bw.Write(0x59444F42);
            WriteInt32BE(bw, pixelcount);
            bytesWritten += 8;
            if (alignmentHack)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        bw.Write(data[y * image.Width + x]);
                    }
                    bw.Write((byte)0);
                }
            }
            else
            {
                for (int i = 0; i < pixelcount; i++)
                {
                    bw.Write(data[i]);
                }
            }
            bytesWritten += pixelcount;

            long end = bw.BaseStream.Position;
            bw.BaseStream.Seek(loc, SeekOrigin.Begin);
            WriteInt32BE(bw, bytesWritten);
            bw.BaseStream.Seek(end, SeekOrigin.Begin);

            return bytesWritten + 8;
        }

        public int WriteBBM(PIGImage image, Palette palette, BinaryWriter bw)
        {
            int bytesWritten = 0;
            byte[] data = image.GetData();
            byte[] compressedData;
            int resultantBytes = CompressData(image.Width, image.Height, data, out compressedData);

            //write FORM header
            bw.Write(0x4D524F46);
            //will update later on
            long loc = bw.BaseStream.Position;
            bw.Write(0);
            //Note it as a PBM
            bw.Write(0x204D4250);
            bytesWritten += 4;

            //Write the BITMAP header
            bw.Write(0x44484D42);// 42 4D 48 44
            WriteInt32BE(bw, 0x14);
            bytesWritten += 8;

            //resolution
            WriteUInt16BE(bw, (ushort)image.Width);
            WriteUInt16BE(bw, (ushort)image.Height);
            //offset
            WriteUInt16BE(bw, 0);
            WriteUInt16BE(bw, 0);
            //num planes
            bw.Write((byte)8);
            //mask mode
            if (image.Transparent)
                bw.Write((byte)2);
            else
                bw.Write((byte)0);
            /*if (resultantBytes == 0)
            {*/
                //no compression
                bw.Write((byte)0);
            /*}
            else
            {
                //rle compression
                bw.Write((byte)1);
            }*/
            //pad
            bw.Write((byte)0);
            //transparent index
            WriteUInt16BE(bw, 255);
            //aspect ratio
            bw.Write((byte)5);
            bw.Write((byte)6);
            //screen size
            WriteUInt16BE(bw, 320);
            WriteUInt16BE(bw, 200);
            bytesWritten += 0x14;

            //Write the palette
            //43 4D 41 50
            bw.Write(0x50414D43);
            WriteInt32BE(bw, 0x300);
            bytesWritten += 8;
            for (int c = 0; c < 256; c++)
            {
                bw.Write((byte)palette[c].R);
                bw.Write((byte)palette[c].G);
                bw.Write((byte)palette[c].B);
            }
            bytesWritten += 0x300;
            //Write the body
            //42 4F 44 59
            int pixelcount = image.Width * image.Height;
            //if (resultantBytes == 0)
            {
                bool alignmentHack = false;
                //To this day I still have no clue what this hack is supposed to be for,
                //but the original game's IFF decoder seems to require it. It causes problems in DPaint, 
                //which confuses me. 
                if (image.Width % 2 != 0)
                {
                    pixelcount = (image.Width + 1) * image.Height;
                    alignmentHack = true;
                }
                bw.Write(0x59444F42);
                WriteInt32BE(bw, pixelcount);
                bytesWritten += 8;
                if (alignmentHack)
                {
                    for (int y = 0; y < image.Height; y++)
                    {
                        for (int x = 0; x < image.Width; x++)
                        {
                            bw.Write(data[y * image.Width + x]);
                        }
                        bw.Write((byte)0);
                    }
                }
                else
                {
                    for (int i = 0; i < pixelcount; i++)
                    {
                        bw.Write(data[i]);
                    }
                }
                bytesWritten += pixelcount;
            }
            /*else
            {
                bw.Write(0x59444F42);
                WriteInt32BE(bw, resultantBytes);
                bytesWritten += 8;
                for (int j = 0; j < resultantBytes; j++)
                {
                    bw.Write(compressedData[j]);
                }
                bytesWritten += resultantBytes;
            }*/

            long end = bw.BaseStream.Position;
            bw.BaseStream.Seek(loc, SeekOrigin.Begin);
            WriteInt32BE(bw, bytesWritten);
            bw.BaseStream.Seek(end, SeekOrigin.Begin);

            return bytesWritten + 8;
        }

        public int WriteBBM(PIGImage image, Palette palette, bool overrideAlpha, BinaryWriter bw)
        {
            int bytesWritten = 0;
            byte[] data = image.GetData();

            //write FORM header
            bw.Write(0x4D524F46);
            //will update later on
            long loc = bw.BaseStream.Position;
            bw.Write(0);
            //Note it as a PBM
            bw.Write(0x204D4250);
            bytesWritten += 4;

            //Write the BITMAP header
            bw.Write(0x44484D42);// 42 4D 48 44
            WriteInt32BE(bw, 0x14);
            bytesWritten += 8;

            //resolution
            WriteUInt16BE(bw, (ushort)image.Width);
            WriteUInt16BE(bw, (ushort)image.Height);
            //offset
            WriteUInt16BE(bw, 0);
            WriteUInt16BE(bw, 0);
            //num planes
            bw.Write((byte)8);
            //mask mode
            if (overrideAlpha)
                bw.Write((byte)2);
            else
                bw.Write((byte)0);
            //no compression
            bw.Write((byte)0);
            //pad
            bw.Write((byte)0);
            //transparent index
            WriteUInt16BE(bw, 255);
            //aspect ratio
            bw.Write((byte)5);
            bw.Write((byte)6);
            //screen size
            WriteUInt16BE(bw, 320);
            WriteUInt16BE(bw, 200);
            bytesWritten += 0x14;

            //Write the palette
            //43 4D 41 50
            bw.Write(0x50414D43);
            WriteInt32BE(bw, 0x300);
            bytesWritten += 8;
            for (int c = 0; c < 256; c++)
            {
                bw.Write((byte)palette[c].R);
                bw.Write((byte)palette[c].G);
                bw.Write((byte)palette[c].B);
            }
            bytesWritten += 0x300;
            //Write the body
            //42 4F 44 59
            int pixelcount = image.Width * image.Height;
            bool alignmentHack = false;
            //LBMs require scanlines to be padded to even byte counts. 
            if (image.Width % 2 != 0)
            {
                pixelcount = (image.Width + 1) * image.Height;
                alignmentHack = true;
            }
            bw.Write(0x59444F42);
            WriteInt32BE(bw, pixelcount);
            bytesWritten += 8;
            if (alignmentHack)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        bw.Write(data[y * image.Width + x]);
                    }
                    bw.Write((byte)0);
                }
            }
            else
            {
                for (int i = 0; i < pixelcount; i++)
                {
                    bw.Write(data[i]);
                }
            }
            bytesWritten += pixelcount;

            long end = bw.BaseStream.Position;
            bw.BaseStream.Seek(loc, SeekOrigin.Begin);
            WriteInt32BE(bw, bytesWritten);
            bw.BaseStream.Seek(end, SeekOrigin.Begin);

            return bytesWritten + 8;
        }
    }
}
