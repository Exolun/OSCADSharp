using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// A Three-Dimensional vector
    /// 
    /// Can be used to represent a direction, or a point in space
    /// </summary>
    public class Vector3
    {
        #region Attributes
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        #endregion
                
        public Vector3(double x = 0, double y = 0, double z = 0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        
        /// <summary>
        /// Negates the values of this vector, returning an inverse of it
        /// </summary>
        /// <returns>A negated vector</returns>
        public Vector3 Negate()
        {
            return new Vector3(-this.X, -this.Y, -this.Z);
        }

        /// <summary>
        /// Creates a copy of this vector that's a new instance
        /// with the same values
        /// </summary>
        /// <returns>A clone of this vector</returns>
        public Vector3 Clone()
        {
            return new Vector3(this.X, this.Y, this.Z);
        }

        /// <summary>
        /// Returns the average position of the provided positions
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        public static Vector3 Average(params Vector3[] positions)
        {
            if(positions == null || positions.Length == 0)
            {
                return null;
            }
            else if (positions.Length == 1)
            {
                return positions[0];
            }

            var sum = new Vector3();

            foreach (var pos in positions)
            {
                sum += pos;
            }

            return new Vector3(sum.X / positions.Length, sum.Y / positions.Length, sum.Z / positions.Length);
        }

        /// <summary>
        /// Returns the unit vector for this vector
        /// </summary>
        /// <returns></returns>
        public Vector3 Normalize()
        {
            if(this.X == 0 && this.Y == 0 && this.Z == 0)
            {
                return this;
            }

            double sum = Math.Abs(this.X) + Math.Abs(this.Y) + Math.Abs(this.Z);
            return new Vector3(this.X / sum, this.Y / sum, this.Z / sum);
        }

        public double Dot(Vector3 other)
        {
            return this.X * other.X + this.Y * other.Y + this.Z * other.Z;
        }

        #region Operators/Overrides
        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return left.X == right.X &&
                left.Y == right.Y &&
                left.Z == right.Z;
        }

        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return !(left.X == right.X &&
                left.Y == right.Y &&
                left.Z == right.Z);
        }
        
        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Vector3 operator *(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        public static Vector3 operator *(Vector3 left, double right)
        {
            return new Vector3(left.X * right, left.Y * right, left.Z * right);
        }

        public static Vector3 operator *(double left, Vector3 right)
        {
            return new Vector3(left * right.X, left * right.Y, left * right.Z);
        }
        #endregion

        internal Matrix ToMatrix()
        {
            double[] coords = { this.X, this.Y, this.Z, 0 };
            return new Matrix(coords, 4, 1);
        }

        public override string ToString()
        {
            return String.Format("[X: {0}, Y: {1}, Z: {2}]", this.X.ToString(), this.Y.ToString(), this.Z.ToString());
        }
    }
}
