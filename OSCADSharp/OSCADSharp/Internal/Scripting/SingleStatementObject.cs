using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.Spatial;

namespace OSCADSharp.Scripting
{
    /// <summary>
    /// A statement with just one nested child node
    /// </summary>
    internal abstract class SingleStatementObject : OSCADObject
    {
        protected OSCADObject obj;

        public SingleStatementObject(OSCADObject obj)
        {
            this.obj = obj;

            this.children.Add(obj);
            obj.Parent = this;
        }        
    }
}
