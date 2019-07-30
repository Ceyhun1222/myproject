using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class ManoeuvringAreaAvailability : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ManoeuvringAreaAvailability; }
		}
		
		public CodeStatusAirport? OperationalStatus
		{
			get { return GetNullableFieldValue <CodeStatusAirport> ((int) PropertyManoeuvringAreaAvailability.OperationalStatus); }
			set { SetNullableFieldValue <CodeStatusAirport> ((int) PropertyManoeuvringAreaAvailability.OperationalStatus, value); }
		}
		
		public CodeAirportWarning? Warning
		{
			get { return GetNullableFieldValue <CodeAirportWarning> ((int) PropertyManoeuvringAreaAvailability.Warning); }
			set { SetNullableFieldValue <CodeAirportWarning> ((int) PropertyManoeuvringAreaAvailability.Warning, value); }
		}
		
		public List <ManoeuvringAreaUsage> Usage
		{
			get { return GetObjectList <ManoeuvringAreaUsage> ((int) PropertyManoeuvringAreaAvailability.Usage); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyManoeuvringAreaAvailability
	{
		OperationalStatus = PropertyPropertiesWithSchedule.NEXT_CLASS,
		Warning,
		Usage,
		NEXT_CLASS
	}
	
	public static class MetadataManoeuvringAreaAvailability
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataManoeuvringAreaAvailability ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyManoeuvringAreaAvailability.OperationalStatus, (int) EnumType.CodeStatusAirport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyManoeuvringAreaAvailability.Warning, (int) EnumType.CodeAirportWarning, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyManoeuvringAreaAvailability.Usage, (int) ObjectType.ManoeuvringAreaUsage, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
