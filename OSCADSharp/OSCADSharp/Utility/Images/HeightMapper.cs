using OSCADSharp.Solids;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Utility.Images
{
    internal class HeightMapper
    {
        private int height;
        private string heightMode;
        private Color[,] pixels;
        private int width;
        private Dictionary<Color, int> heightMappings;

        public HeightMapper(int width, int height, Color[,] pixels, string heightMode)
        {
            this.width = width;
            this.height = height;
            this.pixels = pixels;
            this.heightMode = heightMode;
        }

        internal void SetHeightMappings()
        {
            if (this.heightMode != "None")
            {
                this.heightMappings = new Dictionary<Color, int>();
                double max = 4 * 256;

                for (int x = 0; x < this.width; x++)
                {
                    for (int y = 0; y < this.height; y++)
                    {
                        var color = pixels[x, y];
                        double csum = (double)(color.R + color.G + color.B + color.A);
                        heightMappings[color] = Convert.ToInt32(csum != 0 ? (csum / max) * 10 : .25);
                    }
                }
            }
        }

        internal void SetHeight(Color color, Cube cube)
        {
            if (this.heightMode != "None")
            {
                cube.Size.Z = heightMappings[color];
            }
            else
            {
                cube.Size.Z = 1.0;
            }
        }

        internal double GetZTranslation(double cubeHeight)
        {
            if (this.heightMode != "Bidirectional")
            {
                return 0;
            }
            else
            {
                return -cubeHeight / 2;
            }
        }
    }
}
