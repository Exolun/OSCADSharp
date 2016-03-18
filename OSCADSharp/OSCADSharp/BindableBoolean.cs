using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    internal class BindableBoolean : IBindable
    {
        internal string InnerValue
        {
            get;
            set;
        }

        private Bindings bindings = new Bindings(new Dictionary<string, string>()
        {
            { "innervalue", "innervalue" }
        });

        /// <summary>
        /// Creates a bindable boolean, which is more or less a 
        /// proxy for bindings on boolean fields
        /// </summary>
        /// <param name="propertyName">Name of the property in the containing class for binding.
        /// This will be used as a synonym</param>
        internal BindableBoolean(string propertyName)
        {
            this.boundProperty = propertyName;
            this.bindings.Synonym("innervalue", propertyName);
        }

        private string boundProperty = null;

        internal bool IsBound { get; set; } = false;
        public void Bind(string property, Variable variable)
        {
            this.IsBound = true;
            var stringifiedVar = new Variable(variable.Text, variable.Value.ToString().ToLower());
            this.bindings.Add<BindableBoolean>(this, property, stringifiedVar);
        }

        public override string ToString()
        {
            return this.bindings.Get(this.boundProperty).BoundVariable.Text;
        }

        public BindableBoolean Clone()
        {
            var clone = new BindableBoolean(this.boundProperty);
            clone.bindings = this.bindings;

            return clone;
        }
    }
}
