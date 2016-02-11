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
            OSCADObject cube = new Cube(new Vector3(15, 15, 15));

            cube = cube.Color("Red")
                .Mirror(1, 0, 0)
                .Resize(10, 20, 10)
                .Rotate(90, 90, 0)
                .Scale(1, 1, 2)
                .Translate(10, 5, 2);

            OSCADObject cylinder = new Cylinder(35.4, 50.8).Translate(10, 5, 2);

            var combined = cube.Intersection(cylinder).Color("Blue");
            combined = cube.Clone().Mirror(0, 0, 1).Union(combined);

            var text = new Text3D("Hello").Translate(-30, 0, 0);

            combined = text.Union(combined);

            string script = text.ToString();

            File.WriteAllLines("test.scad", new string[] { script.ToString() });            
        }
    }
}
