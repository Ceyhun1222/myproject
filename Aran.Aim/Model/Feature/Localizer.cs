using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class Localizer : NavaidEquipment
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.Localizer; }
		}
		
		public ValFrequency Frequency
		{
			get { return (ValFrequency ) GetValue ((int) PropertyLocalizer.Frequency); }
			set { SetValue ((int) PropertyLocalizer.Frequency, value); }
		}
		
		public double? MagneticBearing
		{
			get { return GetNullableFieldValue <double> ((int) PropertyLocalizer.MagneticBearing); }
			set { SetNullableFieldValue <double> ((int) PropertyLocalizer.MagneticBearing, value); }
		}
		
		public double? MagneticBearingAccuracy
		{
			get { return GetNullableFieldValue <double> ((int) PropertyLocalizer.MagneticBearingAccuracy); }
			set { SetNullableFieldValue <double> ((int) PropertyLocalizer.MagneticBearingAccuracy, value); }
		}
		
		public double? TrueBearing
		{
			get { return GetNullableFieldValue <double> ((int) PropertyLocalizer.TrueBearing); }
			set { SetNullableFieldValue <double> ((int) PropertyLocalizer.TrueBearing, value); }
		}
		
		public double? TrueBearingAccuracy
		{
			get { return GetNullableFieldValue <double> ((int) PropertyLocalizer.TrueBearingAccuracy); }
			set { SetNullableFieldValue <double> ((int) PropertyLocalizer.TrueBearingAccuracy, value); }
		}
		
		public double? Declination
		{
			get { return GetNullableFieldValue <double> ((int) PropertyLocalizer.Declination); }
			set { SetNullableFieldValue <double> ((int) PropertyLocalizer.Declination, value); }
		}
		
		public double? WidthCourse
		{
			get { return GetNullableFieldValue <double> ((int) PropertyLocalizer.WidthCourse); }
			set { SetNullableFieldValue <double> ((int) PropertyLocalizer.WidthCourse, value); }
		}
		
		public double? WidthCourseAccuracy
		{
			get { return GetNullableFieldValue <double> ((int) PropertyLocalizer.WidthCourseAccuracy); }
			set { SetNullableFieldValue <double> ((int) PropertyLocalizer.WidthCourseAccuracy, value); }
		}
		
		public CodeILSBackCourse? BackCourseUsable
		{
			get { return GetNullableFieldValue <CodeILSBackCourse> ((int) PropertyLocalizer.BackCourseUsable); }
			set { SetNullableFieldValue <CodeILSBackCourse> ((int) PropertyLocalizer.BackCourseUsable, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyLocalizer
	{
		Frequency = PropertyNavaidEquipment.NEXT_CLASS,
		MagneticBearing,
		MagneticBearingAccuracy,
		TrueBearing,
		TrueBearingAccuracy,
		Declination,
		WidthCourse,
		WidthCourseAccuracy,
		BackCourseUsable,
		NEXT_CLASS
	}
	
	public static class MetadataLocalizer
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataLocalizer ()
		{
			PropInfoList = MetadataNavaidEquipment.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyLocalizer.Frequency, (int) DataType.ValFrequency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLocalizer.MagneticBearing, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLocalizer.MagneticBearingAccuracy, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLocalizer.TrueBearing, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLocalizer.TrueBearingAccuracy, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLocalizer.Declination, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLocalizer.WidthCourse, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLocalizer.WidthCourseAccuracy, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyLocalizer.BackCourseUsable, (int) EnumType.CodeILSBackCourse, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
