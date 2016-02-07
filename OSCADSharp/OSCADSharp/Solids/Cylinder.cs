using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Solids
{
    /// <summary>
    /// A Cylinder geometry
    /// </summary>
    public class Cylinder
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


    }
}
