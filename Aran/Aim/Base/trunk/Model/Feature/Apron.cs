using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class Apron : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.Apron; }
		}
		
		public string Name
		{
			get { return GetFieldValue <string> ((int) PropertyApron.Name); }
			set { SetFieldValue <string> ((int) PropertyApron.Name, value); }
		}
		
		public bool? Abandoned
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyApron.Abandoned); }
			set { SetNullableFieldValue <bool> ((int) PropertyApron.Abandoned, value); }
		}
		
		public SurfaceCharacteristics SurfaceProperties
		{
			get { return GetObject <SurfaceCharacteristics> ((int) PropertyApron.SurfaceProperties); }
			set { SetValue ((int) PropertyApron.SurfaceProperties, value); }
		}
		
		public FeatureRef AssociatedAirportHeliport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyApron.AssociatedAirportHeliport); }
			set { SetValue ((int) PropertyApron.AssociatedAirportHeliport, value); }
		}
		
		public List <ApronContamination> Contaminant
		{
			get { return GetObjectList <ApronContamination> ((int) PropertyApron.Contaminant); }
		}
		
		public List <ApronAreaAvailability> Availability
		{
			get { return GetObjectList <ApronAreaAvailability> ((int) PropertyApron.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyApron
	{
		Name = PropertyFeature.NEXT_CLASS,
		Abandoned,
		SurfaceProperties,
		AssociatedAirportHeliport,
		Contaminant,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataApron
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataApron ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyApron.Name, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApron.Abandoned, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApron.SurfaceProperties, (int) ObjectType.SurfaceCharacteristics, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApron.AssociatedAirportHeliport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApron.Contaminant, (int) ObjectType.ApronContamination, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApron.Availability, (int) ObjectType.ApronAreaAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
