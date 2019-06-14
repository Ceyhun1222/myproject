using System;
using Aran.Geometries;
using Aran.Aim.Features;
using Aran.PANDA.Common;

namespace Aran.PANDA.Conventional.Racetrack
{
	public static class GeomFunctions
	{

		//public static Point GmlToAranPoint(Feature feature)
		//{
		//    return null;
		//    //List<GeometryObject> geomObjects = GlobalParams.Database.HoldingQpi.GetFeatureGeometries(feature);
		//    //return (from s in geomObjects
		//    //        where s != null && s.geometryType == Delib.Classes.Enums.GeometryType.Point
		//    //        select Assign((Delib.Classes.GeomObjects.Point)s)).FirstOrDefault();

		//}

		//public static Point GmlToAranPointPrj(Feature feature)
		//{
		//    //List<GeometryObject> geomObjects = GlobalParams.Database.HoldingQpi.GetFeatureGeometries(feature);

		//    //return (from s in geomObjects
		//    //        where s.geometryType == Delib.Classes.Enums.GeometryType.Point
		//    //        select AssignToPrj((Delib.Classes.GeomObjects.Point)s)).FirstOrDefault();

		//    return null;

		//}

		public static Point Assign ( AixmPoint dPoint )
		{
			Point aranPoint = new Point
			{
				X = dPoint.Geo.X,
				Y = dPoint.Geo.Y
			};
			return aranPoint;
		}

		public static Point AssignToPrj ( AixmPoint aixmPoint )
		{
			Point aranPoint = new Point
			{
				X = aixmPoint.Geo.X,
				Y = aixmPoint.Geo.Y
			};
			return GlobalParams.SpatialRefOperation.ToPrj<Point> ( aranPoint );
		}

		public static Point Assign ( ElevatedPoint elevPoint )
		{
			Point aranPoint = new Point
			{
				X = elevPoint.Geo.X,
				Y = elevPoint.Geo.Y
			};
			return aranPoint;
		}

		public static Point AssignToPrj ( ElevatedPoint aixmPoint )
		{
			Point aranPoint = new Point
			{
				X = aixmPoint.Geo.X,
				Y = aixmPoint.Geo.Y
			};
			return GlobalParams.SpatialRefOperation.ToPrj<Point> ( aranPoint );
		}

		public static Point AimToAranPrj ( Point aranPt )
		{
			if ( aranPt == null )
				return null;
			return GlobalParams.SpatialRefOperation.ToPrj<Point> ( new Point ( aranPt.X, aranPt.Y ) );
		}

		public static AixmPoint AranPointPrjToAixm ( Point ptPrj )
		{
			if ( ptPrj == null )
				return null;
			Point ptGeo = GlobalParams.SpatialRefOperation.ToGeo<Point> ( ptPrj );
			AixmPoint aixmPoint = new AixmPoint ( );
			aixmPoint.Geo.X = ptGeo.X;
			aixmPoint.Geo.Y = ptGeo.Y;
			aixmPoint.Geo.Z = ptGeo.Z;

			return aixmPoint;
		}

		public static Polygon MoveShablonAroundTolArea ( Polygon shablon, Ring toleranceArea, Point ptnavPrj )
		{
			Polygon [] tmpP = new Polygon [ toleranceArea.Count ];

			for ( int i = 0; i < toleranceArea.Count; i++ )
			{
				tmpP [ i ] = ( Polygon ) TransForm.Move ( shablon, ptnavPrj, toleranceArea [ i ] );
			}

			return ChainHullAlgorithm.ConvexHull ( tmpP [ 0 ], tmpP [ 1 ], tmpP [ 2 ], tmpP [ 3 ] );

		}

		public static LineString MaxPointsFromArcToSpiral ( Point ptCenter, Ring arc, LineString spiral )
		{
			LineString maxPoints = new LineString ( );
			foreach ( Point toPoint in arc )
			{
				LineString tmpPart = ( LineString ) TransForm.Move ( spiral, ptCenter, toPoint );
				Point tmpPoint;
				MaxDistFromPointToGeometry ( ptCenter, tmpPart, out tmpPoint );
				maxPoints.Add ( tmpPoint );
			}
			return maxPoints;
		}
		

		public static Point MaxDistFromLineToMultipoint ( Point refpnt, double direction, MultiPoint multipoint )
		{
			Point result = new Point ( );
			if ( multipoint.IsEmpty )
				return result;	
			double maxDist = ARANFunctions.PointToLineDistance ( multipoint [ 0 ], refpnt, direction );
			foreach ( Point pnt in multipoint )
			{
				double distance = ARANFunctions.PointToLineDistance ( pnt, refpnt, direction );
				if ( distance > maxDist )
				{
					result = pnt;
					maxDist = distance;
				}
			}
			return result;
		}

		public static Point MinDistFromLineToMultipoint ( Point refpnt, double direction, MultiPoint multipoint )
		{
			Point result = new Point ( );
			if ( multipoint.IsEmpty )
				return result;
			double minDist = ARANFunctions.PointToLineDistance ( multipoint [ 0 ], refpnt, direction );
			foreach ( Point pnt in multipoint )
			{
				double distance = ARANFunctions.PointToLineDistance ( pnt, refpnt, direction );
				if ( distance < minDist )
				{
					result = pnt;
					minDist = distance;
				}
			}
			return result;
		}

		public static MultiPoint MinAndMaxDistFromLineToMultipoint ( Point refpnt, double direction, MultiPoint multipoint )
		{
			MultiPoint result = new MultiPoint ( );
			if ( multipoint.IsEmpty)
				return result;
			var minPnt = multipoint [ 0 ];
			var maxPnt = minPnt;
			double minDist = ARANFunctions.PointToLineDistance ( minPnt, refpnt, direction );
			double maxDist = minDist;
			//int i = 0;
			foreach (Point pnt in multipoint )
			{
				double distance = ARANFunctions.PointToLineDistance ( pnt, refpnt, direction );
				if ( distance > maxDist )
				{
					maxPnt = pnt;
					maxDist = distance;
				}
				if ( distance < minDist )
				{
					minPnt = pnt;
					minDist = distance;
				}
				//i++;
			}
			result.Add ( minPnt );
			result.Add ( maxPnt );
			return result;
		}

		private static double MinDistFromPointToGeometry ( Point fromPoint, MultiPoint mltPnt, out Point result )
		{
			double minDistance = 1000000000000000000;
			result = new Point ( );
			foreach ( Point toPoint in mltPnt )
			{
				var dx = toPoint.X - fromPoint.X;
				var dy = toPoint.Y - fromPoint.Y;

				var distance = dx * dx + dy * dy;
				if ( distance <= minDistance )
				{
					minDistance = distance;
					result = toPoint;
				}
			}
			return Math.Sqrt ( minDistance );
		}

		public static double MinDistFromPointToGeometry ( Point fromPoint, LineString lnString, out Point result )
		{
			return MinDistFromPointToGeometry ( fromPoint, ( MultiPoint ) lnString, out result );
		}

		public static double MinDistFromPointToGeometry ( Point fromPoint, MultiLineString mltLineString, out Point result )
		{
			return MinDistFromPointToGeometry ( fromPoint, mltLineString.ToMultiPoint ( ), out result );
		}


		private static double MaxDistFromPointToGeometry ( Point fromPoint, MultiPoint mltPnt, out Point result )
		{
			double maxDistance = 0;
			result = new Point ( );
			foreach ( Point toPoint in mltPnt )
			{
				var dx = toPoint.X - fromPoint.X;
				var dy = toPoint.Y - fromPoint.Y;

				var distance = dx * dx + dy * dy;
				if ( distance >= maxDistance )
				{
					maxDistance = distance;
					result = toPoint;
				}
			}
			return Math.Sqrt ( maxDistance );
		}

		public static double MaxDistFromPointToGeometry ( Point fromPoint, LineString lnString, out Point result )
		{
			return MaxDistFromPointToGeometry ( fromPoint, ( MultiPoint ) lnString, out result );
		}

		public static double MaxDistFromPointToGeometry ( Point fromPoint, MultiLineString mltLineString, out Point result )
		{
			return MaxDistFromPointToGeometry ( fromPoint, mltLineString.ToMultiPoint ( ), out result );
		}

		
		public static LineString BaseArea5 ( Point ptCenter, Point fromPt, Point endPt )
		{
			LineString baseArea5 = new LineString ( );
			double rad1 = ARANFunctions.ReturnAngleInRadians ( ptCenter, fromPt );
			double rad2 = ARANFunctions.ReturnAngleInRadians ( ptCenter, endPt );
			double n = Math.Abs ( rad2 - rad1 ) * 180 / ARANMath.C_PI;
			var distance1 = ARANFunctions.ReturnDistanceInMeters ( ptCenter, fromPt );
			var distance2 = ARANFunctions.ReturnDistanceInMeters ( ptCenter, endPt );
			double incDist = ( distance2 - distance1 ) / n;
			for ( int i = 0; i < n; i++ )
			{
				Point pt = ARANFunctions.LocalToPrj ( ptCenter, rad1 - i * ARANMath.C_PI / 180, distance1 + i * incDist, 0 );
				baseArea5.Add ( pt );
			}
			return baseArea5;

		}

		public static LineString PointsOnGeometry ( LineString part, Point pt )
		{
			LineString result = new LineString ( );
			foreach ( Point item in part )
			{
				result.Add ( item );
				if ( ( item.X == pt.X ) & ( item.Y == pt.Y ) )
					return result;
			}
			return null;
		}

		public static Polygon MoveGeometryAroundTwoPoint ( Geometry geom, Point geomCenterPoint, Point fromPt, Point toPt, TurnDirection turn )
		{
			Ring arc = ARANFunctions.CreateArcPrj ( geomCenterPoint, fromPt, toPt, ARANMath.ChangeDirection ( turn ) );
			Geometry [] geomCollection = new Geometry [ arc.Count ];
			for ( int i = 0; i < arc.Count; i++ )
			{
				geomCollection [ i ] = TransForm.Move ( geom, geomCenterPoint, arc [ i ] );
			}
			return ChainHullAlgorithm.ConvexHull ( geomCollection );
		}

		public static Ring AimToAranRingPrj ( Ring aimRing )
		{
			Ring result = new Ring ( );
			for ( int i = 0; i < aimRing.Count; i++ )
			{
				Point ptPrj = GlobalParams.SpatialRefOperation.ToPrj<Point> ( aimRing [ i ] );
				result.Add ( ptPrj );
			}
			return result;
		}

		public static Ring AranToAimRingGeo ( Ring aranRing )
		{
			Ring result = new Ring ( );
			for ( int i = 0; i < aranRing.Count; i++ )
			{
				Point ptGeo = GlobalParams.SpatialRefOperation.ToGeo<Point> ( aranRing [ i ] );
				result.Add ( ptGeo );
			}
			return result;
		}

		public static Curve ConvertPolylineToCurve ( MultiLineString mltLineString )
		{
			Curve curve = new Curve ( );
			foreach ( LineString item in mltLineString )
			{
				curve.Geo.Add ( item );
			}
			return curve;
		}

		public static Curve ConvertToCurve ( MultiPoint mltPnt )
		{
			Curve curve = new Curve ( );
			LineString lnString = new LineString ( );
			foreach ( Point pnt in mltPnt )
			{
				lnString.Add ( pnt );
			}
			curve.Geo.Add ( lnString );
			return curve;
		}

		public static Surface ConvertPolygonToSurface ( Polygon polygon )
		{
			Surface surface = new Surface ();
			surface.Geo.Add ( polygon );
			return surface;
		}

		public static Surface ConvertMultiPolygonToSurface ( MultiPolygon mltPolygon )
		{
			Surface surface = new Surface ( );
			foreach ( Polygon polygon in mltPolygon )
			{
				surface.Geo.Add ( polygon );
			}
			return surface;
		}
	}
}