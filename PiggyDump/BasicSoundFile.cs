using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LibDescent.Data;

namespace Descent2Workshop
{
    public class BasicSoundFile
    {
        public short FormatTag { get; set; }
        public short NumChannels { get; set; }
        public int SamplesPerSec { get; set; }
        public int AvgbytesPerSec { get; set; }
        public short BlockAlign { get; set; }
        public short BitsPerSample { get; set; }
        public byte[] Data { get; set; }

        private static byte[] Resample(byte[] olddata, int sourcebits, int sourceSampleRate, int destSampleRate)
        {
            double resampleRate = (double)sourceSampleRate / destSampleRate;
            int bytesPerSample = sourcebits == 16 ? 2 : 1;
            int length = (int)Math.Ceiling(olddata.Length / bytesPerSample / resampleRate);
            byte[] newdata = new byte[length];

            if (sourcebits == 16)
            {
                int sourceSampleCount = olddata.Length / 2;
                for (int i = 0; i < length; i++)
                {
                    int sourceCursor = (int)Math.Floor(i * resampleRate);
                    //Simple clamp to avoid oversampling
                    sourceCursor = Math.Min(sourceSampleCount - 1, sourceCursor);

                    int srcsample = BitConverter.ToInt16(olddata, sourceCursor * 2);
                    srcsample += 32768;
                    newdata[i] = (byte)(srcsample * 255 / 65535);
                }
            }
            else
            {
                int sourceSampleCount = olddata.Length;
                for (int i = 0; i < length; i++)
                {
                    int sourceCursor = (int)Math.Floor(i * resampleRate);
                    //Simple clamp to avoid oversampling
                    sourceCursor = Math.Min(sourceSampleCount - 1, sourceCursor);

                    newdata[i] = olddata[sourceCursor];
                }
            }

            return newdata;
        }

        public static BasicSoundFile ReadFromStream(Stream stream, int targetSampleRate)
        {
            BasicSoundFile sound = new BasicSoundFile();
            BinaryReader br = new BinaryReader(stream);

            uint containerSig = br.ReadUInt32();// = Util.MakeSig(
            if (containerSig != Util.MakeSig('R', 'I', 'F', 'F'))
                throw new InvalidDataException("File is not a RIFF container.");
            uint size = br.ReadUInt32();
            uint formatSig = br.ReadUInt32();
            if (formatSig != Util.MakeSig('W', 'A', 'V', 'E'))
                throw new InvalidDataException("File is not in WAVE format.");

            byte[] tempdata = null;

            while (br.BaseStream.Position < size)
            {
                uint sig = br.ReadUInt32();
                uint length = br.ReadUInt32();
                long position = br.BaseStream.Position;

                if (sig == Util.MakeSig('f', 'm', 't', ' '))
                {
                    //TODO: Remove the four billion limitations here, make more flexible, this is why I was considering naudio originally I think.....
                    if (length < 16)
                        throw new InvalidDataException("Insufficient information in fmt header");
                    sound.FormatTag = br.ReadInt16();
                    if (sound.FormatTag != 1)
                        throw new InvalidDataException("Unsupported encoding method, only uncompressed waves are currently supported.");
                    sound.NumChannels = br.ReadInt16();
                    if (sound.NumChannels != 1 && sound.NumChannels != 2)
                        throw new InvalidDataException("Only mono or stereo sounds are supported. Preferrably mono.");
                    sound.SamplesPerSec = br.ReadInt32();
                    sound.AvgbytesPerSec = br.ReadInt32();
                    sound.BlockAlign = br.ReadInt16();
                    sound.BitsPerSample = br.ReadInt16();
                    if (sound.BitsPerSample != 8 && sound.BitsPerSample != 16)
                        throw new InvalidDataException("Only 8 or 16 bit sounds are supported.");
                }
                else if (sig == Util.MakeSig('d', 'a', 't', 'a'))
                {
                    //Strip stereo data
                    if (sound.NumChannels != 1)
                    {
                        uint numBytes = length / (uint)sound.NumChannels;
                        tempdata = new byte[numBytes];
                        if (sound.BitsPerSample == 16)
                        {
                            uint numSamples = numBytes / 2;
                            for (uint i = 0; i < numSamples; i++)
                            {
                                tempdata[i * 2] = br.ReadByte();
                                tempdata[i * 2 + 1] = br.ReadByte();
                                br.ReadBytes((int)numBytes - 2);
                            }
                        }
                        else
                        {
                            for (uint i = 0; i < numBytes; i++)
                            {
                                tempdata[i] = br.ReadByte();
                                br.ReadBytes((int)numBytes - 1);
                            }
                        }
                    }
                    else
                    {
                        tempdata = br.ReadBytes((int)length);
                    }
                }

                br.BaseStream.Seek(position + length, SeekOrigin.Begin);
            }

            if (tempdata == null)
                throw new InvalidDataException("WAV file lacks a data section.");

            if (sound.BitsPerSample == 8 && sound.SamplesPerSec == targetSampleRate)
                sound.Data = tempdata;

            else
                sound.Data = Resample(tempdata, sound.BitsPerSample, sound.SamplesPerSec, targetSampleRate);

            return sound;
        }
    }
}
