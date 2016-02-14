using OSCADSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Transforms
{
    /// <summary>
    /// Creates an object that's the minkowski sum of child objects
    /// </summary>
    internal class MinkowskiedObject : BlockStatementObject
    {
        
        public MinkowskiedObject(IEnumerable<OSCADObject> children) : base("minkowski()", children)
        {
        }        
    }
}
