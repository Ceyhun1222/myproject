using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Attributes
{
    public sealed class CrudFeatureConfigurationAttribute:Attribute
    {
        public FeatureCategories FeatureCategory { get; set; }
       
    }
}
