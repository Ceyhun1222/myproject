using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.PANDA.Common
{
    public interface IInterfaceData
    {
        HorizantalDistanceType DistanceUnit { get; set; }
        VerticalDistanceType HeightUnit { get; set; }
        HorizantalSpeedType SpeedUnit { get; set; }

        double DistancePrecision { get; set; }

        double HeightPrecision { get; set; }

        double SpeedPrecision { get; set; }

        double DSpeedPrecision { get; set; }

        double AnglePrecision { get; set; }

        double GradientPrecision { get; set; }
    }
}
