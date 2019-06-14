using Aerodrome.Features;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public class RwyDirectionConverter : IConverter<RunwayDirection, AM_RunwayDirection>
    {
        public AM_RunwayDirection Convert(RunwayDirection source)
        {
            AM_RunwayDirection rwyDir = new AM_RunwayDirection()
            {
                idnumber = source.ID
            };
            return rwyDir;
        }
    }
}
