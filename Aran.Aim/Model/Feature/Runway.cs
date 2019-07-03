using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class Runway : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.Runway; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyRunway.Designator); }
			set { SetFieldValue <string> ((int) PropertyRunway.Designator, value); }
		}
		
		public CodeRunway? Type
		{
			get { return GetNullableFieldValue <CodeRunway> ((int) PropertyRunway.Type); }
			set { SetNullableFieldValue <CodeRunway> ((int) PropertyRunway.Type, value); }
		}
		
		public ValDistance NominalLength
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunway.NominalLength); }
			set { SetValue ((int) PropertyRunway.NominalLength, value); }
		}
		
		public ValDistance LengthAccuracy
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunway.LengthAccuracy); }
			set { SetValue ((int) PropertyRunway.LengthAccuracy, value); }
		}
		
		public ValDistance NominalWidth
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunway.NominalWidth); }
			set { SetValue ((int) PropertyRunway.NominalWidth, value); }
		}
		
		public ValDistance WidthAccuracy
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunway.WidthAccuracy); }
			set { SetValue ((int) PropertyRunway.WidthAccuracy, value); }
		}
		
		public ValDistance WidthShoulder
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunway.WidthShoulder); }
			set { SetValue ((int) PropertyRunway.WidthShoulder, value); }
		}
		
		public ValDistance LengthStrip
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunway.LengthStrip); }
			set { SetValue ((int) PropertyRunway.LengthStrip, value); }
		}
		
		public ValDistance WidthStrip
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunway.WidthStrip); }
			set { SetValue ((int) PropertyRunway.WidthStrip, value); }
		}
		
		public ValDistanceSigned LengthOffset
		{
			get { return (ValDistanceSigned ) GetValue ((int) PropertyRunway.LengthOffset); }
			set { SetValue ((int) PropertyRunway.LengthOffset, value); }
		}
		
		public ValDistanceSigned WidthOffset
		{
			get { return (ValDistanceSigned ) GetValue ((int) PropertyRunway.WidthOffset); }
			set { SetValue ((int) PropertyRunway.WidthOffset, value); }
		}
		
		public bool? Abandoned
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRunway.Abandoned); }
			set { SetNullableFieldValue <bool> ((int) PropertyRunway.Abandoned, value); }
		}
		
		public SurfaceCharacteristics SurfaceProperties
		{
			get { return GetObject <SurfaceCharacteristics> ((int) PropertyRunway.SurfaceProperties); }
			set { SetValue ((int) PropertyRunway.SurfaceProperties, value); }
		}
		
		public FeatureRef AssociatedAirportHeliport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRunway.AssociatedAirportHeliport); }
			set { SetValue ((int) PropertyRunway.AssociatedAirportHeliport, value); }
		}
		
		public List <RunwayContamination> OverallContaminant
		{
			get { return GetObjectList <RunwayContamination> ((int) PropertyRunway.OverallContaminant); }
		}
		
		public List <RunwaySectionContamination> AreaContaminant
		{
			get { return GetObjectList <RunwaySectionContamination> ((int) PropertyRunway.AreaContaminant); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunway
	{
		Designator = PropertyFeature.NEXT_CLASS,
		Type,
		NominalLength,
		LengthAccuracy,
		NominalWidth,
		WidthAccuracy,
		WidthShoulder,
		LengthStrip,
		WidthStrip,
		LengthOffset,
		WidthOffset,
		Abandoned,
		SurfaceProperties,
		AssociatedAirportHeliport,
		OverallContaminant,
		AreaContaminant,
		NEXT_CLASS
	}
	
	public static class MetadataRunway
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunway ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunway.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.Type, (int) EnumType.CodeRunway, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.NominalLength, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.LengthAccuracy, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.NominalWidth, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.WidthAccuracy, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.WidthShoulder, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.LengthStrip, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.WidthStrip, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.LengthOffset, (int) DataType.ValDistanceSigned, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.WidthOffset, (int) DataType.ValDistanceSigned, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.Abandoned, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.SurfaceProperties, (int) ObjectType.SurfaceCharacteristics, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.AssociatedAirportHeliport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.OverallContaminant, (int) ObjectType.RunwayContamination, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunway.AreaContaminant, (int) ObjectType.RunwaySectionContamination, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
