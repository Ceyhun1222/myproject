using System;
using ARAN.GeometryClasses;
using System.Windows.Forms;
using System.Collections.Generic;
using ARAN.Contracts.UI;
using ARAN.Contracts.GeometryOperators;
using ARAN.Common;
using ARAN;

namespace Holding
{
	public static class Shablons
	{
		public  const double degToRadValue  = Math.PI / 180;
		public const  double degEps = 1.0 / 36000.0;
		public const double radEps = degEps * degToRadValue;
        public const double distEps  = 0.0001;
		public static  double guidanceTolerance = ARANFunctions.DegToRad(5);
		public  static  double sinGuidanceTolerance = Math.Sin(guidanceTolerance);
		public  static  double cosGuidanceTolerance = Math.Cos(guidanceTolerance);
		
		//public static int HoldingShablon(ref Point ptNavPrj,double IAS,double AbsH,double dISA, double T,double Axis, SideDirection Turn, int NavType, ref MultiPoint HoldingArea, ref Ring Shablon, ref MultiPoint Line3, ref Part Turn180)
		//{
			
		//    double V;
		//    // 2
		//    double v3600;
		//    // 3		
		//    double R;
		//    // 4
		//    double Rv;
		//    // 5
		//    double H;
		//    // 6
		//    double w;
		//    // 7
		//    double w_;
		//    // 8
		//    double E45;
		//    // 9
		//    double E;
		//    // 9
		//    double t_;
		//    //12
		//    double L;
		//    //13
		//    double ab;
		//    //14
		//    double ac;
		//    //14_
		//    double gi1;
		//    //15
		//    double gi2;
		//    //15_
		//    double xe;
		//    //15_
		//    double ye;
		//    //20
		//    int HoldingShablon;
		//    Point PtB, PtC, PtCntB, PtCntC, PtI1,
		//                PtI3,PtI4,PtCntI2,PtCntI4,PtCntI1,
		//                PtCntI3, PtI2, PtN3, PtN4_, ptTmp,PtG;
		//    double fTmp,RadCSt = 0,TurnAng,RadI1St = 0,RadI2St = 0,
		//            RadI3St = 0,RadI4St = 0,RadN4St = 0,RadI1End = 0;
		//    double RadI2End = 0,RadI3End = 0,RadI4End = 0,
		//            RadN4End = 0;

		//    bool bN3flg;
		//    Point PtSt;

		//    ARAN.Contracts.UI.UIContract ui = new ARAN.Contracts.UI.UIContract();

		//    //Set Shablon = New MultiPoint
		//    Shablon = new Ring();
		//    HoldingArea = new MultiPoint();
		//    Line3 = new MultiPoint();
		//    Turn180 = new Part();
		//    HoldingShablon = 0;

		//    V = ARANMath.IASToTAS(IAS, AbsH, dISA);
		//    v3600 = 0.277777777777778 * V;
		//    R = 943.27 / V;
		//    if ((R > 3.0))
		//        R = 3.0;

		//    Rv = 1000.0 * V / (62.83 * R);
		//    H = AbsH / 1000.0;
		//    w = 12.0 * H + 87.0;

		//    w_ = 0.277777777777778 * w;

		//    E = w_ / R;
		//    E45 = 45.0 * E;
		//    t_ = 60.0 * T;

		//    L = v3600 * t_;
		//    ab = 5.0 * v3600;
		//    ac = 11.0 * v3600;
		//    gi1 = (t_ - 5.0) * v3600;
		//    gi2 = (t_ + 21.0) * v3600;
			
			


		//    xe = 2.0 * Rv + (t_ + 15.0) * v3600 + (26.0 + 195.0 / R + t_) * w_;
		//    ye = 11.0 * v3600 * System.Math.Cos(ARANFunctions.DegToRad(20.0)) + Rv * System.Math.Sin(ARANFunctions.DegToRad(20.0)) + Rv + (t_ + 15.0) * v3600 * System.Math.Tan(ARANFunctions.DegToRad(5.0)) + (21.0 + 125.0 / R + t_) * w_;

		//    //Конец вычисления требуемых параметров
		//    //Вычисление центров спиралей

		//    //PtB = ARANFunctions.PointAlongPlane(ptNavPrj, Axis +  Math.PI, ab);
		//    //PtC = ARANFunctions.PointAlongPlane(ptNavPrj, Axis +  Math.PI, ac);
		//    //PtCntB = ARANFunctions.PointAlongPlane(PtB, Axis - Math.PI/2 * (int)Turn, Rv);
		//    //PtCntC = ARANFunctions.PointAlongPlane(PtC, Axis - Math.PI/2 * (int)Turn, Rv);
		//    //PtG = ARANFunctions.PointAlongPlane(PtC, Axis - Math.PI/2 * (int)Turn, 2.0 * Rv);
		//    PtCntB = ARANFunctions.LocalToPrj(ptNavPrj, Axis, -ab, -Rv * (int)Turn);
		//    PtCntC = ARANFunctions.LocalToPrj(ptNavPrj, Axis, -ac, -Rv * (int)Turn);
		//    PtG = ARANFunctions.LocalToPrj(ptNavPrj, Axis, -ac,-Rv * 2 * (int)Turn);
		//    ui.DrawPointWithText(PtCntB,2, "PtCntB");
		//    ui.DrawPointWithText(PtCntC,2,"PtCntC");
		//    ui.DrawPointWithText(PtG, 2, "PtG");
		//    //PtI1 = ARANFunctions.PointAlongPlane(PtG, Axis - ARANFunctions.DegToRad(5) * (int)Turn, gi1);
		//    //PtI2 = ARANFunctions.PointAlongPlane(PtG, Axis - ARANFunctions.DegToRad(5) * (int)Turn, gi2);
		//    //PtI3 = ARANFunctions.PointAlongPlane(PtG, Axis + ARANFunctions.DegToRad(5) * (int)Turn, gi1);
		//    //PtI4 = ARANFunctions.PointAlongPlane(PtG, Axis + ARANFunctions.DegToRad(5) * (int)Turn, gi2);
		//    //PtCntI1 = ARANFunctions.PointAlongPlane(PtI1, Axis + Math.PI/2 * (int)Turn, Rv);
		//    //PtCntI2 = ARANFunctions.PointAlongPlane(PtI2, Axis + Math.PI/2 * (int)Turn, Rv);
		//    //PtCntI3 = ARANFunctions.PointAlongPlane(PtI3, Axis + Math.PI/2 * (int)Turn, Rv);
		//    //PtCntI4 = ARANFunctions.PointAlongPlane(PtI4, Axis + Math.PI/2 * (int)Turn, Rv);

		//    PtI1 = ARANFunctions.LocalToPrj(PtG, Axis, gi1 * cosGuidanceTolerance, -gi1 * sinGuidanceTolerance * (int)Turn);
		//    PtI2 = ARANFunctions.LocalToPrj(PtG, Axis, gi2 * cosGuidanceTolerance, -gi2 * sinGuidanceTolerance * (int)Turn);
		//    PtI3 = ARANFunctions.LocalToPrj(PtG, Axis, gi1 * cosGuidanceTolerance, gi1 * sinGuidanceTolerance  *(int)Turn);
		//    PtI4 = ARANFunctions.LocalToPrj(PtG, Axis, gi2 * cosGuidanceTolerance, gi2 * sinGuidanceTolerance * (int)Turn);

		//    PtCntI1 = ARANFunctions.LocalToPrj(PtI1, Axis,0,(int)Turn * Rv);
		//    PtCntI2 = ARANFunctions.LocalToPrj(PtI2, Axis,0, (int)Turn *Rv);
		//    PtCntI3 = ARANFunctions.LocalToPrj(PtI3, Axis, 0, (int)Turn * Rv);
		//    PtCntI4 = ARANFunctions.LocalToPrj(PtI4, Axis, 0, (int)Turn * Rv);

		//    PtN3 = ARANFunctions.LocalToPrj(PtG, Axis, 0,(int)Turn *2 *Rv);

		//    ui.DrawPointWithText(PtI1, 2, "PtI1");
		//    ui.DrawPointWithText(PtI2, 2, "PtI2");
		//    ui.DrawPointWithText(PtI3, 2, "PtI3");
		//    ui.DrawPointWithText(PtI4, 2, "PtI4");
		//    ui.DrawPointWithText(PtCntI1, 2, "PtCntI1");
		//    ui.DrawPointWithText(PtCntI2, 2, "PtCntI2");
		//    ui.DrawPointWithText(PtCntI3, 2, "PtCntI3");
		//    ui.DrawPointWithText(PtCntI4, 2, "PtCntI4");
		//    //PtN3 = ARANFunctions.PointAlongPlane(PtI3, Axis + Math.PI/2 * (int)Turn, 2.0 * Rv);
		//    //Конец вычисления центров спиралей

		//    //===========HoldingArea Nom Track
		//    HoldingArea.AddPoint(ptNavPrj);
		//    //PtC
		//    HoldingArea[0].M = Axis + Math.PI;//change

		//    //HoldingArea.AddPoint(ARANFunctions.PointAlongPlane(ptNavPrj, Axis +  Math.PI.0 + Math.PI/2 * (int)Turn, 2.0 * Rv));
		//    HoldingArea.AddPoint(ARANFunctions.LocalToPrj(ptNavPrj, Axis,0,-(int)Turn * 2.0 * Rv));
			
		//    //PtG '
		//    HoldingArea[1].M = Axis;

		//    //HoldingArea.AddPoint(ARANFunctions.PointAlongPlane(HoldingArea[1], Axis, L));
		//    HoldingArea.AddPoint(ARANFunctions.LocalToPrj(HoldingArea[1], Axis, L, 0));
		//    HoldingArea[2].M = Axis;

		//    //HoldingArea.AddPoint(ARANFunctions.PointAlongPlane(HoldingArea[2], Axis + Math.PI/2 * (int)Turn, 2.0 * Rv));
		//    HoldingArea.AddPoint(ARANFunctions.LocalToPrj(HoldingArea[2], Axis, 0, (int)Turn * 2 * Rv));
		//    HoldingArea[3].M = Axis + Math.PI;

		//    HoldingArea.AddPoint(HoldingArea[0]);
		//    ui.DrawPoint(HoldingArea[0],255,ARAN.Contracts.UI.ePointStyle.smsCircle);
		//    ui.DrawPoint(HoldingArea[1], 255, ARAN.Contracts.UI.ePointStyle.smsCircle);
		//    ui.DrawPoint(HoldingArea[2], 255, ARAN.Contracts.UI.ePointStyle.smsCircle);
		//    ui.DrawPoint(HoldingArea[3], 255, ARAN.Contracts.UI.ePointStyle.smsCircle);
		//    //return 0;			
					 
		//    //Коней вычисления центров спиралей
		//    //===========Параметры спиралей
		//    double R0C;
		//    double Rad0C;
		//    double R0I1;
		//    double Rad0I1;
		//    double R0I2;
		//    double Rad0I2;
		//    double R0I3;
		//    double Rad0I3;
		//    double R0I4;
		//    double Rad0I4;
		//    double RN3;
		//    double R0N4;

		//    R0C = Rv + 11.0 * w_;
		//    //Rad0C = Axis + Math.PI/2 * (int)Turn;
		//    Rad0C = Axis + Math.PI / 2 * (int)Turn;
		//    R0I1 = Rv + (t_ + 6.0) * w_ + 4.0 * E45;
		//    //Rad0I1 = Axis - Math.PI/2 * (int)Turn;
		//    Rad0I1 = Axis - Math.PI / 2 * (int)Turn;
		//    R0I2 = R0I1 + 14.0 * w_;
		//    Rad0I2 = Rad0I1;
		//    R0I3 = R0I1;
		//    Rad0I3 = Rad0I1;
		//    R0I4 = R0I2;
		//    Rad0I4 = Rad0I1;
		//    RN3 = (t_ + 6.0) * w_ + 8.0 * E45;
		//    //RadN3 = Axis + Math.PI/2 * (int)Turn
		//    //================= Line3 ===========================
		//    int Solution;
		//    double Rad1Tmp = 0;
		//    double Rad2Tmp = 0;
		//    double R1Tmp = 0;
		//    double R2Tmp = 0;

		//    int I;
		//    double CntRad1 = 0.0;
		//    double CntRad2 = 0.0;
		//    int TouchRes;

		//    TouchRes = TouchTo2Spiral(PtG, R0C - Rv, 0.0, 0.0, PtI3, R0I3 - Rv, 0.0, 0.0, -(Turn), ref CntRad1,
		//    ref CntRad2);

		//    if (TouchRes > 0)
		//    {
		//        Line3.AddPoint(ARANFunctions.LocalToPrj(PtG, CntRad1, R0C - Rv,0));
		//        Line3.AddPoint(ARANFunctions.LocalToPrj(PtI3, CntRad2, R0I3 - Rv,0));
		//        Line3[0].M = ARANFunctions.ReturnAngleAsDegree(Line3[0], Line3[1]);
		//        Line3[1].M = Line3[0].M;
		//    }

		//    TouchRes = TouchTo2Spiral(PtI3, R0I3 - Rv, 0.0, 0.0, PtI4, R0I4 - Rv, 0.0, 0.0, -(Turn), ref CntRad1,
		//    ref CntRad2);
		//    if (TouchRes > 0)
		//    {
		//        I = Line3.Count;
		//        Line3.AddPoint(ARANFunctions.LocalToPrj(PtI3, CntRad1, R0I3 - Rv,0));
		//        Line3.AddPoint(ARANFunctions.LocalToPrj(PtI4, CntRad2, R0I4 - Rv,0));
		//        Line3[I].M = ARANFunctions.ReturnAngleAsDegree(Line3[I], Line3[I + 1]);
		//        Line3[I + 1].M = Line3[I].M;
		//    }

		//    //Line3.AddPoint(ARANFunctions.PointAlongPlane(PtI4, Axis +  Math.PI.0 - Math.PI/2 * (int)Turn, R0I4 - Rv));
		//    Line3.AddPoint(ARANFunctions.LocalToPrj(PtI4, Axis, 0, -(int)Turn * (R0I4 - Rv)));
		//    Line3[Line3.Count - 1].M = Axis + Math.PI;
		//    //=====================================Turn180
		//    double R0B = 0;
		//    double Rad0B = 0;
		//    double RadBEnd = 0;
		//    double RadBstr = 0;
		//    double RadCend = 0;
		//    double TrackToler = 0;
		//    double TouchRad = 0;

		//    //R0B = Rv + 5 * w_ + 4.0 * E45;
		//    R0B = Rv + 5 * w_ + 4.0 * E45;
		//    Rad0B = ARANMath.Modulus(Rad0C + Math.PI, 2 * Math.PI);

		//    Rad1Tmp = ARANFunctions.ReturnAngleAsRadian(PtCntB, PtCntC);
		//    Rad2Tmp = Rad1Tmp;

		//    ChangeSpiralStartParam(E, R0C, Rad0C, ref R1Tmp, ref Rad1Tmp, Turn, Turn);
		//    ChangeSpiralStartParam(E, R0B, Rad0B, ref R2Tmp, ref Rad2Tmp, Turn, -(Turn));
		//    Solution = TouchTo2Spiral(PtCntC, R1Tmp, E, Rad1Tmp, PtCntB, R2Tmp, E, Rad2Tmp, Turn, ref RadCend,
		//    ref RadBstr);
		//    //Solution = TouchTo2Spiral(PtCntC, R1Tmp, E, Rad1Tmp, PtCntB, R2Tmp, E, Rad2Tmp, (int)Turn, RadCend, RadBstr)

		//    if (Solution == 0)
		//    {
		//        MessageBox.Show("ERROR ON 'HoldingShablon'");
		//        //    msgbox "Параметры шаблона ипподрома неправильны"
		//        return HoldingShablon;
		//    }

		//    Turn180.Add(ptNavPrj);

		//    //R1Tmp = R0C + E * ARANMath.Modulus((int)Turn * (RadCend - Rad0C), 2 *Math.PI);
		//    //Rad1Tmp = ARANMath.Modulus(RadCend + (int)Turn * (Math.PI/2 - System.Math.Atan(ARANFunctions.RadToDeg(E) / R1Tmp)), 2 *Math.PI);
						  
		//    //if (NavType == 2)
		//    //{
		//    //    TrackToler = InitHolding.Navaid_Database.NDB.TrackingTolerance;
		//    //}
		//    //else
		//    //{
		//    //    TrackToler = InitHolding.Navaid_Database.VOR.TrackingTolerance;
		//    //}
		//    //TouchRad = ARANMath.Modulus(Axis + (int)Turn * TrackToler, 2 *Math.PI);

		//    ////If AnglesSideDef(TouchRad, Rad1Tmp) = (int)Turn Then
		//    //if (ARANFunctions.AnglesSideDef(Rad1Tmp, TouchRad) == (int)Turn)
		//    //{
		//    //    Rad2Tmp = ARANMath.Modulus(Rad0C + Math.PI/2 * (int)Turn, 2 *Math.PI);
		//    //    RadBEnd = ARANFunctions.SpiralTouchAngle(R0C, E, Rad2Tmp, TouchRad, Turn);
		//    //    RadBEnd = ARANMath.Modulus(Rad0C + RadBEnd * (int)Turn, 2 *Math.PI);
		//    //    CreateSpiralBy2Radial(PtCntC, R0C, Rad0C, RadBEnd, E, Turn, Turn180);
		//    //}
		//    //else
		//    //{
		//    //    Rad2Tmp = ARANMath.Modulus(Rad0B + Math.PI/2 * (int)Turn, 2 *Math.PI);
		//    //    RadBEnd = ARANFunctions.SpiralTouchAngle(R0B, E, Rad2Tmp, TouchRad, Turn);

		//    //    RadBEnd = ARANMath.Modulus(Rad0B + RadBEnd * (int)Turn, 2 * Math.PI);
		//    //    CreateSpiralBy2Radial(PtCntC, R0C, Rad0C, RadCend, E, Turn, Turn180);
		//    //    ui.DrawPart(Turn180, 255, 2);


		//    //    if (ARANFunctions.AnglesSideDef(Rad0B, RadBstr) == (int)Turn)
		//    //    {
		//    //        R0B = R0B - E * ARANMath.SubtractAngles(Rad0B, RadBstr);
		//    //    }
		//    //    else
		//    //    {
		//    //        R0B = R0B + E * ARANMath.SubtractAngles(Rad0B, RadBstr);
		//    //    }
		//    //    //CreateSpiralBy2Radial(PtCntB, R0B, RadBstr, RadBEnd, E, Turn,Turn180);

		//    //}

		//    CreateSpiralBy2Radial(PtCntB, Rv+5*w_, Axis +(int)Turn * Math.PI * 0.5, Axis, E, Turn, Turn180);
		//    ui.DrawPart(Turn180, 255, 2);
		//    //return 0;
		//    //===================================================
			
		//    Rad1Tmp = ARANFunctions.ReturnAngleAsRadian(PtCntI1, PtCntC) - Math.PI * (45.0 /180.0) * (int)Turn;
		//    ChangeSpiralStartParam(E, R0I1, Rad0I1, ref R1Tmp, ref Rad1Tmp, Turn, -(Turn));
		//    Solution = TouchTo2Spiral(PtCntC, R0C, E, Rad0C, PtCntI1, R1Tmp, E, Rad1Tmp, Turn, ref RadCend,
		//    ref RadI1St);
		//    TurnAng = ARANMath.SubtractAnglesWithSign(Rad0C, RadCSt, Turn);
		//    R1Tmp = R0C + E * TurnAng;
		//    PtSt = ARANFunctions.PointAlongPlane(PtCntC, RadCSt, R1Tmp);
		//    CreateSpiralBy2Radial(PtCntC, R1Tmp, RadCSt, RadCend, E, Turn, Shablon);

		//    //DrawPolyLine Turn180, 0, 2
		//    if (Solution == 0)
		//    {
		//        MessageBox.Show("ERROR ON 'HoldingShablon'");
		//        //    msgbox "Параметры шаблона ипподрома неправильны"
		//        return HoldingShablon;
		//    }

		//    Rad2Tmp = ARANFunctions.ReturnAngleAsRadian(PtCntI2, PtCntI1);
		//    ChangeSpiralStartParam(E, R0I2, Rad0I2, ref R2Tmp, ref Rad2Tmp, Turn, -(Turn));
		//    Solution = TouchTo2Spiral(PtCntI1, R1Tmp, E, Rad1Tmp, PtCntI2, R2Tmp, E, Rad2Tmp, Turn, ref RadI1End,
		//    ref RadI2St);
		//    if (Solution == 0)
		//    {
		//        MessageBox.Show("ERROR ON 'HoldingShablon'");
		//        //    msgbox "Параметры шаблона ипподрома неправильны"
		//        return HoldingShablon;
		//    }

		//    Rad1Tmp = ARANFunctions.ReturnAngleAsRadian(PtCntI4, PtCntI2);
		//    Rad2Tmp = Rad1Tmp;
		//    ChangeSpiralStartParam(E, R0I2, Rad0I2, ref R1Tmp, ref Rad1Tmp, Turn, -(Turn));
		//    ChangeSpiralStartParam(E, R0I4, Rad0I4, ref R2Tmp, ref  Rad2Tmp, Turn, -(Turn));
		//    Solution = TouchTo2Spiral(PtCntI2, R1Tmp, E, Rad1Tmp, PtCntI4, R2Tmp, E, Rad2Tmp, Turn, ref RadI2End,
		//    ref RadI4St);
		//    if (Solution == 0)
		//    {
		//        MessageBox.Show("ERROR ON 'HoldingShablon'");
		//        //    msgbox "Параметры шаблона ипподрома неправильны"
		//        return HoldingShablon;
		//    }

		//    Rad1Tmp = ARANFunctions.ReturnAngleAsDegree(PtCntI3, PtCntI4);
		//    Rad2Tmp = Rad1Tmp;
		//    ChangeSpiralStartParam(E, R0I4, Rad0I4, ref R1Tmp, ref Rad1Tmp, Turn, -(Turn));
		//    ChangeSpiralStartParam(E, R0I3, Rad0I3, ref R2Tmp, ref Rad2Tmp, Turn, -(Turn));
		//    Solution = TouchTo2Spiral(PtCntI4, R1Tmp, E, Rad1Tmp, PtCntI3, R2Tmp, E, Rad2Tmp, Turn, ref RadI4End,
		//    ref RadI3St);
		//    if (Solution == 0)
		//    {
		//        MessageBox.Show("ERROR ON 'HoldingShablon'");
		//        //    msgbox "Параметры шаблона ипподрома неправильны"
		//        return HoldingShablon;
		//    }

		//    TurnAng = ARANMath.Modulus((RadI4End - Rad0I4) * (int)Turn, 2 * Math.PI);
		//    R1Tmp = R0I4 + E * TurnAng;
		//    ptTmp = ARANFunctions.PointAlongPlane(PtCntI4, RadI4End, R1Tmp);
		//    fTmp = System.Math.Atan(E / R1Tmp);
		//    Rad1Tmp = RadI4End + ( Math.PI - fTmp) * (int)Turn;
		//    PtN4_ = ARANFunctions.LocalToPrj(ptTmp, Rad1Tmp, R1Tmp - Rv,0);
		//    RadN4St = ARANMath.Modulus(Rad1Tmp +  Math.PI, 2 * Math.PI);

		//    R0N4 = R1Tmp - Rv;

		//    Solution = TouchTo2Spiral(PtN4_, R0N4, 0.0, RadN4St, PtN3, RN3, 0.0, Rad2Tmp, Turn, ref RadN4End,
		//    ref RadI3St);

		//    bN3flg = Solution != 0;

		//    if (!bN3flg)
		//    {
		//        PtN3 = PtN4_;
		//        RN3 = R0N4;
		//    }

		//    //DrawPoint PtN4_, 0
		//    //DrawPoint PtN3, 255
		//    //DrawPolygon CreatePrjCircle(PtN4_, R0N4), 0
		//    //DrawPolygon CreatePrjCircle(PtN3, RN3), 255

		//    Rad2Tmp = ARANFunctions.ReturnAngleAsRadian(PtCntC, PtN3);
		//    ChangeSpiralStartParam(E, R0C, Rad0C, ref R2Tmp, ref Rad2Tmp, Turn, -(Turn));
		//    Solution = TouchTo2Spiral(PtN3, RN3, 0.0, Rad1Tmp, PtCntC, R2Tmp, E, Rad2Tmp, Turn, ref RadI3End,
		//    ref RadCSt);

		//    if (Solution == 0)
		//    {
		//        MessageBox.Show("ERROR ON 'HoldingShablon'");
		//        //    msgbox "Параметры шаблона ипподрома неправильны"
		//        return HoldingShablon;
		//    }

		//    if (!bN3flg)
		//    {
		//        RadN4End = RadI3End;
		//    }

		//    if (ARANFunctions.AnglesSideDef(RadI3St, RadI3End) == (int)Turn)
		//    {
		//        RadI3St = RadI3End;
		//    }

		//    TurnAng = ARANMath.SubtractAnglesWithSign(Rad0C, RadCSt, Turn);
		//    R1Tmp = R0C + E * TurnAng;
		//    PtSt = ARANFunctions.PointAlongPlane(PtCntC, RadCSt, R1Tmp);
		//    CreateSpiralBy2Radial(PtCntC, R1Tmp, RadCSt, RadCend, E, Turn, Shablon);

		//    TurnAng = ARANMath.SubtractAnglesWithSign(Rad0I1, RadI1St, Turn);
		//    R1Tmp = R0I1 + E * TurnAng;
		//    CreateSpiralBy2Radial(PtCntI1, R1Tmp, RadI1St, RadI1End, E, Turn, Shablon);

		//    TurnAng = ARANMath.SubtractAnglesWithSign(Rad0I2, RadI2St, Turn);
		//    R1Tmp = R0I2 + E * TurnAng;
		//    CreateSpiralBy2Radial(PtCntI2, R1Tmp, RadI2St, RadI2End, E, Turn, Shablon);

		//    TurnAng = ARANMath.SubtractAnglesWithSign(Rad0I4, RadI4St, Turn);
		//    R1Tmp = R0I4 + E * TurnAng;
		//    CreateSpiralBy2Radial(PtCntI4, R1Tmp, RadI4St, RadI4End, E, Turn, Shablon);

		//    CreateSpiralBy2Radial(PtN4_, R0N4, RadN4St, RadN4End, 0.0, Turn, Shablon);
		//    //DrawPolygon Shablon, 255

		//    if (bN3flg)
		//    {
		//        CreateSpiralBy2Radial(PtN3, RN3, RadI3St, RadI3End, 0.0, Turn,Shablon);
		//    }

		//    Shablon.AddPoint(PtSt);

		//    Point ptE;
			
		//    Shablon = (Ring)TransForm.RotateGeometry(ptNavPrj,Shablon, -Axis);
		//    GeoMinMaxPoint minMaxPoint = GeomFunctions.QueryCoords(Shablon);
		//    ptE = new Point();
																		
		//    if ((int)Turn < 0)
		//    {
		//        ptE.X = minMaxPoint.XMax - xe;
		//        ptE.Y = minMaxPoint.YMin + ye;
		//    }
		//    else
		//    {
		//        ptE.X = minMaxPoint.XMax - xe;
		//        ptE.Y = minMaxPoint.YMax - ye;
		//    }

		//    Shablon.AddPoint(ptNavPrj);
		//    Shablon.AddPoint(ptE);

		//    Shablon.AddPoint(ptNavPrj);
		//    Shablon.AddPoint(ptE);

		//    Shablon = (Ring)TransForm.RotateGeometry(ptNavPrj,Shablon, Axis);

		//    HoldingShablon = 1;

		//    return HoldingShablon;
		//}
		
		public static int HoldingShablon(Point wayPoint, double ias, double altitude, double dIsa, double time, double axis, SideDirection side, int navType, out PolyLine inboundTrack, out Polygon shablon, out Part line3, out Part turn180,out Point ptE)
		{

			double V, v3600,R,Rv,H,w, w_,E45,E,t_, L,
				 ab,ac,gi1,gi2,xe,ye;

			double Wb, Wc, Wd, We, Wf, Wg, Wh, Wo, Wp, Wi1,
				Wi3, Wi2, Wi4, Wj, Wk, Wl, Wm, Wn3, Wn4;

			double tmpD, tmpDirection, alpha;

			Point  PtCntB, PtCntC, PtI1,
						PtI3, PtI4, PtCntI2, PtCntI4, PtCntI1,
						PtCntI3, PtI2, PtG;
			

			Point PtJ,PtK,PtL,PtM,PtN3,PtN4;
 			Point tmpFromPoint, tmpToPoint;
			//MultiPoint intersectArcN3Turn180;
			Ring ArcI1, ArcI2, ArcJ, Arck, ArcL, ArcM, ArcN3, ArcN4;
			PolyLine Arc3N,spiralA = null;
			
			
			line3 = new Part();
			turn180 = new Part();

			V = ARANMath.IASToTAS(ias, altitude, dIsa);
			v3600 =  V;
			R = 943.27 * 0.277777777777778 / V;
			if ((R > 3.0))
				R = 3.0;

			Rv = 1000.0 *3.6* V / (62.83 * R);
			H = altitude / 1000.0;
			w = 12.0 * H + 87.0;

			w_ = 0.277777777777778 * w;

			E = w_ / R;
			E45 = 45.0 * E;
			t_ = 60.0 * time;

			L = v3600 * t_;
			ab = 5.0 * v3600;
			ac = 11.0 * v3600;
			gi1 = (t_ - 5.0) * v3600;
			gi2 = (t_ + 21.0) * v3600;

			Wb = 5 * w_;
			Wc = 11 * w_;
			Wd = Wc + E45;
			We = Wc + 2 * E45;
			Wf = Wc + 3 * E45;
			Wg = Wc + 4 * E45;
			Wh = Wb + 4 * E45;
			Wo = Wb + 5 * E45;
			Wp = Wb + 6 * E45;
			Wi1 = (t_ + 6) * w_ + 4 * E45;
			Wi3 = Wi1;
			Wi2 = Wi1 + 14 * w_;
			Wi4 = Wi2;
			Wj = Wi2 + E45;
			Wk = Wi2 + 2 * E45;
			Wl = Wk;
			Wm = Wi2 + 3 * E45;
			Wn3 = Wi1 + 4 * E45;
			Wn4 = Wi2 + 4 * E45;

			xe = 2.0 * Rv + (t_ + 15.0) * v3600 + (26.0 + 195.0 / R + t_) * w_;
			ye = 11.0 * v3600 * System.Math.Cos(ARANFunctions.DegToRad(20.0)) + Rv * System.Math.Sin(ARANFunctions.DegToRad(20.0)) + Rv + (t_ + 15.0) * v3600 * System.Math.Tan(ARANFunctions.DegToRad(5.0)) + (21.0 + 125.0 / R + t_) * w_;
						
			PtCntB = ARANFunctions.LocalToPrj(wayPoint, axis, -ab, -Rv * (int)side);
			PtCntC = ARANFunctions.LocalToPrj(wayPoint, axis, -ac, -Rv * (int)side);
			PtG = ARANFunctions.LocalToPrj(wayPoint, axis, -ac, -Rv * 2 * (int)side);
			
			PtI1 = ARANFunctions.LocalToPrj(PtG, axis, gi1 * cosGuidanceTolerance, -gi1 * sinGuidanceTolerance * (int)side);
			PtI2 = ARANFunctions.LocalToPrj(PtG, axis, gi2 * cosGuidanceTolerance, -gi2 * sinGuidanceTolerance * (int)side);
			PtI3 = ARANFunctions.LocalToPrj(PtG, axis, gi1 * cosGuidanceTolerance, gi1 * sinGuidanceTolerance * (int)side);
			PtI4 = ARANFunctions.LocalToPrj(PtG, axis, gi2 * cosGuidanceTolerance, gi2 * sinGuidanceTolerance * (int)side);

			PtCntI1 = ARANFunctions.LocalToPrj(PtI1, axis, 0, (int)side * Rv);
			PtCntI2 = ARANFunctions.LocalToPrj(PtI2, axis, 0, (int)side * Rv);
			PtCntI3 = ARANFunctions.LocalToPrj(PtI3, axis, 0, (int)side * Rv);
			PtCntI4 = ARANFunctions.LocalToPrj(PtI4, axis, 0, (int)side * Rv);

			PtN3 = ARANFunctions.LocalToPrj(PtG, axis, 0, (int)side * 2 * Rv);

			PtJ = ARANFunctions.LocalToPrj(PtCntI2, axis,  Rv * Math.Cos(Math.PI * 0.25), -(int)side * Rv * Math.Sin(Math.PI * 0.25));
			PtK = ARANFunctions.LocalToPrj(PtCntI2,axis,Rv,0);
			PtL = ARANFunctions.LocalToPrj(PtCntI4, axis, Rv, 0);
			PtM = ARANFunctions.LocalToPrj(PtCntI4, axis, Rv * Math.Cos(Math.PI * 0.25), (int)side * Rv * Math.Sin(Math.PI * 0.25));
			PtN4 = ARANFunctions.LocalToPrj(PtCntI4, axis, 0, (int)side * Rv);
			PtN3 = ARANFunctions.LocalToPrj(PtCntI3, axis, 0, (int)side * Rv);
		
			//ui.DrawPolyline(ARANFunctions.CalcTrajectoryFromMultiPoint(HoldingArea), 255, 1);
			inboundTrack = CalculateHoldingArea(wayPoint, axis, (int) side, Rv, L);
			
			//Turn180 
			
			turn180.Add(wayPoint);
			CreateSpiralBy2Radial(PtCntB, Rv + 5 * w_, axis + (int)side * Math.PI * 0.5, axis, E / ARANFunctions.DegToRad(1), (int) side, turn180);
			//End Turn180

			tmpFromPoint = ARANFunctions.LocalToPrj(PtI1, axis, -Wi1, 0);
			tmpToPoint = ARANFunctions.LocalToPrj(PtI1, axis, Wi1, 0);
			ArcI1 = new Ring();
			ArcI1 = ARANFunctions.CreateArcPrj(PtI1, tmpFromPoint, tmpToPoint, ARANMath.ChangeDirection(side));

			tmpFromPoint = ARANFunctions.LocalToPrj(PtI2, axis, -Wi2, 0);
			tmpToPoint = ARANFunctions.LocalToPrj(PtI2, axis, Wi2, 0);
			ArcI2 = ARANFunctions.CreateArcPrj(PtI2, tmpFromPoint, tmpToPoint, ARANMath.ChangeDirection(side));

			tmpFromPoint =ARANFunctions.LocalToPrj(PtJ, axis,  -Wj * Math.Cos(Math.PI * 0.25),-(int)side * Wj * Math.Sin(Math.PI * 0.25)); //ARANFunctions.LocalToPrj(PtJ, Axis, , Wj);
			tmpToPoint =ARANFunctions.LocalToPrj(PtJ, axis, Wj * Math.Cos(Math.PI * 0.25),(int)side * Wj * Math.Sin(Math.PI * 0.25)); //ARANFunctions.LocalToPrj(PtJ, Axis, , Wj);
			ArcJ = ARANFunctions.CreateArcPrj(PtJ, tmpFromPoint, tmpToPoint, ARANMath.ChangeDirection(side));

			tmpFromPoint = ARANFunctions.LocalToPrj(PtK, axis, 0, -(int)side *Wk);
			tmpToPoint = ARANFunctions.LocalToPrj(PtK, axis, 0, (int)side * Wk);
			Arck = ARANFunctions.CreateArcPrj(PtK, tmpFromPoint, tmpToPoint, ARANMath.ChangeDirection(side));

			tmpFromPoint = ARANFunctions.LocalToPrj(PtL, axis, 0,-(int)side * Wl);
			tmpToPoint = ARANFunctions.LocalToPrj(PtL, axis, 0, (int)side * Wl);
			ArcL = ARANFunctions.CreateArcPrj(PtL, tmpFromPoint, tmpToPoint, ARANMath.ChangeDirection(side));

			tmpFromPoint = ARANFunctions.LocalToPrj(PtM, axis, Wm * Math.Cos(Math.PI * 0.25), -(int)side * Wm * Math.Sin(Math.PI * 0.25));
			tmpToPoint = ARANFunctions.LocalToPrj(PtM, axis, -Wm * Math.Cos(Math.PI * 0.25), (int)side * Wm * Math.Sin(Math.PI * 0.25));
			ArcM = ARANFunctions.CreateArcPrj(PtM, tmpFromPoint, tmpToPoint, ARANMath.ChangeDirection(side));

			tmpFromPoint = ARANFunctions.LocalToPrj(PtN4, axis,Wn4,0);
			tmpToPoint = ARANFunctions.LocalToPrj(PtN4, axis, -Wn4, 0);
			ArcN4 = ARANFunctions.CreateArcPrj(PtN4, tmpFromPoint, tmpToPoint, ARANMath.ChangeDirection(side));

			tmpFromPoint = ARANFunctions.LocalToPrj(PtN3, axis, Wn3, 0);
			tmpToPoint = ARANFunctions.LocalToPrj(PtN3, axis, -Wn3, 0);
			ArcN3 = ARANFunctions.CreateArcPrj(PtN3, tmpFromPoint, tmpToPoint, ARANMath.ChangeDirection(side));
			
			tmpFromPoint = ARANFunctions.LocalToPrj(PtN3, axis, 0, (int)side *Wn3);
			tmpToPoint = ARANFunctions.LocalToPrj(PtN3, axis,0,- (int)side *Wn3);
			Arc3N = ARANFunctions.DrawArcPrj(PtN3, tmpFromPoint, tmpToPoint, (int)side);

            //intersectArcN3Turn180 = null;
			Geometry geom = GlobalParams.GeomOperators.Intersect(Arc3N, ARANFunctions.CreatePolyLineFromParts(turn180));
			
            if (geom.GetGeometryType() ==GeometryType.Polyline ||  geom.AsMultiPoint.Count == 0)
			{
				tmpFromPoint = ARANFunctions.CircleVectorIntersect(PtN3, Wn3, wayPoint, axis + Math.PI, 2 * Wn3);
				double distance = ARANFunctions.ReturnDistanceAsMeter(tmpFromPoint, wayPoint);
				tmpToPoint = ARANFunctions.LocalToPrj(wayPoint, axis, distance, 0);
				spiralA = ARANFunctions.DrawArcPrj(wayPoint, tmpFromPoint, tmpToPoint, (int)side);
              
            }
			
			
			shablon = ChainHullAlgorithm.ConvexHull(turn180, ArcI1, ArcI2, ArcJ, Arck, ArcL, ArcM, ArcN3, ArcN4, spiralA);
			Polygon turnShablon = TransForm.RotateGeometry(wayPoint, shablon, -axis).AsPolygon;
			GeoMinMaxPoint minMax = TransForm.QueryCoords(turnShablon);
			Point ptMin = new Point(minMax.XMin, minMax.YMin);
			Point ptMax = new Point(minMax.XMax, minMax.YMax);
			ptE = new Point();

			if (side == SideDirection.sideLeft)
			{
				ptE.SetCoords(minMax.XMax - xe, minMax.YMax - ye);
			}
			else
				ptE.SetCoords(minMax.XMax - xe, minMax.YMin + ye);

			ptE = TransForm.RotateGeometry(wayPoint, ptE, axis).AsPoint;
			//End Calculate


			//Calculate Line3 Intersect
			tmpDirection = ARANFunctions.ReturnAngleAsRadian(PtI3, PtG);
			tmpD = ARANFunctions.ReturnDistanceAsMeter(PtI3,PtG);
			alpha = (int)side * Math.Acos((Wi3 -Wg)/tmpD);
			tmpFromPoint = ARANFunctions.LocalToPrj(PtG, tmpDirection - alpha, Wg, 0);
			tmpFromPoint.M = tmpDirection - alpha - (int)side *Math.PI / 2;
			line3.Add((Point)tmpFromPoint.Clone());
			tmpToPoint = ARANFunctions.LocalToPrj(PtI3, tmpDirection - alpha, Wi3, 0);
			tmpToPoint.M = tmpFromPoint.M;
			line3.Add((Point)tmpToPoint.Clone());

			tmpDirection = ARANFunctions.ReturnAngleAsRadian(PtI4, PtI3);
			tmpD = ARANFunctions.ReturnDistanceAsMeter(PtI4, PtI3);
			alpha = (int)side * Math.Acos((Wi4 - Wi3) / tmpD);
			tmpFromPoint = ARANFunctions.LocalToPrj(PtI3, tmpDirection - alpha, Wi3, 0);
			tmpFromPoint.M = tmpDirection - alpha - (int)side * Math.PI / 2;
			line3.Add((Point)tmpFromPoint.Clone());
			tmpToPoint = ARANFunctions.LocalToPrj(PtI4, tmpDirection - alpha, Wi4, 0);
			tmpToPoint.M = tmpFromPoint.M;
			line3.Add((Point)tmpToPoint.Clone());

            Point tmpPoint = new Point ();
            tmpPoint.M = tmpFromPoint.M;
            tmpPoint = ARANFunctions.LocalToPrj ( line3 [3], tmpPoint.M, 5*L,0 );
            line3.Add ( ( Point ) tmpPoint.Clone () );
            //End Calculate Line3

			//ui.DrawPart(turn180, 1, 1);
			//ui.DrawRing(ArcN3,1,eFillStyle.sfsHollow);
			//ui.DrawPointWithText(tmpFromPoint, 1, "ptFrom");
			////ui.DrawPolyline(Arc3N, 1, 1);

			//ui.DrawPolygon(Shablon, 1, eFillStyle.sfsHollow);
			//if (spiralA != null)
			//    ui.DrawPolyline(spiralA, 255, 1);
			//ui.DrawPolygon(turnShablon, 1, ARAN.Contracts.UI.eFillStyle.sfsDiagonalCross);
			//ui.DrawPolyline(inboundTrack, 1, 2);
			//ui.DrawPointWithText(ptMin, 1, "ptMin");
			//ui.DrawPointWithText(ptMax, 1, "ptMax");
			//ui.DrawPointWithText(ptE, 1, "PTE");

			return 1;
			
	}

		public static void HoldingShablonForRnaw(Point wayPoint, double IAS, double AbsH, double dISA, double T, double Axis, SideDirection side, int NavType, out PolyLine HoldingArea, out Polygon Shablon, out Part Line3, out Part Turn180,out Point ptE)
		{
			
			double V, v3600, R, Rv, H, w, w_, E45, E, t_, L,
			 ab, ac, xe, ye;

			Point PtCntB;
			Part Line2;

			Line3 = new Part();
			Turn180 = new Part();

			V = ARANMath.IASToTAS(IAS, AbsH, dISA);
			v3600 = V;
			R = 943.27 * 0.277777777777778 / V;
			if ((R > 3.0))
				R = 3.0;

			Rv = 1000.0 * 3.6 * V / (62.83 * R);
			H = AbsH / 1000.0;
			w = 12.0 * H + 87.0;

			w_ = 0.277777777777778 * w;

			E = w_ / R;
			E45 = 45.0 * E;
			t_ = 60.0 * T;

			L = v3600 * t_;
			ab = 5.0 * v3600;
			ac = 11.0 * v3600;

			xe = 2.0 * Rv + L + 11 * v3600 + (22.0 + 195.0 / R) * w_;
			ye = 11.0 * v3600 * System.Math.Cos(ARANFunctions.DegToRad(20.0)) + Rv * System.Math.Sin(ARANFunctions.DegToRad(20.0)) + Rv + (22.0 + 125.0 / R) * w_;

			//Turn180 calculate 
			PtCntB = ARANFunctions.LocalToPrj(wayPoint, Axis, -ab, -Rv * (int)side);
			Turn180.Add(wayPoint);
			CreateSpiralBy2Radial(PtCntB, Rv + 5 * w_, Axis + (int)side * Math.PI * 0.5, Axis, E / ARANFunctions.DegToRad(1), (int)side, Turn180);

			//	End Calculate

			HoldingArea = CalculateHoldingArea(wayPoint, Axis, (int)side, Rv, L);

			//Calculate Line2
			TouchTurn180InRad = ARANFunctions.SpiralTouchAngle(Rv + 5 * w_, E / ARANFunctions.DegToRad(1), Axis, Axis + (int)side * Math.PI * 0.5, side);
			Line2 = new Part();
			CreateSpiralBy2Radial(PtCntB, Rv + 5 * w_, Axis + (int)side * Math.PI * 0.5, Axis + (int)side * TouchTurn180InRad, E / ARANFunctions.DegToRad(1), (int)side, Line2);
			 
			//End Calculate
			
			//Calculate RnawShablon
			Point tmpPoint = ARANFunctions.LocalToPrj(wayPoint, Axis, 0, -(int)side * 2 * Rv);
			Point PtI = ARANFunctions.LocalToPrj(tmpPoint, Axis, L, 0);
			Point PtI1 = ARANFunctions.LineLineIntersect(Line2[Line2.Count - 1], Axis, PtI, Axis - (int)side * Math.PI / 2) as Point;
						
			Part rotateAraoundPtPrj = TransForm.RotateGeometry(wayPoint, (Geometry)Turn180, Math.PI) as Part;
			Part MovePartToI1 = TransForm.Move(rotateAraoundPtPrj, wayPoint, PtI1) as Part;
			Part MovePartToI = TransForm.Move(rotateAraoundPtPrj, wayPoint, PtI) as Part;
            //
			Shablon = ChainHullAlgorithm.ConvexHull(Turn180, MovePartToI, MovePartToI1);
			
            Polygon turnShablon = TransForm.RotateGeometry(wayPoint, Shablon, -Axis).AsPolygon;
			GeoMinMaxPoint minMax = TransForm.QueryCoords(turnShablon);
			Point ptMin = new Point(minMax.XMin,minMax.YMin);
			Point ptMax = new Point(minMax.XMax,minMax.YMax);
			ptE = new Point();
						
			if (side == SideDirection.sideLeft)
			{
				ptE.SetCoords(minMax.XMax - xe, minMax.YMax - ye);
			}
			else
				ptE.SetCoords(minMax.XMax - xe, minMax.YMin + ye);

			ptE = TransForm.RotateGeometry(wayPoint, ptE, Axis).AsPoint;
		
			//	ui.DrawPointWithText(ptE, 1, "PTE");
			//ui.DrawPointWithText(ptMin, 1, "ptMin");
			//ui.DrawPointWithText(ptMax, 1, "ptMax");
			//ui.DrawPart(Line2, 255, 7);
			//ui.DrawPolygon(turnShablon, 1, ARAN.Contracts.UI.eFillStyle.sfsDiagonalCross);
			//ui.DrawPolyline(HoldingArea, 1, 2);
			
			//End Calculate


		}
	
		public static PolyLine  CalculateHoldingArea (Point ptNavPrj,double axis,int side,double Rv,double L)
		{
			MultiPoint holdingArea = new MultiPoint();
			holdingArea.AddPoint(ptNavPrj);
			//PtC
			holdingArea[0].M = axis + Math.PI;//change

			holdingArea.AddPoint(ARANFunctions.LocalToPrj(ptNavPrj, axis, 0, -(int)side * 2.0 * Rv));

			//PtG '
			holdingArea[1].M = axis;

			holdingArea.AddPoint(ARANFunctions.LocalToPrj(holdingArea[1], axis, L, 0));
			holdingArea[2].M = axis;

			holdingArea.AddPoint(ARANFunctions.LocalToPrj(holdingArea[2], axis, 0, (int)side * 2 * Rv));
			holdingArea[3].M = axis + Math.PI;
			holdingArea.AddPoint(holdingArea[0]);

			return ARANFunctions.CalcTrajectoryFromMultiPoint(holdingArea);
		}

		public static void ChangeSpiralStartParam(double E, double StR0, double StRadial, ref double NewStR0, ref double NewStRadial, int side, int sideChange)
		{
			double TurnDeg;

			TurnDeg = ARANMath.Modulus((NewStRadial - StRadial) * (int)sideChange, 2*Math.PI);

			if (TurnDeg >= Math.PI)
			{
				sideChange = -(sideChange);
				TurnDeg = ARANMath.Modulus((NewStRadial - StRadial) * (int)sideChange, 2 *Math.PI);
			}

			NewStR0 = StR0 + E * TurnDeg *180/Math.PI * (int)side *(int)sideChange;

			if (NewStR0 < 0.0)
			{
				TurnDeg = StR0 / E;
				NewStR0 = 0.0;
			}
			NewStRadial = ARANMath.Modulus(TurnDeg * (int)sideChange + StRadial,2* Math.PI);
		}

		public static void CreateSpiralBy2Radial(Point ptCnt, double r0, double AztStRad, double AztEndRad, double E, int side, MultiPoint pPointCollection)
		{
			double dAlpha;
			double TurnRad;
			double R;
			int N;
			int I;
			Point ptCur;

            
			TurnRad = ARANMath.Modulus((AztEndRad - AztStRad) * (int)side, 2 * Math.PI);
            dAlpha = Math.PI / 180.0;
			N = (int)Math.Ceiling(TurnRad / dAlpha);
			if (N < 2)
				N = 2;
			else if (N < 5)
				N = 5;
			else if (N < 10)
				N = 10;
			dAlpha = TurnRad / N;
			ptCur = new Point();
			for (I = 0; I <= N; I++)
			{
                R = r0 + ( I * dAlpha * E);// * 180/Math.PI );
				// + dphi0 * coef
				ptCur = ARANFunctions.LocalToPrj(ptCnt, AztStRad + (I * dAlpha) * (int)side, R, 0);
				pPointCollection.AddPoint(ptCur);
			}
		}

		public static int TouchTo2Spiral(Point pntCnt1, double r10, double e1, double aztSt1, Point pntCnt2, double r20, double e2, double aztSt2, SideDirection side, ref double touchAngle1, ref double touchAngle2, out Point touchPnt1, out Point touchPnt2)
        {

            #region In Radian

            bool bOutOfSpiral;
            int i, j, k;
            double f, f_, r1, r2 = 0, fTmp, phi1 = 0, phi2 = 0, phi10 = 0, phi20 = 0, aztR1E, aztR2E, aztO1O2, distance;
            //Point touchPnt1, touchPnt2;

            aztO1O2 = ARANFunctions.ReturnAngleAsRadian ( pntCnt1, pntCnt2 );
            bOutOfSpiral = false;

            touchPnt1 = null;
            touchPnt2 = null;

            for ( k = 0; k <= 10; k++ )
            {
                phi10 = ARANMath.Modulus ( aztO1O2 - ( ARANMath.C_PI_2 + 10 *( ARANMath.C_PI /180 ) * k ) * ( int ) side, ARANMath.C_2xPI );
                fTmp = ARANMath.Modulus ( ( phi10 - aztSt1 ) * ( int ) side, ARANMath.C_2xPI );
                r1 = r10 + e1 * fTmp;// *180 / Math.PI;
                //pnt1 = ARANFunctions.LocalToPrj ( pntCnt1, phi10, R1, 0 );
                touchPnt1 = ARANFunctions.PointAlongPlane ( pntCnt1, ARANFunctions.RadToDeg ( phi10 ), r1 );

                phi20 = ARANFunctions.ReturnAngleAsRadian ( pntCnt2, touchPnt1 );
                distance = ARANFunctions.ReturnDistanceAsMeter ( pntCnt2, touchPnt1 );
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
                aztR1E = System.Math.Atan ( e1 * ( int ) side /  r1 );

                for ( i = 0; i <= 20; i++ )
                {
                    fTmp = ARANMath.Modulus ( ( phi20 - aztSt2 ) *( int ) side, ARANMath.C_2xPI );
                    r2 = r20 + e2 * fTmp;// *180/Math.PI;
                    aztR2E = System.Math.Atan ( e2 * ( int ) side /r2 );
                    f = phi20 - phi10 + aztR1E - aztR2E;
                    fTmp = e2 * e2;

                    f_ = ( 1 + /* 180 * */ fTmp / ( /*Math.PI * */( r2 * r2 + fTmp ) ) );

                    phi2 = phi20 - ( f / f_ );

                    if ( System.Math.Abs ( System.Math.Sin ( f / f_ ) ) < radEps )
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
                fTmp = ARANFunctions.ReturnAngleAsRadian ( touchPnt1, touchPnt2 );
                fTmp = ARANMath.SubtractAnglesWithSign ( phi1 + ( ARANMath.C_PI_2 * ( int ) side - aztR1E ), fTmp, side );
                phi1 = phi10 + fTmp * ( int ) side;
                if ( System.Math.Abs ( fTmp ) < ( radEps * 50 ) )
                {
                    touchAngle1 = ARANMath.Modulus ( phi1, ARANMath.C_2xPI );
                    touchAngle2 = ARANMath.Modulus ( phi2, ARANMath.C_2xPI );
                    //InitHolding.UI.DrawPointWithText ( touchPnt1, 65357*255, "Pt1" );
                    //InitHolding.UI.DrawPointWithText ( touchPnt2, 65357*255, "Pt2" );
                    //double dist = ARANFunctions.ReturnDistanceAsMeter ( touchPnt1, touchPnt2 );
                    //Part tmpPart1 = new Part (), tmpPart2 = new Part ();
                    //CreateSpiralBy2Radial ( pntCnt1, r10, aztSt1, aztSt1 - ( int ) side * ARANMath.C_PI_2, e1 / ARANFunctions.RadToDeg ( 1 ), ( int ) side, tmpPart1 );
                    //CreateSpiralBy2Radial ( pntCnt2, r20, aztSt2, aztSt2 -( int ) side*ARANMath.C_PI_2, e2/ ARANFunctions.RadToDeg ( 1 ), ( int ) side, tmpPart2 );
                    //InitHolding.UI.DrawPart ( tmpPart1, 0, 1 );
                    //InitHolding.UI.DrawPart ( tmpPart2, 0, 1 );
                    //Part tmpLine = new Part ();
                    //tmpLine.Add ( touchPnt1 );
                    //tmpLine.Add ( touchPnt2 );
                    //InitHolding.UI.DrawPart ( tmpLine, 0, 1 );
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
            //        Part tmpPart1 = new Part (), tmpPart2 = new Part ();
            //        CreateSpiralBy2Radial ( pntCnt1, r10, ARANFunctions.DegToRad ( aztSt1 ), ARANFunctions.DegToRad ( aztSt1 ) - ( int ) side * ARANMath.C_PI_2, e1, ( int ) side, tmpPart1 );
            //        CreateSpiralBy2Radial ( pntCnt2, r20, ARANFunctions.DegToRad ( aztSt2 ), ARANFunctions.DegToRad ( aztSt2 ) - ( int ) side * ARANMath.C_PI_2, e2, ( int ) side, tmpPart2 );
            //        InitHolding.ui.DrawPart ( tmpPart1, 0, 1 );
            //        InitHolding.ui.DrawPart ( tmpPart2, 0, 1 );
            //        Part tmpLine = new Part ();
            //        tmpLine.Add ( pt1 );
            //        tmpLine.Add ( pt2 );
            //        InitHolding.ui.DrawPart ( tmpLine, 0, 1 );
            //        return 1;
            //    }
            //    else
            //        phi10 = phi1;
            //}
            //return 0;

                #endregion
        }

		public static double CalculateWd(double T, double ias, double dias, double absH,double att,double xtt,out double tMin)
		{
			double ds, Rv, V, v3600, R, WD, WDmin;
			double w, w_, E, E45, t_, Wc, Wg,H;
			
			V = ARANMath.IASToTAS(ias, absH, dias);
			v3600 = V;
			R = 943.27 * 0.277777777777778 / V;
			if ((R > 3.0))
				R = 3.0;

			Rv = 1000.0 * 3.6* V / (62.83 * R);
			t_ = T * 60;
			ds = v3600 * t_;
			WD = Math.Sqrt(Math.Pow(ds, 2) + 4 * Math.Pow(Rv, 2));
			H = absH / 1000.0;
			w = 12.0 * H + 87.0;
			w_ = 0.277777777777778 * w;

			E = w_ / R;
			E45 = 45.0 * E;

			Wc = 11 * w_;
			Wg = Wc + 4 * E45;
			WDmin = Math.Sqrt(Math.Pow(att + 11 * v3600, 2) + Math.Pow(2 * Rv + xtt, 2)) + Wg;

			if (WD < WDmin)
			{
				tMin = Math.Sqrt(Math.Pow(WDmin, 2) - 4 * Math.Pow(Rv, 2)) / v3600;
				return WDmin;
			}
			tMin = t_;		
			return WD;

		}

        public static double SpiralTouchToFix ( Point ptCnt, double E, double r0, double aztStRad, SideDirection side, Point fixPnt, int spiralIntercept, double axis )
        {
            int i, j;
            double r, f, f_, xA, yA, xSp, ySp, phi = 0, phi0, fTmp, sinA, cosA, dPhi, spAngle, aztToFix, distToFix;
            double result;

            aztToFix = ARANFunctions.ReturnAngleAsRadian ( ptCnt, fixPnt );
            distToFix = ARANFunctions.ReturnDistanceAsMeter ( ptCnt, fixPnt );

            fTmp = ARANMath.Modulus ( ( aztToFix - aztStRad ) * ( int ) side, ARANMath.C_2xPI );

            if ( ( fTmp > ARANMath.C_PI ) && ( r0 == 0.0 ) )
            {
                fTmp = fTmp - ARANMath.C_2xPI;
            }
            Point touchPnt;
            r = r0 + E * fTmp;
            if ( System.Math.Abs ( r - distToFix ) < distEps )
            {
                return aztToFix;
            }

            if ( r < distToFix )
            {
                phi0 = ARANMath.Modulus ( aztToFix + ARANMath.C_PI_2 * ( 1 + spiralIntercept ), ARANMath.C_2xPI );
                for ( i = 0; i<=30; i++ )
                {
                    phi = phi0;
                    spAngle = ARANFunctions.SpiralTouchAngle ( r0, E, aztStRad + ARANMath.C_PI_2 * ( int ) side, phi, side );
                    r = r0 + E * spAngle;

                    result = ARANMath.Modulus ( aztStRad + spAngle * ( int ) side, ARANMath.C_2xPI);
                    
                    xSp = ptCnt.X + r * System.Math.Cos ( result );
                    ySp = ptCnt.Y + r * System.Math.Sin ( result );

                    touchPnt = new Point ( xSp, ySp );
                    GlobalParams.UI.DrawPointWithText ( touchPnt, 0, "T" + i.ToString() );
                    
                    fTmp = System.Math.Atan2 ( fixPnt.Y - ySp, fixPnt.X - xSp );

                    phi0 = ARANMath.Modulus ( fTmp + ARANMath.C_PI_2 * ( 1 + spiralIntercept ), ARANMath.C_2xPI );

                    dPhi = ARANMath.SubtractAngles ( phi, phi0 );

                    if ( dPhi < radEps )
                    {
                        return result;
                    }
                }
            }
            else
            {

                cosA = System.Math.Cos ( axis + ARANMath.C_PI_2 * ( int ) side );
                sinA = System.Math.Sin ( axis + ARANMath.C_PI_2 * ( int ) side );
                xA = distToFix * System.Math.Cos ( aztToFix );
                yA = distToFix * System.Math.Sin ( aztToFix );
                phi0 = aztToFix;
                for ( j = 0; j <= 20; j++ )
                {
                    fTmp = ARANMath.Modulus ( ( phi0 - aztStRad ) * ( int ) side, ARANMath.C_2xPI );
                    r = r0 + E * fTmp;
                    f = r * ( System.Math.Sin ( phi0 ) * cosA - System.Math.Cos ( phi0 ) * sinA ) + xA * sinA - yA * cosA;
                    f_ = ARANFunctions.RadToDeg ( E ) * ( int ) side * ( System.Math.Sin ( phi0 ) * cosA - System.Math.Cos ( phi0 ) * sinA ) + r * ( System.Math.Cos ( phi0 ) * cosA + System.Math.Sin ( phi0 ) * sinA );
                    phi = phi0 - f / f_;
                    if ( System.Math.Abs ( System.Math.Sin ( phi - phi0 ) ) < 0.001 )
                    {
                        return ARANMath.Modulus ( phi, ARANMath.C_2xPI );
                    }
                    else
                        phi0 = phi;
                }
            }
            return ARANMath.Modulus ( phi, ARANMath.C_2xPI );
        }
                
        public static double TurnRadius(double ias, double altitude,double dIsa)
        {
            double  V = ARANMath.IASToTAS(ias, altitude, dIsa);
            double R = 943.27 * 0.277777777777778 / V;
            if ((R > 3.0))
                R = 3.0;

           return  1000.0 * 3.6 * V / (62.83 * R);
        }

        public static double TouchTurn180InRad { get; set; }
	}
}
