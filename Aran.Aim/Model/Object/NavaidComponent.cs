using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class NavaidComponent : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.NavaidComponent; }
		}
		
		public uint? CollocationGroup
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyNavaidComponent.CollocationGroup); }
			set { SetNullableFieldValue <uint> ((int) PropertyNavaidComponent.CollocationGroup, value); }
		}
		
		public CodePositionInILS? MarkerPosition
		{
			get { return GetNullableFieldValue <CodePositionInILS> ((int) PropertyNavaidComponent.MarkerPosition); }
			set { SetNullableFieldValue <CodePositionInILS> ((int) PropertyNavaidComponent.MarkerPosition, value); }
		}
		
		public bool? ProvidesNavigableLocation
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyNavaidComponent.ProvidesNavigableLocation); }
			set { SetNullableFieldValue <bool> ((int) PropertyNavaidComponent.ProvidesNavigableLocation, value); }
		}
		
		public AbstractNavaidEquipmentRef TheNavaidEquipment
		{
			get { return (AbstractNavaidEquipmentRef ) GetValue ((int) PropertyNavaidComponent.TheNavaidEquipment); }
			set { SetValue ((int) PropertyNavaidComponent.TheNavaidEquipment, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNavaidComponent
	{
		CollocationGroup = PropertyAObject.NEXT_CLASS,
		MarkerPosition,
		ProvidesNavigableLocation,
		TheNavaidEquipment,
		NEXT_CLASS
	}
	
	public static class MetadataNavaidComponent
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNavaidComponent ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNavaidComponent.CollocationGroup, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidComponent.MarkerPosition, (int) EnumType.CodePositionInILS, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidComponent.ProvidesNavigableLocation, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidComponent.TheNavaidEquipment, (int) DataType.AbstractNavaidEquipmentRef, PropertyTypeCharacter.Nullable);
		}
	}
}
