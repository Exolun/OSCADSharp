using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// Knowns where the default installation locations for OpenSCAD
    /// are, and finds the right path for the current environment
    /// </summary>
    internal class OpenSCADPathFinder
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
