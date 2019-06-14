using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata.ISO
{
	public class CiResponsibleParty : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.CiResponsibleParty; }
		}
		
		public string IndividualName
		{
			get { return GetFieldValue <string> ((int) PropertyCiResponsibleParty.IndividualName); }
			set { SetFieldValue <string> ((int) PropertyCiResponsibleParty.IndividualName, value); }
		}
		
		public string OrganizationName
		{
			get { return GetFieldValue <string> ((int) PropertyCiResponsibleParty.OrganizationName); }
			set { SetFieldValue <string> ((int) PropertyCiResponsibleParty.OrganizationName, value); }
		}
		
		public string PositionName
		{
			get { return GetFieldValue <string> ((int) PropertyCiResponsibleParty.PositionName); }
			set { SetFieldValue <string> ((int) PropertyCiResponsibleParty.PositionName, value); }
		}
		
		public CiContact ContactInfo
		{
			get { return GetObject <CiContact> ((int) PropertyCiResponsibleParty.ContactInfo); }
			set { SetValue ((int) PropertyCiResponsibleParty.ContactInfo, value); }
		}
		
		public CiRoleCode? RoleCode
		{
			get { return GetNullableFieldValue <CiRoleCode> ((int) PropertyCiResponsibleParty.RoleCode); }
			set { SetNullableFieldValue <CiRoleCode> ((int) PropertyCiResponsibleParty.RoleCode, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCiResponsibleParty
	{
		IndividualName = PropertyBtAbstractObject.NEXT_CLASS,
		OrganizationName,
		PositionName,
		ContactInfo,
		RoleCode,
		NEXT_CLASS
	}
	
	public static class MetadataCiResponsibleParty
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCiResponsibleParty ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCiResponsibleParty.IndividualName, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiResponsibleParty.OrganizationName, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiResponsibleParty.PositionName, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiResponsibleParty.ContactInfo, (int) ObjectType.CiContact, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiResponsibleParty.RoleCode, (int) EnumType.CiRoleCode, PropertyTypeCharacter.Nullable);
		}
	}
}
