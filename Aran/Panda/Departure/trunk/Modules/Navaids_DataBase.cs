using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eNavaidType
	{
		NONE = -1,
		VOR = 0,
		DME = 1,
		NDB = 2,
		LLZ = 3,
		TACAN = 4,
		//StartPoint = 5
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class Navaids_DataBase
	{
		public static ITable NavaidTypes;

		public static VORData VOR = new VORData();
		public static NDBData NDB = new NDBData();
		public static DMEData DME = new DMEData();
		public static LLZData LLZ = new LLZData();

		public static string[] NavTypeNames = new string[] { "VOR", "DME", "NDB", "LOC", "TACAN", "Radar FIX" };

		public static string GetNavTypeName(eNavaidType navType)
		{
			if (navType == eNavaidType.NONE)
				return "WPT";
			else
				return NavTypeNames[(int)navType];
		}

		public static void InitModule()
		{
			int i, j, n;
			int NavaidTypesCount,
				iBaseName, iNavTypeName, iParam_Name, iValue, iUnit, iMultiplier;

			double Value, Multiplier;
			string ParamName;

			ITable pNavaidTable;
			IRow pRow;

			NavaidTypes = FileRoutines.OpenTableFromFile(GlobalVars.ConstDir + @"\Navaids\", "Navaids");

			iBaseName = NavaidTypes.FindField("BaseName");
			iNavTypeName = NavaidTypes.FindField("Name");

			NavaidTypesCount = NavaidTypes.RowCount(null);

			for (j = 0; j < NavaidTypesCount; j++)
			{
				pNavaidTable = FileRoutines.OpenTableFromFile(GlobalVars.ConstDir + @"\Navaids\",
					NavaidTypes.GetRow(j).get_Value(iBaseName).ToString());

				string sNavTypeName = NavaidTypes.GetRow(j).get_Value(iNavTypeName).ToString();
				switch (sNavTypeName)
				{
					case "VOR":
						iParam_Name = -1;
						iValue = -1;
						iUnit = -1;
						iMultiplier = -1;

						for (i = 0; i < pNavaidTable.Fields.FieldCount; i++)
						{
							if (pNavaidTable.Fields.get_Field(i).Name == "PARAM_NAME")
								iParam_Name = i;

							if (pNavaidTable.Fields.get_Field(i).Name == "VALUE")
								iValue = i;

							if (pNavaidTable.Fields.get_Field(i).Name == "UNIT")
								iUnit = i;

							if (pNavaidTable.Fields.get_Field(i).Name == "MULTIPLIER")
								iMultiplier = i;

						}

						n = pNavaidTable.RowCount(null);

						for (i = 0; i < n; i++)
						{
							pRow = pNavaidTable.GetRow(i);
							ParamName = pRow.Value[iParam_Name].ToString();
							Multiplier = System.Convert.ToDouble(pRow.Value[iMultiplier]);
							Value = System.Convert.ToDouble(pRow.Value[iValue]) * Multiplier;

							if (pRow.Value[iUnit].ToString() == "rad" || pRow.Value[iUnit].ToString() == "°")
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

						for (i = 0; i < pNavaidTable.Fields.FieldCount; i++)
						{
							if (pNavaidTable.Fields.get_Field(i).Name == "PARAM_NAME")
								iParam_Name = i;

							if (pNavaidTable.Fields.get_Field(i).Name == "VALUE")
								iValue = i;

							if (pNavaidTable.Fields.get_Field(i).Name == "UNIT")
								iUnit = i;

							if (pNavaidTable.Fields.get_Field(i).Name == "MULTIPLIER")
								iMultiplier = i;

						}

						n = pNavaidTable.RowCount(null);

						for (i = 0; i < n; i++)
						{
							pRow = pNavaidTable.GetRow(i);
							ParamName = pRow.Value[iParam_Name].ToString();
							Multiplier = System.Convert.ToDouble(pRow.Value[iMultiplier]);
							Value = System.Convert.ToDouble(pRow.Value[iValue]) * Multiplier;

							if (pRow.Value[iUnit].ToString() == "rad" || pRow.Value[iUnit].ToString() == "°")
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

						for (i = 0; i < pNavaidTable.Fields.FieldCount; i++)
						{
							if (pNavaidTable.Fields.get_Field(i).Name == "PARAM_NAME")
								iParam_Name = i;

							if (pNavaidTable.Fields.get_Field(i).Name == "VALUE")
								iValue = i;

							if (pNavaidTable.Fields.get_Field(i).Name == "UNIT")
								iUnit = i;

							if (pNavaidTable.Fields.get_Field(i).Name == "MULTIPLIER")
								iMultiplier = i;

						}

						n = pNavaidTable.RowCount(null);

						for (i = 0; i < n; i++)
						{
							pRow = pNavaidTable.GetRow(i);
							ParamName = pRow.Value[iParam_Name].ToString();
							Multiplier = System.Convert.ToDouble(pRow.Value[iMultiplier]);
							Value = System.Convert.ToDouble(pRow.Value[iValue]) * Multiplier;

							if (pRow.Value[iUnit].ToString() == "rad" || pRow.Value[iUnit].ToString() == "°")
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

						for (i = 0; i < pNavaidTable.Fields.FieldCount; i++)
						{
							if (pNavaidTable.Fields.get_Field(i).Name == "PARAM_NAME")
								iParam_Name = i;

							if (pNavaidTable.Fields.get_Field(i).Name == "VALUE")
								iValue = i;

							if (pNavaidTable.Fields.get_Field(i).Name == "UNIT")
								iUnit = i;

							if (pNavaidTable.Fields.get_Field(i).Name == "MULTIPLIER")
								iMultiplier = i;
						}

						n = pNavaidTable.RowCount(null);

						for (i = 0; i < n; i++)
						{
							pRow = pNavaidTable.GetRow(i);
							ParamName = pRow.Value[iParam_Name].ToString();
							Multiplier = System.Convert.ToDouble(pRow.Value[iMultiplier]);
							Value = System.Convert.ToDouble(pRow.Value[iValue]) * Multiplier;

							if (pRow.Value[iUnit].ToString() == "rad" || pRow.Value[iUnit].ToString() == "°")
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

		//public static int NavNameToID(string Name)
		//{
		//    for (int i = 0; i < GlobalVars.NavaidList.Length; i++)
		//        if (GlobalVars.NavaidList[i].Name == Name)
		//            return i;

		//    return -1;
		//}

		public static double VORFIXTolerArea(IPoint ptVor, double Aztin, double AbsH, out IPointCollection TolerArea)
		{
			double vORFIXTolerAreaReturn = 0;
			double R = 0;
			IPoint ptTmp = null;
			IPoint ptCur = null;
			double fTmp = 0;
			double fTmpH = 0;

			fTmpH = AbsH - ptVor.Z;
			R = fTmpH * System.Math.Tan(Functions.DegToRad(VOR.ConeAngle));

			TolerArea = new Polygon();
			ptTmp = Functions.PointAlongPlane(ptVor, Aztin - (90.0 + VOR.TrackAccuracy), VOR.LateralDeviationCoef * fTmpH);
			fTmp = Functions.CircleVectorIntersect(ptVor, R, ptTmp, Aztin - VOR.TrackAccuracy, out ptCur);
			TolerArea.AddPoint(ptCur);
			fTmp = Functions.CircleVectorIntersect(ptVor, R, ptTmp, 180.0 + Aztin - VOR.TrackAccuracy, out ptCur);
			TolerArea.AddPoint(ptCur);
			ptTmp = Functions.PointAlongPlane(ptVor, Aztin + 90.0 + VOR.TrackAccuracy, VOR.LateralDeviationCoef * fTmpH);
			fTmp = Functions.CircleVectorIntersect(ptVor, R, ptTmp, 180.0 + Aztin + VOR.TrackAccuracy, out ptCur);
			TolerArea.AddPoint(ptCur);

			fTmp = Functions.CircleVectorIntersect(ptVor, R, ptTmp, Aztin + VOR.TrackAccuracy, out ptCur);
			TolerArea.AddPoint(ptCur);

			vORFIXTolerAreaReturn = R;
			return vORFIXTolerAreaReturn;
		}

		public static double NDBFIXTolerArea(IPoint ptNDB, double Aztin, double AbsH, out IPointCollection TolerArea)
		{
			double nDBFIXTolerAreaReturn = 0;
			double R = 0;
			double qN = 0;
			IPoint ptTmp = null;
			IPoint ptCur = null;
			double fTmp = 0;
			double fTmpH = 0;

			fTmpH = AbsH - ptNDB.Z;
			R = fTmpH * System.Math.Tan(Functions.DegToRad(NDB.ConeAngle));

			TolerArea = new Polygon();

			qN = R * System.Math.Sin(Functions.DegToRad(NDB.Entry2ConeAccuracy));
			ptTmp = Functions.PointAlongPlane(ptNDB, Aztin - 90.0, qN + System.Math.Sqrt(R * R - qN * qN) * System.Math.Tan(Functions.DegToRad(NDB.TrackAccuracy)));
			fTmp = Functions.CircleVectorIntersect(ptNDB, R, ptTmp, Aztin - NDB.TrackAccuracy, out ptCur);
			TolerArea.AddPoint(ptCur);
			fTmp = Functions.CircleVectorIntersect(ptNDB, R, ptTmp, 180.0 + Aztin - NDB.TrackAccuracy, out ptCur);
			TolerArea.AddPoint(ptCur);
			ptTmp = Functions.PointAlongPlane(ptNDB, Aztin + 90.0, qN + System.Math.Sqrt(R * R - qN * qN) * System.Math.Tan(Functions.DegToRad(NDB.TrackAccuracy)));
			fTmp = Functions.CircleVectorIntersect(ptNDB, R, ptTmp, 180.0 + Aztin + NDB.TrackAccuracy, out ptCur);
			TolerArea.AddPoint(ptCur);
			fTmp = Functions.CircleVectorIntersect(ptNDB, R, ptTmp, Aztin + NDB.TrackAccuracy, out ptCur);
			TolerArea.AddPoint(ptCur);
			nDBFIXTolerAreaReturn = R;
			return nDBFIXTolerAreaReturn;
		}

		//NavaidToFIXableNavaid
		//public static NavaidType NavaidType2FixableNavaidType(NavaidType val)
		//{
		//	NavaidType result = new NavaidType();

		//	result.pPtGeo = val.pPtGeo;
		//	result.pPtPrj = val.pPtPrj;
		//	//result.pFeature = val.pFeature;

		//	result.NAV_Ident = val.NAV_Ident;
		//	result.Identifier = val.Identifier;
		//	result.Name = val.Name;
		//	result.CallSign = val.CallSign;
		//	result.MagVar = val.MagVar;
		//	result.HorAccuracy = val.HorAccuracy;

		//	result.TypeCode = val.TypeCode;
		//	result.Range = val.Range;
		//	result.index = val.index;

		//	result.PairNavaidIndex = val.PairNavaidIndex;
		//	result.PairNavaidType = val.PairNavaidType;

		//	result.Tag = val.Tag;

		//	//Res.Front;
		//	//Res.Dist;
		//	//Res.CLShift;
		//	//Res.ValCnt;
		//	//Res.ValMin;
		//	//Res.ValMax;

		//	return result;
		//}

		public static NavaidType WPT_FIXToNavaid(WPT_FIXType val)
		{
			NavaidType result = new NavaidType();

			result.pPtGeo = val.pPtGeo;
			result.pPtPrj = val.pPtPrj;

			result.NAV_Ident = val.NAV_Ident;
			result.Identifier = val.Identifier;
			result.Name = val.Name;
			result.CallSign = val.CallSign;
			result.TypeCode = val.TypeCode;

			result.MagVar = val.MagVar;
			result.HorAccuracy = val.HorAccuracy;

			if (val.TypeCode == eNavaidType.VOR)
				result.Range = VOR.Range;
			else if (val.TypeCode == eNavaidType.NDB)
				result.Range = NDB.Range;
			else if (val.TypeCode == eNavaidType.DME)
				result.Range = DME.Range;
			else
				result.Range = LLZ.Range;

			result.index = -1;
			result.PairNavaidIndex = -1;

			//Res.GP_RDH = 0;
			//Res.Course = 0;
			//Res.LLZ_THR = 0;
			//Res.SecWidth = 0;

			result.ValCnt = -2;
			result.Tag = val.Tag;
			return result;
		}
	}
}
