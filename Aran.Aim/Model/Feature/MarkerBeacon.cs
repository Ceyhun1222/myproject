using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class MarkerBeacon : NavaidEquipment
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.MarkerBeacon; }
		}
		
		public CodeMarkerBeaconSignal? Class
		{
			get { return GetNullableFieldValue <CodeMarkerBeaconSignal> ((int) PropertyMarkerBeacon.Class); }
			set { SetNullableFieldValue <CodeMarkerBeaconSignal> ((int) PropertyMarkerBeacon.Class, value); }
		}
		
		public ValFrequency Frequency
		{
			get { return (ValFrequency ) GetValue ((int) PropertyMarkerBeacon.Frequency); }
			set { SetValue ((int) PropertyMarkerBeacon.Frequency, value); }
		}
		
		public double? AxisBearing
		{
			get { return GetNullableFieldValue <double> ((int) PropertyMarkerBeacon.AxisBearing); }
			set { SetNullableFieldValue <double> ((int) PropertyMarkerBeacon.AxisBearing, value); }
		}
		
		public string AuralMorseCode 
		{
			get { return GetFieldValue <string> ((int) PropertyMarkerBeacon.AuralMorseCode ); }
			set { SetFieldValue <string> ((int) PropertyMarkerBeacon.AuralMorseCode , value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMarkerBeacon
	{
		Class = PropertyNavaidEquipment.NEXT_CLASS,
		Frequency,
		AxisBearing,
		AuralMorseCode ,
		NEXT_CLASS
	}
	
	public static class MetadataMarkerBeacon
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMarkerBeacon ()
		{
			PropInfoList = MetadataNavaidEquipment.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMarkerBeacon.Class, (int) EnumType.CodeMarkerBeaconSignal, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMarkerBeacon.Frequency, (int) DataType.ValFrequency, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMarkerBeacon.AxisBearing, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMarkerBeacon.AuralMorseCode , (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
