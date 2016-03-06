using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Scripting;
using OSCADSharp.Solids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.UnitTests.Transforms
{
    [TestClass]
    public class RotateTests
    {
        [TestMethod]
        public void Rotate_CanBindAngle()
        {
            var cyl = new Cylinder().Rotate(0, 90, 0);
            var rotVar = new Variable("myRot", new Vector3(120, 90, 30));

            cyl.Bind("angle", rotVar);

            string script = cyl.ToString();
            Assert.IsTrue(script.Contains("rotate(myRot)"));
        }

        [TestMethod]
        public void Rotate_CanBindAngleWithParameters()
        {
            var xAngle = new Variable("xAngle", 30);
            var yAngle = new Variable("yAngle", -20);

            var cyl = new Cylinder().Rotate(xAngle, yAngle, 120);

            string script = cyl.ToString();
            Assert.IsTrue(script.Contains("rotate([xAngle, yAngle, 120])"));
        }

        [TestMethod]
        public void Rotate_CanMultiplyAVectorVariableInline()
        {
            var rotation = new Variable("cubeRot", 15);

            var cube = new Cube(20, 20, 80, true).Rotate(rotation * 1.5, 30, 30);

            string script = cube.ToString();

            Assert.IsTrue(script.Contains("rotate([cubeRot * 1.5, 30, 30])"));
        }
    }
}
