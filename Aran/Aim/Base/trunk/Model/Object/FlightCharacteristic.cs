using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class FlightCharacteristic : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FlightCharacteristic; }
		}
		
		public CodeFlight? Type
		{
			get { return GetNullableFieldValue <CodeFlight> ((int) PropertyFlightCharacteristic.Type); }
			set { SetNullableFieldValue <CodeFlight> ((int) PropertyFlightCharacteristic.Type, value); }
		}
		
		public CodeFlightRule? Rule
		{
			get { return GetNullableFieldValue <CodeFlightRule> ((int) PropertyFlightCharacteristic.Rule); }
			set { SetNullableFieldValue <CodeFlightRule> ((int) PropertyFlightCharacteristic.Rule, value); }
		}
		
		public CodeFlightStatus? Status
		{
			get { return GetNullableFieldValue <CodeFlightStatus> ((int) PropertyFlightCharacteristic.Status); }
			set { SetNullableFieldValue <CodeFlightStatus> ((int) PropertyFlightCharacteristic.Status, value); }
		}
		
		public CodeMilitaryStatus? Military
		{
			get { return GetNullableFieldValue <CodeMilitaryStatus> ((int) PropertyFlightCharacteristic.Military); }
			set { SetNullableFieldValue <CodeMilitaryStatus> ((int) PropertyFlightCharacteristic.Military, value); }
		}
		
		public CodeFlightOrigin? Origin
		{
			get { return GetNullableFieldValue <CodeFlightOrigin> ((int) PropertyFlightCharacteristic.Origin); }
			set { SetNullableFieldValue <CodeFlightOrigin> ((int) PropertyFlightCharacteristic.Origin, value); }
		}
		
		public CodeFlightPurpose? Purpose
		{
			get { return GetNullableFieldValue <CodeFlightPurpose> ((int) PropertyFlightCharacteristic.Purpose); }
			set { SetNullableFieldValue <CodeFlightPurpose> ((int) PropertyFlightCharacteristic.Purpose, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFlightCharacteristic
	{
		Type = PropertyAObject.NEXT_CLASS,
		Rule,
		Status,
		Military,
		Origin,
		Purpose,
		NEXT_CLASS
	}
	
	public static class MetadataFlightCharacteristic
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFlightCharacteristic ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFlightCharacteristic.Type, (int) EnumType.CodeFlight, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightCharacteristic.Rule, (int) EnumType.CodeFlightRule, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightCharacteristic.Status, (int) EnumType.CodeFlightStatus, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightCharacteristic.Military, (int) EnumType.CodeMilitaryStatus, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightCharacteristic.Origin, (int) EnumType.CodeFlightOrigin, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightCharacteristic.Purpose, (int) EnumType.CodeFlightPurpose, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
