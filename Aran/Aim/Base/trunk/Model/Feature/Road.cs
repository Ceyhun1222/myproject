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
	public class Road : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.Road; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyRoad.Designator); }
			set { SetFieldValue <string> ((int) PropertyRoad.Designator, value); }
		}
		
		public CodeStatusOperations? Status
		{
			get { return GetNullableFieldValue <CodeStatusOperations> ((int) PropertyRoad.Status); }
			set { SetNullableFieldValue <CodeStatusOperations> ((int) PropertyRoad.Status, value); }
		}
		
		public CodeRoad? Type
		{
			get { return GetNullableFieldValue <CodeRoad> ((int) PropertyRoad.Type); }
			set { SetNullableFieldValue <CodeRoad> ((int) PropertyRoad.Type, value); }
		}
		
		public bool? Abandoned
		{
			get { return GetNullableFieldValue <bool> ((int) PropertyRoad.Abandoned); }
			set { SetNullableFieldValue <bool> ((int) PropertyRoad.Abandoned, value); }
		}
		
		public FeatureRef AssociatedAirport
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRoad.AssociatedAirport); }
			set { SetValue ((int) PropertyRoad.AssociatedAirport, value); }
		}
		
		public SurfaceCharacteristics SurfaceProperties
		{
			get { return GetObject <SurfaceCharacteristics> ((int) PropertyRoad.SurfaceProperties); }
			set { SetValue ((int) PropertyRoad.SurfaceProperties, value); }
		}
		
		public List <FeatureRefObject> AccessibleStand
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyRoad.AccessibleStand); }
		}
		
		public ElevatedSurface SurfaceExtent
		{
			get { return GetObject <ElevatedSurface> ((int) PropertyRoad.SurfaceExtent); }
			set { SetValue ((int) PropertyRoad.SurfaceExtent, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRoad
	{
		Designator = PropertyFeature.NEXT_CLASS,
		Status,
		Type,
		Abandoned,
		AssociatedAirport,
		SurfaceProperties,
		AccessibleStand,
		SurfaceExtent,
		NEXT_CLASS
	}
	
	public static class MetadataRoad
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRoad ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRoad.Designator, (int) AimFieldType.SysString, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoad.Status, (int) EnumType.CodeStatusOperations, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoad.Type, (int) EnumType.CodeRoad, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoad.Abandoned, (int) AimFieldType.SysBool, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoad.AssociatedAirport, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoad.SurfaceProperties, (int) ObjectType.SurfaceCharacteristics, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoad.AccessibleStand, (int) FeatureType.AircraftStand, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRoad.SurfaceExtent, (int) ObjectType.ElevatedSurface, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
