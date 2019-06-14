using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class HoldingAssessment : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.HoldingAssessment; }
		}
		
		public ValDistanceVertical UpperLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyHoldingAssessment.UpperLimit); }
			set { SetValue ((int) PropertyHoldingAssessment.UpperLimit, value); }
		}
		
		public CodeVerticalReference? UpperLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyHoldingAssessment.UpperLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyHoldingAssessment.UpperLimitReference, value); }
		}
		
		public ValDistanceVertical LowerLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyHoldingAssessment.LowerLimit); }
			set { SetValue ((int) PropertyHoldingAssessment.LowerLimit, value); }
		}
		
		public CodeVerticalReference? LowerLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyHoldingAssessment.LowerLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyHoldingAssessment.LowerLimitReference, value); }
		}
		
		public ValSpeed SpeedLimit
		{
			get { return (ValSpeed ) GetValue ((int) PropertyHoldingAssessment.SpeedLimit); }
			set { SetValue ((int) PropertyHoldingAssessment.SpeedLimit, value); }
		}
		
		public string PatternTemplate
		{
			get { return GetFieldValue <string> ((int) PropertyHoldingAssessment.PatternTemplate); }
			set { SetFieldValue <string> ((int) PropertyHoldingAssessment.PatternTemplate, value); }
		}
		
		public bool? TurbulentAir
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyHoldingAssessment.TurbulentAir); }
			set { SetNullableFieldValue <bool> ((int) PropertyHoldingAssessment.TurbulentAir, value); }
		}
		
		public ValDistance LegLengthToward
		{
			get { return (ValDistance ) GetValue ((int) PropertyHoldingAssessment.LegLengthToward); }
			set { SetValue ((int) PropertyHoldingAssessment.LegLengthToward, value); }
		}
		
		public ValDistance LegLengthAway
		{
			get { return (ValDistance ) GetValue ((int) PropertyHoldingAssessment.LegLengthAway); }
			set { SetValue ((int) PropertyHoldingAssessment.LegLengthAway, value); }
		}
		
		public SegmentPoint HoldingPoint
		{
			get { return (SegmentPoint ) GetAbstractObject ((int) PropertyHoldingAssessment.HoldingPoint, AbstractType.SegmentPoint); }
			set { SetValue ((int) PropertyHoldingAssessment.HoldingPoint, value); }
		}
		
		public FeatureRef UnplannedHolding
		{
			get { return (FeatureRef ) GetValue ((int) PropertyHoldingAssessment.UnplannedHolding); }
			set { SetValue ((int) PropertyHoldingAssessment.UnplannedHolding, value); }
		}
		
		public FeatureRef AssessedHoldingPattern
		{
			get { return (FeatureRef ) GetValue ((int) PropertyHoldingAssessment.AssessedHoldingPattern); }
			set { SetValue ((int) PropertyHoldingAssessment.AssessedHoldingPattern, value); }
		}
		
		public List <ObstacleAssessmentArea> ObstacleAssessment
		{
			get { return GetObjectList <ObstacleAssessmentArea> ((int) PropertyHoldingAssessment.ObstacleAssessment); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyHoldingAssessment
	{
		UpperLimit = PropertyFeature.NEXT_CLASS,
		UpperLimitReference,
		LowerLimit,
		LowerLimitReference,
		SpeedLimit,
		PatternTemplate,
		TurbulentAir,
		LegLengthToward,
		LegLengthAway,
		HoldingPoint,
		UnplannedHolding,
		AssessedHoldingPattern,
		ObstacleAssessment,
		NEXT_CLASS
	}
	
	public static class MetadataHoldingAssessment
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataHoldingAssessment ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyHoldingAssessment.UpperLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingAssessment.UpperLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingAssessment.LowerLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingAssessment.LowerLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingAssessment.SpeedLimit, (int) DataType.ValSpeed, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingAssessment.PatternTemplate, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingAssessment.TurbulentAir, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingAssessment.LegLengthToward, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingAssessment.LegLengthAway, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingAssessment.HoldingPoint, (int) AbstractType.SegmentPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingAssessment.UnplannedHolding, (int) FeatureType.UnplannedHolding, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingAssessment.AssessedHoldingPattern, (int) FeatureType.HoldingPattern, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyHoldingAssessment.ObstacleAssessment, (int) ObjectType.ObstacleAssessmentArea, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
