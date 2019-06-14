using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.Contracts.Registry;

namespace ARAN.GeometryClasses
{

	public class Point : Geometry
	{
		public Point()
			: base()
		{
			SetEmpty();
		}

		public Point(double initX, double initY)
			: base()
		{
			SetEmpty();
			_x = initX;
			_y = initY;
		}

		public Point(double initX, double initY, double initZ)
			: base()
		{
			SetEmpty();
			_x = initX;
			_y = initY;
			_z = initZ;
		}
		public Point(Point point)
			: base()
		{
			Assign(point);
		}


		public void SetCoords(double x, double y)
		{
			_isEmpty = false;
			_x = x;
			_y = y;
		}

		public void SetCoords(double x, double y, double z)
		{
			_isEmpty = false;
			_x = x;
			_y = y;
			_z = z;
		}

		public void SetEmpty()
		{
			_isEmpty = true;
			_x = Double.NaN;
			_y = Double.NaN;
			_z = 0.0;
			_t = 0.0;
			_m = 0.0;
		}

		public double X
		{
			get { return _x; }
			set
			{
				_x = value;
				_isEmpty = false;
			}
			//_isEmpty = false;

		}

		public double Y
		{
			get { return _y; }
			set
			{
				_y = value;
				_isEmpty = false;
			}
		}

		public double Z
		{
			get { return _z; }
			//_isEmpty = false;
			set
			{
				_z = value;
				_isEmpty = false;
			}
		}

		public double T
		{
			get
			{
				return _t;
				//_isEmpty = false;
			}
			set
			{
				_t = value;
				_isEmpty = false;
			}
		}

		public double M
		{
			get { return _m; }//_isEmpty = false;
			set
			{
				_m = value;
				_isEmpty = false;
			}
		}

		public bool IsEmpty()
		{
			return _isEmpty;
		}

		public override Object Clone()
		{
			Point cln = new Point();
			cln.Assign(this);
			return cln;
		}

		public override void Assign(PandaItem source)
		{
			Point src = (Point)source;
			_isEmpty = src._isEmpty;
			_x = src._x;
			_y = src._y;
			_z = src._z;
			_t = src._t;
			_m = src._m;
		}

		public override void Pack(int handle)
		{
			Registry_Contract.PutDouble(handle, _x);
			Registry_Contract.PutDouble(handle, _y);
			Registry_Contract.PutDouble(handle, _z);
			Registry_Contract.PutDouble(handle, _m);
			Registry_Contract.PutDouble(handle, _t);
		}

		public override void UnPack(int handle)
		{
			_x = Registry_Contract.GetDouble(handle);
			_y = Registry_Contract.GetDouble(handle);
			_z = Registry_Contract.GetDouble(handle);
			_m = Registry_Contract.GetDouble(handle);
			_t = Registry_Contract.GetDouble(handle);
		}

		public override GeometryType GetGeometryType()
		{
			return GeometryType.Point;
		}

		private double _x = 0;
		private double _y = 0;
		private double _z = 0;
		private double _t = 0;
		private double _m = 0;
		private bool _isEmpty = true;
	}
}
