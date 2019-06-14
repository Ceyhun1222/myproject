using System;

namespace Aran.PANDA.Common
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class EnumArray<TItemType, TEnumType>
	{
		private TItemType[] _items;
		private Array _enumValue;

		public EnumArray()
		{
			_enumValue = Enum.GetValues(typeof(TEnumType));
			int count = _enumValue.GetLength(0);
			_items = new TItemType[count];
		}

		public TItemType this[TEnumType enumValue]
		{
			get
			{
				int index = IndexOf(enumValue);
				if (index == -1)
					return default(TItemType);
				return _items[index];
			}
			set
			{
				int index = IndexOf(enumValue);
				if (index == -1)
					return;
				_items[index] = value;
			}
		}

		public TItemType this [ int index ]
		{
			get
			{
				return _items [ index ];
			}
			set
			{
				_items [ index ] = value;
			}
		}

		public int Length
		{
			get
			{
				return _items.Length;
			}
		}

		public int IndexOf(TEnumType enumValue)
		{
			for (int i = 0; i < _enumValue.GetLength(0); i++)
			{
				if (_enumValue.GetValue(i).Equals(enumValue))
					return i;
			}
			return -1;
		}
	}
}
