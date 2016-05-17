using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Utility.Images
{
    internal class ImageSimplifier
    {
        private int height;
        private Color[,] pixels;
        private int width;

        public ImageSimplifier(int width, int height, Color[,] pixels)
        {
            this.width = width;
            this.height = height;
            this.pixels = pixels;
        }

        internal void GlobalReduction(byte simplificationAmount)
        {
            if (simplificationAmount == 0)
            {
                return;
            }

            // These calls color values from their originals to new ones based upon the color's proximity to the simpification  amount
            // the channelSelector and colorFactory functions allow us to use the same function for each.
            // TODO:  Refactor into a class             
            mapColors(width, height, simplificationAmount, (c) => { return c.R; }, (orig, retained) => { return Color.FromArgb(orig.A, retained.R, orig.G, orig.B); });
            mapColors(width, height, simplificationAmount, (c) => { return c.G; }, (orig, retained) => { return Color.FromArgb(orig.A, orig.R, retained.G, orig.B); });
            mapColors(width, height, simplificationAmount, (c) => { return c.B; }, (orig, retained) => { return Color.FromArgb(orig.A, orig.R, orig.G, retained.B); });
        }

        private void mapColors(int width, int height, int simplificationAmount, Func<Color, byte> channelSelector, Func<Color, Color, Color> colorFactory)
        {
            Dictionary<byte, Color> colorsRetained = new Dictionary<byte, Color>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color curColor;
                    bool colorMatched = false;
                    curColor = this.pixels[x, y];
                    for (int simVal = 0; simVal < simplificationAmount; simVal++)
                    {
                        int upperLimit = channelSelector(curColor) + simVal;
                        int lowerLimit = channelSelector(curColor) - simVal;
                        if (upperLimit <= 255 && colorsRetained.ContainsKey((byte)upperLimit))
                        {
                            this.pixels[x, y] = colorFactory(curColor, colorsRetained[(byte)upperLimit]);
                            colorMatched = true;
                        }
                        else if (lowerLimit >= 0 && colorsRetained.ContainsKey((byte)lowerLimit))
                        {
                            this.pixels[x, y] = colorFactory(curColor, colorsRetained[(byte)lowerLimit]);
                            colorMatched = true;
                        }
                    }

                    if (!colorMatched)
                    {
                        colorsRetained[channelSelector(curColor)] = curColor;
                    }
                }
            }
        }
    }
}
