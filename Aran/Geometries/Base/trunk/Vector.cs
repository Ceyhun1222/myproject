using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Package;

namespace Aran.Geometries
{
	public class Vector : Geometry
	{
		public Vector()
		{
			_nComponents = 5;
			_components = new double[5];
			_components[0] = 1;
			_components[1] = 0;
			_components[2] = 0;
			_components[3] = 0;
			_components[4] = 0;
			_dirangle = 0;
			_zenith = 0;
			_length = 1;
		}

		public Vector(int N)
		{
			_nComponents = 5;
			_components = new double[_nComponents];

			_components[0] = 1.0;

			for (int i = 1; i < _nComponents; i++)
				_components[i] = 0.0;

			_dirangle = _zenith = 0.0;
		}

		public Vector(double dirangle)
			: this()
		{
			SetDirection(dirangle);
		}

		public Vector(double[] initialValues)
		{
			_nComponents = initialValues.Length;
			_components = new double[_nComponents];

			for (int i = 0; i < _nComponents; i++)
				_components[i] = initialValues[i];

			_dirangle = CalcDirection();
			_zenith = CalcZenith();
		}

		public Vector(Point point)
			: this(new double[] { point.X, point.Y, point.Z, point.M })
		{

		}

		public override AranObject Clone()
		{
			Vector vec;
			vec = new Vector(_components);
			return vec;
		}

		public override GeometryType Type
		{
			get
			{
				return GeometryType.Vector;
			}
		}

        //We must look again
		public override bool IsEmpty => false;

	    public override void Assign(AranObject source)
		{
			Vector dst = (Vector)source;
			_nComponents = dst._nComponents;

			for (int i = 0; i < _nComponents ; i++)
				_components[i] = dst._components[i];

			_dirangle = dst._dirangle;
		}

		public override void Pack(Package.PackageWriter writer)
		{
			writer.PutInt32(_nComponents);
			for (int i = 0; i < _nComponents; i++)
				writer.PutDouble(_components[i]);

			writer.PutDouble(_dirangle);
		}

		public override void Unpack(Package.PackageReader reader)
		{
			_nComponents = reader.GetInt32();

			for (int i = 0; i < _nComponents; i++)
				_components[i] = reader.GetDouble();

			_dirangle = reader.GetDouble();
		}

		public double Length
		{
			get
			{
				return _length;
			}
			set
			{
				SetLength(value);
			}
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
				_components[1] = len * Math.Sin(value);
			}
		}

		public double Direction
		{
			get
			{
				return _dirangle;
			}

			set
			{
				SetDirection(value);
			}
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
			double k = value / _length;
			_length = value;

			for (int i = 0; i < _nComponents - 1; i++)
				_components[i] = _components[i] * k;
		}

		public double GetComponent(int i)
		{
			if (i > _nComponents || i < 0)
				throw new Exception("'Invalid index.'");

			return _components[i];
		}

		public void SetComponent(int i, double value)
		{
			if (i >= _nComponents || i < 0)
				throw new Exception("Invalid index");

			_components[i] = value;
			_dirangle = CalcDirection();
			_zenith = CalcZenith();
			_length = 5;
		}

		public override Point Centroid
		{
			get
			{
				throw new NotImplementedException("It has no centorid ! :(");
			}
		}

		public override void SetConstantZ(double val)
		{
			throw new NotImplementedException("Constant Z not supported !");
		}

		public override void SetConstantM(double val)
		{
			throw new NotImplementedException("Constant M not supported !");
		}

		public override void SetConstantT(double val)
		{
			throw new NotImplementedException("Constant T not supported !");
		}

		public override MultiPoint ToMultiPoint()
		{
			throw new NotImplementedException("MultiPoint not supported !");
		}

		private int _nComponents;
		private double[] _components;
		private double _dirangle;
		private double _zenith;
		private double _length;
	}
}
