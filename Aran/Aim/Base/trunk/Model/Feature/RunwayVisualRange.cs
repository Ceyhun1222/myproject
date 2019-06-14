using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.Objects;

namespace Aran.Aim.Features
{
	public class RunwayVisualRange : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RunwayVisualRange; }
		}
		
		public CodeRVRReading? ReadingPosition
		{
			get { return GetNullableFieldValue <CodeRVRReading> ((int) PropertyRunwayVisualRange.ReadingPosition); }
			set { SetNullableFieldValue <CodeRVRReading> ((int) PropertyRunwayVisualRange.ReadingPosition, value); }
		}
		
		public List <FeatureRefObject> AssociatedRunwayDirection
		{
			get { return GetObjectList <FeatureRefObject> ((int) PropertyRunwayVisualRange.AssociatedRunwayDirection); }
		}
		
		public ElevatedPoint Location
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyRunwayVisualRange.Location); }
			set { SetValue ((int) PropertyRunwayVisualRange.Location, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunwayVisualRange
	{
		ReadingPosition = PropertyFeature.NEXT_CLASS,
		AssociatedRunwayDirection,
		Location,
		NEXT_CLASS
	}
	
	public static class MetadataRunwayVisualRange
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunwayVisualRange ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunwayVisualRange.ReadingPosition, (int) EnumType.CodeRVRReading, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayVisualRange.AssociatedRunwayDirection, (int) FeatureType.RunwayDirection, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayVisualRange.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
