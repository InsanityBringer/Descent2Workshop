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
    public class TmapInfoEventArgs : EventArgs
    {
        public int Value { get; }
        public TmapInfoEventArgs(int value)
        {
            this.Value = value;
        }
    }
    public class TMapInfoTransaction : Transaction
    {
        private int oldValue;
        private int newValue;
        private int index;
        private EditorHAMFile datafile;
        private PIGFile pigfile;

        public EventHandler<TmapInfoEventArgs> eventHandler;
        public TMapInfoTransaction(string label, int page, int tab, EditorHAMFile datafile, PIGFile pigfile, int index, int newtexture) : base(label, null, "", page, tab)
        {
            newValue = Math.Max(0, Math.Min(pigfile.Bitmaps.Count-1, newtexture));
            this.index = index;
            this.datafile = datafile;
            this.pigfile = pigfile;
        }

        public override bool Apply()
        {
            oldValue = datafile.Textures[index];
            datafile.Textures[index] = (ushort)newValue;
            eventHandler?.Invoke(this, new TmapInfoEventArgs(newValue));

            return true;
        }

        public override void Revert()
        {
            datafile.Textures[index] = (ushort)oldValue;
        }
    }
}
