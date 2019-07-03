using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class ApproachLightingSystem : GroundLightSystem
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.ApproachLightingSystem; }
		}
		
		public CodeApproachLightingICAO? ClassICAO
		{
			get { return GetNullableFieldValue <CodeApproachLightingICAO> ((int) PropertyApproachLightingSystem.ClassICAO); }
			set { SetNullableFieldValue <CodeApproachLightingICAO> ((int) PropertyApproachLightingSystem.ClassICAO, value); }
		}
		
		public CodeApproachLighting? Type
		{
			get { return GetNullableFieldValue <CodeApproachLighting> ((int) PropertyApproachLightingSystem.Type); }
			set { SetNullableFieldValue <CodeApproachLighting> ((int) PropertyApproachLightingSystem.Type, value); }
		}
		
		public ValDistance Length
		{
			get { return (ValDistance ) GetValue ((int) PropertyApproachLightingSystem.Length); }
			set { SetValue ((int) PropertyApproachLightingSystem.Length, value); }
		}
		
		public bool? SequencedFlashing
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyApproachLightingSystem.SequencedFlashing); }
			set { SetNullableFieldValue <bool> ((int) PropertyApproachLightingSystem.SequencedFlashing, value); }
		}
		
		public bool? AlignmentIndicator
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyApproachLightingSystem.AlignmentIndicator); }
			set { SetNullableFieldValue <bool> ((int) PropertyApproachLightingSystem.AlignmentIndicator, value); }
		}
		
		public FeatureRef ServedRunwayDirection
		{
			get { return (FeatureRef ) GetValue ((int) PropertyApproachLightingSystem.ServedRunwayDirection); }
			set { SetValue ((int) PropertyApproachLightingSystem.ServedRunwayDirection, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyApproachLightingSystem
	{
		ClassICAO = PropertyGroundLightSystem.NEXT_CLASS,
		Type,
		Length,
		SequencedFlashing,
		AlignmentIndicator,
		ServedRunwayDirection,
		NEXT_CLASS
	}
	
	public static class MetadataApproachLightingSystem
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataApproachLightingSystem ()
		{
			PropInfoList = MetadataGroundLightSystem.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyApproachLightingSystem.ClassICAO, (int) EnumType.CodeApproachLightingICAO, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachLightingSystem.Type, (int) EnumType.CodeApproachLighting, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachLightingSystem.Length, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachLightingSystem.SequencedFlashing, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachLightingSystem.AlignmentIndicator, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachLightingSystem.ServedRunwayDirection, (int) FeatureType.RunwayDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
