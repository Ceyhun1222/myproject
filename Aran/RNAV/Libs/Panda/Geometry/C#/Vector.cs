using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.Contracts.Registry;

namespace ARAN.GeometryClasses
{
	public class Vector : Geometry
	{
		public Vector()
		{
			_nComponents = 5;
			_components = new double[5];
            _components [0] = 1;
            _components [1] = 0;
            _components [2] = 0;
            _components [3] = 0;
            _components [4] = 0;
            _dirangle = 0;
            _zenith = 0;
            _length = 1;
		}

		public Vector(int N)
		{
			_nComponents = 5;
			_components = new double[_nComponents];

			for (int i = 0; i < _nComponents; i++)
			{
				_components[i] = 0.0;
			}
			_dirangle = _zenith = 0.0;
		}

		public Vector(double dirangle)
			: this()
		{
            SetDirection ( dirangle );
		}

		public Vector(double[] initialValues)
		{
			int i;
			_nComponents = initialValues.Length;
			_components = new double[_nComponents];

			for (i = 0; i < _nComponents; i++)
			{
				_components[i] = initialValues[i];
			}
			_dirangle = CalcDirection();
			_zenith = CalcZenith();
		}

		public Vector(Point point)
			: this(new double[] { point.X, point.Y, point.Z, point.M })
		{

		}

		public override Object Clone()
		{
			Vector vec;
			vec = new Vector(_components);
			return vec;
		}

		public override void UnPack(int handle)
		{
			int i;
			_nComponents = Registry_Contract.GetInt32(handle);
			for (i = 0; i < _nComponents - 1; i++)
			{
				_components[i] = Registry_Contract.GetDouble(handle);
			}
			_dirangle = Registry_Contract.GetDouble(handle);
		}

		public override void Assign(PandaItem source)
		{
			int i;
			Vector dst;
			dst = (Vector)source;
			dst._nComponents = _nComponents;

			for (i = 0; i < dst._nComponents - 1; i++)
			{
				dst._components[i] = _components[i];
			}
			dst._dirangle = _dirangle;
		}

		public override GeometryType GetGeometryType()
		{
			return GeometryType.Vector;
		}

		public double Length
		{
			get { return _length; }
			set { SetLength(value); }
		}

		protected double CalcDirection()
		{
			if (_nComponents > 1)
			{
				return Math.Atan2(_components[1], _components[0]);
			}
			else
				return 0.0;
		}

		protected void SetDirection(double value)
		{
			double len;
			_dirangle = value;
			if ((_nComponents > 1) & (!double.IsNaN(_dirangle)))
			{
				len = Math.Sqrt(_components[0] * _components[0] + _components[1] * _components[1]);

				_components[0] = len * Math.Cos(value);
				_components[1] = len * Math.Sin(value); ;
			}
		}

		public double direction
		{
			get { return _dirangle; }
			set { SetDirection(value); }
		}

		protected double CalcZenith()
		{
			if (_nComponents > 2)
			{
				return Math.Acos(_components[2] / Math.Sqrt(_components[0] * _components[0] + _components[1] * _components[1] + _components[2] * _components[2]));
			}
			else
				return double.NaN;
		}

		protected void SetZenith(double value)
		{
			double v1;
			double len;

			_zenith = value;
			if ((_nComponents > 2) & (!double.IsNaN(_zenith)))
			{
				v1 = Math.Sqrt(_components[0] * _components[0] + _components[1] * _components[1]);
				len = Math.Sqrt(v1 * v1 + _components[2] * _components[2]);
				_components[0] = Math.Sin(value) * _components[0];
				_components[1] = Math.Sin(value) * _components[1];
				_components[2] = len * Math.Cos(value);
			}
		}

		protected double CalcLength()
		{
			return 0;
		}

		protected void SetLength(double value)
		{
			int i;
			double k;
			k = value / _length;
			_length = value;
			for (i = 0; i < _nComponents - 1; i++)
			{
				_components[i] = _components[i] * k;
			}
		}

		public double GetComponent(int i)
		{
			if (i > _nComponents)
			{
				throw new Exception("'Invalid index.'");
			}
			return _components[i];
		}

		public void SetComponent(int i, double value)
		{
			if (i >= _nComponents)
			{
				throw new Exception("Invalid index");
			}
			else
			{
				_components[i] = value;
				_dirangle = CalcDirection();
				_zenith = CalcZenith();
				_length = 5;

			}
		}

		public override void Pack(int handle)
		{
			int i;
			Registry_Contract.PutInt32(handle, _nComponents);
			for (i = 0; i < _nComponents - 1; i++)
			{
				Registry_Contract.PutDouble(handle, _components[i]);
			}
			Registry_Contract.PutDouble(handle, _dirangle);
		}

		private int _nComponents;
		private double[] _components;
		private double _dirangle;
		private double _zenith;
		private double _length;
	}
}
