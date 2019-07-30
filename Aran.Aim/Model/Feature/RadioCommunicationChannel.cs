using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class RadioCommunicationChannel : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RadioCommunicationChannel; }
		}
		
		public CodeCommunicationMode? Mode
		{
			get { return GetNullableFieldValue <CodeCommunicationMode> ((int) PropertyRadioCommunicationChannel.Mode); }
			set { SetNullableFieldValue <CodeCommunicationMode> ((int) PropertyRadioCommunicationChannel.Mode, value); }
		}
		
		public CodeFacilityRanking? Rank
		{
			get { return GetNullableFieldValue <CodeFacilityRanking> ((int) PropertyRadioCommunicationChannel.Rank); }
			set { SetNullableFieldValue <CodeFacilityRanking> ((int) PropertyRadioCommunicationChannel.Rank, value); }
		}
		
		public ValFrequency FrequencyTransmission
		{
			get { return (ValFrequency ) GetValue ((int) PropertyRadioCommunicationChannel.FrequencyTransmission); }
			set { SetValue ((int) PropertyRadioCommunicationChannel.FrequencyTransmission, value); }
		}
		
		public ValFrequency FrequencyReception
		{
			get { return (ValFrequency ) GetValue ((int) PropertyRadioCommunicationChannel.FrequencyReception); }
			set { SetValue ((int) PropertyRadioCommunicationChannel.FrequencyReception, value); }
		}
		
		public string Logon
		{
			get { return GetFieldValue <string> ((int) PropertyRadioCommunicationChannel.Logon); }
			set { SetFieldValue <string> ((int) PropertyRadioCommunicationChannel.Logon, value); }
		}
		
		public CodeRadioEmission? EmissionType
		{
			get { return GetNullableFieldValue <CodeRadioEmission> ((int) PropertyRadioCommunicationChannel.EmissionType); }
			set { SetNullableFieldValue <CodeRadioEmission> ((int) PropertyRadioCommunicationChannel.EmissionType, value); }
		}
		
		public bool? SelectiveCall
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRadioCommunicationChannel.SelectiveCall); }
			set { SetNullableFieldValue <bool> ((int) PropertyRadioCommunicationChannel.SelectiveCall, value); }
		}
		
		public bool? FlightChecked
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRadioCommunicationChannel.FlightChecked); }
			set { SetNullableFieldValue <bool> ((int) PropertyRadioCommunicationChannel.FlightChecked, value); }
		}
		
		public CodeCommunicationDirection? TrafficDirection
		{
			get { return GetNullableFieldValue <CodeCommunicationDirection> ((int) PropertyRadioCommunicationChannel.TrafficDirection); }
			set { SetNullableFieldValue <CodeCommunicationDirection> ((int) PropertyRadioCommunicationChannel.TrafficDirection, value); }
		}
		
		public List <ElevatedPoint> Location
		{
			get { return GetObjectList <ElevatedPoint> ((int) PropertyRadioCommunicationChannel.Location); }
		}
		
		public List <RadioCommunicationOperationalStatus> Availability
		{
			get { return GetObjectList <RadioCommunicationOperationalStatus> ((int) PropertyRadioCommunicationChannel.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRadioCommunicationChannel
	{
		Mode = PropertyFeature.NEXT_CLASS,
		Rank,
		FrequencyTransmission,
		FrequencyReception,
		Logon,
		EmissionType,
		SelectiveCall,
		FlightChecked,
		TrafficDirection,
		Location,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataRadioCommunicationChannel
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRadioCommunicationChannel ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRadioCommunicationChannel.Mode, (int) EnumType.CodeCommunicationMode, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioCommunicationChannel.Rank, (int) EnumType.CodeFacilityRanking, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioCommunicationChannel.FrequencyTransmission, (int) DataType.ValFrequency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioCommunicationChannel.FrequencyReception, (int) DataType.ValFrequency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioCommunicationChannel.Logon, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioCommunicationChannel.EmissionType, (int) EnumType.CodeRadioEmission, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioCommunicationChannel.SelectiveCall, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioCommunicationChannel.FlightChecked, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioCommunicationChannel.TrafficDirection, (int) EnumType.CodeCommunicationDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioCommunicationChannel.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadioCommunicationChannel.Availability, (int) ObjectType.RadioCommunicationOperationalStatus, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
