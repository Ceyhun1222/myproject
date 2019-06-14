using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata
{
	public class DataQualityElement : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.DataQualityElement; }
		}
		
		public string evaluationMethodName
		{
			get { return GetFieldValue <string> ((int) PropertyDataQualityElement.evaluationMethodName); }
			set { SetFieldValue <string> ((int) PropertyDataQualityElement.evaluationMethodName, value); }
		}
		
		public DQEvaluationMethodTypeCode? evaluationMethodType
		{
			get { return GetNullableFieldValue <DQEvaluationMethodTypeCode> ((int) PropertyDataQualityElement.evaluationMethodType); }
			set { SetNullableFieldValue <DQEvaluationMethodTypeCode> ((int) PropertyDataQualityElement.evaluationMethodType, value); }
		}
		
		public bool? pass
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyDataQualityElement.pass); }
			set { SetNullableFieldValue <bool> ((int) PropertyDataQualityElement.pass, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDataQualityElement
	{
		evaluationMethodName,
		evaluationMethodType,
		pass,
		NEXT_CLASS
	}
	
	public static class MetadataDataQualityElement
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDataQualityElement ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDataQualityElement.evaluationMethodName, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDataQualityElement.evaluationMethodType, (int) EnumType.DQEvaluationMethodTypeCode, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDataQualityElement.pass, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
		}
	}
}
