using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.PANDA.Vss
{
    public class DrawElementMethodAttribute : Attribute
    {
        public DrawElementMethodAttribute(DrawElementType elementType)
        {
            ElementType = elementType;
        }

        public DrawElementType ElementType { get; private set; }
    }
}
