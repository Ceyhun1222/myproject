using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class EquipmentUnavailableAdjustmentColumn : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.EquipmentUnavailableAdjustmentColumn; }
		}
		
		public CodeApproach? GuidanceEquipment
		{
			get { return GetNullableFieldValue <CodeApproach> ((int) PropertyEquipmentUnavailableAdjustmentColumn.GuidanceEquipment); }
			set { SetNullableFieldValue <CodeApproach> ((int) PropertyEquipmentUnavailableAdjustmentColumn.GuidanceEquipment, value); }
		}
		
		public bool? LandingSystemLights
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyEquipmentUnavailableAdjustmentColumn.LandingSystemLights); }
			set { SetNullableFieldValue <bool> ((int) PropertyEquipmentUnavailableAdjustmentColumn.LandingSystemLights, value); }
		}
		
		public bool? EquipmentRVR
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyEquipmentUnavailableAdjustmentColumn.EquipmentRVR); }
			set { SetNullableFieldValue <bool> ((int) PropertyEquipmentUnavailableAdjustmentColumn.EquipmentRVR, value); }
		}
		
		public ValDistanceVertical VisibilityAdjustment
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyEquipmentUnavailableAdjustmentColumn.VisibilityAdjustment); }
			set { SetValue ((int) PropertyEquipmentUnavailableAdjustmentColumn.VisibilityAdjustment, value); }
		}
		
		public bool? ApproachLightingInoperative
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyEquipmentUnavailableAdjustmentColumn.ApproachLightingInoperative); }
			set { SetNullableFieldValue <bool> ((int) PropertyEquipmentUnavailableAdjustmentColumn.ApproachLightingInoperative, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyEquipmentUnavailableAdjustmentColumn
	{
		GuidanceEquipment = PropertyAObject.NEXT_CLASS,
		LandingSystemLights,
		EquipmentRVR,
		VisibilityAdjustment,
		ApproachLightingInoperative,
		NEXT_CLASS
	}
	
	public static class MetadataEquipmentUnavailableAdjustmentColumn
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataEquipmentUnavailableAdjustmentColumn ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyEquipmentUnavailableAdjustmentColumn.GuidanceEquipment, (int) EnumType.CodeApproach, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyEquipmentUnavailableAdjustmentColumn.LandingSystemLights, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyEquipmentUnavailableAdjustmentColumn.EquipmentRVR, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyEquipmentUnavailableAdjustmentColumn.VisibilityAdjustment, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyEquipmentUnavailableAdjustmentColumn.ApproachLightingInoperative, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
