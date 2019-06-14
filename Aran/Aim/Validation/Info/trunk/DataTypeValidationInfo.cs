using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.ValidationInfo
{
    public static partial class DataTypeValidationInfo
    {
        public static bool TextDesignatorType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 16;
            return Character3Type(out pattern);
        }
        public static bool ValBearingType(out double? min, out double? max, out string pattern)
        {
            min = 000;
            max = 360;
            pattern = null;
            return true;
        }
        public static bool ValAngleType(out double? min, out double? max, out string pattern)
        {
            min = -180;
            max = 180;
            pattern = null;
            return true;
        }
        public static bool ValSlopeType(out double? min, out double? max, out string pattern)
        {
            min = -100;
            max = 100;
            pattern = null;
            return true;
        }
        public static bool ValDistanceVerticalType(out double? min, out double? max, out string pattern)
        {
            min = null;
            max = null;
            return ValDistanceVerticalBaseType(out pattern);
        }
        public static bool ValDistanceType(out double? min, out double? max, out string pattern)
        {
            min = 0;
            max = null;
            pattern = null;
            return true;
        }
        public static bool NoNumberType(out double? min, out double? max, out string pattern)
        {
            min = 0;
            max = 18446744073709551615;
            pattern = null;
            return true;
        }
        public static bool TextNameType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 60;
            return Character3Type(out pattern);
        }
        public static bool ValSpeedType(out double? min, out double? max, out string pattern)
        {
            min = 0;
            max = null;
            pattern = null;
            return true;
        }
        public static bool ValFrictionType(out double? min, out double? max, out string pattern)
        {
            min = null;
            max = null;
            return ValFrictionBaseType(out pattern);
        }
        public static bool TimeType(out double? min, out double? max, out string pattern)
        {
            min = null;
            max = null;
            return TimeBaseType(out pattern);
        }
        public static bool ValPercentType(out double? min, out double? max, out string pattern)
        {
            min = 0;
            max = 100;
            pattern = null;
            return true;
        }
        public static bool NoSequenceType(out double? min, out double? max, out string pattern)
        {
            min = 0;
            max = 18446744073709551615;
            pattern = null;
            return true;
        }
        public static bool ValPCNType(out double? min, out double? max, out string pattern)
        {
            min = null;
            max = null;
            return ValPCNBaseType(out pattern);
        }
        public static bool ValWeightType(out double? min, out double? max, out string pattern)
        {
            min = 0;
            max = null;
            pattern = null;
            return true;
        }
        public static bool CodeAirportHeliportDesignatorType(out double? min, out double? max, out string pattern)
        {
            min = 3;
            max = 6;
            return AlphanumericType(out pattern);
        }
        public static bool CodeICAOType(out double? min, out double? max, out string pattern)
        {
            min = 4;
            max = 4;
            return AlphaType(out pattern);
        }
        public static bool CodeIATAType(out double? min, out double? max, out string pattern)
        {
            min = 3;
            max = 3;
            return AlphaType(out pattern);
        }
        public static bool ValMagneticVariationType(out double? min, out double? max, out string pattern)
        {
            min = -180;
            max = 180;
            pattern = null;
            return true;
        }
        public static bool DateYearType(out double? min, out double? max, out string pattern)
        {
            min = null;
            max = null;
            return DateYearBaseType(out pattern);
        }
        public static bool ValMagneticVariationChangeType(out double? min, out double? max, out string pattern)
        {
            min = -180;
            max = 180;
            pattern = null;
            return true;
        }
        public static bool ValFLType(out double? min, out double? max, out string pattern)
        {
            min = 0;
            max = 18446744073709551615;
            pattern = null;
            return true;
        }
        public static bool TextInstructionType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 10000;
            return Character2Type(out pattern);
        }
        public static bool CodeAirspaceDesignatorType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 10;
            return Character3Type(out pattern);
        }
        public static bool TextAddressType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 500;
            return Character2Type(out pattern);
        }
        public static bool TextPhoneType(out double? min, out double? max, out string pattern)
        {
            min = null;
            max = null;
            return TextPhoneBaseType(out pattern);
        }
        public static bool CodeAircraftICAOType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 4;
            return AlphanumericType(out pattern);
        }
        public static bool DateMonthDayType(out double? min, out double? max, out string pattern)
        {
            min = null;
            max = null;
            return DateMonthDayBaseType(out pattern);
        }
        public static bool CodeLanguageType(out double? min, out double? max, out string pattern)
        {
            min = null;
            max = null;
            return CodeLanguageBaseType(out pattern);
        }
        public static bool CodeAuralMorseType(out double? min, out double? max, out string pattern)
        {
            min = null;
            max = null;
            return CodeAuralMorseBaseType(out pattern);
        }
        public static bool CodeNavaidDesignatorType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 4;
            return AlphanumericType(out pattern);
        }
        public static bool CodeSpecialNavigationChainDesignatorType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 4;
            return AlphanumericType(out pattern);
        }
        public static bool CodeDesignatedPointDesignatorType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 5;
            return AlphanumericType(out pattern);
        }
        public static bool TextPropertyNameType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 60;
            return TextPropertyNameBaseType(out pattern);
        }
        public static bool TextNoteType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 10000;
            return Character2Type(out pattern);
        }
        public static bool CodeOrganisationDesignatorType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 12;
            return Character1Type(out pattern);
        }
        public static bool ValAlarmLimitType(out double? min, out double? max, out string pattern)
        {
            min = 0;
            max = 50.8;
            pattern = null;
            return true;
        }
        public static bool ValHexType(out double? min, out double? max, out string pattern)
        {
            min = null;
            max = null;
            return ValHexBaseType(out pattern);
        }
        public static bool CodeRNPType(out double? min, out double? max, out string pattern)
        {
            min = null;
            max = null;
            return CodeRNPBaseType(out pattern);
        }
        public static bool TextSIDSTARDesignatorType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 7;
            return Character1Type(out pattern);
        }
        public static bool CodeFlightRestrictionDesignatorType(out double? min, out double? max, out string pattern)
        {
            min = 1;
            max = 16;
            return Character1Type(out pattern);
        }
    }
}
