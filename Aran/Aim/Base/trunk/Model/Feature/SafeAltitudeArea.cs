using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class SafeAltitudeArea : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.SafeAltitudeArea; }
		}
		
		public CodeSafeAltitude? SafeAreaType
		{
			get { return GetNullableFieldValue <CodeSafeAltitude> ((int) PropertySafeAltitudeArea.SafeAreaType); }
			set { SetNullableFieldValue <CodeSafeAltitude> ((int) PropertySafeAltitudeArea.SafeAreaType, value); }
		}
		
		public SignificantPoint CentrePoint
		{
			get { return GetObject <SignificantPoint> ((int) PropertySafeAltitudeArea.CentrePoint); }
			set { SetValue ((int) PropertySafeAltitudeArea.CentrePoint, value); }
		}
		
		public List <SafeAltitudeAreaSector> Sector
		{
			get { return GetObjectList <SafeAltitudeAreaSector> ((int) PropertySafeAltitudeArea.Sector); }
		}
		
		public List <FeatureRefObject> Location
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertySafeAltitudeArea.Location); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySafeAltitudeArea
	{
		SafeAreaType = PropertyFeature.NEXT_CLASS,
		CentrePoint,
		Sector,
		Location,
		NEXT_CLASS
	}
	
	public static class MetadataSafeAltitudeArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSafeAltitudeArea ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySafeAltitudeArea.SafeAreaType, (int) EnumType.CodeSafeAltitude, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySafeAltitudeArea.CentrePoint, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySafeAltitudeArea.Sector, (int) ObjectType.SafeAltitudeAreaSector, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySafeAltitudeArea.Location, (int) FeatureType.AirportHeliport, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
