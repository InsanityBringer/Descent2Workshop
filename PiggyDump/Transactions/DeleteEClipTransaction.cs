using LibDescent.Data;
using LibDescent.Edit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Descent2Workshop.Transactions
{
    public class DeleteEClipTransaction : Transaction
    {
        List<ChangedHAMReference> references = new List<ChangedHAMReference>();
        EditorHAMFile datafile;
        EClip lastValue;
        int deleteNum;

        public DeleteEClipTransaction(EditorHAMFile hamfile, int num, int tab) : base("Delete eclip", null, null, num, tab)
        {
            deleteNum = num;
            datafile = hamfile;
        }

        public override bool Apply()
        {
            lastValue = datafile.EClips[deleteNum];
            datafile.EClips.RemoveAt(deleteNum);

            //Resolve references
            references.Clear();
            if (datafile.TMapInfo.Count > 0)
            {
                for (int i = 0; i < datafile.TMapInfo.Count; i++)
                {
                    if (datafile.TMapInfo[i].EClipNum == deleteNum)
                    {
                        datafile.TMapInfo[i].EClipNum = -1;
                        references.Add(new ChangedHAMReference(datafile.TMapInfo[i].Clone(), datafile.TMapInfo, i));
                    }
                }
            }

            if (datafile.EClips.Count > 0)
            {
                for (int i = 0; i < datafile.EClips.Count; i++)
                {
                    if (datafile.EClips[i].ExplosionEClip == i)
                    {
                        datafile.EClips[i].ExplosionEClip = -1;
                        references.Add(new ChangedHAMReference(datafile.EClips[i].Clone(), datafile.EClips, i));
                    }
                }
            }

            return true;
        }

        public override void Revert()
        {
            //This needs to happen first in delete EClip transactions, since eclips self reference.
            foreach (ChangedHAMReference reference in references)
                reference.source[reference.id] = reference.obj;
            
            datafile.EClips.Insert(deleteNum, lastValue);
        }

        public override bool NeedsListUpdate() //must be done due to self-reference
        {
            return true;
        }
    }
}
