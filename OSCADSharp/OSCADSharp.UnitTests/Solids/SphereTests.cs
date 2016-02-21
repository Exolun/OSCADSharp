using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Solids;

namespace OSCADSharp.UnitTests
{
    [TestClass]
    public class SphereTests
    {
        private Sphere sphere;

        [TestInitialize]
        public void Setup()
        {
            this.sphere = new Sphere(10);
        }

        [TestMethod]
        public void Sphere_RadiusIsHalfofDiameter()
        {
            Assert.AreEqual(5, this.sphere.Radius);
        }

        [TestMethod]
        public void Sphere_ResolutionAffectsFNvalue()
        {
            this.sphere.Resolution = 30;

            string script = this.sphere.ToString();

            Assert.IsTrue(script.Contains("$fn = 30"));
        }

        [TestMethod]
        public void Sphere_FAandFSAreAffectedByAngleANdFragmentSize()
        {
            this.sphere.MinimumAngle = 2;
            this.sphere.MinimumFragmentSize = 4;

            string script = this.sphere.ToString();

            Assert.IsTrue(script.Contains("$fa = 2"));
            Assert.IsTrue(script.Contains("$fs = 4"));
        }

        [TestMethod]
        public void Sphere_ParameterlessSphereHasMethodCallAndEndsWithSemicolon()
        {
            var basicSphere = new Sphere();

            string script = basicSphere.ToString();

            Assert.IsTrue(script.StartsWith("sphere("));
            Assert.IsTrue(script.TrimEnd().EndsWith(");"));
        }

        [TestMethod]
        public void Sphere_CloneYieldsSameScript()
        {
            var sphere = new Sphere()
            {
                Diameter = 10,
                MinimumAngle = 5,
                MinimumFragmentSize = 5,
                Resolution = 30
            };

            var clone = sphere.Clone();

            Assert.IsTrue(sphere.IsSameAs(clone));
        }

        [TestMethod]
        public void Sphere_PositionIsAtZero()
        {
            var sphere = new Sphere();

            Assert.AreEqual(new Vector3(), sphere.Position());
        }

        [TestMethod]
        public void Sphere_BoundsAreInExpectedPositionCentered()
        {
            var obj = new Sphere(30);

            Assert.AreEqual(new Vector3(15, 15, 15), obj.Bounds().TopRight);
            Assert.AreEqual(new Vector3(-15, -15, -15), obj.Bounds().BottomLeft);
        }
    }
}
