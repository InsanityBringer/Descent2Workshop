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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LibDescent.Data;
using LibDescent.Edit;

namespace Descent2Workshop.Transactions
{
    public struct ChangedHAMReference
    {
        public object obj;
        public IList source;
        public int id;

        public ChangedHAMReference(object obj, IList source, int id)
        {
            this.obj = obj; this.source = source; this.id = id;
        }
    }

    public class DeleteTMAPInfoTransaction : Transaction
    {
        List<ChangedHAMReference> references = new List<ChangedHAMReference>();
        EditorHAMFile datafile;
        TMAPInfo lastValue;
        ushort lastTexture;
        int deleteNum;
        public DeleteTMAPInfoTransaction(EditorHAMFile hamfile, int num, int tab) : base("Delete texture", null, null, num, tab)
        {
            deleteNum = num;
            datafile = hamfile;
        }

        public override bool Apply()
        {
            lastTexture = datafile.Textures[deleteNum];
            lastValue = datafile.TMapInfo[deleteNum];
            RedoPage = Math.Max(0, Page - 1);

            datafile.Textures.RemoveAt(deleteNum);
            datafile.TMapInfo.RemoveAt(deleteNum);

            references.Clear();

            if (datafile.Textures.Count != 0)
            {
                //Change every reference to the deleted element
                for (int i = 0; i < datafile.EClips.Count(); i++)
                {
                    EClip temp = datafile.EClips[i].Clone();
                    //Change references that exactly reference the object to -1
                    if (datafile.EClips[i].ChangingObjectTexture == deleteNum)
                    {
                        datafile.EClips[i].ChangingWallTexture = -1;
                        references.Add(new ChangedHAMReference(temp, datafile.EClips, i));
                    }

                    //Change references to further elements by subtracting one
                    else if (datafile.EClips[i].ChangingObjectTexture > deleteNum)
                    {
                        datafile.EClips[i].ChangingWallTexture--;
                        references.Add(new ChangedHAMReference(temp, datafile.EClips, i));
                    }
                }

                for (int i = 0; i < datafile.TMapInfo.Count(); i++)
                {
                    TMAPInfo temp = datafile.TMapInfo[i].Clone();
                    //Change references that exactly reference the object to -1
                    if (datafile.TMapInfo[i].DestroyedID == deleteNum)
                    {
                        datafile.TMapInfo[i].DestroyedID = -1;
                        references.Add(new ChangedHAMReference(temp, datafile.TMapInfo, i));
                    }

                    //Change references to further elements by subtracting one
                    else if (datafile.TMapInfo[i].DestroyedID > deleteNum)
                    {
                        datafile.TMapInfo[i].DestroyedID--;
                        references.Add(new ChangedHAMReference(temp, datafile.TMapInfo, i));
                    }
                }

                for (int i = 0; i < datafile.WClips.Count(); i++)
                {
                    WClip temp = datafile.WClips[i].Clone();
                    bool change = false;
                    //Change references that exactly reference the object to -1
                    for (int j = 0; j < datafile.WClips[i].NumFrames; j++)
                    {
                        if (datafile.WClips[i].Frames[j] == deleteNum)
                        {
                            datafile.WClips[i].Frames[j] = 0;
                            change = true;
                        }

                        //Change references to further elements by subtracting one
                        else if (datafile.WClips[i].Frames[j] > deleteNum)
                        {
                            datafile.WClips[i].Frames[j]--;
                            change = true;
                        }
                    }

                    if (change)
                        references.Add(new ChangedHAMReference(temp, datafile.WClips, i));
                }
            }
            return true;
        }

        public override void Revert()
        {
            datafile.Textures.Insert(deleteNum, lastTexture);
            datafile.TMapInfo.Insert(deleteNum, lastValue);

            foreach (ChangedHAMReference reference in references)
            {
                reference.source[reference.id] = reference.obj;
            }
        }

        public override bool ChangesListSize()
        {
            return true;
        }
    }
}
