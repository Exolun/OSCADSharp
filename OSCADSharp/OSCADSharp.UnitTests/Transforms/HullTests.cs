using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.UnitTests
{

    [TestClass]
    public class HullTests
    {
        [TestMethod]
        public void Hull_BasisObjectAppearsInHullChildren()
        {
            var cube = new Cube();
            var sphere = new Sphere().Translate(0, 0, 2);
            var hull = cube.Hull(sphere);

            var children = hull.Children();

            Assert.IsTrue(children.Contains(cube));
            Assert.IsTrue(children.Contains(sphere));
        }
    }
}
