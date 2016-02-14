using OSCADSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Transforms
{
    /// <summary>
    /// An object that's been resized to a specified set of X/Y/Z dimensions
    /// </summary>
    internal class ResizedObject : OSCADObject, IMimicer
    {
        /// <summary>
        /// Size of the object in terms of X/Y/Z
        /// </summary>
        internal Vector3 Size { get; set; }
        private OSCADObject obj;

        /// <summary>
        /// Creates a resized object
        /// </summary>
        /// <param name="obj">The object(s) to be resized</param>
        /// <param name="size">The size to resize to, in terms of x/y/z dimensions</param>
        internal ResizedObject(OSCADObject obj, Vector3 size)
        {
            this.obj = obj;
            this.Size = size;

            this.children.Add(obj);
        }

        public override string ToString()
        {
            string resizeCommand = String.Format("resize([{0}, {1}, {2}])", this.Size.X.ToString(),
                this.Size.Y.ToString(), this.Size.Z.ToString());
            var formatter = new BlockFormatter(resizeCommand, this.obj.ToString());
            return formatter.ToString();
        }

        public override OSCADObject Clone()
        {
            return new ResizedObject(this.obj.Clone(), this.Size);
        }

        public OSCADObject MimicObject(OSCADObject obj)
        {
            return new ResizedObject(obj, this.Size);
        }
    }
}
