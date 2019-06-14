using Aran.Geometries;
using Aran.PANDA.Common;
using Aran.AranEnvironment.Symbols;

namespace Aran.PANDA.Conventional.Racetrack.Procedures
{
	public class VorVor
	{
		public VorVor ( MainController controller )
		{
			_controller = controller;
			_toleranceArea = new Ring ( );
		}

		internal Polygon ConstructBasicArea ( Point homingVorPntPrj, Point intersectingVorPntPrj, double altitude, double homingDirection, double intersectDirection, double tas, SideDirection side, double time, Point dsgPntPrj, EntryDirection entryDirection, bool drawShablon, bool drawNominalTraject, bool drawToleranceArea )
		{
			Point[] tolerancePnts = ConstructFixToleranceArea ( homingVorPntPrj, intersectingVorPntPrj, altitude, homingDirection, intersectDirection, dsgPntPrj );

			_toleranceArea.Clear ( );
			_toleranceArea.Add ( tolerancePnts[ 0 ] );
			_toleranceArea.Add ( tolerancePnts[ 1 ] );
			_toleranceArea.Add ( tolerancePnts[ 3 ] );
			_toleranceArea.Add ( tolerancePnts[ 2 ] );
			_toleranceArea.Add ( tolerancePnts[ 0 ] );

			LineString line3;
			Point pntR;

			if (entryDirection == EntryDirection.Away)
				Shablons.HoldingShablon ( _controller, dsgPntPrj, tas, altitude, time, homingDirection, side, 0, out _nominalTrack, out _shablon, out line3, out _turn180, out _pntE, out pntR );
			else
				Shablons.HoldingShablon ( _controller, dsgPntPrj, tas, altitude, time, homingDirection + ARANMath.C_PI, side, 0, out _nominalTrack, out _shablon, out line3, out _turn180, out _pntE, out pntR );

			LineString lnStringTemplate = new LineString ( );
			if ( GlobalParams.IsTestVersion )
				lnStringTemplate.Add ( _pntE );
			lnStringTemplate.Add ( dsgPntPrj );
			lnStringTemplate.AddMultiPoint ( _shablon.ToMultiPoint ( ) );

			_controller.SafeDeleteGraphic ( _templateHandle );
			if ( drawShablon )
				_templateHandle = _controller.DrawLineString ( lnStringTemplate, 255, 1 );

			_controller.SafeDeleteGraphic ( _nominalTrackHandle );
			if ( drawNominalTraject )
				_nominalTrackHandle = _controller.DrawLineString ( _nominalTrack, 1, 1 );


			_controller.SafeDeleteGraphic ( _toleranceAreaHandle );
			if ( drawToleranceArea )
				_toleranceAreaHandle = _controller.DrawRing ( _toleranceArea, 1, eFillStyle.sfsCross );

			MultiPoint result = new MultiPoint ( );

			Polygon shablonOnA1 = ( ( Polygon ) TransForm.Move ( _shablon, dsgPntPrj, _toleranceArea [ 0 ] ) );
			//GlobalParams.DrawPolygon ( shablonOnA1, 1, AranEnvironment.Symbols.eFillStyle.sfsHollow );
			result.AddMultiPoint ( shablonOnA1.ToMultiPoint ( ) );


			Polygon shablonOnA2 = ( ( Polygon ) TransForm.Move ( _shablon, dsgPntPrj, _toleranceArea [ 1 ] ) );
			//GlobalParams.DrawPolygon ( shablonOnA2, 1, AranEnvironment.Symbols.eFillStyle.sfsHollow );
			result.AddMultiPoint ( shablonOnA2.ToMultiPoint ( ) );

			Polygon shablonOnA3 = ( ( Polygon ) TransForm.Move ( _shablon, dsgPntPrj, _toleranceArea [ 2 ] ) );
			//GlobalParams.DrawPolygon ( shablonOnA3, 1, AranEnvironment.Symbols.eFillStyle.sfsHollow );
			result.AddMultiPoint ( shablonOnA3.ToMultiPoint ( ) );

			Polygon shablonOnA4 = ( ( Polygon ) TransForm.Move ( _shablon, dsgPntPrj, _toleranceArea [ 3 ] ) );
			//GlobalParams.DrawPolygon ( shablonOnA4, 1, AranEnvironment.Symbols.eFillStyle.sfsHollow );
			result.AddMultiPoint ( shablonOnA4.ToMultiPoint ( ) );

			return ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( result ) ) [ 0 ];
		}

		internal MultiPolygon ProtectIntersectingRadialEntry ( Point homingVorPntPrj, Point intersectVorPntPrj, double homingDirection, double intersectDirection, SideDirection side, Point dsgPntPrj, double altitude )
		{
			if ( ARANMath.SideDef ( homingVorPntPrj, homingDirection, intersectVorPntPrj ) != SideDirection.sideLeft )
				return null;

			MultiPoint result = new MultiPoint ( );

			Point[] ePoints = ConstructFixToleranceArea ( intersectVorPntPrj, homingVorPntPrj, altitude, intersectDirection, homingDirection, dsgPntPrj );
			//GlobalParams.DrawToleranceAreaPoints ( ePoints );
			
			double rotateDir = ARANMath.SubtractAngles ( intersectDirection + ARANMath.C_PI, homingDirection );

			LineString turn180OnE3 = (LineString) TransForm.RotateGeometry ( dsgPntPrj, _turn180, rotateDir );
			turn180OnE3 = ( LineString ) TransForm.Move ( turn180OnE3, dsgPntPrj, ePoints [ 0 ] );
			//GlobalParams.DrawLineString ( turn180OnE3, 1, 1 );
			result.AddMultiPoint ( turn180OnE3 );

			LineString turn180OnE4 = ( LineString ) TransForm.Move ( turn180OnE3, ePoints [ 0 ], ePoints [ 2 ] );
			//GlobalParams.DrawLineString ( turn180OnE4, 1, 1 );
			result.AddMultiPoint ( turn180OnE4 );

			Polygon tmp = ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( result ) ) [ 0 ];
			return ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( tmp, ReciprocalEntryArea ( ) );
		}

		private Point[] ConstructFixToleranceArea ( Point homingVorPntPrj, Point intersectingVorPntPrj, double altitude, double homingDirection, double intersectDirection, Point dsgPntPrj )
		{
			LineString homingVorRightLnString = new LineString ( );
			double dist = ARANFunctions.ReturnDistanceInMeters ( homingVorPntPrj, dsgPntPrj );
			homingVorRightLnString.Add ( homingVorPntPrj );
			homingVorRightLnString.Add ( ARANFunctions.LocalToPrj ( homingVorPntPrj, homingDirection - GlobalParams.NavaidDatabase.Vor.TrackingTolerance, 2 * dist, 0 ) );
			//_controller.DrawLineString ( homingVorRightLnString, 1, 1 );

			LineString homingVorLeftLnString = new LineString
			{
				homingVorPntPrj,
				ARANFunctions.LocalToPrj(homingVorPntPrj, homingDirection + GlobalParams.NavaidDatabase.Vor.TrackingTolerance,
					2 * dist, 0)
			};
			//_controller.DrawLineString ( homingVorLeftLnString, 1, 1 );

			LineString intersectingVorRightLnString = new LineString ( );
			dist = ARANFunctions.ReturnDistanceInMeters ( intersectingVorPntPrj, dsgPntPrj );
			intersectingVorRightLnString.Add ( intersectingVorPntPrj );
			intersectingVorRightLnString.Add ( ARANFunctions.LocalToPrj ( intersectingVorPntPrj, intersectDirection - GlobalParams.NavaidDatabase.Vor.IntersectingTolerance, 2 * dist, 0 ) );
			//_controller.DrawLineString ( intersectingVorRightLnString, 1, 1 );

			LineString intersectingVorLeftLnString = new LineString
			{
				intersectingVorPntPrj,
				ARANFunctions.LocalToPrj(intersectingVorPntPrj,
					intersectDirection + GlobalParams.NavaidDatabase.Vor.IntersectingTolerance, 2 * dist, 0)
			};
			//_controller.DrawLineString ( intersectingVorLeftLnString, 1, 1 );

			Point [] result = new Point [ 4 ];
			result [ 0 ] = ( ( MultiPoint ) GlobalParams.GeomOperators.Intersect ( homingVorRightLnString, intersectingVorLeftLnString ) ) [ 0 ];
			result [ 1 ] = ( ( MultiPoint ) GlobalParams.GeomOperators.Intersect ( homingVorRightLnString, intersectingVorRightLnString ) ) [ 0 ];
			result [ 2 ] = ( ( MultiPoint ) GlobalParams.GeomOperators.Intersect ( homingVorLeftLnString, intersectingVorLeftLnString ) ) [ 0 ];
			result [ 3 ] = ( ( MultiPoint ) GlobalParams.GeomOperators.Intersect ( homingVorLeftLnString, intersectingVorRightLnString ) ) [ 0 ];
			return result;
		}

		internal Polygon ReciprocalEntryArea ( )
		{
			MultiPoint result = new MultiPoint ( );
			Polygon curve5 = ( Polygon ) TransForm.Move ( _shablon, _pntE, _toleranceArea [ 1 ] );
			result.AddMultiPoint ( curve5.ToMultiPoint ( ) );
			//GlobalParams.DrawPolygon ( curve5, 1, AranEnvironment.Symbols.eFillStyle.sfsHollow );

			Polygon curve6 = ( Polygon ) TransForm.Move ( _shablon, _pntE, _toleranceArea [ 3 ] );
			result.AddMultiPoint ( curve6.ToMultiPoint ( ) );
			//GlobalParams.DrawPolygon ( curve6, 1, AranEnvironment.Symbols.eFillStyle.sfsHollow );

			return ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( result ) ) [ 0 ];
		}
		
		public Ring ToleranceArea => _toleranceArea;

		internal LineString NominalTrack => _nominalTrack;

		internal void SetTemplateVisiblity ( bool visible )
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

		private readonly Ring _toleranceArea;
		private LineString _turn180;
		private Polygon _shablon;
		private Point _pntE;
		private LineString _nominalTrack;
		private readonly MainController _controller;
		private int _templateHandle;
		private int _nominalTrackHandle;
		private int _toleranceAreaHandle;
	}
}