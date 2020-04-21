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
using System.Threading.Tasks;
using LibDescent.Data;
using LibDescent.Edit;

namespace Descent2Workshop.Transactions
{
    public class TMapInfoEClipTransaction : Transaction
    {
        private int tmapIndex;
        private int oldValue;
        private int newValue;

        private int oldTmapIndex = -1;
        private EditorHAMFile datafile;
        private PIGFile pigfile;

        public TMapInfoEClipTransaction(string label, int page, int tab, EditorHAMFile datafile, PIGFile pigfile, int index, int newEClip) : base(label, null, "", page, tab)
        {
            newValue = newEClip;
            tmapIndex = index;
            this.datafile = datafile;
            this.pigfile = pigfile;
        }

        public override bool Apply()
        {
            TMAPInfo info = datafile.TMapInfo[tmapIndex];
            oldValue = info.EClipNum;

            if (newValue != -1)
            {
                oldTmapIndex = datafile.EClips[newValue].ChangingWallTexture;
                if (oldTmapIndex != -1)
                    datafile.TMapInfo[oldTmapIndex].EClipNum = -1;
            }
            info.EClipNum = (short)newValue;
            datafile.EClips[newValue].ChangingWallTexture = (short)tmapIndex;


            return true;
        }

        public override void Revert()
        {
            TMAPInfo info = datafile.TMapInfo[tmapIndex];
            info.EClipNum = (short)oldValue;
            datafile.EClips[newValue].ChangingWallTexture = (short)oldTmapIndex;

            if (oldTmapIndex != -1)
            {
                datafile.TMapInfo[oldTmapIndex].EClipNum = (short)newValue;
            }
        }
    }
}
