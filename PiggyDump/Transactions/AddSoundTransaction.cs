using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibDescent.Data;

namespace Descent2Workshop.Transactions
{
    //honestly starting to think I should have named transactions "operations" or something
    public class AddSoundTransaction : Transaction
    {
        private SNDFile sndFile;
        private SoundCache cache;
        private int id;
        private string name;
        private byte[] data;
        public AddSoundTransaction(SNDFile sndFile, SoundCache cache, int id, string name, byte[] data) : base("Add sound")
        {
            this.sndFile = sndFile;
            this.id = id;
            this.name = name;
            this.data = data;
            this.cache = cache;
        }
        public override bool Apply()
        {
            SoundData sound = new SoundData();
            sound.Name = name;
            sound.Length = data.Length;
            sound.Data = data;

            sndFile.Sounds.Add(sound);
            cache.CacheSound(data);

            return true;
        }

        public override bool CanMergeWith(Transaction transaction)
        {
            return false; //can never merge
        }

        public override object GetOldValue()
        {
            return 0;
        }

        public override void MergeIn(Transaction transaction)
        {
            throw new InvalidOperationException();
        }

        public override void Revert()
        {
            base.Revert();
        }
    }
}
