using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRuleManager
{
    static class Extensions
    {
        public static T GetValue<T>(this ExpandoObject eo, string propName, T defaultValue = default(T))
        {
            if ((eo as IDictionary<string, object>).TryGetValue(propName, out object value))
                return (T)value;
            
            return defaultValue;
        }
    }
}
