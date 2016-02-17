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
    internal class MinkowskiedObject : MultiBlockStatementObject
    {
        
        public MinkowskiedObject(IEnumerable<OSCADObject> children) : base("minkowski()", children)
        {
        }

        public override Vector3 Position()
        {
            throw new NotSupportedException("Position is not supported on Minkowskied objects.");
        }
    }
}
