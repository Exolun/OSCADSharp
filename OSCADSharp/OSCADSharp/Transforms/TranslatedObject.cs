using OSCADSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Transforms
{
    /// <summary>
    /// An object or objects that have been moved along the specified vector
    /// </summary>
    internal class TranslatedObject : OSCADObject, IMimicer
    {
        internal Vector3 Vector { get; set; }
        private OSCADObject obj;

        /// <summary>
        /// Creates a translated object
        /// </summary>
        /// <param name="obj">Object(s) to translate</param>
        /// <param name="vector">Amount to translate by</param>
        internal TranslatedObject(OSCADObject obj, Vector3 vector)
        {
            this.obj = obj;
            this.Vector = vector;

            this.children.Add(obj);
        }

        public override string ToString()
        {
            string translateCommmand = String.Format("translate(v = [{0}, {1}, {2}])",
                this.Vector.X.ToString(), this.Vector.Y.ToString(), this.Vector.Z.ToString());
            var formatter = new BlockFormatter(translateCommmand, this.obj.ToString());
            return formatter.ToString();
        }

        public override OSCADObject Clone()
        {
            return new TranslatedObject(this.obj.Clone(), this.Vector);
        }

        public OSCADObject MimicObject(OSCADObject obj)
        {
            return new TranslatedObject(obj, this.Vector);
        }
    }
}
