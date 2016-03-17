using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// Contains definitions for external APIs used by OSCADSharp
    /// </summary>
    public static class Dependencies
    {
        /// <summary>
        /// Used to write scripts to file
        /// </summary>
        public static IFileWriter FileWriter { get; private set; } = new DefaultFileWriter();

        /// <summary>
        /// Sets the filewriter for OSCADSharp to use
        /// </summary>
        /// <param name="writer"></param>
        public static void SetFileWriter(IFileWriter writer)
        {
            FileWriter = writer;
        }


        /// <summary>
        /// Factory method to provide the class used to perform actions on output scripts
        /// </summary>
        public static Func<string, IFileInvoker> FileInvokerFactory { get; private set; } = (path) => { return new DefaultFileInvoker(path); };

        /// <summary>
        /// Sets the factory method OSCADSharp will use to get
        /// file invoker objects
        /// </summary>
        /// <param name="invokerFactoryMethod"></param>
        public static void SetFileInvokerFactory(Func<string, IFileInvoker> invokerFactoryMethod)
        {
            FileInvokerFactory = invokerFactoryMethod;
        }
    }
}
