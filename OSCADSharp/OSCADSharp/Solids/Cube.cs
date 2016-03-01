using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.Spatial;
using OSCADSharp.Bindings;
using OSCADSharp.Scripting;

namespace OSCADSharp.Solids
{
    /// <summary>
    /// A Cube geometry
    /// </summary>
    public class Cube : OSCADObject, IBindable
    {
        #region Attributes
        private Vector3 size = new BindableVector(1, 1, 1, sizeSynonyms);
        private bool center = false;
        private BindableBoolean centerBinding = new BindableBoolean("center");

        /// <summary>
        /// The Size of the cube in terms of X/Y/Z units
        /// </summary>
        public Vector3 Size
        {
            get { return this.size; }
            set { this.size = new BindableVector(value, sizeSynonyms); }
        }

        /// <summary>
        /// If True, the center of the cube will be at 0, 0, 0
        /// 
        /// If False (default) one corner will be centered at 0,0, 0, with the cube extending into the positive octant (positive X/Y/Z)
        /// </summary>
        public bool Center
        {
            get { return this.center; }
            set
            {
                this.center = value;
                this.centerBinding.InnerValue = this.center.ToString().ToLower();
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new cube object with the default initialization values
        /// </summary>
        public Cube()
        {
        }

        /// <summary>
        /// Creates a new Cube object
        /// </summary>
        /// <param name="size">The Size of the cube in terms of X/Y/Z dimensions</param>
        /// <param name="center">Indicates whether the cube should be centered on the origin</param>
        public Cube(Vector3 size = null, bool center = false)
        {
            this.Size = new BindableVector(size, sizeSynonyms) ?? new BindableVector(1, 1, 1, sizeSynonyms);
            this.Center = center;
        }

        /// <summary>
        /// Creates a new Cube object with Length/Width/Height
        /// </summary>
        /// <param name="length">Size on the X axis</param>
        /// <param name="width">Size on the Y axis</param>
        /// <param name="height">Size on the Z axis</param>
        /// <param name="center">Indicates whether the cube should be centered on the origin</param>
        public Cube(double length, double width, double height, bool center = false)
        {
            this.Size.X = length;
            this.Size.Y = width;
            this.Size.Z = height;

            this.Center = center;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Converts this object to an OpenSCAD script
        /// </summary>
        /// <returns>Script for this object</returns>
        public override string ToString()
        {
            return String.Format("cube(size = {0}, center = {1}); {2}",
                this.Size.ToString(), 
                this.centerBinding.IsBound ? this.centerBinding.ToString() : this.center.ToString().ToLower(), 
                Environment.NewLine); ;
        }

        /// <summary>
        /// Gets a copy of this object that is a new instance
        /// </summary>
        /// <returns></returns>
        public override OSCADObject Clone()
        {
            return new Cube()
            {
                Name = this.Name,
                Size = this.Size,
                Center = this.Center
            };
        }

        /// <summary>
        /// Gets the position of this object's center (origin) in
        /// world space
        /// </summary>
        /// <returns></returns>
        public override Vector3 Position()
        {
            Vector3 position;
            if(this.Center == false)
            {
                position = new Vector3(this.Size.X / 2, this.Size.Y / 2, this.Size.Z / 2);
            }
            else
            {
                position = new Vector3();
            }

            return position;
        }

        /// <summary>
        /// Returns the approximate boundaries of this OpenSCAD object
        /// </summary>
        /// <returns></returns>
        public override Bounds Bounds()
        {
            if(Center == false)
            {
                return new Bounds(new Vector3(), new Vector3(this.Size.X, this.Size.Y, this.Size.Z));
            }
            else
            {
                return new Bounds(new Vector3(-this.Size.X / 2, -this.Size.Y / 2, -this.Size.Z / 2), 
                                  new Vector3(this.Size.X / 2, this.Size.Y / 2, this.Size.Z / 2));
            }
        }

        private Bindings.Bindings bindings = new Bindings.Bindings();
        private static readonly Dictionary<string, string> sizeSynonyms = new Dictionary<string, string>()
        {
            {"size.x", "x" },
            {"size.y", "y" },
            {"size.z", "z" },
            {"length", "x" },
            {"width", "y" },
            {"height", "z" }
        };

        /// <summary>
        /// Binds a a variable to a property on this object
        /// </summary>
        /// <param name="property">A string specifying the property such as "Diameter" or "Radius"</param>
        /// <param name="variable">The variable to bind the to.  This variable will appear in script output in lieu of the 
        /// literal value of the property</param>
        public void Bind(string property, Variable variable)
        {
            if (sizeSynonyms.ContainsKey(property.ToLower()))
            {
                BindableVector vec;
                if (this.size is BindableVector)
                    vec = this.Size as BindableVector;
                else
                    vec = new BindableVector(this.size);

                vec.Bind(property, variable);
            }

            if(property.ToLower() == "center")
            {
                this.centerBinding.Bind(property, variable);
                this.center = Convert.ToBoolean(variable.Value);
            }
        }
        #endregion
    }
}
