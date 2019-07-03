using System.Xml;
using Aran.Aixm;
using Aran.Package;

namespace Aran.Aim.DataTypes
{
    public abstract class ADataType :
        AimObject,
        IAimProperty
    {
        protected override AimObjectType AimObjectType
        {
            get { return AimObjectType.DataType; }
        }

        public abstract DataType DataType { get; }

        #region IAranProperty Members

        AimPropertyType IAimProperty.PropertyType
        {
            get { return AimPropertyType.DataType; }
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
    public static class MetadataADataType
    {
        public static AimPropInfoList PropInfoList;

        static MetadataADataType ()
        {
            PropInfoList = MetadataAimObject.PropInfoList.Clone ();
        }
    }
}