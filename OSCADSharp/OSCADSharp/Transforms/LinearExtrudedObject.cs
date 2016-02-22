using OSCADSharp.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSCADSharp.Spatial;

namespace OSCADSharp.Transforms
{
    /// <summary>
    /// Linear Extrusion is a modeling operation that takes a 2D polygon as input and extends it in the third dimension. This way a 3D shape is created.
    /// 
    /// This is a limited subset of the capabilities
    /// </summary>
    internal class LinearExtrudedObject : OSCADObject
    {
        /// <summary>
        /// Height to extrude to
        /// </summary>
        public double Height { get; set; } = 1.0;
        private OSCADObject obj;

        //TODO: Possibly implement everything else?
        //linear_extrude(height = fanwidth, center = true, convexity = 10, twist = -fanrot, slices = 20, scale = 1.0) {...}        

        /// <summary>
        /// An object that will be extruded from 2d to 3d
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="height"></param>
        public LinearExtrudedObject(OSCADObject obj, double height)
        {
            this.obj = obj;
            this.Height = height;

            this.children.Add(obj);
        }

        public override OSCADObject Clone()
        {
            return new LinearExtrudedObject(this.obj.Clone(), this.Height)
            {
                Name = this.Name
            };
        }

        public override string ToString()
        {
            string extrudeCommand = String.Format("linear_extrude(height = {0})", this.Height.ToString());
            var formatter = new SingleBlockFormatter(extrudeCommand, this.obj.ToString());
            return formatter.ToString();
        }

        public override Vector3 Position()
        {
            throw new NotSupportedException();
        }

        public override Bounds Bounds()
        {
            throw new NotImplementedException();
        }
    }
}
