using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Stasy.Helper
{
    public class FeaturePropertyMappingInfo
    {
        public FeaturePropertyMappingInfo()
        {
            ForeignKeySetters = new List<SetterPropertyInfo>();
        }

        public string ForeignKeyPropertyName { get; set; }     

        public List<SetterPropertyInfo> ForeignKeySetters { get; set; }

        public string ForeignKeySeparator { get; set; }

        public bool IsCollection { get; set; }

    }

    public class SetterPropertyInfo
    {
        public PropertyInfo SetterProperty { get; set; }

        public string PropertyNameForRelation { get; set; }
    }
}
