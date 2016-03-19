﻿using OSCADSharp.DataBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Utility
{
    /// <summary>
    /// Extends the capabilities of StringBuilder with domain-specific behavior
    /// aimed at constructing OpenSCAD commands
    /// </summary>
    internal class StatementBuilder
    {
        private StringBuilder SB { get; set; } = new StringBuilder();
        private Bindings bindings = null;
        
        internal StatementBuilder(Bindings bindings)
        {
            this.bindings = bindings;
        }

        /// <summary>
        /// Special append method for conditionally adding value-pairs
        /// </summary>
        /// <param name="name">The Name of the value-pair</param>
        /// <param name="value">The value - if null this method does nothing</param>
        /// <param name="prefixWithComma">(optional) Flag indicating whether a comma should be added before the value-pair</param>
        internal void AppendValuePairIfExists(string name, object value, bool prefixWithComma = false)
        {
            bool useBinding = this.shouldUseBinding(name);

            if (!String.IsNullOrEmpty(value?.ToString()))
            {
                if (prefixWithComma)
                {
                    SB.Append(", ");
                }

                SB.Append(name);
                SB.Append(" = ");

                if (useBinding)
                {
                    SB.Append(this.bindings.Get(name).BoundVariable.Text);
                }
                else
                {
                    SB.Append(value);
                }
            }
        }

        private bool shouldUseBinding(string name)
        {
            return this.bindings != null && this.bindings.Contains(name);
        }

        /// <summary>
        /// Pass-through for StringBuilder.Append
        /// </summary>
        /// <param name="text"></param>
        internal void Append(string text)
        {
            SB.Append(text);
        }

        /// <summary>
        /// Gets this builder's full string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return SB.ToString();
        }
    }
}