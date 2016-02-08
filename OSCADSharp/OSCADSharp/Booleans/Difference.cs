using OSCADSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Booleans
{
    /// <summary>
    /// Subtracts the 2nd (and all further) child nodes from the first one (logical and not).    
    /// </summary>
    internal class Difference : BlockStatementObject
    {
        /// <summary>
        /// Creates a subtraction of child nodes
        /// </summary>
        /// <param name="children"></param>
        public Difference(IEnumerable<OSCADObject> children) : base("difference()", children)
        {
        }
    }
}
