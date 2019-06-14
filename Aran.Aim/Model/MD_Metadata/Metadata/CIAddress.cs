using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Metadata
{
	public class CIAddress : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.CIAddress; }
		}
		
		public string deliveryPoint
		{
			get { return GetFieldValue <string> ((int) PropertyCIAddress.deliveryPoint); }
			set { SetFieldValue <string> ((int) PropertyCIAddress.deliveryPoint, value); }
		}
		
		public string city
		{
			get { return GetFieldValue <string> ((int) PropertyCIAddress.city); }
			set { SetFieldValue <string> ((int) PropertyCIAddress.city, value); }
		}
		
		public string administrativeArea
		{
			get { return GetFieldValue <string> ((int) PropertyCIAddress.administrativeArea); }
			set { SetFieldValue <string> ((int) PropertyCIAddress.administrativeArea, value); }
		}
		
		public string postalCode
		{
			get { return GetFieldValue <string> ((int) PropertyCIAddress.postalCode); }
			set { SetFieldValue <string> ((int) PropertyCIAddress.postalCode, value); }
		}
		
		public string country
		{
			get { return GetFieldValue <string> ((int) PropertyCIAddress.country); }
			set { SetFieldValue <string> ((int) PropertyCIAddress.country, value); }
		}
		
		public string electronicMailAddress
		{
			get { return GetFieldValue <string> ((int) PropertyCIAddress.electronicMailAddress); }
			set { SetFieldValue <string> ((int) PropertyCIAddress.electronicMailAddress, value); }
		}
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCIAddress
	{
		deliveryPoint,
		city,
		administrativeArea,
		postalCode,
		country,
		electronicMailAddress,
		NEXT_CLASS
	}
	
	public static class MetadataCIAddress
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCIAddress ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCIAddress.deliveryPoint, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCIAddress.city, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCIAddress.administrativeArea, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCIAddress.postalCode, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCIAddress.country, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCIAddress.electronicMailAddress, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
