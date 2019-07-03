using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class VisualGlideSlopeIndicator : GroundLightSystem
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.VisualGlideSlopeIndicator; }
		}
		
		public CodeVASIS? Type
		{
			get { return GetNullableFieldValue <CodeVASIS> ((int) PropertyVisualGlideSlopeIndicator.Type); }
			set { SetNullableFieldValue <CodeVASIS> ((int) PropertyVisualGlideSlopeIndicator.Type, value); }
		}
		
		public CodeSide? Position
		{
			get { return GetNullableFieldValue <CodeSide> ((int) PropertyVisualGlideSlopeIndicator.Position); }
			set { SetNullableFieldValue <CodeSide> ((int) PropertyVisualGlideSlopeIndicator.Position, value); }
		}
		
		public uint? NumberBox
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyVisualGlideSlopeIndicator.NumberBox); }
			set { SetNullableFieldValue <uint> ((int) PropertyVisualGlideSlopeIndicator.NumberBox, value); }
		}
		
		public bool? Portable
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyVisualGlideSlopeIndicator.Portable); }
			set { SetNullableFieldValue <bool> ((int) PropertyVisualGlideSlopeIndicator.Portable, value); }
		}
		
		public double? SlopeAngle
		{
			get { return GetNullableFieldValue <double> ((int) PropertyVisualGlideSlopeIndicator.SlopeAngle); }
			set { SetNullableFieldValue <double> ((int) PropertyVisualGlideSlopeIndicator.SlopeAngle, value); }
		}
		
		public ValDistanceVertical MinimumEyeHeightOverThreshold
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyVisualGlideSlopeIndicator.MinimumEyeHeightOverThreshold); }
			set { SetValue ((int) PropertyVisualGlideSlopeIndicator.MinimumEyeHeightOverThreshold, value); }
		}
		
		public FeatureRef RunwayDirection
		{
			get { return (FeatureRef ) GetValue ((int) PropertyVisualGlideSlopeIndicator.RunwayDirection); }
			set { SetValue ((int) PropertyVisualGlideSlopeIndicator.RunwayDirection, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyVisualGlideSlopeIndicator
	{
		Type = PropertyGroundLightSystem.NEXT_CLASS,
		Position,
		NumberBox,
		Portable,
		SlopeAngle,
		MinimumEyeHeightOverThreshold,
		RunwayDirection,
		NEXT_CLASS
	}
	
	public static class MetadataVisualGlideSlopeIndicator
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataVisualGlideSlopeIndicator ()
		{
			PropInfoList = MetadataGroundLightSystem.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyVisualGlideSlopeIndicator.Type, (int) EnumType.CodeVASIS, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVisualGlideSlopeIndicator.Position, (int) EnumType.CodeSide, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVisualGlideSlopeIndicator.NumberBox, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVisualGlideSlopeIndicator.Portable, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVisualGlideSlopeIndicator.SlopeAngle, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVisualGlideSlopeIndicator.MinimumEyeHeightOverThreshold, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVisualGlideSlopeIndicator.RunwayDirection, (int) FeatureType.RunwayDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
