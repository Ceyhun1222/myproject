using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Aim.BusinessRules.SbvrParser
{
    public class EnumEx
    {
        public static bool TryGetValueFromDescription<T>(string description, bool ignoreCase, out T retVal)
        {
            var type = typeof(T);

            if (!type.IsEnum)
                throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attribute != null)
                {
                    if (string.Compare(attribute.Description, description, ignoreCase) == 0)
                    {
                        retVal = (T)field.GetValue(null);
                        return true;
                    }
                }
                else
                {
                    if (string.Compare(field.Name, description, ignoreCase) == 0)
                    {
                        retVal = (T)field.GetValue(null);
                        return true;
                    }
                }
            }

            retVal = default(T);
            return false;
        }
    }
}
