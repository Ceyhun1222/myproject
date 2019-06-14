using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geometry;

namespace EnrouteIntersect.Model
{
	internal class RouteSegment
	{
		public int Id
		{
			get; set;
		}

		public string Route
		{
			get; set;
		}

		public IPolyline Shape
		{
			get; set;
		}

		public string Designator
		{
			get; set;
		}

		public bool Modified
		{
			get;
			set;
		}
	}
}
