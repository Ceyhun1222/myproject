using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChartValidator
{
	public static class ExternalMagVariation
	{
		[DllImport ("Adacalc.dll", EntryPoint = "MagVar")]
		public static extern double MagVar (double lat, double lon, double alt, int day, int month, int year, int model);

	}
}
