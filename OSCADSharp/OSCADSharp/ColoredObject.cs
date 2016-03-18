using OSCADSharp.DataBinding;
using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// An object that has color and/or opacity applied to it
    /// </summary>
    internal class ColoredObject : SingleStatementObject
    {
        #region Attributes
        internal string ColorName { get; set; } = "Yellow";
        internal double Opacity { get; set; } = 1.0;
        #endregion
        
        /// <summary>
        /// Creates a colorized object
        /// </summary>
        /// <param name="obj">The object(s) to which color will be applied</param>
        /// <param name="color">The string-wise name of the color to be applied</param>
        /// <param name="opacity">Opacity from 0.0 to 1.0 </param>
        internal ColoredObject(OSCADObject obj, string color = "Yellow", double opacity = 1.0) : base(obj)
        {
            this.ColorName = color;
            this.Opacity = opacity;
        }

        /// <summary>
        /// Creates a colorized object with predefined bindings
        /// </summary>
        /// <param name="obj">The object(s) to which color will be applied</param>
        /// <param name="colorName"></param>
        /// <param name="opacity"></param>
        internal ColoredObject(OSCADObject obj, Variable colorName, Variable opacity) : base(obj)
        {
            this.Bind("color", colorName);
            this.BindIfVariableNotNull("opacity", opacity);
        }

        public override string ToString()
        {
            string colorName = this.bindings.Contains("color") ? this.bindings.Get("color").BoundVariable.Text :
                "\""+this.ColorName+"\"";
            string opacity = this.bindings.Contains("opacity") ? this.bindings.Get("opacity").BoundVariable.Text
                : this.Opacity.ToString();

            string colorCommand = String.Format("color({0}, {1})", colorName, opacity);
            var formatter = new SingleBlockFormatter(colorCommand, this.obj.ToString());            
            return formatter.ToString();
        }

        public override OSCADObject Clone()
        {
            return new ColoredObject(this.obj.Clone(), this.ColorName, this.Opacity)
            {
                Name = this.Name,
                bindings = this.bindings.Clone()
            };
        }
        
        public override Vector3 Position()
        {
            return this.obj.Position();
        }

        public override Bounds Bounds()
        {
            return this.obj.Bounds();
        }

        private Bindings bindings = new Bindings(new Dictionary<string, string>() {
            {"color", "color" },
            {"opacity", "opacity" }
        });

        public override void Bind(string property, Variable variable)
        {
            this.bindings.Add<ColoredObject>(this, property, variable);
        }
    }
}
