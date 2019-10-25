using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Descent2Workshop.Editor
{
    public abstract class Tool : IInputEventHandler
    {
        protected EditorState state;
        public Tool(EditorState state)
        {
            this.state = state;
        }
        public abstract bool HandleEvent(InputEvent ev);
        public abstract Operation GenerateOperation();

        public abstract void EndTool();
    }
}
