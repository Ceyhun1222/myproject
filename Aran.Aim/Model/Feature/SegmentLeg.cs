using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public abstract class SegmentLeg : Feature
	{
		public virtual SegmentLegType SegmentLegType 
		{
			get { return (SegmentLegType) FeatureType; }
		}
		
		public CodeSegmentTermination? EndConditionDesignator
		{
			get { return GetNullableFieldValue <CodeSegmentTermination> ((int) PropertySegmentLeg.EndConditionDesignator); }
			set { SetNullableFieldValue <CodeSegmentTermination> ((int) PropertySegmentLeg.EndConditionDesignator, value); }
		}
		
		public CodeTrajectory? LegPath
		{
			get { return GetNullableFieldValue <CodeTrajectory> ((int) PropertySegmentLeg.LegPath); }
			set { SetNullableFieldValue <CodeTrajectory> ((int) PropertySegmentLeg.LegPath, value); }
		}
		
		public CodeSegmentPath? LegTypeARINC
		{
			get { return GetNullableFieldValue <CodeSegmentPath> ((int) PropertySegmentLeg.LegTypeARINC); }
			set { SetNullableFieldValue <CodeSegmentPath> ((int) PropertySegmentLeg.LegTypeARINC, value); }
		}
		
		public double? Course
		{
			get { return GetNullableFieldValue <double> ((int) PropertySegmentLeg.Course); }
			set { SetNullableFieldValue <double> ((int) PropertySegmentLeg.Course, value); }
		}
		
		public CodeCourse? CourseType
		{
			get { return GetNullableFieldValue <CodeCourse> ((int) PropertySegmentLeg.CourseType); }
			set { SetNullableFieldValue <CodeCourse> ((int) PropertySegmentLeg.CourseType, value); }
		}
		
		public CodeDirectionReference? CourseDirection
		{
			get { return GetNullableFieldValue <CodeDirectionReference> ((int) PropertySegmentLeg.CourseDirection); }
			set { SetNullableFieldValue <CodeDirectionReference> ((int) PropertySegmentLeg.CourseDirection, value); }
		}
		
		public CodeDirectionTurn? TurnDirection
		{
			get { return GetNullableFieldValue <CodeDirectionTurn> ((int) PropertySegmentLeg.TurnDirection); }
			set { SetNullableFieldValue <CodeDirectionTurn> ((int) PropertySegmentLeg.TurnDirection, value); }
		}
		
		public ValSpeed SpeedLimit
		{
			get { return (ValSpeed ) GetValue ((int) PropertySegmentLeg.SpeedLimit); }
			set { SetValue ((int) PropertySegmentLeg.SpeedLimit, value); }
		}
		
		public CodeSpeedReference? SpeedReference
		{
			get { return GetNullableFieldValue <CodeSpeedReference> ((int) PropertySegmentLeg.SpeedReference); }
			set { SetNullableFieldValue <CodeSpeedReference> ((int) PropertySegmentLeg.SpeedReference, value); }
		}
		
		public CodeAltitudeUse? SpeedInterpretation
		{
			get { return GetNullableFieldValue <CodeAltitudeUse> ((int) PropertySegmentLeg.SpeedInterpretation); }
			set { SetNullableFieldValue <CodeAltitudeUse> ((int) PropertySegmentLeg.SpeedInterpretation, value); }
		}
		
		public double? BankAngle
		{
			get { return GetNullableFieldValue <double> ((int) PropertySegmentLeg.BankAngle); }
			set { SetNullableFieldValue <double> ((int) PropertySegmentLeg.BankAngle, value); }
		}
		
		public ValDistance Length
		{
			get { return (ValDistance ) GetValue ((int) PropertySegmentLeg.Length); }
			set { SetValue ((int) PropertySegmentLeg.Length, value); }
		}
		
		public ValDuration Duration
		{
			get { return (ValDuration ) GetValue ((int) PropertySegmentLeg.Duration); }
			set { SetValue ((int) PropertySegmentLeg.Duration, value); }
		}
		
		public bool? ProcedureTurnRequired
		{
			get { return GetNullableFieldValue <bool> ((int) PropertySegmentLeg.ProcedureTurnRequired); }
			set { SetNullableFieldValue <bool> ((int) PropertySegmentLeg.ProcedureTurnRequired, value); }
		}
		
		public ValDistanceVertical UpperLimitAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertySegmentLeg.UpperLimitAltitude); }
			set { SetValue ((int) PropertySegmentLeg.UpperLimitAltitude, value); }
		}
		
		public CodeVerticalReference? UpperLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertySegmentLeg.UpperLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertySegmentLeg.UpperLimitReference, value); }
		}
		
		public ValDistanceVertical LowerLimitAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertySegmentLeg.LowerLimitAltitude); }
			set { SetValue ((int) PropertySegmentLeg.LowerLimitAltitude, value); }
		}
		
		public CodeVerticalReference? LowerLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertySegmentLeg.LowerLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertySegmentLeg.LowerLimitReference, value); }
		}
		
		public CodeAltitudeUse? AltitudeInterpretation
		{
			get { return GetNullableFieldValue <CodeAltitudeUse> ((int) PropertySegmentLeg.AltitudeInterpretation); }
			set { SetNullableFieldValue <CodeAltitudeUse> ((int) PropertySegmentLeg.AltitudeInterpretation, value); }
		}
		
		public ValDistanceVertical AltitudeOverrideATC
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertySegmentLeg.AltitudeOverrideATC); }
			set { SetValue ((int) PropertySegmentLeg.AltitudeOverrideATC, value); }
		}
		
		public CodeVerticalReference? AltitudeOverrideReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertySegmentLeg.AltitudeOverrideReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertySegmentLeg.AltitudeOverrideReference, value); }
		}
		
		public double? VerticalAngle
		{
			get { return GetNullableFieldValue <double> ((int) PropertySegmentLeg.VerticalAngle); }
			set { SetNullableFieldValue <double> ((int) PropertySegmentLeg.VerticalAngle, value); }
		}
		
		public TerminalSegmentPoint StartPoint
		{
			get { return GetObject <TerminalSegmentPoint> ((int) PropertySegmentLeg.StartPoint); }
			set { SetValue ((int) PropertySegmentLeg.StartPoint, value); }
		}
		
		public TerminalSegmentPoint EndPoint
		{
			get { return GetObject <TerminalSegmentPoint> ((int) PropertySegmentLeg.EndPoint); }
			set { SetValue ((int) PropertySegmentLeg.EndPoint, value); }
		}
		
		public Curve Trajectory
		{
			get { return GetObject <Curve> ((int) PropertySegmentLeg.Trajectory); }
			set { SetValue ((int) PropertySegmentLeg.Trajectory, value); }
		}
		
		public TerminalSegmentPoint ArcCentre
		{
			get { return GetObject <TerminalSegmentPoint> ((int) PropertySegmentLeg.ArcCentre); }
			set { SetValue ((int) PropertySegmentLeg.ArcCentre, value); }
		}
		
		public FeatureRef Angle
		{
			get { return (FeatureRef ) GetValue ((int) PropertySegmentLeg.Angle); }
			set { SetValue ((int) PropertySegmentLeg.Angle, value); }
		}
		
		public FeatureRef Distance
		{
			get { return (FeatureRef ) GetValue ((int) PropertySegmentLeg.Distance); }
			set { SetValue ((int) PropertySegmentLeg.Distance, value); }
		}
		
		public List <AircraftCharacteristic> AircraftCategory
		{
			get { return GetObjectList <AircraftCharacteristic> ((int) PropertySegmentLeg.AircraftCategory); }
		}
		
		public HoldingUse Holding
		{
			get { return GetObject <HoldingUse> ((int) PropertySegmentLeg.Holding); }
			set { SetValue ((int) PropertySegmentLeg.Holding, value); }
		}
		
		public List <ObstacleAssessmentArea> DesignSurface
		{
			get { return GetObjectList <ObstacleAssessmentArea> ((int) PropertySegmentLeg.DesignSurface); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySegmentLeg
	{
		EndConditionDesignator = PropertyFeature.NEXT_CLASS,
		LegPath,
		LegTypeARINC,
		Course,
		CourseType,
		CourseDirection,
		TurnDirection,
		SpeedLimit,
		SpeedReference,
		SpeedInterpretation,
		BankAngle,
		Length,
		Duration,
		ProcedureTurnRequired,
		UpperLimitAltitude,
		UpperLimitReference,
		LowerLimitAltitude,
		LowerLimitReference,
		AltitudeInterpretation,
		AltitudeOverrideATC,
		AltitudeOverrideReference,
		VerticalAngle,
		StartPoint,
		EndPoint,
		Trajectory,
		ArcCentre,
		Angle,
		Distance,
		AircraftCategory,
		Holding,
		DesignSurface,
		NEXT_CLASS
	}
	
	public static class MetadataSegmentLeg
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSegmentLeg ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySegmentLeg.EndConditionDesignator, (int) EnumType.CodeSegmentTermination, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.LegPath, (int) EnumType.CodeTrajectory, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.LegTypeARINC, (int) EnumType.CodeSegmentPath, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.Course, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.CourseType, (int) EnumType.CodeCourse, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.CourseDirection, (int) EnumType.CodeDirectionReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.TurnDirection, (int) EnumType.CodeDirectionTurn, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.SpeedLimit, (int) DataType.ValSpeed, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.SpeedReference, (int) EnumType.CodeSpeedReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.SpeedInterpretation, (int) EnumType.CodeAltitudeUse, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.BankAngle, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.Length, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.Duration, (int) DataType.ValDuration, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.ProcedureTurnRequired, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.UpperLimitAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.UpperLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.LowerLimitAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.LowerLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.AltitudeInterpretation, (int) EnumType.CodeAltitudeUse, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.AltitudeOverrideATC, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.AltitudeOverrideReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.VerticalAngle, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.StartPoint, (int) ObjectType.TerminalSegmentPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.EndPoint, (int) ObjectType.TerminalSegmentPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.Trajectory, (int) ObjectType.Curve, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.ArcCentre, (int) ObjectType.TerminalSegmentPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.Angle, (int) FeatureType.AngleIndication, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.Distance, (int) FeatureType.DistanceIndication, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.AircraftCategory, (int) ObjectType.AircraftCharacteristic, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.Holding, (int) ObjectType.HoldingUse, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentLeg.DesignSurface, (int) ObjectType.ObstacleAssessmentArea, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
