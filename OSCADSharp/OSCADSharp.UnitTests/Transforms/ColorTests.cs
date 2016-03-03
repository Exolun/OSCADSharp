﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Scripting;
using OSCADSharp.Solids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.UnitTests.Transforms
{
    [TestClass]
    public class ColorTests
    {
        [TestMethod]
        public void Color_BoundFieldsAppearInOutput()
        {
            Variable colorVar = new Variable("myFavoriteColor", "blue");
            Variable cubeOpacity = new Variable("cubeOpacity", .6);

            var obj = new Cube().Color("Red", .5);
            obj.Bind("color", colorVar);
            obj.Bind("opacity", cubeOpacity);

            string script = obj.ToString();

            Assert.IsTrue(script.Contains("myFavoriteColor"));
            Assert.IsTrue(script.Contains("cubeOpacity"));
        }
    }
}