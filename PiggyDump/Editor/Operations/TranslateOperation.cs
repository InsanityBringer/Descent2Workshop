using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Descent2Workshop.Editor.Operations
{
    public class TranslateOperation : Operation
    {
        private List<LevelVertex> vertset;
        private Vector3 translation;
        public void SetVerts(List<LevelVertex> verts)
        {
            vertset = verts;
        }

        public void SetTranslation(Vector3 translation)
        {
            this.translation = translation;
        }

        public override void Apply(EditorState context)
        {
            foreach (LevelVertex vert in vertset)
            {
                vert.location.x += translation.X;
                vert.location.y += translation.Y;
                vert.location.z += translation.Z;
            }
            context.updateFlags |= UpdateFlags.World;
        }

        public override Operation GenerateCounterOperation()
        {
            TranslateOperation newOp = new TranslateOperation();
            SetVerts(vertset);
            SetTranslation(translation *= -1);

            return newOp;
        }
    }
}
