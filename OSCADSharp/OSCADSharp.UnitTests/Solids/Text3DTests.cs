using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSCADSharp.Scripting;
using OSCADSharp.Solids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.UnitTests
{
    [TestClass]
    public class Text3DTests
    {
        [TestMethod]
        public void Text_PositionIsCentered()
        {
            var text = new Text3D("Bom chicka bow wow");

            Assert.AreEqual(new Vector3(), text.Position());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Text_BoundsNotSupported()
        {
            var obj = new Text3D("BBBB", 16).Bounds();
        }

        [TestMethod]
        public void Text_TextAndSizeBindingAffectsOutput()
        {
            var text = new Variable("txt", "yo");
            var size = new Variable("txtSize", 34);

            var obj = new Text3D();
            obj.Bind("Text", text);
            obj.Bind("Size", size);

            string script = obj.ToString();
            
            Assert.AreEqual(text.Value, obj.Text);
            Assert.IsTrue(size.Value.ToString() == obj.Size.ToString());

            Assert.IsTrue(script.Contains(String.Format("text(\"{0}\"", text.Name)));
            Assert.IsTrue(script.Contains(String.Format("size = {0}", size.Name)));
        }

        [TestMethod]
        public void Text_FontSpacingBindingAffectsOutput()
        {
            var font = new Variable("font", "wingdings");
            var spacing = new Variable("spacing", 34);

            var obj = new Text3D();
            obj.Bind("font", font);
            obj.Bind("SpacinG", spacing);

            string script = obj.ToString();

            Assert.AreEqual(font.Value, obj.Font);
            Assert.IsTrue(spacing.Value.ToString() == obj.Spacing.ToString());

            Assert.IsTrue(script.Contains(String.Format("font = {0}", font.Name)));
            Assert.IsTrue(script.Contains(String.Format("spacing = {0}", spacing.Name)));
        }

        [TestMethod]
        public void Text_TextDirectionLanguageBindingAffectsOutput()
        {
            var direction = new Variable("direction", "ltr");
            var language = new Variable("language", "en");

            var obj = new Text3D();
            obj.Bind("textdirection", direction);
            obj.Bind("language", language);

            string script = obj.ToString();

            Assert.AreEqual(direction.Value, obj.TextDirection);
            Assert.AreEqual(language.Value, obj.Language);

            Assert.IsTrue(script.Contains(String.Format("direction = {0}", direction.Name)));
            Assert.IsTrue(script.Contains(String.Format("language = {0}", language.Name)));
        }
    }
}
