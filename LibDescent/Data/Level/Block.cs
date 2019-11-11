using System;
using System.IO;

namespace LibDescent.Data
{
    public struct Block
    {
        public Segment[] Segments { get; }

        public static Block FromStream(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}