using System;
using ARAN.GeometryClasses;
using ARAN.Common;
using ARAN.Contracts.GeometryOperators;
using ARAN.Contracts.UI;

namespace Holding.Convential
{
    public class VORDME
    {
        public VORDME ( Navaids_DataBase navaidsDB )
        {
            _navaidsDB = navaidsDB;
            _trackingTolerance = ARANFunctions.DegToRad ( navaidsDB.VOR.TrackingTolerance );
        }

        #region Toward part
        
        public Polygon TowardConstructBasicArea (   SideDirection side, Point dmePntPrj, Point vorPntPrj,Point designatedPointPrj, double direction, 
                                                    double nominalDistanceInPrj, double nominalDistance, double limitingDistanceInPrj, double limitingDistance,
                                                    Time time, double ias, double tas, double legLength, double altitude, double radius)
        {
            double d1 = _navaidsDB.DME.MinimalError + _navaidsDB.DME.ErrorScalingUp * nominalDistance;
            double d2 = _navaidsDB.DME.MinimalError + _navaidsDB.DME.ErrorScalingUp * limitingDistance;
            _dl2 = limitingDistanceInPrj + d2;
            double _dl1 = limitingDistanceInPrj - d2;

            _toleranceArea = TowardConstructFixToleranceArea ( dmePntPrj, vorPntPrj, nominalDistanceInPrj, direction, side, _trackingTolerance, d1 );
            PolyLine PrtctInboundTurnPolyLine = new PolyLine ();

            double DL1 = limitingDistanceInPrj - d2;
            double DL2 = limitingDistanceInPrj + d2;
            PolyLine _nominalTrack;
            Point _pntE;

            Shablons.HoldingShablon ( designatedPointPrj, ias, altitude, 15, time.ValueInMinutes, direction, side, 0, out _nominalTrack, out _shablon, out _line3, out _turn180, out _pntE );

            GlobalParams.UI.DrawPolygon ( _shablon, 0, eFillStyle.sfsNull );
            GlobalParams.UI.DrawPart ( _turn180, 0, 1 );
            GlobalParams.UI.DrawPolyline ( _nominalTrack, 0, 1 );
            GlobalParams.UI.DrawPart ( _line3, 0, 1 );
            GlobalParams.UI.DrawPointWithText ( _pntE, 0, "Pnt E" );

            PolyLine Line3PolyLine = ARANFunctions.CreatePolyLineFromParts ( _line3 );
            Line3PolyLine = TransForm.Move ( Line3PolyLine, designatedPointPrj, _toleranceArea [3] ).AsPolyline;
            //InitHolding.ui.DrawPolyline ( Line3PolyLine, 255, 1 );

            PolyLine shablonPolyLine = ARANFunctions.PolygonToPolyLine ( TransForm.Move ( _shablon, designatedPointPrj, _toleranceArea [0] ).AsPolygon );
            //InitHolding.ui.DrawPolyline ( shablonPolyLine, 0, 1 );
            Part circleDL1 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, DL1 );
            Geometry geom  = GlobalParams.GeomOperators.Intersect ( shablonPolyLine, ARANFunctions.CreatePolyLineFromParts ( circleDL1 ) );
            //GlobalParams.UI.DrawPart ( circleDL1, 0, 1 );

            #region Protection of the Outbound turn

            Point C3OnShablon, tmp, C6OnLine3;
            Point pntC1 = ( ( MultiPoint ) geom ) [0];
            tmp = ( ( MultiPoint ) geom ) [1];
            if ( ARANFunctions.SideDef ( vorPntPrj, direction, pntC1 ) == side )
                pntC1 = tmp;
            //InitHolding.UI.DrawPointWithText ( pntC1, 0, "C1" );

            geom = GlobalParams.GeomOperators.Intersect ( Line3PolyLine, ARANFunctions.CreatePolyLineFromParts ( circleDL1 ) );
            Point _pntC4 = ( ( MultiPoint ) geom ) [0];
            //InitHolding.UI.DrawPointWithText ( _pntC4, 0, "C4" );

            Part circleDL2 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, DL2 );
            //InitHolding.UI.DrawPolyline ( ARANFunctions.CreatePolyLineFromParts ( circleDL2 ), 255, 1 );
            geom = GlobalParams.GeomOperators.Intersect ( shablonPolyLine, ARANFunctions.CreatePolyLineFromParts ( circleDL2 ) );

            C3OnShablon = ( ( MultiPoint ) geom ) [0];
            tmp = ( ( MultiPoint ) geom ) [1];
            if ( ARANFunctions.SideDef ( vorPntPrj, direction, C3OnShablon ) == side )
                C3OnShablon = tmp;
            //InitHolding.UI.DrawPointWithText ( C3OnShablon, 1, "C3 Shab" );

            geom = GlobalParams.GeomOperators.Intersect ( Line3PolyLine, ARANFunctions.CreatePolyLineFromParts ( circleDL2 ) );
            C6OnLine3 = ( ( MultiPoint ) geom ) [0];
            //InitHolding.UI.DrawPointWithText ( C6OnLine3, 1, "C6 Line" );

            Line tmpline = new Line ( C3OnShablon, pntC1 );
            Point pntC2 = ARANFunctions.LocalToPrj ( C3OnShablon, tmpline.DirVector.direction, Math.Abs ( d2+d1-1800 ), 0 );
            //InitHolding.UI.DrawPointWithText ( pntC2, 0, "C2" );
            tmpline = new Line ( pntC2, direction );
            _pntC3 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, DL2, tmpline, 10000000000 );
            //InitHolding.UI.DrawPointWithText ( _pntC3, 0, "C3" );

            tmpline = new Line ( C6OnLine3, _pntC4 );
            Point _pntC5 = ARANFunctions.PointAlongPlane ( C6OnLine3, tmpline.DirVector.direction, Math.Abs ( d2+d1-1800 ) );
            //InitHolding.UI.DrawPointWithText ( _pntC5, 0, "C5" );
            tmpline = new Line ( _pntC5, direction );
            _pntC6 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, DL2, tmpline, 1000000 );
            //InitHolding.UI.DrawPointWithText ( _pntC6, 0, "C6" );

            PolyLine Rps  = new PolyLine ();
            Part Rp1 = new Part (), Rp2 = new Part ();
            Rp1.AddPoint ( vorPntPrj );
            Rp1.AddPoint ( ARANFunctions.PointAlongPlane ( vorPntPrj, direction + ( int ) side * _trackingTolerance, 2500000 ) );
            Rp2.AddPoint ( vorPntPrj );
            Rp2.AddPoint ( ARANFunctions.PointAlongPlane ( vorPntPrj, direction - ( int ) side * _trackingTolerance, 2500000 ) );
            Rps.AddPart ( Rp1 );
            Rps.AddPart ( Rp2 );
            //InitHolding.UI.DrawPolyline ( Rps, 255*255*255, 1 );
            if ( ARANFunctions.SideDef ( vorPntPrj, direction + ( int ) side* _trackingTolerance, _pntC4 ) == side )
            {
                _pntC4 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, DL1, vorPntPrj, direction + ( int ) side * _trackingTolerance, 10000000 );
                _pntC6 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, DL2, vorPntPrj, direction + ( int ) side * _trackingTolerance, 10000000 );
                _pntC5 = null;
            }
            else if ( ARANFunctions.SideDef ( vorPntPrj, direction + ( int ) side * _trackingTolerance, _pntC5 ) == side )
            {
                _pntC5 = GlobalParams.GeomOperators.Intersect ( Rps, Line3PolyLine ).AsMultiPoint [0];
                //ARANFunctions.LineLineIntersect ( _line3.AsLine.RefPoint, _line3.AsVector.direction, vorPntPrj, direction + ( int ) side * _trackingTolerance ).AsPoint;
                _pntC6 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, DL2, vorPntPrj, direction + ( int ) side * _trackingTolerance, 10000 );
            }
            //InitHolding.UI.DrawPointWithText ( _pntC4, 0, "C4" );
            //if (_pntC5 != null)
            //InitHolding.UI.DrawPointWithText ( _pntC5, 0, "C5" );
            //InitHolding.UI.DrawPointWithText ( _pntC6, 0, "C6" );

            #endregion

            #region Protection of the inbound turn
            double ang1 = ARANFunctions.ReturnAngleAsRadian ( dmePntPrj, _pntC3 );
            double ang2 = ARANFunctions.ReturnAngleAsRadian ( dmePntPrj, _pntC6 );

            double littleAng = ARANMath.SubtractAngles ( ang1, ang2 );
            Point tmpPoint = new Point ();
            int n = ( int ) Math.Floor ( ARANFunctions.RadToDeg ( littleAng ) );
            Part MaxDistances = new Part ();

            Part tmpTurn180 = TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI ).AsPart;
            tmpTurn180 = ( Part ) TransForm.Move ( tmpTurn180, designatedPointPrj, _pntC3 ).AsPart.Clone ();
            PrtctInboundTurnPolyLine.AddPart ( tmpTurn180 );
            GeomFunctions.MaxDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
            MaxDistances.AddPoint ( ( Point ) tmpPoint.Clone () );

            tmp.Assign ( _pntC3 );
            for ( int i =0; i<= n; i++ )
            {
                tmpPoint = ARANFunctions.LocalToPrj ( dmePntPrj, ang1 + ( int ) side * ARANFunctions.DegToRad ( i ), DL2, 0 );
                tmpTurn180 = ( Part ) TransForm.Move ( tmpTurn180, tmp, tmpPoint ).AsPart.Clone ();
                tmp.Assign ( tmpPoint );
                PrtctInboundTurnPolyLine.AddPart ( tmpTurn180 );
                GeomFunctions.MaxDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
                MaxDistances.AddPoint ( ( Point ) tmpPoint.Clone () );
            }

            tmpTurn180 = ( Part ) TransForm.Move ( tmpTurn180, tmp, _pntC6 ).AsPart.Clone ();
            PrtctInboundTurnPolyLine.AddPart ( tmpTurn180 );
            GeomFunctions.MaxDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
            MaxDistances.Add ( ( Point ) tmpPoint.Clone () );

            tmpTurn180 = ( Part ) TransForm.Move ( tmpTurn180, _pntC6, _pntC4 ).AsPart.Clone ();
            PrtctInboundTurnPolyLine.AddPart ( tmpTurn180 );
            GeomFunctions.MaxDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
            MaxDistances.Add ( ( Point ) tmpPoint.Clone () );

            if ( _pntC5 != null )
            {
                tmpTurn180 = ( Part ) TransForm.Move ( tmpTurn180, _pntC4, _pntC5 ).AsPart.Clone ();
                PrtctInboundTurnPolyLine.AddPart ( tmpTurn180 );
                GeomFunctions.MaxDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
                MaxDistances.Add ( ( Point ) tmpPoint.Clone () );
            }

            //InitHolding.ui.DrawPolyline ( PrtctInboundTurnPolyLine, 0, 1 );
            //InitHolding.ui.DrawPart ( MaxDistances, 0, 1 );
            #endregion
            
            PolyLine circle = ARANFunctions.CreatePolyLineFromParts ( ARANFunctions.CreateArcAsPartPrj ( dmePntPrj, ARANFunctions.PointAlongPlane ( dmePntPrj, direction + ARANMath.C_PI/2, _dl2 ),
                ARANFunctions.PointAlongPlane ( dmePntPrj, direction - ARANMath.C_PI/2, _dl2 ), SideDirection.sideLeft ) );

            Geometry InnerPolygonOnA1 = null, OuterPolygonOnA1 = null, InnerPolygonOnA3 = null, OuterPolygonOnA3 = null;

            GlobalParams.GeomOperators.Cut ( TransForm.Move ( _shablon, designatedPointPrj, _toleranceArea [0] ).AsPolygon, circle, ref InnerPolygonOnA1, ref OuterPolygonOnA1 );
            GlobalParams.GeomOperators.Cut ( TransForm.Move ( _shablon, designatedPointPrj, _toleranceArea [3] ).AsPolygon, circle, ref InnerPolygonOnA3, ref OuterPolygonOnA3 );

            PolyLine OuterPolyLineOnA1 = ARANFunctions.PolygonToPolyLine ( OuterPolygonOnA1.AsPolygon );
            foreach ( Part part in OuterPolyLineOnA1 )
                PrtctInboundTurnPolyLine.AddPart ( part );

            PolyLine OuterPolyLineOnA3 = ARANFunctions.PolygonToPolyLine ( OuterPolygonOnA3.AsPolygon );
            foreach ( Part part in OuterPolyLineOnA3 )
                PrtctInboundTurnPolyLine.AddPart ( part );
            _basicArea = ChainHullAlgorithm.ConvexHull ( PrtctInboundTurnPolyLine );
            return _basicArea;
        }

        private Point [] TowardConstructFixToleranceArea ( Point dmePntPrj, Point vorPntPrj, double nominalDistanceInPrj, double direction, SideDirection side, double trackingTolerance, double d1 )
        {
            Point A1 = new Point ();
            Point A2 = new Point ();
            Point A3 = new Point ();
            Point A4 = new Point ();

            ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj - d1, vorPntPrj, direction + trackingTolerance, 350000, A1 );
            ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj + d1, vorPntPrj, direction + trackingTolerance, 350000, A2 );
            ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj - d1, vorPntPrj, direction - trackingTolerance, 350000, A3 );
            ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj + d1, vorPntPrj, direction - trackingTolerance, 350000, A4 );

            MultiPoint tmpMultipnt = new MultiPoint ();
            ARANFunctions.AddArcToMultiPoint ( dmePntPrj, A3, A1, TurnDirection.CCW, ref tmpMultipnt );
            ARANFunctions.AddArcToMultiPoint ( dmePntPrj, A2, A4, TurnDirection.CW, ref tmpMultipnt );
            MultiPoint minAndmaxPoints = GeomFunctions.MinAndMaxDistFromLineToMultipoint ( vorPntPrj, direction - ARANMath.C_PI/2, tmpMultipnt );
            double directionPerpendicular = direction - ARANMath.C_PI_2;

            Point[] toleranceArea = new Point [4];
            toleranceArea [0] = ARANFunctions.LineLineIntersect ( minAndmaxPoints [0], directionPerpendicular, vorPntPrj, direction - ( int ) side * trackingTolerance ).AsPoint;
            toleranceArea [1] = ARANFunctions.LineLineIntersect ( minAndmaxPoints [1], directionPerpendicular, vorPntPrj, direction - ( int ) side * trackingTolerance ).AsPoint;
            toleranceArea [2] = ARANFunctions.LineLineIntersect ( minAndmaxPoints [1], directionPerpendicular, vorPntPrj, direction + ( int ) side * trackingTolerance ).AsPoint;
            toleranceArea [3] = ARANFunctions.LineLineIntersect ( minAndmaxPoints [0], directionPerpendicular, vorPntPrj, direction + ( int ) side * trackingTolerance ).AsPoint;

            GlobalParams.UI.DrawPointWithText ( toleranceArea [0], 255*255, "0" );
            GlobalParams.UI.DrawPointWithText ( toleranceArea [1], 255*255, "1" );
            GlobalParams.UI.DrawPointWithText ( toleranceArea [2], 255*255, "2" );
            GlobalParams.UI.DrawPointWithText ( toleranceArea [3], 255*255, "3" );

            return toleranceArea;
        }

        public Polygon TowardConstructProtectSector1 (double ias, double tas, double direction,double altitude, Point vorPntPrj, Point dmePntPrj, Point designatedPointPrj,SideDirection side)
        {
            double rad = ARANFunctions.ReturnAngleAsRadian ( _toleranceArea [0], _toleranceArea [3] );
            rad = ARANMath.SubtractAngles ( rad, direction );

            double R = 943.27 * 0.277777777777778 / tas;
            if ( ( R > 3.0 ) )
                R = 3.0;

            double Rv = 1000.0 *3.6* tas / ( 62.83 * R );
            double H = altitude / 1000.0;
            double w = 12.0 * H + 87.0;
            double w_ = 0.277777777777778 * w;
            double E = w_ / R;
            E = E / ARANFunctions.DegToRad ( 1 );
            double ab = 5.0 * tas;

            Point PtCntB = ARANFunctions.LocalToPrj ( designatedPointPrj, direction, -ab, -Rv * ( int ) side );
            //InitHolding.ui.DrawPointWithText ( PtCntB, 0, "Cnt 1" );
            PtCntB = TransForm.Move ( PtCntB, designatedPointPrj, _toleranceArea [3] ).AsPoint;
            //InitHolding.ui.DrawPointWithText ( PtCntB, 0, "Cnt 2" );
            PtCntB = TransForm.Flip ( PtCntB, _toleranceArea [3], direction ).AsPoint;
            //InitHolding.ui.DrawPointWithText ( PtCntB, 0, "Cnt 3" );
            PtCntB = TransForm.RotateGeometry ( _toleranceArea [3], PtCntB, - (int) side * rad ).AsPoint;
            GlobalParams.UI.DrawPointWithText ( PtCntB, 0, "Cnt 4" );

            double angle = direction + (int) side * rad  + ( int ) side * ARANMath.C_PI_2;
            Point StrtPnt = ARANFunctions.LocalToPrj ( PtCntB, angle, Rv + 5 * w_, 0 );
            GlobalParams.UI.DrawPointWithText ( StrtPnt, 0, "Str Pnt" );

            double touchAngle = ARANFunctions.SpiralTouchAngle ( Rv + 5 * w_, E, angle, _line3 [0].M, side );
            double dir = angle - touchAngle * ( int ) side;
            double S = E * touchAngle + Rv+5*w_;
            Point TouchPoint= ARANFunctions.LocalToPrj ( PtCntB, dir, S, 0 );
            GlobalParams.UI.DrawPointWithText ( TouchPoint, 255*255*255, "Tch Pnt" );

            Polygon result  = new Polygon ();
            PolyLine ProtectionAreaInPolyLine = new PolyLine ();
            ProtectionAreaInPolyLine.AddPart ( new Part () );
            ProtectionAreaInPolyLine [0].Add ( _toleranceArea [3] );            
            Shablons.CreateSpiralBy2Radial ( PtCntB, Rv + 5 * w_, angle, dir, E, ( int ) ARANMath.ChangeDirection ( side ), ProtectionAreaInPolyLine [0] );

            ProtectionAreaInPolyLine [0].Add ( ( Point ) ( ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, TouchPoint, _line3 [3].M, 100000 ).Clone () ) );
            Point pntC10 = ProtectionAreaInPolyLine [0] [ProtectionAreaInPolyLine [0].Count-1];

            Part tmpTurn180 = TransForm.Flip ( _turn180, designatedPointPrj, direction ).AsPart;
            tmpTurn180 = TransForm.RotateGeometry ( designatedPointPrj, tmpTurn180, ARANMath.C_PI ).AsPart;
            tmpTurn180 = TransForm.Move ( tmpTurn180, designatedPointPrj, pntC10 ).AsPart;
            GlobalParams.UI.DrawPart ( tmpTurn180, 255*255*255, 1 );

            double angC10 = ARANFunctions.ReturnAngleAsRadian ( dmePntPrj, pntC10 );

            Point pnt = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, direction-( int ) side * _trackingTolerance, 15000 );
            //InitHolding.ui.DrawPointWithText ( pnt, 0, "C7" );

            double angC7 = ARANFunctions.ReturnAngleAsRadian ( dmePntPrj, pnt );

            int countDeg = ( int ) Math.Floor ( ARANFunctions.RadToDeg ( ARANMath.SubtractAngles ( angC10, angC7 ) ) );
            Point tmpA, tmp;
            tmp = ( Point ) pntC10.Clone ();

            for ( int i =0; i<=countDeg; i++ )
            {
                tmpA = ARANFunctions.LocalToPrj ( dmePntPrj, angC10 - ( int ) side* ARANFunctions.DegToRad ( i ), _dl2, 0 );
                //InitHolding.ui.DrawPointWithText ( tmpA, 0, "tmp" );
                tmpTurn180 = TransForm.Move ( tmpTurn180, tmp, tmpA ).AsPart;
                ProtectionAreaInPolyLine.AddPart ( ( Part ) tmpTurn180.Clone () );
                //InitHolding.ui.DrawPart ( tmpTurn180, 0, 1 );
                tmp.Assign ( tmpA );
            }

            //InitHolding.ui.DrawPolyline ( ProtectionAreaInPolyLine, 0, 1 );
            result = ARANFunctions.PolyLineToPolygon ( ProtectionAreaInPolyLine );
            result  = ChainHullAlgorithm.ConvexHull ( result );

            Polygon DifBaseAndProtectSect1  =  ( Polygon ) GlobalParams.GeomOperators.Difference ( result, _basicArea );
            //if ( DifBaseAndProtectSect1 != null )
            //    InitHolding.ui.DrawPolygon ( DifBaseAndProtectSect1, 0, eFillStyle.sfsCross );

            result = DifBaseAndProtectSect1;
            //InitHolding.ui.DrawPolygon ( result, 0, eFillStyle.sfsForwardDiagonal );
            return result;
        }

        public Polygon TowardConstructProtectSector2 ( double tas, double limitingDistanceInPrj, double direction, SideDirection side, Point dmePntPrj, Point designatedPointPrj, Time time )
        {
            Polygon result = null;
            if ( time.ValueInSeconds < 90 )
            {
                result = new Polygon ();
                PolyLine PrtctAreaSec2InPolyLine =new PolyLine ();
                Point pntC7 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, _toleranceArea [0], direction - ( int ) side * ARANFunctions.DegToRad ( 30 + 5 ), 10000000000 );
                //ui.DrawPointWithText ( pntC7, 0, "C7" );
                Point pntC8 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, _toleranceArea [2], direction - ( int ) side * ARANFunctions.DegToRad ( 30 - 5 ), 10000000000 );
                //ui.DrawPointWithText ( pntC8, 0, "C8" );
                //ui.DrawPolygon ( Shablon, 255*255, eFillStyle.sfsHorizontal );
                Part  tmpTurn180 = TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI - ( int ) side * ARANFunctions.DegToRad ( 30 ) ).AsPart;
                tmpTurn180 = TransForm.Move ( tmpTurn180, designatedPointPrj, pntC7 ).AsPart;
                //ui.DrawPart ( tmpTurn180, 0, 1);

                double angleC7 = ARANFunctions.ReturnAngleAsRadian ( dmePntPrj, pntC7 );
                double angle = ARANMath.SubtractAngles ( angleC7, ARANFunctions.ReturnAngleAsRadian ( dmePntPrj, pntC8 ) );
                int count = ( int ) Math.Floor ( ARANFunctions.RadToDeg ( angle ) );
                int sideInInt = ( int ) side;
                Point tmp = new Point ();
                for ( int i =0; i<=count; i++ )
                {
                    tmp.Assign ( ARANFunctions.LocalToPrj ( dmePntPrj, angleC7 + sideInInt*ARANFunctions.DegToRad ( i ), _dl2, 0 ) );
                    PrtctAreaSec2InPolyLine.Add ( ( Part ) TransForm.Move ( tmpTurn180, pntC7, tmp ).AsPart.Clone () );
                    //ui.DrawPart ( PrtctAreaSec2InPolyLine [PrtctAreaSec2InPolyLine.Count-1], 0, 1);
                    //ui.DrawPointWithText ( tmp, 0, "tmp" );
                }

                PrtctAreaSec2InPolyLine.Add ( ( Part ) TransForm.Move ( tmpTurn180, pntC7, pntC8 ).AsPart.Clone () );
                //ui.DrawPart ( PrtctAreaSec2InPolyLine [PrtctAreaSec2InPolyLine.Count-1], 0, 1 );
                result = ARANFunctions.PolyLineToPolygon ( PrtctAreaSec2InPolyLine );

                Polygon ConvexedPolygon = ChainHullAlgorithm.ConvexHull ( _basicArea, result );
                result = GlobalParams.GeomOperators.Difference ( ConvexedPolygon, _basicArea ).AsPolygon;
                //InitHolding.ui.DrawPolygon ( result, 255*255*255, eFillStyle.sfsCross );
                //ui.DrawPolygon ( tmpPolygon, 0, eFillStyle.sfsVertical );
            }
            return result;
        }

        public Polygon TowardConstructRecipDirectEntry2SecondaryPnt ( double legLength, double direction, Point designatedPointPrj, Point vorPntPrj,
                                                                      Point dmePntPrj, SideDirection side, double radius, out int resultAngle)
        {
            int sideInInt = ( int ) side;
            Point SecPnt = ARANFunctions.LocalToPrj ( designatedPointPrj, direction, legLength, -(int)side* 2*radius );
            GlobalParams.UI.DrawPointWithText ( SecPnt, 0, "Sec Pnt" );
            double angleRE = ARANFunctions.ReturnAngleAsRadian ( vorPntPrj, SecPnt );
            Part prt = new Part();
            prt.Add(vorPntPrj);
            prt.Add(ARANFunctions.LocalToPrj(vorPntPrj,angleRE - (int) side * _trackingTolerance,100000,0));
            //InitHolding.ui.DrawPart ( prt, 0, 1 );
            prt [1] = ARANFunctions.LocalToPrj ( vorPntPrj, angleRE + ( int ) side * _trackingTolerance, 100000, 0 );
            //InitHolding.ui.DrawPart ( prt, 0, 1 );

            double angleRE_RP = ARANMath.SubtractAngles ( direction, angleRE );
            Point pntI2 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, angleRE - ( int ) side*_trackingTolerance, 100000000 );
            //InitHolding.ui.DrawPointWithText ( pntI2, 0, "I2" );
            Point pntI4 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, angleRE + ( int ) side *_trackingTolerance, 100000000 );
            //InitHolding.ui.DrawPointWithText ( pntI4, 0, "I4" );
            double angI2 = ARANFunctions.ReturnAngleAsRadian ( vorPntPrj, pntI2 );
            double angI4 = ARANFunctions.ReturnAngleAsRadian ( vorPntPrj, pntI4 );
            int n = ( int ) Math.Floor (ARANFunctions.RadToDeg (ARANMath.SubtractAngles ( angI2, angI4 ) ));
            Point tmpPnt = new Point ();
            Polygon result = new Polygon ();
            PolyLine PrtctSecPnt = new PolyLine ();
            Part tmpPart = TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI-sideInInt*angleRE_RP ).AsPart;
            tmpPart = TransForm.Move ( tmpPart, designatedPointPrj, pntI2 ).AsPart;
            for ( int i = 0; i<=n; i++ )
            {
                tmpPnt.Assign ( ARANFunctions.LocalToPrj ( vorPntPrj, angI2 + sideInInt * ARANFunctions.DegToRad ( i ), _dl2, 0 ) );
                //InitHolding.ui.DrawPointWithText ( tmpPnt, 0, "Tmp" );
                PrtctSecPnt.AddPart ( TransForm.Move ( tmpPart, pntI2, tmpPnt ).AsPart );
                //ui.DrawPart ( PrtctSecPnt [PrtctSecPnt.Count-1], 0, 1 );
            }
            PrtctSecPnt.AddPart ( TransForm.Move ( tmpPart, pntI2, pntI4 ).AsPart );
            //InitHolding.ui.DrawPolyline( PrtctSecPnt , 0, 1 );
            //PrtctAreaDirEntr2SecPnt = ARANFunctions.PolyLineToPolygon ( PrtctSecPnt );
            Polygon tmp;
            tmp = ChainHullAlgorithm.ConvexHull ( _basicArea, PrtctSecPnt );
            result = ChainHullAlgorithm.ConvexHull ( PrtctSecPnt );
            result = GlobalParams.GeomOperators.Difference ( result, _basicArea ).AsPolygon;
            //InitHolding.ui.DrawPolygon ( result, 255*255, eFillStyle.sfsCross );

            resultAngle  = ( int ) ARANFunctions.RadToDeg ( angleRE_RP );
            return result;
        }

        #endregion

        #region Away part

        public Polygon AwayConstructBasicArea ( SideDirection side, Point dmePntPrj, Point vorPntPrj, Point designatedPointPrj, double direction,
                                                    double nominalDistanceInPrj, double nominalDistance, double limitingDistanceInPrj, double limitingDistance,
                                                    Time time, double ias, double tas, double legLength, double altitude, double radius )
        {
            double d1 = _navaidsDB.DME.MinimalError + _navaidsDB.DME.ErrorScalingUp * nominalDistance;
            double d2 = _navaidsDB.DME.MinimalError + _navaidsDB.DME.ErrorScalingUp * limitingDistance;

            double dl1 = limitingDistanceInPrj + d2;
            _dl2 = limitingDistanceInPrj - d2;

            _toleranceArea = AwayConstructFixToleranceArea ( dmePntPrj, vorPntPrj, nominalDistanceInPrj, direction, side, _trackingTolerance, d1 );

            PolyLine nominalTrack;
            Point pntE;

            Shablons.HoldingShablon ( designatedPointPrj, ias, altitude, 15, time.ValueInMinutes, direction, side, 0, out nominalTrack, out _shablon, out _line3, out _turn180, out pntE );

            nominalTrack = TransForm.RotateGeometry ( designatedPointPrj, nominalTrack, ARANMath.C_PI ).AsPolyline;
            _turn180 = TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI ).AsPart;
            _line3 = TransForm.RotateGeometry ( designatedPointPrj, _line3, ARANMath.C_PI ).AsPart;
            _shablon = TransForm.RotateGeometry ( designatedPointPrj, _shablon, ARANMath.C_PI ).AsPolygon;
            pntE = TransForm.RotateGeometry ( designatedPointPrj, pntE, ARANMath.C_PI ).AsPoint;

            GlobalParams.UI.DrawPolygon ( _shablon, 0, eFillStyle.sfsNull );
            GlobalParams.UI.DrawPart ( _turn180, 0, 1 );
            GlobalParams.UI.DrawPolyline ( nominalTrack, 0, 1 );
            GlobalParams.UI.DrawPart ( _line3, 0, 1 );
            GlobalParams.UI.DrawPointWithText ( pntE, 0, "Pnt E" );

            if ( !_isWithLimitingRadial )
            {
                #region Protection of the Outbound turn

                PolyLine Line3PolyLine = ARANFunctions.CreatePolyLineFromParts ( _line3 );
                Line3PolyLine = TransForm.Move ( Line3PolyLine, designatedPointPrj, _toleranceArea [3] ).AsPolyline;
                //InitHolding.ui.DrawPolyline ( Line3PolyLine, 255, 1 );
                PolyLine shablonPolyLine = ARANFunctions.PolygonToPolyLine ( TransForm.Move ( _shablon, designatedPointPrj, _toleranceArea [0] ).AsPolygon );
                //InitHolding.ui.DrawPolyline ( shablonPolyLine, 0, 1 );
                Part circleDL1 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, dl1 );
                //InitHolding.UI.DrawPart ( circleDL1, 0, 1 );
                Line straightPart = GetStraightPartOfPolygon ( side, direction, vorPntPrj, _shablon );

                straightPart.RefPoint = TransForm.Move ( straightPart.RefPoint, designatedPointPrj, _toleranceArea [0] ).AsPoint;

                Part tmpPart = new Part ();
                tmpPart.Add ( straightPart.RefPoint );
                tmpPart.Add ( ARANFunctions.LocalToPrj ( tmpPart [0], straightPart.DirVector.direction, straightPart.DirVector.Length, 0 ) );
                //InitHolding.ui.DrawPart ( tmpPart, 0, 1 );

                Point pntC1  = ARANFunctions.CircleVectorIntersect ( dmePntPrj, dl1, straightPart.RefPoint, straightPart.DirVector.direction - ( int ) side * ARANMath.C_PI, straightPart.DirVector.Length );
                //InitHolding.UI.DrawPointWithText ( pntC1, 0, "C1" );

                Point C3OnShablon, tmp, C6OnLine3;

                Geometry geom = GlobalParams.GeomOperators.Intersect ( Line3PolyLine, ARANFunctions.CreatePolyLineFromParts ( circleDL1 ) );
                Point pntC4 = ( ( MultiPoint ) geom ) [0];

                Part circleDL2 = ARANFunctions.CreateCircleAsPartPrj ( dmePntPrj, _dl2 );
                //InitHolding.UI.DrawPart ( circleDL2, 0, 1 );
                //InitHolding.ui.DrawPointWithText ( pntC4, 0, "C4" );

                C3OnShablon = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, straightPart.RefPoint, straightPart.DirVector.direction + ARANMath.C_PI, straightPart.DirVector.Length );
                //InitHolding.ui.DrawPointWithText ( C3OnShablon, 1, "C3 Shab" );

                geom = GlobalParams.GeomOperators.Intersect ( Line3PolyLine, ARANFunctions.CreatePolyLineFromParts ( circleDL2 ) );
                C6OnLine3 = ( ( MultiPoint ) geom ) [0];
                //InitHolding.ui.DrawPointWithText ( C6OnLine3, 1, "C6 Line" );

                Line tmpline = new Line ( C3OnShablon, pntC1 );
                Point pntC2 = ARANFunctions.LocalToPrj ( C3OnShablon, tmpline.DirVector.direction, Math.Abs ( d2+d1-1800 ), 0 );
                //InitHolding.UI.DrawPointWithText ( pntC2, 0, "C2" );
                tmpline = new Line ( pntC2, direction );
                _pntC3 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, tmpline, 10000000000 );

                tmpline = new Line ( C6OnLine3, pntC4 );
                Point pntC5 = ARANFunctions.PointAlongPlane ( C6OnLine3, tmpline.DirVector.direction, Math.Abs ( d2+d1-1800 ) );
                //InitHolding.ui.DrawPointWithText ( pntC5, 0, "C5" );
                tmpline = new Line ( pntC5, direction );
                _pntC6 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, tmpline, 1000000 );

                PolyLine Rps  = new PolyLine ();
                Part Rp1 = new Part (), Rp2 = new Part ();
                Rp1.AddPoint ( vorPntPrj );
                Rp1.AddPoint ( ARANFunctions.PointAlongPlane ( vorPntPrj, direction + ( int ) side * _trackingTolerance, 150000 ) );
                Rp2.AddPoint ( vorPntPrj );
                Rp2.AddPoint ( ARANFunctions.PointAlongPlane ( vorPntPrj, direction - ( int ) side * _trackingTolerance, 150000 ) );
                Rps.AddPart ( Rp1 );
                Rps.AddPart ( Rp2 );
                //InitHolding.UI.DrawPolyline ( Rps, 255*255*255, 1 );
                if ( ARANFunctions.SideDef ( vorPntPrj, direction - ( int ) side * _trackingTolerance, pntC4 ) != side )
                {
                    pntC4 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, dl1, vorPntPrj, direction - ( int ) side * _trackingTolerance, 10000000 );
                    _pntC6 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, direction - ( int ) side * _trackingTolerance, 10000000 );
                    pntC5 = null;
                }
                else if ( ARANFunctions.SideDef ( vorPntPrj, direction - ( int ) side * _trackingTolerance, pntC5 ) != side )
                {
                    pntC5 = GlobalParams.GeomOperators.Intersect ( Line3PolyLine, Rps ).AsMultiPoint [0];
                    _pntC6 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, direction - ( int ) side * _trackingTolerance, 10000 );
                }
                //InitHolding.UI.DrawPointWithText ( pntC4, 0, "C4" );
                //if (pntC5 != null)
                //    InitHolding.UI.DrawPointWithText ( pntC5, 0, "C5" );
                //InitHolding.UI.DrawPointWithText ( pntC6, 0, "C6" );
                #endregion

                #region Protection of the inbound turn

                double ang1 = ARANFunctions.ReturnAngleAsRadian ( dmePntPrj, _pntC3 );
                double ang2 = ARANFunctions.ReturnAngleAsRadian ( dmePntPrj, _pntC6 );

                double littleAng = ARANMath.SubtractAngles ( ang1, ang2 );
                Point tmpPoint = new Point ();
                int n = ( int ) Math.Floor ( ARANFunctions.RadToDeg ( littleAng ) );
                Part MinDistances = new Part ();

                Polygon prtctInboundTurnPlygon = new Polygon ();
                Part tmpTurn180 = TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI ).AsPart;

                tmpTurn180 = ( Part ) TransForm.Move ( tmpTurn180, designatedPointPrj, pntC2 ).AsPart.Clone ();
                //InitHolding.ui.DrawPart ( tmpTurn180, 0, 1 );
                prtctInboundTurnPlygon.AddRing ( ARANFunctions.PartToRing ( tmpTurn180 ) );
                GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
                MinDistances.AddPoint ( ( Point ) tmpPoint.Clone () );

                tmpTurn180 = ( Part ) TransForm.Move ( tmpTurn180, pntC2, _pntC3 ).AsPart.Clone ();
                //InitHolding.ui.DrawPart ( tmpTurn180, 0, 1 );
                prtctInboundTurnPlygon = GlobalParams.GeomOperators.UnionGeometry ( prtctInboundTurnPlygon, ARANFunctions.CreatePolygonFromRings ( ARANFunctions.PartToRing ( tmpTurn180 ) ) ).AsPolygon;
                GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
                MinDistances.AddPoint ( ( Point ) tmpPoint.Clone () );

                tmp = new Point ();
                tmp.Assign ( _pntC3 );
                for ( int i =0; i<= n; i++ )
                {
                    tmpPoint = ARANFunctions.LocalToPrj ( dmePntPrj, ang1 - ( int ) side * ARANFunctions.DegToRad ( i ), _dl2, 0 );
                    tmpTurn180 = ( Part ) TransForm.Move ( tmpTurn180, tmp, tmpPoint ).AsPart.Clone ();
                    tmp.Assign ( tmpPoint );
                    prtctInboundTurnPlygon = GlobalParams.GeomOperators.UnionGeometry ( prtctInboundTurnPlygon, ARANFunctions.CreatePolygonFromRings ( ARANFunctions.PartToRing ( tmpTurn180 ) ) ).AsPolygon;
                    GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
                    MinDistances.AddPoint ( ( Point ) tmpPoint.Clone () );
                }

                Geometry innerPolygon = null, outerPolygon = null;
                MultiPoint mltPnt = new MultiPoint ();
                foreach ( Ring rng in TransForm.Move ( _shablon, designatedPointPrj, _toleranceArea [0] ).AsPolygon )
                {
                    mltPnt.AddMultiPoint ( rng );
                }
                foreach ( Ring rng in TransForm.Move ( _shablon, designatedPointPrj, _toleranceArea [3] ).AsPolygon )
                {
                    mltPnt.AddMultiPoint ( rng );
                }
                mltPnt.AddPoint ( pntC2 );
                mltPnt.AddPoint ( _pntC3 );
                _basicArea = GlobalParams.GeomOperators.ConvexHull ( mltPnt ).AsPolygon;

                PolyLine circle = ARANFunctions.CreatePolyLineFromParts ( ARANFunctions.CreateArcAsPartPrj ( dmePntPrj, ARANFunctions.LocalToPrj ( dmePntPrj, direction + ARANMath.C_PI/2, _dl2, 0 ),
                                      ARANFunctions.LocalToPrj ( dmePntPrj, direction - ARANMath.C_PI/2, _dl2, 0 ), SideDirection.sideLeft ) );
                //InitHolding.ui.DrawPolyline ( circle, 0, 2 );

                GlobalParams.GeomOperators.Cut ( _basicArea, circle, ref outerPolygon, ref innerPolygon );
                _basicArea = outerPolygon.AsPolygon;

                tmpTurn180 = ( Part ) TransForm.Move ( tmpTurn180, tmp, _pntC6 ).AsPart.Clone ();
                //prtctInboundTurnPlygon = geomOperators.UnionGeometry ( prtctInboundTurnPlygon, ARANFunctions.CreatePolygonFromRings ( ARANFunctions.PartToRing ( tmpTurn180 ) ) ).AsPolygon;
                //GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
                //MinDistances.Add ( ( Point ) tmpPoint.Clone () );

                //prtctInboundTurnPlygon = geomOperators.UnionGeometry ( prtctInboundTurnPlygon, ARANFunctions.CreatePolygonFromRings ( ARANFunctions.PartToRing ( tmpTurn180 ) ) ).AsPolygon;
                //GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
                //MinDistances.Add ( ( Point ) tmpPoint.Clone () );
                mltPnt.Clear ();
                foreach ( Ring rng in _basicArea )
                {
                    mltPnt.AddMultiPoint ( rng );
                }
                mltPnt.AddMultiPoint ( ARANFunctions.PartToRing ( tmpTurn180 ) );
                tmpTurn180 = ( Part ) TransForm.Move ( tmpTurn180, _pntC6, pntC4 ).AsPart.Clone ();
                mltPnt.AddMultiPoint ( ARANFunctions.PartToRing ( tmpTurn180 ) );

                if ( pntC5 != null )
                {
                    tmpTurn180 = ( Part ) TransForm.Move ( tmpTurn180, pntC4, pntC5 ).AsPart.Clone ();
                    mltPnt.AddMultiPoint ( ARANFunctions.PartToRing ( tmpTurn180 ) );
                    //prtctInboundTurnPlygon = geomOperators.UnionGeometry ( prtctInboundTurnPlygon, ARANFunctions.CreatePolygonFromRings ( ARANFunctions.PartToRing ( tmpTurn180 ) ) ).AsPolygon;
                    //GeomFunctions.MinDistFromPointToGeometry ( dmePntPrj, tmpTurn180, out tmpPoint );
                    //MinDistances.Add ( ( Point ) tmpPoint.Clone () );
                }
                _basicArea = GlobalParams.GeomOperators.ConvexHull ( mltPnt ).AsPolygon;

                //InitHolding.ui.DrawPolygon ( prtctInboundTurnPlygon, 0, eFillStyle.sfsCross );
                //InitHolding.ui.DrawPart ( MinDistances, 0, 1 );
                #endregion

                _basicArea = GlobalParams.GeomOperators.UnionGeometry ( _basicArea, prtctInboundTurnPlygon ).AsPolygon;
            }
            else
            {
                #region Procedure away from the station with limiting radial

                #region Calculate Point "R"

                Part dirPart = new Part();
                dirPart.Add(designatedPointPrj);
                dirPart.Add(ARANFunctions.LocalToPrj(designatedPointPrj, direction - ARANMath.C_PI, 100000000, 0));
                Geometry geometry = GlobalParams.GeomOperators.Intersect ( ARANFunctions.PolygonToPolyLine( _shablon), ARANFunctions.CreatePolyLineFromParts( dirPart ));
                Point pntIntersct = geometry.AsMultiPoint [0];
                GlobalParams.UI.DrawPointWithText (pntIntersct , 0, "Int" );

                double dIsa = 15;
                double V = ARANMath.IASToTAS ( ias, altitude, dIsa );
                double v3600 =  V;
                double R = 943.27 * 0.277777777777778 / V;
                if ( ( R > 3.0 ) )
                    R = 3.0;

                double Rv = 1000.0 *3.6* V / ( 62.83 * R );
                double H = altitude / 1000.0;
                double w = 12.0 * H + 87.0;
                double w_ = 0.277777777777778 * w;
                double E = w_ / R;
                E = E / ARANFunctions.DegToRad ( 1 );
                double ab = 5.0 * v3600;

                Point PtCntB = ARANFunctions.LocalToPrj ( designatedPointPrj, direction, -ab, -Rv * ( int ) side );
                PtCntB = TransForm.RotateGeometry ( designatedPointPrj, PtCntB, ARANMath.C_PI ).AsPoint;
                PtCntB = TransForm.Move ( PtCntB, designatedPointPrj, _toleranceArea [1] ).AsPoint;
                GlobalParams.UI.DrawPointWithText ( PtCntB, 0, "Cnt 2" );

                double angle = direction + ( int ) side * ARANMath.C_PI_2;
                Point StrtPnt = ARANFunctions.LocalToPrj ( PtCntB, angle, Rv + 5 * w_, 0 );
                StrtPnt = TransForm.RotateGeometry ( PtCntB, StrtPnt, ARANMath.C_PI ).AsPoint;
                GlobalParams.UI.DrawPointWithText ( StrtPnt, 0, "Str Pnt" );

                Part tmpPart = new Part ();
                tmpPart.Add ( TransForm.RotateGeometry(PtCntB, _toleranceArea [1], ARANMath.C_PI).AsPoint );
                Shablons.CreateSpiralBy2Radial ( PtCntB, Rv + 5 * w_, angle, direction, E, ( int ) side, tmpPart );
                tmpPart = TransForm.RotateGeometry ( PtCntB, tmpPart, ARANMath.C_PI ).AsPart;
                GlobalParams.UI.DrawPart ( tmpPart, 255, 1 );

                double touchAngle = Shablons.SpiralTouchToFix ( PtCntB, E , Rv + 5 * w_, angle, side, pntIntersct, 1, direction);
                //Point pnt = ARANFunctions.LocalToPrj ( PtCntB, touchAngle, (Rv+ 5 * w_) + E * touchAngle, 0 );
                //GlobalParams.UI.DrawPointWithText ( pnt, 0, "Tch" );

                #endregion
            
                #endregion
            }
            return _basicArea;
        }

        private Point [] AwayConstructFixToleranceArea ( Point dmePntPrj, Point vorPntPrj, double nominalDistanceInPrj, double direction, SideDirection side, double trackingTolerance, double d1 )
        {
            Point A1 = new Point ();
            Point A2 = new Point ();
            Point A3 = new Point ();
            Point A4 = new Point ();

            ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj + d1, vorPntPrj, direction - trackingTolerance, 350000, A1 );
            ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj - d1, vorPntPrj, direction - trackingTolerance, 350000, A2 );
            ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj + d1, vorPntPrj, direction + trackingTolerance, 350000, A3 );
            ARANFunctions.CircleVectorIntersect ( dmePntPrj, nominalDistanceInPrj - d1, vorPntPrj, direction + trackingTolerance, 350000, A4 );

            MultiPoint tmpMultipnt = new MultiPoint ();
            ARANFunctions.AddArcToMultiPoint ( dmePntPrj, A3, A1, TurnDirection.CW, ref tmpMultipnt );
            ARANFunctions.AddArcToMultiPoint ( dmePntPrj, A2, A4, TurnDirection.CCW, ref tmpMultipnt );
            MultiPoint minAndmaxPoints = GeomFunctions.MinAndMaxDistFromLineToMultipoint ( vorPntPrj, direction - ARANMath.C_PI/2, tmpMultipnt );
            double directionPerpendicular = direction - ARANMath.C_PI_2;

            Point[] toleranceArea = new Point [4];
            toleranceArea [0] = ARANFunctions.LineLineIntersect ( minAndmaxPoints [1], directionPerpendicular, vorPntPrj, direction + ( int ) side * trackingTolerance ).AsPoint;
            toleranceArea [1] = ARANFunctions.LineLineIntersect ( minAndmaxPoints [0], directionPerpendicular, vorPntPrj, direction + ( int ) side * trackingTolerance ).AsPoint;
            toleranceArea [2] = ARANFunctions.LineLineIntersect ( minAndmaxPoints [0], directionPerpendicular, vorPntPrj, direction - ( int ) side * trackingTolerance ).AsPoint;
            toleranceArea [3] = ARANFunctions.LineLineIntersect ( minAndmaxPoints [1], directionPerpendicular, vorPntPrj, direction - ( int ) side * trackingTolerance ).AsPoint;

            GlobalParams.UI.DrawPointWithText ( toleranceArea [0], 255*255, "0" );
            GlobalParams.UI.DrawPointWithText ( toleranceArea [1], 255*255, "1" );
            GlobalParams.UI.DrawPointWithText ( toleranceArea [2], 255*255, "2" );
            GlobalParams.UI.DrawPointWithText ( toleranceArea [3], 255*255, "3" );

            return toleranceArea;
        }

        public Polygon AwayConstructProtectSector1 (double direction, Point designatedPointPrj, Point dmePntPrj, Point vorPntPrj, SideDirection side, double altitude, double ias)
        {
            double rad = ARANFunctions.ReturnAngleAsRadian ( _toleranceArea [0], _toleranceArea [3] );
            rad = ARANMath.SubtractAngles ( rad, direction );

            double dIsa = 15;
            double V = ARANMath.IASToTAS ( ias, altitude, dIsa );
            double v3600 =  V;
            double R = 943.27 * 0.277777777777778 / V;
            if ( ( R > 3.0 ) )
                R = 3.0;

            double Rv = 1000.0 *3.6* V / ( 62.83 * R );
            double H = altitude / 1000.0;
            double w = 12.0 * H + 87.0;
            double w_ = 0.277777777777778 * w;
            double E = w_ / R;
            E = E / ARANFunctions.DegToRad ( 1 );
            double ab = 5.0 * v3600;

            Point PtCntB = ARANFunctions.LocalToPrj ( designatedPointPrj, direction, -ab, -Rv * ( int ) side );
            //InitHolding.ui.DrawPointWithText ( PtCntB, 0, "Cnt 0" );
            PtCntB = TransForm.RotateGeometry ( designatedPointPrj, PtCntB, ARANMath.C_PI ).AsPoint;
            //InitHolding.ui.DrawPointWithText ( PtCntB, 0, "Cnt 1" );
            PtCntB = TransForm.Move ( PtCntB, designatedPointPrj, _toleranceArea [3] ).AsPoint;
            //InitHolding.ui.DrawPointWithText ( PtCntB, 0, "Cnt 2" );
            PtCntB = TransForm.Flip ( PtCntB, _toleranceArea [3], direction ).AsPoint;
            //InitHolding.ui.DrawPointWithText ( PtCntB, 0, "Cnt 3" );
            PtCntB = TransForm.RotateGeometry ( _toleranceArea [3], PtCntB, -( int ) side * rad ).AsPoint;
            //InitHolding.ui.DrawPointWithText ( PtCntB, 0, "Cnt 4" );

            double angle = direction + ( int ) side * ( rad  + ARANMath.C_PI_2 );
            Point StrtPnt = ARANFunctions.LocalToPrj ( PtCntB, angle, Rv + 5 * w_, 0 );
            StrtPnt = TransForm.RotateGeometry (PtCntB,StrtPnt,ARANMath.C_PI ).AsPoint;
            //InitHolding.ui.DrawPointWithText ( StrtPnt, 0, "Str Pnt" );

            double touchAngle = ARANFunctions.SpiralTouchAngle ( Rv + 5 * w_, E, angle, _line3 [0].M, side );
            double dir = angle  -  touchAngle * ( int ) side;
            double S = E * touchAngle + Rv+5*w_;
            Point TouchPoint= ARANFunctions.LocalToPrj ( PtCntB, dir, S, 0 );
            TouchPoint = TransForm.RotateGeometry ( PtCntB, TouchPoint, ARANMath.C_PI ).AsPoint;

            //InitHolding.ui.DrawPart ( _turn180, 0, 1 );
            Part turn180 = TransForm.Flip ( _turn180, designatedPointPrj, direction ).AsPart;
            //InitHolding.ui.DrawPart ( turn180, 255, 1 );
            turn180 = TransForm.RotateGeometry ( designatedPointPrj, turn180, -(int) side * rad ).AsPart;
            //InitHolding.ui.DrawPart ( turn180, 255*255, 1 );
            turn180 = TransForm.Move ( turn180, designatedPointPrj, _toleranceArea [3] ).AsPart;
            //InitHolding.ui.DrawPart ( turn180, 255*255*255, 1 );

            Polygon result  = new Polygon ();
            Part tmpPart = new Part ();
            tmpPart.Add ( _toleranceArea [3] );            
            Shablons.CreateSpiralBy2Radial ( PtCntB, Rv + 5 * w_, angle + ARANMath.C_PI, dir+ ARANMath.C_PI, E, ( int ) ARANMath.ChangeDirection ( side ),tmpPart);

            Point pntC10 = ( Point ) ( ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, TouchPoint, _line3 [3].M, 100000 ).Clone () );
            if ( pntC10 == null )
                return null;

            tmpPart.Add (pntC10 );            
            turn180 = TransForm.Move ( turn180, _toleranceArea [3], pntC10 ).AsPart;
            turn180 = TransForm.RotateGeometry ( pntC10, turn180, ARANMath.C_PI + ( int ) side * rad ).AsPart;            
            double angC10 = ARANFunctions.ReturnAngleAsRadian ( dmePntPrj, pntC10 );
            Point pntIntersectRp1DL2 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, vorPntPrj, direction + ( int ) side * _trackingTolerance, 15000000 );
            tmpPart.Add ( pntIntersectRp1DL2 );
            result.AddRing ( ARANFunctions.PartToRing ( tmpPart ) );
            //InitHolding.ui.DrawPolygon ( result, 255, eFillStyle.sfsCross);
            //InitHolding.ui.DrawPointWithText ( pntIntersectRp1DL2, 0, "Intersect" );
            double angIntersectRp1DL2 = ARANFunctions.ReturnAngleAsRadian ( dmePntPrj, pntIntersectRp1DL2 );
            int countDeg = ( int ) Math.Floor ( ARANFunctions.RadToDeg ( ARANMath.SubtractAngles ( angC10, angIntersectRp1DL2 ) ) );
            Point tmpA, tmp;
           
            tmp = ( Point ) pntC10.Clone ();
            Polygon turn180s = new Polygon ();
            for ( int i =0; i<=countDeg; i++ )
            {
                tmpA = ARANFunctions.LocalToPrj ( dmePntPrj, angC10 + ( int ) side* ARANFunctions.DegToRad ( i ), _dl2, 0 );
                //InitHolding.ui.DrawPointWithText ( tmpA, 0, "tmp" );
                turn180 = TransForm.Move ( turn180, tmp, tmpA ).AsPart;
                if ( i == 0 )
                    turn180s.AddRing ( ARANFunctions.PartToRing ( turn180 ) );
                else
                    turn180s = GlobalParams.GeomOperators.UnionGeometry ( turn180s, ARANFunctions.CreatePolygonFromRings ( ARANFunctions.PartToRing ( turn180 ) ) ).AsPolygon;
                //InitHolding.ui.DrawPart ( turn180, 0, 1 );
                tmp.Assign ( tmpA );
            }

            result = GlobalParams.GeomOperators.UnionGeometry ( result, turn180s ).AsPolygon;
            //InitHolding.ui.DrawPolygon ( result, 0, eFillStyle.sfsVertical );
            return result  =  ( Polygon ) GlobalParams.GeomOperators.Difference ( result, _basicArea );
        }

        public Polygon AwayConstructProtectSector2 ( double tas, double limitingDistanceInPrj, double direction, SideDirection side, Point dmePntPrj, Point designatedPointPrj, Time time, double altitude)
        {
            Polygon result = null;
            if ( time.ValueInSeconds < 90 )
            {
                result = new Polygon ();
                Point pntC7 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, _toleranceArea [0], direction - ( int ) side * ARANFunctions.DegToRad ( 30 + 5 ), 10000000000 );
                //InitHolding.UI.DrawPointWithText ( pntC7, 0, "C7" );
                Point pntC8 = ARANFunctions.CircleVectorIntersect ( dmePntPrj, _dl2, _toleranceArea [2], direction - ( int ) side * ARANFunctions.DegToRad ( 30 - 5 ), 10000000000 );
                //InitHolding.UI.DrawPointWithText ( pntC8, 0, "C8" );
                Part  tmpTurn180 = TransForm.RotateGeometry ( designatedPointPrj, _turn180, ARANMath.C_PI - ( int ) side * ARANFunctions.DegToRad ( 30 ) ).AsPart;
                tmpTurn180 = TransForm.Move ( tmpTurn180, designatedPointPrj, pntC7 ).AsPart;
                //InitHolding.UI.DrawPart ( tmpTurn180, 255, 1 );

                #region Draw the common tangents to the curve "10" (first and last turn180) and to the basic area

                double R =   943.27 * 0.277777777777778 / tas;
                if ( R > 3.0 )
                    R = 3.0;
                double Rv = 1000.0 *3.6 * tas / ( 62.83 * R );
                double H = altitude / 1000.0;
                double w = 12.0 * H + 87.0;
                double w_ = 0.277777777777778 * w;
                double E = w_ / R;
                double ab = 5.0 * tas;

                Point pntCntBOnC3 = ARANFunctions.LocalToPrj ( designatedPointPrj, direction, -ab, -Rv * ( int ) side );
                pntCntBOnC3 = TransForm.Move ( pntCntBOnC3, designatedPointPrj, _pntC3 ).AsPoint;
                double angleForC3 = direction + ( int ) side * ARANMath.C_PI_2;

                Point pntCntBOnC7 = ARANFunctions.LocalToPrj ( designatedPointPrj, direction - ( int ) side * ARANFunctions.DegToRad ( 30 ), -ab, -Rv * ( int ) side );
                pntCntBOnC7 = TransForm.Move ( pntCntBOnC7, designatedPointPrj, pntC7 ).AsPoint;
                double angleForC7 = angleForC3 - ( int ) side * ARANFunctions.DegToRad ( 30 );
                double cntRad1 = double.NaN, cntRad2 = double.NaN;
                E = E / ARANFunctions.DegToRad ( 1 );
                Point tangentPnt1, tangentPnt2;
                Shablons.TouchTo2Spiral ( pntCntBOnC3, Rv + 5 * w_, E, angleForC3, pntCntBOnC7, Rv + 5 * w_, E, angleForC7, side, ref cntRad1, ref cntRad2, out tangentPnt1, out tangentPnt2);

                Polygon polygonCmnTangent = new Polygon ();
                Ring ringOnC3 = new Ring ();
                ringOnC3.Add ( designatedPointPrj );
                ringOnC3.Add ( ( tangentPnt1.Clone () as Point ) );
                ringOnC3.Add ( ( tangentPnt2.Clone () as Point ) );
                polygonCmnTangent.AddRing ( ringOnC3 );

                Point pntCntBonC8 = TransForm.Move ( pntCntBOnC7, pntC7, pntC8 ).AsPoint;
                //InitHolding.UI.DrawPointWithText ( pntCntBonC8, 0, "Cnt C8 2" );

                Point pntCntBonC6 = TransForm.Move ( pntCntBOnC3, _pntC3, _pntC6 ).AsPoint;
                //InitHolding.UI.DrawPointWithText ( pntCntBonC6, 0, "Cnt C6 2" );

                Shablons.TouchTo2Spiral ( pntCntBonC8, Rv+5*w_, E, angleForC7, pntCntBonC6, Rv + 5*w_, E, angleForC3, side, ref cntRad1, ref cntRad2, out tangentPnt1, out tangentPnt2);

                Ring ringOnC6 = new Ring ();
                ringOnC6.Add ( designatedPointPrj );
                ringOnC6.Add ( ( tangentPnt1.Clone () as Point ) );
                ringOnC6.Add ( ( tangentPnt2.Clone () as Point ) );
                polygonCmnTangent.AddRing ( ringOnC6 );

                polygonCmnTangent = GlobalParams.GeomOperators.Difference ( polygonCmnTangent, _basicArea ).AsPolygon;
                //InitHolding.UI.DrawPolygon ( polygonCmnTangent, 255, eFillStyle.sfsCross );
                #endregion
                
                double angleC7 = ARANFunctions.ReturnAngleAsRadian ( dmePntPrj, pntC7 );
                double angle = ARANMath.SubtractAngles ( angleC7, ARANFunctions.ReturnAngleAsRadian ( dmePntPrj, pntC8 ) );
                int count = ( int ) Math.Floor ( ARANFunctions.RadToDeg ( angle ) );
                int sideInInt = ( int ) side;

                result = GlobalParams.GeomOperators.Difference ( ARANFunctions.CreatePolygonFromRings ( ARANFunctions.PartToRing ( tmpTurn180 ) ), _basicArea ).AsPolygon;

                Point tmp = new Point ();                
                for ( int i = 1; i<=count; i++ )
                {
                    tmp.Assign ( ARANFunctions.LocalToPrj ( dmePntPrj, angleC7 - sideInInt*ARANFunctions.DegToRad ( i ), _dl2, 0 ) );
                    result = GlobalParams.GeomOperators.UnionGeometry ( result, ARANFunctions.CreatePolygonFromRings ( ARANFunctions.PartToRing ( TransForm.Move ( tmpTurn180, pntC7, tmp ).AsPart ) ) ).AsPolygon;
                }
                result = GlobalParams.GeomOperators.UnionGeometry ( result, ARANFunctions.CreatePolygonFromRings ( ARANFunctions.PartToRing ( TransForm.Move ( tmpTurn180, pntC7, pntC8 ).AsPart ) ) ).AsPolygon;
                result = GlobalParams.GeomOperators.Difference ( result, _basicArea ).AsPolygon;
                result = GlobalParams.GeomOperators.UnionGeometry ( result, polygonCmnTangent ).AsPolygon;
                return result;
            }
            
            return result;
        }

        public  Polygon AwayConstructRecipDirectEntryToSecondaryPnt (double direction, double radius, Point designatedPntPrj, Point vorPntPrj, double legLength, SideDirection side, out int angle )
        {
            Point secPnt = ARANFunctions.LocalToPrj (designatedPntPrj,direction, - legLength, (int) side * 2 * radius);
            GlobalParams.UI.DrawPointWithText ( secPnt, 0, "Sec Pnt" );
            angle = ( int ) ARANFunctions.RadToDeg ( ARANMath.SubtractAngles ( direction, ARANFunctions.ReturnAngleAsRadian ( vorPntPrj, secPnt ) ) );
            return null;
        }

        private Line GetStraightPartOfPolygon (SideDirection side, double direction, Point vorPntPrj, Polygon polygon )
        {
            Line result = new Line ();
            PolyLine tmpPolyline = new PolyLine ();
            Part tmpPart = new Part ();
            tmpPart.AddPoint ( vorPntPrj );
            tmpPart.AddPoint ( ARANFunctions.LocalToPrj ( vorPntPrj, direction, 100000000, 0 ) );
            tmpPolyline.AddPart ( tmpPart );
            Geometry[] Geom = new Geometry [2] { null,null};
            GlobalParams.GeomOperators.Cut ( polygon, tmpPolyline, ref Geom [0], ref Geom [1] );
            int firstIndex = 0;
            if ( side == SideDirection.sideRight )
            {                
                tmpPolyline = ARANFunctions.PolygonToPolyLine ( Geom [1].AsPolygon );
            }
            else
            {
                tmpPolyline = ARANFunctions.PolygonToPolyLine ( Geom [0].AsPolygon );
            }
            //InitHolding.ui.DrawPolyline ( tmpPolyline, 255, 2 );
            double dist;
            double max = 0, secMax = 0;
            int maxFirstIndex = 0;
            for ( int i = 0; i<= tmpPolyline [0].Count-2; i++ )
            {
                dist = ARANFunctions.ReturnDistanceAsMeter ( tmpPolyline [0].AsPart [i], tmpPolyline [0].AsPart [i+1] );
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
                result.RefPoint = tmpPolyline [0].AsPart [firstIndex];
                result.DirVector.direction = ARANFunctions.ReturnAngleAsRadian ( result.RefPoint, tmpPolyline [0].AsPart [firstIndex + 1] );
                result.DirVector.Length = 5 * secMax;
            }
            else
            {
                result.RefPoint = tmpPolyline [0].AsPart [firstIndex+1];
                result.DirVector.direction = ARANFunctions.ReturnAngleAsRadian ( result.RefPoint, tmpPolyline [0].AsPart [firstIndex] );
                result.DirVector.Length = 5 * secMax;
            }
            //InitHolding.ui.DrawPointWithText ( result.RefPoint, 0, "Ref" );
            //InitHolding.ui.DrawPointWithText ( tmpPolyline [0].AsPart [firstIndex + 1], 0, "2-nd" );
            return result;
        }

        #endregion

        public void SetWithLimitingRadial (bool isWithLimitingRadial)
        {
            _isWithLimitingRadial = isWithLimitingRadial;
        }

        private Point[] _toleranceArea;
        private double _trackingTolerance;
        
        private Polygon _shablon;
        private Part _line3, _turn180;
        private Polygon _basicArea;
        private double _dl2;
        private Point _pntC3, _pntC6;
        private bool _isWithLimitingRadial = false;

        private Navaids_DataBase _navaidsDB;
    }

    public class VORNDB
    {
        public VORNDB ( Navaids_DataBase navaidsDB )
        {
            _navaidsDB = navaidsDB;
        }

        private Point [] ConstructFixToleranceArea (NavaidPntPrj navPntPrj, double altitude, double direction, out double radius)
        {
            Point[] result = new Point [4];            
            if (navPntPrj.Type == NavType.NavType_Vor)
                radius = altitude * Math.Tan ( 50 * ARANMath.C_PI / 180 );
            else
                radius = altitude * Math.Tan ( 40 * ARANMath.C_PI / 180 );
            Part circleZV = ARANFunctions.CreateCircleAsPartPrj ( navPntPrj.Value, radius );
            if ( navPntPrj.Type == NavType.NavType_Vor )
            {
                double qV = radius * Math.Sin ( 5 * ARANMath.C_PI / 180 );
                result [1] = ARANFunctions.LocalToPrj ( navPntPrj.Value, direction, -radius * Math.Cos ( 5 * ARANMath.C_PI / 180 ), -qV );
                result [3] = ARANFunctions.LocalToPrj ( navPntPrj.Value, direction, -radius * Math.Cos ( 5 * ARANMath.C_PI / 180 ), qV );
                result [0] = ARANFunctions.CircleVectorIntersect ( navPntPrj.Value, radius, result [1], direction - ARANFunctions.DegToRad ( 5 ), 100000000 );
                result [2] = ARANFunctions.CircleVectorIntersect ( navPntPrj.Value, radius, result [3], direction + ARANFunctions.DegToRad ( 5 ), 100000000 );
            }
            else
            {
                double qV = radius * Math.Sin ( 15 * ARANMath.C_PI / 180 );
                result [1] = ARANFunctions.LocalToPrj ( navPntPrj.Value, direction, -radius * Math.Cos ( 15 * ARANMath.C_PI / 180 ), -qV );
                result [3] = ARANFunctions.LocalToPrj ( navPntPrj.Value, direction, -radius * Math.Cos ( 15 * ARANMath.C_PI / 180 ), qV );
                result [0] = ARANFunctions.CircleVectorIntersect ( navPntPrj.Value, radius, result [1], direction - ARANFunctions.DegToRad ( 5 ), 100000000 );
                result [2] = ARANFunctions.CircleVectorIntersect ( navPntPrj.Value, radius, result [3], direction + ARANFunctions.DegToRad ( 5 ), 100000000 );
            }
            return result;
        }

        public Polygon ConstructBasicArea (NavaidPntPrj navPntPrj, double altitude, double direction, double ias, SideDirection side, Time time)
        {
            Polygon result = new Polygon();
            double radius;
            _toleranceArea = ConstructFixToleranceArea ( navPntPrj, altitude, direction, out radius );
            for ( int i = 0; i <= 3; i++ )
                GlobalParams.UI.DrawPointWithText ( _toleranceArea [i], 0, i.ToString () );
            
            PolyLine nominalTrack;
            Polygon shablon;
            Part line3, turn180;
            Point pntE;

            Shablons.HoldingShablon ( navPntPrj.Value, ias, altitude, 15, time.ValueInMinutes, direction, side, 0, out nominalTrack, out shablon, out line3, out turn180, out pntE );

            GlobalParams.UI.DrawPolygon ( shablon, 255, eFillStyle.sfsCross );
            GlobalParams.UI.DrawPointWithText ( pntE, 0, "E" );
            GlobalParams.UI.DrawPolyline ( nominalTrack, 255, 1 );
            GlobalParams.UI.DrawPart ( turn180, 255, 1 );
            GlobalParams.UI.DrawPart ( line3, 255, 1 );
            return null;

            MultiPoint mltPnt = new MultiPoint ();

            Polygon tmpShablon = TransForm.Move ( shablon, navPntPrj.Value, _toleranceArea [2] ).AsPolygon;
            //GlobalParams.UI.DrawPolygon (tmpShablon, 0, eFillStyle.sfsHorizontal);
            foreach ( Ring rng in tmpShablon )
                mltPnt.AddMultiPoint ( rng );

            tmpShablon = TransForm.Move ( shablon, navPntPrj.Value, _toleranceArea [0] ).AsPolygon;
            //GlobalParams.UI.DrawPolygon ( tmpShablon, 0, eFillStyle.sfsHorizontal );
            foreach ( Ring rng in tmpShablon )
                mltPnt.AddMultiPoint ( rng );

            tmpShablon = TransForm.Move ( shablon, navPntPrj.Value, _toleranceArea [1] ).AsPolygon;
            //GlobalParams.UI.DrawPolygon ( tmpShablon, 0, eFillStyle.sfsHorizontal );
            foreach ( Ring rng in tmpShablon )
                mltPnt.AddMultiPoint ( rng );

            tmpShablon = TransForm.Move ( shablon, navPntPrj.Value, _toleranceArea [3] ).AsPolygon;
            //GlobalParams.UI.DrawPolygon ( tmpShablon, 0, eFillStyle.sfsHorizontal );
            foreach ( Ring rng in tmpShablon )
                mltPnt.AddMultiPoint ( rng );

            result = GlobalParams.GeomOperators.ConvexHull ( mltPnt ).AsPolygon;
            GlobalParams.UI.DrawPolygon ( result, 0, eFillStyle.sfsNull );

            Point tmpPntOnCircle;
            Polygon tmpPolygon = new Polygon ();
            for ( int i = 0; i<= 35; i++ )
            {
                tmpPntOnCircle = ARANFunctions.LocalToPrj ( navPntPrj.Value, ARANFunctions.DegToRad ( i * 10), radius, 0 );
                //GlobalParams.UI.DrawPolygon ( TransForm.Move ( shablon, pntE, _toleranceArea[0] ).AsPolygon, 0, eFillStyle.sfsVertical );
                //GlobalParams.UI.DrawPolygon ( TransForm.Move ( shablon, pntE, tmpPntOnCircle ).AsPolygon, 0, eFillStyle.sfsNull );
                tmpPolygon = GlobalParams.GeomOperators.UnionGeometry ( tmpPolygon, TransForm.Move ( shablon, pntE, tmpPntOnCircle ).AsPolygon ).AsPolygon;
            }
            GlobalParams.UI.DrawPolygon ( tmpPolygon, 0, eFillStyle.sfsCross );
            return null;//result;
        }

        private Navaids_DataBase _navaidsDB;
        private Point[] _toleranceArea;
    }
}