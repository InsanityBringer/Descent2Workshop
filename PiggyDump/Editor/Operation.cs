using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Descent2Workshop.Editor
{
    public abstract class Operation
    {
        public abstract Operation GenerateCounterOperation();
        public abstract void Apply();
    }
}
