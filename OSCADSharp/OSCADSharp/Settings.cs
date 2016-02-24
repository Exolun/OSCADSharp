using OSCADSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// Settings for OpenSCAD scripts
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Code-gen header
        /// </summary>
        public static readonly string OSCADSharpHeader = String.Format("/*Code Generated using OSCADSharp on {0}. {1}{2}For more information, please visit https://github.com/Exolun/OSCADSharp */{3}", 
            DateTime.Now.ToString(), Environment.NewLine, Environment.NewLine, Environment.NewLine);

        /// <summary>
        /// Global variables that can be assigned for output at the 
        /// top of OpenSCAD scripts
        /// </summary>
        public static Variables Globals = new Variables();
    }
}
