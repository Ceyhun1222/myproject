using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class FlightRestriction : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.FlightRestriction; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyFlightRestriction.Designator); }
			set { SetFieldValue <string> ((int) PropertyFlightRestriction.Designator, value); }
		}
		
		public CodeFlightRestriction? Type
		{
			get { return GetNullableFieldValue <CodeFlightRestriction> ((int) PropertyFlightRestriction.Type); }
			set { SetNullableFieldValue <CodeFlightRestriction> ((int) PropertyFlightRestriction.Type, value); }
		}
		
		public string Instruction
		{
			get { return GetFieldValue <string> ((int) PropertyFlightRestriction.Instruction); }
			set { SetFieldValue <string> ((int) PropertyFlightRestriction.Instruction, value); }
		}
		
		public FlightConditionCombination Flight
		{
			get { return GetObject <FlightConditionCombination> ((int) PropertyFlightRestriction.Flight); }
			set { SetValue ((int) PropertyFlightRestriction.Flight, value); }
		}
		
		public List <FlightRestrictionRoute> RegulatedRoute
		{
			get { return GetObjectList <FlightRestrictionRoute> ((int) PropertyFlightRestriction.RegulatedRoute); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFlightRestriction
	{
		Designator = PropertyFeature.NEXT_CLASS,
		Type,
		Instruction,
		Flight,
		RegulatedRoute,
		NEXT_CLASS
	}
	
	public static class MetadataFlightRestriction
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFlightRestriction ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFlightRestriction.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRestriction.Type, (int) EnumType.CodeFlightRestriction, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRestriction.Instruction, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRestriction.Flight, (int) ObjectType.FlightConditionCombination, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFlightRestriction.RegulatedRoute, (int) ObjectType.FlightRestrictionRoute, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
