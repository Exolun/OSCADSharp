using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Scripting
{
    internal class Bindings
    {
        internal List<BindingMapper> Mappers { get; set; } = new List<BindingMapper>();

        private Dictionary<string, Variable> bindings = new Dictionary<string, Variable>();
        internal void Add(string property, Variable variable)
        {
            bindings[property] = variable;
        }
    }
}
