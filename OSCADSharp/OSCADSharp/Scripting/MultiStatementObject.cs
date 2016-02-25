using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.Spatial;

namespace OSCADSharp.Scripting
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
                if (bounds.X_Min < newBottomLeft.X)
                {
                    newBottomLeft.X = bounds.X_Min;
                }
                if (bounds.Y_Min < newBottomLeft.Y)
                {
                    newBottomLeft.Y = bounds.Y_Min;
                }
                if (bounds.Z_Min < newBottomLeft.Z)
                {
                    newBottomLeft.Z = bounds.Z_Min;
                }

                if (bounds.X_Max > newTopRight.X)
                {
                    newTopRight.X = bounds.X_Max;
                }
                if (bounds.Y_Max> newTopRight.Y)
                {
                    newTopRight.Y = bounds.Y_Max;
                }
                if (bounds.Z_Max > newTopRight.Z)
                {
                    newTopRight.Z = bounds.Z_Max;
                }
            }

            return new Bounds(newBottomLeft, newTopRight);
        }
    }
}
