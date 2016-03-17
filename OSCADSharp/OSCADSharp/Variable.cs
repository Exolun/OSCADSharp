using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// A value for setting object properties in script output to
    /// a specific variable
    /// </summary>
    public class Variable
    {
        /// <summary>
        /// Creates a new Variable with the specified text/value
        /// </summary>
        /// <param name="text">Text of the variable.  This is the text that will appear in script output for this variable</param>
        /// <param name="value">The variable's value</param>
        /// <param name="addGlobal">A flag indicating whether to add this variable to Variables.Global</param>
        public Variable(string text, object value, bool addGlobal = false)
        {
            this.Text = text;
            this.Value = value;

            if (addGlobal)
            {
                Variables.Global.Add(this);
            }
        }

        /// <summary>
        /// Text of the variable
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Value of the variable.
        /// 
        /// Must be compatible with the data type being assigned to.
        /// </summary>
        public object Value { get; set; }
             
        /// <summary>
        /// Gets this variable as a text = value string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} = {1}", this.Text, this.Value.ToString());
        }

        #region Operators
        private static Variable applyMixedOperatorLeft(string oprtor, Variable left, object right, Func<object, object, object> calcMethod)
        {
            if (VariableCalculator.IsNumeric(right))
            {
                return new CompoundVariable(String.Format("{0} {1} {2}", left.Text, oprtor, right.ToString()),
                    calcMethod(left.Value, right));
            }

            throw new NotSupportedException(String.Format("Cannot use {0} operator on a variable with an object of type {1}",
                oprtor, typeof(object).ToString()));
        }

        private static Variable applyMixedOperatorRight(string oprtor, object left, Variable right, Func<object, object, object> calcMethod)
        {
            if (VariableCalculator.IsNumeric(left))
            {
                return new CompoundVariable(String.Format("{0} {1} {2}", left.ToString(), oprtor, right.Text),
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
            return new CompoundVariable(String.Format("{0} + {1}", left.Text, right.Text), VariableCalculator.Add(left.Value, right.Value));
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
            return new CompoundVariable(String.Format("{0} - {1}", left.Text, right.Text), VariableCalculator.Subtract(left.Value, right.Value));
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

            return new CompoundVariable(String.Format("-{0}", right.Text), value);
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
            return new CompoundVariable(String.Format("{0} * {1}", left.Text, right.Text), VariableCalculator.Multiply(left.Value, right.Value));
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
            return new CompoundVariable(String.Format("{0} / {1}", left.Text, right.Text), VariableCalculator.Divide(left.Value, right.Value));
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
