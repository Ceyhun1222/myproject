using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class TouchDownLiftOffLightSystem : GroundLightSystem
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.TouchDownLiftOffLightSystem; }
		}
		
		public CodeTLOFSection? Position
		{
			get { return GetNullableFieldValue <CodeTLOFSection> ((int) PropertyTouchDownLiftOffLightSystem.Position); }
			set { SetNullableFieldValue <CodeTLOFSection> ((int) PropertyTouchDownLiftOffLightSystem.Position, value); }
		}
		
		public FeatureRef LightedTouchDownLiftOff
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTouchDownLiftOffLightSystem.LightedTouchDownLiftOff); }
			set { SetValue ((int) PropertyTouchDownLiftOffLightSystem.LightedTouchDownLiftOff, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTouchDownLiftOffLightSystem
	{
		Position = PropertyGroundLightSystem.NEXT_CLASS,
		LightedTouchDownLiftOff,
		NEXT_CLASS
	}
	
	public static class MetadataTouchDownLiftOffLightSystem
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTouchDownLiftOffLightSystem ()
		{
			PropInfoList = MetadataGroundLightSystem.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTouchDownLiftOffLightSystem.Position, (int) EnumType.CodeTLOFSection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTouchDownLiftOffLightSystem.LightedTouchDownLiftOff, (int) FeatureType.TouchDownLiftOff, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
