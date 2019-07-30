using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class FlightConditionCircumstance : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.FlightConditionCircumstance; }
		}
		
		public bool? ReferenceLocation
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyFlightConditionCircumstance.ReferenceLocation); }
			set { SetNullableFieldValue <bool> ((int) PropertyFlightConditionCircumstance.ReferenceLocation, value); }
		}
		
		public CodeLocationQualifier? RelationWithLocation
		{
			get { return GetNullableFieldValue <CodeLocationQualifier> ((int) PropertyFlightConditionCircumstance.RelationWithLocation); }
			set { SetNullableFieldValue <CodeLocationQualifier> ((int) PropertyFlightConditionCircumstance.RelationWithLocation, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFlightConditionCircumstance
	{
		ReferenceLocation = PropertyAObject.NEXT_CLASS,
		RelationWithLocation,
		NEXT_CLASS
	}
	
	public static class MetadataFlightConditionCircumstance
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFlightConditionCircumstance ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFlightConditionCircumstance.ReferenceLocation, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightConditionCircumstance.RelationWithLocation, (int) EnumType.CodeLocationQualifier, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
