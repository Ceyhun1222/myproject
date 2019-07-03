using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class ArrestingGear : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.ArrestingGear; }
		}
		
		public CodeStatusOperations? Status
		{
			get { return GetNullableFieldValue <CodeStatusOperations> ((int) PropertyArrestingGear.Status); }
			set { SetNullableFieldValue <CodeStatusOperations> ((int) PropertyArrestingGear.Status, value); }
		}
		
		public ValDistance Length
		{
			get { return (ValDistance ) GetValue ((int) PropertyArrestingGear.Length); }
			set { SetValue ((int) PropertyArrestingGear.Length, value); }
		}
		
		public ValDistance Width
		{
			get { return (ValDistance ) GetValue ((int) PropertyArrestingGear.Width); }
			set { SetValue ((int) PropertyArrestingGear.Width, value); }
		}
		
		public CodeArrestingGearEngageDevice? EngageDevice
		{
			get { return GetNullableFieldValue <CodeArrestingGearEngageDevice> ((int) PropertyArrestingGear.EngageDevice); }
			set { SetNullableFieldValue <CodeArrestingGearEngageDevice> ((int) PropertyArrestingGear.EngageDevice, value); }
		}
		
		public CodeArrestingGearEnergyAbsorb? AbsorbType
		{
			get { return GetNullableFieldValue <CodeArrestingGearEnergyAbsorb> ((int) PropertyArrestingGear.AbsorbType); }
			set { SetNullableFieldValue <CodeArrestingGearEnergyAbsorb> ((int) PropertyArrestingGear.AbsorbType, value); }
		}
		
		public bool? Bidirectional
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyArrestingGear.Bidirectional); }
			set { SetNullableFieldValue <bool> ((int) PropertyArrestingGear.Bidirectional, value); }
		}
		
		public ValDistance Location
		{
			get { return (ValDistance ) GetValue ((int) PropertyArrestingGear.Location); }
			set { SetValue ((int) PropertyArrestingGear.Location, value); }
		}
		
		public List <FeatureRefObject> RunwayDirection
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyArrestingGear.RunwayDirection); }
		}
		
		public SurfaceCharacteristics SurfaceProperties
		{
			get { return GetObject <SurfaceCharacteristics> ((int) PropertyArrestingGear.SurfaceProperties); }
			set { SetValue ((int) PropertyArrestingGear.SurfaceProperties, value); }
		}
		
		public ArrestingGearExtent Extent
		{
			get { return GetObject <ArrestingGearExtent> ((int) PropertyArrestingGear.Extent); }
			set { SetValue ((int) PropertyArrestingGear.Extent, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyArrestingGear
	{
		Status = PropertyFeature.NEXT_CLASS,
		Length,
		Width,
		EngageDevice,
		AbsorbType,
		Bidirectional,
		Location,
		RunwayDirection,
		SurfaceProperties,
		Extent,
		NEXT_CLASS
	}
	
	public static class MetadataArrestingGear
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataArrestingGear ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyArrestingGear.Status, (int) EnumType.CodeStatusOperations, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyArrestingGear.Length, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyArrestingGear.Width, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyArrestingGear.EngageDevice, (int) EnumType.CodeArrestingGearEngageDevice, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyArrestingGear.AbsorbType, (int) EnumType.CodeArrestingGearEnergyAbsorb, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyArrestingGear.Bidirectional, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyArrestingGear.Location, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyArrestingGear.RunwayDirection, (int) FeatureType.RunwayDirection, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyArrestingGear.SurfaceProperties, (int) ObjectType.SurfaceCharacteristics, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyArrestingGear.Extent, (int) ObjectType.ArrestingGearExtent, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
