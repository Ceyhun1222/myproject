using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Geometry = Aran.Geometries.Geometry;

namespace Aran.Delta
{
    public enum PropertyType
    {
        Geo,
        LowerLimit,
        UpperLimit,
        LayerName,
    }

    public static class FeatureExtension
    {
        private static Dictionary<string,Dictionary<PropertyType, object>> propList =
           new Dictionary<string,  Dictionary<PropertyType, object>>();

        private static void SetValue(string key, object value, PropertyType propType)
        {
            if (key == null) return;
            if (propList.ContainsKey(key))
            {
                if (propList[key].ContainsKey(propType))
                {
                    propList[key][propType] = value;

                }
                else
                {
                    propList[key].Add(propType, value);
                }

            }
            else
            {
                var propTypeDic = new Dictionary<PropertyType, object>();
                propTypeDic.Add(propType, value);
                propList.Add(key, propTypeDic);
            }
        }

        private static object GetValue(string key, PropertyType propType)
        {
            try
            {
                if (propList.ContainsKey(key))
                    if (propList[key].ContainsKey(propType))
                        return propList[key][propType];
                return null;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        private static void SetValue(Airspace airspace, object value, PropertyType propType)
        {
            string key = airspace.Designator;
            if (key == null)
                key = airspace.Name;
            if (key == null) return;
            SetValue(key, value, propType);
        }

        private static object GetValue(Airspace airspace, PropertyType propType)
        {
            try
            {
                string key = airspace.Designator;
                return GetValue(key, propType);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void SetGeom(this Airspace airspace, Geometry value)
        {
            SetValue(airspace,  value, PropertyType.Geo);
        }

        public static Geometry GetGeom(this Airspace airspace)
        {
            try
            {
                return (Geometry)GetValue(airspace, PropertyType.Geo);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SetLowerLimit(this Airspace airspace, ValDistanceVertical value)
        {
            SetValue(airspace, value, PropertyType.LowerLimit);
        }

        public static ValDistanceVertical GetLowerLimit(this Airspace airspace)
        {
            try
            {
                return (ValDistanceVertical)GetValue(airspace, PropertyType.LowerLimit);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SetUpperLimit(this Airspace airspace, ValDistanceVertical value)
        {
            SetValue(airspace, value, PropertyType.UpperLimit);
        }

        public static ValDistanceVertical GetUpperLimit(this Airspace airspace)
        {
            try
            {
                return (ValDistanceVertical)GetValue(airspace, PropertyType.UpperLimit);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SetLayerName(this Airspace airspace, string value)
        {
            SetValue(airspace, value, PropertyType.LayerName);
        }

        public static string GetLayerName(this Airspace airspace)
        {
            try
            {
                return (string)GetValue(airspace, PropertyType.LayerName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Route extensions

        public static void SetGeom(this Route route, Geometry value)
        {
            SetValue(route.Name, value, PropertyType.Geo);
        }

        public static Geometry GetGeom(this Route route)
        {
            try
            {
                return (Geometry)GetValue(route.Name, PropertyType.Geo);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SetLayerName(this Route route, string value)
        {
            SetValue(route.Name, value, PropertyType.LayerName);
        }

        public static string GetLayerName(this Route route)
        {
            try
            {
                return (string)GetValue(route.Name, PropertyType.LayerName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ValDistanceVertical GetLowerLimit(this Route route)
        {
            try
            {
                return (ValDistanceVertical)GetValue(route.Name, PropertyType.LowerLimit);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SetUpperLimit(this Route airspace, ValDistanceVertical value)
        {
            SetValue(airspace.Name, value, PropertyType.UpperLimit);
        }

        public static ValDistanceVertical GetUpperLimit(this Route route)
        {
            try
            {
                return (ValDistanceVertical)GetValue(route.Name, PropertyType.UpperLimit);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SetLowerLimit(this Route route, ValDistanceVertical value)
        {
            SetValue(route.Name, value, PropertyType.LowerLimit);
        }

    }
}
