using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                        ChangedTMAPReference reference;
                        reference.type = 0;
                        reference.frameNum = -1;
                        reference.num = i;
                        reference.oldValue = datafile.EClips[i].ChangingWallTexture;
                        references.Add(reference);

                        datafile.EClips[i].ChangingWallTexture = -1;
                    }

                    //Change references to further elements by subtracting one
                    else if (datafile.EClips[i].ChangingObjectTexture > deleteNum)
                    {
                        ChangedTMAPReference reference;
                        reference.type = 0;
                        reference.frameNum = -1;
                        reference.num = i;
                        reference.oldValue = datafile.EClips[i].ChangingWallTexture;
                        references.Add(reference);

                        datafile.EClips[i].ChangingWallTexture--;
                    }
                }

                for (int i = 0; i < datafile.TMapInfo.Count(); i++)
                {
                    //Change references that exactly reference the object to -1
                    if (datafile.TMapInfo[i].DestroyedID == deleteNum)
                    {
                        ChangedTMAPReference reference;
                        reference.type = 1;
                        reference.frameNum = -1;
                        reference.num = i;
                        reference.oldValue = datafile.TMapInfo[i].DestroyedID;
                        references.Add(reference);

                        datafile.TMapInfo[i].DestroyedID = -1;
                    }

                    //Change references to further elements by subtracting one
                    else if (datafile.TMapInfo[i].DestroyedID > deleteNum)
                    {
                        ChangedTMAPReference reference;
                        reference.type = 1;
                        reference.frameNum = -1;
                        reference.num = i;
                        reference.oldValue = datafile.TMapInfo[i].DestroyedID;
                        references.Add(reference);

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
                            ChangedTMAPReference reference;
                            reference.type = 2;
                            reference.frameNum = j;
                            reference.num = i;
                            reference.oldValue = datafile.WClips[i].Frames[j];
                            references.Add(reference);

                            datafile.WClips[i].Frames[j] = 0;
                        }

                        //Change references to further elements by subtracting one
                        else if (datafile.WClips[i].Frames[j] > deleteNum)
                        {
                            ChangedTMAPReference reference;
                            reference.type = 2;
                            reference.frameNum = j;
                            reference.num = i;
                            reference.oldValue = datafile.WClips[i].Frames[j];
                            references.Add(reference);

                            datafile.WClips[i].Frames[j]--;
                        }
                    }
                }
            }
            return true;
        }

        public override void Revert()
        {
            datafile.Textures.Insert(deleteNum, lastTexture);
            datafile.TMapInfo.Insert(deleteNum, lastValue);

            foreach (ChangedTMAPReference reference in references)
            {
                switch (reference.type)
                {
                    case 0:
                        datafile.EClips[reference.num].ChangingWallTexture = (short)reference.oldValue;
                        break;
                    case 1:
                        datafile.TMapInfo[reference.num].DestroyedID = (short)reference.oldValue;
                        break;
                    case 2:
                        datafile.WClips[reference.num].Frames[reference.frameNum] = (ushort)reference.oldValue;
                        break;
                }
            }
        }

        public override bool ChangesListSize()
        {
            return true;
        }
    }
}
