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
        float scale = 1.0f;

        bool fixAxis = false;
        Vector3 fixedAxis;
        float xScale = 1.0f, yScale = 1.0f;
        public GrabTool(EditorState state, Vector3 xVector, Vector3 yVector, List<LevelVertex> verts) : base(state)
        {
            this.xVector = xVector; this.yVector = yVector; this.verts = verts;
            state.shadow.shadowVertices = verts;
            state.shadow.translation = new Vector3(0, 0, 0);

            Console.WriteLine("{0}", Vector3.Dot(xVector, new Vector3(1.0f, 0.0f, 0.0f)));
        }

        public void SetGrabScale(float scale)
        {
            this.scale = scale;
        }

        private Vector3 GetTranslation()
        {
            Vector3 translation;
            if (!fixAxis)
            {
                translation = xVector * xAmount + yVector * yAmount;
                state.shadow.translation = translation;
            }
            else
            {
                translation = (fixedAxis * xAmount * xScale) + (fixedAxis * yAmount * yScale);
                state.shadow.translation = translation;
            }
            return translation;
        }

        public override Operation GenerateOperation()
        {
            TranslateOperation newOp = new TranslateOperation();
            newOp.SetTranslation(GetTranslation());
            newOp.SetVerts(verts);
            state.updateFlags |= UpdateFlags.Shadow;
            return newOp;
        }

        public override bool HandleEvent(InputEvent ev)
        {
            if (ev.type == EventType.Key)
            {
                if (ev.down)
                {
                    if (ev.key == System.Windows.Forms.Keys.X)
                    {
                        fixAxis = true;
                        fixedAxis = new Vector3(1.0f, 0.0f, 0.0f);
                        xScale = Vector3.Dot(xVector, fixedAxis);
                        yScale = Vector3.Dot(yVector, fixedAxis);
                    }
                    else if (ev.key == System.Windows.Forms.Keys.Y)
                    {
                        fixAxis = true;
                        fixedAxis = new Vector3(0.0f, 1.0f, 0.0f);
                        xScale = Vector3.Dot(xVector, fixedAxis);
                        yScale = Vector3.Dot(yVector, fixedAxis);
                    }
                    else if (ev.key == System.Windows.Forms.Keys.Z)
                    {
                        fixAxis = true;
                        fixedAxis = new Vector3(0.0f, 0.0f, 1.0f);
                        xScale = Vector3.Dot(xVector, fixedAxis);
                        yScale = Vector3.Dot(yVector, fixedAxis);
                    }
                    else if (ev.key == System.Windows.Forms.Keys.Escape)
                    {
                        state.AbortTool();
                    }
                }
            }
            if (ev.type == EventType.MouseMove)
            {
                xAmount += ev.deltaX * scale;
                yAmount += -ev.deltaY * scale;
                state.shadow.translation = GetTranslation();
            }
            if (ev.type == EventType.MouseButton)
            {
                if (ev.down)
                {
                    if (ev.mouseButton == System.Windows.Forms.MouseButtons.Left)
                        state.ApplyTool();
                }
            }
            state.updateFlags |= UpdateFlags.Shadow;
            return true;
        }

        public override void EndTool()
        {
            //Destroy shadow
            state.shadow.shadowVertices = null;
            state.updateFlags |= UpdateFlags.Shadow;
        }
    }
}
