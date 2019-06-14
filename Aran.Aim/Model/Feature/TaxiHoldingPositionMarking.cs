using System;
using System.Collections.Generic;
using Aran.Aim;
using Aran.Aim.PropertyEnum;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;

namespace Aran.Aim.Features
{
	public class TaxiHoldingPositionMarking : Marking
	{
		public override FeatureType FeatureType
		{
			get { return FeatureType.TaxiHoldingPositionMarking; }
		}
		
		public FeatureRef MarkedTaxiHold
		{
			get { return (FeatureRef ) GetValue ((int) PropertyTaxiHoldingPositionMarking.MarkedTaxiHold); }
			set { SetValue ((int) PropertyTaxiHoldingPositionMarking.MarkedTaxiHold, value); }
		}
		
	}
}

namespace Aran.Aim.PropertyEnum
{
	public enum PropertyTaxiHoldingPositionMarking
	{
		MarkedTaxiHold = PropertyMarking.NEXT_CLASS,
		NEXT_CLASS
	}
	
	public static class MetadataTaxiHoldingPositionMarking
	{
		public static AimPropInfoList PropInfoList;
		
		static MetadataTaxiHoldingPositionMarking ()
		{
			PropInfoList = MetadataMarking.PropInfoList.Clone ();
			
			PropInfoList.Add (PropertyTaxiHoldingPositionMarking.MarkedTaxiHold, (int) FeatureType.TaxiHoldingPosition, PropertyTypeCharacter.Nullable);
			PropInfoList.Add (PropertyFeature.Metadata, (int) ObjectType.MdMetadata, PropertyTypeCharacter.Nullable);
		}
	}
}
