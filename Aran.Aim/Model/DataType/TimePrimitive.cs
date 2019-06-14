using System;
using System.Collections.Generic;
using Aran;
using Aran.PropertyEnum;
using Aran.DataTypes;

namespace Aran.DataTypes
{
	public class TimePrimitive : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.TimePrimitive; }
		}
		
		public DateTime BeginPosition
		{
			get { return GetFieldValue <DateTime> ((int) PropertyTimePrimitive.BeginPosition); }
			set { SetFieldValue <DateTime> ((int) PropertyTimePrimitive.BeginPosition, value); }
		}
		
		public DateTime? EndPosition
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertyTimePrimitive.EndPosition); }
			set { SetNullableFieldValue <DateTime> ((int) PropertyTimePrimitive.EndPosition, value); }
		}
		
	}
}

namespace Aran.PropertyEnum
{
	public enum PropertyTimePrimitive
	{
		BeginPosition,
		EndPosition,
		NEXT_CLASS
	}
	
	public static class MetadataTimePrimitive
	{
		public static AranPropInfoList PropInfoList;
		
		static MetadataTimePrimitive ()
		{
			PropInfoList = MetadataAranObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTimePrimitive.BeginPosition, (int) AranFieldType.SysDateTime);
			PropInfoList.Add (PropertyTimePrimitive.EndPosition, (int) AranFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
		}
	}
}
