using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Solids
{
    /// <summary>
    /// A Sphere geometry
    /// </summary>
    public class Sphere : OSCADObject
    {
        #region Attributes
        /// <summary>
        /// This is the radius of the sphere
        /// </summary>
        public double Radius { get; set; } = 1;

        /// <summary>
        /// This is the diameter of the sphere
        /// </summary>
        public double Diameter {
            get { return this.Radius * 2; }
            set { this.Radius = value / 2; }
        }
        
        /// <summary>
        /// Minimum angle (in degrees) of each cylinder fragment.
        /// ($fa in OpenSCAD)
        /// </summary>
        public int MinimumAngle { get; set; } = 12;

        /// <summary>
        /// Fragment size in mm
        /// ($fs in OpenSCAD)
        /// </summary>
        public int MinimumFragmentSize { get; set; } = 2;

        /// <summary>
        /// Number of fragments in 360 degrees. Values of 3 or more override MinimumAngle and MinimumCircumferentialLength
        /// ($fn in OpenSCAD)
        /// </summary>
        public int Resolution { get; set; } = 0;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a sphere with the default initialization values
        /// </summary>
        public Sphere()
        {
        }

        /// <summary>
        /// Creates a sphere of the specified diameter
        /// </summary>
        /// <param name="diameter">Diameter of the sphere</param>
        public Sphere(double diameter)
        {
            this.Diameter = diameter;
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return String.Format("sphere($fn = {0}, $fa = {1}, $fs = {2}, r = {3});", 
                this.Resolution.ToString(), this.MinimumAngle.ToString(), 
                this.MinimumFragmentSize.ToString(), this.Radius.ToString());
        }

        public override OSCADObject Clone()
        {
            return new Sphere()
            {
                Resolution = this.Resolution,
                MinimumAngle = this.MinimumAngle,
                MinimumFragmentSize = this.MinimumFragmentSize,
                Radius = this.Radius
            };
        }

        public override bool Equals(object other)
        {
            if(other.GetType() == typeof(Sphere))
            {
                Sphere otherSphere = other as Sphere;
                return this.Diameter == otherSphere.Diameter &&
                    this.Radius == otherSphere.Radius &&
                    this.MinimumAngle == otherSphere.MinimumAngle &&
                    this.MinimumFragmentSize == otherSphere.MinimumFragmentSize &&
                    this.Resolution == otherSphere.Resolution;
            }
            else
            {
                return base.Equals(other);
            }            
        }
        #endregion
    }
}
