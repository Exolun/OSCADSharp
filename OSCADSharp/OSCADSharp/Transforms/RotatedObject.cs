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
        public Vector3 Angle { get; set; } = new Vector3();

        public override string ToString()
        {
            return String.Format("rotate([{0}, {1}, {2}])", 
                this.Angle.X.ToString(), this.Angle.Y.ToString(), this.Angle.Z.ToString());
        }
    }
}
