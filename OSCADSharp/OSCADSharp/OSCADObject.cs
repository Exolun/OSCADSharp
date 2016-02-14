using OSCADSharp.Booleans;
using OSCADSharp.Transforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    public abstract class OSCADObject
    {
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
            return doBoolean("Union", objects, (children) => { return new Union(children); });
        }

        /// <summary>
        /// Subtracts the 2nd (and all further) child nodes from the first one (logical and not).
        /// May be used with either 2D or 3D objects, but don't mix them.
        /// </summary>
        /// <param name="objects">child nodes</param>
        /// <returns></returns>
        public OSCADObject Difference(params OSCADObject[] objects)
        {
            return doBoolean("Difference", objects, (children) => { return new Difference(children); });
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
            return doBoolean("Intersection", objects, (children) => { return new Intersection(children); });
        }

        private OSCADObject doBoolean(string name, OSCADObject[] objects, Func<IEnumerable<OSCADObject>, OSCADObject> factory)
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


        protected List<OSCADObject> children = new List<OSCADObject>();
        /// <summary>
        /// Returns all children of this OSCADObject
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OSCADObject> Children()
        {
            Stack<OSCADObject> toTraverse = new Stack<OSCADObject>(this.children);
            List<OSCADObject> allChildren = new List<OSCADObject>();
            OSCADObject child = null;

            while(toTraverse.Count > 0)
            {
                child = toTraverse.Pop();
                allChildren.Add(child);

                foreach (var subChild in child.Children())
                {
                    toTraverse.Push(subChild);
                }
            }

            return allChildren;
        }

        /// <summary>
        /// Copies the transforms that have been applied to another OSCADObject, and applies
        /// the same transforms to this object. (Only transforms)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public OSCADObject Mimic(OSCADObject other)
        {
            IEnumerable<OSCADObject> children = other.Children();
            Stack<OSCADObject> stack = new Stack<OSCADObject>();
            OSCADObject finalObject = this;
            stack.Push(other);

            foreach (var child in children)
            {
                stack.Push(child);
            }

            while(stack.Count > 0)
            {
                var current = stack.Pop();
                if(!(current is IMimicer))
                {
                    continue;
                }
                else
                {
                    finalObject = ((IMimicer)current).MimicObject(finalObject);
                }
            }

            return finalObject;
        }
    }
}
