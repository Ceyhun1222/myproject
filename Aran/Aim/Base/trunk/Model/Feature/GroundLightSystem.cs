using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public abstract class GroundLightSystem : Feature
	{
		public virtual GroundLightSystemType GroundLightSystemType 
		{
			get { return (GroundLightSystemType) FeatureType; }
		}
		
		public bool? EmergencyLighting
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyGroundLightSystem.EmergencyLighting); }
			set { SetNullableFieldValue <bool> ((int) PropertyGroundLightSystem.EmergencyLighting, value); }
		}
		
		public CodeLightIntensity? IntensityLevel
		{
			get { return GetNullableFieldValue <CodeLightIntensity> ((int) PropertyGroundLightSystem.IntensityLevel); }
			set { SetNullableFieldValue <CodeLightIntensity> ((int) PropertyGroundLightSystem.IntensityLevel, value); }
		}
		
		public CodeColour? Colour
		{
			get { return GetNullableFieldValue <CodeColour> ((int) PropertyGroundLightSystem.Colour); }
			set { SetNullableFieldValue <CodeColour> ((int) PropertyGroundLightSystem.Colour, value); }
		}
		
		public List <LightElement> Element
		{
			get { return GetObjectList <LightElement> ((int) PropertyGroundLightSystem.Element); }
		}
		
		public List <GroundLightingAvailability> Availability
		{
			get { return GetObjectList <GroundLightingAvailability> ((int) PropertyGroundLightSystem.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyGroundLightSystem
	{
		EmergencyLighting = PropertyFeature.NEXT_CLASS,
		IntensityLevel,
		Colour,
		Element,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataGroundLightSystem
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataGroundLightSystem ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyGroundLightSystem.EmergencyLighting, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGroundLightSystem.IntensityLevel, (int) EnumType.CodeLightIntensity, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGroundLightSystem.Colour, (int) EnumType.CodeColour, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGroundLightSystem.Element, (int) ObjectType.LightElement, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGroundLightSystem.Availability, (int) ObjectType.GroundLightingAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
