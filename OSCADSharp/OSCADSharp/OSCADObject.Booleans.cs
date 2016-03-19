using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    public abstract partial class OSCADObject
    {
        #region Difference
        /// <summary>
        /// Subtracts the 2nd (and all further) child nodes from the first one (logical and not).    
        /// </summary>
        private class DifferencedObject : MultiStatementObject
        {
            /// <summary>
            /// Creates a subtraction of child nodes
            /// </summary>
            /// <param name="children"></param>
            internal DifferencedObject(IEnumerable<OSCADObject> children) : base("difference()", children)
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
        #endregion

        #region Intersection
        /// <summary>
        /// Creates the intersection of all child nodes
        /// </summary>
        private class IntersectedObject : MultiStatementObject
        {
            /// <summary>
            /// Creates the intersection of all child nodes
            /// </summary>
            /// <param name="children"></param>
            internal IntersectedObject(IEnumerable<OSCADObject> children) : base("intersection()", children)
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
        #endregion

        #region Union
        /// <summary>
        /// A union of child nodes. This is the sum of all children (logical or).    
        /// </summary>
        private class UnionedObject : MultiStatementObject
        {
            /// <summary>
            /// Create a union that is the combination of all children
            /// </summary>
            /// <param name="children">OSCADObjects to combine</param>
            internal UnionedObject(IEnumerable<OSCADObject> children) : base("union()", children)
            {
            }
        }
        #endregion
    }
}
