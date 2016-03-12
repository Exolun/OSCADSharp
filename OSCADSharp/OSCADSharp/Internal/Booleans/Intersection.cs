using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// Creates the intersection of all child nodes
    /// </summary>
    internal class Intersection : MultiStatementObject
    {
        /// <summary>
        /// Creates the intersection of all child nodes
        /// </summary>
        /// <param name="children"></param>
        public Intersection(IEnumerable<OSCADObject> children) : base("intersection()", children)
        {
        }

        public override Vector3 Position()
        {
            throw new NotSupportedException("Position is not supported on Intersected objects.");
        }

        public override Bounds Bounds()
        {
            throw new NotSupportedException("Bounds is not supported on Intersected objects.");
        }
    }
}
