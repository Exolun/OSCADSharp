using OSCADSharp.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.IO
{   
    /// <summary>
    /// Settings for OpenSCAD scripts
    /// </summary>
    public static class OutputSettings
    {   
        /// <summary>
        /// Code-gen header
        /// </summary>
        public static string OSCADSharpHeader { get; private set; } = String.Format("/*Code Generated using OSCADSharp on {0}. {1}{2}For more information, please visit https://github.com/Exolun/OSCADSharp */{3}",
            DateTime.Now.ToString(), Environment.NewLine, Environment.NewLine, Environment.NewLine);

        /// <summary>
        /// Path to the OpenSCAD executable for file invocation
        /// (Default value is set the default install directory on Windows)
        /// </summary>
        public static string OpenSCADPath { get; set; } = new OpenSCADPathFinder().GetPath();

        /// <summary>
        /// Known where the default installation locations for OpenSCAD
        /// are, and finds the right path for the current environment
        /// </summary>
        private class OpenSCADPathFinder
        {
            private string[] possibleFilePaths = new string[]
            {
            @"C:\Program Files (x86)\OpenSCAD\openscad.exe",
            @"C:\Program Files\OpenSCAD\openscad.exe"
            };

            internal string GetPath()
            {
                foreach (string path in possibleFilePaths)
                {
                    if (File.Exists(path))
                        return path;
                }

                return null;
            }
        }
    }
}
