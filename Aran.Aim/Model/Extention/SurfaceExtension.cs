using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.PropertyEnum;

namespace Aran.Aim.Features
{
    public class SurfaceExtension : AObject
    {
        public override ObjectType ObjectType
        {
            get { return Aim.ObjectType.SurfaceExtension; }
        }

        public string Items
        {
            get { return GetFieldValue<string>((int)PropertySurfaceExtension.Items); }
            set { SetFieldValue<string>((int)PropertySurfaceExtension.Items, value); }
        }

        public string Value
        {
            get { return GetFieldValue<string>((int)PropertySurfaceExtension.Value); }
            set { SetFieldValue<string>((int)PropertySurfaceExtension.Value, value); }
        }
    }
}

namespace Aran.Aim.PropertyEnum
{
    public enum PropertySurfaceExtension
    {
        Items = PropertyAObject.NEXT_CLASS,
        Value,
        NEXT_CLASS
    }

    public static class MetadataSurfaceExtension
    {
        public static AimPropInfoList PropInfoList;

        static MetadataSurfaceExtension()
        {
            PropInfoList = MetadataAObject.PropInfoList.Clone();

            PropInfoList.Add(PropertySurfaceExtension.Items, (int)AimFieldType.SysString);
            PropInfoList.Add(PropertySurfaceExtension.Value, (int)AimFieldType.SysString);
            PropInfoList.Add(PropertyDBEntity.Annotation, (int)ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
        }
    }

}
