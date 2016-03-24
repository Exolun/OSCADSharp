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

        [TestMethod]
        public void Union_AddingObjectsCreatesUnion()
        {
            var obj = new Cube() + new Cylinder();
            var union = new Cube().Union(new Cylinder());

            Assert.IsTrue(obj.GetType() == union.GetType());
        }

        [TestMethod]
        public void Union_ChainingAdditionAddsToTheSameUnion()
        {
            var obj = new Cube() + new Sphere() + new Text3D("WOW!") + new Cylinder().Translate(1, 2, 5);
            var union = new Sphere().Union(new Cube());

            var unionChildrenCount = obj.Children().Where(child => child.GetType() == union.GetType()).Count();

            Assert.AreEqual(0, unionChildrenCount);
        }

        [TestMethod]
        public void Union_BoundsForUnionedObjectsBecomeExtremitiesOfBoth()
        {
            var obj = new Cube(5, 10, 20) + new Sphere(10).Translate(-10, 5, 0);

            var bounds = obj.Bounds();

            Assert.AreEqual(20, bounds.ZMax);
            Assert.AreEqual(10, bounds.YMax);
            Assert.AreEqual(5, bounds.XMax);

            Assert.AreEqual(-5, bounds.ZMin);
            Assert.AreEqual(0, bounds.YMin);
            Assert.AreEqual(-15, bounds.XMin);
        }
    }
}
