using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Descent2Workshop
{
    public class HXMResourceAllocator
    {
        public int Maximum { get; }

        //TODO make an enum
        /// <summary>
        /// Each byte contains the allocation state. 0=free 1=used 2=locked
        /// </summary>
        public byte[] Allocation { get; }

        private byte[] oldAllocation;

        public HXMResourceAllocator(int maximum, int preallocate)
        {
            if (preallocate >= maximum)
                throw new ArgumentException("HXMResourceAllocator created with preallocation greater than maximum size.");

            Maximum = maximum;
            Allocation = new byte[maximum];
            oldAllocation = new byte[maximum];

            for (int i = 0; i < preallocate; i++)
                Allocation[i] = 1;
        }

        public void SaveAllocation()
        {
            Array.Copy(Allocation, oldAllocation, Maximum);
        }

        public void RevertAllocation()
        {
            Array.Copy(oldAllocation, Allocation, Maximum);
        }

        public void AllocateRange(int start, int size)
        {
            int end = start + size;

            if (start >= Maximum || end >= Maximum)
                throw new ArgumentException("HXMResourceAllocator range allocated that exceeds the limit");

            for (int i = start; i < end; i++)
            {
                if (Allocation[i] != 2)
                    Allocation[i] = 1;
            }
        }

        public void FreeRange(int start, int size)
        {
            int end = start + size;

            if (start >= Maximum || end >= Maximum)
                throw new ArgumentException("HXMResourceAllocator range allocated that exceeds the limit");

            for (int i = start; i < end; i++)
            {
                if (Allocation[i] != 2)
                    Allocation[i] = 0;
            }
        }

        public void LockElement(int elem)
        {
            Allocation[elem] = 2;
        }

        public int FindFreeRange(int size)
        {
            int starti;
            int i = 0;
            int j;
            while (i < Maximum)
            {
                if (Allocation[i] != 0)
                    i++;
                else
                {
                    starti = i;
                    for (j = 0; j < size; i++, j++)
                    {
                        if (i+1 >= Maximum)
                            return -1;

                        if (Allocation[i] != 0)
                            break;
                    }

                    if (j == size)
                        return starti;
                }
            }
            return -1;
        }
    }
}
