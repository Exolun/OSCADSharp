using OSCADSharp.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// Contains definitions for external APIs used by OSCADSharp
    /// </summary>
    public static class Dependencies
    {
        /// <summary>
        /// Used to write scripts to file
        /// </summary>
        public static IFileWriter FileWriter = new DefaultFileWriter();

        /// <summary>
        /// Factory method to provide the class used to perform actions on output scripts
        /// </summary>
        public static Func<string, IFileInvoker> FileInvokerFactory = (path) => { return new DefaultFileInvoker(path); };
    }
}
