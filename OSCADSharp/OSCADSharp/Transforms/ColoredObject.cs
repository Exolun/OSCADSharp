using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Transforms
{
    /// <summary>
    /// An object that has color and/or opacity applied to it
    /// </summary>
    public class ColoredObject
    {
        #region Attributes
        public string Color { get; set; } = "Yellow";
        public double Opacity { get; set; } = 1.0;
        #endregion

        public override string ToString()
        {
            return String.Format("color(\"{0}\", {1})", this.Color, this.Opacity);
        }
    }
}
