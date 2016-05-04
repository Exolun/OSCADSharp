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
    internal class PolygonalImageProcessor : IImageProcessor
    {
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
            obj = obj.Rotate(0, 0, 180);
            obj = obj.Translate(ImageBounds.Length, ImageBounds.Width, 0);

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
            throw new NotImplementedException();
        }

        private Dictionary<string, List<KeyValuePair<Point, Color>>> separateColors(Bitmap img)
        {
            var colorGroupings = new Dictionary<string, List<KeyValuePair<Point, Color>>>();
            for (int column = 0; column < img.Width; column++)
            {
                for (int row = 0; row < img.Height; row++)
                {
                    var color = img.GetPixel(column, row);
                    if (!colorGroupings.ContainsKey(color.Name))
                    {
                        colorGroupings[color.Name] = new List<KeyValuePair<Point, Color>>();
                    }

                    colorGroupings[color.Name].Add(new KeyValuePair<Point, Color>(new Point(column, row), color));
                }
            }

            return colorGroupings;
        }
    }
}
