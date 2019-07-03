using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AuthorityForAerialRefuelling : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AuthorityForAerialRefuelling; }
		}
		
		public CodeAuthority? Type
		{
			get { return GetNullableFieldValue <CodeAuthority> ((int) PropertyAuthorityForAerialRefuelling.Type); }
			set { SetNullableFieldValue <CodeAuthority> ((int) PropertyAuthorityForAerialRefuelling.Type, value); }
		}
		
		public FeatureRef TheOrganisationAuthority
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAuthorityForAerialRefuelling.TheOrganisationAuthority); }
			set { SetValue ((int) PropertyAuthorityForAerialRefuelling.TheOrganisationAuthority, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAuthorityForAerialRefuelling
	{
		Type = PropertyAObject.NEXT_CLASS,
		TheOrganisationAuthority,
		NEXT_CLASS
	}
	
	public static class MetadataAuthorityForAerialRefuelling
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAuthorityForAerialRefuelling ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAuthorityForAerialRefuelling.Type, (int) EnumType.CodeAuthority, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAuthorityForAerialRefuelling.TheOrganisationAuthority, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.Nullable);
		}
	}
}
