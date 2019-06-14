using Aran.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aran.PANDA.RNAV.PBNAPVBaroVNav
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public struct D3DPlane
	{
		public Point Origin;
		public double X;
		public double Y;
		public double Z;
		public double A;
		public double B;
		public double C;
		public double D;
	} // end TD3DPlane

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct D3DPolygone
	{
		public Polygon Poly;
		public D3DPlane Plane;
	} // end TD3DPolygone

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum OFZPlane
	{
		ZeroPlane = 0,
		InnerApproachPlane = 1,

		InnerTransition1lPlane = 2,
		InnerTransition2lPlane = 3,

		BalkedLandingPlane = 4,

		InnerTransition2rPlane = 5,
		InnerTransition1rPlane = 6,
		CommonPlane = 7,
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public enum Plane
	{
		IntermediateApproach = 0,
		FinalApproachSurface = 1,
		HorizontalSurface = 2,
		MissedApproachSurface = 3,
		//IntermediateMissedApproachSurface = 3,
		//FinalMissedApproachSurface = 4,

		cStraightMA = 3,
		cStraightSegment_TIA = 4,
		cMATurnArea = 5
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ReportHeader
	{
		public string Aerodrome;
		public string Procedure;
		public string Category;
		public string Database;
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public struct ReportPoint
	{
		public string Description;
		public double TrueCourse;
		public double turnAngle;
		public double Height;
		public double Radius;
		public double TurnArcLen;
		public double DistToNext;

		public double Lat;
		public double Lon;

		public double CenterLat;
		public double CenterLon;

		public int Turn;
	}

}
