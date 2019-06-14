using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.PANDA.Conventional.Settings
{
	public class ConstClass
	{
		static readonly string [ ] GHorisontalDistanceUnitName = new string [ ] { "m", "km", "NM" };

		static readonly string [ ] GVerticalDistanceUnitName = new string [ ] { "m", "feet", "FL", "SM" };

		static readonly string [ ] GHorisontalSpeedUnitName = new string [ ] { "meter/sec", "km/h", "knot" };

		static readonly string [ ] GVerticalSpeedUnit = new string [ ] { "m/min", "feet/min" };
	}
}
