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
    public class ScaleTests
    {
        [TestMethod]
        public void Scale_TranslateRotateScaleStillYieldsCorrectPosition()
        {
            var obj = new Cube(5, 5, 20)
                   .Translate(30, 0, 0).Rotate(0, 90, 0).Scale(2, 2, 2);
            
            Assert.AreEqual(new Vector3(20, 5, -65), obj.Position());
        }

        [TestMethod]
        public void Scale_BoundsScaleWithObject()
        {
            var obj = new Cube(5, 5, 5).Scale(2, 2, 3);

            var bounds = obj.Bounds();
            Assert.AreEqual(bounds.X_Max, 10);
            Assert.AreEqual(bounds.Y_Max, 10);
            Assert.AreEqual(bounds.Z_Max, 15);

            Assert.AreEqual(bounds.X_Min, 0);
            Assert.AreEqual(bounds.Y_Min, 0);
            Assert.AreEqual(bounds.Z_Min, 0);
        }
    }
}
