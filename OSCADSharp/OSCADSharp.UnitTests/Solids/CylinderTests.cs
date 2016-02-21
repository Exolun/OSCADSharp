using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
