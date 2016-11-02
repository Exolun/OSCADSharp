using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.Spatial;

namespace OSCADSharp.Solids.Compound
{
    /// <summary>
    /// A cylindrical container, by default it has a solid bottom and an open top.
    /// </summary>
    public class Tube : OSCADObject
    {
        #region Attributes
        /// <summary>
        /// Number of fragments in 360 degrees. Values of 3 or more override MinimumAngle and MinimumCircumferentialLength
        /// ($fn in OpenSCAD)
        /// </summary>
        public int? Resolution { get; set; }

        /// <summary>
        /// The thickness of the container walls
        /// </summary>
        public double WallThickness { get; set; } = .1;

        /// <summary>
        /// Height of the tube
        /// </summary>
        public double Height { get; set; } = 1;

        /// <summary>
        /// Radius of tube. r1 = r2 = r.
        /// </summary>
        public double Radius
        {
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
        /// Diameter of tube. r1 = r2 = d /2.
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
        /// If True, the center of the box will be at 0, 0, 0
        /// 
        /// If False (default) one corner will be centered at 0,0, 0, with the cube extending into the positive octant (positive X/Y/Z)
        /// </summary>
        public bool Center { get; set; } = false;

        /// <summary>
        /// If true, the container has a solid bottom
        /// </summary>
        public bool Bottom { get; set; } = true;

        /// <summary>
        /// If true, the container has a solid top, otherwise the top is open 
        /// </summary>
        public bool Top { get; set; } = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a tube with the specified dimensions / configuration
        /// </summary>
        /// <param name="diameter"></param>
        /// <param name="height"></param>
        /// <param name="wallThickness"></param>
        /// <param name="bottom"></param>
        /// <param name="top"></param>
        /// <param name="center"></param>
        public Tube(double diameter, double height, double wallThickness = .1, bool bottom = false, bool top = false, bool center = false)
        {
            this.Diameter = diameter;
            this.Height = height;
            this.WallThickness = wallThickness;
            this.Bottom = bottom;
            this.Top = top;
            this.Center = center;
        }

        /// <summary>
        /// Creates a tube with the default initialization values
        /// </summary>
        public Tube()
        {
        }
        #endregion

        #region Overrides        
        /// <summary>
        /// Converts this object to an OpenSCAD script
        /// </summary>
        /// <returns>Script for this object</returns>
        public override string ToString()
        {
            OSCADObject inner = new Cylinder() {
                Diameter1 = this.Diameter1 - WallThickness * 2,
                Diameter2 = this.Diameter2 - WallThickness * 2,
                Height = this.Height - WallThickness * 2,
                Center = this.Center,
                Resolution = this.Resolution
            };

            if (!this.Bottom && !this.Top)
            {
                ((Cylinder)inner).Height += WallThickness * 4;
            }
            else if (!this.Top)
            {
                ((Cylinder)inner).Height += WallThickness * 2;
                inner = inner.Translate(0, 0, WallThickness);
            }
            else if (!this.Bottom)
            {
                ((Cylinder)inner).Height += WallThickness * 2;
                inner = inner.Translate(0, 0, -WallThickness);
            }
            
            OSCADObject cyl = new Cylinder()
            {
                Diameter1 = this.Diameter1,
                Diameter2 = this.Diameter2,
                Height = this.Height,
                Center = this.Center,
                Resolution = this.Resolution
            } - inner;

            return cyl.ToString();
        }

        /// <summary>
        /// Returns the approximate boundaries of this OpenSCAD object
        /// </summary>
        /// <returns></returns>
        public override Bounds Bounds()
        {
            if (this.Center == false)
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
                Radius2 = this.Radius2
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
        #endregion
    }
}
