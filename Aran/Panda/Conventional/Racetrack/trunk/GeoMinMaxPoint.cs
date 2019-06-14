using Aran.Geometries;

namespace Aran.PANDA.Conventional.Racetrack
{
	public class GeoMinMaxPoint
	{
		public void AddMinPoint ( Point minPoint )
		{
			XMin = minPoint.X;
			YMin = minPoint.Y;
		}

		public void AddMaxPoint ( Point maxPoint )
		{
			XMax = maxPoint.X;
			YMax = maxPoint.Y;
		}

		public void AddMinMaxPoint ( Point minPoint, Point maxPoint )
		{
			XMin = minPoint.X;
			YMin = minPoint.Y;
			XMax = maxPoint.X;
			YMax = maxPoint.Y;
		}

		private double XMin
		{
			get;
			set;
		}

		private double YMin
		{
			get;
			set;
		}

		private double XMax
		{
			get;
			set;
		}

		private double YMax
		{
			get;
			set;
		}
	}
}
