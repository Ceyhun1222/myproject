using ESRI.ArcGIS.Geodatabase;
//using ESRI.ArcGIS.Geometry;
//using Microsoft.VisualBasic;
//using System.Drawing;

using System;
using System.Collections;
using System.Diagnostics;

using System.Windows;
using System.Windows.Forms;

using Aran.Geometries;
using Aran.Panda.Common;

namespace Aran.PANDA.RNAV.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class Navaids_DataBase
	{
		public static ITable NavaidTypes;

		public static VORData VOR = new VORData();
		public static NDBData NDB = new NDBData();
		public static DMEData DME = new DMEData();
		public static LLZData LLZ = new LLZData();

		public static string[] NavTypeNames = new string[] { "VOR", "DME", "NDB", "LLZ", "TACAN", "Radar FIX" };

		public static string GetNavTypeName(eNavaidType navType)
		{
			if (navType == eNavaidType.CodeNONE)
				return "WPT";
			else
				return NavTypeNames[(int)navType];
		}

		public static void OpenTableFromFile(ref ESRI.ArcGIS.Geodatabase.ITable pTable, string sFolderName, string sFileName)
		{
			ESRI.ArcGIS.Geodatabase.IWorkspaceFactory pFact = new ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactory();
			ESRI.ArcGIS.Geodatabase.IWorkspace pWorkspace = pFact.OpenFromFile(sFolderName, GlobalVars.GetApplicationHWnd());
			pTable = ((ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)(pWorkspace)).OpenTable(sFileName);
		}

		public static void InitModule()
		{
			int I = 0;
			int N = 0;
			int J = 0;
			int NavaidTypesCount = 0;
			int iBaseName = 0;
			int iNavTypeName = 0;
			int iParam_Name = 0;
			int iValue = 0;
			int iUnit = 0;
			int iMultiplier = 0;

			double Value = 0;
			double Multiplier = 0;
			string ParamName = null;

			ITable pNavaidTable = null;
			IRow pRow = null;

			OpenTableFromFile(ref NavaidTypes, GlobalVars.InstallDir + @"\constants\Navaids\", "Navaids");

			iBaseName = NavaidTypes.FindField("BaseName");
			iNavTypeName = NavaidTypes.FindField("Name");

			NavaidTypesCount = NavaidTypes.RowCount(null);

			for (J = 0; J <= NavaidTypesCount - 1; J++)
			{
				OpenTableFromFile(ref pNavaidTable, GlobalVars.InstallDir + @"\constants\Navaids\",
					NavaidTypes.GetRow(J).get_Value(iBaseName).ToString());

				string sNavTypeName = NavaidTypes.GetRow(J).get_Value(iNavTypeName).ToString();
				switch (sNavTypeName)
				{
					case "VOR":
						iParam_Name = -1;
						iValue = -1;
						iUnit = -1;
						iMultiplier = -1;

						for (I = 0; I <= pNavaidTable.Fields.FieldCount - 1; I++)
						{
							if (pNavaidTable.Fields.get_Field(I).Name == "PARAM_NAME")
								iParam_Name = I;

							if (pNavaidTable.Fields.get_Field(I).Name == "VALUE")
								iValue = I;

							if (pNavaidTable.Fields.get_Field(I).Name == "UNIT")
								iUnit = I;

							if (pNavaidTable.Fields.get_Field(I).Name == "MULTIPLIER")
								iMultiplier = I;

						}

						N = pNavaidTable.RowCount(null);

						for (I = 0; I <= N - 1; I++)
						{
							pRow = pNavaidTable.GetRow(I);
							ParamName = Convert.ToString(pRow.get_Value(iParam_Name));
							Multiplier = Convert.ToDouble(pRow.get_Value(iMultiplier));
							Value = Convert.ToDouble(pRow.get_Value(iValue)) * Multiplier;
							if (pRow.get_Value(iUnit).ToString() == "rad" || pRow.get_Value(iUnit).ToString() == "°")
								Value = Math.Round(GlobalVars.RadToDegValue * Value, 1);

							if (ParamName == "Range")
								VOR.Range = Value;

							if (ParamName == "FA Range")
								VOR.FA_Range = Value;

							if (ParamName == "Initial width")
								VOR.InitWidth = Value;

							if (ParamName == "Splay angle")
								VOR.SplayAngle = Value;

							if (ParamName == "Tracking tolerance")
								VOR.TrackingTolerance = Value;

							if (ParamName == "Intersecting tolerance")
								VOR.IntersectingTolerance = Value;

							if (ParamName == "Cone angle")
								VOR.ConeAngle = Value;

							if (ParamName == "Track accuracy")
								VOR.TrackAccuracy = Value;

							if (ParamName == "Lateral deviation coef.")
								VOR.LateralDeviationCoef = Value;

							if (ParamName == "EnRoute Tracking toler")
								VOR.EnRouteTrackingToler = Value;

							if (ParamName == "EnRoute Tracking2 toler")
								VOR.EnRouteTracking2Toler = Value;

							if (ParamName == "EnRoute Inter toler")
								VOR.EnRouteInterToler = Value;

							if (ParamName == "EnRoute PrimArea With")
								VOR.EnRoutePrimAreaWith = Value;

							if (ParamName == "EnRoute SecArea With")
								VOR.EnRouteSecAreaWith = Value;
						}
						// ============================ NDB ====================================
						break;
					case "NDB":
						iParam_Name = -1;
						iValue = -1;
						iUnit = -1;
						iMultiplier = -1;

						for (I = 0; I <= pNavaidTable.Fields.FieldCount - 1; I++)
						{
							if (pNavaidTable.Fields.get_Field(I).Name == "PARAM_NAME")
								iParam_Name = I;

							if (pNavaidTable.Fields.get_Field(I).Name == "VALUE")
								iValue = I;

							if (pNavaidTable.Fields.get_Field(I).Name == "UNIT")
								iUnit = I;

							if (pNavaidTable.Fields.get_Field(I).Name == "MULTIPLIER")
								iMultiplier = I;

						}

						N = pNavaidTable.RowCount(null);

						for (I = 0; I <= N - 1; I++)
						{
							pRow = pNavaidTable.GetRow(I);
							ParamName = Convert.ToString(pRow.get_Value(iParam_Name));
							Multiplier = Convert.ToDouble(pRow.get_Value(iMultiplier));
							Value = Convert.ToDouble(pRow.get_Value(iValue)) * Multiplier;

							if (pRow.get_Value(iUnit).ToString() == "rad" || pRow.get_Value(iUnit).ToString() == "°")
								Value = Math.Round(GlobalVars.RadToDegValue * Value, 1);

							if (ParamName == "Range")
								NDB.Range = Value;

							if (ParamName == "FA Range")
								NDB.FA_Range = Value;

							if (ParamName == "Initial width")
								NDB.InitWidth = Value;

							if (ParamName == "Splay angle")
								NDB.SplayAngle = Value;

							if (ParamName == "Tracking tolerance")
								NDB.TrackingTolerance = Value;

							if (ParamName == "Intersecting tolerance")
								NDB.IntersectingTolerance = Value;

							if (ParamName == "Cone angle")
								NDB.ConeAngle = Value;

							if (ParamName == "Track accuracy")
								NDB.TrackAccuracy = Value;

							if (ParamName == "Entry into the cone accuracy")
								NDB.Entry2ConeAccuracy = Value;

							if (ParamName == "Lateral deviation coef.")
								NDB.LateralDeviationCoef = Value;

							if (ParamName == "EnRoute Tracking toler")
								NDB.EnRouteTrackingToler = Value;

							if (ParamName == "EnRoute Tracking2 toler")
								NDB.EnRouteTracking2Toler = Value;

							if (ParamName == "EnRoute Inter toler")
								NDB.EnRouteInterToler = Value;

							if (ParamName == "EnRoute PrimArea With")
								NDB.EnRoutePrimAreaWith = Value;

							if (ParamName == "EnRoute SecArea With")
								NDB.EnRouteSecAreaWith = Value;
						}
						// ============================ DME ====================================
						break;
					case "DME":
						iParam_Name = -1;
						iValue = -1;
						iUnit = -1;
						iMultiplier = -1;

						for (I = 0; I <= pNavaidTable.Fields.FieldCount - 1; I++)
						{
							if (pNavaidTable.Fields.get_Field(I).Name == "PARAM_NAME")
								iParam_Name = I;

							if (pNavaidTable.Fields.get_Field(I).Name == "VALUE")
								iValue = I;

							if (pNavaidTable.Fields.get_Field(I).Name == "UNIT")
								iUnit = I;

							if (pNavaidTable.Fields.get_Field(I).Name == "MULTIPLIER")
								iMultiplier = I;

						}

						N = pNavaidTable.RowCount(null);

						for (I = 0; I <= N - 1; I++)
						{
							pRow = pNavaidTable.GetRow(I);
							ParamName = Convert.ToString(pRow.get_Value(iParam_Name));
							Multiplier = Convert.ToDouble(pRow.get_Value(iMultiplier));
							Value = Convert.ToDouble(pRow.get_Value(iValue)) * Multiplier;

							if (pRow.get_Value(iUnit).ToString() == "rad" || pRow.get_Value(iUnit).ToString() == "°")
								Value = Math.Round(GlobalVars.RadToDegValue * Value, 1);

							if (ParamName == "Range")
								DME.Range = Value;

							if (ParamName == "Minimal Error")
								DME.MinimalError = Value;

							if (ParamName == "Error Scaling Up")
								DME.ErrorScalingUp = Value;

							if (ParamName == "Slant Angle")
								DME.SlantAngle = Value;

							if (ParamName == "TP_div")
								DME.TP_div = Value;
						}
						// ============================ LLZ ====================================
						break;
					case "LLZ":
						iParam_Name = -1;
						iValue = -1;
						iUnit = -1;
						iMultiplier = -1;

						for (I = 0; I <= pNavaidTable.Fields.FieldCount - 1; I++)
						{
							if (pNavaidTable.Fields.get_Field(I).Name == "PARAM_NAME")
								iParam_Name = I;

							if (pNavaidTable.Fields.get_Field(I).Name == "VALUE")
								iValue = I;

							if (pNavaidTable.Fields.get_Field(I).Name == "UNIT")
								iUnit = I;

							if (pNavaidTable.Fields.get_Field(I).Name == "MULTIPLIER")
								iMultiplier = I;
						}

						N = pNavaidTable.RowCount(null);

						for (I = 0; I <= N - 1; I++)
						{
							pRow = pNavaidTable.GetRow(I);
							ParamName = Convert.ToString(pRow.get_Value(iParam_Name));
							Multiplier = Convert.ToDouble(pRow.get_Value(iMultiplier));
							Value = Convert.ToDouble(pRow.get_Value(iValue)) * Multiplier;

							if (pRow.get_Value(iUnit).ToString() == "rad" || pRow.get_Value(iUnit).ToString() == "°")
								Value = Math.Round(GlobalVars.RadToDegValue * Value, 1);

							if (ParamName == "Range")
								LLZ.Range = Value;

							if (ParamName == "Tracking tolerance")
								LLZ.TrackingTolerance = Value;

							if (ParamName == "Intersecting tolerance")
								LLZ.IntersectingTolerance = Value;
						}
						// ============================ FIN ====================================
						break;
				}
			}
		}

		public static int NavNameToID(string Name)
		{
			for (int i = 0; i < GlobalVars.NavaidList.Length; i++)
				if ((GlobalVars.NavaidList[i].Name == Name))
					return i;

			return -1;
		}

		public static double VORFIXTolerArea(Point ptVor, double Aztin, double AbsH, out Polygon TolerArea)
		{
			double vORFIXTolerAreaReturn;

			Point ptCur;
			double fTmp;
			double fTmpH = AbsH - ptVor.Z;
			double R = fTmpH * Math.Tan(VOR.ConeAngle);

			TolerArea = new Polygon();
			TolerArea.ExteriorRing = new Ring();

			Point ptTmp = ARANFunctions.PointAlongPlane(ptVor, Aztin - (ARANMath.C_PI_2 + VOR.TrackAccuracy), VOR.LateralDeviationCoef * fTmpH);

			ptCur = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, Aztin - VOR.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCur);
			ptCur = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, 180.0 + Aztin - VOR.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCur);
			ptTmp = ARANFunctions.PointAlongPlane(ptVor, Aztin + 90.0 + VOR.TrackAccuracy, VOR.LateralDeviationCoef * fTmpH);
			ptCur = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, 180.0 + Aztin + VOR.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCur);

			ptCur = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, Aztin + VOR.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCur);

			vORFIXTolerAreaReturn = R;
			return vORFIXTolerAreaReturn;
		}

		public static double NDBFIXTolerArea(Point ptNDB, double Aztin, double AbsH, out Polygon TolerArea)
		{
			double nDBFIXTolerAreaReturn;
			double R ;
			double qN ;
			Point ptTmp;
			Point ptCur;
			double fTmp;
			double fTmpH;

			fTmpH = AbsH - ptNDB.Z;
			R = fTmpH * Math.Tan(NDB.ConeAngle);

			TolerArea = new Polygon();
			TolerArea.ExteriorRing = new Ring();

			qN = R * Math.Sin(ARANMath.DegToRad(NDB.Entry2ConeAccuracy));
			ptTmp = ARANFunctions.PointAlongPlane(ptNDB, Aztin - ARANMath.C_PI_2, qN + Math.Sqrt(R * R - qN * qN) * Math.Tan(NDB.TrackAccuracy));
			ptCur = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, Aztin - NDB.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCur);
			ptCur = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, 180.0 + Aztin - NDB.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCur);
			ptTmp = ARANFunctions.PointAlongPlane(ptNDB, Aztin + 90.0, qN + Math.Sqrt(R * R - qN * qN) * Math.Tan(NDB.TrackAccuracy));
			ptCur = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, 180.0 + Aztin + NDB.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCur);
			ptCur = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, Aztin + NDB.TrackAccuracy, out fTmp);
			TolerArea.ExteriorRing.Add(ptCur);
			nDBFIXTolerAreaReturn = R;
			return nDBFIXTolerAreaReturn;
		}

		public static FIXableNavaidType NavaidType2FixableNavaidType(NavaidType val)
		{
			FIXableNavaidType result = new FIXableNavaidType();

			result.pPtGeo = val.pPtGeo;
			result.pPtPrj = val.pPtPrj;
			result.pFeature = val.pFeature;

			result.Identifier = val.Identifier;
			result.Name = val.Name;
			result.CallSign = val.CallSign;
			result.MagVar = val.MagVar;

			result.TypeCode = val.TypeCode;
			result.Range = val.Range;
			result.index = val.index;
			result.PairNavaidIndex = val.PairNavaidIndex;


			result.PairNavaidType = val.PairNavaidType;
			result.Tag = val.Tag;
			//Res.Front;
			//Res.Dist;
			//Res.CLShift;
			//Res.ValCnt;
			//Res.ValMin;
			//Res.ValMax;

			return result;
		}

		public static NavaidType FixableNavaidType2NavaidType(FIXableNavaidType val)
		{
			NavaidType result = new NavaidType();

			result.pPtGeo = val.pPtGeo;
			result.pPtPrj = val.pPtPrj;
			result.pFeature = val.pFeature;

			result.Identifier = val.Identifier;
			result.Name = val.Name;
			result.CallSign = val.CallSign;
			result.MagVar = val.MagVar;

			result.TypeCode = val.TypeCode;
			result.Range = val.Range;
			result.index = val.index;
			result.PairNavaidIndex = val.PairNavaidIndex;
			result.PairNavaidType = val.PairNavaidType;

			result.index = val.index;

			//Res.Course = Val_Renamed.Course;
			//Res.GPAngle = Val.GPAngle
			//Res.GP_RDH = Val_Renamed.GP_RDH;
			//Res.LLZ_THR = Val_Renamed.LLZ_THR;
			//Res.SecWidth = Val_Renamed.SecWidth;
			//Res.AngleWidth = Val.AngleWidth
			result.Tag = val.Tag;

			return result;
		}

		public static FIXableNavaidType WPT_FIXToFixableNavaid(WPT_FIXType val)
		{
			FIXableNavaidType result = new FIXableNavaidType();

			result.pPtGeo = val.pPtGeo;
			result.pPtPrj = val.pPtPrj;
			result.pFeature = val.pFeature;

			result.Identifier = val.Identifier;
			result.Name = val.Name;
			result.CallSign = val.CallSign;

			result.TypeCode = val.TypeCode;
			result.MagVar = val.MagVar;

			if (val.TypeCode == eNavaidType.CodeVOR)
				result.Range = VOR.Range;
			else if (val.TypeCode == eNavaidType.CodeNDB)
				result.Range = NDB.Range;
			else if (val.TypeCode == eNavaidType.CodeDME)
				result.Range = DME.Range;
			else
				result.Range = LLZ.Range;


			result.index = -1;
			result.PairNavaidIndex = -1;

			//Res.GP_RDH = 0;
			//Res.Course = 0;
			//Res.LLZ_THR = 0;
			//Res.SecWidth = 0;

			result.Tag = val.Tag;
			result.ValCnt = -2;
			return result;
		}

		public static NavaidType WPT_FIXToNavaid(WPT_FIXType val)
		{
			NavaidType result = new NavaidType();

			result.pPtGeo = val.pPtGeo;
			result.pPtPrj = val.pPtPrj;
			result.pFeature = val.pFeature;

			result.Identifier = val.Identifier;
			result.Name = val.Name;
			result.CallSign = val.CallSign;

			result.TypeCode = val.TypeCode;
			result.MagVar = val.MagVar;

			if (val.TypeCode == eNavaidType.CodeVOR)
				result.Range = VOR.Range;
			else if (val.TypeCode == eNavaidType.CodeNDB)
				result.Range = NDB.Range;
			else if (val.TypeCode == eNavaidType.CodeDME)
				result.Range = DME.Range;
			else
				result.Range = LLZ.Range;

			result.index = -1;
			result.PairNavaidIndex = -1;
			result.Tag = val.Tag;

			return result;
		}
	}
}
