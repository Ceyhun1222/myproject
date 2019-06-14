using System;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace EOSID
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public enum eNavaidClass
	{
		CodeNONE = -1,
		CodeDME = 0,
		CodeVOR = 1,
		CodeNDB = 2,
		CodeLLZ = 3,
		CodeTACAN = 4
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class Navaids_DataBase
	{
		public static ITable NavaidTypes;

		public static VORData VOR = new VORData();
		public static NDBData NDB = new NDBData();
		public static DMEData DME = new DMEData();
		public static LLZData LLZ = new LLZData();

		public static string[] NavTypeNames = new string[] { "DME", "VOR", "NDB", "LOC", "TACAN", "Radar FIX" };

		public static string GetNavTypeName(eNavaidClass navType)
		{
			if (navType == eNavaidClass.CodeNONE)
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

				switch (System.Convert.ToString(NavaidTypes.GetRow(j).get_Value(iNavTypeName)))
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

		public static double VORFIXTolerArea(IPoint ptVor, double Aztin, double AbsH, out IPointCollection TolerArea)
		{
			double fTmpH = AbsH - ptVor.Z;
			double R = fTmpH * System.Math.Tan(Functions.DegToRad(VOR.ConeAngle));

			IPoint ptTmp, ptCur;

			TolerArea = new Polygon();
			ptTmp = Functions.LocalToPrj(ptVor, Aztin - (90.0 + VOR.TrackAccuracy), VOR.LateralDeviationCoef * fTmpH, 0.0);
			ptCur = Functions.CircleVectorIntersect(ptVor, R, ptTmp, Aztin - VOR.TrackAccuracy);
			TolerArea.AddPoint(ptCur);
			ptCur = Functions.CircleVectorIntersect(ptVor, R, ptTmp, 180.0 + Aztin - VOR.TrackAccuracy);
			TolerArea.AddPoint(ptCur);
			ptTmp = Functions.LocalToPrj(ptVor, Aztin + 90.0 + VOR.TrackAccuracy, VOR.LateralDeviationCoef * fTmpH, 0);
			ptCur = Functions.CircleVectorIntersect(ptVor, R, ptTmp, 180.0 + Aztin + VOR.TrackAccuracy);
			TolerArea.AddPoint(ptCur);
			ptCur = Functions.CircleVectorIntersect(ptVor, R, ptTmp, Aztin + VOR.TrackAccuracy);
			TolerArea.AddPoint(ptCur);

			return R;
		}

		public static double NDBFIXTolerArea(IPoint ptNDB, double Aztin, double AbsH, out IPointCollection TolerArea)
		{
			double fTmpH = AbsH - ptNDB.Z;
			double R = fTmpH * System.Math.Tan(Functions.DegToRad(NDB.ConeAngle));
			double qN = R * System.Math.Sin(Functions.DegToRad(NDB.Entry2ConeAccuracy));

			IPoint ptTmp, ptCur;
			TolerArea = new Polygon();

			ptTmp = Functions.LocalToPrj(ptNDB, Aztin - 90.0, qN + System.Math.Sqrt(R * R - qN * qN) * System.Math.Tan(Functions.DegToRad(NDB.TrackAccuracy)), 0.0);
			ptCur = Functions.CircleVectorIntersect(ptNDB, R, ptTmp, Aztin - NDB.TrackAccuracy);
			TolerArea.AddPoint(ptCur);
			ptCur = Functions.CircleVectorIntersect(ptNDB, R, ptTmp, 180.0 + Aztin - NDB.TrackAccuracy);
			TolerArea.AddPoint(ptCur);
			ptTmp = Functions.LocalToPrj(ptNDB, Aztin + 90.0, qN + System.Math.Sqrt(R * R - qN * qN) * System.Math.Tan(Functions.DegToRad(NDB.TrackAccuracy)), 0.0);
			ptCur = Functions.CircleVectorIntersect(ptNDB, R, ptTmp, 180.0 + Aztin + NDB.TrackAccuracy);
			TolerArea.AddPoint(ptCur);
			ptCur = Functions.CircleVectorIntersect(ptNDB, R, ptTmp, Aztin + NDB.TrackAccuracy);
			TolerArea.AddPoint(ptCur);

			return R;
		}

		public static NavaidData WPT_FIXToNavaid(WPT_FIXData pWPT)
		{
			NavaidData result = new NavaidData();

			result.pPtGeo = pWPT.pPtGeo;
			result.pPtPrj = pWPT.pPtPrj;

			result.Name = pWPT.Name;
			result.CallSign = pWPT.Name;
			result.ID = pWPT.ID;

			result.TypeCode = pWPT.TypeCode;
			result.MagVar = pWPT.MagVar;

			if (result.TypeCode == eNavaidClass.CodeVOR)
				result.Range = VOR.Range;
			else if (result.TypeCode == eNavaidClass.CodeNDB)
				result.Range = NDB.Range;
			else if (result.TypeCode == eNavaidClass.CodeDME)
				result.Range = DME.Range;
			else
				result.Range = LLZ.Range;

			result.PairNavaidIndex = -1;
			result.index = -1;
			result.Tag = pWPT.Tag;

			result.ValCnt = -1;
			result.ValMin = null;
			result.ValMax = null;

			return result;
		}


	}
}
