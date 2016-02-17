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
    }
}
