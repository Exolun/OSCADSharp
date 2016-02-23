using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Scripting
{
    /// <summary>
    /// Extends the capabilities of StringBuilder with domain-specific behavior
    /// </summary>
    internal class StatementBuilder
    {
        private StringBuilder SB { get; set; } = new StringBuilder();

        /// <summary>
        /// Special append method for conditionally adding value-pairs
        /// </summary>
        /// <param name="name">The Name of the value-pair</param>
        /// <param name="value">The value - if null this method does nothing</param>
        /// <param name="prefixWithComma">(optional) Flag indicating whether a comma should be added before the value-pair</param>
        public void AppendValuePairIfExists(string name, object value, bool prefixWithComma = false)
        {
            if (!String.IsNullOrEmpty(value?.ToString()))
            {
                if (prefixWithComma)
                {
                    SB.Append(", ");
                }

                SB.Append(name);
                SB.Append("=");
                SB.Append(value);
            }
        }

        /// <summary>
        /// Pass-through for StringBuilder.Append
        /// </summary>
        /// <param name="text"></param>
        public void Append(string text)
        {
            SB.Append(text);
        }

        /// <summary>
        /// Pass-through for StringBuilder.AppendLine
        /// </summary>
        /// <param name="text"></param>
        public void AppendLine(string text)
        {
            SB.AppendLine(text);
        }

        /// <summary>
        /// Gets this builder's full string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return SB.ToString();
        }
    }
}
