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
    public class RotatedObject
    {
        /// <summary>
        /// The angle to rotate, in terms of X/Y/Z euler angles
        /// </summary>
        public Vector3 Angle { get; set; } = new Vector3();
        private OSCADObject obj;

        public RotatedObject(OSCADObject obj, Vector3 angle)
        {
            this.obj = obj;
            this.Angle = angle;
        }

        public override string ToString()
        {
            string rotateCommand = String.Format("rotate([{0}, {1}, {2}])",
                this.Angle.X.ToString(), this.Angle.Y.ToString(), this.Angle.Z.ToString());
            var formatter = new BlockFormatter(rotateCommand, this.obj.ToString());
            return formatter.ToString();
        }
    }
}
