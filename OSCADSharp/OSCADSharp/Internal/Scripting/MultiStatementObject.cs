using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{

    /// <summary>
    /// A statement that has multiple child nodes, whose ToString output
    /// is more or less just an aggregate of the children
    /// </summary>
    internal class MultiStatementObject : OSCADObject
    {
        private string outerStatement;

        internal MultiStatementObject(string outerStatement, IEnumerable<OSCADObject> children)
        {
            this.outerStatement = outerStatement;
            this.children = children.ToList();
            foreach (var child in children)
            {
                child.Parent = this;
            }
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

            return new MultiStatementObject(this.outerStatement, childClones)
            {
                Name = this.Name
            };
        }

        public override Vector3 Position()
        {
            var positions = this.children.Select(child => child.Position());
            return Vector3.Average(positions.ToArray());
        }

        public override Bounds Bounds()
        {
            var newBottomLeft = new Vector3();
            var newTopRight = new Vector3();

            foreach (var child in this.children)
            {
                var bounds = child.Bounds();
                if (bounds.XMin < newBottomLeft.X)
                {
                    newBottomLeft.X = bounds.XMin;
                }
                if (bounds.YMin < newBottomLeft.Y)
                {
                    newBottomLeft.Y = bounds.YMin;
                }
                if (bounds.ZMin < newBottomLeft.Z)
                {
                    newBottomLeft.Z = bounds.ZMin;
                }

                if (bounds.XMax > newTopRight.X)
                {
                    newTopRight.X = bounds.XMax;
                }
                if (bounds.YMax> newTopRight.Y)
                {
                    newTopRight.Y = bounds.YMax;
                }
                if (bounds.ZMax > newTopRight.Z)
                {
                    newTopRight.Z = bounds.ZMax;
                }
            }

            return new Bounds(newBottomLeft, newTopRight);
        }

        public override void Bind(string property, Variable variable)
        {
            throw new NotSupportedException("This object has no bindable properties.");
        }
    }
}
