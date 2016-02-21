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
    public class Text3DTests
    {
        [TestMethod]
        public void Text_PositionIsCentered()
        {
            var text = new Text3D("Bom chicka bow wow");

            Assert.AreEqual(new Vector3(), text.Position());
        }

        [TestMethod]
        public void Text_BoundsBasedOnFontSize()
        {
            var obj = new Text3D("BBBB", 16);

            Assert.AreEqual(new Vector3(32, 8, 0.5), obj.Bounds().TopRight);
            Assert.AreEqual(new Vector3(-32, -8, -0.5), obj.Bounds().BottomLeft);
        }
    }
}
