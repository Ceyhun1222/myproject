using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Data.Filters;
using Aran.Aim.Features;
using Aran.Converters;
using Ag = Aran.Geometries;
using Aran.AranEnvironment.Symbols;
using Aran.PANDA.Common;
using Aran.PANDA.Vss;

namespace Aran.Panda.Vss
{
    public partial class FirstPageControl : PageControl
    {
        private InstrumentApproachProcedure _currentIAP;
        private AirportHeliport _currAirportHeliport;
        private RunwayDirection _currRunwayDir;
        private Runway _currRunway;
        private FinalLeg _currFinalLeg;
        private Navaid _guidanceNavad;
        private NavaidEquipment _guidanceEquipment;
        private MapLocation _endPointLocation;
        private MapLocation _runwayThrLocation;
        private RunwayCentrelinePoint _thrRwyClinePoint;
        private PrjLine _rwyCourseLine;
        private MapLocation _fafLocation;
        private Ag.Point _ficThrPoint;
        private Ag.Point _trackCourseIntersectPoint;
        private Ag.Point _point1400FromThr;
        private MapLine _trackLine;
        private double[] _codeNumberArr;
        private string[] _codeLetterArr;
        private Dictionary<DrawElementType, int> _drawedElementDict;
        private SecondPageControl _secondPageControl;
        private const double Distance1400  = 1400;

        
        public FirstPageControl()
        {
            InitializeComponent();

            _codeNumberArr = new double[] { 800d, 1200d, 1800d };
            _codeLetterArr = new string[] { "A", "B", "C", "DEF" };

            _drawedElementDict = new Dictionary<DrawElementType, int>();
            ui_finalLegGuidanceFacilityLabel.Text = string.Empty;

            Globals.MainForm.FormClosing += MainForm_FormClosing;
			ARANFunctions.InitEllipsoid ( );
        }


        public event EventHandler AreaChanged;


        public override void LoadPage()
        {
            base.LoadPage();

            #region Set Units Labels

            var min = Globals.UnitConverter.HeightToDisplayUnits(30);
            var max = Globals.UnitConverter.HeightToDisplayUnits(300);
            var val = Globals.UnitConverter.HeightToDisplayUnits(100);
            ui_ochNud.Minimum = (decimal)min;
            ui_ochNud.Maximum = (decimal)max;
            ui_ochNud.Value = (decimal)val;
            ui_ochUnitLabel.Text = Globals.UnitConverter.HeightUnit;
    
            #endregion

            IsFirst = true;

            _currAirportHeliport = Globals.Qpi.GetFeature(
                FeatureType.AirportHeliport, Globals.PandaSettings.Aeroport) as AirportHeliport;

            if (_currAirportHeliport == null) return;
            var compOps = new ComparisonOps(ComparisonOpType.EqualTo, "airportHeliport", _currAirportHeliport.Identifier);
            
            var gr = Globals.DbPro.GetVersionsOf(
                FeatureType.InstrumentApproachProcedure,
                TimeSliceInterpretationType.BASELINE, Guid.Empty, true, null, null,
                compOps.ToFilter());

            if (!gr.IsSucceed)
                Globals.ShowError("Error on loading [InstrumentApproachProcedure]!\nMessage: " + gr.Message);

            var procedureList= gr.List.Cast<InstrumentApproachProcedure>().OrderBy(pro=>pro.Name);
            ui_iapCB.Items.Clear();
            foreach (InstrumentApproachProcedure iap in procedureList) {
                if (!string.IsNullOrEmpty(iap.Name))
                    ui_iapCB.Items.Add(new ComboBoxItem(iap.Name, iap));
            }

            SetAirportUI();

            if (ui_iapCB.Items.Count > 0)
                ui_iapCB.SelectedIndex = 0;
        }

        public override void NextClicked()
        {
            base.NextClicked();

            DoPageChanged(_secondPageControl, true);

            _secondPageControl.LoadPage();
        }

        public override void SetAllPageControls(IEnumerable<PageControl> allPageControls)
        {
            base.SetAllPageControls(allPageControls);

            _secondPageControl = allPageControls.Where(pg => pg is SecondPageControl).First() as SecondPageControl;
        }


        public MapLine TrackLine { get { return _trackLine; } }

        public PrjLine RunwayCourseLine { get { return _rwyCourseLine; } }

        public double OffsetAngle { get; private set; }

        public double AbeamDistanceFrom1400M { get; private set; }

        //*** Distance between Thr and Point = (Intersect with TrackLine and RwyCourse)
        public double IntersectThrDistance { get; private set; }

        public double StripWidth { get; private set; }

        public Ag.Point Point60FromThr { get; private set; }

        public Ag.Point Point60FromThrRight { get; private set; }

        public Ag.Point Point60FromThrLeft { get; private set; }

        public Ag.Polygon VssArea { get; private set; }

        public double TrackCourse { get; private set; }

        public double RwyCourse { get; private set; }

        public double OCH { get; private set; }

        public double? VssLength { get; private set; }

        public double? VssSlopeAngle { get; private set; }


        public List<VssObstacle> CalculateObstacles()
        {
            var rwyDirPlus90 = _rwyCourseLine.GetAngleInRad() - ARANMath.C_PI_2;
            var verticalAngle = Math.Abs(ARANMath.DegToRad(_currFinalLeg.VerticalAngle.Value));
            var thrHeight = 0d;

            if (_thrRwyClinePoint.Location != null)
                thrHeight = ConverterToSI.Convert(_thrRwyClinePoint.Location.Elevation, 0);

			if ( Ag.Geometry.IsNullOrEmpty ( VssArea ) )
			{
				Globals.ShowError ( "Invalid VSS Area (null)", true );
				return null;
			}

			var areaPrjGeom = Globals.GeomOpers.GeoTransformations(VssArea, Globals.ViewSpRef, Globals.Wgs84SpRef);

            if (Ag.Geometry.IsNullOrEmpty(areaPrjGeom)) {
                Globals.ShowError("Invalid VSS Area", true);
                return null;
            }

            var vsList = Globals.Qpi.GetVerticalStructureList(areaPrjGeom as Ag.MultiPolygon);

            var vssObsList = new List<VssObstacle>();

            foreach (var vs in vsList) {
				//if (vs.Part.Count == 0 )
				//    vs.Part[0].HorizontalProjection.Choice != VerticalStructurePartGeometryChoice.ElevatedPoint) 
				//{
				//	continue;
				//}
				var vssObs = new VssObstacle ( );
				vssObs.Identifier = vs.Identifier;
				vssObs.Name = vs.Name;
				switch ( vs.Part[ 0 ].HorizontalProjection.Choice )
				{
					case VerticalStructurePartGeometryChoice.ElevatedPoint:
						var ep = vs.Part[ 0 ].HorizontalProjection.Location;
						vssObs.LocationPrj = Globals.GeomOpers.GeoTransformations (
							ep.Geo, Globals.Wgs84SpRef, Globals.ViewSpRef ) as Ag.Point;
						vssObs.Elevation = ConverterToSI.Convert ( ep.Elevation, 0 );
						break;
					case VerticalStructurePartGeometryChoice.ElevatedCurve:
						var elevatedCurve = vs.Part[ 0 ].HorizontalProjection.LinearExtent;
						vssObs.LocationPrj = Globals.GeomOpers.GeoTransformations (
							elevatedCurve.Geo, Globals.Wgs84SpRef, Globals.ViewSpRef ) as Ag.MultiLineString;
						if ( !Globals.GeomOpers.Disjoint ( VssArea, vssObs.LocationPrj ) )
							vssObs.LocationPrj = Globals.GeomOpers.Intersect ( VssArea, vssObs.LocationPrj );
						vssObs.Elevation = ConverterToSI.Convert ( elevatedCurve.Elevation, 0 );
						break;
					case VerticalStructurePartGeometryChoice.ElevatedSurface:
						var elevatedSurface = vs.Part[ 0 ].HorizontalProjection.SurfaceExtent;
						vssObs.LocationPrj = Globals.GeomOpers.GeoTransformations (
							elevatedSurface.Geo, Globals.Wgs84SpRef, Globals.ViewSpRef ) as Ag.MultiPolygon;
						if ( !Globals.GeomOpers.Disjoint ( VssArea, vssObs.LocationPrj ) )
							vssObs.LocationPrj = Globals.GeomOpers.Intersect ( VssArea, vssObs.LocationPrj );
						vssObs.Elevation = ConverterToSI.Convert ( elevatedSurface.Elevation, 0 );
						break;
					default:
						break;
				}
				vssObs.Height = vssObs.Elevation - thrHeight;
				vssObsList.Add(vssObs);
            }
			Aran.Geometries.LineString lnSeg = new Ag.LineString ( );
			lnSeg.Add ( new Ag.Point ( ARANFunctions.PointAlongPlane ( Point60FromThr, rwyDirPlus90, 1000 ) ) );
			lnSeg.Add ( new Ag.Point ( ARANFunctions.PointAlongPlane ( Point60FromThr, rwyDirPlus90-ARANMath.C_PI, 1000 ) ) );
			//Globals.AranEnv.Graphics.DrawLineString ( lnSeg, 255, 2 );
			//Ag.Geometry geomPrjTmp;
			foreach (var vssObs in vssObsList) {
				//geomPrjTmp = ;
				//if ( vssObs.LocationPrj is Ag.MultiLineString || vssObs.LocationPrj is Ag.MultiPolygon )
				//{
				//	if ( !Globals.GeomOpers.Disjoint ( VssArea, geomPrjTmp ) )
				//		geomPrjTmp = Globals.GeomOpers.Intersect ( VssArea, geomPrjTmp );
				//}
				vssObs.VssX = Globals.GeomOpers.GetDistance ( vssObs.LocationPrj, lnSeg );
				//if ( vssObs.LocationPrj is Aran.Geometries.Point )
				//{
				//	vssObs.VssX = ARANFunctions.PointToLineDistance ( vssObs.LocationPrj as Aran.Geometries.Point, Point60FromThr, rwyDirPlus90 );
				//}				
                vssObs.HSurface = vssObs.VssX * Math.Tan(verticalAngle - Globals.Const_SlopeOffset);
                vssObs.HPenetrate = vssObs.Height - vssObs.HSurface;

                if (vssObs.HPenetrate >= 0) {
                    if (vssObs.VssX == 0)
                        vssObs.NeededSlope = 90d;
                    else
                        vssObs.NeededSlope = ARANMath.RadToDeg(Math.Atan(vssObs.Height / vssObs.VssX) + Globals.Const_SlopeOffset);
                }
            }

            return vssObsList;
        }


        
        private void IAP_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbItem = ui_iapCB.SelectedItem as ComboBoxItem;
            var iap = cbItem?.Tag as InstrumentApproachProcedure;
            SetIAP(iap);
        }

        private void FinalLeg_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbItem = (sender as ComboBox)?.SelectedItem as ComboBoxItem;
            var finalLeg = cbItem?.Tag as FinalLeg;

            SetFinalLeg(finalLeg);
            SetTrackLine();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Globals.ClearDrawedGraphics(this);
        }


        #region Set Member

        private void SetIAP(InstrumentApproachProcedure iap)
        {
            _currentIAP = iap;
            RunwayDirection rwyDir = null;
            FinalLeg finalLeg = null;

            List<ComboBoxItem> finalLegCbiList = null;

            if (iap != null) {
                if (iap.Landing != null && iap.Landing.Runway.Count > 0)
                {
                    var featRef = iap.Landing.Runway[0].Feature;
                    featRef.FeatureType = FeatureType.RunwayDirection;
                    rwyDir = featRef.GetFeature() as RunwayDirection;
                }
                //if landing property is null or not set for procedure look segment legs
                else
                {
                    foreach (var procedureTransition in iap.FlightTransition)
                    {
                        var landingCollection = procedureTransition.DepartureRunwayTransition.Runway;
                        if (landingCollection?.Count > 0)
                        {
                            var featRef = landingCollection[0].Feature;
                            featRef.FeatureType = FeatureType.RunwayDirection;
                            rwyDir = featRef.GetFeature() as RunwayDirection;
                            break;
                        }
                    }
                }

                finalLegCbiList = GetFinalLegs();
            }

            
	        if (rwyDir == null)
	        {
				Globals.ShowError ($"Landing property of procedure ({iap.Name}) is not defined!", true );
				return;
			}
			ui_finalLegCB.Items.Clear();
            finalLegCbiList?.ForEach(cbi => ui_finalLegCB.Items.Add(cbi));

            SetCurrentIAPUI();

            SetRunwayDir(rwyDir);
            SetFAFLocation(FindFAPPoint());

            if (ui_finalLegCB.Items.Count > 0)
                ui_finalLegCB.SelectedIndex = 0;
            else
                SetFinalLeg(finalLeg);
        }

        private void SetRunwayDir(RunwayDirection rwyDir)
        {
            _currRunwayDir = rwyDir;
            MapLocation runwayThrLocation = null;
            Runway runway = null;
            _thrRwyClinePoint = null;

            if (_currRunwayDir != null) {
				_currRunwayDir.UsedRunway.FeatureType = FeatureType.Runway;
				runway = _currRunwayDir.UsedRunway.GetFeature() as Runway;

                #region Fill THR or DTHR RunwayCentrelinePoint

                var compOps = new ComparisonOps(ComparisonOpType.EqualTo, "onRunway", _currRunwayDir.Identifier);
                var list = Globals.Qpi.GetFeatures<RunwayCentrelinePoint>(Guid.Empty, compOps.ToFilter());

                foreach (var rcp in list) {
                    if (rcp.Role == CodeRunwayPointRole.THR || rcp.Role == CodeRunwayPointRole.DISTHR) {
                        runwayThrLocation = new MapLocation(rcp.Location.Geo);
                        _thrRwyClinePoint = rcp;
                        break;
                    }
                }

                #endregion
            }

            SetRunwayDirUI();

            SetRunway(runway);
            SetRunwayThrLocation(runwayThrLocation);

            var thrElevVal = double.NaN;
            if (_thrRwyClinePoint != null && _thrRwyClinePoint.Location != null){
                thrElevVal = ConverterToSI.Convert(_thrRwyClinePoint.Location.Elevation, double.NaN);
            }

            ui_thrElevTB.Text = (double.IsNaN(thrElevVal) ? string.Empty :
                string.Format("{0}  {1}",
                    Globals.UnitConverter.HeightToDisplayUnits(thrElevVal),
                    Globals.UnitConverter.HeightUnit));
        }

        private void SetRunway(Runway runway)
        {
            _currRunway = runway;
            StripWidth = double.NaN;

            if (runway != null) {

                if (runway.NominalLength == null) {
                    Globals.ShowError("Runway NominalLength not defined!", true);
                    _currRunway = null;
                }
                else {
                    if (runway.WidthStrip != null)
                        StripWidth = Aran.Converters.ConverterToSI.Convert(runway.WidthStrip, 0);
                    else {
                        var length = ConverterToSI.Convert(runway.NominalLength, double.NaN);
                        if (!double.IsNaN(length)) {
                            var codeNumber = GetCodeNumber(length);
                            switch (codeNumber) {
                                case 1:
                                case 2:
                                    StripWidth = 75;
                                    break;
                                default:
                                    StripWidth = 150;
                                    break;
                            }
                        }
                    }
                }
            }

            SetRunwayUI();
        }

        private void SetRunwayThrLocation(MapLocation value)
        {
            _runwayThrLocation = value;
            _rwyCourseLine = null;
            RwyCourse = double.NaN;

            if (_runwayThrLocation != null) {

                #region SetRwyCourseLine

                if (_currRunwayDir.TrueBearing != null) {

                    RwyCourse = ARANMath.DegToRad(_currRunwayDir.TrueBearing.Value);

                    var radCourse = ARANFunctions.AztToDirection(_runwayThrLocation.Geo, _currRunwayDir.TrueBearing.Value, Globals.Wgs84SpRef, Globals.ViewSpRef);
                    radCourse += ARANMath.C_PI;

                    var secondPoint = ARANFunctions.PointAlongPlane(_runwayThrLocation.Prj, radCourse, Globals.Const_CourseDrawDistance);

                    _rwyCourseLine = new PrjLine() {
                        Start = _runwayThrLocation.Prj,
                        End = secondPoint
                    };
                }

                #endregion
            }

            if (double.IsNaN(RwyCourse)) {
                Globals.ShowError("RunwayDirection Course value is not defined!", true);
                return;
            }

            DrawRwyDirCourse();
        }

        private void SetFAFLocation(MapLocation value)
        {
            _fafLocation = value;

            //if (_fafLocation != null) {

            //    if (_endPointLocation != null && !_trackLineCalculated) {
            //        SetTrackLine();
            //    }
            //}

            DrawFafPoint();
        }

        private void SetFinalLeg(FinalLeg value)
        {
            _currFinalLeg = value;
            Navaid guidanceNavaid = null;
            TrackCourse = double.NaN;

            if (_currFinalLeg != null) {

                if (_currFinalLeg.Course != null)
                    TrackCourse = ARANMath.DegToRad(_currFinalLeg.Course.Value);

                guidanceNavaid = GetAngleUseNavaid(_currFinalLeg.StartPoint);

                if (guidanceNavaid == null)
                    guidanceNavaid = GetAngleUseNavaid(_currFinalLeg.EndPoint);

                if (_currFinalLeg.VerticalAngle == null || _currFinalLeg.VerticalAngle.Value >= 0) {
                    Globals.ShowError("FinalLeg VerticalAngle is greater than 0°", true);
                    return;
                }
            }

            if (double.IsNaN(TrackCourse)) {
                Globals.ShowError("FinalLeg Course value not defined!", true);
                return;
            }

            SetGuidanceNavaid(guidanceNavaid);

            SetFinalLegUI();
        }

        private void SetGuidanceNavaid(Navaid nav)
        {
            _guidanceNavad = nav;
            _guidanceEquipment = null;
            MapLocation navLoc = null;

            var guidanceTypes = new NavaidEquipmentType[] { 
                NavaidEquipmentType.Localizer, NavaidEquipmentType.VOR, NavaidEquipmentType.NDB, NavaidEquipmentType.TACAN };

            if (nav != null) {
                
                ElevatedPoint location = null;

                if (nav.NavaidEquipment.Count == 0) {
                    location = nav.Location;
                }
                else {
                    foreach (var item in nav.NavaidEquipment) {
                        if (guidanceTypes.Contains(item.TheNavaidEquipment.Type)) {
							_guidanceEquipment = item.TheNavaidEquipment.GetFeature() as NavaidEquipment;
                            location = _guidanceEquipment.Location;
                            break;
                        }
                    }
                }

                if (location == null) {
                    Globals.ShowError("Navaid point is not defined!", true);
                    return;
                }
                else {
                    navLoc = new MapLocation(location.Geo);
                }
            }
            else
            {
				_currFinalLeg.EndPoint.PointChoice.FixDesignatedPoint.FeatureType = FeatureType.DesignatedPoint;
				var dp = _currFinalLeg.EndPoint.PointChoice.FixDesignatedPoint.GetFeature ( ) as DesignatedPoint;
				if ( dp == null )
				{
					Globals.ShowError (
						$"DesignatedPoint which defined as End Point ('{_currFinalLeg.EndPoint.PointChoice.FixDesignatedPoint.Identifier}') was deleted ", true );
					return;
				}

				if ( dp.Location == null )
				{
					Globals.ShowError ( "[DesignatedPoint] [Location] has no value", true );
					return;
				}

				navLoc = new MapLocation ( dp.Location.Geo );
			}

            SetGuidanceNavLocation(navLoc);
        }

        private void SetGuidanceNavLocation(MapLocation value)
        {
            _endPointLocation = value;

            //if (_endPointLocation != null) {
            //    if (_fafLocation != null && !_trackLineCalculated) {
            //        SetTrackLine();
            //    }
            //}

            DrawGuidanceFacilityCourse();
        }

        private void SetTrackLine()
        {
            var start = _fafLocation;
            var end = _endPointLocation;

            _ficThrPoint = null;
            _trackCourseIntersectPoint = null;
            IntersectThrDistance = double.NaN;
            _point1400FromThr = null;
            AbeamDistanceFrom1400M = double.NaN;
            Point60FromThr = null;
            Point60FromThrLeft = null;
            Point60FromThrRight = null;
            VssArea = null;
            VssLength = null;
            VssSlopeAngle = null;

            //if (TrackLine == null) return;

            //TrackCourse =ARANMath.DegToRad(ARANFunctions.DirToAzimuth(TrackLine.Start.Prj,
            //    ARANFunctions.ReturnAngleInRadians(TrackLine.Start.Prj, TrackLine.End.Prj),Globals.ViewSpRef,Globals.Wgs84SpRef));

            if (start == null || end == null)
                _trackLine = null;
            else
                _trackLine = new MapLine(start, end);

            if (_trackLine != null) {

                if (_rwyCourseLine != null) {

                    #region Set FicTHR

                    var rwyInverseDir = _rwyCourseLine.GetAngleInRad();
                    var rwyDirPlus90 = rwyInverseDir + ARANMath.C_PI_2;
                    var trackDir = ARANFunctions.AztToDirection(
                        TrackLine.Start.Geo, ARANMath.RadToDeg(TrackCourse), Globals.Wgs84SpRef, Globals.ViewSpRef);

                    //var rGeom = ARANFunctions.LineLineIntersect(
                    //    _trackLine.Start.Prj, trackDir, _rwyCourseLine.Start, rwyDirPlus90);

                    //if (!Ag.Geometry.IsNullOrEmpty(rGeom)) {
                    //    if (rGeom.Type == Ag.GeometryType.Point) {
                    //        _ficThrPoint = rGeom as Ag.Point;
                    //    }
                    //}

                    //var graphics = Globals.AranEnv.Graphics;

                    
                    #endregion

                    #region Set OffsetAngle

                    //var offsetAngle = ARANMath.Modulus(TrackCourse - RwyCourse, ARANMath.C_2xPI);
                    //if (offsetAngle > ARANMath.C_PI)
                    //    offsetAngle = -1 * ARANMath.C_2xPI - offsetAngle;

                    //OffsetAngle = offsetAngle;
                    OffsetAngle = ARANMath.SubtractAnglesWithSign(TrackCourse, RwyCourse, SideDirection.sideRight);
                    
                    #endregion

                    #region Set TrackCourseIntersectPoint

                    var rGeom = ARANFunctions.LineLineIntersect(
                        _trackLine.Start.Prj, trackDir, _rwyCourseLine.Start, rwyInverseDir);

                    if (!Ag.Geometry.IsNullOrEmpty(rGeom)) {
                        if (rGeom.Type == Ag.GeometryType.Point) {
                            var rPoint = rGeom as Ag.Point;
                            var side = ARANMath.SideDef(_rwyCourseLine.Start, rwyDirPlus90, rPoint);

                            _trackCourseIntersectPoint = rPoint;

                            IntersectThrDistance = ARANFunctions.ReturnDistanceInMeters(rPoint, _rwyCourseLine.Start);

                            //*** Rwy istiqameti 180 ferqle olduguna gore Right-la yoxlamaq lazimdir
                            if (side == SideDirection.sideLeft) {
                                IntersectThrDistance = -IntersectThrDistance;
                            }
                        }
                    }

                    #endregion

                    #region Set Point 1400M from THR

                    _point1400FromThr = ARANFunctions.PointAlongPlane(_rwyCourseLine.Start, rwyInverseDir, 1400);

                    #endregion

                    #region Set AbeamDistance from 1400M

                    rGeom = ARANFunctions.LineLineIntersect(
                        _point1400FromThr, rwyDirPlus90, _trackLine.Start.Prj, trackDir);

                    bool sameDirection = false;
                    SideDirection sideAbeamDistancePt = SideDirection.sideOn;
                    if (!Ag.Geometry.IsNullOrEmpty(rGeom)) {
                        var rPoint = rGeom as Ag.Point;
                        AbeamDistanceFrom1400M = ARANFunctions.ReturnDistanceInMeters(_point1400FromThr, rPoint);


                        sideAbeamDistancePt = ARANMath.SideDef(_rwyCourseLine.Start, rwyInverseDir, rPoint);
                        var sideFinalPt = ARANMath.SideDef(_rwyCourseLine.Start, rwyInverseDir, TrackLine.Start.Prj);

                        sameDirection = sideAbeamDistancePt == sideFinalPt;

                        //if (ARANMath.SideDef(_rwyCourseLine.Start, rwyInverseDir, rPoint) == SideDirection.sideRight)
                        //    AbeamDistanceFrom1400M = -1 * AbeamDistanceFrom1400M;

                    }

                    #endregion

                    double distanceFromThr = 60;

                    #region Set Point 60M From THR

                    Point60FromThr = ARANFunctions.PointAlongPlane(_rwyCourseLine.Start, rwyInverseDir, distanceFromThr);
                    Point60FromThrLeft = ARANFunctions.PointAlongPlane(Point60FromThr, rwyDirPlus90, StripWidth / 2);
                    Point60FromThrRight = ARANFunctions.PointAlongPlane(Point60FromThr, rwyDirPlus90 + ARANMath.C_PI, StripWidth / 2);

                    #endregion

                    #region VSS Boundary

                    var rightSplay = double.NaN;
                    var leftSplay = double.NaN;

                    var pt60FromThrRight = Point60FromThrRight;
                    var pt60FromThrLeft = Point60FromThrLeft;


                    var distance1400FromTrackStart = Distance1400 - distanceFromThr;


                    if (_guidanceEquipment != null && _guidanceEquipment.NavaidEquipmentType == NavaidEquipmentType.Localizer) {
                        rightSplay = 0;
                        leftSplay = 0;
                    }
                    else {
                        
                        if (Math.Abs(ARANMath.RadToDeg(OffsetAngle)) <= 5d) {  //&& Math.Abs(AbeamDistanceFrom1400M) < 150d) {

                            var splayAngle = Math.Abs(OffsetAngle);

                            if (Math.Abs(AbeamDistanceFrom1400M) < 150d || IntersectThrDistance<1400)
                                splayAngle = Math.Atan(Globals.Const_StandardSplayAngleTan +
                                                       Math.Abs(AbeamDistanceFrom1400M) / distance1400FromTrackStart);

                            if (sameDirection) {
                                var maxAngle = Math.Max(splayAngle,Globals.Const_StandardSplayAngleTan+Math.Abs(OffsetAngle));
                                if (sideAbeamDistancePt == SideDirection.sideRight)
                                {
                                    rightSplay = maxAngle;
                                    leftSplay = Globals.Const_StandardSplayAngle;
                                }
                                else
                                {
                                    leftSplay= maxAngle;
                                    rightSplay = Globals.Const_StandardSplayAngle;
                                }
                            }
                            //startpoint and intercetion in 1400 point doesn't same  side
                            else {
                                if (sideAbeamDistancePt == SideDirection.sideLeft)
                                {
                                    rightSplay = Globals.Const_StandardSplayAngle + Math.Abs(OffsetAngle);
                                    leftSplay = splayAngle;
                                }
                                else
                                {
                                    leftSplay = Globals.Const_StandardSplayAngle + Math.Abs(OffsetAngle);
                                    rightSplay = splayAngle;
                                }
                            }
                        }
                        else {
                            if (OffsetAngle >= 0) {
                                rightSplay = Globals.Const_StandardSplayAngle + OffsetAngle;
                                leftSplay = Globals.Const_StandardSplayAngle;
                            }
                            else {
                                rightSplay = Globals.Const_StandardSplayAngle;
                                leftSplay = Globals.Const_StandardSplayAngle + OffsetAngle;
                            }
                        }
                    }

                    if (_currFinalLeg.VerticalAngle != null)
                    {
                        var verticalAngle = Math.Abs(ARANMath.DegToRad(_currFinalLeg.VerticalAngle.Value));
                        var vssSlopeAngle = verticalAngle - Globals.Const_SlopeOffset;

                        if (vssSlopeAngle < 0) {
                            Globals.ShowError("Selected FinalLeg VerticalAngle value is less than 1.12°");
                            return;
                        }

                        var vssLength = OCH / Math.Tan(vssSlopeAngle);
                        var vssEndPoint = ARANFunctions.PointAlongPlane(Point60FromThr, rwyInverseDir, vssLength);

                        var vssRightDir = rwyInverseDir - rightSplay;
                        var vssLeftDir = rwyInverseDir + leftSplay;

                        var vssRightLen = vssLength / Math.Cos(rightSplay);
                        var vssLeftLen = vssLength / Math.Cos(leftSplay);

                        var vvRightEndPoint = ARANFunctions.PointAlongPlane(pt60FromThrRight, vssRightDir, vssRightLen);
                        var vvLeftEndPoint = ARANFunctions.PointAlongPlane(pt60FromThrLeft, vssLeftDir, vssLeftLen);

                        VssArea = new Ag.Polygon();
                        VssArea.ExteriorRing.Add(pt60FromThrRight);
                        VssArea.ExteriorRing.Add(pt60FromThrLeft);
                        VssArea.ExteriorRing.Add(vvLeftEndPoint); 
                        VssArea.ExteriorRing.Add(vvRightEndPoint);
                        VssArea.ExteriorRing.Add(pt60FromThrRight);

                        VssLength = vssLength;
                        VssSlopeAngle = vssSlopeAngle;
                    }

                    #endregion
                }


            }

			#region Set UI

            ui_vssLengthTB.Text = (VssLength == null ? string.Empty : Globals.DistanceFormat(VssLength.Value));
            ui_vssSlopeAngleTB.Text = (VssSlopeAngle == null ? string.Empty : Globals.GetDoubleText(ARANMath.RadToDeg(VssSlopeAngle.Value)));

            ui_OffsetAngleTB.Text = Globals.GetDoubleText(ARANMath.RadToDeg(OffsetAngle));
            ui_distanceToIntersTB.Text = Globals.DistanceFormat(IntersectThrDistance);
            ui_abeamDistFrom1400mTB.Text = Globals.DistanceFormat(AbeamDistanceFrom1400M);

            #endregion

            DrawTrackLine();
            DrawFicThr();
            DrawTrackCourseIntersect();
            DrawPoint1400FromThr();
            DrawPoint60FromThr();
            DrawPoint60FromThrLeft();
            DrawPoint60FromThrRight();
            DrawVssArea();



            if (AreaChanged != null)
                AreaChanged(this, null);
        }

        #endregion

        #region Set UI

        private void SetAirportUI()
        {
            var ah = _currAirportHeliport;

            ui_airportDesignatorTB.Text = ah.Designator;
            ui_airportNameTB.Text = ah.Name;
        }

        private void SetRunwayUI()
        {
            var runway = _currRunway ?? new Runway();

            ui_runwayDesignatorTB.Text = runway.Designator;
            ui_runwayLengthTB.Text = Globals.GetValDistanceText(runway.NominalLength);
            ui_runwayStripWidthTB.Text = Globals.DistanceFormat(StripWidth);

            ui_runwayCodeNumberTB.Text = string.Empty;
            ui_runwayCodeLetterCB.Items.Clear();

            var length = ConverterToSI.Convert(runway.NominalLength, double.NaN);
            if (!double.IsNaN(length)) {
                var codeNumber = GetCodeNumber(length);
                ui_runwayCodeNumberTB.Text = codeNumber.ToString();
                var codeLetterStr = _codeLetterArr[codeNumber - 1];
                foreach (var c in codeLetterStr)
                    ui_runwayCodeLetterCB.Items.Add(c);
                ui_runwayCodeLetterCB.SelectedIndex = 0;
            }
        }

        private void SetRunwayDirUI()
        {
            var rd = _currRunwayDir ?? new RunwayDirection();

            ui_rwyDirDesignatorTB.Text = rd.Designator;
            ui_rwyDirTrueBearingTB.Text = Globals.GetDoubleText(rd.TrueBearing);


        }

        private void SetCurrentIAPUI()
        {
            var iap = _currentIAP ?? new InstrumentApproachProcedure();
            
            ui_approachTypeTB.Text = Globals.GetTextOrEmpty(iap.ApproachType);
        }

        private void SetFinalLegUI()
        {
            var fl = _currFinalLeg ?? new FinalLeg();

            ui_finalLegGuidanceSystemTB.Text = Globals.GetTextOrEmpty(fl.GuidanceSystem);
            ui_finalLegLandingSysCategoryTB.Text = Globals.GetTextOrEmpty(fl.LandingSystemCategory);

            var course = Globals.GetDoubleText(fl.Course);
            var courseType = Globals.GetTextOrEmpty(fl.CourseType);
            var courseDir = Globals.GetTextOrEmpty(fl.CourseDirection);

            #region Course

            var s = string.Empty;

            if (course != string.Empty) {
                s = course;
                if (courseType != string.Empty)
                    s += "   " + courseType;

                if (courseDir != string.Empty)
                    s += "   " + courseDir;
            }

            ui_finalLegCourseTB.Text = s;

            #endregion

            ui_finalLegVertAngleTB.Text = Globals.GetDoubleText(fl.VerticalAngle);

            if (_guidanceEquipment != null) {
                ui_finalLegGuidanceFacilityTB.Text = _guidanceEquipment.Designator;
                ui_finalLegGuidanceFacilityLabel.Text = _guidanceEquipment.FeatureType.ToString();
            }
            else {
                ui_finalLegGuidanceFacilityTB.Text = string.Empty;
                ui_finalLegGuidanceFacilityLabel.Text = string.Empty;
            }
        }

        #endregion

        #region Link Clicked

        private void AirportHeliport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Globals.ShowFeatureInfo(_currAirportHeliport);
        }

        private void Runway_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Globals.ShowFeatureInfo(_currRunway);
        }

        private void RunwayDirection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Globals.ShowFeatureInfo(_currRunwayDir);
        }

        private void IAP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Globals.ShowFeatureInfo(_currentIAP);
        }

        private void FinalLeg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Globals.ShowFeatureInfo(_currFinalLeg);
        }

        private void GuidanceFacility_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Globals.ShowFeatureInfo(_guidanceEquipment);
        }

        private void RunwayCentrelinePoint_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Globals.ShowFeatureInfo(_thrRwyClinePoint);
        }

        #endregion

        #region Helper Functions

        private int GetCodeNumber(double length)
        {
            for (int i = _codeNumberArr.Length - 1; i >= 0; i--) {
                if (length >= _codeNumberArr[i])
                    return (i + 2);
            }
            return 1;
        }

        private static Navaid GetAngleUseNavaid(TerminalSegmentPoint tsp)
        {
            if (tsp == null)
                return null;
            
            if (tsp.FacilityMakeup.Count == 0)
                return null;
            
            if (tsp.FacilityMakeup[0].FacilityAngle.Count == 0)
                return null;

            if (tsp.FacilityMakeup[0].FacilityAngle[0].AlongCourseGuidance == false)
                return null;

			tsp.FacilityMakeup[ 0 ].FacilityAngle[ 0 ].TheAngleIndication.FeatureType = FeatureType.AngleIndication;
			var angleIndication = tsp.FacilityMakeup[0].FacilityAngle[0].TheAngleIndication.GetFeature() as AngleIndication;

            if (angleIndication?.PointChoice?.Choice != SignificantPointChoice.Navaid) return null;
            angleIndication.PointChoice.NavaidSystem.FeatureType = FeatureType.Navaid;

            return angleIndication.PointChoice.NavaidSystem.GetFeature() as Navaid;
        }

        private MapLocation FindFAPPoint()
        {
            if (_currentIAP == null)
                return null;

            var procTrans = _currentIAP.FlightTransition?.Where(
                trans => trans.Type != null && trans.Type.Value == CodeProcedurePhase.FINAL).FirstOrDefault();

            if (procTrans == null) {
                Globals.ShowError("[FINAL] Transition does not exists!", true);
                return null;
            }

            TerminalSegmentPoint fafTerminalSegmentPoint = null;

            for(int i = procTrans.TransitionLeg.Count - 1; i >= 0; i--)
            {
				var segmentLeg = procTrans.TransitionLeg[i].TheSegmentLeg.GetFeature() as SegmentLeg;

                if (IsTerminalSegmentPointFAF(segmentLeg?.EndPoint)) {
                    fafTerminalSegmentPoint = segmentLeg?.EndPoint;
                    break;
                }

                if (IsTerminalSegmentPointFAF(segmentLeg?.StartPoint)) {
                    fafTerminalSegmentPoint = segmentLeg?.StartPoint;
                    break;
                }
            }

            if (fafTerminalSegmentPoint == null) {
                Globals.ShowError("[FAF] Point does not defined!", true);
                return null;
            }

            if (fafTerminalSegmentPoint.PointChoice == null) {
                Globals.ShowError("[FAF] [PointChoice] has no value !", true);
                return null;
            }

            if (fafTerminalSegmentPoint.PointChoice.Choice != SignificantPointChoice.DesignatedPoint) {
                Globals.ShowError("[FAF] Point has not references to [DesignatedPoint]!", true);
                return null;
            }

			fafTerminalSegmentPoint.PointChoice.FixDesignatedPoint.FeatureType = FeatureType.DesignatedPoint;
	        //var t = fafTerminalSegmentPoint.PointChoice.FixDesignatedPoint.GetFeature();
			var dp = fafTerminalSegmentPoint.PointChoice.FixDesignatedPoint.GetFeature() as DesignatedPoint;
			if(dp == null)
			{
				Globals.ShowError(
					$"DesignatedPoint which defined as FAF Point ('{fafTerminalSegmentPoint.PointChoice.FixDesignatedPoint.Identifier}') was deleted ", true);
				return null;
			}

			if (dp.Location == null) {
                Globals.ShowError("[DesignatedPoint] [Location] has no value", true);
                return null;
            }

            return new MapLocation(dp.Location.Geo);
        }

        private List<ComboBoxItem> GetFinalLegs()
        {
            var list = new List<ComboBoxItem>();

            var finalIdentifierSeqNumDict = new Dictionary<Guid, int>();

            foreach (var procTrans in _currentIAP.FlightTransition) {
                foreach (var procTransLeg in procTrans.TransitionLeg) {
                    if (procTransLeg.TheSegmentLeg != null &&
                        procTransLeg.TheSegmentLeg.Type == SegmentLegType.FinalLeg)
                    {

                        var finalLeg = procTransLeg.TheSegmentLeg.GetFeature() as FinalLeg;

                        if (finalLeg?.VerticalAngle == null || finalLeg.VerticalAngle.Value >= 0)
                            continue;

                        if (finalLeg?.StartPoint?.Role == CodeProcedureFixRole.FAF || finalLeg?.StartPoint?.Role == CodeProcedureFixRole.SDF)
                        {
                            finalIdentifierSeqNumDict.Add(
                                procTransLeg.TheSegmentLeg.Identifier,
                                procTransLeg.SeqNumberARINC == null ? -1 : (int)procTransLeg.SeqNumberARINC.Value);
                        }
                    }
                }
            }

            if (finalIdentifierSeqNumDict.Count > 0) {

                var compOps = new ComparisonOps(ComparisonOpType.In, "Identifier", finalIdentifierSeqNumDict.Keys.ToList());
                var gr = Globals.DbPro.GetVersionsOf(FeatureType.FinalLeg, TimeSliceInterpretationType.BASELINE,
                    Guid.Empty, true, null, null, compOps.ToFilter());

                if (!gr.IsSucceed) {
                    Globals.ShowError(string.Format("Error on loading [FinalLeg]\nMessage: {0}", gr.Message), true);
                    return null;
                }

                var finalLegList = gr.GetListAs<FinalLeg>();

                

                foreach (var fl in finalLegList) {
                    var seqNum = finalIdentifierSeqNumDict[fl.Identifier];
                    list.Add(
                        new ComboBoxItem(
                            (seqNum == -1 ? "#" : seqNum.ToString()),
                            fl));
                }
            }

            return list;
        }

        // ReSharper disable once InconsistentNaming
        private static bool IsTerminalSegmentPointFAF(TerminalSegmentPoint tsp)
        {
            return tsp?.Role == CodeProcedureFixRole.FAF;
        }

        #endregion

        #region Draw Functions

        [DrawElementMethod(DrawElementType.GuidanceFacilityCourse)]
        private void DrawGuidanceFacilityCourse(DrawOperType operType = DrawOperType.Redraw)
        {
            return;

            var graphics = Globals.AranEnv.Graphics;
            var elemType = DrawElementType.GuidanceFacilityCourse;
            int elementId;

            if (operType == DrawOperType.Clear || operType == DrawOperType.Redraw) {

                if (_drawedElementDict.TryGetValue(elemType, out elementId)) {
                    graphics.DeleteGraphic(elementId);
                    _drawedElementDict.Remove(elemType);
                }

                if (operType == DrawOperType.Clear)
                    return;
            }

            if (_currFinalLeg.Course == null || _endPointLocation == null)
                return;

            var degCourse = ARANFunctions.AztToDirection(_endPointLocation.Geo, _currFinalLeg.Course.Value, Globals.Wgs84SpRef, Globals.ViewSpRef);
            var radCourse = ARANMath.DegToRad(degCourse);

            var secondPoint = ARANFunctions.PointAlongPlane(_endPointLocation.Prj, radCourse, Globals.Const_CourseDrawDistance);

            var lineString = new Ag.LineString();
            lineString.Add(_endPointLocation.Prj);
            lineString.Add(secondPoint);

            elementId = graphics.DrawLineString(lineString, Globals.LineSymbol(elemType));

            if (_drawedElementDict.ContainsKey(elemType))
                _drawedElementDict[elemType] = elementId;
            else
                _drawedElementDict.Add(elemType, elementId);

        }

        [DrawElementMethod(DrawElementType.RwyDirCourse)]
        private void DrawRwyDirCourse(DrawOperType operType = DrawOperType.Redraw)
        {
            var graphics = Globals.AranEnv.Graphics;
            var elemType = DrawElementType.RwyDirCourse;
            int elementId;

            if (operType == DrawOperType.Clear || operType == DrawOperType.Redraw) {

                if (_drawedElementDict.TryGetValue(elemType, out elementId)) {
                    graphics.DeleteGraphic(elementId);
                    _drawedElementDict.Remove(elemType);
                }

                if (operType == DrawOperType.Clear)
                    return;
            }

            if (_rwyCourseLine == null)
                return;

            var lineString = _rwyCourseLine.ToLineString();
            elementId = graphics.DrawLineString(lineString, Globals.LineSymbol(elemType));

            if (_drawedElementDict.ContainsKey(elemType))
                _drawedElementDict[elemType] = elementId;
            else
                _drawedElementDict.Add(elemType, elementId);
        }

        [DrawElementMethod(DrawElementType.FafPoint)]
        private void DrawFafPoint(DrawOperType operType = DrawOperType.Redraw)
        {
            var graphics = Globals.AranEnv.Graphics;
            var elemType = DrawElementType.FafPoint;
            int elementId;

            if (operType == DrawOperType.Clear || operType == DrawOperType.Redraw) {

                if (_drawedElementDict.TryGetValue(elemType, out elementId)) {
                    graphics.DeleteGraphic(elementId);
                    _drawedElementDict.Remove(elemType);
                }

                if (operType == DrawOperType.Clear)
                    return;
            }

            if (_fafLocation == null)
                return;

            elementId = graphics.DrawPointWithText(_fafLocation.Prj, "FAF", Globals.PointSymbol(elemType));

            if (_drawedElementDict.ContainsKey(elemType))
                _drawedElementDict[elemType] = elementId;
            else
                _drawedElementDict.Add(elemType, elementId);
        }

        [DrawElementMethod(DrawElementType.TrackLine)]
        private void DrawTrackLine(DrawOperType operType = DrawOperType.Redraw)
        {
            var graphics = Globals.AranEnv.Graphics;
            var elemType = DrawElementType.TrackLine;
            int elementId;

            if (operType == DrawOperType.Clear || operType == DrawOperType.Redraw) {

                if (_drawedElementDict.TryGetValue(elemType, out elementId)) {
                    graphics.DeleteGraphic(elementId);
                    _drawedElementDict.Remove(elemType);
                }

                if (operType == DrawOperType.Clear)
                    return;
            }

            if (_trackLine == null)
                return;

            elementId = graphics.DrawLineString(_trackLine.ToPrjLineString(), Globals.LineSymbol(elemType));

            if (_drawedElementDict.ContainsKey(elemType))
                _drawedElementDict[elemType] = elementId;
            else
                _drawedElementDict.Add(elemType, elementId);
        }

        [DrawElementMethod(DrawElementType.FicTHR)]
        private void DrawFicThr(DrawOperType operType = DrawOperType.Redraw)
        {
            var graphics = Globals.AranEnv.Graphics;
            var elemType = DrawElementType.FicTHR;
            int elementId;

            if (operType == DrawOperType.Clear || operType == DrawOperType.Redraw) {

                if (_drawedElementDict.TryGetValue(elemType, out elementId)) {
                    graphics.DeleteGraphic(elementId);
                    _drawedElementDict.Remove(elemType);
                }

                if (operType == DrawOperType.Clear)
                    return;
            }

            if (_ficThrPoint == null)
                return;

            elementId = graphics.DrawPointWithText(_ficThrPoint, "FicTHR", Globals.PointSymbol(elemType));

            if (_drawedElementDict.ContainsKey(elemType))
                _drawedElementDict[elemType] = elementId;
            else
                _drawedElementDict.Add(elemType, elementId);
        }

        [DrawElementMethod(DrawElementType.TrackCourseIntersect)]
        private void DrawTrackCourseIntersect(DrawOperType operType = DrawOperType.Redraw)
        {
            var graphics = Globals.AranEnv.Graphics;
            var elemType = DrawElementType.TrackCourseIntersect;
            int elementId;

            if (operType == DrawOperType.Clear || operType == DrawOperType.Redraw) {

                if (_drawedElementDict.TryGetValue(elemType, out elementId)) {
                    graphics.DeleteGraphic(elementId);
                    _drawedElementDict.Remove(elemType);
                }

                if (operType == DrawOperType.Clear)
                    return;
            }

            if (_trackCourseIntersectPoint == null)
                return;

            elementId = graphics.DrawPointWithText(_trackCourseIntersectPoint, "Intersect", Globals.PointSymbol(elemType));

            if (_drawedElementDict.ContainsKey(elemType))
                _drawedElementDict[elemType] = elementId;
            else
                _drawedElementDict.Add(elemType, elementId);
        }

        [DrawElementMethod(DrawElementType.Point1400FromThr)]
        private void DrawPoint1400FromThr(DrawOperType operType = DrawOperType.Redraw)
        {
            var graphics = Globals.AranEnv.Graphics;
            var elemType = DrawElementType.Point1400FromThr;
            int elementId;

            if (operType == DrawOperType.Clear || operType == DrawOperType.Redraw) {

                if (_drawedElementDict.TryGetValue(elemType, out elementId)) {
                    graphics.DeleteGraphic(elementId);
                    _drawedElementDict.Remove(elemType);
                }

                if (operType == DrawOperType.Clear)
                    return;
            }

            if (_point1400FromThr == null)
                return;

            elementId = graphics.DrawPointWithText(_point1400FromThr, "[1400 M]", Globals.PointSymbol(elemType));

            if (_drawedElementDict.ContainsKey(elemType))
                _drawedElementDict[elemType] = elementId;
            else
                _drawedElementDict.Add(elemType, elementId);
        }

        [DrawElementMethod(DrawElementType.Point60FromThr)]
        private void DrawPoint60FromThr(DrawOperType operType = DrawOperType.Redraw)
        {
            var graphics = Globals.AranEnv.Graphics;
            var elemType = DrawElementType.Point60FromThr;
            int elementId;

            if (operType == DrawOperType.Clear || operType == DrawOperType.Redraw) {

                if (_drawedElementDict.TryGetValue(elemType, out elementId)) {
                    graphics.DeleteGraphic(elementId);
                    _drawedElementDict.Remove(elemType);
                }

                if (operType == DrawOperType.Clear)
                    return;
            }

            if (Point60FromThr == null)
                return;

            elementId = graphics.DrawPointWithText(Point60FromThr, "[60 M]", Globals.PointSymbol(elemType));

            if (_drawedElementDict.ContainsKey(elemType))
                _drawedElementDict[elemType] = elementId;
            else
                _drawedElementDict.Add(elemType, elementId);
        }

        [DrawElementMethod(DrawElementType.Point60FromThrRight)]
        private void DrawPoint60FromThrRight(DrawOperType operType = DrawOperType.Redraw)
        {
            var graphics = Globals.AranEnv.Graphics;
            var elemType = DrawElementType.Point60FromThrRight;
            int elementId;

            if (operType == DrawOperType.Clear || operType == DrawOperType.Redraw) {

                if (_drawedElementDict.TryGetValue(elemType, out elementId)) {
                    graphics.DeleteGraphic(elementId);
                    _drawedElementDict.Remove(elemType);
                }

                if (operType == DrawOperType.Clear)
                    return;
            }

            if (Point60FromThrRight == null)
                return;

            elementId = graphics.DrawPointWithText(Point60FromThrRight, "[60 M Right]", Globals.PointSymbol(elemType));

            if (_drawedElementDict.ContainsKey(elemType))
                _drawedElementDict[elemType] = elementId;
            else
                _drawedElementDict.Add(elemType, elementId);
        }

        [DrawElementMethod(DrawElementType.Point60FromThrLeft)]
        private void DrawPoint60FromThrLeft(DrawOperType operType = DrawOperType.Redraw)
        {
            var graphics = Globals.AranEnv.Graphics;
            var elemType = DrawElementType.Point60FromThrLeft;
            int elementId;

            if (operType == DrawOperType.Clear || operType == DrawOperType.Redraw) {

                if (_drawedElementDict.TryGetValue(elemType, out elementId)) {
                    graphics.DeleteGraphic(elementId);
                    _drawedElementDict.Remove(elemType);
                }

                if (operType == DrawOperType.Clear)
                    return;
            }

            if (Point60FromThrLeft == null)
                return;

            elementId = graphics.DrawPointWithText(Point60FromThrLeft, "[60 M Left]", Globals.PointSymbol(elemType));

            if (_drawedElementDict.ContainsKey(elemType))
                _drawedElementDict[elemType] = elementId;
            else
                _drawedElementDict.Add(elemType, elementId);
        }

        [DrawElementMethod(DrawElementType.VssArea)]
        private void DrawVssArea(DrawOperType operType = DrawOperType.Redraw)
        {
            var graphics = Globals.AranEnv.Graphics;
            var elemType = DrawElementType.VssArea;
            int elementId;

            if (operType == DrawOperType.Clear || operType == DrawOperType.Redraw) {

                if (_drawedElementDict.TryGetValue(elemType, out elementId)) {
                    graphics.DeleteGraphic(elementId);
                    _drawedElementDict.Remove(elemType);
                }

                if (operType == DrawOperType.Clear)
                    return;
            }

            if (Ag.Geometry.IsNullOrEmpty(VssArea))
                return;

            elementId = graphics.DrawPolygon(VssArea, Globals.PolygonSymbol(elemType));

            if (_drawedElementDict.ContainsKey(elemType))
                _drawedElementDict[elemType] = elementId;
            else
                _drawedElementDict.Add(elemType, elementId);
        }

        #endregion

        private void OCH_ValueChanged(object sender, EventArgs e)
        {
            OCH = Globals.UnitConverter.HeightToInternalUnits(Convert.ToDouble(ui_ochNud.Value));
            SetTrackLine();
        }

        
    }
}
