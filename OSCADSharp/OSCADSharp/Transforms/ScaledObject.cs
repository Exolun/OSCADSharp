using OSCADSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.Spatial;
using OSCADSharp.Bindings;

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
        internal Vector3 ScaleFactor { get; set; } = new BindableVector(1, 1, 1);

        /// <summary>
        /// Creates a scaled object
        /// </summary>
        /// <param name="obj">Object(s) to be scaled</param>
        /// <param name="scale">Scale factor in x/y/z components</param>
        internal ScaledObject(OSCADObject obj, Vector3 scale) : base(obj)
        {
            this.ScaleFactor = new BindableVector(scale);
        }

        public override string ToString()
        {
            string scale = this.bindings.Contains("scalefactor") ? this.bindings.Get("scalefactor").BoundVariable.Name : this.ScaleFactor.ToString();

            string scaleCommand = String.Format("scale(v = {0})", scale);
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

        private Bindings.Bindings bindings = new Bindings.Bindings(new Dictionary<string, string>() {
            { "scalefactor", "scalefactor" }
        });
        public override void Bind(string property, Variable variable)
        {
            var bindableVec = this.ScaleFactor as BindableVector;
            property = property == "scale" ? "scalefactor" : property;

            if (bindableVec != null && property == "x" || property == "y" || property == "z")
            {
                bindableVec.Bind(property, variable);
            }
            else
            {
                this.bindings.Add<ScaledObject>(this, property, variable);
            }
        }
    }
}
