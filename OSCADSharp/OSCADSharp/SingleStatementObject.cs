﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSCADSharp
{
    /// <summary>
    /// A statement with just one nested child node
    /// </summary>
    internal abstract class SingleStatementObject : OSCADObject
    {
        protected OSCADObject obj { get; set; }

        protected SingleStatementObject(OSCADObject obj)
        {
            this.obj = obj;

            this.m_children.Add(obj);
            obj.Parent = this;
        }        
    }
}