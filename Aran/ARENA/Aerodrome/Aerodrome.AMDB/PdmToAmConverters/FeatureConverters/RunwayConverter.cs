using Aerodrome.Features;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public class RunwayConverter : IConverter<Runway, AM_Runway>
    {
        public AM_Runway Convert(Runway source)
        {
            AM_Runway amRwy = new AM_Runway()
            {
                Name = source.Designator,
                idnumber=source.ID
            };
            return amRwy;
        }
    }
}
