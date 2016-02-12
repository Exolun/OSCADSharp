﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Solids;

namespace OSCADSharp.UnitTests
{
    [TestClass]
    public class SphereTests
    {
        private Sphere sphere;

        [TestInitialize]
        public void Setup()
        {
            this.sphere = new Sphere(10);
        }

        [TestMethod]
        public void Sphere_RadiusIsHalfofDiameter()
        {
            Assert.AreEqual(5, this.sphere.Radius);
        }

        [TestMethod]
        public void Sphere_ResolutionAffectsFNvalue_scriptWithFNof30RR()
        {
            this.sphere.Resolution = 30;

            string script = this.sphere.ToString();

            Assert.IsTrue(script.Contains("$fn = 30"));
        }

        [TestMethod]
        public void Sphere_FAandFSAreAffectedByAngleANdFragmentSize_scriptWithMatchingAngleAndFragmentSize()
        {
            this.sphere.MinimumAngle = 2;
            this.sphere.MinimumFragmentSize = 4;

            string script = this.sphere.ToString();

            Assert.IsTrue(script.Contains("$fa = 2"));
            Assert.IsTrue(script.Contains("$fs = 4"));
        }

        [TestMethod]
        public void Sphere_ParameterlessSphereHasMethodCallAndEndsWithSemicolon_scriptWithSphereMethodCall()
        {
            var basicSphere = new Sphere();

            string script = basicSphere.ToString();

            Assert.IsTrue(script.StartsWith("sphere("));
            Assert.IsTrue(script.EndsWith(");"));
        }
    }
}
