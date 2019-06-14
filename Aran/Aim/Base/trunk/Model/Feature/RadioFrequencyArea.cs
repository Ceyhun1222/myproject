using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class RadioFrequencyArea : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RadioFrequencyArea; }
		}
		
		public CodeRadioFrequencyArea? Type
		{
			get { return GetNullableFieldValue <CodeRadioFrequencyArea> ((int) PropertyRadioFrequencyArea.Type); }
			set { SetNullableFieldValue <CodeRadioFrequencyArea> ((int) PropertyRadioFrequencyArea.Type, value); }
		}
		
		public double? AngleScallop
		{
			get { return GetNullableFieldValue <double> ((int) PropertyRadioFrequencyArea.AngleScallop); }
			set { SetNullableFieldValue <double> ((int) PropertyRadioFrequencyArea.AngleScallop, value); }
		}
		
		public CodeRadioSignal? SignalType
		{
			get { return GetNullableFieldValue <CodeRadioSignal> ((int) PropertyRadioFrequencyArea.SignalType); }
			set { SetNullableFieldValue <CodeRadioSignal> ((int) PropertyRadioFrequencyArea.SignalType, value); }
		}
		
		public EquipmentChoice Equipment
		{
			get { return GetObject <EquipmentChoice> ((int) PropertyRadioFrequencyArea.Equipment); }
			set { SetValue ((int) PropertyRadioFrequencyArea.Equipment, value); }
		}
		
		public List <CircleSector> Sector
		{
			get { return GetObjectList <CircleSector> ((int) PropertyRadioFrequencyArea.Sector); }
		}
		
		public List <Surface> Extent
		{
			get { return GetObjectList <Surface> ((int) PropertyRadioFrequencyArea.Extent); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRadioFrequencyArea
	{
		Type = PropertyFeature.NEXT_CLASS,
		AngleScallop,
		SignalType,
		Equipment,
		Sector,
		Extent,
		NEXT_CLASS
	}
	
	public static class MetadataRadioFrequencyArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRadioFrequencyArea ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRadioFrequencyArea.Type, (int) EnumType.CodeRadioFrequencyArea, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioFrequencyArea.AngleScallop, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioFrequencyArea.SignalType, (int) EnumType.CodeRadioSignal, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioFrequencyArea.Equipment, (int) ObjectType.EquipmentChoice, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioFrequencyArea.Sector, (int) ObjectType.CircleSector, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioFrequencyArea.Extent, (int) ObjectType.Surface, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
