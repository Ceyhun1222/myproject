using System;
using System.Collections.Generic;
using System.Text;
using ARAN.Common;
using ARAN.Contracts.Registry;
using System.Collections;

namespace ARAN.GeometryClasses
{
    public abstract class GeometryList<T> : Geometry, IEnumerable where T : Geometry, new()
    {

        public GeometryList()
        {
            _list = new List<T>();
        }

        public void Insert(int index, T _item)
        {
            _list.Insert(index, _item);
        }

        public void Remove(int index)
        {
            _list.RemoveAt(index);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public void Clear()
        {
            _list.Clear();
        }

        public void Add(T _item)
        {
            _list.Add((T)_item.Clone());
        }

        public override void Assign(PandaItem list)
        {
            int i;
            Clear();
            for (i = 0; i < ((GeometryList<T>)list).Count; i++)
                Add(((GeometryList<T>)list)[i]);
        }

        public override void Pack(int handle)
        {
            Registry_Contract.PutInt32(handle, _list.Count);

            for (int i = 0; i < _list.Count; i++)
            {
                _list[i].Pack(handle);
            }
        }

        public override void UnPack(int handle)
        {
            Clear();
            T t = new T();
            int count = Registry_Contract.GetInt32(handle);

            for (int i = 0; i < count; i++)
            {
                t.UnPack(handle);
                Add(t);
            }
        }

        public virtual T this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        public T[] ToArray()
        {
            return _list.ToArray();
        }

        private List<T> _list;

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }
}
