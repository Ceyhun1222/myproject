using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class Timesheet : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Timesheet; }
		}
		
		public CodeTimeReference? TimeReference
		{
			get { return GetNullableFieldValue <CodeTimeReference> ((int) PropertyTimesheet.TimeReference); }
			set { SetNullableFieldValue <CodeTimeReference> ((int) PropertyTimesheet.TimeReference, value); }
		}
		
		public string StartDate
		{
			get { return GetFieldValue <string> ((int) PropertyTimesheet.StartDate); }
			set { SetFieldValue <string> ((int) PropertyTimesheet.StartDate, value); }
		}
		
		public string EndDate
		{
			get { return GetFieldValue <string> ((int) PropertyTimesheet.EndDate); }
			set { SetFieldValue <string> ((int) PropertyTimesheet.EndDate, value); }
		}
		
		public CodeDay? Day
		{
			get { return GetNullableFieldValue <CodeDay> ((int) PropertyTimesheet.Day); }
			set { SetNullableFieldValue <CodeDay> ((int) PropertyTimesheet.Day, value); }
		}
		
		public CodeDay? DayTil
		{
			get { return GetNullableFieldValue <CodeDay> ((int) PropertyTimesheet.DayTil); }
			set { SetNullableFieldValue <CodeDay> ((int) PropertyTimesheet.DayTil, value); }
		}
		
		public string StartTime
		{
			get { return GetFieldValue <string> ((int) PropertyTimesheet.StartTime); }
			set { SetFieldValue <string> ((int) PropertyTimesheet.StartTime, value); }
		}
		
		public CodeTimeEvent? StartEvent
		{
			get { return GetNullableFieldValue <CodeTimeEvent> ((int) PropertyTimesheet.StartEvent); }
			set { SetNullableFieldValue <CodeTimeEvent> ((int) PropertyTimesheet.StartEvent, value); }
		}
		
		public ValDuration StartTimeRelativeEvent
		{
			get { return (ValDuration ) GetValue ((int) PropertyTimesheet.StartTimeRelativeEvent); }
			set { SetValue ((int) PropertyTimesheet.StartTimeRelativeEvent, value); }
		}
		
		public CodeTimeEventCombination? StartEventInterpretation
		{
			get { return GetNullableFieldValue <CodeTimeEventCombination> ((int) PropertyTimesheet.StartEventInterpretation); }
			set { SetNullableFieldValue <CodeTimeEventCombination> ((int) PropertyTimesheet.StartEventInterpretation, value); }
		}
		
		public string EndTime
		{
			get { return GetFieldValue <string> ((int) PropertyTimesheet.EndTime); }
			set { SetFieldValue <string> ((int) PropertyTimesheet.EndTime, value); }
		}
		
		public CodeTimeEvent? EndEvent
		{
			get { return GetNullableFieldValue <CodeTimeEvent> ((int) PropertyTimesheet.EndEvent); }
			set { SetNullableFieldValue <CodeTimeEvent> ((int) PropertyTimesheet.EndEvent, value); }
		}
		
		public ValDuration EndTimeRelativeEvent
		{
			get { return (ValDuration ) GetValue ((int) PropertyTimesheet.EndTimeRelativeEvent); }
			set { SetValue ((int) PropertyTimesheet.EndTimeRelativeEvent, value); }
		}
		
		public CodeTimeEventCombination? EndEventInterpretation
		{
			get { return GetNullableFieldValue <CodeTimeEventCombination> ((int) PropertyTimesheet.EndEventInterpretation); }
			set { SetNullableFieldValue <CodeTimeEventCombination> ((int) PropertyTimesheet.EndEventInterpretation, value); }
		}
		
		public bool? DaylightSavingAdjust
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyTimesheet.DaylightSavingAdjust); }
			set { SetNullableFieldValue <bool> ((int) PropertyTimesheet.DaylightSavingAdjust, value); }
		}
		
		public bool? Excluded
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyTimesheet.Excluded); }
			set { SetNullableFieldValue <bool> ((int) PropertyTimesheet.Excluded, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTimesheet
	{
		TimeReference = PropertyAObject.NEXT_CLASS,
		StartDate,
		EndDate,
		Day,
		DayTil,
		StartTime,
		StartEvent,
		StartTimeRelativeEvent,
		StartEventInterpretation,
		EndTime,
		EndEvent,
		EndTimeRelativeEvent,
		EndEventInterpretation,
		DaylightSavingAdjust,
		Excluded,
		NEXT_CLASS
	}
	
	public static class MetadataTimesheet
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTimesheet ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTimesheet.TimeReference, (int) EnumType.CodeTimeReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.StartDate, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.EndDate, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.Day, (int) EnumType.CodeDay, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.DayTil, (int) EnumType.CodeDay, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.StartTime, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.StartEvent, (int) EnumType.CodeTimeEvent, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.StartTimeRelativeEvent, (int) DataType.ValDuration, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.StartEventInterpretation, (int) EnumType.CodeTimeEventCombination, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.EndTime, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.EndEvent, (int) EnumType.CodeTimeEvent, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.EndTimeRelativeEvent, (int) DataType.ValDuration, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.EndEventInterpretation, (int) EnumType.CodeTimeEventCombination, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.DaylightSavingAdjust, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimesheet.Excluded, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
