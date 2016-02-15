using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Scripting
{

    /// <summary>
    /// A statement that has multiple child nodes, whose ToString output
    /// is more or less just an aggregate of the children
    /// </summary>
    internal class MultiBlockStatementObject : OSCADObject
    {
        private string outerStatement;

        internal MultiBlockStatementObject(string outerStatement, IEnumerable<OSCADObject> children)
        {
            this.outerStatement = outerStatement;
            this.children = children.ToList();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var child in this.children)
            {
                sb.Append(child.ToString());
            }

            var formatter = new SingleBlockFormatter(this.outerStatement, sb.ToString());
            return formatter.ToString();
        }

        public override OSCADObject Clone()
        {
            List<OSCADObject> childClones = new List<OSCADObject>();
            foreach (var child in this.children)
            {
                childClones.Add(child.Clone());
            }

            return new MultiBlockStatementObject(this.outerStatement, childClones);
        }

        public override Vector3 Position()
        {
            throw new NotImplementedException();
        }
    }
}
