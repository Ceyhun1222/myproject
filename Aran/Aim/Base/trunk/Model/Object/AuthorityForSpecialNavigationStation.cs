using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AuthorityForSpecialNavigationStation : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AuthorityForSpecialNavigationStation; }
		}
		
		public CodeAuthorityRole? Type
		{
			get { return GetNullableFieldValue <CodeAuthorityRole> ((int) PropertyAuthorityForSpecialNavigationStation.Type); }
			set { SetNullableFieldValue <CodeAuthorityRole> ((int) PropertyAuthorityForSpecialNavigationStation.Type, value); }
		}
		
		public FeatureRef TheOrganisationAuthority
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAuthorityForSpecialNavigationStation.TheOrganisationAuthority); }
			set { SetValue ((int) PropertyAuthorityForSpecialNavigationStation.TheOrganisationAuthority, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAuthorityForSpecialNavigationStation
	{
		Type = PropertyAObject.NEXT_CLASS,
		TheOrganisationAuthority,
		NEXT_CLASS
	}
	
	public static class MetadataAuthorityForSpecialNavigationStation
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAuthorityForSpecialNavigationStation ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAuthorityForSpecialNavigationStation.Type, (int) EnumType.CodeAuthorityRole, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAuthorityForSpecialNavigationStation.TheOrganisationAuthority, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.Nullable);
		}
	}
}
