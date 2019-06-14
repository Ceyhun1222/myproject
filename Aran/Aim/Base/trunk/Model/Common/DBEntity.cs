using System.Collections.Generic;
using Aran.Aim.Features;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using System;

namespace Aran.Aim
{
    public interface IEditDBEntity
    {
        void SetId (long id);
        Aran.Aim.DB.IDataListener Listener { get; set; }
        bool AddLoaded (int propertyIndex);
    }

    public abstract class DBEntity :
        AimObject,
        IEditDBEntity
    {
        public DBEntity ()
        {
            _loadedProperties = new List<int> ();
        }

        public long Id
        {
            get { return GetFieldValue<long> ((int) PropertyDBEntity.Id); }
            set { SetFieldValue<long> ((int) PropertyDBEntity.Id, value); }
        }

        public List<Note> Annotation
        {
            get { return GetObjectList <Note> ((int) PropertyDBEntity.Annotation); }
        }

        #region GetValue

        protected T GetObject<T> (int propertyIndex) where T : AObject, new ()
        {
            object value = GetValue (propertyIndex);

            if (value == null && 
                AddLoaded (propertyIndex))
            {
                AObjectFilter refInfo = new AObjectFilter (this, propertyIndex);

                T aObject = new T ();
                bool exists = _listener.GetObject (aObject, refInfo);
                if (exists)
                {
                    SetValuePrivate (propertyIndex, aObject);
                }
                else
                {
                    value = GetValue (propertyIndex);
                    aObject = (value != null ? (T) value : null);
                }

                return aObject;
            }

            return (T) value;
        }

        protected List<T> GetObjectList<T> (int propertyIndex) where T : AObject, new ()
        {
            IAimProperty value = GetValue (propertyIndex);
            if (value != null)
                return (List<T>) (AObjectList<T>) value;

            AObjectList<T> objectList = null;

            if (AddLoaded (propertyIndex))
            {
                AObjectFilter refInfo = new AObjectFilter (this, propertyIndex);
                objectList = _listener.GetObjects<T> (refInfo);
                
                if (objectList == null)
                {
                    value = GetValue (propertyIndex);
                    if (value != null)
                        objectList = value as AObjectList<T>;
                    else
                        objectList = new AObjectList<T> ();
                }

                //_listener = null;
            }
            else
            {
                objectList = new AObjectList<T> ();
            }

            SetValuePrivate (propertyIndex, objectList);
            return objectList;
        }

        protected AObject GetAbstractObject (int propertyIndex, AbstractType abstractType)
        {
            IAimProperty value = GetValue (propertyIndex);

            if (value == null &&
                AddLoaded (propertyIndex))
            {
                AObjectFilter refInfo = new AObjectFilter (this, propertyIndex);

                value = _listener.GetAbstractObject (abstractType, refInfo);
                if (value != null)
                {
                    SetValuePrivate (propertyIndex, value);
                }
                else
                {
                    value = GetValue (propertyIndex);
                }
            }

            return (AObject) value;
        }

        #endregion

        protected bool AddLoaded (int propertyIndex)
        {
            if (_listener == null)
                return false;

            if (_loadedProperties.Contains (propertyIndex))
                return false;

            _loadedProperties.Add (propertyIndex);
            return true;
        }

        #region IEditDBEntity

        Aran.Aim.DB.IDataListener IEditDBEntity.Listener
        {
            get { return _listener; }
            set { _listener = value; }
        }

        void IEditDBEntity.SetId (long id)
        {
            Id = id;
        }

        bool IEditDBEntity.AddLoaded (int propertyIndex)
        {
            return AddLoaded (propertyIndex);
        }

        #endregion

        public override void Assign (AranObject source)
        {
            _listener = ((IEditDBEntity) source).Listener;
            base.Assign (source);
        }

        private Aran.Aim.DB.IDataListener _listener;
        private List<int> _loadedProperties;
    }
}

namespace Aran.Aim.PropertyEnum
{
    public enum PropertyDBEntity
    {
        Id,
        Annotation,
        NEXT_CLASS
    }

    public static class MetadataDBEntity
    {
        public static AimPropInfoList PropInfoList;

        static MetadataDBEntity ()
        {
            PropInfoList = MetadataAimObject.PropInfoList.Clone ();
            PropInfoList.Add (PropertyDBEntity.Id, (int) AimFieldType.SysInt64, "");
			//PropInfoList.Add ( PropertyDBEntity.Annotation, ( int ) ObjectType.Note );
        }
    }
}