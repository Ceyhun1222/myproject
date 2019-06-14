using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.ValidationInfo
{
    partial class DataTypeValidationInfo
    {
        #region private

        private static bool AlphanumericType(out string pattern)
        {
            pattern = "[^A-Z{1,}0-9]";
            return true;
        }

        private static bool Character2Type(out string pattern)
        {
            pattern = null;
            return true;
        }
        private static bool Character3Type(out string pattern)
        {
            pattern = "[^A-Z0-9,!\" &#\\$%'\\(\\)\\*\\+\\-\\./:;<=>\\?@\\[\\\\\\]\\^_\\|\\{\\}]";
            return true;
        }

        private static bool ValFrictionBaseType(out string pattern)
        {
            pattern = "^(0\\.[0-9]{2})$";
            return true;
        }

        private static bool TimeBaseType(out string pattern)
        {
            pattern = "(^(([0-1]{1}[0-9]{1})|(2{1}[0-3]{1})):[0-5]{1}[0-9]{1}$)|^(24:00)$";
            return true;
        }

        private static bool ValPCNBaseType(out string pattern)
        {
            pattern = "^[0-9]{1,3}(\\.[0-9]){0,1}$";
            return true;
        }

        private static bool DateYearBaseType(out string pattern)
        {
            pattern = "^[1-9]{1}[0-9]{3}$";
            return true;
        }

        private static bool DateMonthDayBaseType(out string pattern)
        {
            pattern = "(^(0[1-9]|1[0-9]|2[0-9]){1}\\-(0[1-9]|1[0-2]){1}$)|(^((30{1})\\-(02|04|06|09|11){1})$)|(^((31{1})\\-(01|03|05|07|08|10|12){1})$)";
            return true;
        }

        private static bool CodeLanguageBaseType(out string pattern)
        {
            pattern = "^[a-z]{3}$";
            return true;
        }

        private static bool CodeAuralMorseBaseType(out string pattern)
        {
            pattern = "[^\\-\\.]";
            return true;
        }

        private static bool TextPhoneBaseType(out string pattern)
        {
            pattern = "^((\\+\\(\\d{1,4}\\)){0,1}([0-9]{1,}[\\s|\\-]{0,1}[0-9]{1,}){1,})$";
            return true;
        }

        private static bool ValHexBaseType(out string pattern)
        {
            pattern = "^([A-F]|[0-9]){8}$";
            return true;
        }

        private static bool CodeRNPBaseType(out string pattern)
        {
            pattern = "^[0-9]{1,2}(\\.[0-9]{1}){0,1}$";
            return true;
        }

        private static bool TextPropertyNameBaseType(out string pattern)
        {
            pattern = "^[a-z]{1,}([A-Z]{1}[a-z]{1,}){1,}$";
            return true;
        }
        private static bool ValDistanceVerticalBaseType(out string pattern)
        {
            pattern = "(^(\\+|\\-){0,1}[0-9]{1,8}(\\.[0-9]{1,4}){0,1}$)|^(UNL|GND|FLOOR|CEILING)$";
            return true;
        }

        private static bool Character1Type(out string pattern)
        {
            pattern = "[^A-Z0-9\\-\\+]";
            return true;
        }

        private static bool AlphaType(out string pattern)
        {
            pattern = "[^A-Z]";
            return true;
        }


        #endregion
    }
}
