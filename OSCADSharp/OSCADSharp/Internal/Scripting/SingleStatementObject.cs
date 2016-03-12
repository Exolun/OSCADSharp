using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// A statement with just one nested child node
    /// </summary>
    internal abstract class SingleStatementObject : OSCADObject
    {
        protected OSCADObject obj;

        protected SingleStatementObject(OSCADObject obj)
        {
            this.obj = obj;

            this.children.Add(obj);
            obj.Parent = this;
        }        
    }
}
