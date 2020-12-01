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

        public override bool CanMergeWith(Transaction transaction)
        {
            if (!(transaction is IndexedUnsignedTransaction))
                return false;

            IndexedUnsignedTransaction other = (IndexedUnsignedTransaction)transaction;

            //In order to merge two transactions, the transactions must be operating on the same exact object and field. 
            //Indexed transactions need to share indices too
            if (other.target == target &&
                other.property == property &&
                other.index == index)
            {
                return true;
            }
            return false;
        }

        public override void MergeIn(Transaction transaction)
        {
            IndexedUnsignedTransaction other = (IndexedUnsignedTransaction)transaction;
            newValue = other.newValue;
        }
    }
}
