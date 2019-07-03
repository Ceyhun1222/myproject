using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class ServiceOperationalStatus : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ServiceOperationalStatus; }
		}
		
		public CodeStatusService? OperationalStatus
		{
			get { return GetNullableFieldValue <CodeStatusService> ((int) PropertyServiceOperationalStatus.OperationalStatus); }
			set { SetNullableFieldValue <CodeStatusService> ((int) PropertyServiceOperationalStatus.OperationalStatus, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyServiceOperationalStatus
	{
		OperationalStatus = PropertyPropertiesWithSchedule.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataServiceOperationalStatus
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataServiceOperationalStatus ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyServiceOperationalStatus.OperationalStatus, (int) EnumType.CodeStatusService, PropertyTypeCharacter.Nullable);
		}
	}
}
