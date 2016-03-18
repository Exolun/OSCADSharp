using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.IO
{
    /// <summary>
    /// A class that takes text and writes to file
    /// </summary>
    public interface IFileWriter
    {
        /// <summary>
        /// Writes lines of text to a file at the path specified
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        void WriteAllLines(string path, string[] contents);
    }
}
