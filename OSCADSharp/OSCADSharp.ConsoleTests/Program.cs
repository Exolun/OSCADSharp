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
                .Mirror(new Vector3(1, 0, 0))
                .Resize(new Vector3(10, 20, 10))
                .Rotate(new Vector3(90, 90, 0))
                .Scale(new Vector3(1, 1, 2))
                .Translate(new Vector3(10, 5, 2));

            Console.WriteLine(cube.ToString());
            Console.ReadKey();
        }
    }
}
