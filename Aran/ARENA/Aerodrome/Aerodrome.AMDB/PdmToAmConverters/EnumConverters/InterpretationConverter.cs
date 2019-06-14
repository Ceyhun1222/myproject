using Aerodrome.Enums;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{ 
    public class InterpretationConverter : IConverter<dataInterpretation, AM_InterpretationType>
    {
        public AM_InterpretationType Convert(dataInterpretation source)
        {
            switch (source)
            {
                case dataInterpretation.BASELINE:
                    return AM_InterpretationType.Baseline;
            case dataInterpretation.PERMDELTA:
                return AM_InterpretationType.PERMDELTA;
            case dataInterpretation.SNAPSHOT:
                return AM_InterpretationType.Snapshot;
            case dataInterpretation.TEMPDELTA:
                return AM_InterpretationType.TEMPDELTA;

            default:
                    return AM_InterpretationType.Baseline;
            }
        }
    }
}

