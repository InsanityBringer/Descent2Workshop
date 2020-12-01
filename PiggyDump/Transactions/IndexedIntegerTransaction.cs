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
    public class IndexedIntegerTransaction : Transaction
    {
        public uint index;
        public int newValue;
        public int oldValue;

        public EventHandler<UndoIndexedEventArgs> undoEvent;
        public IndexedIntegerTransaction(string label, object target, string propertyName, int page, int tab, uint index, int newValue) : base(label, target, propertyName, page, tab)
        {
            this.index = index;
            this.newValue = newValue;
        }

        public override bool Apply()
        {
            //okay so this is a dumb hack so we can generalize this mess
            if (property.PropertyType == typeof(short[]))
            {
                short[] array = (short[])property.GetValue(target);
                //Preserve the old value for undo purposes
                oldValue = array[index];
                //Set the new value
                array[index] = (short)newValue;
            }
            else if (property.PropertyType == typeof(sbyte[]))
            {
                sbyte[] array = (sbyte[])property.GetValue(target);
                //Preserve the old value for undo purposes
                oldValue = array[index];
                //Set the new value
                array[index] = (sbyte)newValue;
            }
            else
            {
                int[] array = (int[])property.GetValue(target);
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
            if (property.PropertyType == typeof(short[]))
            {
                short[] array = (short[])property.GetValue(target);
                array[index] = (short)oldValue;
            }
            else if (property.PropertyType == typeof(sbyte[]))
            {
                sbyte[] array = (sbyte[])property.GetValue(target);
                array[index] = (sbyte)oldValue;
            }
            else
            {
                int[] array = (int[])property.GetValue(target);
                array[index] = oldValue;
            }
            undoEvent?.Invoke(this, new UndoIndexedEventArgs((int)index));
        }

        public override bool CanMergeWith(Transaction transaction)
        {
            if (!(transaction is IndexedIntegerTransaction))
                return false;

            IndexedIntegerTransaction other = (IndexedIntegerTransaction)transaction;

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
            IndexedIntegerTransaction other = (IndexedIntegerTransaction)transaction;
            newValue = other.newValue;
        }
    }
}
