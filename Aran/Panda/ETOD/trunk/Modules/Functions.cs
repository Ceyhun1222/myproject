using System;
using System.Drawing;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Catalog;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Aran.Aim.Features;

namespace ETOD
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eRoundMode
	{
		rmNONE = 0,
		rmFLOOR = 1,
		rmNERAEST = 2,
		rmCEIL = 3,
		rmSPECIAL = 4
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class Functions
	{
		public static double ConvertDistance(double Val_Renamed, eRoundMode RoundMode)
		{
			if (RoundMode < eRoundMode.rmNONE || RoundMode > eRoundMode.rmCEIL) RoundMode = eRoundMode.rmNONE;

			switch (RoundMode)
			{
				case eRoundMode.rmNONE:
					return Val_Renamed * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Multiplier;
				case eRoundMode.rmFLOOR:
					return System.Math.Round(Val_Renamed * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Multiplier / GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding - 0.4999) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding;
				case eRoundMode.rmNERAEST:
					return System.Math.Round(Val_Renamed * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Multiplier / GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding;
				case eRoundMode.rmCEIL:
					return System.Math.Round(Val_Renamed * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Multiplier / GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding + 0.4999) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding;
			}
			return Val_Renamed;
		}

		public static double ConvertHeight(double Val_Renamed, eRoundMode RoundMode)
		{
			if (RoundMode < eRoundMode.rmNONE || RoundMode > eRoundMode.rmSPECIAL) RoundMode = eRoundMode.rmNONE;

			switch (RoundMode)
			{
				case eRoundMode.rmNONE:
					return Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier;
				case eRoundMode.rmFLOOR:
					return System.Math.Round(Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier / GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding - 0.4999) * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding;
				case eRoundMode.rmNERAEST:
					return System.Math.Round(Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier / GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding) * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding;
				case eRoundMode.rmCEIL:
					return System.Math.Round(Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier / GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding + 0.4999) * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding;
				case eRoundMode.rmSPECIAL:
					if (GlobalVars.HeightUnit == 0)
						return System.Math.Round(Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier / 50.0) * 50.0;
					else if (GlobalVars.HeightUnit == 1)
						return System.Math.Round(Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier / 100.0) * 100.0;
					else
						return System.Math.Round(Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier / GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding) * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding;
			}

			return Val_Renamed;
		}

		public static double ConvertSpeed(double Val_Renamed, eRoundMode RoundMode)
		{
			if (RoundMode < eRoundMode.rmNONE || RoundMode > eRoundMode.rmCEIL) RoundMode = eRoundMode.rmNONE;

			switch (RoundMode)
			{
				case eRoundMode.rmNONE:
					return Val_Renamed * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Multiplier;
				case eRoundMode.rmFLOOR:
					return System.Math.Round(Val_Renamed * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Multiplier / GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Rounding - 0.4999) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Rounding;
				case eRoundMode.rmNERAEST:
					return System.Math.Round(Val_Renamed * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Multiplier / GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Rounding) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Rounding;
				case eRoundMode.rmCEIL:
					return System.Math.Round(Val_Renamed * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Multiplier / GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Rounding + 0.4999) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Rounding;
			}

			return Val_Renamed;
		}

		public static double DeConvertDistance(double Val_Renamed)
		{
			return Val_Renamed / GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Multiplier;
		}

		public static double DeConvertHeight(double Val_Renamed)
		{
			return Val_Renamed / GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier;
		}

		public static double DeConvertSpeed(double Val_Renamed)
		{
			return Val_Renamed / GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Multiplier;
		}

		public static T RegRead<T>(RegistryKey hKey, string key, string valueName, T defaultValue)
		{
			try
			{
				RegistryKey regKey = hKey.OpenSubKey(key, false);
				if (regKey != null)
				{
					object value = regKey.GetValue(valueName);
					if (value != null)
					{
						try
						{
							return (T)Convert.ChangeType(value, typeof(T));
						}
						catch { }
					}
				}
			}
			catch { }

			return defaultValue;
		}

		public static int RegWrite(Microsoft.Win32.RegistryKey HKey, string Key, string ValueName, object Value)
		{
			try
			{
				Microsoft.Win32.RegistryKey regKey = HKey.OpenSubKey(Key, true);
				if (regKey == null)
				{
					return -1;
				}

				regKey.SetValue(ValueName, Value);
				return 0;
			}
			catch { }

			return -1;
		}

		public static double DegToRad(double Val_Renamed)
		{
			return Val_Renamed * GlobalVars.DegToRadValue;
		}

		public static double RadToDeg(double Val_Renamed)
		{
			return Val_Renamed * GlobalVars.RadToDegValue;
		}

		public static IGeometry ToGeo(IGeometry prjGeometry)
		{
			ESRI.ArcGIS.Geometry.IGeometry toGeoReturn = null;
			IClone pClone = null;

			pClone = ((IClone)(prjGeometry));
			toGeoReturn = ((IGeometry)(pClone.Clone()));

			toGeoReturn.SpatialReference = GlobalVars.pSpRefPrj;
			toGeoReturn.Project(GlobalVars.pSpRefShp);
			return toGeoReturn;
		}

		public static IGeometry ToPrj(IGeometry geoGeometry)
		{
			ESRI.ArcGIS.Geometry.IGeometry toPrjReturn = null;
			IClone pClone = null;

			pClone = ((IClone)(geoGeometry));
			toPrjReturn = ((IGeometry)(pClone.Clone()));

			toPrjReturn.SpatialReference = GlobalVars.pSpRefShp;
			toPrjReturn.Project(GlobalVars.pSpRefPrj);
			return toPrjReturn;
		}

		public static double Azt2Dir(IPoint ptGeo, decimal Azt)
		{
			return Azt2Dir(ptGeo, (double)Azt);
		}

		public static double Azt2Dir(IPoint ptGeo, double Azt)
		{
			IClone pClone = null;
			ILine pLine = null;
			double ResX = 0;
			double ResY = 0;

			IPoint Pt10 = null;
			IPoint Pt11 = null;
			int J = 0;

			pClone = ((IClone)(ptGeo));
			Pt11 = ((IPoint)(pClone.Clone()));
			J = NativeMethods.PointAlongGeodesic(Pt11.X, Pt11.Y, 10.0, Azt, ref ResX, ref ResY);

			Pt10 = ((IPoint)(pClone.Clone()));
			Pt10.PutCoords(ResX, ResY);

			pLine = new Line();
			pLine.PutCoords(Pt11, Pt10);
			pLine = ((ESRI.ArcGIS.Geometry.ILine)(ToPrj(pLine)));

			return NativeMethods.Modulus(GlobalVars.RadToDegValue * pLine.Angle, 360.0);
		}

		public static double Dir2Azt(IPoint pPtPrj, double Dir_Renamed)
		{
			double resD = 0;
			double resI = 0;
			IPoint PtN = null;
			IPoint Pt10 = null;

			PtN = Functions.PointAlongPlane(pPtPrj, Dir_Renamed, 10.0);

			Pt10 = ((ESRI.ArcGIS.Geometry.IPoint)(Functions.ToGeo(PtN)));
			PtN = ((ESRI.ArcGIS.Geometry.IPoint)(Functions.ToGeo(pPtPrj)));

			NativeMethods.ReturnGeodesicAzimuth(PtN.X, PtN.Y, Pt10.X, Pt10.Y, ref resD, ref resI);
			return resD;
		}

		//private static void QuickSort(ObstacleType[] A, int iLo, int iHi)
		//{
		//    ObstacleType t;
		//    double Mid_Renamed = 0;
		//    int Lo = 0;
		//    int Hi = 0;

		//    Lo = iLo;
		//    Hi = iHi;
		//    Mid_Renamed = A[(Lo + Hi) / 2].Dist;
		//    do
		//    {
		//        while (A[Lo].Dist < Mid_Renamed)
		//        {
		//            Lo = Lo + 1;
		//        }
		//        while (A[Hi].Dist > Mid_Renamed)
		//        {
		//            Hi = Hi - 1;
		//        }
		//        if ((Lo <= Hi))
		//        {
		//            t = A[Lo];
		//            A[Lo] = A[Hi];
		//            A[Hi] = t;
		//            Lo = Lo + 1;
		//            Hi = Hi - 1;
		//        }
		//    }
		//    while (!(Lo > Hi));

		//    if ((Hi > iLo))
		//    {
		//        QuickSort(A, iLo, Hi);
		//    }
		//    if ((Lo < iHi))
		//    {
		//        QuickSort(A, Lo, iHi);
		//    }
		//}

		//public static void Sort(ObstacleType[] A)
		//{
		//    int Lo = 0;
		//    int Hi = 0;

		//    System.Array transTemp0 = A;
		//    Lo =   transTemp0.GetLowerBound(0);
		//    System.Array transTemp1 = A;
		//    Hi =   transTemp1.GetUpperBound(0);

		//    if ((Lo == Hi))
		//    {
		//        return;
		//    }
		//    QuickSort(A, Lo, Hi);
		//}

		//public static void MoveInToOut(ref ObstacleType[] PtOutList, ref ObstacleType[] PtInList)
		//{
		//    int N = 0;
		//    int M = 0;
		//    int I = 0;
		//    int J = 0;

		//    System.Array transTemp2 = PtOutList;
		//    N =  /* TRANSINFO: .NET Equivalent of Microsoft.VisualBasic NameSpace */ transTemp2.GetUpperBound(0);
		//    System.Array transTemp3 = PtInList;
		//    M =  /* TRANSINFO: .NET Equivalent of Microsoft.VisualBasic NameSpace */ transTemp3.GetUpperBound(0);

		//    if (M < 0)
		//    {
		//        return;
		//    }

		//    if (N > -1)
		//    {
		//        System.Array.Resize<ObstacleType>(ref PtOutList, N + M + 2);
		//        J = N + 1;
		//    }
		//    else
		//    {
		//        PtOutList = GlobalVars.InitArray<ObstacleType>(M + 1);   //--- Checked
		//        J = 0;
		//    }

		//    for (I = 0; I <= M; I++)
		//    {
		//        PtOutList[J] = PtInList[I];
		//        J = J + 1;
		//    }

		//    PtInList = new ObstacleType[0];
		//}

		//public static void GetInnerObstacles(ref ObstacleType[] PtOutList, ref ObstacleType[] PtInList, IPointCollection pLPolygon,
		//                                    RWYType DER, double DepDir, bool Primary, bool Guidance, IPoint ptNav = null)
		//{
		//    IPoint ptTmp = null;
		//    IPoint ptCur = null;
		//    IProximityOperator pProxiOperator = null;

		//    int N = 0;
		//    int I = 0;
		//    int iIn = 0;
		//    int oldInners = 0;
		//    double d = 0;

		//    if (pLPolygon.PointCount < 1)
		//        return;

		//    oldInners = PtInList.GetUpperBound(0);
		//    N = PtOutList.GetUpperBound(0);	//.GetLength(0);//

		//    if (N >= 0)
		//    {
		//        //if (oldInners < 0)
		//        //    PtInList = GlobalVars.InitArray<ObstacleHd> (N + 1);    //--- Checked
		//        //    System.Array.Resize<ObstacleHd> (ref PtInList, N + 1);
		//        //else
		//        System.Array.Resize<ObstacleType>(ref PtInList, oldInners + N + 2);
		//    }
		//    else
		//        return;

		//    pProxiOperator = ((IProximityOperator)(pLPolygon));

		//    iIn = oldInners;
		//    I = 0;
		//    //			DrawPolygon(pLPolygon);

		//    while (I <= N)
		//    {
		//        ptCur = PtOutList[I].pPtPrj;
		//        d = pProxiOperator.ReturnDistance(ptCur);

		//        if (d == 0.0)
		//        {
		//            iIn = iIn + 1;
		//            PtInList[iIn] = PtOutList[I];

		//            PtInList[iIn].Dist = Functions.Point2LineDistancePrj(ptCur, DER.pPtPrj[eRWY.PtEnd], DepDir + 90.0) - PtInList[iIn].HorAccuracy;
		//            if (PtInList[iIn].Dist <= 0.0)
		//            {
		//                PtInList[iIn].Dist = GlobalVars.distEps;
		//            }

		//            if (!(Guidance))
		//            {
		//                PtInList[iIn].CLShift = Functions.Point2LineDistancePrj(ptCur, DER.pPtPrj[eRWY.PtEnd], DepDir) - PtInList[iIn].HorAccuracy;
		//            }
		//            else
		//            {
		//                PtInList[iIn].CLShift = Functions.Point2LineDistancePrj(ptCur, ptNav, DepDir) - PtInList[iIn].HorAccuracy;
		//            }

		//            if (PtInList[iIn].CLShift <= 0.0)
		//            {
		//                PtInList[iIn].CLShift = GlobalVars.distEps;
		//            }

		//            PtInList[iIn].Prima = Primary;
		//            PtOutList[I] = PtOutList[N];
		//            N = N - 1;
		//        }
		//        else
		//        {
		//            ptTmp = pProxiOperator.ReturnNearestPoint(ptCur, esriSegmentExtension.esriNoExtension);
		//            PtOutList[I].SurfaceHeight = Functions.Point2LineDistancePrj(ptTmp, DER.pPtPrj[eRWY.PtEnd], DepDir + 90.0) - PtInList[I].HorAccuracy;
		//            if (PtOutList[I].SurfaceHeight <= 0.0)
		//                PtOutList[I].SurfaceHeight = GlobalVars.distEps;

		//            PtOutList[I].Dist = d - PtInList[I].HorAccuracy;
		//            if (PtInList[I].Dist <= 0.0)
		//                PtInList[I].Dist = GlobalVars.distEps;

		//            I = I + 1;
		//        }
		//    }

		//    if (iIn > -1)
		//        System.Array.Resize<ObstacleType>(ref PtInList, iIn + 1);
		//    else
		//        PtInList = new ObstacleType[0];

		//    if (N > -1)
		//        System.Array.Resize<ObstacleType>(ref PtOutList, N + 1);
		//    else
		//        PtOutList = new ObstacleType[0];

		//    if (iIn > 0)
		//        Sort(PtInList);
		//}

		public static void AddGraphicsElement(IElement graphicElement)
		{
			if (graphicElement == null)
				return;
			try
			{
				GlobalVars.GetActiveView().GraphicsContainer.AddElement(graphicElement, 0);
			}
			catch
			{
				try
				{
					if (graphicElement is ESRI.ArcGIS.Carto.GroupElement)
					{
						ESRI.ArcGIS.Carto.IGroupElement pGroupElement = (ESRI.ArcGIS.Carto.IGroupElement)graphicElement;
						for (int i = 0; i < pGroupElement.ElementCount; i++)
							if (pGroupElement.Element[i] != null)
								GlobalVars.GetActiveView().GraphicsContainer.AddElement(pGroupElement.Element[i], 0);
					}
				}
				catch
				{
				}
			}
		}

		public static void DeleteGraphicsElement(IElement graphicElement)
		{
			if (graphicElement == null)
				return;

			try
			{
				GlobalVars.GetActiveView().GraphicsContainer.DeleteElement(graphicElement);
			}
			catch
			{
				try
				{
					if (graphicElement is ESRI.ArcGIS.Carto.GroupElement)
					{
						ESRI.ArcGIS.Carto.IGroupElement pGroupElement = (ESRI.ArcGIS.Carto.IGroupElement)graphicElement;
						for (int i = 0; i < pGroupElement.ElementCount; i++)
							if (pGroupElement.Element[i] != null)
								GlobalVars.GetActiveView().GraphicsContainer.DeleteElement(pGroupElement.Element[i]);
					}
				}
				catch
				{
				}
			}
		}

		public static IElement DrawPolygon(IPolygon pPolygon, int Color = -1, bool drawFlg = true, int SFStyle = 1)
		{
			ESRI.ArcGIS.Carto.IElement drawPolygonReturn = null;
			IRgbColor pRGB = null;
			ISimpleFillSymbol pFillSym = null;
			IFillShapeElement pFillShpElement = null;
			ILineSymbol pLineSimbol = null;
			IElement pElementofPoly = null;

			pRGB = new RgbColor();
			pFillSym = new SimpleFillSymbol();
			pFillShpElement = ((ESRI.ArcGIS.Carto.IFillShapeElement)(new PolygonElement()));

			pElementofPoly = ((ESRI.ArcGIS.Carto.IElement)(pFillShpElement));
			pElementofPoly.Geometry = ((ESRI.ArcGIS.Geometry.IGeometry)(pPolygon));

			if (Color != -1)
			{
				pRGB.RGB = Color;
			}
			else
			{
				Random rnd = new Random();
				pRGB.Red = rnd.Next(256);
				pRGB.Green = rnd.Next(256);
				pRGB.Blue = rnd.Next(256);
			}

			pFillSym.Color = pRGB;
			pFillSym.Style = ((ESRI.ArcGIS.Display.esriSimpleFillStyle)(SFStyle)); // esriSFSNull 'esriSFSDiagonalCross

			pLineSimbol = new SimpleLineSymbol();
			pLineSimbol.Color = pRGB;
			pLineSimbol.Width = 1;
			pFillSym.Outline = pLineSimbol;

			pFillShpElement.Symbol = pFillSym;
			drawPolygonReturn = pElementofPoly;

			if (drawFlg)
			{
				IGraphicsContainer pGraphics = null;
				pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(pElementofPoly, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return drawPolygonReturn;
		}

		public static IElement DrawPolygon(IPointCollection pPolygon, int Color = -1, bool drawFlg = true, int SFStyle = 1)
		{
			return DrawPolygon(pPolygon as IPolygon, Color, drawFlg, SFStyle);
		}

		public static int SideDef(IPoint PtInLine, double LineAngle, IPoint PtOutLine)
		{
			double Angle12 = 0;
			double dAngle = 0;

			Angle12 = Functions.ReturnAngleInDegrees(PtInLine, PtOutLine);

			dAngle = NativeMethods.Modulus(LineAngle - Angle12, 360.0);

			if ((dAngle == 0.0) || (dAngle == 180.0))
			{
				return 0;
			}
			if ((dAngle < 180.0))
			{
				return 1;
			}
			return -1;
		}


		public static int SideFrom2Angle(double Angle0, double Angle1)
		{
			double dAngle = 0;

			dAngle = Functions.SubtractAngles(Angle0, Angle1);

			if ((180.0 - dAngle < GlobalVars.degEps) || (dAngle < GlobalVars.degEps))
			{
				return 0;
			}

			dAngle = NativeMethods.Modulus(Angle1 - Angle0, 360.0);

			if ((dAngle < 180.0))
			{
				return 1;
			}
			return -1;
		}

		public static void CreateWindSpiral(IPoint pPtGeo, double aztNom, double AztSt, ref double AztEnd,
											double r0, double coef, int TurnDir, IPointCollection pPointCollection)
		{
			int I = 0;
			int N = 0;

			double R = 0;
			double azt0 = 0;
			double dphi = 0;
			double dphi0 = 0;
			double dAlpha = 0;
			double TurnAng = 0;

			IPoint ptCnt = null;
			IPoint ptCur = null;

			ptCnt = Functions.PointAlongPlane(pPtGeo, aztNom + 90.0 * TurnDir, r0);

			if (Functions.SubtractAngles(aztNom, AztEnd) < GlobalVars.degEps)
			{
				AztEnd = aztNom;
			}

			dphi0 = (AztSt - aztNom) * TurnDir;
			dphi0 = NativeMethods.Modulus(dphi0, 360.0);

			if ((dphi0 < 0.001))
			{
				dphi0 = 0.0;
			}
			else
			{
				dphi0 = SpiralTouchAngle(r0, coef, aztNom, AztSt, TurnDir);
			}

			dphi = SpiralTouchAngle(r0, coef, aztNom, AztEnd, TurnDir);

			TurnAng = dphi - dphi0;

			azt0 = aztNom + (dphi0 - 90.0) * TurnDir;
			azt0 = NativeMethods.Modulus(azt0, 360.0);

			if (TurnAng < 0.0)
			{
				return;
			}

			dAlpha = 1.0;
			N = System.Convert.ToInt32(TurnAng / dAlpha);
			if (N < 5)
			{
				N = 5;
			}
			else if (N < 10)
			{
				N = 10;
			}

			dAlpha = TurnAng / N;

			ptCur = new ESRI.ArcGIS.Geometry.Point();
			for (I = 0; I <= N; I++)
			{
				R = r0 + (dAlpha * coef * I) + dphi0 * coef;
				ptCur = Functions.PointAlongPlane(ptCnt, azt0 + (I * dAlpha) * TurnDir, R);
				pPointCollection.AddPoint(ptCur);
			}
			// Dim pCurve As ICurve
			// Dim pPolyLine As IPointCollection
			// Set pPolyLine = New Polyline
			// 
			// Set pPolyLine = pPointCollection
			// Set pCurve = pPolyLine

		}

		public static string MyDD2Str(double X, short Mode)
		{
			int lSign = 0;

			double xDeg = 0;
			double xMin = 0;
			double xIMin = 0;
			double xSec = 0;
			string sSign = null;
			string sTmp = null;
			string sResult = null;

			lSign = System.Math.Sign(X);
			X = System.Math.Abs(X);
			X = NativeMethods.Modulus(X, 360.0);
			if (X == 0.0)
			{
				X = 360.0;
			}

			sSign = "";
			if (lSign < 0)
			{
				sSign = "-";
			}

			xDeg =  /* TRANSINFO: .NET Equivalent of Microsoft.VisualBasic NameSpace */ System.Math.Floor((double)X);
			sResult = "";

			switch (Mode)
			{
				case (System.Int16)0:
					sResult = sSign + System.Math.Round(X, 2).ToString() + "°";

					break;
				case (System.Int16)1:
					sTmp = sSign + xDeg.ToString() + "°";
					xMin = System.Math.Round((X - xDeg) * 60.0, 4);
					sResult = sTmp + xMin.ToString() + "'";

					break;
				case (System.Int16)2:
					xDeg =  /* TRANSINFO: .NET Equivalent of Microsoft.VisualBasic NameSpace */ System.Math.Floor((double)X);
					sTmp = sSign + xDeg.ToString() + "°";
					xMin = (X - xDeg) * 60.0;
					xIMin =  /* TRANSINFO: .NET Equivalent of Microsoft.VisualBasic NameSpace */ System.Math.Floor((double)xMin);
					sTmp = sTmp + xIMin.ToString() + "'";
					xSec = (xMin - xIMin) * 60.0;
					sResult = sTmp + System.Math.Round(xSec, 2).ToString() + @"""";

					break;
				case (System.Int16)3:
					xDeg =  /* TRANSINFO: .NET Equivalent of Microsoft.VisualBasic NameSpace */ System.Math.Floor((double)X);
					sTmp = sSign + xDeg.ToString() + "°";
					xMin = (X - xDeg) * 60.0;
					xIMin =  /* TRANSINFO: .NET Equivalent of Microsoft.VisualBasic NameSpace */ System.Math.Floor((double)xMin);
					sTmp = sTmp + xIMin.ToString() + "'";
					xSec = (xMin - xIMin) * 60.0;
					sResult = sTmp + System.Math.Round(xSec, 2).ToString() + @"""";

					if ((X > 0))
					{
						sResult = sResult + "N";
					}
					else
					{
						sResult = sResult + "S";
					}

					break;
				case (System.Int16)4:
					xDeg =  /* TRANSINFO: .NET Equivalent of Microsoft.VisualBasic NameSpace */ System.Math.Floor((double)X);
					sTmp = sSign + xDeg.ToString() + "°";
					xMin = (X - xDeg) * 60.0;
					xIMin =  /* TRANSINFO: .NET Equivalent of Microsoft.VisualBasic NameSpace */ System.Math.Floor((double)xMin);
					sTmp = sTmp + xIMin.ToString() + "'";
					xSec = (xMin - xIMin) * 60.0;
					sResult = sTmp + System.Math.Round(xSec, 2).ToString() + @"""";

					if ((X > 0))
					{
						sResult = sResult + "E";
					}
					else
					{
						sResult = sResult + "W";
					}
					break;
			}


			return sResult;
		}

		public static IPolygon CreatePrjCircle(IPoint pPoint1, double Radius)
		{
			IPointCollection returnValue = new Polygon();
			IPoint pPtGeo = new ESRI.ArcGIS.Geometry.Point();

			for (int i = 0; i <= 359; i++)
			{
				double iInRad = i * GlobalVars.DegToRadValue;
				pPtGeo.X = pPoint1.X + Radius * System.Math.Cos(iInRad);
				pPtGeo.Y = pPoint1.Y + Radius * System.Math.Sin(iInRad);
				returnValue.AddPoint(pPtGeo);
			}

			ITopologicalOperator2 pTopo = returnValue as ITopologicalOperator2;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();
			return returnValue as IPolygon;
		}

		public static IPolygon CreateArcPrj(IPoint ptCnt, IPoint ptFrom, IPoint ptTo, int ClWise)
		{
			ESRI.ArcGIS.Geometry.IPointCollection createArcPrjReturn = null;
			int I = 0;
			int J = 0;

			double R = 0;
			double dX = 0;
			double dY = 0;
			double daz = 0;
			double AngStep = 0;
			double AztFrom = 0;
			double AztTo = 0;
			double iInRad = 0;
			IPoint ptCur = null;
			IPoint pPtGeo = null;

			ptCur = new ESRI.ArcGIS.Geometry.Point();
			pPtGeo = new ESRI.ArcGIS.Geometry.Point();
			createArcPrjReturn = new Polygon();

			dX = ptFrom.X - ptCnt.X;
			dY = ptFrom.Y - ptCnt.Y;
			R = System.Math.Sqrt(dX * dX + dY * dY);

			AztFrom = GlobalVars.RadToDegValue * NativeMethods.ATan2(dY, dX);
			AztFrom = NativeMethods.Modulus(AztFrom, 360.0);

			AztTo = GlobalVars.RadToDegValue * NativeMethods.ATan2(ptTo.Y - ptCnt.Y, ptTo.X - ptCnt.X);
			AztTo = NativeMethods.Modulus(AztTo, 360.0);

			daz = NativeMethods.Modulus((AztTo - AztFrom) * ClWise, 360.0);

			I = System.Convert.ToInt32(System.Math.Ceiling(daz));

			if (I < 1)
			{
				I = 1;
			}
			else if (I < 5)
			{
				I = 5;
			}
			else if (I < 10)
			{
				I = 10;
			}

			AngStep = daz / I;

			createArcPrjReturn.AddPoint(ptFrom);
			for (J = 1; J <= I - 1; J++)
			{
				iInRad = GlobalVars.DegToRadValue * (AztFrom + J * AngStep * ClWise);
				pPtGeo.X = ptCnt.X + R * System.Math.Cos(iInRad);
				pPtGeo.Y = ptCnt.Y + R * System.Math.Sin(iInRad);
				createArcPrjReturn.AddPoint(pPtGeo);
			}

			createArcPrjReturn.AddPoint(ptTo);
			return createArcPrjReturn as IPolygon;
		}

		public static IElement DrawPolygonSFS(IPolygon pPolygon, ISimpleFillSymbol pFillSym, bool drawFlg = true)
		{
			ESRI.ArcGIS.Carto.IElement drawPolygonSFSReturn = null;
			IFillShapeElement pFillShpElement = null;
			IElement pElementofPoly = null;

			pFillShpElement = ((ESRI.ArcGIS.Carto.IFillShapeElement)(new PolygonElement()));
			pElementofPoly = ((ESRI.ArcGIS.Carto.IElement)(pFillShpElement));
			pElementofPoly.Geometry = ((ESRI.ArcGIS.Geometry.IGeometry)(pPolygon));
			pFillShpElement.Symbol = pFillSym;
			drawPolygonSFSReturn = pElementofPoly;

			IGraphicsContainer pGraphics = null;
			if (drawFlg)
			{
				pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(pElementofPoly, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return drawPolygonSFSReturn;
		}

		public static IElement DrawPolygonSFS(IPointCollection pPolygon, ISimpleFillSymbol pFillSym, bool drawFlg = true)
		{
			return DrawPolygonSFS(pPolygon as IPolygon, pFillSym, drawFlg);
		}

		//public static void ConsiderObs(ObstacleType[] PtOutList, IPoint ptCenter, RWYType DER, double fRadius, ref double MaxDist, ref int ObsNum)
		//{
		//    int I = 0;
		//    int N = 0;
		//    double fDist = 0;
		//    double AztDir = 0;
		//    IPoint ptCur = null;

		//    MaxDist = 0.0;
		//    ObsNum = 0;

		//    if (fRadius == 0)
		//        return;

		//    AztDir = DER.pPtPrj[eRWY.PtEnd].M; // Azt2Dir(DER.pPtGeo(ptEnd), DER.pPtGeo(ptEnd).M)
		//    N = PtOutList.GetUpperBound(0);

		//    for (I = 0; I <= N; I++)
		//    {
		//        ptCur = PtOutList[I].pPtPrj;

		//        PtOutList[I].Height = ptCur.Z - DER.pPtPrj[eRWY.PtEnd].Z + PtOutList[I].VertAccuracy;
		//        PtOutList[I].CLShift = Functions.Point2LineDistancePrj(ptCur, DER.pPtPrj[eRWY.PtEnd], AztDir);

		//        fDist = Functions.ReturnDistanceInMeters(ptCur, ptCenter);
		//        if (fDist <= fRadius)
		//        {
		//            ObsNum++;
		//            if (fDist > MaxDist)
		//                MaxDist = fDist;
		//        }
		//    }
		//}

		public static double Point2LineDistancePrj(IPoint PtTest, IPoint PtLine, double dirAngle)
		{
			dirAngle = GlobalVars.DegToRadValue * dirAngle;
			double CosA = System.Math.Cos(dirAngle);
			double SinA = System.Math.Sin(dirAngle);

			double dX = PtTest.X - PtLine.X;
			double dY = PtTest.Y - PtLine.Y;
			return System.Math.Abs(dY * CosA - dX * SinA);
		}

		public static double ReturnAngleInDegrees(IPoint ptFrom, IPoint ptTo)
		{
			double returnAngleInDegreesReturn = 0;
			double fdX = 0, fdY = 0;
			fdX = ptTo.X - ptFrom.X;
			fdY = ptTo.Y - ptFrom.Y;
			returnAngleInDegreesReturn = NativeMethods.Modulus(RadToDeg(NativeMethods.ATan2(fdY, fdX)), 360.0);
			return returnAngleInDegreesReturn;
		}

		public static double ReturnDistanceInMeters(IPoint ptFrom, IPoint ptTo)
		{
			double returnDistanceInMetersReturn = 0;
			double fdX = 0;
			double fdY = 0;

			fdX = ptTo.X - ptFrom.X;
			fdY = ptTo.Y - ptFrom.Y;
			returnDistanceInMetersReturn = System.Math.Sqrt(fdX * fdX + fdY * fdY);
			return returnDistanceInMetersReturn;
		}

		public static IPoint PointAlongPlane(IPoint ptFrom, double dirAngle, double Dist)
		{
			IClone pClone = ptFrom as IClone;
			IPoint pointAlongPlaneReturn = pClone.Clone() as IPoint;

			dirAngle = GlobalVars.DegToRadValue * dirAngle;
			double CosA = System.Math.Cos(dirAngle);
			double SinA = System.Math.Sin(dirAngle);

			pointAlongPlaneReturn.X = ptFrom.X + Dist * CosA;
			pointAlongPlaneReturn.Y = ptFrom.Y + Dist * SinA;

			return pointAlongPlaneReturn;
		}

		public static double SubtractAngles(double X, double Y)
		{
			double subtractAnglesReturn = 0;
			X = NativeMethods.Modulus(X, 360.0);
			Y = NativeMethods.Modulus(Y, 360.0);
			subtractAnglesReturn = NativeMethods.Modulus(X - Y, 360.0);
			if (subtractAnglesReturn > 180.0)
			{
				subtractAnglesReturn = 360.0 - subtractAnglesReturn;
			}
			return subtractAnglesReturn;
		}



		public static void RefreshCommandBar(ICommandItem[] mBar, int iFlag = 0xFFFF)
		{
			int J = 1;

			for (int I = 0; I < mBar.Length; I++)
			{
				if ((iFlag & J) != 0)
				{
					if (!((mBar[I] == null)))
					{
						mBar[I].Refresh();
					}
				}
				J = J * 2;
			}
		}

		public static void SortIntervals(Interval[] Intervals, bool RightSide = false)
		{
			int I = 0;
			int J = 0;
			int N = 0;
			Interval Tmp;

			System.Array transTemp10 = Intervals;
			N =  /* TRANSINFO: .NET Equivalent of Microsoft.VisualBasic NameSpace */ transTemp10.GetUpperBound(0);

			for (I = 0; I <= N - 1; I++)
			{
				for (J = I + 1; J <= N; J++)
				{
					if (RightSide)
					{
						if (Intervals[I].Right_Renamed > Intervals[J].Right_Renamed)
						{
							Tmp = Intervals[I];
							Intervals[I] = Intervals[J];
							Intervals[J] = Tmp;
						}
					}
					else
					{
						if (Intervals[I].Left_Renamed > Intervals[J].Left_Renamed)
						{
							Tmp = Intervals[I];
							Intervals[I] = Intervals[J];
							Intervals[J] = Tmp;
						}
					}
				}
			}
		}

		//public static void GetObstInRange(ObstacleType[] ObstSource, ref ObstacleType[] ObstDest, double Range)
		//{
		//    int N = 0;
		//    int I = 0;
		//    int J = 0;

		//    N = ObstSource.GetUpperBound(0);
		//    if (N >= 0)
		//        ObstDest = GlobalVars.InitArray<ObstacleType>(N + 1);    //--- Checked
		//    else
		//    {
		//        ObstDest = new ObstacleType[0];
		//        return;
		//    }

		//    J = -1;
		//    for (I = 0; I <= N; I++)
		//    {
		//        if (ObstSource[I].Dist > Range)
		//            break;

		//        J = J + 1;
		//        ObstDest[J] = ObstSource[I];
		//    }

		//    if (J < 0)
		//        ObstDest = new ObstacleType[0];
		//    else
		//        System.Array.Resize<ObstacleType>(ref ObstDest, J + 1);
		//}

		public static double IAS2TAS(double IAS, double h, double dT)
		{
			if ((h >= 288.0 / 0.006496) || (h >= (288.0 + dT) / 0.006496))
			{
				return 2.0 * IAS;
				//     h = Int(288.0 / 0.006496)
			}
			else
				return IAS * 171233.0 * System.Math.Sqrt(288.0 + dT - 0.006496 * h) / (Math.Pow((288.0 - 0.006496 * h), 2.628));
		}

		public static double Bank2Radius(double Bank, double V)
		{
			double Rv = 0;

			Rv = 6.355 * System.Math.Tan(GlobalVars.DegToRadValue * Bank) / (GlobalVars.PI * V);

			if (Rv > 0.003)
				Rv = 0.003;

			if (Rv > 0)
				return V / (20.0 * GlobalVars.PI * Rv);
			else
				return -1;
		}

		public static double Radius2Bank(double R, double V)
		{
			if (R > 0.0)
				return GlobalVars.RadToDegValue * (System.Math.Atan(V * V / (20.0 * R * 6.355)));

			return -1;
		}

		public static IPointCollection ReturnPolygonPartAsPolyline(IPointCollection pPolygon, IPoint PtDerL, double CLDir, int Turn)
		{
			ESRI.ArcGIS.Geometry.IPointCollection returnPolygonPartAsPolylineReturn = null;
			int I = 0;
			int N = 0;
			int Side = 0;
			IPolyline pLine = null;
			IPoint pPt = null;
			IPointCollection pTmpPoly = null;

			pTmpPoly = RemoveAgnails(pPolygon as IPolygon);
			pTmpPoly = ReArrangePolygon(pTmpPoly, PtDerL, CLDir, false);

			pPt = Functions.PointAlongPlane(PtDerL, CLDir + 180.0, 30000.0);
			returnPolygonPartAsPolylineReturn = new Polyline();
			N = pTmpPoly.PointCount - 1;

			for (I = 0; I <= N; I++)
			{
				Side = Functions.SideDef(pPt, CLDir, pTmpPoly.get_Point(I));
				if (Side == Turn)
				{
					returnPolygonPartAsPolylineReturn.AddPoint(pTmpPoly.get_Point(I));
				}
			}

			if (Turn < 0)
			{
				pLine = ((ESRI.ArcGIS.Geometry.IPolyline)(returnPolygonPartAsPolylineReturn));
				pLine.ReverseOrientation();
			}
			return returnPolygonPartAsPolylineReturn;
		}

		public static IPointCollection ReArrangePolygon(IPointCollection pPolygon, IPoint ptBase, double BaseDir, bool bFlag = false)
		{
			ESRI.ArcGIS.Geometry.IPointCollection reArrangePolygonReturn = null;
			int I = 0;
			int J = 0;

			int N = 0;
			int iStart = 0;
			int Side = 0;

			double dl = 0;
			double dm = 0;

			double dX0 = 0;
			double dY0 = 0;

			double dX1 = 0;
			double dY1 = 0;
			IPointCollection pPoly = null;
			ITopologicalOperator2 pTopoOper = null;

			IPoint pPt = null;

			pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPolygon));
			pTopoOper.IsKnownSimple_2 = false;
			pTopoOper.Simplify();

			if (pPolygon.PointCount < 4)
			{
				return pPolygon;
			}

			pPt = Functions.PointAlongPlane(ptBase, BaseDir + 180.0, 30000.0);

			pPoly = new Polyline();
			pPoly.AddPointCollection(pPolygon);

			N = pPoly.PointCount - 1;
			pPoly.RemovePoints(N, 1);

			iStart = -1;

			Side = Functions.SideDef(pPt, BaseDir, pPoly.get_Point(0));
			if (Side < 0)
			{
				dm = Functions.Point2LineDistancePrj(pPoly.get_Point(0), pPt, BaseDir + 90.0);
				iStart = 0;
			}

			for (I = 1; I <= N - 1; I++)
			{
				Side = Functions.SideDef(pPt, BaseDir, pPoly.get_Point(I));
				if (Side < 0)
				{
					dl = Functions.Point2LineDistancePrj(pPoly.get_Point(I), pPt, BaseDir + 90.0);
					if ((dl < dm) || (iStart < 0))
					{
						dm = dl;
						iStart = I;
					}
				}
			}

			if (bFlag)
			{
				if (iStart == 0)
				{
					iStart = N - 1;
				}
				else
				{
					iStart = iStart - 1;
				}
			}

			dX0 = pPoly.get_Point(1).X - pPoly.get_Point(0).X;
			dY0 = pPoly.get_Point(1).Y - pPoly.get_Point(0).Y;
			I = 1;

			while (I < N)
			{
				J = (I + 1) % (N - 1);
				dX1 = pPoly.get_Point(J).X - pPoly.get_Point(I).X;
				dY1 = pPoly.get_Point(J).Y - pPoly.get_Point(I).Y;
				dl = Functions.ReturnDistanceInMeters(pPoly.get_Point(J), pPoly.get_Point(I));

				if (dl < 0.001)
				{
					pPoly.RemovePoints(I, 1);
					N = N - 1;
					J = System.Convert.ToInt32(NativeMethods.Modulus(I + 1, N));
					if (I <= iStart)
					{
						iStart = iStart - 1;
					}

					dX1 = dX0;
					dY1 = dY0;
				}
				else if ((dY0 != 0.0) && (I != iStart))
				{
					if (dY1 != 0.0)
					{
						if (System.Math.Abs(System.Math.Abs(dX0 / dY0) - System.Math.Abs(dX1 / dY1)) < 0.00001)
						{
							pPoly.RemovePoints(I, 1);
							N = N - 1;
							J = (I + 1) % N;
							if (I <= iStart)
							{
								iStart = iStart - 1;
							}

							dX1 = dX0;
							dY1 = dY0;
						}
						else
						{
							I = I + 1;
						}
					}
					else
					{
						I = I + 1;
					}
				}
				else if ((dX0 != 0.0) && (I != iStart))
				{
					if (dX1 != 0.0)
					{
						if (System.Math.Abs(System.Math.Abs(dY0 / dX0) - System.Math.Abs(dY1 / dX1)) < 0.00001)
						{
							pPoly.RemovePoints(I, 1);
							N = N - 1;
							J = (I + 1) % N;
							if (I <= iStart)
							{
								iStart = iStart - 1;
							}

							dX1 = dX0;
							dY1 = dY0;
						}
						else
						{
							I = I + 1;
						}
					}
					else
					{
						I = I + 1;
					}
				}
				else
				{
					I = I + 1;
				}
				dX0 = dX1;
				dY0 = dY1;
			}

			N = pPoly.PointCount;
			reArrangePolygonReturn = new Polygon();

			for (I = N - 1; I >= 0; I += -1)
			{
				J = System.Convert.ToInt32(NativeMethods.Modulus(I + iStart, N));
				reArrangePolygonReturn.AddPoint(pPoly.get_Point(J));
			}

			// DrawPolygon ReArrangePolygon, 255
			// Set pPoly = New Polyline
			// pPoly.re
			// pPoly.ReverseOrientation
			return reArrangePolygonReturn;
		}

		//public static double CalcSpiralStartPoint(IPointCollection LinePoint, ref ObstacleType DetObs, double coef, double r0, double DepDir, double AztDir, int TurnDir, bool bDerFlg)
		//{
		//    double calcSpiralStartPointReturn = 0;
		//    IPoint[] BasePoints = null;
		//    IPoint ptTmp2 = null;
		//    IPoint PtTurn = null;
		//    IPoint ptCnt = null;
		//    IPoint ptTmp = null;
		//    ILine pLine = null;

		//    IConstructPoint pConstructor = null;
		//    IProximityOperator pProxi = null;

		//    double ASinAlpha = 0;
		//    double MaxTheta = 0;
		//    double dAlpha = 0;
		//    double alpha = 0;
		//    double Theta = 0;
		//    double hTMin = 0;
		//    double Dist = 0;
		//    double fTmp = 0;
		//    double hT = 0;

		//    int Offset = 0;
		//    int N = 0;
		//    int I = 0;
		//    int iMin = 0;
		//    int Side1 = 0;
		//    int Side = 0;


		//    if (bDerFlg)
		//    {
		//        Offset = 1;
		//    }
		//    else
		//    {
		//        Offset = 0;
		//    }

		//    pLine = new Line();
		//    pProxi = ((ESRI.ArcGIS.Geometry.IProximityOperator)(pLine));

		//    N = LinePoint.PointCount - Offset;

		//    if (N < 2)
		//    {
		//        DetObs.CourseAdjust = GlobalVars.NO_VALUE;
		//        return -100;
		//    }

		//    if (N > 0)
		//    {
		//        BasePoints = new ESRI.ArcGIS.Geometry.IPoint[N + 1 + 1]; //--- Checked
		//    }
		//    else
		//    {
		//        BasePoints = new ESRI.ArcGIS.Geometry.IPoint[0];
		//    }

		//    for (I = 0; I <= N - 1; I++)
		//    {
		//        BasePoints[I] = LinePoint.get_Point(I + Offset);

		//        if (I == N - 1)
		//        {
		//            BasePoints[I].M = BasePoints[I - 1].M;
		//        }
		//        else
		//        {
		//            BasePoints[I].M = Functions.ReturnAngleInDegrees(LinePoint.get_Point(I + Offset), LinePoint.get_Point(I + Offset + 1));
		//        }
		//    }

		//    ptCnt = new ESRI.ArcGIS.Geometry.Point();
		//    pConstructor = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptCnt));

		//    PtTurn = null;

		//    hTMin = GlobalVars.RModel;
		//    iMin = -1;

		//    MaxTheta = SpiralTouchAngle(r0, coef, DepDir, DepDir + 90.0 * TurnDir, TurnDir);
		//    if (MaxTheta > 180.0)
		//    {
		//        MaxTheta = 360.0 - MaxTheta;
		//    }

		//    for (I = 0; I <= N - 2; I++)
		//    {
		//        Side = Functions.SideDef(BasePoints[I], (BasePoints[I].M), DetObs.pPtPrj);
		//        alpha = DepDir + 90.0 * Side;

		//        if (System.Math.Abs(System.Math.Sin(GlobalVars.DegToRadValue * (alpha - BasePoints[I].M))) > GlobalVars.degEps)
		//        {

		//            dAlpha = Functions.SubtractAngles(alpha, BasePoints[I].M);
		//            ptTmp = Functions.PointAlongPlane(BasePoints[I], DepDir - 90.0 * Side, r0);

		//            Dist = Functions.Point2LineDistancePrj(DetObs.pPtPrj, ptTmp, BasePoints[I].M);
		//            Side1 = Functions.SideDef(ptTmp, BasePoints[I].M, DetObs.pPtPrj);

		//            Theta = 0.5 * MaxTheta;
		//            do
		//            {
		//                fTmp = Theta;
		//                ASinAlpha = Dist / (r0 + Theta * coef);
		//                if (System.Math.Abs(ASinAlpha) <= 1.0)
		//                    Theta = dAlpha - GlobalVars.RadToDegValue * (Side1 * TurnDir * ArcSin(ASinAlpha));
		//                else
		//                {
		//                    Theta = MaxTheta;
		//                    break;
		//                }
		//            }
		//            while (System.Math.Abs(fTmp - Theta) > GlobalVars.degEps);

		//            fTmp = System.Math.Sin(GlobalVars.DegToRadValue * Theta) * (r0 + Theta * coef);

		//            if (Theta > MaxTheta)
		//            {
		//                hT = System.Math.Sin(GlobalVars.DegToRadValue * MaxTheta) * (r0 + MaxTheta * coef);
		//                fTmp = (hT - fTmp);
		//                Theta = MaxTheta;
		//            }
		//            else
		//            {
		//                hT = fTmp;
		//                fTmp = 0.0;
		//            }

		//            ptTmp2 = Functions.PointAlongPlane(DetObs.pPtPrj, DepDir + 180.0, hT + fTmp);
		//            pConstructor.ConstructAngleIntersection(ptTmp2, GlobalVars.DegToRadValue * (DepDir + 90.0), ptTmp, GlobalVars.DegToRadValue * BasePoints[I].M);

		//            ptTmp = Functions.PointAlongPlane(ptCnt, DepDir - TurnDir * 90.0, r0);

		//            pLine.FromPoint = BasePoints[I];
		//            pLine.ToPoint = BasePoints[I + 1];

		//            fTmp = pProxi.ReturnDistance(ptTmp);

		//            if (fTmp < GlobalVars.distEps)
		//            {
		//                if (hT < hTMin)
		//                {
		//                    hTMin = hT;
		//                    iMin = I;
		//                    PtTurn = ptTmp;
		//                    PtTurn.M = Theta;
		//                    PtTurn.Z = DetObs.Dist - hTMin;
		//                    if ((PtTurn.Z < 0.0))
		//                    {
		//                        PtTurn.Z = 0.0;
		//                    }
		//                }
		//            }
		//        }
		//    }

		//    if (iMin > -1)
		//    {
		//        calcSpiralStartPointReturn = PtTurn.Z;
		//        DetObs.CourseAdjust = PtTurn.M;
		//    }
		//    else
		//    {
		//        calcSpiralStartPointReturn = -100.0;
		//        DetObs.CourseAdjust = -9999.0;
		//    }
		//    return calcSpiralStartPointReturn;
		//}

		public static double SpiralTouchAngle(double r0, double coef0, double aztNominal, double aztTouch, int TurnDir)
		{
			double spiralTouchAngleReturn = 0;
			int I = 0;
			double d = 0;
			double coef = 0;
			double delta = 0;
			double turnAngle = 0;
			double TouchAngle = 0;

			TouchAngle = NativeMethods.Modulus((aztTouch - aztNominal) * TurnDir, 360.0);
			TouchAngle = GlobalVars.DegToRadValue * TouchAngle;
			turnAngle = TouchAngle;
			coef = GlobalVars.RadToDegValue * coef0;

			for (I = 0; I <= 9; I++)
			{
				d = coef / (r0 + coef * turnAngle);
				delta = (turnAngle - TouchAngle - System.Math.Atan(d)) / (2.0 - 1.0 / (d * d + 1.0));
				turnAngle = turnAngle - delta;
				if ((System.Math.Abs(delta) < GlobalVars.radEps))
					break;
			}

			spiralTouchAngleReturn = GlobalVars.RadToDegValue * turnAngle;
			if (spiralTouchAngleReturn < 0.0)
				spiralTouchAngleReturn = NativeMethods.Modulus(spiralTouchAngleReturn, 360.0);

			return spiralTouchAngleReturn;
		}

		public static double ArcSin(double X)
		{
			double arcSinReturn = 0;
			if ((System.Math.Abs(X) >= 1.0) && (System.Math.Abs(X) <= 1.001))
			{
				if (X > 0)
				{
					arcSinReturn = 0.5 * GlobalVars.PI;
				}
				else
				{
					arcSinReturn = -0.5 * GlobalVars.PI;
				}
			}
			else
			{
				arcSinReturn = System.Math.Asin(X);
			}
			return arcSinReturn;
		}

		public static double ArcCos(double X)
		{
			double arcCosReturn = 0;
			if ((System.Math.Abs(X) >= 1.0) && (System.Math.Abs(X) <= 1.001))
			{
				arcCosReturn = 0.0;
			}
			else
			{
				arcCosReturn = System.Math.Acos(X); // System.Math.Atan(-X / System.Math.Sqrt(-X * X + 1)) + 0.5 * PI
			}
			return arcCosReturn;
		}

		public static double Max(double X, double Y)
		{
			double maxReturn = 0;
			if (X > Y)
			{
				maxReturn = X;
			}
			else
			{
				maxReturn = Y;
			}
			return maxReturn;
		}

		public static double Min(double X, double Y)
		{
			double minReturn = 0;
			if (X < Y)
			{
				minReturn = X;
			}
			else
			{
				minReturn = Y;
			}
			return minReturn;
		}

		public static double FixToTouchSpiral(IPoint ptBase, double coef0, double TurnR, int TurnDir,
												double Theta, IPoint FixPnt, double DepCourse)
		{
			double fixToTouchSpiralReturn = 0;
			int I = 0;

			double R = 0;
			double X1 = 0;
			double Y1 = 0;
			double Theta0 = 0;
			double Theta1 = 0;
			double fTmp1 = 0;
			double fTmp2 = 0;
			double Theta1New = 0;
			double coef = 0;
			double Dist = 0;
			double FixTheta = 0;
			double dTheta = 0;
			double CntTheta = 0;

			double f = 0;
			double F1 = 0;
			double SinT = 0;
			double CosT = 0;

			IPoint PtCntSpiral = null;
			IPoint ptOut = null;

			fixToTouchSpiralReturn = -1000;

			coef = GlobalVars.RadToDegValue * coef0;
			Theta0 = NativeMethods.Modulus(90.0 * TurnDir + DepCourse + 180.0, 360.0);
			PtCntSpiral = Functions.PointAlongPlane(ptBase, DepCourse + 90.0 * TurnDir, TurnR);
			Dist = Functions.ReturnDistanceInMeters(PtCntSpiral, FixPnt);
			FixTheta = Functions.ReturnAngleInDegrees(PtCntSpiral, FixPnt);
			dTheta = NativeMethods.Modulus((FixTheta - Theta0) * TurnDir, 360.0);
			R = TurnR + dTheta * coef0;
			if (Dist < R)
			{
				return fixToTouchSpiralReturn;
			}

			X1 = FixPnt.X - PtCntSpiral.X;
			Y1 = FixPnt.Y - PtCntSpiral.Y;
			CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, Theta, TurnDir);
			CntTheta = NativeMethods.Modulus(Theta0 + CntTheta * TurnDir, 360.0);
			// ===============================Variant Fiddowsy ==================================

			Theta1 = CntTheta;
			for (I = 0; I <= 20; I++)
			{
				dTheta = NativeMethods.Modulus((Theta1 - Theta0) * TurnDir, 360.0);
				SinT = System.Math.Sin(GlobalVars.DegToRadValue * Theta1);
				CosT = System.Math.Cos(GlobalVars.DegToRadValue * Theta1);
				R = TurnR + dTheta * coef0;
				f = R * R - (Y1 * R + X1 * coef * TurnDir) * SinT - (X1 * R - Y1 * coef * TurnDir) * CosT;
				F1 = 2 * R * coef * TurnDir - (Y1 * R + 2 * X1 * coef * TurnDir) * CosT + (X1 * R - 2 * Y1 * coef * TurnDir) * SinT;
				Theta1New = Theta1 - GlobalVars.RadToDegValue * (f / F1);

				fTmp1 = Functions.SubtractAngles(Theta1New, Theta1);
				if (fTmp1 < 0.0001)
				{
					Theta1 = Theta1New;
					break;
				}
				Theta1 = Theta1New;
			}

			dTheta = NativeMethods.Modulus((Theta1 - Theta0) * TurnDir, 360.0);
			R = TurnR + dTheta * coef0;

			ptOut = Functions.PointAlongPlane(PtCntSpiral, Theta1, R);
			fTmp1 = Functions.ReturnAngleInDegrees(ptOut, FixPnt);
			CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, TurnDir);
			CntTheta = NativeMethods.Modulus(Theta0 + CntTheta * TurnDir, 360.0);
			fTmp2 = Functions.SubtractAngles(CntTheta, Theta1);

			if (fTmp2 < 0.0001)
			{
				fixToTouchSpiralReturn = fTmp1;
				return fixToTouchSpiralReturn;
			}

			// Exit Function
			// '=============================11==================================
			// Dim Theta11 As Double
			// Dim Theta12 As Double
			// Dim Theta21 As Double
			// Dim Theta22 As Double
			// Dim Theta2New As Double
			// Dim SinCoef As Double
			// Dim CosCoef As Double
			// Dim sin1 As Double
			// Dim A As Double
			// Dim B As Double
			// Dim C As Double
			// Dim d As Double
			// Dim SolFlag11 As Boolean
			// Dim SolFlag12 As Boolean
			// Dim SolFlag21 As Boolean
			// Dim SolFlag22 As Boolean
			// Theta1 = CntTheta
			// SolFlag11 = False

			// For I = 0 To 20
			// 	dTheta = NativeMethods.Modulus ((Theta1 - Theta0) * TurnDir, 360.0)
			// 	'SinCoef*SinX+CosCoef*CosX = 1
			// 	R = TurnR + dTheta * coef0
			// 	SinCoef = (Y1 + TurnDir * coef * X1 / R) / R
			// 	CosCoef = (X1 - TurnDir * coef * Y1 / R) / R
			// 	'a*x2 + b*x + c = 0
			// 	A = SinCoef * SinCoef + CosCoef * CosCoef
			// 	B = -2 * SinCoef
			// 	C = 1 - CosCoef * CosCoef
			// 	d = B * B - 4.0 * A * C
			// 	If d < 0.0 Then
			// 		Theta1New = -d * System.Math.Sign(-B * A) * 90.0
			// 	Else
			// 		sin1 = (-B + System.Math.Sqrt(d)) / (2.0 * A)
			// 		Theta1New = RadToDegValue * (ArcSin(sin1))
			// 	End If

			// 	fTmp1 = Functions.SubtractAngles(Theta1New, Theta1)
			// 	If fTmp1 < 0.0001 Then
			// 		SolFlag11 = True
			// 		Theta11 = Theta1
			// 		Exit For
			// 	End If
			// 	Theta1 = Theta1New
			// Next I
			// '=============================12==================================
			// Theta1 = CntTheta
			// SolFlag12 = False

			// For I = 0 To 20
			// 	dTheta = NativeMethods.Modulus ((Theta1 - Theta0) * TurnDir, 360.0)
			// 	'SinCoef*SinX+CosCoef*CosX = 1
			// 	R = TurnR + dTheta * coef0
			// 	SinCoef = (Y1 + TurnDir * coef * X1 / R) / R
			// 	CosCoef = (X1 - TurnDir * coef * Y1 / R) / R
			// 	'a*x2 + b*x + c = 0
			// 	A = SinCoef * SinCoef + CosCoef * CosCoef
			// 	B = -2 * SinCoef
			// 	C = 1 - CosCoef * CosCoef
			// 	d = B * B - 4.0 * A * C
			// 	If d < 0.0 Then
			// 		Theta1New = 180.0 + d * System.Math.Sign(-B * A) * 90.0
			// 	Else
			// 		sin1 = (-B + System.Math.Sqrt(d)) / (2.0 * A)
			// 		Theta1New = 180.0 - RadToDegValue * (ArcSin(sin1))
			// 	End If

			// 	fTmp1 = Functions.SubtractAngles(Theta1New, Theta1)
			// 	If fTmp1 < 0.0001 Then
			// 		SolFlag12 = True
			// 		Theta12 = Theta1
			// 		Exit For
			// 	End If
			// 	Theta1 = Theta1New
			// Next I
			// '=============================21==================================
			// Theta1 = CntTheta
			// SolFlag21 = False

			// For I = 0 To 20
			// 	dTheta = NativeMethods.Modulus ((Theta1 - Theta0) * TurnDir, 360.0)
			// 	'SinCoef*SinX+CosCoef*CosX = 1
			// 	R = TurnR + dTheta * coef0
			// 	SinCoef = (Y1 + TurnDir * coef * X1 / R) / R
			// 	CosCoef = (X1 - TurnDir * coef * Y1 / R) / R
			// 	'a*x2 + b*x + c = 0
			// 	A = SinCoef * SinCoef + CosCoef * CosCoef
			// 	B = -2 * SinCoef
			// 	C = 1 - CosCoef * CosCoef
			// 	d = B * B - 4.0 * A * C
			// 	If d < 0.0 Then
			// 		Theta1New = -d * System.Math.Sign(-B * A) * 90.0
			// 	Else
			// 		sin1 = (-B - System.Math.Sqrt(d)) / (2.0 * A)
			// 		Theta1New = RadToDegValue * (ArcSin(sin1))
			// 	End If

			// 	fTmp1 = Functions.SubtractAngles(Theta1New, Theta1)
			// 	If fTmp1 < 0.0001 Then
			// 		SolFlag21 = True
			// 		Theta21 = Theta1
			// 		Exit For
			// 	End If
			// 	Theta1 = Theta1New
			// Next I
			// '=============================22==================================
			// Theta1 = CntTheta + 180.0
			// SolFlag22 = False

			// For I = 0 To 20
			// 	dTheta = NativeMethods.Modulus ((Theta1 - Theta0) * TurnDir, 360.0)
			// 	'SinCoef*SinX+CosCoef*CosX = 1
			// 	R = TurnR + dTheta * coef0
			// 	SinCoef = (Y1 + TurnDir * coef * X1 / R) / R
			// 	CosCoef = (X1 - TurnDir * coef * Y1 / R) / R
			// 	'a*x2 + b*x + c = 0
			// 	A = SinCoef * SinCoef + CosCoef * CosCoef
			// 	B = -2 * SinCoef
			// 	C = 1 - CosCoef * CosCoef
			// 	d = B * B - 4.0 * A * C
			// 	If d < 0.0 Then
			// 		Theta1New = 180.0 + d * System.Math.Sign(-B * A) * 90.0
			// 	Else
			// 		sin1 = (-B - System.Math.Sqrt(d)) / (2.0 * A)
			// 		Theta1New = 180.0 - RadToDegValue * (ArcSin(sin1))
			// 	End If

			// 	fTmp1 = Functions.SubtractAngles(Theta1New, Theta1)
			// 	If fTmp1 < 0.0001 Then
			// 		SolFlag22 = True
			// 		Theta22 = Theta1
			// 		Exit For
			// 	End If
			// 	Theta1 = Theta1New
			// Next I
			// '================================11=====================================
			// If SolFlag11 Then
			// 	dTheta = NativeMethods.Modulus ((Theta11 - Theta0) * TurnDir, 360.0)
			// 	R = TurnR + dTheta * coef0
			// 	ptOut = Functions.PointAlongPlane(PtCntSpiral, Theta11, R)
			// 	fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
			// 	CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, TurnDir)
			// 	CntTheta = NativeMethods.Modulus (Theta0 + CntTheta * TurnDir, 360.0)
			// 	fTmp2 = Functions.SubtractAngles(CntTheta, Theta11)
			// 	If fTmp2 < 0.0001 Then
			// 		FixToTouchSpiral = fTmp1
			// 		Exit Function
			// 	End If
			// End If
			// '================================12=====================================
			// If SolFlag12 Then
			// 	dTheta = NativeMethods.Modulus ((Theta12 - Theta0) * TurnDir, 360.0)
			// 	R = TurnR + dTheta * coef0
			// 	ptOut = Functions.PointAlongPlane(PtCntSpiral, Theta12, R)
			// 	fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
			// 	CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, TurnDir)
			// 	CntTheta = NativeMethods.Modulus (Theta0 + CntTheta * TurnDir, 360.0)
			// 	fTmp2 = Functions.SubtractAngles(CntTheta, Theta12)
			// 	If fTmp2 < 0.0001 Then
			// 		FixToTouchSpiral = fTmp1
			// 		Exit Function
			// 	End If
			// End If
			// '================================21=====================================
			// If SolFlag21 Then
			// 	dTheta = NativeMethods.Modulus ((Theta21 - Theta0) * TurnDir, 360.0)
			// 	R = TurnR + dTheta * coef0
			// 	ptOut = Functions.PointAlongPlane(PtCntSpiral, Theta21, R)
			// 	fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
			// 	CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, TurnDir)
			// 	CntTheta = NativeMethods.Modulus (Theta0 + CntTheta * TurnDir, 360.0)
			// 	fTmp2 = Functions.SubtractAngles(CntTheta, Theta21)
			// 	If fTmp2 < 0.0001 Then
			// 		FixToTouchSpiral = fTmp1
			// 		Exit Function
			// 	End If
			// End If
			// '================================22=====================================
			// If SolFlag22 Then
			// 	dTheta = NativeMethods.Modulus ((Theta22 - Theta0) * TurnDir, 360.0)
			// 	R = TurnR + dTheta * coef0
			// 	ptOut = Functions.PointAlongPlane(PtCntSpiral, Theta22, R)
			// 	fTmp1 = ReturnAngleInDegrees(ptOut, FixPnt)
			// 	CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp1, TurnDir)
			// 	CntTheta = NativeMethods.Modulus (Theta0 + CntTheta * TurnDir, 360.0)
			// 	fTmp2 = Functions.SubtractAngles(CntTheta, Theta22)
			// 	If fTmp2 < 0.0001 Then
			// 		FixToTouchSpiral = fTmp1
			// 		Exit Function
			// 	End If
			// End If
			return fixToTouchSpiralReturn;
		}

		public static bool RayPolylineIntersect(IPointCollection pPolyline, IPoint RayPt, double RayDir, ref IPoint InterPt)
		{
			return RayPolylineIntersect(pPolyline as IPolyline, RayPt, RayDir, ref InterPt);
		}

		public static bool RayPolylineIntersect(IPolyline pPolyline, IPoint RayPt, double RayDir, ref IPoint InterPt)
		{
			bool rayPolylineIntersectReturn = false;
			int I = 0;
			int N = 0;
			double d = 0;
			double dMin = 0;
			IPolyline pLine = null;
			IPointCollection pPoints = null;
			ITopologicalOperator pTopo = null;

			pLine = ((ESRI.ArcGIS.Geometry.IPolyline)(new Polyline()));
			pLine.FromPoint = RayPt;
			dMin = 5000000.0;
			pLine.ToPoint = Functions.PointAlongPlane(RayPt, RayDir, dMin);

			pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator)(pPolyline));
			pPoints = ((ESRI.ArcGIS.Geometry.IPointCollection)(pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry0Dimension)));
			N = pPoints.PointCount;

			rayPolylineIntersectReturn = N > 0;

			if (N == 0)
			{
				return rayPolylineIntersectReturn;
			}

			if (N == 1)
			{
				InterPt = pPoints.get_Point(0);
			}
			else
			{
				for (I = 0; I <= N - 1; I++)
				{
					d = Functions.ReturnDistanceInMeters(RayPt, pPoints.get_Point(I));
					if (d < dMin)
					{
						dMin = d;
						InterPt = pPoints.get_Point(I);
					}
				}
			}
			return rayPolylineIntersectReturn;
		}

		public static Polygon RemoveFars(IPointCollection pPolygon, IPoint pPoint)
		{
			return RemoveFars(pPolygon as IPolygon, pPoint);
		}

		public static Polygon RemoveFars(IPolygon pPolygon, IPoint pPoint)
		{
			ESRI.ArcGIS.Geometry.Polygon removeFarsReturn = null;
			IGeometryCollection Geocollect = null;
			IGeometryCollection lCollect = null;
			IProximityOperator pProxi = null;
			double OutDist = 0;
			double tmpDist = 0;
			IClone pClone = null;
			int I = 0;
			int N = 0;

			pClone = ((ESRI.ArcGIS.esriSystem.IClone)(pPolygon));
			removeFarsReturn = ((ESRI.ArcGIS.Geometry.Polygon)(pClone.Clone()));
			Geocollect = ((ESRI.ArcGIS.Geometry.IGeometryCollection)(removeFarsReturn));
			N = Geocollect.GeometryCount;
			lCollect = ((ESRI.ArcGIS.Geometry.IGeometryCollection)(new Polygon()));

			if (N > 1)
			{
				pProxi = ((ESRI.ArcGIS.Geometry.IProximityOperator)(pPoint));
				OutDist = 20000000000.0;

				for (I = 0; I <= N - 1; I++)
				{
					lCollect.AddGeometry(Geocollect.get_Geometry(I));
					tmpDist = pProxi.ReturnDistance(lCollect as IGeometry);
					if (OutDist > tmpDist)
					{
						OutDist = tmpDist;
					}
					lCollect.RemoveGeometries(0, 1);
				}

				I = 0;
				while (I < N)
				{
					lCollect.AddGeometry(Geocollect.get_Geometry(I));
					tmpDist = pProxi.ReturnDistance(lCollect as IGeometry);
					if (OutDist < tmpDist)
					{
						Geocollect.RemoveGeometries(I, 1);
						N = N - 1;
					}
					else
					{
						I = I + 1;
					}
					lCollect.RemoveGeometries(0, 1);
				}
			}
			return removeFarsReturn;
		}

		public static IPolygon RemoveHoles(IPointCollection pPolygon)
		{
			return RemoveHoles(pPolygon as IPolygon);
		}

		public static IPolygon RemoveHoles(IPolygon pPolygon)
		{
			IPolygon removeHolesReturn = null;
			IGeometryCollection NewPolygon = null;
			IRing pInteriorRing = null;
			IClone pClone = null;
			int I = 0;

			pClone = ((IClone)(pPolygon));
			removeHolesReturn = ((IPolygon)(pClone.Clone()));
			NewPolygon = ((ESRI.ArcGIS.Geometry.IGeometryCollection)(removeHolesReturn));

			I = 0;
			while (I < NewPolygon.GeometryCount)
			{
				pInteriorRing = ((ESRI.ArcGIS.Geometry.IRing)(NewPolygon.get_Geometry(I)));
				if (!(pInteriorRing.IsExterior))
				{
					NewPolygon.RemoveGeometries(I, 1);
				}
				else
				{
					I = I + 1;
				}
			}
			return removeHolesReturn;
		}

		public static Polygon RemoveAgnails(IPointCollection pPolygon)
		{
			return RemoveAgnails(pPolygon as IPolygon);
		}

		public static Polygon RemoveAgnails(IPolygon pPolygon)
		{
			ESRI.ArcGIS.Geometry.Polygon removeAgnailsReturn = null;
			int J = 0;
			int K = 0;
			int L = 0;
			int N = 0;

			double dl = 0;
			double dXX = 0;
			double dYY = 0;

			double dX0 = 0;
			double dY0 = 0;
			double dX1 = 0;
			double dY1 = 0;

			ITopologicalOperator2 pTopo = null;
			IPointCollection pPoly = null;
			IPolygon2 pPGone = null;
			IClone pClone = null;

			pClone = ((ESRI.ArcGIS.esriSystem.IClone)(pPolygon));
			pPoly = ((ESRI.ArcGIS.Geometry.IPointCollection)(pClone.Clone()));

			pPGone = ((ESRI.ArcGIS.Geometry.IPolygon2)(pPoly));
			pPGone.Close();

			N = pPoly.PointCount - 1;

			if (N <= 3)
			{
				return ((ESRI.ArcGIS.Geometry.Polygon)(pPoly));
			}

			pPoly.RemovePoints(N, 1);

			J = 0;
			while (J < N)
			{
				if (N < 4)
					break;

				K = (J + 1) % N;
				L = (J + 2) % N;

				dX0 = pPoly.get_Point(K).X - pPoly.get_Point(J).X;
				dY0 = pPoly.get_Point(K).Y - pPoly.get_Point(J).Y;

				dX1 = pPoly.get_Point(L).X - pPoly.get_Point(K).X;
				dY1 = pPoly.get_Point(L).Y - pPoly.get_Point(K).Y;

				dXX = dX1 * dX1;
				dYY = dY1 * dY1;
				dl = dXX + dYY;

				if (dl < 0.00001)
				{
					pPoly.RemovePoints(K, 1);
					N = N - 1;
					if (J >= N)
					{
						J = N - 1;
					}
				}
				else if ((dYY > dXX))
				{
					if (System.Math.Abs(dX0 / dY0 - dX1 / dY1) < 0.00001)
					{
						pPoly.RemovePoints(K, 1);
						N = N - 1;
						J = (J - 2) % N;
						if (J < 0)
						{
							J = J + N;
						}
					}
					else
					{
						J = J + 1;
					}
				}
				else
				{
					if (System.Math.Abs(dY0 / dX0 - dY1 / dX1) < 0.00001)
					{
						pPoly.RemovePoints(K, 1);
						N = N - 1;
						J = (J - 2) % N;
						if (J < 0)
						{
							J = J + N;
						}
					}
					else
					{
						J = J + 1;
					}
				}
			}

			pPGone = ((ESRI.ArcGIS.Geometry.IPolygon2)(pPoly));
			pPGone.Close();

			pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPoly));
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			removeAgnailsReturn = ((ESRI.ArcGIS.Geometry.Polygon)(pPoly));
			return removeAgnailsReturn;
		}

		public static IElement DrawPolyLineSFS(IPolyline pLine, ISimpleLineSymbol pLineSym, bool drawFlg = true)
		{
			ESRI.ArcGIS.Carto.IElement drawPolyLineSFSReturn = null;
			ILineElement pLineElement = null;
			IElement pElementOfpLine = null;
			IGeometry pGeometry = null;

			// Dim pLine2 As ILine
			// Set pLine2 = pLine

			// Dim pTopoOperator As ITopologicalOperator
			// Dim pClone As IClone
			// Dim pLine1 As ILine
			// 
			// Set pClone = pLine
			// Set pLine1 = pClone.Clone
			// Set pTopoOperator = pLine1
			// pTopoOperator.Simplify
			// pTopoOperator.Clip p_LicenseEnvelop
			// 
			// If pLine1.IsEmpty Then
			//     MsgBox  "Out of licensed area"
			//     Exit Function
			// End If

			pLineElement = ((ESRI.ArcGIS.Carto.ILineElement)(new LineElement()));
			pElementOfpLine = ((ESRI.ArcGIS.Carto.IElement)(pLineElement));
			pGeometry = ((ESRI.ArcGIS.Geometry.IGeometry)(pLine));

			pElementOfpLine.Geometry = pGeometry;

			pLineElement.Symbol = pLineSym;
			drawPolyLineSFSReturn = pElementOfpLine;

			IGraphicsContainer pGraphics = null;
			if (drawFlg)
			{
				pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(pElementOfpLine, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}

			return drawPolyLineSFSReturn;
		}

		public static Interval[] IntervalsDifference(Interval A, Interval B)
		{
			Interval[] res = null;

			res = GlobalVars.InitArray<Interval>(1);    //--- Checked

			if ((B.Left_Renamed == B.Right_Renamed) || (B.Right_Renamed < A.Left_Renamed) || (A.Right_Renamed < B.Left_Renamed))
			{
				res[0] = A;
			}
			else if ((A.Left_Renamed < B.Left_Renamed) && (A.Right_Renamed > B.Right_Renamed))
			{
				res = GlobalVars.InitArray<Interval>(2);   //--- Checked
				res[0].Left_Renamed = A.Left_Renamed;
				res[0].Right_Renamed = B.Left_Renamed;
				res[1].Left_Renamed = B.Right_Renamed;
				res[1].Right_Renamed = A.Right_Renamed;
			}
			else if (A.Right_Renamed > B.Right_Renamed)
			{
				res[0].Left_Renamed = B.Right_Renamed;
				res[0].Right_Renamed = A.Right_Renamed;
			}
			else if ((A.Left_Renamed < B.Left_Renamed))
			{
				res[0].Left_Renamed = A.Left_Renamed;
				res[0].Right_Renamed = B.Left_Renamed;
			}
			else
			{
				res = new Interval[0];
			}

			return res;
		}

		//public static double CalcTA_Hpenet(ObstacleType[] PtTrnList, double hTurn, double TA_PDG, double MaxDist, ref int index, double iniH = 0)
		//{
		//    double calcTA_HpenetReturn = 0;
		//    int N = 0;
		//    int I = 0;

		//    index = -1;
		//    System.Array transTemp52 = PtTrnList;
		//    N =  /* TRANSINFO: .NET Equivalent of Microsoft.VisualBasic NameSpace */ transTemp52.GetUpperBound(0);
		//    calcTA_HpenetReturn = iniH;

		//    if (N < 0)
		//    {
		//        return calcTA_HpenetReturn;
		//    }

		//    for (I = 0; I <= N; I++)
		//    {
		//        PtTrnList[I].hPent = PtTrnList[I].Height + PtTrnList[I].MOC - hTurn - PtTrnList[I].Dist * TA_PDG;

		//        if ((PtTrnList[I].Dist <= MaxDist) && (calcTA_HpenetReturn < PtTrnList[I].hPent))
		//        {
		//            calcTA_HpenetReturn = PtTrnList[I].hPent;
		//            index = I;
		//        }
		//    }
		//    return calcTA_HpenetReturn;
		//}

		//public static double CalcTA_PDG(ObstacleType[] PtTrnList, double hTurn, double TA_PDG, double MaxDist, ref int index)
		//{
		//    double calcTA_PDGReturn = 0;
		//    int N = 0;
		//    int I = 0;
		//    double PDGi = 0;

		//    index = -1;
		//    System.Array transTemp53 = PtTrnList;
		//    N =  /* TRANSINFO: .NET Equivalent of Microsoft.VisualBasic NameSpace */ transTemp53.GetUpperBound(0);

		//    if ((N < 0))
		//    {
		//        return TA_PDG;
		//    }

		//    calcTA_PDGReturn = 0.0;

		//    for (I = 0; I <= N; I++)
		//    {
		//        if ((PtTrnList[I].hPent > 0.0) && (PtTrnList[I].Dist <= MaxDist))
		//        {
		//            PDGi = PtTrnList[I].hPent / PtTrnList[I].Dist;
		//            if (calcTA_PDGReturn < PDGi)
		//            {
		//                index = I;
		//                calcTA_PDGReturn = PDGi;
		//            }
		//        }
		//    }

		//    calcTA_PDGReturn = System.Math.Round(calcTA_PDGReturn + TA_PDG + 0.0004999, 3);
		//    return calcTA_PDGReturn;
		//}


		public static IPointCollection CalcTouchByFixDir(IPoint PtSt, IPoint PtFix, double TurnR, double DirCur,
														ref double DirFix, int TurnDir, double SnapAngle,
														ref double dDir, ref IPoint FlyBy)
		{
			ESRI.ArcGIS.Geometry.IPointCollection calcTouchByFixDirReturn = null;
			IConstructPoint Constructor = null;

			IPoint ptCnt1 = null;
			IPoint Pt1 = null;
			IPoint Pt10 = null;
			IPoint Pt11 = null;

			IPoint Pt2 = null;
			IPoint pt3 = null;

			int SideD = 0;
			int SideT = 0;

			double DeltaAngle = 0;
			double DeltaDist = 0;
			double distToTmp = 0;
			double dirToTmp = 0;
			double OutDir = 0;
			double OutDir0 = 0;
			double OutDir1 = 0;
			double Dist = 0;

			calcTouchByFixDirReturn = new Multipoint();

			if (Functions.SubtractAngles(DirCur, DirFix) < 0.5)
			{
				DirFix = DirCur;
				if (Functions.ReturnDistanceInMeters(PtFix, PtSt) < GlobalVars.distEps)
				{
					FlyBy = new ESRI.ArcGIS.Geometry.Point();
					FlyBy.PutCoords(PtFix.X, PtFix.Y);
					calcTouchByFixDirReturn.AddPoint(PtSt);
					calcTouchByFixDirReturn.AddPoint(PtSt);
					return calcTouchByFixDirReturn;
				}
			}

			ptCnt1 = Functions.PointAlongPlane(PtSt, DirCur + 90.0 * TurnDir, TurnR);
			PtSt.M = DirCur;

			OutDir0 = NativeMethods.Modulus(DirFix - SnapAngle * TurnDir, 360.0);
			OutDir1 = NativeMethods.Modulus(DirFix + SnapAngle * TurnDir, 360.0);

			Pt10 = Functions.PointAlongPlane(ptCnt1, OutDir0 - 90.0 * TurnDir, TurnR);
			Pt11 = Functions.PointAlongPlane(ptCnt1, OutDir1 - 90.0 * TurnDir, TurnR);

			SideT = Functions.SideDef(Pt10, DirFix, PtFix);
			SideD = Functions.SideDef(Pt10, DirFix, ptCnt1);

			if (SideT * SideD < 0)
			{
				Pt1 = Pt10;
				OutDir = OutDir0;
			}
			else
			{
				Pt1 = Pt11;
				OutDir = OutDir1;
			}

			Pt1.M = OutDir;

			FlyBy = new ESRI.ArcGIS.Geometry.Point();
			Constructor = ((ESRI.ArcGIS.Geometry.IConstructPoint)(FlyBy));

			Constructor.ConstructAngleIntersection(Pt1, GlobalVars.DegToRadValue * OutDir, PtFix, GlobalVars.DegToRadValue * DirFix);

			Dist = Functions.ReturnDistanceInMeters(Pt1, FlyBy);

			dirToTmp = Functions.ReturnAngleInDegrees(PtFix, FlyBy);
			distToTmp = Functions.ReturnDistanceInMeters(PtFix, FlyBy);

			SideT = AnglesSideDef(OutDir, DirFix);

			if (SideT > 0)
			{
				DeltaAngle = NativeMethods.Modulus(180.0 + DirFix - OutDir, 360.0);
			}
			else if (SideT < 0)
			{
				DeltaAngle = NativeMethods.Modulus(OutDir - 180.0 - DirFix, 360.0);
			}

			DeltaAngle = 0.5 * DeltaAngle;
			DeltaDist = TurnR / System.Math.Tan(GlobalVars.DegToRadValue * DeltaAngle);

			dDir = Dist - DeltaDist;

			if (DeltaDist <= Dist)
			{
				Pt2 = Functions.PointAlongPlane(FlyBy, OutDir - 180.0, DeltaDist);
				pt3 = Functions.PointAlongPlane(FlyBy, DirFix, DeltaDist);
			}
			else
			{
				Pt2 = Functions.PointAlongPlane(FlyBy, OutDir, DeltaDist);
				pt3 = Functions.PointAlongPlane(FlyBy, DirFix - 180.0, DeltaDist);
			}

			Pt2.M = OutDir;
			pt3.M = DirFix;

			calcTouchByFixDirReturn.AddPoint(PtSt);
			calcTouchByFixDirReturn.AddPoint(Pt1);
			calcTouchByFixDirReturn.AddPoint(Pt2);
			calcTouchByFixDirReturn.AddPoint(pt3);
			return calcTouchByFixDirReturn;
		}

		public static int AnglesSideDef(double X, double Y)
		{
			double Z = 0;
			Z = NativeMethods.Modulus(X - Y, 360.0);
			if ((Z > 360.0 - GlobalVars.degEps) || (Z < GlobalVars.degEps))
			{
				return 0;
			}
			else if (Z > 180.0 - GlobalVars.degEps)
			{
				return -1;
			}
			else if (Z < 180.0 + GlobalVars.degEps)
			{
				return 1;
			}

			return 2;
		}

		public static IPointCollection CalcTrajectoryFromMultiPoint(IPointCollection MultiPoint)
		{
			ESRI.ArcGIS.Geometry.IPointCollection calcTrajectoryFromMultiPointReturn = null;
			IConstructPoint ptConstr = null;
			IPoint FromPt = null;
			IPoint CntPt = null;
			IPoint ToPt = null;

			double fTmp = 0;
			double fE = 0;

			int Side = 0;
			int I = 0;
			int N = 0;

			CntPt = new ESRI.ArcGIS.Geometry.Point();
			ptConstr = ((ESRI.ArcGIS.Geometry.IConstructPoint)(CntPt));
			calcTrajectoryFromMultiPointReturn = new Polyline();
			fE = GlobalVars.DegToRadValue * 0.5;

			N = MultiPoint.PointCount - 2;

			calcTrajectoryFromMultiPointReturn.AddPoint(MultiPoint.get_Point(0));

			for (I = 0; I <= N; I++)
			{
				FromPt = MultiPoint.get_Point(I);
				ToPt = MultiPoint.get_Point(I + 1);
				fTmp = GlobalVars.DegToRadValue * (FromPt.M - ToPt.M);

				if ((System.Math.Abs(System.Math.Sin(fTmp)) <= fE) && (System.Math.Cos(fTmp) > 0.0))
				{
					calcTrajectoryFromMultiPointReturn.AddPoint(ToPt);
				}
				else
				{
					if (System.Math.Abs(System.Math.Sin(fTmp)) > fE)
					{
						ptConstr.ConstructAngleIntersection(FromPt, GlobalVars.DegToRadValue * (NativeMethods.Modulus(FromPt.M + 90.0, 360.0)), ToPt, GlobalVars.DegToRadValue * (NativeMethods.Modulus(ToPt.M + 90.0, 360.0)));
					}
					else
					{
						CntPt.PutCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y));
					}
					Side = Functions.SideDef(FromPt, FromPt.M, ToPt);
					calcTrajectoryFromMultiPointReturn.AddPointCollection(CreateArcPrj(CntPt, FromPt, ToPt, -Side) as IPointCollection);
				}
			}
			return calcTrajectoryFromMultiPointReturn;
		}

		public static IElement DrawPoint(IPoint pPoint, int Color = - 1, bool drawFlg = true)
		{
			ESRI.ArcGIS.Carto.IElement drawPointReturn = null;
			IMarkerElement pMarkerShpElement = null;
			IElement pElementofpPoint = null;
			ISimpleMarkerSymbol pMarkerSym = null;
			IRgbColor pRGB = null;

			pMarkerShpElement = ((ESRI.ArcGIS.Carto.IMarkerElement)(new MarkerElement()));

			pElementofpPoint = ((ESRI.ArcGIS.Carto.IElement)(pMarkerShpElement));
			pElementofpPoint.Geometry = pPoint;

			pRGB = new RgbColor();
			if (Color == -1)
			{
				Random rnd = new Random();
				pRGB.Red = rnd.Next(256);
				pRGB.Green = rnd.Next(256);
				pRGB.Blue = rnd.Next(256);
			}
			else
			{
				pRGB.RGB = Color;
			}

			pMarkerSym = new SimpleMarkerSymbol();
			pMarkerSym.Color = pRGB;
			pMarkerSym.Size = 8;
			pMarkerShpElement.Symbol = pMarkerSym;
			drawPointReturn = pElementofpPoint;
			IGraphicsContainer pGraphics = null;
			if (drawFlg)
			{
				pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(pElementofpPoint, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return drawPointReturn;
		}

		public static IElement DrawPointWithText(IPoint pPoint, string sText, int Color = -1, bool drawFlg = true)
		{
			ESRI.ArcGIS.Carto.IElement drawPointWithTextReturn = null;
			IRgbColor pRGB = null;
			IMarkerElement pMarkerShpElement = null;
			IElement pElementofpPoint = null;
			ISimpleMarkerSymbol pMarkerSym = null;

			ITextElement pTextElement = null;
			IElement pElementOfText = null;
			IGroupElement pGroupElement = null;
			IElement pCommonElement = null;
			ITextSymbol pTextSymbol = null;

			pTextElement = ((ESRI.ArcGIS.Carto.ITextElement)(new TextElement()));
			pElementOfText = ((ESRI.ArcGIS.Carto.IElement)(pTextElement));

			pTextSymbol = new TextSymbol();
			pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
			pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;

			pTextElement.Text = sText;
			pTextElement.ScaleText = false;
			pTextElement.Symbol = pTextSymbol;

			pElementOfText.Geometry = pPoint;

			pMarkerShpElement = ((ESRI.ArcGIS.Carto.IMarkerElement)(new MarkerElement()));

			pElementofpPoint = ((ESRI.ArcGIS.Carto.IElement)(pMarkerShpElement));
			pElementofpPoint.Geometry = pPoint;

			pRGB = new RgbColor();
			if (Color != -1)
			{
				pRGB.RGB = Color;
			}
			else
			{
				Random rnd = new Random();
				pRGB.Red = rnd.Next(256);
				pRGB.Green = rnd.Next(256);
				pRGB.Blue = rnd.Next(256);
			}

			pMarkerSym = new SimpleMarkerSymbol();
			pMarkerSym.Color = pRGB;
			pMarkerSym.Size = 6;
			pMarkerShpElement.Symbol = pMarkerSym;

			pGroupElement = ((ESRI.ArcGIS.Carto.IGroupElement)(new GroupElement()));
			pGroupElement.AddElement(pElementofpPoint);
			pGroupElement.AddElement(pTextElement as IElement);

			pCommonElement = ((ESRI.ArcGIS.Carto.IElement)(pGroupElement));
			drawPointWithTextReturn = pCommonElement;

			IGraphicsContainer pGraphics = null;
			if (drawFlg)
			{
				pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(pCommonElement, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return drawPointWithTextReturn;
		}

		public static IElement DrawLine(Line pLine, int Color = -1, double Width = 1, bool drawFlg = true)
		{
			ESRI.ArcGIS.Carto.IElement drawLineReturn = null;
			ILineElement pLineElement = null;
			IElement pElementOfpLine = null;
			ISimpleLineSymbol pLineSym = null;
			IRgbColor pRGB = null;
			IGeometry pGeometry = null;

			pLineElement = ((ESRI.ArcGIS.Carto.ILineElement)(new LineElement()));
			pElementOfpLine = ((ESRI.ArcGIS.Carto.IElement)(pLineElement));
			pGeometry = pLine;

			pElementOfpLine.Geometry = pGeometry;

			pRGB = new RgbColor();
			if (Color == -1)
			{
				Random rnd = new Random();
				pRGB.Red = rnd.Next(256);
				pRGB.Green = rnd.Next(256);
				pRGB.Blue = rnd.Next(256);
			}
			else
			{
				pRGB.RGB = Color;
			}


			pLineSym = new SimpleLineSymbol();
			pLineSym.Color = pRGB;
			pLineSym.Style = esriSimpleLineStyle.esriSLSSolid;
			pLineSym.Width = Width;

			pLineElement.Symbol = pLineSym;
			drawLineReturn = pElementOfpLine;

			IGraphicsContainer pGraphics = null;
			if (drawFlg)
			{
				pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(pElementOfpLine, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return drawLineReturn;
		}

		public static IElement DrawPolyline(IPolyline pLine, int pColor, double Width = 1, bool drawFlg = true)
		{
			ESRI.ArcGIS.Carto.IElement drawPolyLineReturn = null;
			ILineElement pLineElement = null;
			IElement pElementOfpLine = null;
			ISimpleLineSymbol pLineSym = null;
			IRgbColor pRGB = null;
			IGeometry pGeometry = null;

			pLineElement = ((ESRI.ArcGIS.Carto.ILineElement)(new LineElement()));
			pElementOfpLine = ((ESRI.ArcGIS.Carto.IElement)(pLineElement));
			pGeometry = ((ESRI.ArcGIS.Geometry.IGeometry)(pLine));

			pElementOfpLine.Geometry = pGeometry;

			pRGB = new RgbColor();
			pRGB.RGB = pColor;
			pLineSym = new SimpleLineSymbol();
			pLineSym.Color = pRGB;
			pLineSym.Style = esriSimpleLineStyle.esriSLSSolid;
			pLineSym.Width = Width;

			pLineElement.Symbol = pLineSym;
			drawPolyLineReturn = pElementOfpLine;

			IGraphicsContainer pGraphics = null;
			if (drawFlg)
			{
				pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(pElementOfpLine, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return drawPolyLineReturn;
		}

		public static IElement DrawPolyline(IPointCollection pLine, int pColor, double Width = 1, bool drawFlg = true)
		{
			return DrawPolyline(pLine as IPolyline, pColor, Width, drawFlg);
		}

		public static IPointCollection TurnToFixPrj(IPoint PtSt, double TurnR, int TurnDir, IPoint FixPnt)
		{
			ESRI.ArcGIS.Geometry.IPointCollection turnToFixPrjReturn = null;
			IPoint ptCnt = null;
			IPoint Pt1 = null;
			double DeltaAngle = 0;
			double DirFx2Cnt = 0;
			double DistFx2Cnt = 0;
			double DirCur = 0;

			DirCur = PtSt.M;

			turnToFixPrjReturn = new Multipoint();
			ptCnt = Functions.PointAlongPlane(PtSt, DirCur + 90.0 * TurnDir, TurnR);

			DistFx2Cnt = Functions.ReturnDistanceInMeters(ptCnt, FixPnt);


			// If DistFx2Cnt + distEps < TurnR Then
			//     TurnR = DistFx2Cnt
			//     Exit Function
			// End If

			// If DistFx2Cnt < TurnR Then
			//     DistFx2Cnt = TurnR
			// End If

			DirFx2Cnt = Functions.ReturnAngleInDegrees(ptCnt, FixPnt);
			DeltaAngle = -GlobalVars.RadToDegValue * (ArcCos(TurnR / DistFx2Cnt)) * TurnDir;

			if (double.IsNaN(DeltaAngle))
				DeltaAngle = 0.0;

			Pt1 = Functions.PointAlongPlane(ptCnt, DirFx2Cnt + DeltaAngle, TurnR);
			Pt1.M = Functions.ReturnAngleInDegrees(Pt1, FixPnt);

			turnToFixPrjReturn.AddPoint(PtSt);
			turnToFixPrjReturn.AddPoint(Pt1);
			return turnToFixPrjReturn;
		}

		public static double CircleVectorIntersect(IPoint PtCent, double R, IPoint ptVect, double DirVect, ref IPoint ptRes)
		{
			double circleVectorIntersectReturn = 0;
			double d = 0;
			double DistCnt2Vect = 0;
			IPoint ptTmp = null;
			IConstructPoint Constr = null;

			ptTmp = new ESRI.ArcGIS.Geometry.Point();
			Constr = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptTmp));

			Constr.ConstructAngleIntersection(PtCent, GlobalVars.DegToRadValue * (DirVect + 90.0), ptVect, GlobalVars.DegToRadValue * DirVect);

			DistCnt2Vect = Functions.ReturnDistanceInMeters(PtCent, ptTmp);

			if (DistCnt2Vect < R)
			{
				d = System.Math.Sqrt(R * R - DistCnt2Vect * DistCnt2Vect);
				ptRes = Functions.PointAlongPlane(ptTmp, DirVect, d);
				circleVectorIntersectReturn = d; // Functions.ReturnDistanceInMeters(ptRes, PtTmp)
			}
			else
			{
				circleVectorIntersectReturn = 0.0;
				ptRes = new ESRI.ArcGIS.Geometry.Point();
			}
			return circleVectorIntersectReturn;
		}

		public static double CircleVectorIntersect(IPoint PtCent, double R, IPoint ptVect, double DirVect)
		{
			ESRI.ArcGIS.Geometry.IPoint transTemp66 = null;
			return CircleVectorIntersect(PtCent, R, ptVect, DirVect, ref transTemp66);
		}

		public static bool AngleInSector(double Angle, double X, double Y)
		{
			bool angleInSectorReturn = false;
			double Sector = 0;
			double Sub1 = 0;
			double Sub2 = 0;

			Sector = Functions.SubtractAngles(X, Y);
			Sub1 = Functions.SubtractAngles(X, Angle);
			Sub2 = Functions.SubtractAngles(Angle, Y);

			angleInSectorReturn = !((Sub1 + Sub2 > Sector + GlobalVars.degEps));
			return angleInSectorReturn;
		}

		public static IPointCollection CreateBasePoints(IPointCollection pPolygone, IPolyline K1K1, double lDepDir, int lTurnDir)
		{
			ESRI.ArcGIS.Geometry.IPointCollection createBasePointsReturn = null;
			IPointCollection TmpPoly = null;
			bool bFlg = false;
			int I = 0;
			int N = 0;
			int Side = 0;

			bFlg = false;
			N = pPolygone.PointCount;
			TmpPoly = new Polyline();
			createBasePointsReturn = new Polygon();

			if (lTurnDir > 0)
			{
				for (I = 0; I <= N - 1; I++)
				{
					Side = Functions.SideDef(K1K1.FromPoint, lDepDir + 90.0, pPolygone.get_Point(I));
					if ((Side < 0))
					{
						if (bFlg)
						{
							createBasePointsReturn.AddPoint(pPolygone.get_Point(I));
						}
						else
						{
							TmpPoly.AddPoint(pPolygone.get_Point(I));
						}
					}
					else if (!(bFlg))
					{
						bFlg = true;
						createBasePointsReturn.AddPoint(K1K1.FromPoint);
						createBasePointsReturn.AddPoint(K1K1.ToPoint);
					}
				}
			}
			else
			{
				for (I = N - 1; I >= 0; I += -1)
				{
					Side = Functions.SideDef(K1K1.FromPoint, lDepDir + 90.0, pPolygone.get_Point(I));
					if ((Side < 0))
					{
						if (bFlg)
						{
							createBasePointsReturn.AddPoint(pPolygone.get_Point(I));
						}
						else
						{
							TmpPoly.AddPoint(pPolygone.get_Point(I));
						}
					}
					else if (!(bFlg))
					{
						bFlg = true;
						createBasePointsReturn.AddPoint(K1K1.ToPoint);
						createBasePointsReturn.AddPoint(K1K1.FromPoint);
					}
				}
			}

			createBasePointsReturn.AddPointCollection(TmpPoly);
			return createBasePointsReturn;
		}

		public static IPolygon PolygonDifference(IPolygon Source, IPolygon Subtractor)
		{
			IPolygon polygonDifferenceReturn = null;
			ITopologicalOperator2 pTopo = null;

			pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(Source));
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(Source));
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			try
			{
				polygonDifferenceReturn = pTopo.Difference(Subtractor as IGeometry) as IPolygon;
				pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(polygonDifferenceReturn));
				pTopo.IsKnownSimple_2 = false;
				pTopo.Simplify();
				return polygonDifferenceReturn;
			}
			catch
			{
				polygonDifferenceReturn = Subtractor;
			}

			return polygonDifferenceReturn;
		}

		public static IPolygon PolygonIntersection(IPointCollection pPoly1, IPointCollection pPoly2)
		{
			return PolygonIntersection(pPoly1 as IPolygon, pPoly2 as IPolygon);
		}

		public static IPolygon PolygonIntersection(IPolygon pPoly1, IPolygon pPoly2)
		{
			IPolygon polygonIntersectionReturn = null;
			ITopologicalOperator2 pTopo = null;
			Polygon pTmpPoly0 = null;
			Polygon pTmpPoly1 = null;

			pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPoly2));
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pPoly1));
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			try
			{
				polygonIntersectionReturn = pTopo.Intersect(pPoly2 as IGeometry, esriGeometryDimension.esriGeometry2Dimension) as IPolygon;
				pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(polygonIntersectionReturn));
				pTopo.IsKnownSimple_2 = false;
				pTopo.Simplify();
			}
			catch
			{
				try
				{
					pTmpPoly0 = pTopo.Union(pPoly2 as IGeometry) as Polygon;
					pTmpPoly1 = pTopo.SymmetricDifference(pPoly2 as IGeometry) as Polygon;

					pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pTmpPoly1));
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();

					pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pTmpPoly0));
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();
					polygonIntersectionReturn = pTopo.Difference(pTmpPoly1 as IGeometry) as IPolygon;

					pTopo = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(polygonIntersectionReturn));
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();
				}
				catch
				{
					polygonIntersectionReturn = pPoly2;
				}
			}
			return polygonIntersectionReturn;
		}

		public static string DegToStr(double V)
		{
			string degToStrReturn = null;
			int L = 0;
			int I = 0;

			degToStrReturn = V.ToString();
			L = degToStrReturn.Length;

			for (I = 1; I <= 3 - L; I++)
			{
				degToStrReturn = "0" + degToStrReturn;
			}

			return degToStrReturn + "°";
		}

		public static void shall_SortfSort(ObstacleType[] obsArray)
		{
			ObstacleType TempVal;
			int GapSize = 0;
			int I = 0;
			int CurPos = 0;
			int FirstRow = obsArray.GetLowerBound(0);
			int LastRow = obsArray.GetUpperBound(0);
			int NumRows = LastRow - FirstRow + 1;

			if (LastRow < 0)
				return;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (I = (GapSize + FirstRow); I <= LastRow; I++)
				{
					CurPos = I;
					TempVal = obsArray[I];
					while (obsArray[CurPos - GapSize].fSort > TempVal.fSort)
					{
						obsArray[CurPos] = obsArray[CurPos - GapSize];
						CurPos = CurPos - GapSize;
						if (CurPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void shall_SortfSortD(ObstacleType[] obsArray)
		{
			ObstacleType TempVal;
			int FirstRow = obsArray.GetLowerBound(0);
			int LastRow = obsArray.GetUpperBound(0);
			int NumRows = LastRow - FirstRow + 1;
			int I = 0;
			int CurPos = 0;
			int GapSize = 0;

			if (LastRow < 0)
				return;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (I = GapSize + FirstRow; I <= LastRow; I++)
				{
					CurPos = I;
					TempVal = obsArray[I];
					while (obsArray[CurPos - GapSize].fSort < TempVal.fSort)
					{
						obsArray[CurPos] = obsArray[CurPos - GapSize];
						CurPos = CurPos - GapSize;
						if (CurPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void shall_SortsSort(ObstacleType[] obsArray)
		{
			ObstacleType TempVal;
			int GapSize = 0;
			int I = 0;
			int CurPos = 0;
			int FirstRow = obsArray.GetLowerBound(0);
			int LastRow = obsArray.GetUpperBound(0);
			int NumRows = LastRow - FirstRow + 1;

			if (LastRow < 0)
				return;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (I = (GapSize + FirstRow); I <= LastRow; I++)
				{
					CurPos = I;
					TempVal = obsArray[I];
					while (String.Compare(obsArray[CurPos - GapSize].sSort, TempVal.sSort) > 0)
					{
						obsArray[CurPos] = obsArray[CurPos - GapSize];
						CurPos = CurPos - GapSize;
						if (CurPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void shall_SortsSortD(ObstacleType[] obsArray)
		{
			ObstacleType TempVal;
			int GapSize = 0;
			int I = 0;
			int CurPos = 0;
			int FirstRow = obsArray.GetLowerBound(0);
			int LastRow = obsArray.GetUpperBound(0);
			int NumRows = LastRow - FirstRow + 1;

			if (LastRow < 0)
				return;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (I = (GapSize + FirstRow); I <= LastRow; I++)
				{
					CurPos = I;
					TempVal = obsArray[I];

					while (String.Compare(obsArray[CurPos - GapSize].sSort, TempVal.sSort) < 0)
					{
						obsArray[CurPos] = obsArray[CurPos - GapSize];
						CurPos = CurPos - GapSize;
						if (CurPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void SaveMxDocument(string FileName, bool ClearGraphics)
		{
			/*
			IGxLayer pGxLayer;
			IGxFile pGxFile;
			ILayer pLyr;
			IGraphicsContainer pGraphics;
			pGraphics = GlobalVars.GetActiveView().GraphicsContainer;

			pLyr = GlobalVars.pMap.ActiveGraphicsLayer;
			pLyr.Name = "PANDA Chart";
			pLyr.SpatialReference = GlobalVars.pSpRefPrj;

			pGxLayer = new GxLayer();
			pGxFile = (IGxFile)pGxLayer;

			pGxFile.Path = FileName + "_Chart.lyr";

			pGxLayer.Layer = pLyr;

			pGraphics.DeleteAllElements();

			pGxLayer = new GxLayer();
			pGxFile = (IGxFile)pGxLayer;

			pGxFile.Path = FileName + "_Chart.lyr";

			GlobalVars.pMap.AddLayer(pGxLayer.Layer);
			GlobalVars.Application.SaveAsDocument(FileName + "_Chart.mxd", true);
			GlobalVars.pMap.DeleteLayer(pGxLayer.Layer);
			*/
			/*
			IGxLayer pGxLayer;
			IGxFile pGxFile;
			ILayer pLyr;
			IGraphicsContainer pGraphics;
			pGraphics = GlobalVars.GetActiveView().GraphicsContainer;

			pLyr = GlobalVars.pMap.ActiveGraphicsLayer;
			pLyr.Name = "PANDA Chart";
			pLyr.SpatialReference = GlobalVars.pSpRefPrj;

			pGxLayer = new GxLayer();
			pGxLayer.Layer = pLyr;

			pGxFile = (IGxFile)pGxLayer;
			pGxFile.Path = FileName + "_Chart.lyr";
			pGxFile.Save();

			pGraphics.DeleteAllElements();
			*/
			/*
				GlobalVars.pMap.AddLayer(pGxLayer.Layer);
				GlobalVars.Application.SaveAsDocument(FileName + "_Chart.mxd", true);
				GlobalVars.pMap.DeleteLayer(pGxLayer.Layer);
			*/
		}

		public static void TextBoxFloat(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if ((KeyChar < '0' || KeyChar > '9') && KeyChar != DecSep)
				KeyChar = '\0';
			else if (KeyChar == DecSep)
			{
				if (BoxText.Contains(DecSep.ToString()))
					KeyChar = '\0';
			}
		}

		public static void TextBoxInteger(ref char KeyChar)
		{
			if (KeyChar < 32)
				return;
			if ((KeyChar < '0') || (KeyChar > '9'))
				KeyChar = '\0';
		}

		public static void SetThreadLocaleByConfig()
		{
			GlobalVars.LangCode = Functions.RegRead<Int32>(Microsoft.Win32.Registry.CurrentUser,
				GlobalVars.PandaRegKey, "LanguageCode", GlobalVars.NeutralLangCode);

			NativeMethods.SetThreadLocale(GlobalVars.LangCode);
		}

		public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			System.Windows.Forms.MessageBox.Show(e.Exception.Message, "Error",
				System.Windows.Forms.MessageBoxButtons.OK,
				System.Windows.Forms.MessageBoxIcon.Error);
		}

		public static void HandleThreadException()
		{
			if (_errorHandled)
				return;
			_errorHandled = true;
			System.Windows.Forms.Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Functions.Application_ThreadException);
		}

		public static int RGB(uint red, uint green, uint blue)
		{
			return (int)(((byte)blue) << 16) | (((byte)green) << 8) | ((byte)red);
		}

		private static bool _errorHandled = false;

		public static int Quadric(double a, double b, double c, out double x0, out double x1)
		{
			double d = b * b - 4 * a * c;
			x0 = x1 = 0.0;

			if (d < 0.0)
				return 0;

			d = Math.Sqrt(d);

			if (d < 0.000001)
			{
				x0 = x1 = -0.5 * b / a;
				return 1;
			}

			x0 = 0.5 * (-b - d) / a;
			x1 = 0.5 * (-b + d) / a;

			return 2;
		}

		public static RunwayCentrelinePoint GetNearestPoint(ref RWYCLPoint[] CLPointArray, IPoint pPtGeo, double MaxDist = 5.0)
		{
			IPoint ptA = (IPoint)Functions.ToPrj(pPtGeo);
			RunwayCentrelinePoint result = null;

			for (int i = 0, n = CLPointArray.GetLength(0); i < n; i++)
			{
				if (CLPointArray[i].pPtPrj != null)
				{
					double fTmp = Functions.ReturnDistanceInMeters(ptA, CLPointArray[i].pPtPrj);
					if (fTmp < MaxDist)
					{
						result = CLPointArray[i].pCLPoint;
						MaxDist = fTmp;
					}
				}
			}
			return result;
		}
	}
}


