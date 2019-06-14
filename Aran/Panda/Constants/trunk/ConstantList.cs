using System.Collections.Generic;

namespace Aran.PANDA.Constants
{
	public class ConstantList<T> : AranObject, IAranCloneable where T : NamedConstantObject, new()
	{
		public ConstantList()
		{
			_list = new List<T>();
		}

		public ConstantList(int count)
		{
			_list = new List<T>(count);
			for (int i = 0; i < count; i++)
			{
				_list.Add(null);
			}
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public void Clear()
		{
			_list.Clear();
		}

		protected T ConstantByIndex(int index)
		{
			if ((index >= 0) & (index < _list.Count))
			{
				if (_list[index] == null || !_list[index].Assigned)
					throw new System.Exception("The value is not assigned to constant: " + index.ToString());

				return _list[index];
			}

			throw new System.Exception("Index out of constants list bounds!");
		}

		protected T ConstantByName(string name)
		{
			T result = null;
			foreach (var item in _list)
			{
				if (item != null && item.Name == name)
				{
					result = item;
					if (!result.Assigned)
						throw new System.Exception("The value is not assigned to constant: \"" + name + "\"");

					return result;
				}
			}

			throw new System.Exception("\"" + name + "\" is not valid constant name!");
		}

		public void AddItem(T value)
		{
			_list.Add((T)value.Clone());
		}

		public void ReplaceItem(T value, int index)
		{
			_list[index] = (T)value.Clone();
		}

		#region IAranCloneable Members

		AranObject IAranCloneable.Clone()
		{
			ConstantList<T> constantList = new ConstantList<T>();
			constantList.Assign(this);
			return constantList;
		}

		public void Assign(AranObject source)
		{
			Clear();
			for (int i = 0; i < ((ConstantList<T>)source).Count; i++)
				AddItem(((ConstantList<T>)source).ConstantByIndex(i));
		}

		#endregion

		private IList<T> _list;
	}
}
