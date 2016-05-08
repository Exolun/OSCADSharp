using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Solids.Imported
{
    /// <summary>
    /// Helper class to reuse code in image processing for finding adjacent pixels
    /// </summary>
    internal class NeighboringPointFinder
    {
        bool cardinalOnly = false;

        internal NeighboringPointFinder(bool cardinalDirectionsOnly = false)
        {
            this.cardinalOnly = cardinalDirectionsOnly;
        }

        internal Point Above(Point origin)
        {
            return new Point(origin.X, origin.Y + 1);
        }

        internal Point Below(Point origin)
        {
            return new Point(origin.X, origin.Y - 1);
        }

        internal Point Left(Point origin)
        {
            return new Point(origin.X - 1, origin.Y);
        }

        internal Point Right(Point origin)
        {
            return new Point(origin.X + 1, origin.Y);
        }

        internal Point UpperLeft(Point origin)
        {
            return new Point(origin.X - 1, origin.Y + 1);
        }

        internal Point UpperRight(Point origin)
        {
            return new Point(origin.X + 1, origin.Y + 1);
        }

        internal Point LowerLeft(Point origin)
        {
            return new Point(origin.X - 1, origin.Y - 1);
        }

        internal Point LowerRight(Point origin)
        {
            return new Point(origin.X + 1, origin.Y - 1);
        }

        internal List<Point> GetNeighbors(Point origin)
        {
            if (this.cardinalOnly)
            {
                return  new List<Point>()
                {
                        this.Above(origin),
                        this.Below(origin),
                        this.Left(origin),
                        this.Right(origin)
                };
            }
            else
            {
                return  new List<Point>()
                {
                        this.Above(origin),
                        this.Below(origin),
                        this.Left(origin),
                        this.Right(origin),
                        this.UpperLeft(origin),
                        this.UpperRight(origin),
                        this.LowerLeft(origin),
                        this.LowerRight(origin)
                };
            }
        }
    }
}
