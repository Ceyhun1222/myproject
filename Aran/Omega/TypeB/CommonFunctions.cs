using System;
using System.Linq;
using Aran.Geometries;
using Aran.Omega.TypeB.Models;
using Aran.Omega.SettingsUI;
using Aran.Panda.Common;

namespace Aran.Omega.TypeB
{
    public static class CommonFunctions
    {
        public static PlaneParam CalcPlaneParam(Aran.Geometries.Point pt1, Aran.Geometries.Point pt2, Aran.Geometries.Point pt3)
        {
            PlaneParam fPlane = new PlaneParam();
            fPlane.C = pt1.X * (pt2.Y - pt3.Y) + pt2.X * (pt3.Y - pt1.Y) + pt3.X * (pt1.Y - pt2.Y);
            fPlane.A = (pt1.Y * (pt2.Z - pt3.Z) + pt2.Y * (pt3.Z - pt1.Z) + pt3.Y * (pt1.Z - pt2.Z))/fPlane.C;
            fPlane.B = (pt1.Z * (pt2.X - pt3.X) + pt2.Z * (pt3.X - pt1.X) + pt3.Z * (pt1.X - pt2.X))/fPlane.C;
            fPlane.D = (-(pt1.X * (pt2.Y * pt3.Z - pt3.Y * pt2.Z) + pt2.X * (pt3.Y * pt1.Z - pt1.Y * pt3.Z) + pt3.X * (pt1.Y * pt2.Z - pt2.Y * pt1.Z)))/fPlane.C;
            return fPlane;
        }

        public static PlaneParam CalcPlaneParamFromPrjPts(Aran.Geometries.Point center,double axis ,Aran.Geometries.Point ptPrj1, Aran.Geometries.Point ptPrj2, Aran.Geometries.Point ptPrj3)
        {
            Aran.Geometries.Point pt1= ARANFunctions.PrjToLocal(center, axis, ptPrj1);
            Aran.Geometries.Point pt2 = ARANFunctions.PrjToLocal(center, axis, ptPrj2);
            Aran.Geometries.Point pt3 = ARANFunctions.PrjToLocal(center, axis, ptPrj3);

            PlaneParam fPlane = new PlaneParam();
            fPlane.C = pt1.X * (pt2.Y - pt3.Y) + pt2.X * (pt3.Y - pt1.Y) + pt3.X * (pt1.Y - pt2.Y);
            fPlane.A = (pt1.Y * (pt2.Z - pt3.Z) + pt2.Y * (pt3.Z - pt1.Z) + pt3.Y * (pt1.Z - pt2.Z)) / fPlane.C;
            fPlane.B = (pt1.Z * (pt2.X - pt3.X) + pt2.Z * (pt3.X - pt1.X) + pt3.Z * (pt1.X - pt2.X)) / fPlane.C;
            fPlane.D = (-(pt1.X * (pt2.Y * pt3.Z - pt3.Y * pt2.Z) + pt2.X * (pt3.Y * pt1.Z - pt1.Y * pt3.Z) + pt3.X * (pt1.Y * pt2.Z - pt2.Y * pt1.Z))) / fPlane.C;
            return fPlane;
        }

        public static PlaneParam CalcPlaneParamY(Aran.Geometries.Point pt1,Aran.Geometries.Point pt2,double yCoef)
        {
            PlaneParam planeParam =new PlaneParam();
            planeParam.A= (pt2.Z - pt1.Z + yCoef * Math.Abs(pt1.Y) - yCoef * Math.Abs(pt2.Y)) / (pt2.X - pt1.X);
            planeParam.B = yCoef;
            planeParam.C = -1;
            planeParam.D = pt1.Z - yCoef * Math.Abs(pt1.Y) - pt1.X * planeParam.A;
            return planeParam;
        }

        public static Aran.Geometries.MultiPolygon CreateMultipolygonFromPoints(Aran.Geometries.Point[] points)
        {
            var ring = new Ring();
            foreach (var pt in points)
            {
                ring.Add(pt);
            }
            return  new MultiPolygon { new Polygon { ExteriorRing = ring } };
        }

        public static Aran.Geometries.Point GetXyPoint(Aran.Geometries.Point pt, Aran.Geometries.Point ptBase,double direction)
        {
            var ptResult = ARANFunctions.PrjToLocal(ptBase, direction, pt);
            return ptResult;
        }

        public static Aran.Omega.SettingsUI.SurfaceModel GetSurfaceModel(Aran.Panda.Constants.SurfaceType surfaceType)
        {
            var surfaceModel = (SurfaceModel)GlobalParams.Settings.OLSModelList.First(x =>
            {
                var sur = x as SurfaceModel;
                if (sur != null && sur.Surface == surfaceType)
                    return true;

                return false;
            });
            return surfaceModel;
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

        public static double GetDistance(Aran.Geometries.Point ptRefPt, Aran.Geometries.MultiPoint mltPoint)
        {
            var ptToSegmentDistance = ARANFunctions.PointToSegmentDistance(ptRefPt, mltPoint[0], mltPoint[1]);
            var minDistance = ptToSegmentDistance;
            for (var i = 1; i < mltPoint.Count - 1; i++)
            {
                ptToSegmentDistance = ARANFunctions.PointToSegmentDistance(ptRefPt, mltPoint[i],
                    mltPoint[i + 1]);
             
                if (ptToSegmentDistance < minDistance && Math.Abs(ptToSegmentDistance)>0.00001)
                    minDistance = ptToSegmentDistance;
            }
            return minDistance;
        }

        public static double GetDistance(Aran.Geometries.MultiPolygon multiPolygon, Aran.Geometries.MultiPoint mltPoint)
        {
            var distance = (from Polygon polygon in multiPolygon
                from Point pt in polygon.ExteriorRing
                select CommonFunctions.GetDistance(pt, mltPoint)).DefaultIfEmpty().Min();

            return distance;
        }

        public static double GetDistance(Aran.Geometries.MultiLineString multiLineString,
            Aran.Geometries.MultiPoint mltPoint)
        {
            var distance = (from LineString lineString in multiLineString
                from Point pt in lineString
                select CommonFunctions.GetDistance(pt, mltPoint)).DefaultIfEmpty().Min();

            return distance;
        }
    }
}
