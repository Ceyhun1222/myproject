using System;
using Aran.Geometries;
using Aran.PANDA.Common;

namespace Aran.PANDA.Conventional.Racetrack
{
	public static class Shablons
	{
		private const double DegToRadValue = ARANMath.C_PI / 180;
		private const double DegEps = 1.0 / 36000.0;
		private const double RadEps = DegEps * DegToRadValue;
		private const double DistEps = 0.0001;
		private static readonly double GuidanceTolerance = ARANMath.DegToRad ( 5 );
		private static readonly double SinGuidanceTolerance = Math.Sin ( GuidanceTolerance );
		private static readonly double CosGuidanceTolerance = Math.Cos ( GuidanceTolerance );

		public static double TouchTurn180InRad
		{
			get;
			set;
		}

		public static int HoldingShablon ( MainController controller, Point wayPoint, double tas, double altitude, double time, double axis, SideDirection side, int navType, out LineString inboundTrack, out Polygon shablon, out LineString line3, out LineString turn180, out Point ptE, out Point pntR)
		{
			TurnDirection turnDir = ( TurnDirection ) side;
			double v = tas;
			double v3600 = v;
			double r = 943.27 * 0.277777777777778 / v;
			if ( ( r > 3.0 ) )
				r = 3.0;
			GlobalParams.SpiralParameterR = r;			
			double rv = 3600 * v / ( 62.83 * r );

			double h = altitude / 1000.0;
			double w = 12.0 * h + 87.0;

			double w_ = 0.277777777777778 * w;

			double e = w_ / r;
			double e45 = 45.0 * e;
			GlobalParams.SpiralParameterE45 = e45;
			double t = 60.0 * time;

			double l = v3600 * t;
			double ab = 5.0 * v3600;
			double ac = 11.0 * v3600;
			double gi1 = ( t - 5.0 ) * v3600;
			double gi2 = ( t + 21.0 ) * v3600;

			double wb = 5 * w_;
			double wc = 11 * w_;
			double wd = wc + e45;
			double we = wc + 2 * e45;
			double wf = wc + 3 * e45;
			double wg = wc + 4 * e45;
			double wh = wb + 4 * e45;
			double wo = wb + 5 * e45;
			double wp = wb + 6 * e45;
			double wi1 = ( t + 6 ) * w_ + 4 * e45;
			double wi3 = wi1;
			double wi2 = wi1 + 14 * w_;
			double wi4 = wi2;
			double wj = wi2 + e45;
			double wk = wi2 + 2 * e45;
			double wl = wk;
			double wm = wi2 + 3 * e45;
			double wn3 = wi1 + 4 * e45;
			double wn4 = wi2 + 4 * e45;

			double xe = 2.0 * rv + ( t + 15.0 ) * v3600 + ( 26.0 + 195.0 / r + t ) * w_;
			double ye = 11.0 * v3600 * Math.Cos ( ARANMath.DegToRad ( 20.0 ) ) + rv * Math.Sin ( ARANMath.DegToRad ( 20.0 ) ) + rv + ( t + 15.0 ) * v3600 * Math.Tan ( ARANMath.DegToRad ( 5.0 ) ) + ( 26.0 + 125.0 / r + t ) * w_;

#warning Newly added
			axis = ARANMath.Modulus ( axis - ARANMath.C_PI, ARANMath.C_2xPI );

            Point pntCntB = ARANFunctions.LocalToPrj ( wayPoint, axis, -ab, -rv * ( int ) turnDir );
			Point pntCntC = ARANFunctions.LocalToPrj ( wayPoint, axis, -ac, -rv * ( int ) turnDir );
			Point pntG = ARANFunctions.LocalToPrj ( wayPoint, axis, -ac, -rv * 2 * ( int ) turnDir );
			Point pntI1 = ARANFunctions.LocalToPrj ( pntG, axis, gi1 * CosGuidanceTolerance, -gi1 * SinGuidanceTolerance * ( int ) turnDir );
			Point pntI2 = ARANFunctions.LocalToPrj ( pntG, axis, gi2 * CosGuidanceTolerance, -gi2 * SinGuidanceTolerance * ( int ) turnDir );
			Point pntI3 = ARANFunctions.LocalToPrj ( pntG, axis, gi1 * CosGuidanceTolerance, gi1 * SinGuidanceTolerance * ( int ) turnDir );
			Point pntI4 = ARANFunctions.LocalToPrj ( pntG, axis, gi2 * CosGuidanceTolerance, gi2 * SinGuidanceTolerance * ( int ) turnDir );
			Point pntCntI1 = ARANFunctions.LocalToPrj ( pntI1, axis, 0, ( int ) turnDir * rv );
			Point pntCntI2 = ARANFunctions.LocalToPrj ( pntI2, axis, 0, ( int ) turnDir * rv );
			Point pntCntI3 = ARANFunctions.LocalToPrj ( pntI3, axis, 0, ( int ) turnDir * rv );
			Point pntCntI4 = ARANFunctions.LocalToPrj ( pntI4, axis, 0, ( int ) turnDir * rv );

            //controller.DrawPointWithText(pntCntB, 255, "CntB");
            //controller.DrawPointWithText(pntCntC, 255, "CntC");
            //controller.DrawPointWithText(pntG, 255, "G");
            //controller.DrawPointWithText(pntI1, 255, "I1");
            //controller.DrawPointWithText(pntI2, 255, "I2");
            //controller.DrawPointWithText(pntI3, 255, "I3");
            //controller.DrawPointWithText(pntI4, 255, "I4");
            //controller.DrawPointWithText(pntCntI1, 255, "CntI1");
            //controller.DrawPointWithText(pntCntI2, 255, "CntI2");
            //controller.DrawPointWithText(pntCntI3, 255, "CntI3");
            //controller.DrawPointWithText(pntCntI4, 255, "CntI4");

			double quarterPi = ARANMath.C_PI * 0.25;
			Point pntJ = ARANFunctions.LocalToPrj ( pntCntI2, axis, rv * Math.Cos ( quarterPi ), -( int ) turnDir * rv * Math.Sin ( quarterPi ) );				

			Point pntK = ARANFunctions.LocalToPrj ( pntCntI2, axis, rv, 0 );
			Point pntL = ARANFunctions.LocalToPrj ( pntCntI4, axis, rv, 0 );
			Point pntM = ARANFunctions.LocalToPrj ( pntCntI4, axis, rv * Math.Cos ( quarterPi ), ( int ) turnDir * rv * Math.Sin ( quarterPi ) );
			Point pntN4 = ARANFunctions.LocalToPrj ( pntCntI4, axis, 0, ( int ) turnDir * rv );
			Point pntN3 = ARANFunctions.LocalToPrj ( pntCntI3, axis, 0, ( int ) turnDir * rv );

		    inboundTrack = CalculateHoldingArea(wayPoint, axis, (int) turnDir, rv, l, controller);
			//Turn180 
			turn180 = new LineString {wayPoint};
			//controller.DrawPointWithText ( pntCntC, 1, "C" );
			CreateSpiralBy2Radial ( pntCntC, rv + 11 * w_, axis + ( int ) turnDir * ARANMath.C_PI_2, axis, e / ARANMath.DegToRad ( 1 ), ( int ) turnDir, turn180 );
			//CreateSpiralBy2Radial ( pntCntC, Rv + 11 * w_, axis + ( int ) turnDir * ARANMath.C_PI_2, axis +  ARANMath.C_PI_2, E / ARANMath.DegToRad ( 1 ), ( int ) turnDir, turn180, controller );
			//End Turn180

			#region Locating Point R

			LineString tmpLnString = new LineString
			{
				ARANFunctions.LocalToPrj(wayPoint, axis - (int) side * ARANMath.C_PI_2, rv, 0),
				ARANFunctions.LocalToPrj(wayPoint, axis - (int) side * ARANMath.C_PI_2, 10000000, 0)
			};
			Geometry geom = GlobalParams.GeomOperators.Intersect ( tmpLnString, turn180 );
			pntR = null;
			if ( geom is MultiPoint )
				pntR = ( ( MultiPoint ) geom )[ 0 ];
			#endregion

			Point tmpFromPoint = ARANFunctions.LocalToPrj ( pntI1, axis, -wi1, 0 );
			Point tmpToPoint = ARANFunctions.LocalToPrj ( pntI1, axis, wi1, 0 );

			LineString spiralA = null;

			//ArcI1 = ARANFunctions.CreateArcPrj ( PtI1, tmpFromPoint, tmpToPoint, ARANMath.ChangeDirection ( turnDir ) );			
			Ring arcI1 = ARANFunctions.CreateArcPrj ( pntI1, tmpFromPoint, tmpToPoint, turnDir );

			tmpFromPoint = ARANFunctions.LocalToPrj ( pntI2, axis, -wi2, 0 );
			tmpToPoint = ARANFunctions.LocalToPrj ( pntI2, axis, wi2, 0 );
			Ring arcI2 = ARANFunctions.CreateArcPrj ( pntI2, tmpFromPoint, tmpToPoint, turnDir );

			tmpFromPoint = ARANFunctions.LocalToPrj ( pntJ, axis, -wj * Math.Cos ( quarterPi ), -( int ) turnDir * wj * Math.Sin ( quarterPi ) );
			//ARANFunctions.LocalToPrj(PtJ, Axis, , Wj);
			tmpToPoint = ARANFunctions.LocalToPrj ( pntJ, axis, wj * Math.Cos ( quarterPi ), ( int ) turnDir * wj * Math.Sin ( quarterPi ) );
			//ARANFunctions.LocalToPrj(PtJ, Axis, , Wj);
			Ring arcJ = ARANFunctions.CreateArcPrj ( pntJ, tmpFromPoint, tmpToPoint, turnDir );

			tmpFromPoint = ARANFunctions.LocalToPrj ( pntK, axis, 0, -( int ) turnDir * wk );
			tmpToPoint = ARANFunctions.LocalToPrj ( pntK, axis, 0, ( int ) turnDir * wk );
			Ring arck = ARANFunctions.CreateArcPrj ( pntK, tmpFromPoint, tmpToPoint, turnDir );

			tmpFromPoint = ARANFunctions.LocalToPrj ( pntL, axis, 0, -( int ) turnDir * wl );
			tmpToPoint = ARANFunctions.LocalToPrj ( pntL, axis, 0, ( int ) turnDir * wl );
			Ring arcL = ARANFunctions.CreateArcPrj ( pntL, tmpFromPoint, tmpToPoint, turnDir );

			tmpFromPoint = ARANFunctions.LocalToPrj ( pntM, axis, wm * Math.Cos ( quarterPi ), -( int ) turnDir * wm * Math.Sin ( quarterPi ) );
			tmpToPoint = ARANFunctions.LocalToPrj ( pntM, axis, -wm * Math.Cos ( quarterPi ), ( int ) turnDir * wm * Math.Sin ( quarterPi ) );
			Ring arcM = ARANFunctions.CreateArcPrj ( pntM, tmpFromPoint, tmpToPoint, turnDir );

			tmpFromPoint = ARANFunctions.LocalToPrj ( pntN4, axis, wn4, 0 );
			tmpToPoint = ARANFunctions.LocalToPrj ( pntN4, axis, -wn4, 0 );
			Ring arcN4 = ARANFunctions.CreateArcPrj ( pntN4, tmpFromPoint, tmpToPoint, turnDir );

			tmpFromPoint = ARANFunctions.LocalToPrj ( pntN3, axis, wn3, 0 );
			tmpToPoint = ARANFunctions.LocalToPrj ( pntN3, axis, -wn3, 0 );
			Ring arcN3 = ARANFunctions.CreateArcPrj ( pntN3, tmpFromPoint, tmpToPoint, turnDir );

			tmpFromPoint = ARANFunctions.LocalToPrj ( pntN3, axis, 0, ( int ) turnDir * wn3 );
			tmpToPoint = ARANFunctions.LocalToPrj ( pntN3, axis, 0, -( int ) turnDir * wn3 );
			//LineString tmpTurn90 = new LineString ( );
			//CreateSpiralBy2Radial ( pntCntB, Rv + 5 * w_, axis + ( int ) turnDir * ARANMath.C_PI_2, axis - ( int ) turnDir * ARANMath.C_PI_2, E / ARANMath.DegToRad ( 1 ), ( int ) turnDir, tmpTurn90 );

			var arc3N = ARANFunctions.CreateArcAsPartPrj ( pntN3, tmpFromPoint, tmpToPoint, ( TurnDirection ) turnDir );

			//intersectArcN3Turn180 = null;
			geom = GlobalParams.GeomOperators.Intersect ( arc3N, ARANFunctions.CreatePolyLineFromParts ( turn180 ) );

			if ( geom.Type == GeometryType.MultiLineString || ( ( MultiPoint ) geom ).Count == 0 )
			{
#warning Test this variant
				double tmpOutDouble;
				tmpFromPoint = ARANFunctions.CircleVectorIntersect ( pntN3, wn3, wayPoint, axis + ARANMath.C_PI, out tmpOutDouble );
				double distance = ARANFunctions.ReturnDistanceInMeters ( tmpFromPoint, wayPoint );
				tmpToPoint = ARANFunctions.LocalToPrj ( wayPoint, axis, distance, 0 );
				spiralA = ARANFunctions.CreateArcAsPartPrj ( wayPoint, tmpFromPoint, tmpToPoint, ( TurnDirection ) turnDir );
			}

			//controller.DrawRing ( ArcI1, 0, AranEnvironment.Symbols.eFillStyle.sfsHollow );
			//controller.DrawRing ( ArcI2, 0, AranEnvironment.Symbols.eFillStyle.sfsHollow );
			//controller.DrawRing ( ArcJ, 0, AranEnvironment.Symbols.eFillStyle.sfsHollow );
			//controller.DrawRing ( Arck, 0, AranEnvironment.Symbols.eFillStyle.sfsHollow );
			//controller.DrawRing ( ArcL, 0, AranEnvironment.Symbols.eFillStyle.sfsHollow );
			//controller.DrawRing ( ArcM, 0, AranEnvironment.Symbols.eFillStyle.sfsHollow );
			//controller.DrawRing ( ArcN3, 0, AranEnvironment.Symbols.eFillStyle.sfsHollow );
			//controller.DrawRing ( ArcN4, 0, AranEnvironment.Symbols.eFillStyle.sfsHollow );
			//if ( spiralA != null )
			//    controller.DrawLineString ( spiralA, 1, 0 );

			shablon = ChainHullAlgorithm.ConvexHull ( turn180, arcI1, arcI2, arcJ, arck, arcL, arcM, arcN3, arcN4, spiralA );
			Polygon turnShablon = ( Polygon ) TransForm.RotateGeometry ( wayPoint, shablon, -axis );
			Box minMax = TransForm.QueryCoords ( turnShablon );
			Point ptMin = new Point ( minMax.XMin, minMax.YMin );
			Point ptMax = new Point ( minMax.XMax, minMax.YMax );
			ptE = new Point ( );

			if ( turnDir == TurnDirection.CCW )
				ptE.SetCoords ( minMax.XMax - xe, minMax.YMax - ye );
			else
				ptE.SetCoords ( minMax.XMax - xe, minMax.YMin + ye );

			ptE = ( Point ) TransForm.RotateGeometry ( wayPoint, ptE, axis );
			//End Calculate


			//Calculate Line3 Intersect
			line3 = new LineString ( );
			double tmpDirection = ARANFunctions.ReturnAngleInRadians ( pntI3, pntG );
			double tmpD = ARANFunctions.ReturnDistanceInMeters ( pntI3, pntG );
			double alpha = ( int ) turnDir * Math.Acos ( ( wi3 - wg ) / tmpD );
			tmpFromPoint = ARANFunctions.LocalToPrj ( pntG, tmpDirection - alpha, wg, 0 );
			tmpFromPoint.M = tmpDirection - alpha - ( int ) turnDir * ARANMath.C_PI_2;
			line3.Add ( ( Point ) tmpFromPoint.Clone ( ) );
			tmpToPoint = ARANFunctions.LocalToPrj ( pntI3, tmpDirection - alpha, wi3, 0 );
			tmpToPoint.M = tmpFromPoint.M;
			line3.Add ( ( Point ) tmpToPoint.Clone ( ) );

			tmpDirection = ARANFunctions.ReturnAngleInRadians ( pntI4, pntI3 );
			tmpD = ARANFunctions.ReturnDistanceInMeters ( pntI4, pntI3 );
			alpha = ( int ) turnDir * Math.Acos ( ( wi4 - wi3 ) / tmpD );
			tmpFromPoint = ARANFunctions.LocalToPrj ( pntI3, tmpDirection - alpha, wi3, 0 );
			tmpFromPoint.M = tmpDirection - alpha - ( int ) turnDir * ARANMath.C_PI_2;
			line3.Add ( ( Point ) tmpFromPoint.Clone ( ) );
			tmpToPoint = ARANFunctions.LocalToPrj ( pntI4, tmpDirection - alpha, wi4, 0 );
			tmpToPoint.M = tmpFromPoint.M;
			line3.Add ( ( Point ) tmpToPoint.Clone ( ) );

			Point tmpPoint = new Point {M = tmpFromPoint.M};
			tmpPoint = ARANFunctions.LocalToPrj ( line3 [ 3 ], tmpPoint.M, 5 * l, 0 );
			line3.Add ( ( Point ) tmpPoint.Clone ( ) );
			//End Calculate Line3

			return 1;
		}

		private static LineString CalculateHoldingArea ( Point ptNavPrj, double axis, int side, double rv, double l, MainController controller)
		{
			MultiPoint holdingArea = new MultiPoint {ptNavPrj};
			//PtC
			holdingArea [ 0 ].M = axis + ARANMath.C_PI;//change

			holdingArea.Add ( ARANFunctions.LocalToPrj ( ptNavPrj, axis, 0, -( int ) side * 2.0 * rv ) );
			//PtG '
			holdingArea [ 1 ].M = axis;
		    

			holdingArea.Add ( ARANFunctions.LocalToPrj ( holdingArea [ 1 ], axis, l, 0 ) );
			holdingArea [ 2 ].M = axis;
		    controller.SecondaryPoint = holdingArea[2];
		    controller.DrawEndOutboundLeg();

            holdingArea.Add ( ARANFunctions.LocalToPrj ( holdingArea [ 2 ], axis, 0, ( int ) side * 2 * rv ) );
			holdingArea [ 3 ].M = axis + ARANMath.C_PI;
			holdingArea.Add ( holdingArea [ 0 ] );

			return ARANFunctions.CalcTrajectoryFromMultiPoint ( holdingArea );
		}

		public static void ChangeSpiralStartParam ( double e, double stR0, double stRadial, SideDirection side, SideDirection sideChange, double change4Direction, out double newStRadial, out double newStR0 )
		{
			var turnDeg = ARANMath.Modulus ( ( change4Direction - stRadial ) * ( int ) sideChange, ARANMath.C_2xPI );

			if ( turnDeg >= ARANMath.C_PI )
			{
				sideChange = ARANMath.ChangeDirection ( sideChange );
				turnDeg = ARANMath.Modulus ( ( change4Direction - stRadial ) * ( int ) sideChange, ARANMath.C_2xPI );
			}

			newStR0 = stR0 + e * turnDeg * ( int ) side * ( int ) sideChange;

			if ( newStR0 < 0.0 )
			{
				turnDeg = stR0 / e;
				newStR0 = 0.0;
			}
			newStRadial = ARANMath.Modulus ( turnDeg * ( int ) sideChange + stRadial, ARANMath.C_2xPI );
		}

		public static void CreateSpiralBy2Radial ( Point ptCnt, double r0, double aztStRad, double aztEndRad, double e, int side, MultiPoint multiPoint, MainController controller = null )
		{
			var turnRad = ARANMath.Modulus ( ( aztEndRad - aztStRad ) * ( int ) side, ARANMath.C_2xPI );
			var dAlpha = ARANMath.C_PI / 180.0;
			var n = ( int ) Math.Ceiling ( turnRad / dAlpha );
			if ( n < 2 )
				n = 2;
			else if ( n < 5 )
				n = 5;
			else if ( n < 10 )
				n = 10;
			dAlpha = turnRad / n;
			for ( int i = 0; i <= n; i++ )
			{
				var r = r0 + ( i * dAlpha * e );
				// + dphi0 * coef
				Point ptCur = ARANFunctions.LocalToPrj ( ptCnt, aztStRad + ( i * dAlpha ) * ( int ) side, r, 0 );
				multiPoint.Add ( ptCur );
				controller?.DrawPointWithText ( ptCur, 1, i.ToString ( ) );
			}
		}

		public static int TouchTo2Spiral ( Point pntCnt1, double r10, double e1, double aztSt1, Point pntCnt2, double r20, double e2, double aztSt2, SideDirection side, ref double touchAngle1, ref double touchAngle2, out Point touchPnt1, out Point touchPnt2 )
		{

			#region In Radian

			int j, k;
			double r1, r2 = 0, fTmp, phi1 = 0, phi2 = 0, phi10 = 0, phi20 = 0;
			//Point touchPnt1, touchPnt2;

			var aztO1O2 = ARANFunctions.ReturnAngleInRadians ( pntCnt1, pntCnt2 );
			var bOutOfSpiral = false;

			touchPnt1 = null;
			touchPnt2 = null;

			for ( k = 0; k <= 10; k++ )
			{
				phi10 = ARANMath.Modulus ( aztO1O2 - ( ARANMath.C_PI_2 + 10 * ( ARANMath.C_PI / 180 ) * k ) * ( int ) side, ARANMath.C_2xPI );
				fTmp = ARANMath.Modulus ( ( phi10 - aztSt1 ) * ( int ) side, ARANMath.C_2xPI );
				r1 = r10 + e1 * fTmp;// *180 / Math.PI;
				//pnt1 = ARANFunctions.LocalToPrj ( pntCnt1, phi10, R1, 0 );
				touchPnt1 = ARANFunctions.PointAlongPlane ( pntCnt1, ARANMath.RadToDeg ( phi10 ), r1 );

				phi20 = ARANFunctions.ReturnAngleInRadians ( pntCnt2, touchPnt1 );
				var distance = ARANFunctions.ReturnDistanceInMeters ( pntCnt2, touchPnt1 );
				fTmp = ARANMath.Modulus ( ( phi20 - aztSt2 ) * ( int ) side, ARANMath.C_2xPI );
				r2 = r20 + e2 * fTmp;// *180/Math.PI;

				if ( r2 < distance )
				{
					phi20 = phi10;
					bOutOfSpiral = true;
					break; // TODO: might not be correct. Was : Exit For
				}
			}

			if ( !bOutOfSpiral )
			{
				return 0;
			}

			for ( j = 0; j <= 30; j++ )
			{
				fTmp = ARANMath.Modulus ( ( phi10 - aztSt1 ) * ( int ) side, ARANMath.C_2xPI );
				r1 = r10 + e1 * fTmp;//*180/Math.PI;
				var aztR1E = Math.Atan ( e1 * ( int ) side / r1 );

				int i;
				for ( i = 0; i <= 20; i++ )
				{
					fTmp = ARANMath.Modulus ( ( phi20 - aztSt2 ) * ( int ) side, ARANMath.C_2xPI );
					r2 = r20 + e2 * fTmp;// *180/Math.PI;
					var aztR2E = Math.Atan ( e2 * ( int ) side / r2 );
					var f = phi20 - phi10 + aztR1E - aztR2E;
					fTmp = e2 * e2;

					var f_ = ( 1 + /* 180 * */ fTmp / ( /*Math.PI * */( r2 * r2 + fTmp ) ) );

					phi2 = phi20 - ( f / f_ );

					if ( Math.Abs ( Math.Sin ( f / f_ ) ) < RadEps )
					{
						break; // TODO: might not be correct. Was : Exit For
					}
					else
					{
						phi20 = phi2;
					}
				}

				touchPnt1 = ARANFunctions.LocalToPrj ( pntCnt1, phi1, r1, 0 );
				touchPnt2 = ARANFunctions.LocalToPrj ( pntCnt2, phi2, r2, 0 );
				fTmp = ARANFunctions.ReturnAngleInRadians ( touchPnt1, touchPnt2 );
				fTmp = ARANMath.SubtractAnglesWithSign ( phi1 + ( ARANMath.C_PI_2 * ( int ) side - aztR1E ), fTmp, side );
				phi1 = phi10 + fTmp * ( int ) side;
				if ( Math.Abs ( fTmp ) < ( RadEps * 50 ) )
				{
					touchAngle1 = ARANMath.Modulus ( phi1, ARANMath.C_2xPI );
					touchAngle2 = ARANMath.Modulus ( phi2, ARANMath.C_2xPI );
					return 1;
				}
				else
				{
					phi10 = phi1;
				}
			}
			return 0;

			#endregion

			#region In Degree

			//double fTmp, distance, phi1 = 0, phi2 = 0, phi10 = 0, phi20 = 0, aztR1E, aztR2E, aztO1O2;
			//double r1 = 0, r2 = 0, f, f_;
			//long i, j, k;
			//Point pt1;
			//Point pt2;
			//bool bOutOfSpiral;

			//aztSt1 = ARANFunctions.RadToDeg ( aztSt1 );
			//aztSt2 = ARANFunctions.RadToDeg ( aztSt2 );
			//e1 = e1 / ARANFunctions.RadToDeg ( 1 );
			//e2 = e2 / ARANFunctions.RadToDeg ( 1 );

			//aztO1O2 = ARANFunctions.ReturnAngleAsDegree ( pntCnt1, pntCnt2 );
			//bOutOfSpiral = false;
			//touchAngle1 = 0;
			//touchAngle1 = 0;
			//for ( k = 0; k <= 10; k++ )
			//{
			//    phi10 = ARANMath.Modulus ( aztO1O2 - ( 90 + 10 * k ) *( int ) side, 360 );
			//    fTmp = ARANMath.Modulus ( ( phi10 - aztSt1 ) * ( int ) side, 360 );
			//    r1 = r10 + e1 * fTmp;
			//    pt1 = ARANFunctions.PointAlongPlane ( pntCnt1, phi10, r1 );

			//    phi20 = ARANFunctions.ReturnAngleAsDegree ( pntCnt2, pt1 );
			//    distance = ARANFunctions.ReturnDistanceAsMeter ( pntCnt2, pt1 );
			//    fTmp = ARANMath.Modulus ( ( phi20 - aztSt2 ) *( int ) side, 360 );
			//    r2 = r20 + e2 * fTmp;

			//    if ( r2 < distance )
			//    {
			//        phi20 = phi10;
			//        bOutOfSpiral = true;
			//        break;
			//    }
			//}
			//if ( !bOutOfSpiral )
			//    return 0;

			//for ( j = 0; j<= 30; j++ )
			//{
			//    fTmp = ARANMath.Modulus ( ( phi10 - aztSt1 ) * ( int ) side, 360 );
			//    r1 = r10 + e1 * fTmp;
			//    aztR1E = ARANFunctions.RadToDeg ( Math.Atan ( ARANFunctions.RadToDeg ( e1 * ( int ) side ) / r1 ) );

			//    for ( i = 0; i<= 20; i++ )
			//    {
			//        fTmp = ARANMath.Modulus ( ( phi20 - aztSt2 ) * ( int ) side, 360 );
			//        r2 = r20 + e2 * fTmp;
			//        aztR2E = ARANFunctions.RadToDeg ( Math.Atan ( ARANFunctions.RadToDeg ( e2 * ( int ) side ) / r2 ) );
			//        f = phi20 - phi10 + aztR1E - aztR2E;
			//        fTmp = ARANFunctions.RadToDeg ( e2 ) * ARANFunctions.RadToDeg ( e2 );
			//        f_ = 1 + fTmp / ( r2 * r2 + fTmp );
			//        phi2 = phi20 - f / f_;

			//        if ( Math.Abs ( Math.Sin ( ARANFunctions.DegToRad ( f / f_ ) ) ) < degEps )
			//            break;
			//        else
			//            phi20 = phi2;
			//    }

			//    pt1 = ARANFunctions.PointAlongPlane ( pntCnt1, ARANFunctions.DegToRad ( phi1 ), r1 );
			//    pt2 = ARANFunctions.PointAlongPlane ( pntCnt2, ARANFunctions.DegToRad ( phi2 ), r2 );
			//    fTmp = ARANFunctions.ReturnAngleAsDegree ( pt1, pt2 );
			//    fTmp = ARANFunctions.RadToDeg ( ARANMath.SubtractAnglesWithSign ( ARANFunctions.DegToRad ( phi1 + ( 90 * ( int ) side - aztR1E ) ), ARANFunctions.DegToRad ( fTmp ), ( side ) ) );
			//    phi1 = phi10 + fTmp * ( int ) side;
			//    if ( Math.Abs ( fTmp ) < ( degEps * 50) )
			//    {
			//        touchAngle1 = ARANMath.Modulus ( phi1, 360 );
			//        InitHolding.ui.DrawPointWithText ( pt1, 0, "Pt 1" );
			//        touchAngle2 = ARANMath.Modulus ( phi2, 360 );
			//        InitHolding.ui.DrawPointWithText ( pt2, 0, "Pt 2" );
			//        //Point PtCnt1, double r10, double E1, double AztSt1, 
			//        //Point PtCnt2, double r20, double E2, double AztSt2, 
			//        //SideDirection side
			//        double dist = ARANFunctions.ReturnDistanceAsMeter ( pt1, pt2 );
			//        LineString tmpPart1 = new LineString (), tmpPart2 = new LineString ();
			//        CreateSpiralBy2Radial ( pntCnt1, r10, ARANFunctions.DegToRad ( aztSt1 ), ARANFunctions.DegToRad ( aztSt1 ) - ( int ) side * ARANMath.C_PI_2, e1, ( int ) side, tmpPart1 );
			//        CreateSpiralBy2Radial ( pntCnt2, r20, ARANFunctions.DegToRad ( aztSt2 ), ARANFunctions.DegToRad ( aztSt2 ) - ( int ) side * ARANMath.C_PI_2, e2, ( int ) side, tmpPart2 );
			//        InitHolding.ui.DrawLineString ( tmpPart1, 0, 1 );
			//        InitHolding.ui.DrawLineString ( tmpPart2, 0, 1 );
			//        LineString tmpLine = new LineString ();
			//        tmpLine.Add ( pt1 );
			//        tmpLine.Add ( pt2 );
			//        InitHolding.ui.DrawLineString ( tmpLine, 0, 1 );
			//        return 1;
			//    }
			//    else
			//        phi10 = phi1;
			//}
			//return 0;

			#endregion
		}

		public static double SpiralTouchToFix ( Point ptCnt, double e, double r0, double aztStRad, SideDirection side, Point fixPnt, SideDirection sideDirIncomparisonFixPoint, double axis )
		{
			double phi = 0, phi0;

			var aztToFix = ARANFunctions.ReturnAngleInRadians ( ptCnt, fixPnt );
			var distToFix = ARANFunctions.ReturnDistanceInMeters ( ptCnt, fixPnt );

			var fTmp = ARANMath.Modulus ( ( aztToFix - aztStRad ) * ( int ) side, ARANMath.C_2xPI );

			if ( ( fTmp > ARANMath.C_PI ) && ( r0 == 0.0 ) )
			{
				fTmp = fTmp - ARANMath.C_2xPI;
			}
			var r = r0 + e * fTmp;
			if ( Math.Abs ( r - distToFix ) < DistEps )
			{
				return aztToFix;
			}

			if ( r < distToFix )
			//if (false)
			{
				phi0 = ARANMath.Modulus ( aztToFix + ARANMath.C_PI_2 * ( 1 + (int) sideDirIncomparisonFixPoint ), ARANMath.C_2xPI );
				int i;
				for ( i = 0; i <= 30; i++ )
				{
					phi = phi0;					
					var spAngle = ARANFunctions.SpiralTouchAngle ( r0, e, aztStRad + ARANMath.C_PI_2 * ( int ) side, phi, side );
					r = r0 + e * spAngle;

					var result = ARANMath.Modulus ( aztStRad + spAngle * ( int ) side, ARANMath.C_2xPI );

					var xSp = ptCnt.X + r * Math.Cos ( result );
					var ySp = ptCnt.Y + r * Math.Sin ( result );
					
					new Point ( xSp, ySp );
					fTmp = Math.Atan2 ( fixPnt.Y - ySp, fixPnt.X - xSp );

					phi0 = ARANMath.Modulus ( fTmp + ARANMath.C_PI_2 * ( 1 + (int) sideDirIncomparisonFixPoint ), ARANMath.C_2xPI );

					var dPhi = ARANMath.SubtractAngles ( phi, phi0 );

					if ( dPhi < RadEps )
					{
						return ( ( int ) side )* ( result - aztStRad );
					}
				}
			}
			else
			{

				var cosA = Math.Cos ( axis + ARANMath.C_PI_2 * ( int ) side );
				var sinA = Math.Sin ( axis + ARANMath.C_PI_2 * ( int ) side );
				var xA = distToFix * Math.Cos ( aztToFix );
				var yA = distToFix * Math.Sin ( aztToFix );
				phi0 = aztToFix;
				int j;
				for ( j = 0; j <= 20; j++ )
				{
					fTmp = ARANMath.Modulus ( ( phi0 - aztStRad ) * ( int ) side, ARANMath.C_2xPI );
					r = r0 + e * fTmp;
					var f = r * ( Math.Sin ( phi0 ) * cosA - Math.Cos ( phi0 ) * sinA ) + xA * sinA - yA * cosA;
					var f_ = ARANMath.RadToDeg ( e ) * ( int ) side * ( Math.Sin ( phi0 ) * cosA - Math.Cos ( phi0 ) * sinA ) + r * ( Math.Cos ( phi0 ) * cosA + Math.Sin ( phi0 ) * sinA );
					//f_ = E * ( int ) side * ( System.Math.Sin ( phi0 ) * cosA - System.Math.Cos ( phi0 ) * sinA ) + r * ( System.Math.Cos ( phi0 ) * cosA + System.Math.Sin ( phi0 ) * sinA );
					phi = phi0 - f / f_;
					if ( Math.Abs ( Math.Sin ( phi - phi0 ) ) < 0.001 )
					{
						return ARANMath.Modulus ( phi, ARANMath.C_2xPI );
					}
					else
						phi0 = phi;
				}
			}
			return ARANMath.Modulus ( phi, ARANMath.C_2xPI );
		}
	}
}