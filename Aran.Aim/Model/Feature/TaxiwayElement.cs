using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class TaxiwayElement : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.TaxiwayElement; }
		}
		
		public CodeTaxiwayElement? Type
		{
			get { return GetNullableFieldValue <CodeTaxiwayElement> ((int) PropertyTaxiwayElement.Type); }
			set { SetNullableFieldValue <CodeTaxiwayElement> ((int) PropertyTaxiwayElement.Type, value); }
		}
		
		public ValDistance Length
		{
			get { return (ValDistance ) GetValue ((int) PropertyTaxiwayElement.Length); }
			set { SetValue ((int) PropertyTaxiwayElement.Length, value); }
		}
		
		public ValDistance Width
		{
			get { return (ValDistance ) GetValue ((int) PropertyTaxiwayElement.Width); }
			set { SetValue ((int) PropertyTaxiwayElement.Width, value); }
		}
		
		public CodeGradeSeparation? GradeSeparation
		{
			get { return GetNullableFieldValue <CodeGradeSeparation> ((int) PropertyTaxiwayElement.GradeSeparation); }
			set { SetNullableFieldValue <CodeGradeSeparation> ((int) PropertyTaxiwayElement.GradeSeparation, value); }
		}
		
		public SurfaceCharacteristics SurfaceProperties
		{
			get { return GetObject <SurfaceCharacteristics> ((int) PropertyTaxiwayElement.SurfaceProperties); }
			set { SetValue ((int) PropertyTaxiwayElement.SurfaceProperties, value); }
		}
		
		public FeatureRef AssociatedTaxiway
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTaxiwayElement.AssociatedTaxiway); }
			set { SetValue ((int) PropertyTaxiwayElement.AssociatedTaxiway, value); }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyTaxiwayElement.Extent); }
			set { SetValue ((int) PropertyTaxiwayElement.Extent, value); }
		}
		
		public List <ManoeuvringAreaAvailability> Availability
		{
			get { return GetObjectList <ManoeuvringAreaAvailability> ((int) PropertyTaxiwayElement.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTaxiwayElement
	{
		Type = PropertyFeature.NEXT_CLASS,
		Length,
		Width,
		GradeSeparation,
		SurfaceProperties,
		AssociatedTaxiway,
		Extent,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataTaxiwayElement
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTaxiwayElement ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTaxiwayElement.Type, (int) EnumType.CodeTaxiwayElement, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiwayElement.Length, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiwayElement.Width, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiwayElement.GradeSeparation, (int) EnumType.CodeGradeSeparation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiwayElement.SurfaceProperties, (int) ObjectType.SurfaceCharacteristics, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiwayElement.AssociatedTaxiway, (int) FeatureType.Taxiway, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiwayElement.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiwayElement.Availability, (int) ObjectType.ManoeuvringAreaAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
