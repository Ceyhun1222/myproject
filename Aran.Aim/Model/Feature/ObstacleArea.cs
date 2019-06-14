using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class ObstacleArea : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.ObstacleArea; }
		}
		
		public CodeObstacleArea? Type
		{
			get { return GetNullableFieldValue <CodeObstacleArea> ((int) PropertyObstacleArea.Type); }
			set { SetNullableFieldValue <CodeObstacleArea> ((int) PropertyObstacleArea.Type, value); }
		}
		
		public CodeObstacleAssessmentSurface? ObstructionIdSurfaceCondition
		{
			get { return GetNullableFieldValue <CodeObstacleAssessmentSurface> ((int) PropertyObstacleArea.ObstructionIdSurfaceCondition); }
			set { SetNullableFieldValue <CodeObstacleAssessmentSurface> ((int) PropertyObstacleArea.ObstructionIdSurfaceCondition, value); }
		}
		
		public ObstacleAreaOrigin Reference
		{
			get { return GetObject <ObstacleAreaOrigin> ((int) PropertyObstacleArea.Reference); }
			set { SetValue ((int) PropertyObstacleArea.Reference, value); }
		}
		
		public Surface SurfaceExtent
		{
			get { return GetObject <Surface> ((int) PropertyObstacleArea.SurfaceExtent); }
			set { SetValue ((int) PropertyObstacleArea.SurfaceExtent, value); }
		}
		
		public List <FeatureRefObject> Obstacle
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyObstacleArea.Obstacle); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyObstacleArea
	{
		Type = PropertyFeature.NEXT_CLASS,
		ObstructionIdSurfaceCondition,
		Reference,
		SurfaceExtent,
		Obstacle,
		NEXT_CLASS
	}
	
	public static class MetadataObstacleArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataObstacleArea ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyObstacleArea.Type, (int) EnumType.CodeObstacleArea, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleArea.ObstructionIdSurfaceCondition, (int) EnumType.CodeObstacleAssessmentSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleArea.Reference, (int) ObjectType.ObstacleAreaOrigin, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleArea.SurfaceExtent, (int) ObjectType.Surface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyObstacleArea.Obstacle, (int) FeatureType.VerticalStructure, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
