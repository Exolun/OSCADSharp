using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    internal class CubeBindings : ICloneable<CubeBindings>, IBindings
    {
        private Bindings bindings = new Bindings();
        private static readonly Dictionary<string, string> sizeSynonyms = new Dictionary<string, string>()
        {
            {"size.x", "x" },
            {"size.y", "y" },
            {"size.z", "z" },
            {"length", "x" },
            {"width", "y" },
            {"height", "z" }
        };

        public BindableVector SizeBinding { get; set; } = new BindableVector(new Vector3(), sizeSynonyms);
        public BindableBoolean CenterBinding { get; set; } = new BindableBoolean("center");

        public void Bind<T>(T obj, string property, Variable variable)
        {
            string prop = property.ToLower();
            Cube cube = obj as Cube;
            if (sizeSynonyms.ContainsKey(prop))
            {
                this.SizeBinding.X = cube.Size.X;
                this.SizeBinding.Y = cube.Size.Y;
                this.SizeBinding.Z = cube.Size.Z;

                this.SizeBinding.Bind(prop, variable);
                cube.Size = new Vector3(SizeBinding.X, SizeBinding.Y, SizeBinding.Z);
            }
            else if (prop == "center")
            {
                this.CenterBinding.Bind(prop, variable);
                cube.Center = Convert.ToBoolean(variable.Value);                
            }
            else
            {
                throw new KeyNotFoundException(String.Format("No bindable property matching the name {0} was found", prop));
            }
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

        public CubeBindings Clone()
        {
            return new CubeBindings()
            {
                bindings = bindings.Clone(),
                CenterBinding = this.CenterBinding,
                SizeBinding = this.SizeBinding
            };
        }
    }
}
