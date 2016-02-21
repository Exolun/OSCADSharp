using OSCADSharp.Booleans;
using OSCADSharp.Spatial;
using OSCADSharp.Transforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// Represents any Object or collection of objects that becomes am
    /// an OpenSCAD script when converted to a string.
    /// </summary>
    public abstract class OSCADObject
    {
        #region Fields
        private uint id = Ids.Get();

        /// <summary>
        /// The unique Id of the object
        /// these values auto-increment
        /// </summary>
        public uint Id { get { return this.id; } }
        #endregion

        #region Transforms
        /// <summary>
        /// Applies Color and/or Opacity to this object
        /// </summary>
        /// <param name="colorName">The name of the color to apply</param>
        /// <param name="opacity">The opacity from 0.0 to 1.0</param>
        /// <returns>A colorized object</returns>
        public OSCADObject Color(string colorName, double opacity = 1.0)
        {
            return new ColoredObject(this, colorName, opacity);
        }        

        /// <summary>
        /// Mirrors the object about a plane, as specified by the normal
        /// </summary>
        /// <param name="normal">The normal vector of the plane intersecting the origin of the object,
        /// through which to mirror it.</param>
        /// <returns>A mirrored object</returns>
        public OSCADObject Mirror(Vector3 normal)
        {
            return new MirroredObject(this, normal);
        }

        /// <summary>
        /// Mirrors the object about a plane, as specified by the normal
        /// described by the x/y/z components provided
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>A mirrored object</returns>
        public OSCADObject Mirror(double x, double y, double z)
        {
            return this.Mirror(new Vector3(x, y, z));
        }

        /// <summary>
        /// Resizes to a specified set of X/Y/Z dimensions
        /// </summary>
        /// <param name="newsize">The X/Y/Z dimensions</param>
        /// <returns>A resized object</returns>
        public OSCADObject Resize(Vector3 newsize)
        {
            return new ResizedObject(this, newsize);
        }

        /// <summary>
        /// Resizes to a specified set of X/Y/Z dimensions
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>A resized object</returns>
        public OSCADObject Resize(double x, double y, double z)
        {
            return this.Resize(new Vector3(x, y, z));
        }

        /// <summary>
        /// Rotates about a specified X/Y/Z euler angle
        /// </summary>
        /// <param name="angle">The angle(s) to rotate</param>
        /// <returns>A rotated object</returns>
        public OSCADObject Rotate(Vector3 angle)
        {
            return new RotatedObject(this, angle);
        }

        /// <summary>
        /// Rotates about a specified X/Y/Z euler angle
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>A rotated object</returns>
        public OSCADObject Rotate(double x, double y, double z)
        {
            return this.Rotate(new Vector3(x, y, z));
        }

        /// <summary>
        /// Rescales an object by an X/Y/Z scale factor
        /// </summary>
        /// <param name="scale">The scale to apply. For example 1, 2, 1 would yield 2x scale on the Y axis</param>
        /// <returns>A scaled object</returns>
        public OSCADObject Scale(Vector3 scale)
        {
            return new ScaledObject(this, scale);
        }

        /// <summary>
        /// Rescales an object by an X/Y/Z scale factor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>A scaled object</returns>
        public OSCADObject Scale(double x, double y, double z)
        {
            return this.Scale(new Vector3(x, y, z));
        }

        /// <summary>
        /// Translates an object by the specified amount
        /// </summary>
        /// <param name="translation">The vector upon which to translate (move object(s))</param>
        /// <returns>A translated object</returns>
        public OSCADObject Translate(Vector3 translation)
        {
            return new TranslatedObject(this, translation);
        }

        /// <summary>
        /// Translates an object by the specified amount
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>A translated object</returns>
        public OSCADObject Translate(double x, double y, double z)
        {
            return this.Translate(new Vector3(x, y, z));
        }

        /// <summary>
        /// Creates a minkowski sum of child nodes (including this object)
        /// </summary>
        /// <param name="objects">Nodes to sum with</param>
        /// <returns>A minkowski sum</returns>
        public OSCADObject Minkowski(params OSCADObject[] objects)
        {
            return doBlockStatement("Minkowski", objects, (children) => { return new MinkowskiedObject(children); });            
        }

        /// <summary>
        /// Creates a conved hull from child nodes (including this object)
        /// </summary>
        /// <param name="objects">Nodes to hull</param>
        /// <returns>Hull of nodes</returns>
        public OSCADObject Hull(params OSCADObject[] objects)
        {
            return doBlockStatement("Hull", objects, (children) => { return new HulledObject(children); });            
        }
        #endregion

        #region Boolean Operations
        /// <summary>
        /// Creates a union of all its child nodes. This is the sum of all children (logical or).
        /// May be used with either 2D or 3D objects, but don't mix them.
        /// </summary>
        /// <param name="objects">child nodes</param>
        /// <returns></returns>
        public OSCADObject Union(params OSCADObject[] objects)
        {
            return doBlockStatement("Union", objects, (children) => { return new Union(children); });
        }

        /// <summary>
        /// Subtracts the 2nd (and all further) child nodes from the first one (logical and not).
        /// May be used with either 2D or 3D objects, but don't mix them.
        /// </summary>
        /// <param name="objects">child nodes</param>
        /// <returns></returns>
        public OSCADObject Difference(params OSCADObject[] objects)
        {
            return doBlockStatement("Difference", objects, (children) => { return new Difference(children); });
        }

        /// <summary>
        /// Creates the intersection of all child nodes. This keeps the overlapping portion (logical and).
        /// Only the area which is common or shared by all children is retained.
        /// May be used with either 2D or 3D objects, but don't mix them.
        /// </summary>
        /// <param name="objects">child nodes</param>
        /// <returns></returns>
        public OSCADObject Intersection(params OSCADObject[] objects)
        {
            return doBlockStatement("Intersection", objects, (children) => { return new Intersection(children); });
        }

        private OSCADObject doBlockStatement(string name, OSCADObject[] objects, Func<IEnumerable<OSCADObject>, OSCADObject> factory)
        {
            if (objects == null || objects.Length < 1)
            {
                throw new ArgumentException(name + " requires at least one non-null entities");
            }

            IEnumerable<OSCADObject> children = new List<OSCADObject>() { this };
            children = children.Concat(objects);
            return factory(children);
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Returns the computed position of this object.
        /// 
        /// For some objects that are the aggregate of many operations or
        /// multiple children, this may be an approximation or average
        /// of the position.
        /// </summary>
        /// <returns></returns>
        public abstract Vector3 Position();

        /// <summary>
        /// Returns the approximate boundaries of this OpenSCAD object
        /// </summary>
        /// <returns></returns>
        public abstract Bounds Bounds();

        /// <summary>
        /// Creates a copy of this object and all of its children
        /// 
        /// This is not a deep copy in the sense that all OSCADObjects will be new instances,
        /// but any complex objects used as parameters (Such as Vector3s) will be referenced by the copies
        /// </summary>
        /// <returns>Clone of this object</returns>
        public abstract OSCADObject Clone();        

        /// <summary>
        /// Indicates whether this OSCADObject is the same as another OSCADObject.
        /// This processes the scripts for both objects and is computationally expensive.  
        /// DO NOT use on deeply nested structures.
        /// </summary>
        /// <param name="other">OSCADObject to compare to</param>
        /// <returns>True if scripts are exactly the same, false otherwise</returns>
        public bool IsSameAs(OSCADObject other)
        {
            return this.ToString() == other.ToString();
        }

        /// <summary>
        /// Internal collection of children for this object
        /// </summary>
        protected List<OSCADObject> children = new List<OSCADObject>();

        /// <summary>
        /// Returns all children of this OSCADObject
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OSCADObject> Children()
        {
            // Initial children are reversed here because for objects with multiple children (such as boolean operations)
            // the natural collection order would yield opposite the expected order in a stack (first child would be the last popped)
            Stack<OSCADObject> toTraverse = new Stack<OSCADObject>(this.children.Reverse<OSCADObject>());
            List<OSCADObject> allChildren = new List<OSCADObject>();
            OSCADObject child = null;

            while(toTraverse.Count > 0)
            {
                child = toTraverse.Pop();
                allChildren.Add(child);

                foreach (var subChild in child.children)
                {
                    toTraverse.Push(subChild);
                }
            }

            return allChildren;
        }

        /// <summary>
        /// Copies the transforms that have been applied to another OSCADObject, and applies
        /// the same transforms to this object. (Only pure transforms, like Translate, Rotate, Scale, Color)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public OSCADObject Mimic(OSCADObject other)
        {
            OSCADObject finalObject = this;

            Stack<OSCADObject> toTraverse = new Stack<OSCADObject>();
            toTraverse.Push(other);
            other.Children().ToList().ForEach(child => toTraverse.Push(child));            

            while(toTraverse.Count > 0)
            {
                var current = toTraverse.Pop() as IMimic;     
                if(current != null)
                {
                    finalObject = current.MimicObject(finalObject);                
                }
            }

            return finalObject;
        }

        /// <summary>
        /// Writes the script for this OSCADObject to the file specified
        /// </summary>
        /// <param name="filePath">Path for the file to write.  Including filename and (optionally) file extension</param>
        public void ToFile(string filePath)
        {
            string path = filePath;

            if (!path.EndsWith(".scad"))
            {
                path += ".scad";
            }

            File.WriteAllLines(path, new string[] { this.ToString() });
        }
        #endregion

        #region Operators
        /// <summary>
        /// Adds two OSCADObjects together (unions them)
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static OSCADObject operator +(OSCADObject left, OSCADObject right)
        {
            if(left.GetType() == typeof(Union))
            {
                left.children.Add(right);
                return left;
            }
            else if(right.GetType() == typeof(Union))
            {
                right.children.Add(left);
                return right;
            }
            else
            {
                return new Union(new OSCADObject[] {left, right });
            }
        }

        /// <summary>
        /// Subtracts two OSCADObjects (differences them)
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static OSCADObject operator -(OSCADObject left, OSCADObject right)
        {
            if (left.GetType() == typeof(Difference))
            {
                left.children.Add(right);
                return left;
            }
            else if (right.GetType() == typeof(Difference))
            {
                right.children.Add(left);
                return right;
            }
            else
            {
                return new Difference(new OSCADObject[] {left, right });
            }
        }
        #endregion
    }
}
