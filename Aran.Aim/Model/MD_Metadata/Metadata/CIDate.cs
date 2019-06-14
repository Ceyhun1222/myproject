using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Metadata
{
	public class CIDate : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.CIDate; }
		}
		
		public DateTime? Date
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertyCIDate.Date); }
			set { SetNullableFieldValue <DateTime> ((int) PropertyCIDate.Date, value); }
		}
		
		public CIDateTypeCode? DateType
		{
			get { return GetNullableFieldValue <CIDateTypeCode> ((int) PropertyCIDate.DateType); }
			set { SetNullableFieldValue <CIDateTypeCode> ((int) PropertyCIDate.DateType, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCIDate
	{
		Date,
		DateType,
		NEXT_CLASS
	}
	
	public static class MetadataCIDate
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCIDate ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCIDate.Date, (int) AimFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyCIDate.DateType, (int) EnumType.CIDateTypeCode, PropertyTypeCharacter.Nullable);
		}
	}
}
