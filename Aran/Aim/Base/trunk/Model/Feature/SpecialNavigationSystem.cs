using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class SpecialNavigationSystem : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.SpecialNavigationSystem; }
		}
		
		public CodeSpecialNavigationSystem? Type
		{
			get { return GetNullableFieldValue <CodeSpecialNavigationSystem> ((int) PropertySpecialNavigationSystem.Type); }
			set { SetNullableFieldValue <CodeSpecialNavigationSystem> ((int) PropertySpecialNavigationSystem.Type, value); }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertySpecialNavigationSystem.Designator); }
			set { SetFieldValue <string> ((int) PropertySpecialNavigationSystem.Designator, value); }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertySpecialNavigationSystem.Name); }
			set { SetFieldValue <string> ((int) PropertySpecialNavigationSystem.Name, value); }
		}
		
		public AuthorityForSpecialNavigationSystem ResponsibleOrganisation
		{
			get { return GetObject <AuthorityForSpecialNavigationSystem> ((int) PropertySpecialNavigationSystem.ResponsibleOrganisation); }
			set { SetValue ((int) PropertySpecialNavigationSystem.ResponsibleOrganisation, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySpecialNavigationSystem
	{
		Type = PropertyFeature.NEXT_CLASS,
		Designator,
		Name,
		ResponsibleOrganisation,
		NEXT_CLASS
	}
	
	public static class MetadataSpecialNavigationSystem
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSpecialNavigationSystem ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySpecialNavigationSystem.Type, (int) EnumType.CodeSpecialNavigationSystem, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialNavigationSystem.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialNavigationSystem.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySpecialNavigationSystem.ResponsibleOrganisation, (int) ObjectType.AuthorityForSpecialNavigationSystem, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
