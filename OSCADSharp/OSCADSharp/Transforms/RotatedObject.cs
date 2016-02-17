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
    internal class RotatedObject : OSCADObject, IMimic
    {
        /// <summary>
        /// The angle to rotate, in terms of X/Y/Z euler angles
        /// </summary>
        internal Vector3 Angle { get; set; } = new Vector3();
        private OSCADObject obj;

        /// <summary>
        /// Creates an object with rotation applied
        /// </summary>
        /// <param name="obj">The object being rotated</param>
        /// <param name="angle">The angle to rotate</param>
        internal RotatedObject(OSCADObject obj, Vector3 angle)
        {
            this.obj = obj;
            this.Angle = angle;

            this.children.Add(obj);
        }

        public override string ToString()
        {
            string rotateCommand = String.Format("rotate([{0}, {1}, {2}])",
                this.Angle.X.ToString(), this.Angle.Y.ToString(), this.Angle.Z.ToString());
            var formatter = new SingleBlockFormatter(rotateCommand, this.obj.ToString());
            return formatter.ToString();
        }

        public override OSCADObject Clone()
        {
            return new RotatedObject(this.obj.Clone(), this.Angle);
        }

        public OSCADObject MimicObject(OSCADObject obj)
        {
            return new RotatedObject(obj, this.Angle);
        }

        public override Vector3 Position()
        {
            return Matrix.GetRotatedPoint(this.obj.Position(), this.Angle.X, this.Angle.Y, this.Angle.Z);
        }
    }
}
