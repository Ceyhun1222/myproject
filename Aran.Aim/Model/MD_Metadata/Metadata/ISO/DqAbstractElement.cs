using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata.ISO
{
    public abstract class DqAbstractElement : BtAbstractObject
    {
        public virtual DqAbstractElementType DqAbstractElementType
        {
            get { return (DqAbstractElementType)ObjectType; }
        }

        public List<BtString> NameOfMeasure
        {
            get { return GetObjectList<BtString>((int)PropertyDqAbstractElement.NameOfMeasure); }
        }

        public MdIdentifier MeasureIdentification
        {
            get { return GetObject<MdIdentifier>((int)PropertyDqAbstractElement.MeasureIdentification); }
            set { SetValue((int)PropertyDqAbstractElement.MeasureIdentification, value); }
        }

        public string MeasureDescription
        {
            get { return GetFieldValue<string>((int)PropertyDqAbstractElement.MeasureDescription); }
            set { SetFieldValue<string>((int)PropertyDqAbstractElement.MeasureDescription, value); }
        }

        public DqEvaluationMethodTypeCode? EvaluationMethodType
        {
            get { return GetNullableFieldValue<DqEvaluationMethodTypeCode>((int)PropertyDqAbstractElement.EvaluationMethodType); }
            set { SetNullableFieldValue<DqEvaluationMethodTypeCode>((int)PropertyDqAbstractElement.EvaluationMethodType, value); }
        }

        public string EvaluationMethodDescription
        {
            get { return GetFieldValue<string>((int)PropertyDqAbstractElement.EvaluationMethodDescription); }
            set { SetFieldValue<string>((int)PropertyDqAbstractElement.EvaluationMethodDescription, value); }
        }

        public CiCitation EvaluationProcedure
        {
            get { return GetObject<CiCitation>((int)PropertyDqAbstractElement.EvaluationProcedure); }
            set { SetValue((int)PropertyDqAbstractElement.EvaluationProcedure, value); }
        }

        public List<BtDateTime> DateTime
        {
            get { return GetObjectList<BtDateTime>((int)PropertyDqAbstractElement.DateTime); }
        }

        public List<DqAbstractResultObject> Result
        {
            get { return GetObjectList<DqAbstractResultObject>((int)PropertyDqAbstractElement.Result); }
        }

    }
}

namespace Aran.Aim.PropertyEnum
{
    public enum PropertyDqAbstractElement
    {
        NameOfMeasure = PropertyBtAbstractObject.NEXT_CLASS,
        MeasureIdentification,
        MeasureDescription,
        EvaluationMethodType,
        EvaluationMethodDescription,
        EvaluationProcedure,
        DateTime,
        Result,
        NEXT_CLASS
    }

    public static class MetadataDqAbstractElement
    {
        public static AimPropInfoList PropInfoList;

        static MetadataDqAbstractElement()
        {
            PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone();

            PropInfoList.Add(PropertyDqAbstractElement.NameOfMeasure, (int)ObjectType.BtString, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
            PropInfoList.Add(PropertyDqAbstractElement.MeasureIdentification, (int)ObjectType.MdIdentifier, PropertyTypeCharacter.Nullable);
            PropInfoList.Add(PropertyDqAbstractElement.MeasureDescription, (int)AimFieldType.SysString, PropertyTypeCharacter.Nullable);
            PropInfoList.Add(PropertyDqAbstractElement.EvaluationMethodType, (int)EnumType.DqEvaluationMethodTypeCode, PropertyTypeCharacter.Nullable);
            PropInfoList.Add(PropertyDqAbstractElement.EvaluationMethodDescription, (int)AimFieldType.SysString, PropertyTypeCharacter.Nullable);
            PropInfoList.Add(PropertyDqAbstractElement.EvaluationProcedure, (int)ObjectType.CiCitation, PropertyTypeCharacter.Nullable);
            PropInfoList.Add(PropertyDqAbstractElement.DateTime, (int)ObjectType.BtDateTime, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
            PropInfoList.Add(PropertyDqAbstractElement.Result, (int)ObjectType.DqAbstractResultObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
        }
    }
}
