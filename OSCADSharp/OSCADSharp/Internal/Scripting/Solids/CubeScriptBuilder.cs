using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    internal class CubeScriptBuilder
    {
        private CubeBindings bindings;
        private Cube cube;

        public CubeScriptBuilder(CubeBindings bindings, Cube cube)
        {
            this.bindings = bindings;
            this.cube = cube;
        }

        internal string GetScript()
        {
            return String.Format("cube(size = {0}, center = {1}); {2}",
                this.bindings.SizeBinding.ToString(),
                this.bindings.CenterBinding.IsBound ? this.bindings.CenterBinding.ToString() :
                    this.cube.Center.ToString().ToLower(),
                Environment.NewLine);
        }
    }
}
