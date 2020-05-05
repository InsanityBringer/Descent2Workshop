using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibDescent.Data;

namespace Descent2Workshop.Transactions
{
    public class VClipRemapTransaction : Transaction
    {
        private VClip clip;
        private PIGFile piggyFile;
        private int frame;

        private ushort[] framebackup;
        private int oldframes;
        private Fix oldFrameTime;
        public VClipRemapTransaction(string label, int page, int tab, VClip clip, PIGFile piggyFile, int clipFirstFrame) : base(label, clip, "", page, tab)
        {
            this.clip = clip;
            this.piggyFile = piggyFile;
            this.frame = clipFirstFrame;

            framebackup = new ushort[clip.Frames.Length];
        }

        public override bool Apply()
        {
            for (int i = 0; i < framebackup.Length; i++)
            {
                framebackup[i] = clip.Frames[i];
            }
            oldframes = clip.NumFrames;
            oldFrameTime = clip.FrameTime;
            clip.RemapVClip(frame, piggyFile);
            return true;
        }

        public override void Revert()
        {
            clip.NumFrames = oldframes;
            clip.FrameTime = oldFrameTime;
            for (int i = 0; i < framebackup.Length; i++)
            {
                clip.Frames[i] = framebackup[i];
            }
        }
    }
}
