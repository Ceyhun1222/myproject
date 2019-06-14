using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class RadioCommunicationOperationalStatus : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.RadioCommunicationOperationalStatus; }
		}
		
		public CodeStatusService? OperationalStatus
		{
			get { return GetNullableFieldValue <CodeStatusService> ((int) PropertyRadioCommunicationOperationalStatus.OperationalStatus); }
			set { SetNullableFieldValue <CodeStatusService> ((int) PropertyRadioCommunicationOperationalStatus.OperationalStatus, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRadioCommunicationOperationalStatus
	{
		OperationalStatus = PropertyPropertiesWithSchedule.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataRadioCommunicationOperationalStatus
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRadioCommunicationOperationalStatus ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRadioCommunicationOperationalStatus.OperationalStatus, (int) EnumType.CodeStatusService, PropertyTypeCharacter.Nullable);
		}
	}
}
