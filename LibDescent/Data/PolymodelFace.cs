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

namespace LibDescent.Data
{
    public class PolymodelFace
    {
        public int NumPoints;
        public FixVector FaceVector, Normal;
        public byte cr, cg, cb;
        public short textureID;
        public bool isTextured = false;
        public FixVector[] points;
        public FixVector[] UVLCoords;

        public PolymodelFace(int points, FixVector vec, FixVector norm, int colordata, FixVector[] pointdata)
        {
            NumPoints = points;
            FaceVector = vec; Normal = norm;

            cr = (byte)(((colordata >> 10) & 31) * 255 / 31);
            cg = (byte)(((colordata >> 5) & 31) * 255 / 31);
            cb = (byte)((colordata & 31) * 255 / 31);

            this.points = pointdata;
        }

        public PolymodelFace(int points, FixVector vec, FixVector norm, short texid, FixVector[] pointdata, FixVector[] UVLCoords)
        {
            NumPoints = points;
            FaceVector = vec; Normal = norm;

            textureID = texid; isTextured = true;

            this.points = pointdata;
            this.UVLCoords = UVLCoords;
        }
    }
}
