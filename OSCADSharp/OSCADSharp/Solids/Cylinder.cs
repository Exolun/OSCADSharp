using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.Spatial;

namespace OSCADSharp.Solids
{
    /// <summary>
    /// A Cylinder geometry
    /// </summary>
    public class Cylinder : OSCADObject
    {
        #region Attributes
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
        public bool Center { get; set; } = false;

        /// <summary>
        /// Minimum angle (in degrees) of each cylinder fragment.
        /// ($fa in OpenSCAD)
        /// </summary>
        public int MinimumAngle { get; set; } = 12;

        /// <summary>
        /// Minimum circumferential length of each fragment.
        /// ($fs in OpenSCAD)
        /// </summary>
        public int MinimumCircumferentialLength { get; set; } = 2;

        /// <summary>
        /// Number of fragments in 360 degrees. Values of 3 or more override MinimumAngle and MinimumCircumferentialLength
        /// ($fn in OpenSCAD)
        /// </summary>
        public int Resolution { get; set; } = 0;
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
        #endregion

        #region Overrides
        /// <summary>
        /// Converts this object to an OpenSCAD script
        /// </summary>
        /// <returns>Script for this object</returns>
        public override string ToString()
        {
            return String.Format("cylinder($fn = {0}, $fa = {1}, $fs = {2}, h = {3}, r1 = {4}, r2 = {5}, center = {6}); {7}", 
                Resolution.ToString(), MinimumAngle.ToString(),  MinimumCircumferentialLength.ToString(),
                Height.ToString(), Radius1.ToString(), Radius2.ToString(), Center.ToString().ToLower(), Environment.NewLine);
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
                Center = this.Center
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
        #endregion
    }
}
