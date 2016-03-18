using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.DataBinding;
using OSCADSharp.Spatial;
using OSCADSharp.Utility;
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

        [TestMethod]
        public void Translate_CanBindVector()
        {
            var cube = new Cube().Translate(10, 0, 0);
            var vec = new Variable("vec", new Vector3(0, 20, 30));

            cube.Bind("vector", vec);

            string script = cube.ToString();
            Assert.IsTrue(script.Contains("v = vec"));
        }

        [TestMethod]
        public void Translate_CanBindVectorsByParameters()
        {
            var y = new Variable("yAmt", -35);
            var z = new Variable("zAmt", 40);

            var cube = new Cube().Translate(-5, y, z);

            string script = cube.ToString();
            Assert.IsTrue(script.Contains("v = [-5, yAmt, zAmt]"));
        }

        [TestMethod]
        public void Translate_AddingTwoVariablesYieldsInlineOperation()
        {
            var cylinDiameter = new Variable("cylinDiam", Inches.One);
            var wallThickness = new Variable("wallThickness", Inches.Eigth);

            var cylin = new Cylinder(cylinDiameter, cylinDiameter);
            cylin.Height = 15;

            var obj = cylin.Translate(cylinDiameter + wallThickness, 0, 0);

            string script = obj.ToString();
            Assert.IsTrue(script.Contains("v = [cylinDiam + wallThickness, 0, 0]"));
        }
    }
}
