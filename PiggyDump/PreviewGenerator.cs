using LibDescent.Data;
using System;

namespace Descent2Workshop
{
    public static class PreviewGenerator
    {
        public static IIndexedImage PaletteToImage(Palette pal)
        {
            var img = new BBMImage(256, 256);
            for (int i = 0; i < 256; i++)
                img.Palette[i] = pal[i];
            for (int y = 0; y < 256; y++)
                for (int x = 0; x < 256; x++)
                    img.Data[y * 256 + x] = (byte)(((y / 16) * 16) + (x / 16));
            return img;
        }

        public static IIndexedImage FontToImage(Font font)
        {
            int dataHeight = font.Height;
            int charHeight = dataHeight + 1;

            // Calculate width
            var totalWidth = 0;
            foreach (var width in font.CharWidths)
                totalWidth += width;
            var area = totalWidth * charHeight;
            int imgWidth = (int)Math.Ceiling(Math.Sqrt(area));

            // Calculate height
            int lineWidth = 0, lines = 1;
            foreach (var w in font.CharWidths)
            {
                if (lineWidth + w > imgWidth)
                {
                    lineWidth = 0;
                    lines++;
                }
                lineWidth += w;
            }
            int imgHeight = lines * charHeight;

            // Create image
            var img = new BBMImage((short)imgWidth, (short)imgHeight);

            // Set palette
            if (font.Colored)
            {
                var pal = new Palette(font.Palette);
                for (int i = 0; i < 256; i++)
                    img.Palette[i] = pal[i];
            }
            else
            {
                img.Palette[0] = new LibDescent.Data.Color(255, 255, 255, 255);
                img.Palette[255] = new LibDescent.Data.Color(0, 0, 0, 0);
            }

            // Add characters
            int imgX = 0, imgY = 0;
            for (int i = 0; i < font.NumChars; i++)
            {
                int charWidth = font.CharWidths[i];
                int charPtr = font.CharPointers[i];
                if (imgX + charWidth > imgWidth)
                {
                    // Fill left over space
                    for (int cy = 0; cy < dataHeight; cy++)
                        for (int cx = 0; cx < imgWidth - imgX; cx++)
                            img.Data[(imgY + cy) * imgWidth + imgX + cx] = 255;
                    // Fill spacing line
                    for (int cy = dataHeight; cy < charHeight; cy++)
                        for (int cx = 0; cx < imgWidth; cx++)
                            img.Data[(imgY + cy) * imgWidth + cx] = 255;
                    imgX = 0;
                    imgY += charHeight;
                }
                if (font.Colored)
                {
                    for (int charY = 0; charY < dataHeight; charY++)
                        for (int charX = 0; charX < charWidth; charX++)
                            img.Data[(imgY + charY) * imgWidth + imgX + charX] =
                                font.FontData[charPtr + charY * charWidth + charX];
                }
                else
                {
                    int byteWidth = (charWidth + 7) / 8;
                    for (int charY = 0; charY < dataHeight; charY++)
                        for (int byteIdx = 0; byteIdx < byteWidth; byteIdx++)
                        {
                            byte b = font.FontData[charPtr + charY * byteWidth + byteIdx];
                            int charXLimit = Math.Min(byteIdx * 8 + 8, charWidth);
                            for (int charX = byteIdx * 8; charX < charXLimit; charX++, b <<= 1)
                                img.Data[(imgY + charY) * imgWidth + imgX + charX] =
                                    (b & 0x80) != 0 ? (byte)0 : (byte)255;

                        }
                }
                imgX += charWidth;
            }

            // Fill left over space
            for (int charY = 0; charY < dataHeight; charY++)
                for (int charX = 0; charX < imgWidth - imgX; charX++)
                    img.Data[(imgY + charY) * imgWidth + imgX + charX] = 255;
            // Fill spacing line(s)
            for (imgY += dataHeight; imgY < imgHeight; imgY++)
                for (int charX = 0; charX < imgWidth; charX++)
                    img.Data[imgY * imgWidth + charX] = 255;

            return img;
       }
    }
}
