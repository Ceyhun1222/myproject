using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Metadata;

namespace Aran.Aim.DataTypes
{
	public class TimeSlice : ADataType
	{
		public override DataType DataType
		{
			get { return DataType.TimeSlice; }
		}
		
		public TimePeriod ValidTime
		{
			get { return (TimePeriod ) GetValue ((int) PropertyTimeSlice.ValidTime); }
			set { SetValue ((int) PropertyTimeSlice.ValidTime, value); }
		}
		
		public TimeSliceInterpretationType Interpretation
		{
			get { return GetFieldValue <TimeSliceInterpretationType> ((int) PropertyTimeSlice.Interpretation); }
			set { SetFieldValue <TimeSliceInterpretationType> ((int) PropertyTimeSlice.Interpretation, value); }
		}
		
		public int SequenceNumber
		{
			get { return GetFieldValue <int> ((int) PropertyTimeSlice.SequenceNumber); }
			set { SetFieldValue <int> ((int) PropertyTimeSlice.SequenceNumber, value); }
		}
		
		public int CorrectionNumber
		{
			get { return GetFieldValue <int> ((int) PropertyTimeSlice.CorrectionNumber); }
			set { SetFieldValue <int> ((int) PropertyTimeSlice.CorrectionNumber, value); }
		}
		
		public FeatureTimeSliceMetadata TimeSliceMetadata
		{
			get { return GetObject <FeatureTimeSliceMetadata> ((int) PropertyTimeSlice.TimeSliceMetadata); }
			set { SetValue ((int) PropertyTimeSlice.TimeSliceMetadata, value); }
		}
		
		public TimePeriod FeatureLifetime
		{
			get { return (TimePeriod ) GetValue ((int) PropertyTimeSlice.FeatureLifetime); }
			set { SetValue ((int) PropertyTimeSlice.FeatureLifetime, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTimeSlice
	{
		ValidTime,
		Interpretation,
		SequenceNumber,
		CorrectionNumber,
		TimeSliceMetadata,
		FeatureLifetime,
		NEXT_CLASS
	}
	
	public static class MetadataTimeSlice
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTimeSlice ()
		{
			PropInfoList = MetadataAimObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTimeSlice.ValidTime, (int) DataType.TimePeriod, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTimeSlice.Interpretation, (int) EnumType.TimeSliceInterpretationType);
			PropInfoList.Add (PropertyTimeSlice.SequenceNumber, (int) AimFieldType.SysInt32);
			PropInfoList.Add (PropertyTimeSlice.CorrectionNumber, (int) AimFieldType.SysInt32);
			PropInfoList.Add (PropertyTimeSlice.TimeSliceMetadata, (int) ObjectType.FeatureTimeSliceMetadata);
			PropInfoList.Add (PropertyTimeSlice.FeatureLifetime, (int) DataType.TimePeriod, PropertyTypeCharacter.Nullable);
		}
	}
}
