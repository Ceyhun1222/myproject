using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata.ISO
{
	public class BtDateTime : AObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.BtDateTime; }
		}
		
		public DateTime? Value
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertyBtDateTime.Value); }
			set { SetNullableFieldValue <DateTime> ((int) PropertyBtDateTime.Value, value); }
		}
		
		public CiDateTypeCode? DateType
		{
			get { return GetNullableFieldValue <CiDateTypeCode> ((int) PropertyBtDateTime.DateType); }
			set { SetNullableFieldValue <CiDateTypeCode> ((int) PropertyBtDateTime.DateType, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyBtDateTime
	{
		Value = PropertyAObject.NEXT_CLASS,
		DateType,
		NEXT_CLASS
	}
	
	public static class MetadataBtDateTime
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataBtDateTime ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyBtDateTime.Value, (int) AimFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyBtDateTime.DateType, (int) EnumType.CiDateTypeCode, PropertyTypeCharacter.Nullable);
		}
	}
}
