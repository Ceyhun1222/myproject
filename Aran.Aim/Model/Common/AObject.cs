using System.Xml;
using Aran.Aixm;
using Aran.Package;

namespace Aran.Aim.Objects
{
    public abstract class AObject : 
        DBEntity,
        IAimProperty
    {
        protected override AimObjectType AimObjectType
        {
            get { return AimObjectType.Object; }
        }

        public abstract ObjectType ObjectType { get; }

        #region IAranProperty Members

        AimPropertyType IAimProperty.PropertyType
        {
            get { return AimPropertyType.Object; }
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
    }
}

namespace Aran.Aim.PropertyEnum
{
    public enum PropertyAObject
    {
        NEXT_CLASS = PropertyDBEntity.NEXT_CLASS
    }

    public static class MetadataAObject
    {
        public static AimPropInfoList PropInfoList;

        static MetadataAObject ()
        {
            PropInfoList = MetadataDBEntity.PropInfoList.Clone ();
        }
    }
}
