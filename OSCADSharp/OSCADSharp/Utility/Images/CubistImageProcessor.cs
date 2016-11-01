using OSCADSharp.Solids;
using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Utility.Images
{
    /// <summary>
    /// Processes a bitmap image by treating contiguous same-color regions as cubes
    /// </summary>
    internal class CubistImageProcessor : IImageProcessor
    {
        private enum TraversalDirection
        {
            None,
            Bidirectional,
            X_Only,
            Y_Only
        }

        #region Private Fields
        private int scannedRows = 0;
        private int height = 0;
        private int width = 0;
        private string imagePath;
        private string heightMode;        
        List<OSCADObject> cubes = new List<OSCADObject>();
        private Color[,] pixels;
        private bool useGrayScale;
        private byte simplificationAmount;
        private HeightMapper htMapper;
        #endregion

        #region Internal Fields
        public Bounds ImageBounds { get; set; }
        #endregion

        internal CubistImageProcessor(string imagePath, string heightMode = "None", bool useGrayScale = false, byte simplificationAmount = 0)
        {
            this.heightMode = heightMode;
            this.imagePath = imagePath;
            this.useGrayScale = useGrayScale;
            this.simplificationAmount = simplificationAmount;
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
            this.setColorArray(img);

            var simplifier = new ImageSimplifier(img.Width, img.Height, pixels);
            simplifier.BasicResample(this.simplificationAmount);

            this.htMapper = new HeightMapper(img.Width, img.Height, pixels, this.heightMode);
            this.htMapper.SetHeightMappings();
            
            this.ImageBounds = new Bounds(new Vector3(), new Vector3(img.Width, img.Height, 1));            
            
            List<OSCADObject> cubes = new List<OSCADObject>();
            bool[,] visited = new bool[img.Width, img.Height];
            this.height = img.Height;
            this.width = img.Width;

            Point? start = this.getNextPoint(ref visited, img.Width - 1, img.Height - 1);
            do
            {
                System.Drawing.Color color = pixels[((Point)start).X, ((Point)start).Y];
                var cube = this.traverseIterative((Point)start, ref visited, color);
                if (cube != null)
                {
                    markVisited(ref visited, cube, (Point)start);
                    if (color.A != 0)
                    {
                        this.htMapper.SetHeight(color, cube);
                        string cubeColor = String.Format("[{0}, {1}, {2}]", color.R == 0 ? 0 : color.R / 255.0, color.G == 0 ? 0 : color.G / 255.0, color.B == 0 ? 0 : color.B / 255.0);
                        cubes.Add(cube.Translate(((Point)start).X, ((Point)start).Y, this.htMapper.GetZTranslation(cube.Size.Z))
                        .Color(cubeColor, color.A));
                    }
                }

                start = this.getNextPoint(ref visited, img.Width - 1, img.Height - 1);
            } while (start != null);

            return cubes;
        }     
        private void setColorArray(Bitmap img)
        {
            this.pixels = new Color[img.Width, img.Height];
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    setPixelColorValue(img, x, y);
                }
            }
        }

        private void setPixelColorValue(Bitmap img, int x, int y)
        {
            if(this.useGrayScale)
            {
                Color rgbColor = img.GetPixel(x, y);
                int grayscaleVal =  (rgbColor.R + rgbColor.G + rgbColor.B) / 3;
                pixels[x, y] = Color.FromArgb(rgbColor.A, grayscaleVal, grayscaleVal, grayscaleVal); 
            }
            else
            {
                pixels[x, y] = img.GetPixel(x, y);
            }
        }

        private Cube traverseIterative(Point start, ref bool[,] visited, System.Drawing.Color color, Cube cube = null)
        {
            TraversalDirection direction = TraversalDirection.None;
            Cube current = cube ?? new Cube();
            direction = traversePixels(ref start, visited, color, current);

            while (direction != TraversalDirection.None)
            {
                direction = traversePixels(ref start, visited, color, current);
            }
            
            return current;
        }

        private TraversalDirection traversePixels(ref Point start, bool[,] visited, Color color, Cube cube)
        {
            var direction = TraversalDirection.Bidirectional;
            var bounds = cube.Bounds();
            //Bidirectional
            visited[start.X, start.Y] = true;
            Point next = new Point(start.X + 1, start.Y + 1);
            if (next.X >= this.width || next.Y >= this.height)
            {
                direction = TraversalDirection.None;
            }
            else
            {
                if(pixelCanBeTraversed(ref visited, next, color))
                {
                    visited[next.X, next.Y] = true;
                    cube.Size.X += 1;
                    cube.Size.Y += 1;
                }
                else
                {
                    direction = TraversalDirection.None;   
                }
            }

            if (direction != TraversalDirection.None)
                return direction;

            //X-axis
            direction = TraversalDirection.X_Only;
            next.Y = start.Y;
            
            if (next.X >= this.width || next.Y >= this.height)
            {
                direction = TraversalDirection.None;
            }
            else
            {
                if (pixelCanBeTraversed(ref visited, next, color))
                {
                    visited[next.X, next.Y] = true;
                    cube.Size.X += 1;
                }
                else
                {
                    direction = TraversalDirection.None;
                }
            }

            if (direction != TraversalDirection.None)
                return direction;

            //Y-Axis
            direction = TraversalDirection.Y_Only;
            next.X = start.X;
            next.Y = start.Y + 1;

            if (next.X >= this.width || next.Y >= this.height)
            {
                direction = TraversalDirection.None;
            }
            else
            {
                if (pixelCanBeTraversed(ref visited, next, color))
                {
                    visited[next.X, next.Y] = true;
                    cube.Size.Y += 1;
                }
                else
                {
                    direction = TraversalDirection.None;
                }
            }
            
            return direction;
        }

        private bool pixelCanBeTraversed(ref bool[,] visited, Point pixel, Color colorToMatch)
        {
            return pixel.X < this.width && pixel.Y < this.height &&
                    visited[pixel.X, pixel.Y] == false && pixels[pixel.X, pixel.Y] == colorToMatch;

        }

        private void markVisited(ref bool[,] visited, Cube cube, Point start)
        {
            var bounds = cube.Bounds();
            for (int x = start.X; x < start.X + bounds.Length && x < this.width; x++)
            {
                for (int y = start.Y; y < start.Y + bounds.Width && y <this.height; y++)
                {
                    visited[x, y] = true;
                }
            }
        }

        private Point? getNextPoint(ref bool[,] visited, int width, int height)
        {
            int rowStart = this.scannedRows;
            for (int row = rowStart; row <= height; row++)
            {
                for (int column = 0; column <= width; column++)
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

