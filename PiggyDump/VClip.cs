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

namespace PiggyDump
{
    public class VClip : HAMElement
    {
        public int play_time; // fix
        public int num_frames;
        public int frame_time; // fix
        public int flags;
        public short sound_num;
        public ushort[] frames = new ushort[30];
        public int light_value;

        public int ID;

        public static string GetTagName(int tag)
        {
            return "";
        }

        public void RemapVClip(int firstFrame, PIGFile piggyFile)
        {
            int numFrames = 0;
            int nextFrame = 0;
            PIGImage img = piggyFile.images[firstFrame];
            if (img.isAnimated)
            {
                //Clear the old animation
                for (int i = 0; i < 30; i++) frames[i] = 0;

                frames[numFrames] = (ushort)(firstFrame + numFrames);
                img = piggyFile.images[firstFrame + numFrames + 1];
                numFrames++;
                while (img.frame == numFrames)
                {
                    frames[numFrames] = (ushort)(firstFrame + numFrames);
                    img = piggyFile.images[firstFrame + numFrames + 1];
                    numFrames++;
                    nextFrame++;
                }
                this.num_frames = numFrames-1;
            }
        }
    }
}
