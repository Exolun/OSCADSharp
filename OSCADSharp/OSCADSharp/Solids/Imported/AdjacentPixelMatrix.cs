using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Solids.Imported
{
    /// <summary>
    /// A matrix for finding neighbors in regions of pixels
    /// </summary>
    internal class AdjacentPixelMatrix
    {
        private Point topLeft;
        private Point bottomRight;
        private KeyValuePair<Point, Color>?[,] grid;

        public Point TopLeft { get { return this.topLeft; }  }
        public Point BottomRight { get { return this.bottomRight; } }

        internal AdjacentPixelMatrix(List<KeyValuePair<Point, Color>> pixelGrouping)
        {
            this.createGrid(pixelGrouping);
        }

        public bool IsOutOfBounds(Point pt)
        {
            if (pt.X < topLeft.X || pt.X > bottomRight.X || pt.Y < topLeft.Y || pt.Y > bottomRight.Y)
            {
                return true;
            }

            return false;
        }

        public KeyValuePair<Point, Color>? At(int x, int y)
        {
            return this.grid[x, y];
        }

        private KeyValuePair<Point, Color>?[,] createGrid(List<KeyValuePair<Point, Color>> colorGrouping)
        {
            this.topLeft = new Point(int.MaxValue, int.MaxValue);
            this.bottomRight = new Point(int.MinValue, int.MinValue);

            foreach (var pair in colorGrouping)
            {
                if (pair.Key.X < topLeft.X)
                    topLeft.X = pair.Key.X;

                if (pair.Key.Y < topLeft.Y)
                    topLeft.Y = pair.Key.Y;

                if (pair.Key.X > bottomRight.X)
                    bottomRight.X = pair.Key.X;

                if (pair.Key.Y > bottomRight.Y)
                    bottomRight.Y = pair.Key.Y;
            }

            int width = bottomRight.X - topLeft.X + 1;
            int height = bottomRight.Y - topLeft.Y + 1;
            this.grid = new KeyValuePair<Point, Color>?[width, height];

            foreach (var pair in colorGrouping)
            {
                var pt = pair.Key;
                grid[pt.X - topLeft.X, pt.Y - topLeft.Y] = pair;
            }

            return grid;
        }
    }
}
