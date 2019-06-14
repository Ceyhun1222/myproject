using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class HoldingPattern : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.HoldingPattern; }
		}
		
		public CodeHoldingUsage? Type
		{
			get { return GetNullableFieldValue <CodeHoldingUsage> ((int) PropertyHoldingPattern.Type); }
			set { SetNullableFieldValue <CodeHoldingUsage> ((int) PropertyHoldingPattern.Type, value); }
		}
		
		public double? OutboundCourse
		{
			get { return GetNullableFieldValue <double> ((int) PropertyHoldingPattern.OutboundCourse); }
			set { SetNullableFieldValue <double> ((int) PropertyHoldingPattern.OutboundCourse, value); }
		}
		
		public CodeCourse? OutboundCourseType
		{
			get { return GetNullableFieldValue <CodeCourse> ((int) PropertyHoldingPattern.OutboundCourseType); }
			set { SetNullableFieldValue <CodeCourse> ((int) PropertyHoldingPattern.OutboundCourseType, value); }
		}
		
		public double? InboundCourse
		{
			get { return GetNullableFieldValue <double> ((int) PropertyHoldingPattern.InboundCourse); }
			set { SetNullableFieldValue <double> ((int) PropertyHoldingPattern.InboundCourse, value); }
		}
		
		public CodeDirectionTurn? TurnDirection
		{
			get { return GetNullableFieldValue <CodeDirectionTurn> ((int) PropertyHoldingPattern.TurnDirection); }
			set { SetNullableFieldValue <CodeDirectionTurn> ((int) PropertyHoldingPattern.TurnDirection, value); }
		}
		
		public ValDistanceVertical UpperLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyHoldingPattern.UpperLimit); }
			set { SetValue ((int) PropertyHoldingPattern.UpperLimit, value); }
		}
		
		public CodeVerticalReference? UpperLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyHoldingPattern.UpperLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyHoldingPattern.UpperLimitReference, value); }
		}
		
		public ValDistanceVertical LowerLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyHoldingPattern.LowerLimit); }
			set { SetValue ((int) PropertyHoldingPattern.LowerLimit, value); }
		}
		
		public CodeVerticalReference? LowerLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyHoldingPattern.LowerLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyHoldingPattern.LowerLimitReference, value); }
		}
		
		public ValSpeed SpeedLimit
		{
			get { return (ValSpeed ) GetValue ((int) PropertyHoldingPattern.SpeedLimit); }
			set { SetValue ((int) PropertyHoldingPattern.SpeedLimit, value); }
		}
		
		public string Instruction
		{
			get { return GetFieldValue <string> ((int) PropertyHoldingPattern.Instruction); }
			set { SetFieldValue <string> ((int) PropertyHoldingPattern.Instruction, value); }
		}
		
		public bool? NonStandardHolding
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyHoldingPattern.NonStandardHolding); }
			set { SetNullableFieldValue <bool> ((int) PropertyHoldingPattern.NonStandardHolding, value); }
		}
		
		public HoldingPatternLength OutboundLegSpan
		{
			get { return GetObject <HoldingPatternLength> ((int) PropertyHoldingPattern.OutboundLegSpan); }
			set { SetValue ((int) PropertyHoldingPattern.OutboundLegSpan, value); }
		}
		
		public SegmentPoint HoldingPoint
		{
			get { return (SegmentPoint ) GetAbstractObject ((int) PropertyHoldingPattern.HoldingPoint, AbstractType.SegmentPoint); }
			set { SetValue ((int) PropertyHoldingPattern.HoldingPoint, value); }
		}
		
		public Curve Extent
		{
			get { return GetObject <Curve> ((int) PropertyHoldingPattern.Extent); }
			set { SetValue ((int) PropertyHoldingPattern.Extent, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyHoldingPattern
	{
		Type = PropertyFeature.NEXT_CLASS,
		OutboundCourse,
		OutboundCourseType,
		InboundCourse,
		TurnDirection,
		UpperLimit,
		UpperLimitReference,
		LowerLimit,
		LowerLimitReference,
		SpeedLimit,
		Instruction,
		NonStandardHolding,
		OutboundLegSpan,
		HoldingPoint,
		Extent,
		NEXT_CLASS
	}
	
	public static class MetadataHoldingPattern
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataHoldingPattern ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyHoldingPattern.Type, (int) EnumType.CodeHoldingUsage, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.OutboundCourse, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.OutboundCourseType, (int) EnumType.CodeCourse, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.InboundCourse, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.TurnDirection, (int) EnumType.CodeDirectionTurn, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.UpperLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.UpperLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.LowerLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.LowerLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.SpeedLimit, (int) DataType.ValSpeed, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.Instruction, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.NonStandardHolding, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.OutboundLegSpan, (int) ObjectType.HoldingPatternLength, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.HoldingPoint, (int) AbstractType.SegmentPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingPattern.Extent, (int) ObjectType.Curve, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
