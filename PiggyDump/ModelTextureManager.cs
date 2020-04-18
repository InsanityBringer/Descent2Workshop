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
using OpenTK.Graphics.OpenGL;
using LibDescent.Data;
using LibDescent.Edit;

namespace Descent2Workshop
{
    public class ModelTextureManager
    {

        public int LoadTexture(Bitmap bmp)
        {
            int id = OpenTK.Graphics.OpenGL.GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            bmp.Dispose();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            return id;
        }

        public List<int> LoadPolymodelTextures(Polymodel model, PIGFile pigFile, Palette palette, EditorHAMFile hamFile)
        {
            List<int> textureIDs = new List<int>();
            Bitmap image; EClip clip;
            foreach (string textureName in model.TextureList)
            {
                if (hamFile.EClipNameMapping.ContainsKey(textureName.ToLower()))
                {
                    clip = hamFile.EClipNameMapping[textureName.ToLower()];
                    image = PiggyBitmapUtilities.GetBitmap(pigFile, palette, clip.vc.Frames[0]);
                }
                else
                {
                    image = PiggyBitmapUtilities.GetBitmap(pigFile, palette, textureName);
                }
                textureIDs.Add(LoadTexture(image));
            }

            return textureIDs;
        }

        public List<int> LoadPolymodelTextures(Polymodel model, Palette palette, PIGFile pigFile)
        {
            List<int> textureIDs = new List<int>();
            Bitmap image;
            foreach (string textureName in model.TextureList)
            {
                image = PiggyBitmapUtilities.GetBitmap(pigFile, palette, textureName);
                textureIDs.Add(LoadTexture(image));
            }

            return textureIDs;
        }

        public List<int> LoadPolymodelTexturesHack(Polymodel model, PIGFile pigFile)
        {
            List<int> textureIDs = new List<int>();
            Bitmap image;
            image = (Bitmap)Image.FromFile("c:/dev/d2test/tex0.png");
            textureIDs.Add(LoadTexture(image));
            image.Dispose();
            image = (Bitmap)Image.FromFile("c:/dev/d2test/tex1.png");
            textureIDs.Add(LoadTexture(image));
            image.Dispose();

            return textureIDs;
        }

        public void FreeTextureList(List<int> textureList)
        {
            foreach (int textureID in textureList)
            {
                GL.DeleteTexture(textureID);
            }
        }
    }
}
