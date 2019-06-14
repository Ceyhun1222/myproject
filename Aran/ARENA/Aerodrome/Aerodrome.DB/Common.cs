using Aerodrome.DataType;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.DB
{
    public static class Common
    {
        public static Dictionary<string, Enum> PropertyTypeMapping = new Dictionary<string, Enum>();

        static Common()
        {
            PropertyTypeMapping.Add(typeof(string).FullName, esriFieldType.esriFieldTypeString);
            PropertyTypeMapping.Add(typeof(double).FullName, esriFieldType.esriFieldTypeDouble);
            PropertyTypeMapping.Add(typeof(DateTime).FullName, esriFieldType.esriFieldTypeDate);
            PropertyTypeMapping.Add(typeof(int).FullName, esriFieldType.esriFieldTypeInteger);
            PropertyTypeMapping.Add(typeof(short).FullName, esriFieldType.esriFieldTypeSmallInteger);
            PropertyTypeMapping.Add(typeof(IPoint).FullName, esriGeometryType.esriGeometryPoint);
            PropertyTypeMapping.Add(typeof(IPolyline).FullName, esriGeometryType.esriGeometryPolyline);
            PropertyTypeMapping.Add(typeof(IPolygon).FullName, esriGeometryType.esriGeometryPolygon);
        }

        public static T GetAttributeFrom<T>(Type type, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);
            var property = type.GetProperty(propertyName);

            var hasAttr = Attribute.IsDefined(property, attrType);

            //var attr = (T[])property.GetCustomAttributes(attrType, false);

            if (!hasAttr) return null;

            return (T)property.GetCustomAttributes(attrType, false).FirstOrDefault();

        }

    }
}
