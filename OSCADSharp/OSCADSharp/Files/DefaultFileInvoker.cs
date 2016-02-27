using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Files
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
            try
            {
                Process.Start(Settings.OpenSCADPath, String.Format("-o {0} {1}", outputFile, this.filePath));
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Cannot open because Settings.OpenSCADPath is not a valid file path.  Please check that the path defined points to you OpenSCAD executable.");
            }
        }        

        public void Open()
        {
            try
            {
                Process.Start(Settings.OpenSCADPath, String.Format("{0}", this.filePath));
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Cannot open because Settings.OpenSCADPath is not a valid file path.  Please check that the path defined points to you OpenSCAD executable.");
            }
        }
    }
}
