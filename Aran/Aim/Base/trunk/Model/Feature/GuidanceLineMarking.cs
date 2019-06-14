using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class GuidanceLineMarking : Marking
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.GuidanceLineMarking; }
		}
		
		public FeatureRef MarkedGuidanceLine
		{
			get { return (FeatureRef ) GetValue ((int) PropertyGuidanceLineMarking.MarkedGuidanceLine); }
			set { SetValue ((int) PropertyGuidanceLineMarking.MarkedGuidanceLine, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyGuidanceLineMarking
	{
		MarkedGuidanceLine = PropertyMarking.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataGuidanceLineMarking
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataGuidanceLineMarking ()
		{
			PropInfoList = MetadataMarking.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyGuidanceLineMarking.MarkedGuidanceLine, (int) FeatureType.GuidanceLine, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
