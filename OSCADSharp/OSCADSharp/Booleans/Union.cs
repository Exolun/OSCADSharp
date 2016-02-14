using OSCADSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Booleans
{
    /// <summary>
    /// A union of child nodes. This is the sum of all children (logical or).    
    /// </summary>
    internal class Union : MultiBlockStatementObject
    {
        /// <summary>
        /// Create a union that is the combination of all children
        /// </summary>
        /// <param name="children">OSCADObjects to combine</param>
        internal Union(IEnumerable<OSCADObject> children) : base("union()", children)
        {
        }
    }
}
