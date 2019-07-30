using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class ApronAreaAvailability : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ApronAreaAvailability; }
		}
		
		public CodeStatusAirport? OperationalStatus
		{
			get { return GetNullableFieldValue <CodeStatusAirport> ((int) PropertyApronAreaAvailability.OperationalStatus); }
			set { SetNullableFieldValue <CodeStatusAirport> ((int) PropertyApronAreaAvailability.OperationalStatus, value); }
		}
		
		public CodeAirportWarning? Warning
		{
			get { return GetNullableFieldValue <CodeAirportWarning> ((int) PropertyApronAreaAvailability.Warning); }
			set { SetNullableFieldValue <CodeAirportWarning> ((int) PropertyApronAreaAvailability.Warning, value); }
		}
		
		public List <ApronAreaUsage> Usage
		{
			get { return GetObjectList <ApronAreaUsage> ((int) PropertyApronAreaAvailability.Usage); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyApronAreaAvailability
	{
		OperationalStatus = PropertyPropertiesWithSchedule.NEXT_CLASS,
		Warning,
		Usage,
		NEXT_CLASS
	}
	
	public static class MetadataApronAreaAvailability
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataApronAreaAvailability ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyApronAreaAvailability.OperationalStatus, (int) EnumType.CodeStatusAirport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronAreaAvailability.Warning, (int) EnumType.CodeAirportWarning, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronAreaAvailability.Usage, (int) ObjectType.ApronAreaUsage, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
