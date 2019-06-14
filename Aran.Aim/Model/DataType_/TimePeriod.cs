using System;
using System.Collections.Generic;
using Aran.Aim.PropertyEnum;

namespace Aran.Aim.DataTypes
{
	public class TimePeriod : ADataType
	{
        public TimePeriod ()
        {
        }

        public TimePeriod (DateTime beginPosition)
        {
            BeginPosition = beginPosition;
        }

        public TimePeriod (DateTime beginPosition, DateTime endPosition) 
            : this (beginPosition)
        {
            EndPosition = endPosition;
        }

		public override DataType DataType
		{
			get { return DataType.TimePeriod; }
		}
		
		public DateTime BeginPosition
		{
			get { return GetFieldValue <DateTime> ((int) PropertyTimePeriod.BeginPosition); }
			set { SetFieldValue <DateTime> ((int) PropertyTimePeriod.BeginPosition, value); }
		}
		
		public DateTime? EndPosition
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertyTimePeriod.EndPosition); }
			set { SetNullableFieldValue <DateTime> ((int) PropertyTimePeriod.EndPosition, value); }
		}		
	}
}
namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTimePeriod
	{
		BeginPosition,
		EndPosition
	}

    public static class MetadataTimePeriod
    {
        public static AimPropInfoList PropInfoList;

        static MetadataTimePeriod ()
        {
            PropInfoList = MetadataADataType.PropInfoList.Clone ();

            PropInfoList.Add (PropertyTimePeriod.BeginPosition, (int) AimFieldType.SysDateTime);
            PropInfoList.Add (PropertyTimePeriod.EndPosition, (int) AimFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
        }
    }
}
