using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class FASDataBlock : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FASDataBlock; }
		}
		
		public double? HorizontalAlarmLimit
		{
			get { return GetNullableFieldValue <double> ((int) PropertyFASDataBlock.HorizontalAlarmLimit); }
			set { SetNullableFieldValue <double> ((int) PropertyFASDataBlock.HorizontalAlarmLimit, value); }
		}
		
		public double? VerticalAlarmLimit
		{
			get { return GetNullableFieldValue <double> ((int) PropertyFASDataBlock.VerticalAlarmLimit); }
			set { SetNullableFieldValue <double> ((int) PropertyFASDataBlock.VerticalAlarmLimit, value); }
		}
		
		public ValDistance ThresholdCourseWidth
		{
			get { return (ValDistance ) GetValue ((int) PropertyFASDataBlock.ThresholdCourseWidth); }
			set { SetValue ((int) PropertyFASDataBlock.ThresholdCourseWidth, value); }
		}
		
		public ValDistance LengthOffset
		{
			get { return (ValDistance ) GetValue ((int) PropertyFASDataBlock.LengthOffset); }
			set { SetValue ((int) PropertyFASDataBlock.LengthOffset, value); }
		}
		
		public string CRCRemainder
		{
			get { return GetFieldValue <string> ((int) PropertyFASDataBlock.CRCRemainder); }
			set { SetFieldValue <string> ((int) PropertyFASDataBlock.CRCRemainder, value); }
		}
		
		public uint? OperationType
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyFASDataBlock.OperationType); }
			set { SetNullableFieldValue <uint> ((int) PropertyFASDataBlock.OperationType, value); }
		}
		
		public uint? ServiceProviderSBAS
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyFASDataBlock.ServiceProviderSBAS); }
			set { SetNullableFieldValue <uint> ((int) PropertyFASDataBlock.ServiceProviderSBAS, value); }
		}
		
		public uint? ApproachPerformanceDesignator
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyFASDataBlock.ApproachPerformanceDesignator); }
			set { SetNullableFieldValue <uint> ((int) PropertyFASDataBlock.ApproachPerformanceDesignator, value); }
		}
		
		public string RouteIndicator
		{
			get { return GetFieldValue <string> ((int) PropertyFASDataBlock.RouteIndicator); }
			set { SetFieldValue <string> ((int) PropertyFASDataBlock.RouteIndicator, value); }
		}
		
		public uint? ReferencePathDataSelector
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyFASDataBlock.ReferencePathDataSelector); }
			set { SetNullableFieldValue <uint> ((int) PropertyFASDataBlock.ReferencePathDataSelector, value); }
		}
		
		public string ReferencePathIdentifier
		{
			get { return GetFieldValue <string> ((int) PropertyFASDataBlock.ReferencePathIdentifier); }
			set { SetFieldValue <string> ((int) PropertyFASDataBlock.ReferencePathIdentifier, value); }
		}
		
		public string CodeICAO
		{
			get { return GetFieldValue <string> ((int) PropertyFASDataBlock.CodeICAO); }
			set { SetFieldValue <string> ((int) PropertyFASDataBlock.CodeICAO, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFASDataBlock
	{
		HorizontalAlarmLimit = PropertyAObject.NEXT_CLASS,
		VerticalAlarmLimit,
		ThresholdCourseWidth,
		LengthOffset,
		CRCRemainder,
		OperationType,
		ServiceProviderSBAS,
		ApproachPerformanceDesignator,
		RouteIndicator,
		ReferencePathDataSelector,
		ReferencePathIdentifier,
		CodeICAO,
		NEXT_CLASS
	}
	
	public static class MetadataFASDataBlock
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFASDataBlock ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFASDataBlock.HorizontalAlarmLimit, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFASDataBlock.VerticalAlarmLimit, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFASDataBlock.ThresholdCourseWidth, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFASDataBlock.LengthOffset, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFASDataBlock.CRCRemainder, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFASDataBlock.OperationType, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFASDataBlock.ServiceProviderSBAS, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFASDataBlock.ApproachPerformanceDesignator, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFASDataBlock.RouteIndicator, (int) AimFieldType.SysString);
			PropInfoList.Add (PropertyFASDataBlock.ReferencePathDataSelector, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFASDataBlock.ReferencePathIdentifier, (int) AimFieldType.SysString);
			PropInfoList.Add (PropertyFASDataBlock.CodeICAO, (int) AimFieldType.SysString);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
