using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class RunwayDirection : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RunwayDirection; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyRunwayDirection.Designator); }
			set { SetFieldValue <string> ((int) PropertyRunwayDirection.Designator, value); }
		}
		
		public double? TrueBearing
		{
			get { return GetNullableFieldValue <double> ((int) PropertyRunwayDirection.TrueBearing); }
			set { SetNullableFieldValue <double> ((int) PropertyRunwayDirection.TrueBearing, value); }
		}
		
		public double? TrueBearingAccuracy
		{
			get { return GetNullableFieldValue <double> ((int) PropertyRunwayDirection.TrueBearingAccuracy); }
			set { SetNullableFieldValue <double> ((int) PropertyRunwayDirection.TrueBearingAccuracy, value); }
		}
		
		public double? MagneticBearing
		{
			get { return GetNullableFieldValue <double> ((int) PropertyRunwayDirection.MagneticBearing); }
			set { SetNullableFieldValue <double> ((int) PropertyRunwayDirection.MagneticBearing, value); }
		}
		
		public CodeDirectionTurn? PatternVFR
		{
			get { return GetNullableFieldValue <CodeDirectionTurn> ((int) PropertyRunwayDirection.PatternVFR); }
			set { SetNullableFieldValue <CodeDirectionTurn> ((int) PropertyRunwayDirection.PatternVFR, value); }
		}
		
		public double? SlopeTDZ
		{
			get { return GetNullableFieldValue <double> ((int) PropertyRunwayDirection.SlopeTDZ); }
			set { SetNullableFieldValue <double> ((int) PropertyRunwayDirection.SlopeTDZ, value); }
		}
		
		public ValDistanceVertical ElevationTDZ
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyRunwayDirection.ElevationTDZ); }
			set { SetValue ((int) PropertyRunwayDirection.ElevationTDZ, value); }
		}
		
		public ValDistance ElevationTDZAccuracy
		{
			get { return (ValDistance ) GetValue ((int) PropertyRunwayDirection.ElevationTDZAccuracy); }
			set { SetValue ((int) PropertyRunwayDirection.ElevationTDZAccuracy, value); }
		}
		
		public CodeRunwayMarking? ApproachMarkingType
		{
			get { return GetNullableFieldValue <CodeRunwayMarking> ((int) PropertyRunwayDirection.ApproachMarkingType); }
			set { SetNullableFieldValue <CodeRunwayMarking> ((int) PropertyRunwayDirection.ApproachMarkingType, value); }
		}
		
		public CodeMarkingCondition? ApproachMarkingCondition
		{
			get { return GetNullableFieldValue <CodeMarkingCondition> ((int) PropertyRunwayDirection.ApproachMarkingCondition); }
			set { SetNullableFieldValue <CodeMarkingCondition> ((int) PropertyRunwayDirection.ApproachMarkingCondition, value); }
		}
		
		public CodeLightingJAR? ClassLightingJAR
		{
			get { return GetNullableFieldValue <CodeLightingJAR> ((int) PropertyRunwayDirection.ClassLightingJAR); }
			set { SetNullableFieldValue <CodeLightingJAR> ((int) PropertyRunwayDirection.ClassLightingJAR, value); }
		}
		
		public CodeApproachGuidance? PrecisionApproachGuidance
		{
			get { return GetNullableFieldValue <CodeApproachGuidance> ((int) PropertyRunwayDirection.PrecisionApproachGuidance); }
			set { SetNullableFieldValue <CodeApproachGuidance> ((int) PropertyRunwayDirection.PrecisionApproachGuidance, value); }
		}
		
		public FeatureRef UsedRunway
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRunwayDirection.UsedRunway); }
			set { SetValue ((int) PropertyRunwayDirection.UsedRunway, value); }
		}
		
		public FeatureRef StartingElement
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRunwayDirection.StartingElement); }
			set { SetValue ((int) PropertyRunwayDirection.StartingElement, value); }
		}
		
		public List <ManoeuvringAreaAvailability> Availability
		{
			get { return GetObjectList <ManoeuvringAreaAvailability> ((int) PropertyRunwayDirection.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunwayDirection
	{
		Designator = PropertyFeature.NEXT_CLASS,
		TrueBearing,
		TrueBearingAccuracy,
		MagneticBearing,
		PatternVFR,
		SlopeTDZ,
		ElevationTDZ,
		ElevationTDZAccuracy,
		ApproachMarkingType,
		ApproachMarkingCondition,
		ClassLightingJAR,
		PrecisionApproachGuidance,
		UsedRunway,
		StartingElement,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataRunwayDirection
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunwayDirection ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunwayDirection.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.TrueBearing, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.TrueBearingAccuracy, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.MagneticBearing, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.PatternVFR, (int) EnumType.CodeDirectionTurn, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.SlopeTDZ, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.ElevationTDZ, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.ElevationTDZAccuracy, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.ApproachMarkingType, (int) EnumType.CodeRunwayMarking, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.ApproachMarkingCondition, (int) EnumType.CodeMarkingCondition, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.ClassLightingJAR, (int) EnumType.CodeLightingJAR, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.PrecisionApproachGuidance, (int) EnumType.CodeApproachGuidance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.UsedRunway, (int) FeatureType.Runway, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.StartingElement, (int) FeatureType.RunwayElement, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirection.Availability, (int) ObjectType.ManoeuvringAreaAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
