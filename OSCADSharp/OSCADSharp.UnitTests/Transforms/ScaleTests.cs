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
    }
}
