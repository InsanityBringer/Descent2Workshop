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
            Bitmap bitmap = new Bitmap(image.width, image.height);
            int[] rgbData = new int[image.width * image.height];
            byte[] rawData = image.GetData();
            byte b;

            for (int i = 0; i < rawData.Length; i++)
            {
                b = rawData[i];
                rgbData[i] = ((b == 255 ? 0 : 255) << 24) + (palette[b, 0] << 16) + (palette[b, 1] << 8) + (palette[b, 2]);
            }

            BitmapData bits = bitmap.LockBits(new Rectangle(0, 0, image.width, image.height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(rgbData, 0, bits.Scan0, image.width * image.height);
            bitmap.UnlockBits(bits);

            return bitmap;
        }

        public static Bitmap GetBitmap(PIGImage image, Palette palette)
        {
            Bitmap bitmap = new Bitmap(image.width, image.height);
            int[] rgbData = new int[image.width * image.height];
            byte[] rawData = image.GetData();
            byte b;

            for (int i = 0; i < rawData.Length; i++)
            {
                b = rawData[i];
                rgbData[i] = ((b == 255 ? 0 : 255) << 24) + (palette[b, 0] << 16) + (palette[b, 1] << 8) + (palette[b, 2]);
            }

            BitmapData bits = bitmap.LockBits(new Rectangle(0, 0, image.width, image.height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(rgbData, 0, bits.Scan0, image.width * image.height);
            bitmap.UnlockBits(bits);

            return bitmap;
        }

        public static Bitmap GetBitmap(PIGFile piggyFile, Palette palette, string name)
        {
            PIGImage image = piggyFile.GetImage(name);
            Bitmap bitmap = new Bitmap(image.width, image.height);
            int[] rgbData = new int[image.width * image.height];
            byte[] rawData = image.GetData();
            byte b;

            for (int i = 0; i < rawData.Length; i++)
            {
                b = rawData[i];
                rgbData[i] = ((b == 255 ? 0 : 255) << 24) + (palette[b, 0] << 16) + (palette[b, 1] << 8) + (palette[b, 2]);
            }

            BitmapData bits = bitmap.LockBits(new Rectangle(0, 0, image.width, image.height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            System.Runtime.InteropServices.Marshal.Copy(rgbData, 0, bits.Scan0, image.width * image.height);
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

            image.averageIndex = (byte)palette.GetNearestColor(totalr, totalg, totalb);
        }
    }
}
