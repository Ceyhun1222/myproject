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
	public abstract class Service : Feature
	{
		public virtual ServiceType ServiceType 
		{
			get { return (ServiceType) FeatureType; }
		}
		
		public CodeFlightDestination? FlightOperations
		{
			get { return GetNullableFieldValue <CodeFlightDestination> ((int) PropertyService.FlightOperations); }
			set { SetNullableFieldValue <CodeFlightDestination> ((int) PropertyService.FlightOperations, value); }
		}
		
		public CodeFacilityRanking? Rank
		{
			get { return GetNullableFieldValue <CodeFacilityRanking> ((int) PropertyService.Rank); }
			set { SetNullableFieldValue <CodeFacilityRanking> ((int) PropertyService.Rank, value); }
		}
		
		public bool? CompliantICAO
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyService.CompliantICAO); }
			set { SetNullableFieldValue <bool> ((int) PropertyService.CompliantICAO, value); }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyService.Name); }
			set { SetFieldValue <string> ((int) PropertyService.Name, value); }
		}
		
		public ElevatedPoint Location
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyService.Location); }
			set { SetValue ((int) PropertyService.Location, value); }
		}
		
		public FeatureRef ServiceProvider
		{
			get { return (FeatureRef ) GetValue ((int) PropertyService.ServiceProvider); }
			set { SetValue ((int) PropertyService.ServiceProvider, value); }
		}
		
		public List <CallsignDetail> CallSign
		{
			get { return GetObjectList <CallsignDetail> ((int) PropertyService.CallSign); }
		}
		
		public List <FeatureRefObject> RadioCommunication
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyService.RadioCommunication); }
		}
		
		public List <ContactInformation> GroundCommunication
		{
			get { return GetObjectList <ContactInformation> ((int) PropertyService.GroundCommunication); }
		}
		
		public List <ServiceOperationalStatus> Availability
		{
			get { return GetObjectList <ServiceOperationalStatus> ((int) PropertyService.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyService
	{
		FlightOperations = PropertyFeature.NEXT_CLASS,
		Rank,
		CompliantICAO,
		Name,
		Location,
		ServiceProvider,
		CallSign,
		RadioCommunication,
		GroundCommunication,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataService ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyService.FlightOperations, (int) EnumType.CodeFlightDestination, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyService.Rank, (int) EnumType.CodeFacilityRanking, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyService.CompliantICAO, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyService.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyService.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyService.ServiceProvider, (int) FeatureType.Unit, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyService.CallSign, (int) ObjectType.CallsignDetail, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable, "call-sign");
			PropInfoList.Add (PropertyService.RadioCommunication, (int) FeatureType.RadioCommunicationChannel, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyService.GroundCommunication, (int) ObjectType.ContactInformation, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyService.Availability, (int) ObjectType.ServiceOperationalStatus, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
