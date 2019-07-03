using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class RunwayProtectAreaLightSystem : GroundLightSystem
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RunwayProtectAreaLightSystem; }
		}
		
		public CodeProtectAreaSection? Position
		{
			get { return GetNullableFieldValue <CodeProtectAreaSection> ((int) PropertyRunwayProtectAreaLightSystem.Position); }
			set { SetNullableFieldValue <CodeProtectAreaSection> ((int) PropertyRunwayProtectAreaLightSystem.Position, value); }
		}
		
		public FeatureRef LightedArea
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRunwayProtectAreaLightSystem.LightedArea); }
			set { SetValue ((int) PropertyRunwayProtectAreaLightSystem.LightedArea, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunwayProtectAreaLightSystem
	{
		Position = PropertyGroundLightSystem.NEXT_CLASS,
		LightedArea,
		NEXT_CLASS
	}
	
	public static class MetadataRunwayProtectAreaLightSystem
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunwayProtectAreaLightSystem ()
		{
			PropInfoList = MetadataGroundLightSystem.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunwayProtectAreaLightSystem.Position, (int) EnumType.CodeProtectAreaSection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayProtectAreaLightSystem.LightedArea, (int) FeatureType.RunwayProtectArea, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
