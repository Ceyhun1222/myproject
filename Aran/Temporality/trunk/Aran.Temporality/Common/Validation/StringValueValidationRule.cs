using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using Aran.Temporality.Common.Logging;
using Fare;

namespace Aran.Temporality.Common.Validation
{
    public class StringValueValidationRule : ValidationRule
    {
        private Regex _regex;

        public string Example { get; set; }

        private string _pattern;
        public string Pattern
        {
            get { return _pattern; }
            set
            {
                _pattern = value;
                if (Pattern==null) return;

                _regex=new Regex(Pattern,RegexOptions.Singleline);
                

               
                
               
            }
        }

        public int MinimumLength { get; set; } = -1;
        public int MaximumLength { get; set; } = -1;

        private readonly Random _random=new Random();
        readonly Regex _digitalRegex = new Regex(Regex.Escape("[0-9]"));

        private void GenerateExample()
        {
            try
            {
                var xeger = new Xeger(Pattern);
                int i = 0;
               while(i++<100)
               {
                   var exampleVariant=xeger.Generate();

                   while (exampleVariant.IndexOf("[0-9]", StringComparison.Ordinal)>-1)
                   {
                      var randomDigit = ((char)('0' + _random.Next(9))).ToString();
                      exampleVariant = _digitalRegex.Replace(exampleVariant, randomDigit, 1);
                   }
                 
                   if (MinimumLength>-1)
                   {
                       if (exampleVariant.Length<MinimumLength)
                       {
                           continue;
                       }
                   }

                   if (MaximumLength > -1)
                   {
                       if (exampleVariant.Length > MaximumLength)
                       {
                           exampleVariant = exampleVariant.Substring(0, MaximumLength);

                           if (!MatchPattern(exampleVariant)) continue;
                       }
                   }


                   Example = exampleVariant;
                   break;
               }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(StringValueValidationRule)).Error(ex, ex.Message);
            }
        }
         
        private bool MatchPattern(string inputString)
        {
            return string.IsNullOrEmpty(_regex.Replace(inputString, ""));
        }

        public override ValidationResult Validate(object value,CultureInfo cultureInfo)
        {
            var result = new ValidationResult(true, null);
            var inputString = (value ?? string.Empty).ToString();

            string error = null;

            if (MaximumLength > 0 && inputString.Length > MaximumLength)
            {
                error = "Value should contain no more than " + MaximumLength+" symbols";
            }

            if (MinimumLength > 0 && inputString.Length < MinimumLength)
            {
                error = "Value should contain at least " + MinimumLength + " symbols";
            }

            if (Pattern != null && !MatchPattern(inputString))
            {
                GenerateExample();

                if (error==null)
                {
                    error = "Value should be of specific pattern " + Pattern;
                    
                    if (Example!=null) error+=", for example " + Example;
                }
                else
                {
                    error += " of specific pattern " + Pattern;
                    if (Example != null) error += ", for example " + Example;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                result = new ValidationResult(false, error);
            }

            return result;
        }
    }
}
