using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.DataBinding
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

        #region CompoundVariable
        private class CompoundVariable : Variable
        {
            internal CompoundVariable(string name, object value) : base(name, value, false)
            {
            }
        }
        #endregion

        #region VariableCalculator
        private static class VariableCalculator
        {
            internal static bool IsNumeric(object value)
            {
                return value is int || value is double || value is float || value is decimal;
            }

            internal static bool IsVector(object value)
            {
                return value is Vector3 || value is BindableVector;
            }

            private static object computeExpression(BinaryExpression expr, object left, object right)
            {
                object result = null;

                if (IsNumeric(left) && IsNumeric(right))
                    result = Expression.Lambda<Func<double>>(expr).Compile()();
                if (IsVector(left) || IsVector(right))
                    result = Expression.Lambda<Func<Vector3>>(expr).Compile()();

                return result;
            }

            private static BinaryExpression makeExpression(Func<ConstantExpression, ConstantExpression, BinaryExpression> methodToUse,
                object left, object right)
            {
                if (IsNumeric(left))
                    left = Convert.ToDouble(left);
                if (IsNumeric(right))
                    right = Convert.ToDouble(right);

                var leftExpr = Expression.Constant(left, left.GetType());
                var rightExpr = Expression.Constant(right, right.GetType());
                BinaryExpression expr = methodToUse(leftExpr, rightExpr);
                return expr;
            }

            internal static object Add(object left, object right)
            {
                BinaryExpression expr = makeExpression(Expression.Add, left, right);
                return computeExpression(expr, left, right);
            }

            internal static object Subtract(object left, object right)
            {
                BinaryExpression expr = makeExpression(Expression.Subtract, left, right);
                return computeExpression(expr, left, right);
            }

            internal static object Multiply(object left, object right)
            {
                BinaryExpression expr = makeExpression(Expression.Multiply, left, right);
                return computeExpression(expr, left, right);
            }

            internal static object Divide(object left, object right)
            {
                BinaryExpression expr = makeExpression(Expression.Divide, left, right);
                return computeExpression(expr, left, right);
            }
        }

        #endregion
    }
}
