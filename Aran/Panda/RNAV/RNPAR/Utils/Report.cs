using System.Globalization;
using Aran.Panda.RNAV.RNPAR.Properties;
using Aran.PANDA.Common;
using System.Linq;
using Aran.Panda.RNAV.RNPAR.UI.ViewModel.Procedure;
using Env = Aran.Panda.RNAV.RNPAR.Context.AppEnvironment;

namespace Aran.Panda.RNAV.RNPAR.Utils
{
    public class Report
    {
        protected UnitConverter UnitConverter => Env.Current.UnitContext.UnitConverter;
        protected string HeightUnit => UnitConverter.HeightUnit;
        protected string HeightUnitM => UnitConverter.HeightUnitM;
        protected string DistanceUnit => UnitConverter.DistanceUnit;
        protected string SpeedUnit => UnitConverter.SpeedUnit;

        public void SaveAccuracy(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            var accurRep = new ReportFile();

            accurRep.OpenFile(RepFileName + "_Accuracy", RepFileTitle + ": " + Resources.str00173);
            //AccurRep.H1(My.Resources.str15479 + " - " + RepFileTitle + ": " + My.Resources.str00173);
            accurRep.WriteString(Resources.str15479 + " - " + RepFileTitle + ": " + Resources.str00173, true);

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            accurRep.WriteHeader(pReport);
            //accurRep.Param("Distance accuracy", GlobalVars.settings.DistancePrecision.ToString(), GlobalVars.DistanceConverter[GlobalVars.DistanceUnit].Unit);
            //accurRep.Param("Angle accuracy", GlobalVars.settings.AnglePrecision.ToString(), "degrees");

            //AccurRep.WriteMessage();
            accurRep.WriteMessage("=================================================");
            accurRep.WriteMessage();

            // =============================================================================================================
            var finalPhaseLegs = Env.Current.RNPContext.FinalPhase.CurrentState._FASLegs;
            //var preFinalState = Env.Current.RNPContext.PreFinalPhase.CurrentState;
            var intermediateLegs = Env.Current.RNPContext.IntermediatePhase.CurrentState._ImASLegs;

            if (finalPhaseLegs != null && finalPhaseLegs.Count() > 0)
                foreach (var leg in finalPhaseLegs)
                {
                    Functions.SaveFixAccurasyInfo(accurRep, leg.StartPrj, "WPT");
                }

            if (intermediateLegs != null && intermediateLegs.Count() > 0)
                foreach (var leg in finalPhaseLegs)
                {
                    Functions.SaveFixAccurasyInfo(accurRep, leg.StartPrj, "WPT");
                }



            // =============================================================================================================

            accurRep.CloseFile();
        }

        public void SaveGeometry(string RepFileName, string RepFileTitle, ReportHeader pReport, ReportPoint[] reportPoints, double allRoutsLen)
        {
            var distanceConverter = Env.Current.UnitContext.UnitConverter.DistanceConverter[Env.Current.UnitContext.UnitConverter.DistanceUnitIndex];

            var GuidGeomRep = new ReportFile();
            //TODO RNPAR set ThrPtPrj
            GuidGeomRep.ThrPtPrj = Env.Current.RNPContext.PreFinalPhase.CurrentState.ptTHRprj;

            GuidGeomRep.OpenFile(RepFileName + "_Geometry", RepFileTitle + ": " + Resources.str00517);
            GuidGeomRep.WriteString(Resources.str15479 + " - " + RepFileTitle + ": " + Resources.str00517, true);


            GuidGeomRep.WriteString("");
            GuidGeomRep.WriteString(RepFileTitle, true);
            GuidGeomRep.WriteHeader(pReport);

            GuidGeomRep.WriteString("");
            GuidGeomRep.WriteString("");


            int n = reportPoints.Length;
            for (int i = 0; i < n; i++)
                GuidGeomRep.WritePoint(reportPoints[i]);

            GuidGeomRep.WriteString("");

            GuidGeomRep.Param(Resources.str00519, Functions.ConvertDistance(allRoutsLen, eRoundMode.NEAREST).ToString(CultureInfo.InvariantCulture), distanceConverter.Unit);

            GuidGeomRep.CloseFile();
        }

        public void SaveProtocol(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            //ReportsFrm.SortForSave();

            var GuidProtRep = new ReportFile();
            //TODO RNPAR set ThrPtPrj
            //GuidProtRep.ThrPtPrj = DER.pPtPrj[eRWY.PtDER];

            GuidProtRep.OpenFile(RepFileName + "_Protocol", RepFileTitle + ": " + Resources.str00170);

            GuidProtRep.WriteString(Resources.str15479 + " - " + RepFileTitle + ": " + Resources.str00170, true);
            GuidProtRep.WriteString("");
            GuidProtRep.WriteString(RepFileTitle, true);

            GuidProtRep.WriteHeader(pReport);
            GuidProtRep.WriteString("");
            GuidProtRep.WriteString("");

            //TODO RNPAR set get listview from reports
            GuidProtRep.SaveObstacles("OFZ", Env.Current.RNPContext.ReportForm.ListView01, Env.Current.RNPContext.PreFinalPhase.CurrentState.OFZObstacleList);
            GuidProtRep.SaveObstacles("OAS", Env.Current.RNPContext.ReportForm.ListView02, Env.Current.RNPContext.PreFinalPhase.CurrentState.OASObstacleList);
            GuidProtRep.SaveObstacles("FAS", Env.Current.RNPContext.ReportForm.ListView03, Env.Current.RNPContext.FinalPhase.CurrentState.FASObstaclesList);
            GuidProtRep.SaveObstacles("Intermediate Leg", Env.Current.RNPContext.ReportForm.dataGridView07, Env.Current.RNPContext.IntermediatePhase.CurrentState._ImASLegs[0].ObstaclesList);

            string name = "COORD";
            if (Env.Current.RNPContext.IntermediatePhase.CurrentState._ImASLegs.Count > 1)
                for (int i = 1; i < Env.Current.RNPContext.IntermediatePhase.CurrentState._ImASLegs.Count; i++)
                {

                    Core.Model.RFLeg leg = (Core.Model.RFLeg)Env.Current.RNPContext.IntermediatePhase.CurrentState._ImASLegs[i];
                    if (leg.legType == Model.LegType.FlyBy)
                        continue;
                    Core.Model.RFLeg nextLeg = default(Core.Model.RFLeg);
                    if (i - 1 > 0)
                    {
                        nextLeg =
                            Env.Current.RNPContext.IntermediatePhase.CurrentState._ImASLegs[i - 1].legType ==
                            Model.LegType.FlyBy
                                ? Env.Current.RNPContext.IntermediatePhase.CurrentState._ImASLegs[i - 2]
                                : Env.Current.RNPContext.IntermediatePhase.CurrentState._ImASLegs[i - 1];
                    }
                    GuidProtRep.SaveObstacles($"Initial - StartFix {(leg.IsWpt?leg.startWpt.Name:name)} - EndFix - {(nextLeg.IsWpt? nextLeg.startWpt.Name : name)}", Env.Current.RNPContext.ReportForm.dataGridView07, leg.ObstaclesList);
                }
            GuidProtRep.CloseFile();
        }

        public void SaveLog(string RepFileName, string RepFileTitle, ReportHeader pReport)
        {
            var mainViewModel = Env.Current.ApplicationContext.MainViewModel;
            var GuidLogRep = new ReportFile();

            //TODO RNPAR set ThrPtPrj
            //GuidLogRep.ThrPtPrj = DER.pPtPrj[eRWY.PtDER];

            GuidLogRep.OpenFile(RepFileName + "_Log", RepFileTitle + ": " + Resources.str00520);

            GuidLogRep.WriteString(Resources.str15479 + " - " + RepFileTitle + ": " + Resources.str00520, true);
            GuidLogRep.WriteString("");
            GuidLogRep.WriteString(RepFileTitle, true);

            GuidLogRep.WriteHeader(pReport);

            GuidLogRep.WriteString("");
            GuidLogRep.WriteString("");

            //TODO RNPAR get info from needed properties/fields

            var firstSate = mainViewModel.ViewModel;

            while (firstSate.PreviousState != null)
                firstSate = firstSate.PreviousState;

            GuidLogRep.ExH2(firstSate.Header);

            var initState = firstSate as InitializationViewModel;

            GuidLogRep.HTMLMessage("[ " + firstSate.Header + " ]");
            GuidLogRep.WriteString("");

            GuidLogRep.Param("RWY", initState.SelectedRunway.Name, "");
            GuidLogRep.Param("True bearing", initState.TrueBearing.ToString(CultureInfo.InvariantCulture), "°");
            GuidLogRep.Param("Aircraft category", initState.SelectedAircraftCategory.ToString(), "");
            GuidLogRep.Param("IAS", initState.IAS.ToString(CultureInfo.InvariantCulture), SpeedUnit);
            GuidLogRep.Param("VPA", initState.VPA.ToString(CultureInfo.InvariantCulture), "°");
            GuidLogRep.Param("Missed approach gradient", initState.MissedApproachGradient.ToString(CultureInfo.InvariantCulture), "%");
            GuidLogRep.Param("FApr track offset angle", initState.FAOffsetAngle.ToString(CultureInfo.InvariantCulture), "°");
            GuidLogRep.Param("FApr true bearing", initState.FATrueBearing.ToString(CultureInfo.InvariantCulture), "°");
            GuidLogRep.Param("FAS RNP value", initState.FASRNPValue.ToString(CultureInfo.InvariantCulture), DistanceUnit);
            GuidLogRep.Param("Preliminary DH", initState.DH.ToString(CultureInfo.InvariantCulture), DistanceUnit);
            GuidLogRep.Param("THR elevation", initState.THR.ToString(CultureInfo.InvariantCulture), HeightUnit);
            GuidLogRep.Param("Reference datum height", initState.DatumHeight.ToString(CultureInfo.InvariantCulture), HeightUnit);
            GuidLogRep.Param("Type of altimeter margin", initState.SelectedAltimeterMarginType.ToString(), "");
            GuidLogRep.Param("TWC", initState.TWC.ToString(CultureInfo.InvariantCulture), SpeedUnit);
            GuidLogRep.Param("Min Aerodrome temperature", initState.MinTemperature.ToString(CultureInfo.InvariantCulture), "C°");
            GuidLogRep.Param("Altimeter margin", initState.AltimeterMargin.ToString(CultureInfo.InvariantCulture), HeightUnit);
            GuidLogRep.Param("Alignment Pt-THR distance", initState.AltimeterTHR.ToString(CultureInfo.InvariantCulture), HeightUnitM);
            GuidLogRep.Param("FAS min OCH", initState.MinOCH.ToString(CultureInfo.InvariantCulture), HeightUnit);
            GuidLogRep.Param("MA RNP value", initState.MARNPValue.ToString(CultureInfo.InvariantCulture), DistanceUnit);
            GuidLogRep.Param("Max planned FAP ALT", initState.MaxFAPAlt.ToString(CultureInfo.InvariantCulture), HeightUnit);

            GuidLogRep.WriteString("");
            GuidLogRep.WriteString("");

            var finalState = initState.NextState as FinalApproachViewModel;

            GuidLogRep.ExH2(finalState.Header);
            GuidLogRep.HTMLMessage("[ " + finalState.Header + " ]");
            GuidLogRep.WriteString("");

            if (finalState.RfLeg)
            {
                GuidLogRep.Param("FROP distance", finalState.FropDistance, DistanceUnit);
                GuidLogRep.Param("FROP altitude", finalState.FropAltitude, HeightUnit);
                GuidLogRep.Param("Bank Angle", finalState.BankAngle, "°");
                GuidLogRep.Param("RF leg radius", finalState.RfLegRadius, DistanceUnit);
                GuidLogRep.Param("FROP WPT", finalState.SelectedFropWpt?.Name ?? "", "");
                GuidLogRep.Param("FAP WPT", finalState.SelectedRfWpt?.Name, "");
                GuidLogRep.Param("Turn direction", finalState.TurnDirection == 0 ? "Left" : "Right", "");
                GuidLogRep.Param("Entry true course", finalState.EntryTrueCourse, "°");
                GuidLogRep.Param("RF start altitude", finalState.RfStartAltitude, HeightUnit);
            }
            else
            {
                GuidLogRep.Param("FAP distance", finalState.FapDistance, DistanceUnit);
                GuidLogRep.Param("FAP altitude", finalState.FapAltitude, HeightUnit);
                GuidLogRep.Param("FAP WPT", finalState.SelectedStraightWpt?.Name, "");
            }
            GuidLogRep.WriteString("");
            GuidLogRep.Param("FAS Total length", finalState.FasTotalLength, DistanceUnit);
            GuidLogRep.Param("OAS gradient", finalState.OasGradient, "%");
            GuidLogRep.Param("OAS origin", finalState.OasOrigin, DistanceUnit);
            GuidLogRep.Param("Z surface origin", finalState.ZSurfaceOrigin, DistanceUnit);
            GuidLogRep.Param("TrD", finalState.TrD, DistanceUnit);

            GuidLogRep.WriteString("");
            GuidLogRep.WriteString("");


            var interSate = finalState.NextState as IntermediateApproachViewModel;

            GuidLogRep.ExH2(interSate.Header);
            GuidLogRep.HTMLMessage("[ " + interSate.Header + " ]");
            GuidLogRep.WriteString("");

            GuidLogRep.WriteString("");
            GuidLogRep.WriteString("");


            GuidLogRep.CloseFile();
        }


        public double ConvertTracToPoints(out ReportPoint[] GuidPoints)
        {
            double result = 0.0;

            int k = Env.Current.RNPContext.FinalPhase.CurrentState._FASLegs.Count;
            int m = Env.Current.RNPContext.IntermediatePhase.CurrentState._ImASLegs.Count;
            int n = m + k;
            if (n == 0)
            {
                GuidPoints = new ReportPoint[0];
                return 0.0;
            }

            //GuidPoints = new ReportPoint[n + 1];
            GuidPoints = new ReportPoint[n + 1];


            for (int i = 0; i < Env.Current.RNPContext.FinalPhase.CurrentState._FASLegs.Count; i++)
            {

                var leg = Env.Current.RNPContext.FinalPhase.CurrentState._FASLegs[i];
                if (i == 0)
                {
                    GuidPoints[i].TrueCourse = ARANFunctions.DirToAzimuth(leg.RollOutPrj, leg.RollOutDir, Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
                    GuidPoints[i].DistToNext = -1.0;
                    GuidPoints[i].Altitude = leg.RollOutAltitude;    // leg.UpperLimit;
                    GuidPoints[i].Radius = -1.0;

                    GuidPoints[i].Description = leg.IsEndWpt ? leg.endWpt.Name : "FAP";
                    GuidPoints[i].Lat = leg.RollOutGeo.Y;
                    GuidPoints[i].Lon = leg.RollOutGeo.X;
                    if (leg.legType == Model.LegType.FixedRadius)
                    {
                        GuidPoints[i].DistToNext = leg.DistToNext;
                    }
                }

                GuidPoints[i + 1].TrueCourse = ARANFunctions.DirToAzimuth(leg.RollOutPrj, leg.RollOutDir, Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
                GuidPoints[i + 1].DistToNext = leg.DistToNext;
                GuidPoints[i + 1].Altitude = leg.StartAltitude;    // leg.UpperLimit;
                GuidPoints[i + 1].Radius = -1.0;

                GuidPoints[i + 1].Description = leg.IsWpt?leg.startWpt.Name:"COORD";
                var startGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(leg.StartPrj);
                GuidPoints[i + 1].Lat = startGeo.Y;
                GuidPoints[i + 1].Lon = startGeo.X;

                if (leg.legType != Model.LegType.Straight)
                {
                    GuidPoints[i + 1].Radius = leg.Radius;
                    var centerGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(leg.Center);
                    GuidPoints[i + 1].CenterLat= centerGeo.Y;
                    GuidPoints[i + 1].CenterLon = centerGeo.X;
                    GuidPoints[i + 1].DistToNext = leg.Nominal.Length;
                }

                result += leg.Nominal.Length;
                
            }

            for (int i = 0; i < Env.Current.RNPContext.IntermediatePhase.CurrentState._ImASLegs.Count; i++)
            {
                var leg = Env.Current.RNPContext.IntermediatePhase.CurrentState._ImASLegs[i];
                GuidPoints[k + i + 1].TrueCourse = ARANFunctions.DirToAzimuth(leg.RollOutPrj, leg.RollOutDir, Env.Current.SpatialContext.SpatialReferenceProjection, Env.Current.SpatialContext.SpatialReferenceGeo);
                GuidPoints[k + i + 1].DistToNext = leg.DistToNext;
                GuidPoints[k + i + 1].Altitude = leg.StartAltitude;    // leg.UpperLimit;
                GuidPoints[k + i + 1].Radius = -1.0; 

                GuidPoints[k + i + 1].Description = leg.IsWpt ? leg.startWpt.Name : "COORD";
                GuidPoints[k + i + 1].Lat = leg.StartGeo.Y;
                GuidPoints[k + i + 1].Lon = leg.StartGeo.X;

                if (leg.legType != Model.LegType.Straight)
                {
                    GuidPoints[k + i + 1].Radius = leg.Radius;
                    var centerGeo = Env.Current.SpatialContext.SpatialReferenceOperation.ToGeo(leg.Center);
                    GuidPoints[k + i + 1].CenterLat = centerGeo.Y;
                    GuidPoints[k + i + 1].CenterLon = centerGeo.X;
                    GuidPoints[k + i + 1].DistToNext = leg.Nominal.Length;
                }

                result += leg.Nominal.Length;

            }
            return result;
        }
    }
}
