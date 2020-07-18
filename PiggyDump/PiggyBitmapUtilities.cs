using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            for (int i = 0; i < 255; i++)
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

        public static Bitmap GetBitmap(PIGFile piggyFile, Palette palette, int index)
        {
            PIGImage image = piggyFile.GetImage(index);
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

        public static Bitmap GetBitmap(PIGImage image, Palette palette)
        {
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
                if (basedata[i * 4 + 3] < 255)
                {
                    image.Transparent = true;
                    color = 255;
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
    }
}
