using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.Contracts.Registry;


namespace ARAN.AIXMTypes
{
	public class Ndb : SignificanPoint
	{
		public Ndb()
			: base(AIXMType.NDB)
		{
			_orgId = 0;
			_frequency = "";
			_class = "";
			_magneticVariation = 0;
			_ilsPosition = 0;

		}

		public void Assign(AIXM value)
		{
			Ndb src;
			src = value.AsNdb();
			_orgId = src._orgId;
			_frequency = src._frequency;
			_class = src._class;
			_magneticVariation = src._magneticVariation;
			_ilsPosition = src._ilsPosition;
		}

		public override Object Clone()
		{
			Ndb cln = new Ndb();
			cln.Assign(this);
			return cln;
		}

		public override void Pack(int handle)
		{
			base.Pack(handle);
			Registry_Contract.PutInt32(handle, _orgId);
			Registry_Contract.PutString(handle, _frequency);
			Registry_Contract.PutString(handle, _class);
			Registry_Contract.PutDouble(handle, _magneticVariation);
			Registry_Contract.PutDouble(handle, _ilsPosition);
		}

		public override void UnPack(int handle)
		{
			base.UnPack(handle);
			_orgId = Registry_Contract.GetInt32(handle);
			_frequency = Registry_Contract.GetString(handle);
			_class = Registry_Contract.GetString(handle);
			_magneticVariation = Registry_Contract.GetDouble(handle);
			_ilsPosition = Registry_Contract.GetDouble(handle);
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

		public String GetClas()
		{
			return _class;

		}

		public void GetClas(String value)
		{
			_class = value;
		}

		public double GetMagneticVariation()
		{
			return _magneticVariation;
		}

		public void SetMagneticVariation(double value)
		{
			_magneticVariation = value;
		}

		public double GetIlsPosition()
		{
			return _ilsPosition;
		}

		public void SetIlsPositin(double value)
		{
			_ilsPosition = value;
		}

		private int _orgId;
		private String _frequency;
		private String _class;
		private double _magneticVariation;
		private double _ilsPosition;

	}
}
