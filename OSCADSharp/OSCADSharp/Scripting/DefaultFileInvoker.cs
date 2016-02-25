using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Scripting
{
    internal class DefaultFileInvoker : IFileInvoker
    {
        private string filePath;
        public DefaultFileInvoker(string filePath)
        {
            this.filePath = filePath;
        }

        public void CreateModel(string outputFile)
        {
            Process.Start(Settings.OpenSCADPath, String.Format("-o {0} {1}", outputFile, this.filePath));
        }        

        public void Open()
        {
            Process.Start(Settings.OpenSCADPath, String.Format("{0}", this.filePath));
        }
    }
}
