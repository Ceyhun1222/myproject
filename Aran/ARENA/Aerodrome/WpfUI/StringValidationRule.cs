using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfUI
{
    public class StringValidationRule : ValidationRule
    {
        /// <summary>
        /// Gets or sets the maximal length of the string
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Checks if the string is valied
        /// </summary>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is null)
                return new ValidationResult(false, "Empty string");
            if (value.ToString().Length == 0)
                return new ValidationResult(false, "Empty string");
            if (value.ToString().Length > Length)
                return new ValidationResult(false, "The string is to long");

            return new ValidationResult(true, null);
        }

    }
}
