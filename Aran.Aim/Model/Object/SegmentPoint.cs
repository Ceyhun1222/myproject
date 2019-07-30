using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Objects;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public abstract class SegmentPoint : AObject
	{
		public virtual SegmentPointType SegmentPointType 
		{
			get { return (SegmentPointType) ObjectType; }
		}
		
		public CodeATCReporting? ReportingATC
		{
			get { return GetNullableFieldValue <CodeATCReporting> ((int) PropertySegmentPoint.ReportingATC); }
			set { SetNullableFieldValue <CodeATCReporting> ((int) PropertySegmentPoint.ReportingATC, value); }
		}
		
		public bool? FlyOver
		{
			get { return GetNullableFieldValue <bool> ((int) PropertySegmentPoint.FlyOver); }
			set { SetNullableFieldValue <bool> ((int) PropertySegmentPoint.FlyOver, value); }
		}
		
		public bool? Waypoint
		{
			get { return GetNullableFieldValue <bool> ((int) PropertySegmentPoint.Waypoint); }
			set { SetNullableFieldValue <bool> ((int) PropertySegmentPoint.Waypoint, value); }
		}
		
		public bool? RadarGuidance
		{
			get { return GetNullableFieldValue <bool> ((int) PropertySegmentPoint.RadarGuidance); }
			set { SetNullableFieldValue <bool> ((int) PropertySegmentPoint.RadarGuidance, value); }
		}
		
		public List <PointReference> FacilityMakeup
		{
			get { return GetObjectList <PointReference> ((int) PropertySegmentPoint.FacilityMakeup); }
		}
		
		public SignificantPoint PointChoice
		{
			get { return GetObject <SignificantPoint> ((int) PropertySegmentPoint.PointChoice); }
			set { SetValue ((int) PropertySegmentPoint.PointChoice, value); }
		}
		
		public FeatureRef ExtendedServiceVolume
		{
			get { return (FeatureRef ) GetValue ((int) PropertySegmentPoint.ExtendedServiceVolume); }
			set { SetValue ((int) PropertySegmentPoint.ExtendedServiceVolume, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySegmentPoint
	{
		ReportingATC = PropertyAObject.NEXT_CLASS,
		FlyOver,
		Waypoint,
		RadarGuidance,
		FacilityMakeup,
		PointChoice,
		ExtendedServiceVolume,
		NEXT_CLASS
	}
	
	public static class MetadataSegmentPoint
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSegmentPoint ()
		{
			PropInfoList = MetadataAObject.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySegmentPoint.ReportingATC, (int) EnumType.CodeATCReporting, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentPoint.FlyOver, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentPoint.Waypoint, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentPoint.RadarGuidance, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentPoint.FacilityMakeup, (int) ObjectType.PointReference, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentPoint.PointChoice, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySegmentPoint.ExtendedServiceVolume, (int) FeatureType.RadioFrequencyArea, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
