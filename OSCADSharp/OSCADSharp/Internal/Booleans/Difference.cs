using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// Subtracts the 2nd (and all further) child nodes from the first one (logical and not).    
    /// </summary>
    internal class Difference : MultiStatementObject
    {
        /// <summary>
        /// Creates a subtraction of child nodes
        /// </summary>
        /// <param name="children"></param>
        public Difference(IEnumerable<OSCADObject> children) : base("difference()", children)
        {
        }

        public override Vector3 Position()
        {
            return m_children[0].Position();
        }

        public override Bounds Bounds()
        {
            return m_children[0].Bounds();
        }
    }
}
