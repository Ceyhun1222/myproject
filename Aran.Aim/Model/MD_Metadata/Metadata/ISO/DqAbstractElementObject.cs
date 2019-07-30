using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
    public class DqAbstractElementObject : AObject
    {
        public override ObjectType ObjectType
        {
            get { return ObjectType.DqAbstractElementObject; }
        }

        public DqAbstractElement Value
        {
            get { return (DqAbstractElement)GetAbstractObject((int)PropertyDqAbstractElementObject.Value, AbstractType.DqAbstractElement); }
            set { SetValue((int)PropertyDqAbstractElementObject.Value, value); }
        }

    }
}

namespace Aran.Aim.PropertyEnum
{
    public enum PropertyDqAbstractElementObject
    {
        Value = PropertyAObject.NEXT_CLASS,
        NEXT_CLASS
    }

    public static class MetadataDqAbstractElementObject
    {
        public static AimPropInfoList PropInfoList;

        static MetadataDqAbstractElementObject()
        {
            PropInfoList = MetadataAObject.PropInfoList.Clone();

            PropInfoList.Add(PropertyDqAbstractElementObject.Value, (int)AbstractType.DqAbstractElement, PropertyTypeCharacter.Nullable);
        }
    }
}
