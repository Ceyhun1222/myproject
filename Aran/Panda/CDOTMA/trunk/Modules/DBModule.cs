using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using Microsoft.Win32;

using PDM;
using ZzArchivator;
using CDOTMA.CoordinatSystems;
using NetTopologySuite.Geometries;
using GeoAPI.Geometries;
using Converter;
using Converters.ConvertESRIvvsJTS;
using CDOTMA.Geometries;
using Converters;
//using Aran.Aim.Features;

namespace CDOTMA
{
	public delegate void Process(double percents);
 
	[System.Runtime.InteropServices.ComVisible(false)]
	[FlagsAttribute]
	public enum DataType
	{
		NoData = 0,
		Airspace = 1,
		AirportHeliport = 2,
		IAP = 4,
		STAR = 8,
		SID = 16,
		Route = 32
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class DBModule
	{
		static string _FileName;
		static string _TempDirName;

		static public string FileName { get { return _FileName; } }

		public static CoordinatSystem LoadViewPrjFile()
		{
			string tempPrjFilename = System.IO.Path.Combine(_TempDirName, "view.prj");

			CoordinatSystem result;
			FileStream fs = null;
			try
			{
				fs = new System.IO.FileStream(tempPrjFilename, FileMode.Open);
				result = CoordinatSystem.LoadFromStream(fs);
			}
			catch
			{
				result = null;
			}
			finally
			{
				if (fs != null)
				{
					fs.Close();
					fs.Dispose();
				}
			}

			return result;
		}

		public static void SaveViewPrjFile(CoordinatSystem cs)
		{
			string tempPrjFilename = System.IO.Path.Combine(_TempDirName, "view.prj");
			FileStream fs = new System.IO.FileStream(tempPrjFilename, FileMode.OpenOrCreate);

			cs.SaveToStream(fs);

			//StreamWriter sw = new StreamWriter(fs);
			//sw.Write(cs.ToString());
			//sw.Close();
			//sw.Dispose();

			fs.Close();
			fs.Dispose();
		}

		public static Settings LoadSettings()
		{
			string tempCfgFilename = System.IO.Path.Combine(_TempDirName, "DisplaySettings.cfg");

			Settings result;
			FileStream fs = null;
			try
			{
				fs = new System.IO.FileStream(tempCfgFilename, FileMode.Open);
				result = Settings.Read(fs);
			}
			catch
			{
				result = null;
			}
			finally
			{
				if (fs != null)
				{
					fs.Close();
					fs.Dispose();
				}
			}

			return result;
		}

		public static void SaveSettings(Settings settings)
		{
			string tempCfgFilename = System.IO.Path.Combine(_TempDirName, "DisplaySettings.cfg");
			settings.Write(tempCfgFilename);
		}

		#region read squental data

		//public static string GetPathToTemplateFile()
		//{
		//    const string subkey = "Software\\RISK\\ARENA";
		//    const string keyName = subkey;

		//    return (string)CDRegistry.RegRead(Registry.CurrentUser, keyName, "PdmFile", "");
		//}

		public static void PackFileToData(string FileName, string ArchiveName)
		{
			ArenaStatic.ArenaStaticProc.CompressDirectory(_TempDirName, ArchiveName, new string[] { ".cfg", ".prj" });
			//ArenaStatic.ArenaStaticProc.CompressDirectory(_TempDirName + "\\" + FileName, ArchiveName);
			//ZzArchivatorClass.AddFileToArchive(GlobalVars.ApplicationDir + @"\Utils\7z.exe", _TempDirName + "\\" + FileName, ArchiveName);
		}

		public static void PackDataFile(string ArchiveName)
		{
			ZzArchivatorClass.AddFolderToArchive(GlobalVars.ApplicationDir + @"\Utils\7z.exe", ArchiveName, _TempDirName);
		}

		public static void SetDataFile(string FileName)
		{
			_FileName = FileName;

			var dInf = Directory.CreateDirectory(System.IO.Path.GetTempPath() + @"\PDM\" + System.IO.Path.GetFileNameWithoutExtension(FileName));
			_TempDirName = dInf.FullName;

			ArenaStatic.ArenaStaticProc.SetTargetDB(_TempDirName);
			ArenaStatic.ArenaStaticProc.DecompressToDirectory(FileName, _TempDirName);
            //ZzArchivatorClass.ExtractFromArchive(GlobalVars.ApplicationDir + @"\Utils\7z.exe", FileName, _TempDirName);
			//ZzArchivatorClass.ExtractFromArchive(GetPathToTemplateFile() + @"\Utils\7z.exe", FileName, _TempDirName);

			//========================================================================================================================
			CoordinatSystem cs = LoadViewPrjFile();
			if (cs == null)
			{
				ProjectionDlg.ShowDialog(out cs, null, "Define Projected Coordinate System of View");
				SaveViewPrjFile(cs);
				PackFileToData("view.prj", FileName);
			}
			GlobalVars.pSpRefPrj = cs;

			Settings settings = LoadSettings();
			if (settings == null)
			{
				SettingsDlg.ShowDialog(out settings, null);				//settings = new Settings();
				SaveSettings(settings);
				PackFileToData("DisplaySettings.cfg", FileName);
			}

			GlobalVars.settings = settings;
			GlobalVars.unitConverter = new UnitConverter(settings);

			return;
		}

		static string[] FN;
		public static IEnumerable<PDMObject> GetNextObject()
		{
			FN = Directory.GetFiles(_TempDirName, "*.pdm");

			FileStream fs;
			while (true)
			{
				for (int i = FN.Length; i > 0; i--)
				{
					string file = FN[i - 1];
					fs = new System.IO.FileStream(file, FileMode.Open);

					//var byteArr = new byte[fs.Length];
					//fs.Position = 0;
					//var count = fs.Read(byteArr, 0, byteArr.Length);

					//if (count != byteArr.Length)
					//{
					//    fs.Close();
					//    Console.WriteLine(@"Test Failed: Unable to read data from file");
					//}

					var xmlSer = new XmlSerializer(typeof(PDM_ObjectsList));
					PDM_ObjectsList prj = (PDM_ObjectsList)xmlSer.Deserialize(fs);

					for (int j = 0; j < prj.PDMObject_list.Count; j++)
						yield return prj.PDMObject_list[j];

					fs.Close();
					fs.Dispose();
				}

				yield break;
			}
		}

		#endregion

		public static int FillADHPFields(ref ADHPType CurrADHP, AirportHeliport pADHP)
		{
			if (CurrADHP.pPtGeo != null)
				return 0;

			CurrADHP.pAirportHeliport = pADHP;
			if (pADHP == null)
				return -1;

			CurrADHP.pAirportHeliport = pADHP;
			CurrADHP.Name = pADHP.Name;
			CurrADHP.Identifier = pADHP.ID;

			//Geometry.RelationalOperator pRelational;
			//pADHP.RebuildGeo();
			//Point pPtGeo1 = (Point)FromESRIToJTS.FromGeometry(pADHP.Geo);

			Point pPtGeo = FromESRIToJTS.AIXMCoordsToPoint(pADHP.Lon, pADHP.Lat);
			pPtGeo.Z = ConverterToSI.Convert((double)pADHP.Elev, pADHP.Elev_UOM);

			//    End if

			Point pPtPrj = Functions.ToPrj(pPtGeo) as Point;
			if (pPtPrj.IsEmpty)
				return -1;

			//pRelational = (ESRI.ArcGIS.Geometry.IRelationalOperator)GlobalVars.p_LicenseRect;
			//if (!pRelational.Contains(pPtPrj))
			//    return -1;

			CurrADHP.pPtGeo = pPtGeo;
			CurrADHP.pPtPrj = pPtPrj;
			//CurrADHP.OrgID = pADHP.ResponsibleOrganisation.TheOrganisationAuthority.Identifier;

			CurrADHP.MagVar = pADHP.MagneticVariation.HasValue ?(double)pADHP.MagneticVariation : 0;

			//CurADHP.Elev = ConverterToSI.ConvertToSI(pElevPoint.Elevation, 0.0)
			//CurADHP.pPtGeo.Z = CurADHP.Elev
			//CurADHP.pPtPrj.Z = CurADHP.Elev

			//CurADHP.MinTMA = ConverterToSI.Convert(pADHP.transitionAltitude, 2500.0)
			//CurADHP.TransitionAltitude = ConverterToSI.ConvertToSI(ah.TransitionAltitude)

			CurrADHP.ISAtC = 15.0;	//ConverterToSI.Convert(pADHP.ReferenceTemperature, 15.0);

			CurrADHP.TransitionLevel = ConverterToSI.Convert((double)pADHP.TransitionAltitude, pADHP.TransitionAltitudeUOM, 2500.0);
			CurrADHP.WindSpeed = 56.0;
			return 1;
		}

		public static void AddNavaids(ref List<NavaidType> NavaidList, ref List<NavaidType> DMEList, NavaidSystem pNavaid) /*ADHPType CurrADHP, double Radius*/
		{
			int i, j;
			NavaidType Navaid;
			eNavaidType NavTypeCode;

			/*
			if (pNavaid.Components.Count == 0)
			{
				if (pNavaid.CodeNavaidSystemType == NavaidSystemType.VOR)
					NavTypeCode = eNavaidType.CodeVOR;
				else if (pNavaid.CodeNavaidSystemType == NavaidSystemType.DME)
					NavTypeCode = eNavaidType.CodeDME;
				else if (pNavaid.CodeNavaidSystemType == NavaidSystemType.NDB)
					NavTypeCode = eNavaidType.CodeNDB;
				else if (pNavaid.CodeNavaidSystemType == NavaidSystemType.TACAN)
					NavTypeCode = eNavaidType.CodeTACAN;
				else
					return;

				Point pPtGeo = ConvertGeometry.StrCoordsToPoint(pNavaid.Lat, pNavaid.Lon);
				if (pPtGeo == null)
					return;

				pPtGeo.Z = ConverterToSI.Convert(pNavaid.Elev, pNavaid.Elev_UOM);

				Point pPtPrj = (Point)Functions.ToPrj(pPtGeo);

				if (pPtPrj.IsEmpty)
					return;

				Navaid = new NavaidType();

				Navaid.pPtGeo = pPtGeo;
				Navaid.pPtPrj = pPtPrj;

				//Navaid.MagVar = pNavaid.MagVar;
				//Navaid.pFeature = pNavaid;

				if (NavTypeCode == eNavaidType.CodeDME)
					Navaid.Range = 350000.0;		//DME.Range
				else if (NavTypeCode == eNavaidType.CodeNDB)
					Navaid.Range = 350000.0;		//NDB.Range
				else
					Navaid.Range = 350000.0;		//VOR.Range

				Navaid.PairNavaidIndex = -1;

				Navaid.ID = pNavaid.ID;
				//Navaid.Name = pNavaid.NavName;
				Navaid.CallSign = pNavaid.Designator;

				Navaid.TypeCode = NavTypeCode;
				Navaid.index = DMEList.Count + NavaidList.Count;

				if (NavTypeCode == eNavaidType.CodeDME)
					DMEList.Add(Navaid);
				else
					NavaidList.Add(Navaid);

				return;
			}
			*/

			int cn = NavaidList.Count;
			int cd = DMEList.Count;

			for (i = 0; i < pNavaid.Components.Count; i++)
			{
				NavaidComponent navComp;
				navComp = (NavaidComponent)pNavaid.Components[i];
				if (navComp == null)
					continue;

				if (navComp.PDM_Type == PDM_ENUM.VOR)
					NavTypeCode = eNavaidType.CodeVOR;
				else if (navComp.PDM_Type == PDM_ENUM.DME)
					NavTypeCode = eNavaidType.CodeDME;
				else if (navComp.PDM_Type == PDM_ENUM.NDB)
					NavTypeCode = eNavaidType.CodeNDB;
				else if (navComp.PDM_Type == PDM_ENUM.TACAN)
					NavTypeCode = eNavaidType.CodeTACAN;
				else
					continue;

				//navComp.RebuildGeo();
				//Point pPtGeo = (Point)FromESRIToJTS.FromGeometry(navComp.Geo);
				if (navComp.Elev == null)
					continue;

				if (navComp.MagVar == null)
					continue;

				Point pPtGeo = FromESRIToJTS.AIXMCoordsToPoint(navComp.Lon, navComp.Lat);
				pPtGeo.Z = ConverterToSI.Convert((double)navComp.Elev, navComp.Elev_UOM);

				Point pPtPrj = (Point)Functions.ToPrj(pPtGeo);

				if (pPtPrj.IsEmpty)
					continue;

				Navaid = new NavaidType();

				Navaid.pPtGeo = pPtGeo;
				Navaid.pPtPrj = pPtPrj;
				Navaid.MagVar = (double)navComp.MagVar;
				Navaid.pFeature = navComp;

				if (NavTypeCode == eNavaidType.CodeDME)
					Navaid.Range = 350000.0;		//DME.Range
				else if (NavTypeCode == eNavaidType.CodeNDB)
					Navaid.Range = 350000.0;		//NDB.Range
				else
					Navaid.Range = 350000.0;		//VOR.Range

				Navaid.PairNavaidIndex = -1;

				Navaid.ID = pNavaid.ID;		// navComp.ID;
				Navaid.Name = navComp.NavName;
				Navaid.CallSign = navComp.Designator;

				Navaid.TypeCode = NavTypeCode;
				Navaid.index = DMEList.Count + NavaidList.Count;

				if (NavTypeCode == eNavaidType.CodeDME)
					DMEList.Add(Navaid);
				else
				{
					NavaidList.Add(Navaid);

					LegPoint lpt = new LegPoint(Navaid);
					GlobalVars.LegPoints.Add(lpt);
				}
			}

			for (j = cn; j < NavaidList.Count; j++)
				for (i = cd; i < DMEList.Count; i++)
				{
					double fDist = Functions.ReturnDistanceInMeters(NavaidList[j].pPtPrj, DMEList[i].pPtPrj);

					if (fDist <= 2.0)
					{
						Navaid = NavaidList[j];
						Navaid.PairNavaidIndex = i;
						NavaidList[j] = Navaid;

						Navaid = DMEList[i];
						Navaid.PairNavaidIndex = j;
						DMEList[i] = Navaid;
						break;
					}
				}
		}

		static WPT_FIXType NavaidToWPT(NavaidType Navaid)
		{
			WPT_FIXType result = new WPT_FIXType();

			result.pPtGeo = Navaid.pPtGeo;
			result.pPtPrj = Navaid.pPtPrj;
			//public Feature pFeature;
			//public Guid Identifier;

			result.pFeature = Navaid.pFeature;
			result.ID = Navaid.ID;

			result.Name = Navaid.Name;
			result.CallSign = Navaid.CallSign;
			result.MagVar = Navaid.MagVar;
			result.TypeCode = Navaid.TypeCode;
			result.Tag = Navaid.Tag;

			return result;
		}

		static void FillWPTFields(ref WPT_FIXType wptFix, WayPoint wpt)
		{
			wptFix.CallSign = wpt.Designator;

			//if (wpt.Elev == null)	return;

			wpt.RebuildGeo();

			Point pPtGeo = (Point)FromESRIToJTS.FromGeometry(wpt.Geo);
			if (pPtGeo == null)
				return;

			if (wpt.Elev != null)
				pPtGeo.Z = ConverterToSI.Convert((double)wpt.Elev, wpt.Elev_UOM) + 300.0;
			else if (!double.IsNaN(pPtGeo.Z))
				pPtGeo.Z += 300.0;
			else
				pPtGeo.Z = 300.0;

			Point pPtPrj = (Point)Functions.ToPrj(pPtGeo);

			if (pPtPrj.IsEmpty)
				return;

			//if (AIXMWPT.Designator == null)
			//    continue;

			//if Not AIXMWPT.MagneticVariation is Nothing 
			//	WPTList[iWPTNum].MagVar = AIXMWPT.MagneticVariation.Value
			//else
			//	WPTList[iWPTNum].MagVar = CurrADHP.MagVar
			//End if

			wptFix.MagVar = 0.0; //CurrADHP.MagVar

			wptFix.pPtGeo = pPtGeo;
			wptFix.pPtPrj = pPtPrj;

			wptFix.Name = wpt.Designator;
			wptFix.CallSign = wpt.Designator;
			wptFix.ID = wpt.ID;

			wptFix.pFeature = wpt;

			wptFix.TypeCode = eNavaidType.CodeNONE;
		}

		#region FillAST

		public static bool FillASTFields(ref AirspaceType Ast, Airspace airspace)
		{
			Ast.pAirspace = airspace;
			Ast.AsVT = new List<AirspaceVolumeType>();

			for (int i = 0; i < airspace.AirspaceVolumeList.Count; i++)
			{
				AirspaceVolumeType AsVT = new AirspaceVolumeType();

				if (FillASTVFields(ref AsVT, airspace.AirspaceVolumeList[i]))
					Ast.AsVT.Add(AsVT);
			}

			return Ast.AsVT.Count > 0;
		}

		public static bool FillASTVFields(ref AirspaceVolumeType AsVT, AirspaceVolume airspaceVolume)
		{
			AsVT.pAirspaceVolume = airspaceVolume;
			airspaceVolume.RebuildGeo2();

			//MultiPolygon airspaceVolumeGeom;	//AsVT.geometryGeo
			try
			{
				AsVT.geometryGeo = (MultiPolygon)FromESRIToJTS.FromGeometry(airspaceVolume.Geo);
			}
			catch
			{
				AsVT.geometryGeo = null;
			}

			if (AsVT.geometryGeo == null)
				return false;

			GeometryExtension UserData = new GeometryExtension(AsVT.geometryGeo);
			UserData.SpatialReference = new GeographicCoordinatSystem();
			AsVT.geometryGeo.UserData = UserData;

			AsVT.geometryPrj = (MultiPolygon)Functions.ToPrj(AsVT.geometryGeo);

			return true;
		}

		#endregion

		//================================================================================
		static SegmentPoint prevPt;
		static Point prevPtGeo;
		static Point prevPtPrj;

		#region FillRoute
		private static void FillRoute(ref ProcedureType prc, PDMObject pDMObject)
		{
			Enroute pdmEnrt = (Enroute)pDMObject;
			prc.pProcedure = pdmEnrt;
			prc.procType = PROC_TYPE_code.Multiple;
			prc.Name = pdmEnrt.TxtDesig;

			if (prc.Name == null)
				prc.Name = "";

			prc.procTransitions = new List<Transition>();

			Transition tr = new Transition();
			tr.Owner = prc;
			tr.procLegs = new List<Leg>();

			List<LineString> nominal = new List<LineString>();

			for (int i = 0; i < pdmEnrt.Routes.Count; i++)
			{
				Leg leg = new Leg();
				leg.Owner = tr.Owner;

				//if (pdmEnrt.Routes[i].StartPoint.Elev == null)	continue;

				FillLeg(ref leg, pdmEnrt.Routes[i]);

				if (leg.ptEndPrj != null && leg.ptStartGeo != null)// && leg.PathGeomPrj!= null)
				{
					tr.procLegs.Add(leg);
					TraceLeg trleg = new TraceLeg(leg);
					GlobalVars.TraceLegs.Add(trleg);

					foreach (var gm in leg.PathGeomPrj.Geometries)
						nominal.Add((LineString)gm);
				}
			}

			tr.NominalLine = new MultiLineString(nominal.ToArray());
			prc.procTransitions.Add(tr);
			prc.NominalLine = tr.NominalLine;
		}

		private static void FillLeg(ref Leg leg, RouteSegment routeSegment)
		{
			leg.pProcLeg = routeSegment;
			leg.Name = routeSegment.ID;
			//leg.PathCode = CodeSegmentPath. ;//routeSegment.CodeType;// .LegTypeARINC;
			//routeSegment.RebuildGeo();
			leg.Length = ConverterToSI.Convert((double)routeSegment.ValLen, routeSegment.UomValLen);
			leg.HStart = ConverterToSI.Convert((double)routeSegment.ValDistVerLower, routeSegment.UomValDistVerLower);
			leg.HFinish = ConverterToSI.Convert((double)routeSegment.ValDistVerUpper, routeSegment.UomValDistVerUpper);

			routeSegment.RebuildGeo2();
			leg.PathGeomGeo = (MultiLineString)FromESRIToJTS.FromGeometry(routeSegment.Geo);
			leg.PathGeomPrj = (MultiLineString)Functions.ToPrj(leg.PathGeomGeo);

			leg.ptStart = routeSegment.StartPoint;

			if (leg.ptStart == null)
			{
				leg.ptStartGeo = null;
				leg.ptStartPrj = null;
			}
			else
			{
				//leg.ptStartGeo = FromESRIToJTS.AIXMCoordsToPoint(leg.ptStart.Lon, leg.ptStart.Lat);
				if (leg.ptStart.Geo != null)
				{
					leg.ptStartGeo = (Point)FromESRIToJTS.FromGeometry(leg.ptStart.Geo);
					leg.ptStartGeo.Z = leg.HStart;      // ConverterToSI.Convert((double)leg.ptStart.Elev, leg.ptStart.Elev_UOM);
					leg.ptStartPrj = Functions.ToPrj(leg.ptStartGeo) as Point;
				}
			}

			leg.ptEnd = routeSegment.EndPoint;

			if (leg.ptEnd == null)
			{
				leg.ptEndGeo = null;
				leg.ptEndPrj = null;
			}
			else
			{
				//leg.ptEndGeo = FromESRIToJTS.AIXMCoordsToPoint(leg.ptEnd.Lon, leg.ptEnd.Lat);
				if (leg.ptEnd.Geo != null)
				{
					leg.ptEndGeo = (Point)FromESRIToJTS.FromGeometry(leg.ptStart.Geo);
					leg.ptEndGeo.Z = leg.HFinish;       // ConverterToSI.Convert((double)leg.ptEnd.Elev, leg.ptEnd.Elev_UOM);
					leg.ptEndPrj = Functions.ToPrj(leg.ptEndGeo) as Point;
				}
			}


			//double ftmp = Functions.ReturnDistanceInMeters(leg.ptEndPrj, leg.ptStartPrj);
			//leg.GuidanceNav = procedureLeg ;
			//leg.BankAngle = routeSegment.BankAngle;
			//leg.PDG = Math.Tan(Functions.DegToRad(routeSegment.VerticalAngle));
			//if (routeSegment.CodeDir == CODE_ROUTE_SEGMENT_DIR.BACKWARD)
			//{
			//    leg.AztIn = routeSegment.ValTrueTrack + 180.0;
			//    leg.AztOut = NativeMethods.Modulus(routeSegment.ValReversTrueTrack);
			//}
			//else
			{
				leg.AztIn = (double)routeSegment.ValTrueTrack;
				leg.AztOut = NativeMethods.Modulus((double)routeSegment.ValReversTrueTrack + 180.0);
			}

			if (leg.ptStartGeo != null)
				leg.DirIn = Functions.Azt2Dir(leg.ptStartGeo, leg.AztIn);

			if (leg.ptEndGeo != null)
				leg.DirOut = Functions.Azt2Dir(leg.ptEndGeo, leg.AztOut);
		}

		#endregion

		#region FiillProcedure

		private static void FiillProcedure(ref ProcedureType prc, PDMObject pDMObject)
		{
			Procedure pdmPrc = (Procedure)pDMObject;
			prc.pProcedure = pdmPrc;
			prc.procType = pdmPrc.ProcedureType;

			prc.Name = pdmPrc.ProcedureIdentifier;
			if (prc.Name == null)
				prc.Name = "";

			prc.procTransitions = new List<Transition>();

			List<LineString> nominal = new List<LineString>();

			for (int i = 0; i < pdmPrc.Transitions.Count; i++)
			{
				Transition tr = new Transition();
				tr.Owner = prc;

				FiillTransition(ref tr, pdmPrc.Transitions[i]);
				prc.procTransitions.Add(tr);

				foreach (var gm in tr.NominalLine.Geometries)
					nominal.Add((LineString)gm);

				//nominal.AddRange((LineString[])tr.NominalLine.Geometries);
			}

			prc.NominalLine = new MultiLineString(nominal.ToArray());
		}

		private static void FiillTransition(ref Transition tr, ProcedureTransitions procedureTransitions)
		{
			tr.pProcTransition = procedureTransitions;
			tr.Name = procedureTransitions.TransitionIdentifier;
			tr.procLegs = new List<Leg>();
			prevPt = null;
			prevPtGeo = prevPtPrj = null;
			tr.RouteBuffer = null;

			List<LineString> nominal = new List<LineString>();

			for (int i = 0; i < procedureTransitions.Legs.Count; i++)
			{
				Leg leg = new Leg();
				leg.Owner = tr.Owner;

				//if (procedureTransitions.Legs[i].Length == null)		continue;

				FillLeg(ref leg, procedureTransitions.Legs[i]);

				if (procedureTransitions.Legs[i].LegPathField == TrajectoryType.STRAIGHT)
				{
					leg.AztIn = leg.AztOut;
					leg.DirIn = leg.DirOut;
				}
				else if (tr.procLegs.Count > 0)
				{
					leg.AztIn = tr.procLegs[tr.procLegs.Count - 1].AztOut;
					leg.DirIn = tr.procLegs[tr.procLegs.Count - 1].DirOut;
				}

				tr.procLegs.Add(leg);
				TraceLeg trleg = new TraceLeg(leg);
				GlobalVars.TraceLegs.Add(trleg);
				if (leg.ptEnd != null)
				{
					prevPt = leg.ptEnd;
					prevPtGeo = leg.ptEndGeo;
					prevPtPrj = leg.ptEndPrj;
				}
				else if (leg.ptStart != null)
				{
					prevPt = leg.ptStart;
					prevPtGeo = leg.ptStartGeo;
					prevPtPrj = leg.ptStartPrj;
				}

				if (leg.PathGeomPrj != null)
					foreach (var gm in leg.PathGeomPrj.Geometries)
						nominal.Add((LineString)gm);
			}

			tr.NominalLine = new MultiLineString(nominal.ToArray());
		}

		private static void FillLeg(ref Leg leg, ProcedureLeg procedureLeg)
		{
			leg.pProcLeg = procedureLeg;
			leg.Name = procedureLeg.ID;
			leg.PathCode = procedureLeg.LegTypeARINC;
			leg.BankAngle = (double)procedureLeg.BankAngle;
			leg.PDG = Math.Tan(Functions.DegToRad((double)procedureLeg.VerticalAngle));

			if (leg.PDG > 0.0)
			{
				leg.HFinish = ConverterToSI.Convert((double)procedureLeg.UpperLimitAltitude, procedureLeg.UpperLimitAltitudeUOM);
				leg.HStart = ConverterToSI.Convert((double)procedureLeg.LowerLimitAltitude, procedureLeg.LowerLimitAltitudeUOM);
			}
			else
			{
				leg.HFinish = ConverterToSI.Convert((double)procedureLeg.LowerLimitAltitude, procedureLeg.LowerLimitAltitudeUOM);
				leg.HStart = ConverterToSI.Convert((double)procedureLeg.UpperLimitAltitude, procedureLeg.UpperLimitAltitudeUOM);
			}

			leg.ptStart = procedureLeg.StartPoint;

			if (leg.ptStart == null)
			{
				leg.ptStartGeo = null;
				leg.ptStartPrj = null;
			}
			else
			{
				leg.ptStart.RebuildGeo();
				if (leg.ptStart.Geo != null)
				{
					//leg.ptStartGeo = new Point(((IPoint)(leg.ptStart.Geo)).X, ((IPoint)(leg.ptStart.Geo)).Y);
					leg.ptStartGeo = (Point)FromESRIToJTS.FromGeometry(leg.ptStart.Geo);
					leg.ptStartGeo.Z = leg.HStart;		// ConverterToSI.Convert((double)leg.ptStart.Elev, leg.ptStart.Elev_UOM);
					leg.ptStartPrj = Functions.ToPrj(leg.ptStartGeo) as Point;
				}
			}

			leg.ptEnd = procedureLeg.EndPoint;
			if (leg.ptEnd == null)
			{
				leg.ptEndGeo = null;
				leg.ptEndPrj = null;
			}
			else
			{
				leg.ptEnd.RebuildGeo();
				if (leg.ptEnd.Geo != null)
				{
					leg.ptEndGeo = (Point)FromESRIToJTS.FromGeometry(leg.ptEnd.Geo)	;//FromESRIToJTS.AIXMCoordsToPoint(leg.ptEnd.Lon, leg.ptEnd.Lat);
					leg.ptEndGeo.Z = leg.HFinish;			// ConverterToSI.Convert((double)leg.ptEnd.Elev, leg.ptEnd.Elev_UOM);
					leg.ptEndPrj = Functions.ToPrj(leg.ptEndGeo) as Point;
				}
			}

			procedureLeg.RebuildGeo2();
			if (procedureLeg.Geo != null)
			{
				leg.PathGeomGeo = (MultiLineString)FromESRIToJTS.FromGeometry(procedureLeg.Geo);
				leg.PathGeomPrj = (MultiLineString)Functions.ToPrj(leg.PathGeomGeo);
			}
			else if(leg.ptStartGeo != null && leg.ptEndGeo != null)
			{
				LineString[] ls = new LineString[1];
				ls[0] = new LineString(new Coordinate[] { new Coordinate(leg.ptStartGeo.X, leg.ptStartGeo.Y, leg.ptStartGeo.Z), new Coordinate(leg.ptEndGeo.X, leg.ptEndGeo.Y, leg.ptEndGeo.Z) } );

				leg.PathGeomGeo = new MultiLineString(ls);
				leg.PathGeomPrj = (MultiLineString)Functions.ToPrj(leg.PathGeomGeo);
			}

			//leg.GuidanceNav = procedureLeg;
			if (procedureLeg.Length == null)
			{
				if (leg.PathGeomPrj != null)
					leg.Length = leg.PathGeomPrj.Length;
			}
			else
				leg.Length = ConverterToSI.Convert((double)procedureLeg.Length, procedureLeg.LengthUOM);

			if ((procedureLeg.Course == null || double.IsNaN((double)procedureLeg.Course)) && leg.PathGeomPrj != null)
			{
				LineString ls = (LineString)leg.PathGeomPrj.Geometries[0];

				leg.DirOut = Functions.ReturnAngleInDegrees((Point)ls.StartPoint, (Point)ls.EndPoint);
				leg.AztOut = Functions.Dir2Azt((Point)ls.EndPoint, leg.DirOut);
			}
			else
			{
				switch (procedureLeg.CourseType)
				{
					case CodeCourse.TRUE_BRG:
						leg.AztOut = (double)procedureLeg.Course;
						break;
					case CodeCourse.MAG_BRG:
						leg.AztOut = (double)procedureLeg.Course;// + 0.0;//procedureLeg.EndPoint.MagVar;
						break;
					default:
						leg.AztOut = (double)procedureLeg.Course;
						break;
				}

				if (leg.ptEndGeo != null)
					leg.DirOut = Functions.Azt2Dir(leg.ptEndGeo, leg.AztOut);
				else if (leg.ptStartGeo != null)
					leg.DirOut = Functions.Azt2Dir(leg.ptStartGeo, leg.AztOut);
				else if (prevPtGeo != null)
					leg.DirOut = Functions.Azt2Dir(prevPtGeo, leg.AztOut);
				else
					leg.DirOut = 90.0 - leg.AztOut;
			}
		}
		#endregion

		#region FiillLists
		//================================================================================
		public static void FillLists(DataType datatypes, Process info)
		{
			GlobalVars.ADHPList = new List<ADHPType>();
			GlobalVars.AstList = new List<AirspaceType>();
			GlobalVars.AirspaceVolumeList = new List<AirspaceVolumeType>();
			GlobalVars.NavaidList = new List<NavaidType>();
			GlobalVars.DMEList = new List<NavaidType>();

			GlobalVars.WPTList = new List<WPT_FIXType>();
			//GlobalVars.Procedures = new List<ProcedureType>();

			GlobalVars.ApproachProcedures = new List<ProcedureType>();
			GlobalVars.DepartureProcedures = new List<ProcedureType>();
			GlobalVars.ArrivalProcedures = new List<ProcedureType>();
			GlobalVars.Routs = new List<ProcedureType>();

			GlobalVars.TraceLegs = new List<TraceLeg>();
			GlobalVars.LegPoints = new List<LegPoint>();
			ProcedureType prc;
			int i = 0;
			double invLen = -1.0;

			foreach (PDMObject pobj in GetNextObject())
			{
				if (invLen < 0.0)
				{
					if (FN.Length == 0)
						invLen = 0.0;
					else
						invLen = 1.0 / (10.0 * FN.Length);
				}

				//System.Diagnostics.Debug.WriteLine(pobj.PDM_Type);
				switch (pobj.PDM_Type)
				{
					case PDM_ENUM.AirportHeliport:
						if ((datatypes & DataType.AirportHeliport) != DataType.NoData)
						{
							AirportHeliport airportHeliport = (AirportHeliport)pobj;
							ADHPType adhp = new ADHPType();
							FillADHPFields(ref adhp, airportHeliport);

							GlobalVars.ADHPList.Add(adhp);
						}
						break;

					case PDM_ENUM.Airspace:
						if ((datatypes & DataType.Airspace) != DataType.NoData)
						{
							Airspace airspace = (Airspace)pobj;
							AirspaceType Ast = new AirspaceType();

							if (FillASTFields(ref Ast, airspace))
								GlobalVars.AstList.Add(Ast);
						}
						break;

					case PDM_ENUM.NavaidSystem:
						NavaidSystem navSys = (NavaidSystem)pobj;

						AddNavaids(ref GlobalVars.NavaidList, ref GlobalVars.DMEList, navSys);
						break;

					case PDM_ENUM.WayPoint:
						WayPoint wpt = (WayPoint)pobj;
						WPT_FIXType wptFix = new WPT_FIXType();

						FillWPTFields(ref wptFix, wpt);

						if (wptFix.pPtGeo == null)
							continue;

						//GlobalVars.WPTList.Add(wptFix);
						LegPoint lpt = new LegPoint(wptFix);

						GlobalVars.LegPoints.Add(lpt);

						break;

					//case PDM_ENUM.Procedure:
					case PDM_ENUM.Enroute:
						if ((datatypes & DataType.Route) != DataType.NoData)
						{
							prc = new ProcedureType();
							FillRoute(ref prc, pobj);
							GlobalVars.Routs.Add(prc);
						}
						break;

					case PDM_ENUM.InstrumentApproachProcedure:
						if ((datatypes & DataType.IAP) != DataType.NoData)
						{
							prc = new ProcedureType();
							FiillProcedure(ref prc, pobj);
							GlobalVars.ApproachProcedures.Add(prc);
						}
						break;

					case PDM_ENUM.StandardInstrumentArrival:
						if ((datatypes & DataType.STAR) != DataType.NoData)
						{
							prc = new ProcedureType();
							FiillProcedure(ref prc, pobj);
							GlobalVars.ArrivalProcedures.Add(prc);
						}
						break;

					case PDM_ENUM.StandardInstrumentDeparture:
						if ((datatypes & DataType.SID) != DataType.NoData)
						{
							prc = new ProcedureType();
							FiillProcedure(ref prc, pobj);
							GlobalVars.DepartureProcedures.Add(prc);
						}
						break;
				}

				i++;
				info(i * invLen);
			}

			i = 0;
			while (i < GlobalVars.LegPoints.Count)
			{
				LegPoint lpt = GlobalVars.LegPoints[i];

				bool used = false;
				foreach (TraceLeg leg in GlobalVars.TraceLegs)
				{
					int res = lpt.CheckLeg(leg);

					if (res > 0)
						used = true;
				}

				if (!used)
					GlobalVars.LegPoints.Remove(lpt);
				else
				{
					if(lpt.TypeCode > eNavaidType.CodeNONE)
						GlobalVars.WPTList.Add(NavaidToWPT(lpt.parentNav));
					else
						GlobalVars.WPTList.Add(lpt.parentWpt);
					i++;
				}
			}

			info(100.0);
		}
		//================================================================================
		#endregion
	}
}
