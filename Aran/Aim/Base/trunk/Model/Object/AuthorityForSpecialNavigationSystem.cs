using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AuthorityForSpecialNavigationSystem : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AuthorityForSpecialNavigationSystem; }
		}
		
		public CodeAuthorityRole? Type
		{
			get { return GetNullableFieldValue <CodeAuthorityRole> ((int) PropertyAuthorityForSpecialNavigationSystem.Type); }
			set { SetNullableFieldValue <CodeAuthorityRole> ((int) PropertyAuthorityForSpecialNavigationSystem.Type, value); }
		}
		
		public FeatureRef TheOrganisationAuthority
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAuthorityForSpecialNavigationSystem.TheOrganisationAuthority); }
			set { SetValue ((int) PropertyAuthorityForSpecialNavigationSystem.TheOrganisationAuthority, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAuthorityForSpecialNavigationSystem
	{
		Type = PropertyAObject.NEXT_CLASS,
		TheOrganisationAuthority,
		NEXT_CLASS
	}
	
	public static class MetadataAuthorityForSpecialNavigationSystem
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAuthorityForSpecialNavigationSystem ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAuthorityForSpecialNavigationSystem.Type, (int) EnumType.CodeAuthorityRole, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAuthorityForSpecialNavigationSystem.TheOrganisationAuthority, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.Nullable);
		}
	}
}
