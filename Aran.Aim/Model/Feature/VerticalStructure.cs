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
	public class VerticalStructure : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.VerticalStructure; }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyVerticalStructure.Name); }
			set { SetFieldValue <string> ((int) PropertyVerticalStructure.Name, value); }
		}
		
		public CodeVerticalStructure? Type
		{
			get { return GetNullableFieldValue <CodeVerticalStructure> ((int) PropertyVerticalStructure.Type); }
			set { SetNullableFieldValue <CodeVerticalStructure> ((int) PropertyVerticalStructure.Type, value); }
		}
		
		public bool? Lighted
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyVerticalStructure.Lighted); }
			set { SetNullableFieldValue <bool> ((int) PropertyVerticalStructure.Lighted, value); }
		}
		
		public bool? MarkingICAOStandard
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyVerticalStructure.MarkingICAOStandard); }
			set { SetNullableFieldValue <bool> ((int) PropertyVerticalStructure.MarkingICAOStandard, value); }
		}
		
		public bool? Group
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyVerticalStructure.Group); }
			set { SetNullableFieldValue <bool> ((int) PropertyVerticalStructure.Group, value); }
		}
		
		public ValDistance Length
		{
			get { return (ValDistance ) GetValue ((int) PropertyVerticalStructure.Length); }
			set { SetValue ((int) PropertyVerticalStructure.Length, value); }
		}
		
		public ValDistance Width
		{
			get { return (ValDistance ) GetValue ((int) PropertyVerticalStructure.Width); }
			set { SetValue ((int) PropertyVerticalStructure.Width, value); }
		}
		
		public ValDistance Radius
		{
			get { return (ValDistance ) GetValue ((int) PropertyVerticalStructure.Radius); }
			set { SetValue ((int) PropertyVerticalStructure.Radius, value); }
		}
		
		public bool? LightingICAOStandard
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyVerticalStructure.LightingICAOStandard); }
			set { SetNullableFieldValue <bool> ((int) PropertyVerticalStructure.LightingICAOStandard, value); }
		}
		
		public bool? SynchronisedLighting
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyVerticalStructure.SynchronisedLighting); }
			set { SetNullableFieldValue <bool> ((int) PropertyVerticalStructure.SynchronisedLighting, value); }
		}
		
		public FeatureRef Marker
		{
			get { return (FeatureRef ) GetValue ((int) PropertyVerticalStructure.Marker); }
			set { SetValue ((int) PropertyVerticalStructure.Marker, value); }
		}
		
		public List <VerticalStructurePart> Part
		{
			get { return GetObjectList <VerticalStructurePart> ((int) PropertyVerticalStructure.Part); }
		}
		
		public List <FeatureRefObject> HostedPassengerService
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyVerticalStructure.HostedPassengerService); }
		}
		
		public List <AbstractGroundLightSystemRefObject> SupportedGroundLight
		{
			get { return GetObjectList <AbstractGroundLightSystemRefObject> ((int) PropertyVerticalStructure.SupportedGroundLight); }
		}
		
		public List <AbstractNavaidEquipmentRefObject> HostedNavaidEquipment
		{
			get { return GetObjectList <AbstractNavaidEquipmentRefObject> ((int) PropertyVerticalStructure.HostedNavaidEquipment); }
		}
		
		public List <FeatureRefObject> HostedSpecialNavStation
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyVerticalStructure.HostedSpecialNavStation); }
		}
		
		public List <FeatureRefObject> HostedUnit
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyVerticalStructure.HostedUnit); }
		}
		
		public List <FeatureRefObject> HostedOrganisation
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyVerticalStructure.HostedOrganisation); }
		}
		
		public List <AbstractServiceRefObject> SupportedService
		{
			get { return GetObjectList <AbstractServiceRefObject> ((int) PropertyVerticalStructure.SupportedService); }
		}
		
		public List <VerticalStructureLightingStatus> LightingAvailability
		{
			get { return GetObjectList <VerticalStructureLightingStatus> ((int) PropertyVerticalStructure.LightingAvailability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyVerticalStructure
	{
		Name = PropertyFeature.NEXT_CLASS,
		Type,
		Lighted,
		MarkingICAOStandard,
		Group,
		Length,
		Width,
		Radius,
		LightingICAOStandard,
		SynchronisedLighting,
		Marker,
		Part,
		HostedPassengerService,
		SupportedGroundLight,
		HostedNavaidEquipment,
		HostedSpecialNavStation,
		HostedUnit,
		HostedOrganisation,
		SupportedService,
		LightingAvailability,
		NEXT_CLASS
	}
	
	public static class MetadataVerticalStructure
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataVerticalStructure ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyVerticalStructure.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.Type, (int) EnumType.CodeVerticalStructure, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.Lighted, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.MarkingICAOStandard, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.Group, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.Length, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.Width, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.Radius, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.LightingICAOStandard, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.SynchronisedLighting, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.Marker, (int) FeatureType.MarkerBeacon, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.Part, (int) ObjectType.VerticalStructurePart, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.HostedPassengerService, (int) FeatureType.PassengerService, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.SupportedGroundLight, (int) ObjectType.AbstractGroundLightSystemRefObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.HostedNavaidEquipment, (int) ObjectType.AbstractNavaidEquipmentRefObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.HostedSpecialNavStation, (int) FeatureType.SpecialNavigationStation, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.HostedUnit, (int) FeatureType.Unit, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.HostedOrganisation, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.SupportedService, (int) ObjectType.AbstractServiceRefObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVerticalStructure.LightingAvailability, (int) ObjectType.VerticalStructureLightingStatus, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
