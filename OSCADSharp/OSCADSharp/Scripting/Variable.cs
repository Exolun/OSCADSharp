using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Scripting
{
    /// <summary>
    /// A value for setting object properties in script output to
    /// a specific variable
    /// </summary>
    public class Variable
    {
        /// <summary>
        /// Creates a new Variable with the specified name/value
        /// </summary>
        /// <param name="name">Name of the variable.  This is the name that will appear in script output</param>
        /// <param name="value">The variable's value</param>
        public Variable(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Name of the variable
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value of the variable.
        /// 
        /// Must be compatible with the data type being assigned to.
        /// </summary>
        public object Value { get; set; }
             
        /// <summary>
        /// Gets this variable as a name = value string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} = {1}", this.Name, this.Value.ToString());
        }

        #region Operators
        /// <summary>
        /// Adds two variables together
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variable operator +(Variable left, Variable right)
        {
            return new Variable(String.Format("{0} + {1}", left.Name, right.Name), VariableCalculator.Add(left.Value, right.Value));
        }

        /// <summary>
        /// Subtracts two variables
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variable operator -(Variable left, Variable right)
        {
            return new Variable(String.Format("{0} - {1}", left.Name, right.Name), VariableCalculator.Subtract(left.Value, right.Value));
        }

        /// <summary>
        /// Multiplies two variables
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variable operator *(Variable left, Variable right)
        {
            return new Variable(String.Format("{0} * {1}", left.Name, right.Name), VariableCalculator.Multiply(left.Value, right.Value));
        }

        /// <summary>
        /// Divides two variables
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variable operator /(Variable left, Variable right)
        {
            return new Variable(String.Format("{0} / {1}", left.Name, right.Name), VariableCalculator.Divide(left.Value, right.Value));
        }
        #endregion
    }
}
