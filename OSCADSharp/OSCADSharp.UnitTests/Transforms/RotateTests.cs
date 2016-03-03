using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Scripting;
using OSCADSharp.Solids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.UnitTests.Transforms
{
    [TestClass]
    public class RotateTests
    {
        [TestMethod]
        public void Rotate_CanBindAngle()
        {
            var cyl = new Cylinder().Rotate(0, 90, 0);
            var rotVar = new Variable("myRot", new Vector3(120, 90, 30));

            cyl.Bind("angle", rotVar);

            string script = cyl.ToString();
            Assert.IsTrue(script.Contains("rotate(myRot)"));
        }
    }
}
