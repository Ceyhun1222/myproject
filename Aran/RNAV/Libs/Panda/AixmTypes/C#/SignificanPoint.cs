using System;
using System.Collections.Generic;
using System.Text;
using ARAN.GeometryClasses; 
using ARAN.Common;
using ARAN.Contracts.Registry;


namespace ARAN.AIXMTypes
{
	public abstract class SignificanPoint : AIXM
	{
		public SignificanPoint(AIXMType type)
			: base(type)
		{
			ptGeo = new Point();
			ptPrj = new Point();
			_frameId = "";
			_geoAccuracy = 0;
			_elevation = -9999;
			_elevAccuracy = 0;
		}

		public override void Assign(PandaItem source)
		{
			base.Assign((AIXM)source);
			SignificanPoint value = (SignificanPoint)source;
			ptGeo.Assign(value.ptGeo);
			ptPrj.Assign(value.ptPrj);
			_frameId = value._frameId;
			_geoAccuracy = value._geoAccuracy;
			_elevation = value._elevation;
			_elevAccuracy = value._elevAccuracy;
		}

		public override void Pack(int handle)
		{
			base.Pack(handle);
			ptGeo.Pack(handle);
			ptPrj.Pack(handle);

			Registry_Contract.PutString(handle, _frameId);
			Registry_Contract.PutDouble(handle, _geoAccuracy);
			Registry_Contract.PutDouble(handle, _elevation);
			Registry_Contract.PutDouble(handle, _elevAccuracy);
		}

		public override void UnPack(int handle)
		{
			base.UnPack(handle);
			ptGeo.UnPack(handle);
			ptPrj.UnPack(handle);

			_frameId = Registry_Contract.GetString(handle);
			_geoAccuracy = Registry_Contract.GetDouble(handle);
			_elevation = Registry_Contract.GetDouble(handle);
			_elevAccuracy = Registry_Contract.GetDouble(handle);
		}

		public Point getGeo()
		{
			return ptGeo;
		}

		public Point getPrj()
		{
			return ptPrj;
		}

		public string getFrameId()
		{
			return _frameId;
		}

        public void setFrameId(string frameId)
        {
            _frameId = frameId;
        }

		public double getGeoAccuracy()
		{
			return _geoAccuracy;
		}

        public void setGeoAccuracy(double geoAccuracy)
        {
            _geoAccuracy = geoAccuracy;
        }

		public double getElevation()
		{
			return _elevation;
		}

        public void setElevation(double elevation)
        {
            _elevation = elevation;
        }

		public double getElevationAccuracy()
		{
			return _elevAccuracy;
		}

        public void setElevationAccuracy(double elevAccuracy)
        {
            _elevAccuracy = elevAccuracy;
        }

		private Point ptGeo;
		private Point ptPrj;
		private string _frameId;
		private double _geoAccuracy;
		private double _elevation;
		private double _elevAccuracy;
	}
}
