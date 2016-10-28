using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.Spatial;

namespace OSCADSharp.Solids.Compound
{
    /// <summary>
    /// Represents a four-sided container, by default it has a solid bottom and an open top.
    /// </summary>
    public class Box : OSCADObject
    {
        #region Attributes
        /// <summary>
        /// The thickness of the container walls
        /// </summary>
        public double WallThickness { get; set; } = .1;

        /// <summary>
        /// The Size of the box in terms of X/Y/Z units
        /// </summary>
        public Vector3 Size { get; set; } = new Vector3(1, 1, 1);

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
        /// Creates a box with the default initialization values
        /// </summary>
        public Box()
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
            OSCADObject inner = new Cube(new Vector3(this.Size.X - WallThickness*2, 
                this.Size.Y - WallThickness*2, 
                this.Size.Z - WallThickness*2),
                this.Center);
                        
            if (!this.Bottom && !this.Top)
            {
                ((Cube)inner).Size.Z += WallThickness * 4;
            }
            else if (!this.Top)
            {
                ((Cube)inner).Size.Z += WallThickness*2;
                inner = inner.Translate(0, 0, WallThickness);
            }
            else if (!this.Bottom)
            {
                ((Cube)inner).Size.Z += WallThickness*2;
                inner = inner.Translate(0, 0, -WallThickness);
            }


            if (!this.Center)
            {
                inner = inner.Translate(this.WallThickness, this.WallThickness, this.WallThickness);
            }

            OSCADObject box = new Cube(this.Size, this.Center) - inner;

            return box.ToString();
        }

        /// <summary>
        /// Returns the approximate boundaries of this OpenSCAD object
        /// </summary>
        /// <returns></returns>
        public override Bounds Bounds()
        {
            if (Center == false)
            {
                return new Bounds(new Vector3(), new Vector3(this.Size.X, this.Size.Y, this.Size.Z));
            }
            else
            {
                return new Bounds(new Vector3(-this.Size.X / 2, -this.Size.Y / 2, -this.Size.Z / 2),
                                  new Vector3(this.Size.X / 2, this.Size.Y / 2, this.Size.Z / 2));
            }
        }

        /// <summary>
        /// Gets a copy of this object that is a new instance
        /// </summary>
        /// <returns></returns>
        public override OSCADObject Clone()
        {
            return new Box()
            {
                Name = this.Name,
                Size = this.Size,
                Bottom = this.Bottom,
                Top = this.Top,
                Center = this.Center,
                WallThickness = this.WallThickness
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
                position = new Vector3(this.Size.X / 2, this.Size.Y / 2, this.Size.Z / 2);
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
