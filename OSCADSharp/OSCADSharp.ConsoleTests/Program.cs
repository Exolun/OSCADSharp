using OSCADSharp.Solids;
using OSCADSharp.Solids.Compound;
using OSCADSharp.Solids.Imported;
using OSCADSharp.Spatial;
using OSCADSharp.Utility;

namespace OSCADSharp.ConsoleTests
{
    class Program
    {
        private static OSCADObject getEndCover(OSCADObject outer)
        {
            var bounds = outer.Bounds();
            var endCover = new Cube(Inches.Sixteenth, bounds.Width, bounds.Height, true);
            var choppedOut = endCover.Clone().Scale(2, (bounds.Width-Inches.Quarter)/ bounds.Width, (bounds.Height - Inches.Half) / bounds.Height);
            choppedOut = choppedOut.Translate(0, Inches.Quarter, 0);

            return endCover - choppedOut;
        }

        private static void makeBracket()
        {
            OSCADObject cube = new Cube(new Vector3(Inches.ToMillimeters(2.75), Inches.One, Inches.ToMillimeters(1.25)), true);
            var outside = cube.Clone();
            ((Cube)outside).Size.X += Inches.Sixteenth;
            ((Cube)outside).Size.Z += Inches.Sixteenth;
            cube = cube.Scale(2, 1, 1).Translate(0, Inches.Sixteenth, 0);

            var obj = outside - cube;
            var bounds = obj.Bounds();

            obj = obj + getEndCover(outside).Translate(bounds.XMax, 0, 0);
            obj = obj + getEndCover(outside).Translate(-bounds.XMax, 0, 0);

            obj.ToFile("leftBracket").Open();
        }

        private static void makePeg()
        {
            OSCADObject flatInnerPortion = new Cylinder(Inches.Quarter, Inches.Eigth, true) { Resolution = 30 };
            OSCADObject shaft = new Cylinder(Inches.Eigth, Inches.Half, true) { Resolution = 30 };
            flatInnerPortion = flatInnerPortion.Translate(0, 0, shaft.Bounds().ZMax);

            OSCADObject pegShaft = new Cylinder(Inches.Quarter, Inches.Half - Inches.Eigth, true) { Resolution = 30 }
                .Translate(0, 0, -Inches.Eigth);
            OSCADObject bottomBall = new Sphere(Inches.Quarter* 1.1)
                .Translate(0, 0, pegShaft.Bounds().ZMin);

            var obj = flatInnerPortion + shaft + pegShaft + bottomBall;
            obj = obj.Rotate(0, 180, 0).Translate(0, 0, obj.Bounds().ZMax);

            obj.ToFile("peg");
        }

        private static void makeLaserStand()
        {
            OSCADObject center = new Cube(Inches.One, Inches.Half, Inches.One*1.2, true);
            OSCADObject outerColumn = new Cylinder(Inches.One, Inches.One, true).Resize(Inches.One * 1.5, Inches.Half * 1.5, Inches.One);
            var bnds = outerColumn.Bounds();
            OSCADObject bottom = new Cylinder(Inches.One, Inches.One, true)
                .Resize(Inches.One * 2, Inches.Half * 2, Inches.One)
                .Translate(0, 0, 0);



            var obj = (outerColumn) + bottom;

            bnds = obj.Bounds();
            var cap = makeStandCap();

            obj = obj + cap.Translate(0, 0, bnds.ZMax - Inches.Sixteenth);

            obj.ToFile("laserStand2").Open();

        }

        private static OSCADObject makeStandCap()
        {
            OSCADObject center = new Cube(Inches.One - Inches.Sixteenth, Inches.Half - Inches.Sixteenth, Inches.Quarter, true);

            OSCADObject outerColumn = new Cylinder(Inches.One, Inches.One, true).Resize(Inches.One * 1.5, Inches.Half * 1.5, Inches.One);
            var bnds = center.Bounds();

            OSCADObject top = new Cylinder(Inches.One, Inches.One, true)
                .Resize(Inches.One * 2.25, Inches.Half * 2.25, Inches.Quarter);
            OSCADObject cutout = new Cylinder(Inches.One, Inches.One, true)
                .Resize(Inches.One * 2.1, Inches.Half * 2.1, Inches.Quarter);
            top = top - cutout.Translate(0, 0, Inches.Quarter/2);


            OSCADObject brim = top.Clone();

            var obj = center + top.Translate(0, 0, bnds.Height / 2 + Inches.Sixteenth / 2);
            return obj;
        }

        private static void makeCardClip()
        {
            var leftSide = new Cube(Inches.Sixteenth, Inches.Half, Inches.Half, true)
                .Rotate(0, -3, 0).Translate(Inches.Sixteenth, 0, Inches.Half / 2);
            var rightSide = leftSide.Clone().Mirror(1, 0, 0);
            var bottom = new Cube(Inches.Quarter*.85, Inches.Half, Inches.Sixteenth, true);

            var obj = leftSide + rightSide + bottom;
            //obj = new Sphere() { Radius = .25, Resolution = 30 }.Translate(0, 0, -1).Minkowski(obj);

            //90 degree joint
            obj = obj.Translate(0, 0, Inches.Eigth) +
                obj.Clone().Rotate(0, 180, 0).Translate(0, 0, 0)
                + new Cube(Inches.Quarter * .85, Inches.Half, Inches.Quarter * .85, true);

            obj = obj.Rotate(90, 0, 0);

            obj.ToFile("clip-180");
        }

        public static void makeBadge()
        {
            var img = ImportedImage.FromFile("badge-1.png", new ImageImportOptions()
            {
                HeightMapping = ImageImportOptions.HeightMappingMode.Vertical                
            });
            img.Resize(Inches.One * 2.4, Inches.One * 2.75, Inches.Eigth).ToFile("badge");
        }

        public static void makeLoop()
        {
            double outerDiam = Inches.Quarter+ Inches.Sixteenth;
            double height = Inches.Eigth + Inches.Sixteenth;
            double holeSize = Inches.Quarter;
            double paddingSize = Inches.Sixteenth;


            var outer = new Cylinder(holeSize + paddingSize, height, true) { Resolution = 50 };
            var inner = new Cylinder(holeSize, height*2, true) { Resolution = 50 };            

            var bounds = outer.Bounds();
            var bottomDisk = new Cylinder(outerDiam + Inches.Eigth, Inches.Sixteenth / 2, true) { Resolution = 50 }.Translate(0, 0, bounds.ZMin - Inches.Sixteenth / 4);
        

            var whole = ((outer + bottomDisk) - inner);

            whole.ToFile("loop");

            var topDisk = new Cylinder(outerDiam + Inches.Eigth, Inches.Sixteenth / 2, true) - new Cylinder(holeSize + Inches.Sixteenth, Inches.Half, true) { Resolution = 50 };
            topDisk.ToFile("topLoop");
        }

        public static void makeHolder()
        {
            double innerSize = (Inches.One * 4);

            var cutout = new Cube(Inches.One * 10, Inches.Half, Inches.Half, true).Translate(0, innerSize/2, 0);

            cutout = cutout + cutout.Rotate(0, 0, 90) + cutout.Rotate(0, 0, 180) + cutout.Rotate(0, 0, 270);

            OSCADObject innerCyl = new Cylinder(innerSize, Inches.Eigth + Inches.Sixteenth, true) {
                Resolution = 60
            };
            var outerCyl = new Cylinder(innerSize + Inches.Eigth, Inches.Quarter + Inches.Sixteenth, true) {
                Resolution = 60
            };

            innerCyl = innerCyl.Translate(0, 0, outerCyl.Bounds().ZMax - (Inches.Eigth + Inches.Sixteenth)/2 + .1);
            var whole = (outerCyl - innerCyl) - cutout;
            whole.ToFile("coasterHolder-square").Open();
        }

        public static void makeDisk()
        {
            double glassGap = Inches.Eigth;
            double outer = (Inches.One * 3) + glassGap*2;
            double holeSize = (Inches.One - Inches.Sixteenth);
            double thickness = Inches.Quarter;

            var outerCylinder = new Cylinder(outer, thickness, true) { Resolution = 100 };
            var innerCutout = new Cylinder(outer - glassGap, thickness, true) { Resolution = 100 }.Translate(0, 0, thickness - Inches.Sixteenth);

            var hole = new Cylinder(holeSize, thickness * 2, true) { Resolution = 100 };

            var whole = (outerCylinder - innerCutout) - hole;
            whole.ToFile("candleDisk").Open();
        }

        public static void makePaddle()
        {
            double holeSize = Inches.One + Inches.Eigth;
            double holeHeight = Inches.One;
            double shaftSize = holeSize + Inches.Quarter;
            double totalLength = Inches.One * 8;
            double totalWidth = Inches.One * 5;

            var blade = new Sphere() { Resolution = 80 }.Resize(totalLength, totalWidth, Inches.One*2);
            var copy = blade.Clone().Translate(0, 0, -Inches.One).Scale(1.4, 1.4, 1.1);
            blade = blade - copy;

            //var gap = new Cube(Inches.One, Inches.One * 8, Inches.One*8, true).Translate(blade.Bounds().XMin, 0, 0);
            //blade = blade - gap;

            OSCADObject shaft = new Cylinder(shaftSize, totalLength / 4, true) { Resolution = 80 }.Rotate(0, 90, 0);
            var hole = new Cylinder(holeSize, holeHeight * 2, true) { Resolution = 80 }.Rotate(0, 90, 0).Translate(shaft.Bounds().XMin, 0, 0);
            blade = blade - hole.Translate(blade.Bounds().XMin + Inches.Half, 0, +Inches.Quarter);
            shaft = shaft - hole;
            shaft = shaft.Translate(blade.Bounds().XMin + Inches.Half, 0, +Inches.Quarter);

            var gapCloser = new Sphere(shaftSize) { Resolution = 80 }.Translate(shaft.Bounds().XMax, 0, +Inches.Quarter);

            var whole = (blade + shaft + gapCloser).Rotate(180, 0, 0);
            whole.ToFile("paddle").Open();
        }

        public static void makepaddleHandle()
        {
            double holeSize = Inches.One + Inches.Eigth;
            double holeHeight = Inches.One;
            double shaftSize = holeSize + Inches.Quarter;
            double totalLength = Inches.One * 8;
            double totalWidth = Inches.One * 5;

            var blade = new Sphere() { Resolution = 80 }.Resize(totalLength, totalWidth, Inches.One * 2);
            var copy = blade.Clone().Translate(0, 0, -Inches.One).Scale(1.4, 1.4, 1.1);
            blade = blade - copy;

            //var gap = new Cube(Inches.One, Inches.One * 8, Inches.One*8, true).Translate(blade.Bounds().XMin, 0, 0);
            //blade = blade - gap;

            OSCADObject shaft = new Cylinder(shaftSize, totalLength / 4, true) { Resolution = 80 }.Rotate(0, 90, 0);
            var hole = new Cylinder(holeSize, holeHeight * 2, true) { Resolution = 80 }.Rotate(0, 90, 0).Translate(shaft.Bounds().XMin, 0, 0);
            blade = blade - hole.Translate(blade.Bounds().XMin + Inches.Half, 0, +Inches.Quarter);
            shaft = shaft - hole;
            shaft = shaft.Translate(blade.Bounds().XMin + Inches.Half, 0, +Inches.Quarter);

            var top = new Cylinder(shaftSize, Inches.One * 3, true) { Resolution = 80 }.Rotate(90, 0, 0).Translate(shaft.Bounds().XMax, 0, shaft.Bounds().ZMax - shaftSize/2);
            var gapCloserLeft = new Sphere(shaftSize) { Resolution = 80 }.Translate(top.Bounds().XMax - shaftSize/2, top.Bounds().YMax, +Inches.Quarter);
            var gapCloserRight = gapCloserLeft.Mirror(0, 1, 0);

            var whole = shaft + top + gapCloserLeft + gapCloserRight;
            whole.ToFile("paddleHandle").Open();
        }

        public static void  makeGreatSword()
        {
            double referenceWidth = Inches.Eigth;
            double overallLength = Inches.One*1.5;

            var tang = new Cylinder(referenceWidth, overallLength, true) { Resolution = 6 }.Scale(.5, 2, 1);
            var pommel = new Sphere(referenceWidth * 1.2){ Resolution = 11 }.Translate(0, 0, -Inches.Half - Inches.Eigth).Color("Gold");
            var hilt = new Cylinder(referenceWidth, Inches.Half * .75, true) { Resolution = 10 }
                .Rotate(90, 0, 0)
                .Translate(0, 0, -Inches.Quarter).Color("Gold");
            var hiltRight = new Sphere(referenceWidth * 1.1) { Resolution = 8 }
            .Scale(1, 1, 1.2).Color("Gold")
                .Translate(0, hilt.Bounds().YMin, hilt.Position().Z);
            var hiltLeft = hiltRight.Clone().Mirror(0, 1, 0);
            var hiltCenter = new Cylinder(referenceWidth * .9, Inches.Half * .8, true) { Resolution = 8 }
                .Translate(0, 0, -Inches.Quarter - Inches.Quarter*.75).Color("Gold");
            hilt = hilt + hiltLeft + hiltRight + hiltCenter;

            var blade = tang.Translate(0, 0, Inches.Half);
            var tip = new Cylinder(referenceWidth, Inches.Quarter, true) { Resolution = 6, Diameter2 = .1 }.Scale(.5, 2, 1).Translate(0, 0, blade.Bounds().ZMax + Inches.Eigth);
            blade = blade + tip;
            blade = blade.Color("Silver");

            var whole = (blade + pommel + hilt).Scale(.5, .5, .5);
            whole.ToFile("greatsword").Open();
        }

        public static void makeACBrackets()
        {
            double width = Inches.One * 6.5;
            double height = Inches.One;
            double depth = Inches.One;
            double thickness = Inches.Quarter;

            var mainBox = new Cube(width, depth, height, true);
            var cutout = mainBox.Clone().Scale(1.1, 1, 1).Translate(0, thickness, thickness);
            var hole = new Cylinder(Inches.Eigth, Inches.One * 2, true) { Resolution = 30 }.Rotate(90, 0, 0);


            var whole = mainBox - cutout - hole.Translate(0, 0, Inches.Quarter) - 
                hole.Clone().Translate(-Inches.One * 2, 0, Inches.Quarter) - hole.Clone().Translate(+Inches.One * 2, 0, Inches.Quarter);
            whole.ToFile("acBracket");
        }

        public static void makeDiceHolder()
        {
            double chamberDiameter = Inches.One;

            var singleChamberCenter = new Cylinder(chamberDiameter, Inches.Half + Inches.Eigth, true) { Resolution = 6 };
            var singleChamber = new Cylinder(chamberDiameter + Inches.Eigth, Inches.Half + Inches.Eigth, true) { Resolution = 6 } - singleChamberCenter.Scale(1, 1, 2);
            singleChamber = singleChamber.Rotate(0, 0, 30);
            var bottom = singleChamber;

            for (int angle = 60; angle < 420; angle+=60)
            {
                bottom += singleChamber.Clone().Translate(chamberDiameter - Inches.Sixteenth, 0, 0).Rotate(0, 0, angle);
            }

            var hexSection = bottom.Clone();
            var outerCylCenter = new Cylinder(chamberDiameter * 2.9, Inches.Half, true) { Resolution = 90 }.Translate(0, 0, Inches.Eigth);
            var outerCyl = (new Cylinder(chamberDiameter * 2.9 + Inches.Eigth, Inches.Half, true) { Resolution = 90 } - outerCylCenter).Translate(0, 0, bottom.Bounds().ZMin + Inches.Half);

            bottom += outerCyl;

            var lidCenter = new Cylinder(chamberDiameter * 2.9 + Inches.Eigth + Inches.Sixteenth*.5, Inches.One, true) { Resolution = 90 }.Translate(0, 0, Inches.Eigth);
            var lid = (new Cylinder(chamberDiameter * 2.9 + Inches.Quarter + Inches.Sixteenth * .5, Inches.One, true) { Resolution = 90 } - lidCenter).Translate(0, 0, bottom.Bounds().ZMin + Inches.Half);            

            var top = lid.Clone().Mirror(0, 0, 1).Rotate(0, 180, 0);
            //OSCADObject dragon = ImportedImage.FromFile("dragonInsignia.png", new ImageImportOptions() {
            //    UseGrayScale = true,
            //    HeightMapping = ImageImportOptions.HeightMappingMode.None,
            //    SimplificationAmount = 100
            //});
            //var topBounds = top.Bounds();

            //dragon = dragon.Scale(.1, .1, Inches.Quarter).Translate(-topBounds.Length/2 + Inches.Sixteenth,  -topBounds.Width/2 + Inches.Sixteenth, topBounds.ZMin - Inches.Eigth);

            var whole = top;//bottom;// /*bottom + */ top - dragon;
            whole.ToFile("diceHolder_cap").Open();
        }

        public static void makeSideTwoByFourBracket()
        {
            double wallThickness = Inches.Eigth;

            double innerWidth = Inches.One * 4 + Inches.Sixteenth;
            double innerHeight = Inches.One * 2 + Inches.Sixteenth;
            double innerDepth = Inches.Half + Inches.Sixteenth;

            OSCADObject twoByfourHolder = new Box()
            {
                Size = new Vector3(innerWidth + wallThickness * 2, innerHeight + wallThickness * 2, innerDepth + wallThickness),
                Center = true,
                WallThickness = wallThickness
            };

            var whole = twoByfourHolder.Rotate(90, 0, 90) + 
                twoByfourHolder.Clone().Rotate(90, 0, 90).Rotate(0, 90, 0).Translate(innerHeight/2 - wallThickness*2 + Inches.Sixteenth*.5, 0, -innerHeight/2 - innerDepth/2 - wallThickness + Inches.Sixteenth);
            whole = whole.Rotate(0, -90, 0);
            whole.ToFile("twobyFourSideBracket");
        }

        public static void makeCenterTwoByFourBracket()
        {
            double wallThickness = Inches.Eigth;

            double innerWidth = Inches.One * 4 + Inches.Sixteenth;
            double innerHeight = Inches.One * 2 + Inches.Sixteenth;
            double innerDepth = Inches.Half + Inches.Sixteenth;

            OSCADObject twoByfourHolder = new Box()
            {
                Size = new Vector3(innerWidth + wallThickness * 2, innerHeight + wallThickness * 2, innerDepth + wallThickness),
                Center = true,
                WallThickness = wallThickness
            };

            var whole = twoByfourHolder.Rotate(180, 0, 0).Rotate(0, 0, 90);
            var left = twoByfourHolder.Clone().Rotate(0, 0, 90).Rotate(0, 90, 0)
                .Translate(innerDepth - wallThickness*2, 0, whole.Bounds().ZMax + innerHeight / 2);
            var right = left.Mirror(1, 0, 0);
            whole = whole + left + right;
            //    .Rotate(90, 0, 90) +
            //    twoByfourHolder.Clone().Rotate(90, 0, 90).Rotate(0, 90, 0).Translate(innerHeight / 2 - wallThickness * 2 + Inches.Sixteenth * .5, 0, -innerHeight / 2 - innerDepth / 2 - wallThickness + Inches.Sixteenth);
            //whole = whole.Rotate(0, -90, 0);
            whole.ToFile("twobyFourCenterBracket");
        }

        static void Main(string[] args)
        {
            //makeCenterTwoByFourBracket();
            //makeSideTwoByFourBracket();
            makeDiceHolder();
            //makeACBrackets();
            makeGreatSword();
            makepaddleHandle();
            makePaddle();
            //makeDisk();
            //makeHolder();
            //makeLoop();
            //makeBadge();
            //makeCardClip();
            //makeLaserStand();
            //makeStandCap();

            var tubeMe = new Tube()
            {
                Diameter1 = Inches.One,
                Diameter2 = Inches.Half,
                Height = Inches.One,
                Bottom = false,
                Resolution = 100,
                //Center = true,
                WallThickness = Inches.Sixteenth
            };
            tubeMe.ToFile("tubeTest");

            //var boxMe = new Box() {
            //    Size = new Vector3(10, 10, 10),
            //    WallThickness = .5
            //};
            //boxMe.ToFile("boxtest");

            //var img = ImportedImage.FromFile("twixCrop_small.png", new ImageImportOptions()
            //{
            //    HeightMapping = ImageImportOptions.HeightMappingMode.Vertical,
            //    UseGrayScale = false,
            //    SimplificationAmount = 25
            //});
            //img.ToFile("twixCrop");

            //var img = ImportedImage.FromFile("vermont-nc.png").Scale(1, 1, Inches.Quarter + Inches.Eigth);
            //var imgPos = img.Position();z
            //var _base = new Cylinder(img.Bounds().Width + Inches.Quarter, Inches.Quarter) { Resolution = 100 };

            //var rim = _base.Clone().Scale(1, 1, 1.25) - _base.Clone().Scale(.9, .9, 3.5).Translate(0, 0, -Inches.Eigth);
            //var coaster = img + _base.Translate(imgPos.X, imgPos.Y, 0) + rim.Translate(imgPos.X, imgPos.Y, Inches.Quarter); ;


            //coaster.ToFile("seaImg").Open();

            //makePeg();

            //var diam = new Variable("mainColumn", Inches.Half);
            //var height = new Variable("overallHeight", Inches.Quarter);
            //Variables.Global.Add(diam);
            //Variables.Global.Add(height);

            //var cyl = new Cylinder(diam, diam, height);


            //var pos = cyl.Position();
            //var cyl1 = new Cylinder(1, 100, true).Translate(pos);
            //var cyl2 = new Cylinder(1, 100, true).Rotate(0, 90, 0).Translate(pos);
            //var cyl3 = new Cylinder(1, 100, true).Rotate(90, 0, 0).Translate(pos);
            //var axisHelper = cyl1.Union(cyl2, cyl3).Color("Red");

            ////var topCorner = new Sphere().Translate(obj.Bounds().TopRight);
            ////var botCorner = new Sphere().Translate(obj.Bounds().BottomLeft);

            //(cyl + axisHelper).ToFile("test.scad").Open();

            //Console.ReadKey();
        }
    }
}
