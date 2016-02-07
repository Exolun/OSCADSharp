using OSCADSharp.Solids;
using OSCADSharp.Transforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            OSCADObject cube = new Cube()
            {
                Size = new Vector3(5, 5, 5),
                Center = false
            };

            cube = cube.Color("Red")
                .Mirror(1, 0, 0)
                .Resize(10, 20, 10)
                .Rotate(90, 90, 0)
                .Scale(1, 1, 2)
                .Translate(10, 5, 2);

            Console.WriteLine(cube.ToString());
            Console.ReadKey();
        }
    }
}
