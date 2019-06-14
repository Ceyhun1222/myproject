using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aran.Aim.ValidationInfo
{
    public static class CommonValidation
    {
        public static bool CheckValue (int aimTypeIndex, int propertyIndex,
            object value, out CommonValidationErrorType errorType)
        {
            AimObjectType aot = AimMetadata.GetAimObjectType (aimTypeIndex);
            double? min = null;
            double? max = null;
            string pattern = null;

            if (aot == AimObjectType.Feature)
            {
                ValidationInfo.GetFeatureMinMax ((FeatureType) aimTypeIndex, propertyIndex,
                    out min, out max, out pattern);
            }
            else if (aot == AimObjectType.Object)
            {
                ObjectValidationInfo.GetObjectMinMax ((ObjectType) aimTypeIndex, propertyIndex,
                    out min, out max, out pattern);
            }

            if (value is string)
            {
                string strValue = value.ToString ();
                if (min != null && strValue.Length < min)
                {
                    errorType = CommonValidationErrorType.Min;
                    return false;
                }
                if (max != null && strValue.Length > max)
                {
                    errorType = CommonValidationErrorType.Max;
                    return false;
                }
                if (pattern != null)
                {
                    Regex regex = new Regex (pattern);
                    if (regex.IsMatch (strValue))
                    {
                        errorType = CommonValidationErrorType.Pattern;
                        return false;
                    }
                }
            }

            errorType = 0;
            return true;
        }

        private static bool String (string value, int minLength, int maxLength)
        {
            if (value.Length >= minLength && value.Length <= maxLength)
                return true;
            return false;
        }
    }

    public enum CommonValidationErrorType { Min, Max, Pattern }
}
