using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;

namespace ARAN.GeometryClasses
{
	public class Plane : Geometry
	{
		public Plane()
			: base()
		{
			_refPoint = new Point();
			_vector = new Vector();
		}

		public Plane(Point refPoint, Vector refvector)
			: this()
		{
			_refPoint.Assign(refPoint);
			_vector.Assign(refvector);
		}

		public override object Clone()
		{
			return new Plane(_refPoint, _vector);
		}

		public override void Assign(PandaItem source)
		{
			Plane dst = (Plane)source;
			dst._refPoint.Assign(_refPoint);
			dst._vector.Assign(_vector);
		}

		public override void Pack(int handle)
		{
			_refPoint.Pack(handle);
			_vector.Pack(handle);
		}

		public override void UnPack(int handle)
		{
			_refPoint.UnPack(handle);
			_vector.UnPack(handle);
		}

		public Point GetRefPoint()
		{
			return _refPoint;
		}

		public void SetRefPoint(Point val)
		{
			_refPoint.Assign(val);
		}

		public Vector GetRefVector()
		{
			return _vector;
		}

		public void SetNormVector(Vector val)
		{
			_vector.Assign(val);
		}

		public override GeometryType GetGeometryType()
		{
			return GeometryType.GeometrycPlane;
		}

		private Point _refPoint;
		private Vector _vector;


	}
}
