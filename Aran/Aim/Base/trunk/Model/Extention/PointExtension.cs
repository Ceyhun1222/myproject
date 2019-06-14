using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.PropertyEnum;

namespace Aran.Aim.Features
{
    public class PointExtension : AObject, ICRCExtension
    {
        public override ObjectType ObjectType
        {
            get { return Aim.ObjectType.PointExtension; }
        }

        public string CRCItems
        {
            get { return GetFieldValue<string>((int)PropertyPointExtension.CRCItems); }
            private set { SetFieldValue<string>((int)PropertyPointExtension.CRCItems, value); }
        }

        public string CRCValue
        {
            get { return GetFieldValue<string>((int)PropertyPointExtension.CRCValue); }
            private set { SetFieldValue<string>((int)PropertyPointExtension.CRCValue, value); }
        }


        void ICRCExtension.SetCRCItems(string value)
        {
            CRCItems = value;
        }

        string ICRCExtension.GetCRCItems()
        {
            return CRCItems;
        }

        void ICRCExtension.SetCRCValue(string value)
        {
            CRCValue = value;
        }

        string ICRCExtension.GetCRCValue()
        {
            return CRCValue;
        }
    }
}

namespace Aran.Aim.PropertyEnum
{
    public enum PropertyPointExtension
    {
        CRCItems = PropertyAObject.NEXT_CLASS,
        CRCValue,
        NEXT_CLASS
    }

    public static class MetadataPointExtension
    {
        public static AimPropInfoList PropInfoList;

        static MetadataPointExtension()
        {
            PropInfoList = MetadataAObject.PropInfoList.Clone();

            PropInfoList.Add(PropertyPointExtension.CRCItems, (int)AimFieldType.SysString);
            PropInfoList.Add(PropertyPointExtension.CRCValue, (int)AimFieldType.SysString);
            PropInfoList.Add(PropertyDBEntity.Annotation, (int)ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
        }
    }
}

