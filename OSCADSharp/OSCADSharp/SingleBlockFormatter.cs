using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// A class that creates blocks of curly-braced script with the
    /// specified level of indentation
    /// </summary>
    internal class SingleBlockFormatter
    {
        private string outerCode;
        private string innerCode;
        private string indentationAmount = "    ";

        internal SingleBlockFormatter(string outerCode, string innerCode)
        {
            this.outerCode = outerCode;
            this.innerCode = innerCode;
        }

        // Assessed performance on this method by using Translate 1,000 times
        // on a Cube and converting it to a string. It completed in about 12.5 seconds.
        // This method was the bottleneck by far at 60% of the CPU time spent.
        // Although this is probably okay for generating scripts (that is a huge script), we should
        // refrain from using the stringified version of deeply nested objects for equivalence checks        
        // -MLS 2/13/2016        
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
