using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Aran.Geometries
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
			_vector.Direction = direction;
		}

		public Line(Point fromPoint, Point toPoint)
			: this()
		{
			_refPoint.Assign(fromPoint);
			_vector = new Vector(new double[] { toPoint.X - fromPoint.X, toPoint.Y - fromPoint.Y });
		}

		//public override Object Clone ( )
		//{
		//    return new Line ( _refPoint, _vector );
		//}

		public Line(Line other)
			: this()
		{
			_refPoint.Assign(other._refPoint);
			_vector.Assign(other._vector);
		}

		public override GeometryType Type
		{
			get { return Geometries.GeometryType.Line; }
		}

		public override bool IsEmpty
		{
			get
			{
				return (_refPoint == null) || (_vector == null) || _refPoint.IsEmpty ||  _vector.IsEmpty;
			}
		}

		public override void Assign(AranObject source)
		{
			Line dst;
			dst = (Line)source;
			dst._refPoint.Assign(_refPoint);
			dst._vector.Assign(_vector);
		}

		public override void Pack(Package.PackageWriter writer)
		{
			_refPoint.Pack(writer);
			_vector.Pack(writer);
		}

		public override void Unpack(Package.PackageReader reader)
		{
			_refPoint.Unpack(reader);
			_vector.Unpack(reader);
		}

		public override AranObject Clone()
		{
			return new Line(this);
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

		public override Point Centroid
		{
			get
			{
				throw new NotImplementedException("It has no centorid ! :(");
			}
		}

		public override MultiPoint ToMultiPoint()
		{
			MultiPoint result = new MultiPoint();
			result.Add(_refPoint);
			return result;
		}

		public override void SetConstantZ(double val)
		{
			_refPoint.Z = val;
		}

		public override void SetConstantM(double val)
		{
			_refPoint.M = val;
		}

		public override void SetConstantT(double val)
		{
			_refPoint.T = val;
		}

		private Point _refPoint;
		private Vector _vector;
	}
}
