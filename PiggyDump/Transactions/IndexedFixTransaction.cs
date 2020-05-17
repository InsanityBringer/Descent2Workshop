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
using LibDescent.Data;

namespace Descent2Workshop.Transactions
{
    public class IndexedFixTransaction : Transaction
    {
        public uint index;
        public Fix newValue;
        public Fix oldValue;

        public EventHandler<UndoIndexedEventArgs> undoEvent;
        public IndexedFixTransaction(string label, object target, string propertyName, int page, int tab, uint index, Fix newValue) : base(label, target, propertyName, page, tab)
        {
            this.index = index;
            this.newValue = newValue;
        }

        public override bool Apply()
        {
            Fix[] array = (Fix[])property.GetValue(target);
            //Preserve the old value for undo purposes
            oldValue = array[index];
            //Set the new value
            array[index] = newValue;

            undoEvent?.Invoke(this, new UndoIndexedEventArgs((int)index)); //this was poorly named since it should be done on redo too...
            return oldValue != newValue; //this is a little bad, but the results should not matter. Not that this is very ACID anyways...
        }

        public override void Revert()
        {
            Fix[] array = (Fix[])property.GetValue(target);
            array[index] = oldValue;

            undoEvent?.Invoke(this, new UndoIndexedEventArgs((int)index));
        }
    }
}
