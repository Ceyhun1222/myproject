using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.PropertyEnum;

namespace Aran.Aim.Features
{
    public class CurveExtension : AObject
    {
        public override ObjectType ObjectType
        {
            get { return Aim.ObjectType.CurveExtension; }
        }

        public string Items
        {
            get { return GetFieldValue<string>((int)PropertyCurveExtension.Items); }
            set { SetFieldValue<string>((int)PropertyCurveExtension.Items, value); }
        }

        public string Value
        {
            get { return GetFieldValue<string>((int)PropertyCurveExtension.Value); }
            set { SetFieldValue<string>((int)PropertyCurveExtension.Value, value); }
        }
        
    }
}
namespace Aran.Aim.PropertyEnum
{
    public enum PropertyCurveExtension
    {
        Items = PropertyAObject.NEXT_CLASS,
        Value,
        NEXT_CLASS
    }

    public static class MetadataCurveExtension
    {
        public static AimPropInfoList PropInfoList;

        static MetadataCurveExtension()
        {
            PropInfoList = MetadataAObject.PropInfoList.Clone();

            PropInfoList.Add(PropertyCurveExtension.Items, (int)AimFieldType.SysString,"CRC Item");
            PropInfoList.Add(PropertyCurveExtension.Value, (int)AimFieldType.SysString,"CRC Value");
            PropInfoList.Add(PropertyDBEntity.Annotation, (int)ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
        }
    }
}

