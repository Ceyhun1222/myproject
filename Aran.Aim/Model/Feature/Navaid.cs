using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class Navaid : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.Navaid; }
		}
		
		public CodeNavaidService? Type
		{
			get { return GetNullableFieldValue <CodeNavaidService> ((int) PropertyNavaid.Type); }
			set { SetNullableFieldValue <CodeNavaidService> ((int) PropertyNavaid.Type, value); }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyNavaid.Designator); }
			set { SetFieldValue <string> ((int) PropertyNavaid.Designator, value); }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyNavaid.Name); }
			set { SetFieldValue <string> ((int) PropertyNavaid.Name, value); }
		}
		
		public bool? FlightChecked
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyNavaid.FlightChecked); }
			set { SetNullableFieldValue <bool> ((int) PropertyNavaid.FlightChecked, value); }
		}
		
		public CodeNavaidPurpose? Purpose
		{
			get { return GetNullableFieldValue <CodeNavaidPurpose> ((int) PropertyNavaid.Purpose); }
			set { SetNullableFieldValue <CodeNavaidPurpose> ((int) PropertyNavaid.Purpose, value); }
		}
		
		public CodeSignalPerformanceILS? SignalPerformance
		{
			get { return GetNullableFieldValue <CodeSignalPerformanceILS> ((int) PropertyNavaid.SignalPerformance); }
			set { SetNullableFieldValue <CodeSignalPerformanceILS> ((int) PropertyNavaid.SignalPerformance, value); }
		}
		
		public CodeCourseQualityILS? CourseQuality
		{
			get { return GetNullableFieldValue <CodeCourseQualityILS> ((int) PropertyNavaid.CourseQuality); }
			set { SetNullableFieldValue <CodeCourseQualityILS> ((int) PropertyNavaid.CourseQuality, value); }
		}
		
		public CodeIntegrityLevelILS? IntegrityLevel
		{
			get { return GetNullableFieldValue <CodeIntegrityLevelILS> ((int) PropertyNavaid.IntegrityLevel); }
			set { SetNullableFieldValue <CodeIntegrityLevelILS> ((int) PropertyNavaid.IntegrityLevel, value); }
		}
		
		public List <FeatureRefObject> TouchDownLiftOff
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyNavaid.TouchDownLiftOff); }
		}
		
		public List <NavaidComponent> NavaidEquipment
		{
			get { return GetObjectList <NavaidComponent> ((int) PropertyNavaid.NavaidEquipment); }
		}
		
		public ElevatedPoint Location
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyNavaid.Location); }
			set { SetValue ((int) PropertyNavaid.Location, value); }
		}
		
		public List <FeatureRefObject> RunwayDirection
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyNavaid.RunwayDirection); }
		}
		
		public List <FeatureRefObject> ServedAirport
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyNavaid.ServedAirport); }
		}
		
		public List <NavaidOperationalStatus> Availability
		{
			get { return GetObjectList <NavaidOperationalStatus> ((int) PropertyNavaid.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNavaid
	{
		Type = PropertyFeature.NEXT_CLASS,
		Designator,
		Name,
		FlightChecked,
		Purpose,
		SignalPerformance,
		CourseQuality,
		IntegrityLevel,
		TouchDownLiftOff,
		NavaidEquipment,
		Location,
		RunwayDirection,
		ServedAirport,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataNavaid
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNavaid ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNavaid.Type, (int) EnumType.CodeNavaidService, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaid.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaid.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaid.FlightChecked, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaid.Purpose, (int) EnumType.CodeNavaidPurpose, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaid.SignalPerformance, (int) EnumType.CodeSignalPerformanceILS, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaid.CourseQuality, (int) EnumType.CodeCourseQualityILS, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaid.IntegrityLevel, (int) EnumType.CodeIntegrityLevelILS, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaid.TouchDownLiftOff, (int) FeatureType.TouchDownLiftOff, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaid.NavaidEquipment, (int) ObjectType.NavaidComponent, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaid.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaid.RunwayDirection, (int) FeatureType.RunwayDirection, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaid.ServedAirport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaid.Availability, (int) ObjectType.NavaidOperationalStatus, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
