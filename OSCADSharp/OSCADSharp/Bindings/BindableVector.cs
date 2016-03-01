using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.Scripting;

namespace OSCADSharp.Bindings
{
    internal class BindableVector : Vector3, IBindable
    {
        private Bindings bindings = new Bindings(new Dictionary<string, string>()
        {
            { "x", "x" },
            { "y", "y" },
            { "z", "z" }
        });

        public void Bind(string property, Variable variable)
        {
            this.bindings.Add<BindableVector>(this, property, variable);
        }

        public override string ToString()
        {
            string x = this.bindings.Contains("x") ? this.bindings.Get("x").BoundVariable.Name : this.X.ToString();
            string y = this.bindings.Contains("y") ? this.bindings.Get("y").BoundVariable.Name : this.Y.ToString();
            string z = this.bindings.Contains("z") ? this.bindings.Get("z").BoundVariable.Name : this.Z.ToString();

            return String.Format("[{0}, {1}, {2}]", x, y, z);
        }
    }
}
