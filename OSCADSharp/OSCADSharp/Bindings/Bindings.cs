using OSCADSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Bindings
{
    internal class Bindings
    {
        #region Fields
        private Dictionary<string, Binding> bindings = new Dictionary<string, Binding>();
        private Dictionary<string, string> propertyNametoOpenSCADFieldMappings = new Dictionary<string, string>();
        #endregion
        
        #region Constructors
        public Bindings(Dictionary<string, string> mappings)
        {
            this.propertyNametoOpenSCADFieldMappings = mappings;
        }
        #endregion

        #region Private Methods
        private void SetProperty<T>(T instance, string property, Variable variable)
        {
            PropertyInfo[] properties;
            properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = properties.Length - 1; i >= 0; i--)
            {
                var prop = properties[i];
                if (prop.Name.ToLower() == property.ToLower())
                {
                    prop.SetValue(instance, variable.Value);
                }
            }
        }

        /// <summary>
        /// Returns true if this set of bindings can map the specified property to an OpenSCAD field
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private bool HasMapping(string propertyName)
        {
            return this.propertyNametoOpenSCADFieldMappings.ContainsKey(propertyName.ToLower());
        }

        /// <summary>
        /// Returns the corresponding OpenSCAD output field name for
        /// a given property name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private string PropertyToOpenSCADField(string propertyName)
        {
            return this.propertyNametoOpenSCADFieldMappings[propertyName.ToLower()];
        }

        private void Add(Binding binding)
        {
            bindings[binding.OpenSCADFieldName] = binding;
        }
        #endregion

        #region Internal API
        internal void Add<T>(T instance, string propertyName, Variable variable)
        {
            if (!this.HasMapping(propertyName))
            {
                throw new KeyNotFoundException(String.Format("No bindable property matching the name {0} was found"));
            }

            //Assign mapping r -> radius -> variable
            var binding = new Binding()
            {
                OpenSCADFieldName = this.PropertyToOpenSCADField(propertyName),
                BoundVariable = variable
            };

            //Set value of property to variable value
            this.SetProperty<T>(instance, propertyName, variable);
            this.Add(binding);
        }

        internal bool Contains(string propertyName)
        {
            return bindings.ContainsKey(propertyName);
        }

        internal Binding Get(string propertyName)
        {
            return bindings[propertyName];
        }        
        #endregion
    }
}
