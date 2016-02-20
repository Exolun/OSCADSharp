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
    public class ResizeTests
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Resize_PositionNotSupportedAfterResize()
        {
            var pos = new Cube(5, 5, 20)
                .Translate(30, 0, 0).Rotate(0, 90, 0).Resize(2, 2, 2).Position();
        }
    }
}
