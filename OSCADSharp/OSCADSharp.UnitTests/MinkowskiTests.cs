using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Solids;

namespace OSCADSharp.UnitTests
{
    [TestClass]
    public class MinkowskiTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Minkowski_PositionThrowsNotSupportedException()
        {
            var obj = new Cylinder().Intersection(new Sphere()).Translate(0, 5, 5);

            var pos = obj.Position();
        }
    }
}
