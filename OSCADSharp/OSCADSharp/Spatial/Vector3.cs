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
        /// <summary>
        /// X component of this vector
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y component of this vector
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Z component of this vector
        /// </summary>
        public double Z { get; set; }
        #endregion
        
        /// <summary>
        /// Creates a new Vector with the specified X/Y/Z values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
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

        /// <summary>
        /// Gets the Dot product of two vectors
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double Dot(Vector3 other)
        {
            return this.X * other.X + this.Y * other.Y + this.Z * other.Z;
        }

        #region Operators/Overrides
        /// <summary>
        /// Compares this vector to another object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// Gets a hashcode that's based on the the string for this vector
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Compares two vectors
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return left.X == right.X &&
                left.Y == right.Y &&
                left.Z == right.Z;
        }

        /// <summary>
        /// Does a negated comparison of two vectors
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return !(left.X == right.X &&
                left.Y == right.Y &&
                left.Z == right.Z);
        }
        
        /// <summary>
        /// Adds two vectors
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        /// <summary>
        /// Subtracts two vectors
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        /// <summary>
        /// Multiplies two vectors together
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 left, Vector3 right)
        {
            return new Vector3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        /// <summary>
        /// Multiplies (scales) a vector by a double
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 left, double right)
        {
            return new Vector3(left.X * right, left.Y * right, left.Z * right);
        }

        /// <summary>
        /// Muptiplies (scales) a vector by a double
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts this object to an OpenSCAD script
        /// </summary>
        /// <returns>Script for this object</returns>
        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}]", this.X.ToString(), this.Y.ToString(), this.Z.ToString());
        }
    }
}
