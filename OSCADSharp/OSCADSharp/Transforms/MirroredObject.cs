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
    /// An object that's mirrored on a plane
    /// </summary>
    internal class MirroredObject : OSCADObject, IMimic
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

            this.children.Add(obj);
        }
        
        public override string ToString()
        {
            string mirrorCommand = String.Format("mirror([{0}, {1}, {2}])", this.Normal.X, this.Normal.Y, this.Normal.Z);
            var formatter = new SingleBlockFormatter(mirrorCommand, this.obj.ToString());
            return formatter.ToString();
        }

        public override OSCADObject Clone()
        {
            return new MirroredObject(this.obj.Clone(), this.Normal);
        }

        public OSCADObject MimicObject(OSCADObject obj)
        {
            return new MirroredObject(obj, this.Normal);
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

        public override Bounds Bounds()
        {
            throw new NotImplementedException();
        }
    }
}
