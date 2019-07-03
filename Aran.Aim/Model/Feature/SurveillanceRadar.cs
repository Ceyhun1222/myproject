using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public abstract class SurveillanceRadar : RadarEquipment
	{
		public virtual SurveillanceRadarType SurveillanceRadarType 
		{
			get { return (SurveillanceRadarType) FeatureType; }
		}
		
		public ValDistanceVertical VerticalCoverageAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertySurveillanceRadar.VerticalCoverageAltitude); }
			set { SetValue ((int) PropertySurveillanceRadar.VerticalCoverageAltitude, value); }
		}
		
		public ValDistance VerticalCoverageDistance
		{
			get { return (ValDistance ) GetValue ((int) PropertySurveillanceRadar.VerticalCoverageDistance); }
			set { SetValue ((int) PropertySurveillanceRadar.VerticalCoverageDistance, value); }
		}
		
		public double? VerticalCoverageAzimuth
		{
			get { return GetNullableFieldValue <double> ((int) PropertySurveillanceRadar.VerticalCoverageAzimuth); }
			set { SetNullableFieldValue <double> ((int) PropertySurveillanceRadar.VerticalCoverageAzimuth, value); }
		}
		
		public bool? AntennaTiltFixed
		{
			get { return GetNullableFieldValue <bool> ((int) PropertySurveillanceRadar.AntennaTiltFixed); }
			set { SetNullableFieldValue <bool> ((int) PropertySurveillanceRadar.AntennaTiltFixed, value); }
		}
		
		public double? TiltAngle
		{
			get { return GetNullableFieldValue <double> ((int) PropertySurveillanceRadar.TiltAngle); }
			set { SetNullableFieldValue <double> ((int) PropertySurveillanceRadar.TiltAngle, value); }
		}
		
		public string AutomatedRadarTerminalSystem
		{
			get { return GetFieldValue <string> ((int) PropertySurveillanceRadar.AutomatedRadarTerminalSystem); }
			set { SetFieldValue <string> ((int) PropertySurveillanceRadar.AutomatedRadarTerminalSystem, value); }
		}
		
		public List <SurveillanceGroundStation> GroundStation
		{
			get { return GetObjectList <SurveillanceGroundStation> ((int) PropertySurveillanceRadar.GroundStation); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySurveillanceRadar
	{
		VerticalCoverageAltitude = PropertyRadarEquipment.NEXT_CLASS,
		VerticalCoverageDistance,
		VerticalCoverageAzimuth,
		AntennaTiltFixed,
		TiltAngle,
		AutomatedRadarTerminalSystem,
		GroundStation,
		NEXT_CLASS
	}
	
	public static class MetadataSurveillanceRadar
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSurveillanceRadar ()
		{
			PropInfoList = MetadataRadarEquipment.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySurveillanceRadar.VerticalCoverageAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurveillanceRadar.VerticalCoverageDistance, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurveillanceRadar.VerticalCoverageAzimuth, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurveillanceRadar.AntennaTiltFixed, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurveillanceRadar.TiltAngle, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurveillanceRadar.AutomatedRadarTerminalSystem, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySurveillanceRadar.GroundStation, (int) ObjectType.SurveillanceGroundStation, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
