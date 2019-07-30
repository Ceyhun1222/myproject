using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class FlightConditionElement : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FlightConditionElement; }
		}
		
		public uint? Index
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyFlightConditionElement.Index); }
			set { SetNullableFieldValue <uint> ((int) PropertyFlightConditionElement.Index, value); }
		}
		
		public FlightConditionElementChoice FlightCondition
		{
			get { return GetObject <FlightConditionElementChoice> ((int) PropertyFlightConditionElement.FlightCondition); }
			set { SetValue ((int) PropertyFlightConditionElement.FlightCondition, value); }
		}
		
		public FlightConditionCircumstance OperationalCondition
		{
			get { return GetObject <FlightConditionCircumstance> ((int) PropertyFlightConditionElement.OperationalCondition); }
			set { SetValue ((int) PropertyFlightConditionElement.OperationalCondition, value); }
		}
		
		public List <FlightRestrictionLevel> FlightLevel
		{
			get { return GetObjectList <FlightRestrictionLevel> ((int) PropertyFlightConditionElement.FlightLevel); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFlightConditionElement
	{
		Index = PropertyAObject.NEXT_CLASS,
		FlightCondition,
		OperationalCondition,
		FlightLevel,
		NEXT_CLASS
	}
	
	public static class MetadataFlightConditionElement
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFlightConditionElement ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFlightConditionElement.Index, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElement.FlightCondition, (int) ObjectType.FlightConditionElementChoice, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElement.OperationalCondition, (int) ObjectType.FlightConditionCircumstance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionElement.FlightLevel, (int) ObjectType.FlightRestrictionLevel, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
