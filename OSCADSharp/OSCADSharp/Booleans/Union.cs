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
    internal class Union : OSCADObject
    {
        private IEnumerable<OSCADObject> children;

        /// <summary>
        /// Create a union that is the combination of all children
        /// </summary>
        /// <param name="children">OSCADObjects to combine</param>
        internal Union(IEnumerable<OSCADObject> children)
        {
            this.children = children;
        }

        public override string ToString()
        {
            string unionCommand = "union()";
            StringBuilder sb = new StringBuilder();
            foreach (var child in this.children)
            {
                sb.AppendLine(child.ToString());
            }

            var formatter = new BlockFormatter(unionCommand, sb.ToString());
            return formatter.ToString();
        }
    }
}
