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
	public class StandardLevelSector : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.StandardLevelSector; }
		}
		
		public CodeFlightRule? FlightRule
		{
			get { return GetNullableFieldValue <CodeFlightRule> ((int) PropertyStandardLevelSector.FlightRule); }
			set { SetNullableFieldValue <CodeFlightRule> ((int) PropertyStandardLevelSector.FlightRule, value); }
		}
		
		public double? FromTrack
		{
			get { return GetNullableFieldValue <double> ((int) PropertyStandardLevelSector.FromTrack); }
			set { SetNullableFieldValue <double> ((int) PropertyStandardLevelSector.FromTrack, value); }
		}
		
		public double? ToTrack
		{
			get { return GetNullableFieldValue <double> ((int) PropertyStandardLevelSector.ToTrack); }
			set { SetNullableFieldValue <double> ((int) PropertyStandardLevelSector.ToTrack, value); }
		}
		
		public CodeNorthReference? AngleType
		{
			get { return GetNullableFieldValue <CodeNorthReference> ((int) PropertyStandardLevelSector.AngleType); }
			set { SetNullableFieldValue <CodeNorthReference> ((int) PropertyStandardLevelSector.AngleType, value); }
		}
		
		public List <FeatureRefObject> ApplicableAirspace
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyStandardLevelSector.ApplicableAirspace); }
		}
		
		public FeatureRef ApplicableLevelColumn
		{
			get { return (FeatureRef ) GetValue ((int) PropertyStandardLevelSector.ApplicableLevelColumn); }
			set { SetValue ((int) PropertyStandardLevelSector.ApplicableLevelColumn, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyStandardLevelSector
	{
		FlightRule = PropertyFeature.NEXT_CLASS,
		FromTrack,
		ToTrack,
		AngleType,
		ApplicableAirspace,
		ApplicableLevelColumn,
		NEXT_CLASS
	}
	
	public static class MetadataStandardLevelSector
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataStandardLevelSector ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyStandardLevelSector.FlightRule, (int) EnumType.CodeFlightRule, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyStandardLevelSector.FromTrack, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyStandardLevelSector.ToTrack, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyStandardLevelSector.AngleType, (int) EnumType.CodeNorthReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyStandardLevelSector.ApplicableAirspace, (int) FeatureType.Airspace, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyStandardLevelSector.ApplicableLevelColumn, (int) FeatureType.StandardLevelColumn, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
