using OSCADSharp.DataBinding;
using OSCADSharp.Spatial;
using OSCADSharp.Utility;
using System;
using System.Collections.Generic;

namespace OSCADSharp
{
    /// <summary>
    /// An object or objects that have been moved along the specified vector
    /// </summary>
    internal class TranslatedObject : SingleStatementObject
    {
        internal Vector3 Vector { get; set; }
        
        /// <summary>
        /// Creates a translated object
        /// </summary>
        /// <param name="obj">Object(s) to translate</param>
        /// <param name="vector">Amount to translate by</param>
        internal TranslatedObject(OSCADObject obj, Vector3 vector) : base(obj)
        {
            this.Vector = new BindableVector(vector);
        }

        internal TranslatedObject(OSCADObject obj, Variable normal) : base(obj)
        {
            this.Bind("vector", normal);
        }

        internal TranslatedObject(OSCADObject obj, Vector3 vector, Variable x, Variable y, Variable z) : base(obj)
        {
            this.Vector = new BindableVector(vector);

            this.BindIfVariableNotNull("x", x);
            this.BindIfVariableNotNull("y", y);
            this.BindIfVariableNotNull("z", z);
        }

        internal TranslatedObject(OSCADObject obj) : base(obj)
        {
        }

        public override string ToString()
        {
            string translation = this.bindings.Contains("vector") ? this.bindings.Get("vector").BoundVariable.Text : this.Vector.ToString();

            string translateCommmand = String.Format("translate(v = {0})", translation);
            var formatter = new SingleBlockFormatter(translateCommmand, this.obj.ToString());
            return formatter.ToString();
        }

        public override OSCADObject Clone()
        {
            var bindableVec = this.Vector as BindableVector;

            var clone = new TranslatedObject(this.obj.Clone())
            {
                Name = this.Name,
                bindings = this.bindings.Clone(),
                Vector = bindableVec != null ? bindableVec.Clone() : this.Vector.Clone()
            };

            return clone;
        }
        
        public override Vector3 Position()
        {
            return this.obj.Position() + this.Vector;
        }

        public override Bounds Bounds()
        {
            var oldBounds = obj.Bounds();
            return new Bounds(oldBounds.BottomLeft + this.Vector, oldBounds.TopRight + this.Vector);
        }

        private Bindings bindings = new Bindings(new Dictionary<string, string>() {
            { "vector", "vector" }
        });
        public override void Bind(string property, Variable variable)
        {
            var bindableVec = this.Vector as BindableVector;

            if (bindableVec != null && property == "x" || property == "y" || property == "z")
            {
                bindableVec.Bind(property, variable);
            }
            else
            {
                this.bindings.Add<TranslatedObject>(this, property, variable);
            }
        }
    }
}
