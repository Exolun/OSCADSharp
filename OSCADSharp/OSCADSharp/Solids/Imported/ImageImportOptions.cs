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
        /// Indicates whether height-mapping should be used
        /// </summary>
        public bool HeightMapping { get; set; } = true;
    }
}
