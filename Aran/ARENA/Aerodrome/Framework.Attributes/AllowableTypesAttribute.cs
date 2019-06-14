using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Attributes
{
    public sealed class AllowableTypesAttribute:Attribute
    {
      
        public Type[] AllovableTypes { get; set; }

        public AllowableTypesAttribute(params Type[] types)
        {
            this.AllovableTypes = types;
        }
    }
}
