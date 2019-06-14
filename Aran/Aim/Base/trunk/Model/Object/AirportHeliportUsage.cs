using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class AirportHeliportUsage : UsageCondition
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AirportHeliportUsage; }
		}
		
		public CodeOperationAirportHeliport? Operation
		{
			get { return GetNullableFieldValue <CodeOperationAirportHeliport> ((int) PropertyAirportHeliportUsage.Operation); }
			set { SetNullableFieldValue <CodeOperationAirportHeliport> ((int) PropertyAirportHeliportUsage.Operation, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirportHeliportUsage
	{
		Operation = PropertyUsageCondition.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataAirportHeliportUsage
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirportHeliportUsage ()
		{
			PropInfoList = MetadataUsageCondition.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirportHeliportUsage.Operation, (int) EnumType.CodeOperationAirportHeliport, PropertyTypeCharacter.Nullable);
		}
	}
}
