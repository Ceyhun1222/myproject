using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class TaxiHoldingPositionLightSystem : GroundLightSystem
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.TaxiHoldingPositionLightSystem; }
		}
		
		public CodeLightHoldingPosition? Type
		{
			get { return GetNullableFieldValue <CodeLightHoldingPosition> ((int) PropertyTaxiHoldingPositionLightSystem.Type); }
			set { SetNullableFieldValue <CodeLightHoldingPosition> ((int) PropertyTaxiHoldingPositionLightSystem.Type, value); }
		}
		
		public FeatureRef TaxiHolding
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTaxiHoldingPositionLightSystem.TaxiHolding); }
			set { SetValue ((int) PropertyTaxiHoldingPositionLightSystem.TaxiHolding, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTaxiHoldingPositionLightSystem
	{
		Type = PropertyGroundLightSystem.NEXT_CLASS,
		TaxiHolding,
		NEXT_CLASS
	}
	
	public static class MetadataTaxiHoldingPositionLightSystem
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTaxiHoldingPositionLightSystem ()
		{
			PropInfoList = MetadataGroundLightSystem.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTaxiHoldingPositionLightSystem.Type, (int) EnumType.CodeLightHoldingPosition, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyTaxiHoldingPositionLightSystem.TaxiHolding, (int) FeatureType.TaxiHoldingPosition, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
