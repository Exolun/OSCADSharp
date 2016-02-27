using System;
using System.Collections.Concurrent;
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
        /// <summary>
        /// Global variables that can be assigned for output at the 
        /// top of OpenSCAD scripts
        /// </summary>
        public static Variables Global = new Variables();
        private ConcurrentDictionary<string, Variable> variables = new ConcurrentDictionary<string, Variable>();

        /// <summary>
        /// Adds a variable to the collection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, object value)
        {
            this.variables[name] = new Variable(name, value);
        }

        /// <summary>
        /// Removes a variable from the collection
        /// </summary>
        /// <param name="name"></param>
        public Variable Remove(string name)
        { 
            Variable value;
            this.variables.TryRemove(name, out value);
            return value;            
        }

        /// <summary>
        /// Gets a variable by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Variable Get(string name)
        {
            return this.variables[name];         
        }

        /// <summary>
        /// Assigns or gets a variable's value
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Variable this[string name]   // long is a 64-bit integer
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
                if(kvp.Value == null || String.IsNullOrEmpty(kvp.Value.ToString()) || String.IsNullOrEmpty(kvp.Key))
                {
                    continue;
                }
                
                sb.Append(kvp.Value.ToString());
                sb.Append(";");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
