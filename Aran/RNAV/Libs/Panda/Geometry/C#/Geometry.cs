using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.Contracts.Registry;

namespace ARAN.GeometryClasses
{

	public enum GeometryType
	{
		Null,			// 0
		Point,          // 1
		MultiPoint,     // 2
		Part,			// 3
		Ring,			// 4
		Polyline,		// 5
		Polygon,		// 6
		Vector,			// 7
		GeometrycLine,  // 8
		GeometrycPlane  // 9
	}

	public abstract class Geometry : PandaItem
	{

		public abstract Object Clone();
		public abstract void Pack(int handle);
		public abstract void UnPack(int handle);
		public abstract void Assign(PandaItem source);

		public static Geometry GeometryCreate(int geomType)
		{
			Geometry src = null;
			switch ((GeometryType)geomType)
			{
				case GeometryType.Null:
					src = new NullGeometry();
					break;
				case GeometryType.Point:
					src = new Point();
					break;
				case GeometryType.MultiPoint:
					src = new MultiPoint();
					break;
				case GeometryType.Part:
					src = new Part();
					break;
				case GeometryType.Ring:
					src = new Ring();
					break;
				case GeometryType.Polyline:
					src = new PolyLine();
					break;
				case GeometryType.Polygon:
					src = new Polygon();
					break;
				case GeometryType.Vector:
					src = new Vector();
					break;
				case GeometryType.GeometrycLine:
					src = new Line();
					break;
				case GeometryType.GeometrycPlane:
					//src = new Plane();
					break;
			}
			return src;
		}

		public static void PackGeometry(Int32 handle, Geometry geom)
		{
			Registry_Contract.PutInt32(handle, (Int32)(geom.GetGeometryType()));
			geom.Pack(handle);
		}

		public static Geometry UnpackGeometry(Int32 handle)
		{
			GeometryType geomType = (GeometryType)Registry_Contract.GetInt32(handle);
			Geometry result = null;

			switch (geomType)
			{
				case GeometryType.Null:
					result = new NullGeometry();
					break;
				case GeometryType.Point:
					result = new Point();
					break;
				case GeometryType.MultiPoint:
					result = new MultiPoint();
					break;
				case GeometryType.Part:
					result = new Part();
					break;
				case GeometryType.Ring:
					result = new Ring();
					break;
				case GeometryType.Polyline:
					result = new PolyLine();
					break;
				case GeometryType.Polygon:
					result = new Polygon();
					break;
				case GeometryType.Vector:
					result = new Vector();
					break;
				case GeometryType.GeometrycLine:
					result = new Line();
					break;
				case GeometryType.GeometrycPlane:
					result = new Plane();
					break;
				default:
					AranException.ThrowException(Registry_Contract.rcInvalid);
					break;
			}
			result.UnPack(handle);

			return result;
		}

		public Point AsPoint
		{
			get{return (Point)this;}
		}

		public MultiPoint AsMultiPoint
		{
			get{return (MultiPoint)this;}
		}

		public Part AsPart
		{
			get{ return (Part)this;}
		}

		public Ring AsRing
		{
			get{return (Ring)this;}
		}

		public Polygon AsPolygon
		{
			get { return (Polygon)this; }		
		}

		public PolyLine AsPolyline
		{
			get { return (PolyLine)this; }
		}

		public Vector AsVector
		{
			get { return (Vector)this; }
		}

		public Line AsLine
		{
			get { return (Line)this; }
		}

		public Plane AsPlane
		{
			get { return (Plane)this; }
		}

		public abstract GeometryType GetGeometryType();
	}
}
