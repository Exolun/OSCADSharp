using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Solids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.UnitTests.Transforms
{

    [TestClass]
    public class TranslateTests
    {
        [TestMethod]
        public void Translate_BoundsMoveWhenObjectIsTranslated()
        {
            var cube = new Cube();
            var boundsBefore = cube.Bounds();
            var boundsAfter = cube.Translate(5, 2, 3).Bounds();

            Assert.AreEqual(boundsAfter.TopRight, boundsBefore.TopRight + new Vector3(5, 2, 3));
            Assert.AreEqual(boundsAfter.BottomLeft, boundsBefore.BottomLeft + new Vector3(5, 2, 3));
        }
    }
}
