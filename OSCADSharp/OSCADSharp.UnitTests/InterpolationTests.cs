using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Solids;
using OSCADSharp.Spatial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.UnitTests
{
    [TestClass]
    public class InterpolationTests
    {
        //Positive X rotation
        [TestMethod]
        public void Interpolation_RotateOnXAxis()
        {
            var cube = new Cube(9, 9, 9).Rotate(90, 0, 0);
            
            //Rotating on X axis by 90 should shift the center of the cube on the negative Y quadrant
            Assert.AreEqual(new Vector3(4.5, -4.5, 4.5), cube.Position());
        }

        //Negative X rotation
        [TestMethod]
        public void Interpolation_NegativeRotationOnXAxis()
        {
            var cube = new Cube(11, 11, 11).Rotate(-90, 0, 0);

            //Rotating on X axis by -90 should shift the center of the cube on the negative Z quadrant
            Assert.AreEqual(new Vector3(5.5, 5.5, -5.5), cube.Position());
        }

        //Y Rotation
        [TestMethod]
        public void Interpolation_PositiveYRotationWithTallCube()
        {
            var cube = new Cube(10, 12, 23).Rotate(0, 90, 0);

            //Rotating on Y axis by 90 should shift the center of the cube on the negative Z quadrant
            Assert.AreEqual(new Vector3(11.5, 6, -5), cube.Position());
        }

        //Negative Y rotation
        [TestMethod]
        public void Interpolation_NegativeYRotationWithWideCube()
        {
            var cube = new Cube(10, 30, 15).Rotate(0, -90, 0);

            //Rotating on Y axis by -90 should shift the center of the cube on the negative X quadrant
            Assert.AreEqual(new Vector3(-7.5, 15, 5), cube.Position());
        }

        //Z Rotation
        [TestMethod]
        public void Interpolation_ZRotationWithLongCube()
        {
            var cube = new Cube(10, 5, 2).Rotate(0, 0, 115);

            //Rotating on Z axis by 90 should shift the center of the cube on the negative X quadrant
            Assert.AreEqual(new Vector3(-4.37886077629512, 3.4749932808315, 1), cube.Position());
        }

        //Negative Z rotation
        [TestMethod]
        public void Interpolation_NegativeZRotation()
        {
            var cube = new Cube(10, 5, 2).Rotate(0, 0, -95);

            //Rotating on Z axis by 90 should shift the center of the cube on the negative Y quadrant
            Assert.AreEqual(new Vector3(2.05470803149107, -5.19886284732787, 1), cube.Position());
        }

        //Centered rotation (no change)
        [TestMethod]
        public void Interpolation_CenteredCubePositionNotUpdatedWhenRotated()
        {
            var cube = new Cube(5, 20, 20, true).Rotate(15, -120, 270);

            Assert.AreEqual(new Vector3(), cube.Position());
        }

        //X and Y rotation
        [TestMethod]
        public void Interpolation_XAndYRotation()
        {
            var cube = new Cube(5, 5, 5).Rotate(120, 45, 0);

            Assert.AreEqual(new Vector3(2.41481456572267, -3.4150635094611, -1.12071934021007), cube.Position());
        }

        //Y and Z rotation
        [TestMethod]
        public void Interpolation_YandZRotation()
        {
            var cube = new Cube(13, 13, 13).Rotate(0, 270, -35);

            Assert.AreEqual(new Vector3(-1.59624145159665, 9.05273512416025, 6.5), cube.Position());
        }

        //X and Z rotation
        [TestMethod]
        public void Interpolation_XandZRotation()
        {
            var cube = new Cube(13, 13, 13).Rotate(-145, 0, 190);

            Assert.AreEqual(new Vector3(-6.67843481376553, 0.443277802376793, -9.05273512416025), cube.Position());
        }

        //X, Y and Z rotation
        [TestMethod]
        public void Interpolation_XYZRotation()
        {
            var cube = new Cube(13, 13, 13).Rotate(90, 37.5, -180);

            Assert.AreEqual(new Vector3(-9.11374600044971, 6.5, 1.19984742333634), cube.Position());
        }

        [TestMethod]
        public void Interpolation_PositionAfterLotsOfOperations()
        {
            var obj = new Cube(5, 10, 20).Mirror(0, 0, 1).Mirror(0, 1, 0)
                .Rotate(15, -45, 120).Translate(-20, 10, 15).Rotate(90, 15, 25)
                .Translate(-10, -20, -20).Rotate(-90, -90, -45);

            var position = obj.Position();
            Assert.AreEqual(new Vector3(-21.7567866493247, 28.2686425980997, -21.6189570529939), position);
        }
    }
}
