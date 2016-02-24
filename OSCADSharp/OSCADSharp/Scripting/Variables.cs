using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Scripting
{
    /// <summary>
    /// A collection of names/values for variables
    /// </summary>
    public sealed class Variables
    {
        private Dictionary<string, object> variables = new Dictionary<string, object>();

        /// <summary>
        /// Assigns or gets a variable's value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]   // long is a 64-bit integer
        {
            get
            {
                if (variables.ContainsKey(name))
                {
                    return variables[name];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                variables[name] = value;
            }
        }

        /// <summary>
        /// This class is intended for use in other externally-exposed classes,
        /// as such its constructor is not public.
        /// </summary>
        internal Variables()
        {
        }

        /// <summary>
        /// Gets the string representation for all variables
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in this.variables)
            {
                sb.Append(kvp.Key);
                sb.Append(" = ");
                sb.Append(kvp.Value);
                sb.Append(";");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
