using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;

namespace Aran.Aim.Features
{
	public class CheckpointINS : NavigationSystemCheckpoint
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.CheckpointINS; }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCheckpointINS
	{
        NEXT_CLASS = PropertyNavigationSystemCheckpoint.NEXT_CLASS
	}
	
	public static class MetadataCheckpointINS
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCheckpointINS ()
		{
			PropInfoList = MetadataNavigationSystemCheckpoint.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
