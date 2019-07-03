using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AeronauticalGroundLight : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AeronauticalGroundLight; }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyAeronauticalGroundLight.Name); }
			set { SetFieldValue <string> ((int) PropertyAeronauticalGroundLight.Name, value); }
		}
		
		public CodeGroundLighting? Type
		{
			get { return GetNullableFieldValue <CodeGroundLighting> ((int) PropertyAeronauticalGroundLight.Type); }
			set { SetNullableFieldValue <CodeGroundLighting> ((int) PropertyAeronauticalGroundLight.Type, value); }
		}
		
		public CodeColour? Colour
		{
			get { return GetNullableFieldValue <CodeColour> ((int) PropertyAeronauticalGroundLight.Colour); }
			set { SetNullableFieldValue <CodeColour> ((int) PropertyAeronauticalGroundLight.Colour, value); }
		}
		
		public bool? Flashing
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAeronauticalGroundLight.Flashing); }
			set { SetNullableFieldValue <bool> ((int) PropertyAeronauticalGroundLight.Flashing, value); }
		}
		
		public FeatureRef StructureBeacon
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAeronauticalGroundLight.StructureBeacon); }
			set { SetValue ((int) PropertyAeronauticalGroundLight.StructureBeacon, value); }
		}
		
		public FeatureRef AerodromeBeacon
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAeronauticalGroundLight.AerodromeBeacon); }
			set { SetValue ((int) PropertyAeronauticalGroundLight.AerodromeBeacon, value); }
		}
		
		public ElevatedPoint Location
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyAeronauticalGroundLight.Location); }
			set { SetValue ((int) PropertyAeronauticalGroundLight.Location, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAeronauticalGroundLight
	{
		Name = PropertyFeature.NEXT_CLASS,
		Type,
		Colour,
		Flashing,
		StructureBeacon,
		AerodromeBeacon,
		Location,
		NEXT_CLASS
	}
	
	public static class MetadataAeronauticalGroundLight
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAeronauticalGroundLight ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAeronauticalGroundLight.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAeronauticalGroundLight.Type, (int) EnumType.CodeGroundLighting, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAeronauticalGroundLight.Colour, (int) EnumType.CodeColour, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAeronauticalGroundLight.Flashing, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAeronauticalGroundLight.StructureBeacon, (int) FeatureType.VerticalStructure, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAeronauticalGroundLight.AerodromeBeacon, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAeronauticalGroundLight.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
