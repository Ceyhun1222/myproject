using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Omega.VSImporter
{
    static class RowExtension
    {
        public static T GetValue<T>(this IRow row, string fieldName)
        {
            var fieldIndex = row.Fields.FindField(fieldName);
            if (fieldIndex < 0) return default(T);

            var value = row.Value[fieldIndex];
            if (value is DBNull)
                return default(T);
            return (T)value;
        }

        public static bool? CheckAvaliablity(this IRow row, string fieldName)
        {
            var lighting = row.GetValue<string>(fieldName);
            if (lighting != null)
            {
                if (lighting.ToLower() == "available")
                    return true;
                return false;
            }

            return null;
        }

        public static bool? IsGroup(this IRow row, string fieldName)
        {
            var lighting = row.GetValue<string>(fieldName);
            if (lighting != null)
            {
                if (lighting.ToLower() == "yes")
                    return true;
                return false;
            }

            return null;
        }

    }
}
