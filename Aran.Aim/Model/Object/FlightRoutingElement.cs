using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class FlightRoutingElement : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FlightRoutingElement; }
		}
		
		public uint? OrderNumber
		{
			get { return GetNullableFieldValue <uint> ((int) PropertyFlightRoutingElement.OrderNumber); }
			set { SetNullableFieldValue <uint> ((int) PropertyFlightRoutingElement.OrderNumber, value); }
		}
		
		public ValSpeed Speed
		{
			get { return (ValSpeed ) GetValue ((int) PropertyFlightRoutingElement.Speed); }
			set { SetValue ((int) PropertyFlightRoutingElement.Speed, value); }
		}
		
		public CodeSpeedReference? SpeedReference
		{
			get { return GetNullableFieldValue <CodeSpeedReference> ((int) PropertyFlightRoutingElement.SpeedReference); }
			set { SetNullableFieldValue <CodeSpeedReference> ((int) PropertyFlightRoutingElement.SpeedReference, value); }
		}
		
		public CodeComparison? SpeedCriteria
		{
			get { return GetNullableFieldValue <CodeComparison> ((int) PropertyFlightRoutingElement.SpeedCriteria); }
			set { SetNullableFieldValue <CodeComparison> ((int) PropertyFlightRoutingElement.SpeedCriteria, value); }
		}
		
		public List <FlightRestrictionLevel> FlightLevel
		{
			get { return GetObjectList <FlightRestrictionLevel> ((int) PropertyFlightRoutingElement.FlightLevel); }
		}
		
		public FlightRoutingElementChoice Element
		{
			get { return GetObject <FlightRoutingElementChoice> ((int) PropertyFlightRoutingElement.Element); }
			set { SetValue ((int) PropertyFlightRoutingElement.Element, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFlightRoutingElement
	{
		OrderNumber = PropertyAObject.NEXT_CLASS,
		Speed,
		SpeedReference,
		SpeedCriteria,
		FlightLevel,
		Element,
		NEXT_CLASS
	}
	
	public static class MetadataFlightRoutingElement
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFlightRoutingElement ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFlightRoutingElement.OrderNumber, (int) AimFieldType.SysUInt32, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRoutingElement.Speed, (int) DataType.ValSpeed, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRoutingElement.SpeedReference, (int) EnumType.CodeSpeedReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRoutingElement.SpeedCriteria, (int) EnumType.CodeComparison, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRoutingElement.FlightLevel, (int) ObjectType.FlightRestrictionLevel, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRoutingElement.Element, (int) ObjectType.FlightRoutingElementChoice, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
