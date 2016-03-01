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
        private Dictionary<string, string> synonyms = new Dictionary<string, string>();
        #endregion

        #region Constructors
        public Bindings()
        {
            this.propertyNametoOpenSCADFieldMappings = new Dictionary<string, string>();
        }

        public Bindings(Dictionary<string, string> mappings)
        {
            this.propertyNametoOpenSCADFieldMappings = mappings;
        }
        #endregion

        #region Private Methods
        private void setProperty<T>(T instance, string property, Variable variable)
        {
            PropertyInfo[] properties;
            properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = properties.Length - 1; i >= 0; i--)
            {
                var prop = properties[i];
                string lProperty = property.ToLower();
                if (this.hasMatchingSynonym(lProperty))
                    lProperty = this.synonyms[lProperty];

                if (prop.Name.ToLower() == lProperty)
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
        private bool hasMapping(string propertyName)
        {
            return this.propertyNametoOpenSCADFieldMappings.ContainsKey(propertyName.ToLower())
                || this.hasMatchingSynonym(propertyName.ToLower());
        }

        /// <summary>
        /// Returns the corresponding OpenSCAD output field name for
        /// a given property name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private string propertyToOpenSCADField(string propertyName)
        {
            string lpropertyName = propertyName.ToLower();
            if (this.hasMatchingSynonym(lpropertyName))
            {
                return this.synonymToOpenScadField(lpropertyName);
            }

            return this.propertyNametoOpenSCADFieldMappings[lpropertyName];
        }

        private void add(Binding binding)
        {
            bindings[binding.OpenSCADFieldName] = binding;
        }


        private bool hasMatchingSynonym(string synonymName)
        {
            return this.synonyms.ContainsKey(synonymName);
        }

        private string synonymToOpenScadField(string synonymName)
        {
            return this.propertyToOpenSCADField(this.synonyms[synonymName]);
        }
        #endregion

        #region Internal API
        internal void Add<T>(T instance, string propertyName, Variable variable)
        {
            if (!this.hasMapping(propertyName))
            {
                throw new KeyNotFoundException(String.Format("No bindable property matching the name {0} was found"));
            }

            //Assign mapping r -> radius -> variable
            var binding = new Binding()
            {
                OpenSCADFieldName = this.propertyToOpenSCADField(propertyName),
                BoundVariable = variable
            };

            //Set value of property to variable value
            this.setProperty<T>(instance, propertyName, variable);
            this.add(binding);
        }

        internal bool Contains(string openScadFieldName)
        {
            return bindings.ContainsKey(openScadFieldName);
        }

        internal Binding Get(string propertyName)
        {
            return bindings[propertyName];
        }      
        
        internal void Synonym(string propertyName, string alternateName)
        {
            this.synonyms[alternateName] = propertyName;
        }  
        #endregion
    }
}
