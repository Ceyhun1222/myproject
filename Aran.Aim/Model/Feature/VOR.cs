using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class VOR : NavaidEquipment
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.VOR; }
		}
		
		public CodeVOR? Type
		{
			get { return GetNullableFieldValue <CodeVOR> ((int) PropertyVOR.Type); }
			set { SetNullableFieldValue <CodeVOR> ((int) PropertyVOR.Type, value); }
		}
		
		public ValFrequency Frequency
		{
			get { return (ValFrequency ) GetValue ((int) PropertyVOR.Frequency); }
			set { SetValue ((int) PropertyVOR.Frequency, value); }
		}
		
		public CodeNorthReference? ZeroBearingDirection
		{
			get { return GetNullableFieldValue <CodeNorthReference> ((int) PropertyVOR.ZeroBearingDirection); }
			set { SetNullableFieldValue <CodeNorthReference> ((int) PropertyVOR.ZeroBearingDirection, value); }
		}
		
		public double? Declination
		{
			get { return GetNullableFieldValue <double> ((int) PropertyVOR.Declination); }
			set { SetNullableFieldValue <double> ((int) PropertyVOR.Declination, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyVOR
	{
		Type = PropertyNavaidEquipment.NEXT_CLASS,
		Frequency,
		ZeroBearingDirection,
		Declination,
		NEXT_CLASS
	}
	
	public static class MetadataVOR
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataVOR ()
		{
			PropInfoList = MetadataNavaidEquipment.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyVOR.Type, (int) EnumType.CodeVOR, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVOR.Frequency, (int) DataType.ValFrequency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVOR.ZeroBearingDirection, (int) EnumType.CodeNorthReference, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyVOR.Declination, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
