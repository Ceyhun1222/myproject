using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class DeicingArea : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.DeicingArea; }
		}
		
		public FeatureRef AssociatedApron
		{
			get { return (FeatureRef ) GetValue ((int) PropertyDeicingArea.AssociatedApron); }
			set { SetValue ((int) PropertyDeicingArea.AssociatedApron, value); }
		}
		
		public FeatureRef TaxiwayLocation
		{
			get { return (FeatureRef ) GetValue ((int) PropertyDeicingArea.TaxiwayLocation); }
			set { SetValue ((int) PropertyDeicingArea.TaxiwayLocation, value); }
		}
		
		public FeatureRef StandLocation
		{
			get { return (FeatureRef ) GetValue ((int) PropertyDeicingArea.StandLocation); }
			set { SetValue ((int) PropertyDeicingArea.StandLocation, value); }
		}
		
		public SurfaceCharacteristics SurfaceProperties
		{
			get { return GetObject <SurfaceCharacteristics> ((int) PropertyDeicingArea.SurfaceProperties); }
			set { SetValue ((int) PropertyDeicingArea.SurfaceProperties, value); }
		}
		
		public ElevatedSurface Extent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyDeicingArea.Extent); }
			set { SetValue ((int) PropertyDeicingArea.Extent, value); }
		}
		
		public List <ApronAreaAvailability> Availability
		{
			get { return GetObjectList <ApronAreaAvailability> ((int) PropertyDeicingArea.Availability); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDeicingArea
	{
		AssociatedApron = PropertyFeature.NEXT_CLASS,
		TaxiwayLocation,
		StandLocation,
		SurfaceProperties,
		Extent,
		Availability,
		NEXT_CLASS
	}
	
	public static class MetadataDeicingArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDeicingArea ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDeicingArea.AssociatedApron, (int) FeatureType.Apron, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDeicingArea.TaxiwayLocation, (int) FeatureType.Taxiway, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDeicingArea.StandLocation, (int) FeatureType.AircraftStand, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDeicingArea.SurfaceProperties, (int) ObjectType.SurfaceCharacteristics, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDeicingArea.Extent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDeicingArea.Availability, (int) ObjectType.ApronAreaAvailability, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
