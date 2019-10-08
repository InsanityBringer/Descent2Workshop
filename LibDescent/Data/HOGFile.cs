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
using System.IO;

namespace LibDescent.Data
{
    public class HOGFile
    {
        private BinaryReader fileStream;
        private List<HOGLump> lumps = new List<HOGLump>();

        public int NumLumps { get { return lumps.Count; } }
        public string filename;
        
        public void LoadDataFile(string name)
        {
            BinaryReader br = new BinaryReader(File.Open(name, FileMode.Open, FileAccess.Read, FileShare.Read));
            fileStream = br;

            char[] header = new char[3];
            header[0] = (char)br.ReadByte();
            header[1] = (char)br.ReadByte();
            header[2] = (char)br.ReadByte();

            try
            {
                while (true)
                {
                    char[] filenamedata = new char[13];
                    bool hashitnull = false;
                    for (int x = 0; x < 13; x++)
                    {
                        char c = (char)br.ReadByte();
                        if (c == 0)
                        {
                            hashitnull = true;
                        }
                        if (!hashitnull)
                        {
                            filenamedata[x] = c;
                        }
                    }
                    string filename = new string(filenamedata);
                    filename = filename.Trim(' ', '\0');
                    int filesize = br.ReadInt32();
                    int offset = (int)br.BaseStream.Position;
                    br.BaseStream.Seek(filesize, SeekOrigin.Current); //I hate hog files. Wads are cooler..

                    HOGLump lump = new HOGLump(filename, filesize, offset);
                    lumps.Add(lump);
                }
            }
            catch (EndOfStreamException)
            {
                //we got all the files
                //heh
                //i love hog
                byte[] data;
                for (int i = 0; i < NumLumps; i++)
                {
                    data = GetLumpData(i);
                    lumps[i].type = HOGLump.IdentifyLump(lumps[i].name, data);
                }
                filename = name;
            }
        }

        public void SaveDataFile(string filename)
        {
            string tempFilename = Path.ChangeExtension(filename, ".newtmp");
            BinaryWriter bw = new BinaryWriter(File.Open(tempFilename, FileMode.Create));

            bw.Write((byte)'D');
            bw.Write((byte)'H');
            bw.Write((byte)'F');
            HOGLump lump;
            for (int i = 0; i < lumps.Count; i++)
            {
                lump = lumps[i];
                for (int c = 0; c < 13; c++)
                {
                    if (c < lump.name.Length)
                        bw.Write((byte)lump.name[c]);
                    else
                        bw.Write((byte)0);
                }
                bw.Write(lump.size);
                if (lump.offset == -1) //This lump has cached data
                    bw.Write(lump.data);
                else //This lump doesn't have cached data, and instead needs to be read from the old stream
                {
                    byte[] data = GetLumpData(i);
                    bw.Write(data);
                }
                lump.offset = (int)bw.BaseStream.Position - lump.size; //Update the offset for the new file
            }
            bw.Flush();
            bw.Close();
            bw.Dispose();

            //Dispose of the old stream, and open up the new file as the read stream
            fileStream.Close();
            fileStream.Dispose();

            if (File.Exists(filename))
            {
                try
                {
                    File.Delete(filename);
                }
                catch (Exception exc) //Can't delete the old file for whatever reason...
                {
                    File.Delete(tempFilename); //Delete the temp file then...
                    throw exc;
                }
            }
            File.Move(tempFilename, filename);

            fileStream = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read));
            this.filename = filename;
        }

        public int GetLumpNum(string filename)
        {
            //TODO: Dictionary lookup
            for (int i = 0; i < NumLumps; i++)
            {
                if (lumps[i].name.Equals(filename, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        public HOGLump GetLumpHeader(int id)
        {
            return lumps[id];
        }

        public byte[] GetLumpData(int id)
        {
            if (lumps[id].offset == -1)
                return lumps[id].data;

            fileStream.BaseStream.Seek(lumps[id].offset, SeekOrigin.Begin);
            return fileStream.ReadBytes(lumps[id].size);
        }

        public void AddLump(HOGLump lump)
        {
            lumps.Add(lump);
        }

        public void DeleteLump(int id)
        {
            lumps.RemoveAt(id);
        }
    }
}
