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
    /// <summary>
    /// Represents an operation to a VClip that may result in a change to the VClip's frame time.
    /// </summary>
    public class VClipTimeTransaction : Transaction
    {
        private object oldValue;
        private object newValue;
        public VClipTimeTransaction(string label, object target, string propertyName, int page, int tab, object newValue) : base(label, target, propertyName, page, tab)
        {
            this.newValue = newValue;
        }

        public override bool Apply()
        {
            //Preserve the old value for undo purposes
            oldValue = property.GetValue(target);
            //Set the new value
            property.SetValue(target, newValue);
            VClip clip = (VClip)target;
            if (clip.NumFrames <= 0)
                clip.FrameTime = 0;
            else
                clip.FrameTime = clip.PlayTime / clip.NumFrames;

            return oldValue != newValue; //this is a little bad, but the results should not matter. Not that this is very ACID anyways...
        }

        public override void Revert()
        {
            property.SetValue(target, oldValue);
            VClip clip = (VClip)target;
            if (clip.NumFrames <= 0)
                clip.FrameTime = 0;
            else
                clip.FrameTime = clip.PlayTime / clip.NumFrames;
        }

        public override object GetOldValue()
        {
            return oldValue;
        }

        public override bool CanMergeWith(Transaction transaction)
        {
            if (!(transaction is VClipTimeTransaction))
                return false;

            VClipTimeTransaction other = (VClipTimeTransaction)transaction;

            //In order to merge two transactions, the transactions must be operating on the same exact object and field. 
            if (other.target.GetType() == target.GetType() && 
                other.target == target &&
                other.property == property)
            {
                return true;
            }
            return false;
        }

        public override void MergeIn(Transaction transaction)
        {
            VClipTimeTransaction other = (VClipTimeTransaction)transaction;
            newValue = other.newValue;
        }
    }
}
