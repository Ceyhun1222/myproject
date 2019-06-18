﻿using System;
using Aran.Geometries;
using Aran.Panda.Common;

namespace Aran.Omega.TypeB.Models
{
	public static class TransForm
	{
        #region RotateGeometry
        public static Geometry RotateGeometry(Point ptCenter, Geometry geom, double rad)
        {
            _rad = rad;
            switch (geom.Type)
            {
                case GeometryType.Point:
                    return RotatePoint(ptCenter, (Point)geom);

                case GeometryType.Vector:
                    return RotateVector((Vector)geom);

                case GeometryType.Line:
                    Line line = new Line();
                    line.RefPoint = RotatePoint(ptCenter, ((Line)geom).RefPoint);
                    line.DirVector = RotateVector(((Line)geom).DirVector);
                    return line;

                case GeometryType.LineString:
                    LineString lnString = new LineString();
                    lnString.AddMultiPoint(RotateMultiPoint(ptCenter, (MultiPoint)geom));
                    return lnString;

                case GeometryType.Ring:
                    Ring rng = new Ring();
                    rng.AddMultiPoint(RotateMultiPoint(ptCenter, (MultiPoint)geom));
                    return rng;

                case GeometryType.MultiPoint:
                    return RotateMultiPoint(ptCenter, (MultiPoint)geom);

                case GeometryType.MultiLineString:
                    return RotatePolyLine(ptCenter, (MultiLineString)geom);
                case GeometryType.MultiPolygon:
                    return RotateMultiPolygon(ptCenter, (MultiPolygon)geom);
                case GeometryType.Polygon:
                    return RotatePolygon(ptCenter, (Polygon)geom);
            }
            throw new Exception("Geometry Type is not implemented (in RotateGeometry)!");
        }

        private static Point RotatePoint(Point ptCenter, Point pt)
        {
            double dx = pt.X - ptCenter.X;
            double dy = pt.Y - ptCenter.Y;
            double x = ptCenter.X + dx * Math.Cos(_rad) - dy * Math.Sin(_rad);
            double y = ptCenter.Y + dx * Math.Sin(_rad) + dy * Math.Cos(_rad);
            Point result = new Point(x, y);
            return result;
        }

        private static MultiPoint RotateMultiPoint(Point ptCenter, MultiPoint geom)
        {
            MultiPoint result = new MultiPoint();
            for (int i = 0; i < geom.Count; i++)
            {
                result.Add(RotatePoint(ptCenter, geom[i]));
            }
            return result;
        }

        private static Polygon RotatePolygon(Point pntCenter, Polygon sourcePolygon)
        {
            Polygon result = new Polygon();
            foreach (Ring ring in sourcePolygon.InteriorRingList)
            {
                Ring rng = new Ring();
                rng.AddMultiPoint(RotateMultiPoint(pntCenter, ring));
                result.InteriorRingList.Add(rng);
                    //Add(rng);
            }
            result.ExteriorRing.AddMultiPoint(RotateMultiPoint(pntCenter, sourcePolygon.ExteriorRing));
            return result;
        }

        private static MultiPolygon RotateMultiPolygon(Point ptCenter, MultiPolygon complexGeo)
        {
            MultiPolygon resultMultiPolygon = new MultiPolygon();
            foreach (Polygon polygon in complexGeo)
            {
                Polygon resultPolygon = new Polygon();
                foreach (Ring ring in polygon.InteriorRingList)
                {
                    resultPolygon.InteriorRingList.Add((Ring)RotateMultiPoint(ptCenter, ring));
                }
                resultPolygon.ExteriorRing.AddMultiPoint((Ring)RotateMultiPoint(ptCenter, polygon.ExteriorRing));
                resultMultiPolygon.Add(resultPolygon);
            }
            return resultMultiPolygon;
        }

        private static MultiLineString RotatePolyLine(Point ptCenter, MultiLineString complexGeo)
        {
            MultiLineString result = new MultiLineString();
            for (int i = 0; i < complexGeo.Count; i++)
            {
                result.Add((LineString)RotateMultiPoint(ptCenter, complexGeo[i]));
            }
            return result;
        }

        private static Vector RotateVector(Vector vtr)
        {
            Vector result = new Vector();
            result.SetComponent(0, vtr.GetComponent(0) * Math.Cos(_rad) - vtr.GetComponent(1) * Math.Sin(_rad));
            result.SetComponent(1, vtr.GetComponent(0) * Math.Sin(_rad) + vtr.GetComponent(1) * Math.Cos(_rad));
            return result;
        }
        #endregion

        #region MoveGeometry
        public static Geometry Move(Geometry geom, Point fromPt, Point toPt)
        {
            double dx = toPt.X - fromPt.X;
            double dy = toPt.Y - fromPt.Y;

            switch (geom.Type)
            {

                case GeometryType.Point:
                    return MovePoint((Point)geom, dx, dy);

                case GeometryType.LineString:
                    LineString lnString = new LineString();
                    lnString.AddMultiPoint(MoveMultiPoint((MultiPoint)geom, dx, dy));
                    return lnString;

                case GeometryType.Ring:
                    Ring rng = new Ring();
                    rng.AddMultiPoint(MoveMultiPoint((MultiPoint)geom, dx, dy));
                    return rng;

                case GeometryType.MultiPoint:
                    return MoveMultiPoint((MultiPoint)geom, dx, dy);

                case GeometryType.MultiLineString:
                    return MovePolyLine((MultiLineString)geom, dx, dy);
                case GeometryType.Polygon:
                    return MovePolygon((Polygon)geom, dx, dy);
            }
            throw new Exception("Geometry Type is not implemented (in Move) !");
        }

        private static Point MovePoint(Point pt, double dX, double dY)
        {
            Point result = new Point(pt.X + dX, pt.Y + dY);
            return result;
        }

        private static MultiPoint MoveMultiPoint(MultiPoint geom, double dX, double dY)
        {
            MultiPoint result = new MultiPoint();
            for (int i = 0; i < geom.Count; i++)
            {
                result.Add(new Point(geom[i].X + dX, geom[i].Y + dY));
            }
            return result;
        }

        private static Polygon MovePolygon(Polygon geom, double dX, double dY)
        {
            Polygon result = new Polygon();
            for (int i = 0; i < geom.InteriorRingList.Count; i++)
            {
                result.InteriorRingList.Add((Ring)MoveMultiPoint(geom.InteriorRingList[i], dX, dY));
            }
            result.ExteriorRing.AddMultiPoint(MoveMultiPoint(geom.ExteriorRing, dX, dY));
            return result;
        }

        private static MultiLineString MovePolyLine(MultiLineString geom, double dX, double dY)
        {
            MultiLineString result = new MultiLineString();
            for (int i = 0; i < geom.Count; i++)
            {
                LineString lnString = new LineString();
                lnString.AddMultiPoint(MoveMultiPoint(geom[i], dX, dY));
                result.Add(lnString);
            }
            return result;
        }
        #endregion

        #region	 Flip
        public static Geometry Flip(Geometry geom, Point linePoint, double dirRadian)
        {
            switch (geom.Type)
            {

                case GeometryType.Point:
                    return FlipPoint((Point)geom, linePoint, dirRadian);

                case GeometryType.LineString:
                    LineString lnString = new LineString();
                    lnString.AddMultiPoint(FlipMultiPoint((MultiPoint)geom, linePoint, dirRadian));
                    return lnString;

                case GeometryType.Ring:
                    Ring rng = new Ring();
                    rng.AddMultiPoint(FlipMultiPoint((MultiPoint)geom, linePoint, dirRadian));
                    return rng;

                case GeometryType.MultiPoint:
                    return FlipMultiPoint((MultiPoint)geom, linePoint, dirRadian);

                case GeometryType.MultiLineString:
                    return FlipPolyLine((MultiLineString)geom, linePoint, dirRadian);
                case GeometryType.Polygon:
                    return FlipPolygon((Polygon)geom, linePoint, dirRadian);
            }
            throw new Exception("Geometry Type is not implemented ( in Flip)!");
        }

        private static Point FlipPoint(Point pt, Point linePoint, double dirRadian)
        {
            double distance;
            distance = ARANFunctions.PointToLineDistance(pt, linePoint, dirRadian);
            return ARANFunctions.LocalToPrj(pt, dirRadian, 0, -distance * 2);
        }

        private static MultiPoint FlipMultiPoint(MultiPoint geomMultiPoint, Point linePoint, double dirRadian)
        {
            MultiPoint result = new MultiPoint();
            double distance;
            for (int i = 0; i < geomMultiPoint.Count; i++)
            {
                distance = ARANFunctions.PointToLineDistance(geomMultiPoint[i], linePoint, dirRadian);
                result.Add(ARANFunctions.LocalToPrj(geomMultiPoint[i], dirRadian, 0, -distance * 2));
            }
            return result;
        }

        private static Polygon FlipPolygon(Polygon polygon, Point linePoint, double dirRadian)
        {
            Polygon result = new Polygon();
            foreach (Ring ring in polygon.InteriorRingList)
            {
                result.InteriorRingList.Add((Ring)FlipMultiPoint(ring, linePoint, dirRadian));
            }
            result.ExteriorRing = (Ring)FlipMultiPoint(polygon.ExteriorRing, linePoint, dirRadian);
            return result;
        }

        private static MultiLineString FlipPolyLine(MultiLineString polyLine, Point linePoint, double dirRadian)
        {
            MultiLineString result = new MultiLineString();
            for (int i = 0; i < polyLine.Count; i++)
            {
                result.Add((LineString)FlipMultiPoint(polyLine[i], linePoint, dirRadian));
            }
            return result;
        }
        #endregion

        #region QueryCoords

        public static GeoMinMaxPoint QueryCoords(Geometry geom)
        {
            _geoMinMaxPoint = new GeoMinMaxPoint();
            switch (geom.Type)
            {
                case GeometryType.Point:
                    _geoMinMaxPoint.AddMinMaxPoint((Point)geom, (Point)geom);
                    return _geoMinMaxPoint;
                case GeometryType.MultiPoint:
                case GeometryType.LineString:
                case GeometryType.Ring:
                    return PointsMinMax((MultiPoint)geom);
                case GeometryType.Polygon:
                    return MinMaxPolygon((Polygon)geom);
                case GeometryType.MultiLineString:
                    return MinMaxPolyline((MultiLineString)geom);
                case GeometryType.MultiPolygon:
                    MultiPolygon mPolygon = (MultiPolygon)geom;
                    if (mPolygon.Count==0)
                        return MinMaxPolygon(mPolygon[0]);
                    else
                        return PointsMinMax(mPolygon.ToMultiPoint());
                default:
                    throw new Exception("Geometry Type is not implemented ( in QueryCoords)");
            }
        }

        private static GeoMinMaxPoint MinMaxPolyline(MultiLineString geom)
        {
            _geoMinMaxPoint.XMin = geom[0][0].X;
            _geoMinMaxPoint.YMin = geom[0][0].Y;
            _geoMinMaxPoint.XMax = geom[0][0].X;
            _geoMinMaxPoint.YMax = geom[0][0].Y;
            for (int j = 0; j < geom.Count; j++)
            {
                for (int i = 1; i < geom[j].Count; i++)
                {
                    if (_geoMinMaxPoint.XMin > geom[i][j].X)
                        _geoMinMaxPoint.XMin = geom[i][j].X;
                    if (_geoMinMaxPoint.YMin > geom[i][j].Y)
                        _geoMinMaxPoint.YMin = geom[i][j].Y;
                    if (_geoMinMaxPoint.XMax < geom[i][j].X)
                        _geoMinMaxPoint.XMax = geom[i][j].X;
                    if (_geoMinMaxPoint.YMax < geom[i][j].Y)
                        _geoMinMaxPoint.YMax = geom[i][j].Y;
                }
            }
            return _geoMinMaxPoint;
        }

        private static GeoMinMaxPoint MinMaxPolygon(Polygon geom)
        {
            _geoMinMaxPoint.XMin = geom.ExteriorRing[0].X;
            _geoMinMaxPoint.YMin = geom.ExteriorRing[0].Y;
            _geoMinMaxPoint.XMax = geom.ExteriorRing[0].X;
            _geoMinMaxPoint.YMax = geom.ExteriorRing[0].Y;

            for (int j = 1; j < geom.ExteriorRing.Count; j++)
            {
                if (_geoMinMaxPoint.XMin > geom.ExteriorRing[j].X)
                    _geoMinMaxPoint.XMin = geom.ExteriorRing[j].X;
                if (_geoMinMaxPoint.YMin > geom.ExteriorRing[j].Y)
                    _geoMinMaxPoint.YMin = geom.ExteriorRing[j].Y;
                if (_geoMinMaxPoint.XMax < geom.ExteriorRing[j].X)
                    _geoMinMaxPoint.XMax = geom.ExteriorRing[j].X;
                if (_geoMinMaxPoint.YMax < geom.ExteriorRing[j].Y)
                    _geoMinMaxPoint.YMax = geom.ExteriorRing[j].Y;

            }
            
            return _geoMinMaxPoint;
        }
     
        private static GeoMinMaxPoint PointsMinMax(MultiPoint mPoint)
        {
            _geoMinMaxPoint.XMin = mPoint[0].X;
            _geoMinMaxPoint.YMin = mPoint[0].Y;
            _geoMinMaxPoint.XMax = mPoint[0].X;
            _geoMinMaxPoint.YMax = mPoint[0].Y;

            for (int i = 1; i < mPoint.Count; i++)
            {
                if (_geoMinMaxPoint.XMin > mPoint[i].X)
                    _geoMinMaxPoint.XMin = mPoint[i].X;
                if (_geoMinMaxPoint.YMin > mPoint[i].Y)
                    _geoMinMaxPoint.YMin = mPoint[i].Y;
                if (_geoMinMaxPoint.XMax < mPoint[i].X)
                    _geoMinMaxPoint.XMax = mPoint[i].X;
                if (_geoMinMaxPoint.YMax < mPoint[i].Y)
                    _geoMinMaxPoint.YMax = mPoint[i].Y;
            }
            return _geoMinMaxPoint;
        }

        #endregion

        private static double _rad;
        private static GeoMinMaxPoint _geoMinMaxPoint;
	}
}