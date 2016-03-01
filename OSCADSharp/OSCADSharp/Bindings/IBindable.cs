using OSCADSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Bindings
{
    /// <summary>
    /// An object whose properties can be bound to variables
    /// </summary>
    public interface IBindable
    {
        /// <summary>
        /// Binds a variable to property of this object
        /// </summary>
        /// <param name="property"></param>
        /// <param name="variable"></param>
        void Bind(string property, Variable variable);
    }
}
