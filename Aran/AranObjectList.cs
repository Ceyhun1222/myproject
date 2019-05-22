using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Aran
{
    public class AranObjectList <T> :
        AranObject,
        IEnumerable <T>,
        IAranCloneable,
        IList<T>,
        IList
        where T : AranObject
    {
        public AranObjectList ()
        {
            _list = new List<T> ();
        }

        public T this [int index]
        {
            get
            {
                return _list [index];
            }
            set
            {
                _list [index] = value;
            }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public void Insert (int index, T _item)
        {
            _list.Insert (index, _item);
        }

        public void RemoveAt (int index)
        {
            _list.RemoveAt (index);
        }

        public virtual void Remove (T item)
        {
            _list.Remove (item);
        }

        public void Clear ()
        {
            _list.Clear ();
        }

        public void Reverse ()
        {
            _list.Reverse ();
        }

        public void Add (T _item)
        {
            _list.Add (_item);
        }

        public void AddRange (IEnumerable <T> collection)
        {
            _list.AddRange (collection);
        }

        public T [] ToArray ()
        {
            return _list.ToArray ();
        }

        public virtual AranObject Clone ()
        {
            AranObjectList<T> objList = new AranObjectList<T> ();
            objList.Assign (this);
            return objList;
        }

        public virtual void Assign (AranObject source)
        {
            AranObjectList<T> sourceList = (AranObjectList<T>) source;
            _list.Clear ();
            foreach (T aranObject in sourceList)
            {
				var objItem = (aranObject as IAranCloneable).Clone();
                _list.Add (objItem as T);
            }
        }

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator ()
        {
            return _list.GetEnumerator ();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return _list.GetEnumerator ();
        }

        #endregion

        #region IList<T> Members

        public int IndexOf (T item)
        {
            return _list.IndexOf (item);
        }

        #endregion

        #region ICollection<T> Members

        public bool Contains (T item)
        {
            return _list.Contains (item);
        }

        public void CopyTo (T [] array, int arrayIndex)
        {
            _list.CopyTo (array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get { return (_list as IList).IsReadOnly; }
        }

        bool ICollection<T>.Remove (T item)
        {
            return _list.Remove (item);
        }

        #endregion

        #region IList Members

        int IList.Add (object value)
        {
            return (_list as IList).Add (value);
        }

        void IList.Clear ()
        {
            _list.Clear ();
        }

        bool IList.Contains (object value)
        {
            return (_list as IList).Contains (value);
        }

        int IList.IndexOf (object value)
        {
            return (_list as IList).IndexOf (value);
        }

        void IList.Insert (int index, object value)
        {
            (_list as IList).Insert (index, value);
        }

        bool IList.IsFixedSize
        {
            get { return (_list as IList).IsFixedSize; }
        }

        bool IList.IsReadOnly
        {
            get { return (_list as IList).IsReadOnly; }
        }

        void IList.Remove (object value)
        {
            (_list as IList).Remove (value);
        }

        void IList.RemoveAt (int index)
        {
            (_list as IList).RemoveAt (index);
        }

        object IList.this [int index]
        {
            get { return (_list as IList) [index]; }
            set { (_list as IList) [index] = value; }
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo (Array array, int index)
        {
            (_list as IList).CopyTo (array, index);
        }

        int ICollection.Count
        {
            get { return (_list as IList).Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return (_list as IList).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return (_list as IList).SyncRoot; }
        }

        #endregion

        protected List<T> _list;
    }
}
