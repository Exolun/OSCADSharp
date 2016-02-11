using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    public class Sizes
    {
        public const double OneInch = 25.4;
        public const double HalfInch = OneInch / 2;
        public const double QuarterInch = HalfInch / 2;
        public const double EigthInch = QuarterInch / 2;
        public const double SixteenthInch = EigthInch / 2;

        public double InchesToMilimeters(double inches)
        {
            return inches * 0.03937008;
        }
    }
}
