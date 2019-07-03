using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class RunwayDirectionLightSystem : GroundLightSystem
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.RunwayDirectionLightSystem; }
		}
		
		public CodeRunwaySection? Position
		{
			get { return GetNullableFieldValue <CodeRunwaySection> ((int) PropertyRunwayDirectionLightSystem.Position); }
			set { SetNullableFieldValue <CodeRunwaySection> ((int) PropertyRunwayDirectionLightSystem.Position, value); }
		}
		
		public FeatureRef AssociatedRunwayDirection
		{
			get { return (FeatureRef ) GetValue ((int) PropertyRunwayDirectionLightSystem.AssociatedRunwayDirection); }
			set { SetValue ((int) PropertyRunwayDirectionLightSystem.AssociatedRunwayDirection, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyRunwayDirectionLightSystem
	{
		Position = PropertyGroundLightSystem.NEXT_CLASS,
		AssociatedRunwayDirection,
		NEXT_CLASS
	}
	
	public static class MetadataRunwayDirectionLightSystem
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataRunwayDirectionLightSystem ()
		{
			PropInfoList = MetadataGroundLightSystem.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyRunwayDirectionLightSystem.Position, (int) EnumType.CodeRunwaySection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyRunwayDirectionLightSystem.AssociatedRunwayDirection, (int) FeatureType.RunwayDirection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
