using System;
using System.Collections;
using System.Collections.Generic;

namespace Aran.PANDA.Common
{
	[System.Runtime.InteropServices.ComVisible(false)]
	public class EnumArray<TItemType, TEnumType> : IEnumerable<TItemType>
	{
		private TItemType[] _items;
		private Array _enumValue;
		private int[] _indexOf;
		private int _min, _max;

		public EnumArray()
		{
			_enumValue = Enum.GetValues(typeof(TEnumType));
			int count = _enumValue.GetLength(0);

			_items = new TItemType[count];
			_min = Convert.ToInt32(_enumValue.GetValue(0));
			_max = _min;

			for (int i = 1; i < count; i++)
			{
				int iTmp = Convert.ToInt32(_enumValue.GetValue(i));
				if (iTmp < _min) _min = iTmp;
				if (iTmp > _max) _max = iTmp;
			}

			_indexOf = new int[_max - _min + 1];

			for (int i = 0; i <= _max - _min; i++)
				_indexOf[i] = -1;

			for (int i = 0; i < count; i++)
			{
				int iTmp = Convert.ToInt32(_enumValue.GetValue(i));
				_indexOf[iTmp - _min] = i;
			}
		}

		public TItemType this[TEnumType enumValue]
		{
			get
			{
				int i = Convert.ToInt32(enumValue);
				if (i < _min || i > _max)
					return default(TItemType);

				int index = _indexOf[i - _min];

				if (index < 0)
					return default(TItemType);
				return _items[index];
			}

			set
			{
				int i = Convert.ToInt32(enumValue);
				if (i < _min || i > _max)
					return;

				int index = _indexOf[i - _min];
				if (index < 0)
					return;

				_items[index] = value;
			}
		}

		public TItemType this[int index]
		{
			get	{	return _items[index];		}
			set	{	_items[index] = value;		}
		}

		public int Length
		{
			get	{	return _items.Length;	}
		}

		public int IndexOf(TEnumType enumValue)
		{
			for (int i = 0; i < _enumValue.GetLength(0); i++)
				if (_enumValue.GetValue(i).Equals(enumValue))
					return i;

			return -1;
		}

		public EnumArrayEnumarator<TItemType> GetEnumarator()
		{
			return new EnumArrayEnumarator<TItemType>(_items);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return (IEnumerator<TItemType>)GetEnumarator();
		}

		IEnumerator<TItemType> IEnumerable<TItemType>.GetEnumerator()
		{
			return (IEnumerator<TItemType>)GetEnumarator();
		}
	}

	public class EnumArrayEnumarator<TItemType> : IEnumerator<TItemType>
	{
		public TItemType[] _list;
		int position = -1;

		public EnumArrayEnumarator(TItemType[] list)
		{
			_list = list;
		}

		public TItemType Current
		{
			get { return Current; }
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		object IEnumerator.Current
		{
			get
			{
				try
				{
					return _list[position];
				}
				catch (IndexOutOfRangeException)
				{
					throw new InvalidOperationException();
				}
			}
		}

		public bool MoveNext()
		{
			position++;
			return (position < _list.Length);
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}
	}
}


/*
─────────────────────────▄▀▄  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─█  
─────────────────────────█─▀█▀█▄  
─────────────────────────█──█──█  
─────────────────────────█▄▄█──▀█  
────────────────────────▄█──▄█▄─▀█  
────────────────────────█─▄█─█─█─█  
────────────────────────█──█─█─█─█  
────────────────────────█──█─█─█─█  
────▄█▄──▄█▄────────────█──▀▀█─█─█  
──▄█████████────────────▀█───█─█▄▀  
─▄███████████────────────██──▀▀─█  
▄█████████████────────────█─────█  
██████████───▀▀█▄─────────▀█────█  
████████───▀▀▀──█──────────█────█  
██████───────██─▀█─────────█────█  
████──▄──────────▀█────────█────█ Look dude,
███──█──────▀▀█───▀█───────█────█ a good code!
███─▀─██──────█────▀█──────█────█  
███─────────────────▀█─────█────█  
███──────────────────█─────█────█  
███─────────────▄▀───█─────█────█  
████─────────▄▄██────█▄────█────█  
████────────██████────█────█────█  
█████────█──███████▀──█───▄█▄▄▄▄█  
██▀▀██────▀─██▄──▄█───█───█─────█  
██▄──────────██████───█───█─────█  
─██▄────────────▄▄────█───█─────█  
─███████─────────────▄█───█─────█  
──██████─────────────█───█▀─────█  
──▄███████▄─────────▄█──█▀──────█  
─▄█─────▄▀▀▀█───────█───█───────█  
▄█────────█──█────▄███▀▀▀▀──────█  
█──▄▀▀────────█──▄▀──█──────────█  
█────█─────────█─────█──────────█  
█────────▀█────█─────█─────────██  
█───────────────█──▄█▀─────────█  
█──────────██───█▀▀▀───────────█  
█───────────────█──────────────█  
█▄─────────────██──────────────█  
─█▄────────────█───────────────█  
──██▄────────▄███▀▀▀▀▀▄────────█  
─█▀─▀█▄────────▀█──────▀▄──────█  
─█────▀▀▀▀▄─────█────────▀─────█
*/