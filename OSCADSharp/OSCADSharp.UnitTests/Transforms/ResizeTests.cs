using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Solids;
using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.UnitTests.Transforms
{
    [TestClass]
    public class ResizeTests
    {
        [TestMethod]
        public void Resize_BoundaryBasedPositionAfterResizeIsInExpectedLocation()
        {
            var pos = new Cube(5, 5, 20)
                .Translate(30, 0, 0).Rotate(0, 90, 0).Resize(2, 2, 2).Position();

            Assert.AreEqual(new Vector3(1, 1, -13), pos);
        }

        [TestMethod]
        public void Resize_ResizeScalesBoundariesToFit()
        {
            var obj = new Cube(20, 20, 10).Resize(5, 5, 5);

            var bounds = obj.Bounds();
            Assert.AreEqual(new Vector3(5, 5, 5), bounds.TopRight);
            Assert.AreEqual(new Vector3(0, 0, 0), bounds.BottomLeft);
        }
    }
}
