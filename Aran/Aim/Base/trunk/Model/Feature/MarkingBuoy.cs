using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class MarkingBuoy : Feature
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.MarkingBuoy; }
		}
		
		public string Designator
		{
			get { return GetFieldValue <string> ((int) PropertyMarkingBuoy.Designator); }
			set { SetFieldValue <string> ((int) PropertyMarkingBuoy.Designator, value); }
		}
		
		public CodeBuoy? Type
		{
			get { return GetNullableFieldValue <CodeBuoy> ((int) PropertyMarkingBuoy.Type); }
			set { SetNullableFieldValue <CodeBuoy> ((int) PropertyMarkingBuoy.Type, value); }
		}
		
		public CodeColour? Colour
		{
			get { return GetNullableFieldValue <CodeColour> ((int) PropertyMarkingBuoy.Colour); }
			set { SetNullableFieldValue <CodeColour> ((int) PropertyMarkingBuoy.Colour, value); }
		}
		
		public FeatureRef MarkedSeaplaneLandingArea
		{
			get { return (FeatureRef ) GetValue ((int) PropertyMarkingBuoy.MarkedSeaplaneLandingArea); }
			set { SetValue ((int) PropertyMarkingBuoy.MarkedSeaplaneLandingArea, value); }
		}
		
		public ElevatedPoint Location
		{
			get { return GetObject <ElevatedPoint> ((int) PropertyMarkingBuoy.Location); }
			set { SetValue ((int) PropertyMarkingBuoy.Location, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyMarkingBuoy
	{
		Designator = PropertyFeature.NEXT_CLASS,
		Type,
		Colour,
		MarkedSeaplaneLandingArea,
		Location,
		NEXT_CLASS
	}
	
	public static class MetadataMarkingBuoy
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataMarkingBuoy ()
		{
			PropInfoList = MetadataFeature.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyMarkingBuoy.Designator, (int) AimFieldType.SysString);
			PropInfoList.Add (PropertyMarkingBuoy.Type, (int) EnumType.CodeBuoy, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMarkingBuoy.Colour, (int) EnumType.CodeColour, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMarkingBuoy.MarkedSeaplaneLandingArea, (int) FeatureType.SeaplaneLandingArea, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyMarkingBuoy.Location, (int) ObjectType.ElevatedPoint, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyDBEntity.Annotation, (int) ObjectType.Note, PropertyTypeCharacter.List | PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
