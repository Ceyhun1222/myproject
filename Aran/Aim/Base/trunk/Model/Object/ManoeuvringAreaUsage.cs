using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class ManoeuvringAreaUsage : UsageCondition
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ManoeuvringAreaUsage; }
		}
		
		public CodeOperationManoeuvringArea? Operation
		{
			get { return GetNullableFieldValue <CodeOperationManoeuvringArea> ((int) PropertyManoeuvringAreaUsage.Operation); }
			set { SetNullableFieldValue <CodeOperationManoeuvringArea> ((int) PropertyManoeuvringAreaUsage.Operation, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyManoeuvringAreaUsage
	{
		Operation = PropertyUsageCondition.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataManoeuvringAreaUsage
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataManoeuvringAreaUsage ()
		{
			PropInfoList = MetadataUsageCondition.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyManoeuvringAreaUsage.Operation, (int) EnumType.CodeOperationManoeuvringArea, PropertyTypeCharacter.Nullable);
		}
	}
}
