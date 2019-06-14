using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class GroundLightingAvailability : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.GroundLightingAvailability; }
		}
		
		public CodeStatusOperations? OperationalStatus
		{
			get { return GetNullableFieldValue <CodeStatusOperations> ((int) PropertyGroundLightingAvailability.OperationalStatus); }
			set { SetNullableFieldValue <CodeStatusOperations> ((int) PropertyGroundLightingAvailability.OperationalStatus, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyGroundLightingAvailability
	{
		OperationalStatus = PropertyPropertiesWithSchedule.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataGroundLightingAvailability
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataGroundLightingAvailability ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyGroundLightingAvailability.OperationalStatus, (int) EnumType.CodeStatusOperations, PropertyTypeCharacter.Nullable);
		}
	}
}
