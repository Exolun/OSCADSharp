using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OSCADSharp.UnitTests
{
    [TestClass]
    public class IntersectionTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Intersection_PositionThrowsNotSupportedException()
        {
            var obj = new Sphere().Intersection(new Text3D("Sup"));

            var pos = obj.Position();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Intersection_BoundsThrowsNotSupportedException()
        {
            var obj = new Sphere().Intersection(new Text3D("Sup"));

            var pos = obj.Bounds();
        }
    }
}
