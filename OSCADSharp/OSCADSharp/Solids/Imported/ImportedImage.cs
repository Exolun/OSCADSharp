using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.DataBinding;
using OSCADSharp.Spatial;
using System.Drawing;
using OSCADSharp.Solids.Imported.Images;

namespace OSCADSharp.Solids.Imported
{
    /// <summary>
    /// An image file imported and processed into a 3D object
    /// </summary>
    public class ImportedImage : OSCADObject
    {
        #region Internal Properties
        internal OSCADObject m_Object { get; set; }
        internal Bounds m_Bounds { get; set; }
        #endregion

        #region Constructors / Initialization
        /// <summary>
        /// Creates an imported image from the specified file
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static ImportedImage FromFile(string imagePath)
        {
            IImageProcessor processor;
            //if(mode == ImageImportMode.Cubist)
            //{
            processor = new CubistImageProcessor(imagePath);
            //}
            //else
            //{
            //    processor = new PolygonalImageProcessor(imagePath);
            //}

            var obj = processor.ProcessImage();

            var img = new ImportedImage()
            {
                m_Object = obj,
                m_Bounds = processor.ImageBounds
            };

            return img;

        }
        #endregion

        #region OSCADObject Overrides
        /// <summary>
        /// Imported images have no bindable properties
        /// </summary>
        /// <param name="property"></param>
        /// <param name="variable"></param>
        public override void Bind(string property, Variable variable)
        {
            throw new NotSupportedException("Imported images have no bindable properties");
        }

        /// <summary>
        /// Returns the approximate boundaries of this OpenSCAD object
        /// </summary>
        /// <returns></returns>
        public override Bounds Bounds()
        {
            return m_Bounds;
        }

        /// <summary>
        /// Gets a copy of this object that is a new instance
        /// </summary>
        /// <returns></returns>
        public override OSCADObject Clone()
        {
            return this.m_Object.Clone();
        }

        /// <summary>
        /// Gets the position of this object's center (origin) in
        /// world space
        /// </summary>
        /// <returns></returns>
        public override Vector3 Position()
        {
            return Vector3.Average(this.m_Bounds.BottomLeft, this.m_Bounds.TopRight);
        }

        /// <summary>
        /// Converts this object to an OpenSCAD script
        /// </summary>
        /// <returns>Script for this object</returns>
        public override string ToString()
        {
            return this.m_Object.ToString();
        }
        #endregion
    }
}
