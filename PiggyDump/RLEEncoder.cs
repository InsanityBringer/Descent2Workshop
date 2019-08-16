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
using System.Windows.Forms;

namespace PiggyDump
{
    class RLEEncoder
    {
        byte[,] data;
        //int size;

        //byte isChainOfBytes = 128 | 64 | 32;
        //byte getRepeatCount = 1 | 2 | 4 | 8 | 16;

        public byte[] DecodeImage(byte[] input, byte[] linesizes, int size,int x, int y)
        {
            data = new byte[x, y];
            byte[] tempdata = new byte[x * y];
            byte curdata = 0;
            int position = 0;
            byte color = 0;
            int count; 
            int linelocation = 0;
            while (linelocation < (x * y))
            {
                curdata = input[position++];
                if (curdata == 0xE0)
                    continue;

                if (curdata > 0xE0)
                {
                    count = (byte)(curdata & 0x1F);
                    color = input[position++];
                    for (int temp = 0; temp < count; temp++)
                    {
                        tempdata[linelocation++] = color;
                    }
                }
                else
                {
                    tempdata[linelocation++] = curdata;
                }
            }
            /*linelocation = 0;
            for (int cury = 0; cury < y; cury++)
            {                
                for (int curx = 0; curx < x; curx++)
                {
                    data[curx, cury] = tempdata[linelocation];
                    linelocation++;
                }
            }*/
            return tempdata;
        }

        public byte[] DecodeImage_big(byte[] input, ushort[] linesizes, int size, int x, int y)
        {
            data = new byte[x, y];
            byte[] tempdata = new byte[x * y];
            byte curdata = 0;
            int position = 0;
            byte color = 0;
            int count;
            int linelocation = 0;
            while (linelocation < (x * y))
            {
                curdata = input[position++];
                if (curdata == 0xE0)
                    continue;

                if (curdata > 0xE0)
                {
                    count = (byte)(curdata & 0x1F);
                    color = input[position++];
                    for (int temp = 0; temp < count; temp++)
                    {
                        tempdata[linelocation++] = color;
                    }
                }
                else
                {
                    tempdata[linelocation++] = curdata;
                }
            }
            /*linelocation = 0;
            for (int cury = 0; cury < y; cury++)
            {
                for (int curx = 0; curx < x; curx++)
                {
                    data[curx, cury] = tempdata[linelocation];
                    linelocation++;
                }
            }*/
            return tempdata;
        }

        /*public byte[] EncodeImage(byte[] raw, int x)
        {
        }*/
    }
}
