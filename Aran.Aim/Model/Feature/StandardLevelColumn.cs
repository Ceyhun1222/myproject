using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class StandardLevelColumn : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.StandardLevelColumn; }
		}
		
		public CodeLevelSeries? Series
		{
			get { return GetNullableFieldValue <CodeLevelSeries> ((int) PropertyStandardLevelColumn.Series); }
			set { SetNullableFieldValue <CodeLevelSeries> ((int) PropertyStandardLevelColumn.Series, value); }
		}
		
		public UomDistanceVertical UnitOfMeasurement
		{
			get { return GetFieldValue <UomDistanceVertical> ((int) PropertyStandardLevelColumn.UnitOfMeasurement); }
			set { SetFieldValue <UomDistanceVertical> ((int) PropertyStandardLevelColumn.UnitOfMeasurement, value); }
		}
		
		public CodeRVSM? Separation
		{
			get { return GetNullableFieldValue <CodeRVSM> ((int) PropertyStandardLevelColumn.Separation); }
			set { SetNullableFieldValue <CodeRVSM> ((int) PropertyStandardLevelColumn.Separation, value); }
		}
		
		public List <StandardLevel> Level
		{
			get { return GetObjectList <StandardLevel> ((int) PropertyStandardLevelColumn.Level); }
		}
		
		public FeatureRef LevelTable
		{
			get { return (FeatureRef ) GetValue ((int) PropertyStandardLevelColumn.LevelTable); }
			set { SetValue ((int) PropertyStandardLevelColumn.LevelTable, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyStandardLevelColumn
	{
		Series = PropertyFeature.NEXT_CLASS,
		UnitOfMeasurement,
		Separation,
		Level,
		LevelTable,
		NEXT_CLASS
	}
	
	public static class MetadataStandardLevelColumn
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataStandardLevelColumn ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyStandardLevelColumn.Series, (int) EnumType.CodeLevelSeries, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyStandardLevelColumn.UnitOfMeasurement, (int) EnumType.UomDistanceVertical);
			PropInfoList.Add (PropertyStandardLevelColumn.Separation, (int) EnumType.CodeRVSM, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyStandardLevelColumn.Level, (int) ObjectType.StandardLevel, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyStandardLevelColumn.LevelTable, (int) FeatureType.StandardLevelTable, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
