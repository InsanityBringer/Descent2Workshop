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

using OpenTK;

namespace PiggyDump
{
    public struct FixAngles
    {
        public short p, b, h;
    }

    public struct FixVector
    {
        public int x;
        public int y;
        public int z;

        public FixVector(int x, int y, int z)
        {
            this.x = x; this.y = y; this.z = z;
        }

        public override string ToString()
        {
            return string.Format("{0} ,{1}, {2}", x / 65536.0, y / 65536.0, z / 65536.0);
        }

        public void Add(FixVector other)
        {
            this.x += other.x; this.y += other.y; this.z += other.z;
        }

        public Vector3 GetVector3()
        {
            return new Vector3(x / 65536.0f, y / 65536f, z / 65536f);
        }
    }

    public struct FixMatrix
    {
        public FixVector forward;
        public FixVector up;
        public FixVector right;
    }
}
