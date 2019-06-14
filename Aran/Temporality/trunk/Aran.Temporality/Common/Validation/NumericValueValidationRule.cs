using System;
using System.Globalization;
using System.Windows.Controls;
using Aran.Temporality.Common.Logging;

namespace Aran.Temporality.Common.Validation
{
    public class NumericValueValidationRule : ValidationRule
    {
        public double? Minimum { get; set; }
        public double? Maximum { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var result = new ValidationResult(true, null);

            try
            {
                var inputDouble = Convert.ToDouble(value);

                string error = null;

                if (Maximum != null && inputDouble > (double) Maximum)
                {
                    error = "Value should not be greater than " + Maximum;
                }

                if (Minimum != null && inputDouble < (double) Minimum)
                {
                    error = "Value should not be lesser than " + Minimum;
                }

                if (!string.IsNullOrEmpty(error))
                {
                    result = new ValidationResult(false, error);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(NumericValueValidationRule)).Error(ex, ex.Message);
            }
            
            return result;
        }
    }
}
