using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class RadarSystem : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RadarSystem; }
		}
		
		public CodeRadarService? Type
		{
			get { return GetNullableFieldValue <CodeRadarService> ((int) PropertyRadarSystem.Type); }
			set { SetNullableFieldValue <CodeRadarService> ((int) PropertyRadarSystem.Type, value); }
		}
		
		public string Model
		{
			get { return GetFieldValue <string> ((int) PropertyRadarSystem.Model); }
			set { SetFieldValue <string> ((int) PropertyRadarSystem.Model, value); }
		}
		
		public bool? GeneralTerrainMonitor
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRadarSystem.GeneralTerrainMonitor); }
			set { SetNullableFieldValue <bool> ((int) PropertyRadarSystem.GeneralTerrainMonitor, value); }
		}
		
		public string BroadcastIdentifier
		{
			get { return GetFieldValue <string> ((int) PropertyRadarSystem.BroadcastIdentifier); }
			set { SetFieldValue <string> ((int) PropertyRadarSystem.BroadcastIdentifier, value); }
		}
		
		public List <RadarComponent> RadarEquipment
		{
			get { return GetObjectList <RadarComponent> ((int) PropertyRadarSystem.RadarEquipment); }
		}
		
		public List <FeatureRefObject> Office
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyRadarSystem.Office); }
		}
		
		public FeatureRef AirportHeliport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRadarSystem.AirportHeliport); }
			set { SetValue ((int) PropertyRadarSystem.AirportHeliport, value); }
		}
		
		public List <FeatureRefObject> PARRunway
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyRadarSystem.PARRunway); }
		}
		
		public ElevatedPoint Location
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyRadarSystem.Location); }
			set { SetValue ((int) PropertyRadarSystem.Location, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRadarSystem
	{
		Type = PropertyFeature.NEXT_CLASS,
		Model,
		GeneralTerrainMonitor,
		BroadcastIdentifier,
		RadarEquipment,
		Office,
		AirportHeliport,
		PARRunway,
		Location,
		NEXT_CLASS
	}
	
	public static class MetadataRadarSystem
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRadarSystem ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRadarSystem.Type, (int) EnumType.CodeRadarService, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarSystem.Model, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarSystem.GeneralTerrainMonitor, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarSystem.BroadcastIdentifier, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarSystem.RadarEquipment, (int) ObjectType.RadarComponent, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarSystem.Office, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarSystem.AirportHeliport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarSystem.PARRunway, (int) FeatureType.Runway, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarSystem.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
