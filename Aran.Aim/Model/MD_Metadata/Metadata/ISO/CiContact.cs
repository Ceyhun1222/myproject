using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class CiContact : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.CiContact; }
		}
		
		public CiTelephone Phone
		{
			get { return GetObject <CiTelephone> ((int) PropertyCiContact.Phone); }
			set { SetValue ((int) PropertyCiContact.Phone, value); }
		}
		
		public CiAddress Address
		{
			get { return GetObject <CiAddress> ((int) PropertyCiContact.Address); }
			set { SetValue ((int) PropertyCiContact.Address, value); }
		}
		
		public CiOnlineResource OnlineResource
		{
			get { return GetObject <CiOnlineResource> ((int) PropertyCiContact.OnlineResource); }
			set { SetValue ((int) PropertyCiContact.OnlineResource, value); }
		}
		
		public string HoursOfService
		{
			get { return GetFieldValue <string> ((int) PropertyCiContact.HoursOfService); }
			set { SetFieldValue <string> ((int) PropertyCiContact.HoursOfService, value); }
		}
		
		public string ContactInstructions
		{
			get { return GetFieldValue <string> ((int) PropertyCiContact.ContactInstructions); }
			set { SetFieldValue <string> ((int) PropertyCiContact.ContactInstructions, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCiContact
	{
		Phone = PropertyBtAbstractObject.NEXT_CLASS,
		Address,
		OnlineResource,
		HoursOfService,
		ContactInstructions,
		NEXT_CLASS
	}
	
	public static class MetadataCiContact
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCiContact ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCiContact.Phone, (int) ObjectType.CiTelephone, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiContact.Address, (int) ObjectType.CiAddress, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiContact.OnlineResource, (int) ObjectType.CiOnlineResource, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiContact.HoursOfService, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiContact.ContactInstructions, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
