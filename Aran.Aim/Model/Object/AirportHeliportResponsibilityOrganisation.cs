using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class AirportHeliportResponsibilityOrganisation : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.AirportHeliportResponsibilityOrganisation; }
		}
		
		public CodeAuthorityRole? Role
		{
			get { return GetNullableFieldValue <CodeAuthorityRole> ((int) PropertyAirportHeliportResponsibilityOrganisation.Role); }
			set { SetNullableFieldValue <CodeAuthorityRole> ((int) PropertyAirportHeliportResponsibilityOrganisation.Role, value); }
		}
		
		public FeatureRef TheOrganisationAuthority
		{
			get { return (FeatureRef ) GetValue ((int) PropertyAirportHeliportResponsibilityOrganisation.TheOrganisationAuthority); }
			set { SetValue ((int) PropertyAirportHeliportResponsibilityOrganisation.TheOrganisationAuthority, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirportHeliportResponsibilityOrganisation
	{
		Role = PropertyPropertiesWithSchedule.NEXT_CLASS,
		TheOrganisationAuthority,
		NEXT_CLASS
	}
	
	public static class MetadataAirportHeliportResponsibilityOrganisation
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirportHeliportResponsibilityOrganisation ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirportHeliportResponsibilityOrganisation.Role, (int) EnumType.CodeAuthorityRole, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliportResponsibilityOrganisation.TheOrganisationAuthority, (int) FeatureType.OrganisationAuthority, PropertyTypeCharacter.Nullable);
		}
	}
}
