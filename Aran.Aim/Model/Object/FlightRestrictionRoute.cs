using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class FlightRestrictionRoute : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FlightRestrictionRoute; }
		}
		
		public bool? PriorPermission
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyFlightRestrictionRoute.PriorPermission); }
			set { SetNullableFieldValue <bool> ((int) PropertyFlightRestrictionRoute.PriorPermission, value); }
		}
		
		public List <FlightRoutingElement> RouteElement
		{
			get { return GetObjectList <FlightRoutingElement> ((int) PropertyFlightRestrictionRoute.RouteElement); }
		}
		
		public List <ContactInformation> Contact
		{
			get { return GetObjectList <ContactInformation> ((int) PropertyFlightRestrictionRoute.Contact); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFlightRestrictionRoute
	{
		PriorPermission = PropertyAObject.NEXT_CLASS,
		RouteElement,
		Contact,
		NEXT_CLASS
	}
	
	public static class MetadataFlightRestrictionRoute
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFlightRestrictionRoute ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFlightRestrictionRoute.PriorPermission, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRestrictionRoute.RouteElement, (int) ObjectType.FlightRoutingElement, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRestrictionRoute.Contact, (int) ObjectType.ContactInformation, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
