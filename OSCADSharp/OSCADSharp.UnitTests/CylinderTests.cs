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
    public class CylinderTests
    {
        [TestMethod]
        public void Cylinder_ConstructorParametersAffectScriptOutput()
        {
            var cylinder = new Cylinder(5.5, 12.1, true);

            string script = cylinder.ToString();

            Assert.IsTrue(script.Contains("r1 = 2.75"));
            Assert.IsTrue(script.Contains("r2 = 2.75"));
            Assert.IsTrue(script.Contains("h = 12.1"));
            Assert.IsTrue(script.Contains("center = true"));
        }
    }
}
