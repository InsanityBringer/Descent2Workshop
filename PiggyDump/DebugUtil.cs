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
using LibDescent.Edit;

namespace Descent2Workshop
{
    public class DebugUtil
    {
        public static EditorHAMFile TranslatePIGToHam(Descent1PIGFile pigfile, PIGFile convertedPigFile)
        {
            HAMFile file = new HAMFile();
            foreach (ushort texture in pigfile.Textures)
                file.Textures.Add(texture);
            foreach (TMAPInfo tmapInfo in pigfile.TMapInfo)
                file.TMapInfo.Add(tmapInfo);
            foreach (byte sound in pigfile.Sounds)
                file.Sounds.Add(sound);
            foreach (byte sound in pigfile.AltSounds)
                file.AltSounds.Add(sound);
            foreach (VClip clip in pigfile.VClips)
                file.VClips.Add(clip);
            foreach (EClip clip in pigfile.EClips)
                file.EClips.Add(clip);
            foreach (WClip clip in pigfile.WClips)
                file.WClips.Add(clip);
            foreach (Robot robot in pigfile.Robots)
                file.Robots.Add(robot);
            foreach (JointPos joint in pigfile.Joints)
                file.Joints.Add(joint);
            foreach (Weapon weapon in pigfile.Weapons)
            {
                if (weapon.ModelNum == 255) weapon.ModelNum = 0;
                if (weapon.ModelNumInner == 255) weapon.ModelNumInner = 0;
                file.Weapons.Add(weapon);
            }
            foreach (Polymodel model in pigfile.Models)
                if (model != null)
                    file.Models.Add(model);
            foreach (ushort gauge in pigfile.Gauges)
            {
                file.Gauges.Add(gauge);
                file.GaugesHires.Add(gauge);
            }
            foreach (ushort cockpit in pigfile.Cockpits)
                file.Cockpits.Add(cockpit);
            for (int i = 0; i < pigfile.ObjBitmaps.Length; i++)
            {
                file.ObjBitmaps.Add(pigfile.ObjBitmaps[i]);
                file.ObjBitmapPointers.Add(pigfile.ObjBitmapPointers[i]);
            }
            foreach (Powerup powerup in pigfile.Powerups)
                file.Powerups.Add(powerup);
            file.PlayerShip = pigfile.PlayerShip;
            file.Reactors.Add(pigfile.reactor);
            file.FirstMultiBitmapNum = pigfile.FirstMultiBitmapNum;
            for (int i = 0; i < pigfile.BitmapXLATData.Length; i++)
            {
                file.BitmapXLATData[i] = pigfile.BitmapXLATData[i];
            }

            EditorHAMFile res = new EditorHAMFile(file, convertedPigFile);
            res.CreateLocalLists();
            res.GenerateDefaultNamelists();
            res.TranslateData();
            return res;
        }

        public static EditorHAMFile TranslateDATToHam(PSXDatFile pigfile, PIGFile convertedPigFile)
        {
            HAMFile file = new HAMFile();
            foreach (ushort texture in pigfile.Textures)
                file.Textures.Add(texture);
            foreach (TMAPInfo tmapInfo in pigfile.TMapInfo)
                file.TMapInfo.Add(tmapInfo);
            foreach (byte sound in pigfile.Sounds)
                file.Sounds.Add(sound);
            foreach (byte sound in pigfile.AltSounds)
                file.AltSounds.Add(sound);
            foreach (VClip clip in pigfile.VClips)
                file.VClips.Add(clip);
            foreach (EClip clip in pigfile.EClips)
                file.EClips.Add(clip);
            foreach (WClip clip in pigfile.WClips)
                file.WClips.Add(clip);
            foreach (Robot robot in pigfile.Robots)
                file.Robots.Add(robot);
            foreach (JointPos joint in pigfile.Joints)
                file.Joints.Add(joint);
            foreach (Weapon weapon in pigfile.Weapons)
            {
                if (weapon.ModelNum == 255) weapon.ModelNum = 0;
                if (weapon.ModelNumInner == 255) weapon.ModelNumInner = 0;
                file.Weapons.Add(weapon);
            }
            foreach (Polymodel model in pigfile.Models)
                if (model != null)
                    file.Models.Add(model);
            foreach (ushort gauge in pigfile.Gauges)
            {
                file.Gauges.Add(gauge);
                file.GaugesHires.Add(gauge);
            }
            foreach (ushort cockpit in pigfile.Cockpits)
                file.Cockpits.Add(cockpit);
            for (int i = 0; i < pigfile.ObjBitmaps.Length; i++)
            {
                file.ObjBitmaps.Add(pigfile.ObjBitmaps[i]);
                file.ObjBitmapPointers.Add(pigfile.ObjBitmapPointers[i]);
            }
            foreach (Powerup powerup in pigfile.Powerups)
                file.Powerups.Add(powerup);
            file.PlayerShip = pigfile.PlayerShip;
            file.Reactors.Add(pigfile.reactor);
            file.FirstMultiBitmapNum = pigfile.FirstMultiBitmapNum;
            for (int i = 0; i < pigfile.BitmapXLATData.Length; i++)
            {
                file.BitmapXLATData[i] = pigfile.BitmapXLATData[i];
            }

            EditorHAMFile res = new EditorHAMFile(file, convertedPigFile);
            res.CreateLocalLists();
            res.GenerateDefaultNamelists();
            res.TranslateData();
            return res;
        }
    }
}
