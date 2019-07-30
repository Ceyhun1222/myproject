using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class RulesProcedures : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RulesProcedures; }
		}
		
		public CodeRuleProcedure? Category
		{
			get { return GetNullableFieldValue <CodeRuleProcedure> ((int) PropertyRulesProcedures.Category); }
			set { SetNullableFieldValue <CodeRuleProcedure> ((int) PropertyRulesProcedures.Category, value); }
		}
		
		public CodeRuleProcedureTitle? Title
		{
			get { return GetNullableFieldValue <CodeRuleProcedureTitle> ((int) PropertyRulesProcedures.Title); }
			set { SetNullableFieldValue <CodeRuleProcedureTitle> ((int) PropertyRulesProcedures.Title, value); }
		}
		
		public string Content
		{
			get { return GetFieldValue <string> ((int) PropertyRulesProcedures.Content); }
			set { SetFieldValue <string> ((int) PropertyRulesProcedures.Content, value); }
		}
		
		public List <FeatureRefObject> AffectedLocation
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyRulesProcedures.AffectedLocation); }
		}
		
		public List <FeatureRefObject> AffectedArea
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyRulesProcedures.AffectedArea); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRulesProcedures
	{
		Category = PropertyFeature.NEXT_CLASS,
		Title,
		Content,
		AffectedLocation,
		AffectedArea,
		NEXT_CLASS
	}
	
	public static class MetadataRulesProcedures
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRulesProcedures ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRulesProcedures.Category, (int) EnumType.CodeRuleProcedure, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRulesProcedures.Title, (int) EnumType.CodeRuleProcedureTitle, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRulesProcedures.Content, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRulesProcedures.AffectedLocation, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRulesProcedures.AffectedArea, (int) FeatureType.Airspace, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
