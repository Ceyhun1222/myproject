using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class StandardLevelTable : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.StandardLevelTable; }
		}
		
		public CodeLevelTableDesignator? Name
		{
			get { return GetNullableFieldValue <CodeLevelTableDesignator> ((int) PropertyStandardLevelTable.Name); }
			set { SetNullableFieldValue <CodeLevelTableDesignator> ((int) PropertyStandardLevelTable.Name, value); }
		}
		
		public bool? StandardICAO
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyStandardLevelTable.StandardICAO); }
			set { SetNullableFieldValue <bool> ((int) PropertyStandardLevelTable.StandardICAO, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyStandardLevelTable
	{
		Name = PropertyFeature.NEXT_CLASS,
		StandardICAO,
		NEXT_CLASS
	}
	
	public static class MetadataStandardLevelTable
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataStandardLevelTable ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyStandardLevelTable.Name, (int) EnumType.CodeLevelTableDesignator, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyStandardLevelTable.StandardICAO, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
