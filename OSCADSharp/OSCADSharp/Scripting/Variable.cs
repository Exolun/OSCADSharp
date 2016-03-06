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
        private static Variable applyMixedOperatorLeft(string oprtor, Variable left, object right, Func<object, object, object> calcMethod)
        {
            if (VariableCalculator.IsNumeric(right))
            {
                return new Variable(String.Format("{0} {1} {2}", left.Name, oprtor, right.ToString()),
                    calcMethod(left.Value, right));
            }

            throw new NotSupportedException(String.Format("Cannot use {0} operator on a variable with an object of type {1}",
                oprtor, typeof(object).ToString()));
        }

        private static Variable applyMixedOperatorRight(string oprtor, object left, Variable right, Func<object, object, object> calcMethod)
        {
            if (VariableCalculator.IsNumeric(left))
            {
                return new Variable(String.Format("{0} {1} {2}", left.ToString(), oprtor, right.Name),
                    calcMethod(left, right.Value));
            }

            throw new NotSupportedException(String.Format("Cannot use {0} operator on a variable with an object of type {1}",
                oprtor, typeof(object).ToString()));
        }

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
        /// Adds a value to a variable
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variable operator +(Variable left, object right)
        {
            return applyMixedOperatorLeft("+", left, right, VariableCalculator.Add);            
        }

        /// <summary>
        /// Adds a value to a variable
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variable operator +(object left, Variable right)
        {
            return applyMixedOperatorRight("+", left, right, VariableCalculator.Add);
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
        /// Numerical negation on a variable
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variable operator -(Variable right)
        {
            object value = null;

            if (VariableCalculator.IsNumeric(right.Value))
            {
                value = -Convert.ToDouble(right.Value);
            }            
            else if(VariableCalculator.IsVector(right.Value))
            {
                value = ((Vector3)right.Value).Negate();
            }

            return new Variable(String.Format("-{0}", right.Name), value);
        }

        /// <summary>
        /// Subtracts a value from a variable
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variable operator -(Variable left, object right)
        {
            return applyMixedOperatorLeft("-", left, right, VariableCalculator.Subtract);
        }

        /// <summary>
        /// Subtracts a value from a variable
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variable operator -(object left, Variable right)
        {
            return applyMixedOperatorRight("-", left, right, VariableCalculator.Subtract);
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
        /// Multiplies a variable by a value
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variable operator *(Variable left, object right)
        {
            return applyMixedOperatorLeft("*", left, right, VariableCalculator.Multiply);
        }

        /// <summary>
        /// Multiplies a variable by a value
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variable operator *(object left, Variable right)
        {
            return applyMixedOperatorRight("*", left, right, VariableCalculator.Multiply);
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

        /// <summary>
        /// Divides a variable by a value
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variable operator /(Variable left, object right)
        {
            return applyMixedOperatorLeft("/", left, right, VariableCalculator.Divide);
        }

        /// <summary>
        /// Divides a variable by a value
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variable operator /(object left, Variable right)
        {
            return applyMixedOperatorRight("/", left, right, VariableCalculator.Divide);
        }
        #endregion
    }
}
