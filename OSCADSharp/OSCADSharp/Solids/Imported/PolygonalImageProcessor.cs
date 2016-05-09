using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.DataBinding;

namespace OSCADSharp.Solids.Imported
{
    

    /// <summary>
    /// Processes a bitmap image by treating contiguous same-color regions as cubes
    /// </summary>
    internal class PolygonalImageProcessor : IImageProcessor
    {
        #region Private Classes
        private class Polygon : OSCADObject
        {
            private List<Point> points;

            public Polygon(List<Point> points)
            {
                this.points = points;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("polygon(points=[");

                Point pt;
                for (int i = 0; i < this.points.Count; i++)                
                {
                    pt = this.points[i];
                    if(i == 0)
                        sb.Append(String.Format("[{0}, {1}]", pt.X, pt.Y));
                    else
                        sb.Append(String.Format(", [{0}, {1}]", pt.X, pt.Y));
                }

                sb.Append("]);");

                return sb.ToString();
            }

            public override void Bind(string property, Variable variable)
            {
                throw new NotImplementedException();
            }

            public override Bounds Bounds()
            {
                var bottomLeft = new Vector3(int.MaxValue, int.MaxValue, 0);
                var topRight = new Vector3(int.MinValue, int.MinValue, 1);


                foreach (var point in this.points)
                {
                    if (point.X < bottomLeft.X)
                        bottomLeft.X = point.X;

                    if (point.Y < bottomLeft.Y)
                        bottomLeft.Y = point.Y;

                    if (point.X > topRight.X)
                        topRight.X = point.X;

                    if (point.Y > topRight.Y)
                        topRight.Y = point.Y;
                }

                return new Spatial.Bounds(bottomLeft, topRight);
            }

            public override OSCADObject Clone()
            {
                return new Polygon(this.points);
            }

            public override Vector3 Position()
            {
                var bounds = this.Bounds();
                return Vector3.Average(bounds.TopRight, bounds.BottomLeft);
            }
        }
        #endregion

        #region Private Fields
        private string imagePath;
        #endregion

        #region Public Fields
        public Bounds ImageBounds { get; set; }
        #endregion

        #region Constructors
        internal PolygonalImageProcessor(string imagePath)
        {
            this.imagePath = imagePath;
        }
        #endregion

        public OSCADObject ProcessImage()
        {
            var polygons = this.processImage();
            OSCADObject obj = new OSCADObject.MultiStatementObject("union()", polygons);            
            obj = obj.Scale(1, -1, 1).Translate(0, ImageBounds.Width, 0);

            return obj;
        }

        private List<OSCADObject> processImage()
        {
            Bitmap img = new Bitmap(Image.FromFile(this.imagePath));

            if (img.Width > 200 || img.Height > 200)
            {
                throw new InvalidOperationException("Cannot process images larger greater than 200x200 pixels");
            }

            this.ImageBounds = new Bounds(new Vector3(), new Vector3(img.Width, img.Height, 1));

            var separatedColors = this.separateColors(img);
            IEnumerable<List<KeyValuePair<Point, Color>>> contiguousSections = new List<List<KeyValuePair<Point, Color>>>();

            //Parallel.ForEach(separatedColors.Values, (colorGroup) =>
            foreach (var colorGroup in separatedColors.Values)            
            {
                var sections = this.getContiguousSections(colorGroup);
                contiguousSections = contiguousSections.Concat(sections);
            }//);

            return this.convertToPolygons(contiguousSections);
        }

        private List<OSCADObject> convertToPolygons(IEnumerable<List<KeyValuePair<Point, Color>>> contiguousSections)
        {
            List<OSCADObject> objects = new List<OSCADObject>();
            StringBuilder sb = new StringBuilder();
            var orderedSections = contiguousSections.OrderBy(sec => sec.Count);
            var largest = orderedSections.Last().Count;
            
            foreach (var section in orderedSections)
            {
                var color = section[0].Value;
                var orderedSection = this.getOrderedPoints(section);
                OSCADObject pgon = new Polygon(orderedSection);
                pgon = pgon.Color(String.Format("[{0}, {1}, {2}]", color.R == 0 ? 0 : color.R / 255, color.G == 0 ? 0 : color.G / 255, color.B == 0 ? 0 : color.B / 255), color.A);
                //pgon = pgon.Scale(1, 1, (1.0 / section.Count/ largest) * 100);
                objects.Add(pgon);

                //foreach (var pair in section)
                //{
                //    var position = pair.Key;
                //    var color = pair.Value;


                //    var cube = new Cube().Color(String.Format("[{0}, {1}, {2}]", color.R == 0 ? 0 : color.R / 255, color.G == 0 ? 0 : color.G / 255, color.B == 0 ? 0 : color.B / 255), color.A);
                //    cube = cube.Translate(position.X, position.Y, 0);
                //    objects.Add(cube);
                //}
            }

            return objects;
        }

        private List<Point> getOrderedPoints(List<KeyValuePair<Point, Color>> section)
        {
            List<Point> orderedPoints = new List<Point>(section.Count);
            var grid = new AdjacentPixelMatrix(section);
            var traversed = new HashSet<Point>();
            var neighborFinder = new NeighboringPointFinder(true);
            Stack<Point> toTraverse = new Stack<Point>();
            Point? origin = section[0].Key;            
            
            while(origin != null)
            {
                var orig = (Point)origin;
                traversed.Add(orig);
                orderedPoints.Add(orig);

                var below = neighborFinder.Below(orig);
                var right = neighborFinder.Right(orig);
                var above = neighborFinder.Above(orig);
                var left = neighborFinder.Left(orig);                

                
                if (grid.IsInBoundsAndNotNull(above, true) && !traversed.Contains(above))
                {
                    origin = above;
                }
                else if (grid.IsInBoundsAndNotNull(right, true) && !traversed.Contains(right))
                {
                    origin = right;
                }
                else if (grid.IsInBoundsAndNotNull(below, true) && !traversed.Contains(below))
                {
                    origin = below;
                }                
                else if (grid.IsInBoundsAndNotNull(left, true) && !traversed.Contains(left))
                {
                    origin = left;
                }
                else
                {
                    //TODO: Handle additional paths
                    origin = null;
                }
            }
                        
            return orderedPoints;
        }

        private List<List<KeyValuePair<Point, Color>>> getContiguousSections(List<KeyValuePair<Point, Color>> colorGrouping)
        {
            var grid =  new AdjacentPixelMatrix(colorGrouping);
            var sections = new List<List<KeyValuePair<Point, Color>>>();

            while (colorGrouping.Count > 0)
            {
                var origin = colorGrouping[0];
                colorGrouping.RemoveAt(0);
                sections.Add(this.getConnectedPixelsOfSameColor(origin, grid, colorGrouping));
            }

            foreach (var section in sections)
            {
                this.removeCenterPixels(section, grid);
            }

            return sections;
        }

        private void removeCenterPixels(List<KeyValuePair<Point, Color>> section, AdjacentPixelMatrix grid)
        {
            var neighborFinder = new NeighboringPointFinder();

            for (int i = section.Count - 1; i >= 0; i--)        
            {
                var origin = section[i].Key;
                var color = section[i].Value;

                // We only care about cardinal directions for the purpose of removing pixels
                // that lie in the center of a grouping (to avoid redundant vertexes
                List<Point> neighboringPoints = neighborFinder.GetNeighbors(origin);

                bool isOnanEdge = false;
                foreach (var pt in neighboringPoints)
                {
                    //If out of bounds, we found an edge
                    if (grid.IsOutOfBounds(pt))
                    {
                        isOnanEdge = true;
                        break;
                    }

                    //int x = pt.X - grid.TopLeft.X;
                    //int y = pt.Y - grid.TopLeft.Y;

                    //if(grid.At(x, y) == null)
                    //{
                    //    isOnanEdge = true;
                    //    break;
                    //}

                    //if (grid.At(x, y) != null)
                    //{
                    //    var nbr = (KeyValuePair<Point, Color>)grid.At(x, y);
                    //    if(!nbr.Value.Equals(color))
                    //    {
                    //        isOnanEdge = true;
                    //        break;
                    //    }
                    //}
                }

                if (!isOnanEdge)
                {
                    section.RemoveAt(i);
                }
            }
        }

        private List<KeyValuePair<Point, Color>> getConnectedPixelsOfSameColor(KeyValuePair<Point, Color> origin, AdjacentPixelMatrix grid, List<KeyValuePair<Point, Color>> colorGrouping)
        {

            var neighborFinder = new NeighboringPointFinder();

            List<KeyValuePair<Point, Color>> neighbors = new List<KeyValuePair<Point, Color>>();
            HashSet<KeyValuePair<Point, Color>> traversed = new HashSet<KeyValuePair<Point, Color>>();
            Queue<KeyValuePair<Point, Color>> nextOrigins = new Queue<KeyValuePair<Point, Color>>();
            nextOrigins.Enqueue(origin);
            traversed.Add(origin);
            neighbors.Add(origin);

            while (nextOrigins.Count > 0)
            {
                origin = nextOrigins.Dequeue();
                List<Point> neighboringPoints = neighborFinder.GetNeighbors(origin.Key);

                foreach (var pt in neighboringPoints)
                {
                    //Ignore if out of bounds
                    if(grid.IsOutOfBounds(pt))
                    {
                        continue;
                    }

                    int x = pt.X - grid.TopLeft.X;
                    int y = pt.Y - grid.TopLeft.Y;

                    if(grid.At(x, y) != null)
                    {
                        var nbr = (KeyValuePair<Point, Color>)grid.At(x, y);
                        if (!traversed.Contains(nbr) && nbr.Value.Equals(origin.Value))
                        {
                            nextOrigins.Enqueue(nbr);
                            neighbors.Add(nbr);
                            traversed.Add(nbr);
                        }
                    }
                }
            }

            colorGrouping.RemoveAll(elem => traversed.Contains(elem));

            return neighbors;
        }

        

        private Dictionary<string, List<KeyValuePair<Point, Color>>> separateColors(Bitmap img)
        {
            var colorGroupings = new Dictionary<string, List<KeyValuePair<Point, Color>>>();
            for (int column = 0; column < img.Width; column++)
            {
                for (int row = 0; row < img.Height; row++)
                {
                    var color = img.GetPixel(column, row);
                    string key = String.Format("{0}-{1}-{2}", color.R, color.G, color.B);
                    if (!colorGroupings.ContainsKey(key))
                    {
                        colorGroupings[key] = new List<KeyValuePair<Point, Color>>();
                    }

                    colorGroupings[key].Add(new KeyValuePair<Point, Color>(new Point(column, row), color));
                }
            }

            return colorGroupings;
        }
    }
}
