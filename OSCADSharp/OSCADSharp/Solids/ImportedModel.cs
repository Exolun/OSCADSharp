using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.DataBinding;
using OSCADSharp.Spatial;
using System.IO;

namespace OSCADSharp.Solids
{
    /// <summary>
    /// An imported 3D model, supported types are limited.    
    /// </summary>
    public class ImportedModel : OSCADObject
    {
        #region Fields
        private string filePath;
        private Bounds objectBounds;
        #endregion

        #region Constructors/Initialization
        /// <summary>
        /// Imports a model located at the specified file path.
        /// 
        /// If this is an ASCII-based STL model, the bounds and position will automatically be initialized. 
        /// </summary>
        /// <param name="filePath"></param>
        public ImportedModel(string filePath)
        {
            this.filePath = filePath;
            this.correctPath();
            //TODO: Attempt to compute bounds
        }

        /// <summary>
        /// Imports a model located at the specified file path.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="bounds"></param>
        public ImportedModel(string filePath, Bounds bounds)
        {
            this.filePath = filePath;
            this.correctPath();

            this.objectBounds = bounds;         
        }

        private void correctPath()
        {
            if (!File.Exists(this.filePath))
            {
                string currentDir = Directory.GetCurrentDirectory();

                if (!this.filePath.Contains(currentDir))
                {
                    this.filePath = currentDir + "\\" + this.filePath;
                }
            }            
        }
        #endregion

        #region OSCADObject Overrides
        /// <summary>
        /// Binds the specified property to a variable
        /// </summary>
        /// <param name="property"></param>
        /// <param name="variable"></param>
        public override void Bind(string property, Variable variable)
        {
            //No bindable properties on an imported model
        }

        /// <summary>
        /// Returns the approximate boundaries of this OpenSCAD object
        /// </summary>
        /// <returns></returns>
        public override Bounds Bounds()
        {
            if(this.objectBounds == null)
            {
                throw new InvalidOperationException("Could not compute bounds for an imported model where bounds were neither specified nor could be derived.");
            }

            return this.objectBounds;
        }

        /// <summary>
        /// Gets a copy of this object that is a new instance
        /// </summary>
        /// <returns></returns>
        public override OSCADObject Clone()
        {
            return new ImportedModel(this.filePath, this.objectBounds);
        }

        /// <summary>
        /// Gets the position of this object's center (origin) in
        /// world space
        /// </summary>
        /// <returns></returns>
        public override Vector3 Position()
        {
            if(this.objectBounds == null)
            {
                throw new InvalidOperationException("Cannot compute position on an imported model with no bounds");
            }

            return Vector3.Average(this.objectBounds.TopRight, this.objectBounds.BottomLeft);
        }

        /// <summary>
        /// Converts this object to an OpenSCAD script
        /// </summary>
        /// <returns>Script for this object</returns>
        public override string ToString()
        {
            return String.Format("import(\"{0}\");", this.filePath.Replace("\\", @"\\"));
        }
        #endregion
    }
}
