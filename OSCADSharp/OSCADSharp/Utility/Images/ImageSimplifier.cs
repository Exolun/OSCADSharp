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

        /// <summary>
        /// Partitions shades of red/green/blue into buckets and simplifies colors by
        /// preserving only the most commonly occurring hues of each.  SimplificationAmount determines
        /// partition size.
        /// 
        /// Eg.  s = 50, numPartitions = 255/50, approximately 5 shades of each hue
        /// </summary>
        /// <param name="simplificationAmount"></param>
        internal void BasicResample(byte simplificationAmount)
        {
            if (simplificationAmount == 0)
            {
                return;
            }

            int[] reds = new int[256];
            int[] greens = new int[256];
            int[] blues = new int[256];


            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    var pixel = pixels[x, y];
                    reds[pixel.R]++;
                    greens[pixel.G]++;
                    blues[pixel.B]++;
                }
            }

            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    var pixel = pixels[x, y];
                    byte r = this.remapColor(reds, simplificationAmount, pixel.R);
                    byte g = this.remapColor(blues, simplificationAmount, pixel.G); 
                    byte b = this.remapColor(greens, simplificationAmount, pixel.B);

                    pixels[x, y] = Color.FromArgb(pixel.A, r, g, b);
                }
            }
        }

        private byte remapColor(int[] hues, byte simplificationAmount, byte r)
        {
            int startIndex = (r / simplificationAmount) * simplificationAmount;
            int maxVal = 0;
            int indexOfMostCommonVal = startIndex;

            for (int i = startIndex; i < startIndex + simplificationAmount && i < 256; i++)
            {
                if(hues[i] > maxVal)
                {
                    maxVal = hues[i];
                    indexOfMostCommonVal = i;
                }
            }

            return (byte)indexOfMostCommonVal;
        }        
    }
}
