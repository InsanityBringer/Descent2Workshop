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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LibDescent.Data;
using LibDescent.Edit;

namespace Descent2Workshop.Transactions
{
    public class DeleteVClipTransaction : Transaction
    {
        List<ChangedHAMReference> references = new List<ChangedHAMReference>();
        EditorHAMFile datafile;
        VClip lastValue;
        int deleteNum;
        public DeleteVClipTransaction(EditorHAMFile hamfile, int num, int tab) : base("Delete vclip", null, null, num, tab)
        {
            deleteNum = num;
            datafile = hamfile;
        }

        public override bool Apply()
        {
            lastValue = datafile.VClips[deleteNum];
            datafile.VClips.RemoveAt(deleteNum);

            //Resolve references
            references.Clear();
            if (datafile.VClips.Count > 0)
            {
                //Resolve eclip break clips
                for (int i = 0; i < datafile.EClips.Count; i++)
                {
                    if (datafile.EClips[i].ExplosionVClip == deleteNum)
                    {
                        references.Add(new ChangedHAMReference(datafile.EClips[i].Clone(), datafile.EClips, i));
                        datafile.EClips[i].ExplosionVClip = -1;
                    }
                    else if (datafile.EClips[i].ExplosionVClip > deleteNum)
                    {
                        references.Add(new ChangedHAMReference(datafile.EClips[i].Clone(), datafile.EClips, i));
                        datafile.EClips[i].ExplosionVClip--;
                    }
                }

                //Resolve robot vclips
                for (int i = 0; i < datafile.Robots.Count; i++)
                {
                    bool change = false;
                    Robot oldRobot = datafile.Robots[i].Clone();

                    if (datafile.Robots[i].HitVClipNum == deleteNum)
                    {
                        change = true;
                        datafile.Robots[i].HitVClipNum = -1;
                    }
                    else if (datafile.Robots[i].HitVClipNum > deleteNum)
                    {
                        change = true;
                        datafile.Robots[i].HitVClipNum--;
                    }

                    if (datafile.Robots[i].DeathVClipNum == deleteNum)
                    {
                        change = true;
                        datafile.Robots[i].DeathVClipNum = -1;
                    }
                    else if (datafile.Robots[i].DeathVClipNum > deleteNum)
                    {
                        change = true;
                        datafile.Robots[i].DeathVClipNum--;
                    }

                    if (change)
                        references.Add(new ChangedHAMReference(oldRobot, datafile.Robots, i));
                }

                //Resolve weapon vclips
                for (int i = 0; i < datafile.Weapons.Count; i++)
                {
                    bool change = false;
                    Weapon oldWeapon = datafile.Weapons[i].Clone();

                    if (datafile.Weapons[i].RobotHitVClip >= deleteNum)
                    {
                        change = true;
                        datafile.Weapons[i].RobotHitVClip--;
                    }
                    if (datafile.Weapons[i].WallHitVClip >= deleteNum)
                    {
                        change = true;
                        datafile.Weapons[i].WallHitVClip--;
                    }
                    if (datafile.Weapons[i].MuzzleFlashVClip >= deleteNum)
                    {
                        change = true;
                        datafile.Weapons[i].MuzzleFlashVClip--;
                    }
                    if (datafile.Weapons[i].WeaponVClip >= deleteNum)
                    {
                        change = true;
                        datafile.Weapons[i].WeaponVClip--;
                    }

                    if (change)
                        references.Add(new ChangedHAMReference(oldWeapon, datafile.Weapons, i));
                }

                //Resolve powerup vclips
                for (int i = 0; i < datafile.Powerups.Count; i++)
                {
                    if (datafile.Powerups[i].VClipNum >= deleteNum)
                    {
                        references.Add(new ChangedHAMReference(datafile.Powerups[i].Clone(), datafile.Powerups, i));
                        datafile.Powerups[i].VClipNum = Math.Max(0, datafile.Powerups[i].VClipNum - 1);
                    }
                }
            }
            return true;
        }

        public override void Revert()
        {
            datafile.VClips.Insert(deleteNum, lastValue);

            foreach (ChangedHAMReference reference in references)
            {
                reference.source[reference.id] = reference.obj;
            }
        }

        public override bool ChangesListSize()
        {
            return true;
        }
    }
}
