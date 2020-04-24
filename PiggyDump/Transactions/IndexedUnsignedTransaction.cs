using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Descent2Workshop.Transactions
{
    public class UndoIndexedEventArgs : EventArgs
    {
        public int Index { get; }
        public UndoIndexedEventArgs(int index)
        {
            Index = index;
        }
    }
    public class IndexedUnsignedTransaction : Transaction
    {
        public uint index;
        public uint newValue;
        public uint oldValue;

        public EventHandler<UndoIndexedEventArgs> undoEvent;
        public IndexedUnsignedTransaction(string label, object target, string propertyName, int page, int tab, uint index, uint newValue) : base(label, target, propertyName, page, tab)
        {
            this.index = index;
            this.newValue = newValue;
        }

        public override bool Apply()
        {
            //okay so this is a dumb hack so we can generalize this mess
            if (property.PropertyType == typeof(ushort[]))
            {
                ushort[] array = (ushort[])property.GetValue(target);
                //Preserve the old value for undo purposes
                oldValue = array[index];
                //Set the new value
                array[index] = (ushort)newValue;
            }
            else if (property.PropertyType == typeof(byte[]))
            {
                byte[] array = (byte[])property.GetValue(target);
                //Preserve the old value for undo purposes
                oldValue = array[index];
                //Set the new value
                array[index] = (byte)newValue;
            }
            else
            {
                uint[] array = (uint[])property.GetValue(target);
                //Preserve the old value for undo purposes
                oldValue = array[index];
                //Set the new value
                array[index] = newValue;
            }
            undoEvent?.Invoke(this, new UndoIndexedEventArgs((int)index)); //this was poorly named since it should be done on redo too...
            return oldValue != newValue; //this is a little bad, but the results should not matter. Not that this is very ACID anyways...
        }

        public override void Revert()
        {
            //okay so this is a dumb hack so we can generalize this mess
            if (property.PropertyType == typeof(ushort[]))
            {
                ushort[] array = (ushort[])property.GetValue(target);
                array[index] = (ushort)oldValue;
            }
            else if (property.PropertyType == typeof(byte[]))
            {
                byte[] array = (byte[])property.GetValue(target);
                array[index] = (byte)oldValue;
            }
            else
            {
                uint[] array = (uint[])property.GetValue(target);
                array[index] = oldValue;
            }
            undoEvent?.Invoke(this, new UndoIndexedEventArgs((int)index));
        }
    }
}
