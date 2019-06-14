using System.Text.RegularExpressions;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Validation
{
    public static partial class DataTypeValidator
    {
        public static bool TextDesignatorType (string value)
        {
            bool isValid = String (value, 1, 16);
            if (isValid)
                isValid = Character3Type (value);
            return isValid;
        }

        public static bool ValBearingType (double? value)
        {
            bool isValid = Double (value, 000, 360);
            return isValid;
        }

        public static bool ValAngleType (double? value)
        {
            bool isValid = Double (value, -180, 180);
            return isValid;
        }

        public static bool ValSlopeType (double? value)
        {
            bool isValid = Double (value, -100, 100);
            return isValid;
        }

        public static bool ValDistanceVerticalType (ValDistanceVertical value)
        {
            bool isValid = ValDistanceVerticalBaseType (value.Value);
            return isValid;
        }

        public static bool ValDistanceType (ValDistance value)
        {
            bool isValid = DoubleMin (value.Value, 0);
            return isValid;
        }

        public static bool NoNumberType (double? value)
        {
            bool isValid = Double (value, 0, 18446744073709551615);
            return isValid;
        }

        public static bool TextNameType (string value)
        {
            bool isValid = String (value, 1, 60);
            if (isValid)
                isValid = Character3Type (value);
            return isValid;
        }

        public static bool ValSpeedType (ValSpeed value)
        {
            bool isValid = DoubleMin (value.Value, 0);
            return isValid;
        }

        public static bool ValFrictionType (double? value)
        {
            bool isValid = ValFrictionBaseType (value.Value);
            return isValid;
        }

        public static bool TimeType (string value)
        {
            bool isValid = TimeBaseType (value);
            return isValid;
        }

        public static bool ValPercentType (double? value)
        {
            bool isValid = Double (value, 0, 100);
            return isValid;
        }

        public static bool NoSequenceType (double? value)
        {
            bool isValid = Double (value, 0, 18446744073709551615);
            return isValid;
        }

        public static bool ValPCNType (double? value)
        {
            bool isValid = ValPCNBaseType (value.Value);
            return isValid;
        }

        public static bool ValWeightType (ValWeight value)
        {
            bool isValid = DoubleMin (value.Value, 0);
            return isValid;
        }

        public static bool CodeAirportHeliportDesignatorType (string value)
        {
            bool isValid = String (value, 3, 6);
            if (isValid)
                isValid = AlphanumericType (value);
            return isValid;
        }

        public static bool CodeICAOType (string value)
        {
            bool isValid = String (value, 4, 4);
            if (isValid)
                isValid = AlphaType (value);
            return isValid;
        }

        public static bool CodeIATAType (string value)
        {
            bool isValid = String (value, 3, 3);
            if (isValid)
                isValid = AlphaType (value);
            return isValid;
        }

        public static bool ValMagneticVariationType (double? value)
        {
            bool isValid = Double (value, -180, 180);
            return isValid;
        }

        public static bool DateYearType (string value)
        {
            bool isValid = DateYearBaseType (value);
            return isValid;
        }

        public static bool ValMagneticVariationChangeType (double? value)
        {
            bool isValid = Double (value, -180, 180);
            return isValid;
        }

        public static bool ValFLType (ValFL value)
        {
            bool isValid = Double (value.Value, 0, 18446744073709551615);
            return isValid;
        }

        public static bool TextInstructionType (string value)
        {
            bool isValid = String (value, 1, 10000);
            if (isValid)
                isValid = Character2Type (value);
            return isValid;
        }

        public static bool CodeAirspaceDesignatorType (string value)
        {
            bool isValid = String (value, 1, 10);
            if (isValid)
                isValid = Character3Type (value);
            return isValid;
        }

        public static bool TextAddressType (string value)
        {
            bool isValid = String (value, 1, 500);
            if (isValid)
                isValid = Character2Type (value);
            return isValid;
        }

        public static bool TextPhoneType (string value)
        {
            bool isValid = TextPhoneBaseType (value);
            return isValid;
        }

        public static bool CodeAircraftICAOType (string value)
        {
            bool isValid = String (value, 1, 4);
            if (isValid)
                isValid = AlphanumericType (value);
            return isValid;
        }

        public static bool DateMonthDayType (string value)
        {
            bool isValid = DateMonthDayBaseType (value);
            return isValid;
        }

        public static bool CodeLanguageType (string value)
        {
            bool isValid = CodeLanguageBaseType (value);
            return isValid;
        }

        public static bool CodeAuralMorseType (string value)
        {
            bool isValid = CodeAuralMorseBaseType (value);
            return isValid;
        }

        public static bool CodeNavaidDesignatorType (string value)
        {
            bool isValid = String (value, 1, 4);
            if (isValid)
                isValid = AlphanumericType (value);
            return isValid;
        }

        public static bool CodeSpecialNavigationChainDesignatorType (string value)
        {
            bool isValid = String (value, 1, 4);
            if (isValid)
                isValid = AlphanumericType (value);
            return isValid;
        }

        public static bool CodeDesignatedPointDesignatorType (string value)
        {
            bool isValid = String (value, 1, 5);
            if (isValid)
                isValid = AlphanumericType (value);
            return isValid;
        }

        public static bool TextPropertyNameType (string value)
        {
            bool isValid = TextPropertyNameBaseType (value, 1, 60);
            return isValid;
        }

        public static bool TextNoteType (string value)
        {
            bool isValid = String (value, 1, 10000);
            if (isValid)
                isValid = Character2Type (value);
            return isValid;
        }

        public static bool CodeOrganisationDesignatorType (string value)
        {
            bool isValid = String (value, 1, 12);
            if (isValid)
                isValid = Character1Type (value);
            return isValid;
        }

        public static bool ValAlarmLimitType (double? value)
        {
            bool isValid = Double (value, 0, 50.8);
            return isValid;
        }

        public static bool ValHexType (string value)
        {
            bool isValid = ValHexBaseType (value);
            return isValid;
        }

        public static bool CodeRNPType (double? value)
        {
            bool isValid = CodeRNPBaseType (value.Value);
            return isValid;
        }

        public static bool TextSIDSTARDesignatorType (string value)
        {
            bool isValid = String (value, 1, 7);
            if (isValid)
                isValid = Character1Type (value);
            return isValid;
        }

        public static bool CodeFlightRestrictionDesignatorType (string value)
        {
            bool isValid = String (value, 1, 16);
            if (isValid)
                isValid = Character1Type (value);
            return isValid;
        }
    }
}