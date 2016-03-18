using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Utility;
using OSCADSharp.Spatial;

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

        [TestMethod]
        public void Sphere_ScriptOutputDoesNotContainResolutionValuesIfNotSpecified()
        {
            var sphere = new Sphere();

            string script = sphere.ToString();

            Assert.IsTrue(!script.Contains("$fn"));
            Assert.IsTrue(!script.Contains("$fa"));
            Assert.IsTrue(!script.Contains("$fs"));
        }

        [TestMethod]
        public void Sphere_ScriptOutpuHasResolutionValuesIfSpecified()
        {
            var sphere = new Sphere()
            {
                Resolution = 40,
                MinimumAngle = 5,
                MinimumFragmentSize = 2
            };

            string script = sphere.ToString();

            Assert.IsTrue(script.Contains("$fn"));
            Assert.IsTrue(script.Contains("$fa"));
            Assert.IsTrue(script.Contains("$fs"));
        }

        [TestMethod]
        public void Sphere_RadiusVariableBoundAppearsInOutput()
        {
            string variableName = "mySphereRadius";
            double radius = 15;

            Variables.Global.Add(variableName, radius);

            var sphere = new Sphere();
            sphere.Bind("Radius", Variables.Global["mySphereRadius"]);
            Assert.IsTrue(sphere.Radius == radius);

            string script = sphere.ToString();
            Assert.IsTrue(script.Contains("r = mySphereRadius"));
        }

        [TestMethod]
        public void Sphere_BindingDiameterSetsDiameterInOutput()
        {
            string variableName = "diam";
            double diam = 20;

            Variables.Global.Add(variableName, diam);

            var sphere = new Sphere();
            sphere.Bind("Diameter", Variables.Global["diam"]);
            Assert.IsTrue(sphere.Diameter == diam);

            string script = sphere.ToString();
            Assert.IsTrue(script.Contains("d = diam"));
        }

        [TestMethod]
        public void Sphere_ResolutionAngleAndFragmentSizeTest()
        {
            var resolution = new Variable("resolution", 30);
            var angle = new Variable("angle", 5);
            var fragSize = new Variable("fragSize", 10);

            var sphere = new Sphere();
            sphere.Bind("Resolution", resolution);
            sphere.Bind("MinimumAngle", angle);
            sphere.Bind("MinimumFragmentSize", fragSize);

            string script = sphere.ToString();
            Assert.IsTrue(script.Contains("$fn = resolution"));
            Assert.IsTrue(script.Contains("$fa = angle"));
            Assert.IsTrue(script.Contains("$fs = fragSize"));
        }

        [TestMethod]
        public void Sphere_CanCreateSphereWithBindingsFromConstructor()
        {
            var diam = new Variable("width", Inches.One);
            var resolution = new Variable("rez", 100);

            var sphere = new Sphere(diam, resolution);

            Assert.AreEqual(diam.Value, sphere.Diameter);
            Assert.AreEqual(resolution.Value, sphere.Resolution);

            string script = sphere.ToString();

            Assert.IsTrue(script.Contains("d = width"));
            Assert.IsTrue(script.Contains("$fn = rez"));
        }

        [TestMethod]
        public void Sphere_BindingsAreClonedWithObject()
        {
            var diam = new Variable("width", Inches.One);
            var resolution = new Variable("rez", 100);
            var scale = new Variable("theScale", new Vector3(1, 2, 3));

            var sphere = new Sphere(diam, resolution).Scale(scale);
            var clone = sphere.Clone();

            string script = clone.ToString();

            Assert.IsTrue(script.Contains("d = width"));
            Assert.IsTrue(script.Contains("$fn = rez"));
            Assert.IsTrue(script.Contains("scale(v = theScale)"));
        }
    }
}
