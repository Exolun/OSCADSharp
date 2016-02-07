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
    public class ColoredObject : OSCADObject
    {
        #region Attributes
        public string Color { get; set; } = "Yellow";
        public double Opacity { get; set; } = 1.0;
        #endregion

        private OSCADObject obj;

        /// <summary>
        /// Creates a colorized object
        /// </summary>
        /// <param name="obj">The object(s) to which color will be applied</param>
        /// <param name="Color">The string-wise name of the color to be applied</param>
        /// <param name="opacity">Opacity from 0.0 to 1.0 </param>
        public ColoredObject(OSCADObject obj, string Color = "Yellow", double opacity = 1.0)
        {
            this.obj = obj;
            this.Color = Color;
            this.Opacity = opacity;
        }

        public override string ToString()
        {
            string colorCommand = String.Format("color(\"{0}\", {1})", this.Color, this.Opacity);
            var formatter = new BlockFormatter(colorCommand, this.obj.ToString());            
            return formatter.ToString();
        }
    }
}
