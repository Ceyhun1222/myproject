using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class Azimuth : NavaidEquipment
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.Azimuth; }
		}
		
		public CodeMLSAzimuth? Type
		{
			get { return GetNullableFieldValue <CodeMLSAzimuth> ((int) PropertyAzimuth.Type); }
			set { SetNullableFieldValue <CodeMLSAzimuth> ((int) PropertyAzimuth.Type, value); }
		}
		
		public double? TrueBearing
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAzimuth.TrueBearing); }
			set { SetNullableFieldValue <double> ((int) PropertyAzimuth.TrueBearing, value); }
		}
		
		public double? TrueBearingAccuracy
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAzimuth.TrueBearingAccuracy); }
			set { SetNullableFieldValue <double> ((int) PropertyAzimuth.TrueBearingAccuracy, value); }
		}
		
		public double? MagneticBearing
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAzimuth.MagneticBearing); }
			set { SetNullableFieldValue <double> ((int) PropertyAzimuth.MagneticBearing, value); }
		}
		
		public double? AngleProportionalLeft
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAzimuth.AngleProportionalLeft); }
			set { SetNullableFieldValue <double> ((int) PropertyAzimuth.AngleProportionalLeft, value); }
		}
		
		public double? AngleProportionalRight
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAzimuth.AngleProportionalRight); }
			set { SetNullableFieldValue <double> ((int) PropertyAzimuth.AngleProportionalRight, value); }
		}
		
		public double? AngleCoverLeft
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAzimuth.AngleCoverLeft); }
			set { SetNullableFieldValue <double> ((int) PropertyAzimuth.AngleCoverLeft, value); }
		}
		
		public double? AngleCoverRight
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAzimuth.AngleCoverRight); }
			set { SetNullableFieldValue <double> ((int) PropertyAzimuth.AngleCoverRight, value); }
		}
		
		public CodeMLSChannel? Channel
		{
			get { return GetNullableFieldValue <CodeMLSChannel> ((int) PropertyAzimuth.Channel); }
			set { SetNullableFieldValue <CodeMLSChannel> ((int) PropertyAzimuth.Channel, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAzimuth
	{
		Type = PropertyNavaidEquipment.NEXT_CLASS,
		TrueBearing,
		TrueBearingAccuracy,
		MagneticBearing,
		AngleProportionalLeft,
		AngleProportionalRight,
		AngleCoverLeft,
		AngleCoverRight,
		Channel,
		NEXT_CLASS
	}
	
	public static class MetadataAzimuth
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAzimuth ()
		{
			PropInfoList = MetadataNavaidEquipment.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAzimuth.Type, (int) EnumType.CodeMLSAzimuth, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAzimuth.TrueBearing, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAzimuth.TrueBearingAccuracy, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAzimuth.MagneticBearing, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAzimuth.AngleProportionalLeft, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAzimuth.AngleProportionalRight, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAzimuth.AngleCoverLeft, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAzimuth.AngleCoverRight, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAzimuth.Channel, (int) EnumType.CodeMLSChannel, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
