using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class RouteSegment : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RouteSegment; }
		}
		
		public CodeLevel? Level
		{
			get { return GetNullableFieldValue <CodeLevel> ((int) PropertyRouteSegment.Level); }
			set { SetNullableFieldValue <CodeLevel> ((int) PropertyRouteSegment.Level, value); }
		}
		
		public ValDistanceVertical UpperLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyRouteSegment.UpperLimit); }
			set { SetValue ((int) PropertyRouteSegment.UpperLimit, value); }
		}
		
		public CodeVerticalReference? UpperLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyRouteSegment.UpperLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyRouteSegment.UpperLimitReference, value); }
		}
		
		public ValDistanceVertical LowerLimit
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyRouteSegment.LowerLimit); }
			set { SetValue ((int) PropertyRouteSegment.LowerLimit, value); }
		}
		
		public CodeVerticalReference? LowerLimitReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyRouteSegment.LowerLimitReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyRouteSegment.LowerLimitReference, value); }
		}
		
		public ValDistanceVertical MinimumObstacleClearanceAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyRouteSegment.MinimumObstacleClearanceAltitude); }
			set { SetValue ((int) PropertyRouteSegment.MinimumObstacleClearanceAltitude, value); }
		}
		
		public CodeRouteSegmentPath? PathType
		{
			get { return GetNullableFieldValue <CodeRouteSegmentPath> ((int) PropertyRouteSegment.PathType); }
			set { SetNullableFieldValue <CodeRouteSegmentPath> ((int) PropertyRouteSegment.PathType, value); }
		}
		
		public double? TrueTrack
		{
			get { return GetNullableFieldValue <double> ((int) PropertyRouteSegment.TrueTrack); }
			set { SetNullableFieldValue <double> ((int) PropertyRouteSegment.TrueTrack, value); }
		}
		
		public double? MagneticTrack
		{
			get { return GetNullableFieldValue <double> ((int) PropertyRouteSegment.MagneticTrack); }
			set { SetNullableFieldValue <double> ((int) PropertyRouteSegment.MagneticTrack, value); }
		}
		
		public double? ReverseTrueTrack
		{
			get { return GetNullableFieldValue <double> ((int) PropertyRouteSegment.ReverseTrueTrack); }
			set { SetNullableFieldValue <double> ((int) PropertyRouteSegment.ReverseTrueTrack, value); }
		}
		
		public double? ReverseMagneticTrack
		{
			get { return GetNullableFieldValue <double> ((int) PropertyRouteSegment.ReverseMagneticTrack); }
			set { SetNullableFieldValue <double> ((int) PropertyRouteSegment.ReverseMagneticTrack, value); }
		}
		
		public ValDistance Length
		{
			get { return (ValDistance ) GetValue ((int) PropertyRouteSegment.Length); }
			set { SetValue ((int) PropertyRouteSegment.Length, value); }
		}
		
		public ValDistance WidthLeft
		{
			get { return (ValDistance ) GetValue ((int) PropertyRouteSegment.WidthLeft); }
			set { SetValue ((int) PropertyRouteSegment.WidthLeft, value); }
		}
		
		public ValDistance WidthRight
		{
			get { return (ValDistance ) GetValue ((int) PropertyRouteSegment.WidthRight); }
			set { SetValue ((int) PropertyRouteSegment.WidthRight, value); }
		}
		
		public CodeDirectionTurn? TurnDirection
		{
			get { return GetNullableFieldValue <CodeDirectionTurn> ((int) PropertyRouteSegment.TurnDirection); }
			set { SetNullableFieldValue <CodeDirectionTurn> ((int) PropertyRouteSegment.TurnDirection, value); }
		}
		
		public bool? SignalGap
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRouteSegment.SignalGap); }
			set { SetNullableFieldValue <bool> ((int) PropertyRouteSegment.SignalGap, value); }
		}
		
		public ValDistanceVertical MinimumEnrouteAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyRouteSegment.MinimumEnrouteAltitude); }
			set { SetValue ((int) PropertyRouteSegment.MinimumEnrouteAltitude, value); }
		}
		
		public ValDistanceVertical MinimumCrossingAtEnd
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyRouteSegment.MinimumCrossingAtEnd); }
			set { SetValue ((int) PropertyRouteSegment.MinimumCrossingAtEnd, value); }
		}
		
		public CodeVerticalReference? MinimumCrossingAtEndReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyRouteSegment.MinimumCrossingAtEndReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyRouteSegment.MinimumCrossingAtEndReference, value); }
		}
		
		public ValDistanceVertical MaximumCrossingAtEnd
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyRouteSegment.MaximumCrossingAtEnd); }
			set { SetValue ((int) PropertyRouteSegment.MaximumCrossingAtEnd, value); }
		}
		
		public CodeVerticalReference? MaximumCrossingAtEndReference
		{
			get { return GetNullableFieldValue <CodeVerticalReference> ((int) PropertyRouteSegment.MaximumCrossingAtEndReference); }
			set { SetNullableFieldValue <CodeVerticalReference> ((int) PropertyRouteSegment.MaximumCrossingAtEndReference, value); }
		}
		
		public CodeRouteNavigation? NavigationType
		{
			get { return GetNullableFieldValue <CodeRouteNavigation> ((int) PropertyRouteSegment.NavigationType); }
			set { SetNullableFieldValue <CodeRouteNavigation> ((int) PropertyRouteSegment.NavigationType, value); }
		}
		
		public double? RequiredNavigationPerformance
		{
			get { return GetNullableFieldValue <double> ((int) PropertyRouteSegment.RequiredNavigationPerformance); }
			set { SetNullableFieldValue <double> ((int) PropertyRouteSegment.RequiredNavigationPerformance, value); }
		}
		
		public CodeRouteDesignatorSuffix? DesignatorSuffix
		{
			get { return GetNullableFieldValue <CodeRouteDesignatorSuffix> ((int) PropertyRouteSegment.DesignatorSuffix); }
			set { SetNullableFieldValue <CodeRouteDesignatorSuffix> ((int) PropertyRouteSegment.DesignatorSuffix, value); }
		}
		
		public EnRouteSegmentPoint Start
		{
			get { return GetObject <EnRouteSegmentPoint> ((int) PropertyRouteSegment.Start); }
			set { SetValue ((int) PropertyRouteSegment.Start, value); }
		}
		
		public FeatureRef RouteFormed
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRouteSegment.RouteFormed); }
			set { SetValue ((int) PropertyRouteSegment.RouteFormed, value); }
		}
		
		public ObstacleAssessmentArea EvaluationArea
		{
			get { return GetObject <ObstacleAssessmentArea> ((int) PropertyRouteSegment.EvaluationArea); }
			set { SetValue ((int) PropertyRouteSegment.EvaluationArea, value); }
		}
		
		public Curve CurveExtent
		{
			get { return GetObject <Curve> ((int) PropertyRouteSegment.CurveExtent); }
			set { SetValue ((int) PropertyRouteSegment.CurveExtent, value); }
		}
		
		public EnRouteSegmentPoint End
		{
			get { return GetObject <EnRouteSegmentPoint> ((int) PropertyRouteSegment.End); }
			set { SetValue ((int) PropertyRouteSegment.End, value); }
		}
		
		public List <RouteAvailability> Availability
		{
			get { return GetObjectList <RouteAvailability> ((int) PropertyRouteSegment.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRouteSegment
	{
		Level = PropertyFeature.NEXT_CLASS,
		UpperLimit,
		UpperLimitReference,
		LowerLimit,
		LowerLimitReference,
		MinimumObstacleClearanceAltitude,
		PathType,
		TrueTrack,
		MagneticTrack,
		ReverseTrueTrack,
		ReverseMagneticTrack,
		Length,
		WidthLeft,
		WidthRight,
		TurnDirection,
		SignalGap,
		MinimumEnrouteAltitude,
		MinimumCrossingAtEnd,
		MinimumCrossingAtEndReference,
		MaximumCrossingAtEnd,
		MaximumCrossingAtEndReference,
		NavigationType,
		RequiredNavigationPerformance,
		DesignatorSuffix,
		Start,
		RouteFormed,
		EvaluationArea,
		CurveExtent,
		End,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataRouteSegment
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRouteSegment ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRouteSegment.Level, (int) EnumType.CodeLevel, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.UpperLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.UpperLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.LowerLimit, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.LowerLimitReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.MinimumObstacleClearanceAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.PathType, (int) EnumType.CodeRouteSegmentPath, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.TrueTrack, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.MagneticTrack, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.ReverseTrueTrack, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.ReverseMagneticTrack, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.Length, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.WidthLeft, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.WidthRight, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.TurnDirection, (int) EnumType.CodeDirectionTurn, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.SignalGap, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.MinimumEnrouteAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.MinimumCrossingAtEnd, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.MinimumCrossingAtEndReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.MaximumCrossingAtEnd, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.MaximumCrossingAtEndReference, (int) EnumType.CodeVerticalReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.NavigationType, (int) EnumType.CodeRouteNavigation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.RequiredNavigationPerformance, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.DesignatorSuffix, (int) EnumType.CodeRouteDesignatorSuffix, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.Start, (int) ObjectType.EnRouteSegmentPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.RouteFormed, (int) FeatureType.Route, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.EvaluationArea, (int) ObjectType.ObstacleAssessmentArea, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.CurveExtent, (int) ObjectType.Curve, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.End, (int) ObjectType.EnRouteSegmentPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRouteSegment.Availability, (int) ObjectType.RouteAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
