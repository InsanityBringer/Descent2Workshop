using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LibDescent.Data;
using LibDescent.Edit;

namespace Descent2Workshop.Transactions
{
    public struct ChangedTMAPReference
    {
        public int type;
        public int frameNum;
        public int num;
        public int oldValue;
    }

    public class DeleteTMAPInfoTransaction : Transaction
    {
        List<ChangedTMAPReference> references = new List<ChangedTMAPReference>();
        EditorHAMFile datafile;
        EditorHAMFile oldDatafile;
        TMAPInfo lastValue;
        ushort lastTexture;
        int deleteNum;
        public DeleteTMAPInfoTransaction(EditorHAMFile hamfile, object editor, string field, int num, int tab) : base("Delete texture", editor, field, num, tab)
        {
            deleteNum = num;
            datafile = hamfile;
        }

        public override bool Apply()
        {
            //laziness: Just back up the old datafile state. If the user undos, secretly replace the old ham file instance with the copy
            oldDatafile = datafile.Clone();

            lastTexture = datafile.Textures[deleteNum];
            lastValue = datafile.TMapInfo[deleteNum];

            datafile.Textures.RemoveAt(deleteNum);
            datafile.TMapInfo.RemoveAt(deleteNum);

            references.Clear();

            if (datafile.Textures.Count != 0)
            {
                //Change every reference to the deleted element
                for (int i = 0; i < datafile.EClips.Count(); i++)
                {
                    //Change references that exactly reference the object to -1
                    if (datafile.EClips[i].ChangingObjectTexture == deleteNum)
                    {
                        datafile.EClips[i].ChangingWallTexture = -1;
                    }

                    //Change references to further elements by subtracting one
                    else if (datafile.EClips[i].ChangingObjectTexture > deleteNum)
                    {
                        datafile.EClips[i].ChangingWallTexture--;
                    }
                }

                for (int i = 0; i < datafile.TMapInfo.Count(); i++)
                {
                    //Change references that exactly reference the object to -1
                    if (datafile.TMapInfo[i].DestroyedID == deleteNum)
                    {
                        datafile.TMapInfo[i].DestroyedID = -1;
                    }

                    //Change references to further elements by subtracting one
                    else if (datafile.TMapInfo[i].DestroyedID > deleteNum)
                    {
                        datafile.TMapInfo[i].DestroyedID--;
                    }
                }

                for (int i = 0; i < datafile.WClips.Count(); i++)
                {
                    //Change references that exactly reference the object to -1
                    for (int j = 0; j < datafile.WClips[i].NumFrames; j++)
                    {
                        if (datafile.WClips[i].Frames[j] == deleteNum)
                        {
                            datafile.WClips[i].Frames[j] = 0;
                        }

                        //Change references to further elements by subtracting one
                        else if (datafile.WClips[i].Frames[j] > deleteNum)
                        {
                            datafile.WClips[i].Frames[j]--;
                        }
                    }
                }
            }
            return true;
        }

        public override void Revert()
        {
            property.SetValue(target, oldDatafile);
        }

        public override bool ChangesListSize()
        {
            return true;
        }
    }
}
