/*
    Copyright (c) 2020 SaladBadger

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
using System.Collections;
using System.Reflection;

namespace Descent2Workshop.Transactions
{
    public class ListAddTransaction : Transaction
    {
        object addObject;
        int addPos;
        public ListAddTransaction(string label, object target, string propertyName, int addPos, object addObject, int page, int tab) : base(label, target, propertyName, page, tab)
        {
            this.addPos = addPos;
            this.addObject = addObject;

            Type targetType = target.GetType();

            RedoPage = addPos;
        }

        public override bool Apply()
        {
            IList list = (IList)property.GetValue(target);
            if (addPos >= list.Count)
            {
                list.Add(addObject); //just append to avoid issues
                addPos = list.Count - 1; //Change the add pos in case it was an invalid value, needed to avoid issues with undo. 
            }
            else
            {
                list.Insert(addPos, addObject);
            }

            return true;
        }

        public override void Revert()
        {
            IList list = (IList)property.GetValue(target);
            list.RemoveAt(addPos);
        }

        public override bool ChangesListSize()
        {
            return true;
        }
    }
}
