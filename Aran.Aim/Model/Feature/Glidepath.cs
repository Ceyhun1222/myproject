using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class Glidepath : NavaidEquipment
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.Glidepath; }
		}
		
		public ValFrequency Frequency
		{
			get { return (ValFrequency ) GetValue ((int) PropertyGlidepath.Frequency); }
			set { SetValue ((int) PropertyGlidepath.Frequency, value); }
		}
		
		public double? Slope
		{
			get { return GetNullableFieldValue <double> ((int) PropertyGlidepath.Slope); }
			set { SetNullableFieldValue <double> ((int) PropertyGlidepath.Slope, value); }
		}
		
		public double? AngleAccuracy
		{
			get { return GetNullableFieldValue <double> ((int) PropertyGlidepath.AngleAccuracy); }
			set { SetNullableFieldValue <double> ((int) PropertyGlidepath.AngleAccuracy, value); }
		}
		
		public ValDistanceVertical Rdh
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyGlidepath.Rdh); }
			set { SetValue ((int) PropertyGlidepath.Rdh, value); }
		}
		
		public ValDistanceVertical RdhAccuracy
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyGlidepath.RdhAccuracy); }
			set { SetValue ((int) PropertyGlidepath.RdhAccuracy, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyGlidepath
	{
		Frequency = PropertyNavaidEquipment.NEXT_CLASS,
		Slope,
		AngleAccuracy,
		Rdh,
		RdhAccuracy,
		NEXT_CLASS
	}
	
	public static class MetadataGlidepath
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataGlidepath ()
		{
			PropInfoList = MetadataNavaidEquipment.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyGlidepath.Frequency, (int) DataType.ValFrequency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGlidepath.Slope, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGlidepath.AngleAccuracy, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGlidepath.Rdh, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyGlidepath.RdhAccuracy, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
