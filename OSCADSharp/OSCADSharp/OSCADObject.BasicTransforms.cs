using OSCADSharp.DataBinding;
using OSCADSharp.Spatial;
using OSCADSharp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    public abstract partial class OSCADObject
    {
        #region ColoredObject
        /// <summary>
        /// An OSCADObject that has color and/or opacity applied to it
        /// </summary>
        private class ColoredObject : SingleStatementObject
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
                this.BindIfVariableNotNull("color", colorName);
                this.BindIfVariableNotNull("opacity", opacity);
            }

            public override string ToString()
            {
                string colorName = this.bindings.Contains("color") ? this.bindings.Get("color").BoundVariable.Text :
                    "\"" + this.ColorName + "\"";
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
        #endregion

        #region ResizedObject
        /// <summary>
        /// An OSCADObject that's been resized to a specified set of X/Y/Z dimensions
        /// </summary>
        private class ResizedObject : SingleStatementObject
        {
            /// <summary>
            /// Size of the object in terms of X/Y/Z
            /// </summary>
            internal Vector3 Size { get; set; }

            /// <summary>
            /// Creates a resized object
            /// </summary>
            /// <param name="obj">The object(s) to be resized</param>
            /// <param name="size">The size to resize to, in terms of x/y/z dimensions</param>
            internal ResizedObject(OSCADObject obj, Vector3 size) : base(obj)
            {
                Size = new BindableVector(size);
            }

            internal ResizedObject(OSCADObject obj, Variable size) : base(obj)
            {
                this.BindIfVariableNotNull("size", size);
            }

            internal ResizedObject(OSCADObject obj, Vector3 size, Variable x, Variable y, Variable z) : base(obj)
            {
                this.Size = new BindableVector(size);

                this.BindIfVariableNotNull("x", x);
                this.BindIfVariableNotNull("y", y);
                this.BindIfVariableNotNull("z", z);
            }

            internal ResizedObject(OSCADObject obj) : base(obj)
            {
            }

            public override string ToString()
            {
                string size = this.bindings.Contains("size") ? this.bindings.Get("size").BoundVariable.Text : this.Size.ToString();

                string resizeCommand = String.Format("resize({0})", size);
                var formatter = new SingleBlockFormatter(resizeCommand, this.obj.ToString());
                return formatter.ToString();
            }

            public override OSCADObject Clone()
            {
                var bndSize = this.Size as BindableVector;

                return new ResizedObject(this.obj.Clone())
                {
                    Name = this.Name,
                    bindings = this.bindings.Clone(),
                    Size = bndSize != null ? bndSize.Clone() : this.Size.Clone()
                };
            }

            public override Vector3 Position()
            {
                var bounds = this.Bounds();
                return Vector3.Average(bounds.BottomLeft, bounds.TopRight);
            }

            public override Bounds Bounds()
            {
                var oldBounds = obj.Bounds();

                double xScaleFactor = this.Size.X > 0 ? this.Size.X / Math.Abs(oldBounds.XMax - oldBounds.XMin) : 1;
                double yScaleFactor = this.Size.Y > 0 ? this.Size.Y / Math.Abs(oldBounds.YMax - oldBounds.YMin) : 1;
                double zScaleFactor = this.Size.Z > 0 ? this.Size.Z / Math.Abs(oldBounds.ZMax - oldBounds.ZMin) : 1;
                Vector3 scaleMultiplier = new Vector3(xScaleFactor, yScaleFactor, zScaleFactor);

                return new Bounds(oldBounds.BottomLeft * scaleMultiplier, oldBounds.TopRight * scaleMultiplier);
            }


            private Bindings bindings = new Bindings(new Dictionary<string, string>() {
                { "size", "size" }
            });

            public override void Bind(string property, Variable variable)
            {
                var bindableVec = this.Size as BindableVector;

                if (bindableVec != null && property == "x" || property == "y" || property == "z")
                {
                    bindableVec.Bind(property, variable);
                }
                else
                {
                    this.bindings.Add<ResizedObject>(this, property, variable);
                }
            }
        }
        #endregion

        #region RotatedObject
        /// <summary>
        /// An OSCADObject with rotation applied
        /// </summary>
        private class RotatedObject : SingleStatementObject
        {
            /// <summary>
            /// The angle to rotate, in terms of X/Y/Z euler angles
            /// </summary>
            internal Vector3 Angle { get; set; } = new BindableVector();

            /// <summary>
            /// Creates an object with rotation applied
            /// </summary>
            /// <param name="obj">The object being rotated</param>
            /// <param name="angle">The angle to rotate</param>
            internal RotatedObject(OSCADObject obj, Vector3 angle) : base(obj)
            {
                this.Angle = new BindableVector(angle);
            }

            internal RotatedObject(OSCADObject obj, Variable normal) : base(obj)
            {
                this.BindIfVariableNotNull("angle", normal);
            }

            internal RotatedObject(OSCADObject obj, Vector3 angle, Variable x, Variable y, Variable z) : base(obj)
            {
                this.Angle = new BindableVector(angle);

                this.BindIfVariableNotNull("x", x);
                this.BindIfVariableNotNull("y", y);
                this.BindIfVariableNotNull("z", z);
            }

            public override string ToString()
            {
                string angle = this.bindings.Contains("angle") ? this.bindings.Get("angle").BoundVariable.Text : this.Angle.ToString();

                string rotateCommand = String.Format("rotate({0})", angle.ToString());
                var formatter = new SingleBlockFormatter(rotateCommand, this.obj.ToString());
                return formatter.ToString();
            }

            public override OSCADObject Clone()
            {
                return new RotatedObject(this.obj.Clone(), this.Angle)
                {
                    Name = this.Name,
                    bindings = this.bindings.Clone()
                };
            }

            public override Vector3 Position()
            {
                return Matrix.GetRotatedPoint(this.obj.Position(), this.Angle.X, this.Angle.Y, this.Angle.Z);
            }

            public override Bounds Bounds()
            {
                var oldBounds = obj.Bounds();
                return new Bounds(Matrix.GetRotatedPoint(oldBounds.BottomLeft, this.Angle.X, this.Angle.Y, this.Angle.Z),
                                  Matrix.GetRotatedPoint(oldBounds.TopRight, this.Angle.X, this.Angle.Y, this.Angle.Z));
            }

            private Bindings bindings = new Bindings(new Dictionary<string, string>() {
                { "angle", "angle" }
            });

            public override void Bind(string property, Variable variable)
            {
                var bindableVec = this.Angle as BindableVector;

                if (bindableVec != null && property == "x" || property == "y" || property == "z")
                {
                    bindableVec.Bind(property, variable);
                }
                else
                {
                    this.bindings.Add<RotatedObject>(this, property, variable);
                }
            }
        }

        #endregion

        #region ScaledObject
        /// <summary>
        /// An object that's been rescaled
        /// </summary>
        private class ScaledObject : SingleStatementObject
        {
            /// <summary>
            /// The scale factor to be applied
            /// </summary>
            internal Vector3 ScaleFactor { get; set; } = new BindableVector(1, 1, 1);

            /// <summary>
            /// Creates a scaled object
            /// </summary>
            /// <param name="obj">Object(s) to be scaled</param>
            /// <param name="scale">Scale factor in x/y/z components</param>
            internal ScaledObject(OSCADObject obj, Vector3 scale) : base(obj)
            {
                this.ScaleFactor = new BindableVector(scale);
            }

            internal ScaledObject(OSCADObject obj, Variable normal) : base(obj)
            {
                this.BindIfVariableNotNull("scalefactor", normal);
            }

            internal ScaledObject(OSCADObject obj, Vector3 scale, Variable x, Variable y, Variable z) : base(obj)
            {
                this.ScaleFactor = new BindableVector(scale);

                this.BindIfVariableNotNull("x", x);
                this.BindIfVariableNotNull("y", y);
                this.BindIfVariableNotNull("z", z);
            }

            public override string ToString()
            {
                string scale = this.bindings.Contains("scalefactor") ? this.bindings.Get("scalefactor").BoundVariable.Text : this.ScaleFactor.ToString();

                string scaleCommand = String.Format("scale(v = {0})", scale);
                var formatter = new SingleBlockFormatter(scaleCommand, this.obj.ToString());
                return formatter.ToString();
            }

            public override OSCADObject Clone()
            {
                return new ScaledObject(this.obj.Clone(), this.ScaleFactor)
                {
                    Name = this.Name,
                    bindings = this.bindings.Clone()
                };
            }

            public override Vector3 Position()
            {
                return obj.Position() * this.ScaleFactor;
            }

            public override Bounds Bounds()
            {
                var oldBounds = obj.Bounds();
                return new Bounds(oldBounds.BottomLeft * this.ScaleFactor, oldBounds.TopRight * this.ScaleFactor);
            }

            private Bindings bindings = new Bindings(new Dictionary<string, string>() {
                 { "scalefactor", "scalefactor" }
            });
            public override void Bind(string property, Variable variable)
            {
                var bindableVec = this.ScaleFactor as BindableVector;
                property = property == "scale" ? "scalefactor" : property;

                if (bindableVec != null && property == "x" || property == "y" || property == "z")
                {
                    bindableVec.Bind(property, variable);
                }
                else
                {
                    this.bindings.Add<ScaledObject>(this, property, variable);
                }
            }
        }


        #endregion

        #region TranslatedObject
        /// <summary>
        /// An OSCADObject or objects that have been moved along the specified vector
        /// </summary>
        private class TranslatedObject : SingleStatementObject
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
                this.BindIfVariableNotNull("vector", normal);
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
        #endregion
    }
}
