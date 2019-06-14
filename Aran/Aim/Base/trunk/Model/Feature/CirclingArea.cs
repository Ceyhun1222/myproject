using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class CirclingArea : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.CirclingArea; }
		}
		
		public Surface Extent
		{
			get { return GetObject <Surface> ((int) PropertyCirclingArea.Extent); }
			set { SetValue ((int) PropertyCirclingArea.Extent, value); }
		}
		
		public FeatureRef Approach
		{
			get { return (FeatureRef ) GetValue ((int) PropertyCirclingArea.Approach); }
			set { SetValue ((int) PropertyCirclingArea.Approach, value); }
		}
		
		public List <ApproachCondition> Condition
		{
			get { return GetObjectList <ApproachCondition> ((int) PropertyCirclingArea.Condition); }
		}
		
		public AircraftCharacteristic AircraftCategory
		{
			get { return GetObject <AircraftCharacteristic> ((int) PropertyCirclingArea.AircraftCategory); }
			set { SetValue ((int) PropertyCirclingArea.AircraftCategory, value); }
		}
		
		public List <ObstacleAssessmentArea> DesignSurface
		{
			get { return GetObjectList <ObstacleAssessmentArea> ((int) PropertyCirclingArea.DesignSurface); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCirclingArea
	{
		Extent = PropertyFeature.NEXT_CLASS,
		Approach,
		Condition,
		AircraftCategory,
		DesignSurface,
		NEXT_CLASS
	}
	
	public static class MetadataCirclingArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCirclingArea ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCirclingArea.Extent, (int) ObjectType.Surface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCirclingArea.Approach, (int) FeatureType.InstrumentApproachProcedure, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCirclingArea.Condition, (int) ObjectType.ApproachCondition, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCirclingArea.AircraftCategory, (int) ObjectType.AircraftCharacteristic, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCirclingArea.DesignSurface, (int) ObjectType.ObstacleAssessmentArea, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
