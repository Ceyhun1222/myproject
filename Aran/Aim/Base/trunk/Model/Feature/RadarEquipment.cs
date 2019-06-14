using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public abstract class RadarEquipment : Feature
	{
		public virtual RadarEquipmentType RadarEquipmentType 
		{
			get { return (RadarEquipmentType) FeatureType; }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyRadarEquipment.Name); }
			set { SetFieldValue <string> ((int) PropertyRadarEquipment.Name, value); }
		}
		
		public string SerialNumber
		{
			get { return GetFieldValue <string> ((int) PropertyRadarEquipment.SerialNumber); }
			set { SetFieldValue <string> ((int) PropertyRadarEquipment.SerialNumber, value); }
		}
		
		public ValDistance Range
		{
			get { return (ValDistance ) GetValue ((int) PropertyRadarEquipment.Range); }
			set { SetValue ((int) PropertyRadarEquipment.Range, value); }
		}
		
		public ValDistance RangeAccuracy
		{
			get { return (ValDistance ) GetValue ((int) PropertyRadarEquipment.RangeAccuracy); }
			set { SetValue ((int) PropertyRadarEquipment.RangeAccuracy, value); }
		}
		
		public bool? DualChannel
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRadarEquipment.DualChannel); }
			set { SetNullableFieldValue <bool> ((int) PropertyRadarEquipment.DualChannel, value); }
		}
		
		public bool? MovingTargetIndicator
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRadarEquipment.MovingTargetIndicator); }
			set { SetNullableFieldValue <bool> ((int) PropertyRadarEquipment.MovingTargetIndicator, value); }
		}
		
		public CodeStandbyPower? StandbyPower
		{
			get { return GetNullableFieldValue <CodeStandbyPower> ((int) PropertyRadarEquipment.StandbyPower); }
			set { SetNullableFieldValue <CodeStandbyPower> ((int) PropertyRadarEquipment.StandbyPower, value); }
		}
		
		public bool? Digital
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRadarEquipment.Digital); }
			set { SetNullableFieldValue <bool> ((int) PropertyRadarEquipment.Digital, value); }
		}
		
		public bool? MilitaryUseOnly
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRadarEquipment.MilitaryUseOnly); }
			set { SetNullableFieldValue <bool> ((int) PropertyRadarEquipment.MilitaryUseOnly, value); }
		}
		
		public bool? SpecialUseOnly
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRadarEquipment.SpecialUseOnly); }
			set { SetNullableFieldValue <bool> ((int) PropertyRadarEquipment.SpecialUseOnly, value); }
		}
		
		public bool? SpecialAircraftOnly
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRadarEquipment.SpecialAircraftOnly); }
			set { SetNullableFieldValue <bool> ((int) PropertyRadarEquipment.SpecialAircraftOnly, value); }
		}
		
		public double? MagneticVariation
		{
			get { return GetNullableFieldValue <double> ((int) PropertyRadarEquipment.MagneticVariation); }
			set { SetNullableFieldValue <double> ((int) PropertyRadarEquipment.MagneticVariation, value); }
		}
		
		public double? MagneticVariationAccuracy
		{
			get { return GetNullableFieldValue <double> ((int) PropertyRadarEquipment.MagneticVariationAccuracy); }
			set { SetNullableFieldValue <double> ((int) PropertyRadarEquipment.MagneticVariationAccuracy, value); }
		}
		
		public string DateMagneticVariation
		{
			get { return GetFieldValue <string> ((int) PropertyRadarEquipment.DateMagneticVariation); }
			set { SetFieldValue <string> ((int) PropertyRadarEquipment.DateMagneticVariation, value); }
		}
		
		public List <ContactInformation> Contact
		{
			get { return GetObjectList <ContactInformation> ((int) PropertyRadarEquipment.Contact); }
		}
		
		public ElevatedPoint Location
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyRadarEquipment.Location); }
			set { SetValue ((int) PropertyRadarEquipment.Location, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRadarEquipment
	{
		Name = PropertyFeature.NEXT_CLASS,
		SerialNumber,
		Range,
		RangeAccuracy,
		DualChannel,
		MovingTargetIndicator,
		StandbyPower,
		Digital,
		MilitaryUseOnly,
		SpecialUseOnly,
		SpecialAircraftOnly,
		MagneticVariation,
		MagneticVariationAccuracy,
		DateMagneticVariation,
		Contact,
		Location,
		NEXT_CLASS
	}
	
	public static class MetadataRadarEquipment
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRadarEquipment ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRadarEquipment.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.SerialNumber, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.Range, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.RangeAccuracy, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.DualChannel, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.MovingTargetIndicator, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.StandbyPower, (int) EnumType.CodeStandbyPower, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.Digital, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.MilitaryUseOnly, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.SpecialUseOnly, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.SpecialAircraftOnly, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.MagneticVariation, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.MagneticVariationAccuracy, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.DateMagneticVariation, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.Contact, (int) ObjectType.ContactInformation, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRadarEquipment.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
