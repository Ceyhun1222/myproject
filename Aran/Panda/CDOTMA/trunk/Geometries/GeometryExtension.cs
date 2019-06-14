using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetTopologySuite.Geometries;
using CDOTMA.CoordinatSystems;
using GeoAPI.Geometries;
//using ProjNet.CoordinateSystems;

namespace CDOTMA.Geometries
{
	public class GeometryExtension//: ICloneable
	{
		public GeometryExtension(Geometry geom)
		{
			geom.UserData = this;
			Geom = geom;

			_name = "";
			SpatialReference = null;
		}

		public Geometry Geom;

		//CoordinatSystem _spatialReference;
		public CoordinatSystem SpatialReference { get; set; }

		string _name;
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public void Project(CoordinatSystem pSpRef)
		{
			Geom.Project(SpatialReference, pSpRef);
			SpatialReference = pSpRef;
		}

		//public object Clone()
		//{
		//    GeometryExtension result = new GeometryExtension(Geom);

		//    result._name = _name;
		//    result.SpatialReference = SpatialReference;

		//    return result;
		//}
	}

	public static class GeomExtensions
	{
		public static GeometryExtension Extended(this Geometry geom)
		{
			return (GeometryExtension)geom.UserData;
		}

		public static string GetName(this Geometry geom)
		{
			if (geom.UserData == null)
				return "";

			return ((GeometryExtension)geom.UserData).Name;
		}

		public static void SetName(this Geometry geom, string val)
		{
			if (geom.UserData == null)
				geom.UserData = new GeometryExtension(geom);

			((GeometryExtension)geom.UserData).Name = val;
		}

		public static CoordinatSystem GetSpatialReference(this Geometry geom)
		{
			if (geom.UserData == null)
				return null;

			return ((GeometryExtension)geom.UserData).SpatialReference;
		}

		public static void SetSpatialReference(this Geometry geom, CoordinatSystem val)
		{
			if (geom.UserData == null)
				geom.UserData = new GeometryExtension(geom);

			((GeometryExtension)geom.UserData).SpatialReference = val;
		}

		public static void Project(this Geometry geom, CoordinatSystem pSpRefFrom, CoordinatSystem pSpRefTo)
		{
			if (pSpRefTo == pSpRefFrom)
				return;

			double resX, resY;

			for (int i = 0; i < geom.Coordinates.Length; i++)
			{
				if (pSpRefTo is ProjectedCoordinatSystem)
					NativeMethods.GeographicToProjection(geom.Coordinates[i].X, geom.Coordinates[i].Y, out resX, out resY);
				else
					NativeMethods.ProjectionToGeographic(geom.Coordinates[i].X, geom.Coordinates[i].Y, out resX, out resY);

				geom.Coordinates[i].X = resX;
				geom.Coordinates[i].Y = resY;
			}
		}

		public static void Project(this Geometry geom, CoordinatSystem pSpRefTo)
		{
			((GeometryExtension)geom.UserData).Project(pSpRefTo);
		}

		public static Geometry CloneGeom(this Geometry geom)
		{
			Geometry result = (Geometry)geom.Clone();
			GeometryExtension geomSrc = (GeometryExtension)geom.UserData;
			GeometryExtension geomDest = new GeometryExtension(result);

			if (geomSrc != null)
			{
				geomDest.SpatialReference = geomSrc.SpatialReference;
				geomDest.Name = geomSrc.Name;
			}

			//result.UserData = geomDest;

			return result;
		}
	}
}
