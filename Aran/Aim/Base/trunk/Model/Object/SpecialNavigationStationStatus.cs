using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class SpecialNavigationStationStatus : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.SpecialNavigationStationStatus; }
		}
		
		public CodeStatusNavaid? OperationalStatus
		{
			get { return GetNullableFieldValue <CodeStatusNavaid> ((int) PropertySpecialNavigationStationStatus.OperationalStatus); }
			set { SetNullableFieldValue <CodeStatusNavaid> ((int) PropertySpecialNavigationStationStatus.OperationalStatus, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySpecialNavigationStationStatus
	{
		OperationalStatus = PropertyPropertiesWithSchedule.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataSpecialNavigationStationStatus
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSpecialNavigationStationStatus ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySpecialNavigationStationStatus.OperationalStatus, (int) EnumType.CodeStatusNavaid, PropertyTypeCharacter.Nullable);
		}
	}
}
