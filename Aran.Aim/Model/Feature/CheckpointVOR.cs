using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class CheckpointVOR : NavigationSystemCheckpoint
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.CheckpointVOR; }
		}
		
		public FeatureRef CheckPointFacility
		{
			get { return (FeatureRef ) GetValue ((int) PropertyCheckpointVOR.CheckPointFacility); }
			set { SetValue ((int) PropertyCheckpointVOR.CheckPointFacility, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyCheckpointVOR
	{
		CheckPointFacility = PropertyNavigationSystemCheckpoint.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataCheckpointVOR
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataCheckpointVOR ()
		{
			PropInfoList = MetadataNavigationSystemCheckpoint.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyCheckpointVOR.CheckPointFacility, (int) FeatureType.VOR, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
