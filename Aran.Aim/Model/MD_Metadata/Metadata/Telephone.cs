using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata
{
	public class Telephone : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.Telephone; }
		}
		
		public PhoneCodeType? CodeType
		{
			get { return GetNullableFieldValue <PhoneCodeType> ((int) PropertyTelephone.CodeType); }
			set { SetNullableFieldValue <PhoneCodeType> ((int) PropertyTelephone.CodeType, value); }
		}
		
		public string Number
		{
			get { return GetFieldValue <string> ((int) PropertyTelephone.Number); }
			set { SetFieldValue <string> ((int) PropertyTelephone.Number, value); }
		}
		
		public string OtherDescription
		{
			get { return GetFieldValue <string> ((int) PropertyTelephone.OtherDescription); }
			set { SetFieldValue <string> ((int) PropertyTelephone.OtherDescription, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTelephone
	{
		CodeType,
		Number,
		OtherDescription,
		NEXT_CLASS
	}
	
	public static class MetadataTelephone
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTelephone ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTelephone.CodeType, (int) EnumType.PhoneCodeType, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTelephone.Number, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTelephone.OtherDescription, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
		}
	}
}
