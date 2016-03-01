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
            this.Vector = vector;
        }

        public override string ToString()
        {
            string translateCommmand = String.Format("translate(v = [{0}, {1}, {2}])",
                this.Vector.X.ToString(), this.Vector.Y.ToString(), this.Vector.Z.ToString());
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

        public override void Bind(string property, Variable variable)
        {
            throw new NotImplementedException();
        }
    }
}
