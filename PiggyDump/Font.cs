using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace PiggyDump
{
    public class Font
    {
        public const int FT_COLOR = 1;
        public const int FT_PROPORTIONAL = 2;
        public const int FT_KERNED = 4;

        public short width, height;
        public short flags;
        public short baseline;
        public byte firstChar, lastChar;
        public short byteWidth;
        public int numChars;

        public short[] charWidths = new short[256];
        public byte[] fontData;
        public int[] charPointers = new int[256];

        public byte[] palette = new byte[768];
        public byte[] colormap = new byte[256];

        public int LoadFont(string filename)
        {
            BinaryReader br = new BinaryReader(File.Open(filename, FileMode.Open));

            int sig = br.ReadInt32();
            int dataSize = br.ReadInt32();

            width = br.ReadInt16();
            height = br.ReadInt16();
            flags = br.ReadInt16();
            baseline = br.ReadInt16();
            firstChar = br.ReadByte();
            lastChar = br.ReadByte();
            byteWidth = br.ReadInt16();

            int dataPtr = br.ReadInt32();
            int charPtr = br.ReadInt32();
            int widthPtr = br.ReadInt32();
            int kernPtr = br.ReadInt32();

            numChars = lastChar - firstChar + 1;

            br.BaseStream.Seek(dataPtr + 8, SeekOrigin.Begin);

            int fontDataSize = 0;
            int pointer = 0;
            if ((flags & FT_PROPORTIONAL) != 0)
            {
                br.BaseStream.Seek(widthPtr + 8, SeekOrigin.Begin);
                for (int i = 0; i < numChars; i++)
                {
                    charWidths[i] = br.ReadInt16();
                    fontDataSize += height * charWidths[i];
                }
                br.BaseStream.Seek(dataPtr + 8, SeekOrigin.Begin);
                fontData = br.ReadBytes(fontDataSize);
                for (int i = 0; i < numChars; i++)
                {
                    charPointers[i] = pointer;
                    if ((flags & FT_COLOR) != 0)
                    {
                        pointer += height * charWidths[i];
                    }
                    else
                    {
                        pointer += (((charWidths[i]) + 7) >> 3) * height;
                    }
                }
            }
            else
            {
                fontDataSize = width * height * numChars;
                br.BaseStream.Seek(dataPtr + 8, SeekOrigin.Begin);
                fontData = br.ReadBytes(fontDataSize);
                for (int i = 0; i < numChars; i++)
                {
                    charWidths[i] = width; //Allow for lazy code down the line, I guess. 
                    charPointers[i] = pointer;
                    if ((flags & FT_COLOR) != 0)
                    {
                        pointer += height * charWidths[i];
                    }
                    else
                    {
                        pointer += (((charWidths[i]) + 7) >> 3) * height;
                    }
                }
            }

            if ((flags & FT_KERNED) != 0)
            {
                //todo
            }

            if ((flags & FT_COLOR) != 0)
            {
                br.BaseStream.Seek(8 + dataSize, SeekOrigin.Begin);
                palette = br.ReadBytes(768);
            }

            br.Close();
            br.Dispose();

            return 0;
        }

        public Bitmap GetCharacterBitmap(int charNum)
        {
            if (charNum >= numChars) return null;

            int charWidth = charWidths[charNum];
            int[] charData = new int[charWidth * height];

            Bitmap bitmap = new Bitmap(charWidth, height);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, charWidth, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            byte pixel;
            byte r, g, b, a;
            int offset = 0, bitmask = 0;
            if ((flags & FT_COLOR) != 0)
            {
                for (int i = 0; i < charWidth * height; i++)
                {
                    pixel = fontData[charPointers[charNum] + i];
                    r = (byte)(palette[pixel * 3 + 0] * 255 / 63);
                    g = (byte)(palette[pixel * 3 + 1] * 255 / 63);
                    b = (byte)(palette[pixel * 3 + 2] * 255 / 63);
                    a = 255;
                    charData[i] = b + (g << 8) + (r << 16) + (a << 24);
                }
            }
            else
            {
                for (int y = 0; y < height; y++)
                {
                    offset = (((width + 7) >> 3) * y);
                    bitmask = 0x80;
                    for (int x = 0; x < charWidth; x++)
                    {
                        if (bitmask == 0)
                        {
                            bitmask = 0x80;
                            offset++;
                        }
                        pixel = (byte)(fontData[charPointers[charNum] + offset] & (bitmask));
                        if (pixel != 0)
                        {
                            r = 0; g = 255; b = 0; a = 255;
                        }
                        else
                        {
                            r = g = b = 0;
                            a = 255;
                        }
                        bitmask >>= 1;
                        charData[y * charWidth + x] = b + (g << 8) + (r << 16) + (a << 24);
                    }
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(charData, 0, bitmapData.Scan0, charWidth * height);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }
}
