using OSCADSharp.Scripting;
using OSCADSharp.Solids;
using OSCADSharp.Transforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var diam = new Variable("mainColumn", Inches.Half);
            var height = new Variable("overallHeight", Inches.Quarter);
            Variables.Global.Add(diam);
            Variables.Global.Add(height);

            var cyl = new Cylinder(diam, diam, height);
            

            var pos = cyl.Position();
            var cyl1 = new Cylinder(1, 100, true).Translate(pos);
            var cyl2 = new Cylinder(1, 100, true).Rotate(0, 90, 0).Translate(pos);
            var cyl3 = new Cylinder(1, 100, true).Rotate(90, 0, 0).Translate(pos);
            var axisHelper = cyl1.Union(cyl2, cyl3).Color("Red");

            //var topCorner = new Sphere().Translate(obj.Bounds().TopRight);
            //var botCorner = new Sphere().Translate(obj.Bounds().BottomLeft);

            (cyl + axisHelper).ToFile("test.scad").Open();
            
            //Console.ReadKey();
        }
    }
}
