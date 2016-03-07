using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// Invokes OpenSCAD actions on output files
    /// 
    /// If these methods don't work, double check that your Settings.OpenSCADPath value is correct
    /// </summary>
    public interface IFileInvoker
    {
        /// <summary>
        /// Launches OpenSCAD with the specified file open
        /// </summary>
        void Open();

        /// <summary>
        /// Generates an STL from this file with the specified path/name
        /// </summary>
        /// <param name="outputFile"></param>
        void CreateModel(string outputFile);        
    }
}
