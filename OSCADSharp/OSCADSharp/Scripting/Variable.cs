using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Scripting
{
    /// <summary>
    /// A value for setting object properties in script output to
    /// a specific variable
    /// </summary>
    public class Variable
    {
        /// <summary>
        /// Name of the variable
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value of the variable.
        /// 
        /// Must be compatible with the data type being assigned to.
        /// </summary>
        public object Value { get; set; }        
    }
}
