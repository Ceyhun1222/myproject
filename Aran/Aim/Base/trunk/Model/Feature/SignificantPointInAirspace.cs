using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class SignificantPointInAirspace : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.SignificantPointInAirspace; }
		}
		
		public CodeAirspacePointRole? Type
		{
			get { return GetNullableFieldValue <CodeAirspacePointRole> ((int) PropertySignificantPointInAirspace.Type); }
			set { SetNullableFieldValue <CodeAirspacePointRole> ((int) PropertySignificantPointInAirspace.Type, value); }
		}
		
		public CodeAirspacePointPosition? RelativeLocation
		{
			get { return GetNullableFieldValue <CodeAirspacePointPosition> ((int) PropertySignificantPointInAirspace.RelativeLocation); }
			set { SetNullableFieldValue <CodeAirspacePointPosition> ((int) PropertySignificantPointInAirspace.RelativeLocation, value); }
		}
		
		public FeatureRef ContainingAirspace
		{
			get { return (FeatureRef ) GetValue ((int) PropertySignificantPointInAirspace.ContainingAirspace); }
			set { SetValue ((int) PropertySignificantPointInAirspace.ContainingAirspace, value); }
		}
		
		public SignificantPoint Location
		{
			get { return GetObject <SignificantPoint> ((int) PropertySignificantPointInAirspace.Location); }
			set { SetValue ((int) PropertySignificantPointInAirspace.Location, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertySignificantPointInAirspace
	{
		Type = PropertyFeature.NEXT_CLASS,
		RelativeLocation,
		ContainingAirspace,
		Location,
		NEXT_CLASS
	}
	
	public static class MetadataSignificantPointInAirspace
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataSignificantPointInAirspace ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertySignificantPointInAirspace.Type, (int) EnumType.CodeAirspacePointRole, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySignificantPointInAirspace.RelativeLocation, (int) EnumType.CodeAirspacePointPosition, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySignificantPointInAirspace.ContainingAirspace, (int) FeatureType.Airspace, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertySignificantPointInAirspace.Location, (int) ObjectType.SignificantPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
