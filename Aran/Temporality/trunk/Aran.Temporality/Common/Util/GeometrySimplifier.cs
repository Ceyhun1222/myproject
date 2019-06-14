using System;
using System.Collections.Generic;
using Aran.Geometries;

namespace Aran.Temporality.Common.Util
{
    public class GeometrySimplifier
    {
        public static List<Point> GetBorderPoints(Geometry geometry)
        {
            var result=new List<Point>();
            switch (geometry.Type)
            {
                case GeometryType.Null:
                    break;
                case GeometryType.Point:
                    result.Add(geometry as Point);
                    break;
                case GeometryType.LineString:
                    result.AddRange(geometry as LineString);
                    break;
                case GeometryType.Polygon:
                    result.AddRange((geometry as Polygon).ToMultiPoint());
                    break;
                case GeometryType.MultiPoint:
                    result.AddRange(geometry as MultiPoint);
                    break;
                case GeometryType.MultiLineString:
                    result.AddRange((geometry as MultiLineString).ToMultiPoint());
                    break;
                case GeometryType.MultiPolygon:
                    result.AddRange((geometry as MultiPolygon).ToMultiPoint());
                    break;
                case GeometryType.Ring:
                    result.AddRange(geometry as Ring);
                    break;
                case GeometryType.LineSegment:
                    result.Add((geometry as LineSegment).Start);
                    result.Add((geometry as LineSegment).End);
                    break;
                default:
                    throw new NotImplementedException();
            }

            result.RemoveAll(t => t == null);

            return result;
        }


        class Circle {
            public Point Point;
            public double Radius;
        }

        static Dictionary<Geometry, Circle> cach = new Dictionary<Geometry, Circle>();

        public static bool GetExtend(Geometry geometry,out double minX, out double minY, out double maxX, out double maxY)
        {
            minX = 0;
            minY = 0;
            maxX = 0;
            maxY = 0;

            var borderPoints = GetBorderPoints(geometry);
            if (borderPoints.Count == 0) return false;

            //define bounding box
            var firstPoint = borderPoints[0];
            minX = firstPoint.X;
            maxX = firstPoint.X;
            minY = firstPoint.Y;
            maxY = firstPoint.Y;

            foreach (var point in borderPoints)
            {
                if (point.X > maxX) maxX = point.X;
                if (point.Y > maxY) maxY = point.Y;
                if (point.X < minX) minX = point.X;
                if (point.Y < minY) minY = point.Y;
            }

            return true;
        }

        

        public static void Simplify(Geometry geometry, out Point center, out double radius)
        {
            radius = 0;
            center = null;

            if (geometry == null) return;

            Circle circle;

            if (!cach.TryGetValue(geometry, out circle))
            {
                

                //define center
                if (geometry is Point)
                {
                    center = new Point(geometry as Point);
                }
                else
                {
                    double minX, minY,  maxX, maxY;
                    if (!GetExtend(geometry,out minX, out minY, out maxX, out maxY))
                    {
                        return;
                    }


                    //TODO: manage negative coordinates
                    #warning manage negative coordinates


                    center = new Point
                                 {
                                     X = (minX + maxX)*0.5,
                                     Y = (minY + maxY)*0.5,
                                 };

                    radius = NativeMethods.ReturnGeodesicDistance(center.X, center.Y, minX, minY);
                    var radius2 = NativeMethods.ReturnGeodesicDistance(center.X, center.Y, minX, maxY);
                    if (radius2 > radius) radius = radius2;

                    
                }




                circle = new Circle { Point = center, Radius =radius};
                cach[geometry] = circle;
              
                return;
            }

            center= circle.Point;
            radius = circle.Radius;
        }


    }
}
