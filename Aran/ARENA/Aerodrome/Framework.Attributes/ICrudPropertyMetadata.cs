using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Attributes
{
    public interface ICrudPropertyMetadata
    {
        bool DisplayInDetails { get; set; }
        bool DisplayInGrid { get; set; }
        bool DisplayInModification { get; set; }
        bool IsEnabledInEdit { get; set; }
        bool IsEnabledInNew { get; set; }

        PropertyRequirements PropertyRequirement { get; set; }

        PropertyCategories PropertyCategory { get; set; }

        string[] SetterPropertyNames { get; set; }

        string GetDisplayName(PropertyInfo pi);
    }
}
