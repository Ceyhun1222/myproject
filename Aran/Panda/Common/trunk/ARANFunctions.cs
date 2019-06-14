using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;

namespace Aran.PANDA.Common
{
	public static class ARANFunctions
	{
		//[StructLayout(LayoutKind.Explicit)]
		//private struct IntDouble
		//{
		//    [FieldOffset(0)]
		//    public double AsDouble;
		//    [FieldOffset(0)]
		//    public Int32 AsInteger0;
		//    [FieldOffset(4)]
		//    public Int32 AsInteger1;
		//    [FieldOffset(0)]
		//    public Int64 AsInt64;
		//}

		//[StructLayout(LayoutKind.Explicit)]
		//private struct IntSingle
		//{
		//    [FieldOffset(0)]
		//    public Single AsSingle;
		//    [FieldOffset(0)]
		//    public int AsInteger;
		//}

		#region "Converted old functions (not recomended for use!!!)"

		public static double ReturnDistanceInMeters(Point pointFrom, Point pointTo)
		{
			return ARANMath.Hypot(pointTo.X - pointFrom.X, pointTo.Y - pointFrom.Y);
		}

		public static double ReturnAngleInRadians(Point pointFrom, Point pointTo)
		{
			double result = System.Math.Atan2(pointTo.Y - pointFrom.Y, pointTo.X - pointFrom.X);
			if (result < 0.0)
				result += ARANMath.C_2xPI;
			return result;
		}

		public static double ReturnAngleInDegrees(Point pointFrom, Point pointTo)
		{
			return ARANMath.RadToDeg(System.Math.Atan2(pointTo.Y - pointFrom.Y, pointTo.X - pointFrom.X));
		}

		public static Point PointAlongPlane(Point ptOrigin, double dirRad, double dist)
		{
			double CosA = Math.Cos(dirRad);
			double SinA = Math.Sin(dirRad);
			return new Point(ptOrigin.X + dist * CosA, ptOrigin.Y + dist * SinA);
		}

		public static double Point2LineDistancePrj(Point refPoint, Point linePoint, double lineDirInRadian)
		{
			double CosA = Math.Cos(lineDirInRadian);
			double SinA = Math.Sin(lineDirInRadian);
			return System.Math.Abs((refPoint.Y - linePoint.Y) * CosA - (refPoint.X - linePoint.X) * SinA);
		}

		public static double Geometry2LineDistancePrj(Geometry geom, Point linePoint, double lineDirInRadian)
		{
			Aran.Geometries.MultiPoint pPoints = new MultiPoint();

			switch ((geom.Type))
			{
				case GeometryType.Point:
					pPoints.Add((Point)geom);
					break;
				case GeometryType.LineString:
					pPoints.AddMultiPoint((LineString)geom);
					break;
				case GeometryType.Polygon:
					pPoints.AddMultiPoint(((Polygon)geom).ExteriorRing);
					foreach (Ring pRing in ((Polygon)geom).InteriorRingList)
						pPoints.AddMultiPoint(pRing);
					break;
				case GeometryType.MultiPoint:
					pPoints.AddMultiPoint((MultiPoint)geom);
					break;
				case GeometryType.MultiLineString:
					foreach (LineString pLineString in (MultiLineString)geom)
						pPoints.AddMultiPoint(pLineString);
					break;
				case GeometryType.MultiPolygon:
					foreach (Polygon pPolygon in (MultiPolygon)geom)
					{
						pPoints.AddMultiPoint(pPolygon.ExteriorRing);
						foreach (Ring pRing in pPolygon.InteriorRingList)
							pPoints.AddMultiPoint(pRing);
					}
					break;
				case GeometryType.Ring:
					pPoints.AddMultiPoint((Ring)geom);
					break;
				case GeometryType.LineSegment:
					pPoints.Add(((LineSegment)geom).Start);
					pPoints.Add(((LineSegment)geom).End);
					break;

				case GeometryType.Null:
				case GeometryType.GeometryCollection:
				case GeometryType.SpecialGeometry:
				case GeometryType.Line:
				case GeometryType.Vector:
				default:
					return double.MaxValue;
			}

			double CosA = Math.Cos(lineDirInRadian);
			double SinA = Math.Sin(lineDirInRadian);
			double result = double.MaxValue;

			foreach (Point pt in pPoints)
			{
				double currDist = System.Math.Abs((pt.Y - linePoint.Y) * CosA - (pt.X - linePoint.X) * SinA);
				if (currDist < result)
					result = currDist;
			}

			return result;
		}

		#endregion

		#region "Ellipsoidal functions"

		/// <summary>
		/// Obsolete
		/// </summary>
		public static void InitEllipsoid() //WGS 84
		{
			NativeMethods.InitAll();
		}

		public static void InitEllipsoid(double EquatorialRadius, double InverseFlattening)
		{
			NativeMethods.InitEllipsoid(EquatorialRadius, InverseFlattening);
		}

		public static int CalculateInverseProblem(double x0, double y0, double x1, double y1, out double directAzimuth, out double inverseAzimuth)
		{
			return NativeMethods.InverseProblem(x0, y0, x1, y1, out directAzimuth, out inverseAzimuth);
		}

		public static int CalculateInverseProblem(Point pnt1, Point pnt2, out double directAzimuth, out double inverseAzimuth)
		{
			return NativeMethods.InverseProblem(pnt1.X, pnt1.Y, pnt2.X, pnt2.Y, out directAzimuth, out inverseAzimuth);
		}

		public static int ReturnGeodesicAzimuth(Point point0, Point point1, out double directAzimuth, out double inverseAzimuth)
		{
			return NativeMethods.ReturnGeodesicAzimuth(point0.X, point0.Y, point1.X, point1.Y, out directAzimuth, out inverseAzimuth);
		}

		public static double ReturnGeodesicDistance(Point pnt1, Point pnt2)
		{
			return NativeMethods.ReturnGeodesicDistance(pnt1.X, pnt1.Y, pnt2.X, pnt2.Y);
		}

		public static Point PointAlongGeodesic(Point point, double azimuth, double distance)
		{
			double resX, resY;
			NativeMethods.PointAlongGeodesic(point.X, point.Y, distance, azimuth, out resX, out resY);
			return new Point(resX, resY);
		}

		public static double AztToDirection(Point pointGeo, double azimuthInDeg, SpatialReference geoSR, SpatialReference prjSR)
		{
			Point pointToGeo = PointAlongGeodesic(pointGeo, azimuthInDeg, 10.0);
			GeometryOperators _geoOper = new GeometryOperators();

			Point pointFromPrj = (Point)_geoOper.GeoTransformations((Geometry)pointGeo, geoSR, prjSR);
			Point pointToPrj = (Point)_geoOper.GeoTransformations((Geometry)pointToGeo, geoSR, prjSR);
			return ReturnAngleInRadians(pointFromPrj, pointToPrj);
		}

		public static double DirToAzimuth(Point pointPrj, double dirInRadian, SpatialReference prjSR, SpatialReference geoSR)
		{
			Point pointToPrj = PointAlongPlane(pointPrj, dirInRadian, 10.0);
			GeometryOperators _geoOper = new GeometryOperators();

			Point pointFromGeo = (Point)_geoOper.GeoTransformations((Geometry)pointPrj, prjSR, geoSR);
			Point pointToGeo = (Point)_geoOper.GeoTransformations((Geometry)pointToPrj, prjSR, geoSR);

			double directAzumuth, inverseAzumuth;
			NativeMethods.ReturnGeodesicAzimuth(pointFromGeo.X, pointFromGeo.Y, pointToGeo.X, pointToGeo.Y, out directAzumuth, out inverseAzumuth);
			return directAzumuth;
		}

		#endregion

		/*
		void ShowError (Exception exc)
		{
			if( exc is EARANError )
				MessageDlg (exc.Message, EARANError (exc).ErrorType, [mbOK], 0);
			else
				ShowError (exc.Message);
		}

		GeometryOperators GeoOper();
		{
			return  _geoOper;
		}
		*/

		public static int RGB(int red = 0, int green = 0, int blue = 0)
		{
			return ((blue & 255) << 16) | ((green & 255) << 8) | (red & 255);
		}

		public static int GetRandomColor() //GRC
		{
			Random rnd = new Random();
			int red = rnd.Next(256);
			int green = rnd.Next(256);
			int blue = rnd.Next(256);

			return (blue << 16) | (green << 8) | red;
		}

		// ==============================================================================

		#region "Planar functions"

		public static Point LocalToPrj(Point center, double dirInRadian, double x, double y = 0.0)
		{
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double Xnew = center.X + x * CosA - y * SinA;
			double Ynew = center.Y + x * SinA + y * CosA;
			return new Point(Xnew, Ynew);
		}

		public static Point LocalToPrj(Point center, double dirInRadian, Point ptPrj)
		{
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double Xnew = center.X + ptPrj.X * CosA - ptPrj.Y * SinA;
			double Ynew = center.Y + ptPrj.X * SinA + ptPrj.Y * CosA;
			return new Point(Xnew, Ynew);
		}

		public static void LocalToPrj(Point center, double dirInRadian, double x, double y, Point result)
		{
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			result.X = center.X + x * CosA - y * SinA;
			result.Y = center.Y + x * SinA + y * CosA;
		}


		public static void LocalToPrj(Point center, double dirInRadian, Point ptPrj, Point result)
		{
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double x = ptPrj.X;
			double y = ptPrj.Y;
			result.X = center.X + x * CosA - y * SinA;
			result.Y = center.Y + x * SinA + y * CosA;
		}

		// ==================================
		public static Point LocalToPrj(Line axisLine, double x, double y = 0.0)
		{
			axisLine.DirVector.Length = 1.0;
			double CosA = axisLine.DirVector.GetComponent(0);
			double SinA = axisLine.DirVector.GetComponent(1);

			double Xnew = axisLine.RefPoint.X + x * CosA - y * SinA;
			double Ynew = axisLine.RefPoint.Y + x * SinA + y * CosA;
			return new Point(Xnew, Ynew);
		}

		public static Point LocalToPrj(Line axisLine, Point ptPrj)
		{
			axisLine.DirVector.Length = 1.0;
			double CosA = axisLine.DirVector.GetComponent(0);
			double SinA = axisLine.DirVector.GetComponent(1);

			double Xnew = axisLine.RefPoint.X + ptPrj.X * CosA - ptPrj.Y * SinA;
			double Ynew = axisLine.RefPoint.Y + ptPrj.X * SinA + ptPrj.Y * CosA;
			return new Point(Xnew, Ynew);
		}

		public static void LocalToPrj(Line axisLine, double x, double y, ref Point result)
		{
			axisLine.DirVector.Length = 1.0;
			double CosA = axisLine.DirVector.GetComponent(0);
			double SinA = axisLine.DirVector.GetComponent(1);

			result.X = axisLine.RefPoint.X + x * CosA - y * SinA;
			result.Y = axisLine.RefPoint.Y + x * SinA + y * CosA;
		}

		/*
		public static void LocalToPrj(Line axisLine, Point ptPrj, ref Point res)
		{
			axisLine.DirVector.Length = 1.0;
			double CosA = axisLine.DirVector.GetComponent(0);
			double SinA = axisLine.DirVector.GetComponent(1);
			double x = ptPrj.X;
			double y = ptPrj.Y;

			res.X = axisLine.RefPoint.X + x * CosA - y * SinA;
			res.Y = axisLine.RefPoint.Y + x * SinA + y * CosA;
		}
		*/

		// ==============================================================================

		public static Point PrjToLocal(Point center, double dirInRadian, double x, double y)
		{
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double dX = x - center.X;
			double dY = y - center.Y;
			double Xnew = dX * CosA + dY * SinA;
			double Ynew = -dX * SinA + dY * CosA;
			return new Point(Xnew, Ynew);
		}
		/*
		public static void PrjToLocal(Point center, double dirInRadian, double x, double y, ref Point res)
		{
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double dX = x - center.X;
			double dY = y - center.Y;

			res.X = dX * CosA + dY * SinA;
			res.Y = -dX * SinA + dY * CosA;
		}
		*/
		public static void PrjToLocal(Point center, double dirInRadian, double x, double y, out double resX, out double resY)
		{
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double dX = x - center.X;
			double dY = y - center.Y;

			resX = dX * CosA + dY * SinA;
			resY = -dX * SinA + dY * CosA;
		}

		public static Point PrjToLocal(Point center, double dirInRadian, Point ptPrj)
		{
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double dX = ptPrj.X - center.X;
			double dY = ptPrj.Y - center.Y;

			double Xnew = dX * CosA + dY * SinA;
			double Ynew = -dX * SinA + dY * CosA;

			return new Point(Xnew, Ynew, ptPrj.Z);
		}
		/*
		public static void PrjToLocal(Point center, double dirInRadian, Point ptPrj, ref Point res)
		{
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double dX = ptPrj.X - center.X;
			double dY = ptPrj.Y - center.Y;

			res.X = dX * CosA + dY * SinA;
			res.Y = -dX * SinA + dY * CosA;
		}
		*/
		public static void PrjToLocal(Point center, double dirInRadian, Point ptPrj, out double resX, out double resY)
		{
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double dX = ptPrj.X - center.X;
			double dY = ptPrj.Y - center.Y;

			resX = dX * CosA + dY * SinA;
			resY = -dX * SinA + dY * CosA;
		}

		// ==============================================================================
		public static Point PrjToLocal(Line axisLine, double x, double y)
		{
			axisLine.DirVector.Length = 1;
			double CosA = axisLine.DirVector.GetComponent(0);
			double SinA = axisLine.DirVector.GetComponent(1);

			double dX = x - axisLine.RefPoint.X;
			double dY = y - axisLine.RefPoint.Y;

			double Xnew = dX * CosA + dY * SinA;
			double Ynew = -dX * SinA + dY * CosA;
			return new Point(Xnew, Ynew);
		}

		public static Point PrjToLocal(Line axisLine, Point ptPrj)
		{
			axisLine.DirVector.Length = 1.0;
			double CosA = axisLine.DirVector.GetComponent(0);
			double SinA = axisLine.DirVector.GetComponent(1);

			double dX = ptPrj.X - axisLine.RefPoint.X;
			double dY = ptPrj.Y - axisLine.RefPoint.Y;

			double Xnew = dX * CosA + dY * SinA;
			double Ynew = -dX * SinA + dY * CosA;
			return new Point(Xnew, Ynew);
		}

		/*
		public static void PrjToLocal(Line axisLine, double x, double y, ref Point res)
		{
			axisLine.DirVector.Length = 1.0;
			double CosA = axisLine.DirVector.GetComponent(0);
			double SinA = axisLine.DirVector.GetComponent(1);

			double dX = x - axisLine.RefPoint.X;
			double dY = y - axisLine.RefPoint.Y;

			res.X = dX * CosA + dY * SinA;
			res.Y = -dX * SinA + dY * CosA;
		}
		*/

		public static void PrjToLocal(Line axisLine, double x, double y, out double resX, out double resY)
		{
			axisLine.DirVector.Length = 1.0;
			double CosA = axisLine.DirVector.GetComponent(0);
			double SinA = axisLine.DirVector.GetComponent(1);

			double dX = x - axisLine.RefPoint.X;
			double dY = y - axisLine.RefPoint.Y;

			resX = dX * CosA + dY * SinA;
			resY = -dX * SinA + dY * CosA;
		}
		/*
		public static void PrjToLocal(Line axisLine, Point ptPrj, ref Point res)
		{
			axisLine.DirVector.Length = 1.0;
			double CosA = axisLine.DirVector.GetComponent(0);
			double SinA = axisLine.DirVector.GetComponent(1);

			double dX = ptPrj.X - axisLine.RefPoint.X;
			double dY = ptPrj.Y - axisLine.RefPoint.Y;

			res.X = dX * CosA + dY * SinA;
			res.Y = -dX * SinA + dY * CosA;
		}
		*/
		public static void PrjToLocal(Line axisLine, Point ptPrj, out double resX, out double resY)
		{
			axisLine.DirVector.Length = 1.0;
			double CosA = axisLine.DirVector.GetComponent(0);
			double SinA = axisLine.DirVector.GetComponent(1);

			double dX = ptPrj.X - axisLine.RefPoint.X;
			double dY = ptPrj.Y - axisLine.RefPoint.Y;

			resX = dX * CosA + dY * SinA;
			resY = -dX * SinA + dY * CosA;
		}

		// ==============================================================================
		public static double PointToLineDistance(Point refPoint, Point linePoint, double lineDirInRadian)
		{
			double CosA = Math.Cos(lineDirInRadian);
			double SinA = Math.Sin(lineDirInRadian);
			return (refPoint.Y - linePoint.Y) * CosA - (refPoint.X - linePoint.X) * SinA;
		}

		public static double PointToLineDistance(Point refPoint, Line line)
		{
			double CosA = line.DirVector.GetComponent(0);
			double SinA = line.DirVector.GetComponent(0);

			return (refPoint.Y - line.RefPoint.Y) * CosA - (refPoint.X - line.RefPoint.X) * SinA;
		}

		//public static double PointToSegmentDistance(Point refPoint, Point segPoint1, Point segPoint2)
		//{
		//    double dx = segPoint2.X - segPoint1.X;
		//    double dy = segPoint2.Y - segPoint1.Y;

		//    double dx0 = segPoint1.X - refPoint.X;
		//    double dy0 = segPoint1.X - refPoint.X;

		//    double dx1 = segPoint2.X - refPoint.X;
		//    double dy1 = segPoint2.X - refPoint.X;

		//    double SegLen = ARANMath.Hypot(dy, dx);
		//    double dist0 = ARANMath.Hypot(dy0, dx0);
		//    double dist1 = ARANMath.Hypot(dy1, dx1);

		//    if (SegLen < ARANMath.EpsilonDistance)
		//    {
		//        if (dist0 < dist1)
		//            return dist0;
		//        else
		//            return dist1;
		//    }

		//    dx = refPoint.X - segPoint1.X;
		//    dy = refPoint.Y - segPoint1.Y;

		//    return ((refPoint.Y - segPoint2.Y) * dx - (refPoint.X - segPoint2.X) * dy) / SegLen;
		//}

		/// <summary>
		/// This method calculates the shortest distance between the given reference point and a segment with the given start and end points.
		/// </summary>
		/// <param name="ptRefPoint">The reference point</param>
		/// <param name="segPoint1">The start point of the segment</param>
		/// <param name="segPoint2">The end point of the segment</param>
		/// <returns>The shortest distance between the reference point and the segment</returns>
		public static double PointToSegmentDistance(Point ptRefPoint, Point segPoint1, Point segPoint2)
		{
			double xDelta = segPoint2.X - segPoint1.X;
			double yDelta = segPoint2.Y - segPoint1.Y;

			if ((Math.Abs(xDelta) < 0.1) && (Math.Abs(yDelta) < 0.1))
			{
				double xS = 0.5 * (segPoint2.X + segPoint1.X);
				double yS = 0.5 * (segPoint2.Y + segPoint1.Y);

				return ARANMath.Hypot(ptRefPoint.X - xS, ptRefPoint.Y - yS);
			}

			//u = (dx1 * dx + dy1 * dy) / seglen;
			double u = ((ptRefPoint.X - segPoint1.X) * xDelta + (ptRefPoint.Y - segPoint1.Y) * yDelta) / (xDelta * xDelta + yDelta * yDelta);

			//Aran.Geometries.Point closestPoint;
			double closestX, closestY;

			if (u <= 0.0)
			{
				closestX = segPoint1.X;
				closestY = segPoint1.Y;
			}
			else if (u >= 1.0)
			{
				closestX = segPoint2.X;
				closestY = segPoint2.Y;
			}
			else
			{
				closestX = segPoint1.X + u * xDelta;
				closestY = segPoint1.Y + u * yDelta;
			}

			return ARANMath.Hypot(closestX - ptRefPoint.X, closestY - ptRefPoint.Y);
		}

		/// <summary>
		/// This method calculates the shortest distance between the given reference point and the given line segment.
		/// </summary>
		/// <param name="ptRefPoint">The reference point</param>
		/// <param name="lineSegment">The line segment</param>
		/// <returns>The shortest distanse between the reference point and the line segment</returns>
		public static double PointToLineSegmentDistance(Point ptRefPoint, LineSegment lineSegment)
		{
			return PointToSegmentDistance(ptRefPoint, lineSegment.Start, lineSegment.End);
		}

		/// <summary>
		/// This method calculates the shortest distance between the given reference point and the given line string.
		/// </summary>
		/// <param name="ptRefPoint">The reference point</param>
		/// <param name="lineString">The line string</param>
		/// <returns>The shortest distance between the reference point and the line string</returns>
		public static double PointToLineStringDistance(Point ptRefPoint, LineString lineString)
		{
			double minDistance = double.MaxValue;

			for (int i = 0; i < lineString.Count - 1; i++)
			{
				double currDist = PointToSegmentDistance(ptRefPoint, lineString[i], lineString[i + 1]);
				if (currDist < minDistance)
					minDistance = currDist;
			}
			return minDistance;
		}

		static public Point FindRingLatestPoint(Ring ring, Point ptVector, double Direction, double OutDir, out double d, bool FindNearest = false)
		{           //bool TEST = false;
			d = double.NaN;
			int n = ring.Count;
			if (n < 2)
				return null;

			double bisect, fTmp = ARANMath.Modulus(Direction - OutDir, ARANMath.C_2xPI);

			if (fTmp > ARANMath.C_PI)
			{
				fTmp = ARANMath.Modulus(OutDir - Direction, ARANMath.C_2xPI);
				bisect = Direction + 0.5 * fTmp;
			}
			else
				bisect = OutDir + 0.5 * fTmp;

			double farDist = 0;

			for (int i = 0; i < n; i++)
			{
				Point PE = ring[i];
				double tmpDist = ARANMath.Sqr(ptVector.X - PE.X) + ARANMath.Sqr(ptVector.Y - PE.Y);
				if (tmpDist > farDist)
					farDist = tmpDist;
			}

			//if( TEST )
			//	farDist = 1 * (Math.Sqrt(farDist) + 1);
			//else

			farDist = Math.Sqrt(farDist) + 1;

			//farDist = 10 * (Math.Sqrt(farDist) + 1);

			Point farPT = LocalToPrj(ptVector, bisect, farDist, 0);

			//if( TEST )
			//	_UI.DrawPointWithText(farPT, 255, "farPT-" + cnt.ToString());

			double nearDist = ARANMath.Sqr(100.0 * farDist);
			Point result = new Point();

			for (int i = 0; i < n; i++)
			{
				Point PE = ring[i];
				double tmpDist = ARANMath.Sqr(farPT.X - PE.X) + ARANMath.Sqr(farPT.Y - PE.Y);
				if (tmpDist < nearDist)
				{
					nearDist = tmpDist;
					result.Assign(PE);
				}
			}

			//if( TEST )
			//{
			//    GUI.DrawPointWithText(Result, 255, 'Result-' + IntToStr(cnt));
			////	Inc(cnt);
			////	if cnt = 3 then
			//    {
			//        Polygon  polygon = new Polygon();
			//        polygon.Add(Ring);
			//        GUI.DrawPolygon(polygon, 255, sfsCross);
			//    }
			//}

			d = Math.Abs(PointToLineDistance(result, ptVector, Direction));
			return result;
		}

		// ==============================================================================

		public static Geometry LineLineIntersect(Point point1, double dirInRadian1, Point point2, double dirInRadian2)
		{
			Line line1 = new Line(point1, dirInRadian1);
			Line line2 = new Line(point2, dirInRadian2);
			return LineLineIntersect(line1, line2);
		}

		public static Geometry LineLineIntersect(Line line1, Line line2)
		{
			const double Eps = 0.0001;

			double cosF1 = line1.DirVector.GetComponent(0);
			double sinF1 = line1.DirVector.GetComponent(1);

			double cosF2 = line2.DirVector.GetComponent(0);
			double sinF2 = line2.DirVector.GetComponent(1);

			double d = sinF2 * cosF1 - cosF2 * sinF1;

			double Ua = cosF2 * (line1.RefPoint.Y - line2.RefPoint.Y) -
				  sinF2 * (line1.RefPoint.X - line2.RefPoint.X);

			double Ub = cosF1 * (line1.RefPoint.Y - line2.RefPoint.Y) -
				  sinF1 * (line1.RefPoint.X - line2.RefPoint.X);

			if (Math.Abs(d) < 10e-7)
			{
				if (System.Math.Abs(Ua) + System.Math.Abs(Ub) < 2.0 * Eps)
					return (Geometry)line1.Clone();

				return null;
			}

			double k = Ua / d;
			return new Point(line1.RefPoint.X + k * cosF1, line1.RefPoint.Y + k * sinF1);
		}

		public static Geometry LineLineIntersect(Point point1, double dirInRadian1, Point point2, double dirInRadian2, out bool isIntersect)
		{
			Line line1 = new Line(point1, dirInRadian1);
			Line line2 = new Line(point2, dirInRadian2);
			return LineLineIntersect(line1, line2, out isIntersect);
		}

		public static Geometry LineLineIntersect(Line line1, Line line2, out bool isIntersect)
		{
			const double Eps = 0.0001;

			double cosF1 = line1.DirVector.GetComponent(0);
			double sinF1 = line1.DirVector.GetComponent(1);

			double cosF2 = line2.DirVector.GetComponent(0);
			double sinF2 = line2.DirVector.GetComponent(1);

			double d = sinF2 * cosF1 - cosF2 * sinF1;

			double Ua = cosF2 * (line1.RefPoint.Y - line2.RefPoint.Y) -
				  sinF2 * (line1.RefPoint.X - line2.RefPoint.X);

			double Ub = cosF1 * (line1.RefPoint.Y - line2.RefPoint.Y) -
				  sinF1 * (line1.RefPoint.X - line2.RefPoint.X);

			isIntersect = false;

			if (Math.Abs(d) < Eps)
			{
				if (System.Math.Abs(Ua) + System.Math.Abs(Ub) < 2.0 * Eps)
					return (Geometry)line1.Clone();

				return null;
			}

			double k = Ua / d;

			Aran.Geometries.Point result = new Point(line1.RefPoint.X + k * cosF1, line1.RefPoint.Y + k * sinF1);

			double dirToIntersect1 = ReturnAngleInRadians(result, line1.RefPoint);
			double dirToIntersect2 = ReturnAngleInRadians(result, line2.RefPoint);

			isIntersect = (Math.Abs(line1.DirVector.Direction - dirToIntersect1) < Eps && (Math.Abs(line2.DirVector.Direction - dirToIntersect2) < Eps));

			return result;
		}


		//public static Point RingVectorIntersect(Ring ring, Line line, out double d)
		//{
		//    return RingVectorIntersect(ring, line, out d, false);
		//}

		public static Point RingVectorIntersect(Ring ring, Line line, out double d, bool findNearest = false)
		{
			List<Point> ptList = new List<Point>();
			ptList.AddRange(RemoveAgnails(ring));

			int n = ptList.Count;

			d = double.NaN;
			if (n < 2)
				return null;

			Point PE = ptList[0];

			double SinA = line.DirVector.GetComponent(1);
			double CosA = line.DirVector.GetComponent(0);
			double X1 = (PE.X - line.RefPoint.X) * CosA + (PE.Y - line.RefPoint.Y) * SinA;
			double Y1 = -(PE.X - line.RefPoint.X) * SinA + (PE.Y - line.RefPoint.Y) * CosA;

			bool HaveIntersection = false;
			Point result = null;

			for (int i = 1; i <= n + 1; i++)
			{
				double X0 = X1, Y0 = Y1;

				//int j = i & (0 - Convert.ToInt32(i < n));
				int j = i < n ? i : 0;

				PE = ptList[j];
				X1 = (PE.X - line.RefPoint.X) * CosA + (PE.Y - line.RefPoint.Y) * SinA;
				Y1 = -(PE.X - line.RefPoint.X) * SinA + (PE.Y - line.RefPoint.Y) * CosA;

				if ((Y0 * Y1 > 0) || ((X0 < 0) && (X1 < 0)))
					continue;

				double dXE = X1 - X0, dYE = Y1 - Y0, x;

				if (System.Math.Abs(dYE) < ARANMath.EpsilonDistance)
					x = X0;
				else
					x = X0 - Y0 * dXE / dYE;

				if ((!HaveIntersection) || (findNearest && (x < d)) || ((!findNearest) && (x > d)))
					d = x;

				HaveIntersection = true;
			}

			if (HaveIntersection)
				result = LocalToPrj(line, d);
			return result;
		}

		//public static Point RingVectorIntersect(Ring ring, Point ptVector, double direction, out  double d)
		//{
		//    return RingVectorIntersect(ring, ptVector, direction, out  d, false);
		//}

		public static Point RingVectorIntersect(Ring ring, Point ptVector, double direction, out double d, bool findNearest = false)
		{
			Line line = new Line(ptVector, direction);
			return RingVectorIntersect(ring, line, out d, findNearest);
		}

		public static Point PolygonVectorIntersect(Polygon polygon, Line line, out double d)
		{
			return PolygonVectorIntersect(polygon, line, out d, false);
		}

		public static Point PolygonVectorIntersect(Polygon polygon, Line line, out double d, bool findNearest)
		{
			d = double.NaN;

			if (polygon.IsEmpty)
				return null;

			Point result = RingVectorIntersect(polygon.ExteriorRing, line, out d, findNearest);

			foreach (Ring rng in polygon.InteriorRingList)
			{
				double Dist;
				Point ptTmp = RingVectorIntersect(rng, line, out Dist, findNearest);

				if (ptTmp != null)
				{
					if ((result == null) || (findNearest && (Dist < d)) || ((Dist > d) && (!findNearest)))
					{
						result = ptTmp;
						d = Dist;
					}
				}
			}

			return result;
		}

		public static Point PolygonVectorIntersect(Polygon polygon, Point ptVector, double direction, out double d)
		{
			return PolygonVectorIntersect(polygon, ptVector, direction, out d, false);
		}

		public static Point PolygonVectorIntersect(Polygon polygon, Point ptVector, double direction, out double d, bool findNearest)
		{
			Line line = new Line(ptVector, direction);
			return PolygonVectorIntersect(polygon, line, out d, findNearest);
		}


		/// <summary>
		/// Calculates the intersection points of two overlapping circles.
		/// Returns true if the circles intersect.
		/// Returns false if the circles do not intersect.
		/// </summary>
		public static int FindCircleCircleIntersections(
										 double c0x, double c0y,            //Circle 0 Center
										 double r0,                         //Circle 0 Radius

										 double c1x, double c1y,            //Circle 1 Center
										 double r1,                         //Circle 1 Radius

										 out double i0x, out double i0y,    //Intersection Point 1
										 out double i1x, out double i1y)    //Intersection Point 2
		{
			double dx = c1x - c0x;
			double dy = c1y - c0y;
			double d = Math.Sqrt(dx * dx + dy * dy);        // Distance between centers

			i0x = i0y = i1x = i1y = 0;
			// Circles share centers. This results in division by zero,
			// infinite solutions or one circle being contained within the other.
			if (d == 0.0)   // < Eps
				return 0;

			//Circles do not touch each other
			if (d > r0 + r1)
				return 0;

			//One circle is contained within the other
			if (d < Math.Abs(r0 - r1))
				return 0;

			/// <proof>
			/// Considering the two right triangles c0 i0 i1 and c1 i0 i1 we can write:
			/// a^2 + h^2 = r0^2 and b^2 + h^2 = r1^2

			/// Remove h^2 from the equation by setting them equal to themselves:
			/// r0^2 - a^2 = r1^2 - b^2

			/// Substitute b with (d - a) since it is proven that d = a + b:
			/// r0^2 - a^2 = r1^2 - (d - a)^2

			/// Complete the square:
			/// r0^2 - a^2 = r1^2 - (d^2 -2da + a^2)

			/// Subtract r1^2 from both sides:
			/// r0^2 - r1^2 - a^2 = -(d^2 -2da + a^2)

			/// Invert the signs:
			/// r0^2 - r1^2 - a^2 = -d^2 + 2da - a^2

			/// Adding a^2 to each side cancels them out:
			/// r0^2 - r1^2 = -d^2 + 2da

			/// Add d^2 to both sides to shift it to the other side:
			/// r0^2 - r1^2 + d^2 = 2da

			/// Divide by 2d to finally solve for a:
			/// a = (r0^2 - r1^2 + d^2)/ (2d)
			/// </proof>

			double invDist = 1.0 / d;
			double a = 0.5 * invDist * (r0 * r0 - r1 * r1 + d * d);
			//Solve for h by substituting a into a^2 + h^2 = r0^2
			double h = Math.Sqrt(r0 * r0 - a * a);

			// Find slopes.
			double sx = dx * invDist;
			double sy = dy * invDist;

			//Find point p2 by adding the a offset in relation to line d to point p0
			double px2 = c0x + a * sx;
			double py2 = c0y + a * sy;

			//Tangent circles have only one intersection
			if (d == r0 + r1)
			{
				i0x = i1y = px2;
				i0y = i1y = py2;
				return 1;
			}

			//Get the perpendicular slope by multiplying by the negative reciprocal
			//Then multiply by the h offset in relation to d to get the actual offsets
			double mx = h * sy;
			double my = h * sx;

			//Add the offsets to point p2 to obtain the intersection points
			i0x = px2 - mx; i0y = py2 + my;
			i1x = px2 + mx; i1y = py2 - my;

			return 2;
		}

		/// <summary>
		/// Calculates the intersection points of two overlapping circles.
		/// Returns true if the circles intersect.
		/// Returns false if the circles do not intersect.
		/// </summary>
		public static int FindCircleCircleIntersections(
														double c0x, double c0y,     //Circle 0 Center
														double r0,                  //Circle 0 Radius

														double c1x, double c1y,     //Circle 1 Center
														double r1,                  //Circle 1 Radius

														out Point i0,               //Intersection Point
														out Point i1)               //Intersection Point
		{
			// Find the distance between the centers.
			double dx = c1x - c0x;
			double dy = c1y - c0y;
			double dist = Math.Sqrt(dx * dx + dy * dy);

			i0 = i1 = null;

			// See how manhym solutions there are.

			// Circles share centers. This results in division by zero,
			// infinite solutions or one circle being contained within the other.
			if (dist == 0.0)    // < Eps
				return 0;

			// No solutions, the circles coincide.
			//if ((dist == 0) && (radius0 == radius1))
			//	return 0;

			// No solutions, the circles are too far apart.
			// Circles do not touch each other
			if (dist > r0 + r1)
				return 0;

			// No solutions, one circle contains the other.
			// One circle is contained within the other
			if (dist < Math.Abs(r0 - r1))
				return 0;

			/// <proof>
			/// Considering the two right triangles c0 i0 i1 and c1 i0 i1 we can write:
			/// a^2 + h^2 = r0^2 and b^2 + h^2 = r1^2

			/// Remove h^2 from the equation by setting them equal to themselves:
			/// r0^2 - a^2 = r1^2 - b^2

			/// Substitute b with (d - a) since it is proven that d = a + b:
			/// r0^2 - a^2 = r1^2 - (d - a)^2

			/// Complete the square:
			/// r0^2 - a^2 = r1^2 - (d^2 -2da + a^2)

			/// Subtract r1^2 from both sides:
			/// r0^2 - r1^2 - a^2 = -(d^2 -2da + a^2)

			/// Invert the signs:
			/// r0^2 - r1^2 - a^2 = -d^2 + 2da - a^2

			/// Adding a^2 to each side cancels them out:
			/// r0^2 - r1^2 = -d^2 + 2da

			/// Add d^2 to both sides to shift it to the other side:
			/// r0^2 - r1^2 + d^2 = 2da

			/// Divide by 2d to finally solve for a:
			/// a = (r0^2 - r1^2 + d^2)/ (2d)
			/// </proof>

			// Find a and h.
			double invDist = 1.0 / dist;
			double a = 0.5 * invDist * (r0 * r0 - r1 * r1 + dist * dist);
			//Solve for h by substituting a into a^2 + h^2 = r0^2
			double h = Math.Sqrt(r0 * r0 - a * a);

			// Find slopes.
			double sx = dx * invDist;
			double sy = dy * invDist;

			// Find P2.
			// Find point p2 by adding the a offset in relation to line d to point p0
			double cx2 = c0x + a * sx;
			double cy2 = c0y + a * sy;

			i0 = new Aran.Geometries.Point();
			i1 = new Aran.Geometries.Point();

			// Get the points P3.
			// See if we have 1 or 2 solutions.
			if (dist == r0 + r1)
			{
				i0.X = i1.X = cx2;
				i0.Y = i1.Y = cy2;
				return 1;
			}

			double mx = h * sx;
			double my = h * sx;

			i0.X = cx2 + my; i0.Y = cy2 - mx;
			i1.X = cx2 - my; i1.Y = cy2 + mx;

			return 2;
		}

		public static int FindCircleCircleIntersections(
										 Point c0,                      //Circle 0 Center
										 double r0,                     //Circle 0 Radius

										 Point c1,                      //Circle 1 Center
										 double r1,                     //Circle 1 Radius

										 out Point i0,                  //Intersection Point
										 out Point i1)                  //Intersection Point
		{
			double dx = c1.X - c0.X;
			double dy = c1.Y - c0.Y;
			double d = Math.Sqrt(dx * dx + dy * dy);        // Distance between centers

			i1 = i0 = null;

			// Circles share centers. This results in division by zero,
			// infinite solutions or one circle being contained within the other.
			if (d == 0.0)   // < Eps
				return 0;

			//Circles do not touch each other
			if (d > r0 + r1)
				return 0;

			//One circle is contained within the other
			if (d < Math.Abs(r0 - r1))
				return 0;

			/// <proof>
			/// Considering the two right triangles c0 i0 i1 and c1 i0 i1 we can write:
			/// a^2 + h^2 = r0^2 and b^2 + h^2 = r1^2

			/// Remove h^2 from the equation by setting them equal to themselves:
			/// r0^2 - a^2 = r1^2 - b^2

			/// Substitute b with (d - a) since it is proven that d = a + b:
			/// r0^2 - a^2 = r1^2 - (d - a)^2

			/// Complete the square:
			/// r0^2 - a^2 = r1^2 - (d^2 -2da + a^2)

			/// Subtract r1^2 from both sides:
			/// r0^2 - r1^2 - a^2 = -(d^2 -2da + a^2)

			/// Invert the signs:
			/// r0^2 - r1^2 - a^2 = -d^2 + 2da - a^2

			/// Adding a^2 to each side cancels them out:
			/// r0^2 - r1^2 = -d^2 + 2da

			/// Add d^2 to both sides to shift it to the other side:
			/// r0^2 - r1^2 + d^2 = 2da

			/// Divide by 2d to finally solve for a:
			/// a = (r0^2 - r1^2 + d^2)/ (2d)
			/// </proof>

			double invDist = 1.0 / d;
			double a = 0.5 * invDist * (r0 * r0 - r1 * r1 + d * d);
			//Solve for h by substituting a into a^2 + h^2 = r0^2
			double h = Math.Sqrt(r0 * r0 - a * a);

			// Find slopes.
			double sx = dx * invDist;
			double sy = dy * invDist;

			//Find point p2 by adding the a offset in relation to line d to point p0
			double px2 = c0.X + a * sx;
			double py2 = c0.Y + a * sy;

			i0 = new Point();
			i1 = new Point();

			//Tangent circles have only one intersection
			if (d == r0 + r1)
			{
				i0.X = i1.X = px2;
				i0.Y = i1.Y = py2;
				return 1;
			}

			//Get the perpendicular slope by multiplying by the negative reciprocal
			//Then multiply by the h offset in relation to d to get the actual offsets
			double mx = h * sy;
			double my = h * sx;

			//Add the offsets to point p2 to obtain the intersection points
			i0.X = px2 - mx; i0.Y = py2 + my;
			i1.X = px2 + mx; i1.Y = py2 - my;

			return 2;
		}

		/// <summary>
		/// Calculates the tangent points on circle from a given point.
		/// Returns true if the given point lies outside the circle.
		/// Returns false if the given point is inside the circle.
		/// </summary>
		public static int getCircleTangentPoints(
								   double cr, double cx, double cy,     //Circle Radius and Center
								   double px, double py,                //Point to determine tangency
								   out double tx0, out double ty0,      //Tangent Point 0
								   out double tx1, out double ty1)      //Tangent Point 1
		{
			double dx = px - cx;
			double dy = py - cy;
			double hyp = Math.Sqrt(dx * dx + dy * dy); //Distance to center of circle

			//Point is inside the circle
			if (hyp < cr)
			{
				tx0 = tx1 = ty0 = ty1 = 0;
				return 0;
			}

			//Point is lies on the circle, so there is only one tangent point
			if (hyp == cr)
			{
				tx0 = tx1 = px;
				ty0 = ty1 = py;
				return 1;
			}

			//Since the tangent lines are always perpendicular to the radius, so
			//we can use the Pythagorean theorem to solve for the missing side
			double pr = Math.Sqrt(hyp * hyp - cr * cr);

			return FindCircleCircleIntersections(cx, cy, cr, px, py, pr, out tx0, out ty0, out tx1, out ty1);
		}

		public static int getCircleTangentPoints(
								   double circleRadius, Point circleCenter,     //Circle Radius and Center
								   Point pointToDetermine,                      //Point to determine tangency
								   out Point tangentPoint0,                     //Tangent Point 0
								   out Point tangentPoint1)                     //Tangent Point 1
		{
			double resultPointForX = pointToDetermine.X - circleCenter.X;
			double resultPointForY = pointToDetermine.Y - circleCenter.Y;

			double distanceToCenter = Math.Sqrt(resultPointForX * resultPointForX + resultPointForY * resultPointForY); //Distance to center of circle

			//Point is inside the circle
			if (distanceToCenter < circleRadius)
			{
				tangentPoint0 = tangentPoint1 = null;
				return 0;
			}

			//Point is lies on the circle, so there is only one tangent point
			if (distanceToCenter == circleRadius)
			{
				tangentPoint0 = new Point();
				tangentPoint1 = new Point();
				tangentPoint0.X = tangentPoint1.X = pointToDetermine.X;
				tangentPoint0.Y = tangentPoint1.Y = pointToDetermine.Y;
				return 1;
			}

			//Since the tangent lines are always perpendicular to the radius, so
			//we can use the Pythagorean theorem to solve for the missing side
			double tangent = Math.Sqrt(distanceToCenter * distanceToCenter - circleRadius * circleRadius);

			int result = FindCircleCircleIntersections(circleCenter, circleRadius, pointToDetermine, tangent, out tangentPoint0, out tangentPoint1);

			return result;
		}

		public static int CutRingByLine(Ring ring, LineString nNLine, out Ring leftRing, out Ring rightRing)
		{
			Polygon polygon = new Polygon();
			polygon.ExteriorRing = ring;
			MultiPolygon mPolyIn = new MultiPolygon();
			mPolyIn.Add(polygon);

			try
			{
				Geometry GeomLeft, GeomRight;
				GeometryOperators gOper = new GeometryOperators();
				gOper.Cut(mPolyIn, nNLine, out GeomLeft, out GeomRight);

				MultiPolygon LeftMPoly = (MultiPolygon)GeomLeft;
				polygon = LeftMPoly[0];
				leftRing = polygon.ExteriorRing;

				MultiPolygon RightMPoly = (MultiPolygon)GeomRight;
				polygon = RightMPoly[0];
				rightRing = polygon.ExteriorRing;
				return 2;
			}
			catch
			{

				leftRing = rightRing = null;
				return 0;
			}
		}

		public static int CutRingByNNLine(Ring ring, LineString nNLine, out Ring leftRing, out Ring rightRing)
		{
			int n = ring.Count;

			leftRing = null;
			rightRing = null;

			if (n < 2)
				return 0;

			Point ptCenter = nNLine[1];

			double dX = nNLine[0].X - ptCenter.X;
			double dY = nNLine[0].Y - ptCenter.Y;

			double Len = Math.Sqrt(dY * dY + dX * dX);
			double K = 1.0 / Len;

			double SinA = dY * K;
			double CosA = dX * K;

			dX = nNLine[2].X - ptCenter.X;
			dY = nNLine[2].Y - ptCenter.Y;

			Len = Math.Sqrt(dY * dY + dX * dX);
			K = 1.0 / Len;

			double SinB = dY * K;
			double CosB = dX * K;

			Point PE = ring[0];

			double X1A = (PE.X - ptCenter.X) * CosA + (PE.Y - ptCenter.Y) * SinA;
			double Y1A = -(PE.X - ptCenter.X) * SinA + (PE.Y - ptCenter.Y) * CosA;

			double X1B = (PE.X - ptCenter.X) * CosB + (PE.Y - ptCenter.Y) * SinB;
			double Y1B = -(PE.X - ptCenter.X) * SinB + (PE.Y - ptCenter.Y) * CosB;

			int IxA = -1;
			int IxB = -1;

			double X0A, Y0A, X0B, Y0B;
			double dA = 0, dB = 0;  // for remove compiler warning;

			for (int i = 0; i <= n; i++)
			{
				X0A = X1A;
				Y0A = Y1A;

				X0B = X1B;
				Y0B = Y1B;

				//int j = (i + 1) & (0 - Convert.ToInt32(i + 1 < n));
				int j = i + 1 < n ? (i + 1) : 0;

				PE = ring[j];

				X1A = (PE.X - ptCenter.X) * CosA + (PE.Y - ptCenter.Y) * SinA;
				Y1A = -(PE.X - ptCenter.X) * SinA + (PE.Y - ptCenter.Y) * CosA;

				X1B = (PE.X - ptCenter.X) * CosB + (PE.Y - ptCenter.Y) * SinB;
				Y1B = -(PE.X - ptCenter.X) * SinB + (PE.Y - ptCenter.Y) * CosB;

				double dXE, dYE, x;

				if ((Y0A * Y1A < 0) && ((X0A > 0) || (X1A > 0)))
				{
					dXE = X1A - X0A;
					dYE = Y1A - Y0A;

					if (System.Math.Abs(dYE) < ARANMath.Epsilon)
						x = X0A;    //	0.5 * (X1A + X0A);	//	
					else
						x = X0A - Y0A * dXE / dYE;

					if (x > 0 && (IxA < 0 || x < dA))
					{
						dA = x;
						IxA = i;
					}
				}

				if ((Y0B * Y1B < 0) && (X0B > 0 || X1B > 0))
				{
					dXE = X1B - X0B;
					dYE = Y1B - Y0B;

					if (System.Math.Abs(dYE) < ARANMath.Epsilon)
						x = X0B;    //	0.5 * (X1B + X0B);	//
					else
						x = X0B - Y0B * dXE / dYE;

					if (x > 0 && (IxB < 0 || x < dB))
					{
						dB = x;
						IxB = i;
					}
				}
			}

			if ((IxA >= 0) && (IxB >= 0))
			{
				X0A = ptCenter.X + dA * CosA;
				Y0A = ptCenter.Y + dA * SinA;

				X0B = ptCenter.X + dB * CosB;
				Y0B = ptCenter.Y + dB * SinB;

				if (IxA == IxB)
				{
					//TO DO :::
					return 2;
				}

				leftRing = new Ring();
				rightRing = new Ring();

				Point pt0 = new Point(X0A, Y0A);
				Point pt1 = new Point(X0B, Y0B);

				int m = (IxA - IxB) % n;
				if (m <= 0)
					m += n;

				rightRing.Add(pt0);
				rightRing.Add(ptCenter);
				rightRing.Add(pt1);

				for (int i = 1; i <= m; i++)
				{
					int j = (i + IxB) % n;
					rightRing.Add(ring[j]);
				}

				m = n - m;

				//	m = (IxB - IxA) % n;
				//	if(m < 0) m += n;

				leftRing.Add(pt1);
				leftRing.Add(ptCenter);
				leftRing.Add(pt0);

				for (int i = 1; i <= m; i++)
				{
					int j = (i + IxA) % n;
					leftRing.Add(ring[j]);
				}
				return 2;
			}
			return 0;
		}

		public static double SpiralTouchToPoint(Point ptCnt, double r0, double coef,
			double EntryDir, TurnDirection turnSide, Point ptDst)
		{
			double result = 9999.0;
			double fTurn = -(int)turnSide;
			double SpStartRadial = ARANMath.Modulus(EntryDir + (ARANMath.C_PI_2 - Math.Atan2(coef, r0)) * fTurn, ARANMath.C_2xPI);

			double dX = ptDst.X - ptCnt.X;
			double dY = ptDst.Y - ptCnt.Y;

			double DirToPnt = Math.Atan2(dY, dX);
			double DistToPnt = ARANMath.Hypot(dY, dX);

			double SpAngle = ARANMath.Modulus((SpStartRadial - DirToPnt) * fTurn, ARANMath.C_2xPI);
			if (SpAngle > Math.PI && r0 == 0.0) SpAngle = SpAngle - ARANMath.C_2xPI;

			double R = r0 + coef * SpAngle;

			if (Math.Abs(R - DistToPnt) < ARANMath.EpsilonDistance)
				return SpAngle;

			if (R > DistToPnt) return result;

			double fTmp = DirToPnt;

			for (int i = 0; i < 30; i++)
			{
				double Phi = fTmp;
				SpAngle = SpiralTouchAngle(r0, coef, SpStartRadial, EntryDir, Phi, turnSide);

				R = r0 + coef * SpAngle;
				fTmp = SpStartRadial - SpAngle * fTurn;

				double Xsp = ptCnt.X + R * Math.Cos(fTmp);
				double Ysp = ptCnt.Y + R * Math.Sin(fTmp);

				fTmp = Math.Atan2(ptDst.Y - Ysp, ptDst.X - Xsp);
				double dPhi = Math.Abs(Phi - fTmp);

				if (dPhi < ARANMath.EpsilonRadian)
					return SpAngle;
			}

			return result;
		}

		public static double SpiralTouchAngle(double r0, double coeff, double SpStartRadial, double StartTouch, double EndTouch, TurnDirection turnDir)
		{
			double fTurnSide = (int)turnDir;
			EndTouch = ARANMath.Modulus(EndTouch, ARANMath.C_2xPI);
			double turnAngle = ARANMath.Modulus((EndTouch - StartTouch) * fTurnSide, ARANMath.C_2xPI);

			//====================================================================
			//double touchAngle = turnAngle;
			//for (int i = 0; i < 10; i++)
			//{
			//	double d = coeff / (r0 + coeff * turnAngle);
			//	double delta = (turnAngle - touchAngle - Math.Atan(d)) / (2 - 1 / (d * d + 1));
			//	turnAngle -= delta;
			//	if (System.Math.Abs(delta) < ARANMath.EpsilonRadian)
			//		break;
			//}

			//return ARANMath.Modulus(turnAngle, ARANMath.C_2xPI);

			if (turnAngle < ARANMath.EpsilonRadian)
				return turnAngle;
			//====================================================================

			for (int i = 0; i < 10; i++)
			{
				double rEnd = r0 + coeff * turnAngle;

				double EndTouchAppr = ARANMath.Modulus(SpStartRadial + (turnAngle + ARANMath.C_PI_2 - Math.Atan2(coeff, rEnd)) * fTurnSide, ARANMath.C_2xPI);
				double delta = (EndTouchAppr - EndTouch) * fTurnSide;
				if (delta < -Math.PI || delta > Math.PI)
					delta = ARANMath.Modulus(delta, ARANMath.C_2xPI);

				//turnAngle -= delta;
				turnAngle = ARANMath.Modulus(turnAngle - delta, ARANMath.C_2xPI);

				if (Math.Abs(delta) < ARANMath.EpsilonRadian)
					break;
			}

			return ARANMath.Modulus(turnAngle, ARANMath.C_2xPI);
		}

		public static double SpiralTouchAngle(double r0, double coeff, double nominalDir, double touchDir, SideDirection turnDir)
		{
			double touchAngle = ARANMath.Modulus((touchDir - nominalDir) * (int)(turnDir), ARANMath.C_2xPI);
			double turnAngle = touchAngle;

			for (int i = 0; i <= 9; i++)
			{
				double d = coeff / (r0 + coeff * turnAngle);
				double delta = (turnAngle - touchAngle - Math.Atan(d)) / (2 - 1 / (d * d + 1));
				turnAngle = turnAngle - delta;
				if (System.Math.Abs(delta) < ARANMath.EpsilonRadian)
					break;
			}

			return ARANMath.Modulus(turnAngle, ARANMath.C_2xPI);
		}

		public static MultiPoint CreateWindSpiral(Point pnt, double radianNominalDir, double radianStartDir, double radianEndDir, double startRadius, double coefficient, SideDirection turnSide)
		{
			if (ARANMath.SubtractAngles(radianNominalDir, radianEndDir) < ARANMath.EpsilonRadian)
				radianEndDir = radianNominalDir;

			int TurnDir = (int)turnSide;
			double startDphi = ARANMath.Modulus(TurnDir * (radianStartDir - radianNominalDir), ARANMath.C_2xPI);

			if (startDphi < ARANMath.EpsilonRadian)
				startDphi = 0.0;
			else
				startDphi = SpiralTouchAngle(startRadius, coefficient,
														radianNominalDir, radianStartDir, turnSide);

			double endDphi = SpiralTouchAngle(startRadius, coefficient, radianNominalDir, radianEndDir, turnSide);
			double TurnAng = endDphi - startDphi;
			double azt0 = ARANMath.Modulus(radianNominalDir - TurnDir * (startDphi - ARANMath.C_PI_2), ARANMath.C_2xPI);

			MultiPoint result = new MultiPoint();

			if (TurnAng > 0)
			{
				int n = (int)Math.Floor(ARANMath.RadToDeg(TurnAng));

				if (n <= 1) n = 1;
				else if (n <= 5) n = 5;
				else if (n < 10) n = 10;
				double dAlpha = TurnAng / n;

				Point PtCnt = PointAlongPlane(pnt, radianNominalDir + TurnDir * ARANMath.C_PI_2, startRadius);   //??????????

				for (int i = 0; i <= n; i++)
				{
					double R = startRadius + (ARANMath.RadToDeg(dAlpha) * coefficient * i) + ARANMath.RadToDeg(startDphi) * coefficient;
					Point ptCurr = PointAlongPlane(PtCnt, azt0 - i * dAlpha * TurnDir, R);
					result.Add(ptCurr);
				}
			}
			return result;
		}

		public static void AddSpiralToRing(Point ptCnt, double r0, double coef, double StartRadial, double TurnAngle,
							TurnDirection turnSide, ref Ring ring)
		{
			double fSide = (int)turnSide;

			int n = (int)Math.Round(ARANMath.RadToDeg(TurnAngle));

			if (n > 0)
			{
				if (n <= 1) n = 1;
				else if (n <= 5) n = 5;
				else if (n < 10) n = 10;
				double dAlpha = TurnAngle / n;

				for (int i = 0; i < n; i++)
				{
					double fTmp = i * dAlpha;
					double R = r0 + fTmp * coef;
					Point ptCur = ARANFunctions.LocalToPrj(ptCnt, StartRadial + fTmp * fSide, R, 0);
					ring.Add(ptCur);
				}
			}
		}

		/*
		public static double PointToRingDistanceF(Point point, Ring ring)
		{

			int I, Ix, J, N, M, Mn;
			double distMin, dX, dY, dXY,
			dXMin, dYMin, dXYMin, Len, K,
			dXp, dYp, x, d0, Eps;

			Point PtTmp, Pt0, Pt1;

			int[] IArray = new int[3];
			int[] IxArray = new int[3];

			N = ring.Count;
			Eps = ARANMath.EpsilonDistance * ARANMath.EpsilonDistance;

			IArray[0] = 0;
			IArray[1] = 0;
			IArray[2] = 0;

			PtTmp = ring[0];
			dXMin = Math.Abs(PtTmp.X - point.X);
			dYMin = Math.Abs(PtTmp.Y - point.Y);
			dXYMin = dXMin + dYMin;
			distMin = ARANMath.Hypot(dXMin, dYMin);

			for (I = 1; I < N; I++)
			{
				PtTmp = ring[I];
				dX = Math.Abs(PtTmp.X - point.X);
				dY = Math.Abs(PtTmp.Y - point.Y);
				dXY = dX + dY;

				if (dX < dXMin)
				{
					dXMin = dX;
					IArray[0] = I;
				}

				if (dY < dYMin)
				{
					dYMin = dY;
					IArray[1] = I;
				}

				if (dXY < dXYMin)
				{
					dXYMin = dXY;
					IArray[2] = I;
				}
			}

			Mn = 2;
			I = 0;
			while (I < Mn - 1)
			{
				J = I + 1;
				while (J < Mn)
				{
					if (IArray[I] == IArray[J])
					{
						if (J < 2)
							IArray[J] = IArray[J + 1];
						Mn--;
					}
					else
						J++;
				}
				I++;
			}

			for (M = 0; M <= Mn; M++)
			{
				IxArray[1] = IArray[M];

				I = IxArray[1];
				J = (I + N - 1) % N;

				Pt0 = ring[I];
				Pt1 = ring[J];

				dX = Pt0.X - Pt1.X;
				dY = Pt0.Y - Pt1.Y;
				Len = dY * dY + dX * dX;

				while (Len < Eps)
				{
					J = (J + N - 1) % N;
					Pt1 = ring[J];
					dX = Pt0.X - Pt1.X;
					dY = Pt0.Y - Pt1.Y;
					Len = dY * dY + dX * dX;
				}
				IxArray[0] = J;

				J = (I + 1) % N;
				Pt1 = ring[J];
				dX = Pt0.X - Pt1.X;
				dY = Pt0.Y - Pt1.Y;
				Len = dY * dY + dX * dX;

				while (Len < Eps)
				{
					J = (J + 1) % N;
					Pt1 = ring[J];
					dX = Pt0.X - Pt1.X;
					dY = Pt0.Y - Pt1.Y;
					Len = dY * dY + dX * dX;
				}
				IxArray[2] = J;

				for (Ix = 0; Ix <= 1; Ix++)
				{
					I = IxArray[Ix];
					J = IxArray[Ix + 1];

					Pt0 = ring[I];
					Pt1 = ring[J];

					dX = Pt1.X - Pt0.X;
					dY = Pt1.Y - Pt0.Y;

					Len = ARANMath.Hypot(dY, dX);
					K = 1 / Len;

					dXp = point.X - Pt0.X;
					dYp = point.Y - Pt0.Y;

					x = K * (dXp * dX + dYp * dY);

					if (x < 0)
					{
						d0 = ARANMath.Hypot(dYp, dXp);
						if (d0 < distMin)
							distMin = d0;
					}
					else if (x > Len)
					{
						dX = Pt1.X - point.X;
						dY = Pt1.Y - point.Y;
						d0 = ARANMath.Hypot(dY, dX);
						if (d0 < distMin)
							distMin = d0;
					}
					else
					{
						d0 = Math.Abs(K * (-dXp * dY + dYp * dX));
						if (d0 < distMin)
							distMin = d0;
					}
				}
			}

			return distMin;
		}
		/**/

		public static double PointToRingDistance(Point point, Ring ring)
		{
			int n = ring.Count;
			if (n == 0)
				return double.NaN;

			int i = n - 1;
			Point P0 = ring[i], P1;

			double dXp = point.X - P0.X;
			double dYp = point.Y - P0.Y;
			double distMin = ARANMath.Hypot(dYp, dXp);

			double Eps = Math.Pow(ARANMath.EpsilonDistance, 2);
			double Len2, dX, dY, d0;

			do
			{
				i = (i + 1) & (0 - Convert.ToInt32(i + 1 < n));
				if (i == n - 1)
					return distMin;

				P1 = ring[i];
				dX = P1.X - P0.X;
				dY = P1.Y - P0.Y;
				Len2 = dY * dY + dX * dX;
			}
			while (Len2 >= Eps);

			dXp = point.X - P0.X;
			dYp = point.Y - P0.Y;

			double X0 = (dXp * dX + dYp * dY) - Len2;

			while (i < n)
			{
				P0 = P1;
				int j = i;

				do
				{
					j = (j + 1) & (0 - Convert.ToInt32(j + 1 < n));
					if (j == i)
						return distMin;

					P1 = ring[j];
					dX = P1.X - P0.X;
					dY = P1.Y - P0.Y;
					Len2 = dY * dY + dX * dX;
				}
				while (Len2 >= Eps);

				dXp = point.X - P0.X;
				dYp = point.Y - P0.Y;
				double x = dXp * dX + dYp * dY;

				if (x > 0)
				{
					if (x < Len2)
					{
						d0 = System.Math.Abs(dYp * dX - dXp * dY) / Math.Sqrt(Len2);
						if (d0 < distMin)
							distMin = d0;
					}
				}
				else if (X0 > 0)
				{
					d0 = ARANMath.Hypot(dYp, dXp);
					if (d0 < distMin)
						distMin = d0;
				}

				X0 = x - Len2;

				if (j < i)
					break;
				i = j;
			}

			return distMin;
		}

#if Test
		public static Point CircleVectorIntersect(Point centerPoint, double radius, Line line, out double d)
		{
			Line line1 = new Line(centerPoint, line.DirVector.Direction + ARANMath.C_PI_2);
			Geometry geom = LineLineIntersect(line, line1);

			d = -1.0;
			if (geom.Type != GeometryType.Point)
				return null;

			double distToVect = ARANMath.Hypot(centerPoint.X - ((Point)geom).X, centerPoint.Y - ((Point)geom).Y);

			if (distToVect < radius)
			{
				d = Math.Sqrt(radius * radius - distToVect * distToVect);
				return PointAlongPlane(((Point)geom), line.DirVector.Direction, d);
			}

			return null;
		}

		public static Point CircleVectorIntersect(Point centerPoint, double radius, Point ptVector, double dirVector, out double d)
		{
			Line line = new Line(centerPoint, dirVector);
			Line line1 = new Line(centerPoint, dirVector + ARANMath.C_PI_2);

			Geometry geom = LineLineIntersect(line, line1);

			d = -1.0;
			if (geom.Type != GeometryType.Point)
				return null;

			Point ptIntersect = (Point)geom;

			double distToVect = ARANMath.Hypot(centerPoint.X - ptIntersect.X, centerPoint.Y - ptIntersect.Y);

			if (distToVect < radius)
			{
				d = Math.Sqrt(radius * radius - distToVect * distToVect);
				return PointAlongPlane(ptIntersect, dirVector, d);
			}

			return null;
		}

		public static Point CircleVectorIntersect2(Point centerPoint, double radius, Point ptVector, double dirVector, out double d)
        {
            //double tanAlpha = Math.Tan(dirVector);
            //double tanBetha = Math.Tan(dirVector + ARANMath.C_PI_2);

            //double x = (tanBetha * centerPoint.X - tanAlpha * ptVector.X - centerPoint.Y + ptVector.Y);
            //double y = tanBetha * (x - centerPoint.X) + centerPoint.Y;
            //double DistCnt2Vect = ARANMath.Hypot(centerPoint.X - x, centerPoint.Y - y);

			Point pnt1 = ptVector;
            Point pnt2 = new Point(pnt1.X + Math.Cos(dirVector), pnt1.Y + Math.Sin(dirVector));

			Point pnt3 = centerPoint;
            Point pnt4 = new Point(pnt3.X + Math.Cos(dirVector + ARANMath.C_PI_2), pnt3.Y + Math.Sin(dirVector + ARANMath.C_PI_2));


            double x1 = pnt1.X;
            double x2 = pnt2.X;
            double x3 = pnt3.X;
            double x4 = pnt4.X;

            double y1 = pnt1.Y;
            double y2 = pnt2.Y;
            double y3 = pnt3.Y;
            double y4 = pnt4.Y;

            double x = ((x1 * y2 - x2 * y1) * (x3 - x4) - (x3 * y4 - x4 * y3) * (x1 - x2)) / ((x1 - x2) * (y3 - y4) - (x3 - x4) * (y1 - y2));
            double y = ((x1 * y2 - x2 * y1) * (y3 - y4) - (x3 * y4 - x4 * y3) * (y1 - y2)) / ((x1 - x2) * (y3 - y4) - (x3 - x4) * (y1 - y2));
            Point pnt = new Point(x, y);

            //double dirFromVectrToCentPnt = ReturnAngleInRadians(centerPoint, ptVector);
            //double subtractedAngle = ARANMath.SubtractAngles(dirVector, dirFromVectrToCentPnt);
            //double distFromVectrToCentPnt = ReturnDistanceInMeters(ptVector, centerPoint);

            double deltaX = (centerPoint.X - pnt.X);
            double deltaY = (centerPoint.Y - pnt.Y);
            double SqrDistCnt2Vect = deltaX * deltaX + deltaY * deltaY;

            if (SqrDistCnt2Vect < radius * radius)
            {
                //double dist = Math.Cos(subtractedAngle) * distFromVectrToCentPnt;
                //Point pnt = PointAlongPlane(ptVector, dirVector, dist);

                double CosA = Math.Cos(dirVector);
                double SinA = Math.Sin(dirVector);

                d = System.Math.Sqrt(radius * radius - SqrDistCnt2Vect);
                return new Point(pnt.X + d * CosA, pnt.Y + d * SinA);
            }

            d = -1.0;
            return new Point();
        }


        public static Point CircleVectorIntersect3(Point centerPoint, double radius, Point ptVector, double dirVector, out double d)
        {
            double sinA = Math.Sin(dirVector);
            double cosA = Math.Cos(dirVector);
            double dx = centerPoint.X - ptVector.X;
            double dy = centerPoint.Y - ptVector.Y;
            double distVector = cosA * dx + sinA * dy;
            double dist1 = ARANMath.Hypot(dx, dy);
            double distCnt2Vect = Math.Tan(dirVector) * distVector;
            if (distCnt2Vect < radius)
            {
                d = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(distCnt2Vect, 2))+distVector;
                double xNew = ptVector.X + d * cosA;
                double yNew = ptVector.Y + d * sinA;
                return new Point(xNew, yNew);
            }
            d = 0.0;
            return new Point();
        }

        public static Point CircleVectorIntersect4(Point centerPoint, double radius, Point ptVector, double dirVector, out double d)
        {
            double sinA = Math.Sin(dirVector);
            double cosA = Math.Cos(dirVector);

			double dx = centerPoint.X - ptVector.X;
            double dy = centerPoint.Y - ptVector.Y;

            double distToInPt = cosA * dx + sinA * dy;

			double dist1 = ARANMath.Hypot(dx, dy);

            double distCnt2Vect = Math.Tan(dirVector) * distToInPt;

            if (distCnt2Vect < radius)
            {
                d = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(distCnt2Vect, 2))+distToInPt;
                double xNew = ptVector.X + d * cosA;
                double yNew = ptVector.Y + d * sinA;
                return new Point(xNew, yNew);
            }
            d = -1.0;
            return new Point();
        }
#endif

		public static Point CircleVectorIntersect(Point centerPoint, double radius, Point ptVector, double dirVector, out double d)
		{
			double v1x = Math.Cos(dirVector);
			double v1y = Math.Sin(dirVector);

			double v2x = centerPoint.X - ptVector.X;
			double v2y = centerPoint.Y - ptVector.Y;

			double distToIntersect = v1x * v2x + v1y * v2y;

			Point ptIntersect = new Point(ptVector.X + v1x * distToIntersect, ptVector.Y + v1y * distToIntersect);

			double dx = centerPoint.X - ptIntersect.X;
			double dy = centerPoint.Y - ptIntersect.Y;

			double distCenterToIntersect2 = dx * dx + dy * dy;
			double radius2 = radius * radius;

			if (distCenterToIntersect2 < radius2)
			{
				d = Math.Sqrt(radius2 - distCenterToIntersect2);

				double xNew = ptVector.X + (d + distToIntersect) * v1x;
				double yNew = ptVector.Y + (d + distToIntersect) * v1y;
				return new Point(xNew, yNew);

				//return LocalToPrj(ptIntersect, dirVector, d, 0);
			}

			d = -1.0;
			return new Point(); // null;
		}

		public static Point CircleVectorIntersect(Point centerPoint, double radius, Point ptVector, double dirVector)
		{
			double fTemp;
			return CircleVectorIntersect(centerPoint, radius, ptVector, dirVector, out fTemp);
		}

		public static Point CircleVectorIntersect(Point centerPoint, double radius, Line line, out double d)
		{
			return CircleVectorIntersect(centerPoint, radius, line.RefPoint, line.DirVector.Direction, out d);
		}

		public static Point CircleVectorIntersect(Point centerPoint, double radius, Line line)
		{
			double fTemp;
			return CircleVectorIntersect(centerPoint, radius, line.RefPoint, line.DirVector.Direction, out fTemp);
		}

		//public static void CircleVectorIntersect(Point centPoint, double radius, Line Line, double d, Point Result)
		//{
		//    double distToVect;
		//    Line Line1;
		//    Geometry geom;
		//    Point ptTmp;
		//    Result.SetEmpty();

		//    Line1 = new Line(centPoint, Line.DirVector.direction + 0.5 * Math.PI);

		//    geom = LineLineIntersect(Line, Line1);

		//    if (geom.GeometryType != GeometryType.Point)
		//        return;

		//    distToVect = Hypot(centPoint.X - ((Point)geom).X, centPoint.Y - ((Point)geom).Y);
		//    d = -1.0;

		//    if (distToVect < radius)
		//    {
		//        d = Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(distToVect, 2));
		//        ptTmp = PointAlongPlane((Point)geom, Line.DirVector.direction, d);
		//        Result.Assign(ptTmp);
		//    }

		//}

		//public static void CircleVectorIntersect(Point CenterPoint, double Radius, Point ptVector, double Direction, double d, Point Result)
		//{

		//    double distToVect;
		//    Geometry geom;
		//    Result.SetEmpty();
		//    Point ptTmp;

		//    geom = LineLineIntersect(ptVector, Direction, CenterPoint, Direction + 0.5 * Math.PI);

		//    if (geom.GeometryType != GeometryType.Point)
		//        return;

		//    distToVect = Hypot(CenterPoint.X - ((Point)geom).X, CenterPoint.Y - ((Point)geom).Y);
		//    d = -1.0;

		//    if (distToVect < Radius)
		//    {
		//        d = Math.Sqrt(Math.Pow(Radius, 2) - Math.Pow(distToVect, 2));
		//        ptTmp = PointAlongPlane((Point)geom, Direction, d);
		//        Result.Assign(ptTmp);
		//    }

		//}

		public static Ring CreateCirclePrj(Point ptCnt, double radius, int n = 360)
		{
			Ring result = new Ring();
			result.AddMultiPoint(CreateCircle(ptCnt, radius, n));
			return result;
		}

		public static Polygon CreateTorPrj(Aran.Geometries.Point ptCntPrj, double minRadius, double maxRadius)
		{
			//double xMin, yMin, xMax, yMax;
			Aran.Geometries.Point ptMin = new Aran.Geometries.Point();
			Aran.Geometries.Point ptMax = new Aran.Geometries.Point();
			Aran.Geometries.Ring minRing = new Aran.Geometries.Ring();
			Aran.Geometries.Ring maxRing = new Aran.Geometries.Ring();
			Aran.Geometries.Polygon result = new Aran.Geometries.Polygon();

			for (int i = 0; i <= 359; i++)
			{
				ptMin = LocalToPrj(ptCntPrj, i * ARANMath.DegToRadValue, minRadius, 0);
				ptMax = LocalToPrj(ptCntPrj, i * ARANMath.DegToRadValue, maxRadius, 0);
				minRing.Add(ptMin);
				maxRing.Add(ptMax);
			}

			result.InteriorRingList.Add(minRing);
			result.ExteriorRing = maxRing;
			return result;
		}

		public static Polygon CreateTorGeo(Aran.Geometries.Point ptCntGeo, double minRadius, double maxRadius)
		{
			Aran.Geometries.Point ptMin, ptMax;
			Aran.Geometries.Ring minRing = new Aran.Geometries.Ring();
			Aran.Geometries.Ring maxRing = new Aran.Geometries.Ring();
			Aran.Geometries.Polygon result = new Aran.Geometries.Polygon();

			for (int i = 0; i <= 359; i++)
			{
				ptMin = PointAlongGeodesic(ptCntGeo, i, minRadius);
				ptMax = PointAlongGeodesic(ptCntGeo, i, maxRadius);
				minRing.Add(ptMin);
				maxRing.Add(ptMax);
			}

			result.InteriorRingList.Add(minRing);
			result.ExteriorRing = maxRing;
			return result;
		}

		public static MultiPolygon CreateCircleAsMultiPolyPrj(Point ptCnt, double radius)
		{
			Polygon poly = new Polygon();
			poly.ExteriorRing = CreateCirclePrj(ptCnt, radius);
			MultiPolygon mpoly = new MultiPolygon();
			mpoly.Add(poly);
			return mpoly;
		}

		public static LineString CreateCircleAsPartPrj(Point ptCnt, double radius)
		{
			LineString result = new LineString();
			result.AddMultiPoint(CreateCircle(ptCnt, radius));
			return result;
		}

		public static Ring CreateArcPrj(Point ptCnt, Point ptFrom, Point ptTo, TurnDirection direction)
		{
			double dX = ptFrom.X - ptCnt.X;
			double dY = ptFrom.Y - ptCnt.Y;
			double AztFrom = Math.Atan2(dY, dX);

			double AztTo = Math.Atan2(ptTo.Y - ptCnt.Y, ptTo.X - ptCnt.X);

			double fDir = (int)(direction);
			double dAz = ARANMath.Modulus((AztTo - AztFrom) * fDir, ARANMath.C_2xPI);

			int n = (int)Math.Floor(ARANMath.RadToDeg(dAz));
			if (n <= 1) n = 1;
			else if (n <= 5) n = 5;
			else if (n < 10) n = 10;

			double AngleStep = dAz / n * fDir;
			double R = Math.Sqrt(dX * dX + dY * dY);

			Ring result = new Ring();
			result.Add(ptFrom);

			for (int i = 1; i < n; i++)
			{
				double iInRad = AztFrom + i * AngleStep;
				Point Pt = new Point(ptCnt.X + R * Math.Cos(iInRad), ptCnt.Y + R * Math.Sin(iInRad));
				result.Add(Pt);
			}
			result.Add(ptTo);
			return result;
		}

		public static LineString CreateArcAsPartPrj(Point ptCnt, Point ptFrom, Point ptTo, TurnDirection direction)
		{
			double dX = ptFrom.X - ptCnt.X;
			double dY = ptFrom.Y - ptCnt.Y;

			double AztFrom = Math.Atan2(dY, dX);
			double AztTo = Math.Atan2(ptTo.Y - ptCnt.Y, ptTo.X - ptCnt.X);

			double fDir = (int)direction;
			double dAz = ARANMath.Modulus((AztTo - AztFrom) * fDir, ARANMath.C_2xPI);

			int n = (int)Math.Floor(ARANMath.RadToDeg(dAz));
			if (n <= 1) n = 1;
			else if (n <= 5) n = 5;
			else if (n < 10) n = 10;

			double AngleStep = dAz / n;
			double R = Math.Sqrt(dX * dX + dY * dY);

			LineString result = new LineString();
			result.Add(ptFrom);

			for (int i = 1; i < n; i++)
			{
				double iInRad = AztFrom + i * AngleStep * fDir;
				Point Pt = new Point(ptCnt.X + R * Math.Cos(iInRad), ptCnt.Y + R * Math.Sin(iInRad));
				result.Add(Pt);
			}

			result.Add(ptTo);
			return result;
		}

		public static void AddArcToMultiPoint(Point ptCnt, Point ptFrom, Point ptTo, TurnDirection direction, ref MultiPoint result)
		{
			double dX = ptFrom.X - ptCnt.X;
			double dY = ptFrom.Y - ptCnt.Y;

			double AztFrom = Math.Atan2(dY, dX);
			double AztTo = Math.Atan2(ptTo.Y - ptCnt.Y, ptTo.X - ptCnt.X);

			double fDir = (int)direction;
			double dAz = ARANMath.Modulus((AztTo - AztFrom) * fDir, ARANMath.C_2xPI);

			int n = (int)Math.Floor(ARANMath.RadToDeg(dAz));

			if (n <= 1) n = 1;
			else if (n <= 5) n = 5;
			else if (n < 10) n = 10;

			double AngStep = dAz / n;
			double R = Math.Sqrt(dX * dX + dY * dY);

			result.Add(ptFrom);

			for (int i = 1; i < n; i++)
			{
				double iInRad = AztFrom + i * AngStep * fDir;
				Point Pt = new Point(ptCnt.X + R * Math.Cos(iInRad), ptCnt.Y + R * Math.Sin(iInRad));
				result.Add(Pt);
			}
			result.Add(ptTo);
		}

		public static MultiPoint CreateArcGeo(Point centrePoint, double radius, double fromDir, double toDir, TurnDirection direction)
		{
			var result = new MultiPoint();

			double fDir = (int)direction;
			double dAz = ARANMath.Modulus((toDir - fromDir) * fDir, ARANMath.C_2xPI);

			int n = (int)Math.Floor(ARANMath.RadToDeg(dAz));

			if (n <= 1) n = 1;
			else if (n <= 5) n = 5;
			else if (n < 10) n = 10;

			double AngStep = dAz / n;

			for (int i = 0; i <= n; i++)
			{
				double iInRad = fromDir + i * AngStep * fDir;
				Point Pt = new Point(centrePoint.X + radius * Math.Cos(iInRad), centrePoint.Y + radius * Math.Sin(iInRad));
				result.Add(Pt);
			}

			return result;
		}

		public static LineString RingToPart(Ring ring)
		{
			LineString part = new LineString();
			foreach (Point pt in ring)
			{
				part.Add(pt);
			}
			return part;
		}

		public static Ring PartToRing(LineString part)
		{
			Ring ring = new Ring();
			foreach (Point pt in part)
			{
				ring.Add(pt);
			}
			return ring;
		}

		public static Polygon CreatePolygonFromRings(params Ring[] ringList)
		{
			Polygon geom = new Polygon();
			foreach (Ring ring in ringList)
			{
				if (ring.IsExterior)
					geom.ExteriorRing = ring;
				else
					geom.InteriorRingList.Add(ring);
			}
			return geom;
		}

		public static MultiLineString CreatePolyLineFromParts(params LineString[] partList)
		{
			MultiLineString geom = new MultiLineString();
			foreach (LineString part in partList)
			{
				geom.Add(part);
			}
			return geom;
		}

		public static MultiLineString PolygonToPolyLine(Polygon geom)
		{
			MultiLineString polyLine = new MultiLineString();
			int n = geom.ExteriorRing.Count;

			if (n > 0)
			{
				polyLine.Add((LineString)geom.ExteriorRing);
				if (ARANFunctions.ReturnDistanceInMeters(geom.ExteriorRing[0], geom.ExteriorRing[n - 1]) > ARANMath.EpsilonDistance)
					polyLine[0].Add(geom.ExteriorRing[0]);
			}

			foreach (Ring ring in geom.InteriorRingList)
			{
				n = ring.Count;
				if (n > 0)
				{
					polyLine.Add((LineString)ring);
					if (ARANFunctions.ReturnDistanceInMeters(ring[0], ring[n - 1]) > ARANMath.EpsilonDistance)
						polyLine[polyLine.Count - 1].Add(ring[0]);
				}
			}
			return polyLine;
		}

		public static MultiLineString PolygonToPolyLine(MultiPolygon mltPolygon)
		{
			MultiLineString polyLine = new MultiLineString();

			foreach (Aran.Geometries.Polygon geom in mltPolygon)
			{
				int n = geom.ExteriorRing.Count;

				if (n > 0)
				{
					polyLine.Add((LineString)geom.ExteriorRing);
					if (ARANFunctions.ReturnDistanceInMeters(geom.ExteriorRing[0], geom.ExteriorRing[n - 1]) >
						ARANMath.EpsilonDistance)
						polyLine[0].Add(geom.ExteriorRing[0]);
				}

				foreach (Ring ring in geom.InteriorRingList)
				{
					n = ring.Count;
					if (n > 0)
					{
						polyLine.Add((LineString)ring);
						if (ARANFunctions.ReturnDistanceInMeters(ring[0], ring[n - 1]) > ARANMath.EpsilonDistance)
							polyLine[polyLine.Count - 1].Add(ring[0]);
					}
				}
			}
			return polyLine;
		}

		public static MultiLineString FullProtectionAreaBorder(MultiPolygon mltPolygon, double dir)
		{
			MultiLineString polyLine = new MultiLineString();

			foreach (Aran.Geometries.Polygon polygon in mltPolygon)
			{
				Polygon geom = RemoveAgnails(polygon);
				int n = geom.ExteriorRing.Count;

				if (n > 0)
				{
					LineString lineString = new LineString();
					for (int k = 0; k <= n; k++)
					{
						int i = k % n;
						int j = (i + 1) % n;

						double ang = ReturnAngleInRadians(geom.ExteriorRing[i], geom.ExteriorRing[j]);

						double sub = ARANMath.SubtractAngles(ang, dir + 0.5 * Math.PI);

						if (sub < ARANMath.EpsilonRadian || Math.Abs(sub - Math.PI) < ARANMath.EpsilonRadian)
						{
							if (lineString.Count > 0)
							{
								lineString.Add(geom.ExteriorRing[i]);
								polyLine.Add(lineString);
								lineString = new LineString();
							}
						}
						else
							lineString.Add(geom.ExteriorRing[i]);
					}

					if (lineString.Count > 1)
						polyLine.Add(lineString);
				}

				foreach (Ring ring in geom.InteriorRingList)
				{
					n = ring.Count;
					if (n > 1 && ARANFunctions.ReturnDistanceInMeters(ring[0], ring[n - 1]) < ARANMath.EpsilonDistance)
					{
						n--;
						ring.Remove(n);
					}

					if (n > 0)
					{
						LineString lineString = new LineString();

						for (int k = 0; k <= n; k++)
						{
							int i = k % n;
							int j = (i + 1) % n;

							double ang = ARANFunctions.ReturnAngleInRadians(ring[i], ring[j]);
							double sub = ARANMath.SubtractAngles(ang, dir + 0.5 * Math.PI);

							if (sub < ARANMath.EpsilonRadian || Math.Abs(sub - Math.PI) < ARANMath.EpsilonRadian)
							{
								if (lineString.Count > 0)
								{
									lineString.Add(ring[i]);
									polyLine.Add(lineString);
									lineString = new LineString();
								}
								//else
								//	lineString.Clear();
							}
							else
								lineString.Add(ring[i]);
						}

						if (lineString.Count > 1)
							polyLine.Add(lineString);
					}
				}
			}

			return polyLine;
		}

		//public static Polygon PolyLineToPolygon ( MultiLineString geom )
		//{
		//    Polygon polygon = new Polygon ( );
		//    foreach ( LineString part in geom )
		//    {
		//        polygon.Add ( ( Ring ) part );
		//    }
		//    return polygon;
		//}

		public static MultiPolygon RemoveAgnails(MultiPolygon pPolygon)
		{
			MultiPolygon pmPoly = (MultiPolygon)pPolygon.Clone();
			MultiPolygon result = new MultiPolygon();

			for (int i = 0; i < pmPoly.Count; i++)
			{
				Polygon pPoly = RemoveAgnails(pmPoly[i]);
				if (pPoly.Area > 0.0)
					result.Add(pPoly);
			}

			return result;
		}

		public static Polygon RemoveAgnails(Polygon pPolygon)
		{
			Polygon pPoly = (Polygon)pPolygon.Clone();

			for (int i = 0; i < pPoly.InteriorRingList.Count; i++)
				pPoly.InteriorRingList[i] = RemoveAgnails(pPoly.InteriorRingList[i]);

			pPoly.ExteriorRing = RemoveAgnails(pPoly.ExteriorRing);

			return pPoly;
		}

		/// <summary>
		/// for closed rings only
		/// </summary>
		/// 
		/// <param name="pRing"></param>
		/// <returns></returns>

		public static Ring RemoveAgnails_(Ring pRing)
		{
			const double Eps = 0.00001;

			Ring result = (Ring)pRing.Clone();
			int n = result.Count - 1;

			if (n < 3)
				return result;

			int j = 0;

			while (j < n)
			{
				if (n < 4)
					break;

				int k = (j + 1) % n;
				int l = (j + 2) % n;

				double dx0 = result[k].X - result[j].X;
				double dy0 = result[k].Y - result[j].Y;

				double dx1 = result[l].X - result[k].X;
				double dy1 = result[l].Y - result[k].Y;

				double dxx = dx1 * dx1;
				double dyy = dy1 * dy1;
				double dl = dxx + dyy;
				//double eps = Eps * Math.Abs(dy0 * dy1);

				if (dl <= Eps)
				{
					result.Remove(k);
					n--;

					if (j >= n)
						j = n - 1;
				}
				//else if (Math.Abs(Math.Abs(dx0 * dy1) - Math.Abs(dx1 * dy0)) <= eps)
				else if (dyy > dxx)
				{
					if (System.Math.Abs(System.Math.Abs(dx0 / dy0) - System.Math.Abs(dx1 / dy1)) <= Eps)
					//if (System.Math.Abs(dx0 / dy0 - dx1 / dy1) <= Eps)
					{
						result.Remove(k);
						n--;

						if (j >= n)
							j = n - 1;
					}
					else
						j++;
				}
				else if (System.Math.Abs(System.Math.Abs(dy0 / dx0) - System.Math.Abs(dy1 / dx1)) <= Eps)
				//if (System.Math.Abs(dy0 / dx0 - dy1 / dx1) <= Eps)
				{
					result.Remove(k);
					n--;

					if (j >= n)
						j = n - 1;
				}
				else
					j++;
			}

			return result;
		}

		public static Ring RemoveAgnails(this Ring pRing)
		{
			const double Eps = 0.00001;

			Ring result = new Ring();
			int n = pRing.Count;

			if (n < 3)
				return result;

			double dx = pRing[0].X - pRing[n - 1].X;
			double dy = pRing[0].Y - pRing[n - 1].Y;

			if (dx * dx + dy * dy < Eps * Eps)
				n--;

			if (n < 3)
				return result;

			int i = n - 2;
			int j = n - 1;

			for (int k = 0; k < n; k++)
			{
				double dx1 = pRing[k].X - pRing[j].X;
				double dy1 = pRing[k].Y - pRing[j].Y;

				double dxx = dx1 * dx1;
				double dyy = dy1 * dy1;
				double dl1 = dxx + dyy;

				if (dl1 < Eps)
					continue;

				double dx0 = pRing[j].X - pRing[i].X;
				double dy0 = pRing[j].Y - pRing[i].Y;
				double dl0 = dx0 * dx0 + dy0 * dy0;

				if (dl0 >= Eps)
				{
					if (dy0 * dy0 > 0.0)
					{
						if (dyy == 0.0)
							result.Add(pRing[j]);
						else if (Math.Abs(Math.Abs(dx0 / dy0) - Math.Abs(dx1 / dy1)) >= Eps)
							result.Add(pRing[j]);
					}
					else if (dx0 * dx0 > 0.0)
					{
						if (dxx == 0.0)
							result.Add(pRing[j]);
						else if (Math.Abs(Math.Abs(dy0 / dx0) - Math.Abs(dy1 / dx1)) >= Eps)
							result.Add(pRing[j]);
					}
					else
						result.Add(pRing[j]);
				}

				i = j;
				j = k;
			}

			return result;
		}

		//public static MultiLineString CreateOutline(Point StartFIXPt, double StartDir, Point EndFIXPt, double EndDir, Ring ring)
		//{
		//	const double Eps = 0.00001;

		//	double sina = Math.Sin(StartDir);
		//	double cosa = Math.Cos(StartDir);

		//	double sinb = Math.Sin(EndDir);
		//	double cosb = Math.Cos(EndDir);

		//	double v3xs = -sina;
		//	double v3ys = cosa;

		//	double v3xe = -sinb;
		//	double v3ye = cosb;

		//	MultiLineString result = new MultiLineString();

		//	Ring ringL = ring.RemoveAgnails();

		//	int n = ringL.Count;

		//	int st = n - 1;
		//	int p1 = st;

		//	for (int i = 0; i < n; i++)
		//	{
		//		int p0 = p1;
		//		p1 = i;

		//		Point ptl0 = ringL[p0];
		//		Point ptl1 = ringL[p1];

		//		double v2x = ptl1.X - ptl0.X;
		//		double v2y = ptl1.Y - ptl0.Y;

		//		double v2v3 = v2x * v3xs + v2y * v3ys;

		//		if (v2v3 >= -Eps && v2v3 <= Eps)
		//			continue;

		//		double v1x = StartFIXPt.X - ptl0.X;
		//		double v1y = StartFIXPt.Y - ptl0.Y;

		//		double t1 = (v2x * v1y - v2y * v1x) / v2v3;
		//		double t2 = (v1x * v3xs + v1y * v3ys) / v2v3;

		//		if (t1 <= 0.0 && t2 >= 0.0 && t2 <= 1.0)
		//		{
		//			double v0v2 = v2x * cosa + v2y * sina;
		//			if (v0v2 > -Eps && v0v2 < Eps)
		//			{
		//				st = p1;
		//				break;
		//			}
		//		}
		//	}

		//	LineString tmpLine = new LineString();

		//	p1 = st;
		//	for (int i = 0; i < n; i++)
		//	{
		//		bool bInterrupt = false;
		//		int p0 = p1;
		//		p1 = (i + st + 1) % n;

		//		Point ptl0 = ringL[p0];
		//		Point ptl1 = ringL[p1];

		//		double v2x = ptl1.X - ptl0.X;
		//		double v2y = ptl1.Y - ptl0.Y;

		//		double v2v3 = v2x * v3xs + v2y * v3ys;

		//		if (v2v3 < -Eps || v2v3 > Eps)
		//		{
		//			double v1x = StartFIXPt.X - ptl0.X;
		//			double v1y = StartFIXPt.Y - ptl0.Y;

		//			double t2 = (v1x * v3xs + v1y * v3ys) / v2v3;
		//			double t1 = (v2x * v1y - v2y * v1x) / v2v3;

		//			if (t1 <= 0.0 && t2 >= 0.0 && t2 <= 1.0)
		//			{
		//				double v0v2 = v2x * cosa + v2y * sina;
		//				bInterrupt = Math.Abs(v0v2) < Eps;
		//			}
		//		}

		//		if (!bInterrupt)
		//		{
		//			v2v3 = v2x * v3xe + v2y * v3ye;
		//			if (v2v3 < -Eps || v2v3 > Eps)
		//			{
		//				double v1x = EndFIXPt.X - ptl0.X;
		//				double v1y = EndFIXPt.Y - ptl0.Y;

		//				double t2 = (v1x * v3xe + v1y * v3ye) / v2v3;
		//				double t1 = (v2x * v1y - v2y * v1x) / v2v3;

		//				if (t1 >= 0.0 && t2 >= 0.0 && t2 <= 1.0)
		//				{
		//					double v0v2 = v2x * cosb + v2y * sinb;
		//					bInterrupt = Math.Abs(v0v2) < Eps;
		//				}
		//			}
		//		}

		//		if (!bInterrupt)
		//			tmpLine.Add(ptl0);
		//		else if (tmpLine.Count > 0)
		//		{
		//			tmpLine.Add(ptl0);
		//			result.Add(tmpLine);

		//			tmpLine = new LineString();
		//		}
		//	}

		//	if (tmpLine.Count > 1)
		//		result.Add(tmpLine);

		//	return result;
		//}

		//public static MultiLineString CreateOutline(Point StartFIXPt, double StartDir, Point EndFIXPt, double EndDir, Polygon poly)
		//{
		//	MultiLineString result = CreateOutline(StartFIXPt, StartDir, EndFIXPt, EndDir, poly.ExteriorRing);

		//	for (int i = 0; i < poly.InteriorRingList.Count; i++)
		//	{
		//		MultiLineString tmpPartial = CreateOutline(StartFIXPt, StartDir, EndFIXPt, EndDir, poly.InteriorRingList[i]);
		//		for (int j = 0; j < tmpPartial.Count; j++)
		//			result.Add(tmpPartial[j]);
		//	}

		//	return result;
		//}

		//public static MultiLineString CreateOutline(Point StartFIXPt, double StartDir, Point EndFIXPt, double EndDir, MultiPolygon poly)
		//{
		//	if (poly.Count == 0)
		//		return new MultiLineString();

		//	MultiLineString result = CreateOutline(StartFIXPt, StartDir, EndFIXPt, EndDir, poly[0]);

		//	for (int i = 1; i < poly.Count; i++)
		//	{
		//		MultiLineString tmpPartial = CreateOutline(StartFIXPt, StartDir, EndFIXPt, EndDir, poly[i]);
		//		for (int j = 0; j < tmpPartial.Count; j++)
		//			result.Add(tmpPartial[j]);
		//	}

		//	return result;
		//}

		public static int AnglesSideDef(double X, double Y)
		{
			int result;
			double z = ARANMath.Modulus(X - Y, 360.0);
			if (z == 0.0)
				result = 0;
			else if (z > 180.0)
				result = 1;
			else if (z < 180.0)
				result = -1;
			else
				result = 2;
			return result;
		}

		public static int AnglesSideDefRad(double X, double Y)
		{
			int result;
			double z = ARANMath.Modulus(X - Y, ARANMath.C_2xPI);
			if (z == 0.0)
				result = 0;
			else if (z > ARANMath.C_PI)
				result = 1;
			else if (z < ARANMath.C_PI)
				result = -1;
			else
				result = 2;
			return result;
		}

		public static bool AngleInSector(double angle, double x, double y)
		{
			x = NativeMethods.Modulus(x);
			y = NativeMethods.Modulus(y);
			angle = NativeMethods.Modulus(angle);

			if (x > y)
				if ((angle >= x) || (angle <= y)) return true;

			if ((angle >= x) && (angle <= y)) return true;

			return false;
		}

		public static bool AngleInInterval(double angle, Interval interval)
		{
			if (interval.Min == -2)
				return false;

			if (interval.Max == -1)
			{
				if (System.Math.Round(interval.Min, 1) == System.Math.Round(angle, 1))
					return true;

				return false;
			}

			interval.Min = NativeMethods.Modulus(interval.Min);
			interval.Max = NativeMethods.Modulus(interval.Max);
			angle = NativeMethods.Modulus(angle);

			if (interval.Min > interval.Max)
				if ((angle >= interval.Min) || (angle <= interval.Max))
					return true;

			if ((angle >= interval.Min) && (angle <= interval.Max))
				return true;

			return false;
		}

		public static bool AngleInSectorRad(double angleInRad, double xInRad, double yInRad)
		{
			xInRad = NativeMethods.Modulus(xInRad, ARANMath.C_2xPI);
			yInRad = NativeMethods.Modulus(yInRad, ARANMath.C_2xPI);
			angleInRad = NativeMethods.Modulus(angleInRad, ARANMath.C_2xPI);

			if (xInRad > yInRad)
				if ((angleInRad >= xInRad) || (angleInRad <= yInRad)) return true;

			if ((angleInRad >= xInRad) && (angleInRad <= yInRad)) return true;

			return false;
		}

		public static bool AngleInIntervalRad(double angleInRad, Interval intervalInRad)
		{
			if (intervalInRad.Min == -2)
				return false;

			//if (intervalInRad.Max == -1)
			//{
			//	if (System.Math.Round(intervalInRad.Min, 1) == System.Math.Round(angleInRad, 1))
			//		return true;

			//	return false;
			//}

			intervalInRad.Min = NativeMethods.Modulus(intervalInRad.Min, ARANMath.C_2xPI);
			intervalInRad.Max = NativeMethods.Modulus(intervalInRad.Max, ARANMath.C_2xPI);
			angleInRad = NativeMethods.Modulus(angleInRad, ARANMath.C_2xPI);

			if (intervalInRad.Min > intervalInRad.Max)
				if ((angleInRad >= intervalInRad.Min) || (angleInRad <= intervalInRad.Max))
					return true;

			if ((angleInRad >= intervalInRad.Min) && (angleInRad <= intervalInRad.Max))
				return true;

			return false;
		}

		public static Interval IntervalIntersectInDir(Interval interval1, Interval interval2)
		{
			Interval result = new Interval();
			result.Min = double.NaN;
			result.Max = double.NaN;
			double left, right;

			SideDirection interval1Left_2LeftSideDir = (SideDirection)AnglesSideDefRad(interval1.Min, interval2.Min);
			SideDirection interval1Left_2RightSideDir = (SideDirection)AnglesSideDefRad(interval1.Min, interval2.Max);

			SideDirection interval1Right_2LeftSideDir = (SideDirection)AnglesSideDefRad(interval1.Max, interval2.Min);
			SideDirection interval1Right_2RightSideDir = (SideDirection)AnglesSideDefRad(interval1.Max, interval2.Max);

			if ((interval1Left_2LeftSideDir == SideDirection.sideRight && interval1Left_2RightSideDir == SideDirection.sideRight) ||
				(interval1Right_2LeftSideDir == SideDirection.sideLeft && interval1Right_2RightSideDir == SideDirection.sideLeft))
				return result;

			if (AnglesSideDefRad(interval1.Min, interval2.Min) == (int)SideDirection.sideRight)
				left = interval1.Min;
			else
				left = interval2.Min;

			if (AnglesSideDefRad(interval1.Max, interval2.Max) == (int)SideDirection.sideRight)
				right = interval2.Max;
			else
				right = interval1.Max;

			result.Min = left;
			result.Max = right;
			return result;
		}

		public static LineString ConvertPointsToTrackLIne(MultiPoint multiPoint)
		{
			double fE = ARANMath.DegToRad(0.5);
			int n = multiPoint.Count - 1;
			Point CntPt = new Point();

			LineString result = new LineString();
			result.Add(multiPoint[0]);

			for (int i = 0; i < n; i++)
			{
				Point fromPnt = multiPoint[i];
				Point toPnt = multiPoint[i + 1];
				double fTmp = fromPnt.M - toPnt.M;

				//if (fTmp > -fE && fTmp < fE)	//
				if ((System.Math.Abs(Math.Sin(fTmp)) <= fE) && (Math.Cos(fTmp) > 0))
					result.Add(toPnt);
				else
				{
					if (System.Math.Abs(Math.Sin(fTmp)) > fE)
					{
						Geometry tmpGeometry = LineLineIntersect(fromPnt, fromPnt.M + ARANMath.C_PI_2, toPnt, toPnt.M + ARANMath.C_PI_2);
						// 				if tmpGeometry.GeometryType = gtPoint then
						CntPt.Assign(tmpGeometry);
						// 				else
					}
					else
						CntPt.SetCoords(0.5 * (fromPnt.X + toPnt.X), 0.5 * (fromPnt.Y + toPnt.Y));

					SideDirection side = ARANMath.SideDef(fromPnt, fromPnt.M, toPnt);
					LineString arcLineString = CreateArcAsPartPrj(CntPt, fromPnt, toPnt, (TurnDirection)(side));

					result.AddMultiPoint(arcLineString);

					//for (int j = 0; j < arcLineString.Count; j++)
					//	result.Add(arcLineString[j]);
				}
			}

			return result;
		}

		public static LineString CalcTrajectoryFromMultiPoint(Line[] lineArray)
		{
			LineString result = new LineString();
			double fE = ARANMath.DegToRad(0.5);
			int n = lineArray.Length - 1;

			result.Add(lineArray[0].RefPoint);

			for (int i = 0; i < n; i++)
			{
				Line fromLine = lineArray[i];
				Line toLine = lineArray[i + 1];
				double fTmp = fromLine.DirVector.Direction - toLine.DirVector.Direction;

				if ((System.Math.Abs(Math.Sin(fTmp)) <= fE) && (Math.Cos(fTmp) > 0))
					result.Add(toLine.RefPoint);
				else
				{
					Point centrePoint;

					if (System.Math.Abs(Math.Sin(fTmp)) > fE)
						centrePoint = (Point)LineLineIntersect(
										fromLine.RefPoint, fromLine.DirVector.Direction + ARANMath.C_PI_2,
										toLine.RefPoint, toLine.DirVector.Direction + ARANMath.C_PI_2);
					else
						centrePoint = new Point(
											0.5 * (fromLine.RefPoint.X + toLine.RefPoint.X),
											0.5 * (fromLine.RefPoint.Y + toLine.RefPoint.Y));

					SideDirection sideDir = ARANMath.SideDef(fromLine.RefPoint, fromLine.DirVector.Direction, toLine.RefPoint);
					result.AddMultiPoint(CreateArcAsPartPrj(centrePoint, fromLine.RefPoint, toLine.RefPoint, (TurnDirection)(-(int)(sideDir))));
				}
			}

			return result;
		}

		public static LineString CalcTrajectoryFromMultiPoint(MultiPoint mPoint)
		{
			double fE = ARANMath.DegToRad(0.5);
			int n = mPoint.Count - 1;

			LineString result = new LineString();
			result.Add(mPoint[0]);

			for (int i = 0; i < n; i++)
			{
				Point fromPnt = mPoint[i];
				Point toPnt = mPoint[i + 1];
				double fTmp = fromPnt.M - toPnt.M;

				if ((System.Math.Abs(Math.Sin(fTmp)) <= fE) && (Math.Cos(fTmp) > 0))
					result.Add(toPnt);
				else
				{
					Point centrePoint;

					if (System.Math.Abs(Math.Sin(fTmp)) > fE)
						centrePoint = (Point)LineLineIntersect(
										fromPnt, fromPnt.M + ARANMath.C_PI_2,
										toPnt, toPnt.M + ARANMath.C_PI_2);
					else
						centrePoint = new Point(
											0.5 * (fromPnt.X + toPnt.X),
											0.5 * (fromPnt.Y + toPnt.Y));

					SideDirection sideDir = ARANMath.SideDef(fromPnt, fromPnt.M, toPnt);
					result.AddMultiPoint(CreateArcAsPartPrj(centrePoint, fromPnt, toPnt, (TurnDirection)(sideDir)));
				}
			}
			return result;
		}

		public static Point TangentCyrcleIntersectPoint(Point centrePoint, double radius, Point outPoint, SideDirection side)
		{
			double distance = ReturnDistanceInMeters(centrePoint, outPoint);

			if (distance < radius)
				return null;

			double alpha = Math.Asin(radius / distance);
			double dirLine = ReturnAngleInRadians(outPoint, centrePoint);

			int turnVal = (int)(side);
			double dirRadius = dirLine - turnVal * (alpha + ARANMath.C_PI_2);
			dirLine = dirLine - turnVal * alpha;

			return (Point)LineLineIntersect(outPoint, dirLine, centrePoint, dirRadius);
		}

		public static double FixToTouchSprial(Line starLine, Line endLine, double coefficient, double turnRadius, SideDirection turnSide)
		{
			double sinT, cosT, tmpCoef, theta0, d, d2, r, f, F1, theta1, x1, y1,
				centreTheta, fixTheta, dTheta, theta1New;
			int turnVal, i;
			Point spiralCentrePoint, outPoint;

			turnVal = -(int)(turnSide);

			tmpCoef = ARANMath.RadToDeg(coefficient);

			theta0 = ARANMath.Modulus(starLine.DirVector.Direction - ARANMath.C_PI_2 * turnVal, ARANMath.C_2xPI);
			spiralCentrePoint = PointAlongPlane(starLine.RefPoint, starLine.DirVector.Direction + ARANMath.C_PI_2 * turnVal, turnRadius);
			d = ReturnDistanceInMeters(spiralCentrePoint, endLine.RefPoint);
			fixTheta = ReturnAngleInRadians(spiralCentrePoint, endLine.RefPoint);
			dTheta = ARANMath.Modulus((fixTheta - theta0) * turnVal, ARANMath.C_2xPI);
			r = turnRadius + ARANMath.RadToDeg(dTheta) * coefficient;
			if (d < r)
				return 4;

			x1 = endLine.RefPoint.X - spiralCentrePoint.X;
			y1 = endLine.RefPoint.Y - spiralCentrePoint.Y;
			centreTheta = SpiralTouchAngle(turnRadius, coefficient, starLine.DirVector.Direction, endLine.DirVector.Direction, turnSide);
			centreTheta = ARANMath.Modulus(theta0 + centreTheta * turnVal, ARANMath.C_2xPI);

			/// ---Variant Firdowsy

			theta1 = centreTheta;
			for (i = 0; i <= 20; i++)
			{
				dTheta = ARANMath.Modulus((theta1 - theta0) * turnVal, ARANMath.C_2xPI);
				sinT = Math.Sin(theta1);
				cosT = Math.Cos(theta1);
				r = turnRadius + ARANMath.RadToDeg(dTheta) * coefficient;
				f = Math.Pow(r, 2) - (y1 * r + x1 * tmpCoef * turnVal) * sinT - (x1 * r - y1 * tmpCoef * turnVal) * cosT;
				F1 = 2 * r * tmpCoef * turnVal - (y1 * r + 2 * x1 * tmpCoef * turnVal) * cosT + (x1 * r - 2 * y1 * tmpCoef * turnVal) * sinT;
				theta1New = theta1 - (f / F1);

				d = ARANMath.SubtractAngles(theta1New, theta1);
				theta1 = theta1New;
				if (d < ARANMath.DegToRad(0.0001))
					break;
			}

			dTheta = ARANMath.Modulus((theta1 - theta0) * turnVal, ARANMath.C_2xPI);
			r = turnRadius + ARANMath.RadToDeg(dTheta) * coefficient;
			outPoint = PointAlongPlane(spiralCentrePoint, theta1, r);

			d = ReturnAngleInRadians(outPoint, endLine.RefPoint);
			centreTheta = SpiralTouchAngle(turnRadius, coefficient, starLine.DirVector.Direction, d, turnSide);
			centreTheta = ARANMath.Modulus(theta0 + centreTheta * turnVal, ARANMath.C_2xPI);
			d2 = ARANMath.SubtractAngles(centreTheta, theta1);
			if (d2 < 0.0001)
				return d;
			return 4;
		}

		public static List<Point> GetBasePoints(Point refPoint, double refDirection, Polygon TolerPoly, TurnDirection TurnSide)
		{
			const double Eps = 0.0005;      //ARANMath.EpsilonDistance * ARANMath.EpsilonDistance;

			List<Point> TmpList = new List<Point>();
			TmpList.AddRange(RemoveAgnails(TolerPoly.ExteriorRing));

			//TmpList.AddRange(RemoveAgnails(TolerPoly.ExteriorRing));

			int i, n = TmpList.Count;

			//==============================================================================
			#region Remove extra points

			double MaxDist = double.MinValue;
			int i0 = int.MinValue, i1 = 0, i2, ts = -(int)TurnSide;

			Point pt0 = TmpList[n - 1], pt1 = TmpList[0], ptTmp;

			double dx0 = pt1.X - pt0.X;
			double dy0 = pt1.Y - pt0.Y;

			while (i1 < n)
			{
				i2 = i1 + 1 < n ? i1 + 1 : 0;       //i2 = (i1 + 1) & (0 - (int)(i1 + 1 < n);

				Point pt2 = TmpList[i2];

				double dx1 = pt2.X - pt1.X;
				double dy1 = pt2.Y - pt1.Y;
				double eps = Eps * Math.Abs(dy0 * dy1);

				if (Math.Abs(Math.Abs(dx0 * dy1) - Math.Abs(dx1 * dy0)) <= eps)
				{
					TmpList.RemoveAt(i1);
					n = TmpList.Count;

					if (i1 < n)
					{
						pt1 = TmpList[i1];
						dx0 = pt1.X - pt0.X;
						dy0 = pt1.Y - pt0.Y;
					}
				}
				else
				{
					ptTmp = ARANFunctions.PrjToLocal(refPoint, refDirection, pt1);

					//if ((ptTmp.X > MaxDist) && (TurnSide== TurnDirection.NONE || Math.Sign(ptTmp.Y) == ts))
					if ((ptTmp.X > MaxDist) && Math.Sign(ptTmp.Y) == ts)
					{
						MaxDist = ptTmp.X;
						i0 = i1;
					}

					pt0 = pt1;
					pt1 = pt2;

					dx0 = dx1;
					dy0 = dy1;

					i1++;
				}
			}
			#endregion

			//==============================================================================

			#region choose first point
			n = TmpList.Count;

			//MaxDist = int.MinValue;
			//i0 = -999999;
			//for (i = 0; i < n; i++)
			//{
			//    ptTmp = ARANFunctions.PrjToLocal(refPoint, refDirection, TmpList[i]);
			//    if ((ptTmp.X > MaxDist) && (Math.Sign(ptTmp.Y) == -(int)(TurnSide)))
			//    {
			//        MaxDist = ptTmp.X;
			//        i0 = i;
			//    }
			//}

			#endregion

			List<Point> result = new List<Point>();
			pt0 = TmpList[i0];
			result.Add(pt0);

			//i = (i0 + 1) and(0 - Integer(i0 + 1 < n));

			i1 = i0 + 1 < n ? i0 + 1 : 0;
			double fDir0 = ARANMath.Modulus((Math.Atan2(TmpList[i1].Y - pt0.Y, TmpList[i1].X - pt0.X) - refDirection) * (int)TurnSide, ARANMath.C_2xPI);

			int sg = -1;
			if (ARANMath.SubtractAngles(fDir0, ARANMath.C_PI_2) < ARANMath.DegToRadValue)
				sg = 1;

			//i2 = i0 - 1 < 0 ? n - 1 : i0 - 1;
			//double fDir1 = ARANMath.Modulus((Math.Atan2(TmpList[i2].Y - pt0.Y, TmpList[i2].X - pt0.X) - refDirection) * (int)TurnSide, ARANMath.C_2xPI);

			//fDirP = fDir0;
			//else //if (ARANMath.SubtractAngles(fDir1, ARANMath.C_PI_2) < ARANMath.DegToRadValue)
			//	sg = -1;
			//fDirP = fDir1;

			ptTmp = TmpList[i1];

			//fTmp = ARANMath.Modulus((Math.Atan2(ptTmp.Y - pt0.Y, ptTmp.X - pt0.X) - refDirection) * (int)TurnSide, ARANMath.C_2xPI);
			//if (fTmp > Math.PI)	fTmp = ARANMath.C_2xPI - fTmp;
			//sg = 2 * ((int)(fTmp > ARANMath.C_2_PI) - 1) * (int)TurnSide;

			//if (ARANMath.SubtractAngles(fTmp, ARANMath.C_PI) < ARANMath.DegToRadValue || ARANMath.SubtractAngles(fTmp, ARANMath.C_PI_2) < ARANMath.DegToRadValue)
			//{
			//    i = i0 - 1 < 0 ? n - 1 : i0 - 1;
			//    ptTmp = TmpList[i];
			//    fTmp = ARANMath.Modulus((Math.Atan2(pt0.Y - ptTmp.Y, pt0.X - ptTmp.X) - refDirection) * (int)TurnSide, ARANMath.C_2xPI);
			//}
			//sg = fDirP < ARANMath.C_PI ? -1 : 1;

			i0 += n;

			for (i = 1; i < n; i++)
			{
				i1 = (i0 + sg * i) % n;
				result.Add(TmpList[i1]);
			}

			//TmpList = null;
			return result;
		}

		public static IList<int> SortArray(IList<double> items)
		{
			int i, j, count = items.Count;
			int minIndex, curIndex;
			double minVal;
			IList<int> resultIndexList, searchIndexList;

			if (count == 0)
				return null;

			resultIndexList = new List<int>(count);
			searchIndexList = new List<int>(count);

			if (count == 1)
			{
				resultIndexList[0] = 0;
				return resultIndexList;
			}

			for (i = 0; i < count; i++)
				searchIndexList.Add(i);

			for (i = 0; i < count; i++)
			{
				minVal = items[searchIndexList[0]];
				minIndex = searchIndexList[0];
				curIndex = 0;

				for (j = 0; j < searchIndexList.Count; j++)
				{
					if (items[searchIndexList[j]] < minVal)
					{
						minVal = items[searchIndexList[j]];
						minIndex = searchIndexList[j];
						curIndex = j;
					}
				}

				searchIndexList.Remove(curIndex);
				resultIndexList[i] = minIndex;
			}
			return resultIndexList;
		}

		public static MultiPoint SortPoints(Point basePoint, MultiPoint multiPoint)
		{
			int i;
			IList<double> distanceList;
			IList<int> indexList;
			MultiPoint result;

			if (multiPoint == null)
				return null;

			result = new MultiPoint();

			if (multiPoint.Count < 1)
				return result;

			distanceList = new List<double>(multiPoint.Count);

			for (i = 0; i < multiPoint.Count; i++)
				distanceList[i] = ReturnDistanceInMeters(basePoint, multiPoint[i]);

			indexList = SortArray(distanceList);

			for (i = 0; i < multiPoint.Count; i++)
				result.Add(multiPoint[indexList[i]]);
			return result;
		}


		/*
		function GeoToPrj (geoGeometry: Geometry): Geometry;
		begin
			result := GeoOper.geoTransformations (geoGeometry, GGeoSR, GPrjSR);
		end;

		function PrjToGeo (prjGeometry: Geometry): Geometry;
		begin
			result := GeoOper.geoTransformations (prjGeometry, GPrjSR, GGeoSR);
		end;
		*/
		//public void FreeObject(object sender, EventArgs e)
		//{

		//    vObject.Dispose();
		//    vObject = null;
		//}

		//public static string IfThen( bool AValue, string ATrue, string AFalse)
		//{					  		
		//  if (AValue) 
		//    return ATrue;
		//  else
		//    return AFalse;
		//}

		//public static int ShowHelp( long callerHWND, int helpContextId)
		//{
		//    int Result;
		//    Result = HtmlHelp (callerHWND, Application.HelpF, 0xF, helpContextId);
		//    return Result;
		//}

		public static Ring ToleranceArea(Point fixPoint, double aTTDistance, double xTTDistance, double radian, SideDirection turn)
		{
			Point pt1, pt2, pt3, pt4;
			Ring toleranceArea = new Ring();

			pt1 = LocalToPrj(fixPoint, radian, -aTTDistance, (int)turn * xTTDistance);
			pt2 = LocalToPrj(fixPoint, radian, -aTTDistance, (int)(ARANMath.ChangeDirection(turn)) * xTTDistance);
			pt3 = LocalToPrj(fixPoint, radian, aTTDistance, (int)(ARANMath.ChangeDirection(turn)) * xTTDistance);
			pt4 = LocalToPrj(fixPoint, radian, aTTDistance, (int)turn * xTTDistance);
			toleranceArea.Add(pt1);
			toleranceArea.Add(pt2);
			toleranceArea.Add(pt3);
			toleranceArea.Add(pt4);
			return toleranceArea;
		}
		#endregion

		public static Polygon CreateCircleGeo(Aran.Geometries.Point ptCntGeo, double radius, int n = 360)
		{
			Aran.Geometries.Ring ring = new Aran.Geometries.Ring();
			Aran.Geometries.Polygon result = new Aran.Geometries.Polygon();

			double AngleStep = 360.0 / n;

			for (int i = 0; i < n; i++)
			{
				Aran.Geometries.Point pnt = PointAlongGeodesic(ptCntGeo, i * AngleStep, radius);
				ring.Add(pnt);
			}

			ring.Add(ring[0]);
			result.ExteriorRing = ring;
			return result;
		}

		public static MultiPoint CreateCircle(Point ptCnt, double radius, int n = 360)
		{
			MultiPoint result = new MultiPoint();
			double AngleStep = ARANMath.C_2xPI / n;

			for (int i = n; i > 0; i--)
			{
				double iInRad = i * AngleStep;
				double X = ptCnt.X + radius * Math.Cos(iInRad);
				double Y = ptCnt.Y + radius * Math.Sin(iInRad);

				Point Pt = new Point(X, Y, ptCnt.Z);

				Pt.M = ptCnt.M;
				Pt.T = ptCnt.T;

				result.Add(Pt);
			}

			result.Add(result[0]);
			return result;
		}

		public static double DmsStr2Dd(string str, bool isLat)
		{
			double xDeg;
			int degLength;

			if (isLat)
			{
				degLength = 2;
				xDeg = Convert.ToDouble(str.Substring(0, 2));
			}
			else
			{
				degLength = 3;
				xDeg = Convert.ToDouble(str.Substring(0, 3));
			}

			double xMin = Convert.ToDouble(str.Substring(degLength, 2));
			double xSec = Convert.ToDouble(str.Substring(degLength + 2, str.Length - degLength - 2));

			int Sign = 1;
			double x = System.Math.Round(Sign * (System.Math.Abs(xDeg) + System.Math.Abs(xMin / 60.0) + System.Math.Abs(xSec / 3600.0)), 10);
			return System.Math.Abs(x);
		}

		public static void Dd2DmsStr(double longtitude, double latitude, string decSep, string lonSide, string latSide,
			int sign, int resolution, out string longtitudeStr, out string latitudeStr)
		{
			double x = System.Math.Abs(System.Math.Round(System.Math.Abs(longtitude) * sign, 10));

			double xDeg = Fix(x);
			double dx = Math.Round((x - xDeg) * 60, 8);

			double xMin = Fix(dx);
			double xSec = Math.Round((dx - xMin) * 60, 6);

			int Res = Convert.ToInt32(Math.Pow(10, resolution));
			string xDegStr;

			if (xDeg < 10)
				xDegStr = "00" + xDeg;
			else if (xDeg < 100)
				xDegStr = "0" + xDeg;
			else
				xDegStr = xDeg.ToString(CultureInfo.InvariantCulture);

			xDegStr = xDegStr + "°";

			string xMinStr = xMin.ToString(CultureInfo.InvariantCulture);
			if (xMin < 10)
				xMinStr = "0" + xMin;

			xMinStr = xMinStr + "'";

			xSec = Math.Round(xSec * Res) / Res;
			string xSecStr = Math.Truncate(xSec).ToString(CultureInfo.InvariantCulture);

			xSecStr = xSecStr + decSep;

			//this line adding for writing 8 seconds  as 08 seconds
			if (xSec < 10)
				xSecStr = 0 + xSecStr;

			xSec = Math.Round((xSec - Math.Truncate(xSec)) * Res);

			xSecStr = xSecStr + xSec.ToString(CultureInfo.InvariantCulture);

			if (resolution == 0)
			{
				xSecStr = xSecStr.Remove(xSecStr.IndexOf("."), 2);
			}

			xSecStr = xSecStr + "''" + lonSide;
			//xSecStr = xSecStr + lonSide;
			longtitudeStr = xDegStr + xMinStr + xSecStr;


			double y = System.Math.Abs(System.Math.Round(System.Math.Abs(latitude) * sign, 10));

			double yDeg = Math.Truncate(y);
			double dy = Math.Round((y - yDeg) * 60, 8);

			double yMin = Math.Truncate(dy);
			double ySec = Math.Round((dy - yMin) * 60, 6);

			string yDegStr = yDeg.ToString(CultureInfo.InvariantCulture);
			if (yDeg < 10)
				yDegStr = "0" + yDeg.ToString(CultureInfo.InvariantCulture);

			yDegStr = yDegStr + "°";

			string yMinStr = yMin.ToString(CultureInfo.InvariantCulture);
			if (yMin < 10)
				yMinStr = "0" + yMin.ToString(CultureInfo.InvariantCulture);
			yMinStr = yMinStr + "'";

			ySec = Math.Round(ySec * Res) / Res;
			string ySecStr = Math.Truncate(ySec).ToString(CultureInfo.InvariantCulture);

			ySecStr = ySecStr + decSep;

			//this line adding for writing 8 seconds  as 08 seconds
			if (ySec < 10)
				ySecStr = 0 + ySecStr;

			ySec = Math.Round((ySec - Math.Truncate(ySec)) * Res, resolution);

			ySecStr = ySecStr + ySec.ToString(CultureInfo.InvariantCulture);

			if (resolution == 0)
			{
				ySecStr = ySecStr.Remove(ySecStr.IndexOf("."), 2);
			}
			ySecStr = ySecStr + "''" + latSide;
			//ySecStr = ySecStr + latSide;
			latitudeStr = yDegStr + yMinStr + ySecStr;
		}

		public static string Degree2String(double x, Degree2StringMode mode, int accuracyLength = 2)
		{
			string sSign = "", sResult = "", sTmp;
			double xDeg, xMin, xIMin, xSec;
			bool lSign = false;

			string accuracy = "00";
			for (int i = 0; i < accuracyLength; i++)
			{
				if (i == 0)
					accuracy += ".";
				accuracy += "0";
			}

			if (mode == Degree2StringMode.DMSLat)
			{
				lSign = Math.Sign(x) < 0;
				if (lSign)
					x = -x;

				xDeg = System.Math.Floor(x);
				xMin = (x - xDeg) * 60.0;
				xIMin = System.Math.Floor(xMin);
				xSec = (xMin - xIMin) * 60.0;   //		xSec = System.Math.Round((xMin - xIMin) * 60.0, 2);
				if (xSec >= 60.0)
				{
					xSec = 0.0;
					xIMin++;
				}

				if (xIMin >= 60.0)
				{
					xIMin = 0.0;
					xDeg++;
				}

				sTmp = xDeg.ToString("00");
				sResult = sTmp + "°";

				sTmp = xIMin.ToString("00");
				sResult = sResult + sTmp + "'";

				sTmp = xSec.ToString(accuracy);
				sResult = sResult + sTmp + @"""";

				return sResult + (lSign ? "S" : "N");
			}

			if (mode >= Degree2StringMode.DMSLon)
			{
				x = NativeMethods.Modulus(x);
				lSign = x > 180.0;
				if (lSign) x = 360.0 - x;

				xDeg = System.Math.Floor(x);
				xMin = (x - xDeg) * 60.0;
				xIMin = System.Math.Floor(xMin);
				xSec = (xMin - xIMin) * 60.0;
				if (xSec >= 60.0)
				{
					xSec = 0.0;
					xIMin++;
				}

				if (xIMin >= 60.0)
				{
					xIMin = 0.0;
					xDeg++;
				}

				sTmp = xDeg.ToString("000");
				sResult = sTmp + "°";

				sTmp = xIMin.ToString("00");
				sResult = sResult + sTmp + "'";

				sTmp = xSec.ToString(accuracy);
				sResult = sResult + sTmp + @"""";

				return sResult + (lSign ? "W" : "E");
			}

			if (System.Math.Sign(x) < 0) sSign = "-";
			x = NativeMethods.Modulus(System.Math.Abs(x));

			switch (mode)
			{
				case Degree2StringMode.DD:
					return sSign + x.ToString("#0.00##") + "°";
				case Degree2StringMode.DM:
					if (System.Math.Sign(x) < 0) sSign = "-";
					x = NativeMethods.Modulus(System.Math.Abs(x));

					xDeg = System.Math.Floor(x);
					xMin = (x - xDeg) * 60.0;
					if (xMin >= 60)
					{
						x++;
						xMin = 0;
					}

					sResult = sSign + xDeg.ToString() + "°";

					sTmp = xMin.ToString("00.00##");
					return sResult + sTmp + "'";
				case Degree2StringMode.DMS:
					if (System.Math.Sign(x) < 0) sSign = "-";
					x = NativeMethods.Modulus(System.Math.Abs(x));

					xDeg = System.Math.Floor(x);
					xMin = (x - xDeg) * 60.0;
					xIMin = System.Math.Floor(xMin);
					xSec = (xMin - xIMin) * 60.0;
					if (xSec >= 60.0)
					{
						xSec = 0.0;
						xIMin++;
					}

					if (xIMin >= 60.0)
					{
						xIMin = 0.0;
						xDeg++;
					}

					sResult = sSign + xDeg.ToString() + "°";

					sTmp = xIMin.ToString("00");
					sResult = sResult + sTmp + "'";

					sTmp = xSec.ToString("00.00");
					return sResult + sTmp + @"""";
			}
			return sResult;
		}

		private static int Fix(double x)
		{
			return (int)(System.Math.Sign(x) * System.Math.Floor(System.Math.Abs(x)));
		}
	}

	public enum Degree2StringMode
	{
		DD,
		DM,
		DMS,
		DMSLat, //NS
		DMSLon  //EW
	}
}


/*
─────────────────────────▄▀▄  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─▀█▀█▄  
─────────────────────────█──█──█  
─────────────────────────█▄▄█──▀█  
────────────────────────▄█──▄█▄─▀█  
────────────────────────█─▄█─█─█─█  
────────────────────────█──█─█─█─█  
────────────────────────█──█─█─█─█  
────▄█▄──▄█▄────────────█──▀▀█─█─█  
──▄█████████────────────▀█───█─█▄▀  
─▄███████████────────────██──▀▀─█  
▄█████████████────────────█─────█  
██████████───▀▀█▄─────────▀█────█  
████████───▀▀▀──█──────────█────█  
██████───────██─▀█─────────█────█  
████──▄──────────▀█────────█────█ Look dude,
███──█──────▀▀█───▀█───────█────█ a good code!
███─▀─██──────█────▀█──────█────█  
███─────────────────▀█─────█────█  
███──────────────────█─────█────█  
███─────────────▄▀───█─────█────█  
████─────────▄▄██────█▄────█────█  
████────────██████────█────█────█  
█████────█──███████▀──█───▄█▄▄▄▄█  
██▀▀██────▀─██▄──▄█───█───█─────█  
██▄──────────██████───█───█─────█  
─██▄────────────▄▄────█───█─────█  
─███████─────────────▄█───█─────█  
──██████─────────────█───█▀─────█  
──▄███████▄─────────▄█──█▀──────█  
─▄█─────▄▀▀▀█───────█───█───────█  
▄█────────█──█────▄███▀▀▀▀──────█  
█──▄▀▀────────█──▄▀──█──────────█  
█────█─────────█─────█──────────█  
█────────▀█────█─────█─────────██  
█───────────────█──▄█▀─────────█  
█──────────██───█▀▀▀───────────█  
█───────────────█──────────────█  
█▄─────────────██──────────────█  
─█▄────────────█───────────────█  
──██▄────────▄███▀▀▀▀▀▄────────█  
─█▀─▀█▄────────▀█──────▀▄──────█  
─█────▀▀▀▀▄─────█────────▀─────█
*/