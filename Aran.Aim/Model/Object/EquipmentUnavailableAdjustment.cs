using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class EquipmentUnavailableAdjustment : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.EquipmentUnavailableAdjustment; }
		}
		
		public CodeEquipmentUnavailable? Type
		{
			get { return GetNullableFieldValue <CodeEquipmentUnavailable> ((int) PropertyEquipmentUnavailableAdjustment.Type); }
			set { SetNullableFieldValue <CodeEquipmentUnavailable> ((int) PropertyEquipmentUnavailableAdjustment.Type, value); }
		}
		
		public bool? ApproachLightingInoperative
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyEquipmentUnavailableAdjustment.ApproachLightingInoperative); }
			set { SetNullableFieldValue <bool> ((int) PropertyEquipmentUnavailableAdjustment.ApproachLightingInoperative, value); }
		}
		
		public List <EquipmentUnavailableAdjustmentColumn> AdjustmentINOPCol
		{
			get { return GetObjectList <EquipmentUnavailableAdjustmentColumn> ((int) PropertyEquipmentUnavailableAdjustment.AdjustmentINOPCol); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyEquipmentUnavailableAdjustment
	{
		Type = PropertyAObject.NEXT_CLASS,
		ApproachLightingInoperative,
		AdjustmentINOPCol,
		NEXT_CLASS
	}
	
	public static class MetadataEquipmentUnavailableAdjustment
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataEquipmentUnavailableAdjustment ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyEquipmentUnavailableAdjustment.Type, (int) EnumType.CodeEquipmentUnavailable, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyEquipmentUnavailableAdjustment.ApproachLightingInoperative, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyEquipmentUnavailableAdjustment.AdjustmentINOPCol, (int) ObjectType.EquipmentUnavailableAdjustmentColumn, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
