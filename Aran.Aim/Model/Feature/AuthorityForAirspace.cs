using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AuthorityForAirspace : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AuthorityForAirspace; }
		}
		
		public CodeAuthority? Type
		{
			get { return GetNullableFieldValue <CodeAuthority> ((int) PropertyAuthorityForAirspace.Type); }
			set { SetNullableFieldValue <CodeAuthority> ((int) PropertyAuthorityForAirspace.Type, value); }
		}
		
		public FeatureRef ResponsibleOrganisation
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAuthorityForAirspace.ResponsibleOrganisation); }
			set { SetValue ((int) PropertyAuthorityForAirspace.ResponsibleOrganisation, value); }
		}
		
		public FeatureRef AssignedAirspace
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAuthorityForAirspace.AssignedAirspace); }
			set { SetValue ((int) PropertyAuthorityForAirspace.AssignedAirspace, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAuthorityForAirspace
	{
		Type = PropertyFeature.NEXT_CLASS,
		ResponsibleOrganisation,
		AssignedAirspace,
		NEXT_CLASS
	}
	
	public static class MetadataAuthorityForAirspace
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAuthorityForAirspace ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAuthorityForAirspace.Type, (int) EnumType.CodeAuthority, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAuthorityForAirspace.ResponsibleOrganisation, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAuthorityForAirspace.AssignedAirspace, (int) FeatureType.Airspace, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
