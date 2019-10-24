using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Descent2Workshop.Editor.Operations;

namespace Descent2Workshop.Editor.Tools
{
    public class GrabTool : Tool
    {
        List<LevelVertex> verts;
        Vector3 xVector, yVector;
        float xAmount, yAmount;
        public GrabTool(EditorState state, Vector3 xVector, Vector3 yVector, List<LevelVertex> verts) : base(state)
        {
            this.xVector = xVector; this.yVector = yVector; this.verts = verts;
        }

        public override Operation GenerateOperation()
        {
            TranslateOperation newOp = new TranslateOperation();
            newOp.SetTranslation((xVector * xAmount) + (yVector * yAmount));
            newOp.SetVerts(verts);
            return newOp;
        }

        public override bool HandleEvent(InputEvent ev)
        {
            state.updateFlags |= UpdateFlags.Shadow;
            return true;
        }
    }
}
