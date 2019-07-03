using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class MissedApproachLeg : ApproachLeg
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.MissedApproachLeg; }
		}
		
		public CodeMissedApproach? Type
		{
			get { return GetNullableFieldValue <CodeMissedApproach> ((int) PropertyMissedApproachLeg.Type); }
			set { SetNullableFieldValue <CodeMissedApproach> ((int) PropertyMissedApproachLeg.Type, value); }
		}
		
		public bool? ThresholdAfterMAPT
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyMissedApproachLeg.ThresholdAfterMAPT); }
			set { SetNullableFieldValue <bool> ((int) PropertyMissedApproachLeg.ThresholdAfterMAPT, value); }
		}
		
		public ValDistanceVertical HeightMAPT
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyMissedApproachLeg.HeightMAPT); }
			set { SetValue ((int) PropertyMissedApproachLeg.HeightMAPT, value); }
		}
		
		public double? RequiredNavigationPerformance
		{
			get { return GetNullableFieldValue <double> ((int) PropertyMissedApproachLeg.RequiredNavigationPerformance); }
			set { SetNullableFieldValue <double> ((int) PropertyMissedApproachLeg.RequiredNavigationPerformance, value); }
		}
		
		public List <ApproachCondition> Condition
		{
			get { return GetObjectList <ApproachCondition> ((int) PropertyMissedApproachLeg.Condition); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMissedApproachLeg
	{
		Type = PropertyApproachLeg.NEXT_CLASS,
		ThresholdAfterMAPT,
		HeightMAPT,
		RequiredNavigationPerformance,
		Condition,
		NEXT_CLASS
	}
	
	public static class MetadataMissedApproachLeg
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMissedApproachLeg ()
		{
			PropInfoList = MetadataApproachLeg.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMissedApproachLeg.Type, (int) EnumType.CodeMissedApproach, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMissedApproachLeg.ThresholdAfterMAPT, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMissedApproachLeg.HeightMAPT, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMissedApproachLeg.RequiredNavigationPerformance, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMissedApproachLeg.Condition, (int) ObjectType.ApproachCondition, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
