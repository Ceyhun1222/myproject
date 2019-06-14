using System.Collections.Generic;
using Aran.Panda.Constants;

namespace Aran.Panda.Conventional.Racetrack
{
	public class Interval
	{
		public Interval(double left, double right)
		{
			Left = left;
			Right = right;
		}

		public double Left { get; set; }

		public double Right { get; set; }

		public int Tag { get; set; }
	}
}