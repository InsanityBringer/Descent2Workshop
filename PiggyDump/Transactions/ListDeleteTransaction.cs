using System;
using System.Collections;

namespace Descent2Workshop.Transactions
{
    public class ListDeleteTransaction : Transaction
    {
        object addObject;
        int addPos;
        public ListDeleteTransaction(string label, object target, string propertyName, int addPos, int page, int tab) : base(label, target, propertyName, page, tab)
        {
            this.addPos = addPos;

            Type targetType = target.GetType();

            RedoPage = addPos;
        }

        public override bool Apply()
        {
            IList list = (IList)property.GetValue(target);
            addObject = list[addPos];
            list.RemoveAt(addPos);

            return true;
        }

        public override void Revert()
        {
            IList list = (IList)property.GetValue(target);
            list.Insert(addPos, addObject);
        }

        public override bool ChangesListSize()
        {
            return true;
        }
    }
}
