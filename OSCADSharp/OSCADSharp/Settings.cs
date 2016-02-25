using OSCADSharp.Files;
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
        /// Path to the OpenSCAD executable for file invocation
        /// (Default value is set the default install directory on Windows)
        /// </summary>
        public static string OpenSCADPath = @"C:\Program Files (x86)\OpenSCAD\openscad.exe";
    }
}
