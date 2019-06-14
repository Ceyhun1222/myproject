using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AuthorityForNavaidEquipment : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AuthorityForNavaidEquipment; }
		}
		
		public CodeAuthorityRole? Type
		{
			get { return GetNullableFieldValue <CodeAuthorityRole> ((int) PropertyAuthorityForNavaidEquipment.Type); }
			set { SetNullableFieldValue <CodeAuthorityRole> ((int) PropertyAuthorityForNavaidEquipment.Type, value); }
		}
		
		public FeatureRef TheOrganisationAuthority
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAuthorityForNavaidEquipment.TheOrganisationAuthority); }
			set { SetValue ((int) PropertyAuthorityForNavaidEquipment.TheOrganisationAuthority, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAuthorityForNavaidEquipment
	{
		Type = PropertyAObject.NEXT_CLASS,
		TheOrganisationAuthority,
		NEXT_CLASS
	}
	
	public static class MetadataAuthorityForNavaidEquipment
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAuthorityForNavaidEquipment ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAuthorityForNavaidEquipment.Type, (int) EnumType.CodeAuthorityRole, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAuthorityForNavaidEquipment.TheOrganisationAuthority, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.Nullable);
		}
	}
}
