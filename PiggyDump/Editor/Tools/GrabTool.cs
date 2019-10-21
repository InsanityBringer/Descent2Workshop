using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Descent2Workshop.Editor.Tools
{
    public class GrabTool : Tool
    {
        public GrabTool(EditorState state) : base(state)
        {
        }

        public override Operation GenerateOperation()
        {
            throw new NotImplementedException();
        }

        public override bool HandleEvent(InputEvent ev)
        {
            return true;
        }
    }
}
