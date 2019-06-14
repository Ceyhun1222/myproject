using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.PropertyEnum;

namespace Aran.Aim.ValidationInfo
{
    public static class ValidationInfo
    {
        public static bool NoInfo (out double? min, out double? max, out string pattern)
        {
            min = null;
            max = null;
            pattern = null;
            return false;
        }
        public static bool GetFeatureMinMax (FeatureType featureType, int propertyIndex, out double? min, out double? max, out string pattern)
        {
            switch (featureType)
            {
                case FeatureType.RunwayProtectArea:
                    return RunwayProtectAreaInfo ((PropertyRunwayProtectArea) propertyIndex, out min, out max, out pattern);
                case FeatureType.RunwayDirection:
                    return RunwayDirectionInfo ((PropertyRunwayDirection) propertyIndex, out min, out max, out pattern);
                case FeatureType.RunwayCentrelinePoint:
                    return RunwayCentrelinePointInfo ((PropertyRunwayCentrelinePoint) propertyIndex, out min, out max, out pattern);
                case FeatureType.Runway:
                    return RunwayInfo ((PropertyRunway) propertyIndex, out min, out max, out pattern);
                case FeatureType.ArrestingGear:
                    return ArrestingGearInfo ((PropertyArrestingGear) propertyIndex, out min, out max, out pattern);
                case FeatureType.RunwayElement:
                    return RunwayElementInfo ((PropertyRunwayElement) propertyIndex, out min, out max, out pattern);
                case FeatureType.VisualGlideSlopeIndicator:
                    return VisualGlideSlopeIndicatorInfo ((PropertyVisualGlideSlopeIndicator) propertyIndex, out min, out max, out pattern);
                case FeatureType.RunwayBlastPad:
                    return RunwayBlastPadInfo ((PropertyRunwayBlastPad) propertyIndex, out min, out max, out pattern);
                case FeatureType.Taxiway:
                    return TaxiwayInfo ((PropertyTaxiway) propertyIndex, out min, out max, out pattern);
                case FeatureType.TaxiwayElement:
                    return TaxiwayElementInfo ((PropertyTaxiwayElement) propertyIndex, out min, out max, out pattern);
                case FeatureType.GuidanceLine:
                    return GuidanceLineInfo ((PropertyGuidanceLine) propertyIndex, out min, out max, out pattern);
                case FeatureType.Apron:
                    return ApronInfo ((PropertyApron) propertyIndex, out min, out max, out pattern);
                case FeatureType.ApronElement:
                    return ApronElementInfo ((PropertyApronElement) propertyIndex, out min, out max, out pattern);
                case FeatureType.AircraftStand:
                    return AircraftStandInfo ((PropertyAircraftStand) propertyIndex, out min, out max, out pattern);
                case FeatureType.Road:
                    return RoadInfo ((PropertyRoad) propertyIndex, out min, out max, out pattern);
                case FeatureType.TouchDownLiftOffSafeArea:
                    return TouchDownLiftOffSafeAreaInfo ((PropertyTouchDownLiftOffSafeArea) propertyIndex, out min, out max, out pattern);
                case FeatureType.TouchDownLiftOff:
                    return TouchDownLiftOffInfo ((PropertyTouchDownLiftOff) propertyIndex, out min, out max, out pattern);
                case FeatureType.ApproachLightingSystem:
                    return ApproachLightingSystemInfo ((PropertyApproachLightingSystem) propertyIndex, out min, out max, out pattern);
                case FeatureType.SurveyControlPoint:
                    return SurveyControlPointInfo ((PropertySurveyControlPoint) propertyIndex, out min, out max, out pattern);
                case FeatureType.AirportHeliport:
                    return AirportHeliportInfo ((PropertyAirportHeliport) propertyIndex, out min, out max, out pattern);
                case FeatureType.AirportHotSpot:
                    return AirportHotSpotInfo ((PropertyAirportHotSpot) propertyIndex, out min, out max, out pattern);
                case FeatureType.Airspace:
                    return AirspaceInfo ((PropertyAirspace) propertyIndex, out min, out max, out pattern);
                case FeatureType.GeoBorder:
                    return GeoBorderInfo ((PropertyGeoBorder) propertyIndex, out min, out max, out pattern);
                case FeatureType.PrecisionApproachRadar:
                    return PrecisionApproachRadarInfo ((PropertyPrecisionApproachRadar) propertyIndex, out min, out max, out pattern);
                case FeatureType.PrimarySurveillanceRadar:
                    return PrimarySurveillanceRadarInfo ((PropertyPrimarySurveillanceRadar) propertyIndex, out min, out max, out pattern);
                case FeatureType.RadarSystem:
                    return RadarSystemInfo ((PropertyRadarSystem) propertyIndex, out min, out max, out pattern);
                case FeatureType.SecondarySurveillanceRadar:
                    return SecondarySurveillanceRadarInfo ((PropertySecondarySurveillanceRadar) propertyIndex, out min, out max, out pattern);
                case FeatureType.HoldingAssessment:
                    return HoldingAssessmentInfo ((PropertyHoldingAssessment) propertyIndex, out min, out max, out pattern);
                case FeatureType.StandardLevelSector:
                    return StandardLevelSectorInfo ((PropertyStandardLevelSector) propertyIndex, out min, out max, out pattern);
                case FeatureType.RadioFrequencyArea:
                    return RadioFrequencyAreaInfo ((PropertyRadioFrequencyArea) propertyIndex, out min, out max, out pattern);
                case FeatureType.SpecialDate:
                    return SpecialDateInfo ((PropertySpecialDate) propertyIndex, out min, out max, out pattern);
                case FeatureType.RadioCommunicationChannel:
                    return RadioCommunicationChannelInfo ((PropertyRadioCommunicationChannel) propertyIndex, out min, out max, out pattern);
                case FeatureType.PilotControlledLighting:
                    return PilotControlledLightingInfo ((PropertyPilotControlledLighting) propertyIndex, out min, out max, out pattern);
                case FeatureType.InformationService:
                    return InformationServiceInfo ((PropertyInformationService) propertyIndex, out min, out max, out pattern);
                case FeatureType.GroundTrafficControlService:
                    return GroundTrafficControlServiceInfo ((PropertyGroundTrafficControlService) propertyIndex, out min, out max, out pattern);
                case FeatureType.AirTrafficControlService:
                    return AirTrafficControlServiceInfo ((PropertyAirTrafficControlService) propertyIndex, out min, out max, out pattern);
                case FeatureType.AirTrafficManagementService:
                    return AirTrafficManagementServiceInfo ((PropertyAirTrafficManagementService) propertyIndex, out min, out max, out pattern);
                case FeatureType.SearchRescueService:
                    return SearchRescueServiceInfo ((PropertySearchRescueService) propertyIndex, out min, out max, out pattern);
                case FeatureType.PassengerService:
                    return PassengerServiceInfo ((PropertyPassengerService) propertyIndex, out min, out max, out pattern);
                case FeatureType.AircraftGroundService:
                    return AircraftGroundServiceInfo ((PropertyAircraftGroundService) propertyIndex, out min, out max, out pattern);
                case FeatureType.FireFightingService:
                    return FireFightingServiceInfo ((PropertyFireFightingService) propertyIndex, out min, out max, out pattern);
                case FeatureType.AirportClearanceService:
                    return AirportClearanceServiceInfo ((PropertyAirportClearanceService) propertyIndex, out min, out max, out pattern);
                case FeatureType.AirportSuppliesService:
                    return AirportSuppliesServiceInfo ((PropertyAirportSuppliesService) propertyIndex, out min, out max, out pattern);
                case FeatureType.AngleIndication:
                    return AngleIndicationInfo ((PropertyAngleIndication) propertyIndex, out min, out max, out pattern);
                case FeatureType.DistanceIndication:
                    return DistanceIndicationInfo ((PropertyDistanceIndication) propertyIndex, out min, out max, out pattern);
                case FeatureType.Azimuth:
                    return AzimuthInfo ((PropertyAzimuth) propertyIndex, out min, out max, out pattern);
                case FeatureType.CheckpointINS:
                    return CheckpointINSInfo ((PropertyCheckpointINS) propertyIndex, out min, out max, out pattern);
                case FeatureType.CheckpointVOR:
                    return CheckpointVORInfo ((PropertyCheckpointVOR) propertyIndex, out min, out max, out pattern);
                case FeatureType.DME:
                    return DMEInfo ((PropertyDME) propertyIndex, out min, out max, out pattern);
                case FeatureType.Elevation:
                    return ElevationInfo ((PropertyElevation) propertyIndex, out min, out max, out pattern);
                case FeatureType.Glidepath:
                    return GlidepathInfo ((PropertyGlidepath) propertyIndex, out min, out max, out pattern);
                case FeatureType.Localizer:
                    return LocalizerInfo ((PropertyLocalizer) propertyIndex, out min, out max, out pattern);
                case FeatureType.MarkerBeacon:
                    return MarkerBeaconInfo ((PropertyMarkerBeacon) propertyIndex, out min, out max, out pattern);
                case FeatureType.Navaid:
                    return NavaidInfo ((PropertyNavaid) propertyIndex, out min, out max, out pattern);
                case FeatureType.SDF:
                    return SDFInfo ((PropertySDF) propertyIndex, out min, out max, out pattern);
                case FeatureType.NDB:
                    return NDBInfo ((PropertyNDB) propertyIndex, out min, out max, out pattern);
                case FeatureType.SpecialNavigationStation:
                    return SpecialNavigationStationInfo ((PropertySpecialNavigationStation) propertyIndex, out min, out max, out pattern);
                case FeatureType.VOR:
                    return VORInfo ((PropertyVOR) propertyIndex, out min, out max, out pattern);
                case FeatureType.TACAN:
                    return TACANInfo ((PropertyTACAN) propertyIndex, out min, out max, out pattern);
                case FeatureType.SpecialNavigationSystem:
                    return SpecialNavigationSystemInfo ((PropertySpecialNavigationSystem) propertyIndex, out min, out max, out pattern);
                case FeatureType.DirectionFinder:
                    return DirectionFinderInfo ((PropertyDirectionFinder) propertyIndex, out min, out max, out pattern);
                case FeatureType.DesignatedPoint:
                    return DesignatedPointInfo ((PropertyDesignatedPoint) propertyIndex, out min, out max, out pattern);
                case FeatureType.AeronauticalGroundLight:
                    return AeronauticalGroundLightInfo ((PropertyAeronauticalGroundLight) propertyIndex, out min, out max, out pattern);
                case FeatureType.OrganisationAuthority:
                    return OrganisationAuthorityInfo ((PropertyOrganisationAuthority) propertyIndex, out min, out max, out pattern);
                case FeatureType.Unit:
                    return UnitInfo ((PropertyUnit) propertyIndex, out min, out max, out pattern);
                case FeatureType.VerticalStructure:
                    return VerticalStructureInfo ((PropertyVerticalStructure) propertyIndex, out min, out max, out pattern);
                case FeatureType.TerminalArrivalArea:
                    return TerminalArrivalAreaInfo ((PropertyTerminalArrivalArea) propertyIndex, out min, out max, out pattern);
                case FeatureType.InstrumentApproachProcedure:
                    return InstrumentApproachProcedureInfo ((PropertyInstrumentApproachProcedure) propertyIndex, out min, out max, out pattern);
                case FeatureType.StandardInstrumentDeparture:
                    return StandardInstrumentDepartureInfo ((PropertyStandardInstrumentDeparture) propertyIndex, out min, out max, out pattern);
                case FeatureType.NavigationArea:
                    return NavigationAreaInfo ((PropertyNavigationArea) propertyIndex, out min, out max, out pattern);
                case FeatureType.StandardInstrumentArrival:
                    return StandardInstrumentArrivalInfo ((PropertyStandardInstrumentArrival) propertyIndex, out min, out max, out pattern);
                case FeatureType.ArrivalFeederLeg:
                    return ArrivalFeederLegInfo ((PropertyArrivalFeederLeg) propertyIndex, out min, out max, out pattern);
                case FeatureType.ArrivalLeg:
                    return ArrivalLegInfo ((PropertyArrivalLeg) propertyIndex, out min, out max, out pattern);
                case FeatureType.DepartureLeg:
                    return DepartureLegInfo ((PropertyDepartureLeg) propertyIndex, out min, out max, out pattern);
                case FeatureType.FinalLeg:
                    return FinalLegInfo ((PropertyFinalLeg) propertyIndex, out min, out max, out pattern);
                case FeatureType.InitialLeg:
                    return InitialLegInfo ((PropertyInitialLeg) propertyIndex, out min, out max, out pattern);
                case FeatureType.IntermediateLeg:
                    return IntermediateLegInfo ((PropertyIntermediateLeg) propertyIndex, out min, out max, out pattern);
                case FeatureType.MissedApproachLeg:
                    return MissedApproachLegInfo ((PropertyMissedApproachLeg) propertyIndex, out min, out max, out pattern);
                case FeatureType.HoldingPattern:
                    return HoldingPatternInfo ((PropertyHoldingPattern) propertyIndex, out min, out max, out pattern);
                case FeatureType.UnplannedHolding:
                    return UnplannedHoldingInfo ((PropertyUnplannedHolding) propertyIndex, out min, out max, out pattern);
                case FeatureType.FlightRestriction:
                    return FlightRestrictionInfo ((PropertyFlightRestriction) propertyIndex, out min, out max, out pattern);
                case FeatureType.RouteSegment:
                    return RouteSegmentInfo ((PropertyRouteSegment) propertyIndex, out min, out max, out pattern);
                case FeatureType.Route:
                    return RouteInfo ((PropertyRoute) propertyIndex, out min, out max, out pattern);
                case FeatureType.ChangeOverPoint:
                    return ChangeOverPointInfo ((PropertyChangeOverPoint) propertyIndex, out min, out max, out pattern);
                case FeatureType.AerialRefuelling:
                    return AerialRefuellingInfo ((PropertyAerialRefuelling) propertyIndex, out min, out max, out pattern);
            }
            min = null;
            max = null;
            pattern = null;
            return false;
        }

        public static bool AirportHeliportProtectionAreaInfo (PropertyAirportHeliportProtectionArea propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAirportHeliportProtectionArea.Width:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyAirportHeliportProtectionArea.Length:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool RadarEquipmentInfo (PropertyRadarEquipment propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRadarEquipment.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
                case PropertyRadarEquipment.SerialNumber:
                    return DataTypeValidationInfo.TextDesignatorType (out min, out max, out pattern);
                case PropertyRadarEquipment.Range:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyRadarEquipment.RangeAccuracy:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyRadarEquipment.MagneticVariation:
                    return DataTypeValidationInfo.ValMagneticVariationType (out min, out max, out pattern);
                case PropertyRadarEquipment.MagneticVariationAccuracy:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyRadarEquipment.DateMagneticVariation:
                    return DataTypeValidationInfo.DateYearType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool SurveillanceRadarInfo (PropertySurveillanceRadar propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertySurveillanceRadar.VerticalCoverageAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertySurveillanceRadar.VerticalCoverageDistance:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertySurveillanceRadar.VerticalCoverageAzimuth:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertySurveillanceRadar.TiltAngle:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertySurveillanceRadar.AutomatedRadarTerminalSystem:
                    return DataTypeValidationInfo.TextDesignatorType (out min, out max, out pattern);
            }
            return RadarEquipmentInfo ((PropertyRadarEquipment) propIndex, out min, out max, out pattern);
        }
        public static bool ServiceInfo (PropertyService propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyService.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool TrafficSeparationServiceInfo (PropertyTrafficSeparationService propIndex, out double? min, out double? max, out string pattern)
        {

            return ServiceInfo ((PropertyService) propIndex, out min, out max, out pattern);
        }
        public static bool AirportGroundServiceInfo (PropertyAirportGroundService propIndex, out double? min, out double? max, out string pattern)
        {

            return ServiceInfo ((PropertyService) propIndex, out min, out max, out pattern);
        }
        public static bool NavaidEquipmentInfo (PropertyNavaidEquipment propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyNavaidEquipment.Designator:
                    return DataTypeValidationInfo.CodeNavaidDesignatorType (out min, out max, out pattern);
                case PropertyNavaidEquipment.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
                case PropertyNavaidEquipment.MagneticVariation:
                    return DataTypeValidationInfo.ValMagneticVariationType (out min, out max, out pattern);
                case PropertyNavaidEquipment.MagneticVariationAccuracy:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyNavaidEquipment.DateMagneticVariation:
                    return DataTypeValidationInfo.DateYearType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool NavigationSystemCheckpointInfo (PropertyNavigationSystemCheckpoint propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyNavigationSystemCheckpoint.UpperLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyNavigationSystemCheckpoint.LowerLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyNavigationSystemCheckpoint.Distance:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyNavigationSystemCheckpoint.Angle:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool ProcedureInfo (PropertyProcedure propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyProcedure.CommunicationFailureInstruction:
                    return DataTypeValidationInfo.TextInstructionType (out min, out max, out pattern);
                case PropertyProcedure.Instruction:
                    return DataTypeValidationInfo.TextInstructionType (out min, out max, out pattern);
                case PropertyProcedure.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool SegmentLegInfo (PropertySegmentLeg propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertySegmentLeg.Course:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertySegmentLeg.SpeedLimit:
                    return DataTypeValidationInfo.ValSpeedType (out min, out max, out pattern);
                case PropertySegmentLeg.BankAngle:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertySegmentLeg.Length:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertySegmentLeg.UpperLimitAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertySegmentLeg.LowerLimitAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertySegmentLeg.AltitudeOverrideATC:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertySegmentLeg.VerticalAngle:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool ApproachLegInfo (PropertyApproachLeg propIndex, out double? min, out double? max, out string pattern)
        {

            return SegmentLegInfo ((PropertySegmentLeg) propIndex, out min, out max, out pattern);
        }
        public static bool RunwayProtectAreaInfo (PropertyRunwayProtectArea propIndex, out double? min, out double? max, out string pattern)
        {

            return AirportHeliportProtectionAreaInfo ((PropertyAirportHeliportProtectionArea) propIndex, out min, out max, out pattern);
        }
        public static bool RunwayDirectionInfo (PropertyRunwayDirection propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRunwayDirection.Designator:
                    return DataTypeValidationInfo.TextDesignatorType (out min, out max, out pattern);
                case PropertyRunwayDirection.TrueBearing:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyRunwayDirection.TrueBearingAccuracy:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyRunwayDirection.MagneticBearing:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyRunwayDirection.SlopeTDZ:
                    return DataTypeValidationInfo.ValSlopeType (out min, out max, out pattern);
                case PropertyRunwayDirection.ElevationTDZ:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyRunwayDirection.ElevationTDZAccuracy:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool RunwayCentrelinePointInfo (PropertyRunwayCentrelinePoint propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRunwayCentrelinePoint.Designator:
                    return DataTypeValidationInfo.TextDesignatorType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool RunwayInfo (PropertyRunway propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRunway.Designator:
                    return DataTypeValidationInfo.TextDesignatorType (out min, out max, out pattern);
                case PropertyRunway.NominalLength:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyRunway.LengthAccuracy:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyRunway.NominalWidth:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyRunway.WidthAccuracy:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyRunway.WidthShoulder:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyRunway.LengthStrip:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyRunway.WidthStrip:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool ArrestingGearInfo (PropertyArrestingGear propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyArrestingGear.Length:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyArrestingGear.Width:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyArrestingGear.Location:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool RunwayElementInfo (PropertyRunwayElement propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRunwayElement.Length:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyRunwayElement.Width:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool VisualGlideSlopeIndicatorInfo (PropertyVisualGlideSlopeIndicator propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyVisualGlideSlopeIndicator.NumberBox:
                    return DataTypeValidationInfo.NoNumberType (out min, out max, out pattern);
                case PropertyVisualGlideSlopeIndicator.SlopeAngle:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyVisualGlideSlopeIndicator.MinimumEyeHeightOverThreshold:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool RunwayBlastPadInfo (PropertyRunwayBlastPad propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRunwayBlastPad.Length:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool TaxiwayInfo (PropertyTaxiway propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyTaxiway.Designator:
                    return DataTypeValidationInfo.TextDesignatorType (out min, out max, out pattern);
                case PropertyTaxiway.Width:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyTaxiway.WidthShoulder:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyTaxiway.Length:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool TaxiwayElementInfo (PropertyTaxiwayElement propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyTaxiwayElement.Length:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyTaxiwayElement.Width:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool GuidanceLineInfo (PropertyGuidanceLine propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyGuidanceLine.Designator:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
                case PropertyGuidanceLine.MaxSpeed:
                    return DataTypeValidationInfo.ValSpeedType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool ApronInfo (PropertyApron propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyApron.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool ApronElementInfo (PropertyApronElement propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyApronElement.Length:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyApronElement.Width:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool AircraftStandInfo (PropertyAircraftStand propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAircraftStand.Designator:
                    return DataTypeValidationInfo.TextDesignatorType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool RoadInfo (PropertyRoad propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRoad.Designator:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool TouchDownLiftOffSafeAreaInfo (PropertyTouchDownLiftOffSafeArea propIndex, out double? min, out double? max, out string pattern)
        {

            return AirportHeliportProtectionAreaInfo ((PropertyAirportHeliportProtectionArea) propIndex, out min, out max, out pattern);
        }
        public static bool TouchDownLiftOffInfo (PropertyTouchDownLiftOff propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyTouchDownLiftOff.Designator:
                    return DataTypeValidationInfo.TextDesignatorType (out min, out max, out pattern);
                case PropertyTouchDownLiftOff.Length:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyTouchDownLiftOff.Width:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyTouchDownLiftOff.Slope:
                    return DataTypeValidationInfo.ValSlopeType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool ApproachLightingSystemInfo (PropertyApproachLightingSystem propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyApproachLightingSystem.Length:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool SurveyControlPointInfo (PropertySurveyControlPoint propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertySurveyControlPoint.Designator:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool AirportHeliportInfo (PropertyAirportHeliport propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAirportHeliport.Designator:
                    return DataTypeValidationInfo.CodeAirportHeliportDesignatorType (out min, out max, out pattern);
                case PropertyAirportHeliport.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
                case PropertyAirportHeliport.LocationIndicatorICAO:
                    return DataTypeValidationInfo.CodeICAOType (out min, out max, out pattern);
                case PropertyAirportHeliport.DesignatorIATA:
                    return DataTypeValidationInfo.CodeIATAType (out min, out max, out pattern);
                case PropertyAirportHeliport.FieldElevation:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyAirportHeliport.FieldElevationAccuracy:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyAirportHeliport.MagneticVariation:
                    return DataTypeValidationInfo.ValMagneticVariationType (out min, out max, out pattern);
                case PropertyAirportHeliport.MagneticVariationAccuracy:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyAirportHeliport.DateMagneticVariation:
                    return DataTypeValidationInfo.DateYearType (out min, out max, out pattern);
                case PropertyAirportHeliport.MagneticVariationChange:
                    return DataTypeValidationInfo.ValMagneticVariationChangeType (out min, out max, out pattern);
                case PropertyAirportHeliport.TransitionAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyAirportHeliport.TransitionLevel:
                    return DataTypeValidationInfo.ValFLType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool AirportHotSpotInfo (PropertyAirportHotSpot propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAirportHotSpot.Designator:
                    return DataTypeValidationInfo.TextDesignatorType (out min, out max, out pattern);
                case PropertyAirportHotSpot.Instruction:
                    return DataTypeValidationInfo.TextInstructionType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool AirspaceInfo (PropertyAirspace propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAirspace.Designator:
                    return DataTypeValidationInfo.CodeAirspaceDesignatorType (out min, out max, out pattern);
                case PropertyAirspace.LocalType:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
                case PropertyAirspace.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
                case PropertyAirspace.UpperLowerSeparation:
                    return DataTypeValidationInfo.ValFLType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool GeoBorderInfo (PropertyGeoBorder propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyGeoBorder.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool PrecisionApproachRadarInfo (PropertyPrecisionApproachRadar propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyPrecisionApproachRadar.Slope:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyPrecisionApproachRadar.SlopeAccuracy:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
            }
            return RadarEquipmentInfo ((PropertyRadarEquipment) propIndex, out min, out max, out pattern);
        }
        public static bool PrimarySurveillanceRadarInfo (PropertyPrimarySurveillanceRadar propIndex, out double? min, out double? max, out string pattern)
        {

            return SurveillanceRadarInfo ((PropertySurveillanceRadar) propIndex, out min, out max, out pattern);
        }
        public static bool RadarSystemInfo (PropertyRadarSystem propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRadarSystem.Model:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
                case PropertyRadarSystem.BroadcastIdentifier:
                    return DataTypeValidationInfo.TextDesignatorType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool SecondarySurveillanceRadarInfo (PropertySecondarySurveillanceRadar propIndex, out double? min, out double? max, out string pattern)
        {

            return SurveillanceRadarInfo ((PropertySurveillanceRadar) propIndex, out min, out max, out pattern);
        }
        public static bool HoldingAssessmentInfo (PropertyHoldingAssessment propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyHoldingAssessment.UpperLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyHoldingAssessment.LowerLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyHoldingAssessment.SpeedLimit:
                    return DataTypeValidationInfo.ValSpeedType (out min, out max, out pattern);
                case PropertyHoldingAssessment.PatternTemplate:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
                case PropertyHoldingAssessment.LegLengthToward:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyHoldingAssessment.LegLengthAway:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool StandardLevelSectorInfo (PropertyStandardLevelSector propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyStandardLevelSector.FromTrack:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyStandardLevelSector.ToTrack:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool RadioFrequencyAreaInfo (PropertyRadioFrequencyArea propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRadioFrequencyArea.AngleScallop:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool SpecialDateInfo (PropertySpecialDate propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertySpecialDate.DateDay:
                    return DataTypeValidationInfo.DateMonthDayType (out min, out max, out pattern);
                case PropertySpecialDate.DateYear:
                    return DataTypeValidationInfo.DateYearType (out min, out max, out pattern);
                case PropertySpecialDate.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool RadioCommunicationChannelInfo (PropertyRadioCommunicationChannel propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRadioCommunicationChannel.Logon:
                    return DataTypeValidationInfo.TextDesignatorType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool PilotControlledLightingInfo (PropertyPilotControlledLighting propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyPilotControlledLighting.IntensitySteps:
                    return DataTypeValidationInfo.NoNumberType (out min, out max, out pattern);
                case PropertyPilotControlledLighting.ActivationInstruction:
                    return DataTypeValidationInfo.TextInstructionType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool InformationServiceInfo (PropertyInformationService propIndex, out double? min, out double? max, out string pattern)
        {

            return ServiceInfo ((PropertyService) propIndex, out min, out max, out pattern);
        }
        public static bool GroundTrafficControlServiceInfo (PropertyGroundTrafficControlService propIndex, out double? min, out double? max, out string pattern)
        {

            return TrafficSeparationServiceInfo ((PropertyTrafficSeparationService) propIndex, out min, out max, out pattern);
        }
        public static bool AirTrafficControlServiceInfo (PropertyAirTrafficControlService propIndex, out double? min, out double? max, out string pattern)
        {

            return TrafficSeparationServiceInfo ((PropertyTrafficSeparationService) propIndex, out min, out max, out pattern);
        }
        public static bool AirTrafficManagementServiceInfo (PropertyAirTrafficManagementService propIndex, out double? min, out double? max, out string pattern)
        {

            return ServiceInfo ((PropertyService) propIndex, out min, out max, out pattern);
        }
        public static bool SearchRescueServiceInfo (PropertySearchRescueService propIndex, out double? min, out double? max, out string pattern)
        {

            return ServiceInfo ((PropertyService) propIndex, out min, out max, out pattern);
        }
        public static bool PassengerServiceInfo (PropertyPassengerService propIndex, out double? min, out double? max, out string pattern)
        {

            return AirportGroundServiceInfo ((PropertyAirportGroundService) propIndex, out min, out max, out pattern);
        }
        public static bool AircraftGroundServiceInfo (PropertyAircraftGroundService propIndex, out double? min, out double? max, out string pattern)
        {

            return AirportGroundServiceInfo ((PropertyAirportGroundService) propIndex, out min, out max, out pattern);
        }
        public static bool FireFightingServiceInfo (PropertyFireFightingService propIndex, out double? min, out double? max, out string pattern)
        {

            return AirportGroundServiceInfo ((PropertyAirportGroundService) propIndex, out min, out max, out pattern);
        }
        public static bool AirportClearanceServiceInfo (PropertyAirportClearanceService propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAirportClearanceService.SnowPlan:
                    return DataTypeValidationInfo.TextInstructionType (out min, out max, out pattern);
            }
            return AirportGroundServiceInfo ((PropertyAirportGroundService) propIndex, out min, out max, out pattern);
        }
        public static bool AirportSuppliesServiceInfo (PropertyAirportSuppliesService propIndex, out double? min, out double? max, out string pattern)
        {

            return AirportGroundServiceInfo ((PropertyAirportGroundService) propIndex, out min, out max, out pattern);
        }
        public static bool AngleIndicationInfo (PropertyAngleIndication propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAngleIndication.Angle:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyAngleIndication.TrueAngle:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyAngleIndication.MinimumReceptionAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool DistanceIndicationInfo (PropertyDistanceIndication propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyDistanceIndication.Distance:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyDistanceIndication.MinimumReceptionAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool AzimuthInfo (PropertyAzimuth propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAzimuth.TrueBearing:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyAzimuth.TrueBearingAccuracy:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyAzimuth.MagneticBearing:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyAzimuth.AngleProportionalLeft:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyAzimuth.AngleProportionalRight:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyAzimuth.AngleCoverLeft:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyAzimuth.AngleCoverRight:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
            }
            return NavaidEquipmentInfo ((PropertyNavaidEquipment) propIndex, out min, out max, out pattern);
        }
        public static bool CheckpointINSInfo (PropertyCheckpointINS propIndex, out double? min, out double? max, out string pattern)
        {

            return NavigationSystemCheckpointInfo ((PropertyNavigationSystemCheckpoint) propIndex, out min, out max, out pattern);
        }
        public static bool CheckpointVORInfo (PropertyCheckpointVOR propIndex, out double? min, out double? max, out string pattern)
        {

            return NavigationSystemCheckpointInfo ((PropertyNavigationSystemCheckpoint) propIndex, out min, out max, out pattern);
        }
        public static bool DMEInfo (PropertyDME propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyDME.Displace:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NavaidEquipmentInfo ((PropertyNavaidEquipment) propIndex, out min, out max, out pattern);
        }
        public static bool ElevationInfo (PropertyElevation propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyElevation.AngleNominal:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyElevation.AngleMinimum:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyElevation.AngleSpan:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyElevation.AngleAccuracy:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
            }
            return NavaidEquipmentInfo ((PropertyNavaidEquipment) propIndex, out min, out max, out pattern);
        }
        public static bool GlidepathInfo (PropertyGlidepath propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyGlidepath.Slope:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyGlidepath.AngleAccuracy:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyGlidepath.Rdh:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyGlidepath.RdhAccuracy:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
            }
            return NavaidEquipmentInfo ((PropertyNavaidEquipment) propIndex, out min, out max, out pattern);
        }
        public static bool LocalizerInfo (PropertyLocalizer propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyLocalizer.MagneticBearing:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyLocalizer.MagneticBearingAccuracy:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyLocalizer.TrueBearing:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyLocalizer.TrueBearingAccuracy:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyLocalizer.Declination:
                    return DataTypeValidationInfo.ValMagneticVariationType (out min, out max, out pattern);
                case PropertyLocalizer.WidthCourse:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
                case PropertyLocalizer.WidthCourseAccuracy:
                    return DataTypeValidationInfo.ValAngleType (out min, out max, out pattern);
            }
            return NavaidEquipmentInfo ((PropertyNavaidEquipment) propIndex, out min, out max, out pattern);
        }
        public static bool MarkerBeaconInfo (PropertyMarkerBeacon propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyMarkerBeacon.AxisBearing:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyMarkerBeacon.AuralMorseCode:
                    return DataTypeValidationInfo.CodeAuralMorseType (out min, out max, out pattern);
            }
            return NavaidEquipmentInfo ((PropertyNavaidEquipment) propIndex, out min, out max, out pattern);
        }
        public static bool NavaidInfo (PropertyNavaid propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyNavaid.Designator:
                    return DataTypeValidationInfo.CodeNavaidDesignatorType (out min, out max, out pattern);
                case PropertyNavaid.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool SDFInfo (PropertySDF propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertySDF.MagneticBearing:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertySDF.TrueBearing:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
            }
            return NavaidEquipmentInfo ((PropertyNavaidEquipment) propIndex, out min, out max, out pattern);
        }
        public static bool NDBInfo (PropertyNDB propIndex, out double? min, out double? max, out string pattern)
        {

            return NavaidEquipmentInfo ((PropertyNavaidEquipment) propIndex, out min, out max, out pattern);
        }
        public static bool SpecialNavigationStationInfo (PropertySpecialNavigationStation propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertySpecialNavigationStation.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool VORInfo (PropertyVOR propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyVOR.Declination:
                    return DataTypeValidationInfo.ValMagneticVariationType (out min, out max, out pattern);
            }
            return NavaidEquipmentInfo ((PropertyNavaidEquipment) propIndex, out min, out max, out pattern);
        }
        public static bool TACANInfo (PropertyTACAN propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyTACAN.Declination:
                    return DataTypeValidationInfo.ValMagneticVariationType (out min, out max, out pattern);
            }
            return NavaidEquipmentInfo ((PropertyNavaidEquipment) propIndex, out min, out max, out pattern);
        }
        public static bool SpecialNavigationSystemInfo (PropertySpecialNavigationSystem propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertySpecialNavigationSystem.Designator:
                    return DataTypeValidationInfo.CodeSpecialNavigationChainDesignatorType (out min, out max, out pattern);
                case PropertySpecialNavigationSystem.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool DirectionFinderInfo (PropertyDirectionFinder propIndex, out double? min, out double? max, out string pattern)
        {

            return NavaidEquipmentInfo ((PropertyNavaidEquipment) propIndex, out min, out max, out pattern);
        }
        public static bool DesignatedPointInfo (PropertyDesignatedPoint propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyDesignatedPoint.Designator:
                    return DataTypeValidationInfo.CodeDesignatedPointDesignatorType (out min, out max, out pattern);
                case PropertyDesignatedPoint.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool AeronauticalGroundLightInfo (PropertyAeronauticalGroundLight propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAeronauticalGroundLight.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool OrganisationAuthorityInfo (PropertyOrganisationAuthority propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyOrganisationAuthority.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
                case PropertyOrganisationAuthority.Designator:
                    return DataTypeValidationInfo.CodeOrganisationDesignatorType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool UnitInfo (PropertyUnit propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyUnit.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
                case PropertyUnit.Designator:
                    return DataTypeValidationInfo.CodeOrganisationDesignatorType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool VerticalStructureInfo (PropertyVerticalStructure propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyVerticalStructure.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
                case PropertyVerticalStructure.Length:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyVerticalStructure.Width:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyVerticalStructure.Radius:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool TerminalArrivalAreaInfo (PropertyTerminalArrivalArea propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyTerminalArrivalArea.OuterBufferWidth:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyTerminalArrivalArea.LateralBufferWidth:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool InstrumentApproachProcedureInfo (PropertyInstrumentApproachProcedure propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyInstrumentApproachProcedure.CopterTrack:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyInstrumentApproachProcedure.CourseReversalInstruction:
                    return DataTypeValidationInfo.TextInstructionType (out min, out max, out pattern);
            }
            return ProcedureInfo ((PropertyProcedure) propIndex, out min, out max, out pattern);
        }
        public static bool StandardInstrumentDepartureInfo (PropertyStandardInstrumentDeparture propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyStandardInstrumentDeparture.Designator:
                    return DataTypeValidationInfo.TextSIDSTARDesignatorType (out min, out max, out pattern);
            }
            return ProcedureInfo ((PropertyProcedure) propIndex, out min, out max, out pattern);
        }
        public static bool NavigationAreaInfo (PropertyNavigationArea propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyNavigationArea.MinimumCeiling:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyNavigationArea.MinimumVisibility:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool StandardInstrumentArrivalInfo (PropertyStandardInstrumentArrival propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyStandardInstrumentArrival.Designator:
                    return DataTypeValidationInfo.TextSIDSTARDesignatorType (out min, out max, out pattern);
            }
            return ProcedureInfo ((PropertyProcedure) propIndex, out min, out max, out pattern);
        }
        public static bool ArrivalFeederLegInfo (PropertyArrivalFeederLeg propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyArrivalFeederLeg.RequiredNavigationPerformance:
                    return DataTypeValidationInfo.CodeRNPType (out min, out max, out pattern);
            }
            return ApproachLegInfo ((PropertyApproachLeg) propIndex, out min, out max, out pattern);
        }
        public static bool ArrivalLegInfo (PropertyArrivalLeg propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyArrivalLeg.RequiredNavigationPerformance:
                    return DataTypeValidationInfo.CodeRNPType (out min, out max, out pattern);
            }
            return SegmentLegInfo ((PropertySegmentLeg) propIndex, out min, out max, out pattern);
        }
        public static bool DepartureLegInfo (PropertyDepartureLeg propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyDepartureLeg.RequiredNavigationPerformance:
                    return DataTypeValidationInfo.CodeRNPType (out min, out max, out pattern);
                case PropertyDepartureLeg.MinimumObstacleClearanceAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
            }
            return SegmentLegInfo ((PropertySegmentLeg) propIndex, out min, out max, out pattern);
        }
        public static bool FinalLegInfo (PropertyFinalLeg propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyFinalLeg.CourseOffsetAngle:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyFinalLeg.CourseCentrelineDistance:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyFinalLeg.CourseOffsetDistance:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return ApproachLegInfo ((PropertyApproachLeg) propIndex, out min, out max, out pattern);
        }
        public static bool InitialLegInfo (PropertyInitialLeg propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyInitialLeg.RequiredNavigationPerformance:
                    return DataTypeValidationInfo.CodeRNPType (out min, out max, out pattern);
            }
            return ApproachLegInfo ((PropertyApproachLeg) propIndex, out min, out max, out pattern);
        }
        public static bool IntermediateLegInfo (PropertyIntermediateLeg propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyIntermediateLeg.RequiredNavigationPerformance:
                    return DataTypeValidationInfo.CodeRNPType (out min, out max, out pattern);
            }
            return ApproachLegInfo ((PropertyApproachLeg) propIndex, out min, out max, out pattern);
        }
        public static bool MissedApproachLegInfo (PropertyMissedApproachLeg propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyMissedApproachLeg.HeightMAPT:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyMissedApproachLeg.RequiredNavigationPerformance:
                    return DataTypeValidationInfo.CodeRNPType (out min, out max, out pattern);
            }
            return ApproachLegInfo ((PropertyApproachLeg) propIndex, out min, out max, out pattern);
        }
        public static bool HoldingPatternInfo (PropertyHoldingPattern propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyHoldingPattern.OutboundCourse:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyHoldingPattern.InboundCourse:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyHoldingPattern.UpperLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyHoldingPattern.LowerLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyHoldingPattern.SpeedLimit:
                    return DataTypeValidationInfo.ValSpeedType (out min, out max, out pattern);
                case PropertyHoldingPattern.Instruction:
                    return DataTypeValidationInfo.TextInstructionType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool UnplannedHoldingInfo (PropertyUnplannedHolding propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyUnplannedHolding.AuthorizedAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool FlightRestrictionInfo (PropertyFlightRestriction propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyFlightRestriction.Designator:
                    return DataTypeValidationInfo.CodeFlightRestrictionDesignatorType (out min, out max, out pattern);
                case PropertyFlightRestriction.Instruction:
                    return DataTypeValidationInfo.TextInstructionType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool RouteSegmentInfo (PropertyRouteSegment propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRouteSegment.UpperLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyRouteSegment.LowerLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyRouteSegment.MinimumObstacleClearanceAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyRouteSegment.TrueTrack:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyRouteSegment.MagneticTrack:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyRouteSegment.ReverseTrueTrack:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyRouteSegment.ReverseMagneticTrack:
                    return DataTypeValidationInfo.ValBearingType (out min, out max, out pattern);
                case PropertyRouteSegment.Length:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyRouteSegment.WidthLeft:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyRouteSegment.WidthRight:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
                case PropertyRouteSegment.MinimumEnrouteAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyRouteSegment.MinimumCrossingAtEnd:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyRouteSegment.MaximumCrossingAtEnd:
                    return DataTypeValidationInfo.ValDistanceVerticalType (out min, out max, out pattern);
                case PropertyRouteSegment.RequiredNavigationPerformance:
                    return DataTypeValidationInfo.CodeRNPType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool RouteInfo (PropertyRoute propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRoute.DesignatorNumber:
                    return DataTypeValidationInfo.NoNumberType (out min, out max, out pattern);
                case PropertyRoute.LocationDesignator:
                    return DataTypeValidationInfo.TextDesignatorType (out min, out max, out pattern);
                case PropertyRoute.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool ChangeOverPointInfo (PropertyChangeOverPoint propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyChangeOverPoint.Distance:
                    return DataTypeValidationInfo.ValDistanceType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
        public static bool AerialRefuellingInfo (PropertyAerialRefuelling propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAerialRefuelling.DesignatorNumber:
                    return DataTypeValidationInfo.NoNumberType (out min, out max, out pattern);
                case PropertyAerialRefuelling.DesignatorSuffix:
                    return DataTypeValidationInfo.TextDesignatorType (out min, out max, out pattern);
                case PropertyAerialRefuelling.Name:
                    return DataTypeValidationInfo.TextNameType (out min, out max, out pattern);
                case PropertyAerialRefuelling.RadarBeaconSetting:
                    return DataTypeValidationInfo.NoNumberType (out min, out max, out pattern);
                case PropertyAerialRefuelling.XbandRadarSetting:
                    return DataTypeValidationInfo.NoNumberType (out min, out max, out pattern);
            }
            return NoInfo (out min, out max, out pattern);
        }
    }
}