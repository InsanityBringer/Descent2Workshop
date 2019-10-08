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
using System.Numerics;

namespace LibDescent.Data
{
    public class PolymodelPlane
    {
        public FixVector point;
        public FixVector norm;

        public PolymodelPlane(FixVector point, FixVector norm)
        {
            this.point = point;
            this.norm = norm;
        }
    }
    public class PolymodelData
    {
        //int size;
        public byte[] InterpreterData;
        public FixVector[] points = new FixVector[1000];
        public List<PolymodelPlane> planeList = new List<PolymodelPlane>();
        public PolymodelData(int size)
        {
            InterpreterData = new byte[size];
        }

        public void GetSubmodelMinMaxs(int num, Polymodel host)
        {
            MemoryStream ms = new MemoryStream(InterpreterData);
            BinaryReader br = new BinaryReader(ms);
            br.BaseStream.Seek(host.submodels[num].Pointer, SeekOrigin.Begin);
            short opcode = br.ReadInt16();
            FixVector mins = new FixVector(int.MaxValue, int.MaxValue, int.MaxValue);
            FixVector maxs = new FixVector(int.MinValue, int.MinValue, int.MinValue);
            switch (opcode)
            {
                case 1:
                    {
                        short pointc = br.ReadInt16(); //+2
                                                       //data.points = new HAMFile.vms_vector[pointc];
                        for (int x = 0; x < pointc; x++)
                        {
                            FixVector point = new FixVector();
                            point.x = br.ReadInt32();
                            point.y = br.ReadInt32();
                            point.z = br.ReadInt32();

                            mins.x = Math.Min(mins.x, point.x);
                            mins.y = Math.Min(mins.y, point.y);
                            mins.z = Math.Min(mins.z, point.z);
                            maxs.x = Math.Max(maxs.x, point.x);
                            maxs.y = Math.Max(maxs.y, point.y);
                            maxs.z = Math.Max(maxs.z, point.z);
                        }
                    }
                    break;
                case 7:
                    {
                        short pointc = br.ReadInt16();
                        br.ReadInt32();
                        for (int x = 0; x < pointc; x++)
                        {
                            FixVector point = new FixVector();
                            point.x = br.ReadInt32();
                            point.y = br.ReadInt32();
                            point.z = br.ReadInt32();

                            mins.x = Math.Min(mins.x, point.x);
                            mins.y = Math.Min(mins.y, point.y);
                            mins.z = Math.Min(mins.z, point.z);
                            maxs.x = Math.Max(maxs.x, point.x);
                            maxs.y = Math.Max(maxs.y, point.y);
                            maxs.z = Math.Max(maxs.z, point.z);
                        }
                    }
                    break;
            }
            br.Close();
            br.Dispose();

            host.submodels[num].Mins = mins;
            host.submodels[num].Maxs = maxs;
        }

        public void BuildPolymodelData(int num, Polymodel host, string traceto)
        {
            MemoryStream ms = new MemoryStream(InterpreterData);
            BinaryReader br = new BinaryReader(ms);

            FixVector subobjvec = new FixVector();
            subobjvec.x = 0; subobjvec.y = 0; subobjvec.z = 0;

            StreamWriter sw = null;
            if (traceto != "")
            {
                sw = new StreamWriter(File.Open(traceto, FileMode.Create));
            }

            ExecuteInstruction(br, subobjvec, sw, host, 0);

            if (sw != null)
            {
                sw.Flush();
                sw.Close();
            }

            br.Close();
            //sr.Close();
            ms.Dispose();
        }

        int level = 0;

        private void WriteLevel(StreamWriter sw)
        {
            for (int i = 0; i < level; i++)
            {
                sw.Write(" ");
            }
        }

        public void ExecuteInstruction(BinaryReader br, FixVector positionOffset, StreamWriter sw, Polymodel host, int modelnum)
        {
            short opcode = br.ReadInt16();
            level++;
            Random r = new Random();

            while (opcode != 0)
            {
                //sr.WriteLine(String.Format("Reading opcode {0}!", opcode));
                switch (opcode)
                {
                    case 1:
                        {
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                Console.WriteLine("points at {0}", br.BaseStream.Position - 2);
                            }
                            short pointc = br.ReadInt16(); //+2
                            //data.points = new HAMFile.vms_vector[pointc];
                            for (int x = 0; x < pointc; x++)
                            {
                                FixVector point = new FixVector();
                                point.x = new Fix(br.ReadInt32());
                                point.y = new Fix(br.ReadInt32());
                                point.z = new Fix(br.ReadInt32());
                                points[x] = point;

                            }
                        }
                        break;
                    case 2:
                        {
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.WriteLine("flatpoly at {0}", br.BaseStream.Position - 2);
                                sw.Flush();
                            }
                            short pointc = br.ReadInt16(); //+2
                            FixVector vector = new FixVector();
                            vector.x = new Fix(br.ReadInt32());
                            vector.y = new Fix(br.ReadInt32());
                            vector.z = new Fix(br.ReadInt32());
                            FixVector normal = new FixVector();
                            normal.x = new Fix(br.ReadInt32());
                            normal.y = new Fix(br.ReadInt32());
                            normal.z = new Fix(br.ReadInt32());
                            short color = br.ReadInt16();
                            short[] pointindex = new short[pointc];
                            for (int x = 0; x < pointc; x++)
                            {
                                pointindex[x] = br.ReadInt16();
                            }
                            if (pointc % 2 == 0)
                            {
                                br.ReadInt16();
                            }
                            FixVector[] pointdata = new FixVector[pointc];
                            for (int x = 0; x < pointc; x++)
                            {
                                pointdata[x] = points[pointindex[x]];
                            }
                            PolymodelFace pmf = new PolymodelFace(pointc, vector, normal, color, pointdata);
                            host.submodels[modelnum].faces.Add(pmf);
                        }
                        break;
                    case 3:
                        {
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.Write("tmappoly at {0}, ", br.BaseStream.Position - 2);
                                sw.Flush();
                            }
                            short pointc = br.ReadInt16(); //+2
                            FixVector vector = new FixVector();
                            vector.x = new Fix(br.ReadInt32());
                            vector.y = new Fix(br.ReadInt32());
                            vector.z = new Fix(br.ReadInt32());
                            FixVector normal = new FixVector();
                            normal.x = new Fix(br.ReadInt32());
                            normal.y = new Fix(br.ReadInt32());
                            normal.z = new Fix(br.ReadInt32());
                            short texture = br.ReadInt16();
                            if (sw != null)
                            {
                                sw.WriteLine("numverts {1}, texture id {0}", /*host.textureList[texture]*/texture, pointc);
                                sw.Flush();
                            }
                            short[] pointindex = new short[pointc];
                            for (int x = 0; x < pointc; x++)
                            {
                                pointindex[x] = br.ReadInt16();
                            }
                            if (pointc % 2 == 0)
                            {
                                br.ReadInt16();
                            }
                            FixVector[] uvlcoordlist = new FixVector[pointc];
                            for (int x = 0; x < pointc; x++)
                            {
                                FixVector uvlcoords = new FixVector();
                                uvlcoords.x = new Fix(br.ReadInt32());
                                uvlcoords.y = new Fix(br.ReadInt32());
                                uvlcoords.z = new Fix(br.ReadInt32());
                                uvlcoordlist[x] = uvlcoords;
                            }
                            FixVector[] pointdata = new FixVector[pointc];
                            for (int x = 0; x < pointc; x++)
                            {
                                pointdata[x] = points[pointindex[x]];
                            }
                            PolymodelFace pmf = new PolymodelFace(pointc, vector, normal, texture, pointdata, uvlcoordlist);
                            host.submodels[modelnum].faces.Add(pmf);
                        }
                        break;
                    case 4:
                        {
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.WriteLine("sortnorm at {0}", br.BaseStream.Position - 2);
                            }
                            long pos = br.BaseStream.Position - 2;
                            short n_points = br.ReadInt16();
                            FixVector vector = new FixVector();
                            vector.x = new Fix(br.ReadInt32());
                            vector.y = new Fix(br.ReadInt32());
                            vector.z = new Fix(br.ReadInt32());
                            FixVector norm = new FixVector();
                            norm.x = new Fix(br.ReadInt32());
                            norm.y = new Fix(br.ReadInt32());
                            norm.z = new Fix(br.ReadInt32());
                            Vector3 fvector = new Vector3((float)vector.x / 65536f, (float)vector.y / 65536f, (float)vector.z / 65536f);
                            Vector3 fnorm = new Vector3((float)norm.x / 65536f, (float)norm.y / 65536f, (float)norm.z / 65536f);
                            short zFront = br.ReadInt16();
                            short zBack = br.ReadInt16();
                            PolymodelPlane plane = new PolymodelPlane(vector, norm);
                            this.planeList.Add(plane);

                            long returnPos = br.BaseStream.Position;

                            br.BaseStream.Seek((long)zBack + pos, SeekOrigin.Begin);
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.WriteLine("sortnorm entering front");
                            }
                            ExecuteInstruction(br, positionOffset, sw, host, modelnum);
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.WriteLine("return from front");
                            }
                            br.BaseStream.Seek(pos, SeekOrigin.Begin);
                            br.BaseStream.Seek((long)zFront + pos, SeekOrigin.Begin);
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.WriteLine("now entering back");
                            }
                            ExecuteInstruction(br, positionOffset, sw, host, modelnum);
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.WriteLine("return from back");
                            }

                            br.BaseStream.Seek(returnPos, SeekOrigin.Begin);
                        }
                        break;
                    case 5:
                        if (sw != null)
                        {
                            WriteLevel(sw);
                            sw.WriteLine("rodbm at {0}", br.BaseStream.Position - 2);
                        }
                        br.ReadBytes(34);
                        break;
                    case 6:
                        {
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.WriteLine("sobjcall at {0}", br.BaseStream.Position - 2);
                                sw.Flush();
                            }
                            long pos = br.BaseStream.Position - 2;
                            short objnum = br.ReadInt16();
                            FixVector offset = new FixVector();
                            offset.x = positionOffset.x + new Fix(br.ReadInt32());
                            offset.y = positionOffset.y + new Fix(br.ReadInt32());
                            offset.z = positionOffset.z + new Fix(br.ReadInt32());
                            ushort soffset = br.ReadUInt16();
                            br.ReadInt16();
                            long returnpos = br.BaseStream.Position;
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.WriteLine("sobjcall into {0} at {1}", objnum, pos + soffset);
                                sw.Flush();
                            }
                            br.BaseStream.Seek((long)soffset + pos, SeekOrigin.Begin);
                            ExecuteInstruction(br, offset, sw, host, objnum);
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.WriteLine("exited sobjcall into {0}", modelnum);
                                sw.Flush();
                            }
                            br.BaseStream.Seek(returnpos, SeekOrigin.Begin);
                            //subobjvec = offset;
                        }
                        break;
                    case 7:
                        {
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.Write("defpstart at {0} ", br.BaseStream.Position - 2);
                                sw.Flush();
                            }
                            short pointc = br.ReadInt16();
                            short formerpoints = br.ReadInt16();
                            if (sw != null)
                            {
                                sw.WriteLine("numpoints {0} offset {1}", pointc, formerpoints);
                                sw.Flush();
                            }
                            br.ReadInt16();
                            //data.points = new HAMFile.vms_vector[pointc];
                            for (int x = 0; x < pointc; x++)
                            {
                                FixVector point = new FixVector();
                                point.x = new Fix(br.ReadInt32());
                                point.y = new Fix(br.ReadInt32());
                                point.z = new Fix(br.ReadInt32());
                                points[x + formerpoints] = point;
                                //host.submodels[modelnum].points[x + formerpoints] = point;
                                if (sw != null)
                                {
                                    Vector3 vec = new Vector3(point.x + positionOffset.x, point.y + positionOffset.y, point.z + positionOffset.z);
                                    sw.WriteLine("point {0} len from origin {1}", x + formerpoints, vec.Length());
                                }
                            }
                        }
                        break;
                    case 8:
                        {
                            short glowval = br.ReadInt16();
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.WriteLine("glow at {0}, value {1}", br.BaseStream.Position - 2, glowval);
                            }
                        }
                        break;
                    case 9: //Wide SOBJCALL
                        {
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.WriteLine("wide sobjcall at {0}", br.BaseStream.Position - 2);
                                sw.Flush();
                            }
                            long pos = br.BaseStream.Position - 2;
                            short objnum = br.ReadInt16();
                            FixVector offset = new FixVector();
                            offset.x = positionOffset.x + new Fix(br.ReadInt32());
                            offset.y = positionOffset.y + new Fix(br.ReadInt32());
                            offset.z = positionOffset.z + new Fix(br.ReadInt32());
                            int soffset = br.ReadInt32();
                            long returnpos = br.BaseStream.Position;
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.WriteLine("wide sobjcall into {0} at {1}", objnum, pos + soffset);
                                sw.Flush();
                            }
                            br.BaseStream.Seek((long)soffset + pos, SeekOrigin.Begin);
                            ExecuteInstruction(br, offset, sw, host, objnum);
                            if (sw != null)
                            {
                                WriteLevel(sw);
                                sw.WriteLine("exited wide sobjcall into {0}", modelnum);
                                sw.Flush();
                            }
                            br.BaseStream.Seek(returnpos, SeekOrigin.Begin);
                            //subobjvec = offset;
                        }
                        break;
                }
                opcode = br.ReadInt16();
                if (opcode == 0 && sw != null)
                {
                    WriteLevel(sw);
                    sw.WriteLine("exit at {0}", br.BaseStream.Position - 2);
                }
            }
            level--;
        }
    }
}
