using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class RouteAvailability : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.RouteAvailability; }
		}
		
		public CodeDirection? Direction
		{
			get { return GetNullableFieldValue <CodeDirection> ((int) PropertyRouteAvailability.Direction); }
			set { SetNullableFieldValue <CodeDirection> ((int) PropertyRouteAvailability.Direction, value); }
		}
		
		public CodeCardinalDirection? CardinalDirection
		{
			get { return GetNullableFieldValue <CodeCardinalDirection> ((int) PropertyRouteAvailability.CardinalDirection); }
			set { SetNullableFieldValue <CodeCardinalDirection> ((int) PropertyRouteAvailability.CardinalDirection, value); }
		}
		
		public CodeRouteAvailability? Status
		{
			get { return GetNullableFieldValue <CodeRouteAvailability> ((int) PropertyRouteAvailability.Status); }
			set { SetNullableFieldValue <CodeRouteAvailability> ((int) PropertyRouteAvailability.Status, value); }
		}
		
		public List <AirspaceLayer> Levels
		{
			get { return GetObjectList <AirspaceLayer> ((int) PropertyRouteAvailability.Levels); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRouteAvailability
	{
		Direction = PropertyPropertiesWithSchedule.NEXT_CLASS,
		CardinalDirection,
		Status,
		Levels,
		NEXT_CLASS
	}
	
	public static class MetadataRouteAvailability
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRouteAvailability ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRouteAvailability.Direction, (int) EnumType.CodeDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteAvailability.CardinalDirection, (int) EnumType.CodeCardinalDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteAvailability.Status, (int) EnumType.CodeRouteAvailability, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteAvailability.Levels, (int) ObjectType.AirspaceLayer, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
