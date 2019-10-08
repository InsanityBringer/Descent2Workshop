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

namespace PiggyDump
{
    public class Ship : HAMElement
    {
        public const int PropVClip = 0;
        public const int PropModel = 1;
        public const int PropMarkerModel = 2; //This isn't quite correct, but it makes life easier to treat it as if it is. It's dropped by the ship, after all. 
        public static string[] TagNames = { "Explosion VClip", "Model", "Marker Model" };
        public int model_num;
        public int expl_vclip_num;
        public int mass, drag;
        public int max_thrust, reverse_thrust, brakes;		//low_thrust
        public int wiggle;
        public int max_rotthrust;
        public FixVector[] gun_points = new FixVector[8];
        public int markerModel;

        public Polymodel model;
        public Polymodel marker;
        public VClip explosion;

        public void InitReferences(IElementManager manager)
        {
            model = manager.GetModel(model_num);
            marker = manager.GetModel(markerModel);
            explosion = manager.GetVClip(expl_vclip_num);
        }

        public void AssignReferences(IElementManager manager)
        {
            if (model != null) model.AddReference(HAMType.Ship, this, PropModel);
            if (marker != null) marker.AddReference(HAMType.Ship, this, PropMarkerModel);
            if (explosion != null) explosion.AddReference(HAMType.Ship, this, PropVClip);
        }

        public void ClearReferences()
        {
            if (model != null) model.ClearReference(HAMType.Ship, this, PropModel);
            if (marker != null) marker.ClearReference(HAMType.Ship, this, PropMarkerModel);
            if (explosion != null) explosion.ClearReference(HAMType.Ship, this, PropVClip);
        }

        public static string GetTagName(int tag)
        {
            return TagNames[tag];
        }

        public void UpdateShip(int field, int data)
        {
            switch (field)
            {
                case 1:
                    model_num = data;
                    break;
                case 2:
                    expl_vclip_num = data;
                    break;
                case 3:
                    mass = data;
                    break;
                case 4:
                    drag = data;
                    break;
                case 5:
                    max_thrust = data;
                    break;
                case 6:
                    reverse_thrust = data;
                    break;
                case 7:
                    brakes = data;
                    break;
                case 8:
                    wiggle = data;
                    break;
                case 9:
                    max_rotthrust = data;
                    break;
            }
        }
    }
}
