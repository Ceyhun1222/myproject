using Aran.Aim.BusinessRules.SbvrParser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace BRuleManager.Converters
{
    class TaggedStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TaggedKey tgKey)
            {
                if ("Color".Equals(parameter))
                {
                    Color color = new Color();

                    switch (tgKey)
                    {
                        case TaggedKey.Keyword:
                            color = Color.FromRgb(255, 153, 0);
                            break;
                        case TaggedKey.Noun:
                            color = Color.FromRgb(0, 128, 128);
                            break;
                        case TaggedKey.Verb:
                            color = Color.FromRgb(0, 0, 255);
                            break;
                        case TaggedKey.Name:
                            color = Color.FromRgb(51, 153, 102);
                            break;
                        default:
                            color = Color.FromRgb(0, 0, 0);
                            break;
                    }

                    return new SolidColorBrush(color);
                }
                else if ("FontWeight".Equals(parameter))
                {
                    if (tgKey == TaggedKey.Noun)
                        return FontWeights.Bold;
                }
                else if ("TextDecorations".Equals(parameter))
                {
                    if (tgKey == TaggedKey.Noun)
                        return TextDecorations.Underline;
                }
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
