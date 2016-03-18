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
    /// <summary>
    /// A Cylinder geometry
    /// </summary>
    public class Cylinder : OSCADObject
    {
        #region Attributes
        private bool center = false;
        private BindableBoolean centerBinding = new BindableBoolean("center");

        /// <summary>
        /// Height of the cylinder or cone
        /// </summary>
        public double Height { get; set; } = 1;

        /// <summary>
        /// Radius of cylinder. r1 = r2 = r.
        /// </summary>
        public double Radius {
            get
            {
                return (Radius1 + Radius2) / 2;
            }
            set
            {
                this.Radius1 = value;
                this.Radius2 = value;
            }
        }

        /// <summary>
        /// Radius, bottom of cone.
        /// </summary>
        public double Radius1 { get; set; } = 1;

        /// <summary>
        /// Radius, top of cone.
        /// </summary>
        public double Radius2 { get; set; } = 1;

        /// <summary>
        /// Diameter of cylinder. r1 = r2 = d /2.
        /// </summary>
        public double Diameter
        {
            get { return this.Radius * 2; }
            set { this.Radius = value / 2; }
        }

        /// <summary>
        /// Diameter, bottom of cone. r1 = d1 /2
        /// </summary>
        public double Diameter1
        {
            get { return this.Radius1 * 2; }
            set { this.Radius1 = value / 2; }
        }

        /// <summary>
        /// Diameter, top of cone. r2 = d2 /2
        /// </summary>
        public double Diameter2
        {
            get { return this.Radius2 * 2; }
            set { this.Radius2 = value / 2; }
        }

        /// <summary>
        /// Denotes the initial positioning of the cylinder
        /// false: (default), z ranges from 0 to h
        /// true: z ranges from -h/2 to +h/2
        /// </summary>
        public bool Center
        {
            get { return this.center; }
            set
            {
                this.center = value;
                this.centerBinding.InnerValue = this.center.ToString().ToLower();
            }
        }

        /// <summary>
        /// Minimum angle (in degrees) of each cylinder fragment.
        /// ($fa in OpenSCAD)
        /// </summary>
        public int? MinimumAngle { get; set; }

        /// <summary>
        /// Minimum circumferential length of each fragment.
        /// ($fs in OpenSCAD)
        /// </summary>
        public int? MinimumCircumferentialLength { get; set; }

        /// <summary>
        /// Number of fragments in 360 degrees. Values of 3 or more override MinimumAngle and MinimumCircumferentialLength
        /// ($fn in OpenSCAD)
        /// </summary>
        public int? Resolution { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a cylinder with the default initialization values
        /// </summary>
        public Cylinder()
        {
        }

        /// <summary>
        /// Creates a cylinder with the specified diameter and centering
        /// </summary>
        /// <param name="diameter">Diameter of the cylinder</param>
        /// <param name="height">Height of the cylinder</param>
        /// <param name="center">Determines whether the cylinder should be centered on the z-axis, if false the base will start on the Z axis</param>
        public Cylinder(double diameter = 2, double height = 1, bool center = false)
        {
            this.Diameter = diameter;
            this.Height = height;
            this.Center = center;
        }

        /// <summary>
        /// Creates a cylinder with one or more pre-bound variables
        /// </summary>
        /// <param name="diameter1"></param>
        /// <param name="diameter2"></param>
        /// <param name="height"></param>
        /// <param name="center"></param>
        /// <param name="resolution"></param>
        /// <param name="minimumangle"></param>
        /// <param name="minimumcircumferentiallength"></param>
        public Cylinder(Variable diameter1 = null, Variable diameter2 = null, Variable height = null, 
            Variable center = null, Variable resolution = null, Variable minimumangle = null, Variable minimumcircumferentiallength = null)
        {
            this.BindIfVariableNotNull("diameter1", diameter1);
            this.BindIfVariableNotNull("diameter2", diameter2);
            this.BindIfVariableNotNull("height", height);
            this.BindIfVariableNotNull("center", center);
            this.BindIfVariableNotNull("resolution", resolution);
            this.BindIfVariableNotNull("minimumangle", minimumangle);
            this.BindIfVariableNotNull("minimumcircumferentiallength", minimumcircumferentiallength);
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Converts this object to an OpenSCAD script
        /// </summary>
        /// <returns>Script for this object</returns>
        public override string ToString()
        {
            var sb = new StatementBuilder(this.bindings);
            sb.Append("cylinder(");
            sb.AppendValuePairIfExists("center", this.centerBinding.IsBound ? this.centerBinding.ToString() : this.center.ToString().ToLower());

            appendDiameterAndRadius(sb);

            sb.AppendValuePairIfExists("h", this.Height, true);
            sb.AppendValuePairIfExists("$fn", this.Resolution, true);
            sb.AppendValuePairIfExists("$fa", this.MinimumAngle, true);
            sb.AppendValuePairIfExists("$fs", this.MinimumCircumferentialLength, true);
            sb.Append(");");
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        private void appendDiameterAndRadius(StatementBuilder sb)
        {
            if (bindings.Contains("d"))
            {
                sb.AppendValuePairIfExists("d", this.Diameter, true);
            }
            else if (bindings.Contains("r"))
            {
                sb.AppendValuePairIfExists("r", this.Radius, true);
            }
            else if (bindings.Contains("d1") || bindings.Contains("d2"))
            {
                sb.AppendValuePairIfExists("d1", this.Diameter1, true);
                sb.AppendValuePairIfExists("d2", this.Diameter2, true);
            }
            else if (bindings.Contains("r1") || bindings.Contains("r2"))
            {
                sb.AppendValuePairIfExists("r1", this.Radius1, true);
                sb.AppendValuePairIfExists("r2", this.Radius2, true);
            }
            else
            {
                sb.AppendValuePairIfExists("r1", this.Radius1, true);
                sb.AppendValuePairIfExists("r2", this.Radius2, true);
            }
        }

        /// <summary>
        /// Gets a copy of this object that is a new instance
        /// </summary>
        /// <returns></returns>
        public override OSCADObject Clone()
        {
            return new Cylinder()
            {
                Name = this.Name,
                Height = this.Height,
                Radius1 = this.Radius1,
                Radius2 = this.Radius2,
                Resolution = this.Resolution,
                MinimumAngle = this.MinimumAngle,
                MinimumCircumferentialLength = this.MinimumCircumferentialLength,
                Center = this.Center,
                centerBinding = this.centerBinding,
                bindings = this.bindings.Clone()
            };
        }

        /// <summary>
        /// Gets the position of this object's center (origin) in
        /// world space
        /// </summary>
        /// <returns></returns>
        public override Vector3 Position()
        {
            Vector3 position;
            if (this.Center == false)
            {
                position = new Vector3(0, 0, this.Height / 2);
            }
            else
            {
                position = new Vector3();
            }

            return position;
        }

        /// <summary>
        /// Returns the approximate boundaries of this OpenSCAD object
        /// </summary>
        /// <returns></returns>
        public override Bounds Bounds()
        {
            if(this.Center == false)
            {
                return new Bounds(new Vector3(-this.Radius, -this.Radius, 0), 
                                  new Vector3(this.Radius, this.Radius, this.Height));
            }
            else
            {
                return new Bounds(new Vector3(-this.Radius, -this.Radius, -this.Height / 2),
                                  new Vector3(this.Radius, this.Radius, this.Height / 2));
            }
        }
                
        private Bindings bindings = new Bindings(new Dictionary<string, string>()
        {
            {"radius", "r" },
            {"radius1", "r1" },
            {"radius2", "r2" },
            {"diameter", "d" },
            {"diameter1", "d1" },
            {"diameter2", "d2" },
            {"height", "h" },
            {"resolution", "$fn" },
            {"minimumangle", "$fa" },
            {"minimumcircumferentiallength", "$fs" }
        });
        /// <summary>
        /// Binds a a variable to a property on this object
        /// </summary>
        /// <param name="property">A string specifying the property such as "Diameter" or "Radius"</param>
        /// <param name="variable">The variable to bind the to.  This variable will appear in script output in lieu of the 
        /// literal value of the property</param>
        public override void Bind(string property, Variable variable)
        {
            if (property.ToLower() == "center")
            {
                this.centerBinding.Bind(property, variable);
                this.center = Convert.ToBoolean(variable.Value);
            }
            else
            {
                this.bindings.Add<Cylinder>(this, property, variable);
            }
        }
        #endregion
    }
}
