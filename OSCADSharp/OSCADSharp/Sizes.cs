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
    public class Sizes
    {
        /// <summary>
        /// One imperial inch
        /// </summary>
        public const double OneInch = 25.4;

        /// <summary>
        /// Half of an imperial inch
        /// </summary>
        public const double HalfInch = OneInch / 2;

        /// <summary>
        /// Quarter of an imperial inch
        /// </summary>
        public const double QuarterInch = HalfInch / 2;

        /// <summary>
        /// Eigth of an imperial inch
        /// </summary>
        public const double EigthInch = QuarterInch / 2;

        /// <summary>
        /// Sixteenth of an imperial inch
        /// </summary>
        public const double SixteenthInch = EigthInch / 2;

        /// <summary>
        /// Converts inches to millimeters
        /// </summary>
        /// <param name="inches">Number of inches</param>
        /// <returns>Equivalent value in milimeters</returns>
        public static double InchesToMillimeters(double inches)
        {
            return inches * OneInch;
        }
    }
}
