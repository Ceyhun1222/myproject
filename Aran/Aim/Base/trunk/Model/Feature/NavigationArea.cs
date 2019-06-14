using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class NavigationArea : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.NavigationArea; }
		}
		
		public CodeNavigationArea? NavigationAreaType
		{
			get { return GetNullableFieldValue <CodeNavigationArea> ((int) PropertyNavigationArea.NavigationAreaType); }
			set { SetNullableFieldValue <CodeNavigationArea> ((int) PropertyNavigationArea.NavigationAreaType, value); }
		}
		
		public ValDistanceVertical MinimumCeiling
		{
			get { return (ValDistanceVertical ) GetValue ((int) PropertyNavigationArea.MinimumCeiling); }
			set { SetValue ((int) PropertyNavigationArea.MinimumCeiling, value); }
		}
		
		public ValDistance MinimumVisibility
		{
			get { return (ValDistance ) GetValue ((int) PropertyNavigationArea.MinimumVisibility); }
			set { SetValue ((int) PropertyNavigationArea.MinimumVisibility, value); }
		}
		
		public FeatureRef Departure
		{
			get { return (FeatureRef ) GetValue ((int) PropertyNavigationArea.Departure); }
			set { SetValue ((int) PropertyNavigationArea.Departure, value); }
		}
		
		public List <NavigationAreaSector> Sector
		{
			get { return GetObjectList <NavigationAreaSector> ((int) PropertyNavigationArea.Sector); }
		}
		
		public SignificantPoint CentrePoint
		{
			get { return GetObject <SignificantPoint> ((int) PropertyNavigationArea.CentrePoint); }
			set { SetValue ((int) PropertyNavigationArea.CentrePoint, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyNavigationArea
	{
		NavigationAreaType = PropertyFeature.NEXT_CLASS,
		MinimumCeiling,
		MinimumVisibility,
		Departure,
		Sector,
		CentrePoint,
		NEXT_CLASS
	}
	
	public static class MetadataNavigationArea
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataNavigationArea ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyNavigationArea.NavigationAreaType, (int) EnumType.CodeNavigationArea, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationArea.MinimumCeiling, (int) DataType.ValDistanceVertical, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationArea.MinimumVisibility, (int) DataType.ValDistance, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationArea.Departure, (int) FeatureType.StandardInstrumentDeparture, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationArea.Sector, (int) ObjectType.NavigationAreaSector, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyNavigationArea.CentrePoint, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
