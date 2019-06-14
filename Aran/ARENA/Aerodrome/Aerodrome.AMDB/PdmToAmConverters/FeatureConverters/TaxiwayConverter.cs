using Aerodrome.Features;
using PDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aerodrome.Converter
{
    public class TaxiwayConverter : IConverter<Taxiway, AM_Taxiway>
    {
        public AM_Taxiway Convert(Taxiway source)
        {
            AM_Taxiway twy = new AM_Taxiway()
            {
                Name = source.Designator,
                idnumber = source.ID
            };
            return twy;
        }
    }
}
