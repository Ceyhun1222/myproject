using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class AirspaceActivation : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AirspaceActivation; }
		}
		
		public CodeAirspaceActivity? Activity
		{
			get { return GetNullableFieldValue <CodeAirspaceActivity> ((int) PropertyAirspaceActivation.Activity); }
			set { SetNullableFieldValue <CodeAirspaceActivity> ((int) PropertyAirspaceActivation.Activity, value); }
		}
		
		public CodeStatusAirspace? Status
		{
			get { return GetNullableFieldValue <CodeStatusAirspace> ((int) PropertyAirspaceActivation.Status); }
			set { SetNullableFieldValue <CodeStatusAirspace> ((int) PropertyAirspaceActivation.Status, value); }
		}
		
		public List <AirspaceLayer> Levels
		{
			get { return GetObjectList <AirspaceLayer> ((int) PropertyAirspaceActivation.Levels); }
		}
		
		public List <FeatureRefObject> User
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyAirspaceActivation.User); }
		}
		
		public List <AircraftCharacteristic> Aircraft
		{
			get { return GetObjectList <AircraftCharacteristic> ((int) PropertyAirspaceActivation.Aircraft); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirspaceActivation
	{
		Activity = PropertyPropertiesWithSchedule.NEXT_CLASS,
		Status,
		Levels,
		User,
		Aircraft,
		NEXT_CLASS
	}
	
	public static class MetadataAirspaceActivation
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirspaceActivation ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirspaceActivation.Activity, (int) EnumType.CodeAirspaceActivity, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceActivation.Status, (int) EnumType.CodeStatusAirspace, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceActivation.Levels, (int) ObjectType.AirspaceLayer, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceActivation.User, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirspaceActivation.Aircraft, (int) ObjectType.AircraftCharacteristic, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
