using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Scripting
{
    /// <summary>
    /// Opens a file
    /// </summary>
    public interface IFileInvoker
    {
        /// <summary>
        /// Opens the specified file
        /// </summary>
        /// <param name="path">Path to the file to open</param>
        /// <param name="arguments">Command-line arguments to pass in</param>
        void Invoke(string path, string arguments = null);
    }
}
