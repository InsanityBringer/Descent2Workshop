using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Descent2Workshop.Editor
{
    public class ShadowProperties
    {
        public Vector3 translation;
        public Matrix4 rotation;
        public Vector3 scale;
        public List<LevelVertex> shadowVertices;
    }
}
