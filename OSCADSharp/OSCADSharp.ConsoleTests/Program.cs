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
            var cube = new Cube(null, true).Translate(10, 0, 5).Scale(1, 1, 5);
            var sphere = new Sphere().Mimic(cube).Translate(0, 0, 10);
            string script = cube.Hull(sphere, new Cylinder()).ToString();

            File.WriteAllLines("test.scad", new string[] { script.ToString() });
            Console.ReadKey();
        }
    }
}
