using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class Taxiway : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.Taxiway; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyTaxiway.Designator); }
			set { SetFieldValue <string> ((int) PropertyTaxiway.Designator, value); }
		}
		
		public CodeTaxiway? Type
		{
			get { return GetNullableFieldValue <CodeTaxiway> ((int) PropertyTaxiway.Type); }
			set { SetNullableFieldValue <CodeTaxiway> ((int) PropertyTaxiway.Type, value); }
		}
		
		public ValDistance Width
		{
			get { return (ValDistance ) GetValue ((int) PropertyTaxiway.Width); }
			set { SetValue ((int) PropertyTaxiway.Width, value); }
		}
		
		public ValDistance WidthShoulder
		{
			get { return (ValDistance ) GetValue ((int) PropertyTaxiway.WidthShoulder); }
			set { SetValue ((int) PropertyTaxiway.WidthShoulder, value); }
		}
		
		public ValDistance Length
		{
			get { return (ValDistance ) GetValue ((int) PropertyTaxiway.Length); }
			set { SetValue ((int) PropertyTaxiway.Length, value); }
		}
		
		public bool? Abandoned
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyTaxiway.Abandoned); }
			set { SetNullableFieldValue <bool> ((int) PropertyTaxiway.Abandoned, value); }
		}
		
		public SurfaceCharacteristics SurfaceProperties
		{
			get { return GetObject <SurfaceCharacteristics> ((int) PropertyTaxiway.SurfaceProperties); }
			set { SetValue ((int) PropertyTaxiway.SurfaceProperties, value); }
		}
		
		public FeatureRef AssociatedAirportHeliport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTaxiway.AssociatedAirportHeliport); }
			set { SetValue ((int) PropertyTaxiway.AssociatedAirportHeliport, value); }
		}
		
		public List <TaxiwayContamination> Contaminant
		{
			get { return GetObjectList <TaxiwayContamination> ((int) PropertyTaxiway.Contaminant); }
		}
		
		public List <ManoeuvringAreaAvailability> Availability
		{
			get { return GetObjectList <ManoeuvringAreaAvailability> ((int) PropertyTaxiway.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTaxiway
	{
		Designator = PropertyFeature.NEXT_CLASS,
		Type,
		Width,
		WidthShoulder,
		Length,
		Abandoned,
		SurfaceProperties,
		AssociatedAirportHeliport,
		Contaminant,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataTaxiway
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTaxiway ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTaxiway.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiway.Type, (int) EnumType.CodeTaxiway, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiway.Width, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiway.WidthShoulder, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiway.Length, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiway.Abandoned, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiway.SurfaceProperties, (int) ObjectType.SurfaceCharacteristics, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiway.AssociatedAirportHeliport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiway.Contaminant, (int) ObjectType.TaxiwayContamination, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiway.Availability, (int) ObjectType.ManoeuvringAreaAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
