using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.PropertyEnum;

namespace Aran.Aim.Validation
{
    public static class FeatureValidator
    {
        public static bool IsValid(RunwayProtectArea value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((AirportHeliportProtectionArea)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(RunwayDirection value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.TrueBearing);
            if (!isValid)
            {
                errorText = "TrueBearing";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.TrueBearingAccuracy);
            if (!isValid)
            {
                errorText = "TrueBearingAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.MagneticBearing);
            if (!isValid)
            {
                errorText = "MagneticBearing";
                return false;
            }
            isValid = DataTypeValidator.ValSlopeType(value.SlopeTDZ);
            if (!isValid)
            {
                errorText = "SlopeTDZ";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.ElevationTDZ);
            if (!isValid)
            {
                errorText = "ElevationTDZ";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.ElevationTDZAccuracy);
            if (!isValid)
            {
                errorText = "ElevationTDZAccuracy";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RunwayCentrelinePoint value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.Location, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Runway value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.NominalLength);
            if (!isValid)
            {
                errorText = "NominalLength";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.LengthAccuracy);
            if (!isValid)
            {
                errorText = "LengthAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.NominalWidth);
            if (!isValid)
            {
                errorText = "NominalWidth";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.WidthAccuracy);
            if (!isValid)
            {
                errorText = "WidthAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.WidthShoulder);
            if (!isValid)
            {
                errorText = "WidthShoulder";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.LengthStrip);
            if (!isValid)
            {
                errorText = "LengthStrip";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.WidthStrip);
            if (!isValid)
            {
                errorText = "WidthStrip";
                return false;
            }
            isValid = ObjectValidator.IsValid((SurfaceCharacteristics)value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.OverallContaminant.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.OverallContaminant[i], ref errorText);
                if (!isValid)
                {
                    errorText = "OverallContaminant{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ArrestingGear value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Location);
            if (!isValid)
            {
                errorText = "Location";
                return false;
            }
            isValid = ObjectValidator.IsValid((SurfaceCharacteristics)value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RunwayElement value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            isValid = ObjectValidator.IsValid((SurfaceCharacteristics)value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(VisualGlideSlopeIndicator value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoNumberType(value.NumberBox);
            if (!isValid)
            {
                errorText = "NumberBox";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.SlopeAngle);
            if (!isValid)
            {
                errorText = "SlopeAngle";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.MinimumEyeHeightOverThreshold);
            if (!isValid)
            {
                errorText = "MinimumEyeHeightOverThreshold";
                return false;
            }
            return true;
        }

        public static bool IsValid(RunwayVisualRange value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = ObjectValidator.IsValid((ElevatedPoint)value.Location, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RunwayBlastPad value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((SurfaceCharacteristics)value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(TaxiHoldingPosition value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Taxiway value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.WidthShoulder);
            if (!isValid)
            {
                errorText = "WidthShoulder";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = ObjectValidator.IsValid((SurfaceCharacteristics)value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Contaminant.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Contaminant[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Contaminant{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(TaxiwayElement value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            isValid = ObjectValidator.IsValid((SurfaceCharacteristics)value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(GuidanceLine value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.ValSpeedType(value.MaxSpeed);
            if (!isValid)
            {
                errorText = "MaxSpeed";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedCurve)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Apron value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = ObjectValidator.IsValid((SurfaceCharacteristics)value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ApronElement value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            isValid = ObjectValidator.IsValid((SurfaceCharacteristics)value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AircraftStand value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = ObjectValidator.IsValid((SurfaceCharacteristics)value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.Location, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Road value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = ObjectValidator.IsValid((SurfaceCharacteristics)value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedSurface)value.SurfaceExtent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(DeicingArea value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = ObjectValidator.IsValid((SurfaceCharacteristics)value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(PassengerLoadingBridge value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(TouchDownLiftOffSafeArea value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((AirportHeliportProtectionArea)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(TouchDownLiftOff value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            isValid = DataTypeValidator.ValSlopeType(value.Slope);
            if (!isValid)
            {
                errorText = "Slope";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.AimingPoint, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((SurfaceCharacteristics)value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(GroundLightSystem value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ApproachLightingSystem value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            return true;
        }

        public static bool IsValid(Marking value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(FloatingDockSite value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(MarkingBuoy value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = ObjectValidator.IsValid((ElevatedPoint)value.Location, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SeaplaneLandingArea value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SeaplaneRampSite value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedCurve)value.Centreline, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(WorkArea value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SurveyControlPoint value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.Location, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(NonMovementArea value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AirportHeliportProtectionArea value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = ObjectValidator.IsValid((SurfaceCharacteristics)value.SurfaceProperties, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedSurface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AirportHeliportCollocation value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AirportHeliport value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.CodeAirportHeliportDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.CodeICAOType(value.LocationIndicatorICAO);
            if (!isValid)
            {
                errorText = "LocationIndicatorICAO";
                return false;
            }
            isValid = DataTypeValidator.CodeIATAType(value.DesignatorIATA);
            if (!isValid)
            {
                errorText = "DesignatorIATA";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.FieldElevation);
            if (!isValid)
            {
                errorText = "FieldElevation";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.FieldElevationAccuracy);
            if (!isValid)
            {
                errorText = "FieldElevationAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationType(value.MagneticVariation);
            if (!isValid)
            {
                errorText = "MagneticVariation";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.MagneticVariationAccuracy);
            if (!isValid)
            {
                errorText = "MagneticVariationAccuracy";
                return false;
            }
            isValid = DataTypeValidator.DateYearType(value.DateMagneticVariation);
            if (!isValid)
            {
                errorText = "DateMagneticVariation";
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationChangeType(value.MagneticVariationChange);
            if (!isValid)
            {
                errorText = "MagneticVariationChange";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.TransitionAltitude);
            if (!isValid)
            {
                errorText = "TransitionAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValFLType(value.TransitionLevel);
            if (!isValid)
            {
                errorText = "TransitionLevel";
                return false;
            }

            for (int i = 0; i < value.ServedCity.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.ServedCity[i], ref errorText);
                if (!isValid)
                {
                    errorText = "ServedCity{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.ARP, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedSurface)value.AviationBoundary, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Contact.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Contact[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Contact{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AltimeterSource value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AirportHotSpot value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType(value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedSurface)value.Area, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AuthorityForAirspace value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Airspace value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.CodeAirspaceDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.LocalType);
            if (!isValid)
            {
                errorText = "LocalType";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.ValFLType(value.UpperLowerSeparation);
            if (!isValid)
            {
                errorText = "UpperLowerSeparation";
                return false;
            }

            for (int i = 0; i < value.GeometryComponent.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.GeometryComponent[i], ref errorText);
                if (!isValid)
                {
                    errorText = "GeometryComponent{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(GeoBorder value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = ObjectValidator.IsValid((Curve)value.Border, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(PrecisionApproachRadar value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((RadarEquipment)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.Slope);
            if (!isValid)
            {
                errorText = "Slope";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.SlopeAccuracy);
            if (!isValid)
            {
                errorText = "SlopeAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid(PrimarySurveillanceRadar value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((SurveillanceRadar)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(RadarEquipment value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.TextDesignatorType(value.SerialNumber);
            if (!isValid)
            {
                errorText = "SerialNumber";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Range);
            if (!isValid)
            {
                errorText = "Range";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.RangeAccuracy);
            if (!isValid)
            {
                errorText = "RangeAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationType(value.MagneticVariation);
            if (!isValid)
            {
                errorText = "MagneticVariation";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.MagneticVariationAccuracy);
            if (!isValid)
            {
                errorText = "MagneticVariationAccuracy";
                return false;
            }
            isValid = DataTypeValidator.DateYearType(value.DateMagneticVariation);
            if (!isValid)
            {
                errorText = "DateMagneticVariation";
                return false;
            }

            for (int i = 0; i < value.Contact.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Contact[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Contact{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.Location, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RadarSystem value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Model);
            if (!isValid)
            {
                errorText = "Model";
                return false;
            }
            isValid = DataTypeValidator.TextDesignatorType(value.BroadcastIdentifier);
            if (!isValid)
            {
                errorText = "BroadcastIdentifier";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.Location, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SecondarySurveillanceRadar value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((SurveillanceRadar)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(SurveillanceRadar value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((RadarEquipment)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.VerticalCoverageAltitude);
            if (!isValid)
            {
                errorText = "VerticalCoverageAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.VerticalCoverageDistance);
            if (!isValid)
            {
                errorText = "VerticalCoverageDistance";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.VerticalCoverageAzimuth);
            if (!isValid)
            {
                errorText = "VerticalCoverageAzimuth";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.TiltAngle);
            if (!isValid)
            {
                errorText = "TiltAngle";
                return false;
            }
            isValid = DataTypeValidator.TextDesignatorType(value.AutomatedRadarTerminalSystem);
            if (!isValid)
            {
                errorText = "AutomatedRadarTerminalSystem";
                return false;
            }
            return true;
        }

        public static bool IsValid(HoldingAssessment value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.UpperLimit);
            if (!isValid)
            {
                errorText = "UpperLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.LowerLimit);
            if (!isValid)
            {
                errorText = "LowerLimit";
                return false;
            }
            isValid = DataTypeValidator.ValSpeedType(value.SpeedLimit);
            if (!isValid)
            {
                errorText = "SpeedLimit";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.PatternTemplate);
            if (!isValid)
            {
                errorText = "PatternTemplate";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.LegLengthToward);
            if (!isValid)
            {
                errorText = "LegLengthToward";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.LegLengthAway);
            if (!isValid)
            {
                errorText = "LegLengthAway";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(StandardLevelTable value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(StandardLevelSector value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValBearingType(value.FromTrack);
            if (!isValid)
            {
                errorText = "FromTrack";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.ToTrack);
            if (!isValid)
            {
                errorText = "ToTrack";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(StandardLevelColumn value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Level.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Level[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Level{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RadioFrequencyArea value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValAngleType(value.AngleScallop);
            if (!isValid)
            {
                errorText = "AngleScallop";
                return false;
            }

            for (int i = 0; i < value.Sector.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Sector[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Sector{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SpecialDate value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.DateMonthDayType(value.DateDay);
            if (!isValid)
            {
                errorText = "DateDay";
                return false;
            }
            isValid = DataTypeValidator.DateYearType(value.DateYear);
            if (!isValid)
            {
                errorText = "DateYear";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Service value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.Location, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.GroundCommunication.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.GroundCommunication[i], ref errorText);
                if (!isValid)
                {
                    errorText = "GroundCommunication{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RadioCommunicationChannel value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextDesignatorType(value.Logon);
            if (!isValid)
            {
                errorText = "Logon";
                return false;
            }

            for (int i = 0; i < value.Location.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Location[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Location{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(PilotControlledLighting value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoNumberType(value.IntensitySteps);
            if (!isValid)
            {
                errorText = "IntensitySteps";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType(value.ActivationInstruction);
            if (!isValid)
            {
                errorText = "ActivationInstruction";
                return false;
            }

            for (int i = 0; i < value.ControlledLightIntensity.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.ControlledLightIntensity[i], ref errorText);
                if (!isValid)
                {
                    errorText = "ControlledLightIntensity{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(InformationService value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((Service)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(TrafficSeparationService value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((Service)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(GroundTrafficControlService value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((TrafficSeparationService)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(AirTrafficControlService value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((TrafficSeparationService)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(AirTrafficManagementService value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((Service)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(SearchRescueService value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((Service)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(AirportGroundService value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((Service)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(PassengerService value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((AirportGroundService)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(AircraftGroundService value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((AirportGroundService)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(FireFightingService value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((AirportGroundService)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(AirportClearanceService value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((AirportGroundService)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType(value.SnowPlan);
            if (!isValid)
            {
                errorText = "SnowPlan";
                return false;
            }
            return true;
        }

        public static bool IsValid(AirportSuppliesService value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((AirportGroundService)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(AngleIndication value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValBearingType(value.Angle);
            if (!isValid)
            {
                errorText = "Angle";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.TrueAngle);
            if (!isValid)
            {
                errorText = "TrueAngle";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.MinimumReceptionAltitude);
            if (!isValid)
            {
                errorText = "MinimumReceptionAltitude";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(DistanceIndication value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.Distance);
            if (!isValid)
            {
                errorText = "Distance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.MinimumReceptionAltitude);
            if (!isValid)
            {
                errorText = "MinimumReceptionAltitude";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Azimuth value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((NavaidEquipment)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.TrueBearing);
            if (!isValid)
            {
                errorText = "TrueBearing";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.TrueBearingAccuracy);
            if (!isValid)
            {
                errorText = "TrueBearingAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.MagneticBearing);
            if (!isValid)
            {
                errorText = "MagneticBearing";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.AngleProportionalLeft);
            if (!isValid)
            {
                errorText = "AngleProportionalLeft";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.AngleProportionalRight);
            if (!isValid)
            {
                errorText = "AngleProportionalRight";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.AngleCoverLeft);
            if (!isValid)
            {
                errorText = "AngleCoverLeft";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.AngleCoverRight);
            if (!isValid)
            {
                errorText = "AngleCoverRight";
                return false;
            }
            return true;
        }

        public static bool IsValid(CheckpointINS value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((NavigationSystemCheckpoint)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(CheckpointVOR value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((NavigationSystemCheckpoint)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(DME value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((NavaidEquipment)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Displace);
            if (!isValid)
            {
                errorText = "Displace";
                return false;
            }
            return true;
        }

        public static bool IsValid(Elevation value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((NavaidEquipment)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.AngleNominal);
            if (!isValid)
            {
                errorText = "AngleNominal";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.AngleMinimum);
            if (!isValid)
            {
                errorText = "AngleMinimum";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.AngleSpan);
            if (!isValid)
            {
                errorText = "AngleSpan";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.AngleAccuracy);
            if (!isValid)
            {
                errorText = "AngleAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid(Glidepath value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((NavaidEquipment)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.Slope);
            if (!isValid)
            {
                errorText = "Slope";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.AngleAccuracy);
            if (!isValid)
            {
                errorText = "AngleAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.Rdh);
            if (!isValid)
            {
                errorText = "Rdh";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.RdhAccuracy);
            if (!isValid)
            {
                errorText = "RdhAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid(Localizer value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((NavaidEquipment)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.MagneticBearing);
            if (!isValid)
            {
                errorText = "MagneticBearing";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.MagneticBearingAccuracy);
            if (!isValid)
            {
                errorText = "MagneticBearingAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.TrueBearing);
            if (!isValid)
            {
                errorText = "TrueBearing";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.TrueBearingAccuracy);
            if (!isValid)
            {
                errorText = "TrueBearingAccuracy";
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationType(value.Declination);
            if (!isValid)
            {
                errorText = "Declination";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.WidthCourse);
            if (!isValid)
            {
                errorText = "WidthCourse";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.WidthCourseAccuracy);
            if (!isValid)
            {
                errorText = "WidthCourseAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid(MarkerBeacon value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((NavaidEquipment)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.AxisBearing);
            if (!isValid)
            {
                errorText = "AxisBearing";
                return false;
            }
            isValid = DataTypeValidator.CodeAuralMorseType(value.AuralMorseCode);
            if (!isValid)
            {
                errorText = "AuralMorseCode ";
                return false;
            }
            return true;
        }

        public static bool IsValid(Navaid value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.CodeNavaidDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.Location, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(NavaidEquipment value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.CodeNavaidDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationType(value.MagneticVariation);
            if (!isValid)
            {
                errorText = "MagneticVariation";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.MagneticVariationAccuracy);
            if (!isValid)
            {
                errorText = "MagneticVariationAccuracy";
                return false;
            }
            isValid = DataTypeValidator.DateYearType(value.DateMagneticVariation);
            if (!isValid)
            {
                errorText = "DateMagneticVariation";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.Location, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(NavigationSystemCheckpoint value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.UpperLimit);
            if (!isValid)
            {
                errorText = "UpperLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.LowerLimit);
            if (!isValid)
            {
                errorText = "LowerLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Distance);
            if (!isValid)
            {
                errorText = "Distance";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.Angle);
            if (!isValid)
            {
                errorText = "Angle";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.Position, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SDF value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((NavaidEquipment)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.MagneticBearing);
            if (!isValid)
            {
                errorText = "MagneticBearing";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.TrueBearing);
            if (!isValid)
            {
                errorText = "TrueBearing";
                return false;
            }
            return true;
        }

        public static bool IsValid(NDB value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((NavaidEquipment)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(SpecialNavigationStation value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.Position, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(VOR value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((NavaidEquipment)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationType(value.Declination);
            if (!isValid)
            {
                errorText = "Declination";
                return false;
            }
            return true;
        }

        public static bool IsValid(TACAN value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((NavaidEquipment)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValMagneticVariationType(value.Declination);
            if (!isValid)
            {
                errorText = "Declination";
                return false;
            }
            return true;
        }

        public static bool IsValid(SpecialNavigationSystem value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.CodeSpecialNavigationChainDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(DirectionFinder value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((NavaidEquipment)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(DesignatedPoint value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.CodeDesignatedPointDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = ObjectValidator.IsValid((AixmPoint)value.Location, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SignificantPointInAirspace value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AeronauticalGroundLight value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.Location, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(OrganisationAuthority value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.CodeOrganisationDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }

            for (int i = 0; i < value.Contact.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Contact[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Contact{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Unit value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.CodeOrganisationDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = ObjectValidator.IsValid((ElevatedPoint)value.Position, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Contact.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Contact[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Contact{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ObstacleArea value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = ObjectValidator.IsValid((Surface)value.SurfaceExtent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(VerticalStructure value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Radius);
            if (!isValid)
            {
                errorText = "Radius";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(CirclingArea value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = ObjectValidator.IsValid((Surface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((AircraftCharacteristic)value.AircraftCategory, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.DesignSurface.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.DesignSurface[i], ref errorText);
                if (!isValid)
                {
                    errorText = "DesignSurface{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(TerminalArrivalArea value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.OuterBufferWidth);
            if (!isValid)
            {
                errorText = "OuterBufferWidth";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.LateralBufferWidth);
            if (!isValid)
            {
                errorText = "LateralBufferWidth";
                return false;
            }
            isValid = ObjectValidator.IsValid((Surface)value.Buffer, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(InstrumentApproachProcedure value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((Procedure)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.CopterTrack);
            if (!isValid)
            {
                errorText = "CopterTrack";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType(value.CourseReversalInstruction);
            if (!isValid)
            {
                errorText = "CourseReversalInstruction";
                return false;
            }

            for (int i = 0; i < value.MissedInstruction.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.MissedInstruction[i], ref errorText);
                if (!isValid)
                {
                    errorText = "MissedInstruction{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(StandardInstrumentDeparture value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((Procedure)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.TextSIDSTARDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            return true;
        }

        public static bool IsValid(NavigationArea value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.MinimumCeiling);
            if (!isValid)
            {
                errorText = "MinimumCeiling";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.MinimumVisibility);
            if (!isValid)
            {
                errorText = "MinimumVisibility";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(StandardInstrumentArrival value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((Procedure)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.TextSIDSTARDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            return true;
        }

        public static bool IsValid(Procedure value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextInstructionType(value.CommunicationFailureInstruction);
            if (!isValid)
            {
                errorText = "CommunicationFailureInstruction";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType(value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }

            for (int i = 0; i < value.AircraftCharacteristic.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.AircraftCharacteristic[i], ref errorText);
                if (!isValid)
                {
                    errorText = "AircraftCharacteristic{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(NavigationAreaRestriction value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = ObjectValidator.IsValid((ObstacleAssessmentArea)value.DesignSurface, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((CircleSector)value.SectorDefinition, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SegmentLeg value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValBearingType(value.Course);
            if (!isValid)
            {
                errorText = "Course";
                return false;
            }
            isValid = DataTypeValidator.ValSpeedType(value.SpeedLimit);
            if (!isValid)
            {
                errorText = "SpeedLimit";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.BankAngle);
            if (!isValid)
            {
                errorText = "BankAngle";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.UpperLimitAltitude);
            if (!isValid)
            {
                errorText = "UpperLimitAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.LowerLimitAltitude);
            if (!isValid)
            {
                errorText = "LowerLimitAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.AltitudeOverrideATC);
            if (!isValid)
            {
                errorText = "AltitudeOverrideATC";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.VerticalAngle);
            if (!isValid)
            {
                errorText = "VerticalAngle";
                return false;
            }
            isValid = ObjectValidator.IsValid((TerminalSegmentPoint)value.StartPoint, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((TerminalSegmentPoint)value.EndPoint, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((Curve)value.Trajectory, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((TerminalSegmentPoint)value.ArcCentre, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.DesignSurface.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.DesignSurface[i], ref errorText);
                if (!isValid)
                {
                    errorText = "DesignSurface{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ApproachLeg value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((SegmentLeg)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(ArrivalFeederLeg value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((ApproachLeg)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType(value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            return true;
        }

        public static bool IsValid(ArrivalLeg value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((SegmentLeg)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType(value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            return true;
        }

        public static bool IsValid(DepartureLeg value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((SegmentLeg)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType(value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.MinimumObstacleClearanceAltitude);
            if (!isValid)
            {
                errorText = "MinimumObstacleClearanceAltitude";
                return false;
            }

            for (int i = 0; i < value.Condition.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Condition[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Condition{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(FinalLeg value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((ApproachLeg)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.CourseOffsetAngle);
            if (!isValid)
            {
                errorText = "CourseOffsetAngle";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.CourseCentrelineDistance);
            if (!isValid)
            {
                errorText = "CourseCentrelineDistance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.CourseOffsetDistance);
            if (!isValid)
            {
                errorText = "CourseOffsetDistance";
                return false;
            }
            isValid = ObjectValidator.IsValid((TerminalSegmentPoint)value.VisualDescentPoint, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(InitialLeg value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((ApproachLeg)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType(value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            return true;
        }

        public static bool IsValid(IntermediateLeg value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((ApproachLeg)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType(value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            return true;
        }

        public static bool IsValid(MissedApproachLeg value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((ApproachLeg)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.HeightMAPT);
            if (!isValid)
            {
                errorText = "HeightMAPT";
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType(value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }

            for (int i = 0; i < value.Condition.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Condition[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Condition{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ProcedureDME value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SafeAltitudeArea value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Sector.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Sector[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Sector{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(HoldingPattern value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValBearingType(value.OutboundCourse);
            if (!isValid)
            {
                errorText = "OutboundCourse";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.InboundCourse);
            if (!isValid)
            {
                errorText = "InboundCourse";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.UpperLimit);
            if (!isValid)
            {
                errorText = "UpperLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.LowerLimit);
            if (!isValid)
            {
                errorText = "LowerLimit";
                return false;
            }
            isValid = DataTypeValidator.ValSpeedType(value.SpeedLimit);
            if (!isValid)
            {
                errorText = "SpeedLimit";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType(value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }
            isValid = ObjectValidator.IsValid((Curve)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(UnplannedHolding value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.AuthorizedAltitude);
            if (!isValid)
            {
                errorText = "AuthorizedAltitude";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AirspaceBorderCrossing value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(FlightRestriction value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.CodeFlightRestrictionDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType(value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RouteSegment value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.UpperLimit);
            if (!isValid)
            {
                errorText = "UpperLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.LowerLimit);
            if (!isValid)
            {
                errorText = "LowerLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.MinimumObstacleClearanceAltitude);
            if (!isValid)
            {
                errorText = "MinimumObstacleClearanceAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.TrueTrack);
            if (!isValid)
            {
                errorText = "TrueTrack";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.MagneticTrack);
            if (!isValid)
            {
                errorText = "MagneticTrack";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.ReverseTrueTrack);
            if (!isValid)
            {
                errorText = "ReverseTrueTrack";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.ReverseMagneticTrack);
            if (!isValid)
            {
                errorText = "ReverseMagneticTrack";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.WidthLeft);
            if (!isValid)
            {
                errorText = "WidthLeft";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.WidthRight);
            if (!isValid)
            {
                errorText = "WidthRight";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.MinimumEnrouteAltitude);
            if (!isValid)
            {
                errorText = "MinimumEnrouteAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.MinimumCrossingAtEnd);
            if (!isValid)
            {
                errorText = "MinimumCrossingAtEnd";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.MaximumCrossingAtEnd);
            if (!isValid)
            {
                errorText = "MaximumCrossingAtEnd";
                return false;
            }
            isValid = DataTypeValidator.CodeRNPType(value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            isValid = ObjectValidator.IsValid((EnRouteSegmentPoint)value.Start, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((ObstacleAssessmentArea)value.EvaluationArea, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((Curve)value.CurveExtent, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = ObjectValidator.IsValid((EnRouteSegmentPoint)value.End, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RouteDME value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Route value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoNumberType(value.DesignatorNumber);
            if (!isValid)
            {
                errorText = "DesignatorNumber";
                return false;
            }
            isValid = DataTypeValidator.TextDesignatorType(value.LocationDesignator);
            if (!isValid)
            {
                errorText = "LocationDesignator";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ChangeOverPoint value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.Distance);
            if (!isValid)
            {
                errorText = "Distance";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AerialRefuelling value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoNumberType(value.DesignatorNumber);
            if (!isValid)
            {
                errorText = "DesignatorNumber";
                return false;
            }
            isValid = DataTypeValidator.TextDesignatorType(value.DesignatorSuffix);
            if (!isValid)
            {
                errorText = "DesignatorSuffix";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.NoNumberType(value.RadarBeaconSetting);
            if (!isValid)
            {
                errorText = "RadarBeaconSetting";
                return false;
            }
            isValid = DataTypeValidator.NoNumberType(value.XbandRadarSetting);
            if (!isValid)
            {
                errorText = "XbandRadarSetting";
                return false;
            }

            for (int i = 0; i < value.Anchor.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Anchor[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Anchor{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RulesProcedures value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = ObjectValidator.IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }


    }
}
