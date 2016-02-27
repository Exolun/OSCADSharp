using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Scripting
{
    internal class Bindings
    {
        private Dictionary<string, Binding> bindings = new Dictionary<string, Binding>();
        internal void Add(Binding binding)
        {
            bindings[binding.OpenSCADFieldName] = binding;
        }

        internal bool Contains(string property)
        {
            return bindings.ContainsKey(property);
        }

        internal Binding Get(string property)
        {
            return bindings[property];
        }
    }
}
