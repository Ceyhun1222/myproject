using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class AirportHeliportAvailability : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AirportHeliportAvailability; }
		}
		
		public CodeStatusAirport? OperationalStatus
		{
			get { return GetNullableFieldValue <CodeStatusAirport> ((int) PropertyAirportHeliportAvailability.OperationalStatus); }
			set { SetNullableFieldValue <CodeStatusAirport> ((int) PropertyAirportHeliportAvailability.OperationalStatus, value); }
		}
		
		public CodeAirportWarning? Warning
		{
			get { return GetNullableFieldValue <CodeAirportWarning> ((int) PropertyAirportHeliportAvailability.Warning); }
			set { SetNullableFieldValue <CodeAirportWarning> ((int) PropertyAirportHeliportAvailability.Warning, value); }
		}
		
		public List <AirportHeliportUsage> Usage
		{
			get { return GetObjectList <AirportHeliportUsage> ((int) PropertyAirportHeliportAvailability.Usage); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirportHeliportAvailability
	{
		OperationalStatus = PropertyPropertiesWithSchedule.NEXT_CLASS,
		Warning,
		Usage,
		NEXT_CLASS
	}
	
	public static class MetadataAirportHeliportAvailability
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirportHeliportAvailability ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirportHeliportAvailability.OperationalStatus, (int) EnumType.CodeStatusAirport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliportAvailability.Warning, (int) EnumType.CodeAirportWarning, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliportAvailability.Usage, (int) ObjectType.AirportHeliportUsage, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
