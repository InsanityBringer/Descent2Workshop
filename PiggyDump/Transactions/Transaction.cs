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
    public class Transaction
    {
        public string OperationName { get; }
        public object SourceObject { get; }
        public int Page { get; }
        public int Tab { get; }

        protected PropertyInfo property;
        protected object target;

        public Transaction(string label, object target, string propertyName, int page, int tab)
        {
            OperationName = label;
            this.target = target;
            Type targetType = target.GetType();
            property = targetType.GetProperty(propertyName);
            if (!property.CanWrite)
                throw new Exception(string.Format("Transaction::Transaction: Cannot write specified property {0}", propertyName));
            Page = page;
            Tab = tab;
        }
        public virtual bool Apply()
        {
            return false;
        }

        public virtual void Revert()
        {
        }

        public object GetCurrentValue()
        {
            return property.GetValue(target);
        }

        public virtual object GetOldValue()
        {
            return null;
        }
    }
}
