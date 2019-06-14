using System;
using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
//using ESRI.ArcGIS.Geometry;

using Aran.Panda.Common;
using Aran.Geometries;
using Aran.Geometries.Operators;
using Aran.Geometries.SpatialReferences;
using Aran.Panda.Constants;

namespace Aran.Panda.EnrouteStar
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

		public static void InitModule()
		{
			int I, N, J;
			int NavaidTypesCount;
			int iBaseName, iNavTypeName, iParam_Name, iValue, iUnit, iMultiplier;
			double Value, Multiplier;
			string ParamName;

			ITable pNavaidTable;
			IRow pRow;

			DBModule.OpenTableFromFile(out NavaidTypes, GlobalVars.InstallDir + @"\Navaids\", "Navaids");

			iBaseName = NavaidTypes.FindField("BaseName");
			iNavTypeName = NavaidTypes.FindField("Name");

			NavaidTypesCount = NavaidTypes.RowCount(null);

			for (J = 0; J <= NavaidTypesCount - 1; J++)
			{
				DBModule.OpenTableFromFile(out pNavaidTable, GlobalVars.InstallDir + @"\Navaids\",
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
							ParamName = System.Convert.ToString(pRow.get_Value(iParam_Name));
							Multiplier = System.Convert.ToDouble(pRow.get_Value(iMultiplier));
							Value = System.Convert.ToDouble(pRow.get_Value(iValue)) * Multiplier;
							if (pRow.get_Value(iUnit).ToString() == "rad" || pRow.get_Value(iUnit).ToString() == "°")
								Value = Math.Round(ARANMath.RadToDegValue * Value, 1);

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
							ParamName = System.Convert.ToString(pRow.get_Value(iParam_Name));
							Multiplier = System.Convert.ToDouble(pRow.get_Value(iMultiplier));
							Value = System.Convert.ToDouble(pRow.get_Value(iValue)) * Multiplier;

							if (pRow.get_Value(iUnit).ToString() == "rad" || pRow.get_Value(iUnit).ToString() == "°")
								Value = Math.Round(ARANMath.RadToDegValue * Value, 1);

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
							ParamName = System.Convert.ToString(pRow.get_Value(iParam_Name));
							Multiplier = System.Convert.ToDouble(pRow.get_Value(iMultiplier));
							Value = System.Convert.ToDouble(pRow.get_Value(iValue)) * Multiplier;

							if (pRow.get_Value(iUnit).ToString() == "rad" || pRow.get_Value(iUnit).ToString() == "°")
								Value = Math.Round(ARANMath.RadToDegValue * Value, 1);

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
							ParamName = System.Convert.ToString(pRow.get_Value(iParam_Name));
							Multiplier = System.Convert.ToDouble(pRow.get_Value(iMultiplier));
							Value = System.Convert.ToDouble(pRow.get_Value(iValue)) * Multiplier;

							if (pRow.get_Value(iUnit).ToString() == "rad" || pRow.get_Value(iUnit).ToString() == "°")
								Value = Math.Round(ARANMath.RadToDegValue * Value, 1);

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

		public static double VORFIXTolerArea(Point ptVor, double Aztin, double AbsH, out MultiPolygon TolerArea)
		{
			double fTmp;
			double fTmpH = AbsH - ptVor.Z;
			double R = fTmpH * System.Math.Tan(ARANMath.DegToRad(VOR.ConeAngle));
			Point ptTmp;
			Point ptCur;

			Polygon tmpPoly = new Polygon();
			Ring tmpRing = new Ring();
			TolerArea = new MultiPolygon();

			ptTmp = ARANFunctions.PointAlongPlane(ptVor, Aztin - (ARANMath.C_PI_2 + VOR.TrackAccuracy), VOR.LateralDeviationCoef * fTmpH);
			ptCur = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, Aztin - VOR.TrackAccuracy, out fTmp);
			tmpRing.Add(ptCur);

			ptCur = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, 180.0 + Aztin - VOR.TrackAccuracy, out fTmp);
			tmpRing.Add(ptCur);

			ptTmp = ARANFunctions.PointAlongPlane(ptVor, Aztin + 90.0 + VOR.TrackAccuracy, VOR.LateralDeviationCoef * fTmpH);
			ptCur = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, 180.0 + Aztin + VOR.TrackAccuracy, out fTmp);
			tmpRing.Add(ptCur);

			ptCur = ARANFunctions.CircleVectorIntersect(ptVor, R, ptTmp, Aztin + VOR.TrackAccuracy, out fTmp);
			tmpRing.Add(ptCur);

			tmpPoly.ExteriorRing =  tmpRing;
			TolerArea.Add(tmpPoly);

			return R;
		}

		public static double NDBFIXTolerArea(Point ptNDB, double Aztin, double AbsH, out MultiPolygon TolerArea)
		{
			double fTmp;
			double fTmpH = AbsH - ptNDB.Z;
			double R = fTmpH * System.Math.Tan(ARANMath.DegToRad(NDB.ConeAngle));
			double qN = R * System.Math.Sin(ARANMath.DegToRad(NDB.Entry2ConeAccuracy));

			Point ptTmp;
			Point ptCur;

			Polygon tmpPoly = new Polygon();
			Ring tmpRing = new Ring();
			TolerArea = new MultiPolygon();

			ptTmp = ARANFunctions.PointAlongPlane(ptNDB, Aztin - 90.0, qN + System.Math.Sqrt(R * R - qN * qN) * System.Math.Tan(ARANMath.DegToRad(NDB.TrackAccuracy)));
			ptCur = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, Aztin - NDB.TrackAccuracy, out fTmp);
			tmpRing.Add(ptCur);

			ptCur = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, 180.0 + Aztin - NDB.TrackAccuracy, out fTmp);
			tmpRing.Add(ptCur);

			ptTmp = ARANFunctions.PointAlongPlane(ptNDB, Aztin + 90.0, qN + System.Math.Sqrt(R * R - qN * qN) * System.Math.Tan(ARANMath.DegToRad(NDB.TrackAccuracy)));
			ptCur = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, 180.0 + Aztin + NDB.TrackAccuracy, out fTmp);
			tmpRing.Add(ptCur);

			ptCur = ARANFunctions.CircleVectorIntersect(ptNDB, R, ptTmp, Aztin + NDB.TrackAccuracy, out fTmp);
			tmpRing.Add(ptCur);

			tmpPoly.ExteriorRing =  tmpRing;
			TolerArea.Add(tmpPoly);

			return R;
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

			//Res.TypeName_Renamed = Val_Renamed.TypeName_Renamed;
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

			//Res.TypeName_Renamed = Val_Renamed.TypeName_Renamed
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
			//Res.TypeName_Renamed = pWPT.TypeName_Renamed

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
			//'TurnNav.TypeName_Renamed = pWPT.TypeName_Renamed

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
