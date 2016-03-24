
using OSCADSharp.DataBinding;
using OSCADSharp.Solids;
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
            Variables.Global.Add("$fn", 30);

            OSCADObject flatInnerPortion = new Cylinder(Inches.Quarter, Inches.Eigth, true);
            OSCADObject shaft = new Cylinder(Inches.Eigth, Inches.Half, true);
            flatInnerPortion = flatInnerPortion.Translate(0, 0, shaft.Bounds().ZMax);

            OSCADObject pegShaft = new Cylinder(Inches.Quarter, Inches.Half - Inches.Eigth, true)
                .Translate(0, 0, -Inches.Eigth);
            OSCADObject bottomBall = new Sphere(Inches.Quarter* 1.1)
                .Translate(0, 0, pegShaft.Bounds().ZMin);

            var obj = flatInnerPortion + shaft + pegShaft + bottomBall;
            obj = obj.Rotate(0, 180, 0).Translate(0, 0, obj.Bounds().ZMax);

            obj.ToFile("peg");
        }

        static void Main(string[] args)
        {
            makePeg();

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
