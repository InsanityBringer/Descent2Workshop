using LibDescent.Data;
using LibDescent.Edit;
using System;
using System.Collections.Generic;

namespace Descent2Workshop.Transactions
{
    public class DeleteWeaponTransaction : Transaction
    {
        List<ChangedHAMReference> references = new List<ChangedHAMReference>();
        EditorHAMFile datafile;
        Weapon lastValue;
        int deleteNum;

        public DeleteWeaponTransaction(EditorHAMFile hamfile, int num, int tab) : base("Delete weapon", null, null, num, tab)
        {
            deleteNum = num;
            datafile = hamfile;
        }

        public override bool Apply()
        {
            lastValue = datafile.Weapons[deleteNum];
            datafile.Weapons.RemoveAt(deleteNum);
            RedoPage = Math.Max(0, Page - 1); //It's no good if this reaches 0, but the HAM editor will prevent it. 

            //resolve references
            if (datafile.Robots.Count > 0)
            {
                for (int i = 0; i < datafile.Robots.Count; i++)
                {
                    Robot robot = datafile.Robots[i];
                    if (robot.WeaponType == deleteNum)
                    {
                        references.Add(new ChangedHAMReference(robot.Clone(), datafile.Robots, i));
                        robot.WeaponType = 0; 
                    }
                    else if (robot.WeaponType > deleteNum)
                    {
                        references.Add(new ChangedHAMReference(robot.Clone(), datafile.Robots, i));
                        robot.WeaponType--; 
                    }

                    if (robot.WeaponTypeSecondary == deleteNum)
                    {
                        references.Add(new ChangedHAMReference(robot.Clone(), datafile.Robots, i));
                        robot.WeaponTypeSecondary = -1; 
                    }
                    else if (robot.WeaponTypeSecondary > deleteNum)
                    {
                        references.Add(new ChangedHAMReference(robot.Clone(), datafile.Robots, i));
                        robot.WeaponTypeSecondary--; 
                    }
                }
            }

            if (datafile.Weapons.Count > 0)
            {
                for (int i = 0; i < datafile.Weapons.Count; i++)
                {
                    Weapon weapon = datafile.Weapons[i];
                    if (weapon.Children == deleteNum)
                    {
                        references.Add(new ChangedHAMReference(weapon.Clone(), datafile.Weapons, i));
                        weapon.Children = -1;
                    }
                    else if (weapon.Children > deleteNum)
                    {
                        references.Add(new ChangedHAMReference(weapon.Clone(), datafile.Weapons, i));
                        weapon.Children--;
                    }
                }
            }

            return true;
        }

        public override void Revert()
        {
            //This needs to happen first in delete weapon transactions, since weapons can self reference.
            foreach (ChangedHAMReference reference in references)
                reference.source[reference.id] = reference.obj;

            datafile.Weapons.Insert(deleteNum, lastValue);
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
