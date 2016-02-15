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
            var cube = new Cube();
            var sphere = new Sphere().Translate(0, 0, 2);
            var hull = cube.Hull(sphere);

            string script = hull.ToString();

            File.WriteAllLines("test.scad", new string[] { script.ToString() });
            //Console.ReadKey();
        }
    }
}
