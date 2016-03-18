using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.UnitTests
{
    [TestClass]
    public class MirrorTests
    {
        [TestMethod]
        public void Mirror_SingleAxisMirrorInvertsPosition()
        {
            var cube = new Cube(5, 10, 20);
            var xMirror = cube.Clone().Mirror(1, 0, 0);
            var yMirror = cube.Clone().Mirror(0, 1, 0);
            var zMirror = cube.Clone().Mirror(0, 0, 1);

            var pos = cube.Position().Clone();
            pos.X = -pos.X;
            Assert.AreEqual(pos, xMirror.Position());

            pos = cube.Position().Clone();
            pos.Y = -pos.Y;
            Assert.AreEqual(pos, yMirror.Position());

            pos = cube.Position().Clone();
            pos.Z = -pos.Z;
            Assert.AreEqual(pos, zMirror.Position());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Mirror_MultiAxisPositionThrowsNotSupportedException()
        {
            var cube = new Cube(5, 10, 20);

            var pos = cube.Mirror(1, 1, 0).Position();
        }

        [TestMethod]
        public void Mirror_SingleAxisMirrorInvertsBounds()
        {
            OSCADObject cube = new Cube(5, 10, 20);
            var boundsBefore = cube.Bounds();
            cube = cube.Mirror(0, 1, 0);

            Assert.AreEqual(cube.Bounds().YMax, boundsBefore.YMin);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Mirror_MultiAxisBoundsThrowsNotSupportedException()
        {
            var cube = new Cube(5, 10, 20);

            var pos = cube.Mirror(1, 1, 0).Bounds();
        }

        [TestMethod]
        public void Mirror_CanBindNormal()
        {
            var cube = new Cube(5, 20, 15).Mirror(1, 0, 0);
            cube.Bind("normal", new Variable("myVar", new Vector3(1, 0, 0)));

            string script = cube.ToString();
            Assert.IsTrue(script.Contains("mirror(myVar)"));
        }

        [TestMethod]
        public void Mirror_CanBindNormalWithParameter()
        {
            var cube = new Cube(5, 20, 15).Mirror(new Variable("myVar", new Vector3(1, 0, 0)));

            string script = cube.ToString();
            Assert.IsTrue(script.Contains("mirror(myVar)"));
        }

        [TestMethod]
        public void Mirror_VariablesForXandZ()
        {
            var x = new Variable("xComp", 0);
            var z = new Variable("zComp", 1);

            var cube = new Cube().Mirror(x, 0, z);
            string script = cube.ToString();
            Assert.IsTrue(script.Contains("mirror([xComp, 0, zComp])"));
        }
    }
}
