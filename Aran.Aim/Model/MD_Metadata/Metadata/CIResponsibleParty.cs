using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata
{
	public class CIResponsibleParty : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.CIResponsibleParty; }
		}
		
		public string IndividualName
		{
			get { return GetFieldValue <string> ((int) PropertyCIResponsibleParty.IndividualName); }
			set { SetFieldValue <string> ((int) PropertyCIResponsibleParty.IndividualName, value); }
		}
		
		public string OrganisationName
		{
			get { return GetFieldValue <string> ((int) PropertyCIResponsibleParty.OrganisationName); }
			set { SetFieldValue <string> ((int) PropertyCIResponsibleParty.OrganisationName, value); }
		}
		
		public string PositionName
		{
			get { return GetFieldValue <string> ((int) PropertyCIResponsibleParty.PositionName); }
			set { SetFieldValue <string> ((int) PropertyCIResponsibleParty.PositionName, value); }
		}
		
		public CIRoleCode? Role
		{
			get { return GetNullableFieldValue <CIRoleCode> ((int) PropertyCIResponsibleParty.Role); }
			set { SetNullableFieldValue <CIRoleCode> ((int) PropertyCIResponsibleParty.Role, value); }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCIResponsibleParty
	{
		IndividualName,
		OrganisationName,
		PositionName,
		Role,
		NEXT_CLASS
	}
	
	public static class MetadataCIResponsibleParty
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCIResponsibleParty ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCIResponsibleParty.IndividualName, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCIResponsibleParty.OrganisationName, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCIResponsibleParty.PositionName, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCIResponsibleParty.Role, (int) EnumType.CIRoleCode, PropertyTypeCharacter.Nullable);
		}
	}
}
