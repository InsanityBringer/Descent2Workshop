using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Descent2Workshop.Transactions
{
    public enum ImageUpdateEventType
    {
        //A new image was added at all values of UpdateIndicies.
        Insert,
        //An image was deleted from all values of UpdateIndicies. All values are before any deletes are done.
        Delete,
        //An image was updated in any form at all values of UpdateIndicies.
        Update,
        //All values of UpdateIndicies were moved by MoveDisplacement cells. This must preserve relative ordering. 
        Move
    }
    //Arguments for an update to an image provider.
    public class ImageUpdateEventArgs : EventArgs
    {
        public ImageUpdateEventType UpdateType { get; set; }
        public List<int> UpdateIndices { get; } = new List<int>();
        //When UpdateType is Move, this will contain the displacement.
        public int MoveDisplacement { get; set; }
    }
}
