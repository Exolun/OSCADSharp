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
        /// <summary>
        /// The scale factor to be applied
        /// </summary>
        public Vector3 Scale { get; set; } = new Vector3(1, 1, 1);
        private OSCADObject obj;

        /// <summary>
        /// Creates a scaled object
        /// </summary>
        /// <param name="obj">Object(s) to be scaled</param>
        /// <param name="scale">Scale factor in x/y/z components</param>
        public ScaledObject(OSCADObject obj, Vector3 scale)
        {
            this.obj = obj;
            this.Scale = scale;
        }

        public override string ToString()
        {
            string scaleCommand = String.Format("scale(v = [{0}, {1}, {2}])",
                this.Scale.X.ToString(), this.Scale.Y.ToString(), this.Scale.Z.ToString());
            var formatter = new BlockFormatter(scaleCommand, this.obj.ToString());
            return formatter.ToString();
        }
    }
}
