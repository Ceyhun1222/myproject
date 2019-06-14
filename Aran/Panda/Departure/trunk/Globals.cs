using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using ESRI.ArcGIS.Framework;

namespace Aran.PANDA.Departure
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public static class TopoOperExtension
	{
		public static IGeometry Union(this ITopologicalOperator2 thisValue, IPointCollection other)
		{
			return thisValue.Union(other as IGeometry);
		}

		public static void Cut(this ITopologicalOperator2 thisValue, IPolyline cutter, out IPointCollection leftGeom, out IPointCollection rightGeom)
		{
			IGeometry leftG, rightG;
			thisValue.Cut(cutter, out leftG, out rightG);
			leftGeom = leftG as IPointCollection;
			rightGeom = rightG as IPointCollection;
		}

		public static IGeometry Intersect(this ITopologicalOperator2 thisValue, IPointCollection otherGeometry, esriGeometryDimension resultDimension)
		{
			return thisValue.Intersect(otherGeometry as IGeometry, resultDimension);
		}

		public static IGeometry Difference(this ITopologicalOperator2 thisValue, IPointCollection other)
		{
			return thisValue.Difference(other as IGeometry);
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class CommandBarsExtension
	{
		public static ICommandItem FindById(this ICommandBars thisValue, string uidText)
		{
			ESRI.ArcGIS.esriSystem.UID uid = new ESRI.ArcGIS.esriSystem.UID();
			uid.Value = "{" + uidText + "}";
			return thisValue.Find(uid);
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class PointCollectionExtension
	{
		public static void AddFirstPoint(this IPointCollection thisValue, IPoint point)
		{
			thisValue.AddPoint(point, 0);
		}
	}

	[System.Runtime.InteropServices.ComVisible(false)]
	public static class DepartureGuids
	{
		public const string CircleVTool = "1A118710-05FF-4b50-902B-172D5E715251";
		public const string CLTool = "1A118711-05FF-4b50-902B-172D5E715251";
		public const string StraightVTool = "1A118712-05FF-4b50-902B-172D5E715251";
		public const string TurnAreaVTool = "1A118713-05FF-4b50-902B-172D5E715251";
		public const string SecondaryVTool = "1A118714-05FF-4b50-902B-172D5E715251";
		public const string NomTrackVTool = "1A118715-05FF-4b50-902B-172D5E715251";
		public const string KKVTool = "1A118716-05FF-4b50-902B-172D5E715251";
		public const string FIXVTool = "1A118717-05FF-4b50-902B-172D5E715251";
	}
}
