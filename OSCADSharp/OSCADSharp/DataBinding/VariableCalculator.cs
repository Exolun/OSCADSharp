using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.DataBinding
{
    internal static class VariableCalculator
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
}
