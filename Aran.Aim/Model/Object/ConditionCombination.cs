using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class ConditionCombination : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ConditionCombination; }
		}
		
		public CodeLogicalOperator? LogicalOperator
		{
			get { return GetNullableFieldValue <CodeLogicalOperator> ((int) PropertyConditionCombination.LogicalOperator); }
			set { SetNullableFieldValue <CodeLogicalOperator> ((int) PropertyConditionCombination.LogicalOperator, value); }
		}
		
		public List <Meteorology> Weather
		{
			get { return GetObjectList <Meteorology> ((int) PropertyConditionCombination.Weather); }
		}
		
		public List <AircraftCharacteristic> Aircraft
		{
			get { return GetObjectList <AircraftCharacteristic> ((int) PropertyConditionCombination.Aircraft); }
		}
		
		public List <FlightCharacteristic> Flight
		{
			get { return GetObjectList <FlightCharacteristic> ((int) PropertyConditionCombination.Flight); }
		}
		
		public List <ConditionCombination> SubCondition
		{
			get { return GetObjectList <ConditionCombination> ((int) PropertyConditionCombination.SubCondition); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyConditionCombination
	{
		LogicalOperator = PropertyPropertiesWithSchedule.NEXT_CLASS,
		Weather,
		Aircraft,
		Flight,
		SubCondition,
		NEXT_CLASS
	}
	
	public static class MetadataConditionCombination
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataConditionCombination ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyConditionCombination.LogicalOperator, (int) EnumType.CodeLogicalOperator, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyConditionCombination.Weather, (int) ObjectType.Meteorology, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyConditionCombination.Aircraft, (int) ObjectType.AircraftCharacteristic, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyConditionCombination.Flight, (int) ObjectType.FlightCharacteristic, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyConditionCombination.SubCondition, (int) ObjectType.ConditionCombination, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
