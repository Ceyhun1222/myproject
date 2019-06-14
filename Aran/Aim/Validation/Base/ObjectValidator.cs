using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;

namespace Aran.Aim.Validation
{
    public static class ObjectValidator
    {
        public static bool IsValid(NavaidEquipmentDistance value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.Distance);
            if (!isValid)
            {
                errorText = "Distance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.DistanceAccuracy);
            if (!isValid)
            {
                errorText = "DistanceAccuracy";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RunwayDeclaredDistance value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.DeclaredValue.Count; i++)
            {
                isValid = IsValid(value.DeclaredValue[i], ref errorText);
                if (!isValid)
                {
                    errorText = "DeclaredValue{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RunwayDeclaredDistanceValue value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.Distance);
            if (!isValid)
            {
                errorText = "Distance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.DistanceAccuracy);
            if (!isValid)
            {
                errorText = "DistanceAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid(LightActivation value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoNumberType(value.Clicks);
            if (!isValid)
            {
                errorText = "Clicks";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(MarkingElement value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SurfaceContamination value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValFrictionType(value.FrictionCoefficient);
            if (!isValid)
            {
                errorText = "FrictionCoefficient";
                return false;
            }
            isValid = DataTypeValidator.TimeType(value.FurtherClearanceTime);
            if (!isValid)
            {
                errorText = "FurtherClearanceTime";
                return false;
            }
            isValid = DataTypeValidator.ValPercentType(value.Proportion);
            if (!isValid)
            {
                errorText = "Proportion";
                return false;
            }

            for (int i = 0; i < value.CriticalRidge.Count; i++)
            {
                isValid = IsValid(value.CriticalRidge[i], ref errorText);
                if (!isValid)
                {
                    errorText = "CriticalRidge{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Layer.Count; i++)
            {
                isValid = IsValid(value.Layer[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Layer{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Ridge value, ref string errorText)
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
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RunwayContamination value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((SurfaceContamination)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.ClearedLength);
            if (!isValid)
            {
                errorText = "ClearedLength";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.ClearedWidth);
            if (!isValid)
            {
                errorText = "ClearedWidth";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.FurtherClearanceLength);
            if (!isValid)
            {
                errorText = "FurtherClearanceLength";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.FurtherClearanceWidth);
            if (!isValid)
            {
                errorText = "FurtherClearanceWidth";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.ClearedLengthBegin);
            if (!isValid)
            {
                errorText = "ClearedLengthBegin";
                return false;
            }
            return true;
        }

        public static bool IsValid(TaxiwayContamination value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((SurfaceContamination)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.ClearedWidth);
            if (!isValid)
            {
                errorText = "ClearedWidth";
                return false;
            }
            return true;
        }

        public static bool IsValid(SurfaceContaminationLayer value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoSequenceType(value.LayerOrder);
            if (!isValid)
            {
                errorText = "LayerOrder";
                return false;
            }

            for (int i = 0; i < value.Extent.Count; i++)
            {
                isValid = IsValid(value.Extent[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Extent{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RunwaySectionContamination value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((SurfaceContamination)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(TouchDownLiftOffContamination value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((SurfaceContamination)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(ApronContamination value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((SurfaceContamination)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(AircraftStandContamination value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((SurfaceContamination)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(AirportHeliportContamination value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((SurfaceContamination)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(SurfaceCharacteristics value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValPCNType(value.ClassPCN);
            if (!isValid)
            {
                errorText = "ClassPCN";
                return false;
            }
            isValid = DataTypeValidator.ValWeightType(value.WeightSIWL);
            if (!isValid)
            {
                errorText = "WeightSIWL";
                return false;
            }
            isValid = DataTypeValidator.ValWeightType(value.WeightAUW);
            if (!isValid)
            {
                errorText = "WeightAUW";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(City value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(UsageCondition value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Contact.Count; i++)
            {
                isValid = IsValid(value.Contact[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Contact{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ConditionCombination value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Weather.Count; i++)
            {
                isValid = IsValid(value.Weather[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Weather{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Aircraft.Count; i++)
            {
                isValid = IsValid(value.Aircraft[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Aircraft{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AirspaceGeometryComponent value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoSequenceType(value.OperationSequence);
            if (!isValid)
            {
                errorText = "OperationSequence";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AirspaceVolume value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.UpperLimit);
            if (!isValid)
            {
                errorText = "UpperLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.MaximumLimit);
            if (!isValid)
            {
                errorText = "MaximumLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.LowerLimit);
            if (!isValid)
            {
                errorText = "LowerLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.MinimumLimit);
            if (!isValid)
            {
                errorText = "MinimumLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Width);
            if (!isValid)
            {
                errorText = "Width";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AirspaceActivation value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Levels.Count; i++)
            {
                isValid = IsValid(value.Levels[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Levels{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Aircraft.Count; i++)
            {
                isValid = IsValid(value.Aircraft[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Aircraft{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AirspaceLayerClass value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.AssociatedLevels.Count; i++)
            {
                isValid = IsValid(value.AssociatedLevels[i], ref errorText);
                if (!isValid)
                {
                    errorText = "AssociatedLevels{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AirspaceVolumeDependency value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Curve value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.HorizontalAccuracy);
            if (!isValid)
            {
                errorText = "HorizontalAccuracy";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ElevatedCurve value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((Curve)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.Elevation);
            if (!isValid)
            {
                errorText = "Elevation";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.VerticalAccuracy);
            if (!isValid)
            {
                errorText = "VerticalAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid(ElevatedPoint value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((AixmPoint)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.Elevation);
            if (!isValid)
            {
                errorText = "Elevation";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.VerticalAccuracy);
            if (!isValid)
            {
                errorText = "VerticalAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid(ElevatedSurface value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((Surface)value, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.Elevation);
            if (!isValid)
            {
                errorText = "Elevation";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.VerticalAccuracy);
            if (!isValid)
            {
                errorText = "VerticalAccuracy";
                return false;
            }
            return true;
        }

        public static bool IsValid(AixmPoint value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.HorizontalAccuracy);
            if (!isValid)
            {
                errorText = "HorizontalAccuracy";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Surface value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.HorizontalAccuracy);
            if (!isValid)
            {
                errorText = "HorizontalAccuracy";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RadarComponent value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoSequenceType(value.CollocationGroup);
            if (!isValid)
            {
                errorText = "CollocationGroup";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Reflector value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((ElevatedPoint)value.TouchdownReflector, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SurveillanceGroundStation value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ObstacleAssessmentArea value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoNumberType(value.SectionNumber);
            if (!isValid)
            {
                errorText = "SectionNumber";
                return false;
            }
            isValid = DataTypeValidator.ValSlopeType(value.Slope);
            if (!isValid)
            {
                errorText = "Slope";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.AssessedAltitude);
            if (!isValid)
            {
                errorText = "AssessedAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.SlopeLowerAltitude);
            if (!isValid)
            {
                errorText = "SlopeLowerAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValSlopeType(value.GradientLowHigh);
            if (!isValid)
            {
                errorText = "GradientLowHigh";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.SafetyRegulation);
            if (!isValid)
            {
                errorText = "SafetyRegulation";
                return false;
            }

            for (int i = 0; i < value.AircraftCategory.Count; i++)
            {
                isValid = IsValid(value.AircraftCategory[i], ref errorText);
                if (!isValid)
                {
                    errorText = "AircraftCategory{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.SignificantObstacle.Count; i++)
            {
                isValid = IsValid(value.SignificantObstacle[i], ref errorText);
                if (!isValid)
                {
                    errorText = "SignificantObstacle{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            isValid = IsValid((Surface)value.Surface, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = IsValid((Curve)value.StartingCurve, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Obstruction value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.RequiredClearance);
            if (!isValid)
            {
                errorText = "RequiredClearance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.MinimumAltitude);
            if (!isValid)
            {
                errorText = "MinimumAltitude";
                return false;
            }
            isValid = DataTypeValidator.ValAngleType(value.SlopePenetration);
            if (!isValid)
            {
                errorText = "SlopePenetration";
                return false;
            }

            for (int i = 0; i < value.ObstaclePlacement.Count; i++)
            {
                isValid = IsValid(value.ObstaclePlacement[i], ref errorText);
                if (!isValid)
                {
                    errorText = "ObstaclePlacement{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AltitudeAdjustment value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.AltitudeAdjustmentP);
            if (!isValid)
            {
                errorText = "AltitudeAdjustment";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ObstaclePlacement value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValBearingType(value.ObstacleBearing);
            if (!isValid)
            {
                errorText = "ObstacleBearing";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.ObstacleDistance);
            if (!isValid)
            {
                errorText = "ObstacleDistance";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.PointType);
            if (!isValid)
            {
                errorText = "PointType";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(StandardLevel value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.VerticalDistance);
            if (!isValid)
            {
                errorText = "VerticalDistance";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ContactInformation value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.Name);
            if (!isValid)
            {
                errorText = "Name";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.Title);
            if (!isValid)
            {
                errorText = "Title";
                return false;
            }

            for (int i = 0; i < value.Address.Count; i++)
            {
                isValid = IsValid(value.Address[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Address{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.NetworkNode.Count; i++)
            {
                isValid = IsValid(value.NetworkNode[i], ref errorText);
                if (!isValid)
                {
                    errorText = "NetworkNode{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.PhoneFax.Count; i++)
            {
                isValid = IsValid(value.PhoneFax[i], ref errorText);
                if (!isValid)
                {
                    errorText = "PhoneFax{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(OnlineContact value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextAddressType(value.Linkage);
            if (!isValid)
            {
                errorText = "Linkage";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.Protocol);
            if (!isValid)
            {
                errorText = "Protocol";
                return false;
            }
            isValid = DataTypeValidator.TextAddressType(value.eMail);
            if (!isValid)
            {
                errorText = "eMail";
                return false;
            }
            return true;
        }

        public static bool IsValid(PostalAddress value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextAddressType(value.DeliveryPoint);
            if (!isValid)
            {
                errorText = "DeliveryPoint";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.City);
            if (!isValid)
            {
                errorText = "City";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.AdministrativeArea);
            if (!isValid)
            {
                errorText = "AdministrativeArea";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.PostalCode);
            if (!isValid)
            {
                errorText = "PostalCode";
                return false;
            }
            isValid = DataTypeValidator.TextNameType(value.Country);
            if (!isValid)
            {
                errorText = "Country";
                return false;
            }
            return true;
        }

        public static bool IsValid(TelephoneContact value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextPhoneType(value.Voice);
            if (!isValid)
            {
                errorText = "Voice";
                return false;
            }
            isValid = DataTypeValidator.TextPhoneType(value.Facsimile);
            if (!isValid)
            {
                errorText = "Facsimile";
                return false;
            }
            return true;
        }

        public static bool IsValid(AircraftCharacteristic value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.CodeAircraftICAOType(value.TypeAircraftICAO);
            if (!isValid)
            {
                errorText = "TypeAircraftICAO";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.WingSpan);
            if (!isValid)
            {
                errorText = "WingSpan";
                return false;
            }
            isValid = DataTypeValidator.ValWeightType(value.Weight);
            if (!isValid)
            {
                errorText = "Weight";
                return false;
            }
            isValid = DataTypeValidator.NoNumberType(value.Passengers);
            if (!isValid)
            {
                errorText = "Passengers";
                return false;
            }
            isValid = DataTypeValidator.ValSpeedType(value.Speed);
            if (!isValid)
            {
                errorText = "Speed";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(FlightCharacteristic value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(LightElement value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((ElevatedPoint)value.Location, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AirspaceLayer value, ref string errorText)
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

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(CircleSector value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValBearingType(value.FromAngle);
            if (!isValid)
            {
                errorText = "FromAngle";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.ToAngle);
            if (!isValid)
            {
                errorText = "ToAngle";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.InnerDistance);
            if (!isValid)
            {
                errorText = "InnerDistance";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.OuterDistance);
            if (!isValid)
            {
                errorText = "OuterDistance";
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

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Timesheet value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.DateMonthDayType(value.StartDate);
            if (!isValid)
            {
                errorText = "StartDate";
                return false;
            }
            isValid = DataTypeValidator.DateMonthDayType(value.EndDate);
            if (!isValid)
            {
                errorText = "EndDate";
                return false;
            }
            isValid = DataTypeValidator.TimeType(value.StartTime);
            if (!isValid)
            {
                errorText = "StartTime";
                return false;
            }
            isValid = DataTypeValidator.TimeType(value.EndTime);
            if (!isValid)
            {
                errorText = "EndTime";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(PropertiesWithSchedule value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.TimeInterval.Count; i++)
            {
                isValid = IsValid(value.TimeInterval[i], ref errorText);
                if (!isValid)
                {
                    errorText = "TimeInterval{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Meteorology value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.Visibility);
            if (!isValid)
            {
                errorText = "Visibility";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.RunwayVisualRange);
            if (!isValid)
            {
                errorText = "RunwayVisualRange";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(CallsignDetail value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNameType(value.CallSign);
            if (!isValid)
            {
                errorText = "CallSign";
                return false;
            }
            isValid = DataTypeValidator.CodeLanguageType(value.Language);
            if (!isValid)
            {
                errorText = "Language";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Fuel value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Nitrogen value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Oil value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Oxygen value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(PointReference value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((Surface)value.FixToleranceArea, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SegmentPoint value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(TerminalSegmentPoint value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValBearingType(value.LeadRadial);
            if (!isValid)
            {
                errorText = "LeadRadial";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.LeadDME);
            if (!isValid)
            {
                errorText = "LeadDME";
                return false;
            }
            return true;
        }

        public static bool IsValid(EnRouteSegmentPoint value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.TurnRadius);
            if (!isValid)
            {
                errorText = "TurnRadius";
                return false;
            }
            return true;
        }

        public static bool IsValid(AngleUse value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AuthorityForNavaidEquipment value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AuthorityForSpecialNavigationStation value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AuthorityForSpecialNavigationSystem value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(NavaidComponent value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoSequenceType(value.CollocationGroup);
            if (!isValid)
            {
                errorText = "CollocationGroup";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(Note value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextPropertyNameType(value.PropertyName);
            if (!isValid)
            {
                errorText = "PropertyName";
                return false;
            }

            for (int i = 0; i < value.TranslatedNote.Count; i++)
            {
                isValid = IsValid(value.TranslatedNote[i], ref errorText);
                if (!isValid)
                {
                    errorText = "TranslatedNote{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(LinguisticNote value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextNoteType(value.Note.Value);
            if (!isValid)
            {
                errorText = "Note.Value";
                return false;
            }
            return true;
        }

        public static bool IsValid(OrganisationAuthorityAssociation value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(UnitDependency value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(VerticalStructurePart value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.VerticalExtent);
            if (!isValid)
            {
                errorText = "VerticalExtent";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.VerticalExtentAccuracy);
            if (!isValid)
            {
                errorText = "VerticalExtentAccuracy";
                return false;
            }
            isValid = DataTypeValidator.TextDesignatorType(value.Designator);
            if (!isValid)
            {
                errorText = "Designator";
                return false;
            }
            return true;
        }

        public static bool IsValid(CirclingRestriction value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((CircleSector)value.SectorDescription, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = IsValid((Surface)value.RestrictionArea, ref errorText);
            if (!isValid)
            {
                return false;
            }
            return true;
        }

        public static bool IsValid(Minima value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.Altitude);
            if (!isValid)
            {
                errorText = "Altitude";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.Height);
            if (!isValid)
            {
                errorText = "Height";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.MilitaryHeight);
            if (!isValid)
            {
                errorText = "MilitaryHeight";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.RadioHeight);
            if (!isValid)
            {
                errorText = "RadioHeight";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Visibility);
            if (!isValid)
            {
                errorText = "Visibility";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.MilitaryVisibility);
            if (!isValid)
            {
                errorText = "MilitaryVisibility";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(EquipmentUnavailableAdjustment value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.AdjustmentINOPCol.Count; i++)
            {
                isValid = IsValid(value.AdjustmentINOPCol[i], ref errorText);
                if (!isValid)
                {
                    errorText = "AdjustmentINOPCol{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(EquipmentUnavailableAdjustmentColumn value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.VisibilityAdjustment);
            if (!isValid)
            {
                errorText = "VisibilityAdjustment";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(TerminalArrivalAreaSector value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((CircleSector)value.SectorDefinition, ref errorText);
            if (!isValid)
            {
                return false;
            }
            isValid = IsValid((Surface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.SignificantObstacle.Count; i++)
            {
                isValid = IsValid(value.SignificantObstacle[i], ref errorText);
                if (!isValid)
                {
                    errorText = "SignificantObstacle{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(FASDataBlock value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValAlarmLimitType(value.HorizontalAlarmLimit);
            if (!isValid)
            {
                errorText = "HorizontalAlarmLimit";
                return false;
            }
            isValid = DataTypeValidator.ValAlarmLimitType(value.VerticalAlarmLimit);
            if (!isValid)
            {
                errorText = "VerticalAlarmLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.ThresholdCourseWidth);
            if (!isValid)
            {
                errorText = "ThresholdCourseWidth";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.LengthOffset);
            if (!isValid)
            {
                errorText = "LengthOffset";
                return false;
            }
            isValid = DataTypeValidator.ValHexType(value.CRCRemainder);
            if (!isValid)
            {
                errorText = "CRCRemainder";
                return false;
            }
            isValid = DataTypeValidator.NoSequenceType(value.OperationType);
            if (!isValid)
            {
                errorText = "OperationType";
                return false;
            }
            isValid = DataTypeValidator.NoSequenceType(value.ServiceProviderSBAS);
            if (!isValid)
            {
                errorText = "ServiceProviderSBAS";
                return false;
            }
            isValid = DataTypeValidator.NoSequenceType(value.ApproachPerformanceDesignator);
            if (!isValid)
            {
                errorText = "ApproachPerformanceDesignator";
                return false;
            }
            isValid = DataTypeValidator.NoSequenceType(value.ReferencePathDataSelector);
            if (!isValid)
            {
                errorText = "ReferencePathDataSelector";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ApproachAltitudeTable value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.Altitude);
            if (!isValid)
            {
                errorText = "Altitude";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ApproachDistanceTable value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.ValueHAT);
            if (!isValid)
            {
                errorText = "ValueHAT";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.Distance);
            if (!isValid)
            {
                errorText = "Distance";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ApproachTimingTable value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValSpeedType(value.Speed);
            if (!isValid)
            {
                errorText = "Speed";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(FinalProfile value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Altitude.Count; i++)
            {
                isValid = IsValid(value.Altitude[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Altitude{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Distance.Count; i++)
            {
                isValid = IsValid(value.Distance[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Distance{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Timing.Count; i++)
            {
                isValid = IsValid(value.Timing[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Timing{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(MissedApproachGroup value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextInstructionType(value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType(value.AlternateClimbInstruction);
            if (!isValid)
            {
                errorText = "AlternateClimbInstruction";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.AlternateClimbAltitude);
            if (!isValid)
            {
                errorText = "AlternateClimbAltitude";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ApproachCondition value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.CodeRNPType(value.RequiredNavigationPerformance);
            if (!isValid)
            {
                errorText = "RequiredNavigationPerformance";
                return false;
            }
            isValid = DataTypeValidator.ValSlopeType(value.ClimbGradient);
            if (!isValid)
            {
                errorText = "ClimbGradient";
                return false;
            }
            isValid = IsValid((Minima)value.MinimumSet, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.AircraftCategory.Count; i++)
            {
                isValid = IsValid(value.AircraftCategory[i], ref errorText);
                if (!isValid)
                {
                    errorText = "AircraftCategory{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.DesignSurface.Count; i++)
            {
                isValid = IsValid(value.DesignSurface[i], ref errorText);
                if (!isValid)
                {
                    errorText = "DesignSurface{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(NavigationAreaSector value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((CircleSector)value.SectorDefinition, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.SignificantObstacle.Count; i++)
            {
                isValid = IsValid(value.SignificantObstacle[i], ref errorText);
                if (!isValid)
                {
                    errorText = "SignificantObstacle{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            isValid = IsValid((Surface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.SectorCriteria.Count; i++)
            {
                isValid = IsValid(value.SectorCriteria[i], ref errorText);
                if (!isValid)
                {
                    errorText = "SectorCriteria{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(DepartureArrivalCondition value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.MinimumEnrouteAltitude);
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
            isValid = IsValid((AircraftCharacteristic)value.EngineType, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SectorDesign value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValSlopeType(value.DesignGradient);
            if (!isValid)
            {
                errorText = "DesignGradient";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.TerminationAltitude);
            if (!isValid)
            {
                errorText = "TerminationAltitude";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ProcedureTransition value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.CodeDesignatedPointDesignatorType(value.TransitionId);
            if (!isValid)
            {
                errorText = "TransitionId";
                return false;
            }
            isValid = DataTypeValidator.TextInstructionType(value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }
            isValid = DataTypeValidator.ValBearingType(value.VectorHeading);
            if (!isValid)
            {
                errorText = "VectorHeading";
                return false;
            }
            isValid = IsValid((Curve)value.Trajectory, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(HoldingUse value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.TextInstructionType(value.Instruction);
            if (!isValid)
            {
                errorText = "Instruction";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.InstructedAltitude);
            if (!isValid)
            {
                errorText = "InstructedAltitude";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(ProcedureTransitionLeg value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoSequenceType(value.SeqNumberARINC);
            if (!isValid)
            {
                errorText = "SeqNumberARINC";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(LandingTakeoffAreaCollection value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(SafeAltitudeAreaSector value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.BufferWidth);
            if (!isValid)
            {
                errorText = "BufferWidth";
                return false;
            }

            for (int i = 0; i < value.SignificantObstacle.Count; i++)
            {
                isValid = IsValid(value.SignificantObstacle[i], ref errorText);
                if (!isValid)
                {
                    errorText = "SignificantObstacle{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            isValid = IsValid((CircleSector)value.SectorDefinition, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(HoldingPatternDuration value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(HoldingPatternDistance value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceType(value.Length);
            if (!isValid)
            {
                errorText = "Length";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(DirectFlight value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        //public static bool IsValid(DirectFlightClass value, ref string errorText)
        //{
        //    if (value == null)
        //        return true;
        //    bool isValid = DataTypeValidator.ValDistanceType(value.ExceedLength);
        //    if (!isValid)
        //    {
        //        errorText = "ExceedLength";
        //        return false;
        //    }
        //    return true;
        //}

        public static bool IsValid(FlightConditionCombination value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Element.Count; i++)
            {
                isValid = IsValid(value.Element[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Element{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(FlightConditionCircumstance value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(FlightRestrictionLevel value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.ValDistanceVerticalType(value.UpperLevel);
            if (!isValid)
            {
                errorText = "UpperLevel";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.LowerLevel);
            if (!isValid)
            {
                errorText = "LowerLevel";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(FlightRestrictionRoute value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.RouteElement.Count; i++)
            {
                isValid = IsValid(value.RouteElement[i], ref errorText);
                if (!isValid)
                {
                    errorText = "RouteElement{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Contact.Count; i++)
            {
                isValid = IsValid(value.Contact[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Contact{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(FlightRoutingElement value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoSequenceType(value.OrderNumber);
            if (!isValid)
            {
                errorText = "OrderNumber";
                return false;
            }
            isValid = DataTypeValidator.ValSpeedType(value.Speed);
            if (!isValid)
            {
                errorText = "Speed";
                return false;
            }

            for (int i = 0; i < value.FlightLevel.Count; i++)
            {
                isValid = IsValid(value.FlightLevel[i], ref errorText);
                if (!isValid)
                {
                    errorText = "FlightLevel{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(FlightConditionElement value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoSequenceType(value.Index);
            if (!isValid)
            {
                errorText = "Index";
                return false;
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RouteAvailability value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Levels.Count; i++)
            {
                isValid = IsValid(value.Levels[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Levels{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(RoutePortion value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AerialRefuellingPoint value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = DataTypeValidator.NoSequenceType(value.Sequence);
            if (!isValid)
            {
                errorText = "Sequence";
                return false;
            }
            return true;
        }

        public static bool IsValid(AerialRefuellingAnchor value, ref string errorText)
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
            isValid = DataTypeValidator.ValSpeedType(value.SpeedLimit);
            if (!isValid)
            {
                errorText = "SpeedLimit";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.LegSeparation);
            if (!isValid)
            {
                errorText = "LegSeparation";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceType(value.LegLength);
            if (!isValid)
            {
                errorText = "LegLength";
                return false;
            }
            isValid = DataTypeValidator.ValDistanceVerticalType(value.RefuellingBaseLevel);
            if (!isValid)
            {
                errorText = "RefuellingBaseLevel";
                return false;
            }
            isValid = IsValid((Surface)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.VerticalExtent.Count; i++)
            {
                isValid = IsValid(value.VerticalExtent[i], ref errorText);
                if (!isValid)
                {
                    errorText = "VerticalExtent{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Point.Count; i++)
            {
                isValid = IsValid(value.Point[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Point{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AerialRefuellingTrack value, ref string errorText)
        {
            if (value == null)
                return true;
            bool isValid = IsValid((Curve)value.Extent, ref errorText);
            if (!isValid)
            {
                return false;
            }

            for (int i = 0; i < value.Point.Count; i++)
            {
                isValid = IsValid(value.Point[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Point{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.VerticalExtent.Count; i++)
            {
                isValid = IsValid(value.VerticalExtent[i], ref errorText);
                if (!isValid)
                {
                    errorText = "VerticalExtent{index: " + i + "} =>" + errorText;
                    return false;
                }
            }

            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
                if (!isValid)
                {
                    errorText = "Annotation{index: " + i + "} =>" + errorText;
                    return false;
                }
            }
            return true;
        }

        public static bool IsValid(AuthorityForAerialRefuelling value, ref string errorText)
        {
            if (value == null)
                return true;

            bool isValid;
            for (int i = 0; i < value.Annotation.Count; i++)
            {
                isValid = IsValid(value.Annotation[i], ref errorText);
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
