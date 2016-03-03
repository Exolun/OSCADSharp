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

            if (x != null)
                this.Bind("x", x);
            if (y != null)
                this.Bind("y", y);
            if (z != null)
                this.Bind("z", z);
        }

        public override string ToString()
        {
            string translation = this.bindings.Contains("vector") ? this.bindings.Get("vector").BoundVariable.Name : this.Vector.ToString();

            string translateCommmand = String.Format("translate(v = {0})", translation);
            var formatter = new SingleBlockFormatter(translateCommmand, this.obj.ToString());
            return formatter.ToString();
        }

        public override OSCADObject Clone()
        {
            return new TranslatedObject(this.obj.Clone(), this.Vector)
            {
                Name = this.Name
            };
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

        private Bindings.Bindings bindings = new Bindings.Bindings(new Dictionary<string, string>() {
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
