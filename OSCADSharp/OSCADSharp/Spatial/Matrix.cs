using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Spatial
{
    /// <summary>
    /// A Matrix for performing operations on doubles that
    /// represent spatial positions
    /// </summary>
    internal class Matrix
    {
        #region Fields/Properties
        private double[] values;

        internal int ColumnCount { get; private set; }
        internal int RowCount { get; private set; }

        internal double[] GetValues()
        {
            return this.values;
        }
        #endregion

        #region Constructors
        internal Matrix(double[] values, int numRows, int numColumns)
        {
            this.values = values;
            this.RowCount = numRows;
            this.ColumnCount = numColumns;
        }

        internal Matrix(List<double> values, int numColumns)
        {
            this.values = values.ToArray();
            this.ColumnCount = numColumns;
            this.RowCount = values.Count / numColumns;
        }
        #endregion

        #region Public API
        internal Matrix Multiply(Matrix other)
        {
            double[] otherValues = other.GetValues();
            List<double> result = new List<double>();
            int currentRowInResult = 0;

            //Iterate over each row in this matrix
            for (int row = 0; row < this.RowCount; row++)
            {
                //And iterate over each column in the other matrix
                for (int column = 0; column < other.ColumnCount; column++)
                {
                    // Multiply item in the current row on the left, by each item in column right
                    // and add it to the result in the corresponding row/column
                    for (int leftMatrixColumn = 0; leftMatrixColumn < this.ColumnCount; leftMatrixColumn++)
                    {
                        result.Add(0);
                        result[currentRowInResult * other.ColumnCount + column] +=
                            this.values[row * this.ColumnCount + leftMatrixColumn] *
                            otherValues[leftMatrixColumn * other.ColumnCount + column];
                    }
                }

                currentRowInResult++;
            }

            return new Matrix(result, other.ColumnCount);
        }
        #endregion

        #region Static Transformation Matrices
        private static readonly double piOver180 = Math.PI / 180;
        private static double toRadians(double degrees)
        {
            return piOver180 * degrees;
        }

        internal static Matrix Identity()
        {
            return new Matrix(new double[] {
                                1, 0, 0, 0,
                                0, 1, 0, 0,
                                0, 0, 1, 0,
                                0, 0, 0, 1}, 4, 4); ;
        }

        /// <summary>
        /// Gets a transformation matrix for performing rotations on the X-Axis
        /// (Assuming you are in a right-handed 3D world)
        /// </summary>
        /// <param name="angle">Degrees of rotation</param>
        /// <returns>Transformation matrix to perform the rotation</returns>
        internal static Matrix XRotation(double angle)
        {
            if (angle == 0)
                return Identity();

            double radAngle = toRadians(angle);
            double[] rotationArr = new double[] {
                1 , 0, 0, 0,
                0, Math.Cos(radAngle), Math.Sin(radAngle), 0,
                0, -Math.Sin(radAngle), Math.Cos(radAngle), 0,
                0, 0, 0, 1
            };

            return new Matrix(rotationArr, 4, 4);
        }

        /// <summary>
        /// Gets a transformation matrix for performing rotations on the Y-Axis
        /// (Assuming you are in a right-handed 3D world)
        /// </summary>
        /// <param name="angle">Degrees of rotation</param>
        /// <returns>Transformation matrix to perform the rotation</returns>
        internal static Matrix YRotation(double angle)
        {
            if (angle == 0)
                return Identity();

            double radAngle = toRadians(angle);
            double[] rotationArr = new double[] {
                Math.Cos(radAngle), 0, -Math.Sin(radAngle), 0,
                0, 1, 0, 0,
                Math.Sin(radAngle), 0, Math.Cos(radAngle), 0,
                0, 0, 0, 1
            };

            return new Matrix(rotationArr, 4, 4);
        }

        /// <summary>
        /// Gets a transformation matrix for performing rotations on the Z-Axis
        /// (Assuming you are in a right-handed 3D world)
        /// </summary>
        /// <param name="angle">Degrees of rotation</param>
        /// <returns>Transformation matrix to perform the rotation</returns>
        internal static Matrix ZRotation(double angle)
        {
            if (angle == 0)
                return Identity();

            double radAngle = toRadians(angle);
            double[] rotationArr = new double[] {
                Math.Cos(radAngle), Math.Sin(radAngle), 0, 0,
                -Math.Sin(radAngle), Math.Cos(radAngle), 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            };

            return new Matrix(rotationArr, 4, 4);
        }

        /// <summary>
        /// Gets a point's position after rotations have been applied
        /// </summary>
        /// <param name="point">Point to rotate</param>
        /// <param name="xAngle"></param>
        /// <param name="yAngle"></param>
        /// <param name="zAngle"></param>
        /// <returns>Point after rotation</returns>
        internal static Vector3 GetRotatedPoint(Vector3 point, double xAngle, double yAngle, double zAngle)
        {
            var x = XRotation(-xAngle).Multiply(point.ToMatrix());
            var y = YRotation(-yAngle).Multiply(x);
            var z = ZRotation(-zAngle).Multiply(y).GetValues();
            return new Vector3(z[0], z[1], z[2]);
        }
        #endregion
    }
}
