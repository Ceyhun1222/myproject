using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.Contracts.Registry;

namespace ARAN.AIXMTypes
{
	public class Tacan : SignificanPoint
	{
		public Tacan()
			: base(AIXMType.TACAN)
		{
			_orgId = 0;
			_magneticVariation = 0;
			_channel = "";
			_declination = 0;
		}

		public void Assign(AIXM value)
		{
			base.Assign(value);
			Tacan src = value.AsTacan();
			_orgId = src._orgId;
			_magneticVariation = src._magneticVariation;
			_channel = src._channel;
			_declination = src._declination;
		}

		public override Object Clone()
		{
			Tacan cln = new Tacan();
			cln.Assign(this);
			return cln;
		}

		public override void Pack(int handle)
		{
			base.Pack(handle);
			Registry_Contract.PutInt32(handle, _orgId);
			Registry_Contract.PutString(handle, _channel);
			Registry_Contract.PutDouble(handle, _magneticVariation);
			Registry_Contract.PutDouble(handle, _declination);
		}

		public override void UnPack(int handle)
		{
			base.UnPack(handle);
			_orgId = Registry_Contract.GetInt32(handle);
			_channel = Registry_Contract.GetString(handle);
			_magneticVariation = Registry_Contract.GetDouble(handle);
			_declination = Registry_Contract.GetDouble(handle);
		}

		public int GetOrgId()
		{
			return _orgId;
		}

		public void SetOrgId(int value)
		{
			_orgId = value;
		}

		public double GetMagneticVariation()
		{
			return _magneticVariation;
		}

		public void SetMagneticVariation(double value)
		{
			_magneticVariation = value;
		}

		public String GetChannel()
		{
			return _channel;
		}

		public void SetChannel(String value)
		{
			_channel = value;
		}

		public double GetDeclination()
		{
			return _declination;
		}

        public void SetDeclination(double value)
        {
            _declination = value;
        }

		private int _orgId;
		private double _magneticVariation;
		private String _channel;
		private double _declination;
	}
}
