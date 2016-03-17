using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// Constants and conversions for units for us imperial-minded folks.
    /// </summary>
    public static class Inches
    {
        /// <summary>
        /// One imperial inch
        /// </summary>
        public const double One = 25.4;

        /// <summary>
        /// Half of an imperial inch
        /// </summary>
        public const double Half = One / 2;

        /// <summary>
        /// Quarter of an imperial inch
        /// </summary>
        public const double Quarter = Half / 2;

        /// <summary>
        /// Eigth of an imperial inch
        /// </summary>
        public const double Eigth = Quarter / 2;

        /// <summary>
        /// Sixteenth of an imperial inch
        /// </summary>
        public const double Sixteenth = Eigth / 2;

        /// <summary>
        /// Converts inches to millimeters
        /// </summary>
        /// <param name="inches">Number of inches</param>
        /// <returns>Equivalent value in milimeters</returns>
        public static double ToMillimeters(double inches)
        {
            return inches * One;
        }
    }
}
