using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Metadata.ISO;

namespace Aran.Aim.Metadata.ISO
{
	public class CiAddress : BtAbstractObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.CiAddress; }
		}
		
		public List <BtString> DeliveryPoint
		{
			get { return GetObjectList <BtString> ((int) PropertyCiAddress.DeliveryPoint); }
		}
		
		public string City
		{
			get { return GetFieldValue <string> ((int) PropertyCiAddress.City); }
			set { SetFieldValue <string> ((int) PropertyCiAddress.City, value); }
		}
		
		public string AdministrativeArea
		{
			get { return GetFieldValue <string> ((int) PropertyCiAddress.AdministrativeArea); }
			set { SetFieldValue <string> ((int) PropertyCiAddress.AdministrativeArea, value); }
		}
		
		public string PostalCode
		{
			get { return GetFieldValue <string> ((int) PropertyCiAddress.PostalCode); }
			set { SetFieldValue <string> ((int) PropertyCiAddress.PostalCode, value); }
		}
		
		public string Country
		{
			get { return GetFieldValue <string> ((int) PropertyCiAddress.Country); }
			set { SetFieldValue <string> ((int) PropertyCiAddress.Country, value); }
		}
		
		public List <BtString> ElectronicMailAddress
		{
			get { return GetObjectList <BtString> ((int) PropertyCiAddress.ElectronicMailAddress); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCiAddress
	{
		DeliveryPoint = PropertyBtAbstractObject.NEXT_CLASS,
		City,
		AdministrativeArea,
		PostalCode,
		Country,
		ElectronicMailAddress,
		NEXT_CLASS
	}
	
	public static class MetadataCiAddress
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCiAddress ()
		{
			PropInfoList = MetadataBtAbstractObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCiAddress.DeliveryPoint, (int) ObjectType.BtString, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiAddress.City, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiAddress.AdministrativeArea, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiAddress.PostalCode, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiAddress.Country, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCiAddress.ElectronicMailAddress, (int) ObjectType.BtString, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
