using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class OrganisationAuthorityAssociation : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.OrganisationAuthorityAssociation; }
		}
		
		public CodeOrganisationHierarchy? Type
		{
			get { return GetNullableFieldValue <CodeOrganisationHierarchy> ((int) PropertyOrganisationAuthorityAssociation.Type); }
			set { SetNullableFieldValue <CodeOrganisationHierarchy> ((int) PropertyOrganisationAuthorityAssociation.Type, value); }
		}
		
		public FeatureRef TheOrganisationAuthority
		{
			get { return (FeatureRef ) GetValue ((int) PropertyOrganisationAuthorityAssociation.TheOrganisationAuthority); }
			set { SetValue ((int) PropertyOrganisationAuthorityAssociation.TheOrganisationAuthority, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyOrganisationAuthorityAssociation
	{
		Type = PropertyAObject.NEXT_CLASS,
		TheOrganisationAuthority,
		NEXT_CLASS
	}
	
	public static class MetadataOrganisationAuthorityAssociation
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataOrganisationAuthorityAssociation ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyOrganisationAuthorityAssociation.Type, (int) EnumType.CodeOrganisationHierarchy, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyOrganisationAuthorityAssociation.TheOrganisationAuthority, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.Nullable);
		}
	}
}
