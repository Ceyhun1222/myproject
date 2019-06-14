using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChartTypeA.Models;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using PDM;
using Aran.PANDA.Common;

namespace ChartTypeA
{
    class EsriFunctions
    {

        public const double EpsilonDistance = 0.001;
        public const double Epsilon_2Distance = EpsilonDistance * EpsilonDistance;
        public const double EpsilonDegree = 1.0 / 3600.0;
        public const double EpsilonRadian = EpsilonDegree / 360;
        public const double C_2_PI = 0.636619772367581343076;
        public const double C_PI = 3.1415926535897932384626433832795;
        public const double C_2xPI = 2 * C_PI;


        public static double ReturnDistanceAsMetr(IGeometry pt1, IGeometry pt2)
        {
            IProximityOperator proximtyOperator = pt1 as IProximityOperator;
            double distance = proximtyOperator.ReturnDistance(pt2);
            return distance;
        }

        public static bool IsInside(IGeometry geo1, IGeometry geo2)
        {
            IRelationalOperator relOper = geo1 as IRelationalOperator;
            return relOper.Contains(geo2);
        }

        public static IGeometry Intersect(IGeometry geo1, IGeometry geo2)
        {
            ITopologicalOperator2 topoOper = geo1 as ITopologicalOperator2;
            esriGeometryType geomType2 = geo2.GeometryType;
            if (topoOper != null)
            {
                if (geomType2 == esriGeometryType.esriGeometryPolyline)
                    return topoOper.Intersect(geo2, esriGeometryDimension.esriGeometry1Dimension);
                else 
                    return topoOper.Intersect(geo2, esriGeometryDimension.esriGeometry2Dimension);
            }
            return null;
        }

        public static IGeometry Cut(IGeometry geo1, IGeometry geo2)
        {
            ITopologicalOperator2 topoOper = geo2 as ITopologicalOperator2;
            esriGeometryType geomType2 = geo2.GeometryType;
            if (topoOper != null)
            {
                if (geomType2 == esriGeometryType.esriGeometryPolyline)
                    return topoOper.Intersect(geo2, esriGeometryDimension.esriGeometry1Dimension);
                else
                    return topoOper.Intersect(geo2, esriGeometryDimension.esriGeometry2Dimension);
            }
            return null;
        }

        public static IGeometry Buffer(IGeometry geom, double width)
        {
            IGeometry esriGeom = geom;
            ITopologicalOperator topoOper2 = (ITopologicalOperator)esriGeom;
           // SimplifyGeometry(esriGeom);
            IGeometry bufferGeom = topoOper2.Buffer(width);
            SimplifyGeometry(bufferGeom);
            return bufferGeom;
        }

        public static Boolean Disjoint(IGeometry geom, IGeometry other)
        {
            IGeometry esriGeom1 = geom;
            IGeometry esriGeom2 = other;
            IRelationalOperator relOper = (IRelationalOperator)esriGeom1;
            return relOper.Disjoint(esriGeom2);
        }

        public static IGeometry Union(IGeometry geom, IGeometry other)
        {
            IGeometry esriGeom1 = geom;
            IGeometry esriGeom2 = other;
            ITopologicalOperator2 relOper = (ITopologicalOperator2)esriGeom1;
            return relOper.Union(esriGeom2);
        }

        public static IGeometry Difference(IGeometry geom, IGeometry other)
        {
            IGeometry esriGeom1 = geom;
            IGeometry esriGeom2 = other;
            ITopologicalOperator2 relOper = (ITopologicalOperator2)esriGeom1;
            return relOper.Difference(esriGeom2);
        }

        public static void SimplifyGeometry(IGeometry geom)
        {
            ITopologicalOperator2 topoOper = geom as ITopologicalOperator2;
            topoOper.IsKnownSimple_2 = false;
            topoOper.Simplify();
        }

        public static IPoint PrjToLocal(IPoint center, double dirInRadian, IPoint ptPrj)
        {
            double SinA = Math.Sin(dirInRadian);
            double CosA = Math.Cos(dirInRadian);
            double dX = ptPrj.X - center.X;
            double dY = ptPrj.Y - center.Y;

            double Xnew = dX * CosA + dY * SinA;
            double Ynew = -dX * SinA + dY * CosA;

            return new Point {X = Xnew, Y = Ynew, Z = ptPrj.Z};
        }



        public static IPoint LocalToPrj(IPoint center, double dirInRadian, double x, double y = 0.0)
        {
            double SinA = Math.Sin(dirInRadian);
            double CosA = Math.Cos(dirInRadian);
            double Xnew = center.X + x * CosA - y * SinA;
            double Ynew = center.Y + x * SinA + y * CosA;
            return new Point{X=Xnew,Y=Ynew};
        }
        public static PlaneParam CalcPlaneParam(IPoint pt1, IPoint pt2, IPoint pt3)
        {
            PlaneParam fPlane = new PlaneParam();
            fPlane.C = pt1.X * (pt2.Y - pt3.Y) + pt2.X * (pt3.Y - pt1.Y) + pt3.X * (pt1.Y - pt2.Y);
            fPlane.A = (pt1.Y * (pt2.Z - pt3.Z) + pt2.Y * (pt3.Z - pt1.Z) + pt3.Y * (pt1.Z - pt2.Z)) / fPlane.C;
            fPlane.B = (pt1.Z * (pt2.X - pt3.X) + pt2.Z * (pt3.X - pt1.X) + pt3.Z * (pt1.X - pt2.X)) / fPlane.C;
            fPlane.D = (-(pt1.X * (pt2.Y * pt3.Z - pt3.Y * pt2.Z) + pt2.X * (pt3.Y * pt1.Z - pt1.Y * pt3.Z) + pt3.X * (pt1.Y * pt2.Z - pt2.Y * pt1.Z))) / fPlane.C;
            return fPlane;
        }

        public static long ReturnGeodesicAzimuth(IPoint point0, IPoint point1, out double directAzimuth, out double inverseAzimuth)
        {
            return NativeMethods.ReturnGeodesicAzimuth(point0.X, point0.Y, point1.X, point1.Y, out directAzimuth, out inverseAzimuth);
        }

        public static double ReturnAngleInRadians(IPoint pointFrom, IPoint pointTo)
        {
            return System.Math.Atan2(pointTo.Y - pointFrom.Y, pointTo.X - pointFrom.X);
        }

        public static double ReturnGeodesicDistance(IPoint pnt1, IPoint pnt2)
        {
            return NativeMethods.ReturnGeodesicDistance(pnt1.X, pnt1.Y, pnt2.X, pnt2.Y);
        }

        public static double FromDistanceVerticalM(UOM_DIST_HORZ uom, double source)
        {
            switch (uom)
            {
                case UOM_DIST_HORZ.FT:
                    return (source * 0.3048);

                case UOM_DIST_HORZ.M:
                    return source;

                case UOM_DIST_HORZ.NM:
                    return (source * 1852);

                case UOM_DIST_HORZ.KM:
                    return (source * 1000);

                default:
                    throw new Exception("UOM_DIST_HORZ is not implemented !");
            }
        }

        public static double FromHeightVerticalM(UOM_DIST_VERT uom, double source)
        {
            switch (uom)
            {
                case UOM_DIST_VERT.FT:
                    return (source * 0.3048);

                case UOM_DIST_VERT.M:
                    return source;

                case UOM_DIST_VERT.NM:
                    return (source * 1852);

                case UOM_DIST_VERT.KM:
                    return (source * 1000);

                default:
                    throw new Exception("UOM_DIST_HORZ is not implemented !");  
            }
        }

        public static void ChangeMapRotation(double rotationInRadian)
        {
            var map = GlobalParams.Map;

            var activeView = GlobalParams.HookHelper.FocusMap as IActiveView;

            var rotateInDeg = - ARANMath.RadToDeg(rotationInRadian);

            GlobalParams.RotateVal = ARANMath.DegToRad(rotateInDeg);

            ((IActiveView) GlobalParams.HookHelper.FocusMap).ScreenDisplay.DisplayTransformation.Rotation = rotateInDeg;

        }

        public static SideDirection SideDef(IPoint pointOnLine, double lineAngleInRadian, IPoint testPoint)
        {
           
            double fdY = testPoint.Y - pointOnLine.Y;
            double fdX = testPoint.X - pointOnLine.X;
            double fDist = fdY * fdY + fdX * fdX;

            if (fDist < Epsilon_2Distance)
                return SideDirection.sideOn;

            double Angle12 = Math.Atan2(fdY, fdX);
            double rAngle =ARANMath.Modulus(lineAngleInRadian - Angle12, C_2xPI);

            if ((rAngle < EpsilonRadian) || (Math.Abs(rAngle - C_PI) < EpsilonRadian))
                return SideDirection.sideOn;

            if (rAngle < ARANMath.C_PI)
                return SideDirection.sideRight;

            return SideDirection.sideLeft;
        }
    }
}
