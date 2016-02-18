using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Solids;
using System.Linq;

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

        [TestMethod]
        public void Difference_MinusOperatorCreatesDifferenceObject()
        {
            var obj = new Cube() - new Sphere();
            var diff = new Sphere().Difference(new Cube());

            Assert.IsTrue(obj.GetType() == diff.GetType());
        }

        [TestMethod]
        public void Difference_ChainingMinusOperationAddsToTheSameDifferenceObject()
        {
            var obj = new Cube() - new Sphere() - new Text3D("WOW!") - new Cylinder().Translate(1, 2, 5);
            var diff = new Sphere().Difference(new Cube());

            var childrenThatAreDiffs = obj.Children().Where(child => child.GetType() == diff.GetType()).Count();

            Assert.AreEqual(0, childrenThatAreDiffs);
        }
    }
}
