using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.GeometryClasses;
using ARAN.Contracts.Registry;

namespace ARAN.AIXMTypes
{
	public class RwyDirection : AIXM
	{
		public RwyDirection()
			: base(AIXMType.RwyDirection)
		{
			ptGeo = new Point();
			ptPrj = new Point();
			_trueBearing = 0;
			_direction = 0;
			_magBearing = 0;
			_elevTdz = 0;
			_elevTdzAccuracy = 0;
			_rwyId = 0;
			_clearWay = 0;
			_displacment = 0;
		}

		public override void Assign(PandaItem source)
		{
			base.Assign((AIXM)source);
			RwyDirection src = ((AIXM)source).AsRwyDirection();
			ptGeo.Assign(src.ptGeo);
			ptPrj.Assign(src.ptPrj);
			_trueBearing = src._trueBearing;
			_direction = src._direction;
			_magBearing = src._magBearing;
			_elevTdz = src._elevTdz;
			_elevTdzAccuracy = src._elevTdzAccuracy;
			_rwyId = src._rwyId;
			_clearWay = src._clearWay;
			_displacment = src._displacment;
		}

		public override object Clone()
		{
			RwyDirection src = new RwyDirection();
			src.Assign(this);
			return src;
		}

		public Point GetGeo()
		{
			return ptGeo;
		}

		public Point GetPrj()
		{
			return ptPrj;
		}


		public override void Pack(int handle)
		{
			base.Pack(handle);
			ptGeo.Pack(handle);
			ptPrj.Pack(handle);

			Registry_Contract.PutInt32(handle, _rwyId);
			Registry_Contract.PutDouble(handle, _trueBearing);
			Registry_Contract.PutDouble(handle, _magBearing);
			Registry_Contract.PutDouble(handle, _direction);

			Registry_Contract.PutDouble(handle, _elevTdz);
			Registry_Contract.PutDouble(handle, _elevTdzAccuracy);
			Registry_Contract.PutDouble(handle, _clearWay);
			Registry_Contract.PutDouble(handle, _displacment);
		}

		public override void UnPack(int handle)
		{
			base.UnPack(handle);
			ptGeo.UnPack(handle);
			ptPrj.UnPack(handle);
			_rwyId = Registry_Contract.GetInt32(handle);
			_trueBearing = Registry_Contract.GetDouble(handle);
			_magBearing = Registry_Contract.GetDouble(handle);
			_direction = Registry_Contract.GetDouble(handle);
			_elevTdz = Registry_Contract.GetDouble(handle);
			_elevTdzAccuracy = Registry_Contract.GetDouble(handle);
			_clearWay = Registry_Contract.GetDouble(handle);
			_displacment = Registry_Contract.GetDouble(handle);
		}

		public int getRwyId()
		{
			return _rwyId;
		}

		public void GetRwyId(int value)
		{
			_rwyId = value;
		}

		public double GetTrueBearing()
		{
			return _trueBearing;
		}

		public void GetTrueBearing(double value)
		{
			_trueBearing = value;
		}

		public double GetMagBearing()
		{
			return _magBearing;
		}

		public void SetMagBearing(double value)
		{
			_magBearing = value;
		}

		public double getDirection()
		{
			return _direction;
		}

		public void setDirection(double value)
		{
			_direction = value;
		}

		public double getElevTdz()
		{
			return _elevTdz;
		}

		public void setElevTdz(double value)
		{
			_elevTdz = value;
		}

		public double getElevTdzAccuracy()
		{
			return _elevTdzAccuracy;
		}

		public void SetElevTdzAccuracy(double value)
		{
			_elevTdzAccuracy = value;
		}

		public double GetClearWay()
		{
			return _clearWay;
		}

		public void SetClearWay(double value)
		{
			_clearWay = value;
		}

		public double GetDisplacment()
		{
			return _displacment;
		}

		public void SetDisplacment(double value)
		{
			_displacment = value;
		}

		private Point ptGeo;
		private Point ptPrj;
		private int _rwyId;
		private double _trueBearing;
		private double _magBearing;
		private double _direction;
		private double _elevTdz;
		private double _elevTdzAccuracy;
		private double _clearWay;
		private double _displacment;
	}
}
