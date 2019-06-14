using Aerodrome.Enums;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public class ColourTypeConverter : IConverter<ColourType, AM_Color>
    {
        public AM_Color Convert(ColourType source)
        {
            switch(source)
            {
                case ColourType.YELLOW:
                    return AM_Color.Yellow;
                case ColourType.ORANGE:
                    return AM_Color.Orange;
                case ColourType.BLUE:
                    return AM_Color.Blue;
                case ColourType.WHITE:
                    return AM_Color.White;
                default:
                    return AM_Color.White;
            }
        }
    }
}
