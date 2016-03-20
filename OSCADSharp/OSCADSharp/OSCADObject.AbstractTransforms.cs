using OSCADSharp.DataBinding;
using OSCADSharp.Spatial;
using OSCADSharp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    public abstract partial class OSCADObject
    {
        #region MirroredObject
        /// <summary>
        /// An object that's mirrored on a plane
        /// </summary>
        internal class MirroredObject : SingleStatementObject
        {
            /// <summary>
            /// The normal vector of a plane intersecting the origin of the object,
            /// through which to mirror it.
            /// </summary>
            internal Vector3 Normal { get; set; } = new BindableVector();

            /// <summary>
            /// Creates an object that's mirrored on a plane
            /// </summary>
            /// <param name="obj">The object(s) to be mirrored</param>
            /// <param name="normal">The normal vector of the plane on the object's origin to mirror upon</param>
            internal MirroredObject(OSCADObject obj, Vector3 normal) : base(obj)
            {
                this.Normal = new BindableVector(normal);
            }

            internal MirroredObject(OSCADObject obj, Variable normal) : base(obj)
            {
                this.Bind("normal", normal);
            }

            internal MirroredObject(OSCADObject obj, Vector3 normal, Variable x, Variable y, Variable z) : base(obj)
            {
                this.Normal = new BindableVector(normal);
                this.BindIfVariableNotNull("x", x);
                this.BindIfVariableNotNull("y", y);
                this.BindIfVariableNotNull("z", z);
            }

            public override string ToString()
            {
                string normal = this.bindings.Contains("normal") ? this.bindings.Get("normal").BoundVariable.Text : this.Normal.ToString();

                string mirrorCommand = String.Format("mirror({0})", normal);
                var formatter = new SingleBlockFormatter(mirrorCommand, this.obj.ToString());
                return formatter.ToString();
            }

            public override OSCADObject Clone()
            {
                return new MirroredObject(this.obj.Clone(), this.Normal)
                {
                    Name = this.Name,
                    bindings = this.bindings.Clone()
                };
            }

            // TODO:  This will yield incorrect positions if mirroring on multiple axes
            // fix mirrored positions for multiple-axis mirroring
            public override Vector3 Position()
            {
                if (this.isMoreThanOneAxis())
                {
                    throw new NotSupportedException("Getting the position of an object that's been mirrored on more than one axis is not currently supported.");
                }

                var pos = obj.Position();

                double x = this.Normal.X != 0 ? pos.X * -1 : pos.X;
                double y = this.Normal.Y != 0 ? pos.Y * -1 : pos.Y;
                double z = this.Normal.Z != 0 ? pos.Z * -1 : pos.Z;

                return new Vector3(x, y, z);
            }

            private bool isMoreThanOneAxis()
            {
                return (this.Normal.X != 0 && (this.Normal.Y != 0 || this.Normal.Z != 0)) ||
                    (this.Normal.Y != 0 && (this.Normal.X != 0 || this.Normal.Z != 0));
            }

            // TODO:  As with Position, will yield incorrect positions if mirroring on multiple axes
            // fix mirrored positions for multiple-axis mirroring
            public override Bounds Bounds()
            {
                if (this.isMoreThanOneAxis())
                {
                    throw new NotSupportedException("Getting the position of an object that's been mirrored on more than one axis is not currently supported.");
                }

                var oldBounds = this.obj.Bounds();
                var newBottomLeft = new Vector3(this.Normal.X != 0 ? oldBounds.BottomLeft.X * -1 : oldBounds.BottomLeft.X,
                                                this.Normal.Y != 0 ? oldBounds.BottomLeft.Y * -1 : oldBounds.BottomLeft.Y,
                                                this.Normal.Z != 0 ? oldBounds.BottomLeft.Z * -1 : oldBounds.BottomLeft.Z);

                var newTopRight = new Vector3(this.Normal.X != 0 ? oldBounds.TopRight.X * -1 : oldBounds.TopRight.X,
                                              this.Normal.Y != 0 ? oldBounds.TopRight.Y * -1 : oldBounds.TopRight.Y,
                                              this.Normal.Z != 0 ? oldBounds.TopRight.Z * -1 : oldBounds.TopRight.Z);

                return new Bounds(newBottomLeft, newTopRight);
            }

            private Bindings bindings = new Bindings(new Dictionary<string, string>() {
            {"normal", "normal"}
        });
            public override void Bind(string property, Variable variable)
            {
                var bindableVec = this.Normal as BindableVector;

                if (bindableVec != null && property == "x" || property == "y" || property == "z")
                {
                    bindableVec.Bind(property, variable);
                }
                else
                {
                    this.bindings.Add<MirroredObject>(this, property, variable);
                }
            }
        }

        #endregion

        #region HulledObject
        /// <summary>
        /// Creates an object that's the convex hull of child objects
        /// </summary>
        internal class HulledObject : MultiStatementObject
        {
            internal HulledObject(IEnumerable<OSCADObject> children) : base("hull()", children)
            {
            }
        }
        #endregion

        #region MinkowskiedObject
        /// <summary>
        /// Creates an object that's the minkowski sum of child objects
        /// </summary>
        internal class MinkowskiedObject : MultiStatementObject
        {

            internal MinkowskiedObject(IEnumerable<OSCADObject> children) : base("minkowski()", children)
            {
            }

            public override Vector3 Position()
            {
                throw new NotSupportedException("Position is not supported on Minkowskied objects.");
            }

            public override Bounds Bounds()
            {
                throw new NotSupportedException("Bounds is not supported on Minkowskied objects.");
            }
        }
        #endregion
    }
}
