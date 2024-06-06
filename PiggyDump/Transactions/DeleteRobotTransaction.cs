using LibDescent.Data;
using LibDescent.Edit;
using System;
using System.Collections.Generic;

namespace Descent2Workshop.Transactions
{
    public class DeleteRobotTransaction : Transaction
    {
        List<ChangedHAMReference> references = new List<ChangedHAMReference>();
        EditorHAMFile datafile;
        Robot lastValue;
        int deleteNum;

        public DeleteRobotTransaction(EditorHAMFile hamfile, int num, int tab) : base("Delete robot", null, null, num, tab)
        {
            deleteNum = num;
            datafile = hamfile;
        }

        public override bool Apply()
        {
            lastValue = datafile.Robots[deleteNum];
            datafile.Weapons.RemoveAt(deleteNum);
            RedoPage = Math.Max(0, Page - 1); //It's no good if this reaches 0, but the HAM editor will prevent it.

            //resolve references
            if (datafile.Robots.Count > 0)
            {
                for (int i = 0; i < datafile.Robots.Count; i++)
                {
                    Robot robot = datafile.Robots[i];
                    if (robot.ContainsType == 2)
                    {
                        if (robot.ContainsID == deleteNum)
                        {
                            references.Add(new ChangedHAMReference(robot.Clone(), datafile.Robots, i));
                            robot.ContainsID = 0;
                        }
                        else if (robot.ContainsID > deleteNum)
                        {
                            references.Add(new ChangedHAMReference(robot.Clone(), datafile.Robots, i));
                            robot.ContainsID--;
                        }
                    }
                }
            }

            return true;
        }

        public override void Revert()
        {
            //This needs to happen first in delete robot transactions, since robots can self reference.
            foreach (ChangedHAMReference reference in references)
                reference.source[reference.id] = reference.obj;

            datafile.Robots.Insert(deleteNum, lastValue);
        }

        public override bool NeedsListUpdate() //must be done due to self-reference
        {
            return true;
        }

        public override bool ChangesListSize()
        {
            return true;
        }
    }
}
