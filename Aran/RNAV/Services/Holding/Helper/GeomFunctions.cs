using System;
using System.Collections.Generic;
using System.Linq;
using Aran.Geometries;
using Aran.Aim.Features;
using Aran.PANDA.Common;

namespace Holding
{
    public static class GeomFunctions
    {

        public static Aran.Geometries.Point Assign(AixmPoint dPoint)
        {
            Aran.Geometries.Point aranPoint = new Aran.Geometries.Point();
            aranPoint.X = dPoint.Geo.X;
            aranPoint.Y = dPoint.Geo.Y;
            return aranPoint;
        }

        public static Aran.Geometries.Point AssignToPrj(AixmPoint aixmPoint)
        {
            Aran.Geometries.Point aranPoint = new Aran.Geometries.Point();
            aranPoint.X = aixmPoint.Geo.X;
            aranPoint.Y = aixmPoint.Geo.Y;
            return GlobalParams.SpatialRefOperation.ToPrj(aranPoint);
        }

        public static Aran.Geometries.Point Assign(ElevatedPoint elevPoint)
        {
            Aran.Geometries.Point aranPoint = new Aran.Geometries.Point();
            aranPoint.X = elevPoint.Geo.X;
            aranPoint.Y = elevPoint.Geo.Y;
            return aranPoint;
        }

        public static Aran.Geometries.Point AssignToPrj(ElevatedPoint aixmPoint)
        {
            Aran.Geometries.Point aranPoint = new Aran.Geometries.Point();
            aranPoint.X = aixmPoint.Geo.X;
            aranPoint.Y = aixmPoint.Geo.Y;
            return GlobalParams.SpatialRefOperation.ToPrj(aranPoint);
        }

        public static Aran.Geometries.Point AimToAranPointPrj(Aran.Geometries.Point aranPt)
        {
            if (aranPt == null)
                return null;
            return GlobalParams.SpatialRefOperation.ToPrj(new Aran.Geometries.Point(aranPt.X, aranPt.Y));
        }

        public static Aran.Geometries.Point AranToAimPoint(Aran.Geometries.Point aimPt)
        {
            if (aimPt == null)
                return null;
            return new Aran.Geometries.Point(aimPt.X, aimPt.Y);
        }

        public static Polygon MoveShablonAroundTolArea(Polygon shablon, Aran.Geometries.Ring toleranceArea, Aran.Geometries.Point PtnavPrj)
        {
            Polygon[] tmpP = new Polygon[toleranceArea.Count];

            for (int i = 0; i < toleranceArea.Count; i++)
            {
                tmpP[i] = (Polygon)TransForm.Move(shablon, PtnavPrj, toleranceArea[i]);
            }

            return ChainHullAlgorithm.ConvexHull(tmpP[0], tmpP[1], tmpP[2], tmpP[3]);

        }

        public static LineString MaxPointsFromArcToSpiral(Aran.Geometries.Point ptCenter, Aran.Geometries.Ring arc, LineString Spiral)
        {
            LineString maxPoints = new LineString();
            Aran.Geometries.Point tmpPoint;
            foreach (Aran.Geometries.Point toPoint in arc)
            {
                LineString tmpPart = TransForm.Move(Spiral, ptCenter, toPoint) as LineString;
                MaxDistFromPointToGeomety(ptCenter, tmpPart, out tmpPoint);
                maxPoints.Add(tmpPoint);
            }
            return maxPoints;
        }

        public static Aran.Geometries.Point MaxDistFromLineToMultipoint(Aran.Geometries.Point refpnt, double direction, MultiPoint multipoint)
        {
            Aran.Geometries.Point result = null;

            if (multipoint.Count == 0)
                return result;
            double maxDist = ARANFunctions.PointToLineDistance(multipoint[0], refpnt, direction);
            foreach (Aran.Geometries.Point pnt in multipoint)
            {
                double distance = ARANFunctions.PointToLineDistance(pnt, refpnt, direction);
                if (distance > maxDist)
                {
                    result = pnt;
                    maxDist = distance;
                }
            }
            return result;
        }

        public static Aran.Geometries.Point MinDistFromLineToMultipoint(Aran.Geometries.Point refpnt, double direction, List<Aran.Geometries.Point> multipoint)
        {
            Aran.Geometries.Point result = null;

            if (multipoint.Count == 0)
                return result;
            double minDist = ARANFunctions.PointToLineDistance(multipoint[0], refpnt, direction);
            foreach (Aran.Geometries.Point pnt in multipoint)
            {
                double distance = ARANFunctions.PointToLineDistance(pnt, refpnt, direction);
                if (distance < minDist)
                {
                    result = pnt;
                    minDist = distance;
                }
            }
            return result;
        }

        public static MultiPoint MinAndMaxDistFromLineToMultipoint(Aran.Geometries.Point refpnt, double direction, MultiPoint multipoint)
        {
            MultiPoint result = new MultiPoint();
            if (multipoint.Count == 0)
                return result;
            Aran.Geometries.Point minpnt, maxpnt;
            minpnt = multipoint[0];
            maxpnt = minpnt;
            double minDist = ARANFunctions.PointToLineDistance(minpnt, refpnt, direction);
            double maxDist = minDist;
            int i = 0;
            foreach (Aran.Geometries.Point pnt in multipoint)
            {
                double distance = ARANFunctions.PointToLineDistance(pnt, refpnt, direction);
                if (distance > maxDist)
                {
                    maxpnt = pnt;
                    maxDist = distance;
                }
                if (distance < minDist)
                {
                    minpnt = pnt;
                    minDist = distance;
                }
                i++;
            }
            result.Add(minpnt);
            result.Add(maxpnt);
            return result;
        }

        public static double MaxDistFromPointToGeomety(Aran.Geometries.Point fromPoint, MultiPoint geom, out Aran.Geometries.Point result)
        {
            double distance, maxDistance = 0;
            result = new Aran.Geometries.Point();
            double dx, dy;
            foreach (Aran.Geometries.Point toPoint in geom)
            {
                dx = toPoint.X - fromPoint.X;
                dy = toPoint.Y - fromPoint.Y;

                distance = dx * dx + dy * dy;
                if (distance >= maxDistance)
                {
                    maxDistance = distance;
                    result = toPoint;
                }
            }
            return Math.Sqrt(maxDistance);
        }

        public static double MaxDistFromPointToGeometry(Aran.Geometries.Point fromPoint, LineString part, out Aran.Geometries.Point result)
        {
            double distance, maxDistance = 0;
            result = new Aran.Geometries.Point();
            double dx, dy;
            foreach (Aran.Geometries.Point toPoint in part)
            {
                dx = toPoint.X - fromPoint.X;
                dy = toPoint.Y - fromPoint.Y;

                distance = dx * dx + dy * dy;
                if (distance >= maxDistance)
                {
                    maxDistance = distance;
                    result = toPoint;
                }
            }
            return Math.Sqrt(maxDistance);
        }

        public static double MinDistFromPointToGeometry(Aran.Geometries.Point fromPoint, LineString part, out Aran.Geometries.Point result)
        {
            double distance, minDistance = 1000000000000000000;
            result = new Aran.Geometries.Point();
            double dx, dy;
            foreach (Aran.Geometries.Point toPoint in part)
            {
                dx = toPoint.X - fromPoint.X;
                dy = toPoint.Y - fromPoint.Y;

                distance = dx * dx + dy * dy;
                if (distance <= minDistance)
                {
                    minDistance = distance;
                    result = toPoint;
                }
            }
            return Math.Sqrt(minDistance);
        }

        public static double MaxDistFromPointToGeometry(Aran.Geometries.Point fromPoint, MultiLineString polyLine, out Aran.Geometries.Point result)
        {
            double distance, maxDistance = 0;
            result = new Aran.Geometries.Point();
            double dx, dy;
            foreach (LineString part in polyLine)
            {
                foreach (Aran.Geometries.Point toPoint in part)
                {
                    dx = toPoint.X - fromPoint.X;
                    dy = toPoint.Y - fromPoint.Y;

                    distance = dx * dx + dy * dy;
                    if (distance >= maxDistance)
                    {
                        maxDistance = distance;
                        result = toPoint;
                    }
                }
            }
            return Math.Sqrt(maxDistance);
        }

        public static LineString BaseArea5(Aran.Geometries.Point ptCenter, Aran.Geometries.Point fromPt, Aran.Geometries.Point endPt)
        {
            double distance1, distance2;
            LineString baseArea5 = new LineString();
            double rad1 = ARANFunctions.ReturnAngleInRadians(ptCenter, fromPt);
            double rad2 = ARANFunctions.ReturnAngleInRadians(ptCenter, endPt);
            double n = Math.Abs(rad2 - rad1) * 180 / Math.PI;
            distance1 = ARANFunctions.ReturnDistanceInMeters(ptCenter, fromPt);
            distance2 = ARANFunctions.ReturnDistanceInMeters(ptCenter, endPt);
            double incDist = (distance2 - distance1) / n;
            for (int i = 0; i < n; i++)
            {
                Aran.Geometries.Point pt = ARANFunctions.LocalToPrj(ptCenter, rad1 - i * Math.PI / 180, distance1 + i * incDist, 0);
                baseArea5.Add(pt);
            }
            return baseArea5;

        }

        public static LineString PointsOnGeometry(LineString part, Aran.Geometries.Point pt)
        {
            LineString result = new LineString();
            foreach (Aran.Geometries.Point item in part)
            {
                result.Add(item);
                if ((item.X == pt.X) & (item.Y == pt.Y))
                    return result;
            }
            return null;
        }

        public static Polygon MoveGeometryAroundCircle(Geometry geom, Aran.Geometries.Point geomCenterPoint, Aran.Geometries.Point circleCenterPoint, double radius)
        {
            double radStep = 1 * Math.PI / 180;
            double rad;
            Aran.Geometries.Point pt = new Aran.Geometries.Point();
            Geometry[] result = new Geometry[360];
            for (int i = 0; i < 360; i++)
            {
                rad = radStep * i;
                pt.X = circleCenterPoint.X + radius * Math.Cos(rad);
                pt.Y = circleCenterPoint.Y + radius * Math.Sin(rad);
                result[i] = TransForm.Move(geom, geomCenterPoint, pt);
            }
            return ChainHullAlgorithm.ConvexHull(result);

        }

        public static Polygon MoveGeometryAroundTwoPoint(Geometry geom, Aran.Geometries.Point geomCenterPoint, Aran.Geometries.Point fromPt, Aran.Geometries.Point toPt, TurnDirection turn)
        {
            Aran.Geometries.Ring arc = ARANFunctions.CreateArcPrj(geomCenterPoint, fromPt, toPt, turn);
            Geometry[] geomCollection = new Geometry[arc.Count];
            for (int i = 0; i < arc.Count; i++)
            {
                geomCollection[i] = TransForm.Move(geom, geomCenterPoint, arc[i]);
            }
            return ChainHullAlgorithm.ConvexHull(geomCollection);
        }

        public static Aran.Geometries.Ring AranToAimRing(Ring aranRing)
        {
            Aran.Geometries.Ring result = new Aran.Geometries.Ring();
            for (int i = 0; i < aranRing.Count; i++)
            {
                Aran.Geometries.Point ptGeo = AranToAimPoint(GlobalParams.SpatialRefOperation.ToGeo<Aran.Geometries.Point >(aranRing[i]));
                result.Add(ptGeo);
            }
            return result;
        }

        public static Curve ConvertPolylineToCurve(Aran.Geometries.MultiLineString polyline)
        { 
            if (polyline==null)
                return null;
            Curve curve  = new Curve();
            for (int i = 0; i < polyline.Count; i++)
            {
                Aran.Geometries.LineString lineString = new Aran.Geometries.LineString();
                for (int j = 0; j < polyline[i].Count; j++)
                {
                    lineString.Add(GeomFunctions.AranToAimPoint(polyline[i][j]));    
                }
                curve.Geo.Add(lineString);
            }
            return curve;
        }

        public static Surface ConvertMultiPolygonToSurface(Aran.Geometries.MultiPolygon multiPolygon)
        {
            Surface surface = new Surface();
            foreach (Polygon poly in multiPolygon)
            {
                surface.Geo.Add(poly);
            }
            
            return surface;
        }

        public static Aran.Geometries.Polygon CreateExtent(double minX, double minY, double maxX, double maxY)
        {
            var result = new Aran.Geometries.Polygon();
            var ring = new Aran.Geometries.Ring
            {
                new Aran.Geometries.Point(minX, minY),
                new Aran.Geometries.Point(minX, maxY),
                new Aran.Geometries.Point(maxX, maxY),
                new Aran.Geometries.Point(maxX, minY)
            };
            result.ExteriorRing = ring;
            return result;

        }


    }
}
    

	