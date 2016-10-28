using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using OSCADSharp.Solids;

namespace OSCADSharp.UnitTests
{
    [TestClass]
    public class DifferenceTests
    {
        /// <summary>
        /// Note:  The decision that Difference should yield the position of the
        /// first child stems from the common use case of using Difference to bore out
        /// holes for pegs, screws, supports, etc.  It's often used in such a way that 
        /// the overall structure of the object being differenced is not greatly affected,
        /// thus the position should not change.
        /// -MLS 2/17/2016
        /// </summary>
        [TestMethod]     
        public void Difference_PositionYieldsPositionOfFirstChild()
        {
            var sphere = new Sphere().Translate(.25, .25, 1);
            var diff = sphere.Difference(new Cube());

            Assert.AreEqual(sphere.Position(), diff.Position());
        }

        [TestMethod]
        public void Difference_BoundsYieldsBoundsOfFirstChild()
        {
            var sphere = new Sphere().Translate(.25, .25, 1);
            var diff = sphere.Difference(new Cube());

            Assert.AreEqual(sphere.Bounds(), diff.Bounds());
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
