using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Transforms
{
    internal interface IMimicer
    {
        OSCADObject MimicObject(OSCADObject obj);
    }
}
