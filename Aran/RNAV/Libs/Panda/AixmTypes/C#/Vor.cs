using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.Contracts.Registry;

namespace ARAN.AIXMTypes
{
	public enum VORNorthType
	{
		ntOther,
		ntTrueNorth,
		ntMagneticNorth,
		ntGrid
	}
	public class Vor : SignificanPoint
	{
		public Vor()
			: base(AIXMType.VOR)
		{
			_orgId = 0;
			_frequency = "";
			_type = "";
			_magneticVariation = 0;
			_declination = 0;
			_northType = VORNorthType.ntOther;
		}

		public void Assign(AIXM value)
		{
			Vor src;
			src = value.AsVor();
			_orgId = src._orgId;
			_frequency = src._frequency;
			_type = src._type;
			_magneticVariation = src._magneticVariation;
			_declination = src._declination;
			_northType = src._northType;
		}

		public override Object Clone()
		{
			Vor cln = new Vor();
			cln.Assign(this);
			return cln;
		}

		public int GetOrgId()
		{
			return _orgId;
		}

		public void SetOrgId(int value)
		{
			_orgId = value;
		}

		public String GetFrequency()
		{
			return _frequency;
		}

		public void SetFrequency(String value)
		{
			_frequency = value;
		}

		public string GetType()
		{
			return _type;
		}

		public void SetType(String value)
		{
			_type = value;
		}

		public double GetMagneticVariation()
		{
			return _magneticVariation;
		}

		public void SetMagneticVariation(double value)
		{
			_magneticVariation = value;
		}

		public VORNorthType GetVorNorthType()
		{
			return _northType;
		}

		public void SetVorNorthTYpe(VORNorthType value)
		{
			_northType = value;
		}

        public double GetDeclination()
        {
            return _declination;
        }

        public void SetDeclination (double declination)
        {
            _declination = declination;
        }
        

		public override void Pack(int handle)
		{
			base.Pack(handle);
			Registry_Contract.PutInt32(handle, _orgId);
			Registry_Contract.PutString(handle, _frequency);
			Registry_Contract.PutString(handle, _type);
			Registry_Contract.PutDouble(handle, _magneticVariation);
			Registry_Contract.PutDouble(handle, _declination);
			Registry_Contract.PutInt32(handle, (int)_northType);
		}

		public override void UnPack(int handle)
		{
			base.UnPack(handle);
			_orgId = Registry_Contract.GetInt32(handle);
			_frequency = Registry_Contract.GetString(handle);
			_type = Registry_Contract.GetString(handle);
			_magneticVariation = Registry_Contract.GetDouble(handle);
			_declination = Registry_Contract.GetDouble(handle);
			_northType = (VORNorthType)Registry_Contract.GetInt32(handle);
		}

		private int _orgId;
		private String _frequency;
		private String _type;
		private double _magneticVariation;
		private double _declination;
		private VORNorthType _northType;
	}
}
