using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Solids.Imported
{
    /// <summary>
    /// Processes a bitmap image by treating contiguous same-color regions as cubes
    /// </summary>
    internal class CubistImageProcessor : IImageProcessor
    {
        #region Private Fields
        private int scannedRows = 0;
        private string imagePath;
        private bool includeHeight;
        private Dictionary<Color, int> heightMappings;
        List<OSCADObject> cubes = new List<OSCADObject>();
        private Color[,] pixels;
        #endregion

        #region Internal Fields
        public Bounds ImageBounds { get; set; }
        #endregion

        internal CubistImageProcessor(string imagePath, bool includeHeight = true)
        {
            this.includeHeight = includeHeight;
            this.imagePath = imagePath;
        }

        public OSCADObject ProcessImage()
        {
            this.cubes = this.processImage();
            OSCADObject obj = new OSCADObject.MultiStatementObject("union()", cubes);
            return obj.Scale(1, -1, 1).Translate(0, ImageBounds.Width, 0);
        }

        #region Private Methods
        private List<OSCADObject> processImage()
        {
            Bitmap img = new Bitmap(Image.FromFile(this.imagePath));
            this.setPixelArray(img);
            this.setHeightMappings(img);
            this.ImageBounds = new Bounds(new Vector3(), new Vector3(img.Width, img.Height, 1));

            List<OSCADObject> cubes = new List<OSCADObject>();
            bool[,] visited = new bool[img.Width, img.Height];

            Point? start = this.getNextPoint(img, ref visited, img.Width - 1, img.Height - 1);
            do
            {
                System.Drawing.Color color = pixels[((Point)start).X, ((Point)start).Y];

                var cube = this.traverseNext(img, (Point)start, ref visited, color);
                if (cube != null)
                {
                    this.markVisited(ref visited, cube, (Point)start, img);
                    if (color.A != 0)
                    {

                        if (this.includeHeight)
                        {
                            cube.Size.Z = heightMappings[color];
                        }

                        string cubeColor = String.Format("[{0}, {1}, {2}]", color.R == 0 ? 0 : color.R / 255.0, color.G == 0 ? 0 : color.G / 255.0, color.B == 0 ? 0 : color.B / 255.0);
                        cubes.Add(cube.Translate(((Point)start).X, ((Point)start).Y, 0)
                        .Color(cubeColor, color.A));
                    }
                }

                start = this.getNextPoint(img, ref visited, img.Width - 1, img.Height - 1);
            } while (start != null);

            return cubes;
        }

        private void setPixelArray(Bitmap img)
        {
            this.pixels = new Color[img.Width, img.Height];
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    pixels[x, y] = img.GetPixel(x, y);
                }
            }
        }

        private void setHeightMappings(Bitmap img)
        {
            if (this.includeHeight)
            {
                this.heightMappings = new Dictionary<Color, int>();
                double max = 4 * 256;

                for (int x = 0; x < img.Width; x++)
                {
                    for (int y = 0; y < img.Height; y++)
                    {
                        var color = pixels[x, y];
                        double csum = (double)(color.R + color.G + color.B + color.A);
                        heightMappings[color] = Convert.ToInt32(csum != 0 ? (csum / max)  * 10: .25);
                    }
                }
            }
        }

        private void markVisited(ref bool[,] visited, Cube cube, Point start, Bitmap img)
        {
            var bounds = cube.Bounds();
            for (int column = start.X; column < start.X + bounds.Width; column++)
            {
                for (int row = start.Y; row < start.Y + bounds.Length; row++)
                {
                    visited[column, row] = true;
                }
            }
        }

        private Cube traverseNext(Bitmap img, Point start, ref bool[,] visited, System.Drawing.Color color, Cube cube = null)
        {
            bool canTraverse = true;
            if (cube != null)
            {
                canContinueTraversal(img, ref start, ref visited, color, cube, ref canTraverse);
            }
            else
            {
                canTraverse = pixelCanBeTraversed(img, ref visited, new Point(start.X + 1, start.Y + 1), color) &&
                   pixelCanBeTraversed(img, ref visited, new Point(start.X + 1, start.Y), color) &&
                   pixelCanBeTraversed(img, ref visited, new Point(start.X, start.Y + 1), color);
            }


            if (canTraverse)
            {
                if (cube == null)
                {
                    cube = new Cube();
                    cube.Size.X += 1;
                    cube.Size.Y += 1;

                    return traverseNext(img, start, ref visited, color, cube);
                }
                else
                {
                    cube.Size.X += 1;
                    cube.Size.Y += 1;
                    return traverseNext(img, start, ref visited, color, cube);
                }
            }
            else
            {

                if (cube == null)
                {
                    return new Cube();
                }
                else
                {
                    return cube;
                }
            }

        }

        private void canContinueTraversal(Bitmap img, ref Point start, ref bool[,] visited, Color color, Cube cube, ref bool canTraverse)
        {
            var bounds = cube.Bounds();
            for (int column = start.X; column < start.X + bounds.Width && canTraverse; column++)
            {
                for (int row = start.Y; row < start.Y + bounds.Length && canTraverse; row++)
                {
                    if (start.X + column >= img.Width || start.Y + row >= img.Height)
                    {
                        canTraverse = false;
                    }
                    else
                    {
                        canTraverse = canTraverse && pixelCanBeTraversed(img, ref visited, new Point(column, row), color);
                    }
                }
            }
        }

        private bool pixelCanBeTraversed(Bitmap img, ref bool[,] visited, Point pixel, Color colorToMatch)
        {
            return pixel.X < img.Width && pixel.Y < img.Height &&
                    visited[pixel.X, pixel.Y] == false && pixels[pixel.X, pixel.Y] == colorToMatch;

        }

        private Point? getNextPoint(Bitmap img, ref bool[,] visited, int width, int height)
        {
            int rowStart = this.scannedRows;
            for (int row = rowStart; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    if (visited[column, row] == false)
                    {
                        return new Point(column, row);
                    }
                }
                this.scannedRows++;
            }

            return null;
        }


        #endregion
    }
}
