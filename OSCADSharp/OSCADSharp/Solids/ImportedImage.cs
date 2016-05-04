using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.DataBinding;
using OSCADSharp.Spatial;
using System.Drawing;

namespace OSCADSharp.Solids
{
    /// <summary>
    /// An image file imported and processed into a 3D object
    /// </summary>
    public class ImportedImage : OSCADObject
    {
        #region Private Fields
        private string imagePath;

        List<OSCADObject> cubes;
        #endregion
        
        #region Constructors / Initialization
        /// <summary>
        /// Creates an imported image with the default settings
        /// </summary>
        /// <param name="imagePath"></param>
        public ImportedImage(string imagePath)
        {
            this.imagePath = imagePath;

            this.cubes = this.processImage();
        }

        private List<OSCADObject> processImage()
        {
            Bitmap img = new Bitmap(Image.FromFile(this.imagePath));            
            if(img.Width > 200 || img.Height > 200)
            {
                throw new InvalidOperationException("Cannot process images larger greater than 200x200 pixels");
            }

            List<OSCADObject> cubes = new List<OSCADObject>();
            bool[,] visited = new bool[img.Width, img.Height];

            Point? start = this.getNextPoint(img, ref visited, img.Width-1, img.Height-1);
            do
            {
                System.Drawing.Color color = img.GetPixel(((Point)start).X, ((Point)start).Y);                

                var cube = this.traverseNext(img, (Point)start, ref visited, color);
                if(cube != null)
                {
                    this.markVisited(ref visited, cube, (Point)start, img); 
                    if(color.A != 0)
                    {
                        cubes.Add(cube.Translate(((Point)start).X, ((Point)start).Y, 0)
                        .Color(String.Format("[{0}, {1}, {2}]", color.R == 0 ? 0 : color.R / 255, color.G == 0 ? 0 : color.G / 255, color.B == 0 ? 0 : color.B / 255), color.A));
                    }                    
                }
                
                start = this.getNextPoint(img, ref visited, img.Width-1, img.Height-1);                 
            } while (start != null);

            return cubes;
        }

        private void markVisited(ref bool[,] visited, Cube cube, Point start, Bitmap img)
        {
            var bounds = cube.Bounds();
            for(int column = start.X; column < start.X + bounds.Width; column++)
            {
                for (int row = start.Y; row < start.Y + bounds.Length; row++)
                {
                    visited[column, row] = true;
                }
            }
        }

        Cube traverseNext(Bitmap img, Point start, ref bool[,] visited, System.Drawing.Color color, Cube cube = null)
        {
            bool canTraverse = true;
            if(cube != null)
            {
                canContinueTraversal(img, ref start, ref visited, color, cube, ref canTraverse);
            }
            else
            {
                canTraverse = pixelCanBeTraversed(img, ref visited, new Point(start.X + 1, start.Y + 1), color) &&
                   pixelCanBeTraversed(img, ref visited, new Point(start.X + 1, start.Y), color) &&
                   pixelCanBeTraversed(img, ref visited, new Point(start.X, start.Y + 1), color);
            }


            if(canTraverse)
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

                if(cube == null)
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

        bool pixelCanBeTraversed(Bitmap img, ref bool[,] visited, Point pixel, Color colorToMatch) {
            return pixel.X < img.Width && pixel.Y < img.Height &&
                    visited[pixel.X, pixel.Y] == false && img.GetPixel(pixel.X, pixel.Y) == colorToMatch;
                    
        }

        Point? getNextPoint(Bitmap img, ref bool[,] visited, int width, int height)
        {
            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {
                    if(visited[column, row] == false)
                    {
                        return new Point(column, row);                        
                    }
                }
            }

            return null;
        }        
        #endregion

        #region OSCADObject Overrides
        public override void Bind(string property, Variable variable)
        {
            throw new NotImplementedException();
        }

        public override Bounds Bounds()
        {
            throw new NotImplementedException();
        }

        public override OSCADObject Clone()
        {
            throw new NotImplementedException();
        }

        public override Vector3 Position()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var obj = new MultiStatementObject("union()", cubes);
            return obj.ToString();
        }
        #endregion
    }
}
