using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Model.Attribute;

namespace Aran.Aim.Features
{
	public class FlightConditionElementChoice : ChoiceClass
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FlightConditionElementChoice; }
		}
		
		public FlightConditionElementChoiceChoice Choice
		{
			get { return (FlightConditionElementChoiceChoice) RefType; }
		}

        [LinkedFeature(FeatureType.AirportHeliport)]
		public FeatureRef AirportHeliportCondition
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) FlightConditionElementChoiceChoice.AirportHeliport;
			}
		}

         [LinkedFeature(FeatureType.StandardInstrumentDeparture)]
		public FeatureRef StandardInstrumentDepartureCondition
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) FlightConditionElementChoiceChoice.StandardInstrumentDeparture;
			}
		}
		
		public RoutePortion RoutePortionCondition
		{
			get { return (RoutePortion) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) FlightConditionElementChoiceChoice.RoutePortion;
			}
		}

         [LinkedFeature(FeatureType.OrganisationAuthority)]
		public FeatureRef OrganisationCondition
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) FlightConditionElementChoiceChoice.OrganisationAuthority;
			}
		}
		
		public SignificantPoint SignificantPointCondition
		{
			get { return (SignificantPoint) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) FlightConditionElementChoiceChoice.SignificantPoint;
			}
		}
		
		public DirectFlight DirectFlightCondition
		{
			get { return (DirectFlight) GetChoiceAbstractObject (AbstractType.DirectFlight); }
			set
			{
				RefValue = value;
				RefType = (int) FlightConditionElementChoiceChoice.DirectFlight;
			}
		}
		
		public AircraftCharacteristic Aircraft
		{
			get { return (AircraftCharacteristic) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) FlightConditionElementChoiceChoice.AircraftCharacteristic;
			}
		}

         [LinkedFeature(FeatureType.AirspaceBorderCrossing)]
		public FeatureRef BorderCrossingCondition
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) FlightConditionElementChoiceChoice.AirspaceBorderCrossing;
			}
		}

         [LinkedFeature(FeatureType.Airspace)]
		public FeatureRef AirspaceCondition
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) FlightConditionElementChoiceChoice.Airspace;
			}
		}
		
		public FlightCharacteristic Flight
		{
			get { return (FlightCharacteristic) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) FlightConditionElementChoiceChoice.FlightCharacteristic;
			}
		}

         [LinkedFeature(FeatureType.StandardInstrumentArrival)]
		public FeatureRef StandardInstrumentArrivalCondition
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) FlightConditionElementChoiceChoice.StandardInstrumentArrival;
			}
		}
		
		public FlightConditionCombination Operand
		{
			get { return (FlightConditionCombination) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) FlightConditionElementChoiceChoice.FlightConditionCombination;
			}
		}
		
		public Meteorology Weather
		{
			get { return (Meteorology) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) FlightConditionElementChoiceChoice.Meteorology;
			}
		}

        [LinkedFeature(FeatureType.AerialRefuelling)]
		public FeatureRef AerialRefuellingCondition
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) FlightConditionElementChoiceChoice.AerialRefuelling;
			}
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFlightConditionElementChoice
	{
		AirportHeliportCondition = PropertyAObject.NEXT_CLASS,
		StandardInstrumentDepartureCondition,
		RoutePortionCondition,
		OrganisationCondition,
		SignificantPointCondition,
		DirectFlightCondition,
		Aircraft,
		BorderCrossingCondition,
		AirspaceCondition,
		Flight,
		StandardInstrumentArrivalCondition,
		Operand,
		Weather,
		AerialRefuellingCondition,
		NEXT_CLASS
	}
	
	public static class MetadataFlightConditionElementChoice
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFlightConditionElementChoice ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFlightConditionElementChoice.AirportHeliportCondition, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElementChoice.StandardInstrumentDepartureCondition, (int) FeatureType.StandardInstrumentDeparture, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElementChoice.RoutePortionCondition, (int) ObjectType.RoutePortion, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElementChoice.OrganisationCondition, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElementChoice.SignificantPointCondition, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElementChoice.DirectFlightCondition, (int) AbstractType.DirectFlight, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElementChoice.Aircraft, (int) ObjectType.AircraftCharacteristic, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElementChoice.BorderCrossingCondition, (int) FeatureType.AirspaceBorderCrossing, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElementChoice.AirspaceCondition, (int) FeatureType.Airspace, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElementChoice.Flight, (int) ObjectType.FlightCharacteristic, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElementChoice.StandardInstrumentArrivalCondition, (int) FeatureType.StandardInstrumentArrival, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElementChoice.Operand, (int) ObjectType.FlightConditionCombination, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElementChoice.Weather, (int) ObjectType.Meteorology, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElementChoice.AerialRefuellingCondition, (int) FeatureType.AerialRefuelling, PropertyTypeCharacter.Nullable);
		}
	}
}
