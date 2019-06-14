using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class TaxiwayLightSystem : GroundLightSystem
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.TaxiwayLightSystem; }
		}
		
		public CodeTaxiwaySection? Position
		{
			get { return GetNullableFieldValue <CodeTaxiwaySection> ((int) PropertyTaxiwayLightSystem.Position); }
			set { SetNullableFieldValue <CodeTaxiwaySection> ((int) PropertyTaxiwayLightSystem.Position, value); }
		}
		
		public FeatureRef LightedTaxiway
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTaxiwayLightSystem.LightedTaxiway); }
			set { SetValue ((int) PropertyTaxiwayLightSystem.LightedTaxiway, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTaxiwayLightSystem
	{
		Position = PropertyGroundLightSystem.NEXT_CLASS,
		LightedTaxiway,
		NEXT_CLASS
	}
	
	public static class MetadataTaxiwayLightSystem
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTaxiwayLightSystem ()
		{
			PropInfoList = MetadataGroundLightSystem.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTaxiwayLightSystem.Position, (int) EnumType.CodeTaxiwaySection, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiwayLightSystem.LightedTaxiway, (int) FeatureType.Taxiway, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
