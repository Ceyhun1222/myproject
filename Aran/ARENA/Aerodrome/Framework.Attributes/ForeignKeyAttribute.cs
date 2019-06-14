using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class ForeignKeyAttribute : Attribute
    {
        public ForeignKeyAttribute(Type parentType)
        {
            this.ParentType = parentType;
        }

        public Type ParentType { get; private set; }
    }
}
