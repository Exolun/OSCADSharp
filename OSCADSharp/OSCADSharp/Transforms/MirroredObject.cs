using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Transforms
{
    /// <summary>
    /// An object that's mirrored on a plane
    /// </summary>
    public class MirroredObject
    {
        /// <summary>
        /// The normal vector of a plane intersecting the origin of the object,
        /// through which to mirror it.
        /// </summary>
        public Vector3 Normal { get; set; } = new Vector3();

        public override string ToString()
        {
            return String.Format("mirror([{0}, {1}, {2}])", this.Normal.X, this.Normal.Y, this.Normal.Z);
        }
    }
}
