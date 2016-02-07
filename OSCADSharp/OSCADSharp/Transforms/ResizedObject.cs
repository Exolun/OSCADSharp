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
    public class ResizedObject
    {
        public Vector3 Size { get; set; }

        public override string ToString()
        {
            return String.Format("resize([{0}, {1}, {2}])", this.Size.X.ToString(), 
                this.Size.Y.ToString(), this.Size.Z.ToString());
        }
    }
}
