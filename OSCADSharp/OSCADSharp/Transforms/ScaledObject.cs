using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Transforms
{
    /// <summary>
    /// An object that's been rescaled
    /// </summary>
    public class ScaledObject
    {
        public Vector3 Scale { get; set; } = new Vector3(1, 1, 1);

        public override string ToString()
        {
            return String.Format("scale(v = [{0}, {1}, {2}])", 
                this.Scale.X.ToString(), this.Scale.Y.ToString(), this.Scale.Z.ToString());
        }
    }
}
