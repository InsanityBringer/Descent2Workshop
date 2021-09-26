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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Descent2Workshop.Transactions
{
    class ByteArrayReplaceTransaction : Transaction
    {
        byte addObject;
        byte oldObject;
        int addPos;

        public ByteArrayReplaceTransaction(string label, object target, string propertyName, int addPos, byte addObject, int page, int tab) : base(label, target, propertyName, page, tab)
        {
            this.addPos = addPos;
            this.addObject = addObject;
            RedoPage = addPos;
        }

        public override bool Apply()
        {
            byte[] list = (byte[])property.GetValue(target);
            oldObject = list[addPos];
            list[addPos] = addObject;

            return true;
        }

        public override void Revert()
        {
            byte[] list = (byte[])property.GetValue(target);
            list[addPos] = oldObject;
        }
    }
}
