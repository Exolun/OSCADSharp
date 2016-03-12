using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    internal class CompoundVariable : Variable
    {
        public CompoundVariable(string name, object value) : base(name, value, false)
        {
        }
    }
}
