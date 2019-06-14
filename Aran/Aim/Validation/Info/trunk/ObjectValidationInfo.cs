using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.PropertyEnum;

namespace Aran.Aim.ValidationInfo
{
    public static class ObjectValidationInfo
    {
        public static bool NoInfo(out double? min, out double? max, out string pattern)
        {
            min = null;
            max = null;
            pattern = null;
            return false;
        }

        public static bool SurfaceContaminationInfo(PropertySurfaceContamination propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertySurfaceContamination.FrictionCoefficient:
                    return DataTypeValidationInfo.ValFrictionType(out min, out max, out pattern);
                case PropertySurfaceContamination.FurtherClearanceTime:
                    return DataTypeValidationInfo.TimeType(out min, out max, out pattern);
                case PropertySurfaceContamination.Proportion:
                    return DataTypeValidationInfo.ValPercentType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool PointInfo(PropertyAixmPoint propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAixmPoint.HorizontalAccuracy:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool NavaidEquipmentDistanceInfo(PropertyNavaidEquipmentDistance propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyNavaidEquipmentDistance.Distance:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyNavaidEquipmentDistance.DistanceAccuracy:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool RunwayDeclaredDistanceValueInfo(PropertyRunwayDeclaredDistanceValue propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRunwayDeclaredDistanceValue.Distance:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyRunwayDeclaredDistanceValue.DistanceAccuracy:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool LightActivationInfo(PropertyLightActivation propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyLightActivation.Clicks:
                    return DataTypeValidationInfo.NoNumberType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool RidgeInfo(PropertyRidge propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRidge.Distance:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool RunwayContaminationInfo(PropertyRunwayContamination propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRunwayContamination.ClearedLength:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyRunwayContamination.ClearedWidth:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyRunwayContamination.FurtherClearanceLength:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyRunwayContamination.FurtherClearanceWidth:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyRunwayContamination.ClearedLengthBegin:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return SurfaceContaminationInfo((PropertySurfaceContamination)propIndex, out min, out max, out pattern);
        }
        public static bool TaxiwayContaminationInfo(PropertyTaxiwayContamination propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyTaxiwayContamination.ClearedWidth:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return SurfaceContaminationInfo((PropertySurfaceContamination)propIndex, out min, out max, out pattern);
        }
        public static bool SurfaceContaminationLayerInfo(PropertySurfaceContaminationLayer propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertySurfaceContaminationLayer.LayerOrder:
                    return DataTypeValidationInfo.NoSequenceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool RunwaySectionContaminationInfo(PropertyRunwaySectionContamination propIndex, out double? min, out double? max, out string pattern)
        {

            return SurfaceContaminationInfo((PropertySurfaceContamination)propIndex, out min, out max, out pattern);
        }
        public static bool TouchDownLiftOffContaminationInfo(PropertyTouchDownLiftOffContamination propIndex, out double? min, out double? max, out string pattern)
        {

            return SurfaceContaminationInfo((PropertySurfaceContamination)propIndex, out min, out max, out pattern);
        }
        public static bool ApronContaminationInfo(PropertyApronContamination propIndex, out double? min, out double? max, out string pattern)
        {

            return SurfaceContaminationInfo((PropertySurfaceContamination)propIndex, out min, out max, out pattern);
        }
        public static bool AircraftStandContaminationInfo(PropertyAircraftStandContamination propIndex, out double? min, out double? max, out string pattern)
        {

            return SurfaceContaminationInfo((PropertySurfaceContamination)propIndex, out min, out max, out pattern);
        }
        public static bool AirportHeliportContaminationInfo(PropertyAirportHeliportContamination propIndex, out double? min, out double? max, out string pattern)
        {

            return SurfaceContaminationInfo((PropertySurfaceContamination)propIndex, out min, out max, out pattern);
        }
        public static bool SurfaceCharacteristicsInfo(PropertySurfaceCharacteristics propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertySurfaceCharacteristics.ClassPCN:
                    return DataTypeValidationInfo.ValPCNType(out min, out max, out pattern);
                case PropertySurfaceCharacteristics.WeightSIWL:
                    return DataTypeValidationInfo.ValWeightType(out min, out max, out pattern);
                case PropertySurfaceCharacteristics.WeightAUW:
                    return DataTypeValidationInfo.ValWeightType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool CityInfo(PropertyCity propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyCity.Name:
                    return DataTypeValidationInfo.TextNameType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool AirspaceGeometryComponentInfo(PropertyAirspaceGeometryComponent propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAirspaceGeometryComponent.OperationSequence:
                    return DataTypeValidationInfo.NoSequenceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool AirspaceVolumeInfo(PropertyAirspaceVolume propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAirspaceVolume.UpperLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyAirspaceVolume.MaximumLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyAirspaceVolume.LowerLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyAirspaceVolume.MinimumLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyAirspaceVolume.Width:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool CurveInfo(PropertyCurve propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyCurve.HorizontalAccuracy:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool ElevatedCurveInfo(PropertyElevatedCurve propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyElevatedCurve.Elevation:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyElevatedCurve.VerticalAccuracy:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return CurveInfo((PropertyCurve)propIndex, out min, out max, out pattern);
        }
        public static bool ElevatedPointInfo(PropertyElevatedPoint propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyElevatedPoint.Elevation:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyElevatedPoint.VerticalAccuracy:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return PointInfo((PropertyAixmPoint)propIndex, out min, out max, out pattern);
        }
        public static bool ElevatedSurfaceInfo(PropertyElevatedSurface propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyElevatedSurface.Elevation:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyElevatedSurface.VerticalAccuracy:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return SurfaceInfo((PropertySurface)propIndex, out min, out max, out pattern);
        }
        public static bool SurfaceInfo(PropertySurface propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertySurface.HorizontalAccuracy:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool RadarComponentInfo(PropertyRadarComponent propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyRadarComponent.CollocationGroup:
                    return DataTypeValidationInfo.NoSequenceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool ObstacleAssessmentAreaInfo(PropertyObstacleAssessmentArea propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyObstacleAssessmentArea.SectionNumber:
                    return DataTypeValidationInfo.NoNumberType(out min, out max, out pattern);
                case PropertyObstacleAssessmentArea.Slope:
                    return DataTypeValidationInfo.ValSlopeType(out min, out max, out pattern);
                case PropertyObstacleAssessmentArea.AssessedAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyObstacleAssessmentArea.SlopeLowerAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyObstacleAssessmentArea.GradientLowHigh:
                    return DataTypeValidationInfo.ValSlopeType(out min, out max, out pattern);
                case PropertyObstacleAssessmentArea.SafetyRegulation:
                    return DataTypeValidationInfo.TextNameType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool ObstructionInfo(PropertyObstruction propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyObstruction.RequiredClearance:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyObstruction.MinimumAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyObstruction.SlopePenetration:
                    return DataTypeValidationInfo.ValAngleType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool AltitudeAdjustmentInfo(PropertyAltitudeAdjustment propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAltitudeAdjustment.AltitudeAdjustmentP:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool ObstaclePlacementInfo(PropertyObstaclePlacement propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyObstaclePlacement.ObstacleBearing:
                    return DataTypeValidationInfo.ValBearingType(out min, out max, out pattern);
                case PropertyObstaclePlacement.ObstacleDistance:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyObstaclePlacement.PointType:
                    return DataTypeValidationInfo.TextNameType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool StandardLevelInfo(PropertyStandardLevel propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyStandardLevel.VerticalDistance:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool ContactInformationInfo(PropertyContactInformation propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyContactInformation.Name:
                    return DataTypeValidationInfo.TextNameType(out min, out max, out pattern);
                case PropertyContactInformation.Title:
                    return DataTypeValidationInfo.TextNameType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool OnlineContactInfo(PropertyOnlineContact propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyOnlineContact.Linkage:
                    return DataTypeValidationInfo.TextAddressType(out min, out max, out pattern);
                case PropertyOnlineContact.Protocol:
                    return DataTypeValidationInfo.TextNameType(out min, out max, out pattern);
                case PropertyOnlineContact.eMail:
                    return DataTypeValidationInfo.TextAddressType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool PostalAddressInfo(PropertyPostalAddress propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyPostalAddress.DeliveryPoint:
                    return DataTypeValidationInfo.TextAddressType(out min, out max, out pattern);
                case PropertyPostalAddress.City:
                    return DataTypeValidationInfo.TextNameType(out min, out max, out pattern);
                case PropertyPostalAddress.AdministrativeArea:
                    return DataTypeValidationInfo.TextNameType(out min, out max, out pattern);
                case PropertyPostalAddress.PostalCode:
                    return DataTypeValidationInfo.TextNameType(out min, out max, out pattern);
                case PropertyPostalAddress.Country:
                    return DataTypeValidationInfo.TextNameType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool TelephoneContactInfo(PropertyTelephoneContact propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyTelephoneContact.Voice:
                    return DataTypeValidationInfo.TextPhoneType(out min, out max, out pattern);
                case PropertyTelephoneContact.Facsimile:
                    return DataTypeValidationInfo.TextPhoneType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool AircraftCharacteristicInfo(PropertyAircraftCharacteristic propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAircraftCharacteristic.TypeAircraftICAO:
                    return DataTypeValidationInfo.CodeAircraftICAOType(out min, out max, out pattern);
                case PropertyAircraftCharacteristic.WingSpan:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyAircraftCharacteristic.Weight:
                    return DataTypeValidationInfo.ValWeightType(out min, out max, out pattern);
                case PropertyAircraftCharacteristic.Passengers:
                    return DataTypeValidationInfo.NoNumberType(out min, out max, out pattern);
                case PropertyAircraftCharacteristic.Speed:
                    return DataTypeValidationInfo.ValSpeedType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool AirspaceLayerInfo(PropertyAirspaceLayer propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAirspaceLayer.UpperLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyAirspaceLayer.LowerLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool CircleSectorInfo(PropertyCircleSector propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyCircleSector.FromAngle:
                    return DataTypeValidationInfo.ValBearingType(out min, out max, out pattern);
                case PropertyCircleSector.ToAngle:
                    return DataTypeValidationInfo.ValBearingType(out min, out max, out pattern);
                case PropertyCircleSector.InnerDistance:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyCircleSector.OuterDistance:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyCircleSector.UpperLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyCircleSector.LowerLimit:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool TimesheetInfo(PropertyTimesheet propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyTimesheet.StartDate:
                    return DataTypeValidationInfo.DateMonthDayType(out min, out max, out pattern);
                case PropertyTimesheet.EndDate:
                    return DataTypeValidationInfo.DateMonthDayType(out min, out max, out pattern);
                case PropertyTimesheet.StartTime:
                    return DataTypeValidationInfo.TimeType(out min, out max, out pattern);
                case PropertyTimesheet.EndTime:
                    return DataTypeValidationInfo.TimeType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool MeteorologyInfo(PropertyMeteorology propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyMeteorology.Visibility:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyMeteorology.RunwayVisualRange:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool CallsignDetailInfo(PropertyCallsignDetail propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyCallsignDetail.CallSign:
                    return DataTypeValidationInfo.TextNameType(out min, out max, out pattern);
                case PropertyCallsignDetail.Language:
                    return DataTypeValidationInfo.CodeLanguageType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool TerminalSegmentPointInfo(PropertyTerminalSegmentPoint propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyTerminalSegmentPoint.LeadRadial:
                    return DataTypeValidationInfo.ValBearingType(out min, out max, out pattern);
                case PropertyTerminalSegmentPoint.LeadDME:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool EnRouteSegmentPointInfo(PropertyEnRouteSegmentPoint propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyEnRouteSegmentPoint.TurnRadius:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool NavaidComponentInfo(PropertyNavaidComponent propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyNavaidComponent.CollocationGroup:
                    return DataTypeValidationInfo.NoSequenceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool NoteInfo(PropertyNote propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyNote.PropertyName:
                    return DataTypeValidationInfo.TextPropertyNameType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool LinguisticNoteInfo(PropertyLinguisticNote propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyLinguisticNote.Note:
                    return DataTypeValidationInfo.TextNoteType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool VerticalStructurePartInfo(PropertyVerticalStructurePart propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyVerticalStructurePart.VerticalExtent:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyVerticalStructurePart.VerticalExtentAccuracy:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyVerticalStructurePart.Designator:
                    return DataTypeValidationInfo.TextDesignatorType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool MinimaInfo(PropertyMinima propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyMinima.Altitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyMinima.Height:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyMinima.MilitaryHeight:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyMinima.RadioHeight:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyMinima.Visibility:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyMinima.MilitaryVisibility:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool EquipmentUnavailableAdjustmentColumnInfo(PropertyEquipmentUnavailableAdjustmentColumn propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyEquipmentUnavailableAdjustmentColumn.VisibilityAdjustment:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool FASDataBlockInfo(PropertyFASDataBlock propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyFASDataBlock.HorizontalAlarmLimit:
                    return DataTypeValidationInfo.ValAlarmLimitType(out min, out max, out pattern);
                case PropertyFASDataBlock.VerticalAlarmLimit:
                    return DataTypeValidationInfo.ValAlarmLimitType(out min, out max, out pattern);
                case PropertyFASDataBlock.ThresholdCourseWidth:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyFASDataBlock.LengthOffset:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyFASDataBlock.CRCRemainder:
                    return DataTypeValidationInfo.ValHexType(out min, out max, out pattern);
                case PropertyFASDataBlock.OperationType:
                    return DataTypeValidationInfo.NoSequenceType(out min, out max, out pattern);
                case PropertyFASDataBlock.ServiceProviderSBAS:
                    return DataTypeValidationInfo.NoSequenceType(out min, out max, out pattern);
                case PropertyFASDataBlock.ApproachPerformanceDesignator:
                    return DataTypeValidationInfo.NoSequenceType(out min, out max, out pattern);
                case PropertyFASDataBlock.ReferencePathDataSelector:
                    return DataTypeValidationInfo.NoSequenceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool ApproachAltitudeTableInfo(PropertyApproachAltitudeTable propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyApproachAltitudeTable.Altitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool ApproachDistanceTableInfo(PropertyApproachDistanceTable propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyApproachDistanceTable.ValueHAT:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyApproachDistanceTable.Distance:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool ApproachTimingTableInfo(PropertyApproachTimingTable propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyApproachTimingTable.Speed:
                    return DataTypeValidationInfo.ValSpeedType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool MissedApproachGroupInfo(PropertyMissedApproachGroup propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyMissedApproachGroup.Instruction:
                    return DataTypeValidationInfo.TextInstructionType(out min, out max, out pattern);
                case PropertyMissedApproachGroup.AlternateClimbInstruction:
                    return DataTypeValidationInfo.TextInstructionType(out min, out max, out pattern);
                case PropertyMissedApproachGroup.AlternateClimbAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool ApproachConditionInfo(PropertyApproachCondition propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyApproachCondition.RequiredNavigationPerformance:
                    return DataTypeValidationInfo.CodeRNPType(out min, out max, out pattern);
                case PropertyApproachCondition.ClimbGradient:
                    return DataTypeValidationInfo.ValSlopeType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool DepartureArrivalConditionInfo(PropertyDepartureArrivalCondition propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyDepartureArrivalCondition.MinimumEnrouteAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyDepartureArrivalCondition.MinimumCrossingAtEnd:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyDepartureArrivalCondition.MaximumCrossingAtEnd:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool SectorDesignInfo(PropertySectorDesign propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertySectorDesign.DesignGradient:
                    return DataTypeValidationInfo.ValSlopeType(out min, out max, out pattern);
                case PropertySectorDesign.TerminationAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool ProcedureTransitionInfo(PropertyProcedureTransition propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyProcedureTransition.TransitionId:
                    return DataTypeValidationInfo.CodeDesignatedPointDesignatorType(out min, out max, out pattern);
                case PropertyProcedureTransition.Instruction:
                    return DataTypeValidationInfo.TextInstructionType(out min, out max, out pattern);
                case PropertyProcedureTransition.VectorHeading:
                    return DataTypeValidationInfo.ValBearingType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool HoldingUseInfo(PropertyHoldingUse propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyHoldingUse.Instruction:
                    return DataTypeValidationInfo.TextInstructionType(out min, out max, out pattern);
                case PropertyHoldingUse.InstructedAltitude:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool ProcedureTransitionLegInfo(PropertyProcedureTransitionLeg propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyProcedureTransitionLeg.SeqNumberARINC:
                    return DataTypeValidationInfo.NoSequenceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool SafeAltitudeAreaSectorInfo(PropertySafeAltitudeAreaSector propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertySafeAltitudeAreaSector.BufferWidth:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool HoldingPatternDistanceInfo(PropertyHoldingPatternDistance propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyHoldingPatternDistance.Length:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool FlightRestrictionLevelInfo(PropertyFlightRestrictionLevel propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyFlightRestrictionLevel.UpperLevel:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
                case PropertyFlightRestrictionLevel.LowerLevel:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool FlightRoutingElementInfo(PropertyFlightRoutingElement propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyFlightRoutingElement.OrderNumber:
                    return DataTypeValidationInfo.NoSequenceType(out min, out max, out pattern);
                case PropertyFlightRoutingElement.Speed:
                    return DataTypeValidationInfo.ValSpeedType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool FlightConditionElementInfo(PropertyFlightConditionElement propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyFlightConditionElement.Index:
                    return DataTypeValidationInfo.NoSequenceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool AerialRefuellingPointInfo(PropertyAerialRefuellingPoint propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAerialRefuellingPoint.Sequence:
                    return DataTypeValidationInfo.NoSequenceType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool AerialRefuellingAnchorInfo(PropertyAerialRefuellingAnchor propIndex, out double? min, out double? max, out string pattern)
        {
            switch (propIndex)
            {
                case PropertyAerialRefuellingAnchor.OutboundCourse:
                    return DataTypeValidationInfo.ValBearingType(out min, out max, out pattern);
                case PropertyAerialRefuellingAnchor.InboundCourse:
                    return DataTypeValidationInfo.ValBearingType(out min, out max, out pattern);
                case PropertyAerialRefuellingAnchor.SpeedLimit:
                    return DataTypeValidationInfo.ValSpeedType(out min, out max, out pattern);
                case PropertyAerialRefuellingAnchor.LegSeparation:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyAerialRefuellingAnchor.LegLength:
                    return DataTypeValidationInfo.ValDistanceType(out min, out max, out pattern);
                case PropertyAerialRefuellingAnchor.RefuellingBaseLevel:
                    return DataTypeValidationInfo.ValDistanceVerticalType(out min, out max, out pattern);
            }
            return NoInfo(out min, out max, out pattern);
        }
        public static bool GetObjectMinMax(ObjectType objectType, int propertyIndex, out double? min, out double? max, out string pattern)
        {
            switch (objectType)
            {
                case ObjectType.AixmPoint:
                    return PointInfo((PropertyAixmPoint)propertyIndex, out min, out max, out pattern);
                case ObjectType.NavaidEquipmentDistance:
                    return NavaidEquipmentDistanceInfo((PropertyNavaidEquipmentDistance)propertyIndex, out min, out max, out pattern);
                case ObjectType.RunwayDeclaredDistanceValue:
                    return RunwayDeclaredDistanceValueInfo((PropertyRunwayDeclaredDistanceValue)propertyIndex, out min, out max, out pattern);
                case ObjectType.LightActivation:
                    return LightActivationInfo((PropertyLightActivation)propertyIndex, out min, out max, out pattern);
                case ObjectType.Ridge:
                    return RidgeInfo((PropertyRidge)propertyIndex, out min, out max, out pattern);
                case ObjectType.RunwayContamination:
                    return RunwayContaminationInfo((PropertyRunwayContamination)propertyIndex, out min, out max, out pattern);
                case ObjectType.TaxiwayContamination:
                    return TaxiwayContaminationInfo((PropertyTaxiwayContamination)propertyIndex, out min, out max, out pattern);
                case ObjectType.SurfaceContaminationLayer:
                    return SurfaceContaminationLayerInfo((PropertySurfaceContaminationLayer)propertyIndex, out min, out max, out pattern);
                case ObjectType.RunwaySectionContamination:
                    return RunwaySectionContaminationInfo((PropertyRunwaySectionContamination)propertyIndex, out min, out max, out pattern);
                case ObjectType.TouchDownLiftOffContamination:
                    return TouchDownLiftOffContaminationInfo((PropertyTouchDownLiftOffContamination)propertyIndex, out min, out max, out pattern);
                case ObjectType.ApronContamination:
                    return ApronContaminationInfo((PropertyApronContamination)propertyIndex, out min, out max, out pattern);
                case ObjectType.AircraftStandContamination:
                    return AircraftStandContaminationInfo((PropertyAircraftStandContamination)propertyIndex, out min, out max, out pattern);
                case ObjectType.AirportHeliportContamination:
                    return AirportHeliportContaminationInfo((PropertyAirportHeliportContamination)propertyIndex, out min, out max, out pattern);
                case ObjectType.SurfaceCharacteristics:
                    return SurfaceCharacteristicsInfo((PropertySurfaceCharacteristics)propertyIndex, out min, out max, out pattern);
                case ObjectType.City:
                    return CityInfo((PropertyCity)propertyIndex, out min, out max, out pattern);
                case ObjectType.AirspaceGeometryComponent:
                    return AirspaceGeometryComponentInfo((PropertyAirspaceGeometryComponent)propertyIndex, out min, out max, out pattern);
                case ObjectType.AirspaceVolume:
                    return AirspaceVolumeInfo((PropertyAirspaceVolume)propertyIndex, out min, out max, out pattern);
                case ObjectType.Curve:
                    return CurveInfo((PropertyCurve)propertyIndex, out min, out max, out pattern);
                case ObjectType.ElevatedCurve:
                    return ElevatedCurveInfo((PropertyElevatedCurve)propertyIndex, out min, out max, out pattern);
                case ObjectType.ElevatedPoint:
                    return ElevatedPointInfo((PropertyElevatedPoint)propertyIndex, out min, out max, out pattern);
                case ObjectType.ElevatedSurface:
                    return ElevatedSurfaceInfo((PropertyElevatedSurface)propertyIndex, out min, out max, out pattern);
                case ObjectType.Surface:
                    return SurfaceInfo((PropertySurface)propertyIndex, out min, out max, out pattern);
                case ObjectType.RadarComponent:
                    return RadarComponentInfo((PropertyRadarComponent)propertyIndex, out min, out max, out pattern);
                case ObjectType.ObstacleAssessmentArea:
                    return ObstacleAssessmentAreaInfo((PropertyObstacleAssessmentArea)propertyIndex, out min, out max, out pattern);
                case ObjectType.Obstruction:
                    return ObstructionInfo((PropertyObstruction)propertyIndex, out min, out max, out pattern);
                case ObjectType.AltitudeAdjustment:
                    return AltitudeAdjustmentInfo((PropertyAltitudeAdjustment)propertyIndex, out min, out max, out pattern);
                case ObjectType.ObstaclePlacement:
                    return ObstaclePlacementInfo((PropertyObstaclePlacement)propertyIndex, out min, out max, out pattern);
                case ObjectType.StandardLevel:
                    return StandardLevelInfo((PropertyStandardLevel)propertyIndex, out min, out max, out pattern);
                case ObjectType.ContactInformation:
                    return ContactInformationInfo((PropertyContactInformation)propertyIndex, out min, out max, out pattern);
                case ObjectType.OnlineContact:
                    return OnlineContactInfo((PropertyOnlineContact)propertyIndex, out min, out max, out pattern);
                case ObjectType.PostalAddress:
                    return PostalAddressInfo((PropertyPostalAddress)propertyIndex, out min, out max, out pattern);
                case ObjectType.TelephoneContact:
                    return TelephoneContactInfo((PropertyTelephoneContact)propertyIndex, out min, out max, out pattern);
                case ObjectType.AircraftCharacteristic:
                    return AircraftCharacteristicInfo((PropertyAircraftCharacteristic)propertyIndex, out min, out max, out pattern);
                case ObjectType.AirspaceLayer:
                    return AirspaceLayerInfo((PropertyAirspaceLayer)propertyIndex, out min, out max, out pattern);
                case ObjectType.CircleSector:
                    return CircleSectorInfo((PropertyCircleSector)propertyIndex, out min, out max, out pattern);
                case ObjectType.Timesheet:
                    return TimesheetInfo((PropertyTimesheet)propertyIndex, out min, out max, out pattern);
                case ObjectType.Meteorology:
                    return MeteorologyInfo((PropertyMeteorology)propertyIndex, out min, out max, out pattern);
                case ObjectType.CallsignDetail:
                    return CallsignDetailInfo((PropertyCallsignDetail)propertyIndex, out min, out max, out pattern);
                case ObjectType.TerminalSegmentPoint:
                    return TerminalSegmentPointInfo((PropertyTerminalSegmentPoint)propertyIndex, out min, out max, out pattern);
                case ObjectType.EnRouteSegmentPoint:
                    return EnRouteSegmentPointInfo((PropertyEnRouteSegmentPoint)propertyIndex, out min, out max, out pattern);
                case ObjectType.NavaidComponent:
                    return NavaidComponentInfo((PropertyNavaidComponent)propertyIndex, out min, out max, out pattern);
                case ObjectType.Note:
                    return NoteInfo((PropertyNote)propertyIndex, out min, out max, out pattern);
                case ObjectType.LinguisticNote:
                    return LinguisticNoteInfo((PropertyLinguisticNote)propertyIndex, out min, out max, out pattern);
                case ObjectType.VerticalStructurePart:
                    return VerticalStructurePartInfo((PropertyVerticalStructurePart)propertyIndex, out min, out max, out pattern);
                case ObjectType.Minima:
                    return MinimaInfo((PropertyMinima)propertyIndex, out min, out max, out pattern);
                case ObjectType.EquipmentUnavailableAdjustmentColumn:
                    return EquipmentUnavailableAdjustmentColumnInfo((PropertyEquipmentUnavailableAdjustmentColumn)propertyIndex, out min, out max, out pattern);
                case ObjectType.FASDataBlock:
                    return FASDataBlockInfo((PropertyFASDataBlock)propertyIndex, out min, out max, out pattern);
                case ObjectType.ApproachAltitudeTable:
                    return ApproachAltitudeTableInfo((PropertyApproachAltitudeTable)propertyIndex, out min, out max, out pattern);
                case ObjectType.ApproachDistanceTable:
                    return ApproachDistanceTableInfo((PropertyApproachDistanceTable)propertyIndex, out min, out max, out pattern);
                case ObjectType.ApproachTimingTable:
                    return ApproachTimingTableInfo((PropertyApproachTimingTable)propertyIndex, out min, out max, out pattern);
                case ObjectType.MissedApproachGroup:
                    return MissedApproachGroupInfo((PropertyMissedApproachGroup)propertyIndex, out min, out max, out pattern);
                case ObjectType.ApproachCondition:
                    return ApproachConditionInfo((PropertyApproachCondition)propertyIndex, out min, out max, out pattern);
                case ObjectType.DepartureArrivalCondition:
                    return DepartureArrivalConditionInfo((PropertyDepartureArrivalCondition)propertyIndex, out min, out max, out pattern);
                case ObjectType.SectorDesign:
                    return SectorDesignInfo((PropertySectorDesign)propertyIndex, out min, out max, out pattern);
                case ObjectType.ProcedureTransition:
                    return ProcedureTransitionInfo((PropertyProcedureTransition)propertyIndex, out min, out max, out pattern);
                case ObjectType.HoldingUse:
                    return HoldingUseInfo((PropertyHoldingUse)propertyIndex, out min, out max, out pattern);
                case ObjectType.ProcedureTransitionLeg:
                    return ProcedureTransitionLegInfo((PropertyProcedureTransitionLeg)propertyIndex, out min, out max, out pattern);
                case ObjectType.SafeAltitudeAreaSector:
                    return SafeAltitudeAreaSectorInfo((PropertySafeAltitudeAreaSector)propertyIndex, out min, out max, out pattern);
                case ObjectType.HoldingPatternDistance:
                    return HoldingPatternDistanceInfo((PropertyHoldingPatternDistance)propertyIndex, out min, out max, out pattern);
                case ObjectType.FlightRestrictionLevel:
                    return FlightRestrictionLevelInfo((PropertyFlightRestrictionLevel)propertyIndex, out min, out max, out pattern);
                case ObjectType.FlightRoutingElement:
                    return FlightRoutingElementInfo((PropertyFlightRoutingElement)propertyIndex, out min, out max, out pattern);
                case ObjectType.FlightConditionElement:
                    return FlightConditionElementInfo((PropertyFlightConditionElement)propertyIndex, out min, out max, out pattern);
                case ObjectType.AerialRefuellingPoint:
                    return AerialRefuellingPointInfo((PropertyAerialRefuellingPoint)propertyIndex, out min, out max, out pattern);
                case ObjectType.AerialRefuellingAnchor:
                    return AerialRefuellingAnchorInfo((PropertyAerialRefuellingAnchor)propertyIndex, out min, out max, out pattern);
            }
            min = null;
            max = null;
            pattern = null;
            return false;
        }
    }
}
