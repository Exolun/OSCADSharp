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
    public class OSCADObjectTests
    {
        [TestMethod]
        public void OSCADObject_ChildrenForSimpleStructureYieldsAllChildren()
        {
            var cube = new Cube();
            var translatedCube = cube.Translate(1, 2, 5);

            //Should contain both translation and Cube
            var coloredTranslatedCube = translatedCube.Color("Red");
            List<OSCADObject> expectedChildren = new List<OSCADObject>() { cube, translatedCube };

            var children = coloredTranslatedCube.Children();

            Assert.IsTrue(children.Contains(cube));
            Assert.IsTrue(children.Contains(translatedCube));
            Assert.IsFalse(children.Contains(coloredTranslatedCube));
        }

        [TestMethod]
        public void OSCADObject_ClonesContainChildren()
        {
            var text = new Text3D("Hi").Rotate(90, 0, 0);

            var clone = text.Clone();

            //Clone has a child, and it should be the same thing
            Assert.IsTrue(clone.Children().Count() == 1);
            Assert.IsTrue(clone.Children().FirstOrDefault().GetType() == text.Children().FirstOrDefault().GetType());

            //But the child should be a different instance
            Assert.IsFalse(clone.Children().FirstOrDefault() == text.Children().FirstOrDefault());
        }

        [TestMethod]
        public void OSCADObject_MimickedObjectHasSameTransform()
        {
            var cube = new Cube(null, true).Translate(10, 0, 5);
            var sphere = new Sphere().Mimic(cube);

            Assert.IsTrue(sphere.GetType() == cube.GetType());
            Assert.IsTrue(cube.ToString().StartsWith("translate("));
            Assert.IsTrue(sphere.ToString().StartsWith("translate("));
        }
    }
}
