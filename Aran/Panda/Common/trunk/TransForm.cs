using System;
using Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.PANDA.Common
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
					return RotatePoint ( ptCenter, ( Point ) geom );
				
				case GeometryType.Vector:
					return RotateVector ( ( Vector ) geom );
				
				case GeometryType.Line:
					Line line = new Line ( );
					line.RefPoint = RotatePoint ( ptCenter, ( ( Line ) geom ).RefPoint );
					line.DirVector = RotateVector ( ( ( Line ) geom ).DirVector );
					return line;
				
				case GeometryType.LineString:
					LineString lnString = new LineString ( );
					lnString.AddMultiPoint ( RotateMultiPoint ( ptCenter, ( MultiPoint ) geom ) );
					return lnString;

				case GeometryType.Ring:
					Ring rng = new Ring ( );
					rng.AddMultiPoint ( RotateMultiPoint ( ptCenter, ( MultiPoint ) geom ) );
					return rng;

				case GeometryType.MultiPoint:
					return RotateMultiPoint ( ptCenter, ( MultiPoint ) geom );
				
				case GeometryType.MultiLineString:
					return RotatePolyLine ( ptCenter, ( MultiLineString ) geom );
				case GeometryType.MultiPolygon:
					return RotateMultiPolygon ( ptCenter, ( MultiPolygon ) geom );
				case GeometryType.Polygon:
					return RotatePolygon ( ptCenter, ( Polygon ) geom );
			}
			throw new Exception ( "Geometry Type is not implemented (in RotateGeometry)!" );
		}

		private static Point RotatePoint ( Point ptCenter, Point pt )
		{
			double dx = pt.X - ptCenter.X;
			double dy = pt.Y - ptCenter.Y;
			double x = ptCenter.X + dx * Math.Cos ( _rad ) - dy * Math.Sin ( _rad );
			double y = ptCenter.Y + dx * Math.Sin ( _rad ) + dy * Math.Cos ( _rad );
			Point result = new Point ( x, y );
			return result;
		}

		private static MultiPoint RotateMultiPoint(Point ptCenter, MultiPoint geom)
		{
			MultiPoint result = new MultiPoint ( );
			for (int i = 0; i < geom.Count; i++)
			{
				result.Add ( RotatePoint ( ptCenter, geom [ i ] ) );
			}
			return result;
		}

		private static Polygon RotatePolygon ( Point pntCenter, Polygon sourcePolygon )
		{
			Polygon result = new Polygon ( );
			Ring resulExtRng = new Ring ( );
			resulExtRng.AddMultiPoint ( RotateMultiPoint ( pntCenter, sourcePolygon.ExteriorRing ) );
			result.ExteriorRing = resulExtRng;

			foreach ( Ring ring in sourcePolygon.InteriorRingList )
			{
				Ring resultRng = new Ring ( );
				resultRng.AddMultiPoint ( RotateMultiPoint ( pntCenter, ring ) );
				result.InteriorRingList.Add ( resultRng );
			}
			return result;
		}

		private static MultiPolygon RotateMultiPolygon(Point ptCenter, MultiPolygon mltPolygon)
		{
			MultiPolygon resultMultiPolygon = new MultiPolygon ( );
			foreach ( Polygon polygon in mltPolygon )
			{
				Polygon rotatedPolygon = RotatePolygon ( ptCenter, polygon );
				resultMultiPolygon.Add ( rotatedPolygon );
			}
			return resultMultiPolygon;
		}

		private static MultiLineString RotatePolyLine(Point ptCenter, MultiLineString complexGeo)
		{
			MultiLineString result = new MultiLineString ( );
			for (int i = 0; i < complexGeo.Count; i++)
			{
				result.Add ( ( LineString ) RotateMultiPoint ( ptCenter, complexGeo [ i ] ) );
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
		
		public static Geometry Move(Geometry geom,Point fromPt,Point toPt)
		{
			double dx = toPt.X - fromPt.X;
			double dy = toPt.Y - fromPt.Y;
			
			switch (geom.Type)
			{
			
				case GeometryType.Point:
					return MovePoint ( ( Point ) geom, dx, dy );
				
				case GeometryType.LineString:
					LineString lnString = new LineString ( );
					lnString.AddMultiPoint ( MoveMultiPoint ( ( MultiPoint ) geom, dx, dy ) );
					return lnString;

				case GeometryType.Ring:
					Ring rng = new Ring ( );
					rng.AddMultiPoint ( MoveMultiPoint ( ( MultiPoint ) geom, dx, dy ) );
					return rng;

				case GeometryType.MultiPoint:
					return MoveMultiPoint ( ( MultiPoint ) geom, dx, dy );
				
				case GeometryType.MultiLineString:
					return MovePolyLine ( ( MultiLineString ) geom, dx, dy );
				case GeometryType.Polygon:
					return MovePolygon ( ( Polygon ) geom, dx, dy );
			}
			throw new Exception ( "Geometry Type is not implemented (in Move) !" );
		}

		private static Point MovePoint(Point pt, double dX, double dY)
		{
			Point result = new Point ( pt.X + dX, pt.Y + dY );
			return result;
		}

		private static MultiPoint MoveMultiPoint(MultiPoint geom, double dX, double dY)
		{
			MultiPoint result = new MultiPoint ( );
			for (int i = 0; i < geom.Count; i++)
			{
				result.Add ( new Point ( geom [ i ].X + dX, geom [ i ].Y + dY ) );
			}
			return result;
		}

		private static Polygon MovePolygon(Polygon polygon, double dX, double dY)
		{
			Polygon result = new Polygon ( );

			Ring extRng = new Ring ( );
			extRng.AddMultiPoint ( MoveMultiPoint ( polygon.ExteriorRing, dX, dY ) );
			result.ExteriorRing = extRng;

			for (int i = 0; i < polygon.InteriorRingList.Count; i++)
			{
				Ring rng = new Ring ( );
				rng.AddMultiPoint ( MoveMultiPoint ( polygon.InteriorRingList [ i ], dX, dY ) );
				result.InteriorRingList.Add ( rng );
			}
			return result;
		}

		private static MultiLineString MovePolyLine(MultiLineString geom, double dX, double dY)
		{
			MultiLineString result = new MultiLineString ( );
			for (int i = 0; i < geom.Count; i++)
			{
				LineString lnString = new LineString ( );
				lnString.AddMultiPoint ( MoveMultiPoint ( geom [ i ], dX, dY ) );
				result.Add (lnString);
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
					return FlipPoint ( ( Point ) geom, linePoint, dirRadian );
								
				case GeometryType.LineString:
					LineString lnString = new LineString ( );
					lnString.AddMultiPoint ( FlipMultiPoint ( ( MultiPoint ) geom, linePoint, dirRadian ) );
					return lnString;

				case GeometryType.Ring:
					Ring rng = new Ring ( );
					rng.AddMultiPoint ( FlipMultiPoint ( ( MultiPoint ) geom, linePoint, dirRadian ) );
					return rng;

				case GeometryType.MultiPoint:
					return FlipMultiPoint ( ( MultiPoint ) geom, linePoint, dirRadian );

				case GeometryType.MultiLineString:
					return FlipPolyLine ( ( MultiLineString ) geom, linePoint, dirRadian );
				case GeometryType.Polygon:
					return FlipPolygon ( ( Polygon ) geom, linePoint, dirRadian );
			}
			throw new Exception ( "Geometry Type is not implemented ( in Flip)!" );
		}

		private static Point FlipPoint(Point pt,Point linePoint,double dirRadian)
		{
			double distance;
			distance = ARANFunctions.PointToLineDistance ( pt, linePoint, dirRadian );
			return ARANFunctions.LocalToPrj ( pt, dirRadian, 0, -distance * 2 );
		}

		private static MultiPoint FlipMultiPoint(MultiPoint geomMultiPoint, Point linePoint, double dirRadian)
		{
			MultiPoint result = new MultiPoint ( );
			double distance;
			for (int i = 0; i < geomMultiPoint.Count; i++)
			{
				distance = ARANFunctions.PointToLineDistance(geomMultiPoint[i], linePoint, dirRadian);
				result.Add ( ARANFunctions.LocalToPrj ( geomMultiPoint [ i ], dirRadian, 0, -distance * 2 ) );
			}
			return result;
		}

		private static Polygon FlipPolygon(Polygon polygon, Point linePoint, double dirRadian)
		{
			Polygon result = new Polygon ( );
			Ring extRing = new Ring ( );
			extRing.AddMultiPoint ( FlipMultiPoint ( polygon.ExteriorRing, linePoint, dirRadian ) );
			result.ExteriorRing = extRing;

			for (int i = 0; i < polygon.InteriorRingList.Count; i++)
			{
				Ring rng = new Ring();
				rng.AddMultiPoint ( FlipMultiPoint ( polygon.InteriorRingList [ i ], linePoint, dirRadian ) );
				result.InteriorRingList.Add ( rng );
			}
			return result;
			
		}
		
		private static MultiLineString FlipPolyLine(MultiLineString polyLine, Point linePoint, double dirRadian)
		{
			MultiLineString result = new MultiLineString ( );
			for (int i = 0; i < polyLine.Count; i++)
			{	
				LineString lnString = new LineString ( );
				lnString.AddMultiPoint ( FlipMultiPoint ( polyLine [ i ], linePoint, dirRadian ) );
				result.Add ( lnString );
			}
			return result;
		}
		
		#endregion

		#region QueryCoords

		public static Box QueryCoords(Geometry geom)
		{
			switch (geom.Type)
			{
				case GeometryType.Point:
					Box _geoMinMaxPoint = new Box();
					_geoMinMaxPoint [ 0 ] = ( Point ) geom;
					_geoMinMaxPoint [ 1 ] = ( Point ) geom;
					return _geoMinMaxPoint;
				case GeometryType.MultiPoint:
				case GeometryType.LineString:
				case GeometryType.Ring:
					return PointsMinMax ( ( MultiPoint ) geom );
				case GeometryType.Polygon:
					return MinMaxPolygon ( ( Polygon ) geom );
				case GeometryType.MultiLineString:
					return MinMaxPolyline ( ( MultiLineString ) geom );
				default:
					throw new Exception ( "Geometry Type is not implemented ( in QueryCoords)" );
			}
		}

		private static Box MinMaxPolyline(MultiLineString geom)
		{
			return PointsMinMax ( geom.ToMultiPoint ( ) );
		}

		private static Box MinMaxPolygon(Polygon geom)
		{
			return PointsMinMax ( geom.ToMultiPoint ( ) );
		}

		
		private static Box PointsMinMax(MultiPoint mPoint)
		{
			Box _geoMinMaxPoint = new Box ( );
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
		//private static GeoMinMaxPoint _geoMinMaxPoint;
	}
}
