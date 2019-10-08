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

namespace LibDescent.Data
{
    public class EClip : HAMElement
    {
        public const int PropCritClip = 0;
        public const int PropDestVClip = 1;
        public const int PropDestEClip = 2;
        private static string[] TagNames = { "Critical clip", "Destroyed VClip", "Destroyed EClip" };
        public int ID; //Needed for convenience
        public VClip vc = new VClip();				//imbedded vclip
        public int time_left;		//for sequencing
        public int frame_count;	//for sequencing
        public short changing_wall_texture;			//Which element of Textures array to replace.
        public short changing_object_texture;		//Which element of ObjBitmapPtrs array to replace.
        public int flags;			//see above
        public int crit_clip;		//use this clip instead of above one when mine critical
        public int dest_bm_num;	//use this bitmap when monitor destroyed
        public int dest_vclip;		//what vclip to play when exploding
        public int dest_eclip;		//what eclip to play when exploding
        public int dest_size;		//3d size of explosion
        public int sound_num;		//what sound this makes
        public int segnum, sidenum;	//what seg & side, for one-shot clips

        public VClip destVClip;
        public EClip destEClip;
        public EClip critClip;

        public int DestEClipID { get { if (destEClip == null) return -1; return destEClip.ID; } }
        public int DestVClipID { get { if (destVClip == null) return -1; return destVClip.ID; } }
        public int CritClipID { get { if (critClip == null) return -1; return critClip.ID; } }

        public EClip()
        {
            dest_vclip = -1;
            dest_eclip = -1;
            crit_clip = -1;
            dest_bm_num = -1;
            sound_num = -1;
        }

        public void InitReferences(IElementManager manager)
        {
            destVClip = manager.GetVClip(dest_vclip);
            destEClip = manager.GetEClip(dest_eclip);
            critClip = manager.GetEClip(crit_clip);
        }

        public void AssignReferences(IElementManager manager)
        {
            if (destVClip != null) destVClip.AddReference(HAMType.EClip, this, PropDestVClip);
            if (destEClip != null) destEClip.AddReference(HAMType.EClip, this, PropDestEClip);
            if (critClip != null) critClip.AddReference(HAMType.EClip, this, PropCritClip);
        }

        public void ClearReferences()
        {
            if (destVClip != null) destVClip.ClearReference(HAMType.EClip, this, PropDestVClip);
            if (destEClip != null) destEClip.ClearReference(HAMType.EClip, this, PropDestEClip);
            if (critClip != null) critClip.ClearReference(HAMType.EClip, this, PropCritClip);
        }

        public int GetCurrentTMap()
        {
            foreach (HAMReference reference in References)
            {
                if (reference.Type == HAMType.TMAPInfo)
                    return ((TMAPInfo)reference.element).ID;
            }
            return -1;
        }

        public static string GetTagName(int tag)
        {
            return TagNames[tag];
        }
    }
}
