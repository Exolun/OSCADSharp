using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Solids.Imported
{
    /// <summary>
    /// Ways to process imported images into 3D models
    /// </summary>
    public enum ImageImportMode
    {
        /// <summary>
        /// Cubist import mode subdivides the image into rectangular sections,
        /// forming the bitmap from groupings of cubes representing its pixels
        /// </summary>
        Cubist,

        /// <summary>
        /// Polygonal import model scans the image for sections that can be represented
        /// by whole polygons for each contiguous area
        /// </summary>
        Polygonal
    }
}
