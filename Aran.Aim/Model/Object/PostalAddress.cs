using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class PostalAddress : PropertiesWithSchedule
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.PostalAddress; }
		}
		
		public string DeliveryPoint
		{
			get { return GetFieldValue <string> ((int) PropertyPostalAddress.DeliveryPoint); }
			set { SetFieldValue <string> ((int) PropertyPostalAddress.DeliveryPoint, value); }
		}
		
		public string City
		{
			get { return GetFieldValue <string> ((int) PropertyPostalAddress.City); }
			set { SetFieldValue <string> ((int) PropertyPostalAddress.City, value); }
		}
		
		public string AdministrativeArea
		{
			get { return GetFieldValue <string> ((int) PropertyPostalAddress.AdministrativeArea); }
			set { SetFieldValue <string> ((int) PropertyPostalAddress.AdministrativeArea, value); }
		}
		
		public string PostalCode
		{
			get { return GetFieldValue <string> ((int) PropertyPostalAddress.PostalCode); }
			set { SetFieldValue <string> ((int) PropertyPostalAddress.PostalCode, value); }
		}
		
		public string Country
		{
			get { return GetFieldValue <string> ((int) PropertyPostalAddress.Country); }
			set { SetFieldValue <string> ((int) PropertyPostalAddress.Country, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyPostalAddress
	{
		DeliveryPoint = PropertyPropertiesWithSchedule.NEXT_CLASS,
		City,
		AdministrativeArea,
		PostalCode,
		Country,
		NEXT_CLASS
	}
	
	public static class MetadataPostalAddress
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataPostalAddress ()
		{
			PropInfoList = MetadataPropertiesWithSchedule.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyPostalAddress.DeliveryPoint, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPostalAddress.City, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPostalAddress.AdministrativeArea, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPostalAddress.PostalCode, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyPostalAddress.Country, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
