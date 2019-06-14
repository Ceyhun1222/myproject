using System.Text.RegularExpressions;

namespace Aran.Aim.Validation
{
	partial class DataTypeValidator
	{
        private static bool String (string value, int minLength, int maxLength)
        {
            if (value.Length >= minLength && value.Length <= maxLength)
                return true;
            return false;
        }

        private static bool String (string value, string pattern)
        {
            int i;
            foreach (char a in pattern)
            {

                if (a == '*')
                {
                    i = pattern.IndexOf (a);
                    if (pattern [i - 1] != '\\')
                    {
                        pattern.Remove (i, 1);
                    }
                }

            }
            Match m = Regex.Match (value, pattern);
            return m.Success;
        }

        private static bool ValFrictionBaseType (double? value)
        {
            Regex objectForMAtching = new Regex ("^(0\\.[0-9]{2})$");

            return objectForMAtching.IsMatch (value.ToString ());
        }

        private static bool TimeBaseType (string value)
        {
            Regex objectForMAtching = new Regex ("(^(([0-1]{1}[0-9]{1})|(2{1}[0-3]{1})):[0-5]{1}[0-9]{1}$)|^(24:00)$");

            return objectForMAtching.IsMatch (value);
        }

        private static bool ValPCNBaseType (double? value)
        {
            Regex objectForMAtching = new Regex ("^[0-9]{1,3}(\\.[0-9]){0,1}$");

            return objectForMAtching.IsMatch (value.ToString ());
        }

        private static bool DateYearBaseType (string value)
        {
            Regex objectForMAtching = new Regex ("^[1-9]{1}[0-9]{3}$");

            return objectForMAtching.IsMatch (value);
        }

        private static bool DateMonthDayBaseType (string value)
        {
            Regex objectForMAtching = new Regex ("(^(0[1-9]|1[0-9]|2[0-9]){1}\\-(0[1-9]|1[0-2]){1}$)|(^((30{1})\\-(02|04|06|09|11){1})$)|(^((31{1})\\-(01|03|05|07|08|10|12){1})$)");

            return objectForMAtching.IsMatch (value);
        }

        private static bool CodeLanguageBaseType (string value)
        {
            Regex objectForMAtching = new Regex ("^[a-z]{3}$");

            return objectForMAtching.IsMatch (value);
        }

        private static bool CodeAuralMorseBaseType (string value)
        {
            Regex objectForMAtching = new Regex ("[^\\-\\.]");

            return !objectForMAtching.IsMatch (value);
        }

        private static bool TextPhoneBaseType (string value)
        {
            Regex objectForMAtching = new Regex ("^((\\+\\(\\d{1,4}\\)){0,1}([0-9]{1,}[\\s|\\-]{0,1}[0-9]{1,}){1,})$");

            return objectForMAtching.IsMatch (value);
        }

        private static bool ValHexBaseType (string value)
        {
            Regex objectForMAtching = new Regex ("^([A-F]|[0-9]){8}$");

            return objectForMAtching.IsMatch (value);
        }

        private static bool CodeRNPBaseType (double? value)
        {
            Regex objectForMAtching = new Regex ("^[0-9]{1,2}(\\.[0-9]{1}){0,1}$");

            return objectForMAtching.IsMatch (value.ToString ());
        }

        private static bool ValDistanceVerticalBaseType (double? value)
        {
            Regex objectForMAtching = new Regex ("(^(\\+|\\-){0,1}[0-9]{1,8}(\\.[0-9]{1,4}){0,1}$)|^(UNL|GND|FLOOR|CEILING)$");

            return objectForMAtching.IsMatch (value.ToString ());
        }


        private static bool Character1Type (string value)
        {
            Regex objectForMAtching = new Regex ("[^A-Z0-9\\-\\+]");

            return !objectForMAtching.IsMatch (value);
        }

        private static bool Character2Type (string value)
        {
            return true;
        }

        private static bool Character3Type (string value)
        {
            Regex objectForMAtching = new Regex ("[^A-Z0-9,!\"&#\\$%'\\(\\)\\*\\+\\-\\./:;<=>\\?@\\[\\\\\\]\\^_\\|\\{\\}]");

            return !objectForMAtching.IsMatch (value);
        }

        private static bool AlphaType (string value)
        {
            Regex objectForMAtching = new Regex ("[^A-Z]");

            return !objectForMAtching.IsMatch (value);
        }

        private static bool AlphanumericType (string value)
        {
            Regex objectForMAtching = new Regex ("[^A-Z{1,}0-9]");

            return !objectForMAtching.IsMatch (value);
        }

        private static bool TextPropertyNameBaseType (string value, int minValue, int maxValue)
        {
            if (value.Length >= minValue && value.Length <= maxValue)
                return true;
            else
                return false;
        }

        private static bool DoubleMin (double? value, double? minValue)
        {
            if (value >= minValue)
                return true;
            else
                return false;
        }

        private static bool DoubleMax (double? value, double? maxValue)
        {
            if (value <= maxValue)
                return true;
            else
                return false;
        }

        private static bool Double (double? value, double? minValue, double? maxValue)
        {
            if (value >= minValue && value <= maxValue)
                return true;
            else
                return false;
        }

        private static bool UintMin (uint value, uint minValue)
        {
            if (value >= minValue)
                return true;
            else
                return false;
        }

        private static bool UintMax (uint value, uint maxValue)
        {
            if (value <= maxValue)
                return true;
            else
                return false;
        }

        private static bool Uint (uint value, uint minValue, uint maxValue)
        {
            if (value >= minValue && value <= maxValue)
                return true;
            else
                return false;
        }
	}
}
