using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp.Solids
{
    /// <summary>
    /// A Cube geometry
    /// </summary>
    public class Cube : OSCADObject
    {
        #region Attributes
        /// <summary>
        /// The Size of the cube in terms of X/Y/Z units
        /// </summary>
        public Vector3 Size { get; set; } = new Vector3(1, 1, 1);

        /// <summary>
        /// If True, the center of the cube will be at 0, 0, 0
        /// 
        /// If False (default) one corner will be centered at 0,0, 0, with the cube extending into the positive octant (positive X/Y/Z)
        /// </summary>
        public bool Center { get; set; } = false;
        #endregion

        public override string ToString()
        {
            return String.Format("cube(size = [{0}, {1}, {2}], center = {3});", 
                this.Size.X.ToString(), this.Size.Y.ToString(), this.Size.Z.ToString(), this.Center.ToString().ToLower()); ;
        }

        public override OSCADObject Clone()
        {
            return new Cube()
            {
                Size = this.Size,
                Center = this.Center
            };
        }
    }
}
