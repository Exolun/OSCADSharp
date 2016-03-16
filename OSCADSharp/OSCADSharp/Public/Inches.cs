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
        public static double One { get; private set; } = 25.4;

        /// <summary>
        /// Half of an imperial inch
        /// </summary>
        public static double Half { get; private set; } = One / 2;

        /// <summary>
        /// Quarter of an imperial inch
        /// </summary>
        public static double Quarter { get; private set; } = Half / 2;

        /// <summary>
        /// Eigth of an imperial inch
        /// </summary>
        public static double Eigth { get; private set; } = Quarter / 2;

        /// <summary>
        /// Sixteenth of an imperial inch
        /// </summary>
        public static double Sixteenth { get; private set; } = Eigth / 2;

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
