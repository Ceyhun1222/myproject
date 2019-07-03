using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
    public class DqDataQuality : BtAbstractObject
    {
        public override ObjectType ObjectType
        {
            get { return ObjectType.DqDataQuality; }
        }

        public List<DqAbstractElementObject> Report
        {
            get { return GetObjectList<DqAbstractElementObject>((int)PropertyDqDataQuality.Report); }
        }

        public LiLineage Lineage
        {
            get { return GetObject<LiLineage>((int)PropertyDqDataQuality.Lineage); }
            set { SetValue((int)PropertyDqDataQuality.Lineage, value); }
        }

    }
}

namespace Aran.Aim.PropertyEnum
{
    public enum PropertyDqDataQuality
    {
        Report = PropertyBtAbstractObject.NEXT_CLASS,
        Lineage,
        NEXT_CLASS
    }

    public static class MetadataDqDataQuality
    {
        public static AimPropInfoList PropInfoList;

        static MetadataDqDataQuality()
        {
            PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone();

            PropInfoList.Add(PropertyDqDataQuality.Report, (int)ObjectType.DqAbstractElementObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
            PropInfoList.Add(PropertyDqDataQuality.Lineage, (int)ObjectType.LiLineage, PropertyTypeCharacter.Nullable);
        }
    }
}
