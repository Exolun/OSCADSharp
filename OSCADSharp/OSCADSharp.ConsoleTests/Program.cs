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

            for (int i = 0; i < 1000; i++)
            {
                cube = cube.Translate(1, 0, 0);
            }

            string script = cube.ToString();

            //File.WriteAllLines("test.scad", new string[] { script.ToString() });
            Console.ReadKey();
        }
    }
}
