using OSCADSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.Spatial;

namespace OSCADSharp.Transforms
{
    /// <summary>
    /// An object that's been rescaled
    /// </summary>
    internal class ScaledObject : SingleStatementObject
    {
        /// <summary>
        /// The scale factor to be applied
        /// </summary>
        internal Vector3 ScaleFactor { get; set; } = new Vector3(1, 1, 1);

        /// <summary>
        /// Creates a scaled object
        /// </summary>
        /// <param name="obj">Object(s) to be scaled</param>
        /// <param name="scale">Scale factor in x/y/z components</param>
        internal ScaledObject(OSCADObject obj, Vector3 scale) : base(obj)
        {
            this.ScaleFactor = scale;
        }

        public override string ToString()
        {
            string scaleCommand = String.Format("scale(v = [{0}, {1}, {2}])",
                this.ScaleFactor.X.ToString(), this.ScaleFactor.Y.ToString(), this.ScaleFactor.Z.ToString());
            var formatter = new SingleBlockFormatter(scaleCommand, this.obj.ToString());
            return formatter.ToString();
        }

        public override OSCADObject Clone()
        {
            return new ScaledObject(this.obj.Clone(), this.ScaleFactor)
            {
                Name = this.Name
            };
        }
        
        public override Vector3 Position()
        {
            return obj.Position() * this.ScaleFactor;
        }

        public override Bounds Bounds()
        {
            var oldBounds = obj.Bounds();
            return new Bounds(oldBounds.BottomLeft * this.ScaleFactor, oldBounds.TopRight * this.ScaleFactor);
        }
    }
}
