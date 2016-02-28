using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// Responsible for creating identifiers for objects
    /// </summary>
    internal static class Ids
    {
        private static uint globalId = 0;
        private static object idLockObject = new object();

        /// <summary>
        /// Gets a unique auto-incrementing integer id
        /// </summary>
        /// <returns></returns>
        internal static uint Get()
        {
            lock (idLockObject)
            {
                globalId++;
                return globalId;
            }
        }
    }
}
