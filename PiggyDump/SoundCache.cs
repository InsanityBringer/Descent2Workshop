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

namespace Descent2Workshop
{
    /// <summary>
    /// Super basic sound cache. Not actually a cache ATM, since it just hoards everything in memory.
    /// Used to simplify tool-side sound handling, and avoid cluttering LibDescent with unneded data.
    /// </summary>
    public class SoundCache
    {
        public List<byte[]> SoundDataCache { get; set; } = new List<byte[]>();

        public void CacheSound(byte[] data)
        {
            SoundDataCache.Add(data);
        }

        public byte[] GetSound(int num)
        {
            return SoundDataCache[num];
        }

        public void RemoveSoundID(int num)
        {
            SoundDataCache.RemoveAt(num);
        }

        /// <summary>
        /// Helper function to create a sound cache from a given data file.
        /// </summary>
        /// <param name="datafile">The datafile to load from. Must have a valid stream.</param>
        /// <returns>The sound cache with the sound data.</returns>
        public static SoundCache CreateCacheFromFile(SNDFile datafile)
        {
            SoundCache cache = new SoundCache();
            for (int i = 0; i < datafile.Sounds.Count; i++)
            {
                cache.CacheSound(datafile.LoadSound(i));
            }

            return cache;
        }
    }
}
