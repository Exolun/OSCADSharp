using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Scripting;
using OSCADSharp.Solids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.UnitTests
{
    [TestClass]
    public class CylinderTests
    {
        [TestMethod]
        public void Cylinder_ConstructorParametersAffectScriptOutput()
        {
            var cylinder = new Cylinder(5.5, 12.1, true);

            string script = cylinder.ToString();

            Assert.IsTrue(script.Contains("r1 = 2.75"));
            Assert.IsTrue(script.Contains("r2 = 2.75"));
            Assert.IsTrue(script.Contains("h = 12.1"));
            Assert.IsTrue(script.Contains("center = true"));
        }

        [TestMethod]
        public void Cylinder_UncenteredPositionZValueIsHalfTheHeight()
        {
            var cylinder = new Cylinder(3, 40);

            Assert.AreEqual(new Vector3(0, 0, 20), cylinder.Position());
        }

        [TestMethod]
        public void Cylinder_CenteredCylinderPositionIsZero()
        {
            var cylinder = new Cylinder(5, 20, true);

            Assert.AreEqual(new Vector3(), cylinder.Position());
        }

        [TestMethod]
        public void Cylinder_BoundsAreInExpectedPositionNotCentered()
        {
            var obj = new Cylinder(5, 20);

            Assert.AreEqual(new Vector3(2.5, 2.5, 20), obj.Bounds().TopRight);
            Assert.AreEqual(new Vector3(-2.5, -2.5, 0), obj.Bounds().BottomLeft);
        }

        [TestMethod]
        public void Cylinder_BoundsAreInExpectedPositionCentered()
        {
            var obj = new Cylinder(5, 20, true);

            Assert.AreEqual(new Vector3(2.5, 2.5, 10), obj.Bounds().TopRight);
            Assert.AreEqual(new Vector3(-2.5, -2.5, -10), obj.Bounds().BottomLeft);
        }

        [TestMethod]
        public void Cylinder_ScriptOutputDoesNotContainResolutionValuesIfNotSpecified()
        {
            var cylinder = new Cylinder();

            string script = cylinder.ToString();

            Assert.IsTrue(!script.Contains("$fn"));
            Assert.IsTrue(!script.Contains("$fa"));
            Assert.IsTrue(!script.Contains("$fs"));
        }

        [TestMethod]
        public void Cylinder_ScriptOutpuHasResolutionValuesIfSpecified()
        {
            var cylinder = new Cylinder()
            {
                Resolution = 40,
                MinimumAngle = 5,
                MinimumCircumferentialLength = 2
            };

            string script = cylinder.ToString();

            Assert.IsTrue(script.Contains("$fn"));
            Assert.IsTrue(script.Contains("$fa"));
            Assert.IsTrue(script.Contains("$fs"));
        }

        [TestMethod]
        public void Cylinder_CenterBindingAppearsInOutput()
        {
            Variable centerVal = new Variable("isCentered", true);

            var obj = new Cylinder();
            obj.Bind("Center", centerVal);

            string script = obj.ToString();

            Assert.AreEqual(centerVal.Value, obj.Center);
            Assert.IsTrue(script.Contains("center = isCentered"));
        }

        [TestMethod]
        public void Cylinder_R1R2BindingTest()
        {
            Variable radius1 = new Variable("radius1", 5);
            Variable radius2 = new Variable("radius2", 25);

            var obj = new Cylinder();
            obj.Bind("Radius1", radius1);
            obj.Bind("Radius2", radius2);

            string script = obj.ToString();

            Assert.AreEqual(Convert.ToDouble(radius1.Value), obj.Radius1);
            Assert.AreEqual(Convert.ToDouble(radius2.Value), obj.Radius2);
            Assert.IsTrue(script.Contains(String.Format("r1 = {0}", radius1.Name)));
            Assert.IsTrue(script.Contains(String.Format("r2 = {0}", radius2.Name)));
        }

        [TestMethod]
        public void Cylinder_D1RDBindingTest()
        {
            Variable d1 = new Variable("diameter2", 5);
            Variable d2 = new Variable("diameter2", 25);

            var obj = new Cylinder();
            obj.Bind("Diameter1", d1);
            obj.Bind("Diameter2", d2);

            string script = obj.ToString();

            Assert.AreEqual(Convert.ToDouble(d1.Value), obj.Diameter1);
            Assert.AreEqual(Convert.ToDouble(d2.Value), obj.Diameter2);
            Assert.IsTrue(script.Contains(String.Format("d1 = {0}", d1.Name)));
            Assert.IsTrue(script.Contains(String.Format("d2 = {0}", d2.Name)));
        }

        [TestMethod]
        public void Cylinder_DiameterAndRadiusBindingsAreMutuallyExclusive()
        {
            Variable diameter = new Variable("diameter", 15.5);
            Variable radius = new Variable("radius", 45);

            var obj = new Cylinder();
            obj.Bind("Radius", radius);

            string script = obj.ToString();

            Assert.AreEqual(Convert.ToDouble(radius.Value), obj.Radius);
            Assert.IsTrue(script.Contains(String.Format("r = {0}", radius.Name)));
            Assert.IsFalse(script.Contains(String.Format("d = {0}", diameter.Name)));

            obj.Bind("Diameter", diameter);

            script = obj.ToString();

            Assert.AreEqual(Convert.ToDouble(diameter.Value), obj.Diameter);
            Assert.IsTrue(script.Contains(String.Format("d = {0}", diameter.Name)));
            Assert.IsFalse(script.Contains(String.Format("r = {0}", radius.Name)));
        }

        [TestMethod]
        public void Cylinder_BindingsResolutionAngleAndFragmentSizeTest()
        {
            var resolution = new Variable("resolution", 30);
            var angle = new Variable("angle", 5);
            var circLength = new Variable("circLength", 10);

            var cylinder = new Cylinder();
            cylinder.Bind("Resolution", resolution);
            cylinder.Bind("MinimumAngle", angle);
            cylinder.Bind("MinimumCircumferentialLength", circLength);

            string script = cylinder.ToString();
            Assert.IsTrue(script.Contains("$fn = resolution"));
            Assert.IsTrue(script.Contains("$fa = angle"));
            Assert.IsTrue(script.Contains("$fs = circLength"));
        }
    }
}
