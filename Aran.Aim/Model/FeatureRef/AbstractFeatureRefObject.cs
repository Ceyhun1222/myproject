using System;
using Aran.Aim.DataTypes;
using Aran.Aim.PropertyEnum;
using Aran.Aixm;

namespace Aran.Aim.Objects
{
    public abstract class AbstractFeatureRefObject<T> : AObject
        where T : ADataType, new ()
    {
        public T Feature
        {
            get { return (T) GetValue ((int) PropertyAbstractFeatureRefObject.Feature); }
            set { SetValue ((int) PropertyAbstractFeatureRefObject.Feature, value); }
        }

        protected override bool AixmDeserialize (XmlContext context)
        {
            T t = new T ();
            bool isDeserialized = ((IAixmSerializable) t).AixmDeserialize (context);

            if (!isDeserialized)
                return false;

            Feature = t;
            return true;
        }
    }
}

namespace Aran.Aim.PropertyEnum
{
    public enum PropertyAbstractFeatureRefObject
    {
        Feature = PropertyAObject.NEXT_CLASS
    }

    public static class MetadataAbstractFeatureRefObject<T> where T : ADataType, new ()
    {
        public static AimPropInfoList PropInfoList;

        static MetadataAbstractFeatureRefObject ()
        {
            PropInfoList = MetadataAObject.PropInfoList.Clone ();

            int typeIndex = (int) Enum.Parse (typeof (DataType), typeof (T).Name);
            PropInfoList.Add (PropertyAbstractFeatureRefObject.Feature, typeIndex);
        }
    }
}
