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
using System.Reflection;

namespace Descent2Workshop.Transactions
{
    public class IntegerTransaction : Transaction
    {
        private int oldValue;
        private int newValue;
        public IntegerTransaction(string label, object target, string propertyName, int page, int tab, int newValue) : base(label, target, propertyName, page, tab)
        {
            this.newValue = newValue;
        }

        public override bool Apply()
        {
            //okay so this is a dumb hack so we can generalize this mess
            if (property.PropertyType == typeof(short))
            {
                //Preserve the old value for undo purposes
                oldValue = (int)((short)property.GetValue(target));
                //Set the new value
                property.SetValue(target, (short)newValue);
            }
            else if (property.PropertyType == typeof(sbyte))
            {
                //Preserve the old value for undo purposes
                oldValue = (int)((sbyte)property.GetValue(target));
                //Set the new value
                property.SetValue(target, (sbyte)newValue);
            }
            else
            {
                //Preserve the old value for undo purposes
                oldValue = (int)property.GetValue(target);
                //Set the new value
                property.SetValue(target, newValue);
            }
            return oldValue != newValue; //this is a little bad, but the results should not matter. Not that this is very ACID anyways...
        }

        public override void Revert()
        {
            //okay so this is a dumb hack so we can generalize this mess
            if (property.PropertyType == typeof(short))
            {
                property.SetValue(target, (short)oldValue);
            }
            else if (property.PropertyType == typeof(sbyte))
            {
                property.SetValue(target, (sbyte)oldValue);
            }
            else
            {
                property.SetValue(target, oldValue);
            }
        }

        public override object GetOldValue()
        {
            return oldValue;
        }
    }
}
