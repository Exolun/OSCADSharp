using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.DataBinding;
using OSCADSharp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.UnitTests.Scripting
{
    [TestClass]
    public class VariableTests
    {
        [TestMethod]
        public void Variables_CreatingVariableWithTrueForGlobalAddsGlobals()
        {
            var myVariable = new Variable("overallWidth", Inches.ToMillimeters(3.5), true);

            Assert.AreEqual(Variables.Global.Get("overallWidth"), myVariable);
        }

        [TestMethod]
        public void Variables_ComputingAVariableValueResultsInACompoundVariable()
        {
            var compound = new Variable("x", 5) / 12;

            string type = compound.GetType().ToString();
            Assert.AreEqual("OSCADSharp.DataBinding.CompoundVariable", type);
        }
    }
}
