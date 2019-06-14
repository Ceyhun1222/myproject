using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.Panda.RadarMA
{
    public enum RadarPointChoiceType
    {
        AirportHeliport,
        Navaid
    }

    public class RadarPointChoice
    {
        public RadarPointChoiceType PointChoiceType { get; set; }
        public string Name { get; set; }
    }
}
