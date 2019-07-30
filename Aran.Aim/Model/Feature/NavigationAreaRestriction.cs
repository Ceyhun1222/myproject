using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class NavigationAreaRestriction : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.NavigationAreaRestriction; }
		}
		
		public CodeNavigationAreaRestriction? Type
		{
			get { return GetNullableFieldValue <CodeNavigationAreaRestriction> ((int) PropertyNavigationAreaRestriction.Type); }
			set { SetNullableFieldValue <CodeNavigationAreaRestriction> ((int) PropertyNavigationAreaRestriction.Type, value); }
		}
		
		public List <AbstractProcedureRefObject> Procedure
		{
			get { return GetObjectList <AbstractProcedureRefObject> ((int) PropertyNavigationAreaRestriction.Procedure); }
		}
		
		public ObstacleAssessmentArea DesignSurface
		{
			get { return GetObject <ObstacleAssessmentArea> ((int) PropertyNavigationAreaRestriction.DesignSurface); }
			set { SetValue ((int) PropertyNavigationAreaRestriction.DesignSurface, value); }
		}
		
		public CircleSector SectorDefinition
		{
			get { return GetObject <CircleSector> ((int) PropertyNavigationAreaRestriction.SectorDefinition); }
			set { SetValue ((int) PropertyNavigationAreaRestriction.SectorDefinition, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNavigationAreaRestriction
	{
		Type = PropertyFeature.NEXT_CLASS,
		Procedure,
		DesignSurface,
		SectorDefinition,
		NEXT_CLASS
	}
	
	public static class MetadataNavigationAreaRestriction
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNavigationAreaRestriction ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNavigationAreaRestriction.Type, (int) EnumType.CodeNavigationAreaRestriction, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationAreaRestriction.Procedure, (int) ObjectType.AbstractProcedureRefObject, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationAreaRestriction.DesignSurface, (int) ObjectType.ObstacleAssessmentArea, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationAreaRestriction.SectorDefinition, (int) ObjectType.CircleSector, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
