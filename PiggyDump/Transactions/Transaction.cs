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
        public int RedoPage { get; protected set; }
        public int Tab { get; }

        protected PropertyInfo property;
        protected object target;

        /// <summary>
        /// Constructor for operations performed in interfaces with only one tab. 
        /// </summary>
        /// <param name="label">The name of the operation to display in the Undo menu.</param>
        public Transaction(string label)
        {
        }
        /// <summary>
        /// Constructor for operations performed in interfaces with multiple tabs with multiple pages.
        /// Uses reflection to perform the change generically. 
        /// </summary>
        /// <param name="label">The name of the operation to display in the Undo menu.</param>
        /// <param name="target">The instance of the object to target.</param>
        /// <param name="propertyName">The name of the object's property to target.</param>
        /// <param name="page">The page of the current object.</param>
        /// <param name="tab">The tab the current object's page resides on.</param>
        public Transaction(string label, object target, string propertyName, int page, int tab)
        {
            OperationName = label;
            this.target = target;
            if (target != null && propertyName != null) //hack
            {
                Type targetType = target.GetType();
                property = targetType.GetProperty(propertyName);
                //if (!property.CanWrite) //TODO: this check doesn't work with indexed things like arrays that can't be redefined by the user
                //    throw new Exception(string.Format("Transaction::Transaction: Cannot write specified property {0}", propertyName));
            }
            Page = page;
            RedoPage = page;
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

        public virtual bool CanMergeWith(Transaction transaction)
        {
            return false;
        }

        public virtual void MergeIn(Transaction transaction)
        {
        }
    }
}
