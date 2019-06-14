using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class TACAN : NavaidEquipment
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.TACAN; }
		}
		
		public CodeTACANChannel? Channel
		{
			get { return GetNullableFieldValue <CodeTACANChannel> ((int) PropertyTACAN.Channel); }
			set { SetNullableFieldValue <CodeTACANChannel> ((int) PropertyTACAN.Channel, value); }
		}
		
		public double? Declination
		{
			get { return GetNullableFieldValue <double> ((int) PropertyTACAN.Declination); }
			set { SetNullableFieldValue <double> ((int) PropertyTACAN.Declination, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTACAN
	{
		Channel = PropertyNavaidEquipment.NEXT_CLASS,
		Declination,
		NEXT_CLASS
	}
	
	public static class MetadataTACAN
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTACAN ()
		{
			PropInfoList = MetadataNavaidEquipment.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTACAN.Channel, (int) EnumType.CodeTACANChannel, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTACAN.Declination, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
