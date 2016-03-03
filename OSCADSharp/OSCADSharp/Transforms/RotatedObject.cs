using OSCADSharp.Bindings;
using OSCADSharp.Scripting;
using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Transforms
{
    /// <summary>
    /// An object with rotation applied
    /// </summary>
    internal class RotatedObject : SingleStatementObject
    {
        /// <summary>
        /// The angle to rotate, in terms of X/Y/Z euler angles
        /// </summary>
        internal Vector3 Angle { get; set; } = new BindableVector();

        /// <summary>
        /// Creates an object with rotation applied
        /// </summary>
        /// <param name="obj">The object being rotated</param>
        /// <param name="angle">The angle to rotate</param>
        internal RotatedObject(OSCADObject obj, Vector3 angle) : base(obj)
        {
            this.Angle = new BindableVector(angle);
        }

        public override string ToString()
        {
            string angle = this.bindings.Contains("angle") ? this.bindings.Get("angle").BoundVariable.Name : this.Angle.ToString();

            string rotateCommand = String.Format("rotate({0})", angle.ToString());
            var formatter = new SingleBlockFormatter(rotateCommand, this.obj.ToString());
            return formatter.ToString();
        }

        public override OSCADObject Clone()
        {
            return new RotatedObject(this.obj.Clone(), this.Angle)
            {
                Name = this.Name
            };
        }
        
        public override Vector3 Position()
        {
            return Matrix.GetRotatedPoint(this.obj.Position(), this.Angle.X, this.Angle.Y, this.Angle.Z);
        }

        public override Bounds Bounds()
        {
            var oldBounds = obj.Bounds();
            return new Bounds(Matrix.GetRotatedPoint(oldBounds.BottomLeft, this.Angle.X, this.Angle.Y, this.Angle.Z),
                              Matrix.GetRotatedPoint(oldBounds.TopRight, this.Angle.X, this.Angle.Y, this.Angle.Z));
        }

        private Bindings.Bindings bindings = new Bindings.Bindings(new Dictionary<string, string>() {
            { "angle", "angle" }
        });
        public override void Bind(string property, Variable variable)
        {
            var bindableVec = this.Angle as BindableVector;

            if (bindableVec != null && property == "x" || property == "y" || property == "z")
            {
                bindableVec.Bind(property, variable);
            }
            else
            {
                this.bindings.Add<RotatedObject>(this, property, variable);
            }
        }
    }
}
