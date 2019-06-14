using System;
using System.Collections.Generic;
using ARAN.Common;
using ARAN.Contracts.Registry;

namespace ARAN.AIXMTypes
{
	public enum AIXMType
	{
		Null,				//0
		DME,				//1
		VOR,				//2
		Obstacle,			//3
		AHP,				//4
		RWY,				//5
		RwyDirection,		//6
		NDB,				//7
		DesignatedPoint,	//8
		TACAN				//9	
	}

	public abstract class AIXM : PandaItem
	{
		public AIXM()
			: this(AIXMType.Null)
		{
		}

		public AIXM(AIXMType AIXMType)
			: base()
		{
			_AIXMType = AIXMType;
			_id = "";
			_tag = 0;
            _remark = "";
		}

		public abstract Object Clone();

		public virtual void Pack(int handle)
		{
			Registry_Contract.PutString(handle, _id);
			Registry_Contract.PutString(handle, _AIXMID);
			Registry_Contract.PutString(handle, _name);
			Registry_Contract.PutString(handle, _remark);   
		}

		public virtual void UnPack(int handle)
		{
			_id = Registry_Contract.GetString(handle);
			_AIXMID = Registry_Contract.GetString(handle);
			_name = Registry_Contract.GetString(handle);
			_remark = Registry_Contract.GetString(handle);
			_tag = 0;
		}

		public void AIXMPack(int handle)
		{
			Registry_Contract.PutInt32(handle, (int)_AIXMType);
			Pack(handle);
		}

		public static AIXM AIXMUnpack(int handle)
		{
			AIXM obj = null;
			AIXMType aixmtype;
			aixmtype = (AIXMType)Registry_Contract.GetInt32(handle);

			switch (aixmtype)
			{
				case AIXMType.DME:
					obj = new Dme();
					break;

				case AIXMType.VOR:
					obj = new Vor();
					break;

				case AIXMType.Obstacle:
					obj = new Obstacle();
					break;

				case AIXMType.AHP:
					obj = new Ahp();
					break;

				case AIXMType.RWY:
					obj = new Rwy();
					break;

				case AIXMType.RwyDirection:
					obj = new RwyDirection();
					break;

				case AIXMType.NDB:
					obj = new Ndb();
					break;
				case AIXMType.DesignatedPoint:
					obj = new DesignatedPoint();
					break;
				case AIXMType.TACAN:
					obj = new Tacan();
					break;
			}

			obj.UnPack(handle);
			return obj;
		}

		public virtual void Assign(PandaItem source)
		{
			_AIXMType = ((AIXM)source)._AIXMType;
			_id = ((AIXM)source)._id;
			_AIXMID = ((AIXM)source)._AIXMID;
			_name = ((AIXM)source)._name;
			_remark = ((AIXM)source)._remark;
			_tag = ((AIXM)source)._tag;
		}

		public AIXMType GetAIXMType()
		{
			return _AIXMType;
		}

		public string GetName()
		{
			return _name;
		}

		public void SetName(string value)
		{
			_name = value;
		}

		public string GetId()
		{
			return _id;
		}

		public void SetId(string value)
		{
			_id = value;
		}

		public string GetAIXMId()
		{
			return _AIXMID;
		}

		public void SetAIXMId(string value)
		{
			_AIXMID = value;
		}

		public string GetRemark()
		{
			return _remark;
		}

		public void SetRemark(string value)
		{
			_remark = value;
		}

		public int GetTag()
		{
			return _tag;
		}

		public void SetTag(int value)
		{
			_tag = value;
		}

		public Ahp AsAhp()
		{
			if (_AIXMType != AIXMType.AHP)
			{
				FormatException e = new FormatException(_name + " is not a valid AHP.");
				throw e;
			}
			return (Ahp)this;
		}

		public Dme AsDme()
		{
			if (_AIXMType != AIXMType.DME)
			{
				FormatException e = new FormatException(_name + " is not a valid Dme.");
				throw e;
			}
			return (Dme)this;
		}

		public Ndb AsNdb()
		{
			if (_AIXMType != AIXMType.NDB)
			{
				FormatException e = new FormatException(_name + " is not a valid Ndb.");
				throw e;
			}
			return (Ndb)this;
		}

		public Obstacle AsObstacle()
		{
			if (_AIXMType != AIXMType.Obstacle)
			{
				FormatException e = new FormatException(_name + " is not a valid Obstacle.");
				throw e;
			}
			return (Obstacle)this;
		}

		public Rwy AsRwy()
		{
			if (_AIXMType != AIXMType.RWY)
			{
				FormatException e = new FormatException(_name + " is not a valid Rwy.");
				throw e;
			}
			return (Rwy)this;
		}

		public RwyDirection AsRwyDirection()
		{
			if (_AIXMType != AIXMType.RwyDirection)
			{
				FormatException e = new FormatException(_name + " is not a valid RwyDirection.");
				throw e;
			}
			return (RwyDirection)this;
		}

		public Tacan AsTacan()
		{
			if (_AIXMType != AIXMType.TACAN)
			{
				FormatException e = new FormatException(_name + "is not a valid Tacan");
				throw e;
			}
			return (Tacan)this;
		}

		public Vor AsVor()
		{
			if (_AIXMType != AIXMType.VOR)
			{
				FormatException e = new FormatException(_name + "is not a valid Vor");
				throw e;
			}
			return (Vor)this;
		}

		protected AIXMType _AIXMType;
		protected String _id;
		protected String _AIXMID;
		protected String _name;
		protected String _remark;
		protected int _tag;
	}
}
