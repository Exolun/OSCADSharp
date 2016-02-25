using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OSCADSharp.Files;
using OSCADSharp.Scripting;
using OSCADSharp.Solids;
using System;
using System.Collections.Concurrent;
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
        public void OSCADObject_IdsAreSequentialAndDistinct()
        {
            var obj1 = new Sphere();
            var obj2 = new Cube();
            var obj3 = new Sphere();
           

            Assert.IsTrue(obj1.Id < obj2.Id && obj2.Id < obj3.Id);

            Assert.IsTrue(obj1.Id + 1 == obj2.Id);
            Assert.IsTrue(obj2.Id + 1 == obj3.Id);
        }

        [TestMethod]
        public void OSCADObject_ParallelObjectCreationDoesNotYieldDuplicateIds()
        {
            ConcurrentBag<OSCADObject> bag = new ConcurrentBag<OSCADObject>();

            Parallel.For(0, 1000, (i) => {
                bag.Add(new Sphere());
            });

            var ids = bag.Select(obj => obj.Id).ToList();
            Assert.AreEqual(ids.Count, ids.Distinct().Count());
        }

        [TestMethod]
        public void OSCADObject_EachOscadObjectChildHasDistinctId()
        {
            var obj = new Cube(5, 5, 10, true)
                .Translate(0, 5, 10).Rotate(0, 90, 0)
                .Translate(0, 0, 10).Scale(1, 1, 2);

            List<uint> ids = obj.Children().Select(child => child.Id).ToList();
            ids.Add(obj.Id);

            Assert.AreEqual(ids.Count, ids.Distinct().Count());
        }

        [TestMethod]
        public void OSCADObject_ClonedObjectsRetainTheirNamesAfterBasicTransforms()
        {
            var obj = new Cube() { Name = "Cube" }
                            .Translate(1, 1, 1);
            obj.Name = "TranslatedCube";
            obj = obj.Rotate(0, 90, 0);
            obj.Name = "RotatedAndTranslatedCube";
            obj = obj.Scale(1, 1, 2);
            obj.Name = "ScaledAndRotatedAndTranslatedCube";

            var clone = obj.Clone();

            Assert.AreEqual("ScaledAndRotatedAndTranslatedCube", clone.Name);

            var children = clone.Children().ToList();
            Assert.AreEqual("RotatedAndTranslatedCube", children[0].Name);
            Assert.AreEqual("TranslatedCube", children[1].Name);
            Assert.AreEqual("Cube", children[2].Name);
        }

        [TestMethod]
        public void OSCADObject_ClonedObjectsRetainNamesAfterBooleanOperations()
        {
            //Union, Difference using operators
            var obj = new Cube() { Name = "Cube" }
                + new Cylinder() { Name = "Cylinder" } - new Sphere() { Name = "Sphere" };

            obj = obj.Intersection(new Text3D("Heyyy") { Name="Text" });

            var clone = obj.Clone();

            var children = clone.Children().ToList();
            Assert.AreEqual("Text", children[5].Name);
            Assert.AreEqual("Cylinder", children[3].Name);
            Assert.AreEqual("Cube", children[4].Name);
            Assert.AreEqual("Sphere", children[1].Name);
        }

        [TestMethod]
        public void OSCADObject_ChildrenWithRecursiveFalseReturnsOnlyDirectChildren()
        {
            var firstLevel = new Sphere().Union(new Cube(), new Sphere(), new Cylinder());
            firstLevel.Name = "Union";
            var secondLevel = new Text3D() { Name = "Text" }.Difference(firstLevel);

            var children = secondLevel.Children(false).ToList();

            Assert.AreEqual("Text", children[0].Name);
            Assert.AreEqual("Union", children[1].Name);
        }

        [TestMethod]
        public void OSCADObject_ToFileIncludesOSCADSharpGeneratedHeader()
        {
            var cube = new Cube();
            string[] output = null;

            var mock = new Mock<IFileWriter>();
            mock.Setup(_wrtr => _wrtr.WriteAllLines(It.IsAny<string>(), It.IsAny<string[]>()))
                .Callback<string, string[]>((path, contents) => { output = contents; });
            Dependencies.FileWriter = mock.Object;

            cube.ToFile("myFile");

            Assert.AreEqual(Settings.OSCADSharpHeader, output[0]);
       }

        [TestMethod]
        public void OSCADObject_ToFileIncludesGlobalVariablesDefinedInSettings()
        {
            var cube = new Cube();
            string[] output = null;
            Variables.Global["$fn"] = 100;

            var mock = new Mock<IFileWriter>();
            mock.Setup(_wrtr => _wrtr.WriteAllLines(It.IsAny<string>(), It.IsAny<string[]>()))
                .Callback<string, string[]>((path, contents) => { output = contents; });
            Dependencies.FileWriter = mock.Object;

            cube.ToFile("myFile");

            Assert.AreEqual("$fn = 100;\r\n", output[1]);
        }
    }
}

