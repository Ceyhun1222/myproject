using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public class TouchDownLiftOff : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.TouchDownLiftOff; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyTouchDownLiftOff.Designator); }
			set { SetFieldValue <string> ((int) PropertyTouchDownLiftOff.Designator, value); }
		}
		
		public ValDistance Length
		{
			get { return (ValDistance ) GetValue ((int) PropertyTouchDownLiftOff.Length); }
			set { SetValue ((int) PropertyTouchDownLiftOff.Length, value); }
		}
		
		public ValDistance Width
		{
			get { return (ValDistance ) GetValue ((int) PropertyTouchDownLiftOff.Width); }
			set { SetValue ((int) PropertyTouchDownLiftOff.Width, value); }
		}
		
		public double? Slope
		{
			get { return GetNullableFieldValue <double> ((int) PropertyTouchDownLiftOff.Slope); }
			set { SetNullableFieldValue <double> ((int) PropertyTouchDownLiftOff.Slope, value); }
		}
		
		public CodeHelicopterPerformance? HelicopterClass
		{
			get { return GetNullableFieldValue <CodeHelicopterPerformance> ((int) PropertyTouchDownLiftOff.HelicopterClass); }
			set { SetNullableFieldValue <CodeHelicopterPerformance> ((int) PropertyTouchDownLiftOff.HelicopterClass, value); }
		}
		
		public bool? Abandoned
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyTouchDownLiftOff.Abandoned); }
			set { SetNullableFieldValue <bool> ((int) PropertyTouchDownLiftOff.Abandoned, value); }
		}
		
		public ElevatedPoint AimingPoint
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyTouchDownLiftOff.AimingPoint); }
			set { SetValue ((int) PropertyTouchDownLiftOff.AimingPoint, value); }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyTouchDownLiftOff.Extent); }
			set { SetValue ((int) PropertyTouchDownLiftOff.Extent, value); }
		}
		
		public SurfaceCharacteristics SurfaceProperties
		{
			get { return GetObject <SurfaceCharacteristics> ((int) PropertyTouchDownLiftOff.SurfaceProperties); }
			set { SetValue ((int) PropertyTouchDownLiftOff.SurfaceProperties, value); }
		}
		
		public FeatureRef AssociatedAirportHeliport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTouchDownLiftOff.AssociatedAirportHeliport); }
			set { SetValue ((int) PropertyTouchDownLiftOff.AssociatedAirportHeliport, value); }
		}
		
		public FeatureRef ApproachTakeOffArea
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTouchDownLiftOff.ApproachTakeOffArea); }
			set { SetValue ((int) PropertyTouchDownLiftOff.ApproachTakeOffArea, value); }
		}
		
		public List <TouchDownLiftOffContamination> Contaminant
		{
			get { return GetObjectList <TouchDownLiftOffContamination> ((int) PropertyTouchDownLiftOff.Contaminant); }
		}
		
		public List <ManoeuvringAreaAvailability> Availability
		{
			get { return GetObjectList <ManoeuvringAreaAvailability> ((int) PropertyTouchDownLiftOff.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTouchDownLiftOff
	{
		Designator = PropertyFeature.NEXT_CLASS,
		Length,
		Width,
		Slope,
		HelicopterClass,
		Abandoned,
		AimingPoint,
		Extent,
		SurfaceProperties,
		AssociatedAirportHeliport,
		ApproachTakeOffArea,
		Contaminant,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataTouchDownLiftOff
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTouchDownLiftOff ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTouchDownLiftOff.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOff.Length, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOff.Width, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOff.Slope, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOff.HelicopterClass, (int) EnumType.CodeHelicopterPerformance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOff.Abandoned, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOff.AimingPoint, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOff.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOff.SurfaceProperties, (int) ObjectType.SurfaceCharacteristics, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOff.AssociatedAirportHeliport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOff.ApproachTakeOffArea, (int) FeatureType.Runway, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOff.Contaminant, (int) ObjectType.TouchDownLiftOffContamination, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOff.Availability, (int) ObjectType.ManoeuvringAreaAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
