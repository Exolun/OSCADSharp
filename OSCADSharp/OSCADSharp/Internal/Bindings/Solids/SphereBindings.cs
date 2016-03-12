using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    internal class SphereBindings : ICloneable<SphereBindings>, IBindings
    {
        private Bindings bindings = new Bindings(new Dictionary<string, string>()
        {
            { "radius", "r" },
            { "minimumangle", "$fa" },
            { "minimumfragmentsize", "$fs" },
            { "resolution", "$fn" },
            { "diameter", "d" }
        });        

        public SphereBindings Clone()
        {
            return new SphereBindings() {
                bindings = bindings.Clone()
            };
        }

        public bool Contains(string openScadFieldName)
        {
            return this.bindings.Contains(openScadFieldName);
        }

        public Binding Get(string propertyName)
        {
            return this.bindings.Get(propertyName);
        }

        public void Synonym(string propertyName, string alternateName)
        {
            this.bindings.Synonym(propertyName, alternateName);
        }

        public void Bind<T>(T obj, string property, Variable variable)
        {
            this.bindings.Add<T>(obj, property, variable);
        }
    }
}
