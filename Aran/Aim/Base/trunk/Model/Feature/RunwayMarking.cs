using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class RunwayMarking : Marking
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RunwayMarking; }
		}
		
		public CodeRunwaySection? MarkingLocation
		{
			get { return GetNullableFieldValue <CodeRunwaySection> ((int) PropertyRunwayMarking.MarkingLocation); }
			set { SetNullableFieldValue <CodeRunwaySection> ((int) PropertyRunwayMarking.MarkingLocation, value); }
		}
		
		public FeatureRef MarkedRunway
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRunwayMarking.MarkedRunway); }
			set { SetValue ((int) PropertyRunwayMarking.MarkedRunway, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunwayMarking
	{
		MarkingLocation = PropertyMarking.NEXT_CLASS,
		MarkedRunway,
		NEXT_CLASS
	}
	
	public static class MetadataRunwayMarking
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunwayMarking ()
		{
			PropInfoList = MetadataMarking.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunwayMarking.MarkingLocation, (int) EnumType.CodeRunwaySection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayMarking.MarkedRunway, (int) FeatureType.Runway, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
