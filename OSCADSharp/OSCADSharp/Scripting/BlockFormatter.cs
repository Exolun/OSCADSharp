using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Scripting
{
    /// <summary>
    /// A class that creates blocks of curly-braced script with the
    /// specified level of indentation
    /// </summary>
    internal class BlockFormatter
    {
        private string outerCode;
        private string innerCode;
        private string indentationAmount = "    ";

        internal BlockFormatter(string outerCode, string innerCode)
        {
            this.outerCode = outerCode;
            this.innerCode = innerCode;
        }

        //TODO: Assess perf and find a better performing way to apply indentation
        // this approach will likely be a big performance drain with deeply nested structures.
        // -MLS 2/12/2016
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(outerCode);
            sb.AppendLine("{");
            using (StringReader reader = new StringReader(this.innerCode))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    sb.Append(this.indentationAmount);
                    sb.Append(line);
                    sb.Append(Environment.NewLine);
                }
            }
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
