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

using System.Collections.Generic;

namespace LibDescent.Data
{
    public class Submodel
    {
        public int Pointer;
        public FixVector Offset = new FixVector();
        public FixVector Normal = new FixVector();
        public FixVector Point = new FixVector();
        public int Radius;
        public byte Parent;
        public FixVector Mins = new FixVector();
        public FixVector Maxs = new FixVector();
        public List<Submodel> Children = new List<Submodel>();
        //public FixVector[] points = new FixVector[1000];
        public List<PolymodelFace> faces = new List<PolymodelFace>();
        public int ID;
    }
    public class Polymodel
    {
        public const int MAX_SUBMODELS = 10;
        public const int MAX_GUNS = 8;

        public int n_models;
        public int model_data_size;
        public int model_data; //ignored, was pointer
        public List<Submodel> submodels = new List<Submodel>();
        public FixVector mins = new FixVector(), maxs = new FixVector();							//min,max for whole model
        public int rad;
        public byte n_textures;
        public ushort first_texture;
        public byte simpler_model;		//alternate model with less detail (0 if none, model_num+1 else)
        public PolymodelData data;
        public List<string> textureList = new List<string>();
        public bool useTexList = false;
        public int DyingModelnum = -1;
        public int DeadModelnum = -1;

        //Things needed to simplify animation
        public int numGuns;
        public FixVector[] gunPoints = new FixVector[MAX_GUNS];
        public FixVector[] gunDirs = new FixVector[MAX_GUNS];
        public int[] gunSubmodels = new int[MAX_GUNS];
        //A model can have subobjects, but not actually be animated. Avoid creating extra joints if this is the case. 
        public bool isAnimated = false;
        public FixAngles[,] animationMatrix = new FixAngles[MAX_SUBMODELS, Robot.NUM_ANIMATION_STATES];

        public int replacementID;
        public int ID;

        public Polymodel(int numSubobjects)
        {
            for (int i = 0; i < numSubobjects; i++)
            {
                submodels.Add(new Submodel());
            }
        }
        public Polymodel() : this(0) { }

        public void ExpandSubmodels()
        {
            while (submodels.Count < 10)
            {
                submodels.Add(new Submodel());
            }
            for (int x = numGuns; x < MAX_GUNS; x++)
            {
                gunPoints[x] = new FixVector();
                gunDirs[x] = new FixVector(65536, 0, 0);
            }
        }
    }

    public struct JointPos
    {
        public short jointnum;
        public FixAngles angles;
        public int replacementID;
    }
}
