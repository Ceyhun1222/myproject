using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class DeicingAreaMarking : Marking
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.DeicingAreaMarking; }
		}
		
		public FeatureRef MarkedDeicingArea
		{
			get { return (FeatureRef ) GetValue ((int) PropertyDeicingAreaMarking.MarkedDeicingArea); }
			set { SetValue ((int) PropertyDeicingAreaMarking.MarkedDeicingArea, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyDeicingAreaMarking
	{
		MarkedDeicingArea = PropertyMarking.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataDeicingAreaMarking
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataDeicingAreaMarking ()
		{
			PropInfoList = MetadataMarking.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyDeicingAreaMarking.MarkedDeicingArea, (int) FeatureType.DeicingArea, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
