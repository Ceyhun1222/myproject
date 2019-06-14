using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class SDF : NavaidEquipment
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.SDF; }
		}
		
		public ValFrequency Frequency
		{
			get { return (ValFrequency ) GetValue ((int) PropertySDF.Frequency); }
			set { SetValue ((int) PropertySDF.Frequency, value); }
		}
		
		public double? MagneticBearing
		{
			get { return GetNullableFieldValue <double> ((int) PropertySDF.MagneticBearing); }
			set { SetNullableFieldValue <double> ((int) PropertySDF.MagneticBearing, value); }
		}
		
		public double? TrueBearing
		{
			get { return GetNullableFieldValue <double> ((int) PropertySDF.TrueBearing); }
			set { SetNullableFieldValue <double> ((int) PropertySDF.TrueBearing, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySDF
	{
		Frequency = PropertyNavaidEquipment.NEXT_CLASS,
		MagneticBearing,
		TrueBearing,
		NEXT_CLASS
	}
	
	public static class MetadataSDF
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSDF ()
		{
			PropInfoList = MetadataNavaidEquipment.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySDF.Frequency, (int) DataType.ValFrequency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySDF.MagneticBearing, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySDF.TrueBearing, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
