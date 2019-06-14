using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class AirportHeliport : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.AirportHeliport; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyAirportHeliport.Designator); }
			set { SetFieldValue <string> ((int) PropertyAirportHeliport.Designator, value); }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyAirportHeliport.Name); }
			set { SetFieldValue <string> ((int) PropertyAirportHeliport.Name, value); }
		}
		
		public string LocationIndicatorICAO
		{
			get { return GetFieldValue <string> ((int) PropertyAirportHeliport.LocationIndicatorICAO); }
			set { SetFieldValue <string> ((int) PropertyAirportHeliport.LocationIndicatorICAO, value); }
		}
		
		public string DesignatorIATA
		{
			get { return GetFieldValue <string> ((int) PropertyAirportHeliport.DesignatorIATA); }
			set { SetFieldValue <string> ((int) PropertyAirportHeliport.DesignatorIATA, value); }
		}
		
		public CodeAirportHeliport? Type
		{
			get { return GetNullableFieldValue <CodeAirportHeliport> ((int) PropertyAirportHeliport.Type); }
			set { SetNullableFieldValue <CodeAirportHeliport> ((int) PropertyAirportHeliport.Type, value); }
		}
		
		public bool? CertifiedICAO
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAirportHeliport.CertifiedICAO); }
			set { SetNullableFieldValue <bool> ((int) PropertyAirportHeliport.CertifiedICAO, value); }
		}
		
		public bool? PrivateUse
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAirportHeliport.PrivateUse); }
			set { SetNullableFieldValue <bool> ((int) PropertyAirportHeliport.PrivateUse, value); }
		}
		
		public CodeMilitaryOperations? ControlType
		{
			get { return GetNullableFieldValue <CodeMilitaryOperations> ((int) PropertyAirportHeliport.ControlType); }
			set { SetNullableFieldValue <CodeMilitaryOperations> ((int) PropertyAirportHeliport.ControlType, value); }
		}
		
		public ValDistanceVertical FieldElevation
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyAirportHeliport.FieldElevation); }
			set { SetValue ((int) PropertyAirportHeliport.FieldElevation, value); }
		}
		
		public ValDistanceVertical FieldElevationAccuracy
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyAirportHeliport.FieldElevationAccuracy); }
			set { SetValue ((int) PropertyAirportHeliport.FieldElevationAccuracy, value); }
		}
		
		public CodeVerticalDatum? VerticalDatum
		{
			get { return GetNullableFieldValue <CodeVerticalDatum> ((int) PropertyAirportHeliport.VerticalDatum); }
			set { SetNullableFieldValue <CodeVerticalDatum> ((int) PropertyAirportHeliport.VerticalDatum, value); }
		}
		
		public double? MagneticVariation
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAirportHeliport.MagneticVariation); }
			set { SetNullableFieldValue <double> ((int) PropertyAirportHeliport.MagneticVariation, value); }
		}
		
		public double? MagneticVariationAccuracy
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAirportHeliport.MagneticVariationAccuracy); }
			set { SetNullableFieldValue <double> ((int) PropertyAirportHeliport.MagneticVariationAccuracy, value); }
		}
		
		public string DateMagneticVariation
		{
			get { return GetFieldValue <string> ((int) PropertyAirportHeliport.DateMagneticVariation); }
			set { SetFieldValue <string> ((int) PropertyAirportHeliport.DateMagneticVariation, value); }
		}
		
		public double? MagneticVariationChange
		{
			get { return GetNullableFieldValue <double> ((int) PropertyAirportHeliport.MagneticVariationChange); }
			set { SetNullableFieldValue <double> ((int) PropertyAirportHeliport.MagneticVariationChange, value); }
		}
		
		public ValTemperature ReferenceTemperature
		{
			get { return (ValTemperature ) GetValue ((int) PropertyAirportHeliport.ReferenceTemperature); }
			set { SetValue ((int) PropertyAirportHeliport.ReferenceTemperature, value); }
		}
		
		public bool? AltimeterCheckLocation
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAirportHeliport.AltimeterCheckLocation); }
			set { SetNullableFieldValue <bool> ((int) PropertyAirportHeliport.AltimeterCheckLocation, value); }
		}
		
		public bool? SecondaryPowerSupply
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAirportHeliport.SecondaryPowerSupply); }
			set { SetNullableFieldValue <bool> ((int) PropertyAirportHeliport.SecondaryPowerSupply, value); }
		}
		
		public bool? WindDirectionIndicator
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAirportHeliport.WindDirectionIndicator); }
			set { SetNullableFieldValue <bool> ((int) PropertyAirportHeliport.WindDirectionIndicator, value); }
		}
		
		public bool? LandingDirectionIndicator
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAirportHeliport.LandingDirectionIndicator); }
			set { SetNullableFieldValue <bool> ((int) PropertyAirportHeliport.LandingDirectionIndicator, value); }
		}
		
		public ValDistanceVertical TransitionAltitude
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyAirportHeliport.TransitionAltitude); }
			set { SetValue ((int) PropertyAirportHeliport.TransitionAltitude, value); }
		}
		
		public ValFL TransitionLevel
		{
			get { return (ValFL ) GetValue ((int) PropertyAirportHeliport.TransitionLevel); }
			set { SetValue ((int) PropertyAirportHeliport.TransitionLevel, value); }
		}
		
		public ValTemperature LowestTemperature
		{
			get { return (ValTemperature ) GetValue ((int) PropertyAirportHeliport.LowestTemperature); }
			set { SetValue ((int) PropertyAirportHeliport.LowestTemperature, value); }
		}
		
		public bool? Abandoned
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyAirportHeliport.Abandoned); }
			set { SetNullableFieldValue <bool> ((int) PropertyAirportHeliport.Abandoned, value); }
		}
		
		public DateTime? CertificationDate
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertyAirportHeliport.CertificationDate); }
			set { SetNullableFieldValue <DateTime> ((int) PropertyAirportHeliport.CertificationDate, value); }
		}
		
		public DateTime? CertificationExpirationDate
		{
			get { return GetNullableFieldValue <DateTime> ((int) PropertyAirportHeliport.CertificationExpirationDate); }
			set { SetNullableFieldValue <DateTime> ((int) PropertyAirportHeliport.CertificationExpirationDate, value); }
		}
		
		public List <AirportHeliportContamination> Contaminant
		{
			get { return GetObjectList <AirportHeliportContamination> ((int) PropertyAirportHeliport.Contaminant); }
		}
		
		public List <City> ServedCity
		{
			get { return GetObjectList <City> ((int) PropertyAirportHeliport.ServedCity); }
		}
		
		public AirportHeliportResponsibilityOrganisation ResponsibleOrganisation
		{
			get { return GetObject <AirportHeliportResponsibilityOrganisation> ((int) PropertyAirportHeliport.ResponsibleOrganisation); }
			set { SetValue ((int) PropertyAirportHeliport.ResponsibleOrganisation, value); }
		}
		
		public ElevatedPoint ARP
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyAirportHeliport.ARP); }
			set { SetValue ((int) PropertyAirportHeliport.ARP, value); }
		}
		
		public ElevatedSurface AviationBoundary
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyAirportHeliport.AviationBoundary); }
			set { SetValue ((int) PropertyAirportHeliport.AviationBoundary, value); }
		}
		
		public List <FeatureRefObject> AltimeterSource
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyAirportHeliport.AltimeterSource); }
		}
		
		public List <ContactInformation> Contact
		{
			get { return GetObjectList <ContactInformation> ((int) PropertyAirportHeliport.Contact); }
		}
		
		public List <AirportHeliportAvailability> Availability
		{
			get { return GetObjectList <AirportHeliportAvailability> ((int) PropertyAirportHeliport.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyAirportHeliport
	{
		Designator = PropertyFeature.NEXT_CLASS,
		Name,
		LocationIndicatorICAO,
		DesignatorIATA,
		Type,
		CertifiedICAO,
		PrivateUse,
		ControlType,
		FieldElevation,
		FieldElevationAccuracy,
		VerticalDatum,
		MagneticVariation,
		MagneticVariationAccuracy,
		DateMagneticVariation,
		MagneticVariationChange,
		ReferenceTemperature,
		AltimeterCheckLocation,
		SecondaryPowerSupply,
		WindDirectionIndicator,
		LandingDirectionIndicator,
		TransitionAltitude,
		TransitionLevel,
		LowestTemperature,
		Abandoned,
		CertificationDate,
		CertificationExpirationDate,
		Contaminant,
		ServedCity,
		ResponsibleOrganisation,
		ARP,
		AviationBoundary,
		AltimeterSource,
		Contact,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataAirportHeliport
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataAirportHeliport ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyAirportHeliport.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.LocationIndicatorICAO, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.DesignatorIATA, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.Type, (int) EnumType.CodeAirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.CertifiedICAO, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.PrivateUse, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.ControlType, (int) EnumType.CodeMilitaryOperations, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.FieldElevation, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.FieldElevationAccuracy, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.VerticalDatum, (int) EnumType.CodeVerticalDatum, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.MagneticVariation, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.MagneticVariationAccuracy, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.DateMagneticVariation, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.MagneticVariationChange, (int) AimFieldType.SysDouble, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.ReferenceTemperature, (int) DataType.ValTemperature, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.AltimeterCheckLocation, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.SecondaryPowerSupply, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.WindDirectionIndicator, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.LandingDirectionIndicator, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.TransitionAltitude, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.TransitionLevel, (int) DataType.ValFL, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.LowestTemperature, (int) DataType.ValTemperature, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.Abandoned, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.CertificationDate, (int) AimFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.CertificationExpirationDate, (int) AimFieldType.SysDateTime, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.Contaminant, (int) ObjectType.AirportHeliportContamination, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.ServedCity, (int) ObjectType.City, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.ResponsibleOrganisation, (int) ObjectType.AirportHeliportResponsibilityOrganisation, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.ARP, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.AviationBoundary, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.AltimeterSource, (int) FeatureType.AltimeterSource, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.Contact, (int) ObjectType.ContactInformation, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyAirportHeliport.Availability, (int) ObjectType.AirportHeliportAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
