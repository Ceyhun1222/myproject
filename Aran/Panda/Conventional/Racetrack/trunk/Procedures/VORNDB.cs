using System;
using System.Collections.Generic;
using System.Globalization;
using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.AranEnvironment.Symbols;
using Aran.Geometries.Operators;
using GeoAPI.Geometries;

namespace Aran.PANDA.Conventional.Racetrack.Procedures
{
	public class Vorndb
	{
		public Vorndb ( MainController controller )
		{
			_controller = controller;
			_toleranceArea = new Ring ( );
		}

		public Polygon ConstructBasicArea(NavaidPntPrj navPntPrj, double altitude, double direction, double tas,
			SideDirection side, double time, bool drawShablon, bool drawNominalTraject, bool drawToleranceArea)
		{
			_navPntPrj = navPntPrj;
			_direction = direction;
			_side = side;
			_altitude = altitude;

			Point[] toleranceAreaPnts = ConstructFixToleranceArea(_direction, out _radius);

			_toleranceArea.Clear();
			_toleranceArea.Add(toleranceAreaPnts[0]);
			_toleranceArea.Add(toleranceAreaPnts[1]);
			_toleranceArea.Add(toleranceAreaPnts[3]);
			_toleranceArea.Add(toleranceAreaPnts[2]);
			_toleranceArea.Add(toleranceAreaPnts[0]);

			if (navPntPrj.Type == NavType.Vor)
				_priorFixTolerance = Math.Cos(ARANMath.DegToRad(VorTolerance)) * _radius;
			else
				_priorFixTolerance = Math.Cos(ARANMath.DegToRad(NdbTolerance)) * _radius;

			_postFixTolerance = ARANFunctions.PointToLineDistance ( navPntPrj.Value, toleranceAreaPnts[ 0 ], ARANFunctions.ReturnAngleInRadians ( toleranceAreaPnts[ 0 ], toleranceAreaPnts[ 2 ] ) );

			Point pntR;

			Shablons.HoldingShablon ( _controller, navPntPrj.Value, tas, altitude, time, _direction, side, 0, out _nominalTrack, out _shablon, out _line3, out _turn180, out _pntE, out pntR );

			LineString lnStringTemplate = new LineString ( );
			if ( GlobalParams.IsTestVersion )
			{
				lnStringTemplate.Add ( _pntE );
			}
			lnStringTemplate.Add ( navPntPrj.Value );
			lnStringTemplate.AddMultiPoint ( _shablon.ToMultiPoint ( ) );

			_controller.SafeDeleteGraphic ( _templateHandle );
			if ( drawShablon )
				_templateHandle = _controller.DrawLineString ( lnStringTemplate, 255, 1 );

			_controller.SafeDeleteGraphic ( _nominalTrackHandle );
			if ( drawNominalTraject )
				_nominalTrackHandle = _controller.DrawLineString ( _nominalTrack, 1, 1 );

			//_controller.SafeDeleteGraphic ( _turn180Handle );
			//_turn180Handle = _controller.DrawLineString ( turn180, 255 * 255 * 255, 1 );
			GlobalParams.ClearTestVersionElements ( );
			if ( GlobalParams.IsTestVersion )
			{
				foreach ( Point pnt in toleranceAreaPnts )
					GlobalParams.TestVersionHandles.Add ( _controller.DrawPoint ( pnt, 1 ) );
			}
			GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( _pntE, 1, "E" ) );

			_controller.SafeDeleteGraphic ( _toleranceAreaHandle );
			if ( drawToleranceArea )
				_toleranceAreaHandle = _controller.DrawRing ( _toleranceArea, 1, eFillStyle.sfsHollow );

			MultiPoint convexCurve1234 = new MultiPoint ( );

			Polygon curve1 = ( ( Polygon ) TransForm.Move ( _shablon, navPntPrj.Value, toleranceAreaPnts[ 0 ] ) );
			convexCurve1234.AddMultiPoint ( curve1.ToMultiPoint ( ) );


			Polygon curve2 = ( ( Polygon ) TransForm.Move ( _shablon, navPntPrj.Value, toleranceAreaPnts[ 1 ] ) );
			convexCurve1234.AddMultiPoint ( curve2.ToMultiPoint ( ) );

			Polygon curve3 = ( ( Polygon ) TransForm.Move ( _shablon, navPntPrj.Value, toleranceAreaPnts[ 2 ] ) );
			convexCurve1234.AddMultiPoint ( curve3.ToMultiPoint ( ) );

			Polygon curve4 = ( ( Polygon ) TransForm.Move ( _shablon, navPntPrj.Value, toleranceAreaPnts[ 3 ] ) );
			convexCurve1234.AddMultiPoint ( curve4.ToMultiPoint ( ) );

			var result = ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( convexCurve1234 ) )[ 0 ];


			return result;
		}

		internal Polygon ConstructProtectionOmniDirEntry ( )
		{
			#region 3.3.3.2.1.2 Locate point E on circle
			
			MultiPoint result = new MultiPoint ( );
			result.AddMultiPoint ( ( ( Polygon ) TransForm.Move ( _shablon, _pntE, ARANFunctions.LocalToPrj ( _navPntPrj.Value, ARANMath.Modulus ( _direction - ARANMath.C_PI, ARANMath.C_2xPI ), _radius, 0 ) ) ).ToMultiPoint ( ) );
			result.AddMultiPoint ( ( ( Polygon ) TransForm.Move ( _shablon, _pntE, ARANFunctions.LocalToPrj ( _navPntPrj.Value, ARANMath.Modulus ( _direction + ARANMath.C_PI_2, ARANMath.C_2xPI ), _radius, 0 ) ) ).ToMultiPoint ( ) );
			
			#endregion

			#region 3.3.3.2.1.3

			double dirPlus70Deg = _direction - ( int ) _side * ARANMath.DegToRad ( 70 );
			Point [] ePoints = ConstructFixToleranceArea ( dirPlus70Deg, out _radius );
			
			#endregion

			#region Calculate Curve 6 - 7 { 3.3.3.2.1.4 }

			MultiPoint mltPntCurve67 = new MultiPoint ( );
			LineString turn180OnE1 = ( LineString ) TransForm.RotateGeometry ( _navPntPrj.Value, _turn180, -( int ) _side * ARANMath.DegToRad ( 70 ) );
			turn180OnE1 = ( LineString ) TransForm.Move ( turn180OnE1, _navPntPrj.Value, ePoints [ 0 ] );
			mltPntCurve67.AddMultiPoint ( turn180OnE1 );

			LineString turn180OnE3 = ( LineString ) TransForm.Move ( turn180OnE1, ePoints [ 0 ], ePoints [ 2 ] );
			mltPntCurve67.AddMultiPoint ( turn180OnE3 );

			Polygon convexCurve67 = ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( mltPntCurve67 ) ) [ 0 ];
			#endregion

			#region  3.3.3.2.1.5

			Point pntFarthestPnt;
			double radiusArc = GeomFunctions.MaxDistFromPointToGeometry ( _navPntPrj.Value, turn180OnE1, out pntFarthestPnt );
			//GlobalParams.DrawPointWithText(pntFarthestPnt,1,"Max");
			double dir = ARANFunctions.ReturnAngleInRadians ( _navPntPrj.Value, pntFarthestPnt );
			LineString arc = ARANFunctions.CreateArcAsPartPrj ( _navPntPrj.Value, pntFarthestPnt, ARANFunctions.LocalToPrj ( _navPntPrj.Value, dir - ARANMath.C_PI, radiusArc, 0 ), ( TurnDirection ) _side );
			result.AddMultiPoint ( arc );
	
			#endregion

			#region 3.3.3.2.1.6
			
			Polygon curve8 = ( Polygon ) TransForm.Flip ( convexCurve67, _navPntPrj.Value, dirPlus70Deg );
			result.AddMultiPoint ( curve8.ToMultiPoint ( ) );
			result.AddMultiPoint ( mltPntCurve67 );
			
			#endregion

			return ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( result ) ) [ 0 ];
		}

		public Polygon ConstructSector1NotPermitted ( )
		{
			//_debugHandles.ForEach ( item => _controller.SafeDeleteGraphic ( item ) );

			_line3.Remove ( _line3.Count - 1 );

			//_controller.SafeDeleteGraphic ( _line3Handle );
			//_line3Handle = _controller.DrawLineString ( _line3, 1, 1 );

			double trackingTolerance = GlobalParams.NavaidDatabase.Vor.TrackingTolerance;
			if ( _navPntPrj.Type == NavType.Ndb )
				trackingTolerance = GlobalParams.NavaidDatabase.Ndb.TrackingTolerance;

			LineString protectLineString = new LineString
			{
				_navPntPrj.Value,
				ARANFunctions.LocalToPrj(_navPntPrj.Value, _direction - ARANMath.C_PI + (int) (_side) * trackingTolerance,
					35000, 0)
			};

			//_debugHandles.Add(_controller.DrawLineString(protectLineString, 255 * 255, 1));

			Polygon result = new Polygon();
			if (!GeometryOperators.Instance.Crosses(_line3, protectLineString))
				return result;

			LineString tmpTurn180 = ( LineString ) TransForm.RotateGeometry (_navPntPrj.Value, _turn180, ARANMath.C_PI );
			var omniDirPolyline = new LineString();
			omniDirPolyline.AddMultiPoint(ConstructProtectionOmniDirEntry().ToMultiPoint());
			//_debugHandles.Add(_controller.DrawLineString(omniDirPolyline, 255, 1));
			var tmpGeom = (MultiPoint) GlobalParams.GeomOperators.Intersect(protectLineString, omniDirPolyline );
			var intersectPnt = tmpGeom[0];
			var dir = ARANFunctions.ReturnAngleInRadians((Point) intersectPnt, _navPntPrj.Value);
			
			double epislon = 10;
			double step = 1024;
			Point tstPnt = ARANFunctions.LocalToPrj ( intersectPnt, dir, step, 0 );
			bool found = false;
			MultiPoint mltPnt;
			//_debugHandles.Add ( _controller.DrawLineString ( ( LineString ) TransForm.Move ( tmpTurn180, _navPntPrj.Value, tstPnt ), 255 * 255, 1 ) );

			int i = 0;
			while ( true)
			{
				mltPnt =
					(MultiPoint)
					GeometryOperators.Instance.Intersect((LineString) TransForm.Move(tmpTurn180, _navPntPrj.Value, tstPnt),
						omniDirPolyline);
				if ( mltPnt.Count == 0 )
				{
					step = -0.5 * Math.Abs ( step );
					found = true;
				}
				else if ( mltPnt.Count == 1)
					break;
				else if (mltPnt.Count == 2 && found)
					step = 0.5 * Math.Abs(step);
				if (Math.Abs(step) < epislon && mltPnt.Count == 2)
					break;
				tstPnt = ARANFunctions.LocalToPrj(tstPnt, dir, step, 0);
				i++;
			}
			var restTurn180 = (LineString) TransForm.Move(tmpTurn180, _navPntPrj.Value, tstPnt);
			//_debugHandles.Add ( _controller.DrawLineString ( restTurn180, 255 * 255, 1 ) );

			Point midPoint = null;
			if (mltPnt.Count == 2)
				midPoint = new Point(0.5*(mltPnt[0].X + mltPnt[1].X), 0.5*(mltPnt[0].Y + mltPnt[1].Y));
			else if (mltPnt.Count == 1)
				midPoint = mltPnt[0];
			found = false;
			LineString resLineString = new LineString();
			for (i = 0; i < restTurn180.Count; i++)
			{
				if (!found && ARANFunctions.ReturnDistanceInMeters(restTurn180[i], midPoint) < 10*epislon)
					found = true;
				if (found && i > 0)
				{
					var tmpDir = ARANMath.Modulus(ARANFunctions.ReturnAngleInRadians(restTurn180[i-1], restTurn180[i]),
						ARANMath.C_2xPI);
					if (Math.Abs(tmpDir - (_direction + (int) (_side) * trackingTolerance)) < ARANMath.DegToRad(1))
					{
						resLineString.Add(restTurn180[i]);
						break;
					}					
				}
				if (found)
					resLineString.Add(restTurn180[i]);
			}

			resLineString.Add(ARANFunctions.LocalToPrj(resLineString[resLineString.Count - 1],
				_direction + (int) _side * trackingTolerance, 50000, 0));
			resLineString.Add( ARANFunctions.LocalToPrj ( resLineString[ resLineString.Count - 1 ],
				_direction - ( int ) _side * ARANMath.C_PI_2, 15000, 0 ) );
			resLineString.Add ( ARANFunctions.LocalToPrj ( resLineString[ resLineString.Count - 1 ],
				_direction - ARANMath.C_PI, 70000, 0 ) );
			resLineString.Insert(0, ARANFunctions.LocalToPrj(resLineString[0], _direction - ARANMath.C_PI, 10000, 0));

			//_debugHandles.Add(_controller.DrawLineString(resLineString, 255 * 255 * 255, 3));
			result.ExteriorRing.AddMultiPoint(resLineString.ToMultiPoint());			
			return result;
		}

		internal void SetTemplateVisibility ( bool visible )
		{
			GlobalParams.AranEnvironment.Graphics.SetVisible ( _templateHandle, visible );
		}

		internal void SetToleranceAreaVisibility ( bool visible )
		{
			GlobalParams.AranEnvironment.Graphics.SetVisible ( _toleranceAreaHandle, visible );
		}

		internal void SetNominalTrackVisibility ( bool visible )
		{
			GlobalParams.AranEnvironment.Graphics.SetVisible ( _nominalTrackHandle, visible );
		}

		private Point [] ConstructFixToleranceArea ( double direction, out double radius )
		{
			Point [] result = new Point [ 4 ];
			if ( _navPntPrj.Type == NavType.Vor )
				radius = _altitude * Math.Tan ( 50 * ARANMath.C_PI / 180 );
			else
				radius = _altitude * Math.Tan ( 40 * ARANMath.C_PI / 180 );
			LineString circleZv = ARANFunctions.CreateCircleAsPartPrj ( _navPntPrj.Value, radius );
			double tmpOutDouble = 100000000;
			//double qV;
			if ( _navPntPrj.Type == NavType.Vor )
			{
				//qV = radius * Math.Sin ( _vorTolerance * ARANMath.C_PI / 180 );
				result [ 1 ] = ARANFunctions.LocalToPrj ( _navPntPrj.Value, direction - ARANMath.C_PI + ARANMath.DegToRad ( VorTolerance ), radius, 0 );
				result [ 3 ] = ARANFunctions.LocalToPrj ( _navPntPrj.Value, direction - ARANMath.C_PI - ARANMath.DegToRad ( VorTolerance ), radius, 0 );
				result [ 0 ] = ARANFunctions.CircleVectorIntersect ( _navPntPrj.Value, radius, result [ 1 ], direction - ARANMath.DegToRad ( VorTolerance ), out tmpOutDouble );
				result [ 2 ] = ARANFunctions.CircleVectorIntersect ( _navPntPrj.Value, radius, result [ 3 ], direction + ARANMath.DegToRad ( VorTolerance ), out tmpOutDouble );
			}
			else
			{
				//qV = radius * Math.Sin ( _ndbTolerance * ARANMath.C_PI / 180 );
				result [ 1 ] = ARANFunctions.LocalToPrj ( _navPntPrj.Value, direction - ARANMath.C_PI + ARANMath.DegToRad ( NdbTolerance ), radius, 0 );
				result [ 3 ] = ARANFunctions.LocalToPrj ( _navPntPrj.Value, direction - ARANMath.C_PI - ARANMath.DegToRad ( NdbTolerance ), radius, 0 );
				result [ 0 ] = ARANFunctions.CircleVectorIntersect ( _navPntPrj.Value, radius, result [ 1 ], direction - ARANMath.DegToRad ( VorTolerance ), out tmpOutDouble );
				result [ 2 ] = ARANFunctions.CircleVectorIntersect ( _navPntPrj.Value, radius, result [ 3 ], direction + ARANMath.DegToRad ( VorTolerance ), out tmpOutDouble );
			}
			if ( GlobalParams.IsTestVersion )
			{
				LineString circle = ARANFunctions.CreateCircleAsPartPrj ( _navPntPrj.Value, radius );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( circle, 1, 1 ) );
			}
			return result;
		}
		
		public Ring ToleranceArea => _toleranceArea;

		public double PostFixTolerance => _postFixTolerance;

		public double PriorFixTolerance => _priorFixTolerance;

		internal LineString NominalTrack => _nominalTrack;

		private const double VorTolerance = 5;
		private const double NdbTolerance = 15;

		private readonly Ring _toleranceArea;
		private double _priorFixTolerance, _postFixTolerance;
		private LineString _nominalTrack;
		private readonly MainController _controller;
		private int _templateHandle, _nominalTrackHandle;
		private int _toleranceAreaHandle;
		private Polygon _shablon;
		private Point _pntE;
		private NavaidPntPrj _navPntPrj;
		private double _direction;
		private LineString _line3, _turn180;
		private int _line3Handle;
		private List<int> _debugHandles = new List<int>();
		private SideDirection _side;
		private double _radius;
		private double _altitude;
	}
}