/*
    Copyright (c) 2021 SaladBadger

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

using LibDescent.Data;

namespace Descent2Workshop.Transactions
{
    public class ReplaceSoundTransaction : Transaction
    {
        private SNDFile sndFile;
        private SoundCache cache;
        private int id;
        private string name;
        private byte[] data;
        public ReplaceSoundTransaction(SNDFile sndFile, SoundCache cache, int id, string name, byte[] data) : base("Replace sound")
        {
            this.sndFile = sndFile;
            this.id = id;
            this.name = name;
            this.data = data;
            this.cache = cache;
        }
        public override bool Apply()
        {
            SoundData sound = new SoundData();
            sound.Name = name;
            sound.Length = data.Length;
            sound.Data = data;

            sndFile.Sounds[id] = sound;
            //TODO this was the worst idea I ever had
            cache.CacheSoundAt(data, id);

            return true;
        }

        public override bool CanMergeWith(Transaction transaction)
        {
            return false; //can never merge
        }

        public override object GetOldValue()
        {
            //Used to denote where to remove from the list view. 
            return id;
        }

        public override void MergeIn(Transaction transaction)
        {
            throw new InvalidOperationException();
        }

        public override void Revert()
        {
            base.Revert();
        }
    }
}
