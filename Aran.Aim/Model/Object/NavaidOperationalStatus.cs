using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class NavaidOperationalStatus : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.NavaidOperationalStatus; }
		}
		
		public CodeStatusNavaid? OperationalStatus
		{
			get { return GetNullableFieldValue <CodeStatusNavaid> ((int) PropertyNavaidOperationalStatus.OperationalStatus); }
			set { SetNullableFieldValue <CodeStatusNavaid> ((int) PropertyNavaidOperationalStatus.OperationalStatus, value); }
		}
		
		public CodeRadioSignal? SignalType
		{
			get { return GetNullableFieldValue <CodeRadioSignal> ((int) PropertyNavaidOperationalStatus.SignalType); }
			set { SetNullableFieldValue <CodeRadioSignal> ((int) PropertyNavaidOperationalStatus.SignalType, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNavaidOperationalStatus
	{
		OperationalStatus = PropertyPropertiesWithSchedule.NEXT_CLASS,
		SignalType,
		NEXT_CLASS
	}
	
	public static class MetadataNavaidOperationalStatus
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNavaidOperationalStatus ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNavaidOperationalStatus.OperationalStatus, (int) EnumType.CodeStatusNavaid, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidOperationalStatus.SignalType, (int) EnumType.CodeRadioSignal, PropertyTypeCharacter.Nullable);
		}
	}
}
