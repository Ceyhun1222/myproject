using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class FlightConditionCombination : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FlightConditionCombination; }
		}
		
		public CodeFlowConditionOperation? LogicalOperator
		{
			get { return GetNullableFieldValue <CodeFlowConditionOperation> ((int) PropertyFlightConditionCombination.LogicalOperator); }
			set { SetNullableFieldValue <CodeFlowConditionOperation> ((int) PropertyFlightConditionCombination.LogicalOperator, value); }
		}
		
		public List <FlightConditionElement> Element
		{
			get { return GetObjectList <FlightConditionElement> ((int) PropertyFlightConditionCombination.Element); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFlightConditionCombination
	{
		LogicalOperator = PropertyPropertiesWithSchedule.NEXT_CLASS,
		Element,
		NEXT_CLASS
	}
	
	public static class MetadataFlightConditionCombination
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFlightConditionCombination ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFlightConditionCombination.LogicalOperator, (int) EnumType.CodeFlowConditionOperation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionCombination.Element, (int) ObjectType.FlightConditionElement, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
