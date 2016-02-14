using OSCADSharp.Scripting;
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
    internal class ColoredObject : OSCADObject, IMimic
    {
        #region Attributes
        internal string ColorName { get; set; } = "Yellow";
        internal double Opacity { get; set; } = 1.0;
        #endregion

        private OSCADObject obj;

        /// <summary>
        /// Creates a colorized object
        /// </summary>
        /// <param name="obj">The object(s) to which color will be applied</param>
        /// <param name="color">The string-wise name of the color to be applied</param>
        /// <param name="opacity">Opacity from 0.0 to 1.0 </param>
        internal ColoredObject(OSCADObject obj, string color = "Yellow", double opacity = 1.0)
        {
            this.obj = obj;
            this.ColorName = color;
            this.Opacity = opacity;

            this.children.Add(obj);
        }

        public override string ToString()
        {
            string colorCommand = String.Format("color(\"{0}\", {1})", this.ColorName, this.Opacity);
            var formatter = new BlockFormatter(colorCommand, this.obj.ToString());            
            return formatter.ToString();
        }

        public override OSCADObject Clone()
        {
            return new ColoredObject(this.obj.Clone(), this.ColorName, this.Opacity);
        }

        public OSCADObject MimicObject(OSCADObject obj)
        {
            return new ColoredObject(obj, this.ColorName, this.Opacity);
        }
    }
}
