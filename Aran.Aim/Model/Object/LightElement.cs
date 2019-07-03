using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class LightElement : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.LightElement; }
		}
		
		public CodeColour? Colour
		{
			get { return GetNullableFieldValue <CodeColour> ((int) PropertyLightElement.Colour); }
			set { SetNullableFieldValue <CodeColour> ((int) PropertyLightElement.Colour, value); }
		}
		
		public CodeLightIntensity? IntensityLevel
		{
			get { return GetNullableFieldValue <CodeLightIntensity> ((int) PropertyLightElement.IntensityLevel); }
			set { SetNullableFieldValue <CodeLightIntensity> ((int) PropertyLightElement.IntensityLevel, value); }
		}
		
		public ValLightIntensity Intensity
		{
			get { return (ValLightIntensity ) GetValue ((int) PropertyLightElement.Intensity); }
			set { SetValue ((int) PropertyLightElement.Intensity, value); }
		}
		
		public CodeLightSource? Type
		{
			get { return GetNullableFieldValue <CodeLightSource> ((int) PropertyLightElement.Type); }
			set { SetNullableFieldValue <CodeLightSource> ((int) PropertyLightElement.Type, value); }
		}
		
		public ElevatedPoint Location
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyLightElement.Location); }
			set { SetValue ((int) PropertyLightElement.Location, value); }
		}
		
		public List <LightElementStatus> Availability
		{
			get { return GetObjectList <LightElementStatus> ((int) PropertyLightElement.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyLightElement
	{
		Colour = PropertyAObject.NEXT_CLASS,
		IntensityLevel,
		Intensity,
		Type,
		Location,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataLightElement
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataLightElement ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyLightElement.Colour, (int) EnumType.CodeColour, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLightElement.IntensityLevel, (int) EnumType.CodeLightIntensity, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLightElement.Intensity, (int) DataType.ValLightIntensity, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLightElement.Type, (int) EnumType.CodeLightSource, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLightElement.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLightElement.Availability, (int) ObjectType.LightElementStatus, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
