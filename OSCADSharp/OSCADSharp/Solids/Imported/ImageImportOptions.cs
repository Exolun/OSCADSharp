using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Solids.Imported
{
    
    /// <summary>
    /// Configuration options for processing imported images
    /// </summary>
    public class ImageImportOptions
    {
        /// <summary>
        /// Determines how height should be applied to imported images
        /// </summary>
        public enum HeightMappingMode
        {
            /// <summary>
            /// No height mapping at all
            /// </summary>
            None,
            /// <summary>
            /// Extends texture upward with a flat base
            /// </summary>
            Vertical,
            /// <summary>
            /// Height is applied so both sides of the image are textured
            /// </summary>
            Bidirectional
        }

        /// <summary>
        /// Indicates whether height-mapping should be used
        /// </summary>
        public HeightMappingMode HeightMapping { get; set; } = HeightMappingMode.Vertical;

        /// <summary>
        /// Converts the colors in the image to black and white
        /// </summary>
        public bool UseGrayScale { get; set; } = false;

        /// <summary>
        /// Reduces the total number of colors in the image by merging similar colors together.        
        /// </summary>
        public byte SimplificationAmount { get; set; } = 0;

        /// <summary>
        /// Height to resize the image to
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Width to resize the image to
        /// </summary>
        public int? Width { get; set; }
    }
}
