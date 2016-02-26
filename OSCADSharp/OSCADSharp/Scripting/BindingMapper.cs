using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Scripting
{
    internal class BindingMapper
    {
        public string OpenSCADFieldName { get; set; }
        public List<string> BindingOptions { get; set; }
        public Dictionary<string, Func<object, string>> BindingTransformers { get; set; }
    }
}
