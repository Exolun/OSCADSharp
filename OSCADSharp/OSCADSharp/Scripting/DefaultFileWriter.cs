using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Scripting
{
    internal class DefaultFileWriter : IFileWriter
    {
        public void WriteAllLines(string path, string[] contents)
        {
            File.WriteAllLines(path, contents);
        }
    }
}
