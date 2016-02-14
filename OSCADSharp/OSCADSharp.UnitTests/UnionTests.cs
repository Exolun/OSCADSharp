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
    public class UnionTests
    {
        [TestMethod]
        public void Union_AffectedObjectsAreChildren()
        {
            var union = new Cube().Union(new Cylinder());
            var children = union.Children();

            Assert.IsTrue(children.ElementAt(0).GetType() == typeof(Cube));
            Assert.IsTrue(children.ElementAt(1).GetType() == typeof(Cylinder));
        }

        [TestMethod]
        public void Union_AffectedObjectsAreInOutputScript()
        {
            var union = new Cube().Union(new Cylinder());
            string script = union.ToString();

            Assert.IsTrue(script.Contains("cube("));
            Assert.IsTrue(script.Contains("cylinder("));
        }

        [TestMethod]
        public void Union_FirstStatementInOutputScriptIsUnionMethodCall()
        {
            var union = new Cube().Union(new Cylinder());
            string script = union.ToString();

            Assert.IsTrue(script.StartsWith("union()"));
        }

        [TestMethod]
        public void Union_FirstElementAppearsFirstInOutputScript()
        {
            var union = new Cube().Union(new Cylinder());
            string script = union.ToString();

            Assert.IsTrue(script.IndexOf("cube(") < script.IndexOf("cylinder("));
        }
    }
}
