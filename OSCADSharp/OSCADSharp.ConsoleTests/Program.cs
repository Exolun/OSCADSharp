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
            var obj = new Cube(5, 10, 20) + new Sphere(10).Translate(-10, 5, 0);

            //var pos = obj.Position();
            //var cyl1 = new Cylinder(1, 100, true).Translate(pos);
            //var cyl2 = new Cylinder(1, 100, true).Rotate(0, 90, 0).Translate(pos);
            //var cyl3 = new Cylinder(1, 100, true).Rotate(90, 0, 0).Translate(pos);
            //var axisHelper = cyl1.Union(cyl2, cyl3).Color("Red");

            var topCorner = new Sphere().Translate(obj.Bounds().TopRight);
            var botCorner = new Sphere().Translate(obj.Bounds().BottomLeft);

            string script = (obj + topCorner + botCorner).ToString();

            File.WriteAllLines("test.scad", new string[] { script.ToString() });
            //Console.ReadKey();
        }
    }
}
