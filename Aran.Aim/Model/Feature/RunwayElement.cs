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
	public class RunwayElement : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RunwayElement; }
		}
		
		public CodeRunwayElement? Type
		{
			get { return GetNullableFieldValue <CodeRunwayElement> ((int) PropertyRunwayElement.Type); }
			set { SetNullableFieldValue <CodeRunwayElement> ((int) PropertyRunwayElement.Type, value); }
		}
		
		public ValDistance Length
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunwayElement.Length); }
			set { SetValue ((int) PropertyRunwayElement.Length, value); }
		}
		
		public ValDistance Width
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunwayElement.Width); }
			set { SetValue ((int) PropertyRunwayElement.Width, value); }
		}
		
		public CodeGradeSeparation? GradeSeparation
		{
			get { return GetNullableFieldValue <CodeGradeSeparation> ((int) PropertyRunwayElement.GradeSeparation); }
			set { SetNullableFieldValue <CodeGradeSeparation> ((int) PropertyRunwayElement.GradeSeparation, value); }
		}
		
		public SurfaceCharacteristics SurfaceProperties
		{
			get { return GetObject <SurfaceCharacteristics> ((int) PropertyRunwayElement.SurfaceProperties); }
			set { SetValue ((int) PropertyRunwayElement.SurfaceProperties, value); }
		}
		
		public List <FeatureRefObject> AssociatedRunway
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyRunwayElement.AssociatedRunway); }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyRunwayElement.Extent); }
			set { SetValue ((int) PropertyRunwayElement.Extent, value); }
		}
		
		public List <ManoeuvringAreaAvailability> Availability
		{
			get { return GetObjectList <ManoeuvringAreaAvailability> ((int) PropertyRunwayElement.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunwayElement
	{
		Type = PropertyFeature.NEXT_CLASS,
		Length,
		Width,
		GradeSeparation,
		SurfaceProperties,
		AssociatedRunway,
		Extent,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataRunwayElement
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunwayElement ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunwayElement.Type, (int) EnumType.CodeRunwayElement, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayElement.Length, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayElement.Width, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayElement.GradeSeparation, (int) EnumType.CodeGradeSeparation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayElement.SurfaceProperties, (int) ObjectType.SurfaceCharacteristics, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayElement.AssociatedRunway, (int) FeatureType.Runway, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayElement.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayElement.Availability, (int) ObjectType.ManoeuvringAreaAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
