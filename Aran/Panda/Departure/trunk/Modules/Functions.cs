using System;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geometry;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class Functions
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

		public static double ConvertDistance(double Val_Renamed, eRoundMode RoundMode = eRoundMode.NEAREST)
		{
			if (RoundMode < eRoundMode.NONE || RoundMode > eRoundMode.CEIL) RoundMode = eRoundMode.NONE;

			switch (RoundMode)
			{
				case eRoundMode.NONE:
					return Val_Renamed * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Multiplier;
				case eRoundMode.FLOOR:
				//return System.Math.Round(Val_Renamed * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Multiplier / GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding - 0.4999) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding;
				case eRoundMode.CEIL:
				//return System.Math.Round(Val_Renamed * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Multiplier / GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding + 0.4999) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(Val_Renamed * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Multiplier / GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding) * GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Rounding;
			}
			return Val_Renamed;
		}

		public static double ConvertHeight(double Val_Renamed, eRoundMode RoundMode = eRoundMode.NEAREST)
		{
			if (RoundMode < eRoundMode.NONE || RoundMode > eRoundMode.SPECIAL) RoundMode = eRoundMode.NONE;

			switch (RoundMode)
			{
				case eRoundMode.NONE:
					return Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier;
				case eRoundMode.FLOOR:
				//return System.Math.Round(Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier / GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding - 0.4999) * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding;
				case eRoundMode.CEIL:
				//return System.Math.Round(Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier / GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding + 0.4999) * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier / GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding) * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding;
				case eRoundMode.SPECIAL:
					if (GlobalVars.HeightUnit == 0)
						return System.Math.Round(Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier / 50.0) * 50.0;
					else if (GlobalVars.HeightUnit == 1)
						return System.Math.Round(Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier / 100.0) * 100.0;
					else
						return System.Math.Round(Val_Renamed * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Multiplier / GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding) * GlobalVars.HeightConverter[GlobalVars.HeightUnit].Rounding;
			}

			return Val_Renamed;
		}

		//public static double ConvertAngle(double val, eRoundMode RoundMode = eRoundMode.NEAREST)
		//{
		//	if (RoundMode < eRoundMode.NONE || RoundMode >= eRoundMode.SPECIAL)
		//		RoundMode = eRoundMode.NONE;

		//	switch (RoundMode)
		//	{
		//		case eRoundMode.NONE:
		//			return val * GlobalVars.AngleConverter[GlobalVars.AngleUnit].Multiplier;
		//		case eRoundMode.FLOOR:
		//			return System.Math.Round(val * GlobalVars.AngleConverter[GlobalVars.AngleUnit].Multiplier / GlobalVars.AngleConverter[GlobalVars.AngleUnit].Rounding - 0.4999) * GlobalVars.AngleConverter[GlobalVars.AngleUnit].Rounding;
		//		case eRoundMode.CEIL:
		//			return System.Math.Round(val * GlobalVars.AngleConverter[GlobalVars.AngleUnit].Multiplier / GlobalVars.AngleConverter[GlobalVars.AngleUnit].Rounding + 0.4999) * GlobalVars.AngleConverter[GlobalVars.AngleUnit].Rounding;
		//		case eRoundMode.NEAREST:
		//			return System.Math.Round(val * GlobalVars.AngleConverter[GlobalVars.AngleUnit].Multiplier / GlobalVars.AngleConverter[GlobalVars.AngleUnit].Rounding) * GlobalVars.AngleConverter[GlobalVars.AngleUnit].Rounding;
		//		case eRoundMode.SPECIAL:
		//			return System.Math.Round(val * GlobalVars.AngleConverter[GlobalVars.AngleUnit].Multiplier / 5.0) * 5.0;
		//	}

		//	return val;
		//}

		public static double RoundAngle(double val, eRoundMode RoundMode = eRoundMode.NEAREST)
		{
			if (RoundMode < eRoundMode.NONE || RoundMode >= eRoundMode.SPECIAL)
				RoundMode = eRoundMode.NONE;

			switch (RoundMode)
			{
				case eRoundMode.NONE:
					return val;
				case eRoundMode.FLOOR:
					return System.Math.Round(val / GlobalVars.AnglePrecision - 0.4999) * GlobalVars.AnglePrecision;
				case eRoundMode.CEIL:
					return System.Math.Round(val / GlobalVars.AnglePrecision + 0.4999) * GlobalVars.AnglePrecision;
				case eRoundMode.NEAREST:
					return System.Math.Round(val / GlobalVars.AnglePrecision) * GlobalVars.AnglePrecision;
			}

			return val;
		}

		public static double ConvertSpeed(double Val_Renamed, eRoundMode RoundMode = eRoundMode.NEAREST)
		{
			if (RoundMode < eRoundMode.NONE || RoundMode > eRoundMode.CEIL) RoundMode = eRoundMode.NONE;

			switch (RoundMode)
			{
				case eRoundMode.NONE:
					return Val_Renamed * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Multiplier;
				case eRoundMode.FLOOR:
				//return System.Math.Round(Val_Renamed * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Multiplier / GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Rounding - 0.4999) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Rounding;
				case eRoundMode.CEIL:
				//return System.Math.Round(Val_Renamed * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Multiplier / GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Rounding + 0.4999) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Rounding;
				case eRoundMode.NEAREST:
					return System.Math.Round(Val_Renamed * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Multiplier / GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Rounding) * GlobalVars.SpeedConverter[GlobalVars.SpeedUnit].Rounding;
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

		//public static T RegRead<T>(RegistryKey hKey, string key, string valueName, T defaultValue)
		//{
		//    return GlobalVars.gAranEnv.ReadConfig<T>(key, valueName, defaultValue);


		//    //try
		//    //{
		//    //    RegistryKey regKey = hKey.OpenSubKey(key, false);
		//    //    if (regKey != null)
		//    //    {
		//    //        object value = regKey.GetValue(valueName);
		//    //        if (value != null)
		//    //        {
		//    //            try
		//    //            {
		//    //                return (T)Convert.ChangeType(value, typeof(T));
		//    //            }
		//    //            catch { }
		//    //        }
		//    //    }
		//    //}
		//    //catch { }

		//    //return defaultValue;
		//}

		//public static int RegWrite(Microsoft.Win32.RegistryKey hKey, string key, string valueName, object value)
		//{
		//    GlobalVars.gAranEnv.WriteConfig(key, valueName, value);
		//    return 0;

		//    //try
		//    //{


		//    //    Microsoft.Win32.RegistryKey regKey = hKey.OpenSubKey(key, true);
		//    //    if (regKey == null)
		//    //        return -1;

		//    //    regKey.SetValue(valueName, value);
		//    //    return 0;
		//    //}
		//    //catch { }

		//    //return -1;
		//}

		public static double DegToRad(double Val_Renamed)
		{
			return Val_Renamed * GlobalVars.DegToRadValue;
		}

		public static double RadToDeg(double Val_Renamed)
		{
			return Val_Renamed * GlobalVars.RadToDegValue;
		}

		public static DateTime RetrieveLinkerTimestamp()
		{
			const int c_PeHeaderOffset = 60;
			const int c_LinkerTimestampOffset = 8;

			byte[] b = new byte[2048];
			System.IO.Stream s = null;

			try
			{
				string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
				s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				s.Read(b, 0, 2048);
			}
			finally
			{
				if (s != null)
					s.Close();
			}

			int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
			int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);

			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

			dt = dt.AddSeconds(secondsSince1970);
			dt = dt.ToLocalTime();
			return dt;
		}

		public static T[] CopyArray<T>(T[] source) where T : struct
		{
			T[] ta = new T[source.Length];
			for (int i = 0; i < ta.Length; i++)
				ta[i] = source[i];
			return ta;
		}

		public static IGeometry ToGeo(IGeometry prjGeometry)
		{
			IClone pClone = (IClone)prjGeometry;
			ESRI.ArcGIS.Geometry.IGeometry result = (IGeometry)pClone.Clone();

			result.SpatialReference = GlobalVars.pSpRefPrj;
			result.Project(GlobalVars.pSpRefShp);
			return result;
		}

		public static IGeometry ToPrj(IGeometry geoGeometry)
		{
			IClone pClone = (IClone)geoGeometry;
			ESRI.ArcGIS.Geometry.IGeometry result = (IGeometry)pClone.Clone();

			result.SpatialReference = GlobalVars.pSpRefShp;
			result.Project(GlobalVars.pSpRefPrj);
			return result;
		}

		public static double Azt2Dir(IPoint ptGeo, decimal Azt)
		{
			return Azt2Dir(ptGeo, (double)Azt);
		}

		public static double Azt2Dir(IPoint ptGeo, double Azt)
		{
			double ResX;
			double ResY;

			IClone pClone = (IClone)ptGeo;
			IPoint Pt11 = (IPoint)pClone.Clone();

			NativeMethods.PointAlongGeodesic(Pt11.X, Pt11.Y, 10.0, Azt, out ResX, out ResY);

			IPoint Pt10 = (IPoint)pClone.Clone();
			Pt10.PutCoords(ResX, ResY);

			ILine pLine = new Line();
			pLine.PutCoords(Pt11, Pt10);
			pLine = (ESRI.ArcGIS.Geometry.ILine)ToPrj(pLine);

			return NativeMethods.Modulus(GlobalVars.RadToDegValue * pLine.Angle);
		}

		public static double Azt2DirPrj(IPoint ptPrj, double Azt)
		{
			double ResX;
			double ResY;

			IPoint Pt11 = (IPoint)ToGeo(ptPrj);
			IClone pClone = (IClone)Pt11;

			NativeMethods.PointAlongGeodesic(Pt11.X, Pt11.Y, 10.0, Azt, out ResX, out ResY);

			IPoint Pt10 = (IPoint)pClone.Clone();
			Pt10.PutCoords(ResX, ResY);

			ILine pLine = new Line();
			pLine.PutCoords(Pt11, Pt10);
			pLine = (ESRI.ArcGIS.Geometry.ILine)ToPrj(pLine);

			return NativeMethods.Modulus(GlobalVars.RadToDegValue * pLine.Angle);
		}

		public static double Dir2Azt(IPoint pPtPrj, double Dir_Renamed)
		{
			double resD;
			double resI;
			IPoint PtN = Functions.PointAlongPlane(pPtPrj, Dir_Renamed, 10.0);
			IPoint Pt10 = (IPoint)Functions.ToGeo(PtN);

			PtN = (IPoint)Functions.ToGeo(pPtPrj);

			NativeMethods.ReturnGeodesicAzimuth(PtN.X, PtN.Y, Pt10.X, Pt10.Y, out resD, out resI);
			return resD;
		}

		public static double Dir2AztGeo(IPoint pPtGeo, double Dir_Renamed)
		{
			double resD;
			double resI;

			IPoint pPtPrj = ToPrj(pPtGeo) as IPoint;
			IPoint PtN = Functions.PointAlongPlane(pPtPrj, Dir_Renamed, 10.0);

			IPoint Pt10 = (IPoint)Functions.ToGeo(PtN);
			PtN = (IPoint)Functions.ToGeo(pPtPrj);

			NativeMethods.ReturnGeodesicAzimuth(PtN.X, PtN.Y, Pt10.X, Pt10.Y, out resD, out resI);
			return resD;
		}

		private static void QuickSort(ObstacleData[] A, int iLo, int iHi)
		{
			int Lo = iLo;
			int Hi = iHi;
			double Mid_Renamed = A[(Lo + Hi) / 2].Dist;

			do
			{
				while (A[Lo].Dist < Mid_Renamed)
					Lo++;

				while (A[Hi].Dist > Mid_Renamed)
					Hi--;

				if (Lo <= Hi)
				{
					ObstacleData t = A[Lo];
					A[Lo] = A[Hi];
					A[Hi] = t;
					Lo++;
					Hi--;
				}
			}
			while (Lo <= Hi);

			if (Hi > iLo)
				QuickSort(A, iLo, Hi);

			if (Lo < iHi)
				QuickSort(A, Lo, iHi);
		}

		public static void Sort(ObstacleData[] A)
		{
			int Lo = A.GetLowerBound(0);
			int Hi = A.GetUpperBound(0);

			if (Lo == Hi)
				return;

			QuickSort(A, Lo, Hi);
		}

		public static void GetSecTurnAreaObstacles(ObstacleContainer turnObstList, IPolygon TurnAreaSecPolygon, RWYType DER, double DirToNav, IPoint ptNavPrj, double MOCLimit)
		{
			int n = turnObstList.Parts.Length;

			if (n == 0)
				return;

			// DrawPolygon TurnAreaSecPolygon, 0

			ITopologicalOperator2 pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)TurnAreaSecPolygon;
			pTopoOper.IsKnownSimple_2 = false;
			pTopoOper.Simplify();

			IProximityOperator pProxiOperator = (ESRI.ArcGIS.Geometry.IProximityOperator)TurnAreaSecPolygon;
			IPolyline pLine = (ESRI.ArcGIS.Geometry.IPolyline)(new Polyline());

			for (int i = 0; i < n; i++)
			{
				IPoint ptCurr = turnObstList.Parts[i].pPtPrj;
				turnObstList.Parts[i].Height = ptCurr.Z - DER.pPtPrj[eRWY.PtDER].Z + turnObstList.Obstacles[turnObstList.Parts[i].Owner].VertAccuracy;

				double d = 1.0;

				if (!TurnAreaSecPolygon.IsEmpty)
					d = pProxiOperator.ReturnDistance(ptCurr);

				if (d == 0.0)
				{
					turnObstList.Parts[i].Prima = false;
					int Side0 = Functions.SideDef(ptNavPrj, DirToNav, ptCurr);
					double d0 = Functions.Point2LineDistancePrj(ptCurr, ptNavPrj, DirToNav);

					pLine.FromPoint = Functions.PointAlongPlane(ptCurr, DirToNav + 90.0 * Side0, d0);
					pLine.ToPoint = Functions.PointAlongPlane(ptCurr, DirToNav - 90.0 * Side0, GlobalVars.RModel + GlobalVars.RModel);

					IPointCollection pLine1 = (IPointCollection)PolyCut.ClipByPoly(pLine, TurnAreaSecPolygon);

					pLine.FromPoint = pLine1.get_Point(0);
					pLine.ToPoint = pLine1.get_Point(pLine1.PointCount - 1);

					int Side1 = Functions.SideDef(pLine.FromPoint, DirToNav, pLine.ToPoint);
					if (Side0 * Side1 < 0)
						pLine.ReverseOrientation();

					turnObstList.Parts[i].CLShift = (Functions.ReturnDistanceInMeters(ptCurr, pLine.ToPoint) + turnObstList.Obstacles[turnObstList.Parts[i].Owner].HorAccuracy) / pLine.Length;
					if (turnObstList.Parts[i].CLShift > 1.0)
					{
						turnObstList.Parts[i].CLShift = 1.0;
						turnObstList.Parts[i].Prima = true;
					}
				}
				else
				{
					turnObstList.Parts[i].Prima = true;
					turnObstList.Parts[i].CLShift = 1.0;
				}

				double tmpMOC = PANS_OPS_DataBase.dpMOC.Value * (turnObstList.Parts[i].Dist + turnObstList.Parts[i].DistStar);

				if (tmpMOC > MOCLimit)
					tmpMOC = MOCLimit;

				if (tmpMOC < PANS_OPS_DataBase.dpObsClr.Value)
					tmpMOC = PANS_OPS_DataBase.dpObsClr.Value;

				turnObstList.Parts[i].MOC = turnObstList.Parts[i].CLShift * tmpMOC;
			}
		}

		//public static void CalcObstaclesReqTNAH(ObstacleType[] znrList, double fPDG)
		//{
		//    int n = znrList.Length;
		//    if (n == 0)
		//        return;

		//    double TNIA_MOC_Bound = PANS_OPS_DataBase.dpObsClr.Value / PANS_OPS_DataBase.dpMOC.Value;

		//    for (int j = 0; j < n; j++)
		//    {
		//        if (znrList[j].Dist > TNIA_MOC_Bound)
		//            znrList[j].ReqTNH = PANS_OPS_DataBase.dpNGui_Ar1.Value;
		//        else
		//        {
		//            double fTmp;

		//            if (fPDG == PANS_OPS_DataBase.dpPDG_Nom.Value)
		//            {
		//                fTmp = znrList[j].Height + PANS_OPS_DataBase.dpObsClr.Value;
		//                if (PANS_OPS_DataBase.dpOIS_abv_DER.Value + fPDG * znrList[j].Dist >= fTmp)
		//                    fTmp = 0.0;
		//            }
		//            else
		//            {
		//                double X = (PANS_OPS_DataBase.dpObsClr.Value - PANS_OPS_DataBase.dpOIS_abv_DER.Value + znrList[j].Height - PANS_OPS_DataBase.dpPDG_Nom.Value * znrList[j].Dist) / (fPDG - PANS_OPS_DataBase.dpPDG_Nom.Value);

		//                if (X >= znrList[j].Dist)
		//                    fTmp = znrList[j].Height + PANS_OPS_DataBase.dpObsClr.Value;
		//                else
		//                    fTmp = PANS_OPS_DataBase.dpOIS_abv_DER.Value + fPDG * X;
		//            }

		//            znrList[j].ReqTNH = Math.Max(fTmp, PANS_OPS_DataBase.dpNGui_Ar1.Value);
		//        }
		//    }
		//}

		//public static void GetZNRObstList(ref ObstacleType[] fullList, out ObstacleType[] znrList, RWYType DER, double DepDir, double PDG, IPolygon pZNRPrimePoly, 
		//    bool Guided = false, IPoint ptNav = null)
		//{
		//    int n = fullList.Length;
		//    znrList = new ObstacleType[n];

		//    IPoint oPoint;
		//    if (Guided)
		//        oPoint = ptNav;
		//    else
		//        oPoint = DER.pPtPrj[eRWY.PtDER];

		//    IRelationalOperator pBaseRelate = (IRelationalOperator)pZNRPrimePoly;
		//    int j = -1;
		//    for (int i = 0; i < n; i++)
		//    {
		//        IPoint ptCurr = fullList[i].pPtPrj;
		//        if (pBaseRelate.Disjoint(ptCurr))
		//            continue;

		//        znrList[++j] = fullList[i];
		//        znrList[j].Prima = true;

		//        znrList[j].Dist = Functions.Point2LineDistancePrj(ptCurr, DER.pPtPrj[eRWY.PtDER], DepDir + 90.0) - znrList[j].HorAccuracy;
		//        if (znrList[j].Dist <= 0.0)
		//            znrList[j].Dist = GlobalVars.distEps;

		//        znrList[j].CLShift = Functions.Point2LineDistancePrj(ptCurr, oPoint, DepDir) - znrList[j].HorAccuracy;
		//        if (znrList[j].CLShift <= 0.0)
		//            znrList[j].CLShift = GlobalVars.distEps;
		//    }

		//    System.Array.Resize<ObstacleType>(ref znrList, j + 1);
		//}


		static void RemoveSeamPoints(ref IPointCollection pPoints)
		{
			int n = pPoints.PointCount;
			int j = 0;
			while (j < n - 1)
			{
				IPoint pCurrPt = pPoints.Point[j];
				int i = j + 1;
				while (i < n)
				{
					double fDist = ReturnDistanceInMeters(pCurrPt, pPoints.Point[i]);
					if (fDist < GlobalVars.distEps)
					{
						pPoints.RemovePoints(i, 1);
						n--;
					}
					else
						i++;
				}
				j++;
			}
		}

		static void RemoveSeamPoints(ref IPointCollection pPoints, IPoint ptTHR, double Direction)
		{
			double Xmin = 0.0, Ymin = 0.0;
			IPoint pMinXpt = null, pMinYpt = null;
			int n = pPoints.PointCount;

			for (int i = 0; i < n; i++)
			{
				double X, Y;
				IPoint pCurrPt = pPoints.Point[i];
				PrjToLocal(ptTHR, Direction, pCurrPt, out X, out Y);

				if (i == 0)
				{
					Xmin = Math.Abs(X);
					Ymin = Math.Abs(Y);
					pMinXpt = pCurrPt;
					pMinYpt = pCurrPt;
				}
				else
				{
					double fTmp = Math.Abs(X);
					if (Xmin > fTmp)
					{
						Xmin = fTmp;
						pMinXpt = pCurrPt;
					}

					fTmp = Math.Abs(Y);
					if (Ymin > fTmp)
					{
						Ymin = fTmp;
						pMinYpt = pCurrPt;
					}
				}
			}

			pPoints.RemovePoints(0, n);
			pPoints.AddPoint(pMinXpt);

			if (ReturnDistanceInMeters(pMinXpt, pMinYpt) > GlobalVars.distEps)
				pPoints.AddPoint(pMinYpt);
		}

		static int CreateObstacleParts(ref ObstacleContainer ObstList, IPolygon pBaseArea, IPolygon pBufferArea = null)
		{
			IPointCollection pTmpPoints;
			ITopologicalOperator2 pTopoOper;
			IRelationalOperator pRelational;

			int n = ObstList.Obstacles.Length;

			if (n < 0 || (pBufferArea == null && pBaseArea == null))
			{
				ObstList.Parts = new ObstacleData[0];
				return -1;
			}

			int k = -1, c = 10 * n;
			ObstList.Parts = new ObstacleData[c];

			for (int i = 0; i < n; i++)
			{
				IGeometry pCurrGeom = ObstList.Obstacles[i].pGeomPrj;
				IPointCollection pObstPoints = new ESRI.ArcGIS.Geometry.Multipoint();

				if (pBufferArea != null)
				{
					if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPoint)
					{
						pRelational = (IRelationalOperator)pBufferArea;
						if (pRelational.Contains(pCurrGeom))
							pObstPoints.AddPoint((IPoint)pCurrGeom);
					}
					else
					{
						pTopoOper = (ITopologicalOperator2)pCurrGeom;
						pTmpPoints = (IPointCollection)pTopoOper.Intersect(pBufferArea, esriGeometryDimension.esriGeometry0Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);
						pTmpPoints = (IPointCollection)pTopoOper.Intersect(pBufferArea, esriGeometryDimension.esriGeometry1Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);

						if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPolygon)
						{
							pTmpPoints = (IPointCollection)pTopoOper.Intersect(pBufferArea, esriGeometryDimension.esriGeometry2Dimension);
							pObstPoints.AddPointCollection(pTmpPoints);
						}
					}
				}

				if (pBaseArea != null)
				{
					if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPoint)
					{
						pRelational = (IRelationalOperator)pBaseArea;
						if (pRelational.Contains(pCurrGeom))
							pObstPoints.AddPoint((IPoint)pCurrGeom);
					}
					else
					{
						pTopoOper = (ITopologicalOperator2)pCurrGeom;
						pTmpPoints = (IPointCollection)pTopoOper.Intersect(pBaseArea, esriGeometryDimension.esriGeometry0Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);

						pTmpPoints = (IPointCollection)pTopoOper.Intersect(pBaseArea, esriGeometryDimension.esriGeometry1Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);

						if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPolygon)
						{
							pTmpPoints = (IPointCollection)pTopoOper.Intersect(pBaseArea, esriGeometryDimension.esriGeometry2Dimension);
							pObstPoints.AddPointCollection(pTmpPoints);
						}
					}
				}

				RemoveSeamPoints(ref pObstPoints);

				int p = pObstPoints.PointCount;
				ObstList.Obstacles[i].Parts = new int[p];

				for (int j = 0; j < p; j++)
				{
					IPoint pCurrPt = pObstPoints.Point[j];
					k++;
					if (k >= c)
					{
						c += n;
						System.Array.Resize<ObstacleData>(ref ObstList.Parts, c);
					}

					ObstList.Parts[k].pPtPrj = pCurrPt;
					ObstList.Parts[k].Owner = i;
					ObstList.Parts[k].Height = ObstList.Obstacles[i].Height;
					ObstList.Parts[k].Index = j;
					ObstList.Obstacles[i].Parts[j] = k;
				}
			}

			System.Array.Resize<ObstacleData>(ref ObstList.Parts, k + 1);
			return k;
		}

		public static void GetObstInRange(ObstacleContainer ObstSource, out ObstacleContainer ObstDest, double Range)
		{
			int m = ObstSource.Obstacles.Length;
			int n = ObstSource.Parts.Length;

			ObstDest.Obstacles = new Obstacle[m];
			ObstDest.Parts = new ObstacleData[n];

			if (n == 0)
				return;

			int i, k = -1, l = -1;

			for (i = 0; i < m; i++)
				ObstSource.Obstacles[i].NIx = -1;

			for (i = 0; i < n; i++)
			{
				if (ObstSource.Parts[i].Dist > Range)
					break;
				k++;
				ObstDest.Parts[k] = ObstSource.Parts[i];

				if (ObstSource.Obstacles[ObstSource.Parts[i].Owner].NIx < 0)
				{
					l++;
					ObstDest.Obstacles[l] = ObstSource.Obstacles[ObstSource.Parts[i].Owner];
					ObstDest.Obstacles[l].PartsNum = 0;
					System.Array.Resize<int>(ref ObstDest.Obstacles[l].Parts, ObstSource.Obstacles[ObstSource.Parts[i].Owner].PartsNum);
					ObstSource.Obstacles[ObstSource.Parts[i].Owner].NIx = l;
				}

				ObstDest.Parts[k].Owner = ObstSource.Obstacles[ObstSource.Parts[i].Owner].NIx;
				ObstDest.Parts[k].Index = ObstDest.Obstacles[ObstDest.Parts[k].Owner].PartsNum;
				ObstDest.Obstacles[ObstDest.Parts[k].Owner].Parts[ObstDest.Parts[k].Index] = k;
				ObstDest.Obstacles[ObstDest.Parts[k].Owner].PartsNum++;
			}

			System.Array.Resize<Obstacle>(ref ObstDest.Obstacles, l + 1);
			System.Array.Resize<ObstacleData>(ref ObstDest.Parts, k + 1);
		}

		public static void GetObstListInPoly(ObstacleContainer fullList, out ObstacleContainer outList, IPolygon pPolygon)
		{
			int m = fullList.Obstacles.Length;

			if (m == 0 || pPolygon.IsEmpty)
			{
				outList.Obstacles = new Obstacle[0];
				outList.Parts = new ObstacleData[0];
				return;
			}

			outList.Obstacles = new Obstacle[m];

			int n = Math.Max(fullList.Parts.Length, m);
			int c = n, k = -1, l = -1;

			outList.Parts = new ObstacleData[c];

			ITopologicalOperator2 pTopoOper;
			IProximityOperator pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pPolygon;
			IPolyline pLine1 = (IPolyline)(new ESRI.ArcGIS.Geometry.Polyline());

			for (int i = 0; i < m; i++)
			{
				IGeometry pCurrGeom = fullList.Obstacles[i].pGeomPrj;

				if (pProxi.ReturnDistance(pCurrGeom) != 0.0)
					continue;

				IPointCollection pObstPoints = new ESRI.ArcGIS.Geometry.Multipoint();
				if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPoint)
					pObstPoints.AddPoint((IPoint)pCurrGeom);
				else
				{
					pTopoOper = (ITopologicalOperator2)pCurrGeom;
					pObstPoints.AddPointCollection((IPointCollection)pPolygon);

					IPointCollection pTmpPoints = (IPointCollection)pTopoOper.Intersect(pPolygon, esriGeometryDimension.esriGeometry0Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					pTmpPoints = (IPointCollection)pTopoOper.Intersect(pPolygon, esriGeometryDimension.esriGeometry1Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPolygon)
					{
						pTmpPoints = (IPointCollection)pTopoOper.Intersect(pPolygon, esriGeometryDimension.esriGeometry2Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);
					}

					RemoveSeamPoints(ref pObstPoints);
				}

				int p = pObstPoints.PointCount;
				if (p == 0)
					continue;

				l++;
				outList.Obstacles[l] = fullList.Obstacles[i];
				outList.Obstacles[l].PartsNum = p;
				outList.Obstacles[l].Parts = new int[p];

				for (int j = 0; j < p; j++)
				{
					k++;
					if (k >= c)
					{
						c += n;
						System.Array.Resize<ObstacleData>(ref outList.Parts, c);
					}

					IPoint pCurrPt = pObstPoints.Point[j];

					outList.Parts[k].pPtPrj = pCurrPt;
					outList.Parts[k].Owner = l;
					outList.Parts[k].Height = outList.Obstacles[l].Height;
					outList.Parts[k].Index = j;
					outList.Obstacles[l].Parts[j] = k;
				}
			}

			System.Array.Resize<Obstacle>(ref outList.Obstacles, l + 1);
			System.Array.Resize<ObstacleData>(ref outList.Parts, k + 1);
		}

		public static void GetTurnAreaObstacles(ObstacleContainer fullList, out ObstacleContainer turnAreaList, IPolygon pFullPoly, IPolygon pSecPoly, IGeometry pZNRPoly,
			RWYType DER, double DepDir, double DirToNav, IPoint ptNavPrj, double MOCLimit)
		{
			int m = fullList.Obstacles.Length;
			turnAreaList.Obstacles = new Obstacle[m];

			if (m == 0 || pFullPoly.IsEmpty)
			{
				turnAreaList.Parts = new ObstacleData[0];
				return;
			}

			int n = Math.Max(fullList.Parts.Length, m);
			int c = n, k = -1, l = -1;

			turnAreaList.Parts = new ObstacleData[c];

			ITopologicalOperator2 pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pFullPoly;
			pTopoOper.IsKnownSimple_2 = false;
			pTopoOper.Simplify();

			pTopoOper = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pSecPoly;
			pTopoOper.IsKnownSimple_2 = false;
			pTopoOper.Simplify();

			IProximityOperator pBaseProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pFullPoly;
			IProximityOperator pSecondProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pSecPoly;
			IRelationalOperator pSecRelate = (IRelationalOperator)pSecPoly;

			IRelationalOperator pZNRRelate = (IRelationalOperator)pZNRPoly;
			IProximityOperator pZNRProxi = (IProximityOperator)pZNRPoly;

			IPolyline pLine = (IPolyline)(new ESRI.ArcGIS.Geometry.Polyline());

			for (int i = 0; i < m; i++)
			{
				IGeometry pCurrGeom = fullList.Obstacles[i].pGeomPrj;

				if (pBaseProxi.ReturnDistance(pCurrGeom) != 0.0)
					continue;

				IPointCollection pObstPoints;
				if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPoint)
				{
					pObstPoints = new ESRI.ArcGIS.Geometry.Multipoint();
					pObstPoints.AddPoint((IPoint)pCurrGeom);
				}
				else
				{
					pTopoOper = (ITopologicalOperator2)pCurrGeom;

					pObstPoints = (IPointCollection)pTopoOper.Intersect(pSecPoly, esriGeometryDimension.esriGeometry0Dimension);
					IPointCollection pTmpPoints = (IPointCollection)pTopoOper.Intersect(pSecPoly, esriGeometryDimension.esriGeometry1Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPolygon)
					{
						pTmpPoints = (IPointCollection)pTopoOper.Intersect(pSecPoly, esriGeometryDimension.esriGeometry2Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);
					}

					pTmpPoints = (IPointCollection)pTopoOper.Intersect(pFullPoly, esriGeometryDimension.esriGeometry0Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					pTmpPoints = (IPointCollection)pTopoOper.Intersect(pFullPoly, esriGeometryDimension.esriGeometry1Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPolygon)
					{
						pTmpPoints = (IPointCollection)pTopoOper.Intersect(pFullPoly, esriGeometryDimension.esriGeometry2Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);
					}
					RemoveSeamPoints(ref pObstPoints);
				}

				int p = pObstPoints.PointCount;
				if (p == 0)
					continue;

				l++;
				turnAreaList.Obstacles[l] = fullList.Obstacles[i];
				turnAreaList.Obstacles[l].PartsNum = p;
				turnAreaList.Obstacles[l].Parts = new int[p];

				for (int j = 0; j < p; j++)
				{
					k++;
					if (k >= c)
					{
						c += n;
						System.Array.Resize<ObstacleData>(ref turnAreaList.Parts, c);
					}

					IPoint ptCurr = pObstPoints.Point[j];

					turnAreaList.Parts[k].pPtPrj = ptCurr;
					turnAreaList.Parts[k].Owner = l;
					turnAreaList.Parts[k].Height = turnAreaList.Obstacles[l].Height;
					turnAreaList.Parts[k].Index = j;
					turnAreaList.Obstacles[l].Parts[j] = k;

					if (!pSecRelate.Disjoint(ptCurr))
					{
						turnAreaList.Parts[k].Prima = false;
						int Side0 = Functions.SideDef(ptNavPrj, DirToNav, ptCurr);
						double d0 = Functions.Point2LineDistancePrj(ptCurr, ptNavPrj, DirToNav);

						pLine.FromPoint = Functions.PointAlongPlane(ptCurr, DirToNav + 90.0 * Side0, d0);
						pLine.ToPoint = Functions.PointAlongPlane(ptCurr, DirToNav - 90.0 * Side0, GlobalVars.RModel + GlobalVars.RModel);

						//Functions.DrawPolygon(pSecPoly, -1, esriSimpleFillStyle.esriSFSCross);
						//Functions.DrawPolyline(pLine, 255,2);
						//Functions.DrawPointWithText(ptCurr, "Obst");

						IPointCollection pLine1 = (IPointCollection)PolyCut.ClipByPoly(pLine, pSecPoly);

						pLine.FromPoint = pLine1.Point[0];
						pLine.ToPoint = pLine1.Point[pLine1.PointCount - 1];

						int Side1 = Functions.SideDef(pLine.FromPoint, DirToNav, pLine.ToPoint);
						if (Side0 * Side1 < 0)
							pLine.ReverseOrientation();

						turnAreaList.Parts[k].CLShift = (Functions.ReturnDistanceInMeters(ptCurr, pLine.ToPoint) + turnAreaList.Obstacles[l].HorAccuracy) / pLine.Length;
						if (turnAreaList.Parts[k].CLShift > 1.0)
						{
							turnAreaList.Parts[k].CLShift = 1.0;
							turnAreaList.Parts[k].Prima = true;
						}
					}
					else
					{
						turnAreaList.Parts[k].CLShift = 1.0;
						turnAreaList.Parts[k].Prima = true;
					}

					IPoint ptTmp = pZNRProxi.ReturnNearestPoint(ptCurr, esriSegmentExtension.esriNoExtension);

					turnAreaList.Parts[k].DistStar = Functions.Point2LineDistancePrjSgn(ptTmp, DER.pPtPrj[eRWY.PtDER], DepDir - 90.0) - turnAreaList.Obstacles[l].HorAccuracy;
					if (turnAreaList.Parts[k].DistStar < GlobalVars.distEps)
						turnAreaList.Parts[k].DistStar = GlobalVars.distEps;

					double d = pZNRProxi.ReturnDistance(ptCurr);
					turnAreaList.Parts[k].Dist = d - turnAreaList.Obstacles[l].HorAccuracy;
					if (turnAreaList.Parts[k].Dist < GlobalVars.distEps)
						turnAreaList.Parts[k].Dist = GlobalVars.distEps;

					double tmpMOC = PANS_OPS_DataBase.dpMOC.Value * (turnAreaList.Parts[k].Dist + turnAreaList.Parts[k].DistStar);

					if (tmpMOC > MOCLimit)
						tmpMOC = MOCLimit;

					if (tmpMOC < PANS_OPS_DataBase.dpObsClr.Value)
						tmpMOC = PANS_OPS_DataBase.dpObsClr.Value;

					turnAreaList.Parts[k].MOC = turnAreaList.Parts[k].CLShift * tmpMOC;
				}
			}

			System.Array.Resize<Obstacle>(ref turnAreaList.Obstacles, l + 1);
			System.Array.Resize<ObstacleData>(ref turnAreaList.Parts, k + 1);
		}

		public static void GetZNRObstList(ObstacleContainer fullList, out ObstacleContainer znrList, RWYType DER, double DepDir, double fPDG,
			IPolygon pZNRPrimePoly, IPolygon pZNRSecPoly = null, IPoint ptNavPrj = null)
		{
			int m = fullList.Obstacles.Length;
			znrList.Obstacles = new Obstacle[m];

			if (m == 0 || pZNRPrimePoly.IsEmpty)
			{
				znrList.Parts = new ObstacleData[0];
				return;
			}

			int n = Math.Max(fullList.Parts.Length, m);
			int c = n, k = -1, l = -1;

			znrList.Parts = new ObstacleData[c];

			ITopologicalOperator2 pTopoOper;
			IProximityOperator pPrimeProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pZNRPrimePoly;
			IProximityOperator pSecondProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pZNRSecPoly;

			IRelationalOperator pPrimRelate = (IRelationalOperator)pZNRPrimePoly;
			IRelationalOperator pSecRelate = (IRelationalOperator)pZNRSecPoly;

			IPolyline pLine1 = (IPolyline)(new ESRI.ArcGIS.Geometry.Polyline());

			bool Guided = pZNRPrimePoly != null && ptNavPrj != null;

			IPoint oPoint;
			if (Guided)
				oPoint = ptNavPrj;
			else
			{
				oPoint = DER.pPtPrj[eRWY.PtDER];
				//pSecRelate = pPrimRelate;
			}

			double TNIA_MOC_Bound = PANS_OPS_DataBase.dpObsClr.Value / PANS_OPS_DataBase.dpMOC.Value;

			for (int i = 0; i < m; i++)
			{
				//if (fullList.Obstacles[fullList.Parts[i].Owner].UnicalName == "8026")
				//{
				//    string TypeName = fullList.Obstacles[fullList.Parts[i].Owner].TypeName;
				//}

				IGeometry pCurrGeom = fullList.Obstacles[i].pGeomPrj;
				if (pSecondProxi == null)
				{
					if (pPrimeProxi.ReturnDistance(pCurrGeom) != 0.0)
						continue;
				}
				else if (pSecondProxi.ReturnDistance(pCurrGeom) != 0.0 && pPrimeProxi.ReturnDistance(pCurrGeom) != 0.0)
					continue;

				IPointCollection pObstPoints;
				pObstPoints = new ESRI.ArcGIS.Geometry.Multipoint();
				if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPoint)
					pObstPoints.AddPoint((IPoint)pCurrGeom);
				else
				{
					pTopoOper = (ITopologicalOperator2)pCurrGeom;
					IPointCollection pTmpPoints;

					pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRPrimePoly, esriGeometryDimension.esriGeometry0Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRPrimePoly, esriGeometryDimension.esriGeometry1Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPolygon)
					{
						pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRPrimePoly, esriGeometryDimension.esriGeometry2Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);
					}

					if (pZNRSecPoly != null)
					{
						pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRSecPoly, esriGeometryDimension.esriGeometry0Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);

						pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRSecPoly, esriGeometryDimension.esriGeometry1Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);

						if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPolygon)
						{
							pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRSecPoly, esriGeometryDimension.esriGeometry2Dimension);
							pObstPoints.AddPointCollection(pTmpPoints);
						}
					}

					RemoveSeamPoints(ref pObstPoints);
				}

				int p = pObstPoints.PointCount;
				if (p == 0)
					continue;

				l++;
				znrList.Obstacles[l] = fullList.Obstacles[i];
				znrList.Obstacles[l].PartsNum = p;
				znrList.Obstacles[l].Parts = new int[p];

				for (int j = 0; j < p; j++)
				{
					k++;
					if (k >= c)
					{
						c += n;
						System.Array.Resize<ObstacleData>(ref znrList.Parts, c);
					}

					IPoint ptCurr = pObstPoints.Point[j];

					znrList.Parts[k].pPtPrj = ptCurr;
					znrList.Parts[k].Owner = l;
					znrList.Parts[k].Height = znrList.Obstacles[l].Height;
					znrList.Parts[k].Index = j;
					znrList.Obstacles[l].Parts[j] = k;

					znrList.Parts[k].Prima = (!Guided) || pSecRelate.Disjoint(ptCurr);

					znrList.Parts[k].Dist = Functions.Point2LineDistancePrj(ptCurr, DER.pPtPrj[eRWY.PtDER], DepDir + 90.0) - znrList.Obstacles[l].HorAccuracy;
					if (znrList.Parts[k].Dist <= 0.0)
						znrList.Parts[k].Dist = GlobalVars.distEps;

					znrList.Parts[k].CLShift = Functions.Point2LineDistancePrj(ptCurr, oPoint, DepDir) - znrList.Obstacles[l].HorAccuracy;
					if (znrList.Parts[k].CLShift <= 0.0)
						znrList.Parts[k].CLShift = GlobalVars.distEps;

					if (znrList.Parts[k].Dist > TNIA_MOC_Bound)
						znrList.Parts[k].ReqTNH = PANS_OPS_DataBase.dpNGui_Ar1.Value;
					else
					{
						double fTmp;

						if (fPDG == PANS_OPS_DataBase.dpPDG_Nom.Value)
						{
							fTmp = znrList.Parts[k].Height + PANS_OPS_DataBase.dpObsClr.Value;
							if (PANS_OPS_DataBase.dpOIS_abv_DER.Value + fPDG * znrList.Parts[k].Dist >= fTmp)
								fTmp = 0.0;
						}
						else
						{
							double X = (PANS_OPS_DataBase.dpObsClr.Value - PANS_OPS_DataBase.dpOIS_abv_DER.Value + znrList.Parts[k].Height -
										PANS_OPS_DataBase.dpPDG_Nom.Value * znrList.Parts[k].Dist) / (fPDG - PANS_OPS_DataBase.dpPDG_Nom.Value);

							if (X >= znrList.Parts[k].Dist)
								fTmp = znrList.Parts[k].Height + PANS_OPS_DataBase.dpObsClr.Value;
							else
								fTmp = PANS_OPS_DataBase.dpOIS_abv_DER.Value + fPDG * X;
						}

						znrList.Parts[k].ReqTNH = Math.Max(fTmp, PANS_OPS_DataBase.dpNGui_Ar1.Value);
					}
				}
			}

			System.Array.Resize<Obstacle>(ref znrList.Obstacles, l + 1);
			System.Array.Resize<ObstacleData>(ref znrList.Parts, k + 1);

			if (k > 0)
				Sort(znrList.Parts);
		}

		/*
		static void GetFinalObstacles(ObstacleContainer fullList, out ObstacleContainer znrList, IPolygon pZNRPrimePoly, IPolygon pZNRSecPoly, IPolygon SecPoly0, IGeometry pZNRPoly, double TurnAngle, double ArDir, double OutAzt, IPoint ptNavPrj, IPoint FixPntPrj, out int iMax) 
		{
			iMax = -1;

			int m = fullList.Obstacles.Length;
			znrList.Obstacles = new Obstacle[m];

			if (m == 0 || pZNRPrimePoly.IsEmpty)
			{
				znrList.Parts = new ObstacleData[0];
				return;
			}

			int n = Math.Max(fullList.Parts.Length, m);
			int c = n, k = -1, l = -1;
			znrList.Parts = new ObstacleData[c];

			double hMax = -9999.0, MOCValue;
			if (TurnAngle > PANS_OPS_DataBase.dpTr_AdjAngle.Value)
				MOCValue = PANS_OPS_DataBase.dpMOC.Value;
			else
				MOCValue = PANS_OPS_DataBase.dpMOC.Value;

			ITopologicalOperator2 pTopoOper;
			IProximityOperator pBaseProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pZNRPrimePoly;
			IProximityOperator pSecondProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pZNRSecPoly;
			IProximityOperator pSecond0Proxi = (ESRI.ArcGIS.Geometry.IProximityOperator)SecPoly0;
			IProximityOperator pDistProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pZNRPoly;
			IPolyline pLine1 = (IPolyline)(new ESRI.ArcGIS.Geometry.Polyline());

			for (int i = 0; i < m; i++)
			{
				IGeometry pCurrGeom = fullList.Obstacles[i].pGeomPrj;

				if (pBaseProxi.ReturnDistance(pCurrGeom) != 0.0)
					continue;

				IPointCollection pObstPoints;
				if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPoint)
				{
					pObstPoints = new ESRI.ArcGIS.Geometry.Multipoint();
					pObstPoints.AddPoint((IPoint)pCurrGeom);
				}
				else
				{
					pTopoOper = (ITopologicalOperator2)pCurrGeom;

					pObstPoints = (IPointCollection)pTopoOper.Intersect(pZNRSecPoly, esriGeometryDimension.esriGeometry0Dimension);
					IPointCollection pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRSecPoly, esriGeometryDimension.esriGeometry1Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPolygon)
					{
						pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRSecPoly, esriGeometryDimension.esriGeometry2Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);
					}

					pTmpPoints = (IPointCollection)pTopoOper.Intersect(SecPoly0, esriGeometryDimension.esriGeometry0Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					pTmpPoints = (IPointCollection)pTopoOper.Intersect(SecPoly0, esriGeometryDimension.esriGeometry1Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPolygon)
					{
						pTmpPoints = (IPointCollection)pTopoOper.Intersect(SecPoly0, esriGeometryDimension.esriGeometry2Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);
					}

					pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRPrimePoly, esriGeometryDimension.esriGeometry0Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRPrimePoly, esriGeometryDimension.esriGeometry1Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPolygon)
					{
						pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRPrimePoly, esriGeometryDimension.esriGeometry2Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);
					}
					RemoveSeamPoints(ref pObstPoints);
				}

				int p = pObstPoints.PointCount;
				if (p == 0)
					continue;

				l++;
				znrList.Obstacles[l] = fullList.Obstacles[i];
				znrList.Obstacles[l].PartsNum = p;
				znrList.Obstacles[l].Parts = new int[p];

				for (int j = 0; j < p; j++)
				{
					k++;
					if (k >= c)
					{
						c += n;
						System.Array.Resize<ObstacleData>(ref znrList.Parts, c);
					}

					IPoint pCurrPt = pObstPoints.Point[j];

					znrList.Parts[k].pPtPrj = pCurrPt;
					znrList.Parts[k].Owner = l;
					znrList.Parts[k].Height = znrList.Obstacles[l].Height;
					znrList.Parts[k].Index = j;
					znrList.Obstacles[l].Parts[j] = k;

					znrList.Parts[k].AreaFlags = 0;
					znrList.Parts[k].fTmp = 1.0;

					int Side0, Side1;
					double dist, dCL;
					IGeometry pGeometry = (IGeometry)SecPoly0;
					IPolyline pLine;

					if (!pGeometry.IsEmpty)
						dist = pSecond0Proxi.ReturnDistance(pCurrPt);
					else
						dist = 10.0;

					if (dist == 0.0)
					{
						znrList.Parts[k].AreaFlags = 2;

						pTopoOper = (ITopologicalOperator2)SecPoly0;
						Side0 = SideDef(ptNavPrj, ArDir, pCurrPt);
						dCL = Point2LineDistancePrj(pCurrPt, ptNavPrj, ArDir);

						pLine1.FromPoint = PointAlongPlane(pCurrPt, ArDir + 90.0 * Side0, dCL);
						pLine1.ToPoint = PointAlongPlane(pCurrPt, ArDir - 90.0 * Side0, 10.0 * GlobalVars.RModel);

						pLine = (IPolyline)pTopoOper.Intersect(pLine1, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension);

						Side1 = SideDef(pLine.FromPoint, ArDir, pLine.ToPoint);

						if (Side0 * Side1 < 0)
							pLine.ReverseOrientation();

						znrList.Parts[k].fTmp = ReturnDistanceInMeters(pCurrPt, pLine.ToPoint) / pLine.Length;
						if (znrList.Parts[k].fTmp > 1.0)
							znrList.Parts[k].fTmp = 1.0;
					}
					else
					{
						pGeometry = (IGeometry)pZNRSecPoly;
						if (!pGeometry.IsEmpty)
							dist = pSecondProxi.ReturnDistance(pCurrPt);
						else
							dist = 10.0;

						if (dist == 0.0)
						{
							znrList.Parts[k].AreaFlags = 1;
							pTopoOper = (ITopologicalOperator2)pZNRSecPoly;
							Side0 = SideDef(FixPntPrj, OutAzt, pCurrPt);
							dCL = Point2LineDistancePrj(pCurrPt, FixPntPrj, OutAzt);

							pLine1.FromPoint = PointAlongPlane(pCurrPt, OutAzt + 90.0 * Side0, dCL);
							pLine1.ToPoint = PointAlongPlane(pCurrPt, OutAzt - 90.0 * Side0, 10.0 * GlobalVars.RModel);
							pLine = (IPolyline)pTopoOper.Intersect(pLine1, ESRI.ArcGIS.Geometry.esriGeometryDimension.esriGeometry1Dimension);

							Side1 = SideDef(pLine.FromPoint, OutAzt, pLine.ToPoint);

							if (Side0 * Side1 < 0)
								pLine.ReverseOrientation();

							znrList.Parts[k].fTmp = ReturnDistanceInMeters(pCurrPt, pLine.ToPoint) / pLine.Length;
							if (znrList.Parts[k].fTmp > 1.0)
								znrList.Parts[k].fTmp = 1.0;
						}
					}

					znrList.Parts[k].MOC = MOCValue * znrList.Parts[k].fTmp;
					znrList.Parts[k].ReqH = znrList.Parts[k].Height + znrList.Parts[k].MOC;
					znrList.Parts[k].Dist = pDistProxi.ReturnDistance(pCurrPt);
					//AvDist += ObstacleFinalMOCList.Parts[j].Dist;
					if (znrList.Parts[k].ReqH > hMax)
					{
						hMax = znrList.Parts[k].ReqH;
						iMax = k;
					}
				}
			}

			System.Array.Resize<Obstacle>(ref znrList.Obstacles, l + 1);
			System.Array.Resize<ObstacleData>(ref znrList.Parts, k + 1);

			//return iMax;
		}
		*/

		public static void ClassifyObstacles(ObstacleContainer fullList, out ObstacleContainer innerList, out ObstacleContainer outerList, IGeometry pZNRPoly, RWYType DER, double DepDir)
		{
			IProximityOperator pProxiOperator = (IProximityOperator)pZNRPoly;
			int kIn, kOut, lIn, lOut, cIn, cOut, n = fullList.Obstacles.Length;
			cIn = cOut = 10 * n;

			innerList.Obstacles = new Obstacle[n];
			innerList.Parts = new ObstacleData[cIn];

			outerList.Obstacles = new Obstacle[n];
			outerList.Parts = new ObstacleData[cOut];

			kIn = kOut = lIn = lOut = -1;

			for (int i = 0; i < n; i++)
			{
				IGeometry pCurrGeom = fullList.Obstacles[i].pGeomPrj;
				IPointCollection pObstPoints = new ESRI.ArcGIS.Geometry.Multipoint();

				if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPoint)
					pObstPoints.AddPoint((IPoint)pCurrGeom);
				else
				{
					ITopologicalOperator2 pTopoOper = (ITopologicalOperator2)pCurrGeom;
					pObstPoints.AddPointCollection((IPointCollection)pCurrGeom);

					IPointCollection pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRPoly, esriGeometryDimension.esriGeometry0Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRPoly, esriGeometryDimension.esriGeometry1Dimension);
					pObstPoints.AddPointCollection(pTmpPoints);

					if (pCurrGeom.GeometryType == esriGeometryType.esriGeometryPolygon)
					{
						pTmpPoints = (IPointCollection)pTopoOper.Intersect(pZNRPoly, esriGeometryDimension.esriGeometry2Dimension);
						pObstPoints.AddPointCollection(pTmpPoints);
					}

					RemoveSeamPoints(ref pObstPoints);
				}

				int p = pObstPoints.PointCount;
				if (p == 0)
					continue;
				bool bInAdded = false, bOutAdded = false;

				for (int j = 0; j < p; j++)
				{
					IPoint ptCurr = pObstPoints.Point[j];
					double d = pProxiOperator.ReturnDistance(ptCurr);

					if (d == 0.0)
					{
						if (!bInAdded)
						{
							lIn++;
							innerList.Obstacles[lIn] = fullList.Obstacles[i];
							innerList.Obstacles[lIn].PartsNum = 0;
							innerList.Obstacles[lIn].Parts = new int[p];
							bInAdded = true;
						}

						kIn++;
						if (kIn >= cIn)
						{
							cIn += n;
							System.Array.Resize<ObstacleData>(ref innerList.Parts, cIn);
						}

						innerList.Parts[kIn].pPtPrj = ptCurr;
						innerList.Parts[kIn].Owner = lIn;
						innerList.Parts[kIn].Height = innerList.Obstacles[lIn].Height;
						innerList.Parts[kIn].Index = innerList.Obstacles[lIn].PartsNum;

						innerList.Obstacles[lIn].Parts[innerList.Parts[kIn].Index] = kIn;
						innerList.Obstacles[lIn].PartsNum++;

						innerList.Parts[kIn].Dist = Functions.Point2LineDistancePrj(ptCurr, DER.pPtPrj[eRWY.PtDER], DepDir + 90.0) - innerList.Obstacles[lIn].HorAccuracy;
						if (innerList.Parts[kIn].Dist <= 0.0)
							innerList.Parts[kIn].Dist = GlobalVars.distEps;

						innerList.Parts[kIn].CLShift = Functions.Point2LineDistancePrj(ptCurr, DER.pPtPrj[eRWY.PtDER], DepDir) - innerList.Obstacles[lIn].HorAccuracy;

						if (innerList.Parts[kIn].CLShift <= 0.0)
							innerList.Parts[kIn].CLShift = GlobalVars.distEps;
					}
					else
					{
						if (!bOutAdded)
						{
							lOut++;
							outerList.Obstacles[lOut] = fullList.Obstacles[i];
							outerList.Obstacles[lOut].PartsNum = 0;
							outerList.Obstacles[lOut].Parts = new int[p];
							bOutAdded = true;
						}

						kOut++;
						if (kOut >= cOut)
						{
							cOut += n;
							System.Array.Resize<ObstacleData>(ref outerList.Parts, cOut);
						}

						outerList.Parts[kOut].pPtPrj = ptCurr;
						outerList.Parts[kOut].Owner = lOut;
						outerList.Parts[kOut].Height = outerList.Obstacles[lOut].Height;
						outerList.Parts[kOut].Index = outerList.Obstacles[lOut].PartsNum;

						outerList.Obstacles[lOut].Parts[outerList.Parts[kOut].Index] = kOut;
						outerList.Obstacles[lOut].PartsNum++;

						IPoint ptTmp = pProxiOperator.ReturnNearestPoint(ptCurr, esriSegmentExtension.esriNoExtension);
						outerList.Parts[kOut].DistStar = Functions.Point2LineDistancePrjSgn(ptTmp, DER.pPtPrj[eRWY.PtDER], DepDir - 90.0) - outerList.Obstacles[lOut].HorAccuracy;
						if (outerList.Parts[kOut].DistStar < GlobalVars.distEps)
							outerList.Parts[kOut].DistStar = GlobalVars.distEps;

						outerList.Parts[kOut].Dist = d - outerList.Obstacles[lOut].HorAccuracy;
						if (outerList.Parts[kOut].Dist < GlobalVars.distEps)
							outerList.Parts[kOut].Dist = GlobalVars.distEps;
					}
				}

				if (bInAdded)
					System.Array.Resize<int>(ref innerList.Obstacles[lIn].Parts, innerList.Obstacles[lIn].PartsNum);

				if (bOutAdded)
					System.Array.Resize<int>(ref outerList.Obstacles[lOut].Parts, outerList.Obstacles[lOut].PartsNum);
			}

			System.Array.Resize<Obstacle>(ref innerList.Obstacles, lIn + 1);
			System.Array.Resize<ObstacleData>(ref innerList.Parts, kIn + 1);

			System.Array.Resize<Obstacle>(ref outerList.Obstacles, lOut + 1);
			System.Array.Resize<ObstacleData>(ref outerList.Parts, kOut + 1);

			if (kIn > 0)
				Sort(innerList.Parts);
		}

		public static int SideDef(IPoint PtInLine, double LineAngle, IPoint PtOutLine)
		{
			double Angle12 = Functions.ReturnAngleInDegrees(PtInLine, PtOutLine);
			double dAngle = NativeMethods.Modulus(LineAngle - Angle12, 360.0);

			if (dAngle == 0.0 || dAngle == 180.0)
				return 0;

			if (dAngle < 180.0)
				return 1;

			return -1;
		}

		public static SquareSolutionArea CalcZeroDTNAH(double dTNA, double PDGznr, double PDGzr, double alpha,
														double Beta, double dr, double ObsMOC, double LAr12, double MOCLimit)
		{
			double MOC008 = PANS_OPS_DataBase.dpMOC.Value * LAr12;
			if (MOC008 > MOCLimit)
				MOC008 = MOCLimit;

			double Cos30 = System.Math.Cos(GlobalVars.DegToRadValue * Beta);
			double CosAlpha = System.Math.Cos(GlobalVars.DegToRadValue * alpha);
			double SinAlpha = System.Math.Sin(GlobalVars.DegToRadValue * alpha);

			double X01 = 0.0, X02 = 0.0;
			double X11 = 0.0, X12 = 0.0;

			double PDGznr1 = PDGznr;// - PANS_OPS_DataBase.dpMOC.Value;
			double PDGzr1 = PDGzr;// - PANS_OPS_DataBase.dpMOC.Value;

			//======================================================================================================================
			double N0 = dTNA + PDGzr * dr - ObsMOC + PANS_OPS_DataBase.dpObsClr.Value;
			double N1 = dTNA + PDGzr * dr - ObsMOC + MOC008;
			//double N0 = dTNA;

			if (ObsMOC < MOCLimit)
			{
				PDGznr1 -= PANS_OPS_DataBase.dpMOC.Value;
				PDGzr1 -= PANS_OPS_DataBase.dpMOC.Value;
			}
			else
			{
				N0 = dTNA + PDGzr * dr;
				N1 = dTNA + PDGzr * dr;
			}

			//======================================================================================================================
			// B = 2.0 * ((PDGzr - PDGznr * Cos30 / CosAlpha) * PDGzr * dr * CosAlpha * Cos30 - PDGznr * N * Cos30 * Cos30)
			// C = (N * Cos30) ^ 2 + 2.0 * N * PDGzr * dr * CosAlpha * Cos30 * Cos30 / CosAlpha

			double A0 = Math.Pow((PDGznr * Cos30), 2.0) - PDGzr * PDGzr;
			double B0 = 2.0 * Cos30 * (dr * CosAlpha * PDGzr * PDGzr - N0 * PDGznr * Cos30);
			double C0 = (N0 * N0 - Math.Pow((dr * PDGzr), 2)) * Cos30 * Cos30;
			double d0 = B0 * B0 - 4.0 * A0 * C0;
			//======================================================================================================================

			//double N1 = dTNA;

			double A1 = Math.Pow((PDGznr1 * Cos30), 2.0) - PDGzr1 * PDGzr1;
			double B1 = 2.0 * Cos30 * (dr * CosAlpha * PDGzr1 * PDGzr1 - N1 * PDGznr1 * Cos30);
			double C1 = (N1 * N1 - Math.Pow((dr * PDGzr1), 2)) * Cos30 * Cos30;
			double d1 = B1 * B1 - 4.0 * A1 * C1;
			//======================================================================================================================

			SquareSolutionArea calcZeroDTNAHReturn = new SquareSolutionArea();
			calcZeroDTNAHReturn.Solutions = 0;

			if (d0 < 0)
			{
				if (d1 < 0)
				{
					calcZeroDTNAHReturn.Second_Renamed = 0.0;
					calcZeroDTNAHReturn.Solutions = 0;
					return calcZeroDTNAHReturn;
				}
			}
			else
			{
				double d0Sqrt = System.Math.Sqrt(d0);
				X01 = (-B0 - d0Sqrt) / (2.0 * A0);
				X02 = (-B0 + d0Sqrt) / (2.0 * A0);

				if (X01 > X02)
				{
					double fTmp = X01;
					X01 = X02;
					X02 = fTmp;
				}

				if (X02 < 0.0)
					X02 = 0.0;
			}

			if (d1 >= 0)
			{
				double d1Sqrt = System.Math.Sqrt(d1);

				X11 = (-B1 - d1Sqrt) / (2.0 * A1);
				X12 = (-B1 + d1Sqrt) / (2.0 * A1);

				if (X11 > X12)
				{
					double fTmp = X11;
					X11 = X12;
					X12 = fTmp;
				}

				if (X12 < 0.0)
					X12 = 0.0;
			}

			//========================================================================================
			const double Epsilon = 0.0001;

			bool Res1IsSolution = false, Res2IsSolution = false;
			double Y, balans, NewMOC = 0.0;

			double X0 = dr * CosAlpha * Cos30;

			if (ObsMOC <= PANS_OPS_DataBase.dpObsClr.Value)
			{
				Y = balans = 0;
				if (X01 > X0)
				{
					//X01 = X0;
					//Y = dr * SinAlpha;
					//balans = dTNA - X01 * PDGznr - (Y - dr) * PDGzr;
					//Res1IsSolution = balans >= -Epsilon;
				}
				else if (X01 >= 0.0)
				{
					Y = System.Math.Sqrt(dr * dr - 2.0 * dr * CosAlpha / Cos30 * X01 + X01 * X01 / (Cos30 * Cos30));
					balans = dTNA - X01 * PDGznr - (Y - dr) * PDGzr;
					Res1IsSolution = balans >= -Epsilon;
				}

				if (Res1IsSolution)
				{
					if (balans > Epsilon)
						X01 = X01 + balans / PDGznr;

					NewMOC = (LAr12 + X01 + Y) * PANS_OPS_DataBase.dpMOC.Value;
					if (NewMOC > MOCLimit)
						NewMOC = MOCLimit;
				}
			}

			if (Res1IsSolution && ((ObsMOC > PANS_OPS_DataBase.dpObsClr.Value) || (NewMOC > PANS_OPS_DataBase.dpObsClr.Value)))
			{
				if ((X11 > X0))
				{
					X11 = X0;
					Y = dr * SinAlpha;
				}
				else
					Y = System.Math.Sqrt(dr * dr - 2.0 * dr * CosAlpha / Cos30 * X11 + X11 * X11 / (Cos30 * Cos30));

				balans = dTNA - ObsMOC + dr * PDGzr + MOC008 - X11 * PDGznr1 - Y * PDGzr1;
				Res1IsSolution = balans >= -Epsilon;

				if (balans > Epsilon)
					X11 = X11 + balans / PDGznr1;

				X01 = X11;
			}

			NewMOC = 0.0;
			if (ObsMOC <= PANS_OPS_DataBase.dpObsClr.Value)
			{
				if ((X02 > X0))
				{
					X02 = X0;
					Y = dr * SinAlpha;
				}
				else
					Y = System.Math.Sqrt(dr * dr - 2.0 * dr * CosAlpha / Cos30 * X02 + X02 * X02 / (Cos30 * Cos30));

				balans = dTNA - X02 * PDGznr - (Y - dr) * PDGzr;

				Res2IsSolution = balans >= -Epsilon;
				if ((balans > Epsilon))
					X02 = X02 + balans / PDGznr;

				NewMOC = (LAr12 + X02 + Y) * PANS_OPS_DataBase.dpMOC.Value;
				if (NewMOC > MOCLimit)
					NewMOC = MOCLimit;
			}

			if ((ObsMOC > PANS_OPS_DataBase.dpObsClr.Value) || (NewMOC > PANS_OPS_DataBase.dpObsClr.Value))
			{
				if (X12 > X0)
				{
					X12 = X0;
					Y = dr * SinAlpha;
				}
				else
					Y = Math.Sqrt(dr * dr - 2.0 * dr * CosAlpha / Cos30 * X12 + X12 * X12 / (Cos30 * Cos30));

				NewMOC = PANS_OPS_DataBase.dpMOC.Value * (LAr12 + X12 + Y);
				if (NewMOC > MOCLimit)
					NewMOC = MOCLimit;


				balans = dTNA - ObsMOC + NewMOC - X02 * PDGznr - (Y - dr) * PDGzr;
				//balans = (LAr12 + X12) * (PDGznr1 + PANS_OPS_DataBase.dpMOC.Value) + PANS_OPS_DataBase.dpH_abv_DER.Value + Y * (PDGzr1 + PANS_OPS_DataBase.dpMOC.Value);
				//balans = dTNA + dr * PDGzr + NewMOC - X12 * PDGznr1 - Y * PDGzr1 - ObsMOC;

				Res2IsSolution = balans >= -Epsilon;

				if (balans > Epsilon)
					X12 +=  balans / PDGznr1;

				X02 = X12;
			}

			if (Res1IsSolution && Res2IsSolution)
			{
				if (X01 >= 0)
				{
					calcZeroDTNAHReturn.First = X01;
					calcZeroDTNAHReturn.Solutions = 1;
				}
				calcZeroDTNAHReturn.Second_Renamed = X02;
				calcZeroDTNAHReturn.Solutions = calcZeroDTNAHReturn.Solutions | 2;
			}
			else if (Res1IsSolution)
			{
				calcZeroDTNAHReturn.Second_Renamed = X01;
				calcZeroDTNAHReturn.Solutions = 2;
			}
			else if (Res2IsSolution)
			{
				calcZeroDTNAHReturn.Second_Renamed = X02;
				calcZeroDTNAHReturn.Solutions = 2;
			}
			else
			{
				calcZeroDTNAHReturn.Second_Renamed = 0.0;
				calcZeroDTNAHReturn.Solutions = 0;
			}

			return calcZeroDTNAHReturn;
		}

		public static int SideFrom2Angle(double Angle0, double Angle1)
		{
			double dAngle = Functions.SubtractAngles(Angle0, Angle1);

			if (180.0 - dAngle < GlobalVars.degEps || dAngle < GlobalVars.degEps)
				return 0;

			dAngle = NativeMethods.Modulus(Angle1 - Angle0, 360.0);

			if (dAngle < 180.0)
				return 1;

			return -1;
		}

		public static void CreateWindSpiral(IPoint pPtPrj, double aztNom, double AztSt, ref double AztEnd, double r0, double coef, int TurnDir, IPointCollection pPointCollection)
		{
			if (Functions.SubtractAngles(aztNom, AztEnd) < GlobalVars.degEps)
				AztEnd = aztNom;

			double dphi0 = NativeMethods.Modulus((AztSt - aztNom) * TurnDir, 360.0);

			if (dphi0 < 0.001)
				dphi0 = 0.0;
			else
				dphi0 = SpiralTouchAngle(r0, coef, aztNom, AztSt, TurnDir);

			double dphi = SpiralTouchAngle(r0, coef, aztNom, AztEnd, TurnDir);
			double TurnAng = dphi - dphi0;
			double azt0 = NativeMethods.Modulus(aztNom + (dphi0 - 90.0) * TurnDir, 360.0);

			if (TurnAng < 0.0)
				return;

			int n = System.Convert.ToInt32(TurnAng);

			if (n < 1)
				n = 1;
			else if (n < 5)
				n = 5;
			else if (n < 10)
				n = 10;

			double dAlpha = TurnAng / n;

			IPoint ptCnt = Functions.PointAlongPlane(pPtPrj, aztNom + 90.0 * TurnDir, r0);
			IPoint ptCur = new ESRI.ArcGIS.Geometry.Point();

			for (int i = 0; i <= n; i++)
			{
				double R = r0 + (dAlpha * coef * i) + dphi0 * coef;
				ptCur = Functions.PointAlongPlane(ptCnt, azt0 + (i * dAlpha) * TurnDir, R);
				pPointCollection.AddPoint(ptCur);
			}

			// Dim pCurve As ICurve
			// Dim pPolyLine As IPointCollection
			// Set pPolyLine = New Polyline
			// Set pPolyLine = pPointCollection
			// Set pCurve = pPolyLine
		}

		internal static string Degree2String(double X, Degree2StringMode Mode)
		{
			string sSign = "", sResult = "", sTmp;
			double xDeg, xMin, xIMin, xSec;
			bool lSign = false;

			if (Mode == Degree2StringMode.DMSLat)
			{
				lSign = Math.Sign(X) < 0;
				if (lSign)
					X = -X;

				xDeg = System.Math.Floor(X);
				xMin = (X - xDeg) * 60.0;
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

				sTmp = xSec.ToString("00.00");
				sResult = sResult + sTmp + @"""";

				return sResult + (lSign ? "S" : "N");
			}

			if (Mode >= Degree2StringMode.DMSLon)
			{
				X = NativeMethods.Modulus(X);
				lSign = X > 180.0;
				if (lSign) X = 360.0 - X;

				xDeg = System.Math.Floor(X);
				xMin = (X - xDeg) * 60.0;
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

				sTmp = xSec.ToString("00.00");
				sResult = sResult + sTmp + @"""";

				return sResult + (lSign ? "W" : "E");
			}

			if (System.Math.Sign(X) < 0) sSign = "-";
			X = NativeMethods.Modulus(System.Math.Abs(X));

			switch (Mode)
			{
				case Degree2StringMode.DD:
					return sSign + X.ToString("#0.00##") + "°";
				case Degree2StringMode.DM:
					if (System.Math.Sign(X) < 0) sSign = "-";
					X = NativeMethods.Modulus(System.Math.Abs(X));

					xDeg = System.Math.Floor(X);
					xMin = (X - xDeg) * 60.0;
					if (xMin >= 60)
					{
						X++;
						xMin = 0;
					}

					sResult = sSign + xDeg.ToString() + "°";

					sTmp = xMin.ToString("00.00##");
					return sResult + sTmp + "'";
				case Degree2StringMode.DMS:
					if (System.Math.Sign(X) < 0) sSign = "-";
					X = NativeMethods.Modulus(System.Math.Abs(X));

					xDeg = System.Math.Floor(X);
					xMin = (X - xDeg) * 60.0;
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

		public static double dPDGMax(ObstacleData[] PtInList, double PDG, out int idPDGMax)
		{
			idPDGMax = -1;
			if (PDG == PANS_OPS_DataBase.dpPDG_Nom.Value)
				return 0;

			double result = (PANS_OPS_DataBase.dpNGui_Ar1.Value - PANS_OPS_DataBase.dpOIS_abv_DER.Value) / PDG;

			int n = PtInList.Length;

			for (int i = 0; i < n; i++)
			{
				if (!PtInList[i].Ignored)
				{
					double rH = PtInList[i].Height + PtInList[i].MOC - PANS_OPS_DataBase.dpOIS_abv_DER.Value;
					PtInList[i].NomPDGDist = (rH - PANS_OPS_DataBase.dpPDG_Nom.Value * PtInList[i].Dist) / (PDG - PANS_OPS_DataBase.dpPDG_Nom.Value);

					if (result < PtInList[i].NomPDGDist)
					{
						result = PtInList[i].NomPDGDist;
						idPDGMax = i;
					}
				}
			}

			return result;
		}

		public static IPolygon CreatePrjCircle(IPoint pPoint1, double Radius, int n = 360)
		{
			IPointCollection result = new Polygon();
			IPoint pPtCurr = new ESRI.ArcGIS.Geometry.Point();
			double dA = 360.0 * GlobalVars.DegToRadValue / n;

			for (int i = 0; i < n; i++)
			{
				double iInRad = i * dA;
				pPtCurr.X = pPoint1.X + Radius * System.Math.Cos(iInRad);
				pPtCurr.Y = pPoint1.Y + Radius * System.Math.Sin(iInRad);
				result.AddPoint(pPtCurr);
			}

			ITopologicalOperator2 pTopo = (ITopologicalOperator2)result;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();
			return (IPolygon)result;
		}

		public static IPointCollection CreateArcPrj(IPoint ptCnt, IPoint ptFrom, IPoint ptTo, int ClWise)
		{
			double dX = ptFrom.X - ptCnt.X;
			double dY = ptFrom.Y - ptCnt.Y;
			double R = System.Math.Sqrt(dX * dX + dY * dY);

			double AztFrom = GlobalVars.RadToDegValue * NativeMethods.ATan2(dY, dX);
			AztFrom = NativeMethods.Modulus(AztFrom, 360.0);

			double AztTo = GlobalVars.RadToDegValue * NativeMethods.ATan2(ptTo.Y - ptCnt.Y, ptTo.X - ptCnt.X);
			AztTo = NativeMethods.Modulus(AztTo, 360.0);

			double daz = NativeMethods.Modulus((AztTo - AztFrom) * ClWise, 360.0);
			int i = System.Convert.ToInt32(System.Math.Ceiling(daz));

			if (i < 1) i = 1;
			else if (i < 5) i = 5;
			else if (i < 10) i = 10;

			double AngStep = daz / i;

			IPointCollection result = new Polygon();
			IPoint ptCurr = new ESRI.ArcGIS.Geometry.Point();

			result.AddPoint(ptFrom);
			for (int j = 1; j < i; j++)
			{
				double iInRad = GlobalVars.DegToRadValue * (AztFrom + j * AngStep * ClWise);
				ptCurr.X = ptCnt.X + R * System.Math.Cos(iInRad);
				ptCurr.Y = ptCnt.Y + R * System.Math.Sin(iInRad);
				result.AddPoint(ptCurr);
			}

			result.AddPoint(ptTo);
			return result;
		}

		public static double MaxObstacleDist(ObstacleData[] OutList, IPoint ptCenter)
		{
			double MaxDist = 0.0;
			int n = OutList.Length;

			for (int i = 0; i < n; i++)
			{
				double fDist = Functions.ReturnDistanceInMeters(OutList[i].pPtPrj, ptCenter);

				if (fDist > MaxDist)
					MaxDist = fDist;
			}
			return MaxDist;
		}

		public static double Point2LineDistancePrj(IPoint PtTest, IPoint PtLine, double dirAngle)
		{
			dirAngle = GlobalVars.DegToRadValue * dirAngle;
			double CosA = System.Math.Cos(dirAngle);
			double SinA = System.Math.Sin(dirAngle);

			double dX = PtTest.X - PtLine.X;
			double dY = PtTest.Y - PtLine.Y;
			return System.Math.Abs(dY * CosA - dX * SinA);
		}

		public static double Point2LineDistancePrjSgn(IPoint PtTest, IPoint PtLine, double dirAngle)
		{
			dirAngle = GlobalVars.DegToRadValue * dirAngle;
			double CosA = System.Math.Cos(dirAngle);
			double SinA = System.Math.Sin(dirAngle);

			double dX = PtTest.X - PtLine.X;
			double dY = PtTest.Y - PtLine.Y;
			return dY * CosA - dX * SinA;
		}

		public static double ReturnAngleInDegrees(IPoint ptFrom, IPoint ptTo)
		{
			double fdX = ptTo.X - ptFrom.X;
			double fdY = ptTo.Y - ptFrom.Y;
			return NativeMethods.Modulus(RadToDeg(NativeMethods.ATan2(fdY, fdX)), 360.0);
		}

		public static double ReturnDistanceInMeters(IPoint ptFrom, IPoint ptTo)
		{
			double fdX = ptTo.X - ptFrom.X;
			double fdY = ptTo.Y - ptFrom.Y;
			return System.Math.Sqrt(fdX * fdX + fdY * fdY);
		}

		public static IPoint PointAlongPlane(IPoint ptFrom, double dirAngle, double Dist)
		{
			dirAngle = GlobalVars.DegToRadValue * dirAngle;
			double CosA = System.Math.Cos(dirAngle);
			double SinA = System.Math.Sin(dirAngle);

			IPoint result = new ESRI.ArcGIS.Geometry.Point();
			result.X = ptFrom.X + Dist * CosA;
			result.Y = ptFrom.Y + Dist * SinA;

			return result;
		}

		public static void PrjToLocal(IPoint Center, double dirInDegree, IPoint pPt, out double ResX, out double ResY)
		{
			double dirInRadian = GlobalVars.DegToRadValue * dirInDegree;
			double SinA = System.Math.Sin(dirInRadian);
			double CosA = System.Math.Cos(dirInRadian);
			double dX = pPt.X - Center.X;
			double dY = pPt.Y - Center.Y;

			ResX = dX * CosA + dY * SinA;
			ResY = -dX * SinA + dY * CosA;
		}

		public static void PrjToLocal(IPoint Center, double dirInDegree, double X, double Y, out double ResX, out double ResY)
		{
			double dirInRadian = GlobalVars.DegToRadValue * dirInDegree;
			double SinA = System.Math.Sin(dirInRadian);
			double CosA = System.Math.Cos(dirInRadian);
			double dX = X - Center.X;
			double dY = Y - Center.Y;

			ResX = dX * CosA + dY * SinA;
			ResY = -dX * SinA + dY * CosA;
		}

		public static IPoint LocalToPrj(IPoint center, double dirInDegree, double x, double y)
		{
			double dirInRadian = GlobalVars.DegToRadValue * dirInDegree;
			double SinA = Math.Sin(dirInRadian);
			double CosA = Math.Cos(dirInRadian);
			double Xnew = center.X + x * CosA - y * SinA;
			double Ynew = center.Y + x * SinA + y * CosA;
			IPoint result = (IPoint)new ESRI.ArcGIS.Geometry.Point();
			result.PutCoords(Xnew, Ynew);
			return result;
		}

		public static double SubtractAngles(double X, double Y)
		{
			X = NativeMethods.Modulus(X);
			Y = NativeMethods.Modulus(Y);
			double result = NativeMethods.Modulus(X - Y);
			if (result > 180.0)
				result = 360.0 - result;

			return result;
		}

		//double theta = rwy.pPtPrj[eRWY.PtStart].M * GlobalVars.DegToRadValue;
		//double cosTh = Math.Cos(theta);
		//double sinTh = Math.Sin(theta);

		//Проекционные координаты новой точки определяются следующими выражениям:
		//double X = D * cosTh;		// (25)
		//double Y = D * sinTh;		// (26)

		//расстояния D и угла θ.
		//double dXdD = cosTh;		//(27)
		//double dYdD = sinTh;		//(29)	

		//double dXdTh = -D * sinTh;	//(28)
		//double dYdTh = D * cosTh;	//(30)

		//Среднеквадратичное отклонение по проекционным координатам определяется следующими выражениями:

		//double sigX = Math.Sqrt(dXdD* dXdD * rwy.TODAAccuracy * rwy.TODAAccuracy + dXdTh* dXdTh * bearingAccurasy* bearingAccurasy);
		//double sigX_1 = Math.Sqrt(cosTh* cosTh * rwy.TODAAccuracy * rwy.TODAAccuracy + Y* Y * bearingAccurasy * bearingAccurasy);			//(31)

		//double sigY = Math.Sqrt(dYdD* dYdD * rwy.TODAAccuracy * rwy.TODAAccuracy + dYdTh* dYdTh * bearingAccurasy * bearingAccurasy);
		//double sigY_1 = Math.Sqrt(sinTh* sinTh * rwy.TODAAccuracy * rwy.TODAAccuracy + X* X * bearingAccurasy * bearingAccurasy);				//(32)

		//Общая ошибка определяется выражением:
		//double sigP = Math.Sqrt(sigX*sigX + sigY * sigY);

		public static double CalDERcHorisontalAccuracy(RWYType rwy)
		{
			double bearingAccurasy = GlobalVars.settings.AnglePrecision * GlobalVars.DegToRadValue;
			double D = rwy.TODA;

			double sigP = Math.Sqrt(rwy.TODAAccuracy * rwy.TODAAccuracy + D * D * bearingAccurasy * bearingAccurasy);   //(33)
			return sigP;
		}

		public static double CalcHorisontalAccuracy(ESRI.ArcGIS.Geometry.IPoint ptFix, NavaidType GuidanceNav, NavaidType IntersectNav)
		{
			if (GuidanceNav.TypeCode == eNavaidType.DME || IntersectNav.Identifier == Guid.Empty) return 0;
			if (GuidanceNav.Identifier == Guid.Empty) return 0;
			if (IntersectNav.TypeCode == eNavaidType.NONE) return 0;
			if (IntersectNav.ValCnt == -2) return 0;

			const double distEps = 0.0001;
			double sqrt1_2 = 0.5 * Math.Sqrt(2.0);

			double sigL2;

			double GuidDir = ReturnAngleInDegrees(GuidanceNav.pPtPrj, ptFix);
			double dNavNav, LNavNav = ReturnDistanceInMeters(GuidanceNav.pPtPrj, IntersectNav.pPtPrj);

			if (LNavNav < distEps * distEps)
			{
				sigL2 = 0.5 * (GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy + IntersectNav.HorAccuracy * IntersectNav.HorAccuracy);
				dNavNav = GuidDir;
			}
			else
			{
				dNavNav = ReturnAngleInDegrees(GuidanceNav.pPtPrj, IntersectNav.pPtPrj);

				double dX = IntersectNav.pPtPrj.X - GuidanceNav.pPtPrj.X;
				double dY = IntersectNav.pPtPrj.Y - GuidanceNav.pPtPrj.Y;

				double sigX = 0.5 * dX * dX / (LNavNav * LNavNav) * (GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy + IntersectNav.HorAccuracy * IntersectNav.HorAccuracy);
				double sigY = 0.5 * dY * dY / (LNavNav * LNavNav) * (GuidanceNav.HorAccuracy * GuidanceNav.HorAccuracy + IntersectNav.HorAccuracy * IntersectNav.HorAccuracy);

				sigL2 = sigX + sigY;
			}

			double sigT2 = GlobalVars.settings.AnglePrecision * GlobalVars.DegToRadValue;
			sigT2 = sigT2 * sigT2;

			double ted1 = SubtractAngles(dNavNav, GuidDir) * GlobalVars.DegToRadValue;
			double sinT1 = Math.Sin(ted1);
			double cosT1 = Math.Cos(ted1);
			double dY3dX3 = Math.Tan(ted1);

			double dX3dT1, dY3dT1;
			double sigY3, sigX3;

			if (IntersectNav.TypeCode == eNavaidType.DME)
			{
				double sigD2 = DeConvertDistance(GlobalVars.settings.DistancePrecision);
				sigD2 = sigD2 * sigD2;

				double GuidDist = ReturnDistanceInMeters(IntersectNav.pPtPrj, ptFix);
				double sqRoot = Math.Sqrt(GuidDist * GuidDist - LNavNav * LNavNav * sinT1 * sinT1);
				double recip = 1.0 / sqRoot;

				double dX3dL = cosT1 * cosT1 + LNavNav * cosT1 * sinT1 * sinT1 * recip;    //(14)
				double dX3dD = GuidDist * cosT1 * recip;                                   //(15)
				dX3dT1 = 2.0 * LNavNav * cosT1 * sinT1 + sinT1 * sqRoot + cosT1 * cosT1 * sinT1 * LNavNav * LNavNav * recip;    //(16)

				double sigX3_2 = dX3dL * dX3dL * sigL2 + dX3dD * dX3dD * sigD2 + dX3dT1 * dX3dT1 * sigT2;
				sigX3 = Math.Sqrt(sigX3_2);                                         //(17)

				double X3 = LNavNav * cosT1 * cosT1 + cosT1 * sqRoot;                      //(13)
				dY3dT1 = X3 / (cosT1 * cosT1);

				sigY3 = Math.Sqrt(dY3dX3 * dY3dX3 * sigX3_2 + dY3dT1 * dY3dT1 * sigT2);
			}
			else
			{
				double IntersectDir = ReturnAngleInDegrees(IntersectNav.pPtPrj, ptFix);
				double ted2 = SubtractAngles(dNavNav, IntersectDir) * GlobalVars.DegToRadValue;

				double dX3dY3 = 1.0 / dY3dX3;                                              //(7)
				double ctT1T2 = dX3dY3 + 1.0 / Math.Tan(ted2);

				double dY3dL = 1.0 / ctT1T2;
				double Y3 = LNavNav * dY3dL;
				
				dX3dT1 = -Y3 / (sinT1 * sinT1);                                       //(8)

				double fTmp = sinT1 * ctT1T2;
				dY3dT1 = -LNavNav / (fTmp * fTmp);

				fTmp = Math.Sin(ted2) * ctT1T2;
				double dY3dT2 = -LNavNav / (fTmp * fTmp);

				double sigY3_2 = dY3dL * dY3dL * sigL2 + dY3dT1 * dY3dT1 * sigT2 + dY3dT2 * dY3dT2 * sigT2;
				sigY3 = Math.Sqrt(sigY3_2);
				sigX3 = Math.Sqrt(dX3dY3 * dX3dY3 * sigY3_2 + dX3dT1 * dX3dT1 * sigT2);
			}

			double result = Math.Sqrt(sigX3 * sigX3 + sigY3 * sigY3);
			return result;
		}

		public static void SaveDerAccurasyInfo(ReportFile reportFile, RWYType rwy, double horAccuracy)
		{
			//reportFile.Param("Fix Role", "DER");
			reportFile.Param("Calculated horizontal accuracy at DER", horAccuracy.ToString("0.00"), "meters");
			reportFile.WriteMessage();
		}

		public static void SaveFixAccurasyInfo(ReportFile reportFile, IPoint ptFix, string FixRole, NavaidType GuidanceNav, NavaidType IntersectNav, bool isFinal = false)
		{
			//Fix Role				{ FAF, If... }
			//Calculated horizontal accuracy at FIX - in meters

			//Guidance Navaid – Call Sign And Type (RIA VOR)
			//Guidance Navaid Horizontal accuracy - in meters
			//Distance From Guidance Navaid to FIX – in user defined units

			//Intersecting Navaid – Call Sign And Type (RIA VOR)
			//Intersecting Navaid Horizontal accuracy - in meters
			//Distance From Intersecting Navaid to FIX – in user defined units

			if (GuidanceNav.Identifier == Guid.Empty) return;
			if (IntersectNav.TypeCode == eNavaidType.NONE) return;

			if (GuidanceNav.TypeCode == eNavaidType.DME) return;
			if (IntersectNav.IntersectionType == eIntersectionType.OnNavaid) return;

			double HorAccuracy = CalcHorisontalAccuracy(ptFix, GuidanceNav, IntersectNav);

			reportFile.Param("Fix Role", FixRole);
			reportFile.Param("Calculated horizontal accuracy at FIX", HorAccuracy.ToString("0.00"), "meters");
			reportFile.WriteMessage();

			double distance = ReturnDistanceInMeters(ptFix, GuidanceNav.pPtPrj);

			if (GuidanceNav.TypeCode == eNavaidType.NONE)
			{
				reportFile.Param("Distance From DER", ConvertDistance(distance).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
				reportFile.WriteMessage();
			}
			else
			{
				reportFile.Param("Guidance Navaid", GuidanceNav.ToString() + "/" + GuidanceNav.TypeCode.ToString());
				reportFile.Param("Guidance Navaid Horizontal accuracy", GuidanceNav.HorAccuracy.ToString("0.00"), "meters");
				reportFile.Param("Distance From Guidance Navaid to FIX", ConvertDistance(distance).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
				reportFile.WriteMessage();
			}


			reportFile.Param("Intersecting Navaid", IntersectNav.ToString() + "/" + IntersectNav.TypeCode.ToString());
			reportFile.Param("Intersecting Navaid Horizontal accuracy", IntersectNav.HorAccuracy.ToString("0.00"), "meters");

			distance = ReturnDistanceInMeters(ptFix, IntersectNav.pPtPrj);
			reportFile.Param("Distance From Intersecting Navaid to FIX", ConvertDistance(distance).ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);

			if (!isFinal)
			{
				reportFile.WriteMessage("=================================================");
				reportFile.WriteMessage();
			}
		}

		public static void CreateDeparturePolygon(ref IPointCollection pPolygon, double R, int ArrayI, RWYType RWY,
			double DerShift, double DepDir, double CLDir, double PDG, eNavaidType NavType = eNavaidType.NONE, IPoint ptNavPrj = null)
		{
			if (pPolygon.PointCount > 0)
				pPolygon.RemovePoints(0, pPolygon.PointCount);

			IPoint pt0, pt1, pt2, pt3, pt4, pt5;
			double fWd, dpAr1H;

			fWd = 0.5 * PANS_OPS_DataBase.dpNGui_Ar1_Wd.Value;
			//fWd = 0.5 * PANS_OPS_DataBase.dpGui_Ar1_Wd.Value;

			if (NavType <= eNavaidType.NONE)
				dpAr1H = PANS_OPS_DataBase.dpNGui_Ar1.Value - PANS_OPS_DataBase.dpOIS_abv_DER.Value;
			else
				dpAr1H = PANS_OPS_DataBase.dpGui_Ar1.Value - PANS_OPS_DataBase.dpOIS_abv_DER.Value;

			if (DerShift >= 0)
			{
				pt0 = Functions.PointAlongPlane(RWY.pPtPrj[eRWY.PtDER], CLDir - 90.0, fWd + DerShift);
				pt5 = Functions.PointAlongPlane(RWY.pPtPrj[eRWY.PtDER], CLDir + 90.0, fWd);
			}
			else
			{
				pt0 = Functions.PointAlongPlane(RWY.pPtPrj[eRWY.PtDER], CLDir - 90.0, fWd);
				pt5 = Functions.PointAlongPlane(RWY.pPtPrj[eRWY.PtDER], CLDir + 90.0, fWd - DerShift);
			}

			double fDist;
			if (ArrayI == 1)						// ======================== Area I - 3485 - C/L ===========================
				fDist = dpAr1H / PANS_OPS_DataBase.dpPDG_Nom.Value;
			else    //if (ArrayI == 2)				// ====================== Area I - 115/PDG - C/L =========================
				fDist = dpAr1H / PDG;

			IPoint ptDepDir = Functions.PointAlongPlane(RWY.pPtPrj[eRWY.PtDER], CLDir, fDist);

			// ====================== Reserve - NOM TRACK =========================
			pt1 = new ESRI.ArcGIS.Geometry.Point();
			pt4 = new ESRI.ArcGIS.Geometry.Point();
			IConstructPoint pContructor = (ESRI.ArcGIS.Geometry.IConstructPoint)pt1;

			int fAlphaAdjust = AnglesSideDef(DepDir, CLDir);

			if (fAlphaAdjust < 0)   // Right adjust
			{
				pContructor.ConstructAngleIntersection(ptDepDir, GlobalVars.DegToRadValue * (CLDir - 90.0), pt0, GlobalVars.DegToRadValue * (DepDir - PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value));

				pContructor = (ESRI.ArcGIS.Geometry.IConstructPoint)pt4;
				pContructor.ConstructAngleIntersection(ptDepDir, GlobalVars.DegToRadValue * (CLDir + 90.0), pt5, GlobalVars.DegToRadValue * (CLDir + PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value));

				pt2 = Functions.PointAlongPlane(pt1, DepDir - PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value, R);
				pt3 = Functions.PointAlongPlane(pt4, DepDir + PANS_OPS_DataBase.dpAr1_IB_TrAdj.Value, R);
			}
			else                    // Left adjust
			{
				pContructor.ConstructAngleIntersection(ptDepDir, GlobalVars.DegToRadValue * (CLDir - 90.0), pt0, GlobalVars.DegToRadValue * (CLDir - PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value));

				pContructor = (ESRI.ArcGIS.Geometry.IConstructPoint)pt4;
				pContructor.ConstructAngleIntersection(ptDepDir, GlobalVars.DegToRadValue * (CLDir + 90.0), pt5, GlobalVars.DegToRadValue * (DepDir + PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value));

				pt2 = Functions.PointAlongPlane(pt1, DepDir - PANS_OPS_DataBase.dpAr1_IB_TrAdj.Value, R);
				pt3 = Functions.PointAlongPlane(pt4, DepDir + PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value, R);
			}

			pPolygon.AddPoint(pt0);
			pPolygon.AddPoint(pt1);
			pPolygon.AddPoint(pt2);
			pPolygon.AddPoint(pt3);
			pPolygon.AddPoint(pt4);
			pPolygon.AddPoint(pt5);
		}

		public static int CalcPDGToTop(ObstacleContainer ObsList, IPointCollection pPolygon, int ArrayI, RWYType RWY,
										double DerShift, double DepDir, double CLDir, double MOCLimit,
										eNavaidType NavType = eNavaidType.NONE, IPoint ptNavPrj = null)
		{
			double Toler = 0.0, InitWidth = 0.0;

			if (NavType > eNavaidType.NONE)
			{
				if (NavType == eNavaidType.VOR || NavType == eNavaidType.TACAN)
				{
					InitWidth = 0.5 * Navaids_DataBase.VOR.InitWidth;
					Toler = Navaids_DataBase.VOR.SplayAngle;
				}
				else if (NavType == eNavaidType.NDB)
				{
					InitWidth = 0.5 * Navaids_DataBase.NDB.InitWidth;
					Toler = Navaids_DataBase.NDB.SplayAngle;
				}
				else
					return -1;
			}

			double TanToler = System.Math.Tan(GlobalVars.DegToRadValue * Toler);

			int sA = AnglesSideDef(DepDir, CLDir);
			if (sA == 2)
				sA = 0;

			// fAlphaAdjust = DepDir - CLDir
			// If Functions.SubtractAngles(DepDir, CLDir) > degEps Then
			//     sA = Sgn(fAlphaAdjust)
			// Else
			//     sA = 0
			// End If

			double A1 = 0.0, A2 = 0.0;
			IPoint pt1 = null;

			if (sA > 0)
			{
				pt1 = pPolygon.get_Point(0);
				A1 = GlobalVars.DegToRadValue * (CLDir - PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value);
				A2 = GlobalVars.DegToRadValue * (DepDir - PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value);
			}
			else if (sA < 0)
			{
				pt1 = pPolygon.get_Point(5);
				A1 = GlobalVars.DegToRadValue * (CLDir + PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value);
				A2 = GlobalVars.DegToRadValue * (DepDir + PANS_OPS_DataBase.dpAr1_OB_TrAdj.Value);
			}

			IPoint ptTmp = new ESRI.ArcGIS.Geometry.Point();
			double PDG = PANS_OPS_DataBase.dpPDG_Nom.Value;

			int result = -1, n = ObsList.Parts.Length;
			IPoint ptRWYEnd = RWY.pPtPrj[eRWY.PtDER];

			IPolyline pLine = (ESRI.ArcGIS.Geometry.IPolyline)(new Polyline());

			for (int i = 0; i < n; i++)
			{
				IPoint ptObsCurr = ObsList.Parts[i].pPtPrj;
				double tmpMOC;

				if ((ObsList.Parts[i].Dist > 0.0) && (Functions.SideDef(ptObsCurr, CLDir - 90.0, ptRWYEnd) < 0))
					tmpMOC = 0.0;
				else
					tmpMOC = ObsList.Parts[i].Dist * PANS_OPS_DataBase.dpMOC.Value;

				if (tmpMOC > MOCLimit)
					tmpMOC = MOCLimit;

				//if (ObsList.Obstacles[ObsList.Parts[i].Owner].UnicalName == "8026")
				//{
				//    string TypeName = ObsList.Obstacles[ObsList.Parts[i].Owner].TypeName;
				//}


				if (NavType > eNavaidType.NONE && !ObsList.Parts[i].Prima)
				{
					double dNav = Functions.Point2LineDistancePrj(ptObsCurr, ptNavPrj, DepDir - 90.0) + ObsList.Obstacles[ObsList.Parts[i].Owner].HorAccuracy;
					double dCL = Functions.Point2LineDistancePrj(ptObsCurr, ptNavPrj, DepDir) - ObsList.Obstacles[ObsList.Parts[i].Owner].HorAccuracy;
					if (dCL < 0.0)
						dCL = 0.0;

					double NavArea = InitWidth + TanToler * dNav;

					ObsList.Parts[i].fTmp = 2.0 * (NavArea - dCL) / NavArea;
					if (ObsList.Parts[i].fTmp > 1.0)
						ObsList.Parts[i].fTmp = 1.0;
					//==============================================================

					//int Side0 = Functions.SideDef(ptNavPrj, DepDir, ptObsCurr);
					//double dCL = Functions.Point2LineDistancePrj(ptObsCurr, ptNavPrj, DepDir);

					//pLine.FromPoint = Functions.PointAlongPlane(ptObsCurr, DepDir + 90.0 * Side0, dCL);
					//pLine.ToPoint = Functions.PointAlongPlane(ptObsCurr, DepDir - 90.0 * Side0, GlobalVars.RModel + GlobalVars.RModel);

					//IPointCollection pLine1 = (IPointCollection)PolyCut.ClipByPoly(pLine, TurnAreaSecPolygon);

					//pLine.FromPoint = pLine1.get_Point(0);
					//pLine.ToPoint = pLine1.get_Point(pLine1.PointCount - 1);

					//int Side1 = Functions.SideDef(pLine.FromPoint, DepDir, pLine.ToPoint);
					//if (Side0 * Side1 < 0)
					//    pLine.ReverseOrientation();

					//ObsList.Parts[i].fTmp = (Functions.ReturnDistanceInMeters(ptObsCurr, pLine.ToPoint) + ObsList.Obstacles[ObsList.Parts[i].Owner].HorAccuracy) / pLine.Length;
					//if (ObsList.Parts[i].fTmp > 1.0)
					//{
					//    ObsList.Parts[i].fTmp = 1.0;
					//    ObsList.Parts[i].Prima = true;
					//}
					//==============================================================

					ObsList.Parts[i].MOC = tmpMOC * ObsList.Parts[i].fTmp;
				}
				else
				{
					ObsList.Parts[i].fTmp = 1.0;
					ObsList.Parts[i].MOC = tmpMOC;
				}

				double rH = ObsList.Parts[i].Height + ObsList.Parts[i].MOC - PANS_OPS_DataBase.dpOIS_abv_DER.Value;

				ObsList.Parts[i].PDGToTop = (rH - ObsList.Parts[i].MOC) / ObsList.Parts[i].Dist;
				ObsList.Parts[i].PDG = rH / ObsList.Parts[i].Dist;
				ObsList.Parts[i].Ignored = rH + PANS_OPS_DataBase.dpOIS_abv_DER.Value <= PANS_OPS_DataBase.dpPDG_60.Value;

				if (ObsList.Parts[i].PDG > PDG && !ObsList.Parts[i].Ignored)
				{
					PDG = ObsList.Parts[i].PDG;
					result = i;
				}

				if (sA == 0)
					ObsList.Parts[i].PDGAvoid = 9999.0;
				else
				{
					IConstructPoint pContructor = (ESRI.ArcGIS.Geometry.IConstructPoint)ptTmp;
					pContructor.ConstructAngleIntersection(pt1, A1, ptObsCurr, A2);

					if (Functions.SideDef(ptRWYEnd, CLDir - 90.0, ptTmp) > 0)
						ObsList.Parts[i].PDGAvoid = 9999.0;
					else
					{
						double d1 = Functions.Point2LineDistancePrj(ptTmp, ptRWYEnd, CLDir - 90.0) - ObsList.Obstacles[ObsList.Parts[i].Owner].HorAccuracy;

						if (ArrayI == 1)
						{
							if (d1 <= (PANS_OPS_DataBase.dpNGui_Ar1.Value - PANS_OPS_DataBase.dpH_abv_DER.Value) / PANS_OPS_DataBase.dpPDG_Nom.Value)
								ObsList.Parts[i].PDGAvoid = 9999.0;
							else
								ObsList.Parts[i].PDGAvoid = PANS_OPS_DataBase.dpPDG_Nom.Value;
						}
						else
						{
							if (d1 <= 0.0)
								ObsList.Parts[i].PDGAvoid = 9999.0;
							else
							{
								double fTmp = (PANS_OPS_DataBase.dpNGui_Ar1.Value - PANS_OPS_DataBase.dpH_abv_DER.Value) / d1;

								if (fTmp <= PANS_OPS_DataBase.dpPDG_Nom.Value)
									ObsList.Parts[i].PDGAvoid = PANS_OPS_DataBase.dpPDG_Nom.Value;
								else
									ObsList.Parts[i].PDGAvoid = System.Math.Round(fTmp + 0.0004999, 3);
							}
						}
					}
				}
			}

			return result;
		}

		public static void RefreshCommandBar(ICommandItem[] mBar, int iFlag = 0xFFFF)
		{
			return;

			//for (int i = 0, j = 1; i < mBar.Length; i++, j += j)
			//    if ((iFlag & j) != 0 && mBar[i] != null)
			//        mBar[i].Refresh();
		}

		public static void SortIntervals(Interval[] Intervals, bool RightSide = false)
		{
			Interval Tmp;
			int n = Intervals.Length;

			for (int i = 0; i < n - 1; i++)
			{
				for (int j = i + 1; j < n; j++)
				{
					if (RightSide)
					{
						if (Intervals[i].Right > Intervals[j].Right)
						{
							Tmp = Intervals[i];
							Intervals[i] = Intervals[j];
							Intervals[j] = Tmp;
						}
					}
					else
					{
						if (Intervals[i].Left > Intervals[j].Left)
						{
							Tmp = Intervals[i];
							Intervals[i] = Intervals[j];
							Intervals[j] = Tmp;
						}
					}
				}
			}
		}

		public static double CalcLocalPDG(ObstacleData[] ObstList, out int indx)
		{
			indx = -1;
			int n = ObstList.Length;
			double result = PANS_OPS_DataBase.dpPDG_Nom.Value;

			for (int i = 0; i < n; i++)
			{
				ObstList[i].Ignored = ObstList[i].PDG * ObstList[i].Dist + PANS_OPS_DataBase.dpOIS_abv_DER.Value <= PANS_OPS_DataBase.dpPDG_60.Value;

				if (!ObstList[i].Ignored)
				{
					double tmpPDG = Math.Min(ObstList[i].PDGAvoid, ObstList[i].PDG);

					if (result < tmpPDG)
					{
						indx = i;
						result = tmpPDG;
					}
				}
			}

			return result;
		}

		public static double IAS2TAS(double IAS, double h, double dT)
		{
			if ((h >= 288.0 / 0.006496) || (h >= (288.0 + dT) / 0.006496))
			{
				//     h = Int(288.0 / 0.006496)
				return 2.0 * IAS;
			}
			else
				return IAS * 171233.0 * System.Math.Sqrt(288.0 + dT - 0.006496 * h) / (Math.Pow((288.0 - 0.006496 * h), 2.628));
		}

		public static double Bank2Radius(double Bank, double V)
		{
			double Rv = 6.355 * System.Math.Tan(GlobalVars.DegToRadValue * Bank) / (Math.PI * V);

			if (Rv > 0.003)
				Rv = 0.003;

			if (Rv > 0.0)
				return V / (20.0 * Math.PI * Rv);

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
			IPointCollection pTmpPoly = ReArrangePolygon(RemoveAgnails(pPolygon as IPolygon), PtDerL, CLDir, false);
			IPoint pPt = Functions.PointAlongPlane(PtDerL, CLDir + 180.0, 30000.0);

			ESRI.ArcGIS.Geometry.IPointCollection result = new Polyline();
			int N = pTmpPoly.PointCount;

			for (int I = 0; I < N; I++)
			{
				int Side = Functions.SideDef(pPt, CLDir, pTmpPoly.Point[I]);
				if (Side == Turn)
					result.AddPoint(pTmpPoly.get_Point(I));
			}

			if (Turn < 0)
			{
				IPolyline pLine = (ESRI.ArcGIS.Geometry.IPolyline)result;
				pLine.ReverseOrientation();
			}
			return result;
		}

		public static IPointCollection ReArrangePolygon(IPointCollection pPolygon, IPoint ptBase, double BaseDir, bool bFlag = false)
		{
			ITopologicalOperator2 pTopoOper = (ITopologicalOperator2)pPolygon;
			pTopoOper.IsKnownSimple_2 = false;
			pTopoOper.Simplify();

			if (pPolygon.PointCount < 4)
				return pPolygon;

			IPointCollection pPoly = new Polyline();
			pPoly.AddPointCollection(pPolygon);

			IPoint pPt = Functions.PointAlongPlane(ptBase, BaseDir + 180.0, 30000.0);

			int n = pPoly.PointCount - 1;
			pPoly.RemovePoints(n, 1);

			double dl, dm = 0.0;
			int Side = Functions.SideDef(pPt, BaseDir, pPoly.Point[0]);
			int iStart = -1;
			if (Side < 0)
			{
				dm = Functions.Point2LineDistancePrj(pPoly.Point[0], pPt, BaseDir + 90.0);
				iStart = 0;
			}

			int i, j;

			for (i = 1; i < n; i++)
			{
				Side = Functions.SideDef(pPt, BaseDir, pPoly.Point[i]);
				if (Side < 0)
				{
					dl = Functions.Point2LineDistancePrj(pPoly.Point[i], pPt, BaseDir + 90.0);
					if (dl < dm || iStart < 0)
					{
						dm = dl;
						iStart = i;
					}
				}
			}

			if (bFlag)
			{
				if (iStart == 0)
					iStart = n - 1;
				else
					iStart--;
			}

			double dX0 = pPoly.Point[1].X - pPoly.Point[0].X;
			double dY0 = pPoly.Point[1].Y - pPoly.Point[0].Y;

			i = 1;
			while (i < n)
			{
				j = (i + 1) % n;

				double dX1 = pPoly.Point[j].X - pPoly.Point[i].X;
				double dY1 = pPoly.Point[j].Y - pPoly.Point[i].Y;
				dl = Functions.ReturnDistanceInMeters(pPoly.Point[j], pPoly.Point[i]);

				if (dl < 0.001)
				{
					pPoly.RemovePoints(i, 1);
					n--;

					if (i <= iStart)
						iStart--;

					dX1 = dX0;
					dY1 = dY0;
					i--;
				}
				else if (i != iStart)
				{
					if (dY0 != 0.0 && dY1 != 0.0)
					{
						if (System.Math.Abs(System.Math.Abs(dX0 / dY0) - System.Math.Abs(dX1 / dY1)) < 0.00001)
						{
							pPoly.RemovePoints(i, 1);
							n--;

							if (i <= iStart)
								iStart--;

							dX1 = dX0;
							dY1 = dY0;
							i--;
						}
					}
					else if (dX0 != 0.0 && dX1 != 0.0)
					{
						if (System.Math.Abs(System.Math.Abs(dY0 / dX0) - System.Math.Abs(dY1 / dX1)) < 0.00001)
						{
							pPoly.RemovePoints(i, 1);
							n--;

							if (i <= iStart)
								iStart--;

							dX1 = dX0;
							dY1 = dY0;
							i--;
						}
					}
				}
				i++;
				dX0 = dX1;
				dY0 = dY1;
			}

			IPointCollection result = new Polygon();

			n = pPoly.PointCount;
			for (i = n - 1; i >= 0; i--)
			{
				j = (i + iStart) % n;
				result.AddPoint(pPoly.Point[j]);
			}

			// DrawPolygon ReArrangePolygon, 255
			// Set pPoly = New Polyline
			// pPoly.re
			// pPoly.ReverseOrientation
			return result;
		}

		public static double CalcSpiralStartPoint(IPointCollection LinePoint, ref ObstacleData DetObs, double coef, double r0, double DepDir, double AztDir, int TurnDir, bool bDerFlg)
		{
			int Offset = 0;
			if (bDerFlg)
				Offset = 1;

			int N = LinePoint.PointCount - Offset;
			DetObs.CourseAdjust = GlobalVars.NO_VALUE;

			if (N < 2)
				return -100;

			IPoint[] BasePoints = new IPoint[N + 1 + 1];

			int I;
			for (I = 0; I < N; I++)
			{
				BasePoints[I] = LinePoint.get_Point(I + Offset);

				if (I == N - 1)
					BasePoints[I].M = BasePoints[I - 1].M;
				else
					BasePoints[I].M = Functions.ReturnAngleInDegrees(LinePoint.Point[I + Offset], LinePoint.Point[I + Offset + 1]);
			}

			double hTMin = GlobalVars.RModel;
			int iMin = -1;

			double MaxTheta = SpiralTouchAngle(r0, coef, DepDir, DepDir + 90.0 * TurnDir, TurnDir);
			if (MaxTheta > 180.0)
				MaxTheta = 360.0 - MaxTheta;

			IPolyline pLine = (IPolyline)new Polyline();
			IPoint ptCnt = new ESRI.ArcGIS.Geometry.Point();
			IPoint PtTurn = null;

			for (I = 0; I < N - 1; I++)
			{
				int Side = Functions.SideDef(BasePoints[I], (BasePoints[I].M), DetObs.pPtPrj);
				double alpha = DepDir + 90.0 * Side;

				if (System.Math.Abs(System.Math.Sin(GlobalVars.DegToRadValue * (alpha - BasePoints[I].M))) <= GlobalVars.degEps)
					continue;

				double hT, fTmp;

				double dAlpha = Functions.SubtractAngles(alpha, BasePoints[I].M);
				IPoint ptTmp = Functions.PointAlongPlane(BasePoints[I], DepDir - 90.0 * Side, r0);

				double Dist = Functions.Point2LineDistancePrj(DetObs.pPtPrj, ptTmp, BasePoints[I].M);
				int Side1 = Functions.SideDef(ptTmp, BasePoints[I].M, DetObs.pPtPrj);

				double Theta = 0.5 * MaxTheta;
				do
				{
					fTmp = Theta;
					double ASinAlpha = Dist / (r0 + Theta * coef);

					if (System.Math.Abs(ASinAlpha) <= 1.0)
						Theta = dAlpha - GlobalVars.RadToDegValue * (Side1 * TurnDir * ArcSin(ASinAlpha));
					else
					{
						Theta = MaxTheta;
						break;
					}
				}
				while (System.Math.Abs(fTmp - Theta) > GlobalVars.degEps);

				fTmp = System.Math.Sin(GlobalVars.DegToRadValue * Theta) * (r0 + Theta * coef);

				if (Theta > MaxTheta)
				{
					hT = System.Math.Sin(GlobalVars.DegToRadValue * MaxTheta) * (r0 + MaxTheta * coef);
					fTmp = (hT - fTmp);
					Theta = MaxTheta;
				}
				else
				{
					hT = fTmp;
					fTmp = 0.0;
				}

				if (hT >= hTMin)
					continue;

				IPoint ptTmp2 = Functions.PointAlongPlane(DetObs.pPtPrj, DepDir + 180.0, hT + fTmp);

				IConstructPoint pConstructor = (ESRI.ArcGIS.Geometry.IConstructPoint)ptCnt;
				pConstructor.ConstructAngleIntersection(ptTmp2, GlobalVars.DegToRadValue * (DepDir + 90.0), ptTmp, GlobalVars.DegToRadValue * BasePoints[I].M);

				ptTmp = Functions.PointAlongPlane(ptCnt, DepDir - TurnDir * 90.0, r0);

				pLine.FromPoint = BasePoints[I];
				pLine.ToPoint = BasePoints[I + 1];
				IProximityOperator pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pLine;

				if (pProxi.ReturnDistance(ptTmp) >= GlobalVars.distEps)
					continue;

				hTMin = hT;
				iMin = I;

				PtTurn = ptTmp;
				PtTurn.M = Theta;
				PtTurn.Z = DetObs.Dist - hTMin;

				if (PtTurn.Z < 0.0)
					PtTurn.Z = 0.0;
			}

			if (iMin < 0)
				return -100.0;

			DetObs.CourseAdjust = PtTurn.M;
			return PtTurn.Z;
		}

		public static double SpiralTouchAngle(double r0, double coef0, double aztNominal, double aztTouch, int TurnDir)
		{
			double TouchAngle = GlobalVars.DegToRadValue * NativeMethods.Modulus((aztTouch - aztNominal) * TurnDir);
			double coef = GlobalVars.RadToDegValue * coef0;

			double turnAngle = TouchAngle;

			for (int i = 0; i <= 9; i++)
			{
				double d = coef / (r0 + coef * turnAngle);
				double delta = (turnAngle - TouchAngle - System.Math.Atan(d)) / (2.0 - 1.0 / (d * d + 1.0));
				turnAngle = turnAngle - delta;
				if ((System.Math.Abs(delta) < GlobalVars.radEps))
					break;
			}

			double result = GlobalVars.RadToDegValue * turnAngle;
			if (result < 0.0)
				result = NativeMethods.Modulus(result);

			return result;
		}

		public static double ArcSin(double X)
		{
			double absX = System.Math.Abs(X);
			if (absX >= 1.0 && absX <= 1.001)
			{
				if (X > 0)
					return 0.5 * GlobalVars.PI;

				return -0.5 * GlobalVars.PI;
			}

			return System.Math.Asin(X);
		}

		public static double ArcCos(double X)
		{
			double absX = System.Math.Abs(X);
			if (absX >= 1.0 && absX <= 1.001)
				return 0.0;

			return System.Math.Acos(X);             // System.Math.Atan(-X / System.Math.Sqrt(-X * X + 1)) + 0.5 * PI
		}

		public static double Max(double X, double Y)
		{
			if (X > Y)
				return X;
			return Y;
		}

		public static double Min(double X, double Y)
		{
			if (X < Y)
				return X;
			return Y;
		}

		public static double CalcTIAMinTNAH(ObstacleData[] ObsList, double PDG, out double Range, double iniH = 0.0)
		{
			double result, invPDG = 1.0 / PDG;

			if (iniH < PANS_OPS_DataBase.dpNGui_Ar1.Value)
				result = PANS_OPS_DataBase.dpNGui_Ar1.Value;
			else
				result = iniH;

			Range = (result - PANS_OPS_DataBase.dpOIS_abv_DER.Value) * invPDG;

			int n = ObsList.Length;
			if (n <= 0)
				return result;

			for (int i = 0; i < n; i++)
			{
				if (ObsList[i].Dist > Range)
					break;

				double fTmp = ObsList[i].Height + PANS_OPS_DataBase.dpObsClr.Value;

				if (fTmp > result)
				{
					result = fTmp;
					Range = (result - PANS_OPS_DataBase.dpOIS_abv_DER.Value) * invPDG;
				}
			}

			return result;
		}

		public static double FixToTouchSpiral(IPoint ptBase, double coef0, double TurnR, int TurnDir,
												double Theta, IPoint FixPnt, double DepCourse)
		{
			double result = -1000.0;

			double coef = GlobalVars.RadToDegValue * coef0;
			double Theta0 = NativeMethods.Modulus(90.0 * TurnDir + DepCourse + 180.0);
			IPoint PtCntSpiral = Functions.PointAlongPlane(ptBase, DepCourse + 90.0 * TurnDir, TurnR);

			double Dist = Functions.ReturnDistanceInMeters(PtCntSpiral, FixPnt);
			double Theta1 = Functions.ReturnAngleInDegrees(PtCntSpiral, FixPnt);
			double dTheta = NativeMethods.Modulus((Theta1 - Theta0) * TurnDir);

			double R = TurnR + dTheta * coef0;
			if (Dist < R)
				return result;

			double CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, Theta, TurnDir);
			Theta1 = NativeMethods.Modulus(Theta0 + CntTheta * TurnDir);

			double X1 = FixPnt.X - PtCntSpiral.X;
			double Y1 = FixPnt.Y - PtCntSpiral.Y;

			double delta;

			// =================================================================
			for (int i = 0; i <= 20; i++)
			{
				dTheta = NativeMethods.Modulus((Theta1 - Theta0) * TurnDir);
				R = TurnR + dTheta * coef0;

				double SinT = System.Math.Sin(GlobalVars.DegToRadValue * Theta1);
				double CosT = System.Math.Cos(GlobalVars.DegToRadValue * Theta1);

				double f = R * R - (Y1 * R + X1 * coef * TurnDir) * SinT - (X1 * R - Y1 * coef * TurnDir) * CosT;
				double F1 = 2 * R * coef * TurnDir - (Y1 * R + 2 * X1 * coef * TurnDir) * CosT + (X1 * R - 2 * Y1 * coef * TurnDir) * SinT;

				double Theta1New = Theta1 - GlobalVars.RadToDegValue * (f / F1);
				delta = Functions.SubtractAngles(Theta1New, Theta1);

				Theta1 = Theta1New;
				if (delta < 0.0001)
					break;
			}

			dTheta = NativeMethods.Modulus((Theta1 - Theta0) * TurnDir);
			R = TurnR + dTheta * coef0;

			IPoint ptOut = Functions.PointAlongPlane(PtCntSpiral, Theta1, R);
			double fTmp = Functions.ReturnAngleInDegrees(ptOut, FixPnt);
			CntTheta = SpiralTouchAngle(TurnR, coef0, DepCourse, fTmp, TurnDir);
			CntTheta = NativeMethods.Modulus(Theta0 + CntTheta * TurnDir);

			delta = Functions.SubtractAngles(CntTheta, Theta1);
			if (delta < 0.0001)
				return fTmp;

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
			return result;
		}

		public static void CreateNavaidZone(IPoint ptNav, double dirAngle, eNavaidType NavType, double Multiplicity,
											IPointCollection LPolygon, IPointCollection RPolygon, IPointCollection PrimPolygon)
		{
			double dZone;
			double BaseLength;
			double alpha;

			if (NavType == eNavaidType.NDB)
			{
				BaseLength = Navaids_DataBase.NDB.InitWidth * 0.5;
				alpha = Navaids_DataBase.NDB.SplayAngle;
				dZone = Navaids_DataBase.NDB.Range * Multiplicity;
			}
			else if (NavType == eNavaidType.VOR)
			{
				BaseLength = 0.5 * Navaids_DataBase.VOR.InitWidth;
				alpha = Navaids_DataBase.VOR.SplayAngle;
				dZone = Navaids_DataBase.VOR.Range * Multiplicity;
			}
			else
				return;

			double d0 = dZone / System.Math.Cos(GlobalVars.DegToRadValue * alpha);
			double Betta = 0.5 * System.Math.Tan(GlobalVars.DegToRadValue * alpha);
			Betta = System.Math.Atan(Betta);
			Betta = GlobalVars.RadToDegValue * Betta;

			if (LPolygon.PointCount > 0)
				LPolygon.RemovePoints(0, LPolygon.PointCount);

			if (RPolygon.PointCount > 0)
				RPolygon.RemovePoints(0, RPolygon.PointCount);

			if (PrimPolygon.PointCount > 0)
				PrimPolygon.RemovePoints(0, PrimPolygon.PointCount);

			IPoint pt0, pt1, pt2, pt3, pt4, pt5;

			// ==========LeftPolygon
			pt0 = Functions.PointAlongPlane(ptNav, dirAngle + 90.0, BaseLength);
			pt3 = Functions.PointAlongPlane(ptNav, dirAngle + 90.0, 0.5 * BaseLength);
			pt1 = Functions.PointAlongPlane(pt0, dirAngle + alpha, d0);
			pt2 = Functions.PointAlongPlane(pt3, dirAngle + Betta, d0);
			pt4 = Functions.PointAlongPlane(pt3, dirAngle + 180.0 - Betta, d0);
			pt5 = Functions.PointAlongPlane(pt0, dirAngle + 180.0 - alpha, d0);

			LPolygon.AddPoint(pt0);
			LPolygon.AddPoint(pt1);
			LPolygon.AddPoint(pt2);
			LPolygon.AddPoint(pt3);
			LPolygon.AddPoint(pt4);
			LPolygon.AddPoint(pt5);
			// LPolygon.AddPoint Pt0

			PrimPolygon.AddPoint(pt4);
			PrimPolygon.AddPoint(pt3);
			PrimPolygon.AddPoint(pt2);

			// ==========RightPolygon
			pt0 = Functions.PointAlongPlane(ptNav, dirAngle - 90.0, 0.5 * BaseLength);
			pt3 = Functions.PointAlongPlane(ptNav, dirAngle - 90.0, BaseLength);
			pt1 = Functions.PointAlongPlane(pt0, dirAngle - Betta, d0);
			pt2 = Functions.PointAlongPlane(pt3, dirAngle - alpha, d0);
			pt4 = Functions.PointAlongPlane(pt3, dirAngle + 180.0 + alpha, d0);
			pt5 = Functions.PointAlongPlane(pt0, dirAngle + 180.0 + Betta, d0);

			RPolygon.AddPoint(pt0);
			RPolygon.AddPoint(pt1);
			RPolygon.AddPoint(pt2);
			RPolygon.AddPoint(pt3);
			RPolygon.AddPoint(pt4);
			RPolygon.AddPoint(pt5);
			// RPolygon.AddPoint Pt0

			PrimPolygon.AddPoint(pt1);
			PrimPolygon.AddPoint(pt0);
			PrimPolygon.AddPoint(pt5);
			// PrimPolygon.AddPoint PrimPolygon.Point(0)
		}

		public static bool RayPolylineIntersect(IPointCollection pPolyline, IPoint RayPt, double RayDir, ref IPoint InterPt)
		{
			return RayPolylineIntersect(pPolyline as IPolyline, RayPt, RayDir, ref InterPt);
		}

		public static bool RayPolylineIntersect(IPolyline pPolyline, IPoint RayPt, double RayDir, ref IPoint InterPt)
		{
			double dMin = 5000000.0;

			IPolyline pLine = (ESRI.ArcGIS.Geometry.IPolyline)(new Polyline());
			pLine.FromPoint = RayPt;
			pLine.ToPoint = Functions.PointAlongPlane(RayPt, RayDir, dMin);

			ITopologicalOperator pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator)pPolyline;
			IPointCollection pPoints = (ESRI.ArcGIS.Geometry.IPointCollection)pTopo.Intersect(pLine, esriGeometryDimension.esriGeometry0Dimension);

			int n = pPoints.PointCount;
			if (n == 0)
				return false;

			if (n == 1)
			{
				InterPt = pPoints.Point[0];
				return true;
			}

			for (int i = 0; i < n; i++)
			{
				double d = Functions.ReturnDistanceInMeters(RayPt, pPoints.Point[i]);
				if (d < dMin)
				{
					dMin = d;
					InterPt = pPoints.Point[i];
				}
			}

			return true;
		}

		public static void CutPoly(ref IPointCollection poly, IPointCollection CutLine, int Side)
		{
			CutPoly(ref poly, (IPolyline)CutLine, Side);
		}

		public static void CutPoly(ref IPointCollection poly, IPolyline CutLine, int Side)
		{
			IPolygon p = (IPolygon)poly;
			CutPoly(ref p, CutLine, Side);
			poly = (IPointCollection)p;
		}

		public static void CutPoly(ref IPolygon poly, IPolyline CutLine, int Side)
		{
			IPolyline Cutter = CutLine;

			double tmpAzt = Functions.ReturnAngleInDegrees(Cutter.FromPoint, Cutter.ToPoint);
			double Dist = Functions.ReturnDistanceInMeters(Cutter.FromPoint, Cutter.ToPoint);

			ITopologicalOperator2 Topo = (ITopologicalOperator2)Cutter;
			Topo.IsKnownSimple_2 = false;
			Topo.Simplify();

			Topo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)poly;
			Topo.IsKnownSimple_2 = false;
			Topo.Simplify();

			IPointCollection TmpPoly = (IPointCollection)PolyCut.ClipByPoly(Cutter, poly);
			// SRC = 2

			int GIx = 0;

			if (TmpPoly.PointCount != 0)
			{
				IGeometryCollection Geocollect = ((ESRI.ArcGIS.Geometry.IGeometryCollection)(TmpPoly));
				if (Geocollect.GeometryCount > 1)
				{
					double Dist0;

					for (int Ix = 0; Ix <= Geocollect.GeometryCount - 1; Ix++)
					{
						TmpPoly = ((ESRI.ArcGIS.Geometry.IPointCollection)(Geocollect.get_Geometry(Ix)));
						Dist0 = Functions.ReturnDistanceInMeters(TmpPoly.get_Point(0), Cutter.FromPoint);
						double Dist1 = Functions.ReturnDistanceInMeters(TmpPoly.get_Point(1), Cutter.FromPoint);
						if (Dist0 > Dist1)
							Dist0 = Dist1;

						if (Dist > Dist0)
						{
							GIx = Ix;
							Dist = Dist0;
						}
					}

					TmpPoly = ((ESRI.ArcGIS.Geometry.IPointCollection)(Geocollect.Geometry[GIx]));
					Dist = Functions.ReturnDistanceInMeters(TmpPoly.Point[0], Cutter.FromPoint);
					Dist0 = Functions.ReturnDistanceInMeters(TmpPoly.Point[1], Cutter.FromPoint);

					if (Dist < Dist0)
						Dist = Dist0;

					IPoint ptTmp = Functions.PointAlongPlane(Cutter.FromPoint, tmpAzt, Dist + 5.0);
					Cutter.ToPoint = ptTmp;
				}

				IPolygon pLeft;
				IPolygon pRight;
				IPolygon pUnspecified;

				PolyCut.ClipByLine(poly, Cutter, out pLeft, out pRight, out pUnspecified);
				// SRC = 4

				if (Side < 0)
					poly = pRight;
				else
					poly = pLeft;
			}
		}

		public static Polygon RemoveFars(IPointCollection pPolygon, IPoint pPoint)
		{
			return RemoveFars(pPolygon as IPolygon, pPoint);
		}

		public static Polygon RemoveFars(IPolygon pPolygon, IPoint pPoint)
		{
			IClone pClone = (ESRI.ArcGIS.esriSystem.IClone)pPolygon;
			Polygon result = (ESRI.ArcGIS.Geometry.Polygon)pClone.Clone();

			IGeometryCollection Geocollect = (ESRI.ArcGIS.Geometry.IGeometryCollection)result;
			int n = Geocollect.GeometryCount;

			if (n > 1)
			{
				int i;
				double tmpDist, OutDist = 20000000000.0;
				IProximityOperator pProxi = (IProximityOperator)pPoint;
				IGeometryCollection lCollect = (IGeometryCollection)(new Polygon());

				for (i = 0; i < n; i++)
				{
					lCollect.AddGeometry(Geocollect.Geometry[i]);
					tmpDist = pProxi.ReturnDistance((IGeometry)lCollect);
					if (OutDist > tmpDist)
						OutDist = tmpDist;

					lCollect.RemoveGeometries(0, 1);
				}

				i = 0;
				while (i < n)
				{
					lCollect.AddGeometry(Geocollect.Geometry[i]);
					tmpDist = pProxi.ReturnDistance((IGeometry)lCollect);
					if (OutDist < tmpDist)
					{
						Geocollect.RemoveGeometries(i, 1);
						n--;
					}
					else
						i++;

					lCollect.RemoveGeometries(0, 1);
				}
			}

			return result;
		}

		public static Polygon RemoveHoles(IPointCollection pPolygon)
		{
			return RemoveHoles(pPolygon as IPolygon);
		}

		public static Polygon RemoveHoles(IPolygon pPolygon)
		{
			IClone pClone = (IClone)pPolygon;
			Polygon result = (Polygon)pClone.Clone();
			IGeometryCollection NewPolygon = (ESRI.ArcGIS.Geometry.IGeometryCollection)result;

			int i = 0;
			while (i < NewPolygon.GeometryCount)
			{
				IRing pRing = (IRing)NewPolygon.Geometry[i];

				if (!pRing.IsExterior)
					NewPolygon.RemoveGeometries(i, 1);
				else
					i++;
			}
			return result;
		}

		public static Polygon RemoveAgnails(IPointCollection pPolygon)
		{
			return RemoveAgnails(pPolygon as IPolygon);
		}

		public static Polygon RemoveAgnails(IPolygon pPolygon)
		{
			IClone pClone = (IClone)pPolygon;
			IPointCollection pPoly = (IPointCollection)pClone.Clone();

			IPolygon2 pPGone = (IPolygon2)pPoly;
			pPGone.Close();

			int n = pPoly.PointCount - 1;
			if (n <= 3)
				return (Polygon)pPoly;

			pPoly.RemovePoints(n, 1);

			int i = 0;
			while (i < n)
			{
				if (n < 4)
					break;

				int j = (i + 1) % n;
				int k = (i + 2) % n;

				double dX0 = pPoly.get_Point(j).X - pPoly.get_Point(i).X;
				double dY0 = pPoly.get_Point(j).Y - pPoly.get_Point(i).Y;

				double dX1 = pPoly.get_Point(k).X - pPoly.get_Point(j).X;
				double dY1 = pPoly.get_Point(k).Y - pPoly.get_Point(j).Y;

				double dXX = dX1 * dX1;
				double dYY = dY1 * dY1;
				double dl = dXX + dYY;

				if (dl < 0.00001)
				{
					pPoly.RemovePoints(j, 1);
					n--;
					if (i >= n)
						i = n - 1;
				}
				else if (dYY > dXX)
				{
					if (System.Math.Abs(dX0 / dY0 - dX1 / dY1) < 0.00001)
					{
						pPoly.RemovePoints(j, 1);
						n--;
						i = (i - 2) % n;
						if (i < 0)
							i += n;
					}
					else
						i++;
				}
				else
				{
					if (System.Math.Abs(dY0 / dX0 - dY1 / dX1) < 0.00001)
					{
						pPoly.RemovePoints(j, 1);
						n--;
						i = (i - 2) % n;
						if (i < 0)
							i += n;
					}
					else
						i++;
				}
			}

			pPGone = (IPolygon2)pPoly;
			pPGone.Close();

			ITopologicalOperator2 pTopo = (ITopologicalOperator2)pPoly;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			return (Polygon)pPoly;
		}

		public static Interval[] IntervalsDifference(Interval A, Interval B)
		{
			Interval[] res;

			if ((B.Left == B.Right) || (B.Right < A.Left) || (A.Right < B.Left))
			{
				res = new Interval[1];
				res[0] = A;
			}
			else if ((A.Left < B.Left) && (A.Right > B.Right))
			{
				res = new Interval[2];
				res[0].Left = A.Left;
				res[0].Right = B.Left;
				res[1].Left = B.Right;
				res[1].Right = A.Right;
			}
			else if (A.Right > B.Right)
			{
				res = new Interval[1];
				res[0].Left = B.Right;
				res[0].Right = A.Right;
			}
			else if ((A.Left < B.Left))
			{
				res = new Interval[1];
				res[0].Left = A.Left;
				res[0].Right = B.Left;
			}
			else
				res = new Interval[0];

			return res;
		}

		private static double CalcNomPos(IPoint ptDMEprj, double Xs, double Ys, double d0,
										double BaseHeight, double fRefAltitude, double PDG, int AheadBehindSide, int NearSide)
		{
			double hMax = 0.0, dOldPosDME,
			dNomPosDME = d0 + NearSide * Navaids_DataBase.DME.MinimalError;

			int i = 5;
			do
			{
				double dNomPosDer = Xs + AheadBehindSide * System.Math.Sqrt(dNomPosDME * dNomPosDME - Ys * Ys);
				hMax = BaseHeight + dNomPosDer * PDG + fRefAltitude - ptDMEprj.Z;

				dOldPosDME = dNomPosDME;
				dNomPosDME = (d0 + NearSide * Navaids_DataBase.DME.MinimalError) / (1.0 - NearSide * Navaids_DataBase.DME.ErrorScalingUp * System.Math.Sqrt(1.0 + hMax * hMax / (dNomPosDer * dNomPosDer)));

				i--;
				if (i < 0)
					break;
			}
			while (System.Math.Abs(dOldPosDME - dNomPosDME) > GlobalVars.distEps);

			return dNomPosDME;
		}

		private static Interval CalcDMERange(IPoint ptBasePrj, double BaseHeight, double fRefAltitude, double NomDir,
											double PDG, IPoint ptDMEprj, IPolyline KKhMin, IPolyline KKhMax)
		{
			int Side;
			double d0, d1;

			int AheadBehindSide = Functions.SideDef(KKhMin.FromPoint, NomDir + 90.0, ptDMEprj);
			int LeftRightSide = Functions.SideDef(ptBasePrj, NomDir, ptDMEprj);

			double Xs = Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir + 90.0) * Functions.SideDef(ptBasePrj, NomDir + 90.0, ptDMEprj);
			double Ys = Point2LineDistancePrj(ptDMEprj, ptBasePrj, NomDir);

			if (AheadBehindSide < 0)
			{
				if (LeftRightSide > 0)
				{
					d0 = ReturnDistanceInMeters(ptDMEprj, KKhMin.ToPoint);

					Side = Functions.SideDef(KKhMax.FromPoint, NomDir, ptDMEprj);
					if (Side < 0)
						d1 = Point2LineDistancePrj(ptDMEprj, KKhMax.FromPoint, NomDir + 90.0);
					else
						d1 = ReturnDistanceInMeters(ptDMEprj, KKhMax.FromPoint);
				}
				else
				{
					d0 = ReturnDistanceInMeters(ptDMEprj, KKhMin.FromPoint);

					Side = Functions.SideDef(KKhMax.ToPoint, NomDir, ptDMEprj);
					if (Side > 0)
						d1 = Point2LineDistancePrj(ptDMEprj, KKhMax.ToPoint, NomDir + 90.0);
					else
						d1 = ReturnDistanceInMeters(ptDMEprj, KKhMax.ToPoint);
				}
			}
			else
			{
				if (LeftRightSide > 0)
				{
					d0 = ReturnDistanceInMeters(ptDMEprj, KKhMax.ToPoint);

					Side = Functions.SideDef(KKhMin.FromPoint, NomDir, ptDMEprj);
					if (Side < 0)
						d1 = Point2LineDistancePrj(ptDMEprj, KKhMin.ToPoint, NomDir + 90.0);
					else
						d1 = ReturnDistanceInMeters(ptDMEprj, KKhMin.FromPoint);
				}
				else
				{
					d0 = ReturnDistanceInMeters(ptDMEprj, KKhMax.FromPoint);

					Side = Functions.SideDef(KKhMin.ToPoint, NomDir, ptDMEprj);
					if (Side > 0)
						d1 = Point2LineDistancePrj(ptDMEprj, KKhMin.ToPoint, NomDir + 90.0);
					else
						d1 = ReturnDistanceInMeters(ptDMEprj, KKhMin.ToPoint);
				}
			}

			double Dist0 = CalcNomPos(ptDMEprj, Xs, Ys, d0, BaseHeight, fRefAltitude, PDG, AheadBehindSide, 1);
			double Dist1 = CalcNomPos(ptDMEprj, Xs, Ys, d1, BaseHeight, fRefAltitude, PDG, AheadBehindSide, -1);

			Interval result = new Interval();
			result.Left = Dist0;
			result.Right = Dist1;
			return result;
		}

		public static NavaidType[] GetValidNavs(IPolygon pPolygon, RWYType DER, double NomDir, double hMin, double hMax, double PDG, eNavaidType GuidType = eNavaidType.NONE, IPoint GuidNav = null)
		{
			int i, j, L;
			int m, ii, jj;
			int Side, LeftRightSide;
			int AheadBehindSide, AheadBehindKKhMax;

			double ERange, fTmp;
			double Dir_MinL2MaxR, Dir_MinR2MaxL;
			double InterToler, TrackToler = 0.0;

			Interval Intr23 = new Interval();
			Interval Intr55 = new Interval();
			Interval[] IntrRes;
			Interval[] IntrRes1;
			Interval[] IntrRes2;

			IPoint ptFNavPrj;
			IPoint ptFNav;
			IPoint ptBase;
			IPoint ptNear;
			IPoint ptFar;
			IPoint ptNearD;
			IPoint ptFarD;
			IPoint ptMin23;
			IPoint ptMax23;
			IPoint ptTmp;

			IPolyline KKhMin;
			IPolyline KKhMax;
			IPolyline KKhMinDME;
			IPolyline KKhMaxDME;

			IConstructPoint Construct;
			ITopologicalOperator2 pTopoOper;

			eNavaidType K;

			int nNav = GlobalVars.NavaidList.Length + GlobalVars.DMEList.Length;
			NavaidType[] ValidNavs = new NavaidType[nNav];

			if (nNav <= 0)
				return ValidNavs;

			IClone Clone = ((ESRI.ArcGIS.esriSystem.IClone)(pPolygon));
			IPolygon pFIXAreaPolygon = Clone.Clone() as IPolygon;

			pTopoOper = ((ESRI.ArcGIS.Geometry.ITopologicalOperator2)(pFIXAreaPolygon));
			pTopoOper.IsKnownSimple_2 = false;
			pTopoOper.Simplify();

			if (GuidType <= eNavaidType.NONE)
				ptBase = (ESRI.ArcGIS.Geometry.IPoint)DER.pPtPrj[eRWY.PtDER];
			else
			{
				ptBase = new ESRI.ArcGIS.Geometry.Point();
				Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptBase));
				Construct.ConstructAngleIntersection(DER.pPtPrj[eRWY.PtDER], GlobalVars.DegToRadValue * (NomDir + 90.0), GuidNav, GlobalVars.DegToRadValue * NomDir);
				ptBase.Z = DER.pPtPrj[eRWY.PtDER].Z;
			}

			IPolyline pCutter = ((ESRI.ArcGIS.Geometry.IPolyline)(new Polyline()));

			double dMin = (hMin - PANS_OPS_DataBase.dpH_abv_DER.Value) / PDG;
			double dMax = (hMax - PANS_OPS_DataBase.dpH_abv_DER.Value) / PDG;

			Interval IntrH = new Interval();
			IntrH.Left = dMin;
			IntrH.Right = dMax;

			ptTmp = Functions.PointAlongPlane(ptBase, NomDir, dMin);

			pCutter.FromPoint = Functions.PointAlongPlane(ptTmp, NomDir - 90.0, GlobalVars.RModel);
			pCutter.ToPoint = Functions.PointAlongPlane(ptTmp, NomDir + 90.0, GlobalVars.RModel);

			CutPoly(ref pFIXAreaPolygon, pCutter, -1);

			KKhMin = ((ESRI.ArcGIS.Geometry.IPolyline)(pTopoOper.Intersect(pCutter, esriGeometryDimension.esriGeometry1Dimension)));

			ptTmp = KKhMin.FromPoint;
			ptTmp.M = 0;
			KKhMin.FromPoint = ptTmp;

			ptTmp = KKhMin.ToPoint;
			ptTmp.M = 0;
			KKhMin.ToPoint = ptTmp;

			if (Functions.SideDef(ptTmp, NomDir, KKhMin.FromPoint) < 0)
				KKhMin.ReverseOrientation();

			ptTmp = Functions.PointAlongPlane(ptBase, NomDir, dMax);
			pCutter.FromPoint = Functions.PointAlongPlane(ptTmp, NomDir - 90.0, GlobalVars.RModel);
			pCutter.ToPoint = Functions.PointAlongPlane(ptTmp, NomDir + 90.0, GlobalVars.RModel);

			Functions.CutPoly(ref pFIXAreaPolygon, pCutter, 1);

			IGroupElement pGroupElem = (ESRI.ArcGIS.Carto.IGroupElement)(new GroupElement());
			pGroupElem.AddElement(Functions.DrawPolygon(pFIXAreaPolygon, Functions.RGB(195, 195, 195), esriSimpleFillStyle.esriSFSNull, false));
			GlobalVars.FIXElem = (ESRI.ArcGIS.Carto.IElement)pGroupElem;


			KKhMax = ((ESRI.ArcGIS.Geometry.IPolyline)(pTopoOper.Intersect(pCutter, esriGeometryDimension.esriGeometry1Dimension)));
			if (Functions.SideDef(ptTmp, NomDir, KKhMax.FromPoint) < 0)
				KKhMax.ReverseOrientation();

			if (GuidType > eNavaidType.NONE)
			{
				if (GuidType == eNavaidType.VOR)
					TrackToler = Navaids_DataBase.VOR.TrackingTolerance;
				else if (GuidType == eNavaidType.NDB)
					TrackToler = Navaids_DataBase.NDB.TrackingTolerance;

				Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptTmp));

				Side = Functions.SideDef(KKhMin.FromPoint, NomDir + 90.0, GuidNav);

				if (Side < 0)   // средство с зади
				{
					Construct.ConstructAngleIntersection(KKhMin.ToPoint, GlobalVars.DegToRadValue * (NomDir - 90.0), GuidNav, GlobalVars.DegToRadValue * (NomDir - TrackToler));
					KKhMin.FromPoint = ptTmp;
					Construct.ConstructAngleIntersection(KKhMin.FromPoint, GlobalVars.DegToRadValue * (NomDir + 90.0), GuidNav, GlobalVars.DegToRadValue * (NomDir + TrackToler));
					KKhMin.ToPoint = ptTmp;
				}
				else                // средство с переди
				{
					Construct.ConstructAngleIntersection(KKhMin.ToPoint, GlobalVars.DegToRadValue * (NomDir - 90.0), GuidNav, GlobalVars.DegToRadValue * (NomDir + 180.0 + TrackToler));
					KKhMin.FromPoint = ptTmp;
					Construct.ConstructAngleIntersection(KKhMin.FromPoint, GlobalVars.DegToRadValue * (NomDir + 90.0), GuidNav, GlobalVars.DegToRadValue * (NomDir + 180.0 - TrackToler));
					KKhMin.ToPoint = ptTmp;
				}

				Side = Functions.SideDef(KKhMax.FromPoint, NomDir + 90.0, GuidNav);

				if (Side < 0)       // средство с зади
				{
					Construct.ConstructAngleIntersection(KKhMax.ToPoint, GlobalVars.DegToRadValue * (NomDir - 90.0), GuidNav, GlobalVars.DegToRadValue * (NomDir - TrackToler));
					KKhMax.FromPoint = ptTmp;
					Construct.ConstructAngleIntersection(KKhMax.FromPoint, GlobalVars.DegToRadValue * (NomDir + 90.0), GuidNav, GlobalVars.DegToRadValue * (NomDir + TrackToler));
					KKhMax.ToPoint = ptTmp;
				}
				else                // средство с переди
				{
					Construct.ConstructAngleIntersection(KKhMax.ToPoint, GlobalVars.DegToRadValue * (NomDir - 90.0), GuidNav, GlobalVars.DegToRadValue * (NomDir + 180.0 + TrackToler));
					KKhMax.FromPoint = ptTmp;
					Construct.ConstructAngleIntersection(KKhMax.FromPoint, GlobalVars.DegToRadValue * (NomDir + 90.0), GuidNav, GlobalVars.DegToRadValue * (NomDir + 180.0 - TrackToler));
					KKhMax.ToPoint = ptTmp;
				}
			}

			ptTmp = KKhMin.FromPoint;
			ptTmp.M = Functions.ReturnAngleInDegrees(KKhMin.FromPoint, KKhMax.FromPoint);
			KKhMin.FromPoint = ptTmp;

			ptTmp = KKhMin.ToPoint;
			ptTmp.M = Functions.ReturnAngleInDegrees(KKhMin.ToPoint, KKhMax.ToPoint);
			KKhMin.ToPoint = ptTmp;

			ptNearD = new ESRI.ArcGIS.Geometry.Point();
			Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptNearD));
			Construct.ConstructAngleIntersection(ptBase, GlobalVars.DegToRadValue * NomDir, KKhMin.ToPoint, GlobalVars.DegToRadValue * (NomDir + 90.0));

			ptFarD = new ESRI.ArcGIS.Geometry.Point();
			Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptFarD));
			Construct.ConstructAngleIntersection(ptBase, GlobalVars.DegToRadValue * NomDir, KKhMax.ToPoint, GlobalVars.DegToRadValue * (NomDir + 90.0));

			Dir_MinL2MaxR = Functions.ReturnAngleInDegrees(KKhMin.ToPoint, KKhMax.FromPoint);
			Dir_MinR2MaxL = Functions.ReturnAngleInDegrees(KKhMin.FromPoint, KKhMax.ToPoint);

			j = 0;

			for (i = 0; i < GlobalVars.NavaidList.Length; i++)
			{
				ptFNav = GlobalVars.NavaidList[i].pPtGeo;
				ptFNavPrj = GlobalVars.NavaidList[i].pPtPrj;

				K = GlobalVars.NavaidList[i].TypeCode;
				LeftRightSide = Functions.SideDef(ptBase, NomDir, ptFNavPrj);

				AheadBehindSide = Functions.SideDef(KKhMin.FromPoint, NomDir + 90.0, ptFNavPrj);
				AheadBehindKKhMax = Functions.SideDef(KKhMax.FromPoint, NomDir + 90.0, ptFNavPrj);

				if (K == eNavaidType.VOR)
				{
					InterToler = Navaids_DataBase.VOR.IntersectingTolerance;
					ERange = Navaids_DataBase.VOR.Range;
				}
				else if (K == eNavaidType.NDB)
				{
					InterToler = Navaids_DataBase.NDB.IntersectingTolerance;
					ERange = Navaids_DataBase.NDB.Range;
				}
				else
					continue;

				Side = Functions.SideDef(KKhMax.FromPoint, Dir_MinL2MaxR, ptFNavPrj);
				if (Side * LeftRightSide < 0)
					continue;

				Side = Functions.SideDef(KKhMax.ToPoint, Dir_MinR2MaxL, ptFNavPrj);
				if (Side * LeftRightSide < 0)
					continue;

				if (LeftRightSide > 0)
				{
					Side = Functions.SideDef(KKhMin.FromPoint, KKhMin.FromPoint.M, ptFNavPrj);
					if (Side < 0)
						continue;

					if (AheadBehindSide < 0)
					{
						ptNear = KKhMin.FromPoint;
						ptFar = KKhMax.ToPoint;
					}
					else if (AheadBehindKKhMax < 0)
					{
						ptNear = KKhMin.ToPoint;
						ptFar = KKhMax.ToPoint;
					}
					else
					{
						ptNear = KKhMin.ToPoint;
						ptFar = KKhMax.FromPoint;
					}
				}
				else
				{
					Side = Functions.SideDef(KKhMin.ToPoint, KKhMin.ToPoint.M, ptFNavPrj);
					if (Side > 0)
						continue;

					if (AheadBehindSide < 0)
					{
						ptNear = KKhMin.ToPoint;
						ptFar = KKhMax.FromPoint;
					}
					else if (AheadBehindKKhMax < 0)
					{
						ptNear = KKhMin.FromPoint;
						ptFar = KKhMax.FromPoint;
					}
					else
					{
						ptNear = KKhMin.FromPoint;
						ptFar = KKhMax.ToPoint;
					}
				}

				if (ERange < Functions.ReturnDistanceInMeters(ptFNavPrj, ptFar))
					continue;

				double azt_Far = Functions.ReturnAngleInDegrees(ptFNavPrj, ptFar);
				double azt_Near = Functions.ReturnAngleInDegrees(ptFNavPrj, ptNear);

				if (Functions.SubtractAngles(azt_Near, azt_Far) < 2.0 * InterToler)
					continue;

				ValidNavs[j] = GlobalVars.NavaidList[i];

				ValidNavs[j].ValMax = new double[1];
				ValidNavs[j].ValMin = new double[1];
				ValidNavs[j].ValCnt = LeftRightSide;

				if (LeftRightSide > 0)
				{
					ValidNavs[j].ValMax[0] = System.Math.Round(Functions.Dir2Azt(ptFNavPrj, azt_Far + InterToler) - 0.4999);
					ValidNavs[j].ValMin[0] = System.Math.Round(Functions.Dir2Azt(ptFNavPrj, azt_Near - InterToler) + 0.4999);
				}
				else
				{
					ValidNavs[j].ValMin[0] = System.Math.Round(Functions.Dir2Azt(ptFNavPrj, azt_Far - InterToler) + 0.4999);
					ValidNavs[j].ValMax[0] = System.Math.Round(Functions.Dir2Azt(ptFNavPrj, azt_Near + InterToler) - 0.4999);
				}

				if (Functions.SubtractAngles(ValidNavs[j].ValMax[0] + InterToler, ValidNavs[j].ValMin[0] - InterToler) < InterToler)
					continue;

				ValidNavs[j].IntersectionType = eIntersectionType.ByAngle;
				j++;
			}

			// =====  DME ===

			for (i = 0; i < GlobalVars.DMEList.Length; i++)
			{
				ptFNav = GlobalVars.DMEList[i].pPtGeo;
				ptFNavPrj = GlobalVars.DMEList[i].pPtPrj;

				LeftRightSide = Functions.SideDef(ptBase, NomDir, ptFNavPrj);
				AheadBehindSide = Functions.SideDef(KKhMin.FromPoint, NomDir + 90.0, ptFNavPrj);
				AheadBehindKKhMax = Functions.SideDef(KKhMax.FromPoint, NomDir + 90.0, ptFNavPrj);

				if (LeftRightSide > 0)
				{
					if (AheadBehindSide < 0)
						fTmp = Functions.ReturnDistanceInMeters(ptFNavPrj, KKhMin.ToPoint);
					else
						fTmp = Functions.ReturnDistanceInMeters(ptFNavPrj, KKhMax.ToPoint);
				}
				else
				{
					if (AheadBehindSide < 0)
						fTmp = Functions.ReturnDistanceInMeters(ptFNavPrj, KKhMin.FromPoint);
					else
						fTmp = Functions.ReturnDistanceInMeters(ptFNavPrj, KKhMax.FromPoint);
				}

				if (fTmp > Navaids_DataBase.DME.Range)
					continue;

				if (LeftRightSide != 0)
				{
					ptMin23 = new ESRI.ArcGIS.Geometry.Point();
					ptMax23 = new ESRI.ArcGIS.Geometry.Point();
					Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptMin23));
					Construct.ConstructAngleIntersection(ptBase, GlobalVars.DegToRadValue * NomDir, ptFNavPrj, GlobalVars.DegToRadValue * (NomDir - LeftRightSide * PANS_OPS_DataBase.dpTP_by_DME_div.Value));
					Construct = ((ESRI.ArcGIS.Geometry.IConstructPoint)(ptMax23));
					Construct.ConstructAngleIntersection(ptBase, GlobalVars.DegToRadValue * NomDir, ptFNavPrj, GlobalVars.DegToRadValue * (NomDir + LeftRightSide * PANS_OPS_DataBase.dpTP_by_DME_div.Value));
				}
				else
				{
					ptMin23 = ptFNavPrj;
					ptMax23 = ptFNavPrj;
				}

				Intr23.Left = Functions.Point2LineDistancePrj(ptBase, ptMin23, NomDir + 90.0) * Functions.SideDef(ptBase, NomDir + 90.0, ptMin23);
				Intr23.Right = Functions.Point2LineDistancePrj(ptBase, ptMax23, NomDir + 90.0) * Functions.SideDef(ptBase, NomDir + 90.0, ptMax23);
				IntrRes = IntervalsDifference(IntrH, Intr23);

				double Xs = Functions.Point2LineDistancePrj(ptFNavPrj, ptBase, NomDir + 90.0) * Functions.SideDef(ptBase, NomDir + 90.0, ptFNavPrj);
				double Ys = Functions.Point2LineDistancePrj(ptFNavPrj, ptBase, NomDir);

				fTmp = 1.0 / System.Math.Tan(GlobalVars.DegToRadValue * Navaids_DataBase.DME.SlantAngle);
				fTmp = fTmp * fTmp;
				double A = PANS_OPS_DataBase.dpOv_Nav_PDG.Value * PANS_OPS_DataBase.dpOv_Nav_PDG.Value - fTmp;
				double B = 2.0 * ((PANS_OPS_DataBase.dpOIS_abv_DER.Value - ptFNavPrj.Z + ptBase.Z) * PANS_OPS_DataBase.dpOv_Nav_PDG.Value + Xs * fTmp);
				double C = (PANS_OPS_DataBase.dpOIS_abv_DER.Value - ptFNavPrj.Z + ptBase.Z) * (PANS_OPS_DataBase.dpOIS_abv_DER.Value - ptFNavPrj.Z + ptBase.Z) - (Xs * Xs + Ys * Ys) * fTmp;
				double d = B * B - 4 * A * C;

				if (d > 0.0)
				{
					if (A > 0)
					{
						Intr55.Left = 0.5 * (-B - System.Math.Sqrt(d)) / A;
						Intr55.Right = 0.5 * (-B + System.Math.Sqrt(d)) / A;
					}
					else
					{
						Intr55.Left = 0.5 * (-B + System.Math.Sqrt(d)) / A;
						Intr55.Right = 0.5 * (-B - System.Math.Sqrt(d)) / A;
					}

					IntrRes1 = new Interval[0];

					for (ii = 0; ii < IntrRes.Length; ii++)
					{
						IntrRes2 = IntervalsDifference(IntrRes[ii], Intr55);

						if (IntrRes1.Length == 0)
							IntrRes1 = CopyArray<Interval>(IntrRes2);
						else
						{
							L = IntrRes1.Length;
							m = IntrRes2.Length;

							if (m > 0)
							{
								System.Array.Resize<Interval>(ref IntrRes1, L + m);

								for (jj = 0; jj < m; jj++)
									IntrRes1[jj + L] = IntrRes2[jj];
							}
						}
					}

					IntrRes = CopyArray<Interval>(IntrRes1);
				}

				// ==========================================================
				//    18:48 12.09.2007

				if (Ys < 1.5 * Navaids_DataBase.DME.MinimalError)
				{
					Functions.CircleVectorIntersect(ptFNavPrj, 1.5 * Navaids_DataBase.DME.MinimalError, ptBase, NomDir, out ptMin23);
					Functions.CircleVectorIntersect(ptFNavPrj, 1.5 * Navaids_DataBase.DME.MinimalError, ptBase, NomDir + 180.0, out ptMin23);

					Intr23.Left = Functions.Point2LineDistancePrj(ptBase, ptMin23, NomDir + 90.0) * Functions.SideDef(ptBase, NomDir + 90.0, ptMin23);
					Intr23.Right = Functions.Point2LineDistancePrj(ptBase, ptMax23, NomDir + 90.0) * Functions.SideDef(ptBase, NomDir + 90.0, ptMax23);

					IntrRes1 = new Interval[0];

					for (ii = 0; ii < IntrRes.Length; ii++)
					{
						IntrRes2 = IntervalsDifference(IntrRes[ii], Intr23);

						if (IntrRes1.Length == 0)
							IntrRes1 = CopyArray<Interval>(IntrRes2);
						else
						{
							L = IntrRes1.Length;
							m = IntrRes2.Length;

							if (m > 0)
							{
								System.Array.Resize<Interval>(ref IntrRes1, L + m);

								for (jj = 0; jj < m; jj++)
									IntrRes1[jj + L] = IntrRes2[jj];
							}
						}
					}
					IntrRes = CopyArray<Interval>(IntrRes1);
				}
				// ==========================================================
				int IntrRes1MaxIndex = IntrRes.Length - 1;

				ii = 0;
				if (IntrRes1MaxIndex >= 0)
				{
					do
					{
						if (IntrRes[ii].Left == IntrRes[ii].Right)
						{
							for (jj = ii; jj <= IntrRes1MaxIndex - 1; jj++)
								IntrRes[jj] = IntrRes[jj + 1];

							IntrRes1MaxIndex = IntrRes1MaxIndex - 1;
						}
						else
							ii++;
					}
					while (ii < IntrRes1MaxIndex - 1);
				}

				ii = 0;
				while (ii < IntrRes1MaxIndex - 1)
				{
					if (IntrRes[ii].Right == IntrRes[ii + 1].Left)
					{
						IntrRes[ii].Right = IntrRes[ii + 1].Right;
						for (jj = ii + 1; jj <= IntrRes1MaxIndex - 1; jj++)
							IntrRes[jj] = IntrRes[jj + 1];

						IntrRes1MaxIndex = IntrRes1MaxIndex - 1;
					}
					else
						ii++;
				}

				if (IntrRes1MaxIndex < 0)
					continue;

				IntrRes1 = new Interval[IntrRes1MaxIndex + 1];
				m = 0;

				ValidNavs[j] = GlobalVars.DMEList[i];

				for (ii = 0; ii <= IntrRes1MaxIndex; ii++)
				{
					ptNearD = Functions.PointAlongPlane(ptBase, NomDir, IntrRes[ii].Left);
					ptFarD = Functions.PointAlongPlane(ptBase, NomDir, IntrRes[ii].Right);

					pCutter.FromPoint = Functions.PointAlongPlane(ptNearD, NomDir - 90.0, GlobalVars.RModel);
					pCutter.ToPoint = Functions.PointAlongPlane(ptNearD, NomDir + 90.0, GlobalVars.RModel);

					KKhMinDME = ((ESRI.ArcGIS.Geometry.IPolyline)(pTopoOper.Intersect(pCutter, esriGeometryDimension.esriGeometry1Dimension)));

					if (Functions.SideDef(ptNearD, NomDir, KKhMinDME.FromPoint) < 0)
						KKhMinDME.ReverseOrientation();

					pCutter.FromPoint = Functions.PointAlongPlane(ptFarD, NomDir - 90.0, GlobalVars.RModel);
					pCutter.ToPoint = Functions.PointAlongPlane(ptFarD, NomDir + 90.0, GlobalVars.RModel);

					KKhMaxDME = ((ESRI.ArcGIS.Geometry.IPolyline)(pTopoOper.Intersect(pCutter, esriGeometryDimension.esriGeometry1Dimension)));

					if (Functions.SideDef(ptFarD, NomDir, KKhMaxDME.FromPoint) < 0)
						KKhMaxDME.ReverseOrientation();

					//DrawPointWithText(ptBase, "ptBase");
					//System.Windows.Forms.Application.DoEvents();

					IntrRes1[m] = CalcDMERange(ptBase, PANS_OPS_DataBase.dpH_abv_DER.Value, ptBase.Z, NomDir, PDG, ptFNavPrj, KKhMinDME, KKhMaxDME);
					if (IntrRes1[m].Left < IntrRes1[m].Right)
					{
						m++;
						ValidNavs[j].ValCnt = Functions.SideDef(KKhMinDME.FromPoint, NomDir + 90.0, ptFNavPrj);
					}
				}

				m--;

				if (m < 0)
					continue;

				if (m > 0)
					ValidNavs[j].ValCnt = 0;

				ValidNavs[j].ValMax = new double[m + 1];
				ValidNavs[j].ValMin = new double[m + 1];

				for (ii = 0; ii <= m; ii++)
				{
					ValidNavs[j].ValMin[ii] = IntrRes1[ii].Left;
					ValidNavs[j].ValMax[ii] = IntrRes1[ii].Right;
				}

				if (ValidNavs[j].ValMax[0] < ValidNavs[j].ValMin[0])
					continue;

				ValidNavs[j].IntersectionType = eIntersectionType.ByDistance;

				j++;
			}

			if (j == 0)
				return new NavaidType[0];

			System.Array.Resize<NavaidType>(ref ValidNavs, j);
			return ValidNavs;
		}

		public static NavaidType[] GetValidTurnTermNavs(IPoint pTurnComplitPt, double fRefAltitude, double minDist, double MaxDist, double NomDir, double fPDG, eNavaidType GuidType, IPoint GuidNav)
		{
			int AheadBehindSide, LeftRightSide, Side;
			int ii, jj, l, m;
			eNavaidType k;

			double fMinDMEDist;
			double InterToler;
			double azt_Near;
			double azt_Far;
			double ERange;
			double Hequip;

			Interval[] IntrRes1;
			Interval[] IntrRes2;
			Interval[] IntrRes;

			ITopologicalOperator2 pTopoOper;
			IPointCollection pGuidPoly;
			IConstructPoint Construct;
			IProximityOperator pProxi;
			IPointCollection Cutter;
			IPolyline KKhMinDME;
			IPolyline KKhMaxDME;
			IPolyline KKhMax;

			IPoint ptFNavPrj;
			IPoint ptMin23;
			IPoint ptMax23;
			IPoint ptNearD;
			IPoint ptFarD;
			IPoint ptNear;
			IPoint ptFNav;
			IPoint ptTmp;
			IPoint ptFar;

			int nNav = GlobalVars.NavaidList.Length + GlobalVars.DMEList.Length;
			NavaidType[] ValidNavs = new NavaidType[nNav];

			if (nNav == 0)
				return ValidNavs;

			double TrackToler = 0.0;

			if (GuidType == eNavaidType.VOR)
				TrackToler = Navaids_DataBase.VOR.TrackingTolerance;
			else if (GuidType == eNavaidType.NDB)
				TrackToler = Navaids_DataBase.NDB.TrackingTolerance;
			else if (GuidType == eNavaidType.LLZ)
				TrackToler = Navaids_DataBase.LLZ.TrackingTolerance;

			pGuidPoly = new Polygon();
			// If GuidType <> 3 Then
			pGuidPoly.AddPoint(GuidNav);
			pGuidPoly.AddPoint(Functions.PointAlongPlane(GuidNav, NomDir - TrackToler, 3.0 * GlobalVars.MaxModelRadius));
			pGuidPoly.AddPoint(Functions.PointAlongPlane(GuidNav, NomDir + TrackToler, 3.0 * GlobalVars.MaxModelRadius));
			// End If
			pGuidPoly.AddPoint(GuidNav);
			pGuidPoly.AddPoint(Functions.PointAlongPlane(GuidNav, NomDir - TrackToler + 180.0, 3.0 * GlobalVars.MaxModelRadius));
			pGuidPoly.AddPoint(Functions.PointAlongPlane(GuidNav, NomDir + TrackToler + 180.0, 3.0 * GlobalVars.MaxModelRadius));
			pGuidPoly.AddPoint(GuidNav);

			pTopoOper = ((ITopologicalOperator2)(pGuidPoly));
			pTopoOper.IsKnownSimple_2 = false;
			pTopoOper.Simplify();

			ptFar = Functions.PointAlongPlane(pTurnComplitPt, NomDir, MaxDist);
			ptNear = Functions.PointAlongPlane(pTurnComplitPt, NomDir, minDist);

			//DrawPointWithText(ptFar, "ptFar",255);
			//DrawPointWithText(ptNear, "ptNear",255);
			//System.Windows.Forms.Application.DoEvents();

			KKhMax = (IPolyline)(new Polyline());
			ptTmp = new Point();
			Construct = (IConstructPoint)ptTmp;

			Construct.ConstructAngleIntersection(GuidNav, DegToRad(NomDir - TrackToler), ptFar, DegToRad(NomDir + 90.0));
			KKhMax.FromPoint = ptTmp;

			Construct.ConstructAngleIntersection(GuidNav, DegToRad(NomDir + TrackToler), ptFar, DegToRad(NomDir + 90.0));
			KKhMax.ToPoint = ptTmp;

			if (Functions.SideDef(KKhMax.ToPoint, NomDir, KKhMax.FromPoint) > 0)
				KKhMax.ReverseOrientation();

			Cutter = new Polyline();
			KKhMaxDME = ((ESRI.ArcGIS.Geometry.IPolyline)(new Polyline()));

			int j = 0;

			for (int i = 0; i < GlobalVars.NavaidList.Length; i++)
			{
				ptFNav = GlobalVars.NavaidList[i].pPtGeo;
				ptFNavPrj = GlobalVars.NavaidList[i].pPtPrj;
				k = GlobalVars.NavaidList[i].TypeCode;

				LeftRightSide = Functions.SideDef(ptNear, NomDir, ptFNavPrj);
				AheadBehindSide = Functions.SideDef(ptNear, NomDir + 90.0, ptFNavPrj); // ptFar

				if (k == eNavaidType.VOR)
				{
					InterToler = Navaids_DataBase.VOR.IntersectingTolerance;
					ERange = Navaids_DataBase.VOR.Range;
				}
				else if (k == eNavaidType.NDB)
				{
					InterToler = Navaids_DataBase.NDB.IntersectingTolerance;
					ERange = Navaids_DataBase.NDB.Range;
				}
				else
					continue;

				pProxi = (ESRI.ArcGIS.Geometry.IProximityOperator)pGuidPoly;

				if (pProxi.ReturnDistance(ptFNavPrj) == 0.0)
					continue;

				Side = Functions.SideDef(KKhMax.FromPoint, NomDir + LeftRightSide * 90.0, ptFNavPrj);

				if (Side < 0)
					ptFarD = KKhMax.ToPoint;
				else
					ptFarD = KKhMax.FromPoint;

				// If ERange < Functions.ReturnDistanceInMeters(ptFNavPrj, ptFarD) Then Continue For

				//DrawPolyline(KKhMax, 0,2);
				//DrawPolyline(KKhMax, 255, 2);
				//System.Windows.Forms.Application.DoEvents();

				if (GlobalVars.NavaidList[i].Range < Functions.ReturnDistanceInMeters(ptFNavPrj, ptFarD))
					continue;

				azt_Far = Functions.ReturnAngleInDegrees(ptFNavPrj, ptFarD);
				azt_Near = Functions.ReturnAngleInDegrees(ptFNavPrj, ptNear);

				if (Functions.SubtractAngles(azt_Near, azt_Far) < 2.0 * InterToler)
					continue;

				double d = Functions.Point2LineDistancePrj(ptFNavPrj, pTurnComplitPt, NomDir);
				if (RadToDeg(System.Math.Atan(GlobalVars.SIDTerminationFIXToler / d)) < InterToler)
					continue;

				double Betta = 0.5 * (ArcCos(2.0 * d * System.Math.Sin(DegToRad(InterToler)) / GlobalVars.SIDTerminationFIXToler - System.Math.Cos(DegToRad(InterToler))) - DegToRad(InterToler));
				double Dist0 = 0.0, Dist1 = 0.0;

				Construct = (ESRI.ArcGIS.Geometry.IConstructPoint)ptTmp;
				Construct.ConstructAngleIntersection(GuidNav, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir - 90.0) + Betta);
				// DrawPoint ptTmp, RGB(0, 0, 255)
				if (Functions.SideDef(pTurnComplitPt, NomDir - 90.0, ptTmp) <= 0)
					Dist0 = Functions.ReturnDistanceInMeters(pTurnComplitPt, ptTmp);

				Construct.ConstructAngleIntersection(GuidNav, DegToRad(NomDir), ptFNavPrj, DegToRad(NomDir - 90.0) - Betta);
				// DrawPoint ptTmp, RGB(0, 0, 255)

				if (Functions.SideDef(pTurnComplitPt, NomDir - 90.0, ptTmp) <= 0)
					Dist1 = Functions.ReturnDistanceInMeters(pTurnComplitPt, ptTmp);

				if (Dist1 < Dist0)
				{
					double fTmp = Dist1;
					Dist1 = Dist0;
					Dist0 = fTmp;
				}

				if (Dist1 == 0)
					continue;

				//if (Dist1 < minDist) continue;
				//if (Dist0 > MaxDist) continue;

				double d0 = minDist;
				Construct.ConstructAngleIntersection(GuidNav, DegToRad(NomDir), ptFNavPrj, DegToRad(azt_Far + LeftRightSide * InterToler));
				// DrawPoint ptTmp, RGB(255, 0, 255)
				double d1 = Functions.ReturnDistanceInMeters(pTurnComplitPt, ptTmp);
				if (d0 < Dist0)
					d0 = Dist0;

				if (d1 > Dist1)
					d1 = Dist1;

				if (d1 < d0)
					continue;

				ValidNavs[j] = GlobalVars.NavaidList[i];

				ptNearD = PointAlongPlane(pTurnComplitPt, NomDir, d0);
				ptFarD = PointAlongPlane(pTurnComplitPt, NomDir, d1);

				azt_Far = ReturnAngleInDegrees(ptFNavPrj, ptFarD);
				azt_Near = ReturnAngleInDegrees(ptFNavPrj, ptNearD);

				ValidNavs[j].IntersectionType = eIntersectionType.ByAngle;
				ValidNavs[j].ValMax = new double[1];
				ValidNavs[j].ValMin = new double[1];
				ValidNavs[j].ValCnt = LeftRightSide;

				//	ValidNavs[J].ValMax[0] = System.Math.Round(Dir2Azt(ptFNavPrj, azt_Far) - 0.4999);
				//	ValidNavs[J].ValMin[0] = System.Math.Round(Dir2Azt(ptFNavPrj, azt_Near) + 0.4999);
				//	if (Functions.SubtractAngles(ValidNavs[J].ValMax[0], ValidNavs[J].ValMin[0]) < InterToler)
				//		continue;

				if (LeftRightSide > 0)
				{
					ValidNavs[j].ValMax[0] = Math.Round(Dir2Azt(ptFNavPrj, azt_Far) - 0.4999);
					ValidNavs[j].ValMin[0] = Math.Round(Dir2Azt(ptFNavPrj, azt_Near) + 0.4999);
				}
				else
				{
					ValidNavs[j].ValMin[0] = Math.Round(Dir2Azt(ptFNavPrj, azt_Far) + 0.4999);
					ValidNavs[j].ValMax[0] = Math.Round(Dir2Azt(ptFNavPrj, azt_Near) - 0.4999);
				}

				if (SubtractAngles(ValidNavs[j].ValMax[0] + InterToler, ValidNavs[j].ValMin[0] - InterToler) < InterToler)
					continue;

				j++;
			}

			Interval IntrH = new Interval();
			Interval Intr23 = new Interval();
			Interval Intr55 = new Interval();
			Interval Intr3700 = new Interval();

			for (int i = 0; i <= GlobalVars.DMEList.GetUpperBound(0); i++)
			{
				ptFNav = GlobalVars.DMEList[i].pPtGeo;
				ptFNavPrj = GlobalVars.DMEList[i].pPtPrj;

				LeftRightSide = Functions.SideDef(ptNear, NomDir, ptFNavPrj);
				AheadBehindSide = Functions.SideDef(ptNear, NomDir + 90.0, ptFNavPrj); // ptFar

				IntrH.Left = minDist;
				IntrH.Right = MaxDist;
				double fTmp;

				if (AheadBehindSide < 0)
					fTmp = ReturnDistanceInMeters(ptFNavPrj, ptNear);
				else if (LeftRightSide > 0)
					fTmp = ReturnDistanceInMeters(ptFNavPrj, KKhMax.ToPoint);
				else
					fTmp = ReturnDistanceInMeters(ptFNavPrj, KKhMax.FromPoint);

				if (fTmp > Navaids_DataBase.DME.Range)
					continue;

				//    Range checking
				if (LeftRightSide != 0)
				{
					ptMin23 = new Point();
					ptMax23 = new Point();
					Construct = ((IConstructPoint)(ptMin23));
					Construct.ConstructAngleIntersection(pTurnComplitPt, GlobalVars.DegToRadValue * NomDir, ptFNavPrj, GlobalVars.DegToRadValue * (NomDir - LeftRightSide * Navaids_DataBase.DME.TP_div));
					Construct = ((IConstructPoint)(ptMax23));
					Construct.ConstructAngleIntersection(pTurnComplitPt, GlobalVars.DegToRadValue * NomDir, ptFNavPrj, GlobalVars.DegToRadValue * (NomDir + LeftRightSide * Navaids_DataBase.DME.TP_div));
				}
				else
				{
					ptMin23 = ptFNavPrj;
					ptMax23 = ptFNavPrj;
				}

				Intr23.Left = Point2LineDistancePrj(pTurnComplitPt, ptMin23, NomDir + 90.0) * SideDef(pTurnComplitPt, NomDir + 90.0, ptMin23);
				Intr23.Right = Point2LineDistancePrj(pTurnComplitPt, ptMax23, NomDir + 90.0) * SideDef(pTurnComplitPt, NomDir + 90.0, ptMax23);

				fTmp = Point2LineDistancePrj(ptFNavPrj, GuidNav, NomDir + TrackToler) + 5.0;
				double d = Point2LineDistancePrj(ptFNavPrj, GuidNav, NomDir - TrackToler) + 5.0;
				if (d < fTmp)
					d = fTmp;

				fMinDMEDist = (Navaids_DataBase.DME.MinimalError + d) / (1.0 - Navaids_DataBase.DME.ErrorScalingUp);

				if (CircleVectorIntersect(ptFNavPrj, fMinDMEDist, pTurnComplitPt, NomDir + 180.0, out ptMin23) >= 0)
				{
					CircleVectorIntersect(ptFNavPrj, fMinDMEDist, pTurnComplitPt, NomDir, out ptMax23);
					double a = Point2LineDistancePrj(pTurnComplitPt, ptMax23, NomDir + 90.0) * SideDef(pTurnComplitPt, NomDir + 90.0, ptMax23);
					double b = Point2LineDistancePrj(pTurnComplitPt, ptMin23, NomDir + 90.0) * SideDef(pTurnComplitPt, NomDir + 90.0, ptMin23);
					if (Intr23.Left > a)
						Intr23.Left = a;

					if (Intr23.Right < b)
						Intr23.Right = b;
				}

				IntrRes = IntervalsDifference(IntrH, Intr23);
				if (IntrRes.GetUpperBound(0) < 0)
					continue;

				//fTmp = Functions.CircleVectorIntersect(ptFNavPrj, maxDist, pTurnComplitPt, NomDir + 180#, ptTmp)

				double Xs = Point2LineDistancePrj(ptFNavPrj, pTurnComplitPt, NomDir + 90.0) * SideDef(pTurnComplitPt, NomDir + 90.0, ptFNavPrj);
				double Ys = Point2LineDistancePrj(ptFNavPrj, pTurnComplitPt, NomDir);
				Hequip = pTurnComplitPt.Z + fRefAltitude;

				// - ptFNavPrj.Z
				// If Hequip < 0 Then Hequip = 0
				// ======================= 3700 ==============================================================
				double A = 1.0 + PANS_OPS_DataBase.dpOv_Nav_PDG.Value * PANS_OPS_DataBase.dpOv_Nav_PDG.Value;
				double B = 2.0 * PANS_OPS_DataBase.dpOv_Nav_PDG.Value * Hequip - Xs;
				double C = Hequip * Hequip + Xs * Xs + Ys * Ys - Math.Pow(((GlobalVars.SIDTerminationFIXToler * Math.Cos(DegToRad(Navaids_DataBase.DME.TP_div)) - Navaids_DataBase.DME.MinimalError) / Navaids_DataBase.DME.ErrorScalingUp), 2);
				d = B * B - 4.0 * A * C;

				if (d <= 0.0)
					continue;

				d = Math.Sqrt(d);
				if (A > 0)
				{
					Intr3700.Left = 0.5 * (-B - d) / A;
					Intr3700.Right = 0.5 * (-B + d) / A;
				}
				else
				{
					Intr3700.Left = 0.5 * (-B + d) / A;
					Intr3700.Right = 0.5 * (-B - d) / A;
				}

				if (IntrH.Left < Intr3700.Left)
					IntrH.Left = Intr3700.Left;

				if (IntrH.Right > Intr3700.Right)
					IntrH.Right = Intr3700.Right;

				if (IntrH.Left >= IntrH.Right)
					continue;

				// ===========================================================================================

				fTmp = 1.0 / System.Math.Tan(DegToRad(Navaids_DataBase.DME.SlantAngle));
				fTmp = fTmp * fTmp;

				A = PANS_OPS_DataBase.dpOv_Nav_PDG.Value * PANS_OPS_DataBase.dpOv_Nav_PDG.Value - fTmp;
				B = 2.0 * ((Hequip - ptFNavPrj.Z) * PANS_OPS_DataBase.dpOv_Nav_PDG.Value + Xs * fTmp);
				C = Math.Pow((Hequip - ptFNavPrj.Z), 2) - (Xs * Xs + Ys * Ys) * fTmp;
				d = B * B - 4.0 * A * C;

				if (d > 0.0)
				{
					if (A > 0)
					{
						Intr55.Left = 0.5 * (-B - Math.Sqrt(d)) / A;
						Intr55.Right = 0.5 * (-B + Math.Sqrt(d)) / A;
					}
					else
					{
						Intr55.Left = 0.5 * (-B + Math.Sqrt(d)) / A;
						Intr55.Right = 0.5 * (-B - Math.Sqrt(d)) / A;
					}

					IntrRes1 = new Interval[0];

					for (ii = 0; ii < IntrRes.Length; ii++)
					{
						IntrRes2 = IntervalsDifference(IntrRes[ii], Intr55);

						if (IntrRes1.Length == 0)
						{
							IntrRes1 = CopyArray<Interval>(IntrRes2);
						}
						else
						{
							l = IntrRes1.Length;
							m = IntrRes2.Length;
							if (m > 0)
							{
								System.Array.Resize<Interval>(ref IntrRes1, l + m);

								for (jj = 0; jj < m; jj++)
									IntrRes1[jj + l] = IntrRes2[jj];
							}
						}
					}

					if (IntrRes1.Length != IntrRes.Length)
						System.Array.Resize<Interval>(ref IntrRes, IntrRes1.Length);

					System.Array.Copy(IntrRes1, IntrRes, IntrRes1.Length);
				}

				int n = IntrRes.GetUpperBound(0);

				ii = 0;
				if (n >= 0)
				{
					do
					{
						if (IntrRes[ii].Left == IntrRes[ii].Right)
						{
							for (jj = ii; jj < n; jj++)
								IntrRes[jj] = IntrRes[jj + 1];

							n--;
						}
						else
							ii++;
					}
					while (ii < n - 1);
				}

				ii = 0;
				while (ii < n - 1)
				{
					if (IntrRes[ii].Right == IntrRes[ii + 1].Left)
					{
						IntrRes[ii].Right = IntrRes[ii + 1].Right;
						for (jj = ii + 1; jj < n; jj++)
							IntrRes[jj] = IntrRes[jj + 1];

						n--;
					}
					else
						ii++;
				}

				if (n < 0)
					continue;

				ValidNavs[j] = GlobalVars.DMEList[i];

				IntrRes1 = new Interval[n + 1];
				m = 0;

				for (ii = 0; ii <= n; ii++)
				{
					ptNearD = Functions.PointAlongPlane(pTurnComplitPt, NomDir, IntrRes[ii].Left);
					ptFarD = Functions.PointAlongPlane(pTurnComplitPt, NomDir, IntrRes[ii].Right);
					double d1 = Functions.ReturnDistanceInMeters(ptNearD, ptFNavPrj);

					KKhMinDME = (IPolyline)(new Polyline());
					KKhMinDME.FromPoint = ptNearD;
					KKhMinDME.ToPoint = ptNearD;

					KKhMaxDME.FromPoint = Functions.PointAlongPlane(ptFarD, NomDir + 90.0, GlobalVars.RModel);
					KKhMaxDME.ToPoint = Functions.PointAlongPlane(ptFarD, NomDir - 90.0, GlobalVars.RModel);

					KKhMaxDME = (IPolyline)pTopoOper.Intersect(KKhMaxDME, esriGeometryDimension.esriGeometry1Dimension);
					if (KKhMaxDME.IsEmpty)
					{
						KKhMaxDME.FromPoint = ptFarD;
						KKhMaxDME.ToPoint = ptFarD;
					}
					else if (Functions.SideDef(ptFarD, NomDir, KKhMaxDME.FromPoint) < 0)
						KKhMaxDME.ReverseOrientation();

					IntrRes1[m] = CalcDMERange(pTurnComplitPt, pTurnComplitPt.Z, fRefAltitude, NomDir, fPDG, ptFNavPrj, KKhMinDME, KKhMaxDME);
					if (ii == 0)
					{
						if (AheadBehindSide < 0)
						{
							if (IntrRes1[m].Left > d1)
								IntrRes1[m].Left = d1;
						}
						else
						{
							if (IntrRes1[m].Right < d1)
								IntrRes1[m].Right = d1;
						}
					}

					if (IntrRes1[m].Left < IntrRes1[m].Right)
					{
						m++;
						ValidNavs[j].ValCnt = Functions.SideDef(KKhMinDME.FromPoint, NomDir + 90.0, ptFNavPrj);
					}
				}

				m--;
				if (m < 0)
					continue;

				if (m > 0)
					ValidNavs[j].ValCnt = 0;

				ValidNavs[j].ValMax = new double[m + 1];
				ValidNavs[j].ValMin = new double[m + 1];

				for (ii = 0; ii <= m; ii++)
				{
					ValidNavs[j].ValMin[ii] = IntrRes1[ii].Left;
					ValidNavs[j].ValMax[ii] = IntrRes1[ii].Right;
				}

				if (ValidNavs[j].ValMax[0] < ValidNavs[j].ValMin[0])
					continue;

				ValidNavs[j].IntersectionType = eIntersectionType.ByDistance;

				j++;
			}

			if (j == 0)
				return new NavaidType[0];

			System.Array.Resize<NavaidType>(ref ValidNavs, j);
			return ValidNavs;
		}

		public static double CalcTA_Hpenet(ObstacleData[] PtTrnList, double hTurn, double TA_PDG, out int index, double iniH = 0)
		{
			index = -1;
			int n = PtTrnList.Length;

			if (n == 0)
				return iniH;

			double result = iniH;

			for (int i = 0; i < n; i++)
			{
				PtTrnList[i].hPenet = PtTrnList[i].Height + PtTrnList[i].MOC - hTurn - PtTrnList[i].Dist * TA_PDG;

				//if (PtTrnList[i].Dist <= MaxDist)
				{
					if (PtTrnList[i].hPenet > result)
					{
						index = i;
						result = PtTrnList[i].hPenet;
					}
				}
			}

			return result;
		}

		public static double CalcTA_PDG(ObstacleData[] PtTrnList, double hTurn, double TA_PDG, out int index)
		{
			index = -1;
			int n = PtTrnList.Length;

			if (n == 0)
				return TA_PDG;

			double result = 0.0;

			for (int i = 0; i < n; i++)
			{
				//if (PtTrnList[i].Dist <= MaxDist)
				{
					if (PtTrnList[i].hPenet > 0.0)
					{
						double PDGi = PtTrnList[i].hPenet / PtTrnList[i].Dist;
						if (result < PDGi)
						{
							index = i;
							result = PDGi;
						}
					}
				}
			}

			result = System.Math.Round(result + TA_PDG + 0.0004999, 3);
			return result;
		}

		public static double CalcCommon_PDG(ObstacleData[] PtTrnList, double StrArLen, out int index)
		{
			index = -1;
			double result = PANS_OPS_DataBase.dpPDG_Nom.Value;

			int n = PtTrnList.Length;
			if (n == 0)
				return result;

			for (int i = 0; i < n; i++)
			{
				//if (PtTrnList[i].Dist <= MaxDist)
				{
					double PDGi = (PtTrnList[i].Height + PtTrnList[i].MOC - PANS_OPS_DataBase.dpOIS_abv_DER.Value) / (StrArLen + PtTrnList[i].Dist);
					if (result < PDGi)
					{
						index = i;
						result = PDGi;
					}
				}
			}

			return result;
		}

		public static IPointCollection CalcTouchByFixDir(IPoint PtSt, IPoint PtFix, double TurnR, double DirCur,
														ref double DirFix, int TurnDir, double SnapAngle, out double dDir, out IPoint FlyBy)
		{
			IPointCollection result = new Multipoint();

			if (Functions.SubtractAngles(DirCur, DirFix) < 0.5)
			{
				DirFix = DirCur;
				if (Functions.ReturnDistanceInMeters(PtFix, PtSt) < GlobalVars.distEps)
				{
					FlyBy = new ESRI.ArcGIS.Geometry.Point();
					FlyBy.PutCoords(PtFix.X, PtFix.Y);
					result.AddPoint(PtSt);
					result.AddPoint(PtSt);
					dDir = 0.0;
					return result;
				}
			}

			IPoint ptCnt1 = Functions.PointAlongPlane(PtSt, DirCur + 90.0 * TurnDir, TurnR);
			PtSt.M = DirCur;

			double OutDir0 = NativeMethods.Modulus(DirFix - SnapAngle * TurnDir);
			double OutDir1 = NativeMethods.Modulus(DirFix + SnapAngle * TurnDir);

			IPoint Pt10 = Functions.PointAlongPlane(ptCnt1, OutDir0 - 90.0 * TurnDir, TurnR);
			IPoint Pt11 = Functions.PointAlongPlane(ptCnt1, OutDir1 - 90.0 * TurnDir, TurnR);

			int SideT = Functions.SideDef(Pt10, DirFix, PtFix);
			int SideD = Functions.SideDef(Pt10, DirFix, ptCnt1);

			IPoint Pt1;

			if (SideT * SideD < 0)
			{
				Pt1 = Pt10;
				Pt1.M = OutDir0;
			}
			else
			{
				Pt1 = Pt11;
				Pt1.M = OutDir1;
			}

			double OutDir = Pt1.M;

			FlyBy = new ESRI.ArcGIS.Geometry.Point();
			IConstructPoint Constructor = (ESRI.ArcGIS.Geometry.IConstructPoint)FlyBy;
			Constructor.ConstructAngleIntersection(Pt1, GlobalVars.DegToRadValue * OutDir, PtFix, GlobalVars.DegToRadValue * DirFix);

			//double dirToTmp = Functions.ReturnAngleInDegrees(PtFix, FlyBy);
			//double distToTmp = Functions.ReturnDistanceInMeters(PtFix, FlyBy);
			//Functions.DrawPointWithText(FlyBy, "FlyBy");
			//System.Windows.Forms.Application.DoEvents();

			SideT = AnglesSideDef(OutDir, DirFix);

			double DeltaAngle = 0;
			if (SideT > 0)
				DeltaAngle = 0.5 * NativeMethods.Modulus(180.0 + DirFix - OutDir);
			else if (SideT < 0)
				DeltaAngle = 0.5 * NativeMethods.Modulus(OutDir - 180.0 - DirFix);

			double DeltaDist = TurnR / System.Math.Tan(GlobalVars.DegToRadValue * DeltaAngle);

			double Dist = Functions.ReturnDistanceInMeters(Pt1, FlyBy);
			dDir = Dist - DeltaDist;

			IPoint Pt2, pt3;
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

			result.AddPoint(PtSt);
			result.AddPoint(Pt1);
			result.AddPoint(Pt2);
			result.AddPoint(pt3);
			return result;
		}

		public static int AnglesSideDef(double X, double Y)
		{
			double Z = NativeMethods.Modulus(X - Y, 360.0);

			if ((Z > 360.0 - GlobalVars.degEps) || (Z < GlobalVars.degEps))
				return 0;
			else if (Z > 180.0 - GlobalVars.degEps)
				return -1;
			else if (Z < 180.0 + GlobalVars.degEps)
				return 1;

			return 2;
		}

		public static IPolyline CalcTrajectoryFromMultiPoint(IPointCollection MultiPoint)
		{
			double fE = GlobalVars.DegToRadValue * 0.5;
			int n = MultiPoint.PointCount - 1;

			IPointCollection result = new Polyline();
			result.AddPoint(MultiPoint.Point[0]);

			IPoint CntPt = new ESRI.ArcGIS.Geometry.Point();
			IConstructPoint ptConstr = (ESRI.ArcGIS.Geometry.IConstructPoint)CntPt;

			for (int i = 0; i < n; i++)
			{
				IPoint FromPt = MultiPoint.Point[i];
				IPoint ToPt = MultiPoint.Point[i + 1];
				double fTmp = GlobalVars.DegToRadValue * (FromPt.M - ToPt.M);

				if (System.Math.Abs(System.Math.Sin(fTmp)) <= fE && System.Math.Cos(fTmp) > 0.0)
					result.AddPoint(ToPt);
				else
				{
					if (System.Math.Abs(System.Math.Sin(fTmp)) > fE)
						ptConstr.ConstructAngleIntersection(FromPt, GlobalVars.DegToRadValue * (FromPt.M + 90.0), ToPt, GlobalVars.DegToRadValue * (ToPt.M + 90.0));
					else
						CntPt.PutCoords(0.5 * (FromPt.X + ToPt.X), 0.5 * (FromPt.Y + ToPt.Y));

					int Side = Functions.SideDef(FromPt, FromPt.M, ToPt);
					result.AddPointCollection(CreateArcPrj(CntPt, FromPt, ToPt, -Side));
				}
			}
			return (IPolyline)result;
		}

		public static IElement DrawPoint(IPoint pPoint, int iColor = -1, bool drawFlg = true)
		{
			IRgbColor pRGB = new RgbColor();

			if (iColor != -1)
				pRGB.RGB = iColor;
			else
			{
				Random rnd = new Random();
				pRGB.Red = rnd.Next(256);
				pRGB.Green = rnd.Next(256);
				pRGB.Blue = rnd.Next(256);
			}

			ISimpleMarkerSymbol pMarkerSym = new SimpleMarkerSymbol();
			pMarkerSym.Color = pRGB;
			pMarkerSym.Size = 8;

			IMarkerElement pMarkerShpElement = (IMarkerElement)new MarkerElement();
			pMarkerShpElement.Symbol = pMarkerSym;

			IElement result = (ESRI.ArcGIS.Carto.IElement)pMarkerShpElement;
			result.Geometry = pPoint;

			if (drawFlg)
			{
				IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(result, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return result;
		}

		public static IElement DrawPointWithText(IPoint pPoint, string sText, int iColor = -1, bool drawFlg = true)
		{
			IRgbColor pRGB = new RgbColor();

			if (iColor != -1)
				pRGB.RGB = iColor;
			else
			{
				Random rnd = new Random();
				pRGB.Red = rnd.Next(256);
				pRGB.Green = rnd.Next(256);
				pRGB.Blue = rnd.Next(256);
			}

			//====
			ISimpleMarkerSymbol pMarkerSym = new SimpleMarkerSymbol();
			pMarkerSym.Color = pRGB;
			pMarkerSym.Size = 6;

			IMarkerElement pMarkerShpElement = (ESRI.ArcGIS.Carto.IMarkerElement)(new MarkerElement());
			pMarkerShpElement.Symbol = pMarkerSym;

			IElement pElementofpPoint = (ESRI.ArcGIS.Carto.IElement)pMarkerShpElement;
			pElementofpPoint.Geometry = pPoint;
			//====
			ITextSymbol pTextSymbol = new TextSymbol();
			pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
			pTextSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVABottom;

			ITextElement pTextElement = ((ESRI.ArcGIS.Carto.ITextElement)(new TextElement()));
			pTextElement.Text = sText;
			pTextElement.ScaleText = false;
			pTextElement.Symbol = pTextSymbol;

			IElement pElementOfText = (ESRI.ArcGIS.Carto.IElement)pTextElement;
			pElementOfText.Geometry = pPoint;
			//====

			IGroupElement pGroupElement = (ESRI.ArcGIS.Carto.IGroupElement)new GroupElement();
			pGroupElement.AddElement(pElementofpPoint);
			pGroupElement.AddElement((IElement)pTextElement);

			IElement pCommonElement = (ESRI.ArcGIS.Carto.IElement)pGroupElement;

			if (drawFlg)
			{
				IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(pCommonElement, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return pCommonElement;
		}

		//public static IElement DrawLine(Line pLine, int Color = -1, double Width = 1, bool drawFlg = true)
		//{
		//    ESRI.ArcGIS.Carto.IElement drawLineReturn = null;
		//    ILineElement pLineElement = null;
		//    IElement pElementOfpLine = null;
		//    ISimpleLineSymbol pLineSym = null;
		//    IRgbColor pRGB = null;
		//    IGeometry pGeometry = null;

		//    pLineElement = ((ESRI.ArcGIS.Carto.ILineElement)(new LineElement()));
		//    pElementOfpLine = ((ESRI.ArcGIS.Carto.IElement)(pLineElement));
		//    pGeometry = pLine;

		//    pElementOfpLine.Geometry = pGeometry;

		//    pRGB = new RgbColor();
		//    if (Color != -1)
		//        pRGB.RGB = Color;
		//    else
		//    {
		//        Random rnd = new Random();
		//        pRGB.Red = rnd.Next(256);
		//        pRGB.Green = rnd.Next(256);
		//        pRGB.Blue = rnd.Next(256);
		//    }


		//    pLineSym = new SimpleLineSymbol();
		//    pLineSym.Color = pRGB;
		//    pLineSym.Style = esriSimpleLineStyle.esriSLSSolid;
		//    pLineSym.Width = Width;

		//    pLineElement.Symbol = pLineSym;
		//    drawLineReturn = pElementOfpLine;

		//    IGraphicsContainer pGraphics = null;
		//    if (drawFlg)
		//    {
		//        pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
		//        pGraphics.AddElement(pElementOfpLine, 0);
		//        GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
		//    }
		//    return drawLineReturn;
		//}

		public static IElement DrawPolyline(IPolyline pLine, int iColor = -1, double Width = 1, bool drawFlg = true)
		{
			IRgbColor pRGB = new RgbColor();

			if (iColor != -1)
				pRGB.RGB = iColor;
			else
			{
				Random rnd = new Random();
				pRGB.Red = rnd.Next(256);
				pRGB.Green = rnd.Next(256);
				pRGB.Blue = rnd.Next(256);
			}

			ISimpleLineSymbol pLineSym = new SimpleLineSymbol();
			pLineSym.Color = pRGB;
			pLineSym.Style = esriSimpleLineStyle.esriSLSSolid;
			pLineSym.Width = Width;

			ILineElement pLineElement = ((ESRI.ArcGIS.Carto.ILineElement)(new LineElement()));
			pLineElement.Symbol = pLineSym;

			IElement result = (ESRI.ArcGIS.Carto.IElement)pLineElement;
			result.Geometry = (ESRI.ArcGIS.Geometry.IGeometry)pLine;

			if (drawFlg)
			{
				IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(result, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return result;
		}

		public static IElement DrawPolyline(IPointCollection pLine, int pColor = -1, double Width = 1, bool drawFlg = true)
		{
			return DrawPolyline(pLine as IPolyline, pColor, Width, drawFlg);
		}

		public static IElement DrawPolylineSFS(IPolyline pLine, ISimpleLineSymbol pLineSym, bool drawFlg = true)
		{
			ILineElement pLineElement = ((ESRI.ArcGIS.Carto.ILineElement)(new LineElement()));
			IElement result = (ESRI.ArcGIS.Carto.IElement)pLineElement;
			result.Geometry = (ESRI.ArcGIS.Geometry.IGeometry)pLine;
			pLineElement.Symbol = pLineSym;

			if (drawFlg)
			{
				IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(result, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}

			return result;
		}

		public static IElement DrawPolygon(IPolygon pPolygon, int iColor = -1, esriSimpleFillStyle SFStyle = esriSimpleFillStyle.esriSFSNull, bool drawFlg = true)
		{
			IRgbColor pRGB = new RgbColor();

			if (iColor != -1)
				pRGB.RGB = iColor;
			else
			{
				Random rnd = new Random();
				pRGB.Red = rnd.Next(256);
				pRGB.Green = rnd.Next(256);
				pRGB.Blue = rnd.Next(256);
			}

			ILineSymbol pLineSimbol = new SimpleLineSymbol();
			pLineSimbol.Color = pRGB;
			pLineSimbol.Width = 1;

			ISimpleFillSymbol pFillSym = new SimpleFillSymbol();
			pFillSym.Color = pRGB;
			pFillSym.Style = SFStyle;
			pFillSym.Outline = pLineSimbol;

			IFillShapeElement pFillShpElement = (IFillShapeElement)(new PolygonElement());
			pFillShpElement.Symbol = pFillSym;

			IElement result = (IElement)pFillShpElement;
			result.Geometry = (IGeometry)pPolygon;

			if (drawFlg)
			{
				IGraphicsContainer pGraphics = null;
				pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(result, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return result;
		}

		public static IElement DrawPolygon(IPointCollection pPolygon, int Color = -1, esriSimpleFillStyle SFStyle = esriSimpleFillStyle.esriSFSNull, bool drawFlg = true)
		{
			return DrawPolygon(pPolygon as IPolygon, Color, SFStyle, drawFlg);
		}

		public static IElement DrawPolygonSFS(IPolygon pPolygon, ISimpleFillSymbol pFillSym, bool drawFlg = true)
		{
			IFillShapeElement pFillShpElement = (ESRI.ArcGIS.Carto.IFillShapeElement)(new PolygonElement());
			IElement result = (ESRI.ArcGIS.Carto.IElement)pFillShpElement;
			result.Geometry = (ESRI.ArcGIS.Geometry.IGeometry)pPolygon;
			pFillShpElement.Symbol = pFillSym;

			if (drawFlg)
			{
				IGraphicsContainer pGraphics = GlobalVars.GetActiveView().GraphicsContainer;
				pGraphics.AddElement(result, 0);
				GlobalVars.GetActiveView().PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
			}
			return result;
		}

		public static IElement DrawPolygonSFS(IPointCollection pPolygon, ISimpleFillSymbol pFillSym, bool drawFlg = true)
		{
			return DrawPolygonSFS(pPolygon as IPolygon, pFillSym, drawFlg);
		}

		public static IPointCollection TurnToFixPrj(IPoint PtSt, double TurnR, int TurnDir, IPoint FixPnt)
		{
			IPoint ptCnt = Functions.PointAlongPlane(PtSt, PtSt.M + 90.0 * TurnDir, TurnR);
			double DistFx2Cnt = Functions.ReturnDistanceInMeters(ptCnt, FixPnt);

			// If DistFx2Cnt + distEps < TurnR Then
			//     TurnR = DistFx2Cnt
			//     Exit Function
			// End If

			// If DistFx2Cnt < TurnR Then
			//     DistFx2Cnt = TurnR
			// End If

			double DirFx2Cnt = Functions.ReturnAngleInDegrees(ptCnt, FixPnt);
			double DeltaAngle = -GlobalVars.RadToDegValue * (ArcCos(TurnR / DistFx2Cnt)) * TurnDir;

			if (double.IsNaN(DeltaAngle))
				DeltaAngle = 0.0;

			IPoint Pt1 = Functions.PointAlongPlane(ptCnt, DirFx2Cnt + DeltaAngle, TurnR);
			Pt1.M = Functions.ReturnAngleInDegrees(Pt1, FixPnt);

			IPointCollection result = new Multipoint();
			result.AddPoint(PtSt);
			result.AddPoint(Pt1);
			return result;
		}

		public static double CircleVectorIntersect(IPoint PtCent, double R, IPoint ptVect, double DirVect, out IPoint ptRes)
		{
			IPoint ptTmp = new ESRI.ArcGIS.Geometry.Point();
			IConstructPoint Constr = (ESRI.ArcGIS.Geometry.IConstructPoint)ptTmp;

			Constr.ConstructAngleIntersection(PtCent, GlobalVars.DegToRadValue * (DirVect + 90.0), ptVect, GlobalVars.DegToRadValue * DirVect);

			double DistCnt2Vect = Functions.ReturnDistanceInMeters(PtCent, ptTmp);

			if (DistCnt2Vect < R)
			{
				double d = System.Math.Sqrt(R * R - DistCnt2Vect * DistCnt2Vect);
				ptRes = Functions.PointAlongPlane(ptTmp, DirVect, d);
				return d;
			}

			ptRes = new ESRI.ArcGIS.Geometry.Point();
			return 0.0;
		}

		public static double CircleVectorIntersect(IPoint PtCent, double R, IPoint ptVect, double DirVect)
		{
			IPoint ptTmp;
			return CircleVectorIntersect(PtCent, R, ptVect, DirVect, out ptTmp);
		}

		public static double CalcTIAMaxTNAH(ObstacleData[] ObsList, double PDG, ref double Range, double iniH = 0)
		{
			double result;

			if (iniH < PANS_OPS_DataBase.dpNGui_Ar1.Value)
				result = PANS_OPS_DataBase.dpNGui_Ar1.Value;
			else
				result = iniH;

			int n = ObsList.Length;
			if (n == 0)
				return result;

			Range = (result - PANS_OPS_DataBase.dpOIS_abv_DER.Value) / PDG;

			for (int i = n - 1; i >= 0; i--)
			{
				if (ObsList[i].Dist <= Range)
				{
					double fTmp = ObsList[i].Height + PANS_OPS_DataBase.dpObsClr.Value;
					// fTmp = System.Math.Round(ObsList[I].Height + PANS_OPS_DataBase.dpObsClr.Value + 0.4999);

					if (fTmp > result)
					{
						Range = ObsList[i].Dist - 0.1;
						result = Range * PDG + PANS_OPS_DataBase.dpOIS_abv_DER.Value;
					}
				}
			}

			if (result < PANS_OPS_DataBase.dpNGui_Ar1.Value)
				result = PANS_OPS_DataBase.dpNGui_Ar1.Value;

			return result;
		}

		public static bool AngleInSector(double Angle, double X, double Y)
		{
			X = NativeMethods.Modulus(X);
			Y = NativeMethods.Modulus(Y);
			Angle = NativeMethods.Modulus(Angle);

			if (X > Y)
				if ((Angle >= X) || (Angle <= Y)) return true;

			if ((Angle >= X) && (Angle <= Y)) return true;

			return false;
		}

		public static bool AngleInInterval(double Ang, Interval inter)
		{
			if (inter.Left == -2)
				return false;

			if (inter.Right == -1)
			{
				if (System.Math.Round(inter.Left, 1) == System.Math.Round(Ang, 1))
					return true;

				return false;
			}

			inter.Left = NativeMethods.Modulus(inter.Left);
			inter.Right = NativeMethods.Modulus(inter.Right);
			Ang = NativeMethods.Modulus(Ang);

			if (inter.Left > inter.Right)
				if ((Ang >= inter.Left) || (Ang <= inter.Right))
					return true;

			if ((Ang >= inter.Left) && (Ang <= inter.Right))
				return true;

			return false;
		}

		public static IPointCollection CreateBasePoints(IPointCollection pPolygone, IPolyline K1K1, double lDepDir, int lTurnDir)
		{
			bool bFlg = false;
			int i, Side, n = pPolygone.PointCount;

			IPointCollection TmpPoly = new Polyline();
			IPointCollection result = new Polygon();
			IPoint ptTmp;

			if (lTurnDir > 0)
			{
				for (i = 0; i < n; i++)
				{
					ptTmp = pPolygone.Point[i];
					Side = Functions.SideDef(K1K1.FromPoint, lDepDir + 90.0, ptTmp);
					if (Side < 0)
					{
						if (bFlg)
							result.AddPoint(ptTmp);
						else
							TmpPoly.AddPoint(ptTmp);
					}
					else if (!bFlg)
					{
						bFlg = true;
						result.AddPoint(K1K1.FromPoint);
						result.AddPoint(K1K1.ToPoint);
					}
				}
			}
			else
			{
				for (i = n - 1; i >= 0; i--)
				{
					ptTmp = pPolygone.Point[i];
					Side = Functions.SideDef(K1K1.FromPoint, lDepDir + 90.0, ptTmp);
					if (Side < 0)
					{
						if (bFlg)
							result.AddPoint(ptTmp);
						else
							TmpPoly.AddPoint(ptTmp);
					}
					else if (!bFlg)
					{
						bFlg = true;
						result.AddPoint(K1K1.ToPoint);
						result.AddPoint(K1K1.FromPoint);
					}
				}
			}

			result.AddPointCollection(TmpPoly);
			return result;
		}

		public static IPolygon PolygonDifference(IPolygon Source, IPolygon Subtractor)
		{
			ITopologicalOperator2 pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)Source;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)Subtractor;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			try
			{
				IPolygon result = pTopo.Difference(Subtractor as IGeometry) as IPolygon;
				pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)result;
				pTopo.IsKnownSimple_2 = false;
				pTopo.Simplify();
				return result;
			}
			catch
			{
				return Subtractor;
			}
		}

		public static IPolygon PolygonIntersection(IPointCollection pPoly1, IPointCollection pPoly2)
		{
			return PolygonIntersection(pPoly1 as IPolygon, pPoly2 as IPolygon);
		}

		public static IPolygon PolygonIntersection(IPolygon pPoly1, IPointCollection pPoly2)
		{
			return PolygonIntersection(pPoly1, pPoly2 as IPolygon);
		}

		public static IPolygon PolygonIntersection(IPointCollection pPoly1, IPolygon pPoly2)
		{
			return PolygonIntersection(pPoly1 as IPolygon, pPoly2);
		}

		public static IPolygon PolygonIntersection(IPolygon pPolygon1, IPolygon pPolygon2)
		{
			IClone pClone = (IClone)pPolygon1;
			IPolygon pPoly1 = (IPolygon)pClone.Clone();

			pClone = (IClone)pPolygon2;
			IPolygon pPoly2 = (IPolygon)pClone.Clone();

			ITopologicalOperator2 pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pPoly2;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pPoly1;
			pTopo.IsKnownSimple_2 = false;
			pTopo.Simplify();

			IPolygon result;
			try
			{
				result = pTopo.Intersect(pPoly2 as IGeometry, esriGeometryDimension.esriGeometry2Dimension) as IPolygon;
				pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)result;
				pTopo.IsKnownSimple_2 = false;
				pTopo.Simplify();
			}
			catch
			{
				try
				{
					Polygon pTmpPoly0 = pTopo.Union(pPoly2 as IGeometry) as Polygon;
					Polygon pTmpPoly1 = pTopo.SymmetricDifference(pPoly2 as IGeometry) as Polygon;

					pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pTmpPoly1;
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();

					pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)pTmpPoly0;
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();
					result = pTopo.Difference(pTmpPoly1 as IGeometry) as IPolygon;

					pTopo = (ESRI.ArcGIS.Geometry.ITopologicalOperator2)result;
					pTopo.IsKnownSimple_2 = false;
					pTopo.Simplify();
				}
				catch
				{
					result = pPoly2;
				}
			}
			return result;
		}

		public static string DegToStr(double val)
		{
			return val.ToString("000.#####") + "°";
		}

		public static string ToDegString(this double val)
		{
			return val.ToString("000.#####") + "°";
		}

		public static void shall_SortfSort(ObstacleData[] obsArray)
		{
			int LastRow = obsArray.GetUpperBound(0);

			if (LastRow < 0)
				return;

			int FirstRow = obsArray.GetLowerBound(0);
			int NumRows = LastRow - FirstRow + 1;
			int GapSize = 0;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (int i = (GapSize + FirstRow); i <= LastRow; i++)
				{
					int CurrPos = i;
					ObstacleData TempVal = obsArray[i];
					while (obsArray[CurrPos - GapSize].fSort > TempVal.fSort)
					{
						obsArray[CurrPos] = obsArray[CurrPos - GapSize];
						CurrPos = CurrPos - GapSize;
						if (CurrPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurrPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void shall_SortfSortD(ObstacleData[] obsArray)
		{
			int LastRow = obsArray.GetUpperBound(0);
			if (LastRow < 0)
				return;

			int FirstRow = obsArray.GetLowerBound(0);
			int NumRows = LastRow - FirstRow + 1;
			int GapSize = 0;

			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (int i = GapSize + FirstRow; i <= LastRow; i++)
				{
					int CurrPos = i;
					ObstacleData TempVal = obsArray[i];
					while (obsArray[CurrPos - GapSize].fSort < TempVal.fSort)
					{
						obsArray[CurrPos] = obsArray[CurrPos - GapSize];
						CurrPos = CurrPos - GapSize;
						if (CurrPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurrPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void shall_SortsSort(ObstacleData[] obsArray)
		{
			int LastRow = obsArray.GetUpperBound(0);

			if (LastRow < 0)
				return;

			int FirstRow = obsArray.GetLowerBound(0);
			int NumRows = LastRow - FirstRow + 1;

			int GapSize = 0;
			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (int i = (GapSize + FirstRow); i <= LastRow; i++)
				{
					int CurrPos = i;
					ObstacleData TempVal = obsArray[i];
					while (String.Compare(obsArray[CurrPos - GapSize].sSort, TempVal.sSort) > 0)
					{
						obsArray[CurrPos] = obsArray[CurrPos - GapSize];
						CurrPos = CurrPos - GapSize;
						if (CurrPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurrPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		public static void shall_SortsSortD(ObstacleData[] obsArray)
		{

			int LastRow = obsArray.GetUpperBound(0);
			if (LastRow < 0)
				return;

			int FirstRow = obsArray.GetLowerBound(0);
			int NumRows = LastRow - FirstRow + 1;

			int GapSize = 0;
			do
				GapSize = GapSize * 3 + 1;
			while (GapSize <= NumRows);

			do
			{
				GapSize = GapSize / 3;
				for (int i = (GapSize + FirstRow); i <= LastRow; i++)
				{
					int CurrPos = i;
					ObstacleData TempVal = obsArray[i];

					while (String.Compare(obsArray[CurrPos - GapSize].sSort, TempVal.sSort) < 0)
					{
						obsArray[CurrPos] = obsArray[CurrPos - GapSize];
						CurrPos = CurrPos - GapSize;
						if (CurrPos - GapSize < FirstRow)
							break;
					}
					obsArray[CurrPos] = TempVal;
				}
			}
			while (GapSize > 1);
		}

		//static IDictionary<int, int> GetUnicalObstales(ObstacleContainer obstacleList)
		//{
		//	IDictionary<int, int> result = new Dictionary<int, int>();

		//	int i, n = obstacleList.Parts.Length;

		//	for (i = 0; i < n; i++)
		//	{
		//		int owner = obstacleList.Parts[i].Owner;
		//		if (!result.ContainsKey(owner))
		//			result.Add(owner, i);
		//	}

		//	return result;
		//}

		//public static void SortListView(int columnIndex, ListViewColumnSorter lvSorter, ListView listview)
		//{
		//	if (columnIndex != lvSorter.ColumnToSort)
		//	{
		//		listview.Columns[lvSorter.ColumnToSort].ImageIndex = 2;
		//		lvSorter.ColumnToSort = columnIndex;

		//		lvSorter.Order = SortOrder.Ascending;
		//		listview.Columns[lvSorter.ColumnToSort].ImageIndex = 0;
		//	}
		//	else if (lvSorter.Order == SortOrder.Ascending)
		//	{
		//		lvSorter.Order = SortOrder.Descending;
		//		listview.Columns[lvSorter.ColumnToSort].ImageIndex = 1;
		//	}
		//	else
		//	{
		//		lvSorter.Order = SortOrder.Ascending;
		//		listview.Columns[lvSorter.ColumnToSort].ImageIndex = 0;
		//	}

		//	listview.Sort();
		//}

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
				catch { }
			}
		}

		public static bool PriorPostFixTolerance(IPointCollection pPolygon, IPoint pPtPrj, double fDir, out double PriorDist, out double PostDist)
		{
			int I, N;
			Double fDist, fMinDist, fMaxDist;

			IPoint pCurrPt;
			IPolyline pCutterPolyline;
			IPointCollection pIntersection;
			ITopologicalOperator2 pTopological;
			IPointCollection pPoints = pPolygon as IPointCollection;

			pCutterPolyline = new Polyline() as IPolyline;
			pCutterPolyline.FromPoint = PointAlongPlane(pPtPrj, fDir, 1000000.0);
			pCutterPolyline.ToPoint = PointAlongPlane(pPtPrj, fDir + 180.0, 1000000.0);
			pTopological = pPolygon as ITopologicalOperator2;

			PriorDist = -1.0;
			PostDist = -1.0;

			try
			{
				pIntersection = pTopological.Intersect(pCutterPolyline, esriGeometryDimension.esriGeometry0Dimension) as IPointCollection;
			}
			catch
			{
				return false;
			}

			N = pIntersection.PointCount;
			if (N == 0) return false;

			pCurrPt = pIntersection.Point[0];
			fDist = ReturnDistanceInMeters(pPtPrj, pCurrPt) * SideDef(pPtPrj, fDir + 90.0, pCurrPt);
			fMinDist = fDist;
			fMaxDist = fDist;

			for (I = 1; I < N; I++)
			{
				pCurrPt = pIntersection.Point[I];
				fDist = ReturnDistanceInMeters(pPtPrj, pCurrPt) * SideDef(pPtPrj, fDir + 90.0, pCurrPt);
				if (fDist < fMinDist) fMinDist = fDist;
				if (fDist > fMaxDist) fMaxDist = fDist;
			}

			PriorDist = fMinDist;
			PostDist = fMaxDist;

			//fMinDist = Math.Abs(fMinDist)
			//fMaxDist = Math.Abs(fMaxDist)
			return true;
		}

		public static void TextBoxFloat(ref char KeyChar, string BoxText)
		{
			if (KeyChar < 32)
				return;

			char DecSep = (1.1).ToString().ToCharArray()[1];

			if (((KeyChar < '0') || KeyChar > '9') && KeyChar != DecSep)
				KeyChar = '\0';
			else if (KeyChar == DecSep)
			{
				if (BoxText.Contains(DecSep.ToString()))
					KeyChar = '\0';
			}
		}

		public static void TextBoxInteger(ref char KeyChar)
		{
			if (KeyChar < ' ')
				return;
			if ((KeyChar < '0') || (KeyChar > '9'))
				KeyChar = '\0';
		}

		public static void SetThreadLocaleByConfig()
		{
			NativeMethods.SetThreadLocale(GlobalVars.LangCode);
		}

		public static int RGB(uint red, uint green, uint blue)
		{
			return (int)(((byte)blue) << 16) | (((byte)green) << 8) | ((byte)red);
		}

		public static bool ShowSaveDialog(out string FileName, out string FileTitle)
		{
			System.Windows.Forms.SaveFileDialog SaveDialog1 = new System.Windows.Forms.SaveFileDialog();
			string ProjectPath = GlobalVars.GetMapFileName();
			int pos = ProjectPath.LastIndexOf('\\');
			int pos2 = ProjectPath.LastIndexOf('.');

			SaveDialog1.DefaultExt = "";
			SaveDialog1.InitialDirectory = ProjectPath.Substring(0, pos);
			SaveDialog1.Title = Aran.PANDA.Departure.Properties.Resources.str00511;
			SaveDialog1.FileName = ProjectPath.Substring(0, pos2 - 1) + ".htm";
			SaveDialog1.Filter = "PANDA Report File (*.htm)|*.htm|All Files (*.*)|*.*";

			FileTitle = "";
			FileName = "";

			if (SaveDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				FileName = SaveDialog1.FileName;
				FileTitle = FileName;

				pos = FileTitle.LastIndexOf('.');
				if (pos >= 0)
					FileTitle = FileTitle.Substring(0, pos);

				pos2 = FileTitle.LastIndexOf('\\');
				if (pos2 >= 0)
					FileTitle = FileTitle.Substring(pos2 + 1);

				return true;
			}

			return false;
		}

		/*
		public static void ShowSaveDialog(out string FileName, out string FileTitle)
		{
			SaveFileDialog SaveDialog1 = new SaveFileDialog();

			int pos = 0;
			int pos2 = 0;
			string ProjectPath = null;

			FileTitle = "";

			ProjectPath = GlobalVars.GetMapFileName();

			pos = Strings.InStrRev(ProjectPath, @"\", -1, (Microsoft.VisualBasic.CompareMethod)(0));
			pos2 = Strings.InStrRev(ProjectPath, ".", -1, (Microsoft.VisualBasic.CompareMethod)(0));

			SaveDialog1.DefaultExt = "";
			SaveDialog1.InitialDirectory = ProjectPath.Substring(0, pos);
			SaveDialog1.Title = Resources.str511;
			SaveDialog1.FileName = ProjectPath.Substring(0, pos2 - 1) + ".htm";

			if (SaveDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				FileName = SaveDialog1.FileName;

				pos = Strings.InStrRev(FileName, ".", -1, (Microsoft.VisualBasic.CompareMethod)(0));
				if ((pos > 0))
					FileName = FileName.Substring(0, pos - 1);

				FileTitle = FileName;
				pos2 = Strings.InStrRev(FileTitle, @"\", -1, (Microsoft.VisualBasic.CompareMethod)(0));//InStrRev(ProjectPath, "\")
				if (pos2 > 0)
					FileTitle = FileTitle.Substring(pos2);

				//FileTitle = SaveDialog1.FileName;
				//pos2 = Strings.InStrRev(FileTitle, ".", -1, (Microsoft.VisualBasic.CompareMethod)(0));
				//if ((pos2 > 0))
				//	FileTitle = FileTitle.Substring(0, pos2 - 1);
			}
		}
		*/

		//private static bool _errorHandled = false;
		//public static void HandleThreadException()
		//{
		//    if (_errorHandled)
		//        return;
		//    _errorHandled = true;
		//    System.Windows.Forms.Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Functions.Application_ThreadException);
		//}

		public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			System.Windows.Forms.MessageBox.Show(e.Exception.Message, "Error",
				System.Windows.Forms.MessageBoxButtons.OK,
				System.Windows.Forms.MessageBoxIcon.Error);
		}

		//public static void SaveMxDocument(string FileName, bool ClearGraphics)
		//{
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
		//}
	}
}

