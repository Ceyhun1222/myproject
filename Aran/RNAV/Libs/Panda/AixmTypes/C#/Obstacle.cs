using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.Contracts.Registry;
using ARAN.GeometryClasses;

namespace ARAN.AIXMTypes
{
	public class Obstacle : AIXM
	{
		public Obstacle()
			: base(AIXMType.Obstacle)
		{
			ptGeo = new Point();
			ptPrj = new Point();
			_elevation = 0;
			_height = 0;
		}

		public Point GetGeo()
		{
			return ptGeo;
		}

		public Point GetPrj()
		{
			return ptPrj;
		}

		public double GetGeoAccuracy()
		{
			return _geoAccuracy;
		}

		public void SetGeAccuracy(double value)
		{
			_geoAccuracy = value;
		}

		public double GetElevation()
		{
			return _elevation;
		}

		public void SetElevation(double value)
		{
			_elevation = value;
		}

		public double GetElevationWithAccuracy()
		{
			return _elevationWithAccuracy;
		}

		public void SetElevationWithAccuracy(double value)
		{
			_elevationWithAccuracy = value;
		}

		public double Getheight()
		{
			return _height;
		}

		public void SetHeight(double value)
		{
			_height = value;
		}

		public override void Assign(PandaItem source)
		{
			base.Assign((AIXM)source);
			Obstacle src;
			src = ((AIXM)source).AsObstacle();
			ptGeo.Assign(src.ptGeo);
			ptPrj.Assign(src.ptPrj);
			_geoAccuracy = src._geoAccuracy;
			_elevation = src._elevation;
			_elevationWithAccuracy = src._elevationWithAccuracy;
			_height = src._height;

		}

		public override Object Clone()
		{
			Obstacle cln = new Obstacle();
			cln.Assign(this);
			return cln;
		}

		public override void Pack(int handle)
		{
			base.Pack(handle);
			ptGeo.Pack(handle);
			ptPrj.Pack(handle);
			Registry_Contract.PutDouble(handle, _geoAccuracy);
			Registry_Contract.PutDouble(handle, _elevation);
			Registry_Contract.PutDouble(handle, _elevationWithAccuracy);
			Registry_Contract.PutDouble(handle, _height);
		}

		public override void UnPack(int handle)
		{
			base.UnPack(handle);
			ptGeo.UnPack(handle);
			ptPrj.UnPack(handle);
			_geoAccuracy = Registry_Contract.GetDouble(handle);
			_elevation = Registry_Contract.GetDouble(handle);
			_elevationWithAccuracy = Registry_Contract.GetDouble(handle);
			_height = Registry_Contract.GetDouble(handle);
		}

        private double GetElevationAccuracy()
        {
            return _elevationWithAccuracy - _elevation;
        }

		private Point ptGeo;
		private Point ptPrj;
		private double _elevation, _geoAccuracy, _elevationWithAccuracy, _height;
	}
}
