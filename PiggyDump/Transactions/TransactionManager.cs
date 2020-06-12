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
    public class UndoEventArgs : EventArgs
    {
        public Transaction UndoneTransaction { get; }
        public bool Redo { get; }
        public UndoEventArgs(Transaction transaction, bool redo) : base()
        {
            UndoneTransaction = transaction;
            this.Redo = redo;
        }
    }

    public class TransactionManager
    {
        private const int NumTransactions = 60;
        public bool TransactionInProgress { get; private set; }

        private Transaction[] transactionQueue = new Transaction[NumTransactions];
        private int queueHead, queueTail, queuePosition;

        public EventHandler<UndoEventArgs> undoEvent;

        public bool UnsavedFlag = false;
        public bool CanUndo()
        {
            return queuePosition > queueTail;
        }

        public bool CanRedo()
        {
            return queuePosition < queueHead;
        }

        public void ApplyTransaction(Transaction transaction)
        {
            TransactionInProgress = true;
            if (transaction.Apply()) //Only queue the transaction if it actually changes a value
            {
                transactionQueue[queuePosition] = transaction;
                queuePosition++;
                if (queuePosition >= NumTransactions)
                    queuePosition -= NumTransactions;
                queueHead = queuePosition;

                if (queueHead == queueTail) //Bump the tail up if there is overlap
                {
                    queueTail = queueHead + 1;
                    if (queueTail >= NumTransactions)
                        queueTail -= NumTransactions;
                }
            }
            UnsavedFlag = true;
            TransactionInProgress = false;
        }

        public void DoUndo()
        {
            if (queuePosition == queueTail) return;
            int undoPosition = queuePosition - 1;
            if (undoPosition < 0) undoPosition += NumTransactions;
            TransactionInProgress = true;
            transactionQueue[undoPosition].Revert();
            if (undoEvent != null)
            {
                UndoEventArgs args = new UndoEventArgs(transactionQueue[undoPosition], false);
                undoEvent(this, args);
            }
            TransactionInProgress = false;
            queuePosition--;
        }

        public void DoRedo()
        {
            if (queuePosition == queueHead) return;

            TransactionInProgress = true;
            transactionQueue[queuePosition].Apply();
            if (undoEvent != null)
            {
                UndoEventArgs args = new UndoEventArgs(transactionQueue[queuePosition], true);
                undoEvent(this, args);
            }
            TransactionInProgress = false;
            queuePosition++;
            if (queuePosition >= NumTransactions) queuePosition -= NumTransactions;
        }
    }
}
