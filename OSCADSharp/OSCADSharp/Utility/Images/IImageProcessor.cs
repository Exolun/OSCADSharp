using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Utility.Images
{
    internal interface IImageProcessor
    {
        OSCADObject ProcessImage();
        Bounds ImageBounds { get; set; }
    }
}
