using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim.PropertyPrecision
{
    public class PrecisionFormat
    {
        public int IntegerPart { get; set; }
        public int FractionalPart { get; set; }

        public PrecisionFormat(int format)
        {
            IntegerPart = (format / 18) - 1;
            FractionalPart = (format % 18) -1;
        }
    }
}
