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
	public class AirTrafficControlService : TrafficSeparationService
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AirTrafficControlService; }
		}
		
		public CodeServiceATC? Type
		{
			get { return GetNullableFieldValue <CodeServiceATC> ((int) PropertyAirTrafficControlService.Type); }
			set { SetNullableFieldValue <CodeServiceATC> ((int) PropertyAirTrafficControlService.Type, value); }
		}
		
		public List <FeatureRefObject> ClientAirport
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyAirTrafficControlService.ClientAirport); }
		}
		
		public List <FeatureRefObject> ClientAirspace
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyAirTrafficControlService.ClientAirspace); }
		}
		
		public List <RoutePortion> ClientRoute
		{
			get { return GetObjectList <RoutePortion> ((int) PropertyAirTrafficControlService.ClientRoute); }
		}
		
		public List <AbstractProcedureRefObject> ClientProcedure
		{
			get { return GetObjectList <AbstractProcedureRefObject> ((int) PropertyAirTrafficControlService.ClientProcedure); }
		}
		
		public List <FeatureRefObject> ClientHolding
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyAirTrafficControlService.ClientHolding); }
		}
		
		public List <FeatureRefObject> ClientAerialRefuelling
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyAirTrafficControlService.ClientAerialRefuelling); }
		}
		
		public FeatureRef AircraftLocator
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAirTrafficControlService.AircraftLocator); }
			set { SetValue ((int) PropertyAirTrafficControlService.AircraftLocator, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirTrafficControlService
	{
		Type = PropertyTrafficSeparationService.NEXT_CLASS,
		ClientAirport,
		ClientAirspace,
		ClientRoute,
		ClientProcedure,
		ClientHolding,
		ClientAerialRefuelling,
		AircraftLocator,
		NEXT_CLASS
	}
	
	public static class MetadataAirTrafficControlService
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirTrafficControlService ()
		{
			PropInfoList = MetadataTrafficSeparationService.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirTrafficControlService.Type, (int) EnumType.CodeServiceATC, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirTrafficControlService.ClientAirport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirTrafficControlService.ClientAirspace, (int) FeatureType.Airspace, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirTrafficControlService.ClientRoute, (int) ObjectType.RoutePortion, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirTrafficControlService.ClientProcedure, (int) ObjectType.AbstractProcedureRefObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirTrafficControlService.ClientHolding, (int) FeatureType.HoldingPattern, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirTrafficControlService.ClientAerialRefuelling, (int) FeatureType.AerialRefuelling, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirTrafficControlService.AircraftLocator, (int) FeatureType.DirectionFinder, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
