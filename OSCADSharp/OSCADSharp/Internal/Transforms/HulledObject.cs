using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// Creates an object that's the convex hull of child objects
    /// </summary>
    internal class HulledObject : MultiStatementObject
    {
        internal HulledObject(IEnumerable<OSCADObject> children) : base("hull()", children)
        {
        }
    }
}
