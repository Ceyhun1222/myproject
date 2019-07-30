using System;
using System.Collections.Generic;
using System.Xml;
using Aran.Aixm;
using Aran.Aim.Objects;
using Aran.Package;
using System.Collections;

namespace Aran.Aim
{
    public class AObjectListConfig
    {
        public static bool IgnoreNotes;
    }

    public class AObjectList <T> :
        AranObjectList <T>,
        IAixmSerializable,
        IAimProperty,
        IPackable
        where T : AObject, new ()
    {
        #region IAixmSerializable Members

        bool IAixmSerializable.AixmDeserialize (XmlContext context)
        {
            XmlContext tmpContext = new XmlContext ();
            tmpContext.ElementIndex.End = -1;
            tmpContext.PropertyIndex.End = -1;

            //by Andrey, in order to read EAD 5.1 snapshots with 8000+ notes in single airspace
            if (AObjectListConfig.IgnoreNotes)
            {  
                if (typeof(T).IsAssignableFrom(typeof(Features.Note)))
                {
                    return true;
                }
            }


	        for (int i = context.ElementIndex.Start; i < context.ElementIndex.End; i++)
	        {
		        XmlElement itemElement = context.Element.ChildNodes[i] as XmlElement;

		        T newItem = new T();
		        XmlElement firstChild;
		        if (newItem.ObjectType == ObjectType.FeatureRefObject)
			        firstChild = itemElement;
		        else
		        {
			        firstChild = itemElement.FirstChild as XmlElement;
			        if (firstChild == null && AimMetadata.IsAbstractFeatureRefObject((int) newItem.ObjectType))
				        firstChild = itemElement;
		        }

		        if (firstChild == null)
                    continue;

                DeserializeLastException.RemoveLastPropName();

                tmpContext.Element = firstChild;

                bool isDeserialized = (newItem as IAixmSerializable).AixmDeserialize (tmpContext);
                if (isDeserialized)
                    this.Add (newItem);
            }
            return true;
        }

        #endregion

        #region IAimProperty Members

        AimPropertyType IAimProperty.PropertyType
        {
            get { return AimPropertyType.List; }
        }

        IAixmSerializable IAimProperty.GetAixmSerializable ()
        {
            return this;
        }

        IPackable IAimProperty.GetPackable ()
        {
            return this;
        }

        #endregion

        #region IPackable Members

        void IPackable.Pack (PackageWriter writer)
        {
            writer.PutInt32 (Count);
            foreach (T item in this)
            {
                (item as IPackable).Pack (writer);
            }
        }

        void IPackable.Unpack (PackageReader reader)
        {
            Clear ();

            int count = reader.GetInt32 ();
            for (int i = 0; i < count; i++)
            {
                T t = new T ();
                (t as IPackable).Unpack (reader);
                this.Add (t);
            }
        }

        #endregion

        public override void Assign (AranObject source)
        {
            AObjectList<T> sourceList = (AObjectList<T>) source;

            _list.Clear ();
            foreach (T item in sourceList)
            {
                _list.Add ((T) item.Clone ());
            }
        }

        public override AranObject Clone ()
        {
            AObjectList<T> objList = new AObjectList<T> ();
            objList.Assign (this);
            return objList;
        }

        public static implicit operator List<T> (AObjectList<T> objList)
        {
            return objList._list;
        }
    }
}
