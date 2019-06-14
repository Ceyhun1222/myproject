using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class GuidanceLineLightSystem : GroundLightSystem
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.GuidanceLineLightSystem; }
		}
		
		public FeatureRef LightedGuidanceLine
		{
			get { return (FeatureRef ) GetValue ((int) PropertyGuidanceLineLightSystem.LightedGuidanceLine); }
			set { SetValue ((int) PropertyGuidanceLineLightSystem.LightedGuidanceLine, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyGuidanceLineLightSystem
	{
		LightedGuidanceLine = PropertyGroundLightSystem.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataGuidanceLineLightSystem
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataGuidanceLineLightSystem ()
		{
			PropInfoList = MetadataGroundLightSystem.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyGuidanceLineLightSystem.LightedGuidanceLine, (int) FeatureType.GuidanceLine, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
