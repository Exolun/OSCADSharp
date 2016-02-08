using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Booleans
{
    /// <summary>
    /// Creates the intersection of all child nodes
    /// </summary>
    internal class Intersection : BlockStatementObject
    {
        /// <summary>
        /// Creates the intersection of all child nodes
        /// </summary>
        /// <param name="children"></param>
        public Intersection(IEnumerable<OSCADObject> children) : base("intersection()", children)
        {
        }
    }
}
