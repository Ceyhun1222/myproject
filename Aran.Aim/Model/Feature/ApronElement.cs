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
	public class ApronElement : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.ApronElement; }
		}
		
		public CodeApronElement? Type
		{
			get { return GetNullableFieldValue <CodeApronElement> ((int) PropertyApronElement.Type); }
			set { SetNullableFieldValue <CodeApronElement> ((int) PropertyApronElement.Type, value); }
		}
		
		public bool? JetwayAvailability
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyApronElement.JetwayAvailability); }
			set { SetNullableFieldValue <bool> ((int) PropertyApronElement.JetwayAvailability, value); }
		}
		
		public bool? TowingAvailability
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyApronElement.TowingAvailability); }
			set { SetNullableFieldValue <bool> ((int) PropertyApronElement.TowingAvailability, value); }
		}
		
		public bool? DockingAvailability
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyApronElement.DockingAvailability); }
			set { SetNullableFieldValue <bool> ((int) PropertyApronElement.DockingAvailability, value); }
		}
		
		public bool? GroundPowerAvailability
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyApronElement.GroundPowerAvailability); }
			set { SetNullableFieldValue <bool> ((int) PropertyApronElement.GroundPowerAvailability, value); }
		}
		
		public ValDistance Length
		{
			get { return (ValDistance ) GetValue ((int) PropertyApronElement.Length); }
			set { SetValue ((int) PropertyApronElement.Length, value); }
		}
		
		public ValDistance Width
		{
			get { return (ValDistance ) GetValue ((int) PropertyApronElement.Width); }
			set { SetValue ((int) PropertyApronElement.Width, value); }
		}
		
		public FeatureRef AssociatedApron
		{
			get { return (FeatureRef ) GetValue ((int) PropertyApronElement.AssociatedApron); }
			set { SetValue ((int) PropertyApronElement.AssociatedApron, value); }
		}
		
		public SurfaceCharacteristics SurfaceProperties
		{
			get { return GetObject <SurfaceCharacteristics> ((int) PropertyApronElement.SurfaceProperties); }
			set { SetValue ((int) PropertyApronElement.SurfaceProperties, value); }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyApronElement.Extent); }
			set { SetValue ((int) PropertyApronElement.Extent, value); }
		}
		
		public List <FeatureRefObject> SupplyService
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyApronElement.SupplyService); }
		}
		
		public List <ApronAreaAvailability> Availability
		{
			get { return GetObjectList <ApronAreaAvailability> ((int) PropertyApronElement.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyApronElement
	{
		Type = PropertyFeature.NEXT_CLASS,
		JetwayAvailability,
		TowingAvailability,
		DockingAvailability,
		GroundPowerAvailability,
		Length,
		Width,
		AssociatedApron,
		SurfaceProperties,
		Extent,
		SupplyService,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataApronElement
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataApronElement ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyApronElement.Type, (int) EnumType.CodeApronElement, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronElement.JetwayAvailability, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronElement.TowingAvailability, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronElement.DockingAvailability, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronElement.GroundPowerAvailability, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronElement.Length, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronElement.Width, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronElement.AssociatedApron, (int) FeatureType.Apron, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronElement.SurfaceProperties, (int) ObjectType.SurfaceCharacteristics, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronElement.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronElement.SupplyService, (int) FeatureType.AirportSuppliesService, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronElement.Availability, (int) ObjectType.ApronAreaAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
