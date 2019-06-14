
using Aerodrome.Enums;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public class DirectionTypeConverter : IConverter<CodeDirectionType, AM_Direction>
    {
        
        public AM_Direction Convert(CodeDirectionType source)
        {
            switch (source)
            {
                case CodeDirectionType.BOTH:
                    return AM_Direction.Bidirectional;
                case CodeDirectionType.FORWARD:
                    return AM_Direction.Start_To_EndPoint;
                case CodeDirectionType.BACKWARD:
                    return AM_Direction.End_To_StartPoint;
                default:
                    return AM_Direction.Bidirectional;
            }
        }
    }
}

