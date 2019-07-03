using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata;

namespace Aran.Aim.Metadata
{
	public class ResponsibleParty : CIResponsibleParty
	{
		public override DataType DataType
		{
			get { return DataType.ResponsibleParty; }
		}
		
		public string SystemName
		{
			get { return GetFieldValue <string> ((int) PropertyResponsibleParty.SystemName); }
			set { SetFieldValue <string> ((int) PropertyResponsibleParty.SystemName, value); }
		}
		
		public Contact ContactInfo
		{
			get { return (Contact ) GetValue ((int) PropertyResponsibleParty.ContactInfo); }
			set { SetValue ((int) PropertyResponsibleParty.ContactInfo, value); }
		}
		
		public string DigitalCertificate
		{
			get { return GetFieldValue <string> ((int) PropertyResponsibleParty.DigitalCertificate); }
			set { SetFieldValue <string> ((int) PropertyResponsibleParty.DigitalCertificate, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyResponsibleParty
	{
		SystemName = PropertyCIResponsibleParty.NEXT_CLASS,
		ContactInfo,
		DigitalCertificate,
		NEXT_CLASS
	}
	
	public static class MetadataResponsibleParty
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataResponsibleParty ()
		{
			PropInfoList = MetadataCIResponsibleParty.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyResponsibleParty.SystemName, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyResponsibleParty.ContactInfo, (int) DataType.Contact, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyResponsibleParty.DigitalCertificate, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
