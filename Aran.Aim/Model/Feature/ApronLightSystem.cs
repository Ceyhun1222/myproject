using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class ApronLightSystem : GroundLightSystem
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.ApronLightSystem; }
		}
		
		public CodeApronSection? Position
		{
			get { return GetNullableFieldValue <CodeApronSection> ((int) PropertyApronLightSystem.Position); }
			set { SetNullableFieldValue <CodeApronSection> ((int) PropertyApronLightSystem.Position, value); }
		}
		
		public FeatureRef LightedApron
		{
			get { return (FeatureRef ) GetValue ((int) PropertyApronLightSystem.LightedApron); }
			set { SetValue ((int) PropertyApronLightSystem.LightedApron, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyApronLightSystem
	{
		Position = PropertyGroundLightSystem.NEXT_CLASS,
		LightedApron,
		NEXT_CLASS
	}
	
	public static class MetadataApronLightSystem
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataApronLightSystem ()
		{
			PropInfoList = MetadataGroundLightSystem.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyApronLightSystem.Position, (int) EnumType.CodeApronSection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyApronLightSystem.LightedApron, (int) FeatureType.Apron, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
