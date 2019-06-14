using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;

namespace ARAN.GeometryClasses
{
	public class Line : Geometry
	{

		public Line()
			: base()
		{
			_refPoint = new Point();
			_vector = new Vector();
		}

		public Line(Point refpoint, Vector refVector)
			: this()
		{
			_refPoint.Assign(refpoint);
			_vector.Assign(refVector);
		}

		public Line(Point refPoint, double direction)
			: this()
		{
			_refPoint.Assign(refPoint);
            _vector.direction = direction;
		}

		public Line(Point fromPoint, Point toPoint)
			: this()
		{
            _refPoint.Assign ( fromPoint );
			_vector = new Vector(new double[] { toPoint.X - fromPoint.X, toPoint.Y - fromPoint.Y });
		}

		public override Object Clone()
		{
			return new Line(_refPoint, _vector);
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

		public override void Assign(PandaItem source)
		{
			Line dst;
			dst = (Line)source;
			dst._refPoint.Assign(_refPoint);
			dst._vector.Assign(_vector);
		}

		public override GeometryType GetGeometryType()
		{
			return GeometryType.GeometrycLine;
		}

		public Point RefPoint
		{
			get { return _refPoint; }
			set { _refPoint.Assign(value); }
		}
		public Vector DirVector
		{
			get { return _vector; }
			set { _vector.Assign(value); }
		}
		private Point _refPoint;
		private Vector _vector;
	}
}
