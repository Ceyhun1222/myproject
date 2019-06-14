using Aran.Geometries;
using System.Collections.Generic;
using Aran.PANDA.Common;
using Aran.AranEnvironment.Symbols;
using System;
using System.Globalization;
using System.Linq;
using Aran.Aim.Features;
using Aran.Queries;
using Aran.PANDA.Conventional.Racetrack.Procedures;
using Aran.PANDA.Conventional.Racetrack.Controller;
using Aran.AranEnvironment;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Geometries.Operators;
using Aran.PANDA.Conventional.Racetrack.Forms;
using Aran.Aim.Enums;
using Aran.PANDA.Conventional.Racetrack.Properties;
using DoddleReport;
using DoddleReport.Writers;

namespace Aran.PANDA.Conventional.Racetrack
{
	public class MainController : AranPlugin
	{
		private IScreenCapture screenCapture;

		public MainController()
		{
			Altitude4Holding = 4250;
			_minDistInBasicArea = double.NaN;
			_maxDistInBasicArea = double.NaN;
			_tang55Deg = Math.Tan(55 * Math.PI / 180);
			_bufferHandles = new List<int>();
			AreaWithBuffers = new List<Polygon>();
		}

		/// <summary>
		/// Initializes all data and checks for license
		/// </summary>
		/// <returns>Returns string. If there was a error then returns error message, else string.Empty</returns>
		private string Initialize()
		{
			try
			{

				screenCapture = GlobalParams.AranEnvironment.GetScreenCapture(FeatureType.HoldingAssessment.ToString());
				//DsgPntDefined = false;
				GlobalParams.Settings = new Settings();
				GlobalParams.Settings.Load(GlobalParams.AranEnvironment);
				GlobalParams.UnitConverter = new UnitConverter(GlobalParams.Settings);

				//ModuleInstallDir = RegKeys.RegRead<string> ( Registry.CurrentUser, RegKeys.Panda, RegKeys.InstallDirKeyName, System.Reflection.Assembly.GetExecutingAssembly ( ).Location );

				if (GlobalParams.ConstantG == null)
					GlobalParams.ConstantG = new Constants.Constants();

				if (GlobalParams.GeomOperators == null)
					GlobalParams.GeomOperators = new GeometryOperators();

				GlobalParams.SpatialRefOperation = new SpatialReferenceOperation(GlobalParams.AranEnvironment);

				GlobalParams.Database = new DbModule((DbProvider) GlobalParams.AranEnvironment.DbProvider);
				ExtensionFeature.CommonQPI = GlobalParams.Database.HoldingQpi;

				//if ( _speedCategories == null )
				//    _speedCategories = new SpeedCategories ( );
				if (GlobalParams.NavaidDatabase == null)
					GlobalParams.NavaidDatabase = new NavaidsDataBase(GlobalParams.ConstantG.InstallDir);

				//Racetrack.Properties.Resources.Culture = Thread.CurrentThread.CurrentUICulture;
				//Acar = RegFuncs.RegRead<string> ( Microsoft.Win32.Registry.CurrentUser, RegFuncs.Conventional + "\\" + SelectedModul, RegFuncs.LicenseKeyName, null );

				//LicenseRectGeo = DecoderCode.DecodeLCode(Acar, SelectedModul.ToString(), GlobalParams.SpatialRefOperation.SpRefGeo, GlobalParams.SpatialRefOperation.SpRefPrj);
				//LicenseRectPrj = GlobalParams.SpatialRefOperation.ToPrj<Aran.Geometries.MultiPolygon> ( LicenseRectGeo );

				//if ( DecoderCode.LstStDtWriter ( RegFuncs.Conventional + "\\" + SelectedModul.ToString() ) != 0 )
				//	throw new Exception ( "CRITICAL ERROR!!" );

				//if ( LicenseRectGeo == null || LicenseRectGeo.Count == 0 )
				//	throw new Exception ( "ERROR #10LR512" );

				ARANFunctions.InitEllipsoid();
			}
			catch (Exception e)
			{
				return e.Message;
			}

			_frmSettings = new FormSettings(true, true, true, true, true, this);
			_frmMain = new FormMain {Caption = SelectedModul + " " + _selectedMenuItem.Text};

			if (ProcType == ProcedureTypeConv.Vordme)
				_vorDme = new Vordme(this);
			else if (ProcType == ProcedureTypeConv.VorNdb)
				_vorNdb = new Vorndb(this);
			else if (ProcType == ProcedureTypeConv.Vorvor)
				_vorVor = new VorVor(this);

			_frmInfo = new FormInfo(_frmMain.Caption, ProcType);
			_frmAbout = new FormAbout();

			EntryDirection = EntryDirection.Toward;
			Side = SideDirection.sideRight;
			//ProcType = ProcedureTypeConv.VORDME;
			LimDist = new LimitingDistance(this, HasDsgPoint);
			DsgPntSelection = new DesignatedPntSelection(LimDist, ProcType);
			Faclts = new FixFacilities(DsgPntSelection, this);
			Faclts.AddIntersectingDirectionChangedEvent(OnIntersectionDirectionChanged);
			Faclts.AddIntersectionVorListChangedEvent(OnIntersectionVorListChanged);

			SelectedNavPntsPrj = new List<NavaidPntPrj>();
			Faclts.AddNavaidChangedEvent(OnSelectedNavaidChanged);
			//Procedure = new Procedure ( Faclts );
			Speed = new Speed(LimDist, SelectedModul);

			Faclts.AddNavListChangedEvent(OnNavaidListChanged);
			DsgPntSelection.AddtDsgPntListChangedEvent(OnDesignatedPointListChanged);
			DsgPntSelection.AddDsgPntChangedEvent(OnDesignatedPointChanged);
			DsgPntSelection.AddMagVarChangedEvent(OnMagVarChanged);
			DsgPntSelection.AddDirectionChangedEvent(OnDirectionChanged);
			DsgPntSelection.AddIntersectDirectionIntervalChangedEvent(_frmMain.OnIntersectionDirectionIntervalChanged);
			Speed.AddCategoryListChangedEvent(_frmMain.SetCategoryList);
			if (ProcType == ProcedureTypeConv.Vorvor)
			{
				DsgPntSelection.AddNominalDistanceChangedEvent(OnHomingVorDistanceChanged);
				DsgPntSelection.AddIntersectVorDistanceChangedEvent(OnIntersectVorDistChanged);
			}
			else
				DsgPntSelection.AddNominalDistanceChangedEvent(_frmMain.OnDistanceChanged);
			Speed.AddIasIntervalChangedEvent(_frmMain.OnIasIntervalChanged);
			Speed.AddTasChangedEvent(OnTasChanged);

			LimDist.AddTimeChangedEvent(_frmMain.OnTimeChanged);
			LimDist.AddLimDistIntervalChangedEvent(_frmMain.OnLimitDistIntervalChanged);
			LimDist.AddLimDistIntervalChangedEvent(OnLimitDistIntervalChanged);
			LimDist.AddLimDistChangedEvent(OnLimDistChanged);
			LimDist.AddLegLengthChangedEvent(OnLegLengthChanged);

			GuiController = new AppliedGuiValues();
			GuiController.AddAppliedValueChangedHandler(_frmMain.OnAppliedValueChanged);
			_dbResult = new DbResult();
			ProtectionSector1 = new MultiPolygon();
			ProtectionSector2 = new MultiPolygon();
			ProtectionRecipEntry = new MultiPolygon();
			ProtectionInterRadEntryArea = new MultiPolygon();
			ProtectionOmnidirectionEntry = new MultiPolygon();
			return string.Empty;
		}

		#region Set Values

		internal Guid SetNavaid(KeyValuePair<Guid, string> kevValuePair, out string navType)
		{
			SelectedNavaid = _navaidList.Find(nav => nav.Identifier == kevValuePair.Key);
			var elevation = Faclts.GetNavaidElevation();
			var elevation4Gui = GlobalParams.UnitConverter.HeightToDisplayUnits(elevation, eRoundMode.NEAREST);
			_frmInfo.SetElevNavaid(elevation4Gui.ToString(CultureInfo.InvariantCulture));
			navType = "";
			if (SelectedNavaid.Type.HasValue)
				navType = SelectedNavaid.Type.Value.ToString();

		    AltitudeAboveNavaid = AltitudeAboveNavaid - elevation;
		    Faclts.SetAltitude(AltitudeAboveNavaid);
			bool result = Faclts.SetSelectedNavaid(SelectedNavaid);
			if (!result)
				return Guid.Empty;
			_frmInfo.SetNavType(navType.ToString());

			if (ProcType == ProcedureTypeConv.Vordme)
			{
				DefineValidArea4DsgPoint();
				if (DsgPntSelection.SelectedDesignatedPoint != null)
					_vorDme.ConstructToleranceArea(EntryDirection, SelectedNavPntsPrj[1].Value,
						SelectedNavPntsPrj[0].Value,
						DsgPntSelection.NominalDistanceInPrj, DsgPntSelection.NominalDistance,
						DsgPntSelection.Direction, Side,
						DsgPntSelection.SelectedDsgPntPrj, _frmSettings.IsVisibleToleranceArea);
			}
			else if (ProcType == ProcedureTypeConv.Vorvor)
			{
				DrawVorVorRadial();
			}
			GuiController.SetNavaid(SelectedNavaid);
			return SelectedNavaid.Identifier;
		}

		internal Guid SetIntersectingVor(string designator)
		{
			IntersectingVorDsg = designator;
			var navaid = _navaidList.Find(nav => nav.Designator == designator);

			GuiController.SetIntersectingVor(Faclts.IntersectingVor);
			Faclts.SetSelectedIntersectingNavaid(navaid);
			return navaid.Identifier;
		}

		internal void SetMoc(double moc)
		{
			Moc = GlobalParams.UnitConverter.HeightToInternalUnits(moc);
			//Moc = 300;
		}

		private bool IsEqual(List<NavaidPntPrj> appliedNavList, List<NavaidPntPrj> newNavList)
		{
			if (ProcType == ProcedureTypeConv.Vordme)
			{
				return (appliedNavList[0] == newNavList[0] || appliedNavList[1] == newNavList[1]);
			}
			else if (ProcType == ProcedureTypeConv.VorNdb)
			{
				return (appliedNavList[0] == newNavList[0]);
			}
			else if (ProcType == ProcedureTypeConv.Vorvor)
			{

			}
			throw new Exception(Resources.Proc_Type_Not_Found);
		}

		public void SetAltitude(double value)
		{
			if (Faclts.SelectedNavaid == null)
				return;
		    var elevationNavaid = Faclts.GetNavaidElevation();
		    var elevation4Gui = GlobalParams.UnitConverter.HeightToDisplayUnits(elevationNavaid, eRoundMode.NEAREST);
		    _frmInfo.SetElevNavaid(elevation4Gui.ToString(CultureInfo.InvariantCulture));

            Altitude = GlobalParams.UnitConverter.HeightToInternalUnits(value);
            AltitudeAboveNavaid = Altitude - elevationNavaid;
			_frmMain.ChangeSpeedToMach(Altitude > Speed.Height10350 &&
									   (Speed.FlighPhase == FlightPhase.TerminalNormal ||
										Speed.FlighPhase == FlightPhase.TerminalTurbulence));

            //double d1 = GlobalParams.Navaid_Database.DME.MinimalError + GlobalParams.Navaid_Database.DME.ErrorScalingUp * DsgPntSelection.NominalDistanceInGeo;

            if (ProcType == ProcedureTypeConv.Vordme)
				DefineValidArea4DsgPoint();

            DsgPntSelection.SetAltitude(Altitude, elevationNavaid);

			LimDist.SetAltitude(AltitudeAboveNavaid);
			Speed.SetAltitude(Altitude);
			GuiController.SetAltitude(value);
		}

		public void SetNominalDistance(double value)
		{
			DsgPntSelection.SetNominalDistance(value);
			GuiController.SetNominalDistance(value);
		}

		public void SetAircraftCategory(int index)
		{
			Speed.SetAircraftCategory(index);
		}

		public void SetDirection(double directionInDeg)
		{
			GuiController.SetDirection(directionInDeg);
			if (ProcType == ProcedureTypeConv.Vorvor)
			{
				//double elevation = Faclts.GetNavaidElevation();
				Faclts.SetAltitude(AltitudeAboveNavaid);
				Faclts.SetDirection(directionInDeg);
				DrawVorVorRadial();
			}
			else
			{
				DsgPntSelection.SetDirection(directionInDeg);
			}
			DirectionInDeg = (decimal) directionInDeg;
		}

		private void DrawVorVorRadial()
		{
			if (double.IsNaN(DsgPntSelection.Direction) || SelectedNavPntsPrj.Count == 0)
				return;

			SafeDeleteGraphic(_homingVorRadialHandle);
			SafeDeleteGraphic(_homingVorRpsHandle);
			SafeDeleteGraphic(_intersectingVorRadialHandle);
			SafeDeleteGraphic(_intersectingVorRpsHandle);
			SafeDeleteGraphic(_intersectedPntHandle);
			SafeDeleteGraphic(_homingVorCircleHandle);
			SafeDeleteGraphic(_intersectingVorCircleHandle);

			#region Homing Vor graphics

			LineString homingVorRadial = new LineString
			{
				ARANFunctions.LocalToPrj(SelectedNavPntsPrj[0].Value, DsgPntSelection.Direction,
					1.25 * DsgPntSelection.HomingVorDistance, 0),
				SelectedNavPntsPrj[0].Value
			};
			_homingVorRadialHandle = DrawLineString(homingVorRadial, 1, 1);

			LineString homingVorRps = new LineString
			{
				ARANFunctions.LocalToPrj(SelectedNavPntsPrj[0].Value,
					DsgPntSelection.Direction + GlobalParams.NavaidDatabase.Vor.TrackingTolerance,
					1.5 * DsgPntSelection.HomingVorDistance, 0),
				SelectedNavPntsPrj[0].Value,
				ARANFunctions.LocalToPrj(SelectedNavPntsPrj[0].Value,
					DsgPntSelection.Direction - GlobalParams.NavaidDatabase.Vor.TrackingTolerance,
					1.5 * DsgPntSelection.HomingVorDistance, 0)
			};
			_homingVorRpsHandle = DrawLineString(homingVorRps, 1, 1);

			double radius = AltitudeAboveNavaid * Math.Tan(ARANMath.DegToRad(50));
			LineString homingVorCircle = ARANFunctions.CreateCircleAsPartPrj(SelectedNavPntsPrj[0].Value, radius);
			_homingVorCircleHandle = DrawLineString(homingVorCircle, 1, 1);

			#endregion

			#region Intersecting Vor graphics

			LineString intersectingVorRadial = new LineString
			{
				ARANFunctions.LocalToPrj(SelectedNavPntsPrj[1].Value, DsgPntSelection.IntersectionDirection,
					1.25 * DsgPntSelection.IntersectingVorDistance, 0),
				SelectedNavPntsPrj[1].Value
			};
			_intersectingVorRadialHandle = DrawLineString(intersectingVorRadial, 1, 1);

			LineString intersectingVorRps = new LineString
			{
				ARANFunctions.LocalToPrj(SelectedNavPntsPrj[1].Value,
					DsgPntSelection.IntersectionDirection + GlobalParams.NavaidDatabase.Vor.IntersectingTolerance,
					1.5 * DsgPntSelection.IntersectingVorDistance, 0),
				SelectedNavPntsPrj[1].Value,
				ARANFunctions.LocalToPrj(SelectedNavPntsPrj[1].Value,
					DsgPntSelection.IntersectionDirection - GlobalParams.NavaidDatabase.Vor.IntersectingTolerance,
					1.5 * DsgPntSelection.IntersectingVorDistance, 0)
			};
			_intersectingVorRpsHandle = DrawLineString(intersectingVorRps, 1, 1);

			LineString intersectingVorCircle = ARANFunctions.CreateCircleAsPartPrj(SelectedNavPntsPrj[1].Value, radius);
			_intersectingVorCircleHandle = DrawLineString(intersectingVorCircle, 1, 1);

			#endregion

			if (DsgPntSelection.SelectedDsgPntPrj != null)
				_intersectedPntHandle = DrawPoint(DsgPntSelection.SelectedDsgPntPrj, 255);
		}

		internal void SetOverheadDirection(double direction)
		{
			DsgPntSelection.SetDirection(direction, false, false);
			GuiController.SetOverheadDirection(direction);
		}

		public void SetRadiusForDsgPnts(double value)
		{
		    if (Faclts.VorPntPrj == null || Faclts.Dme == null)
		        return;
			//double d1 = 0;
            DsgPntSelection.Radius = GlobalParams.UnitConverter.DistanceToInternalUnits(value);
			if (ProcType == ProcedureTypeConv.Vordme)
				DefineValidArea4DsgPoint();
		}

		public void SetPointChoice(PointChoice choice)
		{
            if (Faclts.VorPntPrj == null || Faclts.Dme == null)
                return;
            DsgPntSelection.ChosenPntType = choice;            
			if (choice == PointChoice.Select && _selectedDsgPnt != null)
				DsgPntSelection.SetDesignatedPoint(_selectedDsgPnt);
			if (ProcType == ProcedureTypeConv.Vordme)
				DefineValidArea4DsgPoint();
		}

		public Guid SetDesignatedPoint(KeyValuePair<Guid, string> dsgPnt, out string typeDsgPnt)
		{
			_selectedDsgPnt = DesignatedPointList.Find(arg => arg.Identifier == dsgPnt.Key);
			typeDsgPnt = "";
			if (_selectedDsgPnt.Type.HasValue)
				typeDsgPnt = _selectedDsgPnt.Type.Value.ToString();
			//_frmInfo.SetTypeDsgPnt ( typeDsgPnt );
			double[] result = DsgPntSelection.SetDesignatedPoint(_selectedDsgPnt);
			GuiController.SetDesignatedPoint(DsgPntSelection.SelectedDsgPntPrj);

			if (ProcType == ProcedureTypeConv.Vorvor)
			{
				_priorFixtolerance = result[1];
				_postFixTolerance = result[0];
				double maxFixToleranceDist = _priorFixtolerance;
				if (_postFixTolerance > maxFixToleranceDist)
					maxFixToleranceDist = _postFixTolerance;
				_frmMain.SetMaxFixToleranceDist(
					GlobalParams.UnitConverter.DistanceToDisplayUnits(maxFixToleranceDist, eRoundMode.NEAREST),
					maxFixToleranceDist < DsgPntSelection.MaxFixToleranceDist);

				_frmInfo.SetWidthToleranceArea(
					GlobalParams.UnitConverter.DistanceToDisplayUnits(_postFixTolerance + _priorFixtolerance,
						eRoundMode.NEAREST));
			}
			return _selectedDsgPnt.Identifier;
		}

		internal void SetDesignatedPoint(Point pnt)
		{
			DsgPntSelection.SetDesignatedPoint(pnt);
			if (_definedDsgPnt)
				GuiController.SetDesignatedPoint(DsgPntSelection.SelectedDsgPntPrj);
		}

		public void SetEntryDirection(bool isToward)
		{
			if (isToward)
			{
				EntryDirection = EntryDirection.Toward;
				DefineValidArea4DsgPoint();
			}
			else
				EntryDirection = EntryDirection.Away;
			LimDist.SetEntryDirection();
			if (ProcType == ProcedureTypeConv.Vorvor || _definedDsgPnt)
				GuiController.SetIsToward(isToward);
		}

		public void SetSideDirection(bool isRight)
		{
			Side = isRight ? SideDirection.sideRight : SideDirection.sideLeft;
			GuiController.SetSide(Side);
		}

		public void SetIas(double ias)
		{
			Speed.SetIas(ias);
			if (ProcType == ProcedureTypeConv.Vordme && EntryDirection == EntryDirection.Away)
				DefineValidArea4DsgPoint();

			GuiController.SetIas(ias);
		}

		public void SetLimitingDistance(double limDist)
		{
			LimDist.SetValue(limDist);
			if (ProcType == ProcedureTypeConv.Vordme)
				DefineValidArea4DsgPoint();
			CalculateLimDistAngle();
			GuiController.SetLimitingDistance(limDist);
		}

		private void CalculateLimDistAngle()
		{
		    Point pnt = null;
		    if (Faclts.SelectedNavaid.Location != null)
		        pnt = Faclts.SelectedNavaid.Location.Geo;
		    else
		    {
		        if (Faclts.VorPntGeo != null)
		            pnt = Faclts.VorPntGeo;
		        else if (Faclts.DmePntGeo != null)
		            pnt = Faclts.DmePntGeo;
		        else
		            throw new Exception($"{Faclts.SelectedNavaid.Name} has no point");
		    }

            Point navPnt = GlobalParams.SpatialRefOperation.ToPrj<Point>(pnt);
			double direction = ARANFunctions.ReturnAngleInDegrees(navPnt,
				ARANFunctions.LocalToPrj(navPnt, DsgPntSelection.Direction, LimDist.ValueInPrj, 0));
			ARANFunctions.DirToAzimuth(navPnt, direction, GlobalParams.SpatialRefOperation.SpRefPrj,
				GlobalParams.SpatialRefOperation.SpRefGeo);
		}

		public void SetTime(double timeInMinute, bool defineValidArea = true)
		{
		    if (ProcType == ProcedureTypeConv.Vordme && (Faclts.Dme == null || Faclts.VorPntPrj == null))
		        return;
            LimDist.SetTime(timeInMinute);
			if (ProcType == ProcedureTypeConv.Vordme && defineValidArea)
				DefineValidArea4DsgPoint();
			//_time.SetValue ( timeInMinute, false );
			GuiController.SetTime(timeInMinute);
		}

		public void SetWithLimitingRadial(bool isWithLimitingRadial)
		{
			_vorDme.SetWithLimitingRadial(isWithLimitingRadial);
			GuiController.SetIsWithLimitingRadial(isWithLimitingRadial);
		}

		internal void SetIntersectingVorRadial(double direction)
		{
			_intersectingVorRadial = direction;
			GuiController.SetIntersectingVorRadial(direction);

			double[] result = DsgPntSelection.SetIntersectingVorRadial(direction);

			if (result == null)
				return;
			_priorFixtolerance = result[1];
			_postFixTolerance = result[0];
			double maxFixToleranceDist = _priorFixtolerance;
			if (_postFixTolerance > maxFixToleranceDist)
				maxFixToleranceDist = _postFixTolerance;
			_frmMain.SetMaxFixToleranceDist(
				GlobalParams.UnitConverter.DistanceToDisplayUnits(maxFixToleranceDist, eRoundMode.NEAREST),
				maxFixToleranceDist < DsgPntSelection.MaxFixToleranceDist);

			_frmInfo.SetWidthToleranceArea(
				GlobalParams.UnitConverter.DistanceToDisplayUnits(_postFixTolerance + _priorFixtolerance,
					eRoundMode.NEAREST));
			DrawVorVorRadial();
		}

		internal void SetDsgPntDefinitionVia(DsgPntDefinition dsgPntDefinition)
		{
			DsgPntSelection.SetPntDefinition4VorVor(dsgPntDefinition);
			_dsgPntDefiniton4VorVor = dsgPntDefinition;
		}

		internal void SetFixToleranceDist(decimal value)
		{
			DsgPntSelection.SetFixToleranceDist((double) value);
		}

		internal void SetLowerLimitHoldingPattern(decimal value)
		{
			LowerLimitHldngPattern = (double) value;
		}

		public void SetLimitingRadial(double angle)
		{
			_frmInfo.SetLimitingRadial(angle);
		}

		internal void SetFlightPhase(FlightPhase flightPhase)
		{
			Speed.SetFlightPhase(flightPhase);
		}

		internal void SetTurbulence(bool isTurbulence)
		{
			_isTurbulence = isTurbulence;
			Speed.IsTurbulence = isTurbulence;
		}

		#endregion

		#region Events

		private void OnIntersectionVorListChanged(object sender, NavaidListEventArg argNavList)
		{
			_frmMain.AddIntersectingVorList(argNavList.NavaidList.Select(navaid => navaid.Designator).ToList(),
				(int) sender);
		}

		private void OnHomingVorDistanceChanged(object sender, DistanceEventArg argDist)
		{
			_frmInfo.SetHomingVorDist(argDist.DistanceForGui);
		}

		private void OnIntersectVorDistChanged(object sender, DistanceEventArg argDist)
		{
			_frmInfo.SetIntersectingVorDist(argDist.DistanceForGui);
		}

		private void OnNavaidListChanged(object sender, NavaidListEventArg argNavList)
		{
			_navaidList = argNavList.NavaidList;
			int index = 0;
			if (SelectedNavaid != null)
			{
				index = _navaidList.FindIndex(nav => nav.Identifier.Equals(SelectedNavaid.Identifier));
				if (index == -1)
					index = 0;
			}

			_frmMain.AddNavaids(_navaidList.ToDictionary(nav => nav.Identifier, nav => nav.Designator), index);
		}

		private void OnDesignatedPointListChanged(object sender, DsgPntListEventArg argDsgPntList)
		{
			DesignatedPointList = argDsgPntList.DsgPntList;
			int selectedIndex = 0;
			if (_selectedDsgPnt != null)
			{
                selectedIndex = DesignatedPointList.
                    FindIndex(dsgPnt => dsgPnt.Identifier == _selectedDsgPnt.Identifier &&
                    dsgPnt.TimeSlice != null && _selectedDsgPnt.TimeSlice != null &&
                    dsgPnt.TimeSlice.SequenceNumber == _selectedDsgPnt.TimeSlice.SequenceNumber &&
                    dsgPnt.TimeSlice.CorrectionNumber == _selectedDsgPnt.TimeSlice.CorrectionNumber);
				if (selectedIndex == -1)
					selectedIndex = 0;
			}
			Dictionary<Guid, string> dictDsgPnt =
				DesignatedPointList.ToDictionary(dsgPnt => dsgPnt.Identifier, dsgPnt => dsgPnt.Designator);
			_frmMain.AddDesignatedPointList(dictDsgPnt, selectedIndex);

			if (DesignatedPointList == null)
				SafeDeleteGraphic(_handleDesignatedPoint);
		}

		private void OnDesignatedPointChanged(object sender, DsgPntEventArg argDsgPnt)
		{

			if (ProcType != ProcedureTypeConv.Vorvor)
				DrawDesigntdPoint(argDsgPnt.DsgPntPrj);
			_dsgPntChanged(null, argDsgPnt);
			if (ProcType == ProcedureTypeConv.Vordme && DsgPntSelection.ChosenPntType == PointChoice.Create)
			{
				//double elevation = Converters.ConverterToSI.Convert(Faclts.SelectedNavaid.Location.Elevation, 0);
			    //Altitude = Altitude - Faclts.GetNavaidElevation();
				double minDist = AltitudeAboveNavaid * _tang55Deg;
				double maxDist = 4130 * Math.Sqrt(AltitudeAboveNavaid);
				double distance = ARANFunctions.ReturnDistanceInMeters(argDsgPnt.DsgPntPrj, Faclts.VorPntPrj);
				if (EntryDirection == EntryDirection.Toward)
				{
					//double d1 = GlobalParams.Navaid_Database.DME.MinimalError + GlobalParams.Navaid_Database.DME.ErrorScalingUp * minDist;
					//minDist += d1;
					//maxDist -= d1;
					minDist = (GlobalParams.NavaidDatabase.Dme.MinimalError + minDist) /
							  (1 - GlobalParams.NavaidDatabase.Dme.ErrorScalingUp);
					maxDist = (maxDist - GlobalParams.NavaidDatabase.Dme.MinimalError) /
							  (1 + GlobalParams.NavaidDatabase.Dme.ErrorScalingUp);
				}
				else if (EntryDirection == EntryDirection.Away)
				{
					minDist += _minDistInBasicArea;
					maxDist -= _maxDistInBasicArea;

					// Area for dsg Pnt should be calculated correctly in case of EntryDirection is Away
					//This is temporary code
					minDist = distance * 0.5;
					maxDist = 2 * distance;
					//
				}
				if (distance < minDist || distance > maxDist)
				{
					_definedDsgPnt = false;
					_frmMain.EnableConstructBtn(false, Resources.Pnt_Definition_Warning);
				}
				else
				{
					_frmMain.EnableConstructBtn(true);
					_definedDsgPnt = true;
				}
			}
			else if (ProcType == ProcedureTypeConv.Vorvor && Faclts.SelectedIntersectingNavaid != null)
			{
				DrawVorVorRadial();
				//Faclts.CheckIntersectNavaid ( Altitude );
				//DsgPntSelection.CalculateIntersectionVorRadial ( );
			}
			if (ProcType == ProcedureTypeConv.Vordme && _frmSettings.IsVisibleToleranceArea)
			{
				_vorDme.ConstructToleranceArea(EntryDirection, SelectedNavPntsPrj[1].Value, SelectedNavPntsPrj[0].Value,
					DsgPntSelection.NominalDistanceInPrj, DsgPntSelection.NominalDistance, DsgPntSelection.Direction,
					Side, argDsgPnt.DsgPntPrj, true);
			}
		}

		private void OnLegLengthChanged(object sender, CommonEventArg argLegLength)
		{
			_frmInfo.SetLegLength(argLegLength.Value.ToString(CultureInfo.InvariantCulture));
		}

		private void OnTasChanged(object sender, CommonEventArg arg)
		{
			_frmInfo.SetTas(arg.Value);
		}

		private void OnIntersectionDirectionChanged(object sender, DirectionEventArg arg)
		{
			_intersectionDirectionChanged(sender, arg);
		}

		private void OnLimitDistIntervalChanged(object sender, SpeedInterval interval)
		{
			if (double.IsNaN(interval.Min) && double.IsNaN(interval.Max))
				_definedDsgPnt = false;
			else
				_definedDsgPnt = true;
		}

		private void OnLimDistChanged(object sender, CommonEventArg arg)
		{
			_limDistChanged(sender, arg);
			CalculateLimDistAngle();
		}

		private void OnMagVarChanged(object sender, CommonEventArg arg)
		{
			if (arg != null)
				_frmInfo.SetMagVar(arg.Value);
			else
				MessageBox.Show("Magnetic variation of navaid is null. /r/nPlease, fill in these field in database");
		}

		private void OnDirectionChanged(object sender, DirectionEventArg argDir)
		{
			_frmMain.OnDirectionChanged(sender, argDir);
			DirectionInDeg = (decimal) ARANMath.Modulus(argDir.DirectionForGui, 360);
		}

		private void OnSelectedNavaidChanged(object sender, NavaidEventArg arg)
		{
			SelectedNavPntsPrj.Clear();
			foreach (NavaidPntPrj navPntPrj in arg.NavaidPntPrjList)
				SelectedNavPntsPrj.Add(navPntPrj);
			SafeDeleteGraphic(_handleForNav1);
			if (SelectedNavPntsPrj == null && SelectedNavPntsPrj.Count == 0)
				return;

			if (SelectedNavPntsPrj[0].Value != null)
				_handleForNav1 = DrawPoint(SelectedNavPntsPrj[0].Value, 0);

			SafeDeleteGraphic(_handleForNav2);
			if (SelectedNavPntsPrj.Count > 1 && SelectedNavPntsPrj[1].Value != null &&
				SelectedNavPntsPrj[1].Type != NavType.Dme)
				_handleForNav2 = DrawPoint(SelectedNavPntsPrj[1].Value, 0);

			if (ProcType == ProcedureTypeConv.VorNdb)
				SafeDeleteGraphic(_handleDesignatedPoint);
		}

		#endregion

		#region IAranPlugin Members

		public override void Startup(IAranEnvironment aranEnv)
		{
			GlobalParams.AranEnvironment = aranEnv;
			GlobalParams.Logger = GlobalParams.AranEnvironment.GetLogger("Conventional_Holding");

			_holdingMenuItem = new ToolStripMenuItem {Text = Resources.MainController_Startup_Conventional_Holding};
			//_raceTrackMenuItem.CheckOnClick = true;
			//_raceTrackMenuItem.Click += new EventHandler ( RacetrackMenuItem_Clicked );

			ToolStripMenuItem tsmItemVorDme = new ToolStripMenuItem
			{
				Text = Resources.MainController_Startup_VOR_DME,
				CheckOnClick = true
			};
			tsmItemVorDme.Click += new EventHandler(tsmItemVorDme_Click);
			_holdingMenuItem.DropDownItems.Add(tsmItemVorDme);

			ToolStripMenuItem tsmItemOverhead =
				new ToolStripMenuItem {Text = Resources.MainController_Startup_Overhead};
			tsmItemOverhead.Click += new EventHandler(tsmItemOverhead_Click);
			tsmItemOverhead.CheckOnClick = true;
			_holdingMenuItem.DropDownItems.Add(tsmItemOverhead);

			ToolStripMenuItem tsmItemVorVor = new ToolStripMenuItem
			{
				Text = Resources.MainController_Startup_VOR_VOR,
				CheckOnClick = true
			};
			tsmItemVorVor.Click += new EventHandler(tsmItemVorVor_Click);
			_holdingMenuItem.DropDownItems.Add(tsmItemVorVor);

			GlobalParams.AranEnvironment.AranUI.AddMenuItem(AranMapMenu.Applications, _holdingMenuItem);

			_racetrackMenuItem = new ToolStripMenuItem {Text = Resources.MainController_Startup_Conventional_Racetrack};

			ToolStripMenuItem tsmItemRacetrackVorDme = new ToolStripMenuItem
			{
				Text = Resources.MainController_Startup_VOR_DME,
				CheckOnClick = true
			};
			tsmItemRacetrackVorDme.Click += new EventHandler(tsmItemRacetrackVorDme_Click);
			_racetrackMenuItem.DropDownItems.Add(tsmItemRacetrackVorDme);

			ToolStripMenuItem tsmItemRacetrackOverhead =
				new ToolStripMenuItem {Text = Resources.MainController_Startup_Overhead};
			tsmItemRacetrackOverhead.Click += new EventHandler(tsmItemRacetrackOverhead_Click);
			tsmItemRacetrackOverhead.CheckOnClick = true;
			_racetrackMenuItem.DropDownItems.Add(tsmItemRacetrackOverhead);

			ToolStripMenuItem tsmItemRacetrackVorVor = new ToolStripMenuItem
			{
				Text = Resources.MainController_Startup_VOR_VOR,
				CheckOnClick = true
			};
			tsmItemRacetrackVorVor.Click += new EventHandler(tsmItemRacetrackVorVor_Click);
			_racetrackMenuItem.DropDownItems.Add(tsmItemRacetrackVorVor);

			//GlobalParams.AranEnvironment.AranUI.AddMenuItem ( AranMapMenu.Applications, _racetrackMenuItem );
		}

        internal void SetMaximumTime(double maximum)
        {
            LimDist.SetMaximumTime(maximum);
        }

        void tsmItemVorVor_Click(object sender, EventArgs e)
		{
			SelectedModul = ModulType.Holding;
			_bufferWidth = 1900;
			Item_Clicked(ProcedureTypeConv.Vorvor, (ToolStripMenuItem) sender);
		}

		void tsmItemOverhead_Click(object sender, EventArgs e)
		{
			SelectedModul = ModulType.Holding;
			_bufferWidth = 1900;
			Item_Clicked(ProcedureTypeConv.VorNdb, (ToolStripMenuItem) sender);
		}

		void tsmItemVorDme_Click(object sender, EventArgs e)
		{
			SelectedModul = ModulType.Holding;
			_bufferWidth = 1900;
			Item_Clicked(ProcedureTypeConv.Vordme, (ToolStripMenuItem) sender);
		}

		void tsmItemRacetrackVorVor_Click(object sender, EventArgs e)
		{
			SelectedModul = ModulType.Racetrack;
			_bufferWidth = 4600;
			Item_Clicked(ProcedureTypeConv.Vorvor, (ToolStripMenuItem) sender);
		}

		void tsmItemRacetrackOverhead_Click(object sender, EventArgs e)
		{
			SelectedModul = ModulType.Racetrack;
			_bufferWidth = 4600;
			Item_Clicked(ProcedureTypeConv.VorNdb, (ToolStripMenuItem) sender);
		}

		void tsmItemRacetrackVorDme_Click(object sender, EventArgs e)
		{
			SelectedModul = ModulType.Racetrack;
			_bufferWidth = 4600;
			Item_Clicked(ProcedureTypeConv.Vordme, (ToolStripMenuItem) sender);
		}

		public override string Name => "Conventional " + SelectedModul;
		
		private void Item_Clicked(ProcedureTypeConv procType, ToolStripMenuItem menuItem)
		{
			_selectedMenuItem = menuItem;
			ProcType = procType;
			GlobalParams.Logger.Info(Name + " is starting ...");


            var slotSelector = true;

            var dbProvider = GlobalParams.AranEnvironment.DbProvider as DbProvider;
            if (dbProvider != null && dbProvider.ProviderType == DbProviderType.TDB)
            {
                dynamic methodResult = new System.Dynamic.ExpandoObject();
                dbProvider.CallSpecialMethod("SelectSlot", methodResult);
                slotSelector = methodResult.Result;
            }

            if (!slotSelector)
            {
                MessageBox.Show("Please first select slot!", "Conventional Holding", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_selectedMenuItem.Checked)
			{
				string errorStr = Initialize();
			    if (errorStr == string.Empty)
			    {
			        AranTool aranToolItem = new AranTool {Visible = false};
			        GlobalParams.AranMapToolMenuItem = aranToolItem;
			        aranToolItem.Cursor = Cursors.Cross;

			        GlobalParams.AranEnvironment.AranUI.AddMapTool(GlobalParams.AranMapToolMenuItem);

			        #region Set EventHandlers

			        _dsgPntChanged = _frmMain.OnDesignatedPointChanged;
			        _reAngleChanged = _frmMain.OnRE_AngleChanged;
			        _intersectionDirectionChanged = _frmMain.OnIntersectionDirectionChanged;
			        _appliedValueChanged = _frmMain.OnAppliedValueChanged;
			        _limDistChanged = _frmMain.OnLimDistChanged;

			        #endregion

			        GlobalParams.AranMapToolMenuItem.MouseClickedOnMap += _frmMain.OnMouseClickedOnMap;
			        GlobalParams.AranMapToolMenuItem.Deactivated += _frmMain.OnDeacitvatedPointPickerTool;

			        AirportHeliport airport =
			            GlobalParams.Database.HoldingQpi.GetAirportHeliport(GlobalParams.Settings.Aeroport);
			        if (airport == null)
			            throw new Exception(Resources.AirportHeliport_Definition_Error);

			        if (airport.Name != null && airport.Designator != null)
			            AirportName = airport.Name.ToUpper() + " (" + airport.Designator + ")";
			        else if (airport.Name != null)
			            AirportName = airport.Name.ToUpper();
			        else if (airport.Designator != null)
			            AirportName = airport.Designator;
			        if (ProcType == ProcedureTypeConv.Vorvor)
			            _frmSettings.SetFixToleranceDist(true,
			                GlobalParams.UnitConverter.DistanceToDisplayUnits(3700, eRoundMode.CEIL));
			        else
			            _frmSettings.SetFixToleranceDist(false);

			        _frmMain.Text = _frmMain.Caption + " - " + Resources.AirportHeliport + " : " + AirportName;
			        _arpPnt = airport.ARP.Geo;


                    _frmMain.Show(GlobalParams.AranEnvironment.Win32Window, this);
			        EnableMenuItems();
                }
				else
				{
					MessageBox.Show(errorStr);
					_selectedMenuItem.Checked = false;
				}
			}
			else
			{
				_frmMain?.Close();
				EnableMenuItems();
			}
		}

		private void EnableMenuItems()
		{
			if (_selectedMenuItem.Checked)
			{
				var selectedMainMenu = _holdingMenuItem;
				var otherMainMenu = _racetrackMenuItem;
				if (SelectedModul == ModulType.Racetrack)
				{
					selectedMainMenu = _racetrackMenuItem;
					otherMainMenu = _holdingMenuItem;
				}
				foreach (var item in selectedMainMenu.DropDownItems)
				{
					if (item != _selectedMenuItem)
						((ToolStripMenuItem) item).Enabled = false;
				}
				foreach (var item in otherMainMenu.DropDownItems)
				{
					((ToolStripMenuItem) item).Enabled = false;
				}
			}
			else
			{
				foreach (var item in _holdingMenuItem.DropDownItems)
					((ToolStripMenuItem) item).Enabled = true;

				foreach (var item in _racetrackMenuItem.DropDownItems)
					((ToolStripMenuItem) item).Enabled = true;
			}
		}

		public override Guid Id => new Guid("b0044eeb-757e-43f5-919e-a3a7d2a71075");

		public override List<Aim.FeatureType> GetLayerFeatureTypes()
		{
			List<Aim.FeatureType> result = new List<Aim.FeatureType>
			{
				Aim.FeatureType.DesignatedPoint,
				Aim.FeatureType.Navaid,
				Aim.FeatureType.VOR,
				Aim.FeatureType.DME,
				Aim.FeatureType.NDB,
				Aim.FeatureType.VerticalStructure
			};
			return result;
		}

        #endregion


	    internal void ShowReportWindow()
	    {
	        _frmReport.Show(new Win32Windows(_frmMain.Handle.ToInt32()));
	    }

	    public void SetError(string errorMessage, bool closeMainWindow)
	    {
	        GlobalParams.Logger.Error(errorMessage);
	        if (closeMainWindow)
	        {
	            MessageBox.Show(errorMessage);
	            //Disposing = true;
	            //_frmMain.Dispose();
	            //_frmMain.Close();
	        }
	    }

	    public void SetOutboundLegDefinition(bool viaTime)
	    {
	        OutboundDefinitionViaTime = viaTime;
	    }

        private void DefineValidArea4DsgPoint()
		{
			if (ProcType == ProcedureTypeConv.Vordme)
			{
			    if (Faclts.VorPntPrj == null)
			        return;
				if (AltitudeAboveNavaid <= 0 || DsgPntSelection.ChosenPntType == PointChoice.None)
					return;

				//double elevation = Converters.ConverterToSI.Convert(Faclts.SelectedNavaid.Location.Elevation, 0);
			    //Altitude -= Faclts.GetNavaidElevation();
				double notCoverageMinDist = AltitudeAboveNavaid * _tang55Deg;
				double notCoverageMaxDist = 4130 * Math.Sqrt(AltitudeAboveNavaid);

				if (EntryDirection == EntryDirection.Toward)
				{
					//double d1 = double.NaN;
					double minDist = (GlobalParams.NavaidDatabase.Dme.MinimalError + notCoverageMinDist) /
									 (1 - GlobalParams.NavaidDatabase.Dme.ErrorScalingUp);
					//d1 = GlobalParams.Navaid_Database.DME.MinimalError + GlobalParams.Navaid_Database.DME.ErrorScalingUp * notCoverageMinDist;
					double maxDist = (notCoverageMaxDist - GlobalParams.NavaidDatabase.Dme.MinimalError) /
									 (1 + GlobalParams.NavaidDatabase.Dme.ErrorScalingUp);
					switch (DsgPntSelection.ChosenPntType)
					{
						case PointChoice.None:
							break;
						case PointChoice.Create:
							if (AltitudeAboveNavaid > 0)
							{
								DrawMin_MaxDistCircle(Faclts.VorPntPrj, notCoverageMinDist, notCoverageMaxDist,
									minDist - notCoverageMinDist, notCoverageMaxDist - maxDist);
								_frmMain.SetInterval4DsgDistance(
									GlobalParams.UnitConverter.DistanceToDisplayUnits(minDist, eRoundMode.NEAREST),
									GlobalParams.UnitConverter.DistanceToDisplayUnits(maxDist, eRoundMode.NEAREST));
							}
							break;
						case PointChoice.Select:
							if (AltitudeAboveNavaid > 0)
							{
								DsgPntSelection.MinDist4DsgPnt = minDist;
								_frmMain.SetInterval4DsgSelection(
									GlobalParams.UnitConverter.DistanceToDisplayUnits(minDist, eRoundMode.NEAREST),
									GlobalParams.UnitConverter.DistanceToDisplayUnits(maxDist, eRoundMode.NEAREST));
							}
							if (DsgPntSelection.Radius > 0)
								DrawMin_MaxDistCircle(Faclts.VorPntPrj, notCoverageMinDist, DsgPntSelection.Radius,
									minDist - notCoverageMinDist);
							break;
						default:
							break;
					}
				}
				else if (EntryDirection == EntryDirection.Away)
				{
					//_vorDme.FindAwayMinMaxDistInBasicArea ( Side, SelectedNavPntsPrj[ 1 ].Value, LimDist.TimeInMin, Speed.TAS, LimDist.LegLength, Altitude, false, false, false, notCoverageMinDist, out _minDistInBasicArea );
					//_maxDistInBasicArea = _minDistInBasicArea;
					switch (DsgPntSelection.ChosenPntType)
					{
						case PointChoice.None:
							break;
						case PointChoice.Create:
							if (AltitudeAboveNavaid > 0)
							{
								DrawMin_MaxDistCircle(Faclts.VorPntPrj, notCoverageMinDist, notCoverageMaxDist);
								_frmMain.SetInterval4DsgDistance(
									GlobalParams.UnitConverter.DistanceToDisplayUnits(notCoverageMinDist,
										eRoundMode.NEAREST),
									GlobalParams.UnitConverter.DistanceToDisplayUnits(notCoverageMaxDist,
										eRoundMode.NEAREST));
							}
							break;
						case PointChoice.Select:
							if (DsgPntSelection.Radius > 0)
							{
								//DsgPntSelection.MinDist4DsgPnt = notCoverageMinDist + _minDistInBasicArea;
								DsgPntSelection.MinDist4DsgPnt = notCoverageMinDist;
								//_frmMain.SetInterval4DsgSelection ( GlobalParams.UnitConverter.DistanceToDisplayUnits ( notCoverageMinDist + _minDistInBasicArea, eRoundMode.NERAEST ), GlobalParams.UnitConverter.DistanceToDisplayUnits ( notCoverageMaxDist - _maxDistInBasicArea, eRoundMode.NERAEST ) );
								_frmMain.SetInterval4DsgSelection(
									GlobalParams.UnitConverter.DistanceToDisplayUnits(notCoverageMinDist,
										eRoundMode.NEAREST),
									GlobalParams.UnitConverter.DistanceToDisplayUnits(notCoverageMaxDist,
										eRoundMode.NEAREST));

								//DrawMin_MaxDistCircle ( Faclts.VorPntPrj, notCoverageMinDist, DsgPntSelection.Radius, _minDistInBasicArea );
								DrawMin_MaxDistCircle(Faclts.VorPntPrj, notCoverageMinDist, notCoverageMaxDist);
							}
							break;
						default:
							break;
					}

				}
			}
			//else
			//    DeleteMin_MaxDistCircle ( );
		}

		internal string CalculateMinimumSpeed()
		{
			return Speed.CalculateMinimumSpeed();
		}

		internal string GetSpeedIntervalString()
		{
			return Speed.GetSpeedIntervalString();
		}

		internal string CalculateMach(double value)
		{
			return Speed.AdaptToStandartSpeedValue(
					   GlobalParams.UnitConverter.SpeedToDisplayUnits(Speed.CalculateMach(0.8))) + " " +
				   GlobalParams.UnitConverter.SpeedUnit;
		}

		public void GetNavaidList()
		{
			Faclts.SetArp(_arpPnt.X, _arpPnt.Y);
			Faclts.SetRadius(GlobalParams.Settings.Radius);

			if (ProcType == ProcedureTypeConv.Vordme)
			{
				Faclts.SetServiceTypes(new CodeNavaidService[] {CodeNavaidService.VOR_DME}, ProcType);
			}
			else if (ProcType == ProcedureTypeConv.VorNdb)
			{

				Faclts.SetServiceTypes(new CodeNavaidService[]
					{
						CodeNavaidService.VOR, CodeNavaidService.NDB,
						CodeNavaidService.VOR_DME, CodeNavaidService.VORTAC,
						CodeNavaidService.NDB_DME, CodeNavaidService.NDB_MKR
					},
					ProcType);
			}
			else if (ProcType == ProcedureTypeConv.Vorvor)
			{
				Faclts.SetServiceTypes(
					new CodeNavaidService[]
						{CodeNavaidService.VOR, CodeNavaidService.VOR_DME, CodeNavaidService.VORTAC}, ProcType);
			}

			//decimal altitude = GlobalParams.UnitConverter.HeightUnit == "ft" ? 10000 : 3050;

            //decimal radius4DsgPnt = GlobalParams.UnitConverter.DistanceUnit == "NM" ? 81 : 150;
		    decimal radius4DsgPnt =
		        ((HorizantalDistanceType) GlobalParams.UnitConverter.DistanceUnitIndex) == HorizantalDistanceType.NM
		            ? 81
		            : 150;
		    decimal altitude = ((VerticalDistanceType) GlobalParams.UnitConverter.HeightUnitIndex) == VerticalDistanceType.Ft
		        ? 10000
		        : 3050;


            _frmMain.InitializeParams(altitude, radius4DsgPnt, 1);

			InitializeGuiValues();
		}

		internal double CalculateIntersectRadial(out double fixToleranceDist)
		{
			double result = DsgPntSelection.CalculateIntersectionVorRadial4Gui(out fixToleranceDist);
			DrawVorVorRadial();
			return ARANMath.Modulus(Math.Round(result), 360);
		}

		private void InitializeGuiValues()
		{
			if (ProcType == ProcedureTypeConv.Vordme)
			{
				GuiController.SetDirection(ARANMath.RadToDeg(DsgPntSelection.Direction));
				GuiController.SetIsToward(EntryDirection == EntryDirection.Toward);
			}
			GuiController.SetLimitingDistance(
				GlobalParams.UnitConverter.DistanceToDisplayUnits(LimDist.ValueInPrj, eRoundMode.NONE));
			GuiController.SetNominalDistance(
				GlobalParams.UnitConverter.DistanceToDisplayUnits(DsgPntSelection.NominalDistanceInPrj,
					eRoundMode.NONE));
			GuiController.SetSide(Side);
			GuiController.SetProcType(ProcType);
		}

		public void ConstructBasicArea()
		{
			if (!GuiController.IsEqualPrevious)
			{
				switch (ProcType)
				{
					case ProcedureTypeConv.Vordme:
						if (EntryDirection == EntryDirection.Toward)
						{
							BasicArea = _vorDme.TowardConstructBasicArea(Side, SelectedNavPntsPrj[1].Value,
								SelectedNavPntsPrj[0].Value,
								DsgPntSelection.SelectedDsgPntPrj, DsgPntSelection.Direction + ARANMath.C_PI,
								DsgPntSelection.NominalDistanceInPrj, DsgPntSelection.NominalDistanceInGeo,
								LimDist.ValueInPrj, LimDist.ValueInGeo, LimDist.TimeInMin, Speed.Tas,
								LimDist.LegLength, AltitudeAboveNavaid, LimDist.Radius, _frmSettings.IsVisibleShablon,
								_frmSettings.IsVisibleNominalTrajectory, _frmSettings.IsVisibleToleranceArea);
						}
						else if (EntryDirection == EntryDirection.Away)
						{
							BasicArea = _vorDme.AwayConstructBasicArea(Side, SelectedNavPntsPrj[1].Value,
								SelectedNavPntsPrj[0].Value,
								DsgPntSelection.SelectedDsgPntPrj, DsgPntSelection.Direction,
								DsgPntSelection.NominalDistanceInPrj,
								DsgPntSelection.NominalDistanceInGeo, LimDist.ValueInPrj, LimDist.ValueInGeo,
								LimDist.TimeInMin,
								Speed.Tas, LimDist.LegLength, AltitudeAboveNavaid, LimDist.Radius, _frmSettings.IsVisibleShablon,
								_frmSettings.IsVisibleNominalTrajectory, _frmSettings.IsVisibleToleranceArea);

							//double elevation = Converters.ConverterToSI.Convert(Faclts.SelectedNavaid.Location.Elevation, 0);
							//Altitude -= Faclts.GetNavaidElevation();
							double notCoverageMinDist = AltitudeAboveNavaid * _tang55Deg;
							LineString lnStringMinCircle =
								ARANFunctions.CreateCircleAsPartPrj(Faclts.VorPntPrj, notCoverageMinDist);
							Geometry geom = GlobalParams.GeomOperators.Intersect(BasicArea, lnStringMinCircle);
							if (geom != null && !geom.IsEmpty)
							{
								LineString lnStrngPerpendic = new LineString
								{
									ARANFunctions.LocalToPrj(Faclts.VorPntPrj,
										DsgPntSelection.Direction - ARANMath.C_PI_2, 100000, 0),
									ARANFunctions.LocalToPrj(Faclts.VorPntPrj,
										DsgPntSelection.Direction + ARANMath.C_PI_2, 100000, 0)
								};
								//DrawLineString ( lnStrngPerpendic, 1, 1 );

								MultiPolygon mltPolygonIntersect =
									ARANFunctions.CreateCircleAsMultiPolyPrj(Faclts.VorPntPrj, notCoverageMinDist);
								geom = GlobalParams.GeomOperators.Intersect(BasicArea, mltPolygonIntersect);

								if (geom is MultiPolygon)
									DrawMultiPolygon(((MultiPolygon) geom), 1, eFillStyle.sfsCross);

								double distFromIntersecGeom =
									GlobalParams.GeomOperators.GetDistance(geom, lnStrngPerpendic);

								LineString lnStrngPerpendic2 = new LineString
								{
									ARANFunctions.LocalToPrj(Faclts.VorPntPrj,
										ARANMath.Modulus(DsgPntSelection.Direction - ARANMath.C_PI_2, ARANMath.C_2xPI),
										100000, distFromIntersecGeom),
									ARANFunctions.LocalToPrj(Faclts.VorPntPrj,
										DsgPntSelection.Direction + ARANMath.C_PI_2, 100000,
										-distFromIntersecGeom)
								};
								//DrawLineString ( lnStrngPerpendic2, 1, 1 );

								LineString lnStr = new LineString
								{
									ARANFunctions.LocalToPrj(Faclts.VorPntPrj, DsgPntSelection.Direction,
										notCoverageMinDist, 100000),
									ARANFunctions.LocalToPrj(Faclts.VorPntPrj, DsgPntSelection.Direction,
										notCoverageMinDist, -100000)
								};
								//DrawLineString ( lnStr, 1, 1 );

								distFromIntersecGeom = notCoverageMinDist - distFromIntersecGeom;
								//if ( geom is MultiLineString )
								//{
								//	MultiLineString mltLnString = ( MultiLineString ) geom;
								//	DrawMultiLineString ( mltLnString, 255 * 255 * 255, 3 );
								//}
								_frmMain.WarnUserWrongParameters(
									GlobalParams.UnitConverter.DistanceToDisplayUnits(distFromIntersecGeom,
										eRoundMode.NEAREST));
							}
						}
						break;
					case ProcedureTypeConv.VorNdb:
						BasicArea = _vorNdb.ConstructBasicArea(SelectedNavPntsPrj[0], AltitudeAboveNavaid,
							DsgPntSelection.Direction, Speed.Tas, Side, LimDist.TimeInMin,
							_frmSettings.IsVisibleShablon, _frmSettings.IsVisibleNominalTrajectory,
							_frmSettings.IsVisibleToleranceArea);
						break;
					case ProcedureTypeConv.Vorvor:
						BasicArea = _vorVor.ConstructBasicArea(SelectedNavPntsPrj[0].Value, SelectedNavPntsPrj[1].Value,
							AltitudeAboveNavaid, DsgPntSelection.Direction, Faclts.IntersectionDirection, Speed.Tas, Side,
							LimDist.TimeInMin, DsgPntSelection.SelectedDsgPntPrj, EntryDirection,
							_frmSettings.IsVisibleShablon, _frmSettings.IsVisibleNominalTrajectory,
							_frmSettings.IsVisibleToleranceArea);
						break;
				}
			}

			foreach (var item in _bufferHandles)
			{
				SafeDeleteGraphic(item);
			}
			SafeDeleteGraphic(_handleBasicArea);
			SafeDeleteGraphic(_handlePrtctSector1);
			SafeDeleteGraphic(_handlePrtctSector2);
			SafeDeleteGraphic(_handlePrtctRecipDir2SecPnt);
            if (BasicArea != null && !BasicArea.IsEmpty)
			{
				_handleBasicArea = DrawPolygon(BasicArea, 0, eFillStyle.sfsHollow);
			}
			GuiController.Constructed = true;

			_reportCalculated = false;
			_calculatedPrtctSctr1 = false;
			_calculatedPrtctSctr2 = false;
			_calculatedPrtcRecipEntry = false;
			_calculatedOmnidirectionalEntry = false;
			_calculatedInterRadEntry = false;
			//CreateAreas();
		}

		public void ConstructProtectionSector1(bool justDeleteOld, bool createAreas = false)
		{
			_protectionSector1Checked = !justDeleteOld;
			if (!GuiController.Constructed)
				return;
			if (justDeleteOld)
			{
				SafeDeleteGraphic(_handlePrtctSector1);
				if (ProcType == ProcedureTypeConv.VorNdb)
				{
					if (_calculatedPrtctSctr1)
					{

						BasicArea = _vorNdb.ConstructBasicArea(SelectedNavPntsPrj[0], AltitudeAboveNavaid,
							DsgPntSelection.Direction, Speed.Tas, Side,
							LimDist.TimeInMin, _frmSettings.IsVisibleShablon, _frmSettings.IsVisibleNominalTrajectory,
							_frmSettings.IsVisibleToleranceArea);
						SafeDeleteGraphic(_handleBasicArea);
						if (BasicArea != null && !BasicArea.IsEmpty)
							_handleBasicArea = DrawPolygon(BasicArea, 0, eFillStyle.sfsHollow);

						SafeDeleteGraphic(_handleOmnidirectionalEntry);
						ProtectionOmnidirectionEntry.Clear();
						ProtectionOmnidirectionEntry.Add(_vorNdb.ConstructProtectionOmniDirEntry());
						if (!ProtectionOmnidirectionEntry.IsEmpty)
						{
							ProtectionOmnidirectionEntry =
								(MultiPolygon) GlobalParams.GeomOperators.UnionGeometry(BasicArea,
									ProtectionOmnidirectionEntry);
							ProtectionOmnidirectionEntry =
								(MultiPolygon) GlobalParams.GeomOperators.ConvexHull(ProtectionOmnidirectionEntry);
							ProtectionOmnidirectionEntry =
								(MultiPolygon) GlobalParams.GeomOperators.Difference(ProtectionOmnidirectionEntry,
									BasicArea) ??
								new MultiPolygon();
							_handleOmnidirectionalEntry =
								DrawMultiPolygon(ProtectionOmnidirectionEntry, 255, eFillStyle.sfsCross);
						}
						_calculatedPrtctSctr1 = false;
					}
				}
				CreateAreas();
				return;
			}

			if (!_calculatedPrtctSctr1)
			{
				ProtectionSector1.Clear();
				if (ProcType == ProcedureTypeConv.Vordme)
				{
					if (EntryDirection == EntryDirection.Toward)
					{
						ProtectionSector1.Add(_vorDme.TowardConstructProtectSector1(GlobalParams.NavaidDatabase.Vor,
							Speed.Ias, Speed.Tas,
							DsgPntSelection.Direction, AltitudeAboveNavaid, SelectedNavPntsPrj[0].Value,
							SelectedNavPntsPrj[1].Value,
							DsgPntSelection.SelectedDsgPntPrj, Side));
					}
					else
					{
						ProtectionSector1.Add(_vorDme.AwayConstructProtectSector1(GlobalParams.NavaidDatabase.Vor,
							DsgPntSelection.Direction, DsgPntSelection.SelectedDsgPntPrj, SelectedNavPntsPrj[1].Value,
							SelectedNavPntsPrj[0].Value,
							Side, AltitudeAboveNavaid, Speed.Ias, Speed.Tas));
					}
					if (!ProtectionSector1.IsEmpty)
					{
						ProtectionSector1 =
							(MultiPolygon) GlobalParams.GeomOperators.Difference(ProtectionSector1, BasicArea) ??
							new MultiPolygon();
					}
				}
				else if (ProcType == ProcedureTypeConv.VorNdb)
				{
					ProtectionSector1 =
						(MultiPolygon)
						GlobalParams.GeomOperators.Intersect(_vorNdb.ConstructSector1NotPermitted(),
							GlobalParams.GeomOperators.UnionGeometry(BasicArea, ProtectionOmnidirectionEntry));
					BasicArea = ((MultiPolygon) GlobalParams.GeomOperators.Difference(BasicArea, ProtectionSector1))[0];

					SafeDeleteGraphic(_handleBasicArea);
					SafeDeleteGraphic(_handleOmnidirectionalEntry);

					if (BasicArea != null && !BasicArea.IsEmpty)
						_handleBasicArea = DrawPolygon(BasicArea, 0, eFillStyle.sfsHollow);

					ProtectionOmnidirectionEntry =
						(MultiPolygon) GlobalParams.GeomOperators.Difference(ProtectionOmnidirectionEntry,
							ProtectionSector1);
					_handleOmnidirectionalEntry =
						DrawMultiPolygon(ProtectionOmnidirectionEntry, 255, eFillStyle.sfsCross);
				}
				_calculatedPrtctSctr1 = true;
			}
			SafeDeleteGraphic(_handlePrtctSector1);
			if (!ProtectionSector1.IsEmpty && ProcType == ProcedureTypeConv.Vordme)
				_handlePrtctSector1 = DrawMultiPolygon(ProtectionSector1, 255 * 255, eFillStyle.sfsCross);

			if (createAreas)
				CreateAreas();
		}

		internal void ConstructProtectionSector2(bool justDeleteOld, bool createAreas = false)
		{
			_protectionSector2Checked = !justDeleteOld;
			if (!GuiController.Constructed)
				return;
			if (justDeleteOld)
			{
				SafeDeleteGraphic(_handlePrtctSector2);
				CreateAreas();
				return;
			}

			if (!_calculatedPrtctSctr2)
			{
				ProtectionSector2.Clear();
				if (ProcType == ProcedureTypeConv.Vordme)
				{
					if (EntryDirection == EntryDirection.Toward)
					{
						ProtectionSector2.Add(_vorDme.TowardConstructProtectSector2(Speed.Tas, LimDist.ValueInPrj,
							DsgPntSelection.Direction, Side, SelectedNavPntsPrj[1].Value,
							DsgPntSelection.SelectedDsgPntPrj,
							LimDist.TimeInMin));
					}
					else
					{
						ProtectionSector2.Add(_vorDme.AwayConstructProtectSector2(Speed.Tas, LimDist.ValueInPrj,
							DsgPntSelection.Direction, Side, SelectedNavPntsPrj[1].Value,
							DsgPntSelection.SelectedDsgPntPrj,
							LimDist.TimeInMin, AltitudeAboveNavaid));
					}
					if (!ProtectionSector2.IsEmpty)
					{
						ProtectionSector2 =
							(MultiPolygon) GlobalParams.GeomOperators.Difference(ProtectionSector2, BasicArea) ??
							new MultiPolygon();
					}
				}
				_calculatedPrtctSctr2 = true;
			}

			SafeDeleteGraphic(_handlePrtctSector2);
			if (!ProtectionSector2.IsEmpty)
			{
				_handlePrtctSector2 = DrawMultiPolygon(ProtectionSector2, 255, eFillStyle.sfsCross);
			}
			if (createAreas)
				CreateAreas();
		}

		internal void ConstructReciprocalEntryArea(bool justDeleteOld, bool createAreas = false)
		{
			_protectionRecipDirChecked = !justDeleteOld;
			if (!GuiController.Constructed || ProcType == ProcedureTypeConv.VorNdb)
				return;

			if (justDeleteOld)
			{
				SafeDeleteGraphic(_handlePrtctRecipDir2SecPnt);
				_reAngleChanged(null, new CommonEventArg(double.NaN));
				CreateAreas();
				return;
			}

			if (!_calculatedPrtcRecipEntry)
			{
				ProtectionRecipEntry.Clear();
				if (ProcType == ProcedureTypeConv.Vordme)
				{
					AngleRe_Rp = 0;
					//Point vorPntPrj, dmePntPrj;
					//if ( __selectedNavPntsPrj[ 0 ].Type == NavType.Vor )
					//{
					//    vorPntPrj = __selectedNavPntsPrj[ 0 ].Value;
					//    dmePntPrj = __selectedNavPntsPrj[ 1 ].Value;
					//}
					//else
					//{
					//    dmePntPrj = __selectedNavPntsPrj[ 0 ].Value;
					//    vorPntPrj = __selectedNavPntsPrj[ 1 ].Value;
					//}
					if (EntryDirection == EntryDirection.Toward)
					{
						ProtectionRecipEntry.Add(_vorDme.TowardConstructRecipDirectEntry2SecondaryPnt(
							GlobalParams.NavaidDatabase.Vor, LimDist.LegLength, DsgPntSelection.Direction,
							DsgPntSelection.SelectedDsgPntPrj,
							SelectedNavPntsPrj[0].Value, SelectedNavPntsPrj[1].Value, Side, LimDist.Radius, EntryDirection,
							out AngleRe_Rp));
					}
					else
					{
                        _vorDme.AwayConstructRecipDirectEntryToSecondaryPnt(DsgPntSelection.Direction, LimDist.Radius,
                            DsgPntSelection.SelectedDsgPntPrj, SelectedNavPntsPrj[0].Value, LimDist.LegLength, Side, 
                            EntryDirection, out AngleRe_Rp);
						ProtectionRecipEntry.Clear();
					}

					_reAngleChanged(null, new CommonEventArg(AngleRe_Rp));
					if (EntryDirection == EntryDirection.Away)
						return;
				}
				else if (ProcType == ProcedureTypeConv.Vorvor)
				{
					ProtectionRecipEntry.Add(_vorVor.ReciprocalEntryArea());
				}
				if (!ProtectionRecipEntry.IsEmpty)
				{
					ProtectionRecipEntry =
						(MultiPolygon) GlobalParams.GeomOperators.Difference(ProtectionRecipEntry, BasicArea) ??
						new MultiPolygon();
				}
				_calculatedPrtcRecipEntry = true;
			}
			else
			{
				_reAngleChanged(null, new CommonEventArg(AngleRe_Rp));
			}

			SafeDeleteGraphic(_handlePrtctRecipDir2SecPnt);
			if (!ProtectionRecipEntry.IsEmpty)
			{
				_handlePrtctRecipDir2SecPnt = DrawMultiPolygon(ProtectionRecipEntry, 255, eFillStyle.sfsCross);
			}
			if (createAreas)
				CreateAreas();
		}

        internal double CalculateReRpAngle()
        {
            var a = ARANFunctions.ReturnAngleInRadians(SelectedNavPntsPrj[0].Value, SecondaryPoint);
            return ARANMath.DegToRad(ARANFunctions.DirToAzimuth(SecondaryPoint, a, GlobalParams.SpatialRefOperation.SpRefPrj, GlobalParams.SpatialRefOperation.SpRefGeo));
            //Point secPnt = ARANFunctions.LocalToPrj(designatedPntPrj, direction, emsal * legLength, (int)side * 2 * radius);
            //var res = (int)ARANMath.RadToDeg(ARANMath.SubtractAngles(DsgPntSelection.Direction, ARANFunctions.ReturnAngleInRadians(SelectedNavPntsPrj[0].Value, SecondaryPoint)));

            //var angle = _vorDme.CalculateReRpAngle(DsgPntSelection.Direction, LimDist.Radius,
            //                DsgPntSelection.SelectedDsgPntPrj, SelectedNavPntsPrj[0].Value, LimDist.LegLength, Side,
            //                EntryDirection);
            //return (int)ARANMath.Modulus(angle);
        }

		internal void ConstructOmnidirectionalEntry(bool justDeleteOld, bool createAreas = false)
		{
			_protectionOmniDirChecked = !justDeleteOld;
			if (!GuiController.Constructed)
				return;

			if (justDeleteOld)
			{
				SafeDeleteGraphic(_handleOmnidirectionalEntry);
				CreateAreas();
				return;
			}

			if (!_calculatedOmnidirectionalEntry)
			{
				ProtectionOmnidirectionEntry.Clear();
				ProtectionOmnidirectionEntry.Add(_vorNdb.ConstructProtectionOmniDirEntry());
				_calculatedOmnidirectionalEntry = true;
			}

			SafeDeleteGraphic(_handleOmnidirectionalEntry);
			if (!ProtectionOmnidirectionEntry.IsEmpty)
			{
				ProtectionOmnidirectionEntry =
					(MultiPolygon) GlobalParams.GeomOperators.UnionGeometry(BasicArea, ProtectionOmnidirectionEntry);
				ProtectionOmnidirectionEntry =
					(MultiPolygon) GlobalParams.GeomOperators.ConvexHull(ProtectionOmnidirectionEntry);
				ProtectionOmnidirectionEntry =
					(MultiPolygon) GlobalParams.GeomOperators.Difference(ProtectionOmnidirectionEntry, BasicArea) ??
					new MultiPolygon();
				_handleOmnidirectionalEntry = DrawMultiPolygon(ProtectionOmnidirectionEntry, 255, eFillStyle.sfsCross);
			}
			if (createAreas)
				CreateAreas();
		}

		internal void ConstructIntersectingRadialEntry(bool justDeleteOld, bool createAreas = false)
		{
			_protectionIntersectingRadEntryChecked = !justDeleteOld;
			if (!GuiController.Constructed)
				return;
			if (justDeleteOld)
			{
				SafeDeleteGraphic(_handlePrtctInterRadEntry);
				CreateAreas();
				return;
			}

			if (!_calculatedInterRadEntry)
			{
				ProtectionInterRadEntryArea = _vorVor.ProtectIntersectingRadialEntry(SelectedNavPntsPrj[0].Value,
					SelectedNavPntsPrj[1].Value, DsgPntSelection.Direction, Faclts.IntersectionDirection, Side,
					DsgPntSelection.SelectedDsgPntPrj, AltitudeAboveNavaid);
				_calculatedInterRadEntry = true;
				if (ProtectionInterRadEntryArea == null)
				{
					SafeDeleteGraphic(_handlePrtctInterRadEntry);
					return;
				}
				ProtectionInterRadEntryArea =
					(MultiPolygon) GlobalParams.GeomOperators.Difference(ProtectionInterRadEntryArea, BasicArea) ??
					new MultiPolygon();
			}

			SafeDeleteGraphic(_handlePrtctInterRadEntry);
			if (ProtectionInterRadEntryArea != null && !ProtectionInterRadEntryArea.IsEmpty)
			{
				_handlePrtctInterRadEntry = DrawMultiPolygon(ProtectionInterRadEntryArea, 255, eFillStyle.sfsCross);
			}
			if (createAreas)
				CreateAreas();
		}

		/// <summary>
		/// Returns whether save button should be enabled or not
		/// </summary>
		/// <returns></returns>
		internal bool CreateReport()
		{
			if (_frmReport != null && _frmReport.Visible)
				return (Report.ObstacleReport.All(rep => rep.Validation) || Report.ReportCount == 0);
			CalculateReport();
			var tmp = Report.ObstacleReport.Where(rep => !rep.Validation).ToList();
			bool isEnabled = (Report.ObstacleReport.All(rep => rep.Validation) || Report.ReportCount == 0);
			if (_frmReport == null)
				_frmReport = new FormReport(Report,
					GlobalParams.UnitConverter.HeightToDisplayUnits(AltitudeAboveNavaid, eRoundMode.NEAREST) + " " +
					GlobalParams.UnitConverter.HeightUnit, this);
			else
			{
				_frmReport.UptadeReport(Report,
					GlobalParams.UnitConverter.HeightToDisplayUnits(AltitudeAboveNavaid, eRoundMode.NEAREST) + " " +
					GlobalParams.UnitConverter.HeightUnit);
			}
			AssessedAltitude = 0;
			if (Report.ObstacleReport != null && Report.ObstacleReport.Count > 0)
				AssessedAltitude = Report.ObstacleReport.Max(rep => rep.Elevation + rep.Moc);
			if (AssessedAltitude == 0)
				AssessedAltitude = GlobalParams.UnitConverter.HeightToDisplayUnits(Moc, eRoundMode.NEAREST);
			_frmSettings.SetMinLowLimitHoldingPattern(AssessedAltitude);
			return isEnabled;
		}

		internal void Save()
		{
			screenCapture.Save(_frmMain);
			CalculateReport();
			Guid id;
		    //_frmReport.SaveReport();
			if (_dbResult.Save(Report, this, out id))
			{
				SaveScreenshotToDb(id);
			    _frmReport.SaveReport();
            }
			else
				screenCapture.Rollback();
		}

		private void SaveScreenshotToDb(Guid id)
		{
			Screenshot screenshot = new Screenshot
			{
				DateTime = DateTime.Now,
				Identifier = id,
				Images = screenCapture.Commit(id)
			};
			GlobalParams.Database.HoldingQpi.SetScreenshot(screenshot);
		}

		private void report_RenderingRow(object sender, ReportRowEventArgs e)
		{
			if (e.Row.DataItem == null)
				return;
			var penetrate = (double) e.Row["Penetrate"];
			if (penetrate > 0)
			{
				e.Row.Fields["Id"].DataStyle.ForeColor = System.Drawing.Color.Red;
				e.Row.Fields["Name"].DataStyle.ForeColor = System.Drawing.Color.Red;
				e.Row.Fields["Elevation"].DataStyle.ForeColor = System.Drawing.Color.Red;
				e.Row.Fields["Moc"].DataStyle.ForeColor = System.Drawing.Color.Red;
				e.Row.Fields["Req_H"].DataStyle.ForeColor = System.Drawing.Color.Red;
				e.Row.Fields["Penetrate"].DataStyle.ForeColor = System.Drawing.Color.Red;
				e.Row.Fields["Area"].DataStyle.ForeColor = System.Drawing.Color.Red;
				e.Row.Fields["GeomType"].DataStyle.ForeColor = System.Drawing.Color.Red;
				e.Row.Fields["HorAccuracy"].DataStyle.ForeColor = System.Drawing.Color.Red;
				e.Row.Fields["VerAccuracy"].DataStyle.ForeColor = System.Drawing.Color.Red;
			}
		}
		
		private void CalculateReport()
		{
			if (!_reportCalculated)
			{
				//CreateAreas ( );
				if (ProcType != ProcedureTypeConv.Vorvor || !_calculatedInterRadEntry)
					ProtectionInterRadEntryArea.Clear();
			    if (Report == null)
			        Report = new Report(AreaWithSectors, AreaWithBuffers, Altitude, Moc,
			            GlobalParams.UnitConverter.HeightToInternalUnits(MocList[0]), _bufferWidth, SelectedModul);
			    else
			        Report.Create(AreaWithSectors, AreaWithBuffers, Altitude, Moc,
			            GlobalParams.UnitConverter.HeightToInternalUnits(MocList[0]), _bufferWidth, SelectedModul);
				_reportCalculated = true;
			}
		}

		private void CleanDisplay()
		{
			ClearDrawedItems();
			//DeleteDesignatedPoint ( );
			//SafeDeleteGraphic ( _handleBasicArea );
			//SafeDeleteGraphic ( _handlePrtctSector1 );
			//SafeDeleteGraphic ( _handlePrtctSector2 );
			//SafeDeleteGraphic ( _handlePrtctRecipDir2SecPnt );
			//SafeDeleteGraphic ( _handleForNav1 );
			//SafeDeleteGraphic ( _handleForNav2 );
		}

		private void ClearDrawedItems()
		{
			while (_drawedItemIndexList.Count > 0)
				SafeDeleteGraphic(_drawedItemIndexList[0]);
		}

		private bool IsDisplayClear => (_drawedItemIndexList.Count == 0);

		internal void CreateAreas()
		{

			AreaWithSectors = BasicArea;
			if (_protectionOmniDirChecked && _calculatedOmnidirectionalEntry)
			{
				AreaWithSectors =
				((MultiPolygon) GlobalParams.GeomOperators.UnionGeometry(AreaWithSectors,
					ProtectionOmnidirectionEntry))[0];
			}

			if (_protectionSector1Checked && _calculatedPrtctSctr1 && ProcType == ProcedureTypeConv.Vordme)
				AreaWithSectors =
					((MultiPolygon) GlobalParams.GeomOperators.UnionGeometry(AreaWithSectors, ProtectionSector1))[0];
			else
				ProtectionSector1.Clear();

			if (_protectionSector2Checked && _calculatedPrtctSctr2 && ProcType == ProcedureTypeConv.Vordme)
				AreaWithSectors =
					((MultiPolygon) GlobalParams.GeomOperators.UnionGeometry(AreaWithSectors, ProtectionSector2))[0];
			else
				ProtectionSector2.Clear();

			if (_protectionRecipDirChecked && _calculatedPrtcRecipEntry)
				AreaWithSectors =
					((MultiPolygon) GlobalParams.GeomOperators.UnionGeometry(AreaWithSectors, ProtectionRecipEntry))[0];


			if (_protectionIntersectingRadEntryChecked && _calculatedInterRadEntry &&
				ProtectionInterRadEntryArea != null)
				AreaWithSectors =
				((MultiPolygon) GlobalParams.GeomOperators.UnionGeometry(AreaWithSectors,
					ProtectionInterRadEntryArea))[0];



			foreach (var item in _bufferHandles)
				SafeDeleteGraphic(item);

			AreaWithBuffers.Clear();
			int i = 1;
			Polygon tmpPolygon = AreaWithSectors;
			LineString lnStringTmp = new LineString();
			var bufferCount = SelectedModul == ModulType.Holding ? 5 : 1;

			while (i <= bufferCount)
			{
				AreaWithBuffers.Add(((MultiPolygon) GlobalParams.GeomOperators.Buffer(tmpPolygon, _bufferWidth))[0]);
				tmpPolygon = AreaWithBuffers[AreaWithBuffers.Count - 1];
				lnStringTmp.Clear();
				lnStringTmp.AddMultiPoint(AreaWithBuffers[AreaWithBuffers.Count - 1].ToMultiPoint());
				_bufferHandles.Add(DrawLineString(lnStringTmp, ARANFunctions.RGB(0, 0, 255), 1));
				i++;
			}
		}

		private AppliedGuiValues GuiController { get; set; }

		private void DrawDesigntdPoint(Point designatedPoint)
		{
			SafeDeleteGraphic(_handleDesignatedPoint);
			if (designatedPoint != null)
				//_handleDesignatedPoint = GlobalParams.DrawPointWithText ( designatedPoint, 255, "DsgPnt" );
				_handleDesignatedPoint = DrawPoint(designatedPoint, 255);
		}

		public void DrawEndOutboundLeg()
		{
			SafeDeleteGraphic(_handleEndOutboundLegPnt);
			if (SecondaryPoint != null)
				_handleEndOutboundLegPnt = DrawPoint(SecondaryPoint, 255);
		}


		#region Properties

		internal Ring ToleranceArea
		{
			get
			{
				switch (ProcType)
				{
					case ProcedureTypeConv.None:
						return null;

					case ProcedureTypeConv.Vordme:
						return _vorDme.ToleranceArea;

					case ProcedureTypeConv.VorNdb:
						return _vorNdb.ToleranceArea;

					case ProcedureTypeConv.Vorvor:
						return _vorVor.ToleranceArea;

					default:
						throw new NotImplementedException(Resources.Proc_Type_Not_Found);
				}
			}
		}

		internal LineString NominalTrack
		{
			get
			{
				switch (ProcType)
				{
					case ProcedureTypeConv.None:
						return null;

					case ProcedureTypeConv.Vordme:
						return _vorDme.NominalTrack;

					case ProcedureTypeConv.VorNdb:
						return _vorNdb.NominalTrack;

					case ProcedureTypeConv.Vorvor:
						return _vorVor.NominalTrack;
					default:
						throw new NotImplementedException(Resources.Proc_Type_Not_Found);
				}
			}
		}

		internal double PostFixTolerance
		{
			get
			{
				if (ProcType == ProcedureTypeConv.VorNdb)
					return _vorNdb.PostFixTolerance;
				if (ProcType == ProcedureTypeConv.Vordme)
					return _vorDme.PostFixTolerance;
				return _postFixTolerance;
			}
		}

		internal double PriorFixTolerance
		{
			get
			{
				if (ProcType == ProcedureTypeConv.VorNdb)
					return _vorNdb.PriorFixTolerance;
				if (ProcType == ProcedureTypeConv.Vordme)
					return _vorDme.PriorFixTolerance;
				return _priorFixtolerance;
			}
		}

		//internal InboundTrack InbountTrack
		//{
		//    get;
		//    private set;
		//}

		internal ProcedureTypeConv ProcType { get; private set; }

		internal double Moc { get; private set; }

		internal List<double> MocList
		{
			get
			{
				List<double> list = new List<double>
				{
					GlobalParams.UnitConverter.HeightToDisplayUnits(300, eRoundMode.NEAREST),
					GlobalParams.UnitConverter.HeightToDisplayUnits(450, eRoundMode.NEAREST),
					GlobalParams.UnitConverter.HeightToDisplayUnits(600, eRoundMode.NEAREST)
				};
				//if ( GlobalParams.UnitConverter.HeightUnit == "ft" )
				//{
				//    list.Add ( 984 );
				//    list.Add ( 1476 );
				//    list.Add ( 1969 );
				//}
				//else
				//{
				//    list.Add ( 300 );
				//    list.Add ( 450 );
				//    list.Add ( 600 );
				//}
				return list;
			}
		}

		internal Speed Speed { get; private set; }

		internal LimitingDistance LimDist { get; private set; }

		internal SideDirection Side { get; private set; }

		private Report Report { get; set; }
        public double Altitude { get; private set; }
        internal double AltitudeAboveNavaid { get; private set; }

		//internal SelectionAirdromeArea SlctdAirdromeArea
		//{
		//    get;
		//    private set;
		//}

		internal FixFacilities Faclts { get; private set; }

		internal DesignatedPntSelection DsgPntSelection { get; private set; }

		internal EntryDirection EntryDirection { get; private set; }

		private Polygon AreaWithSectors { get; set; }

		internal List<Polygon> AreaWithBuffers { get; }

		internal Polygon BasicArea { get; private set; }

		internal MultiPolygon ProtectionSector1 { get; private set; }

		internal MultiPolygon ProtectionSector2 { get; private set; }

		private MultiPolygon ProtectionOmnidirectionEntry { get; set; }

		internal MultiPolygon ProtectionRecipEntry { get; private set; }

		internal MultiPolygon ProtectionInterRadEntryArea { get; private set; }

		public string ModuleInstallDir
		{
			get { return _pandaInstallDir; }
			set
			{
				_pandaInstallDir = value;
				int constantsIndex = _pandaInstallDir.IndexOf(@"\constants", StringComparison.Ordinal);
				if (constantsIndex != -1)
				{
					_pandaInstallDir = _pandaInstallDir.Remove(constantsIndex);
				}
			}
		}

		public string Acar { get; private set; }

		public MultiPolygon LicenseRectGeo { get; set; }

		public MultiPolygon LicenseRectPrj { get; set; }

		internal List<NavaidPntPrj> SelectedNavPntsPrj { get; private set; }

		private List<DesignatedPoint> DesignatedPointList { get; set; }

		public bool HasDsgPoint
		{
			get
			{
				if (DsgPntSelection != null && DsgPntSelection.ChosenPntType == PointChoice.Select)
				{
					if (DesignatedPointList == null || DesignatedPointList.Count == 0)
						return false;
					return true;
				}
				else
					return true;
			}
		}

		public bool HasNavaid
		{
			get
			{
				if (_navaidList == null || _navaidList.Count == 0)
					return false;
				return true;
			}
		}

		public bool SaveSecondaryPoint { get; set; }
		public Point SecondaryPoint { get; set; }

		#endregion

		internal void MainFormClosing()
		{
			if (!IsDisplayClear)
			{
				//if ( MessageBox.Show ( "Clean graphics ?", "Graphics info", MessageBoxButtons.YesNo, MessageBoxIcon.Information ) == System.Windows.Forms.DialogResult.Yes )
				CleanDisplay();
			}
			_selectedMenuItem.Checked = false;
			_holdingMenuItem.Checked = false;
			EnableMenuItems();
		}

		internal void SettingBtnClicked()
		{
			if (!_frmSettings.Visible)
				_frmSettings.Show(new Win32Windows(_frmMain.Handle.ToInt32()));
		}

		internal void InfoBtnClicked()
		{
			_frmInfo.SetSpiralParameters();
			if (!_frmInfo.Visible)
				_frmInfo.Show(new Win32Windows(_frmMain.Handle.ToInt32()));
		}

		internal void AboutBtnClicked()
		{
			if (!_frmAbout.Visible)
				_frmAbout.Show(new Win32Windows(_frmMain.Handle.ToInt32()));
		}

		internal void SetNominalTrajectVisibility(bool visible)
		{

			switch (ProcType)
			{
				case ProcedureTypeConv.Vordme:
					_vorDme.SetNominalTrackVisibility(visible);
					break;
				case ProcedureTypeConv.VorNdb:
					_vorNdb.SetNominalTrackVisibility(visible);
					break;
				case ProcedureTypeConv.Vorvor:
					_vorVor.SetNominalTrackVisibility(visible);
					break;
				default:
					break;
			}
		}

		internal void SetTemplateVisibility(bool visible)
		{
			switch (ProcType)
			{
				case ProcedureTypeConv.Vordme:
					_vorDme.SetTemplateVisibility(visible);
					break;
				case ProcedureTypeConv.VorNdb:
					_vorNdb.SetTemplateVisibility(visible);
					break;
				case ProcedureTypeConv.Vorvor:
					_vorVor.SetTemplateVisiblity(visible);
					break;
				default:
					break;
			}
		}

		internal void SetToleranceAreaVisibility(bool visible)
		{
			switch (ProcType)
			{
				case ProcedureTypeConv.Vordme:
					_vorDme.SetToleranceAreaVisibility(visible);
					break;
				case ProcedureTypeConv.VorNdb:
					_vorNdb.SetToleranceAreaVisibility(visible);
					break;
				case ProcedureTypeConv.Vorvor:
					_vorVor.SetToleranceAreaVisibility(visible);
					break;
				default:
					break;
			}
		}

		internal void SetBufferVisibility(bool visible)
		{
			//if ( !_frmMain.btnConstruct.Enabled )
			//{
			//    CreateAreas ( );
			//}
		}

		private void DrawMin_MaxDistCircle(Point pntCenter, double notCoverageMinRadius, double notCoverageMaxRadius,
			double buffer4MinRadius = double.NaN, double buffer4MaxRadius = double.NaN)
		{            
			DeleteMin_MaxDistCircle();
			Ring minCircle = ARANFunctions.CreateCirclePrj(pntCenter, notCoverageMinRadius);
			_minCircleHandle = DrawRing(minCircle, 255, eFillStyle.sfsHollow);

			Ring maxCircle = ARANFunctions.CreateCirclePrj(pntCenter, notCoverageMaxRadius);
			_maxCircleHandle = DrawRing(maxCircle, 255, eFillStyle.sfsHollow);

			Polygon polygonMinCircle = new Polygon {ExteriorRing = minCircle};

			if (!double.IsNaN(buffer4MinRadius))
			{
				Polygon polygonBuffer4MinCircle =
					((MultiPolygon) GlobalParams.GeomOperators.Buffer(polygonMinCircle, buffer4MinRadius))[0];
				polygonBuffer4MinCircle =
					((MultiPolygon) GlobalParams.GeomOperators.Difference(polygonBuffer4MinCircle, polygonMinCircle))
					[0];
				_buffer4MinCircleHandle = DrawPolygon(polygonBuffer4MinCircle, 255, eFillStyle.sfsVertical);
			}

			if (!double.IsNaN(buffer4MaxRadius))
			{
				//Ring tmp1 = ARANFunctions.CreateCirclePrj ( pntCenter, notCoverageMaxRadius - buffer4MaxRadius );
				//DrawRing ( tmp1, 1, eFillStyle.sfsHollow );
				Polygon polygonBufferMaxCircle =
					ARANFunctions.CreateTorPrj(pntCenter, notCoverageMaxRadius - buffer4MaxRadius,
						notCoverageMaxRadius);
				_buffer4MaxCircleHandle = DrawPolygon(polygonBufferMaxCircle, 255, eFillStyle.sfsCross);
			}
		}

		private void DeleteMin_MaxDistCircle()
		{
			SafeDeleteGraphic(_minCircleHandle);
			SafeDeleteGraphic(_maxCircleHandle);
			SafeDeleteGraphic(_buffer4MinCircleHandle);
			SafeDeleteGraphic(_buffer4MaxCircleHandle);
		}

		#region Draw Methods

		public List<int> DrawToleranceAreaPoints(Ring toleranceArea)
		{
			List<int> result = new List<int>
			{
				DrawPointWithText(toleranceArea[0], 255 * 255, "0"),
				DrawPointWithText(toleranceArea[1], 255 * 255, "1"),
				DrawPointWithText(toleranceArea[2], 255 * 255, "2"),
				DrawPointWithText(toleranceArea[3], 255 * 255, "3")
			};
			return result;
		}

		public int DrawPoint(Point pnt, int color)
		{
			int handle = GlobalParams.AranEnvironment.Graphics.DrawPoint(pnt, color, true, false);
			_drawedItemIndexList.Add(handle);
			return handle;
		}

		public void DrawNavaid(Point pnt)
		{
			GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(_handleNavaid);
			_handleNavaid = GlobalParams.AranEnvironment.Graphics.DrawPoint(pnt, 255 * 255, true, false);
			_drawedItemIndexList.Add(_handleNavaid);
		}

		public int DrawMultiLineString(MultiLineString multiLineString, int color, int width)
		{
			int handle =
				GlobalParams.AranEnvironment.Graphics.DrawMultiLineString(multiLineString, width, color, true, false);
			_drawedItemIndexList.Add(handle);
			return handle;
		}

		private int DrawPolygon(Polygon polygon, int color, eFillStyle fillStyle)
		{
			int handle = GlobalParams.AranEnvironment.Graphics.DrawPolygon(polygon, fillStyle, color, true, false);
			_drawedItemIndexList.Add(handle);
			return handle;
		}

		private int DrawMultiPolygon(MultiPolygon mltPolygon, int color, eFillStyle fillStyle)
		{
			int handle =
				GlobalParams.AranEnvironment.Graphics.DrawMultiPolygon(mltPolygon, fillStyle, color, true, false);
			_drawedItemIndexList.Add(handle);
			return handle;
		}

		public int DrawLineString(LineString lineString, int color, int width)
		{
			int handle = GlobalParams.AranEnvironment.Graphics.DrawLineString(lineString, width, color, true, false);
			_drawedItemIndexList.Add(handle);
			return handle;
		}

		public int DrawRing(Ring ring, int color, eFillStyle fillStyle)
		{
			int handle = GlobalParams.AranEnvironment.Graphics.DrawRing(ring, fillStyle, color, true, false);
			_drawedItemIndexList.Add(handle);
			return handle;
		}

		public int DrawPointWithText(Point pnt, int color, string text)
		{
			int handle = GlobalParams.AranEnvironment.Graphics.DrawPointWithText(pnt, text, color, true, false);
			_drawedItemIndexList.Add(handle);
			return handle;
		}

		public void SafeDeleteGraphic(int handle)
		{
			GlobalParams.AranEnvironment.Graphics.SafeDeleteGraphic(handle);
			_drawedItemIndexList.Remove(handle);
		}

		private readonly List<int> _drawedItemIndexList = new List<int>();
		private int _handleNavaid;

		#endregion

		#region Fields

		internal string AirportName;
		internal ModulType SelectedModul;
		private double _bufferWidth;
		private string _pandaInstallDir;

		private FormMain _frmMain;
		private FormInfo _frmInfo;
		private FormAbout _frmAbout;
		private FormReport _frmReport;
		private FormSettings _frmSettings;

		private DsgPntEventHandler _dsgPntChanged;
		private CommonEventHandler _limDistChanged;
		private DirectionEventHandler _intersectionDirectionChanged;
		private CommonEventHandler _reAngleChanged;
		private AppliedValueChangedEventHandler _appliedValueChanged;

		private Vordme _vorDme;
		private Vorndb _vorNdb;
		private VorVor _vorVor;

		private double _priorFixtolerance, _postFixTolerance;

		private int _handleForNav1,
			_handleForNav2,
			_handleBasicArea,
			_handleDesignatedPoint,
			_handlePrtctSector1,
			_handlePrtctSector2,
			_handlePrtctRecipDir2SecPnt,
			_handleEndOutboundLegPnt;

		private bool _calculatedPrtctSctr1, _calculatedPrtctSctr2, _calculatedPrtcRecipEntry;
		private bool _calculatedInterRadEntry;
		private int _handlePrtctInterRadEntry;
		private DbResult _dbResult;
		private bool _reportCalculated;
		private bool _protectionRecipDirChecked;
		private bool _protectionSector1Checked;
		private bool _protectionSector2Checked;

		private ToolStripMenuItem _holdingMenuItem, _racetrackMenuItem;
		private List<Navaid> _navaidList;

		private DesignatedPoint _selectedDsgPnt;
		public Navaid SelectedNavaid;
		private Point _arpPnt;
		private readonly double _tang55Deg;
		private int _minCircleHandle;
		private int _maxCircleHandle;
		private ToolStripMenuItem _selectedMenuItem;
		private int _handleOmnidirectionalEntry;
		private bool _calculatedOmnidirectionalEntry;
		private bool _definedDsgPnt;
		private readonly List<int> _bufferHandles;
		private int _buffer4MinCircleHandle;
		private int _buffer4MaxCircleHandle;
		private readonly double _minDistInBasicArea;
		private readonly double _maxDistInBasicArea;
		private bool _protectionOmniDirChecked;
        public int AngleRe_Rp;
		private int _homingVorRadialHandle;
		private int _intersectingVorRadialHandle;
		private int _homingVorRpsHandle;
		private int _intersectingVorRpsHandle;
		private int _intersectedPntHandle;
		private DsgPntDefinition _dsgPntDefiniton4VorVor;
		private int _homingVorCircleHandle;
		private int _intersectingVorCircleHandle;
		public readonly double Altitude4Holding;
		public decimal DirectionInDeg;
		public string IntersectingVorDsg;
		private double _intersectingVorRadial;
		private bool _protectionIntersectingRadEntryChecked;
		private bool _isTurbulence;
		public double LowerLimitHldngPattern;
		public double AssessedAltitude;
	    public bool OutboundDefinitionViaTime { get; set; }

        #endregion
	}
}