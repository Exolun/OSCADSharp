using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    internal interface IBindings
    {
        void Bind<T>(T obj, string property, Variable variable);

        bool Contains(string openScadFieldName);

        Binding Get(string propertyName);

        void Synonym(string propertyName, string alternateName);        
    }
}
