using System;
using Aran.PANDA.Common;
using Aran.Geometries;
using System.Collections.Generic;

namespace Aran.PANDA.Conventional.Racetrack.Procedures
{
	public class Vordme
	{
		public Vordme ( MainController controller )
		{
			_controller = controller;
			_isWithLimitingRadial = false;
			_toleranceArea = new Ring ( );
			GlobalParams.TestVersionHandles = new List<int> ( );
		}

		#region Toward part

		public Polygon TowardConstructBasicArea (SideDirection side, Point dmePntPrj, Point vorPntPrj, Point designatedPointPrj, double direction,
													double nominalDistanceInPrj, double nominalDistance, double limitingDistanceInPrj, double limitingDistance,
													double time, double tas, double legLength, double altitude, double radius, bool drawShablon, bool		drawwNominalTraject, bool drawToleranceArea )
		{
			GlobalParams.ClearTestVersionElements ( );
			NavaidsDataBase navaidsDb = GlobalParams.NavaidDatabase;
			// Following part is implemented via directionMinusPi
			double directionMinusPi = direction - ARANMath.C_PI;

			double d1 = navaidsDb.Dme.MinimalError + navaidsDb.Dme.ErrorScalingUp * nominalDistance;
			double d2 = navaidsDb.Dme.MinimalError + navaidsDb.Dme.ErrorScalingUp * limitingDistance;			
			_dl2 = limitingDistanceInPrj + d2;

			TowardConstructFixToleranceArea ( dmePntPrj, vorPntPrj, nominalDistanceInPrj, directionMinusPi, side, navaidsDb.Vor.TrackingTolerance, d1, designatedPointPrj, false );

			Point pntE;
			Point pntR;

			Shablons.HoldingShablon ( _controller, designatedPointPrj, tas, altitude, time, direction, side, 0, out _nominalTrack, out _shablon, out _line3, out _turn180, out pntE, out pntR );
			LineString lnStringTemplate = new LineString {designatedPointPrj};
			lnStringTemplate.AddMultiPoint ( _shablon.ToMultiPoint ( ) );
			
			_controller.SafeDeleteGraphic ( _templateHandle );
			if ( drawShablon )
				_templateHandle = _controller.DrawLineString ( lnStringTemplate, 255, 1 );

			_controller.SafeDeleteGraphic ( _nominalTrackHandle );
			if ( drawwNominalTraject )
				_nominalTrackHandle = _controller.DrawLineString ( _nominalTrack, 1, 1 );

			//_controller.SafeDeleteGraphic ( _turn180Handle );
			//_turn180Handle = _controller.DrawLineString ( _turn180, 255 * 255 * 255, 1 );
			
			//if ( drawToleranceArea )
			//    _controller.DrawToleranceAreaPoints ( ToleranceArea );
			//_controller.DrawLineString ( _turn180, 255, 1 );

			//_controller.DrawLineString ( ( LineString ) TransForm.Move ( _turn180, designatedPointPrj, _toleranceArea[ 0 ] ), 1, 1 );
			#region Protection of the Outbound turn
			//directionMinusPi += ARANMath.C_PI;

			Line straightPart = GetStraightPartOfPolygon ( ARANMath.ChangeDirection ( side ), directionMinusPi, vorPntPrj, _shablon );
			straightPart.RefPoint = ( Point ) TransForm.Move ( straightPart.RefPoint, designatedPointPrj, _toleranceArea [ 0 ] );

			//LineString lnStringDebug = new LineString();
			//lnStringDebug.Add(straightPart.RefPoint);
			//lnStringDebug.Add(ARANFunctions.LocalToPrj(straightPart.RefPoint, straightPart.DirVector.Direction - (int)side * ARANMath.C_PI, 10000, 0));
			//_controller.DrawLineString ( lnStringDebug, 1, 1 );

			double tmpOutDouble;
			double dl1 = limitingDistanceInPrj - d2;

            LineString circleDl1 = ARANFunctions.CreateCircleAsPartPrj(dmePntPrj, dl1);

			Point pntC1 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, dl1, straightPart.RefPoint, straightPart.DirVector.Direction - ( int ) side * ARANMath.C_PI, out tmpOutDouble );

			LineString line3AsLineString = ( ( MultiLineString ) ARANFunctions.CreatePolyLineFromParts ( _line3 ) )[ 0 ];
			line3AsLineString = ( LineString ) TransForm.Move ( line3AsLineString, designatedPointPrj, _toleranceArea [ 3 ] );

			#region
			if ( GlobalParams.IsTestVersion )
			{
				//_controller.DrawPointWithText ( pntC1, 1, "c1" );
				//_controller.DrawPointWithText ( pntC2, 1, "c2" );
				//_controller.DrawPointWithText ( _pntC3, 1, "c3" );
				//_controller.DrawPointWithText ( pntC3OnShablon, 1, "c'3" );

				//_controller.DrawPointWithText ( pntC4, 1, "c4" );
				//if ( pntC5 != null )
				//    _controller.DrawPointWithText ( pntC5, 1, "c5" );
				//_controller.DrawPointWithText ( _pntC6, 1, "c6" );
				//_controller.DrawPointWithText ( pntC6OnLine3, 1, "c'6" );

				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( circleDl1, 1, 1 ) );
				//_controller.DrawLineString ( circleDL2, 1, 1 );
				//_controller.DrawLineString ( tmpTurn180, 1, 1 ) ;
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( line3AsLineString, 1, 1 ) );
			}
			#endregion

			Geometry geom = GlobalParams.GeomOperators.Intersect(line3AsLineString, ARANFunctions.CreatePolyLineFromParts(circleDl1));
            //_controller.DrawLineString(line3AsLineString, 1, 1);
            //_controller.DrawMultiLineString(ARANFunctions.CreatePolyLineFromParts(circleDL1), 1, 1);
			Point pntC4 = ( ( MultiPoint ) geom ) [ 0 ];

			LineString circleDl2 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, _dl2 );
			Point pntC3OnShablon = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, straightPart.RefPoint, straightPart.DirVector.Direction - ( int ) side * ARANMath.C_PI, out tmpOutDouble );

			geom = GlobalParams.GeomOperators.Intersect ( line3AsLineString, ARANFunctions.CreatePolyLineFromParts ( circleDl2 ) );
			Point pntC6OnLine3 = ( ( MultiPoint ) geom ) [ 0 ];

			Line tmpline = new Line ( pntC3OnShablon, pntC1 );
			Point pntC2 = ARANFunctions.LocalToPrj ( pntC3OnShablon, tmpline.DirVector.Direction, Math.Abs ( d2 + d1 - 1800 ), 0 );

			tmpline = new Line ( pntC2, directionMinusPi );
			_pntC3 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, tmpline, out tmpOutDouble );

			tmpline = new Line ( pntC6OnLine3, pntC4 );
			Point pntC5 = ARANFunctions.PointAlongPlane ( pntC6OnLine3, tmpline.DirVector.Direction, Math.Abs ( d2 + d1 - 1800 ) );
			tmpline = new Line ( pntC5, directionMinusPi );
			_pntC6 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, tmpline, out tmpOutDouble );

			LineString rp12 = new LineString
			{
				ARANFunctions.PointAlongPlane(vorPntPrj, directionMinusPi + (int) side * navaidsDb.Vor.TrackingTolerance, 500000),
				vorPntPrj,
				ARANFunctions.PointAlongPlane(vorPntPrj, directionMinusPi - (int) side * navaidsDb.Vor.TrackingTolerance, 500000)
			};

			if ( ARANMath.SideDef ( vorPntPrj, directionMinusPi + ( int ) side * navaidsDb.Vor.TrackingTolerance, pntC4 ) == side )
			{
				pntC4 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, dl1, vorPntPrj, directionMinusPi + ( int ) side * navaidsDb.Vor.TrackingTolerance, out tmpOutDouble );
				_pntC6 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, directionMinusPi + ( int ) side * navaidsDb.Vor.TrackingTolerance, out tmpOutDouble );
				pntC5 = null;
			}
			else if ( ARANMath.SideDef ( vorPntPrj, directionMinusPi + ( int ) side * navaidsDb.Vor.TrackingTolerance, pntC5 ) == side )
			{
				pntC5 = ( ( MultiPoint ) GlobalParams.GeomOperators.Intersect ( rp12, line3AsLineString ) ) [ 0 ];
				_pntC6 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, directionMinusPi + ( int ) side * navaidsDb.Vor.TrackingTolerance, out tmpOutDouble );
			}


            //GlobalParams.DrawPointWithText(_pntC3, 1, "c3");
            //GlobalParams.DrawPointWithText(pntC3OnShablon, 1, "c3'");

            //if (pntC5 != null)
            //    GlobalParams.DrawPointWithText(pntC5, 1, "c5");
            //GlobalParams.DrawPointWithText(_pntC6, 1, "c6");
            //GlobalParams.DrawPointWithText(pntC6OnLine3, 1, "c6'");

            //GlobalParams.DrawLineString(circleDL1, 1, 1);
            //GlobalParams.DrawLineString(circleDL2, 1, 1);
            //GlobalParams.DrawMultiLineString(line3AsMultiLineString, 1, 1);
            //GlobalParams.DrawLineString(Rp12, 1, 1);            

			#endregion

			#region Protection of the inbound turn

			double ang1 = ARANFunctions.ReturnAngleInRadians ( dmePntPrj, _pntC3 );
			double ang2 = ARANFunctions.ReturnAngleInRadians ( dmePntPrj, _pntC6 );

			double littleAng = ARANMath.SubtractAngles ( ang1, ang2 );

			int n = ( int ) Math.Floor ( ARANMath.RadToDeg ( littleAng ) );

			MultiLineString prtctInboundTurnPolyLine = new MultiLineString ( );
			LineString tmpTurn180 = ( LineString ) TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI );
			tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, designatedPointPrj, pntC2 );
			//GlobalParams.DrawLineString ( tmpTurn180, 1, 1 );
			prtctInboundTurnPolyLine.Add ( tmpTurn180 );

			tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, pntC2, _pntC3 );
			//GlobalParams.DrawLineString ( tmpTurn180, 1, 1 );
			prtctInboundTurnPolyLine.Add ( ( LineString ) tmpTurn180.Clone ( ) );

			//return null;
			Point pntTmp = new Point ( _pntC3 );
			for ( int i = 0; i <= n; i++ )
			{
				var tmpPntOnCircleDl2 = ARANFunctions.LocalToPrj ( dmePntPrj, ang1 + ( int ) side * ARANMath.DegToRad ( i ), _dl2, 0 );
				tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, pntTmp, tmpPntOnCircleDl2 );
				pntTmp.Assign ( tmpPntOnCircleDl2 );
				prtctInboundTurnPolyLine.Add ( ( LineString ) tmpTurn180.Clone ( ) );
			}

			_basicArea = ChainHullAlgorithm.ConvexHull ( prtctInboundTurnPolyLine );
			Geometry leftPolygon , rightPolygon ;
			MultiLineString mltLineString = new MultiLineString ( );
			double dist = ARANFunctions.ReturnDistanceInMeters ( pntC2, dmePntPrj );
			LineString circleDls = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, dist );
			mltLineString.Add ( circleDls );
			GlobalParams.GeomOperators.Cut(_basicArea, mltLineString, out leftPolygon, out rightPolygon);
			//GlobalParams.DrawPolygon ( (leftPolygon as MultiPolygon)[0], 0, eFillStyle.sfsHorizontal );
			//GlobalParams.DrawPolygon ( ( rightPolygon as MultiPolygon ) [ 0 ], 0, eFillStyle.sfsHorizontal );
			prtctInboundTurnPolyLine.Clear ( );
			prtctInboundTurnPolyLine.Add ( ( LineString ) ARANFunctions.PolygonToPolyLine ( ( leftPolygon as MultiPolygon ) [ 0 ] ) [ 0 ].Clone ( ) );

			tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, pntTmp, _pntC6 );
			prtctInboundTurnPolyLine.Add ( ( LineString ) tmpTurn180.Clone ( ) );

			tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, _pntC6, pntC4 );
			prtctInboundTurnPolyLine.Add ( ( LineString ) tmpTurn180.Clone ( ) );

			if ( pntC5 != null )
			{
				tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, pntC4, pntC5 );
				prtctInboundTurnPolyLine.Add ( ( LineString ) tmpTurn180.Clone ( ) );
			}

            //foreach (LineString lnString in prtctInboundTurnPolyLine)
            //{
                //GlobalParams.DrawLineString(lnString, 1, 1);
            //}

			#endregion

			prtctInboundTurnPolyLine.Add ( ( LineString ) TransForm.Move ( _turn180, designatedPointPrj, _toleranceArea [ 0 ] ) );
			prtctInboundTurnPolyLine [ 0 ].Add ( pntC2 );
			prtctInboundTurnPolyLine.Add ( ( LineString ) TransForm.Move ( _turn180, designatedPointPrj, _toleranceArea [ 3 ] ) );

			_basicArea = ChainHullAlgorithm.ConvexHull ( prtctInboundTurnPolyLine );
			return _basicArea;
		}

		public Polygon TowardConstructProtectSector1 (VorData vordata, double ias, double tas, double direction, double altitude, Point vorPntPrj, Point dmePntPrj, Point designatedPointPrj, SideDirection side )
		{
			double rad = ARANFunctions.ReturnAngleInRadians ( _toleranceArea [ 0 ], _toleranceArea [ 3 ] );
			rad = ARANMath.SubtractAngles ( rad, direction );

			double r = 943.27 * 0.277777777777778 / tas;
			if ( ( r > 3.0 ) )
				r = 3.0;

			double rv = 3600 * tas / ( 62.83 * r );
			double h = altitude / 1000.0;
			double w = 12.0 * h + 87.0;
			double w_ = 0.277777777777778 * w;
			double e = w_ / r;
			e = ARANMath.RadToDeg ( e );
			double ac = 11.0 * tas;

			Point ptCntC = ARANFunctions.LocalToPrj ( designatedPointPrj, direction, -ac, -rv * ( int ) side );
			ptCntC = ( Point ) TransForm.Flip ( ptCntC, designatedPointPrj, direction );
			ptCntC = ( Point ) TransForm.RotateGeometry ( designatedPointPrj, ptCntC, -( int ) side * rad );
			ptCntC = ( Point ) TransForm.Move ( ptCntC, designatedPointPrj, _toleranceArea [ 3 ] );
			//_controller.DrawPointWithText ( PtCntC, 0, "Cnt" ) );

			var alfa0 = ARANMath.Modulus ( direction - ( int ) side * ( rad + ARANMath.C_PI_2 ), ARANMath.C_2xPI );
			var newSide = ARANMath.ChangeDirection ( side );
			var r0 = rv + 11 * w_;
			var nominalDirection = alfa0 + ( int ) newSide * ARANMath.C_PI_2;
			var touchDirection = _line3 [ 0 ].M;

			Point strtPnt = ARANFunctions.LocalToPrj ( ptCntC, alfa0, r0, 0 );
			//_controller.DrawPointWithText ( StrtPnt, 0, "Str" );

			double touchAngle = ARANFunctions.SpiralTouchAngle ( r0, e, nominalDirection, touchDirection, newSide );
			double dir = alfa0 + touchAngle * ( int ) newSide;
			double s = e * touchAngle + r0;
			Point touchPoint = ARANFunctions.LocalToPrj ( ptCntC, dir, s, 0 );
			//_controller.DrawPointWithText ( TouchPoint, 0, "Tch" );

			MultiLineString multiLnStringProtectionArea = new MultiLineString ( );
			LineString lineString = new LineString ( );
			multiLnStringProtectionArea.Add ( lineString );
			lineString.Add ( _toleranceArea [ 3 ] );
			Shablons.CreateSpiralBy2Radial ( ptCntC, rv + 11 * w_, alfa0, alfa0 - ( int ) side * ARANMath.C_PI_2, e, ( int ) newSide, lineString );

			//// For Test
			//LineString circleDL2 = ARANFunctions.RingToPart ( ARANFunctions.CreateCirclePrj ( dmePntPrj, _dl2 ) );
			//_controller.DrawLineString ( circleDL2, 0, 1 );
			//// end Test
			double tmpOutDouble;
			Point pntC10 = ( Point ) ( ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, touchPoint, _line3 [ 3 ].M, out tmpOutDouble ) );
			lineString.Add ( pntC10 );
			//GlobalParams.DrawPointWithText ( pntC10, 0, "C10" );
			//GlobalParams.DrawLineString ( lineString, 255, 1 );

			var tmpTurn180 = ( LineString ) TransForm.Flip ( _turn180, designatedPointPrj, direction );
			tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, designatedPointPrj, pntC10 );
			tmpTurn180 = ( LineString ) TransForm.RotateGeometry ( pntC10, tmpTurn180, ARANMath.C_PI );

			double angC10 = ARANFunctions.ReturnAngleInRadians ( dmePntPrj, pntC10 );
			Point pntEnd = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, direction - ( int ) side * vordata.TrackingTolerance, out tmpOutDouble );
			//_controller.DrawPointWithText ( pntEnd, 0, "End" );

			double angEnd = ARANFunctions.ReturnAngleInRadians ( dmePntPrj, pntEnd );

			int countDeg = ( int ) Math.Floor ( ARANMath.RadToDeg ( ARANMath.SubtractAngles ( angC10, angEnd ) ) );
			Point tmpA;
			var tmp = ( Point ) pntC10;

			for ( int i = 0; i <= countDeg; i++ )
			{
				tmpA = ARANFunctions.LocalToPrj ( dmePntPrj, angC10 - ( int ) side * ARANMath.DegToRad ( i ), _dl2, 0 );
				tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, tmp, tmpA );
				multiLnStringProtectionArea.Add ( ( LineString ) tmpTurn180 );
				//_controller.DrawLineString ( tmpTurn180, 0, 1 );
				tmp.Assign ( tmpA );
			}

			tmpA = ARANFunctions.LocalToPrj ( dmePntPrj, angEnd, _dl2, 0 );
			tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, tmp, tmpA );
			multiLnStringProtectionArea.Add ( ( LineString ) tmpTurn180 );
			//_controller.DrawLineString ( tmpTurn180, 0, 1 );

			Polygon result = ChainHullAlgorithm.ConvexHull ( multiLnStringProtectionArea.ToMultiPoint ( ) );

			//GlobalParams.DrawPolygon ( result, 255, eFillStyle.sfsHorizontal );
			//GlobalParams.DrawPolygon ( _basicArea, 255 * 255, eFillStyle.sfsVertical );

			//MultiPolygon multiPolygon = ( ( MultiPolygon ) GlobalParams.GeomOperators.Difference ( result, _basicArea ) );
			//Polygon DifBaseAndProtectSect1 = null;
			//if ( multiPolygon.Count > 0 )
			//{
			//    DifBaseAndProtectSect1 = multiPolygon [ 0 ];
			//}
			//result = DifBaseAndProtectSect1;
			return result;
		}

		public Polygon TowardConstructProtectSector2 ( double tas, double limitingDistanceInPrj, double direction, SideDirection side, Point dmePntPrj, Point designatedPointPrj, double time )
		{
			Polygon result = new Polygon();
			// 1.5 = 90 san
			if ( time > 1.5 )
				return result;

			MultiLineString multiLnStringPrtctAreaSec2 = new MultiLineString ( );
			//_controller.DrawLineString ( ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, _dl2 ), 0, 1 );

			double tmpOutDouble;
			LineString tmpLnString = new LineString
			{
				_toleranceArea[0],
				ARANFunctions.LocalToPrj(_toleranceArea[0], direction - (int) side * ARANMath.DegToRad(30 + 5), 1000000, 0)
			};
			//_controller.DrawLineString ( tmpLnString, 1, 1 );

			Point pntC7 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, _toleranceArea[ 0 ], direction - ( int ) side * ARANMath.DegToRad ( 30 + 5 ), out tmpOutDouble );
			//_controller.DrawPointWithText ( pntC7, 0, "C7" );

			LineString tmpLnString2 = new LineString
			{
				_toleranceArea[2],
				ARANFunctions.LocalToPrj(_toleranceArea[2], direction - (int) side * ARANMath.DegToRad(30 - 5), 1000000, 0)
			};
			//_controller.DrawLineString ( tmpLnString2, 1, 1 );

			Point pntC8 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, _toleranceArea [ 2 ], direction - ( int ) side * ARANMath.DegToRad ( 30 - 5 ), out tmpOutDouble );
			//_controller.DrawPointWithText ( pntC8, 0, "C8" );

			LineString tmpTurn180 = ( LineString ) TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI - ( int ) side * ARANMath.DegToRad ( 30 ) );
			tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, designatedPointPrj, pntC7 );
			//GlobalParams.DrawedItemIndexList.Add ( GlobalParams.DrawLineString ( tmpTurn180, 0, 1 ) );

			double angleC7 = ARANFunctions.ReturnAngleInRadians ( dmePntPrj, pntC7 );
			double angle = ARANMath.SubtractAngles ( angleC7, ARANFunctions.ReturnAngleInRadians ( dmePntPrj, pntC8 ) );
			int count = ( int ) Math.Floor ( ARANMath.RadToDeg ( angle ) );
			int sideInInt = ( int ) side;
			Point tmp = new Point ( );
			
			for ( int i = 0; i <= count; i++ )
			{
				tmp.Assign ( ARANFunctions.LocalToPrj ( dmePntPrj, angleC7 + sideInInt * ARANMath.DegToRad ( i ), _dl2, 0 ) );
				
				multiLnStringPrtctAreaSec2.Add ( ( LineString ) TransForm.Move ( tmpTurn180, pntC7, tmp ) );
			}
			multiLnStringPrtctAreaSec2.Add ( ( LineString ) TransForm.Move ( tmpTurn180, pntC7, pntC8 ) );			

			MultiPoint multiPoint = multiLnStringPrtctAreaSec2.ToMultiPoint ( );
			result = ChainHullAlgorithm.ConvexHull ( multiPoint, _basicArea );
			//GlobalParams.DrawedItemIndexList.Add ( GlobalParams.DrawPolygon ( result, 1, eFillStyle.sfsCross ) );
			//result = ( ( MultiPolygon ) GlobalParams.GeomOperators.Difference ( result, _basicArea ) ) [ 0 ];
			return result;
		}

		public Polygon TowardConstructRecipDirectEntry2SecondaryPnt ( VorData vordata, double legLength, double direction, Point designatedPointPrj, Point vorPntPrj,
																	  Point dmePntPrj, SideDirection side, double radius, EntryDirection entryDirection, out int resultAngle )
		{
			int sideInInt = ( int ) side;
			Point secPnt = ARANFunctions.LocalToPrj ( designatedPointPrj, direction, legLength, -( int ) side * 2 * radius );
			//_controller.DrawPointWithText ( SecPnt, 0, "Sec Pnt" );
			double angleRe = ARANFunctions.ReturnAngleInRadians ( vorPntPrj, secPnt );

			LineString prt = new LineString
			{
				ARANFunctions.LocalToPrj(vorPntPrj, angleRe - (int) side * GlobalParams.NavaidDatabase.Vor.TrackingTolerance,
					100000, 0),
				vorPntPrj,
				ARANFunctions.LocalToPrj(vorPntPrj, angleRe + (int) side * GlobalParams.NavaidDatabase.Vor.TrackingTolerance,
					100000, 0)
			};
			//_controller.DrawLineString ( prt, 0, 1 );


			double angleReRp = ARANMath.SubtractAngles ( direction, angleRe );
			resultAngle = ( int ) ARANMath.RadToDeg ( angleReRp );

			double tmpOutDouble;
			Point pntI2 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, angleRe - ( int ) side * vordata.TrackingTolerance, out tmpOutDouble );
			//_controller.DrawPointWithText ( pntI2, 0, "I2" );
			Point pntI4 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, angleRe + ( int ) side * vordata.TrackingTolerance, out tmpOutDouble );
			//_controller.DrawPointWithText ( pntI4, 0, "I4" );

			LineString circleDl2 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj,_dl2 );
			//_controller.DrawLineString ( circleDL2, 0, 1 );

			double angI2 = ARANFunctions.ReturnAngleInRadians ( vorPntPrj, pntI2 );
			double angI4 = ARANFunctions.ReturnAngleInRadians ( vorPntPrj, pntI4 );
			int n = ( int ) Math.Floor ( ARANMath.RadToDeg ( ARANMath.SubtractAngles ( angI2, angI4 ) ) );

			Point tmpPnt = new Point ( );
			MultiLineString prtctSecPnt = new MultiLineString ( );
			LineString tmpPart = ( LineString ) TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI - sideInInt * angleReRp );
			tmpPart = ( LineString ) TransForm.Move ( tmpPart, designatedPointPrj, pntI2 );
			for ( int i = 0; i <= n; i++ )
			{
				tmpPnt.Assign ( ARANFunctions.LocalToPrj ( vorPntPrj, angI2 + sideInInt * ARANMath.DegToRad ( i ), _dl2, 0 ) );
				//InitHolding.ui.DrawPointWithText ( tmpPnt, 0, "Tmp" );
				prtctSecPnt.Add ( ( LineString ) TransForm.Move ( tmpPart, pntI2, tmpPnt ) );
				//_controller.DrawLineString ( PrtctSecPnt [ PrtctSecPnt.Count - 1 ], 0, 1 );
			}
			prtctSecPnt.Add ( ( LineString ) TransForm.Move ( tmpPart, pntI2, pntI4 ) );
			//_controller.DrawMultiLineString ( PrtctSecPnt, 0, 1);

			//PrtctAreaDirEntr2SecPnt = ARANFunctions.PolyLineToPolygon ( PrtctSecPnt );
			//Polygon tmp = ChainHullAlgorithm.ConvexHull ( _basicArea, multiPoint );
			var result = ChainHullAlgorithm.ConvexHull ( _basicArea, prtctSecPnt.ToMultiPoint ( ) );
			//ChainHullAlgorithm.ConvexHull ( PrtctSecPnt );
			//result = ( ( MultiPolygon ) GlobalParams.GeomOperators.Difference ( convexedAllPolygon, _basicArea ) ) [ 0 ];
			//InitHolding.ui.DrawPolygon ( result, 255*255, eFillStyle.sfsCross );

			return result;
		}

		private void TowardConstructFixToleranceArea ( Point dmePntPrj, Point vorPntPrj, double nominalDistanceInPrj, double direction, SideDirection side, double trackingTolerance, double d1, Point dsgPntPrj, bool draw )
		{
            double tmpOutDouble;
			var a1 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj - d1, vorPntPrj, direction + trackingTolerance, out tmpOutDouble );
			var a2 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj + d1, vorPntPrj, direction + trackingTolerance, out tmpOutDouble );
			var a3 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj - d1, vorPntPrj, direction - trackingTolerance, out tmpOutDouble );
			var a4 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj + d1, vorPntPrj, direction - trackingTolerance, out tmpOutDouble );

			if ( draw )
				DrawToleranceArea ( EntryDirection.Toward, a1, a2, a3, a4, dmePntPrj, dsgPntPrj );

            MultiPoint tmpMultipnt = new MultiPoint ( );
			ARANFunctions.AddArcToMultiPoint ( dmePntPrj, a3, a1, TurnDirection.CCW, ref tmpMultipnt );
			ARANFunctions.AddArcToMultiPoint ( dmePntPrj, a2, a4, TurnDirection.CW, ref tmpMultipnt );
			MultiPoint minAndmaxPoints = GeomFunctions.MinAndMaxDistFromLineToMultipoint ( vorPntPrj, direction - ARANMath.C_PI_2, tmpMultipnt );

			_priorFixTolerance = ARANFunctions.ReturnDistanceInMeters ( minAndmaxPoints [ 0 ], dsgPntPrj );
			_postFixTolerance = ARANFunctions.ReturnDistanceInMeters ( minAndmaxPoints [ 1 ], dsgPntPrj );
			double directionPerpendicular = direction - ARANMath.C_PI_2;

			_toleranceArea.Clear ( );
			_toleranceArea.Add ( ( Point ) ARANFunctions.LineLineIntersect ( minAndmaxPoints [ 0 ], directionPerpendicular, vorPntPrj, direction - ( int ) side * trackingTolerance ) );
			_toleranceArea.Add ( ( Point ) ARANFunctions.LineLineIntersect ( minAndmaxPoints [ 1 ], directionPerpendicular, vorPntPrj, direction - ( int ) side * trackingTolerance ) );
			_toleranceArea.Add ( ( Point ) ARANFunctions.LineLineIntersect ( minAndmaxPoints [ 1 ], directionPerpendicular, vorPntPrj, direction + ( int ) side * trackingTolerance ) );
			_toleranceArea.Add ( ( Point ) ARANFunctions.LineLineIntersect ( minAndmaxPoints [ 0 ], directionPerpendicular, vorPntPrj, direction + ( int ) side * trackingTolerance ) );
		}

		#endregion

		#region Away part

		public Polygon AwayConstructBasicArea ( SideDirection side, Point dmePntPrj, Point vorPntPrj, Point designatedPointPrj, double direction,
													double nominalDistanceInPrj, double nominalDistance, double limitingDistanceInPrj, double limitingDistance,
													double time, double tas, double legLength, double altitude, double radius, bool drawShablon, bool drawNominalTraject, bool drawToleranceArea, bool defineValidArea4DsgpPnt = false )
		{
			GlobalParams.ClearTestVersionElements ( );
			NavaidsDataBase navaidsDb = GlobalParams.NavaidDatabase;
			double d1 = navaidsDb.Dme.MinimalError + navaidsDb.Dme.ErrorScalingUp * nominalDistance;
			double d2 = navaidsDb.Dme.MinimalError + navaidsDb.Dme.ErrorScalingUp * limitingDistance;

			AwayConstructFixToleranceArea ( dmePntPrj, vorPntPrj, nominalDistanceInPrj, direction, side, navaidsDb.Vor.TrackingTolerance, d1, designatedPointPrj, drawToleranceArea );

			//LineString nominalTrack;
			Point pntE;
			Point pntR;

			Shablons.HoldingShablon ( _controller, designatedPointPrj, tas, altitude, time, direction, side, 0, out _nominalTrack, out _shablon, out _line3, out _turn180, out pntE, out pntR );

			//nominalTrack = ( LineString ) TransForm.RotateGeometry ( designatedPointPrj, nominalTrack, ARANMath.C_PI );
			//_turn180 = ( LineString ) TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI );
			//LineString tmpLine3 = ( LineString ) TransForm.RotateGeometry ( designatedPointPrj, _line3, ARANMath.C_PI );
			//for ( int i = 0; i < tmpLine3.Count - 1; i++ )
			//{
			//    tmpLine3 [ i ].M = _line3 [ i ].M + ARANMath.C_PI;
			//}
			//_line3.Assign ( tmpLine3 );
			//_shablon = ( Polygon ) TransForm.RotateGeometry ( designatedPointPrj, _shablon, ARANMath.C_PI );
			//pntE = ( Point ) TransForm.RotateGeometry ( designatedPointPrj, pntE, ARANMath.C_PI );

			if ( !defineValidArea4DsgpPnt )
			{
				LineString lnStringTemplate = new LineString ( );
				if ( GlobalParams.IsTestVersion )
					lnStringTemplate.Add ( designatedPointPrj );
				lnStringTemplate.AddMultiPoint ( _shablon.ToMultiPoint ( ) );

				_controller.SafeDeleteGraphic ( _templateHandle );
				if ( drawShablon )
					_templateHandle = _controller.DrawLineString ( lnStringTemplate, 255, 1 );

				_controller.SafeDeleteGraphic ( _nominalTrackHandle );
				if ( drawNominalTraject )
					_nominalTrackHandle = _controller.DrawLineString ( _nominalTrack, 1, 1 );
			}

			//_controller.SafeDeleteGraphic ( _turn180Handle );
			//_turn180Handle = _controller.DrawLineString ( _turn180, 255 * 255 * 255, 1 );

			#region Locating manners of C1, C2 and C3 points are same in With limiting Radial

			Line straightPart = GetStraightPartOfPolygon ( side, direction, vorPntPrj, _shablon );
			straightPart.RefPoint = ( Point ) TransForm.Move ( straightPart.RefPoint, designatedPointPrj, _toleranceArea[ 0 ] );


			double tmpOutDouble;
			double dl1 = limitingDistanceInPrj + d2;
			_dl2 = limitingDistanceInPrj - d2;

			Point pntC1 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, dl1, straightPart.RefPoint, straightPart.DirVector.Direction - ( int ) side * ARANMath.C_PI, out tmpOutDouble );
			//Point pntC12 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, dl1, straightPart.RefPoint, straightPart.DirVector.Direction, out tmpOutDouble );

			//Point pntC3OnStraightPart = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, straightPart.RefPoint, ARANMath.Modulus ( straightPart.DirVector.Direction - ( int ) side * ARANMath.C_PI, ARANMath.C_2xPI ), out tmpOutDouble );
			Point pntC3OnStraightPart = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, straightPart.RefPoint, straightPart.DirVector.Direction - ( int ) side * ARANMath.C_PI, out tmpOutDouble );

			//if ( GlobalParams.IsTestVersion )
			//{
			//    _controller.DrawPolygon ( ( Polygon ) TransForm.Move ( _shablon, designatedPointPrj, _toleranceArea[ 0 ] ), 1, eFillStyle.sfsHollow );

			//    _controller.DrawPointWithText ( straightPart.RefPoint, 1, "Start" );
			//    _controller.DrawPointWithText ( ARANFunctions.LocalToPrj ( straightPart.RefPoint, straightPart.DirVector.Direction, 50000, 0 ), 1, "End" );
			//    //_controller.DrawPointWithText ( ARANFunctions.LocalToPrj ( straightPart.RefPoint, ARANMath.Modulus ( straightPart.DirVector.Direction, ARANMath.C_2xPI ), 50000, 0 ), 1, "End" );

			//    LineString testLnString = new LineString ( );
			//    testLnString.Add ( straightPart.RefPoint );
			//    testLnString.Add ( ARANFunctions.LocalToPrj ( straightPart.RefPoint, straightPart.DirVector.Direction, 50000, 0 ) );
			//    _controller.DrawLineString ( testLnString, 1, 1 );

			//    _controller.DrawPointWithText ( pntC3OnStraightPart, 1, "C3 On" );
			//    //_controller.DrawPointWithText ( pntC3OnStraightPart2, 1, "C3 On2" );
			//    _controller.DrawPointWithText ( pntC1, 1, "C1" );
			//    //_controller.DrawPointWithText ( pntC1, 1, "C1_2" );

			//    LineString circleDL1 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, dl1 );
			//    LineString circleDL2 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, _dl2 );
			//    _controller.DrawLineString ( circleDL1, 1, 1 );
			//    _controller.DrawLineString ( circleDL2, 1, 1 );
			//}

			#endregion

			LineString rps = new LineString
			{
				ARANFunctions.PointAlongPlane(vorPntPrj, direction + (int) side * navaidsDb.Vor.TrackingTolerance,
					1.5 * nominalDistanceInPrj),
				vorPntPrj,
				ARANFunctions.PointAlongPlane(vorPntPrj, direction - (int) side * navaidsDb.Vor.TrackingTolerance,
					1.5 * nominalDistanceInPrj)
			};

			Polygon result;
			double angleRl2 = double.NaN;
			if ( pntC3OnStraightPart.IsEmpty || pntC1.IsEmpty )
			{
				result = AwayConstructBasicAreaWithLimitingRadial ( navaidsDb.Vor, designatedPointPrj, direction, altitude, tas, side, dmePntPrj, straightPart, dl1, pntC1, pntC3OnStraightPart, vorPntPrj, d2, d1, rps, pntR, out angleRl2 );

			}
			else
				result = AwayConstructBasicAreaWithoutLimitingRadial ( navaidsDb.Vor, pntC1, pntC3OnStraightPart, d2, d1, dmePntPrj, direction, vorPntPrj, side, dl1, designatedPointPrj, rps, straightPart );
			_controller.SetLimitingRadial ( ARANMath.DegToRad ( angleRl2 ) );
			result.InteriorRingList.Clear ( );
			return result;
		}

		private Polygon AwayConstructBasicAreaWithLimitingRadial ( VorData vordata, Point designatedPointPrj, double direction, double altitude, double tas, SideDirection side, Point dmePntPrj, Line straightPart, double dl1, Point pntC1, Point pntC3OnStraightPart, Point vorPntPrj, double d2, double d1, LineString rps, Point pntR, out double angleRl2 )
		{
			#region Locating R point 3.3.4.3.1.2. a)

			#region Finding intersection point of the outline of the tmeplate with the C axis

			LineString dirPart = new LineString
			{
				_toleranceArea[1],
				ARANFunctions.LocalToPrj(_toleranceArea[1], direction - ARANMath.C_PI, 50000, 0)
			};

			Polygon tmpPolygon = ( Polygon ) TransForm.Move ( _shablon, designatedPointPrj, _toleranceArea [ 1 ] );
			Geometry geometry = GlobalParams.GeomOperators.Intersect ( ARANFunctions.PolygonToPolyLine ( tmpPolygon ), ARANFunctions.CreatePolyLineFromParts ( dirPart ) );
			Point pntIntersct = ( ( MultiPoint ) geometry ) [ 0 ];

			#endregion 

			//double V = tas;
			//double v3600 = V;
			//double R = 943.27 * 0.277777777777778 / V;
			//if ( ( R > 3.0 ) )
			//    R = 3.0;

			//double Rv = 3600 * V / ( 62.83 * R );
			//double H = altitude / 1000.0;
			//double w = 12.0 * H + 87.0;
			//double w_ = 0.277777777777778 * w;
			//double E = w_ / R;
			//E = E / ARANMath.DegToRad ( 1 );
			//double ab = 5.0 * v3600;

			//Point pntCnt = ARANFunctions.LocalToPrj ( designatedPointPrj, direction, -ab, -Rv * ( int ) side );
			//pntCnt = ( Point ) TransForm.RotateGeometry ( designatedPointPrj, pntCnt, ARANMath.C_PI );
			//pntCnt = ( Point ) TransForm.Move ( pntCnt, designatedPointPrj, _toleranceArea [ 1 ] );
			////_controller.DrawPointWithText ( _toleranceArea[ 1 ], 1, "A2" );

			//double angle = direction + ( int ) side * ARANMath.C_PI_2;
			////Point StrtPnt = ARANFunctions.LocalToPrj ( pntCnt, angle, Rv + 5 * w_, 0 );
			////StrtPnt = ( Point ) TransForm.RotateGeometry ( pntCnt, StrtPnt, ARANMath.C_PI );

			//double touchAngle = Shablons.SpiralTouchToFix ( pntCnt, E, Rv + 5 * w_, angle, side, pntIntersct, side, ARANMath.Modulus ( direction + ( int ) side * ARANMath.C_PI, ARANMath.C_2xPI ) );
			//Point pntR = ARANFunctions.LocalToPrj ( pntCnt, angle + ( int ) side * touchAngle, ( Rv + 5 * w_ ) + E * touchAngle, 0 );
			#endregion

			//pntR = ( Point ) TransForm.RotateGeometry ( designatedPointPrj, pntR, ARANMath.C_PI );
			pntR = ( Point ) TransForm.Move ( pntR, designatedPointPrj, _toleranceArea [ 1 ] );

			double angleRl = ARANMath.Modulus ( ARANFunctions.ReturnAngleInRadians ( dmePntPrj, pntR ), ARANMath.C_2xPI );
			angleRl += ( int ) side * vordata.TrackingTolerance;
			angleRl = Math.Ceiling ( ARANMath.RadToDeg ( angleRl ) );

			angleRl = ARANMath.DegToRad ( angleRl );

			LineString lineStringRl = new LineString {dmePntPrj, ARANFunctions.LocalToPrj(dmePntPrj, angleRl, 100000, 0)};

			Line lineRl2 = new Line {RefPoint = dmePntPrj};
			angleRl2 = angleRl + ( int ) side * vordata.TrackingTolerance;
			lineRl2.DirVector = new Vector ( angleRl2 );

			LineString lineStringRl2 = new LineString
			{
				lineRl2.RefPoint,
				ARANFunctions.LocalToPrj(lineRl2.RefPoint, lineRl2.DirVector.Direction, 1.25 * dl1, 0)
			};

			#region 3.3.4.3.1.4

			LineString lineStringsStraightPart = new LineString
			{
				straightPart.RefPoint,
				ARANFunctions.LocalToPrj(straightPart.RefPoint, straightPart.DirVector.Direction, 2 * dl1, 0)
			};

			Point intersectCurve1Rl2 = ( Point ) ARANFunctions.LineLineIntersect ( straightPart, lineRl2 );

			//double distA1IntersectCurve1Dl1 = ARANFunctions.ReturnDistanceInMeters ( _toleranceArea [ 0 ], pntC1 );
			//double distA1IntersectCurve1Rl2 = ARANFunctions.ReturnDistanceInMeters ( _toleranceArea [ 0 ], intersectCurve1Rl2 );
			//double distA1IntersectCurve1Dl2 = ARANFunctions.ReturnDistanceInMeters ( _toleranceArea [ 0 ], pntC3OnStraightPart );

			Point pntC2 = null;
			double tmpOutDouble;


			//if ( distA1_IntersectCurve1_RL2 < distA1_IntersectCurve1_DL1 )
			if(pntC1.IsEmpty)
			{
				// 3.3.4.3.1.4 a)
				pntC1.Assign ( intersectCurve1Rl2 );
				pntC2 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, dl1, lineRl2, out tmpOutDouble );
				_pntC3 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, lineRl2, out tmpOutDouble );
			} 
			else if (pntC3OnStraightPart.IsEmpty)
			{
				// 3.3.4.3.1.4 b)
				pntC2 = new Point ( intersectCurve1Rl2 );
				_pntC3 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, lineRl2, out tmpOutDouble );
			}

			//else if ( distA1_IntersectCurve1_RL2 < distA1_IntersectCurve1_DL2 )
			//{
			//    // 3.3.4.3.1.4 b)
			//    pntC2 = new Point ( intersectCurve1_RL2 );
			//    _pntC3 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, lineRL2, out tmpOutDouble );
			//}
			//else
			//{
			//    // 3.3.4.3.1.4 c)
			//    tmpline = new Line ( pntC3OnStraightPart, pntC1 );
			//    pntC2 = ARANFunctions.LocalToPrj ( pntC3OnStraightPart, tmpline.DirVector.Direction, Math.Abs ( d2 + d1 - 1800 ), 0 );

			//    tmpline = new Line ( pntC2, direction );
			//    _pntC3 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, tmpline, out tmpOutDouble );
			//}

			Point pntC4, pntC5;
			LocateC4_5_6Points ( designatedPointPrj, dmePntPrj, vorPntPrj, direction, side, vordata, dl1, d1, d2, rps, out pntC5, out pntC4 );

			#endregion

			MultiLineString tmpMultiLineString = new MultiLineString
			{
				((LineString) TransForm.Move(_turn180, designatedPointPrj, _toleranceArea[0]))
			};

			tmpMultiLineString [ 0 ].Add ( pntC1 );
			
			LineString turn180OnA3 = ( ( LineString ) TransForm.Move ( _turn180, designatedPointPrj, _toleranceArea [ 3 ] ) );
			tmpMultiLineString.Add ( turn180OnA3 );

			//_controller.DrawLineString ( tmpMultiLineString[ 0 ], 1, 1 );
			//_controller.DrawLineString ( tmpMultiLineString[ 1 ], 1, 1 );
			_basicArea = ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( tmpMultiLineString ) ) [ 0 ];
			//_controller.DrawPolygon ( _basicArea, 0, eFillStyle.sfsHorizontal );
			//return _basicArea;

			tmpMultiLineString.Clear ( );
			LineString tmpTurn180C1 = ( LineString ) TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI );
			tmpTurn180C1 = ( LineString ) TransForm.Move ( tmpTurn180C1, designatedPointPrj, pntC1 );
			tmpMultiLineString.Add ( ( LineString ) tmpTurn180C1.Clone ( ) );

			LineString tmpTurn180C2 = ( LineString ) TransForm.Move ( tmpTurn180C1, pntC1, pntC2 );
			tmpMultiLineString.Add ( ( LineString ) tmpTurn180C2.Clone ( ) );

			LineString tmpTurn180C3 = ( LineString ) TransForm.Move ( tmpTurn180C2, pntC2, _pntC3 );
			tmpMultiLineString.Add ( ( LineString ) tmpTurn180C3.Clone ( ) );

			Polygon plygonC1C2C3 = ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( tmpMultiLineString ) ) [ 0 ];
			//_controller.DrawPolygon ( plygonC1_C2_C3, 255*255, eFillStyle.sfsVertical );
			Geometry geomLeft = null, geomRight = null;

			//try
			//{
			LineString lnStringCutter = new LineString
			{
				ARANFunctions.LocalToPrj(straightPart.RefPoint, straightPart.DirVector.Direction - ARANMath.C_PI, 100000, 0),
				ARANFunctions.LocalToPrj(straightPart.RefPoint, straightPart.DirVector.Direction, 100000, 0)
			};
			//_controller.DrawLineString ( lnStringCutter, 1, 1 );
			if ( GlobalParams.GeomOperators.Crosses ( plygonC1C2C3, lnStringCutter ) )
				GlobalParams.GeomOperators.Cut ( plygonC1C2C3, lnStringCutter, out geomLeft, out geomRight );
			if ( side == SideDirection.sideRight )
			{
				if ( geomRight != null )
					plygonC1C2C3 = ( ( MultiPolygon ) geomRight )[ 0 ];
			}
			else if ( side == SideDirection.sideLeft )
			{
				if ( geomLeft != null )
					plygonC1C2C3 = ( ( MultiPolygon ) geomLeft )[ 0 ];
			}
			_basicArea = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( _basicArea, plygonC1C2C3 ) )[ 0 ];
			//return _basicArea;

			if ( GlobalParams.IsTestVersion )
			{
				//LineString tmpPart = new LineString ( );
				//angle = ARANMath.Modulus ( angle + ARANMath.C_PI, ARANMath.C_2xPI );
				//Shablons.CreateSpiralBy2Radial ( pntCnt, Rv + 5 * w_, angle, ARANMath.Modulus ( direction + ARANMath.C_PI, ARANMath.C_2xPI ), E, ( int ) side, tmpPart );
				//_controller.DrawLineString ( tmpPart, 255 * 255, 1 );
				//_controller.DrawPointWithText ( pntCnt, 0, "Cnt" );
				//_controller.DrawPointWithText ( StrtPnt, 0, "Str Pnt" );

				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( rps, 1, 1 ) );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( pntIntersct, 0, "Int" ));
				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( pntR, 0, "R point" ));
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( dirPart, 1, 1 ));
				//GlobalParams.TestVersionHandles.Add ( _controller.DrawPolygon ( tmpPolygon, 1, eFillStyle.sfsHollow ));
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( lineStringRl, 1, 1 ));
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( lineStringRl2, 255, 1 ));

				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( lineStringsStraightPart, 1, 1 ));
				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( intersectCurve1Rl2, 1, "Rl2_Curve" ));

				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( pntC1, 1, "c1" ));
				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( pntC2, 1, "c2" ));
				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( _pntC3, 1, "c3" ));
				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( pntC4, 1, "c4" ));
				if ( pntC5 != null )
					GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( pntC5, 1, "c5" ));
				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( _pntC6, 1, "c6" ));

				

				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( tmpTurn180C1, 1, 1 ));
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( tmpTurn180C2, 1, 1 ));
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( tmpTurn180C3, 1, 1 ));


				LineString circleDl1 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, dl1 );
				LineString circleDl2 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, _dl2 );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( circleDl1, 1, 1 ) );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( circleDl2, 1, 1 ) );

				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( _line3, 1, 1 ) );

				//LineString line3OnA4 = ARANFunctions.CreatePolyLineFromParts ( _line3 )[ 0 ];
				//line3OnA4 = ( LineString ) TransForm.Move ( line3OnA4, designatedPointPrj, _toleranceArea[ 3 ] );
				//GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( line3OnA4, 1, 1 ) );
			}
			//return _basicArea;
			Point tmpPoint;
			GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180C3, out tmpPoint );
			Ring ringMinDistC3C6 = new Ring {pntC1, _pntC3, (Point) tmpPoint.Clone()};

			double ang1 = ARANFunctions.ReturnAngleInRadians ( dmePntPrj, _pntC3 );
			double ang2 = ARANFunctions.ReturnAngleInRadians ( dmePntPrj, _pntC6 );

			double littleAng = ARANMath.SubtractAngles ( ang1, ang2 );
			int n = ( int ) Math.Floor ( ARANMath.RadToDeg ( littleAng ) );

			LineString tmpTurn180 = tmpTurn180C3;
			Point tmp = new Point ( _pntC3 );

			MultiLineString tmpTurns = new MultiLineString ( );
			Polygon polygonTurn180SOnDl2 = new Polygon ( );
			for ( int i = 1; i <= n; i++ )
			{
				tmpPoint = ARANFunctions.LocalToPrj ( dmePntPrj, ang1 - ( int ) side * ARANMath.DegToRad ( i ), _dl2, 0 );
				tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, tmp, tmpPoint );
				tmpTurns.Add ( ( LineString ) tmpTurn180.Clone ( ) );
				tmp.Assign ( tmpPoint );
				GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
				ringMinDistC3C6.Add ( ( Point ) tmpPoint.Clone ( ) );
			}
			tmpMultiLineString.Clear ( );
			tmpMultiLineString.Add ( turn180OnA3 );

			tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, tmp, _pntC6 );
			GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
			ringMinDistC3C6.Add ( ( Point ) tmpPoint.Clone ( ) );
			ringMinDistC3C6.Add ( _pntC6 );
			ringMinDistC3C6.Add ( _toleranceArea [ 1 ] );
			tmpMultiLineString.Add ( ( LineString ) tmpTurn180.Clone ( ) );

			tmpMultiLineString[ tmpMultiLineString.Count - 1 ].Add ( tmpPoint );
			tmpMultiLineString[ tmpMultiLineString.Count - 1 ].Add ( _pntC6 );

			Polygon polygonOnC3C6 = new Polygon {ExteriorRing = ringMinDistC3C6};

			_basicArea = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( _basicArea, polygonOnC3C6 ) ) [ 0 ];
			//_controller.DrawPolygon ( _basicArea, 1, eFillStyle.sfsHollow );

			tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, _pntC6, pntC4 );
			tmpMultiLineString.Add ( ( LineString ) tmpTurn180.Clone ( ) );

			if ( pntC5 != null )
			{
				tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, pntC4, pntC5 );
				tmpMultiLineString.Add ( ( LineString ) tmpTurn180.Clone ( ) );
			}
			tmpMultiLineString[ tmpMultiLineString.Count - 1 ].Add ( _toleranceArea[ 0 ] );
			Polygon convexedOnC4C5C6 = ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( tmpMultiLineString ) ) [ 0 ];
			_basicArea = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( _basicArea, convexedOnC4C5C6 ) ) [ 0 ];
			return _basicArea;
		}

		private Polygon AwayConstructBasicAreaWithoutLimitingRadial ( VorData vordata, Point pntC1, Point pntC3OnStraightPart, double d2, double d1, Point dmePntPrj, double direction, Point vorPntPrj, SideDirection side, double dL1, Point designatedPointPrj, LineString rps, Line straightPart )
		{
			Line tmpline = new Line ( pntC3OnStraightPart, pntC1 );
			Point pntC2 = ARANFunctions.LocalToPrj ( pntC3OnStraightPart, tmpline.DirVector.Direction, Math.Abs ( d2 + d1 - 1800 ), 0 );

			tmpline = new Line ( pntC2, direction );
			double tmpOutDouble;
			_pntC3 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, tmpline, out tmpOutDouble );

			Point pntC4, pntC5;

			LocateC4_5_6Points ( designatedPointPrj, dmePntPrj, vorPntPrj, direction, side, vordata, dL1, d1, d2, rps, out pntC5, out pntC4 );

			#region Protection of the inbound turn

			MultiLineString tmpMultiLineString = new MultiLineString ( );

			LineString turn180A1 = ( ( LineString ) TransForm.Move ( _turn180, designatedPointPrj, _toleranceArea[ 0 ] ) );
			turn180A1.Add ( pntC2 );
			tmpMultiLineString.Add ( turn180A1  );
			
			LineString turn180OnA3 = ( ( LineString ) TransForm.Move ( _turn180, designatedPointPrj, _toleranceArea [ 3 ] ) );
			tmpMultiLineString.Add ( turn180OnA3 );

			_basicArea = ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( tmpMultiLineString ) ) [ 0 ];

			//LineString lnStringCutter = new LineString ( );
			//lnStringCutter.Add ( ARANFunctions.LocalToPrj ( straightPart.RefPoint, straightPart.DirVector.Direction - ARANMath.C_PI, 100000, 0 ) );
			//lnStringCutter.Add ( ARANFunctions.LocalToPrj ( straightPart.RefPoint, straightPart.DirVector.Direction, 100000, 0 ) );
			//GlobalParams.GeomOperators.Cut ( plygonC1_C2_C3, lnStringCutter, out geomLeft, out geomRight );

			tmpMultiLineString.Clear ( );
			LineString tmpTurn180C2 = ( LineString ) TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI );
			tmpTurn180C2 = ( LineString ) TransForm.Move ( tmpTurn180C2, designatedPointPrj, pntC2 );
			tmpMultiLineString.Add ( ( LineString ) tmpTurn180C2.Clone ( ) );

			LineString tmpTurn180C3 = ( LineString ) TransForm.Move ( tmpTurn180C2, pntC2, _pntC3 );
			tmpMultiLineString.Add ( ( LineString ) tmpTurn180C3.Clone ( ) );

			Polygon plygonC2C3 = ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( tmpMultiLineString ) ) [ 0 ];

			Geometry geomLeft = null, geomRight = null;
			LineString lnStringCutter = new LineString
			{
				ARANFunctions.LocalToPrj(straightPart.RefPoint, straightPart.DirVector.Direction - ARANMath.C_PI, 100000, 0),
				ARANFunctions.LocalToPrj(straightPart.RefPoint, straightPart.DirVector.Direction, 100000, 0)
			};



			if ( GlobalParams.GeomOperators.Crosses ( plygonC2C3, lnStringCutter ) )
				GlobalParams.GeomOperators.Cut ( plygonC2C3, lnStringCutter, out geomLeft, out geomRight );
			if ( side == SideDirection.sideRight )
			{
				if ( geomRight != null )
					plygonC2C3 = ( ( MultiPolygon ) geomRight )[ 0 ];
			}
			else if ( side == SideDirection.sideLeft )
			{
				if ( geomLeft != null )
					plygonC2C3 = ( ( MultiPolygon ) geomLeft )[ 0 ];
			}
			_basicArea = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( _basicArea, plygonC2C3 ) )[ 0 ];

			//if ( GlobalParams.IsTestVersion )
			//{
			//    GlobalParams.TestVersionHandles.Add ( _controller.DrawPolygon ( plygonC2_C3, 1, eFillStyle.sfsCross ) );
			//    GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( tmpTurn180c2, 1, 1 ) );
			//    GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( tmpTurn180c2, 1, 1 ) );
			//    GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( tmpTurn180c3, 1, 1 ) );
			//    LineString circleDL1 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, dL1 );
			//    LineString circleDL2 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, _dl2 );
			//    GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( circleDL1, 1, 1 ) );
			//    GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( circleDL2, 1, 1 ) );
			//    GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( lnStringCutter, 1, 1 ) );
			//}
			//return _basicArea;
			Point tmpPoint;
			GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180C3, out tmpPoint );
			Ring minDistOc3C6 = new Ring {_toleranceArea[0], pntC1, (Point) tmpPoint.Clone()};

			double ang1 = ARANFunctions.ReturnAngleInRadians ( dmePntPrj, _pntC3 );
			double ang2 = ARANFunctions.ReturnAngleInRadians ( dmePntPrj, _pntC6 );

			double littleAng = ARANMath.SubtractAngles ( ang1, ang2 );
			int n = ( int ) Math.Floor ( ARANMath.RadToDeg ( littleAng ) );

			LineString tmpTurn180 = tmpTurn180C3;
			Point tmp = new Point ( _pntC3 );

			MultiLineString tmpTurns = new MultiLineString ( );
			Polygon polygonTurn180SOnDl2 = new Polygon ( );
			for ( int i = 1; i <= n; i++ )
			{
				tmpPoint = ARANFunctions.LocalToPrj ( dmePntPrj, ang1 - ( int ) side * ARANMath.DegToRad ( i ), _dl2, 0 );
				tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, tmp, tmpPoint );
				tmpTurns.Add ( ( LineString ) tmpTurn180.Clone ( ) );
				tmp.Assign ( tmpPoint );
				GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
				minDistOc3C6.Add ( ( Point ) tmpPoint.Clone ( ) );
			}
			minDistOc3C6.Add ( _toleranceArea [ 3 ] );
			Polygon polygonOnC3C6 = new Polygon {ExteriorRing = minDistOc3C6};
			//_controller.DrawPointWithText ( minDistOC3_C6[ minDistOC3_C6.Count - 2 ], 1, "Err" );
			//_basicArea.ExteriorRing.Add ( minDistOC3_C6[ minDistOC3_C6.Count - 1 ] );
			_basicArea = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( _basicArea, polygonOnC3C6 ) ) [ 0 ];

			tmpMultiLineString.Clear ( );
			tmpMultiLineString.Add ( turn180OnA3 );

			tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, tmp, _pntC6 );
			tmpMultiLineString.Add ( ( LineString ) tmpTurn180.Clone ( ) );
			tmpMultiLineString[ 1 ].Add ( minDistOc3C6[ minDistOc3C6.Count - 2 ] );
			tmpMultiLineString[ 1 ].Add ( _pntC6 );

			tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, _pntC6, pntC4 );
			tmpMultiLineString.Add ( ( LineString ) tmpTurn180.Clone ( ) );

			if ( pntC5 != null )
			{
				tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, pntC4, pntC5 );
				tmpMultiLineString.Add ( ( LineString ) tmpTurn180.Clone ( ) );
			}
			Polygon convexedOnC4C5C6 = ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( tmpMultiLineString ) ) [ 0 ];
			_basicArea = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( _basicArea, convexedOnC4C5C6 ) ) [ 0 ];
			#endregion

			if ( GlobalParams.IsTestVersion )
			{
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( rps, 1, 1 ) );

				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( pntC1, 1, "C1" ) );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( pntC3OnStraightPart, 1, "C'3" ) );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( pntC2, 1, "C2" ) );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( _pntC3, 1, "C3" ) );

				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( pntC4, 1, "c4" ) );
				if ( pntC5 != null )
					GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( pntC5, 1, "c5" ) );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( _pntC6, 1, "c6" ) );

				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( tmpTurn180C2, 1, 1 ) );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( tmpTurn180C2, 1, 1 ) );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( tmpTurn180C3, 1, 1 ) );

				LineString circleDl1 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, dL1 );
				LineString circleDl2 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, _dl2 );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( circleDl1, 1, 1 ) );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( circleDl2, 1, 1 ) );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( lnStringCutter, 1, 1 ) );
				//GlobalParams.TestVersionHandles.Add ( _controller.DrawRing ( minDistOC3_C6, 1, eFillStyle.sfsCross ) );
			}
			return _basicArea;
		}

		private void LocateC4_5_6Points ( Point designatedPointPrj, Point dmePntPrj, Point vorPntPrj, double direction, SideDirection side, VorData vorData, double dL1, double d1, double d2, LineString rps, out Point pntC5, out Point pntC4)
		{
			LineString line3OnA4 = ARANFunctions.CreatePolyLineFromParts ( _line3 )[ 0 ];
			line3OnA4 = ( LineString ) TransForm.Move ( line3OnA4, designatedPointPrj, _toleranceArea[ 3 ] );

			LineString circleDl1 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, dL1 );
			LineString circleDl2 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, _dl2 );
			double tmpOutDouble;
			pntC5 = new Point ( );

			Geometry geom = GlobalParams.GeomOperators.Intersect ( line3OnA4, ARANFunctions.CreatePolyLineFromParts ( circleDl1 ) );
			// 3.3.4.2.1.4 e 2-ci punkt
			if ( geom.IsEmpty )
			{
				pntC4 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, dL1, vorPntPrj, direction - ( int ) side * vorData.TrackingTolerance, out tmpOutDouble );
				_pntC6 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, direction - ( int ) side * vorData.TrackingTolerance, out tmpOutDouble );
				return;
			}
			LineString rp2 = new LineString
			{
				vorPntPrj,
				ARANFunctions.LocalToPrj(vorPntPrj, direction - (int) side * vorData.TrackingTolerance, 1000000000, 0)
			};
			pntC4 = ( ( MultiPoint ) geom )[ 0 ];

			geom = GlobalParams.GeomOperators.Intersect ( line3OnA4, ARANFunctions.CreatePolyLineFromParts ( circleDl2 ) );
			if ( geom.IsEmpty )
			{
				geom = GlobalParams.GeomOperators.Intersect ( line3OnA4, rp2 );
				if ( !geom.IsEmpty )
					pntC5 = ( ( MultiPoint ) geom )[ 0 ];
				geom = GlobalParams.GeomOperators.Intersect ( circleDl2, rp2 );
				_pntC6 = new Point ( );
				if ( !geom.IsEmpty )
					_pntC6 = ( ( MultiPoint ) geom )[ 0 ];
				return;
			}
			Point pntC6OnLine3 = ( ( MultiPoint ) geom )[ 0 ];

			Line tmpline = new Line ( pntC6OnLine3, pntC4 );
			pntC5 = ARANFunctions.LocalToPrj ( pntC6OnLine3, tmpline.DirVector.Direction, Math.Abs ( d2 + d1 - 1800 ), 0 );
			tmpline = new Line ( pntC5, direction );


			_pntC6 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, tmpline, out tmpOutDouble );

			if ( ARANMath.SideDef ( vorPntPrj, direction - ( int ) side * vorData.TrackingTolerance, pntC4 ) != side )
			{
				pntC4 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, dL1, vorPntPrj, direction - ( int ) side * vorData.TrackingTolerance, out tmpOutDouble );
				_pntC6 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, direction - ( int ) side * vorData.TrackingTolerance, out tmpOutDouble );
				pntC5 = null;
			}
			else if ( ARANMath.SideDef ( vorPntPrj, direction - ( int ) side * vorData.TrackingTolerance, pntC5 ) != side )
			{
				pntC5 = ( ( MultiPoint ) GlobalParams.GeomOperators.Intersect ( line3OnA4, rps ) )[ 0 ];
				_pntC6 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, direction - ( int ) side * vorData.TrackingTolerance, out tmpOutDouble );
			}
		}

		public Polygon AwayConstructProtectSector1 ( VorData vordata, double direction, Point designatedPointPrj, Point dmePntPrj, Point vorPntPrj, SideDirection side, double altitude, double ias, double tas )
		{
			double rad = ARANFunctions.ReturnAngleInRadians ( _toleranceArea [ 0 ], _toleranceArea [ 3 ] );
			rad = ARANMath.SubtractAngles ( rad, direction );

            double v = tas;
			double v3600 = v;
			double r = 943.27 * 0.277777777777778 / v;
			if ( ( r > 3.0 ) )
				r = 3.0;

			double rv = 3600 * v / ( 62.83 * r );
			double h = altitude / 1000.0;
			double w = 12.0 * h + 87.0;
			double w_ = 0.277777777777778 * w;
			double e = w_ / r;
			e = ARANMath.RadToDeg ( e );
			double ab = 5.0 * v3600;

			Point ptCntB = ARANFunctions.LocalToPrj ( designatedPointPrj, direction, -ab, -rv * ( int ) side );
			ptCntB = ( Point ) TransForm.RotateGeometry ( designatedPointPrj, ptCntB, ARANMath.C_PI );
			ptCntB = ( Point ) TransForm.Move ( ptCntB, designatedPointPrj, _toleranceArea [ 3 ] );
			ptCntB = ( Point ) TransForm.Flip ( ptCntB, _toleranceArea [ 3 ], direction );
			ptCntB = ( Point ) TransForm.RotateGeometry ( _toleranceArea [ 3 ], ptCntB, -( int ) side * rad );

			SideDirection newSide = ARANMath.ChangeDirection ( side );
			var alfa0 = ARANMath.Modulus ( direction + ( int ) side * ARANMath.C_PI_2 - ( int ) side * rad, ARANMath.C_2xPI );
			var r0 = rv + 5 * w_;
			var nominalDirection = alfa0 + ( int ) newSide * ARANMath.C_PI_2;
			//touchDirection = _line3 [ 0 ].M;
			var touchDirection = _line3[ _line3.Count - 2 ].M;

			double touchAngle = ARANFunctions.SpiralTouchAngle ( r0, e, nominalDirection, touchDirection, newSide );
			double dir = alfa0 + touchAngle * ( int ) newSide;
			double s = e * touchAngle + r0;
			Point touchPoint = ARANFunctions.LocalToPrj ( ptCntB, dir, s, 0 );

			Polygon result = new Polygon ( );
			LineString tmpPart = new LineString {_toleranceArea[3]};
			Shablons.CreateSpiralBy2Radial ( ptCntB, r0, alfa0, dir, e, ( int ) newSide, tmpPart );
			
			double tmpOutDouble;
			Point pntC10 = ( Point ) ( ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, touchPoint, _line3[ _line3.Count - 2 ].M + ARANMath.C_PI, out tmpOutDouble ) );
			if ( pntC10 == null )
#warning Look at this point again in document
				return new Polygon ( );


			tmpPart.Add ( pntC10 );
			tmpPart.Add ( _toleranceArea [ 3 ] );
			result.ExteriorRing = ( Ring ) tmpPart;

			LineString turn180OnC10 = ( LineString ) TransForm.Flip ( _turn180, designatedPointPrj, direction );
			turn180OnC10 = ( LineString ) TransForm.RotateGeometry ( designatedPointPrj, turn180OnC10, ARANMath.C_PI );
			turn180OnC10 = ( LineString ) TransForm.Move ( turn180OnC10, designatedPointPrj, pntC10 );

			Ring minDistRing = new Ring ( );
			Point minDistPoint;
			GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, turn180OnC10, out minDistPoint );
			minDistRing.Add ( ( Point ) minDistPoint.Clone ( ) );

			double angC10 = ARANFunctions.ReturnAngleInRadians ( dmePntPrj, pntC10 );
			Point pntIntersectRp1Dl2 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, direction + ( int ) side * vordata.TrackingTolerance, out tmpOutDouble );

			double angIntersectRp1Dl2 = ARANFunctions.ReturnAngleInRadians ( dmePntPrj, pntIntersectRp1Dl2 );
			int countDeg = ( int ) Math.Floor ( ARANMath.RadToDeg ( ARANMath.SubtractAngles ( angC10, angIntersectRp1Dl2 ) ) );
			for ( int i = 1; i <= countDeg; i++ )
			{
				var tmpA = ARANFunctions.LocalToPrj ( dmePntPrj, angC10 + ( int ) side * ARANMath.DegToRad ( i ), _dl2, 0 );
				LineString tmpTurn180 = ( LineString ) TransForm.Move ( turn180OnC10, pntC10, tmpA );
				GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out minDistPoint );
				minDistRing.Add ( ( Point ) minDistPoint.Clone ( ) );				
			}
			LineString turn180OnPntIntersectDl2Rp1 = ( LineString ) TransForm.Move ( turn180OnC10, pntC10, pntIntersectRp1Dl2 );
			GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, turn180OnPntIntersectDl2Rp1, out minDistPoint );
			minDistRing.Add ( ( Point ) minDistPoint.Clone ( ) );
			minDistRing.Add ( _toleranceArea [ 3 ] );
			Polygon minDistPolygon = new Polygon {ExteriorRing = minDistRing};

			Polygon polygonC10Rp1Dl2 = new Polygon();
			Ring tmpRing = new Ring {touchPoint, _pntC6};
			tmpRing.AddMultiPoint ( turn180OnC10 );
			polygonC10Rp1Dl2.ExteriorRing = tmpRing;
			polygonC10Rp1Dl2 = ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( polygonC10Rp1Dl2 ) )[ 0 ];
			//return polygonC10_Rp1Dl2;
			Polygon unionPolygon = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( minDistPolygon, polygonC10Rp1Dl2 ) ) [ 0 ];

			Geometry geomLeft = null, geomRight = null;
			LineString lnStringCutter = new LineString
			{
				touchPoint,
				ARANFunctions.LocalToPrj(touchPoint, _line3[_line3.Count - 2].M, 1000000, 0)
			};

			//_controller.DrawLineString ( lnStringCutter, 1, 1 );

			if ( GlobalParams.GeomOperators.Crosses ( unionPolygon, lnStringCutter ) )
				GlobalParams.GeomOperators.Cut ( unionPolygon, lnStringCutter, out geomLeft, out geomRight );
			if ( side == SideDirection.sideLeft )
			{
				if ( geomRight != null )
					unionPolygon = ( ( MultiPolygon ) geomRight )[ 0 ];
			}
			else if ( side == SideDirection.sideRight )
			{
				if ( geomLeft != null )
					unionPolygon = ( ( MultiPolygon ) geomLeft )[ 0 ];
			}
			//return unionPolygon;
			if ( GlobalParams.IsTestVersion )
			{
				//GlobalParams.TestVersionHandles.Add ( _controller.DrawPolygon ( unionPolygon, 1, eFillStyle.sfsCross ) );
				LineString circleDl2 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, _dl2 );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( circleDl2, 1, 1 ) );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( _line3, 1, 1 ) );

				LineString rps = new LineString
				{
					ARANFunctions.PointAlongPlane(vorPntPrj, direction + (int) side * vordata.TrackingTolerance, 150000),
					vorPntPrj,
					ARANFunctions.PointAlongPlane(vorPntPrj, direction - (int) side * vordata.TrackingTolerance, 150000)
				};
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( rps, 1, 1 ) );

				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( pntC10, 1, "C10" ) );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawPointWithText ( pntIntersectRp1Dl2, 1, "Int" ) );
				GlobalParams.TestVersionHandles.Add ( _controller.DrawLineString ( turn180OnPntIntersectDl2Rp1, 1, 1 ) );
			}
			//return unionPolygon;
			polygonC10Rp1Dl2.Clear ( );
			polygonC10Rp1Dl2.ExteriorRing = ( Ring ) ( LineString ) TransForm.Move ( turn180OnC10, pntC10, pntIntersectRp1Dl2 );
			//return polygonC10_Rp1Dl2;			
			unionPolygon = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( unionPolygon, polygonC10Rp1Dl2 ) )[ 0 ];
			//return unionPolygon;
			result = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( result, unionPolygon ) ) [ 0 ];
			return result; //( ( MultiPolygon ) GlobalParams.GeomOperators.Difference ( protectedSector1, _basicArea ) ) [ 0 ];
		}

		public Polygon AwayConstructProtectSector2 ( double tas, double limitingDistanceInPrj, double direction, SideDirection side, Point dmePntPrj, Point designatedPointPrj, double time, double altitude )
		{
			Polygon result = new Polygon ( );
			// 1.5 = 90 san
			if ( time > 1.5 )
				return result;

			double tmpOutDouble;

			Point pntC7 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, _toleranceArea [ 0 ], direction - ( int ) side * ARANMath.DegToRad ( 30 + 5 ), out tmpOutDouble );

			Point pntC8 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, _toleranceArea [ 2 ], direction - ( int ) side * ARANMath.DegToRad ( 30 - 5 ), out tmpOutDouble );

			#region Convexing turn180 on C7 with Basic Area
			// To convex turn180 on C7 with BasicArea we have to cut turn180 on C3 with straight part (Curve 1) of shablon on A1 and get inside part of cutting operation
			// After cutting we can to convex turn180 on C7 with BasicArea 
			LineString turn180OnC3 = ( LineString ) TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI );
			turn180OnC3 = ( LineString ) TransForm.Move ( turn180OnC3, designatedPointPrj, _pntC3 );
			result.ExteriorRing = ( Ring ) turn180OnC3;

			Line straightPart = GetStraightPartOfPolygon ( side, direction, dmePntPrj, _shablon );
			straightPart.RefPoint = ( Point ) TransForm.Move ( straightPart.RefPoint, designatedPointPrj, _toleranceArea[ 0 ] );

			LineString lnStringCutter = new LineString
			{
				ARANFunctions.LocalToPrj(straightPart.RefPoint, straightPart.DirVector.Direction - ARANMath.C_PI, 100000, 0),
				ARANFunctions.LocalToPrj(straightPart.RefPoint, straightPart.DirVector.Direction, 100000, 0)
			};

			Geometry geomLeft = null, geomRight = null;
			if ( GlobalParams.GeomOperators.Crosses ( result, lnStringCutter ) )
				GlobalParams.GeomOperators.Cut ( result, lnStringCutter, out geomLeft, out geomRight );
			if ( side == SideDirection.sideRight )
			{
				if ( geomRight != null )
					result = ( ( MultiPolygon ) geomRight )[ 0 ];
			}
			else if ( side == SideDirection.sideLeft )
			{
				if ( geomLeft != null )
					result = ( ( MultiPolygon ) geomLeft )[ 0 ];
			}

			LineString turn180OnC7 = ( LineString ) TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI - ( int ) side * ARANMath.DegToRad ( 30 ) );
			turn180OnC7 = ( LineString ) TransForm.Move ( turn180OnC7, designatedPointPrj, pntC7 );

			// Convext turn180 on C3 and C7 points
			MultiPolygon tmpMultiPolygon = new MultiPolygon {result};
			Polygon tmpPolygon = new Polygon {ExteriorRing = (Ring) turn180OnC7};
			tmpMultiPolygon.Add ( tmpPolygon );

			result = ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( tmpMultiPolygon ) )[ 0 ];
			#endregion

			double angC7 = ARANFunctions.ReturnAngleInRadians ( dmePntPrj, pntC7 );
			double angC8 = ARANFunctions.ReturnAngleInRadians ( dmePntPrj, pntC8 );
			double rad = ARANMath.SubtractAngles ( angC7, angC8 );

			int countDeg = ( int ) Math.Floor ( ARANMath.RadToDeg ( rad ) );

			Ring minDistRing = new Ring {pntC7};
			Point minDistPoint;
			GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, turn180OnC7, out minDistPoint );
			minDistRing.Add ( ( Point ) minDistPoint.Clone ( ) );

			LineString tmpTurn180 = ( LineString ) turn180OnC7.Clone ( );
			Point tmpPnt = new Point ( pntC7 );
			for ( int i = 1; i <= countDeg; i++ )
			{
				var tmpA = ARANFunctions.LocalToPrj ( dmePntPrj, angC7 - ( int ) side * ARANMath.DegToRad ( i ), _dl2, 0 );
				tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, tmpPnt, tmpA );
				GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out minDistPoint );
				minDistRing.Add ( ( Point ) minDistPoint.Clone ( ) );
				tmpPnt.Assign ( tmpA );
			}

			tmpTurn180 = ( LineString ) TransForm.Move ( tmpTurn180, tmpPnt, pntC8 );
			GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out minDistPoint );
			minDistRing.Add ( ( Point ) minDistPoint.Clone ( ) );
			minDistRing.Add ( pntC8 );

			Polygon minDistPolygon = new Polygon {ExteriorRing = minDistRing};

			result = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( result, minDistPolygon ) ) [ 0 ];

			// Convext turn180 on C8 and C6 points
			MultiPolygon mltPolygon = new MultiPolygon ( );
			Polygon polygonC8 = new Polygon {ExteriorRing = (Ring) tmpTurn180};

			mltPolygon.Add ( polygonC8 );

			Polygon polygonC6 = new Polygon {ExteriorRing = (Ring) (LineString) TransForm.Move(turn180OnC3, _pntC3, _pntC6)};
			mltPolygon.Add ( polygonC6 );
			Polygon convexedPolygonC8C6 = ( ( MultiPolygon ) GlobalParams.GeomOperators.ConvexHull ( mltPolygon ) ) [ 0 ];

			result = ( ( MultiPolygon ) GlobalParams.GeomOperators.UnionGeometry ( result, convexedPolygonC8C6 ) ) [ 0 ];

			return result;//( ( MultiPolygon ) GlobalParams.GeomOperators.Difference ( result, _basicArea ) ) [ 0 ];	
		}
		
		public void AwayConstructRecipDirectEntryToSecondaryPnt ( double direction, double radius, Point designatedPntPrj, 
            Point vorPntPrj, double legLength, SideDirection side, EntryDirection entryDirection, out int angle )
		{
            angle = CalculateReRpAngle(direction, radius, designatedPntPrj, vorPntPrj, legLength, side, entryDirection);
		}

        public int CalculateReRpAngle( double direction, double radius, Point designatedPntPrj, Point vorPntPrj, 
            double legLength, SideDirection side, EntryDirection entryDirection)
        {
            var emsal = 1;
            if (entryDirection == EntryDirection.Away)
                emsal = -1;
            Point secPnt = ARANFunctions.LocalToPrj(designatedPntPrj, direction, emsal * legLength, (int)side * 2 * radius);
            return (int)ARANMath.RadToDeg(ARANMath.SubtractAngles(direction, ARANFunctions.ReturnAngleInRadians(vorPntPrj, secPnt)));
        }

        internal void FindAwayMinMaxDistInBasicArea ( SideDirection side, Point dmePntPrj, double time, double tas, double legLength, double altitude, bool drawShablon, bool drawNominalTraject, bool drawToleranceArea, double notCoverageMinDist, out double minDist)
		{
			GlobalParams.ClearTestVersionElements ( );
			LineString testNominalTrack;
			Polygon testShablon;
			Point testPntE;
			LineString testTurn180;
			LineString testLine3;
			Point pntR;

			Shablons.HoldingShablon ( _controller, dmePntPrj, tas, altitude, time, ARANMath.C_PI_2, side, 0, out testNominalTrack, out testShablon, out testLine3, out testTurn180, out testPntE, out pntR );
			//_controller.DrawLineString ( testTurn180, 1, 1 );

			Box box = TransForm.QueryCoords ( testTurn180 );
			Point minPnt = new Point ( box.XMin, box.YMin );
			Point maxPnt = new Point ( box.XMax, box.YMax );
			//_controller.DrawPointWithText ( minPnt, 0, "Min" );
			//_controller.DrawPointWithText ( maxPnt, 255, "Max" );
			double extendHeight = box.YMax - dmePntPrj.Y;

			//minDist = ( GlobalParams.Navaid_Database.DME.MinimalError + extendHeight + notCoverageMinDist ) / ( 1 - GlobalParams.Navaid_Database.DME.ErrorScalingUp ) + legLength;
			minDist = ( GlobalParams.NavaidDatabase.Dme.MinimalError + extendHeight ) / ( 1 - GlobalParams.NavaidDatabase.Dme.ErrorScalingUp ) + legLength;
			return;

			//Polygon result = AwayConstructBasicArea ( side, dmePntPrj, vorPntPrj, designatedPointPrj, direction, nominalDistanceInPrj, nominalDistance, limitingDistanceInPrj, limitingDistance,
			//                                        time, tas, legLength, altitude, radius, false, false, false, true );

			//if ( GlobalParams.IsTestVersion )
			//    _controller.DrawPolygon ( result, 1, eFillStyle.sfsHollow );
			//if ( time <= 1.5 )
			//    result = AwayConstructProtectSector2 ( tas, limitingDistanceInPrj, direction, side, dmePntPrj, designatedPointPrj, time, altitude );
			////_controller.DrawPolygon ( result, 1, eFillStyle.sfsHollow );

			//LineString lnStringResult = new LineString ( );
			//lnStringResult.AddMultiPoint ( result.ToMultiPoint ( ) );
			

			//LineString rp = new LineString();
			//rp.Add(dmePntPrj);
			//rp.Add ( ARANFunctions.LocalToPrj ( dmePntPrj, direction, 5 * nominalDistance, 0 ) );

			////_controller.DrawLineString ( rp, 1, 1 );
			////_controller.DrawLineString ( lnStringResult, 255, 1 );


			//MultiPoint mltPnt = (MultiPoint) GlobalParams.GeomOperators.Intersect ( rp, lnStringResult );
			////_controller.DrawPointWithText ( mltPnt[ 0 ], 1, "1" );
			////_controller.DrawPointWithText ( mltPnt[ 1 ], 1, "2" );
			//double d1 = (mltPnt[0].X  - dmePntPrj.X) * (mltPnt[0].X  - dmePntPrj.X)  + (mltPnt[0].Y  - dmePntPrj.Y) * (mltPnt[0].Y  - dmePntPrj.Y);
			//double d2 = (mltPnt[1].X  - dmePntPrj.X) * (mltPnt[1].X  - dmePntPrj.X)  + (mltPnt[1].Y  - dmePntPrj.Y) * (mltPnt[1].Y  - dmePntPrj.Y);


			//if ( d1 > d2 )
			//    minPnt = mltPnt[ 1 ];
			//else
			//    minPnt = mltPnt[ 0 ];

			//minDist = ARANFunctions.ReturnDistanceInMeters ( designatedPointPrj, minPnt );

			//double dist;
			//maxPnt = null;
			//maxDist = double.MinValue;
			//LineString turn180OnA1 = ( LineString ) TransForm.Move ( _turn180, designatedPointPrj, _toleranceArea[ 0 ] );
			////_controller.DrawLineString ( turn180OnA1, 1, 1 );
			//foreach ( Point pnt in turn180OnA1 )
			//{
			//    dist = ( pnt.X - dmePntPrj.X ) * ( pnt.X - dmePntPrj.X ) + ( pnt.Y - dmePntPrj.Y ) * ( pnt.Y - dmePntPrj.Y );
			//    if ( dist > maxDist )
			//    {
			//        maxDist = dist;
			//        maxPnt = pnt;
			//    }
			//}

			////_controller.DrawPointWithText ( maxPnt, 1, "Max" );
			//maxDist = ARANFunctions.ReturnDistanceInMeters ( maxPnt, designatedPointPrj );
		}

		private void AwayConstructFixToleranceArea ( Point dmePntPrj, Point vorPntPrj, double nominalDistanceInPrj, double direction, SideDirection side, double trackingTolerance, double d1, Point dsgPntPrj, bool draw )
		{
			double tmpOutDouble;
			Point a1 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj + d1, vorPntPrj, direction - trackingTolerance, out tmpOutDouble );
			Point a2 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj - d1, vorPntPrj, direction - trackingTolerance, out tmpOutDouble );
			Point a3 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj + d1, vorPntPrj, direction + trackingTolerance, out tmpOutDouble );
			Point a4 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj - d1, vorPntPrj, direction + trackingTolerance, out tmpOutDouble );

			MultiPoint tmpMultipnt = new MultiPoint ( );
			ARANFunctions.AddArcToMultiPoint ( dmePntPrj, a3, a1, TurnDirection.CW, ref tmpMultipnt );
			ARANFunctions.AddArcToMultiPoint ( dmePntPrj, a2, a4, TurnDirection.CCW, ref tmpMultipnt );
			MultiPoint minAndmaxPoints = GeomFunctions.MinAndMaxDistFromLineToMultipoint ( vorPntPrj, direction - ARANMath.C_PI / 2, tmpMultipnt );
			_priorFixTolerance = ARANFunctions.ReturnDistanceInMeters ( minAndmaxPoints [ 0 ], dsgPntPrj );
			_postFixTolerance = ARANFunctions.ReturnDistanceInMeters ( minAndmaxPoints [ 1 ], dsgPntPrj );
			double directionPerpendicular = direction - ARANMath.C_PI_2;

			_toleranceArea.Clear ( );
			_toleranceArea.Add ( ( Point ) ARANFunctions.LineLineIntersect ( minAndmaxPoints [ 1 ], directionPerpendicular, vorPntPrj, direction + ( int ) side * trackingTolerance ) );
			_toleranceArea.Add ( ( Point ) ARANFunctions.LineLineIntersect ( minAndmaxPoints [ 0 ], directionPerpendicular, vorPntPrj, direction + ( int ) side * trackingTolerance ) );
			_toleranceArea.Add ( ( Point ) ARANFunctions.LineLineIntersect ( minAndmaxPoints [ 0 ], directionPerpendicular, vorPntPrj, direction - ( int ) side * trackingTolerance ) );
			_toleranceArea.Add ( ( Point ) ARANFunctions.LineLineIntersect ( minAndmaxPoints [ 1 ], directionPerpendicular, vorPntPrj, direction - ( int ) side * trackingTolerance ) );

			if ( draw )
				DrawToleranceArea ( EntryDirection.Away, a1, a2, a3, a4, dmePntPrj, dsgPntPrj );
		}

		private void DrawToleranceArea ( EntryDirection entryDirection, Point a1, Point a2, Point a3, Point a4, Point dmePntPrj, Point dsgPntPrj )
		{
			//_controller.DrawPointWithText ( A1, 1, "A1" );
			//_controller.DrawPointWithText ( A2, 1, "A2" );
			//_controller.DrawPointWithText ( A3, 1, "A3" );
			//_controller.DrawPointWithText ( A4, 1, "A4" );

			_controller.SafeDeleteGraphic ( _toleranceEntryRadialHandle );
			LineString rpRadials = new LineString ( );
			if ( entryDirection == EntryDirection.Away )
			{
				rpRadials.Add ( a2 );
				rpRadials.Add ( dmePntPrj );
				rpRadials.Add ( a4 );
			}
			else
			{
				rpRadials.Add ( a1 );
				rpRadials.Add ( dmePntPrj );
				rpRadials.Add ( a3 );
			}
			_toleranceEntryRadialHandle = _controller.DrawLineString ( rpRadials, 0, 1 );

			_controller.SafeDeleteGraphic ( _entryRadialHandle );
			LineString radial = new LineString {dmePntPrj, dsgPntPrj};
			_entryRadialHandle = _controller.DrawLineString ( radial, 255, 1 );

			_controller.SafeDeleteGraphic ( _toleranceAreaHandle );
			LineString toleranceArea = new LineString ( );
			if ( entryDirection == EntryDirection.Away )
			{
				toleranceArea.Add ( a2 );
				toleranceArea.AddMultiPoint ( ARANFunctions.CreateArcAsPartPrj ( dmePntPrj, a1, a3, TurnDirection.CCW ) );
				toleranceArea.Add ( a4 );
				toleranceArea.AddMultiPoint ( ARANFunctions.CreateArcAsPartPrj ( dmePntPrj, a4, a2, TurnDirection.CW ) );
			}
			else
			{
				toleranceArea.Add ( a2 );
				toleranceArea.AddMultiPoint ( ARANFunctions.CreateArcAsPartPrj ( dmePntPrj, a1, a3, TurnDirection.CW ) );
				toleranceArea.Add ( a4 );
				toleranceArea.AddMultiPoint ( ARANFunctions.CreateArcAsPartPrj ( dmePntPrj, a4, a2, TurnDirection.CCW ) );
			}
			_toleranceAreaHandle = _controller.DrawLineString ( toleranceArea, 255 * 255, 1 );
		}

		private Line GetStraightPartOfPolygon ( SideDirection side, double direction, Point vorPntPrj, Polygon polygon )
		{
			Line result = new Line ( );
			MultiLineString tmpPolyline = new MultiLineString ( );
			LineString tmpPart = new LineString ( );
			Geometry [] geom = new Geometry [ 2 ] { null, null };

			if ( polygon.IsPointInside ( vorPntPrj ) )
			{
				tmpPart.Add ( ARANFunctions.LocalToPrj ( vorPntPrj, direction - ARANMath.C_PI, 100000000, 0 ) );
				tmpPart.Add ( ARANFunctions.LocalToPrj ( vorPntPrj, direction, 100000000, 0 ) );
				tmpPolyline.Add ( tmpPart );
			}
			else
			{
				tmpPart.Add ( vorPntPrj );
				tmpPart.Add ( ARANFunctions.LocalToPrj ( vorPntPrj, direction, 100000000, 0 ) );
				tmpPolyline.Add ( tmpPart );
			}
			//_controller.DrawPolygon ( polygon, 1, eFillStyle.sfsHollow );
			//_controller.DrawLineString ( tmpPart, 1, 1 );
			//return null;

			//if ( GlobalParams.GeomOperators.Crosses ( polygon, tmpPolyline ) )
			GlobalParams.GeomOperators.Cut ( polygon, tmpPolyline, out  geom[ 0 ], out geom[ 1 ] );

			int firstIndex = 0;
			if ( side == SideDirection.sideRight )
			{
				var tmpPolygon = ( ( MultiPolygon ) geom [ 1 ] ) [ 0 ];
				tmpPolyline = ARANFunctions.PolygonToPolyLine ( tmpPolygon );
			}
			else
			{
				Polygon tmpPolygon = ( ( MultiPolygon ) geom [ 0 ] ) [ 0 ];
				tmpPolyline = ARANFunctions.PolygonToPolyLine ( tmpPolygon );
			}
			//InitHolding.ui.DrawMultiLineString ( tmpPolyline, 255, 2 );
			double max = 0, secMax = 0;
			int maxFirstIndex = 0;
			for ( int i = 0; i <= tmpPolyline [ 0 ].Count - 2; i++ )
			{
				var dist = ARANFunctions.ReturnDistanceInMeters ( ( ( LineString ) tmpPolyline [ 0 ] ) [ i ], ( ( LineString ) tmpPolyline [ 0 ] ) [ i + 1 ] );
				//InitHolding.ui.DrawPointWithText ( tmpPolyline [0].AsPart [i], 0, i.ToString () );
				if ( dist > max )
				{
					secMax = max;
					firstIndex = maxFirstIndex;
					max = dist;
					maxFirstIndex = i;
				}
				else if ( dist > secMax )
				{
					secMax = dist;
					firstIndex = i;
				}
			}
			if ( side == SideDirection.sideRight )
			{
				result.RefPoint = ( ( LineString ) tmpPolyline [ 0 ] ) [ firstIndex ];
				result.DirVector.Direction = ARANFunctions.ReturnAngleInRadians ( result.RefPoint, ( ( LineString ) tmpPolyline [ 0 ] ) [ firstIndex + 1 ] );
			}
			else
			{
				result.RefPoint = ( ( LineString ) tmpPolyline [ 0 ] ) [ firstIndex + 1 ];
				result.DirVector.Direction = ARANFunctions.ReturnAngleInRadians ( result.RefPoint, ( ( LineString ) tmpPolyline [ 0 ] ) [ firstIndex ] );
			}
			result.DirVector.Length = 5 * secMax;
			return result;
		}

		#endregion

		internal void ConstructToleranceArea ( EntryDirection entryDirection, Point dmePntPrj, Point vorPntPrj, double nominalDistanceInPrj, double nominalDistance, double direction, SideDirection side, Point dsgPntPrj, bool draw )
		{
			NavaidsDataBase navaidsDb = GlobalParams.NavaidDatabase;
			double d1 = navaidsDb.Dme.MinimalError + navaidsDb.Dme.ErrorScalingUp * nominalDistanceInPrj;

			if ( entryDirection == EntryDirection.Toward )
				TowardConstructFixToleranceArea ( dmePntPrj, vorPntPrj, nominalDistanceInPrj, direction, side, navaidsDb.Vor.TrackingTolerance, d1, dsgPntPrj, draw );
			else
				AwayConstructFixToleranceArea ( dmePntPrj, vorPntPrj, nominalDistanceInPrj, direction, side, navaidsDb.Vor.TrackingTolerance, d1, dsgPntPrj, draw );
		}

		internal void SetToleranceAreaVisibility ( bool showToleranceArea )
		{
			GlobalParams.AranEnvironment.Graphics.SetVisible ( _toleranceEntryRadialHandle, showToleranceArea );
			GlobalParams.AranEnvironment.Graphics.SetVisible ( _entryRadialHandle, showToleranceArea );
			GlobalParams.AranEnvironment.Graphics.SetVisible ( _toleranceAreaHandle, showToleranceArea );
		}

		internal void SetNominalTrackVisibility ( bool showNominalTrack )
		{
			GlobalParams.AranEnvironment.Graphics.SetVisible ( _nominalTrackHandle, showNominalTrack );
		}

		internal void SetTemplateVisibility ( bool showTemplate )
		{
			GlobalParams.AranEnvironment.Graphics.SetVisible ( _templateHandle, showTemplate );
		}

		public void SetWithLimitingRadial ( bool isWithLimitingRadial )
		{
			_isWithLimitingRadial = isWithLimitingRadial;
		}

		public Ring ToleranceArea => _toleranceArea;

		public double PriorFixTolerance => _priorFixTolerance;

		public double PostFixTolerance => _postFixTolerance;

		internal LineString NominalTrack => _nominalTrack;

        double _priorFixTolerance, _postFixTolerance;
		private readonly Ring _toleranceArea;
		private Polygon _shablon;
		private LineString _line3, _turn180;
		private Polygon _basicArea;
		private double _dl2;
		private Point _pntC3, _pntC6;
		private bool _isWithLimitingRadial;
		private LineString _nominalTrack;
		private int _toleranceAreaHandle;
		private int _toleranceEntryRadialHandle;
		private int _entryRadialHandle;
		private readonly MainController _controller;
		private int _templateHandle;
		private int _nominalTrackHandle;
	}
}
