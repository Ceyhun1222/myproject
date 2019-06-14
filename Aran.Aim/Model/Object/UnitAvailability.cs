using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class UnitAvailability : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.UnitAvailability; }
		}
		
		public CodeStatusOperations? OperationalStatus
		{
			get { return GetNullableFieldValue <CodeStatusOperations> ((int) PropertyUnitAvailability.OperationalStatus); }
			set { SetNullableFieldValue <CodeStatusOperations> ((int) PropertyUnitAvailability.OperationalStatus, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyUnitAvailability
	{
		OperationalStatus = PropertyPropertiesWithSchedule.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataUnitAvailability
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataUnitAvailability ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyUnitAvailability.OperationalStatus, (int) EnumType.CodeStatusOperations, PropertyTypeCharacter.Nullable);
		}
	}
}
