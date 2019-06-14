using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;

namespace Aran.Aim.Validation
{
    public static class FeatureObjectValidator
    {
        public static bool IsValid (RunwayProtectArea value, ref string errorText)
        {
            bool isValid = IsValid ((AirportHeliportProtectionArea) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (RunwayDirection value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.TrueBearing);
            if (!isValid)
            {
                errorText = "TrueBearing";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.TrueBearingAccuracy);
            if (!isValid)
            {
                errorText = "TrueBearingAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.MagneticBearing);
            if (!isValid)
            {
                errorText = "MagneticBearing";
                return false;
            }
            isValid = DataTypeValidator.ValSlopeType (value.SlopeTDZ);
            if (!isValid)
            {
                errorText = "SlopeTDZ";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.ElevationTDZ);
            if (!isValid)
            {
                errorText = "ElevationTDZ";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.ElevationTDZAccuracy);
            if (!isValid)
            {
                errorText = "ElevationTDZAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid (RunwayCentrelinePoint value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            return true;
        }

        public static bool IsValid (Runway value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.NominalLength);
            if (!isValid)
            {
                errorText = "NominalLength";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.LengthAccuracy);
            if (!isValid)
            {
                errorText = "LengthAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.NominalWidth);
            if (!isValid)
            {
                errorText = "NominalWidth";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.WidthAccuracy);
            if (!isValid)
            {
                errorText = "WidthAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.WidthShoulder);
            if (!isValid)
            {
                errorText = "WidthShoulder";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.LengthStrip);
            if (!isValid)
            {
                errorText = "LengthStrip";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.WidthStrip);
            if (!isValid)
            {
                errorText = "WidthStrip";
                return false;
            }
            return true;
        }

        public static bool IsValid (ArrestingGear value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Location);
            if (!isValid)
            {
                errorText = "Location";
                return false;
            }
            return true;
        }

        public static bool IsValid (RunwayElement value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            return true;
        }

        public static bool IsValid (VisualGlideSlopeIndicator value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoNumberType (value.NumberBox);
            if (!isValid)
            {
                errorText = "NumberBox";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.SlopeAngle);
            if (!isValid)
            {
                errorText = "SlopeAngle";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MinimumEyeHeightOverThreshold);
            if (!isValid)
            {
                errorText = "MinimumEyeHeightOverThreshold";
                return false;
            }
            return true;
        }

        public static bool IsValid (RunwayBlastPad value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            return true;
        }

        public static bool IsValid (NavaidEquipmentDistance value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Distance);
            if (!isValid)
            {
                errorText = "Distance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.DistanceAccuracy);
            if (!isValid)
            {
                errorText = "DistanceAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid (RunwayDeclaredDistanceValue value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Distance);
            if (!isValid)
            {
                errorText = "Distance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.DistanceAccuracy);
            if (!isValid)
            {
                errorText = "DistanceAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid (Taxiway value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.WidthShoulder);
            if (!isValid)
            {
                errorText = "WidthShoulder";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            return true;
        }

        public static bool IsValid (TaxiwayElement value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            return true;
        }

        public static bool IsValid (GuidanceLine value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.ValSpeedType (value.MaxSpeed);
            if (!isValid)
            {
                errorText = "MaxSpeed";
                return false;
            }
            return true;
        }

        public static bool IsValid (Apron value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            return true;
        }

        public static bool IsValid (ApronElement value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            return true;
        }

        public static bool IsValid (AircraftStand value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            return true;
        }

        public static bool IsValid (Road value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }

            isValid = IsValid (value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                errorText = "SurfaceProperties";
                return false;
            }

            return true;
        }

        public static bool IsValid (TouchDownLiftOffSafeArea value, ref string errorText)
        {
            bool isValid = IsValid ((AirportHeliportProtectionArea) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (TouchDownLiftOff value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            isValid = DataTypeValidator.ValSlopeType (value.Slope);
            if (!isValid)
            {
                errorText = "Slope";
                return false;
            }
            return true;
        }

        public static bool IsValid (LightActivation value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoNumberType (value.Clicks);
            if (!isValid)
            {
                errorText = "Clicks";
                return false;
            }
            return true;
        }

        public static bool IsValid (ApproachLightingSystem value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            return true;
        }

        public static bool IsValid (SurfaceContamination value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValFrictionType (value.FrictionCoefficient);
            if (!isValid)
            {
                errorText = "FrictionCoefficient";
                return false;
            }
            isValid = DataTypeValidator.TimeType (value.FurtherClearanceTime);
            if (!isValid)
            {
                errorText = "FurtherClearanceTime";
                return false;
            }
            isValid = DataTypeValidator.ValPercentType (value.Proportion);
            if (!isValid)
            {
                errorText = "Proportion";
                return false;
            }
            return true;
        }

        public static bool IsValid (Ridge value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Distance);
            if (!isValid)
            {
                errorText = "Distance";
                return false;
            }
            return true;
        }

        public static bool IsValid (RunwayContamination value, ref string errorText)
        {
            bool isValid = IsValid ((SurfaceContamination) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.ClearedLength);
            if (!isValid)
            {
                errorText = "ClearedLength";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.ClearedWidth);
            if (!isValid)
            {
                errorText = "ClearedWidth";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.FurtherClearanceLength);
            if (!isValid)
            {
                errorText = "FurtherClearanceLength";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.FurtherClearanceWidth);
            if (!isValid)
            {
                errorText = "FurtherClearanceWidth";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.ClearedLengthBegin);
            if (!isValid)
            {
                errorText = "ClearedLengthBegin";
                return false;
            }
            return true;
        }

        public static bool IsValid (TaxiwayContamination value, ref string errorText)
        {
            bool isValid = IsValid ((SurfaceContamination) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.ClearedWidth);
            if (!isValid)
            {
                errorText = "ClearedWidth";
                return false;
            }
            return true;
        }

        public static bool IsValid (SurfaceContaminationLayer value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoSequenceType (value.LayerOrder);
            if (!isValid)
            {
                errorText = "LayerOrder";
                return false;
            }
            return true;
        }

        public static bool IsValid (RunwaySectionContamination value, ref string errorText)
        {
            bool isValid = IsValid ((SurfaceContamination) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (TouchDownLiftOffContamination value, ref string errorText)
        {
            bool isValid = IsValid ((SurfaceContamination) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (ApronContamination value, ref string errorText)
        {
            bool isValid = IsValid ((SurfaceContamination) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (AircraftStandContamination value, ref string errorText)
        {
            bool isValid = IsValid ((SurfaceContamination) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (AirportHeliportContamination value, ref string errorText)
        {
            bool isValid = IsValid ((SurfaceContamination) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (SurveyControlPoint value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            return true;
        }

        public static bool IsValid (SurfaceCharacteristics value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid = DataTypeValidator.ValPCNType (value.ClassPCN);
            if (!isValid)
            {
                errorText = "ClassPCN";
                return false;
            }
            isValid = DataTypeValidator.ValWeightType (value.WeightSIWL);
            if (!isValid)
            {
                errorText = "WeightSIWL";
                return false;
            }
            isValid = DataTypeValidator.ValWeightType (value.WeightAUW);
            if (!isValid)
            {
                errorText = "WeightAUW";
                return false;
            }
            return true;
        }

        public static bool IsValid (City value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            return true;
        }

        public static bool IsValid (AirportHeliportProtectionArea value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            return true;
        }

        public static bool IsValid (AirportHeliport value, ref string errorText)
        {
            bool isValid = DataTypeValidator.CodeAirportHeliportDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.CodeICAOType (value.LocationIndicatorICAO);
            if (!isValid)
            {
                errorText = "LocationIndicatorICAO";
                return false;
            }
            isValid = DataTypeValidator.CodeIATAType (value.DesignatorIATA);
            if (!isValid)
            {
                errorText = "DesignatorIATA";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.FieldElevation);
            if (!isValid)
            {
                errorText = "FieldElevation";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.FieldElevationAccuracy);
            if (!isValid)
            {
                errorText = "FieldElevationAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationType (value.MagneticVariation);
            if (!isValid)
            {
                errorText = "MagneticVariation";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.MagneticVariationAccuracy);
            if (!isValid)
            {
                errorText = "MagneticVariationAccuracy";
                return false;
            }
            isValid = DataTypeValidator.DateYearType (value.DateMagneticVariation);
            if (!isValid)
            {
                errorText = "DateMagneticVariation";
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationChangeType (value.MagneticVariationChange);
            if (!isValid)
            {
                errorText = "MagneticVariationChange";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.TransitionAltitude);
            if (!isValid)
            {
                errorText = "TransitionAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValFLType (value.TransitionLevel);
            if (!isValid)
            {
                errorText = "TransitionLevel";
                return false;
            }
            return true;
        }

        public static bool IsValid (AirportHotSpot value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType (value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }
            return true;
        }

        public static bool IsValid (Airspace value, ref string errorText)
        {
            bool isValid = DataTypeValidator.CodeAirspaceDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.LocalType);
            if (!isValid)
            {
                errorText = "LocalType";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.ValFLType (value.UpperLowerSeparation);
            if (!isValid)
            {
                errorText = "UpperLowerSeparation";
                return false;
            }
            return true;
        }

        public static bool IsValid (AirspaceGeometryComponent value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoSequenceType (value.OperationSequence);
            if (!isValid)
            {
                errorText = "OperationSequence";
                return false;
            }
            return true;
        }

        public static bool IsValid (AirspaceVolume value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.UpperLimit);
            if (!isValid)
            {
                errorText = "UpperLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MaximumLimit);
            if (!isValid)
            {
                errorText = "MaximumLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.LowerLimit);
            if (!isValid)
            {
                errorText = "LowerLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MinimumLimit);
            if (!isValid)
            {
                errorText = "MinimumLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            return true;
        }

        public static bool IsValid (GeoBorder value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            return true;
        }

        public static bool IsValid (Curve value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.HorizontalAccuracy);
            if (!isValid)
            {
                errorText = "HorizontalAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid (ElevatedCurve value, ref string errorText)
        {
            bool isValid = IsValid ((Curve) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.Elevation);
            if (!isValid)
            {
                errorText = "Elevation";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.VerticalAccuracy);
            if (!isValid)
            {
                errorText = "VerticalAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid (ElevatedPoint value, ref string errorText)
        {
            bool isValid = IsValid ((AixmPoint) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.Elevation);
            if (!isValid)
            {
                errorText = "Elevation";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.VerticalAccuracy);
            if (!isValid)
            {
                errorText = "VerticalAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid (ElevatedSurface value, ref string errorText)
        {
            bool isValid = IsValid ((Surface) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.Elevation);
            if (!isValid)
            {
                errorText = "Elevation";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.VerticalAccuracy);
            if (!isValid)
            {
                errorText = "VerticalAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid (AixmPoint value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.HorizontalAccuracy);
            if (!isValid)
            {
                errorText = "HorizontalAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid (Surface value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.HorizontalAccuracy);
            if (!isValid)
            {
                errorText = "HorizontalAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid (PrecisionApproachRadar value, ref string errorText)
        {
            bool isValid = IsValid ((RadarEquipment) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.Slope);
            if (!isValid)
            {
                errorText = "Slope";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.SlopeAccuracy);
            if (!isValid)
            {
                errorText = "SlopeAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid (PrimarySurveillanceRadar value, ref string errorText)
        {
            bool isValid = IsValid ((SurveillanceRadar) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (RadarComponent value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoSequenceType (value.CollocationGroup);
            if (!isValid)
            {
                errorText = "CollocationGroup";
                return false;
            }
            return true;
        }

        public static bool IsValid (RadarEquipment value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.TextDesignatorType (value.SerialNumber);
            if (!isValid)
            {
                errorText = "SerialNumber";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Range);
            if (!isValid)
            {
                errorText = "Range";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.RangeAccuracy);
            if (!isValid)
            {
                errorText = "RangeAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationType (value.MagneticVariation);
            if (!isValid)
            {
                errorText = "MagneticVariation";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.MagneticVariationAccuracy);
            if (!isValid)
            {
                errorText = "MagneticVariationAccuracy";
                return false;
            }
            isValid = DataTypeValidator.DateYearType (value.DateMagneticVariation);
            if (!isValid)
            {
                errorText = "DateMagneticVariation";
                return false;
            }
            return true;
        }

        public static bool IsValid (RadarSystem value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Model);
            if (!isValid)
            {
                errorText = "Model";
                return false;
            }
            isValid = DataTypeValidator.TextDesignatorType (value.BroadcastIdentifier);
            if (!isValid)
            {
                errorText = "BroadcastIdentifier";
                return false;
            }
            return true;
        }

        public static bool IsValid (SecondarySurveillanceRadar value, ref string errorText)
        {
            bool isValid = IsValid ((SurveillanceRadar) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (SurveillanceRadar value, ref string errorText)
        {
            bool isValid = IsValid ((RadarEquipment) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.VerticalCoverageAltitude);
            if (!isValid)
            {
                errorText = "VerticalCoverageAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.VerticalCoverageDistance);
            if (!isValid)
            {
                errorText = "VerticalCoverageDistance";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.VerticalCoverageAzimuth);
            if (!isValid)
            {
                errorText = "VerticalCoverageAzimuth";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.TiltAngle);
            if (!isValid)
            {
                errorText = "TiltAngle";
                return false;
            }
            isValid = DataTypeValidator.TextDesignatorType (value.AutomatedRadarTerminalSystem);
            if (!isValid)
            {
                errorText = "AutomatedRadarTerminalSystem";
                return false;
            }
            return true;
        }

        public static bool IsValid (ObstacleAssessmentArea value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoNumberType (value.SectionNumber);
            if (!isValid)
            {
                errorText = "SectionNumber";
                return false;
            }
            isValid = DataTypeValidator.ValSlopeType (value.Slope);
            if (!isValid)
            {
                errorText = "Slope";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.AssessedAltitude);
            if (!isValid)
            {
                errorText = "AssessedAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.SlopeLowerAltitude);
            if (!isValid)
            {
                errorText = "SlopeLowerAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValSlopeType (value.GradientLowHigh);
            if (!isValid)
            {
                errorText = "GradientLowHigh";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.SafetyRegulation);
            if (!isValid)
            {
                errorText = "SafetyRegulation";
                return false;
            }
            return true;
        }

        public static bool IsValid (Obstruction value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.RequiredClearance);
            if (!isValid)
            {
                errorText = "RequiredClearance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MinimumAltitude);
            if (!isValid)
            {
                errorText = "MinimumAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.SlopePenetration);
            if (!isValid)
            {
                errorText = "SlopePenetration";
                return false;
            }
            return true;
        }

        public static bool IsValid (AltitudeAdjustment value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.AltitudeAdjustmentP);
            if (!isValid)
            {
                errorText = "AltitudeAdjustment";
                return false;
            }
            return true;
        }

        public static bool IsValid (HoldingAssessment value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.UpperLimit);
            if (!isValid)
            {
                errorText = "UpperLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.LowerLimit);
            if (!isValid)
            {
                errorText = "LowerLimit";
                return false;
            }
            isValid = DataTypeValidator.ValSpeedType (value.SpeedLimit);
            if (!isValid)
            {
                errorText = "SpeedLimit";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.PatternTemplate);
            if (!isValid)
            {
                errorText = "PatternTemplate";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.LegLengthToward);
            if (!isValid)
            {
                errorText = "LegLengthToward";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.LegLengthAway);
            if (!isValid)
            {
                errorText = "LegLengthAway";
                return false;
            }
            return true;
        }

        public static bool IsValid (ObstaclePlacement value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValBearingType (value.ObstacleBearing);
            if (!isValid)
            {
                errorText = "ObstacleBearing";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.ObstacleDistance);
            if (!isValid)
            {
                errorText = "ObstacleDistance";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.PointType);
            if (!isValid)
            {
                errorText = "PointType";
                return false;
            }
            return true;
        }

        public static bool IsValid (StandardLevelSector value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValBearingType (value.FromTrack);
            if (!isValid)
            {
                errorText = "FromTrack";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.ToTrack);
            if (!isValid)
            {
                errorText = "ToTrack";
                return false;
            }
            return true;
        }

        public static bool IsValid (StandardLevel value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.VerticalDistance);
            if (!isValid)
            {
                errorText = "VerticalDistance";
                return false;
            }
            return true;
        }

        public static bool IsValid (RadioFrequencyArea value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValAngleType (value.AngleScallop);
            if (!isValid)
            {
                errorText = "AngleScallop";
                return false;
            }
            return true;
        }

        public static bool IsValid (ContactInformation value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.Title);
            if (!isValid)
            {
                errorText = "Title";
                return false;
            }
            return true;
        }

        public static bool IsValid (OnlineContact value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextAddressType (value.Linkage);
            if (!isValid)
            {
                errorText = "Linkage";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.Protocol);
            if (!isValid)
            {
                errorText = "Protocol";
                return false;
            }
            isValid = DataTypeValidator.TextAddressType (value.eMail);
            if (!isValid)
            {
                errorText = "eMail";
                return false;
            }
            return true;
        }

        public static bool IsValid (PostalAddress value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextAddressType (value.DeliveryPoint);
            if (!isValid)
            {
                errorText = "DeliveryPoint";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.City);
            if (!isValid)
            {
                errorText = "City";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.AdministrativeArea);
            if (!isValid)
            {
                errorText = "AdministrativeArea";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.PostalCode);
            if (!isValid)
            {
                errorText = "PostalCode";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.Country);
            if (!isValid)
            {
                errorText = "Country";
                return false;
            }
            return true;
        }

        public static bool IsValid (TelephoneContact value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextPhoneType (value.Voice);
            if (!isValid)
            {
                errorText = "Voice";
                return false;
            }
            isValid = DataTypeValidator.TextPhoneType (value.Facsimile);
            if (!isValid)
            {
                errorText = "Facsimile";
                return false;
            }
            return true;
        }

        public static bool IsValid (AircraftCharacteristic value, ref string errorText)
        {
            bool isValid = DataTypeValidator.CodeAircraftICAOType (value.TypeAircraftICAO);
            if (!isValid)
            {
                errorText = "TypeAircraftICAO";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.WingSpan);
            if (!isValid)
            {
                errorText = "WingSpan";
                return false;
            }
            isValid = DataTypeValidator.ValWeightType (value.Weight);
            if (!isValid)
            {
                errorText = "Weight";
                return false;
            }
            isValid = DataTypeValidator.NoNumberType (value.Passengers);
            if (!isValid)
            {
                errorText = "Passengers";
                return false;
            }
            isValid = DataTypeValidator.ValSpeedType (value.Speed);
            if (!isValid)
            {
                errorText = "Speed";
                return false;
            }
            return true;
        }

        public static bool IsValid (AirspaceLayer value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.UpperLimit);
            if (!isValid)
            {
                errorText = "UpperLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.LowerLimit);
            if (!isValid)
            {
                errorText = "LowerLimit";
                return false;
            }
            return true;
        }

        public static bool IsValid (CircleSector value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValBearingType (value.FromAngle);
            if (!isValid)
            {
                errorText = "FromAngle";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.ToAngle);
            if (!isValid)
            {
                errorText = "ToAngle";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.InnerDistance);
            if (!isValid)
            {
                errorText = "InnerDistance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.OuterDistance);
            if (!isValid)
            {
                errorText = "OuterDistance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.UpperLimit);
            if (!isValid)
            {
                errorText = "UpperLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.LowerLimit);
            if (!isValid)
            {
                errorText = "LowerLimit";
                return false;
            }
            return true;
        }

        public static bool IsValid (Timesheet value, ref string errorText)
        {
            bool isValid = DataTypeValidator.DateMonthDayType (value.StartDate);
            if (!isValid)
            {
                errorText = "StartDate";
                return false;
            }
            isValid = DataTypeValidator.DateMonthDayType (value.EndDate);
            if (!isValid)
            {
                errorText = "EndDate";
                return false;
            }
            isValid = DataTypeValidator.TimeType (value.StartTime);
            if (!isValid)
            {
                errorText = "StartTime";
                return false;
            }
            isValid = DataTypeValidator.TimeType (value.EndTime);
            if (!isValid)
            {
                errorText = "EndTime";
                return false;
            }
            return true;
        }

        public static bool IsValid (SpecialDate value, ref string errorText)
        {
            bool isValid = DataTypeValidator.DateMonthDayType (value.DateDay);
            if (!isValid)
            {
                errorText = "DateDay";
                return false;
            }
            isValid = DataTypeValidator.DateYearType (value.DateYear);
            if (!isValid)
            {
                errorText = "DateYear";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            return true;
        }

        public static bool IsValid (Meteorology value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Visibility);
            if (!isValid)
            {
                errorText = "Visibility";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.RunwayVisualRange);
            if (!isValid)
            {
                errorText = "RunwayVisualRange";
                return false;
            }
            return true;
        }

        public static bool IsValid (Service value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            return true;
        }

        public static bool IsValid (RadioCommunicationChannel value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextDesignatorType (value.Logon);
            if (!isValid)
            {
                errorText = "Logon";
                return false;
            }
            return true;
        }

        public static bool IsValid (CallsignDetail value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.CallSign);
            if (!isValid)
            {
                errorText = "CallSign";
                return false;
            }
            isValid = DataTypeValidator.CodeLanguageType (value.Language);
            if (!isValid)
            {
                errorText = "Language";
                return false;
            }
            return true;
        }

        public static bool IsValid (PilotControlledLighting value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoNumberType (value.IntensitySteps);
            if (!isValid)
            {
                errorText = "IntensitySteps";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType (value.ActivationInstruction);
            if (!isValid)
            {
                errorText = "ActivationInstruction";
                return false;
            }
            return true;
        }

        public static bool IsValid (InformationService value, ref string errorText)
        {
            bool isValid = IsValid ((Service) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (TrafficSeparationService value, ref string errorText)
        {
            bool isValid = IsValid ((Service) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (GroundTrafficControlService value, ref string errorText)
        {
            bool isValid = IsValid ((TrafficSeparationService) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (AirTrafficControlService value, ref string errorText)
        {
            bool isValid = IsValid ((TrafficSeparationService) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (AirTrafficManagementService value, ref string errorText)
        {
            bool isValid = IsValid ((Service) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (SearchRescueService value, ref string errorText)
        {
            bool isValid = IsValid ((Service) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (AirportGroundService value, ref string errorText)
        {
            bool isValid = IsValid ((Service) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (PassengerService value, ref string errorText)
        {
            bool isValid = IsValid ((AirportGroundService) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (AircraftGroundService value, ref string errorText)
        {
            bool isValid = IsValid ((AirportGroundService) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (FireFightingService value, ref string errorText)
        {
            bool isValid = IsValid ((AirportGroundService) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (AirportClearanceService value, ref string errorText)
        {
            bool isValid = IsValid ((AirportGroundService) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType (value.SnowPlan);
            if (!isValid)
            {
                errorText = "SnowPlan";
                return false;
            }
            return true;
        }

        public static bool IsValid (AirportSuppliesService value, ref string errorText)
        {
            bool isValid = IsValid ((AirportGroundService) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (TerminalSegmentPoint value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValBearingType (value.LeadRadial);
            if (!isValid)
            {
                errorText = "LeadRadial";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.LeadDME);
            if (!isValid)
            {
                errorText = "LeadDME";
                return false;
            }
            return true;
        }

        public static bool IsValid (EnRouteSegmentPoint value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.TurnRadius);
            if (!isValid)
            {
                errorText = "TurnRadius";
                return false;
            }
            return true;
        }

        public static bool IsValid (AngleIndication value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValBearingType (value.Angle);
            if (!isValid)
            {
                errorText = "Angle";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.TrueAngle);
            if (!isValid)
            {
                errorText = "TrueAngle";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MinimumReceptionAltitude);
            if (!isValid)
            {
                errorText = "MinimumReceptionAltitude";
                return false;
            }
            return true;
        }

        public static bool IsValid (DistanceIndication value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Distance);
            if (!isValid)
            {
                errorText = "Distance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MinimumReceptionAltitude);
            if (!isValid)
            {
                errorText = "MinimumReceptionAltitude";
                return false;
            }
            return true;
        }

        public static bool IsValid (Azimuth value, ref string errorText)
        {
            bool isValid = IsValid ((NavaidEquipment) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.TrueBearing);
            if (!isValid)
            {
                errorText = "TrueBearing";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.TrueBearingAccuracy);
            if (!isValid)
            {
                errorText = "TrueBearingAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.MagneticBearing);
            if (!isValid)
            {
                errorText = "MagneticBearing";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.AngleProportionalLeft);
            if (!isValid)
            {
                errorText = "AngleProportionalLeft";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.AngleProportionalRight);
            if (!isValid)
            {
                errorText = "AngleProportionalRight";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.AngleCoverLeft);
            if (!isValid)
            {
                errorText = "AngleCoverLeft";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.AngleCoverRight);
            if (!isValid)
            {
                errorText = "AngleCoverRight";
                return false;
            }
            return true;
        }

        public static bool IsValid (CheckpointINS value, ref string errorText)
        {
            bool isValid = IsValid ((NavigationSystemCheckpoint) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (CheckpointVOR value, ref string errorText)
        {
            bool isValid = IsValid ((NavigationSystemCheckpoint) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (DME value, ref string errorText)
        {
            bool isValid = IsValid ((NavaidEquipment) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Displace);
            if (!isValid)
            {
                errorText = "Displace";
                return false;
            }
            return true;
        }

        public static bool IsValid (Elevation value, ref string errorText)
        {
            bool isValid = IsValid ((NavaidEquipment) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.AngleNominal);
            if (!isValid)
            {
                errorText = "AngleNominal";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.AngleMinimum);
            if (!isValid)
            {
                errorText = "AngleMinimum";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.AngleSpan);
            if (!isValid)
            {
                errorText = "AngleSpan";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.AngleAccuracy);
            if (!isValid)
            {
                errorText = "AngleAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid (Glidepath value, ref string errorText)
        {
            bool isValid = IsValid ((NavaidEquipment) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.Slope);
            if (!isValid)
            {
                errorText = "Slope";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.AngleAccuracy);
            if (!isValid)
            {
                errorText = "AngleAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.Rdh);
            if (!isValid)
            {
                errorText = "Rdh";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.RdhAccuracy);
            if (!isValid)
            {
                errorText = "RdhAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid (Localizer value, ref string errorText)
        {
            bool isValid = IsValid ((NavaidEquipment) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.MagneticBearing);
            if (!isValid)
            {
                errorText = "MagneticBearing";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.MagneticBearingAccuracy);
            if (!isValid)
            {
                errorText = "MagneticBearingAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.TrueBearing);
            if (!isValid)
            {
                errorText = "TrueBearing";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.TrueBearingAccuracy);
            if (!isValid)
            {
                errorText = "TrueBearingAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationType (value.Declination);
            if (!isValid)
            {
                errorText = "Declination";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.WidthCourse);
            if (!isValid)
            {
                errorText = "WidthCourse";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.WidthCourseAccuracy);
            if (!isValid)
            {
                errorText = "WidthCourseAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid (MarkerBeacon value, ref string errorText)
        {
            bool isValid = IsValid ((NavaidEquipment) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.AxisBearing);
            if (!isValid)
            {
                errorText = "AxisBearing";
                return false;
            }
            isValid = DataTypeValidator.CodeAuralMorseType (value.AuralMorseCode);
            if (!isValid)
            {
                errorText = "AuralMorseCode ";
                return false;
            }
            return true;
        }

        public static bool IsValid (Navaid value, ref string errorText)
        {
            bool isValid = DataTypeValidator.CodeNavaidDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            return true;
        }

        public static bool IsValid (NavaidComponent value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoSequenceType (value.CollocationGroup);
            if (!isValid)
            {
                errorText = "CollocationGroup";
                return false;
            }
            return true;
        }

        public static bool IsValid (NavaidEquipment value, ref string errorText)
        {
            bool isValid = DataTypeValidator.CodeNavaidDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationType (value.MagneticVariation);
            if (!isValid)
            {
                errorText = "MagneticVariation";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.MagneticVariationAccuracy);
            if (!isValid)
            {
                errorText = "MagneticVariationAccuracy";
                return false;
            }
            isValid = DataTypeValidator.DateYearType (value.DateMagneticVariation);
            if (!isValid)
            {
                errorText = "DateMagneticVariation";
                return false;
            }
            return true;
        }

        public static bool IsValid (NavigationSystemCheckpoint value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.UpperLimit);
            if (!isValid)
            {
                errorText = "UpperLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.LowerLimit);
            if (!isValid)
            {
                errorText = "LowerLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Distance);
            if (!isValid)
            {
                errorText = "Distance";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.Angle);
            if (!isValid)
            {
                errorText = "Angle";
                return false;
            }
            return true;
        }

        public static bool IsValid (SDF value, ref string errorText)
        {
            bool isValid = IsValid ((NavaidEquipment) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.MagneticBearing);
            if (!isValid)
            {
                errorText = "MagneticBearing";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.TrueBearing);
            if (!isValid)
            {
                errorText = "TrueBearing";
                return false;
            }
            return true;
        }

        public static bool IsValid (NDB value, ref string errorText)
        {
            bool isValid = IsValid ((NavaidEquipment) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (SpecialNavigationStation value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            return true;
        }

        public static bool IsValid (VOR value, ref string errorText)
        {
            bool isValid = IsValid ((NavaidEquipment) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationType (value.Declination);
            if (!isValid)
            {
                errorText = "Declination";
                return false;
            }
            return true;
        }

        public static bool IsValid (TACAN value, ref string errorText)
        {
            bool isValid = IsValid ((NavaidEquipment) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationType (value.Declination);
            if (!isValid)
            {
                errorText = "Declination";
                return false;
            }
            return true;
        }

        public static bool IsValid (SpecialNavigationSystem value, ref string errorText)
        {
            bool isValid = DataTypeValidator.CodeSpecialNavigationChainDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            return true;
        }

        public static bool IsValid (DirectionFinder value, ref string errorText)
        {
            bool isValid = IsValid ((NavaidEquipment) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (DesignatedPoint value, ref string errorText)
        {
            bool isValid = DataTypeValidator.CodeDesignatedPointDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            return true;
        }

        public static bool IsValid (AeronauticalGroundLight value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            return true;
        }

        public static bool IsValid (Note value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextPropertyNameType (value.PropertyName);
            if (!isValid)
            {
                errorText = "PropertyName";
                return false;
            }
            return true;
        }

        public static bool IsValid (LinguisticNote value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNoteType (value.Note.Value);
            if (!isValid)
            {
                errorText = "Note.Value";
                return false;
            }
            return true;
        }

        public static bool IsValid (OrganisationAuthority value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.CodeOrganisationDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            return true;
        }

        public static bool IsValid (Unit value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.CodeOrganisationDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            return true;
        }

        public static bool IsValid (VerticalStructurePart value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.VerticalExtent);
            if (!isValid)
            {
                errorText = "VerticalExtent";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.VerticalExtentAccuracy);
            if (!isValid)
            {
                errorText = "VerticalExtentAccuracy";
                return false;
            }
            isValid = DataTypeValidator.TextDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            return true;
        }

        public static bool IsValid (VerticalStructure value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Radius);
            if (!isValid)
            {
                errorText = "Radius";
                return false;
            }
            return true;
        }

        public static bool IsValid (Minima value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.Altitude);
            if (!isValid)
            {
                errorText = "Altitude";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.Height);
            if (!isValid)
            {
                errorText = "Height";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MilitaryHeight);
            if (!isValid)
            {
                errorText = "MilitaryHeight";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.RadioHeight);
            if (!isValid)
            {
                errorText = "RadioHeight";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Visibility);
            if (!isValid)
            {
                errorText = "Visibility";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.MilitaryVisibility);
            if (!isValid)
            {
                errorText = "MilitaryVisibility";
                return false;
            }
            return true;
        }

        public static bool IsValid (EquipmentUnavailableAdjustmentColumn value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.VisibilityAdjustment);
            if (!isValid)
            {
                errorText = "VisibilityAdjustment";
                return false;
            }
            return true;
        }

        public static bool IsValid (TerminalArrivalArea value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.OuterBufferWidth);
            if (!isValid)
            {
                errorText = "OuterBufferWidth";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.LateralBufferWidth);
            if (!isValid)
            {
                errorText = "LateralBufferWidth";
                return false;
            }
            return true;
        }

        public static bool IsValid (FASDataBlock value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValAlarmLimitType (value.HorizontalAlarmLimit);
            if (!isValid)
            {
                errorText = "HorizontalAlarmLimit";
                return false;
            }
            isValid = DataTypeValidator.ValAlarmLimitType (value.VerticalAlarmLimit);
            if (!isValid)
            {
                errorText = "VerticalAlarmLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.ThresholdCourseWidth);
            if (!isValid)
            {
                errorText = "ThresholdCourseWidth";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.LengthOffset);
            if (!isValid)
            {
                errorText = "LengthOffset";
                return false;
            }
            isValid = DataTypeValidator.ValHexType (value.CRCRemainder);
            if (!isValid)
            {
                errorText = "CRCRemainder";
                return false;
            }
            isValid = DataTypeValidator.NoSequenceType (value.OperationType);
            if (!isValid)
            {
                errorText = "OperationType";
                return false;
            }
            isValid = DataTypeValidator.NoSequenceType (value.ServiceProviderSBAS);
            if (!isValid)
            {
                errorText = "ServiceProviderSBAS";
                return false;
            }
            isValid = DataTypeValidator.NoSequenceType (value.ApproachPerformanceDesignator);
            if (!isValid)
            {
                errorText = "ApproachPerformanceDesignator";
                return false;
            }
            isValid = DataTypeValidator.NoSequenceType (value.ReferencePathDataSelector);
            if (!isValid)
            {
                errorText = "ReferencePathDataSelector";
                return false;
            }
            return true;
        }

        public static bool IsValid (ApproachAltitudeTable value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.Altitude);
            if (!isValid)
            {
                errorText = "Altitude";
                return false;
            }
            return true;
        }

        public static bool IsValid (ApproachDistanceTable value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.ValueHAT);
            if (!isValid)
            {
                errorText = "ValueHAT";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Distance);
            if (!isValid)
            {
                errorText = "Distance";
                return false;
            }
            return true;
        }

        public static bool IsValid (ApproachTimingTable value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValSpeedType (value.Speed);
            if (!isValid)
            {
                errorText = "Speed";
                return false;
            }
            return true;
        }

        public static bool IsValid (InstrumentApproachProcedure value, ref string errorText)
        {
            bool isValid = IsValid ((Procedure) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.CopterTrack);
            if (!isValid)
            {
                errorText = "CopterTrack";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType (value.CourseReversalInstruction);
            if (!isValid)
            {
                errorText = "CourseReversalInstruction";
                return false;
            }
            return true;
        }

        public static bool IsValid (MissedApproachGroup value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextInstructionType (value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType (value.AlternateClimbInstruction);
            if (!isValid)
            {
                errorText = "AlternateClimbInstruction";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.AlternateClimbAltitude);
            if (!isValid)
            {
                errorText = "AlternateClimbAltitude";
                return false;
            }
            return true;
        }

        public static bool IsValid (ApproachCondition value, ref string errorText)
        {
            bool isValid = DataTypeValidator.CodeRNPType (value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            isValid = DataTypeValidator.ValSlopeType (value.ClimbGradient);
            if (!isValid)
            {
                errorText = "ClimbGradient";
                return false;
            }
            return true;
        }

        public static bool IsValid (StandardInstrumentDeparture value, ref string errorText)
        {
            bool isValid = IsValid ((Procedure) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.TextSIDSTARDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            return true;
        }

        public static bool IsValid (NavigationArea value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.MinimumCeiling);
            if (!isValid)
            {
                errorText = "MinimumCeiling";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.MinimumVisibility);
            if (!isValid)
            {
                errorText = "MinimumVisibility";
                return false;
            }
            return true;
        }

        public static bool IsValid (DepartureArrivalCondition value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.MinimumEnrouteAltitude);
            if (!isValid)
            {
                errorText = "MinimumEnrouteAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MinimumCrossingAtEnd);
            if (!isValid)
            {
                errorText = "MinimumCrossingAtEnd";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MaximumCrossingAtEnd);
            if (!isValid)
            {
                errorText = "MaximumCrossingAtEnd";
                return false;
            }
            return true;
        }

        public static bool IsValid (SectorDesign value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValSlopeType (value.DesignGradient);
            if (!isValid)
            {
                errorText = "DesignGradient";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.TerminationAltitude);
            if (!isValid)
            {
                errorText = "TerminationAltitude";
                return false;
            }
            return true;
        }

        public static bool IsValid (StandardInstrumentArrival value, ref string errorText)
        {
            bool isValid = IsValid ((Procedure) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.TextSIDSTARDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            return true;
        }

        public static bool IsValid (Procedure value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextInstructionType (value.CommunicationFailureInstruction);
            if (!isValid)
            {
                errorText = "CommunicationFailureInstruction";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType (value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            return true;
        }

        public static bool IsValid (SegmentLeg value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValBearingType (value.Course);
            if (!isValid)
            {
                errorText = "Course";
                return false;
            }
            isValid = DataTypeValidator.ValSpeedType (value.SpeedLimit);
            if (!isValid)
            {
                errorText = "SpeedLimit";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.BankAngle);
            if (!isValid)
            {
                errorText = "BankAngle";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.UpperLimitAltitude);
            if (!isValid)
            {
                errorText = "UpperLimitAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.LowerLimitAltitude);
            if (!isValid)
            {
                errorText = "LowerLimitAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.AltitudeOverrideATC);
            if (!isValid)
            {
                errorText = "AltitudeOverrideATC";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType (value.VerticalAngle);
            if (!isValid)
            {
                errorText = "VerticalAngle";
                return false;
            }
            return true;
        }

        public static bool IsValid (ProcedureTransition value, ref string errorText)
        {
            bool isValid = DataTypeValidator.CodeDesignatedPointDesignatorType (value.TransitionId);
            if (!isValid)
            {
                errorText = "TransitionId";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType (value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.VectorHeading);
            if (!isValid)
            {
                errorText = "VectorHeading";
                return false;
            }
            return true;
        }

        public static bool IsValid (HoldingUse value, ref string errorText)
        {
            bool isValid = DataTypeValidator.TextInstructionType (value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.InstructedAltitude);
            if (!isValid)
            {
                errorText = "InstructedAltitude";
                return false;
            }
            return true;
        }

        public static bool IsValid (ApproachLeg value, ref string errorText)
        {
            bool isValid = IsValid ((SegmentLeg) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid (ArrivalFeederLeg value, ref string errorText)
        {
            bool isValid = IsValid ((ApproachLeg) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType (value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            return true;
        }

        public static bool IsValid (ArrivalLeg value, ref string errorText)
        {
            bool isValid = IsValid ((SegmentLeg) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType (value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            return true;
        }

        public static bool IsValid (DepartureLeg value, ref string errorText)
        {
            bool isValid = IsValid ((SegmentLeg) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType (value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MinimumObstacleClearanceAltitude);
            if (!isValid)
            {
                errorText = "MinimumObstacleClearanceAltitude";
                return false;
            }
            return true;
        }

        public static bool IsValid (FinalLeg value, ref string errorText)
        {
            bool isValid = IsValid ((ApproachLeg) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.CourseOffsetAngle);
            if (!isValid)
            {
                errorText = "CourseOffsetAngle";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.CourseCentrelineDistance);
            if (!isValid)
            {
                errorText = "CourseCentrelineDistance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.CourseOffsetDistance);
            if (!isValid)
            {
                errorText = "CourseOffsetDistance";
                return false;
            }
            return true;
        }

        public static bool IsValid (InitialLeg value, ref string errorText)
        {
            bool isValid = IsValid ((ApproachLeg) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType (value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            return true;
        }

        public static bool IsValid (IntermediateLeg value, ref string errorText)
        {
            bool isValid = IsValid ((ApproachLeg) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType (value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            return true;
        }

        public static bool IsValid (MissedApproachLeg value, ref string errorText)
        {
            bool isValid = IsValid ((ApproachLeg) value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.HeightMAPT);
            if (!isValid)
            {
                errorText = "HeightMAPT";
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType (value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            return true;
        }

        public static bool IsValid (ProcedureTransitionLeg value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoSequenceType (value.SeqNumberARINC);
            if (!isValid)
            {
                errorText = "SeqNumberARINC";
                return false;
            }
            return true;
        }

        public static bool IsValid (SafeAltitudeAreaSector value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.BufferWidth);
            if (!isValid)
            {
                errorText = "BufferWidth";
                return false;
            }
            return true;
        }

        public static bool IsValid (HoldingPattern value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValBearingType (value.OutboundCourse);
            if (!isValid)
            {
                errorText = "OutboundCourse";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.InboundCourse);
            if (!isValid)
            {
                errorText = "InboundCourse";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.UpperLimit);
            if (!isValid)
            {
                errorText = "UpperLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.LowerLimit);
            if (!isValid)
            {
                errorText = "LowerLimit";
                return false;
            }
            isValid = DataTypeValidator.ValSpeedType (value.SpeedLimit);
            if (!isValid)
            {
                errorText = "SpeedLimit";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType (value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }
            return true;
        }

        public static bool IsValid (UnplannedHolding value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.AuthorizedAltitude);
            if (!isValid)
            {
                errorText = "AuthorizedAltitude";
                return false;
            }
            return true;
        }

        public static bool IsValid (HoldingPatternDistance value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            return true;
        }

        public static bool IsValid (FlightRestrictionLevel value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.UpperLevel);
            if (!isValid)
            {
                errorText = "UpperLevel";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.LowerLevel);
            if (!isValid)
            {
                errorText = "LowerLevel";
                return false;
            }
            return true;
        }

        public static bool IsValid (FlightRestriction value, ref string errorText)
        {
            bool isValid = DataTypeValidator.CodeFlightRestrictionDesignatorType (value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType (value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }
            return true;
        }

        public static bool IsValid (FlightRoutingElement value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoSequenceType (value.OrderNumber);
            if (!isValid)
            {
                errorText = "OrderNumber";
                return false;
            }
            isValid = DataTypeValidator.ValSpeedType (value.Speed);
            if (!isValid)
            {
                errorText = "Speed";
                return false;
            }
            return true;
        }

        public static bool IsValid (FlightConditionElement value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoSequenceType (value.Index);
            if (!isValid)
            {
                errorText = "Index";
                return false;
            }
            return true;
        }

        public static bool IsValid (RouteSegment value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceVerticalType (value.UpperLimit);
            if (!isValid)
            {
                errorText = "UpperLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.LowerLimit);
            if (!isValid)
            {
                errorText = "LowerLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MinimumObstacleClearanceAltitude);
            if (!isValid)
            {
                errorText = "MinimumObstacleClearanceAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.TrueTrack);
            if (!isValid)
            {
                errorText = "TrueTrack";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.MagneticTrack);
            if (!isValid)
            {
                errorText = "MagneticTrack";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.ReverseTrueTrack);
            if (!isValid)
            {
                errorText = "ReverseTrueTrack";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.ReverseMagneticTrack);
            if (!isValid)
            {
                errorText = "ReverseMagneticTrack";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.WidthLeft);
            if (!isValid)
            {
                errorText = "WidthLeft";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.WidthRight);
            if (!isValid)
            {
                errorText = "WidthRight";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MinimumEnrouteAltitude);
            if (!isValid)
            {
                errorText = "MinimumEnrouteAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MinimumCrossingAtEnd);
            if (!isValid)
            {
                errorText = "MinimumCrossingAtEnd";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.MaximumCrossingAtEnd);
            if (!isValid)
            {
                errorText = "MaximumCrossingAtEnd";
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType (value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            return true;
        }

        public static bool IsValid (Route value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoNumberType (value.DesignatorNumber);
            if (!isValid)
            {
                errorText = "DesignatorNumber";
                return false;
            }
            isValid = DataTypeValidator.TextDesignatorType (value.LocationDesignator);
            if (!isValid)
            {
                errorText = "LocationDesignator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            return true;
        }

        public static bool IsValid (ChangeOverPoint value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValDistanceType (value.Distance);
            if (!isValid)
            {
                errorText = "Distance";
                return false;
            }
            return true;
        }

        public static bool IsValid (AerialRefuelling value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoNumberType (value.DesignatorNumber);
            if (!isValid)
            {
                errorText = "DesignatorNumber";
                return false;
            }
            isValid = DataTypeValidator.TextDesignatorType (value.DesignatorSuffix);
            if (!isValid)
            {
                errorText = "DesignatorSuffix";
                return false;
            }
            isValid = DataTypeValidator.TextNameType (value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.NoNumberType (value.RadarBeaconSetting);
            if (!isValid)
            {
                errorText = "RadarBeaconSetting";
                return false;
            }
            isValid = DataTypeValidator.NoNumberType (value.XbandRadarSetting);
            if (!isValid)
            {
                errorText = "XbandRadarSetting";
                return false;
            }
            return true;
        }

        public static bool IsValid (AerialRefuellingPoint value, ref string errorText)
        {
            bool isValid = DataTypeValidator.NoSequenceType (value.Sequence);
            if (!isValid)
            {
                errorText = "Sequence";
                return false;
            }
            return true;
        }

        public static bool IsValid (AerialRefuellingAnchor value, ref string errorText)
        {
            bool isValid = DataTypeValidator.ValBearingType (value.OutboundCourse);
            if (!isValid)
            {
                errorText = "OutboundCourse";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType (value.InboundCourse);
            if (!isValid)
            {
                errorText = "InboundCourse";
                return false;
            }
            isValid = DataTypeValidator.ValSpeedType (value.SpeedLimit);
            if (!isValid)
            {
                errorText = "SpeedLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.LegSeparation);
            if (!isValid)
            {
                errorText = "LegSeparation";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType (value.LegLength);
            if (!isValid)
            {
                errorText = "LegLength";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType (value.RefuellingBaseLevel);
            if (!isValid)
            {
                errorText = "RefuellingBaseLevel";
                return false;
            }
            return true;
        }
    }
}