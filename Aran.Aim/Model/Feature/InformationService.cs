using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class InformationService : Service
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.InformationService; }
		}
		
		public CodeServiceInformation? Type
		{
			get { return GetNullableFieldValue <CodeServiceInformation> ((int) PropertyInformationService.Type); }
			set { SetNullableFieldValue <CodeServiceInformation> ((int) PropertyInformationService.Type, value); }
		}
		
		public bool? Voice
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyInformationService.Voice); }
			set { SetNullableFieldValue <bool> ((int) PropertyInformationService.Voice, value); }
		}
		
		public bool? DataLink
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyInformationService.DataLink); }
			set { SetNullableFieldValue <bool> ((int) PropertyInformationService.DataLink, value); }
		}
		
		public bool? Recorded
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyInformationService.Recorded); }
			set { SetNullableFieldValue <bool> ((int) PropertyInformationService.Recorded, value); }
		}
		
		public List <FeatureRefObject> NavaidBroadcast
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyInformationService.NavaidBroadcast); }
		}
		
		public List <FeatureRefObject> ClientAirspace
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyInformationService.ClientAirspace); }
		}
		
		public List <FeatureRefObject> ClientAirport
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyInformationService.ClientAirport); }
		}
		
		public List <RoutePortion> ClientRoute
		{
			get { return GetObjectList <RoutePortion> ((int) PropertyInformationService.ClientRoute); }
		}
		
		public List <AbstractProcedureRefObject> ClientProcedure
		{
			get { return GetObjectList <AbstractProcedureRefObject> ((int) PropertyInformationService.ClientProcedure); }
		}
		
		public List <FeatureRefObject> ClientHolding
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyInformationService.ClientHolding); }
		}
		
		public List <FeatureRefObject> ClientAerialRefuelling
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyInformationService.ClientAerialRefuelling); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyInformationService
	{
		Type = PropertyService.NEXT_CLASS,
		Voice,
		DataLink,
		Recorded,
		NavaidBroadcast,
		ClientAirspace,
		ClientAirport,
		ClientRoute,
		ClientProcedure,
		ClientHolding,
		ClientAerialRefuelling,
		NEXT_CLASS
	}
	
	public static class MetadataInformationService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataInformationService ()
		{
			PropInfoList = MetadataService.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyInformationService.Type, (int) EnumType.CodeServiceInformation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInformationService.Voice, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInformationService.DataLink, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInformationService.Recorded, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInformationService.NavaidBroadcast, (int) FeatureType.VOR, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInformationService.ClientAirspace, (int) FeatureType.Airspace, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInformationService.ClientAirport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInformationService.ClientRoute, (int) ObjectType.RoutePortion, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInformationService.ClientProcedure, (int) ObjectType.AbstractProcedureRefObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInformationService.ClientHolding, (int) FeatureType.HoldingPattern, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyInformationService.ClientAerialRefuelling, (int) FeatureType.AerialRefuelling, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
