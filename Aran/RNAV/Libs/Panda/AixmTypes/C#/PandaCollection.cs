using System;
using System.Collections.Generic;
using System.Text;
using ARAN.AIXMTypes;
using ARAN.Contracts.Registry;

namespace ARAN.Collection
{
	public class PandaCollection
	{
		public PandaCollection()
			: base()
		{
			_list = new List<AIXM>();
		}

		public void Insert(int index, AIXM value)
		{
			_list.Insert(index, (AIXM)value.Clone());
		}

		public void Remove(int index)
		{
			_list.RemoveAt(index);
		}

		public int Count()
		{
			return _list.Count;
		}

		public void Clear()
		{
			_list.Clear();
		}

		public void Add(AIXM value)
		{
			_list.Add((AIXM)value.Clone());
		}

		public void Assign(PandaCollection value)
		{
			Clear();

			for (int i = 0; i < value.Count(); i++)
			{
				Add(value.GetItem(i));
			}
		}

		public PandaCollection Clone()
		{
			PandaCollection value = new PandaCollection();
			value.Assign(this);
			return value;
		}

		public void Pack(int handle)
		{
			Registry_Contract.PutInt32(handle, _list.Count);
			for (int i = 0; i < _list.Count; i++)
			{
				GetItem(i).AIXMPack(handle);
			}
		}

		public void Unpack(int handle)
		{
			AIXM nextitem;
			Clear();
			int count = Registry_Contract.GetInt32(handle);
			for (int i = 0; i < count; i++)
			{
				nextitem = AIXM.AIXMUnpack(handle);
				Add(nextitem);
			}
		}

		public void SetItem(int index, AIXM value)
		{
			_list[index] = (AIXM)value.Clone();
		}

		public AIXM GetItem(int index)
		{
			return _list[index];
		}

		private List<AIXM> _list;
	}
}
