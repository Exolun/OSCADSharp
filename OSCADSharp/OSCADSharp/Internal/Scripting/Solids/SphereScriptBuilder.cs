using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    internal class SphereScriptBuilder
    {
        private IBindings bindings;
        private Sphere sphere;

        public SphereScriptBuilder(IBindings bindings, Sphere sphere)
        {
            this.bindings = bindings;
            this.sphere = sphere;
        }

        internal string GetScript()
        {
            StatementBuilder sb = new StatementBuilder(this.bindings);
            sb.Append("sphere(");

            if (this.bindings.Contains("d"))
            {
                sb.AppendValuePairIfExists("d", this.sphere.Diameter);
            }
            else
            {
                sb.AppendValuePairIfExists("r", this.sphere.Radius);
            }

            sb.AppendValuePairIfExists("$fn", this.sphere.Resolution, true);
            sb.AppendValuePairIfExists("$fa", this.sphere.MinimumAngle, true);
            sb.AppendValuePairIfExists("$fs", this.sphere.MinimumFragmentSize, true);
            sb.Append(");");
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
    }
}
