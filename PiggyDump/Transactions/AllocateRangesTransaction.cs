using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibDescent.Data;
using LibDescent.Edit;

namespace Descent2Workshop.Transactions
{
    //This is done this way to allow for eventual editing of HXM files without a HAM file. 
    public struct FreeRange
    {
        public int start, count;
    }
    public class AllocateRangesTransaction : Transaction
    {
        private EditorHXMFile datafile;
        private HXMResourceAllocator objBitmapAllocator;
        private HXMResourceAllocator objBmpPtrAllocator;
        private HXMResourceAllocator jointAllocator;
        private List<ChangedHAMReference> changedElements = new List<ChangedHAMReference>();

        public AllocateRangesTransaction(EditorHXMFile datafile, int numObjBitmaps, IList<FreeRange> freedObjBitmaps,
            int numObjBmpPtrs, IList<FreeRange> freedObjBmpPtrs, 
            int numJoints, IList<FreeRange> freedJoints, 
            IList<int> eclipLocks) : base("Allocate elements")
        {
            this.datafile = datafile;
            objBitmapAllocator = new HXMResourceAllocator(600, numObjBitmaps);
            objBmpPtrAllocator = new HXMResourceAllocator(600, numObjBmpPtrs);
            jointAllocator = new HXMResourceAllocator(1500, numJoints);

            foreach (int eclipNum in eclipLocks)
            {
                objBitmapAllocator.LockElement(eclipNum);
            }

            foreach (FreeRange range in freedObjBitmaps)
                objBitmapAllocator.FreeRange(range.start, range.count);
            foreach (FreeRange range in freedObjBmpPtrs)
                objBmpPtrAllocator.FreeRange(range.start, range.count);
            foreach (FreeRange range in freedJoints)
                jointAllocator.FreeRange(range.start, range.count);

            objBitmapAllocator.SaveAllocation();
            objBmpPtrAllocator.SaveAllocation();
            jointAllocator.SaveAllocation();
        }

        public override bool ChangesListSize()
        {
            return true;
        }

        public override bool Apply()
        {
            Polymodel model;
            for (int i = 0; i < datafile.ReplacedModels.Count; i++)
            {
                model = datafile.ReplacedModels[i];
                Polymodel oldmodel = model.Clone();
                int numObjBitmaps = datafile.CountUniqueObjBitmaps(model);

                int newObjBitmapStart = objBitmapAllocator.FindFreeRange(numObjBitmaps);
                int newObjBmpPtrStart = objBmpPtrAllocator.FindFreeRange(model.NumTextures);

                if (newObjBitmapStart != -1 && numObjBitmaps > 0)
                {
                    model.BaseTexture = newObjBitmapStart;
                    objBitmapAllocator.AllocateRange(newObjBitmapStart, numObjBitmaps);
                }
                if (newObjBmpPtrStart != -1 && model.NumTextures > 0)
                {
                    model.FirstTexture = (ushort)newObjBmpPtrStart;
                    objBmpPtrAllocator.AllocateRange(newObjBmpPtrStart, model.NumTextures);
                }

                ChangedHAMReference element = new ChangedHAMReference(oldmodel, datafile.ReplacedModels, i);
                changedElements.Add(element);
            }
            return true;
        }

        public override void Revert()
        {
            objBitmapAllocator.RevertAllocation();
            objBmpPtrAllocator.RevertAllocation();
            jointAllocator.RevertAllocation();

            foreach (ChangedHAMReference element in changedElements)
            {
                element.source[element.id] = element.obj;
            }
        }
    }
}
