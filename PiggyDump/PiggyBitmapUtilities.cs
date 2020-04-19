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
                rgbData[i] = ((b == 255 ? 0 : 255) << 24) + (palette[b, 0] << 16) + (palette[b, 1] << 8) + (palette[b, 2]);
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
                rgbData[i] = ((b == 255 ? 0 : 255) << 24) + (palette[b, 0] << 16) + (palette[b, 1] << 8) + (palette[b, 2]);
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
                rgbData[i] = ((b == 255 ? 0 : 255) << 24) + (palette[b, 0] << 16) + (palette[b, 1] << 8) + (palette[b, 2]);
            }

            BitmapData bits = bitmap.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(rgbData, 0, bits.Scan0, image.Width * image.Height);
            bitmap.UnlockBits(bits);

            return bitmap;
        }

        public static void SetAverageColor(PIGImage image, Palette palette)
        {
            byte[] data = image.GetData();
            byte c;
            int totalr = 0, totalg = 0, totalb = 0;
            for (int i = 0; i < data.Length; i++)
            {
                c = data[i];
                totalr += palette[c, 0];
                totalg += palette[c, 1];
                totalb += palette[c, 2];
            }

            totalr /= data.Length;
            totalg /= data.Length;
            totalb /= data.Length;

            image.AverageIndex = (byte)palette.GetNearestColor(totalr, totalg, totalb);
        }

        public static PIGImage CreatePIGImage(Bitmap bitmap, Palette palette, string newname)
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
                    color = palette.GetNearestColor(basedata[i * 4 + 2], basedata[i * 4 + 1], basedata[i * 4]);
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
