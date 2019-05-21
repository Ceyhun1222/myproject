using System;
using Aran.Package;

namespace Aran.Geometries
{
	public class Point : Geometry
	{
		public Point()
		{
			SetEmpty();
		}

		public Point(double initX, double initY)
		{
			SetEmpty();
			SetCoords(initX, initY);
		}

		public Point(double initX, double initY, double initZ)
		{
			SetCoords(initX, initY, initZ);
		}

		public Point(Point point)
		{
			Assign(point);
		}

		public void SetCoords(double x, double y)
		{
			_x = x;
			_y = y;
		}

		public void SetCoords(double x, double y, double z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public void SetEmpty()
		{
			_x = double.NaN;
			_y = double.NaN;
			_z = double.NaN;
			_t = 0.0;
			_m = 0.0;
		}

		public double X
		{
			get
			{
				return _x;
			}

			set
			{
				_x = value;
			}
		}

		public double Y
		{
			get
			{
				return _y;
			}

			set
			{
				_y = value;
			}
		}

		public double Z
		{
			get
			{
				return _z;
			}

			set
			{
				_z = value;
			}
		}

		public double T
		{
			get
			{
				return _t;
			}

			set
			{
				_t = value;
			}
		}

		public double M
		{
			get
			{
				return _m;
			}

			set
			{
				_m = value;
			}
		}

		public override void SetConstantZ(double val)
		{
			_z = val;
		}

		public override void SetConstantM(double val)
		{
			_m = val;
		}

		public override void SetConstantT(double val)
		{
			_t = val;
		}

		public override MultiPoint ToMultiPoint()
		{
			MultiPoint multiPoint = new MultiPoint();
			multiPoint.Add(this);
			return multiPoint;
		}

		public bool Equals2D(Aran.Geometries.Point other, double tolerance = 0.0001)
		{
			return Math.Abs(X - other.X) < tolerance
				&& Math.Abs(Y - other.Y) < tolerance;
		}

		public override bool Equals(object obj)
		{
			if (obj is Point)
			{
				Point pnt = (Point)obj;
				return (X == pnt.X && Y == pnt.Y && Z == pnt.Z && T == pnt.T && M == pnt.M);
			}
			return base.Equals(obj);
		}

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

		public override bool IsEmpty
		{
			get
			{
				return double.IsNaN(X) || double.IsNaN(Y);
			}
		}

		public override AranObject Clone()
		{
			Point cln = new Point();
			cln.Assign(this);
			return cln;
		}

		public override void Assign(AranObject source)
		{
			Point src = (Point)source;
			_x = src._x;
			_y = src._y;
			_z = src._z;
			_t = src._t;
			_m = src._m;
		}

		public override GeometryType Type
		{
			get { return GeometryType.Point; }
		}

		public override Point Centroid
		{
			get
			{
				return this;
			}
		}

		public override void Pack(PackageWriter writer)
		{
			//writer.PutBool(_isEmpty);
			//if (_isEmpty) return; 

			writer.PutDouble(_x);
			writer.PutDouble(_y);
			writer.PutDouble(_z);
			writer.PutDouble(_m);
			writer.PutDouble(_t);
		}

		public override void Unpack(PackageReader reader)
		{
			_x = reader.GetDouble();
			_y = reader.GetDouble();
			_z = reader.GetDouble();
			_m = reader.GetDouble();
			_t = reader.GetDouble();
		}

        public override Box Extend
        {
            get { return new Box(_x, _y, _x, _y); }
        }

		private double _x;
		private double _y;
		private double _z;
		private double _t;
		private double _m;
	}
}
