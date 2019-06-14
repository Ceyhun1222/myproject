using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public abstract class AirportHeliportProtectionArea : Feature
	{
		public virtual AirportHeliportProtectionAreaType AirportHeliportProtectionAreaType 
		{
			get { return (AirportHeliportProtectionAreaType) FeatureType; }
		}
		
		public ValDistance Width
		{
			get { return (ValDistance ) GetValue ((int) PropertyAirportHeliportProtectionArea.Width); }
			set { SetValue ((int) PropertyAirportHeliportProtectionArea.Width, value); }
		}
		
		public ValDistance Length
		{
			get { return (ValDistance ) GetValue ((int) PropertyAirportHeliportProtectionArea.Length); }
			set { SetValue ((int) PropertyAirportHeliportProtectionArea.Length, value); }
		}
		
		public bool? Lighting
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAirportHeliportProtectionArea.Lighting); }
			set { SetNullableFieldValue <bool> ((int) PropertyAirportHeliportProtectionArea.Lighting, value); }
		}
		
		public bool? ObstacleFree
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAirportHeliportProtectionArea.ObstacleFree); }
			set { SetNullableFieldValue <bool> ((int) PropertyAirportHeliportProtectionArea.ObstacleFree, value); }
		}
		
		public SurfaceCharacteristics SurfaceProperties
		{
			get { return GetObject <SurfaceCharacteristics> ((int) PropertyAirportHeliportProtectionArea.SurfaceProperties); }
			set { SetValue ((int) PropertyAirportHeliportProtectionArea.SurfaceProperties, value); }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyAirportHeliportProtectionArea.Extent); }
			set { SetValue ((int) PropertyAirportHeliportProtectionArea.Extent, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirportHeliportProtectionArea
	{
		Width = PropertyFeature.NEXT_CLASS,
		Length,
		Lighting,
		ObstacleFree,
		SurfaceProperties,
		Extent,
		NEXT_CLASS
	}
	
	public static class MetadataAirportHeliportProtectionArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirportHeliportProtectionArea ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirportHeliportProtectionArea.Width, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliportProtectionArea.Length, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliportProtectionArea.Lighting, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliportProtectionArea.ObstacleFree, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliportProtectionArea.SurfaceProperties, (int) ObjectType.SurfaceCharacteristics, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliportProtectionArea.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
