using OSCADSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.Spatial;

namespace OSCADSharp.Solids
{
    /// <summary>
    /// Create text using fonts installed on the local system or provided as separate font file.
    /// </summary>
    public class Text3D : OSCADObject
    {
        #region Attributes
        /// <summary>
        /// Text to display
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The generated text will have approximately an ascent of the given value (height above the baseline). Default is 10.
        /// Note that specific fonts will vary somewhat and may not fill the size specified exactly, usually slightly smaller.
        /// </summary>
        public uint? Size { get; set; } = null;

        /// <summary>
        /// The name of the font that should be used. This is not the name of the font file, 
        /// but the logical font name (internally handled by the fontconfig library). This can also include a style parameter, see below. 
        /// A list of installed fonts and styles can be obtained using the font list dialog (Help -> Font List).
        /// </summary>
        public string Font { get; set; } = null;
        
        /// <summary>
        /// Factor to increase/decrease the character spacing. The default value of 1 will result in the normal spacing for the font, giving a value greater than 1 will cause the letters to be spaced further apart.
        /// </summary>
        public uint? Spacing { get; set; } = null;

        /// <summary>
        /// Direction of the text flow. Possible values are "ltr" (left-to-right), "rtl" (right-to-left), "ttb" (top-to-bottom) and "btt" (bottom-to-top). Default is "ltr".
        /// </summary>
        public string TextDirection { get; set; }

        /// <summary>
        /// The language of the text. Default is "en".
        /// </summary>
        public string Language { get; set; }

        #endregion
       
        #region Constructors
        /// <summary>
        /// Creates 3d text with the default parameters
        /// if the text is not specified, text will say "Text"
        /// </summary>
        public Text3D()
        {
            this.Text = "Text";
        }

        /// <summary>
        /// Creates 3d text with the specified text to create
        /// </summary>        
        /// <param name="text">Text to display</param>
        /// <param name="size">Font size for the text</param>
        public Text3D(string text, uint? size = null)
        {
            this.Text = text;
            this.Size = size;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Gets a copy of this object that is a new instance
        /// </summary>
        /// <returns></returns>
        public override OSCADObject Clone()
        {
            return new Text3D()
            {
                Text = this.Text,
                Size = this.Size,
                Font = this.Font,
                Spacing = this.Spacing,
                TextDirection = this.TextDirection,
                Language = this.Language                
            };
        }

        private void appendIfValueNotNullOrEmpty(string name, string value, StringBuilder sb)
        {
            if(!String.IsNullOrEmpty(value))
            {
                sb.Append(", ");
                sb.Append(name);
                sb.Append("=");
                sb.Append(value);
            }
        }
        
        /// <summary>
        /// Converts this object to an OpenSCAD script
        /// </summary>
        /// <returns>Script for this object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("text(");
            sb.Append("\"");
            sb.Append(this.Text);
            sb.Append("\"");

            appendIfValueNotNullOrEmpty("size", this.Size?.ToString(), sb);
            // Text is always centered in OSCADSharp to ensure correctness of
            // position interpolation
            appendIfValueNotNullOrEmpty("halign", "\"center\"", sb);
            appendIfValueNotNullOrEmpty("valign", "\"center\"", sb);

            appendIfValueNotNullOrEmpty("font", this.Font, sb);
            appendIfValueNotNullOrEmpty("spacing", this.Spacing?.ToString(), sb);
            appendIfValueNotNullOrEmpty("direction", this.TextDirection?.ToString(), sb);
            appendIfValueNotNullOrEmpty("language", this.Language?.ToString(), sb);            

            sb.Append(");");
            sb.Append(Environment.NewLine);
            
            var formatter = new SingleBlockFormatter(String.Format("linear_extrude(height = {0})", 1), sb.ToString());
            return formatter.ToString();
        }

        /// <summary>
        /// In reaction to the need for this value to be correct, halign and valign will always
        /// be "center" by default, since non-centered text would vary dramatically in position based upon
        /// the font of the text
        /// - MLS 2/15/2016
        /// </summary>
        /// <returns></returns>
        public override Vector3 Position()
        {
            return new Vector3();
        }

        /// <summary>
        /// Returns the approximate boundaries of this OpenSCAD object
        /// 
        /// Disclaimer:  The bounds for text are particularly inaccurate, since
        /// OSCADSharp doesn't have all font data necessary to calculate the
        /// boundaries for all the possible fonts that could be used
        /// </summary>
        /// <returns></returns>
        public override Bounds Bounds()
        {
            double fontSize = this.Size ?? 8;
            double xAmount = fontSize * this.Text.Length;
            
            return new Bounds(new Vector3(-xAmount/2, -fontSize/2, -.5), new Vector3(xAmount / 2, fontSize / 2, .5));
        }
        #endregion
    }
}
