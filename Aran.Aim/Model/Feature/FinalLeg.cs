using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class FinalLeg : ApproachLeg
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.FinalLeg; }
		}
		
		public CodeFinalGuidance? GuidanceSystem
		{
			get { return GetNullableFieldValue <CodeFinalGuidance> ((int) PropertyFinalLeg.GuidanceSystem); }
			set { SetNullableFieldValue <CodeFinalGuidance> ((int) PropertyFinalLeg.GuidanceSystem, value); }
		}
		
		public CodeApproachGuidance? LandingSystemCategory
		{
			get { return GetNullableFieldValue <CodeApproachGuidance> ((int) PropertyFinalLeg.LandingSystemCategory); }
			set { SetNullableFieldValue <CodeApproachGuidance> ((int) PropertyFinalLeg.LandingSystemCategory, value); }
		}
		
		public ValTemperature MinimumBaroVnavTemperature
		{
			get { return (ValTemperature ) GetValue ((int) PropertyFinalLeg.MinimumBaroVnavTemperature); }
			set { SetValue ((int) PropertyFinalLeg.MinimumBaroVnavTemperature, value); }
		}
		
		public bool? RnpDMEAuthorized
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyFinalLeg.RnpDMEAuthorized); }
			set { SetNullableFieldValue <bool> ((int) PropertyFinalLeg.RnpDMEAuthorized, value); }
		}
		
		public double? CourseOffsetAngle
		{
			get { return GetNullableFieldValue <double> ((int) PropertyFinalLeg.CourseOffsetAngle); }
			set { SetNullableFieldValue <double> ((int) PropertyFinalLeg.CourseOffsetAngle, value); }
		}
		
		public CodeSide? CourseOffsetSide
		{
			get { return GetNullableFieldValue <CodeSide> ((int) PropertyFinalLeg.CourseOffsetSide); }
			set { SetNullableFieldValue <CodeSide> ((int) PropertyFinalLeg.CourseOffsetSide, value); }
		}
		
		public ValDistance CourseCentrelineDistance
		{
			get { return (ValDistance ) GetValue ((int) PropertyFinalLeg.CourseCentrelineDistance); }
			set { SetValue ((int) PropertyFinalLeg.CourseCentrelineDistance, value); }
		}
		
		public ValDistance CourseOffsetDistance
		{
			get { return (ValDistance ) GetValue ((int) PropertyFinalLeg.CourseOffsetDistance); }
			set { SetValue ((int) PropertyFinalLeg.CourseOffsetDistance, value); }
		}
		
		public CodeRelativePosition? CourseCentrelineIntersect
		{
			get { return GetNullableFieldValue <CodeRelativePosition> ((int) PropertyFinalLeg.CourseCentrelineIntersect); }
			set { SetNullableFieldValue <CodeRelativePosition> ((int) PropertyFinalLeg.CourseCentrelineIntersect, value); }
		}
		
		public List <ApproachCondition> Condition
		{
			get { return GetObjectList <ApproachCondition> ((int) PropertyFinalLeg.Condition); }
		}
		
		public SignificantPoint FinalPathAlignmentPoint
		{
			get { return GetObject <SignificantPoint> ((int) PropertyFinalLeg.FinalPathAlignmentPoint); }
			set { SetValue ((int) PropertyFinalLeg.FinalPathAlignmentPoint, value); }
		}
		
		public TerminalSegmentPoint VisualDescentPoint
		{
			get { return GetObject <TerminalSegmentPoint> ((int) PropertyFinalLeg.VisualDescentPoint); }
			set { SetValue ((int) PropertyFinalLeg.VisualDescentPoint, value); }
		}
		
		public FASDataBlock FASData
		{
			get { return GetObject <FASDataBlock> ((int) PropertyFinalLeg.FASData); }
			set { SetValue ((int) PropertyFinalLeg.FASData, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyFinalLeg
	{
		GuidanceSystem = PropertyApproachLeg.NEXT_CLASS,
		LandingSystemCategory,
		MinimumBaroVnavTemperature,
		RnpDMEAuthorized,
		CourseOffsetAngle,
		CourseOffsetSide,
		CourseCentrelineDistance,
		CourseOffsetDistance,
		CourseCentrelineIntersect,
		Condition,
		FinalPathAlignmentPoint,
		VisualDescentPoint,
		FASData,
		NEXT_CLASS
	}
	
	public static class MetadataFinalLeg
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataFinalLeg ()
		{
			PropInfoList = MetadataApproachLeg.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFinalLeg.GuidanceSystem, (int) EnumType.CodeFinalGuidance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalLeg.LandingSystemCategory, (int) EnumType.CodeApproachGuidance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalLeg.MinimumBaroVnavTemperature, (int) DataType.ValTemperature, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalLeg.RnpDMEAuthorized, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalLeg.CourseOffsetAngle, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalLeg.CourseOffsetSide, (int) EnumType.CodeSide, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalLeg.CourseCentrelineDistance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalLeg.CourseOffsetDistance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalLeg.CourseCentrelineIntersect, (int) EnumType.CodeRelativePosition, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalLeg.Condition, (int) ObjectType.ApproachCondition, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalLeg.FinalPathAlignmentPoint, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalLeg.VisualDescentPoint, (int) ObjectType.TerminalSegmentPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFinalLeg.FASData, (int) ObjectType.FASDataBlock, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
