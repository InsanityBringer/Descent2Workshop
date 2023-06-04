using System;
using System.Drawing;
using System.Drawing.Imaging;
using LibDescent.Data;

namespace Descent2Workshop
{
    public class PiggyBitmapUtilities
    {
        //duplicated from Palette for performance reasons. 
        public static int GetNearestColorIndex(int R, int G, int B, byte[] palette)
        {
            int bestcolor = 0;
            int bestdist = int.MaxValue;
            int dist;

            for (int i = 0; i < 254; i++)
            {
                int lR = palette[i * 3];
                int lG = palette[i * 3 + 1];
                int lB = palette[i * 3 + 2];
                if ((lR == R) && (lG == G) && (lB == B)) return i;
                dist = (R - lR) * (R - lR) + (G - lG) * (G - lG) + (B - lB) * (B - lB);
                if (dist == 0) 
                    return i;
                else if (dist < bestdist)
                {
                    bestcolor = i;
                    bestdist = dist;
                }
            }
            return bestcolor;
        }

        public static byte[] BuildInverseColormap(byte[] palette)
        {
            byte[] invCmap = new byte[64 * 64 * 64];

            int lr, lg, lb;
            for (int r = 0; r < 64; r++)
            {
                for (int g = 0; g < 64; g++)
                {
                    for (int b = 0; b < 64; b++)
                    {
                        lr = r * 255 / 63;
                        lg = g * 255 / 63;
                        lb = b * 255 / 63;

                        invCmap[r * 4096 + g * 64 + b] = (byte)GetNearestColorIndex(lr, lg, lb, palette);
                    }
                }
            }

            return invCmap;
        }

        public static Bitmap GetBitmap(IImageProvider piggyFile, Palette palette, int index, int scale = 1)
        {
            int[] rgbTable = new int[256];
            if (index > piggyFile.Bitmaps.Count)
                index = 0; //If out of bounds, show the bogus image
            PIGImage image = piggyFile.Bitmaps[index];

            int newWidth = (int)(image.Width * scale);
            int newHeight = (int)(image.Height * scale);
            Bitmap bitmap = new Bitmap(newWidth, newHeight);

            int alpha = 255;
            for (int j = 0; j < 256; j++)
            {
                if (j == 254)
                    alpha = 128;
                else if (j == 255)
                    alpha = 0;

                rgbTable[j] = (alpha << 24) + (palette[j].R << 16) + (palette[j].G << 8) + (palette[j].B);
            }

            int[] rgbData = new int[newWidth * newHeight];
            byte[] rawData = image.GetData();

            switch (scale)
            {
                case 1:
                    Scale1X(image.Width, image.Height, newWidth, newHeight, rawData, rgbData, rgbTable);
                    break;
                case 2:
                    Scale2X(image.Width, image.Height, newWidth, newHeight, rawData, rgbData, rgbTable);
                    break;
                case 3:
                    Scale3X(image.Width, image.Height, newWidth, newHeight, rawData, rgbData, rgbTable);
                    break;
                case 4:
                    Scale4X(image.Width, image.Height, newWidth, newHeight, rawData, rgbData, rgbTable);
                    break;
            }

            BitmapData bits = bitmap.LockBits(new Rectangle(0, 0, newWidth, newHeight), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(rgbData, 0, bits.Scan0, newWidth * newHeight);
            bitmap.UnlockBits(bits);

            return bitmap;
        }

        public static Bitmap GetBitmap(PIGImage image, Palette palette)
        {
            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            int[] rgbData = new int[image.Width * image.Height];
            byte[] rawData = image.GetData();
            byte b;

            int alpha = 255;
            for (int i = 0; i < rawData.Length; i++)
            {
                b = rawData[i];
                if (b == 254)
                    alpha = 128;
                else if (b == 255)
                    alpha = 0;

                rgbData[i] = (alpha << 24) + (palette[b].R << 16) + (palette[b].G << 8) + (palette[b].B);
            }

            BitmapData bits = bitmap.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(rgbData, 0, bits.Scan0, image.Width * image.Height);
            bitmap.UnlockBits(bits);

            return bitmap;
        }

        public static Bitmap GetBitmap(PIGFile piggyFile, Palette palette, string name)
        {
            PIGImage image = piggyFile.GetImage(name);
            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            int[] rgbData = new int[image.Width * image.Height];
            byte[] rawData = image.GetData();
            byte b;

            for (int i = 0; i < rawData.Length; i++)
            {
                b = rawData[i];
                rgbData[i] = ((b == 255 ? 0 : 255) << 24) + (palette[b].R << 16) + (palette[b].G << 8) + (palette[b].B);
            }

            BitmapData bits = bitmap.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(rgbData, 0, bits.Scan0, image.Width * image.Height);
            bitmap.UnlockBits(bits);

            return bitmap;
        }

        public static void SetAverageColor(PIGImage image, byte[] palette)
        {
            byte[] data = image.GetData();
            int c;
            int totalr = 0, totalg = 0, totalb = 0;
            for (int i = 0; i < data.Length; i++)
            {
                c = data[i]*3;
                totalr += palette[c];
                totalg += palette[c+1];
                totalb += palette[c+2];
            }

            totalr /= data.Length;
            totalg /= data.Length;
            totalb /= data.Length;

            image.AverageIndex = (byte)GetNearestColorIndex(totalr, totalg, totalb, palette);
        }

        public static PIGImage CreatePIGImage(Bitmap bitmap, byte[] palette, byte[] invCmap, string newname)
        {
            if (bitmap.Width >= 4096 || bitmap.Height >= 4096) throw new Exception("Bitmap resolution is too high for a PIG bitmap");
            PIGImage image = new PIGImage(bitmap.Width, bitmap.Height, 0, 0, 0, 0, newname);
            byte[] data = new byte[image.Width * image.Height];
            byte[] basedata = new byte[image.Width * image.Height * 4];

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, basedata, 0, basedata.Length);
            bitmap.UnlockBits(bitmapData);
            //todo: i dunno it probably can be optimzied
            int color;
            for (int i = 0; i < data.Length; i++)
            {
                if (basedata[i * 4 + 3] < 85)
                {
                    image.Transparent = true;
                    color = 255;
                }
                else if (basedata[i * 4 + 3] < 169)
                {
                    image.SuperTransparent = true;
                    color = 254;
                }
                else
                {
                    color = invCmap[(basedata[i * 4 + 2] >> 2) * 4096 + (basedata[i * 4 + 1] >> 2) * 64 + (basedata[i * 4] >> 2)];
                }
                data[i] = (byte)color;
            }

            image.Data = data;
            SetAverageColor(image, palette);
            //immediately try to compress if at all possible
            try
            {
                image.RLECompressed = true;
            }
            catch (Exception) { } //it didn't work
            return image;
        }

        //Unrolled scaling loops. These should be faster than a generic algorithim, I hope. Integer factors only.
        //It would be nice if I could do this easier in System.Drawing, am I missing something obvious?
        private static void Scale1X(int w, int h, int newW, int newH, byte[] src, int[] dest, int[] rgbTable)
        {
            int len = w * h;
            for (int i = 0; i < len; i++)
            {
                dest[i] = rgbTable[src[i]];
            }
        }

        private static void Scale2X(int w, int h, int newW, int newH, byte[] src, int[] dest, int[] rgbTable)
        {
            int len = newW * newH;
            int offset = 0;
            int i = 0;
            int c;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    c = rgbTable[src[offset + x]];
                    dest[i] = c;
                    dest[i + 1] = c;
                    dest[i + newW] = c;
                    dest[i + newW + 1] = c;
                    i += 2;
                }
                offset += w;
                i += newW;
            }
        }

        private static void Scale3X(int w, int h, int newW, int newH, byte[] src, int[] dest, int[] rgbTable)
        {
            int len = newW * newH;
            int offset = 0;
            int i = 0;
            int c;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    c = rgbTable[src[offset + x]];
                    dest[i] = c;
                    dest[i + 1] = c;
                    dest[i + 2] = c;
                    dest[i + newW] = c;
                    dest[i + newW + 1] = c;
                    dest[i + newW + 2] = c;
                    dest[i + newW * 2] = c;
                    dest[i + newW * 2 + 1] = c;
                    dest[i + newW * 2 + 2] = c;
                    i += 3;
                }
                offset += w;
                i += newW * 2;
            }
        }

        private static void Scale4X(int w, int h, int newW, int newH, byte[] src, int[] dest, int[] rgbTable)
        {
            int len = newW * newH;
            int offset = 0;
            int i = 0;
            int c;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    c = rgbTable[src[offset + x]];
                    dest[i] = c;
                    dest[i + 1] = c;
                    dest[i + 2] = c;
                    dest[i + 3] = c;
                    dest[i + newW] = c;
                    dest[i + newW + 1] = c;
                    dest[i + newW + 2] = c;
                    dest[i + newW + 3] = c;
                    dest[i + newW * 2] = c;
                    dest[i + newW * 2 + 1] = c;
                    dest[i + newW * 2 + 2] = c;
                    dest[i + newW * 2 + 3] = c;
                    dest[i + newW * 3] = c;
                    dest[i + newW * 3 + 1] = c;
                    dest[i + newW * 3 + 2] = c;
                    dest[i + newW * 3 + 3] = c;
                    i += 4;
                }
                offset += w;
                i += newW * 3;
            }
        }
    }
}
