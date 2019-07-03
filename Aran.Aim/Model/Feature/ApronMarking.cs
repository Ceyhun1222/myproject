using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class ApronMarking : Marking
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.ApronMarking; }
		}
		
		public CodeApronSection? MarkingLocation
		{
			get { return GetNullableFieldValue <CodeApronSection> ((int) PropertyApronMarking.MarkingLocation); }
			set { SetNullableFieldValue <CodeApronSection> ((int) PropertyApronMarking.MarkingLocation, value); }
		}
		
		public FeatureRef MarkedApron
		{
			get { return (FeatureRef ) GetValue ((int) PropertyApronMarking.MarkedApron); }
			set { SetValue ((int) PropertyApronMarking.MarkedApron, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyApronMarking
	{
		MarkingLocation = PropertyMarking.NEXT_CLASS,
		MarkedApron,
		NEXT_CLASS
	}
	
	public static class MetadataApronMarking
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataApronMarking ()
		{
			PropInfoList = MetadataMarking.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyApronMarking.MarkingLocation, (int) EnumType.CodeApronSection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronMarking.MarkedApron, (int) FeatureType.Apron, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
