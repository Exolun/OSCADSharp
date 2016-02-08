using OSCADSharp.Scripting;
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
    internal class MirroredObject : OSCADObject
    {
        /// <summary>
        /// The normal vector of a plane intersecting the origin of the object,
        /// through which to mirror it.
        /// </summary>
        internal Vector3 Normal { get; set; } = new Vector3();

        private OSCADObject obj;

        /// <summary>
        /// Creates an object that's mirrored on a plane
        /// </summary>
        /// <param name="obj">The object(s) to be mirrored</param>
        /// <param name="normal">The normal vector of the plane on the object's origin to mirror upon</param>
        internal MirroredObject(OSCADObject obj, Vector3 normal)
        {
            this.obj = obj;
            this.Normal = normal;
        }
        
        public override string ToString()
        {
            string mirrorCommand = String.Format("mirror([{0}, {1}, {2}])", this.Normal.X, this.Normal.Y, this.Normal.Z);
            var formatter = new BlockFormatter(mirrorCommand, this.obj.ToString());
            return formatter.ToString();
        }

        public override OSCADObject Clone()
        {
            return new MirroredObject(this.obj, this.Normal);
        }
    }
}
