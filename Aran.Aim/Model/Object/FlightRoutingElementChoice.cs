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
	public class FlightRoutingElementChoice : ChoiceClass
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FlightRoutingElementChoice; }
		}
		
		public FlightRoutingElementChoiceChoice Choice
		{
			get { return (FlightRoutingElementChoiceChoice) RefType; }
		}

        [LinkedFeature(FeatureType.StandardInstrumentArrival)]
		public FeatureRef StandardInstrumentArrivalElement
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) FlightRoutingElementChoiceChoice.StandardInstrumentArrival;
			}
		}

        [LinkedFeature(FeatureType.Airspace)]
		public FeatureRef AirspaceElement
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) FlightRoutingElementChoiceChoice.Airspace;
			}
		}

        [LinkedFeature(ObjectType.SignificantPoint)]
		public SignificantPoint PointElement
		{
			get { return (SignificantPoint) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) FlightRoutingElementChoiceChoice.SignificantPoint;
			}
		}

        [LinkedFeature(ObjectType.DirectFlightSegment)]
		public DirectFlightSegment DirectFlightElement
		{
			get { return (DirectFlightSegment) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) FlightRoutingElementChoiceChoice.DirectFlightSegment;
			}
		}

        [LinkedFeature(FeatureType.StandardInstrumentDeparture)]
		public FeatureRef StandardInstrumentDepartureElement
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) FlightRoutingElementChoiceChoice.StandardInstrumentDeparture;
			}
		}
		

		public RoutePortion RoutePortionElement
		{
			get { return (RoutePortion) RefValue; }
			set
			{
				RefValue = value;
				RefType = (int) FlightRoutingElementChoiceChoice.RoutePortion;
			}
		}

        [LinkedFeature(FeatureType.AirportHeliport)]
		public FeatureRef AirportHeliportElement
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) FlightRoutingElementChoiceChoice.AirportHeliport;
			}
		}

        [LinkedFeature(FeatureType.AerialRefuelling)]
		public FeatureRef AerialRefuellingElement
		{
			get { return RefFeature; }
			set
			{
				RefFeature = value;
				RefType = (int) FlightRoutingElementChoiceChoice.AerialRefuelling;
			}
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFlightRoutingElementChoice
	{
		StandardInstrumentArrivalElement = PropertyAObject.NEXT_CLASS,
		AirspaceElement,
		PointElement,
		DirectFlightElement,
		StandardInstrumentDepartureElement,
		RoutePortionElement,
		AirportHeliportElement,
		AerialRefuellingElement,
		NEXT_CLASS
	}
	
	public static class MetadataFlightRoutingElementChoice
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFlightRoutingElementChoice ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFlightRoutingElementChoice.StandardInstrumentArrivalElement, (int) FeatureType.StandardInstrumentArrival, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRoutingElementChoice.AirspaceElement, (int) FeatureType.Airspace, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRoutingElementChoice.PointElement, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRoutingElementChoice.DirectFlightElement, (int) ObjectType.DirectFlightSegment, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRoutingElementChoice.StandardInstrumentDepartureElement, (int) FeatureType.StandardInstrumentDeparture, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRoutingElementChoice.RoutePortionElement, (int) ObjectType.RoutePortion, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRoutingElementChoice.AirportHeliportElement, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRoutingElementChoice.AerialRefuellingElement, (int) FeatureType.AerialRefuelling, PropertyTypeCharacter.Nullable);
		}
	}
}
