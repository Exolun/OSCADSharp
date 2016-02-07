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
    public class TranslatedObject
    {
        public Vector3 Vector { get; set; }


        public override string ToString()
        {
            return String.Format("translate(v = [{0}, {1}, {2}])", 
                this.Vector.X.ToString(), this.Vector.Y.ToString(), this.Vector.Z.ToString());
        }
    }
}
