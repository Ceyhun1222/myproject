using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace Aran.Aim.Features
{
	public abstract class NavaidEquipment : Feature
	{
		public virtual NavaidEquipmentType NavaidEquipmentType 
		{
			get { return (NavaidEquipmentType) FeatureType; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyNavaidEquipment.Designator); }
			set { SetFieldValue <string> ((int) PropertyNavaidEquipment.Designator, value); }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyNavaidEquipment.Name); }
			set { SetFieldValue <string> ((int) PropertyNavaidEquipment.Name, value); }
		}
		
		public CodeRadioEmission? EmissionClass
		{
			get { return GetNullableFieldValue <CodeRadioEmission> ((int) PropertyNavaidEquipment.EmissionClass); }
			set { SetNullableFieldValue <CodeRadioEmission> ((int) PropertyNavaidEquipment.EmissionClass, value); }
		}
		
		public bool? Mobile
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyNavaidEquipment.Mobile); }
			set { SetNullableFieldValue <bool> ((int) PropertyNavaidEquipment.Mobile, value); }
		}
		
		public double? MagneticVariation
		{
			get { return GetNullableFieldValue <double> ((int) PropertyNavaidEquipment.MagneticVariation); }
			set { SetNullableFieldValue <double> ((int) PropertyNavaidEquipment.MagneticVariation, value); }
		}
		
		public double? MagneticVariationAccuracy
		{
			get { return GetNullableFieldValue <double> ((int) PropertyNavaidEquipment.MagneticVariationAccuracy); }
			set { SetNullableFieldValue <double> ((int) PropertyNavaidEquipment.MagneticVariationAccuracy, value); }
		}
		
		public string DateMagneticVariation
		{
			get { return GetFieldValue <string> ((int) PropertyNavaidEquipment.DateMagneticVariation); }
			set { SetFieldValue <string> ((int) PropertyNavaidEquipment.DateMagneticVariation, value); }
		}
		
		public bool? FlightChecked
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyNavaidEquipment.FlightChecked); }
			set { SetNullableFieldValue <bool> ((int) PropertyNavaidEquipment.FlightChecked, value); }
		}
		
		public ElevatedPoint Location
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyNavaidEquipment.Location); }
			set { SetValue ((int) PropertyNavaidEquipment.Location, value); }
		}
		
		public List <AuthorityForNavaidEquipment> Authority
		{
			get { return GetObjectList <AuthorityForNavaidEquipment> ((int) PropertyNavaidEquipment.Authority); }
		}
		
		public List <NavaidEquipmentMonitoring> Monitoring
		{
			get { return GetObjectList <NavaidEquipmentMonitoring> ((int) PropertyNavaidEquipment.Monitoring); }
		}
		
		public List <NavaidOperationalStatus> Availability
		{
			get { return GetObjectList <NavaidOperationalStatus> ((int) PropertyNavaidEquipment.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNavaidEquipment
	{
		Designator = PropertyFeature.NEXT_CLASS,
		Name,
		EmissionClass,
		Mobile,
		MagneticVariation,
		MagneticVariationAccuracy,
		DateMagneticVariation,
		FlightChecked,
		Location,
		Authority,
		Monitoring,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataNavaidEquipment
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNavaidEquipment ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNavaidEquipment.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidEquipment.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidEquipment.EmissionClass, (int) EnumType.CodeRadioEmission, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidEquipment.Mobile, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidEquipment.MagneticVariation, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidEquipment.MagneticVariationAccuracy, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidEquipment.DateMagneticVariation, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidEquipment.FlightChecked, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidEquipment.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidEquipment.Authority, (int) ObjectType.AuthorityForNavaidEquipment, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidEquipment.Monitoring, (int) ObjectType.NavaidEquipmentMonitoring, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavaidEquipment.Availability, (int) ObjectType.NavaidOperationalStatus, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
		}
	}
}
