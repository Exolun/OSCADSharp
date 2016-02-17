using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Solids;

namespace OSCADSharp.UnitTests
{
    [TestClass]
    public class DifferenceTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Difference_PositionThrowsNotSupportedException()
        {
            var diff = new Sphere().Difference(new Cube());

            var pos = diff.Position();
        }
    }
}
