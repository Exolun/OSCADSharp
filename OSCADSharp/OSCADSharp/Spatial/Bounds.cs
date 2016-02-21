using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Spatial
{
    /// <summary>
    /// A set of boundaries
    /// </summary>
    public class Bounds
    {
        /// <summary>
        /// Creates a set of boundaries with the corners specified 
        /// to define its extremities
        /// </summary>
        /// <param name="bottomLeft"></param>
        /// <param name="topRight"></param>
        public Bounds(Vector3 bottomLeft, Vector3 topRight)
        {
            this.BottomLeft = bottomLeft;
            this.TopRight = topRight;
        }

        #region Public Properties
        /// <summary>
        /// Represents the top-right corner of the bounds (prior to any transforms)
        /// </summary>
        public Vector3 TopRight { get; private set; }

        /// <summary>
        /// Represents the bottom-left corner of the bounds  (prior to any transforms)
        /// </summary>
        public Vector3 BottomLeft { get; private set; }

        /// <summary>
        /// X position with the greatest value
        /// </summary>
        public double X_Max { get { return TopRight.X > BottomLeft.X ? TopRight.X : BottomLeft.X; } }

        /// <summary>
        /// X position with the smallest value
        /// </summary>
        public double X_Min { get { return TopRight.X < BottomLeft.X ? TopRight.X : BottomLeft.X; } }

        /// <summary>
        /// Y position with the greatest value
        /// </summary>
        public double Y_Max { get { return TopRight.Y > BottomLeft.Y ? TopRight.Y : BottomLeft.Y; } }

        /// <summary>
        /// Y position with the smallest value
        /// </summary>
        public double Y_Min { get { return TopRight.Y < BottomLeft.Y ? TopRight.Y : BottomLeft.Y; } }

        /// <summary>
        /// Z position with the greatest value
        /// </summary>
        public double Z_Max { get { return TopRight.Z > BottomLeft.Z ? TopRight.Z : BottomLeft.Z; } }

        /// <summary>
        /// Z position with the smallest value
        /// </summary>
        public double Z_Min { get { return TopRight.Z < BottomLeft.Z ? TopRight.Z : BottomLeft.Z; } }
        #endregion
    }
}
