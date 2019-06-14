using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.AranEnvironment;
using Aran.Geometries;
using Aran.Panda.Rnav.Holding.Properties;
using Aran.PANDA.Common;
using Aran.Queries;
using DoddleReport;
using DoddleReport.Writers;
using Holding.Models;
using Aran.PANDA.Rnav.Holding.Properties;
using ProcedureType = Holding.Models.ProcedureType;

namespace Holding.save
{
    public class HoldingSaveMain
    {
        private Bussines_Logic _bLogic;
        private double _assessedAltitude;
        public HoldingSaveMain(Bussines_Logic logic)
        {
            _bLogic = logic;

        }

        public HoldingPattern Save()
        {
            GlobalParams.Database.HoldingQpi.ClearAllFeatures();
            if (_bLogic.HReport.ObstacleReport != null && _bLogic.HReport.ObstacleReport.Count > 0)
            {
                _assessedAltitude = _bLogic.HReport.ObstacleReport.Max(rep => rep.Req_H);
                // + _bLogic.ModelAreamParam.CurMoc;
            }

            _assessedAltitude = _assessedAltitude == 0 ? _bLogic.ModelAreamParam.CurMoc : _assessedAltitude;

            #region :>DesignatedPoint

            CreateSegmentPoint();
            HPattern = CreateHoldingPattern();
            HAssessMent = CreateHoldingAssesment();
            HPattern.HoldingPoint = SegmentPt;
            HAssessMent.HoldingPoint = SegmentPt;
            HAssessMent.AssessedHoldingPattern = HPattern.GetFeatureRef();
            HAssessMent.LowerLimit = HPattern.LowerLimit;
            HAssessMent.LowerLimitReference = HPattern.LowerLimitReference;
            HAssessMent.SpeedLimit = HPattern.SpeedLimit;
            if (_bLogic.ModelAreamParam.Condition == ConditionType.turbo)
                HAssessMent.TurbulentAir = true;
            HAssessMent.UpperLimit = HPattern.UpperLimit;
            HAssessMent.UpperLimitReference = HPattern.UpperLimitReference;

            #endregion

            GlobalParams.Database.HoldingQpi.SetRootFeatureType(Aran.Aim.FeatureType.HoldingAssessment);
            if (GlobalParams.Database.HoldingQpi.Commit(new Aran.Aim.FeatureType[]
                {Aran.Aim.FeatureType.HoldingPattern, Aran.Aim.FeatureType.HoldingAssessment}))
                MessageBox.Show(Resources.Procedure_has_saved_to_database_successfully);

            return HPattern;
        }

        private HoldingPattern CreateHoldingPattern()
        {
            ModelPBN modelPBN = _bLogic.ModelPBN;
            ModelAreaParams modelAreaParam = _bLogic.ModelAreamParam;

            HoldingPattern _hPattern =GlobalParams.Database.HoldingQpi.CreateFeature<HoldingPattern>();
            if (modelPBN.CurFlightPhase.FlightPhaseName == InitHolding.FlightPhaseValue[flightPhase.Enroute])
                _hPattern.Type = CodeHoldingUsage.ENR;
            else
                _hPattern.Type = CodeHoldingUsage.TER;

            _hPattern.InboundCourse  = modelAreaParam.Radial;
            _hPattern.OutboundCourse = ARANMath.Modulus(modelAreaParam.Radial - 180, 360);
            if (modelAreaParam.Turn == TurnDirection.CW)
            {
                _hPattern.TurnDirection = CodeDirectionTurn.RIGHT;
                _hPattern.NonStandardHolding = false;
            }
            else
            {
                _hPattern.TurnDirection = CodeDirectionTurn.LEFT;
                _hPattern.NonStandardHolding = true;
            }
         
            _hPattern.LowerLimit = new ValDistanceVertical();
            if (InitHolding.HeightUnit == VerticalDistanceType.M)
                _hPattern.LowerLimit.Uom = UomDistanceVertical.M;
            else if (InitHolding.HeightUnit == VerticalDistanceType.Ft)
                _hPattern.LowerLimit.Uom = UomDistanceVertical.FT;

            _hPattern.LowerLimit.Value = _assessedAltitude ;
            _hPattern.LowerLimitReference = CodeVerticalReference.MSL;

            _hPattern.UpperLimit = new ValDistanceVertical();
            _hPattern.UpperLimit.Uom = _hPattern.LowerLimit.Uom;
            _hPattern.UpperLimit.Value = modelAreaParam.Altitude; //UpperLimit.ToString();
            _hPattern.UpperLimitReference = CodeVerticalReference.MSL;

            _hPattern.SpeedLimit = new ValSpeed();
            if (InitHolding.SpeedUnit == HorizantalSpeedType.KMInHour)
                _hPattern.SpeedLimit.Uom = UomSpeed.KM_H;
            else if (InitHolding.SpeedUnit == HorizantalSpeedType.Knot)
                _hPattern.SpeedLimit.Uom = UomSpeed.KT;
            
            _hPattern.SpeedLimit.Value = modelAreaParam.Ias;

            _hPattern.OutboundCourseType = CodeCourse.TRUE_BRG;

            HoldingPatternLength hpLength = null;
            if (_bLogic.ProcedureType.CurDistanceType == DistanceType.Time)
            {
                HoldingPatternDuration hpDuration = new HoldingPatternDuration();
                hpDuration.Duration = new ValDuration();
                hpDuration.Duration.Uom = UomDuration.MIN;
                hpDuration.Duration.Value = _bLogic.ProcedureType.Time;
                hpLength = new HoldingPatternLength();
                hpLength.EndTime = hpDuration;
            }
            else
                if (_bLogic.ProcedureType.CurDistanceType == DistanceType.Wd)
                {
                    ValDistance distanceType = new ValDistance();
                    if (InitHolding.DistanceUnit == HorizantalDistanceType.KM)
                        distanceType.Uom = UomDistance.KM;
                    else
                        if (InitHolding.DistanceUnit == HorizantalDistanceType.NM)
                            distanceType.Uom = UomDistance.NM;
                        else
                            if (InitHolding.DistanceUnit == HorizantalDistanceType.NM)
                                distanceType.Uom = UomDistance.NM;

                    double altitude = Common.DeConvertHeight(modelAreaParam.Altitude);
                    double ias = Common.DeConvertSpeed(modelAreaParam.Ias);
                    double RV = Shablons.TurnRadius(ias, altitude, 15);
                    double wd = Common.DeConvertDistance(_bLogic.ProcedureType.WD);
                    double outBoundLegSpan =  Math.Sqrt(wd * wd - 4 * RV * RV);

                    distanceType.Value = Common.ConvertDistance(outBoundLegSpan,roundType.toNearest);
                    hpLength = new HoldingPatternLength();
                    hpLength.EndDistance = new HoldingPatternDistance();
                    hpLength.EndDistance.Length = distanceType;
                }

            _hPattern.OutboundLegSpan = hpLength;
            _hPattern.Extent = new Curve();
            try
            {
                LineString lnString = GlobalParams.SpatialRefOperation.ToGeo(_bLogic.HoldingGeom.HoldingTrack);
                if (lnString != null && lnString.Count > 0)
                    _hPattern.Extent.Geo.Add(lnString);
            }
            catch (Exception)
            {
                throw new Exception("Nominal trajectory is null");
            }

            _hPattern.Instruction = "RNAV,";
            if (_bLogic.ProcedureType.PropType == ProcedureType.withHoldingFunc)
                _hPattern.Instruction += " With holding functionality, ";
            else if (_bLogic.ProcedureType.PropType == ProcedureType.withoutHoldingFunc)
                _hPattern.Instruction += " Without holding functionality, ";
            else
                _hPattern.Instruction += " RNP holding, ";

            _hPattern.Instruction += _bLogic.ModelPBN.CurReciever.RecieverName + ", " + _bLogic.ModelPBN.CurPBN.PBNName+", " ;
            if (_bLogic.ProcedureType.ProtectionType == protectionSectorType.direct)
                _hPattern.Instruction += " Direct";
            else
                _hPattern.Instruction += " Omnidirectional"; 
            
            return _hPattern;
        }

        private HoldingAssessment CreateHoldingAssesment()
        {
            HoldingAssessment _hAssessMent = GlobalParams.Database.HoldingQpi.CreateFeature<HoldingAssessment>();
            DistanceType distanceType = _bLogic.ProcedureType.CurDistanceType;

            _hAssessMent.TurbulentAir = _bLogic.ModelAreamParam.TurboCondition;
            _hAssessMent.ObstacleAssessment.AddRange(CreateObstacleAssesmentArea());
            return _hAssessMent;
        }

        private List<ObstacleAssessmentArea> CreateObstacleAssesmentArea()
        {
            var aircraftCharecteristic = new AircraftCharacteristic();
            var aircraftCat = _bLogic.ModelAreamParam.Category;

            var assessedUom = UomDistanceVertical.M;
            var valSssessedAltitude = new ValDistanceVertical();
            if (InitHolding.HeightUnit == VerticalDistanceType.Ft)
                assessedUom = UomDistanceVertical.FT;

            valSssessedAltitude.Value = _assessedAltitude;
            valSssessedAltitude.Uom = assessedUom;

            var selectedMoc = Common.DeConvertHeight(_bLogic.ModelAreamParam.CurMoc);
            var areaMoc = selectedMoc;

            Holding.Report assessedItem = null;
            if (_bLogic.HReport.ObstacleReport.Count!=0)
                assessedItem =
                _bLogic.HReport.ObstacleReport.Aggregate((rep1, rep2) => rep1.Penetrate > rep2.Penetrate ? rep1 : rep2);
            else
            {
                assessedItem = new Report();
            }

            var baseArea = _bLogic.HoldingGeom.AreaWithSectors;
            if (baseArea != null && assessedItem!=null)
            {
                var mlt = baseArea.ToMultiPoint();

                foreach (Aran.Geometries.Point pt in mlt)
                    pt.Z =Common.DeConvertHeight(assessedItem.Elevation+areaMoc);
            }

            var primaryObstacleAssesment = new ObstacleAssessmentArea();
            primaryObstacleAssesment.AssessedAltitude = valSssessedAltitude;
            primaryObstacleAssesment.AircraftCategory.Add(aircraftCharecteristic);
            primaryObstacleAssesment.Type = CodeObstacleAssessmentSurface.PRIMARY;
            primaryObstacleAssesment.Surface = GeomFunctions.ConvertMultiPolygonToSurface
                (GlobalParams.SpatialRefOperation.ToGeo<MultiPolygon>(_bLogic.HoldingGeom.AreaWithSectors));

            var bufferAreaObstacleAssessmentList = new List<ObstacleAssessmentArea>();
          

            if (_bLogic.HReport.ObstacleReport != null && _bLogic.HReport.ObstacleReport.Count > 0)
            {
                for (int i = 0; i < _bLogic.HoldingGeom.Areas.Count; i++)
                {
                    if (i == 1)
                        areaMoc = Common.ConvertHeight(selectedMoc - 150, roundType.toNearest);
                    else if (i > 1)
                        areaMoc = Common.ConvertHeight(selectedMoc - 150 - 30*(i - 1), roundType.toNearest);

                    var areaAssessedAltitude = 0.0;
                    var areaElevation = 0.0;
                    try
                    {
                        areaAssessedAltitude = _bLogic.HReport.ObstacleReport.Where(rep => rep.AreaNumber == i + 1)
                            .Max(rep => rep.Elevation) + _bLogic.ModelAreamParam.CurMoc;

                        areaElevation = Common.DeConvertHeight(areaMoc + assessedItem.Elevation);
                    }
                    catch (Exception)
                    {
                        areaAssessedAltitude = 0;
                    }


                    areaAssessedAltitude = Math.Abs(areaAssessedAltitude) < 0.01 ? areaMoc : areaAssessedAltitude;

                    var areaGeom = _bLogic.HoldingGeom.Areas[i] as MultiPolygon;
                    if (areaGeom != null)
                    {
                        var mlt = areaGeom.ToMultiPoint();

                        foreach (Aran.Geometries.Point pt in mlt)
                            pt.Z = areaElevation;
                    }

                    var tmpObstacleAssesment = new ObstacleAssessmentArea();
                    tmpObstacleAssesment.AssessedAltitude = new ValDistanceVertical(areaAssessedAltitude, assessedUom);
                    tmpObstacleAssesment.AircraftCategory.Add(aircraftCharecteristic);
                    tmpObstacleAssesment.Type = CodeObstacleAssessmentSurface.SECONDARY;
                    tmpObstacleAssesment.Surface =
                        GeomFunctions.ConvertMultiPolygonToSurface(
                            GlobalParams.SpatialRefOperation.ToGeo<MultiPolygon>(
                                _bLogic.HoldingGeom.Areas[i] as MultiPolygon));
                    bufferAreaObstacleAssessmentList.Add(tmpObstacleAssesment);
                }
            }


            var dbProvider = GlobalParams.AranEnvironment.DbProvider as DbProvider;
            if (dbProvider.ProviderType == DbProviderType.Aran)
            {
                if (_bLogic.HReport.ObstacleReport != null)
                {
                    foreach (var item in _bLogic.HReport.ObstacleReport)
                    {
                        if (item.SurfaceType == ObstactleReportType.BasicArea)
                        {
                            primaryObstacleAssesment.SignificantObstacle.Add(new Obstruction
                            {
                                VerticalStructureObstruction = item.Obstacle.GetFeatureRef()
                            });
                        }
                        else
                        {
                            bufferAreaObstacleAssessmentList[item.AreaNumber - 1].SignificantObstacle.Add(new Obstruction
                            {
                                VerticalStructureObstruction = item.Obstacle.GetFeatureRef()
                            });
                        }
                    }
                }
            }
            var result = new List<ObstacleAssessmentArea>();
            result.Add(primaryObstacleAssesment);
            result.AddRange(bufferAreaObstacleAssessmentList);
            return result;
        }

        private void CreateSegmentPoint()
        {
            var modelPbn = _bLogic.ModelPBN;
            var modelPointChoise = _bLogic.ModelPtChoise;
            var priorFixTolerance = modelPointChoise.ATT;

            var ptReference = new PointReference();
            ptReference.FixToleranceArea = CreateToleranceArea(_bLogic.HoldingGeom.ToleranceArea);
            ptReference.PostFixTolerance = new ValDistanceSigned();

            if (modelPbn.CurFlightPhase.FlightPhaseName == InitHolding.FlightPhaseValue[flightPhase.Enroute])
                SegmentPt = new EnRouteSegmentPoint();
            else
                SegmentPt = new TerminalSegmentPoint();

            SegmentPt.PointChoice = new SignificantPoint();
            switch (_bLogic.ModelPtChoise.PointChoise.Choice)
            {
                case SignificantPointChoice.DesignatedPoint:
                    SegmentPt.PointChoice.FixDesignatedPoint = _bLogic.ModelPtChoise.CurSigPoint.GetFeatureRef();
                    ptReference.Point = SegmentPt.PointChoice.FixDesignatedPoint;
                    break;
                case SignificantPointChoice.Navaid:
                    SegmentPt.PointChoice.NavaidSystem = _bLogic.ModelPtChoise.CurNavaid.GetFeatureRef();
                    break;
                default:
                    SegmentPt.PointChoice.Position = new AixmPoint();
                    SegmentPt.PointChoice.Position.Geo.X = modelPointChoise.CurPoint.X;
                    SegmentPt.PointChoice.Position.Geo.Y = modelPointChoise.CurPoint.Y;
                    break;
            }

            // SegmentPt.FacilityMakeup
            
            if (InitHolding.DistanceUnit == HorizantalDistanceType.KM)
            {
                ptReference.PostFixTolerance.Uom = UomDistance.KM;
                ptReference.PostFixTolerance.Value = (modelPointChoise.ATT * 0.001);
            }
            else if (InitHolding.DistanceUnit == HorizantalDistanceType.NM)
            {
                ptReference.PostFixTolerance.Uom = UomDistance.NM;
                ptReference.PostFixTolerance.Value = (modelPointChoise.ATT*(1.0/1852.0));
            }

            ptReference.PriorFixTolerance = ptReference.PostFixTolerance;
            SegmentPt.FacilityMakeup.Add(ptReference);

            SegmentPt.FlyOver = true;
            SegmentPt.RadarGuidance = false;
            SegmentPt.ReportingATC = CodeATCReporting.COMPULSORY;
            SegmentPt.Waypoint = true;
        }

        private Surface CreateToleranceArea(Aran.Geometries.Ring toleranceArea)
        {
            if (toleranceArea == null)
                return null;

            var surface = new Surface();
            var poly = new Aran.Geometries.Polygon();
            poly.ExteriorRing = GlobalParams.SpatialRefOperation.ToGeo<Ring>(toleranceArea);
            surface.Geo.Add(poly);
            return surface;
        }

        public byte[] CreateHtmlReport()
        {
            try
            {
                var report = new DoddleReport.Report(_bLogic.HReport.ObstacleReport.ToReportSource()); 
                report.RenderingRow += new EventHandler<ReportRowEventArgs>(report_RenderingRow);

                report.TextFields.Title = "Holding RNAV";
                                          
                report.TextFields.SubTitle = "";
                report.TextFields.Footer = "Copyright 2018 &copy; R.I.S.K Company";

                string wayPoint = "";
                if (_bLogic.ModelPtChoise.PointChoise.Choice == SignificantPointChoice.DesignatedPoint)
                    wayPoint = _bLogic.ModelPtChoise.CurSigPoint.Designator + " (Designatedpoint)";
                else if (_bLogic.ModelPtChoise.PointChoise.Choice == SignificantPointChoice.Navaid)
                    wayPoint = _bLogic.ModelPtChoise.CurNavaid.Designator + " (Navaid)";
                else
                    wayPoint = "New Point";

                string procedureType = "";
                if (_bLogic.ProcedureType.PropType == ProcedureType.withHoldingFunc)
                    procedureType = "RNAV systems with holding functionality";
                
                else if (_bLogic.ProcedureType.PropType == ProcedureType.withoutHoldingFunc)
                        procedureType = "RNAV systems without holding functionality";
                
                else if (_bLogic.ProcedureType.PropType == ProcedureType.RNP)
                        procedureType = "RNP holding";
                
               double altitude = Common.DeConvertHeight(_bLogic.ModelAreamParam.Altitude);
               double ias = Common.DeConvertSpeed(_bLogic.ModelAreamParam.Ias);

               double H = altitude / 1000.0;
               double w = (12.0 * H + 87.0) / 3.6;

               string windSpeed = Common.ConvertSpeed_(w, roundType.toNearest)+" "+InitHolding.SpeedConverter.Unit;
                
                string tas =
                    Common.ConvertSpeed_(
                        ARANMath.IASToTASForRnav(ias, altitude, 15),
                        roundType.toNearest) + " " + InitHolding.SpeedConverter.Unit;


                double RV = Shablons.TurnRadius(ias, altitude, 15);
                string turnRadius = Common.ConvertDistance(RV, roundType.toNearest) + " " + InitHolding.DistanceConverter.Unit;
                
                string time = "";
                string outBoundLegLength = "";
                if (_bLogic.ProcedureType.CurDistanceType == DistanceType.Time)
                {
                    time = _bLogic.ProcedureType.Time + " Min";
                    double length = ARANMath.IASToTASForRnav(ias, altitude, 15) * _bLogic.ProcedureType.Time * 60;
                    outBoundLegLength = Common.ConvertDistance(length, roundType.toNearest) + " " + InitHolding.DistanceConverter.Unit;
                }
                else
                {
                   // timeCaption = "WD :";
                    time = _bLogic.ProcedureType.WD + " " + InitHolding.DistanceConverter.Unit;
                    double wd = Common.DeConvertDistance(_bLogic.ProcedureType.WD);

                    outBoundLegLength = Common.ConvertDistance(Math.Sqrt(wd * wd - 4 * RV * RV), roundType.toNearest) + " " + InitHolding.DistanceConverter.Unit;

                }

                var coordPrecision = 2;

                string longtitude = ARANFunctions.Degree2String(_bLogic.ModelPtChoise.CurPoint.X, Degree2StringMode.DMSLon,coordPrecision);
                string latitude = ARANFunctions.Degree2String(_bLogic.ModelPtChoise.CurPoint.Y, Degree2StringMode.DMSLat,coordPrecision);

				//Aran.PANDA.Common.ARANFunctions.Dd2DmsStr(_bLogic.ModelPtChoise.CurPoint.X, _bLogic.ModelPtChoise.CurPoint.Y, ".", "E", "N", 1,coordPrecision,
				//    out longtitude, out latitude);

				report.TextFields.Header = string.Format(@"
                Report Generated : {0}
                Aerodrome : {1}
                Waypoint : {2}
                Latitude : {3}
                Longtitude : {4}
                Flight Phase : {5}
                Reciever : {6}
                PBN : {7}
                Procedure type : {8}
                Protection of entries : {9}
                Altitude(from MSL) : {10}
                IAS : {11}
                TAS : {12}    
                Inbound course(T) : {13}
                Time(min) : {14}
                Wind Speed : {15}
                ATT : {16}
                XTT : {17}
                Turn radius :{18}
                Outbound Leg :{19}
                Elevations and heights are in : {20}
                Distances are in : {21}
                ", DateTime.Now,InitHolding.CurAdhp.Name, wayPoint,latitude,longtitude,
                    _bLogic.ModelPBN.CurFlightPhase.UserFlightPhaseName, _bLogic.ModelPBN.CurReciever.RecieverName,
                    _bLogic.ModelPBN.CurPBN.PBNName, procedureType, _bLogic.ProcedureType.ProtectionType.ToString(),
                    _bLogic.ModelAreamParam.Altitude + " " + InitHolding.HeightConverter.Unit, 
                    _bLogic.ModelAreamParam.Ias + " " + InitHolding.SpeedConverter.Unit,
                    tas,_bLogic.ModelAreamParam.Radial + " " + "°",time,
                    windSpeed,Common.ConvertDistance(_bLogic.ModelPtChoise.ATT, roundType.toNearest) + " " + InitHolding.DistanceConverter.Unit,
                    Common.ConvertDistance(_bLogic.ModelPtChoise.XTT, roundType.toNearest) + " " + InitHolding.DistanceConverter.Unit,
                    turnRadius,outBoundLegLength,
                    InitHolding.HeightConverter.Unit == "m" ? "m" : "Foot",
                    InitHolding.DistanceConverter.Unit);

                // Customize the data fields
                report.DataFields["Altitude"].Hidden = true;
                report.DataFields["Validation"].Hidden = true;
                report.DataFields["Obstacle"].Hidden = true;
                report.DataFields["SurfaceType"].Hidden = true;
                report.DataFields["AreaNumber"].Hidden = true;
                
                report.DataFields["Elevation"].HeaderText = "Abs.H";
                report.DataFields["Moc"].HeaderText = "MOC";
                report.DataFields["Req_H"].HeaderText = "Req.H";
                report.DataFields["Penetrate"].HeaderText = "dh Penetrate";

                var dlg = new SaveFileDialog();
                dlg.FileName = "RNAV Holding report"; // Default file name
                dlg.DefaultExt = ".text"; // Default file extension
                dlg.Title = "Save Omega Report";
                dlg.Filter = "Html documents|*.htm" +
                             "|Pdf document|*.pdf" +
                             "|Excel Worksheets|*.xls";
                // Show save file dialog box
                
                var result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == DialogResult.OK)
                {
                    System.IO.Stream stream = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.OpenOrCreate);
                    if (dlg.FilterIndex == 1)
                    {
                        var writer = new HtmlReportWriter();
                        writer.WriteReport(report, stream);
                    }
                    else if (dlg.FilterIndex == 2)
                    {
                        var writer = new DoddleReport.iTextSharp.PdfReportWriter();
                        writer.WriteReport(report, stream);
                    }
                    else if (dlg.FilterIndex == 3)
                    {
                        var writer = new ExcelReportWriter();
                        writer.WriteReport(report, stream);
                    }

                    byte[] repoBytes;
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        var memWriter = new DoddleReport.OpenXml.ExcelReportWriter();
                        memWriter.WriteReport(report, memoryStream);
                        repoBytes = memoryStream.ToArray();
                    }
                    
                    MessageBox.Show(Resources.The_document_was_saved_successfully, Resources.Holding_Caption,
                        MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return repoBytes;
                }
            }
            catch (Exception e)
            {
                GlobalParams.AranEnvironment.GetLogger("Rnav Holding").Error(e);

                MessageBox.Show(Resources.Error_occured_while_saving_document, Resources.Holding_Caption, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return null;
        }

        private void report_RenderingRow(object sender, ReportRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;
            var penetrate = (double)e.Row["Penetrate"];
            if (penetrate > 0)
            {
                e.Row.Fields["Id"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["Name"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["Elevation"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["Plane"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["GeomType"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["VsType"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["Penetrate"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["Y"].DataStyle.ForeColor = System.Drawing.Color.Red;
                e.Row.Fields["X"].DataStyle.ForeColor = System.Drawing.Color.Red;
            }
        }

        public SegmentPoint SegmentPt { get; set; }
        public DesignatedPoint WayPoint { get; set; }
        public HoldingPattern HPattern { get; set; }
        public HoldingAssessment HAssessMent { get; set; }
        
    }
}
