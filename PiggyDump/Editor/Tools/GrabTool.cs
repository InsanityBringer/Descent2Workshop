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
        float xScale = 1.0f, yScale = 1.0f;
        public GrabTool(EditorState state, Vector3 xVector, Vector3 yVector, List<LevelVertex> verts) : base(state)
        {
            this.xVector = xVector; this.yVector = yVector; this.verts = verts;
            state.shadow.shadowVertices = verts;
            state.shadow.translation = new Vector3(0, 0, 0);
        }

        public void SetGrabScale(float xScale, float yScale)
        {
            this.xScale = xScale; this.yScale = yScale;
        }

        public override Operation GenerateOperation()
        {
            TranslateOperation newOp = new TranslateOperation();
            newOp.SetTranslation((xVector * xAmount) + (yVector * yAmount));
            newOp.SetVerts(verts);
            state.updateFlags |= UpdateFlags.Shadow;
            return newOp;
        }

        public override bool HandleEvent(InputEvent ev)
        {
            if (ev.type == EventType.MouseMove)
            {
                xAmount += ev.deltaX * xScale;
                yAmount += -ev.deltaY * yScale;

                Vector3 translation = xVector * xAmount + yVector * yAmount;
                state.shadow.translation = translation;
            }
            state.updateFlags |= UpdateFlags.Shadow;
            return true;
        }
    }
}
