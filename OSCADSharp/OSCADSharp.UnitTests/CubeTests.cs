using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Solids;

namespace OSCADSharp.UnitTests
{
    [TestClass]
    public class CubeTests
    {
        [TestMethod]
        public void Cube_LengthWidthHeightAffectSizeVector()
        {
            const double length = 10;
            const double width = 7;
            const double height = 9;

            var cube = new Cube(length, width, height);

            Assert.AreEqual(length, cube.Size.X);
            Assert.AreEqual(width, cube.Size.Y);
            Assert.AreEqual(height, cube.Size.Z);
        }

        [TestMethod]
        public void Cube_SizeAppearsInOutput()
        {
            var cube = new Cube(new Vector3(1.5, 5.5, 8.7));

            string script = cube.ToString();

            Assert.IsTrue(script.Contains(String.Format("size = [{0}, {1}, {2}]", 1.5, 5.5, 8.7)));
        }

        [TestMethod]
        public void Cube_CloneYieldsSameScript()
        {
            var cube = new Cube(new Vector3(1.5, 5.5, 8.7));

            var clone = cube.Clone();

            Assert.IsTrue(clone.IsSameAs(cube));
        }

        [TestMethod]
        public void Cube_ParameterlessCubeHasMethodCallInIt()
        {
            var cube = new Cube();

            string script = cube.ToString();

            Assert.IsTrue(script.StartsWith("cube("));
            Assert.IsTrue(script.EndsWith(");"));
        }
    }
}
