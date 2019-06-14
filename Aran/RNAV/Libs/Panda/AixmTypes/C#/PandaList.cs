using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ARAN.Contracts.Registry;

namespace ARAN.Common
{
    public class PandaList<T>where T:PandaItem,new()
    {
        public PandaList ()
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

        public int Count()
        {
            return _list.Count;
        }

        public void Clear()
        {
            _list.Clear();        
        }

        public void Add(T _item)
        {
            _list.Add ((T)_item.Clone());
        }

        public void Assign(PandaList<T> list)
        {
            int i;
            Clear();
            for (i = 0; i < list.Count(); i++)
                Add(list[i]);
        }

        public PandaList<T> Clone()
        {
            PandaList<T> item = new PandaList<T>();
            item.Assign(item);
            return item;
        
        }

        public void Pack(int handle)
        {
            Registry_Contract.PutInt(handle, _list.Count);

            for (int i = 0; i < _list.Count; i++)
            {
                _list[i].Pack(handle);
            }
        }

        public void UnPack(int handle)
        {
            Clear();
            T t = new T();
            int count = Registry_Contract.GetInt(handle);

            for (int i = 0; i < count; i++)
            {
                t.UnPack(handle);
                Add (t);
            }
        }

        public PandaItem GetItem(int i)
        {
            return _list[i];
        }

        protected void SetItem(int index, T _item)
        {
            _list[index] = _item ;
        }

        public T this[int index]
        {
            get { return _list[index]; }
        }	

        private List<T> _list;
    }
}
