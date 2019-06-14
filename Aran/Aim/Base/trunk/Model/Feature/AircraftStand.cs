using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AircraftStand : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AircraftStand; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyAircraftStand.Designator); }
			set { SetFieldValue <string> ((int) PropertyAircraftStand.Designator, value); }
		}
		
		public CodeAircraftStand? Type
		{
			get { return GetNullableFieldValue <CodeAircraftStand> ((int) PropertyAircraftStand.Type); }
			set { SetNullableFieldValue <CodeAircraftStand> ((int) PropertyAircraftStand.Type, value); }
		}
		
		public CodeVisualDockingGuidance? VisualDockingSystem
		{
			get { return GetNullableFieldValue <CodeVisualDockingGuidance> ((int) PropertyAircraftStand.VisualDockingSystem); }
			set { SetNullableFieldValue <CodeVisualDockingGuidance> ((int) PropertyAircraftStand.VisualDockingSystem, value); }
		}
		
		public SurfaceCharacteristics SurfaceProperties
		{
			get { return GetObject <SurfaceCharacteristics> ((int) PropertyAircraftStand.SurfaceProperties); }
			set { SetValue ((int) PropertyAircraftStand.SurfaceProperties, value); }
		}
		
		public ElevatedPoint Location
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyAircraftStand.Location); }
			set { SetValue ((int) PropertyAircraftStand.Location, value); }
		}
		
		public FeatureRef ApronLocation
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAircraftStand.ApronLocation); }
			set { SetValue ((int) PropertyAircraftStand.ApronLocation, value); }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyAircraftStand.Extent); }
			set { SetValue ((int) PropertyAircraftStand.Extent, value); }
		}
		
		public List <AircraftStandContamination> Contaminant
		{
			get { return GetObjectList <AircraftStandContamination> ((int) PropertyAircraftStand.Contaminant); }
		}
		
		public List <ApronAreaAvailability> Availability
		{
			get { return GetObjectList <ApronAreaAvailability> ((int) PropertyAircraftStand.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAircraftStand
	{
		Designator = PropertyFeature.NEXT_CLASS,
		Type,
		VisualDockingSystem,
		SurfaceProperties,
		Location,
		ApronLocation,
		Extent,
		Contaminant,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataAircraftStand
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAircraftStand ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAircraftStand.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftStand.Type, (int) EnumType.CodeAircraftStand, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftStand.VisualDockingSystem, (int) EnumType.CodeVisualDockingGuidance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftStand.SurfaceProperties, (int) ObjectType.SurfaceCharacteristics, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftStand.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftStand.ApronLocation, (int) FeatureType.ApronElement, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftStand.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftStand.Contaminant, (int) ObjectType.AircraftStandContamination, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAircraftStand.Availability, (int) ObjectType.ApronAreaAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
