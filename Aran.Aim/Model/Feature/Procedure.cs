using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public abstract class Procedure : Feature
	{
		public virtual ProcedureType ProcedureType 
		{
			get { return (ProcedureType) FeatureType; }
		}
		
		public string CommunicationFailureInstruction
		{
			get { return GetFieldValue <string> ((int) PropertyProcedure.CommunicationFailureInstruction); }
			set { SetFieldValue <string> ((int) PropertyProcedure.CommunicationFailureInstruction, value); }
		}
		
		public string Instruction
		{
			get { return GetFieldValue <string> ((int) PropertyProcedure.Instruction); }
			set { SetFieldValue <string> ((int) PropertyProcedure.Instruction, value); }
		}
		
		public CodeDesignStandard? DesignCriteria
		{
			get { return GetNullableFieldValue <CodeDesignStandard> ((int) PropertyProcedure.DesignCriteria); }
			set { SetNullableFieldValue <CodeDesignStandard> ((int) PropertyProcedure.DesignCriteria, value); }
		}
		
		public CodeProcedureCodingStandard? CodingStandard
		{
			get { return GetNullableFieldValue <CodeProcedureCodingStandard> ((int) PropertyProcedure.CodingStandard); }
			set { SetNullableFieldValue <CodeProcedureCodingStandard> ((int) PropertyProcedure.CodingStandard, value); }
		}
		
		public bool? FlightChecked
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyProcedure.FlightChecked); }
			set { SetNullableFieldValue <bool> ((int) PropertyProcedure.FlightChecked, value); }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyProcedure.Name); }
			set { SetFieldValue <string> ((int) PropertyProcedure.Name, value); }
		}
		
		public bool? RNAV
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyProcedure.RNAV); }
			set { SetNullableFieldValue <bool> ((int) PropertyProcedure.RNAV, value); }
		}
		
		public List <ProcedureAvailability> Availability
		{
			get { return GetObjectList <ProcedureAvailability> ((int) PropertyProcedure.Availability); }
		}
		
		public List <FeatureRefObject> AirportHeliport
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyProcedure.AirportHeliport); }
		}
		
		public List <AircraftCharacteristic> AircraftCharacteristic
		{
			get { return GetObjectList <AircraftCharacteristic> ((int) PropertyProcedure.AircraftCharacteristic); }
		}
		
		public List <ProcedureTransition> FlightTransition
		{
			get { return GetObjectList <ProcedureTransition> ((int) PropertyProcedure.FlightTransition); }
		}
		
		public List <GuidanceService> GuidanceFacility
		{
			get { return GetObjectList <GuidanceService> ((int) PropertyProcedure.GuidanceFacility); }
		}
		
		public FeatureRef SafeAltitude
		{
			get { return (FeatureRef ) GetValue ((int) PropertyProcedure.SafeAltitude); }
			set { SetValue ((int) PropertyProcedure.SafeAltitude, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyProcedure
	{
		CommunicationFailureInstruction = PropertyFeature.NEXT_CLASS,
		Instruction,
		DesignCriteria,
		CodingStandard,
		FlightChecked,
		Name,
		RNAV,
		Availability,
		AirportHeliport,
		AircraftCharacteristic,
		FlightTransition,
		GuidanceFacility,
		SafeAltitude,
		NEXT_CLASS
	}
	
	public static class MetadataProcedure
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataProcedure ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyProcedure.CommunicationFailureInstruction, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedure.Instruction, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedure.DesignCriteria, (int) EnumType.CodeDesignStandard, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedure.CodingStandard, (int) EnumType.CodeProcedureCodingStandard, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedure.FlightChecked, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedure.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedure.RNAV, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedure.Availability, (int) ObjectType.ProcedureAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedure.AirportHeliport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedure.AircraftCharacteristic, (int) ObjectType.AircraftCharacteristic, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedure.FlightTransition, (int) ObjectType.ProcedureTransition, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedure.GuidanceFacility, (int) ObjectType.GuidanceService, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyProcedure.SafeAltitude, (int) FeatureType.SafeAltitudeArea, PropertyTypeCharacter.Nullable);
		}
	}
}
