using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class ApproachCondition : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.ApproachCondition; }
		}
		
		public CodeMinimaFinalApproachPath? FinalApproachPath
		{
			get { return GetNullableFieldValue <CodeMinimaFinalApproachPath> ((int) PropertyApproachCondition.FinalApproachPath); }
			set { SetNullableFieldValue <CodeMinimaFinalApproachPath> ((int) PropertyApproachCondition.FinalApproachPath, value); }
		}
		
		public double? RequiredNavigationPerformance
		{
			get { return GetNullableFieldValue <double> ((int) PropertyApproachCondition.RequiredNavigationPerformance); }
			set { SetNullableFieldValue <double> ((int) PropertyApproachCondition.RequiredNavigationPerformance, value); }
		}
		
		public double? ClimbGradient
		{
			get { return GetNullableFieldValue <double> ((int) PropertyApproachCondition.ClimbGradient); }
			set { SetNullableFieldValue <double> ((int) PropertyApproachCondition.ClimbGradient, value); }
		}
		
		public Minima MinimumSet
		{
			get { return GetObject <Minima> ((int) PropertyApproachCondition.MinimumSet); }
			set { SetValue ((int) PropertyApproachCondition.MinimumSet, value); }
		}
		
		public List <CirclingRestriction> CirclingRestriction
		{
			get { return GetObjectList <CirclingRestriction> ((int) PropertyApproachCondition.CirclingRestriction); }
		}
		
		public List <AircraftCharacteristic> AircraftCategory
		{
			get { return GetObjectList <AircraftCharacteristic> ((int) PropertyApproachCondition.AircraftCategory); }
		}
		
		public List <LandingTakeoffAreaCollection> LandingArea
		{
			get { return GetObjectList <LandingTakeoffAreaCollection> ((int) PropertyApproachCondition.LandingArea); }
		}
		
		public FeatureRef Altimeter
		{
			get { return (FeatureRef ) GetValue ((int) PropertyApproachCondition.Altimeter); }
			set { SetValue ((int) PropertyApproachCondition.Altimeter, value); }
		}
		
		public List <ObstacleAssessmentArea> DesignSurface
		{
			get { return GetObjectList <ObstacleAssessmentArea> ((int) PropertyApproachCondition.DesignSurface); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyApproachCondition
	{
		FinalApproachPath = PropertyAObject.NEXT_CLASS,
		RequiredNavigationPerformance,
		ClimbGradient,
		MinimumSet,
		CirclingRestriction,
		AircraftCategory,
		LandingArea,
		Altimeter,
		DesignSurface,
		NEXT_CLASS
	}
	
	public static class MetadataApproachCondition
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataApproachCondition ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyApproachCondition.FinalApproachPath, (int) EnumType.CodeMinimaFinalApproachPath, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachCondition.RequiredNavigationPerformance, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachCondition.ClimbGradient, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachCondition.MinimumSet, (int) ObjectType.Minima, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachCondition.CirclingRestriction, (int) ObjectType.CirclingRestriction, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachCondition.AircraftCategory, (int) ObjectType.AircraftCharacteristic, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachCondition.LandingArea, (int) ObjectType.LandingTakeoffAreaCollection, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachCondition.Altimeter, (int) FeatureType.AltimeterSource, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApproachCondition.DesignSurface, (int) ObjectType.ObstacleAssessmentArea, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
