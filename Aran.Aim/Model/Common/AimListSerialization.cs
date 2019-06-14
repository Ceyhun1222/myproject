using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;
using Aran.Package;
using System.Collections.ObjectModel;
using Aran.Aim.Package;

namespace Aran.Aim
{
    [Serializable]
    public class AimListSerialization : ISerializable
    {
        public AimListSerialization (IList list)
        {
            _list = list;
        }

        protected AimListSerialization (SerializationInfo info, StreamingContext context)
        {
            AranPackageReader reader = CommonFunctions.Desirialize (info);
            //_list = reader.GetObject () as IList;
            reader.Dispose ();
        }

        public T [] ToArray<T> ()
        {
            if (_list == null)
            {
                return new T [0];
            }

            T [] ta = new T [_list.Count];
            for (int i = 0; i < _list.Count; i++)
            {
                ta [i] = (T) _list [i];
            }
            return ta;
        }

        public bool IsNull
        {
            get { return (_list == null); }
        }

        void ISerializable.GetObjectData (SerializationInfo info, StreamingContext context)
        {
            AranPackageWriter writer = new AranPackageWriter ();
            //writer.PutObject (_list);
            CommonFunctions.Serialize (writer, info);
        }

        private IList _list;
    }
}
