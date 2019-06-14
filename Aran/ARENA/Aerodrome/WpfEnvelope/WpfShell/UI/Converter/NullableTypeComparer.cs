using Aerodrome.Features;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEnvelope.WpfShell.UI.Converter
{
    public class NullableTypeSorter : IComparer
    {
        string _columnHeader;
        bool _direction;
        public NullableTypeSorter(string columnHeader, bool direction)
        {
            _columnHeader = columnHeader;
            _direction = direction;
        }

        public int Compare(object x, object y)
        {
            var currentProperty = x.GetType().GetProperty(_columnHeader);

            var firstValue = currentProperty.GetValue(x);
            var secondValue = currentProperty.GetValue(y);

            var nilFirstValue = currentProperty.PropertyType.GetProperty(nameof(AM_Nullable<Type>.NilReason)).GetValue(firstValue);
            var nilSecondValue = currentProperty.PropertyType.GetProperty(nameof(AM_Nullable<Type>.NilReason)).GetValue(secondValue);

            if (nilFirstValue != null && nilSecondValue != null)
            {
                return (_direction ? 1 : -1) * nilFirstValue.ToString().CompareTo(nilSecondValue.ToString());
            }

            if (nilFirstValue != null)
                return (_direction ? 1 : -1) * 1;
            if (nilSecondValue != null)
                return (_direction ? 1 : -1) * -1;

            if (firstValue.ToString() is null && secondValue.ToString() is null)
                return 0;
            if (firstValue.ToString() is null)
                return (_direction ? 1 : -1) * 1;
            if (secondValue.ToString() is null)
                return (_direction ? 1 : -1) * -1;

            return (_direction ? 1 : -1) * firstValue.ToString().CompareTo(secondValue.ToString());
        }
    }
}
