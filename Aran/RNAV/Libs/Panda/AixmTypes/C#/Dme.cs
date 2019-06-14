using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.Contracts.Registry;

namespace ARAN.AIXMTypes
{
	public class Dme : SignificanPoint
	{
		public Dme()
			: base(AIXMType.DME)
		{
			_orgId = 0;
			_channel = "";
			_type = "";
			_dispalcement = 0;
			_vorId = 0;
		}

		public void Assign(AIXM value)
		{
			base.Assign((PandaItem)value);
			Dme src;
			src = value.AsDme();
			_orgId = src._orgId;
			_type = src._type;
			_channel = src._channel;
			_dispalcement = src._dispalcement;
			_vorId = src._vorId;
		}

		public override Object Clone()
		{
			Dme cln = new Dme();
			cln.Assign(this);
			return cln;
		}


		public override void Pack(int handle)
		{
			base.Pack(handle);
			Registry_Contract.PutInt32(handle, _orgId);
			Registry_Contract.PutString(handle, _channel);
			Registry_Contract.PutString(handle, _type);
			Registry_Contract.PutDouble(handle, _dispalcement);
			Registry_Contract.PutInt32(handle, _vorId);
		}

		public override void UnPack(int handle)
		{
			base.UnPack(handle);
			_orgId = Registry_Contract.GetInt32(handle);
			_channel = Registry_Contract.GetString(handle);
			_type = Registry_Contract.GetString(handle);
			_dispalcement = Registry_Contract.GetDouble(handle);
			_vorId = Registry_Contract.GetInt32(handle);
		}

		public int getOrgId()
		{
			return _orgId;
		}

		public void setOrgId(int value)
		{
			_orgId = value;
		}

		public String getChannel()
		{
			return _channel;
		}

		public void setChannel(String value)
		{
			_channel = value;
		}

		public String getType()
		{
			return _type;
		}

		public void setType(String value)
		{
			_type = value;
		}

		public double getDispalcement()
		{
			return _dispalcement;
		}

		public void setDispalcement(double value)
		{
			_dispalcement = value;
		}

		public int getVorId()
		{
			return _vorId;
		}

		public void setVorId(int value)
		{
			_vorId = value;
		}

		private int _orgId;
		private String _channel;
		private String _type;
		private double _dispalcement;
		private int _vorId;
	}
}
