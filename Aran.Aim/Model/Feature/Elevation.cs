using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class Elevation : NavaidEquipment
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.Elevation; }
		}
		
		public double? AngleNominal
		{
			get { return GetNullableFieldValue <double> ((int) PropertyElevation.AngleNominal); }
			set { SetNullableFieldValue <double> ((int) PropertyElevation.AngleNominal, value); }
		}
		
		public double? AngleMinimum
		{
			get { return GetNullableFieldValue <double> ((int) PropertyElevation.AngleMinimum); }
			set { SetNullableFieldValue <double> ((int) PropertyElevation.AngleMinimum, value); }
		}
		
		public double? AngleSpan
		{
			get { return GetNullableFieldValue <double> ((int) PropertyElevation.AngleSpan); }
			set { SetNullableFieldValue <double> ((int) PropertyElevation.AngleSpan, value); }
		}
		
		public double? AngleAccuracy
		{
			get { return GetNullableFieldValue <double> ((int) PropertyElevation.AngleAccuracy); }
			set { SetNullableFieldValue <double> ((int) PropertyElevation.AngleAccuracy, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyElevation
	{
		AngleNominal = PropertyNavaidEquipment.NEXT_CLASS,
		AngleMinimum,
		AngleSpan,
		AngleAccuracy,
		NEXT_CLASS
	}
	
	public static class MetadataElevation
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataElevation ()
		{
			PropInfoList = MetadataNavaidEquipment.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyElevation.AngleNominal, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyElevation.AngleMinimum, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyElevation.AngleSpan, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyElevation.AngleAccuracy, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
