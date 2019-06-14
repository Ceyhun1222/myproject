using System;
using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Aim.PropertyEnum;
using Aran.Geometries;
using ESRI.ArcGIS.Geometry;

namespace Aran.Omega.Models
{
    public enum PropertyType
    {
        Distance,
        Geo,
        Elevation,
        EsriGeo,
        JtsGeo,
        Buffer,
    }

    public static class VerticalStructureExtension
    {
        //private static Dictionary<string, double> distanceFromRunway = new Dictionary<string, double>();

        //private static Dictionary<string, Geometry> prjGeoObstacles = new Dictionary<string, Geometry>();

//        private static Dictionary<string, double> elevation = new Dictionary<string, double>();

        private static Dictionary<string, Dictionary<int, Dictionary<PropertyType, object>>> propList =
            new Dictionary<string, Dictionary<int, Dictionary<PropertyType, object>>>();

        private static void SetValue(VerticalStructure vs, int partNumber, object value,PropertyType propType)
        {
             string key = vs.Identifier.ToString();
            if (propList.ContainsKey(key))
            {
                if (propList[key].ContainsKey(partNumber))
                {
                    if (propList[key][partNumber].ContainsKey(propType))
                        propList[key][partNumber][propType] = value;
                    else
                        propList[key][partNumber].Add(propType, value);
                }
                else
                {
                    var dic = new Dictionary<PropertyType, object>();
                    dic.Add(propType, value);
                    propList[key].Add(partNumber, dic);
                }
            }
            else
            {
                var dic = new Dictionary<int, Dictionary<PropertyType, object>>();
                var propTypeDic = new Dictionary<PropertyType, object>();
                propTypeDic.Add(propType, value);
                dic.Add(partNumber, propTypeDic);
                propList.Add(key, dic);
            }
        }

        private static object GetValue(VerticalStructure vs, int partNumber, PropertyType propType)
        {
            try
            {
                string key = vs.Identifier.ToString();
                if (propList.ContainsKey(key))
                    if (propList[key].ContainsKey(partNumber))
                        if (propList[key][partNumber].ContainsKey(propType))
                            return propList[key][partNumber][propType];
                return null;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void SetDistance(this VerticalStructure vs,int partNumber, double distance)
        {
            SetValue(vs, partNumber, distance,PropertyType.Distance);
        }

        public static double GetDistance(this VerticalStructure vs, int partNumber)
        {
            try
            {
                var distance =GetValue(vs, partNumber, PropertyType.Distance);
                if (distance == null)
                    return 0;
                
                return (double)distance;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static void SetElevation(this VerticalStructure vs,int partNumber, double value)
        {
            SetValue(vs, partNumber, value, PropertyType.Elevation);
        }

        public static double GetElevation(this VerticalStructure vs, int partNumber)
        {
            try
            {
                return (double)GetValue(vs, partNumber, PropertyType.Elevation);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static void SetGeom(this VerticalStructure vs,int partNumber, Geometry value)
        {
            SetValue(vs, partNumber, value, PropertyType.Geo);
        }

        public static Geometry GetGeom(this VerticalStructure vs,int partNumber)
        {
            try
            {
                return (Geometry)GetValue(vs, partNumber, PropertyType.Geo);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SetJtsGeom(this VerticalStructure vs, int partNumber, GeoAPI.Geometries.IGeometry value)
        {
            SetValue(vs, partNumber, value, PropertyType.JtsGeo);
        }

        public static GeoAPI.Geometries.IGeometry GetJtsGeom(this VerticalStructure vs, int partNumber)
        {
            try
            {
                return (GeoAPI.Geometries.IGeometry)GetValue(vs, partNumber, PropertyType.JtsGeo);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SetJtsBuffer(this VerticalStructure vs, int partNumber, GeoAPI.Geometries.IGeometry value)
        {
            SetValue(vs, partNumber, value, PropertyType.Buffer);
        }

        public static GeoAPI.Geometries.IGeometry GetJtsBuffer(this VerticalStructure vs, int partNumber)
        {
            try
            {
                return (GeoAPI.Geometries.IGeometry)GetValue(vs, partNumber, PropertyType.Buffer);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public static void SetEsriGeom(this VerticalStructure vs, int partNumber, IGeometry value)
        {
            SetValue(vs, partNumber, value, PropertyType.EsriGeo);
        }

        public static IGeometry GetEsriGeom(this VerticalStructure vs, int partNumber)
        {
            try
            {
                return (IGeometry)GetValue(vs, partNumber, PropertyType.EsriGeo);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static MultiPoint ToMultiPoint(this IEnumerable<Aran.Geometries.Point> pointList)
        {
            var mlt = new MultiPoint();
            foreach (var point in pointList)
                mlt.Add(point);
            return mlt;
        }
    }

    public static class RunwayCenterLinePointExtension
    {

        private static Dictionary<string, double> propList =
           new Dictionary<string, double>();

        public static void SetDistance(this RunwayCentrelinePoint cntln, double distance)
        {
            string key = cntln.Identifier.ToString();
            if (propList.ContainsKey(key))
            {
                propList[key] = distance;
            }
            else
            {
                propList.Add(key,distance);
            }
        }

        public static double GetDistance(this RunwayCentrelinePoint cntln)
        {
             try
            {
                string key = cntln.Identifier.ToString();
                if (propList.ContainsKey(key))
                    return propList[key];
                return 0;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}